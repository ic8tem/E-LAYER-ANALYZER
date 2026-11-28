// E-LAYER ANALYZER - modulo tropo comune (dalla 0.7.0, richiesta dell'autore del 28/11/2026): condotti troposferici per
// il modello misto E-sporadico + tropo e per la linguetta PREVISIONE.
// Il calcolo dei condotti e' quello di LIVETROPOVHF (stesso autore, Forecast/OwnForecast.cs, Refractivity.Analyze), ridotto
// a quello che serve qui: profilo verticale dal suolo a 500 hPa (Open-Meteo, modello GFS, seconda fonte di LIVETROPOVHF),
// rifrattivita' N di ITU-R P.453, rifrattivita' modificata M = N + 157 h(km), strato in cui M cala con la quota (condotto
// risolto) o salto netto nascosto dal passo dei livelli (condotto potenziale da inversione, come in LIVETROPOVHF),
// lunghezza d'onda massima intrappolata di Kerr (1951, eq. 17).
// Angolo critico di intrappolamento (ITU-R P.834 §7.1 [86]): alfa = sqrt(2e-6 * deficit di M) radianti: un raggio che
// arriva sul condotto con un'elevazione piu' alta non resta intrappolato; i modi intrappolati stanno sotto 1 grado ([87]).
// Indizio osservato: gli spot in 144 MHz dichiarati tropo (<TR>) non sono E-sporadico (restano fuori dall'Es puro) ma
// confermano un condotto lungo la loro tratta.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YAMS;

namespace ELA
{
    public class DuctInfo
    {
        public double Lat, Lon;
        public DateTime Utc;
        public bool Ok;              // profilo letto
        public bool HasDuct;
        public double Base, Top;     // m s.l.m.
        public double Deficit;       // unita' M
        public double LambdaMax;     // m (Kerr)
        public bool Surface, Resolved;
        public double DN1km;         // variazione di N nel primo km (N/km)
        public string Source = "";   // "Open-Meteo GFS" oppure "spot <TR>"
        public double Depth { get { return Top - Base; } }
        // angolo critico di intrappolamento (gradi), ITU-R P.834 eq. 27
        public double CriticalDeg { get { return Deficit > 0 ? Math.Sqrt(2e-6 * Deficit) * 180 / Math.PI : 0; } }
        public bool Traps(double fMHz) { return HasDuct && LambdaMax >= 299.792458 / fMHz; }
        // condotto "morbido" di LIVETROPOVHF (Kerr; Hepburn): fino a lambda circa 3 volte lambda max l'onda sente ancora il condotto
        public double Soft(double fMHz)
        {
            if (!HasDuct || LambdaMax <= 0) return 0;
            double lam = 299.792458 / fMHz;
            return Math.Max(0, Math.Min(1, (LambdaMax / lam - 1.0 / 3) / (2.0 / 3)));
        }
    }

    public static class TropoDuct
    {
        static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(25) };
        static readonly int[] PLevels = { 1000, 975, 950, 925, 900, 850, 800, 700, 600, 500 };
        static readonly Dictionary<string, DuctInfo> cache = new Dictionary<string, DuctInfo>();
        static readonly HashSet<string> pending = new HashSet<string>();
        static readonly object lk = new object();
        public static bool Enabled = true;      // IMPOSTAZIONI: si puo' spegnere (niente richieste a Open-Meteo)
        public static bool Busy;
        public static string Status = "";
        public static DateTime LimitUntil = DateTime.MinValue;   // dopo un rifiuto per limite (HTTP 429) si aspetta
        public static event Action Updated;
        public const double GridDeg = 0.5;      // un profilo ogni mezzo grado (come la griglia di LIVETROPOVHF)
        public const int MaxPerBatch = 40;

        static TropoDuct()
        {
            try { http.DefaultRequestHeaders.UserAgent.ParseAdd("E-LAYER-ANALYZER (programma gratuito per radioamatori, github.com/ic8tem/E-LAYER-ANALYZER)"); } catch { }
        }

        static double Snap(double v) { return Math.Round(v / GridDeg) * GridDeg; }
        static string Key(double lat, double lon, DateTime hour)
        {
            return Snap(lat).ToString("0.0", CultureInfo.InvariantCulture) + "_" + Snap(Geo.Norm180(lon)).ToString("0.0", CultureInfo.InvariantCulture) + "_" + hour.ToString("yyyyMMddHH", CultureInfo.InvariantCulture);
        }
        static DateTime Hour(DateTime t) { return new DateTime(t.Year, t.Month, t.Day, t.Hour, 0, 0, DateTimeKind.Utc); }

