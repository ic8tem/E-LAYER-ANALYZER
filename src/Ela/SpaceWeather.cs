// E-LAYER ANALYZER - dati di meteorologia spaziale NOAA SWPC per la linguetta AURORA:
// ovale aurorale OVATION Prime (probabilita' 0-100 % su griglia 1x1 grado), indice Kp planetario, vento solare (RTSW).
// I file si salvano in data/swpc/ e si riusano senza rete.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ELA
{
    public static class SpaceWeather
    {
        public const string OvationUrl = "https://services.swpc.noaa.gov/json/ovation_aurora_latest.json";
        public const string KpUrl = "https://services.swpc.noaa.gov/products/noaa-planetary-k-index.json";
        public const string MagUrl = "https://services.swpc.noaa.gov/json/rtsw/rtsw_mag_1m.json";
        public const string WindUrl = "https://services.swpc.noaa.gov/json/rtsw/rtsw_wind_1m.json";

        public static float[,] Oval;           // [lon 0..359, lat -90..90] probabilita' %
        public static string OvalTime = "";
        public static readonly List<KeyValuePair<DateTime, double>> Kp = new List<KeyValuePair<DateTime, double>>();
        public static double Bz = double.NaN, Bt = double.NaN, Speed = double.NaN, Density = double.NaN;
        public static DateTime WindTime;
        public static DateTime LastFetch = DateTime.MinValue;
        public static string LastError = "";
        static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(40) };

        static SpaceWeather()
        {
            try { http.DefaultRequestHeaders.UserAgent.ParseAdd("E-LAYER-ANALYZER/0.1 (programma per radioamatori)"); } catch { }
        }

        static string Dir(string dataDir) { string d = Path.Combine(dataDir, "swpc"); Directory.CreateDirectory(d); return d; }

        // scarica tutto (thread secondario); restituisce true se almeno un file e' arrivato
        public static async Task<bool> FetchAsync(string dataDir)
        {
            string d = Dir(dataDir);
            bool any = false; LastError = "";
            string[] urls = { OvationUrl, KpUrl, MagUrl, WindUrl };
            string[] files = { "ovation.json", "kp.json", "mag.json", "wind.json" };
            for (int i = 0; i < urls.Length; i++)
            {
                try
                {
                    byte[] b = await http.GetByteArrayAsync(urls[i]);
                    File.WriteAllBytes(Path.Combine(d, files[i]), b);
                    any = true;
                }
                catch (Exception ex) { LastError += files[i] + ": " + ex.Message + " "; }
            }
            LoadCached(dataDir);
            if (any) LastFetch = DateTime.UtcNow;
            return any;
        }

        public static void LoadCached(string dataDir)
        {
            string d = Dir(dataDir);
            try { ParseOvation(Path.Combine(d, "ovation.json")); } catch (Exception ex) { LastError += "ovation: " + ex.Message + " "; }
            try { ParseKp(Path.Combine(d, "kp.json")); } catch (Exception ex) { LastError += "kp: " + ex.Message + " "; }
            try { ParseWind(Path.Combine(d, "mag.json"), Path.Combine(d, "wind.json")); } catch (Exception ex) { LastError += "rtsw: " + ex.Message + " "; }
        }

        static double Num(JsonElement e)
        {
            if (e.ValueKind == JsonValueKind.Number) return e.GetDouble();
            if (e.ValueKind == JsonValueKind.String)
            {
                double v;
                if (double.TryParse(e.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out v)) return v;
            }
            return double.NaN;
        }

        static void ParseOvation(string f)
        {
            if (!File.Exists(f)) return;
            using (JsonDocument doc = JsonDocument.Parse(File.ReadAllBytes(f)))
            {
                JsonElement root = doc.RootElement;
                float[,] g = new float[360, 181];
                JsonElement co;
                if (!root.TryGetProperty("coordinates", out co)) return;
                foreach (JsonElement c in co.EnumerateArray())
                {
                    if (c.GetArrayLength() < 3) continue;
                    int lon = (int)Math.Round(Num(c[0])), lat = (int)Math.Round(Num(c[1]));
                    double v = Num(c[2]);
                    lon = ((lon % 360) + 360) % 360;
                    if (lat < -90 || lat > 90 || double.IsNaN(v)) continue;
                    g[lon, lat + 90] = (float)v;
                }
                JsonElement t;
                OvalTime = root.TryGetProperty("Forecast Time", out t) ? t.GetString() : (root.TryGetProperty("Observation Time", out t) ? t.GetString() : "");
                Oval = g;
            }
        }

        static DateTime Time(string s)
        {
            DateTime t;
            if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out t)) return t;
            return DateTime.MinValue;
        }

        // Kp: vecchio formato (tabella di stringhe con intestazione) e nuovo formato (oggetti) dal 2026
        static void ParseKp(string f)
        {
            if (!File.Exists(f)) return;
            List<KeyValuePair<DateTime, double>> res = new List<KeyValuePair<DateTime, double>>();
            using (JsonDocument doc = JsonDocument.Parse(File.ReadAllBytes(f)))
            {
                foreach (JsonElement r in doc.RootElement.EnumerateArray())
                {
                    if (r.ValueKind == JsonValueKind.Array)
                    {
                        if (r.GetArrayLength() < 2 || r[0].GetString() == "time_tag") continue;
                        res.Add(new KeyValuePair<DateTime, double>(Time(r[0].GetString()), Num(r[1])));
                    }
                    else if (r.ValueKind == JsonValueKind.Object)
                    {
                        JsonElement t, k;
                        if (!r.TryGetProperty("time_tag", out t)) continue;
                        if (!r.TryGetProperty("Kp", out k) && !r.TryGetProperty("kp", out k) && !r.TryGetProperty("kp_index", out k)) continue;
                        res.Add(new KeyValuePair<DateTime, double>(Time(t.GetString()), Num(k)));
                    }
                }
            }
            res.RemoveAll(x => double.IsNaN(x.Value) || x.Key == DateTime.MinValue);
            res.Sort((a, b) => a.Key.CompareTo(b.Key));
            lock (Kp) { Kp.Clear(); Kp.AddRange(res); }
        }

        static JsonElement? LastActive(JsonElement arr)
        {
            JsonElement? best = null; DateTime bt = DateTime.MinValue;
            foreach (JsonElement r in arr.EnumerateArray())
            {
                if (r.ValueKind != JsonValueKind.Object) continue;
                JsonElement a;
                if (r.TryGetProperty("active", out a) && a.ValueKind == JsonValueKind.False) continue;
                JsonElement t;
                if (!r.TryGetProperty("time_tag", out t)) continue;
                DateTime tt = Time(t.GetString());
                if (tt > bt) { bt = tt; best = r; }
            }
            return best;
        }

        static void ParseWind(string magF, string windF)
        {
            if (File.Exists(magF))
                using (JsonDocument doc = JsonDocument.Parse(File.ReadAllBytes(magF)))
                {
                    JsonElement? r = LastActive(doc.RootElement);
                    if (r.HasValue)
                    {
                        JsonElement v;
                        Bz = r.Value.TryGetProperty("bz_gsm", out v) ? Num(v) : double.NaN;
                        Bt = r.Value.TryGetProperty("bt", out v) ? Num(v) : double.NaN;
                        if (r.Value.TryGetProperty("time_tag", out v)) WindTime = Time(v.GetString());
                    }
                }
            if (File.Exists(windF))
                using (JsonDocument doc = JsonDocument.Parse(File.ReadAllBytes(windF)))
                {
                    JsonElement? r = LastActive(doc.RootElement);
                    if (r.HasValue)
                    {
                        JsonElement v;
                        Speed = r.Value.TryGetProperty("proton_speed", out v) ? Num(v) : double.NaN;
                        Density = r.Value.TryGetProperty("proton_density", out v) ? Num(v) : double.NaN;
                    }
                }
        }

        public static double LatestKp
        {
            get { lock (Kp) return Kp.Count > 0 ? Kp[Kp.Count - 1].Value : double.NaN; }
        }

        // probabilita' dell'ovale (0..100) in un punto
        public static double OvalAt(double lat, double lon)
        {
            float[,] g = Oval;
            if (g == null) return 0;
            int lo = ((int)Math.Round(lon) % 360 + 360) % 360, la = (int)Math.Round(lat) + 90;
            if (la < 0 || la > 180) return 0;
            return g[lo, la];
        }

        // confine equatoriale dell'ovale (prima latitudine con probabilita' >= thr) alla longitudine data
        public static double EquatorBoundary(double lon, double thr, bool north = true)
        {
            if (Oval == null) return double.NaN;
            if (north) { for (int la = 30; la <= 89; la++) if (OvalAt(la, lon) >= thr) return la; }
            else { for (int la = -30; la >= -89; la--) if (OvalAt(la, lon) >= thr) return la; }
            return double.NaN;
        }
    }
}
