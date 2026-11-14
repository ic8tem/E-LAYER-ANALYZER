// E-LAYER ANALYZER - campo geomagnetico IGRF-14 (IAGA, doi:10.5281/zenodo.14012302), sviluppo in armoniche sferiche
// fino al grado 13 con le funzioni di Legendre semi-normalizzate di Schmidt (coordinate geocentriche).
// Coefficienti 2025.0 e previsione 2030.0 in data/igrf14.csv, interpolati linearmente alla data di calcolo.
// Epoche storiche 1900-2030 in data/igrf14_storico.csv per i banchi di prova (campo dell'anno del QSO, Igrf.At).
// Aggiornamento automatico e silenzioso (richiesta dell'autore, 14/11/2026): all'avvio e poi ogni 30 giorni il programma
// cerca sul sito NOAA NCEI la generazione successiva (igrf15coeffs.txt, ...); se c'e' la converte in data/igrfNN.csv e la usa.
// Verificato contro ppigrf (K. Laundal) a 40,5N 14,2E / 50N 5W / 60N 20E: scarti < 1 nT.
// Se il file manca si usa il dipolo centrato inclinato (Geo.FieldDir).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ELA
{
    public static class Igrf
    {
        const int N = 13;
        const double RE = 6371.2;   // raggio di riferimento del modello (km)
        static double[,] g = new double[N + 1, N + 1], h = new double[N + 1, N + 1];
        public static bool Loaded;
        // epoca del campo in uso: quella storica impostata con At() nel thread chiamante, altrimenti la data del calcolo
        public static double Epoch { get { return tg != null ? tEpoch : epochNow; } }
        static double epochNow;
        static string info = "";
        static double epochYear;
        // descrizione per la barra di stato e le mappe, nella lingua scelta
        public static int Generation = 14;
        static double epoch0 = 2025.0, epoch1 = 2030.0;
        public static string Info { get { return Loaded ? "IGRF-" + Generation.ToString(CultureInfo.InvariantCulture) + ", " + YAMS.Lang.F("epoca {0}", epochYear.ToString("F2", CultureInfo.InvariantCulture)) : YAMS.Lang.T(info); } }

        // file dei coefficienti della generazione piu' recente presente in data (igrf14.csv, igrf15.csv, ...)
        static string Newest(string dataDir, out int gen)
        {
            gen = 0; string best = null;
            try
            {
                foreach (string f in Directory.GetFiles(dataDir, "igrf*.csv"))
                {
                    Match m = Regex.Match(Path.GetFileName(f), @"^igrf(\d+)\.csv$", RegexOptions.IgnoreCase);
                    int g0;
                    if (m.Success && int.TryParse(m.Groups[1].Value, out g0) && g0 > gen) { gen = g0; best = f; }
                }
            }
            catch { }
            return best;
        }

        public static void Load(string dataDir, DateTime date)
        {
            Loaded = false;
            int gen;
            string f = Newest(dataDir, out gen);
            if (f == null) { info = "igrf14.csv mancante: uso il dipolo"; return; }
            // epoche dall'intestazione (n;m;coeff_2025;coeff_2030)
            double e0 = 2025.0, e1 = 2030.0;
            try
            {
                Match em = Regex.Match(File.ReadLines(f).FirstOrDefaultLine(), @"coeff_(\d{4}(?:\.\d+)?);coeff_(\d{4}(?:\.\d+)?)");
                if (em.Success) { e0 = double.Parse(em.Groups[1].Value, CultureInfo.InvariantCulture); e1 = double.Parse(em.Groups[2].Value, CultureInfo.InvariantCulture); }
            }
            catch { }
            double year = date.Year + (date.DayOfYear - 0.5) / (DateTime.IsLeapYear(date.Year) ? 366.0 : 365.0);
            double t = Math.Max(0, Math.Min(1, (year - e0) / Math.Max(1, e1 - e0)));
            double[,] gg = new double[N + 1, N + 1], hh = new double[N + 1, N + 1];
            int cnt = 0;
            foreach (string line in File.ReadAllLines(f))
            {
                if (line.StartsWith("#")) continue;
                string[] p = line.Split(';');
                if (p.Length < 4) continue;
                int n = int.Parse(p[0], CultureInfo.InvariantCulture), m = int.Parse(p[1], CultureInfo.InvariantCulture);
                double a = double.Parse(p[2], CultureInfo.InvariantCulture), b = double.Parse(p[3], CultureInfo.InvariantCulture);
                double v = a + (b - a) * t;
                if (n < 1 || n > N || Math.Abs(m) > n) continue;
                if (m >= 0) gg[n, m] = v; else hh[n, -m] = v;
                cnt++;
            }
            if (cnt < 195) { info = "igrf14.csv incompleto: uso il dipolo"; return; }
            g = gg; h = hh; epochNow = year; Loaded = true;
            epochYear = year; Generation = gen; epoch0 = e0; epoch1 = e1;
            LoadHistory(dataDir);
        }

        // ---------- campo dell'anno del QSO (richiesta dell'autore, 14/11/2026): i banchi di prova usano le epoche storiche
        // IGRF-14 1900-2030 (data/igrf14_storico.csv, DGRF fino al 2020), interpolate linearmente all'anno del collegamento.
        // L'epoca vale solo nel thread che chiama At() e fino al Dispose: il resto del programma continua con il campo di oggi.
        [ThreadStatic] static double[,] tg, th;
        [ThreadStatic] static double tEpoch;
        static double[] hEp; static int[] hN, hM; static double[][] hV;
        static readonly Dictionary<long, double[][,]> hCache = new Dictionary<long, double[][,]>();
        public static bool HasHistory { get { return hEp != null; } }

        static void LoadHistory(string dataDir)
        {
            try
            {
                string f = Path.Combine(dataDir, "igrf14_storico.csv");
                if (!File.Exists(f) || hEp != null) return;
                double[] ep = null; List<int> ns = new List<int>(), ms = new List<int>(); List<double[]> vs = new List<double[]>();
                foreach (string line in File.ReadAllLines(f))
                {
                    if (line.StartsWith("#") || line.Trim() == "") continue;
                    string[] p = line.Split(';');
                    if (p[0] == "n")
                    {
                        ep = new double[p.Length - 2];
                        for (int i = 2; i < p.Length; i++) ep[i - 2] = double.Parse(p[i], CultureInfo.InvariantCulture);
                        continue;
                    }
                    if (ep == null || p.Length != ep.Length + 2) return;
                    int n = int.Parse(p[0], CultureInfo.InvariantCulture), m = int.Parse(p[1], CultureInfo.InvariantCulture);
                    if (n < 1 || n > N || Math.Abs(m) > n) continue;
                    double[] v = new double[ep.Length];
                    for (int i = 0; i < ep.Length; i++) v[i] = double.Parse(p[i + 2], NumberStyles.Float, CultureInfo.InvariantCulture);
                    ns.Add(n); ms.Add(m); vs.Add(v);
                }
                if (ep == null || ep.Length < 2 || vs.Count < 195) return;
                hN = ns.ToArray(); hM = ms.ToArray(); hV = vs.ToArray(); hEp = ep;
            }
            catch { }
        }

        // anno decimale di una data (meta' del giorno)
        public static double YearOf(DateTime d) { return d.Year + (d.DayOfYear - 0.5) / (DateTime.IsLeapYear(d.Year) ? 366.0 : 365.0); }

        // anno decimale da "aaaa-mm-gg" (o "aaaa"); NaN se la data non e' nota
        public static double YearOf(string date)
        {
            DateTime d;
            if (string.IsNullOrEmpty(date)) return double.NaN;
            if (DateTime.TryParseExact(date.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d)) return YearOf(d);
            int y;
            if (int.TryParse(date.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out y) && y >= 1900 && y <= 2100) return y + 0.5;
            return double.NaN;
        }

        // Campo dell'anno indicato nel thread chiamante, fino al Dispose. Anno NaN, fuori 1900-2030 o senza il file storico:
        // resta il campo attuale. Uso: using (Igrf.At(anno)) { ... }
        public static IDisposable At(double year)
        {
            Scope sc = new Scope { G = tg, H = th, E = tEpoch };
            double[,] gg, hh;
            if (Loaded && HistoryAt(year, out gg, out hh)) { tg = gg; th = hh; tEpoch = year; }
            return sc;
        }

        sealed class Scope : IDisposable
        {
            public double[,] G, H; public double E;
            public void Dispose() { tg = G; th = H; tEpoch = E; }
        }

        static bool HistoryAt(double year, out double[,] gg, out double[,] hh)
        {
            gg = null; hh = null;
            double[] ep = hEp;
            if (ep == null || double.IsNaN(year) || year < ep[0] || year > ep[ep.Length - 1]) return false;
            long key = (long)Math.Round(year * 100);
            lock (hCache)
            {
                double[][,] c;
                if (hCache.TryGetValue(key, out c)) { gg = c[0]; hh = c[1]; return true; }
                int i = 0;
                while (i < ep.Length - 2 && year > ep[i + 1]) i++;
                double t = (year - ep[i]) / (ep[i + 1] - ep[i]);
                gg = new double[N + 1, N + 1]; hh = new double[N + 1, N + 1];
                for (int k = 0; k < hN.Length; k++)
                {
                    double v = hV[k][i] + (hV[k][i + 1] - hV[k][i]) * t;
                    if (hM[k] >= 0) gg[hN[k], hM[k]] = v; else hh[hN[k], -hM[k]] = v;
                }
                if (hCache.Count > 200) hCache.Clear();
                hCache[key] = new[] { gg, hh };
                return true;
            }
        }

        static string FirstOrDefaultLine(this IEnumerable<string> l) { foreach (string x in l) return x; return ""; }

        // ---------- aggiornamento automatico e silenzioso del modello
        public const string NcieUrl = "https://www.ngdc.noaa.gov/IAGA/vmod/coeffs/igrf{0}coeffs.txt";
        public static event Action Updated;

        // all'avvio e ogni 30 giorni: cerca la generazione successiva; nessuna finestra, nessuna domanda
        public static void CheckUpdateSilent(string dataDir, YAMS.Config cfg)
        {
            try
            {
                string last = cfg != null ? cfg.Get("igrf_controllo", "") : "";
                DateTime d;
                if (DateTime.TryParse(last, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d) && (DateTime.UtcNow - d).TotalDays < 30) return;
            }
            catch { }
            Task.Run(async () =>
            {
                try
                {
                    int gen; Newest(dataDir, out gen); if (gen < 14) gen = 14;
                    using (HttpClient c = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                    {
                        c.DefaultRequestHeaders.UserAgent.ParseAdd("E-LAYER-ANALYZER");
                        for (int next = gen + 1; next <= gen + 2; next++)
                        {
                            HttpResponseMessage r = await c.GetAsync(string.Format(CultureInfo.InvariantCulture, NcieUrl, next));
                            if (!r.IsSuccessStatusCode) break;
                            string txt = await r.Content.ReadAsStringAsync();
                            string csv = ToCsv(txt, next);
                            if (csv == null) break;
                            File.WriteAllText(Path.Combine(dataDir, "igrf" + next.ToString(CultureInfo.InvariantCulture) + ".csv"), csv);
                            Avalonia.Threading.Dispatcher.UIThread.Post(() => { Load(dataDir, DateTime.UtcNow); Action a = Updated; if (a != null) a(); });
                        }
                    }
                    if (cfg != null) { cfg.Set("igrf_controllo", DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)); try { cfg.Save(); } catch { } }
                }
                catch { }
            });
        }

        // Converte il file dei coefficienti IGRF di NOAA NCEI (righe "g/h n m valori... SV") nel formato del programma:
        // coefficienti dell'ultima epoca e previsione a 5 anni con la variazione secolare. null se il file non torna.
        public static string ToCsv(string txt, int gen)
        {
            string[] lines = txt.Replace("\r", "").Split('\n');
            double ep = double.NaN; int cols = 0;
            StringBuilder sb = new StringBuilder();
            int cnt = 0;
            foreach (string raw in lines)
            {
                string[] p = raw.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length < 5) continue;
                if (p[0] == "g/h")
                {
                    // ... 2020.0 2025.0 2025-30: l'ultima epoca e' la penultima colonna
                    cols = p.Length;
                    double.TryParse(p[p.Length - 2], NumberStyles.Float, CultureInfo.InvariantCulture, out ep);
                    sb.Append("# IGRF-").Append(gen.ToString(CultureInfo.InvariantCulture)).Append(" (IAGA, NOAA NCEI, aggiornamento automatico): n;m;coeff_")
                      .Append(ep.ToString("F1", CultureInfo.InvariantCulture)).Append(";coeff_").Append((ep + 5).ToString("F1", CultureInfo.InvariantCulture)).Append(" (m<0 = h, nT)\n");
                    continue;
                }
                if ((p[0] != "g" && p[0] != "h") || cols == 0 || double.IsNaN(ep) || p.Length != cols) continue;
                int n, m; double v, sv;
                if (!int.TryParse(p[1], out n) || !int.TryParse(p[2], out m)) return null;
                if (!double.TryParse(p[p.Length - 2], NumberStyles.Float, CultureInfo.InvariantCulture, out v)) return null;
                if (!double.TryParse(p[p.Length - 1], NumberStyles.Float, CultureInfo.InvariantCulture, out sv)) sv = 0;
                int mm = p[0] == "h" ? -m : m;
                sb.Append(n.ToString(CultureInfo.InvariantCulture)).Append(';').Append(mm.ToString(CultureInfo.InvariantCulture)).Append(';')
                  .Append(v.ToString("0.0##", CultureInfo.InvariantCulture)).Append(';').Append((v + 5 * sv).ToString("0.0##", CultureInfo.InvariantCulture)).Append('\n');
                cnt++;
            }
            return cnt >= 195 && !double.IsNaN(ep) ? sb.ToString() : null;
        }

        // Campo in coordinate geocentriche (lat, lon in gradi; r in km): componenti Nord, Est, Giu' (nT)
        public static void Field(double lat, double lon, double r, out double bn, out double be, out double bd)
        {
            double th = (90 - lat) * Geo.D2R, ph = lon * Geo.D2R;
            double ct = Math.Cos(th), st = Math.Sin(th);
            double[,] P = new double[N + 1, N + 1], dP = new double[N + 1, N + 1];
            P[0, 0] = 1; dP[0, 0] = 0;
            for (int n = 1; n <= N; n++)
                for (int m = 0; m <= n; m++)
                {
                    if (n == m)
                    {
                        if (n == 1) { P[1, 1] = st; dP[1, 1] = ct; }
                        else
                        {
                            double k = Math.Sqrt(1 - 1.0 / (2 * n));
                            P[n, n] = k * st * P[n - 1, n - 1];
                            dP[n, n] = k * (st * dP[n - 1, n - 1] + ct * P[n - 1, n - 1]);
                        }
                    }
                    else
                    {
                        if (n == 1) { P[1, 0] = ct; dP[1, 0] = -st; }
                        else
                        {
                            double a = Math.Sqrt(n * n - m * m);
                            double k1 = (2 * n - 1) / a;
                            double k2 = (n - 1) >= m ? Math.Sqrt((n - 1) * (n - 1) - m * m) / a : 0;
                            double p2 = n - 2 >= m ? P[n - 2, m] : 0, d2 = n - 2 >= m ? dP[n - 2, m] : 0;
                            P[n, m] = k1 * ct * P[n - 1, m] - k2 * p2;
                            dP[n, m] = k1 * (ct * dP[n - 1, m] - st * P[n - 1, m]) - k2 * d2;
                        }
                    }
                }
            double[,] G = tg ?? g, H = th ?? h;   // epoca storica del thread (At) o campo attuale
            double br = 0, bt = 0, bp = 0;
            double ratio = RE / r, ar = ratio * ratio;
            for (int n = 1; n <= N; n++)
            {
                ar *= ratio;   // (RE/r)^(n+2)
                for (int m = 0; m <= n; m++)
                {
                    double cm = Math.Cos(m * ph), sm = Math.Sin(m * ph);
                    double gh = G[n, m] * cm + H[n, m] * sm;
                    br += (n + 1) * ar * gh * P[n, m];
                    bt += -ar * gh * dP[n, m];
                    if (st > 1e-9) bp += ar * m * (G[n, m] * sm - H[n, m] * cm) * P[n, m] / st;
                }
            }
            bn = -bt; be = bp; bd = -br;
        }

        // Versore del campo in un punto (coordinate terrestri, km)
        public static Geo.V3 Dir(Geo.V3 p)
        {
            double r = p.Len;
            double lat = Math.Asin(p.Z / r) * Geo.R2D, lon = Math.Atan2(p.Y, p.X) * Geo.R2D;
            double bn, be, bd;
            Field(lat, lon, r, out bn, out be, out bd);
            double f = lat * Geo.D2R, l = lon * Geo.D2R;
            Geo.V3 north = new Geo.V3(-Math.Sin(f) * Math.Cos(l), -Math.Sin(f) * Math.Sin(l), Math.Cos(f));
            Geo.V3 east = new Geo.V3(-Math.Sin(l), Math.Cos(l), 0);
            Geo.V3 up = new Geo.V3(Math.Cos(f) * Math.Cos(l), Math.Cos(f) * Math.Sin(l), Math.Sin(f));
            return (bn * north + be * east + (-bd) * up).Unit;
        }

        // Inclinazione e declinazione (gradi) al suolo
        public static void IncDec(double lat, double lon, out double inc, out double dec)
        {
            double bn, be, bd;
            Field(lat, lon, Geo.R, out bn, out be, out bd);
            inc = Math.Atan2(bd, Math.Sqrt(bn * bn + be * be)) * Geo.R2D;
            dec = Math.Atan2(be, bn) * Geo.R2D;
        }
    }
}
