// E-LAYER ANALYZER - PRODROMI in 24 e 28 MHz per i 50 MHz (dal 22/11/2026, richiesta dell'autore, IC8TEM: «verifica dai
// prodromi in 24 e 28 MHz»). Una nube Es vista in singolo salto in 28 MHz (o 24 MHz) dove, nei 30 minuti prima, non c'e'
// nessuno spot Es in 50 MHz entro 300 km: lo strato si sta ionizzando e i 50 MHz possono aprire li' a breve.
// Verifica sulle decodifiche JTDX di IC8TEM, IW2DNI e IZ7NLJ (aprile-settembre 2022-2026, 94 giorni con 28 e 50 MHz insieme,
// uso interno): dopo una nube in 28 MHz i 50 MHz hanno aperto entro 300 km nell'ora successiva nel 50% dei casi (2083 nubi),
// contro il 30% delle zone senza nube in 28 MHz negli stessi istanti; anticipo mediano 9-16 minuti. La foEs ricavata dai
// 28 MHz conta poco: anche le nubi deboli (sotto 7 MHz) danno il 50%. I 24 MHz (12 m) non c'erano nelle decodifiche: si
// trattano come i 28 MHz, con la distanza minima dell'E-sporadico scalata sulla frequenza.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELA
{
    public class ProdromeAlert : PrecursorAlert
    {
        public double CloudLat, CloudLon, FoEs;
        public int N;
        public Spot Spot;
        public string FromBand = "10 m";
        public double NearKm = double.NaN;          // distanza della propria stazione dalla nube
        public override string LevelText { get { return YAMS.Lang.T("PRODROMO"); } }
        public override string Key { get { return "P|" + Math.Round(CloudLat, 1) + "|" + Math.Round(CloudLon, 1); } }
    }

    public static class Prodromes
    {
        public const double WindowMin = 15, ClosedMin = 30, NearKm = 300, MaxFromStationKm = 1300;
        public static bool Enabled = true;

        public static List<ProdromeAlert> Compute(double Hkm)
        {
            List<ProdromeAlert> res = new List<ProdromeAlert>();
            if (!Enabled) return res;
            DateTime now = DateTime.UtcNow;
            List<Spot> low = SpotStore.Recent(WindowMin, s => s.HasBoth && (s.Band == "10m" || s.Band == "12m") && s.EsCandidate && !s.IsTep).ToList();
            if (low.Count == 0) return res;
            List<double[]> six = new List<double[]>();
            foreach (Spot s in SpotStore.Recent(ClosedMin, s => s.HasBoth && s.Band == "6m" && s.EsCandidate && !s.IsTep && !EsModel.Ft8Isolated50(s)))
            {
                double la, lo; Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                six.Add(new[] { la, lo });
            }
            foreach (Spot s in low.OrderByDescending(x => x.Utc))
            {
                double d = s.DistanceKm;
                if (d > Precursors.MaxHopKm) continue;
                double la, lo; Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                if (six.Any(p => Geo.DistanceKm(la, lo, p[0], p[1]) <= NearKm)) continue;           // i 50 MHz sono gia' aperti li'
                ProdromeAlert m = res.FirstOrDefault(x => Geo.DistanceKm(x.CloudLat, x.CloudLon, la, lo) <= 250);
                double foEs = s.FreqMHz / Geo.SecantFactor(d, Hkm);
                if (m != null) { m.N++; if (foEs > m.FoEs) { m.FoEs = foEs; m.Spot = s; } if (s.Band == "12m") m.FromBand = "12 m"; continue; }
                ProdromeAlert a = new ProdromeAlert
                {
                    CloudLat = la, CloudLon = lo, FoEs = foEs, N = 1, Spot = s, FromBand = s.Band == "12m" ? "12 m" : "10 m",
                    Level = 0, Band = "6 m", FreqMHz = 50.2, TargetLat = la, TargetLon = lo, Region = Chains.ZoneName(la, lo)
                };
                if (Station.Valid)
                {
                    a.NearKm = Geo.DistanceKm(Station.Lat, Station.Lon, la, lo);
                    a.DistKm = a.NearKm; a.Az = Geo.Bearing(Station.Lat, Station.Lon, la, lo);
                }
                res.Add(a);
            }
            // con la stazione: prima le nubi a portata di un salto in 50 MHz (fino a 1300 km, cioe' salti fino a 2600 km)
            return res.OrderBy(a => double.IsNaN(a.NearKm) || a.NearKm > MaxFromStationKm ? 1 : 0).ThenByDescending(a => a.N).Take(15).ToList();
        }
    }
}
