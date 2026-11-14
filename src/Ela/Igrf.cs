// E-LAYER ANALYZER - campo geomagnetico IGRF-14 (IAGA, doi:10.5281/zenodo.14012302), sviluppo in armoniche sferiche
// fino al grado 13 con le funzioni di Legendre semi-normalizzate di Schmidt (coordinate geocentriche).
// Coefficienti 2025.0 e previsione 2030.0 in data/igrf14.csv, interpolati linearmente alla data di calcolo.
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
        public static double Epoch;
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
            g = gg; h = hh; Epoch = year; Loaded = true;
            epochYear = year; Generation = gen; epoch0 = e0; epoch1 = e1;
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
            double br = 0, bt = 0, bp = 0;
            double ratio = RE / r, ar = ratio * ratio;
            for (int n = 1; n <= N; n++)
            {
                ar *= ratio;   // (RE/r)^(n+2)
                for (int m = 0; m <= n; m++)
                {
                    double cm = Math.Cos(m * ph), sm = Math.Sin(m * ph);
                    double gh = g[n, m] * cm + h[n, m] * sm;
                    br += (n + 1) * ar * gh * P[n, m];
                    bt += -ar * gh * dP[n, m];
                    if (st > 1e-9) bp += ar * m * (g[n, m] * sm - h[n, m] * cm) * P[n, m] / st;
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
