// YA-MS - rifrazione troposferica dal modello GFS (facoltativa, spenta all'avvio: senza rifrazione la zona comune e' piu'
// piccola, quindi il valore e' piu' prudente). Richiesta dell'autore, IC8TEM, 22/11/2026.
// Il k = 4/3 non si applica: descrive un raggio tutto dentro la troposfera, mentre verso una scia a 80-110 km il raggio
// attraversa la troposfera solo nei primi chilometri. Qui si traccia il raggio vero in un'atmosfera a strati sferici:
//  - profilo di rifrattivita' N(h) sopra ciascuna stazione dal GFS (Open-Meteo, come in LIVETROPOVHF): suolo e 10 livelli
//    da 1000 a 500 hPa, quote con l'equazione ipsometrica (temperatura virtuale), N = 77,6/T·(P + 4810·e/T) (ITU-R P.453);
//  - sopra l'ultimo livello N decresce in modo esponenziale con altezza di scala 7,35 km (atmosfera di riferimento P.453);
//  - senza GFS si usa l'atmosfera di riferimento ITU-R P.453, N(h) = 315·exp(-h/7,35);
//  - legge di Snell in coordinate sferiche: n·r·cos(e) = costante; l'angolo al centro fino alla quota H vale
//    phi = somma di k·dr / (r·u), con u = sqrt(n²r² − k²) (valore medio nello strato, senza singolarita' a e = 0).
// La correzione restituita e' l'elevazione apparente meno quella geometrica di un punto alla quota delle scie:
// e' positiva (il punto si vede piu' alto) e rende visibili anche punti appena sotto l'orizzonte geometrico.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.Json;

namespace YAMS
{
    public class RefrProfile
    {
        public List<double> Z = new List<double>(), N = new List<double>();   // quota sul mare (km) e rifrattivita'
        public string Source = "";
        public DateTime TimeUtc;

        public double At(double hKm)
        {
            if (Z.Count == 0) return 315 * Math.Exp(-hKm / 7.35);
            if (hKm <= Z[0]) return N[0];
            for (int i = 1; i < Z.Count; i++)
                if (hKm <= Z[i]) return N[i - 1] + (N[i] - N[i - 1]) * (hKm - Z[i - 1]) / (Z[i] - Z[i - 1]);
            double top = Z[Z.Count - 1];
            return hKm > 80 ? 0 : N[N.Count - 1] * Math.Exp(-(hKm - top) / 7.35);
        }
    }

    public static class Refraction
    {
        public static readonly int[] PLevels = { 1000, 975, 950, 925, 900, 850, 800, 700, 600, 500 };

        static double Es(double tC) { return 6.1121 * Math.Exp(17.502 * tC / (tC + 240.97)); }
        static double NOf(double pHpa, double tC, double rh)
        {
            double tk = tC + 273.15, e = Math.Max(0, Math.Min(100, rh)) / 100.0 * Es(tC);
            return 77.6 / tk * (pHpa + 4810.0 * e / tk);
        }
        static double Tv(double tC, double rh, double p)
        {
            double e = Math.Max(0, Math.Min(100, rh)) / 100.0 * Es(tC);
            double q = 0.622 * e / Math.Max(1, p - 0.378 * e);
            return (tC + 273.15) * (1 + 0.61 * q);
        }

        public static RefrProfile Reference()
        {
            return new RefrProfile { Source = Lang.T("atmosfera di riferimento ITU-R P.453") };
        }

        static string Url(LatLon p)
        {
            List<string> v = new List<string> { "surface_pressure", "temperature_2m", "relative_humidity_2m" };
            foreach (int l in PLevels) { v.Add("temperature_" + l + "hPa"); v.Add("relative_humidity_" + l + "hPa"); }
            return "https://api.open-meteo.com/v1/forecast?latitude=" + p.Lat.ToString("0.###", CultureInfo.InvariantCulture) +
                   "&longitude=" + p.Lon.ToString("0.###", CultureInfo.InvariantCulture) + "&hourly=" + string.Join(",", v) +
                   "&models=gfs_seamless&past_days=1&forecast_days=3&timezone=GMT";
        }