        // condotto in un punto all'ora attuale (null se non ancora scaricato; in quel caso il punto va in coda)
        public static DuctInfo At(double lat, double lon)
        {
            DateTime h = Hour(DateTime.UtcNow);
            string k = Key(lat, lon, h);
            lock (lk)
            {
                DuctInfo d;
                if (cache.TryGetValue(k, out d)) return d;
                if (Enabled) pending.Add(k);
            }
            return null;
        }

        public static int Pending { get { lock (lk) return pending.Count; } }

        // scarica i punti in coda (al massimo MaxPerBatch per volta, una richiesta sola a Open-Meteo)
        public static async Task FetchPendingAsync()
        {
            if (!Enabled || Busy || DateTime.UtcNow < LimitUntil) return;
            List<string> keys;
            lock (lk) { keys = pending.Take(MaxPerBatch).ToList(); }
            if (keys.Count == 0) return;
            Busy = true;
            try
            {
                List<double[]> pts = new List<double[]>();
                foreach (string k in keys)
                {
                    string[] p = k.Split('_');
                    pts.Add(new[] { double.Parse(p[0], CultureInfo.InvariantCulture), double.Parse(p[1], CultureInfo.InvariantCulture) });
                }
                List<string> vars = new List<string> { "surface_pressure", "temperature_2m", "relative_humidity_2m" };
                foreach (int pl in PLevels) { vars.Add("temperature_" + pl + "hPa"); vars.Add("relative_humidity_" + pl + "hPa"); }
                string url = "https://api.open-meteo.com/v1/forecast?latitude=" + string.Join(",", pts.Select(x => x[0].ToString("0.0", CultureInfo.InvariantCulture)))
                    + "&longitude=" + string.Join(",", pts.Select(x => x[1].ToString("0.0", CultureInfo.InvariantCulture)))
                    + "&hourly=" + string.Join(",", vars) + "&models=gfs_seamless&past_hours=1&forecast_hours=6&timezone=GMT";
                Status = Lang.F("Scarico {0} profili verticali da Open-Meteo (GFS)...", pts.Count);
                using (HttpResponseMessage resp = await http.GetAsync(url))
                {
                    if ((int)resp.StatusCode == 429)
                    {
                        LimitUntil = DateTime.UtcNow.AddMinutes(10);
                        Status = Lang.T("Open-Meteo ha rifiutato per il limite di richieste: si riprova fra 10 minuti.");
                        return;
                    }
                    resp.EnsureSuccessStatusCode();
                    string json = await resp.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        List<JsonElement> items = doc.RootElement.ValueKind == JsonValueKind.Array ? doc.RootElement.EnumerateArray().ToList() : new List<JsonElement> { doc.RootElement };
                        for (int i = 0; i < items.Count && i < keys.Count; i++)
                        {
                            List<DuctInfo> hours = ParsePoint(items[i], pts[i][0], pts[i][1]);
                            lock (lk)
                            {
                                foreach (DuctInfo d in hours) cache[Key(d.Lat, d.Lon, d.Utc)] = d;
                                if (!cache.ContainsKey(keys[i])) cache[keys[i]] = new DuctInfo { Lat = pts[i][0], Lon = pts[i][1], Utc = Hour(DateTime.UtcNow), Ok = false, Source = "Open-Meteo GFS" };
                                pending.Remove(keys[i]);
                            }
                        }
                    }
                }
                Status = Lang.F("Profili verticali in memoria: {0} (Open-Meteo, modello GFS; ultimo aggiornamento {1:HH:mm} UTC).", Count, DateTime.UtcNow);
                Clean();
            }
            catch (Exception ex)
            {
                Status = Lang.F("Open-Meteo non risponde: {0}", ex.Message);
                LimitUntil = DateTime.UtcNow.AddMinutes(3);
            }
            finally { Busy = false; Updated?.Invoke(); }
        }

        public static int Count { get { lock (lk) return cache.Count; } }

        // tiene solo le ultime 12 ore
        static void Clean()
        {
            DateTime lim = DateTime.UtcNow.AddHours(-12);
            lock (lk)
            {
                List<string> old = cache.Where(kv => kv.Value.Utc < lim).Select(kv => kv.Key).ToList();
                foreach (string k in old) cache.Remove(k);
            }
        }

        static List<DuctInfo> ParsePoint(JsonElement it, double lat, double lon)
        {
            List<DuctInfo> res = new List<DuctInfo>();
            JsonElement h;
            if (!it.TryGetProperty("hourly", out h)) return res;
            DateTime[] times = h.GetProperty("time").EnumerateArray().Select(t => DateTime.SpecifyKind(DateTime.Parse(t.GetString(), CultureInfo.InvariantCulture), DateTimeKind.Utc)).ToArray();
            double elev = 0; JsonElement ev;
            if (it.TryGetProperty("elevation", out ev) && ev.ValueKind == JsonValueKind.Number) elev = ev.GetDouble();
            Func<string, double[]> A = name =>
            {
                JsonElement a;
                if (h.TryGetProperty(name, out a) && a.ValueKind == JsonValueKind.Array)
                    return a.EnumerateArray().Select(x => x.ValueKind == JsonValueKind.Number ? x.GetDouble() : double.NaN).ToArray();
                return Enumerable.Repeat(double.NaN, times.Length).ToArray();
            };
            double[] ps = A("surface_pressure"), t2 = A("temperature_2m"), rh2 = A("relative_humidity_2m");
            double[][] tl = PLevels.Select(pl => A("temperature_" + pl + "hPa")).ToArray();
            double[][] rl = PLevels.Select(pl => A("relative_humidity_" + pl + "hPa")).ToArray();
            for (int k = 0; k < times.Length; k++)
            {
                double[] tLev = new double[PLevels.Length], rLev = new double[PLevels.Length];
                for (int i = 0; i < PLevels.Length; i++) { tLev[i] = k < tl[i].Length ? tl[i][k] : double.NaN; rLev[i] = k < rl[i].Length ? rl[i][k] : double.NaN; }
                DuctInfo d = Analyze(elev, Get(ps, k), Get(t2, k), Get(rh2, k), tLev, rLev);
                d.Lat = lat; d.Lon = lon; d.Utc = times[k]; d.Source = "Open-Meteo GFS";
                res.Add(d);
            }
            return res;
        }
        static double Get(double[] a, int k) { return k < a.Length ? a[k] : double.NaN; }

        // ---------------------------------------------------------------- rifrattivita' (LIVETROPOVHF, ITU-R P.453)

        // pressione di saturazione del vapore (hPa), ITU-R P.453-14 eq. 9
        public static double Es(double tC, double p = 1013.25)
        {
            double ef = 1 + 1e-4 * (7.2 + p * (0.0320 + 5.9e-6 * tC * tC));
            return ef * 6.1121 * Math.Exp((18.678 - tC / 234.5) * tC / (tC + 257.14));
        }
        // N = 77,6/T (P + 4810 e/T)
        public static double N(double pHpa, double tC, double rh)
        {
            double tk = tC + 273.15;
            double e = Math.Max(0, Math.Min(100, rh)) / 100.0 * Es(tC, pHpa);
            return 77.6 / tk * (pHpa + 4810.0 * e / tk);
        }
        static double Tv(double tC, double rh, double p)
        {
            double e = Math.Max(0, Math.Min(100, rh)) / 100.0 * Es(tC);
            double q = 0.622 * e / Math.Max(1, p - 0.378 * e);
            return (tC + 273.15) * (1 + 0.61 * q);
        }
        static double Q(double p, double tC, double rh)
        {
            double e = Math.Max(0, Math.Min(100, rh)) / 100.0 * Es(tC);
            return 0.622 * e / Math.Max(1, p - 0.378 * e);
        }
        // lunghezza d'onda massima intrappolata (m), Kerr 1951 eq. 17 con calo lineare di M nel condotto (come LIVETROPOVHF)
        public static double Kerr(double deficitM, double depthM) { return deficitM > 0 && depthM > 0 ? 2 * Math.Sqrt(2) * (2.0 / 3.0) * Math.Sqrt(deficitM * 1e-6) * depthM : 0; }
        // spessore minimo del condotto per una frequenza (Kerr 1951, eq. 18: lambda[cm] = 0,014 d[ft]^1,5)
        public static double MinDuctM(double fMHz) { return 0.3048 * Math.Pow(29979.2458 / fMHz / 0.014, 2.0 / 3.0); }

        struct Lv { public double Z, P, T, RH, N, M; }

        // profilo dal suolo a 500 hPa e condotto con il deficit di M maggiore (risolto o potenziale)
        public static DuctInfo Analyze(double elev, double ps, double t2, double rh2, double[] tLev, double[] rhLev)
        {
            DuctInfo r = new DuctInfo();
            if (double.IsNaN(ps) || double.IsNaN(t2) || double.IsNaN(rh2)) return r;
            List<Lv> lev = new List<Lv>();
            Lv s = new Lv { Z = elev + 2, P = ps, T = t2, RH = rh2 };
            s.N = N(s.P, s.T, s.RH); s.M = s.N + 157 * s.Z / 1000.0;
            lev.Add(s);
            for (int i = 0; i < PLevels.Length; i++)
            {
                double p = PLevels[i];
                if (p >= ps - 2 || double.IsNaN(tLev[i]) || double.IsNaN(rhLev[i])) continue;
                Lv prev = lev[lev.Count - 1];
                double z = prev.Z + 287.05 * (Tv(prev.T, prev.RH, prev.P) + Tv(tLev[i], rhLev[i], p)) / 2 / 9.80665 * Math.Log(prev.P / p);
                Lv l = new Lv { Z = z, P = p, T = tLev[i], RH = rhLev[i] };
                l.N = N(p, l.T, l.RH); l.M = l.N + 157 * l.Z / 1000.0;
                lev.Add(l);
            }
            if (lev.Count < 3) return r;
            r.Ok = true;
            for (int i = 1; i < lev.Count; i++)
                if (lev[i].Z >= lev[0].Z + 1000)
                {
                    double f = (lev[0].Z + 1000 - lev[i - 1].Z) / Math.Max(1e-6, lev[i].Z - lev[i - 1].Z);
                    r.DN1km = lev[i - 1].N + f * (lev[i].N - lev[i - 1].N) - lev[0].N;
                    break;
                }
            // strati fra livelli consecutivi fino a 3000 m sopra il suolo
            for (int i = 1; i < lev.Count; i++)
            {
                Lv a = lev[i - 1], b = lev[i];
                double dz = b.Z - a.Z;
                if (dz < 10) continue;
                if (a.Z > lev[0].Z + 3000) break;
                double excess = -(b.N - a.N) - 40 * dz / 1000.0;   // calo di N oltre il normale (40 N/km)
                if (excess <= 0) continue;
                double qa = Q(a.P, a.T, a.RH), qb = Q(b.P, b.T, b.RH);
                double lapse = (a.T - b.T) / (dz / 1000.0);
                bool inversion = lapse < 3.0 && qa > 0 && qb / qa < 0.8;   // aria stabile e piu' secca sopra: salto netto
                bool resolved = b.M < a.M;
                double deficit, top;
                if (resolved) { deficit = a.M - b.M; top = b.Z; }
                else if (inversion) { deficit = excess - 157 * 0.05; top = (a.Z + b.Z) / 2; }   // salto in circa 50 m a meta' strato
                else continue;
                if (deficit <= 0) continue;
                double depth = deficit / 0.118;                 // sotto il salto M cresce di circa 118 unita'/km
                if (resolved) depth = Math.Max(depth, dz);
                double bas = top - depth; bool surf = false;
                if (bas <= lev[0].Z + 10) { bas = lev[0].Z; surf = true; }
                if (!r.HasDuct || deficit > r.Deficit)
                {
                    r.HasDuct = true; r.Deficit = deficit; r.Top = top; r.Base = bas; r.Surface = surf; r.Resolved = resolved;
                    r.LambdaMax = Kerr(deficit, top - bas);
                }
            }
            return r;
        }

        // ---------------------------------------------------------------- indizio osservato: spot tropo in 144 MHz

        // spot 2 m dichiarati tropo (<TR>) degli ultimi 'minutes' minuti, oltre 300 km: un condotto c'e' lungo la loro tratta
        public static List<Spot> TropoSpots(double minutes)
        {
            return SpotStore.Recent(minutes, x => x.HasBoth && x.Prop == "TR" && (x.Band == "2m" || x.Band == "70cm") && x.DistanceKm >= 300);
        }

        // il tratto A-B e' coperto da uno spot tropo (punti a meno di km dalla sua tratta, entrambe le estremita')?
        public static Spot ObservedAlong(List<Spot> tr, double la1, double lo1, double la2, double lo2, double km)
        {
            foreach (Spot s in tr)
            {
                if (NearPath(s, la1, lo1, km) && NearPath(s, la2, lo2, km)) return s;
            }
            return null;
        }
        static bool NearPath(Spot s, double la, double lo, double km)
        {
            for (int k = 0; k <= 10; k++)
            {
                double a, b; Geo.IntermediateFast(s.LatS, s.LonS, s.LatD, s.LonD, k / 10.0, out a, out b);
                if (Geo.DistanceKmFast(a, b, la, lo) <= km) return true;
            }
            return false;
        }

        // condotto lungo un tratto: il piu' debole dei profili a inizio, meta' e fine (null finche' mancano i dati)
        public static DuctInfo Along(double la1, double lo1, double la2, double lo2, out bool waiting)
        {
            waiting = false;
            DuctInfo worst = null;
            for (int k = 0; k <= 2; k++)
            {
                double a, b; Geo.IntermediateFast(la1, lo1, la2, lo2, k / 2.0, out a, out b);
                DuctInfo d = At(a, b);
                if (d == null) { waiting = true; continue; }
                if (!d.Ok) continue;
                if (worst == null || !d.HasDuct || (worst.HasDuct && d.Deficit < worst.Deficit)) worst = d;
            }
            return waiting ? null : worst;
        }
    }
}