        // profilo GFS sopra il punto p all'ora piu' vicina a utc; copia sul disco valida 3 ore. null se non disponibile.
        public static RefrProfile FetchGfs(string dataDir, LatLon p, DateTime utc, out string err)
        {
            err = "";
            string file = Path.Combine(dataDir, "radar", "gfs_" + p.Lat.ToString("0.0", CultureInfo.InvariantCulture) + "_" + p.Lon.ToString("0.0", CultureInfo.InvariantCulture) + ".json");
            try
            {
                if (!File.Exists(file) || DateTime.UtcNow - File.GetLastWriteTimeUtc(file) > TimeSpan.FromHours(3))
                {
                    DateTime lm = DateTime.MinValue; bool off;
                    if (RadarFlux.Fetch(Url(p), file + ".dl", ref lm, out off, out err) != 1) return null;
                    if (File.Exists(file)) File.Delete(file);
                    File.Move(file + ".dl", file);
                    File.SetLastWriteTimeUtc(file, DateTime.UtcNow);
                }
                using (JsonDocument doc = JsonDocument.Parse(File.ReadAllText(file)))
                {
                    JsonElement root = doc.RootElement, h = root.GetProperty("hourly");
                    double elev = root.TryGetProperty("elevation", out JsonElement el) ? el.GetDouble() : 0;
                    JsonElement times = h.GetProperty("time");
                    int best = -1; double bd = double.MaxValue; DateTime bt = DateTime.MinValue;
                    for (int i = 0; i < times.GetArrayLength(); i++)
                    {
                        DateTime t = DateTime.SpecifyKind(DateTime.ParseExact(times[i].GetString(), "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture), DateTimeKind.Utc);
                        double d = Math.Abs((t - utc).TotalHours);
                        if (d < bd) { bd = d; best = i; bt = t; }
                    }
                    if (best < 0 || bd > 6) { err = Lang.T("ora fuori dalla previsione GFS"); return null; }
                    Func<string, double> V = name =>
                    {
                        JsonElement a;
                        if (!h.TryGetProperty(name, out a) || a.GetArrayLength() <= best || a[best].ValueKind != JsonValueKind.Number) return double.NaN;
                        return a[best].GetDouble();
                    };
                    double ps = V("surface_pressure"), t2 = V("temperature_2m"), rh2 = V("relative_humidity_2m");
                    if (double.IsNaN(ps) || double.IsNaN(t2) || double.IsNaN(rh2)) { err = Lang.T("dati GFS incompleti"); return null; }
                    RefrProfile pr = new RefrProfile { TimeUtc = bt, Source = Lang.F("GFS {0} UTC", bt.ToString("dd/MM HH", CultureInfo.InvariantCulture)) };
                    double z = (elev + 2) / 1000.0, pPrev = ps, tPrev = t2, rhPrev = rh2;
                    pr.Z.Add(z); pr.N.Add(NOf(ps, t2, rh2));
                    foreach (int l in PLevels)
                    {
                        if (l >= ps - 2) continue;
                        double t = V("temperature_" + l + "hPa"), rh = V("relative_humidity_" + l + "hPa");
                        if (double.IsNaN(t) || double.IsNaN(rh)) continue;
                        double tv = (Tv(tPrev, rhPrev, pPrev) + Tv(t, rh, l)) / 2;
                        z += 287.05 * tv / 9.80665 * Math.Log(pPrev / l) / 1000.0;
                        pr.Z.Add(z); pr.N.Add(NOf(l, t, rh));
                        pPrev = l; tPrev = t; rhPrev = rh;
                    }
                    return pr.Z.Count >= 4 ? pr : null;
                }
            }
            catch (Exception ex) { err = ex.Message; return null; }
        }

        // angolo al centro (rad) percorso dal raggio partito dalla quota h0 (km) con elevazione apparente eDeg fino alla quota H;
        // NaN se il raggio non ci arriva (intrappolato in un condotto). straight = raggio diritto (n = 1).
        public static double CentralAngle(RefrProfile pr, double h0, double H, double eDeg, bool straight)
        {
            double R = Astro.R;
            Func<double, double> n = h => straight ? 1.0 : 1.0 + pr.At(h) * 1e-6;
            double r0 = R + h0, k = n(h0) * r0 * Math.Cos(eDeg * Astro.D2R);
            double phi = 0, h = h0;
            double u1 = Math.Sqrt(Math.Max(0, Math.Pow(n(h0) * r0, 2) - k * k));
            while (h < H - 1e-9)
            {
                double dh = h < h0 + 2 ? 0.01 : h < 20 ? 0.1 : 0.5;
                double h2 = Math.Min(H, h + dh), r1 = R + h, r2 = R + h2;
                double a = Math.Pow(n(h2) * r2, 2) - k * k;
                if (a <= 0) return double.NaN;
                double u2 = Math.Sqrt(a), um = (u1 + u2) / 2;
                if (um <= 0) return double.NaN;
                phi += k * (h2 - h) / ((r1 + r2) / 2 * um);
                h = h2; u1 = u2;
            }
            return phi;
        }

        // elevazione geometrica (gradi) di un punto alla quota H e ad angolo al centro phi, vista dalla quota h0
        public static double GeometricElev(double h0, double H, double phi)
        {
            double R = Astro.R;
            return Math.Atan2((R + H) * Math.Cos(phi) - (R + h0), (R + H) * Math.Sin(phi)) / Astro.D2R;
        }

        // Tabella della correzione: elevazione apparente - elevazione geometrica, in funzione dell'elevazione geometrica,
        // per punti alla quota H visti dalla quota h0. Restituisce la funzione (NaN sotto l'orizzonte rifratto).
        public static Func<double, double> Correction(RefrProfile pr, double h0, double H)
        {
            List<double> eg = new List<double>(), corr = new List<double>();
            for (double ea = 0; ea <= 30.0001; ea += ea < 5 ? 0.1 : 0.5)
            {
                double phi = CentralAngle(pr, h0, H, ea, false);
                if (double.IsNaN(phi)) continue;
                double g = GeometricElev(h0, H, phi);
                if (eg.Count > 0 && g <= eg[eg.Count - 1]) continue;
                eg.Add(g); corr.Add(ea - g);
            }
            if (eg.Count < 2) return e => 0;
            double[] E = eg.ToArray(), C = corr.ToArray();
            return e =>
            {
                if (e < E[0]) return double.NaN;
                if (e >= E[E.Length - 1]) return C[C.Length - 1] * Math.Tan(E[E.Length - 1] * Astro.D2R) / Math.Tan(Math.Max(1, e) * Astro.D2R);
                int i = Array.BinarySearch(E, e);
                if (i >= 0) return C[i];
                i = ~i;
                return C[i - 1] + (C[i] - C[i - 1]) * (e - E[i - 1]) / (E[i] - E[i - 1]);
            };
        }

        // distanza a terra (km) alla quale il raggio con elevazione apparente eDeg raggiunge la quota H
        public static double LayerRangeKm(RefrProfile pr, double h0, double H, double eDeg)
        {
            double phi = CentralAngle(pr, h0, H, eDeg, false);
            return double.IsNaN(phi) ? MsGeometry.LayerRangeKm(H, eDeg, h0) : phi * Astro.R;
        }
    }
}
