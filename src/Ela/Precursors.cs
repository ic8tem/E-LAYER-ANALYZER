// E-LAYER ANALYZER - allerta dell'E-sporadico multi-salto (28 e 50 MHz; in 144 MHz solo il doppio salto) dai "precursori":
// le nubi Es attive in altri continenti, viste dagli spot in singolo salto tra stazioni qualunque, che cadono sui punti di
// riflessione di una tratta dalla propria stazione verso una regione lontana.
// Scelte (approvate dall'autore, IC8TEM):
//  - si guardano solo le tratte trasversali (lontane dalla direzione nord-sud): il multi-salto Es avviene in senso est-ovest,
//    e le strutture Es sono allungate in quella direzione (Takata [P3]); le tratte che rispondono alla regola TEP sono escluse;
//  - i salti seguono la tabella di IK3XTV [3] (singolo salto fino a 2400 km); si verifica SEMPRE anche l'ipotesi del primo
//    salto corto entro 1200 km dal QTH (esperienza IC8TEM [4]), dicendo se la nube del primo salto corto c'e';
//  - un punto di salto e' "coperto" se entro 300 km c'e' una nube attiva con foEs sufficiente (MUF = foEs sec(phi));
//    "non osservato" se nei dintorni (1200 km) non ci sono stazioni negli spot: li' l'assenza di spot non e' assenza di Es;
//  - ALLERTA: tutti i punti coperti; PRECURSORE: manca un solo punto ed e' non osservato.
// Esempi reali: Europa-Guam in 50 MHz, circa 6 salti, preceduta da aperture Europa-Giappone (EI7GL [P1]).
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELA
{
    public class PrecursorPoint
    {
        public double Lat, Lon, HopKm, FoEsNeed;
        public bool Covered, Observed, Short;
        public double CloudFoEs = double.NaN, CloudKm = double.NaN;
        public Spot CloudSpot;
    }

    public class PrecursorAlert
    {
        public string Region, Band;
        public double FreqMHz, TargetLat, TargetLon, DistKm, Az;
        public bool ShortHyp;                 // ipotesi del primo salto corto
        public bool ShortFound;               // nube presente sul primo salto corto (sempre verificata)
        public double ShortKm;
        public int Level;                     // 2 = ALLERTA, 1 = PRECURSORE
        public List<PrecursorPoint> Points = new List<PrecursorPoint>();
        public int Covered { get { return Points.Count(p => p.Covered); } }
        public string LevelText { get { return Level == 2 ? YAMS.Lang.T("ALLERTA") : YAMS.Lang.T("PRECURSORE"); } }
    }

    public static class Precursors
    {
        public const double MinKm = 2800, MaxKm = 13000;        // dal doppio salto di IK3XTV fino ai record (EI7GL [56])
        public const double MaxHopKm = 2400, ShortMaxKm = 1200, ShortMinKm = 500;
        public const double MatchKm = 300, ObservedKm = 1200, MinDevFromMeridian = 30;
        public static double WindowMin = 30;

        // nomi tradotti con Lang.T: Lang.T("Giappone") Lang.T("Corea") Lang.T("Cina orientale") Lang.T("Filippine") Lang.T("Guam e Marianne") Lang.T("Indonesia") Lang.T("Australia settentrionale") Lang.T("Australia orientale") Lang.T("Nuova Zelanda") Lang.T("India") Lang.T("Penisola arabica") Lang.T("Asia centrale") Lang.T("Siberia orientale") Lang.T("Europa occidentale") Lang.T("Europa orientale") Lang.T("Scandinavia") Lang.T("Africa occidentale") Lang.T("Africa australe") Lang.T("Caraibi") Lang.T("America centrale") Lang.T("Messico") Lang.T("Nord America orientale") Lang.T("Nord America centrale") Lang.T("Nord America occidentale") Lang.T("Canada settentrionale") Lang.T("Alaska") Lang.T("Hawaii") Lang.T("Sud America settentrionale") Lang.T("Brasile") Lang.T("Cono sud")
        // regioni obiettivo in tutto il mondo (centro approssimato)
        public static readonly (string Name, double Lat, double Lon)[] Regions =
        {
            ("Giappone", 36, 138), ("Corea", 37, 127.5), ("Cina orientale", 31, 118), ("Filippine", 13, 122), ("Guam e Marianne", 14, 145),
            ("Indonesia", -4, 112), ("Australia settentrionale", -14, 132), ("Australia orientale", -28, 152), ("Nuova Zelanda", -40, 175),
            ("India", 21, 79), ("Penisola arabica", 24, 46), ("Asia centrale", 43, 70), ("Siberia orientale", 55, 135),
            ("Europa occidentale", 47, 2), ("Europa orientale", 54, 35), ("Scandinavia", 62, 15), ("Africa occidentale", 13, -5),
            ("Africa australe", -27, 26), ("Caraibi", 18, -68), ("America centrale", 15, -88), ("Messico", 23, -102),
            ("Nord America orientale", 40, -76), ("Nord America centrale", 40, -95), ("Nord America occidentale", 38, -120),
            ("Canada settentrionale", 60, -95), ("Alaska", 62, -150), ("Hawaii", 21, -157), ("Sud America settentrionale", 6, -66),
            ("Brasile", -15, -47), ("Cono sud", -34, -60),
        };

        class Active { public double Lat, Lon, FoEs; public Spot Spot; }

        public static List<PrecursorAlert> Last = new List<PrecursorAlert>();

        public static List<PrecursorAlert> Compute(double Hkm)
        {
            List<PrecursorAlert> res = new List<PrecursorAlert>();
            if (!Station.Valid) { Last = res; return res; }
            // nubi attive: punti medi degli spot Es in singolo salto (stazioni qualunque, tutto il mondo)
            List<Active> act = new List<Active>();
            foreach (Spot s in SpotStore.Recent(WindowMin, x => x.HasBoth && x.EsCandidate && !x.IsTep))
            {
                double d = s.DistanceKm;
                if (d > MaxHopKm || d < 400) continue;
                double la, lo;
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                act.Add(new Active { Lat = la, Lon = lo, FoEs = s.FreqMHz / Geo.SecantFactor(d, Hkm), Spot = s });
            }
            // stazioni presenti negli spot dell'ultima ora (tutti i modi): dicono dove c'e' chi ascolta
            List<double[]> stations = new List<double[]>();
            foreach (Spot s in SpotStore.Recent(60, x => x.HasBoth)) { stations.Add(new[] { s.LatS, s.LonS }); stations.Add(new[] { s.LatD, s.LonD }); }
            if (act.Count == 0) { Last = res; return res; }

            foreach (var r in Regions)
            {
                double D = Geo.DistanceKm(Station.Lat, Station.Lon, r.Lat, r.Lon);
                if (D < MinKm || D > MaxKm) continue;
                double az = Geo.Bearing(Station.Lat, Station.Lon, r.Lat, r.Lon);
                double dev = Math.Abs(((az % 180) + 180) % 180 - 90);          // 90 = est-ovest, 0 = nord-sud
                if (90 - dev < MinDevFromMeridian) continue;                    // tratta troppo vicina al nord-sud
                if (EsModel.IsTransEquatorialWorld(Station.Lat, Station.Lon, r.Lat, r.Lon)) continue;   // TEP, non Es
                foreach (var band in new[] { ("10 m", 28.5), ("6 m", 50.2), ("2 m", 144.3) })
                {
                    PrecursorAlert best = null;
                    // salti uguali
                    int n = (int)Math.Ceiling(D / MaxHopKm);
                    if (band.Item2 > 100 && n != 2) continue;                    // 144 MHz: solo il doppio salto
                    best = Better(best, Evaluate(r.Name, band.Item1, band.Item2, r.Lat, r.Lon, D, az, 0, Hkm, act, stations));
                    // primo salto corto: si verifica sempre (500-1200 km), con il resto della tratta a salti uguali
                    PrecursorAlert shortBest = null;
                    if (band.Item2 < 100)
                        for (double S = ShortMinKm; S <= ShortMaxKm + 1; S += 100)
                            shortBest = Better(shortBest, Evaluate(r.Name, band.Item1, band.Item2, r.Lat, r.Lon, D, az, S, Hkm, act, stations));
                    bool shortFound = shortBest != null && shortBest.Points.Count > 0 && shortBest.Points[0].Covered;
                    PrecursorAlert pick = Better(best, shortBest);
                    if (pick == null || pick.Level == 0) continue;
                    pick.ShortFound = shortFound;
                    if (shortBest != null) pick.ShortKm = shortBest.ShortKm;
                    res.Add(pick);
                }
            }
            res = res.OrderByDescending(a => a.Level).ThenByDescending(a => (double)a.Covered / a.Points.Count).ThenBy(a => a.DistKm).ToList();
            Last = res;
            return res;
        }

        static PrecursorAlert Better(PrecursorAlert a, PrecursorAlert b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (b.Level != a.Level) return b.Level > a.Level ? b : a;
            double fa = (double)a.Covered / a.Points.Count, fb = (double)b.Covered / b.Points.Count;
            return fb > fa ? b : a;
        }

        static PrecursorAlert Evaluate(string region, string band, double f, double tLat, double tLon, double D, double az, double shortKm,
                                       double Hkm, List<Active> act, List<double[]> stations)
        {
            PrecursorAlert a = new PrecursorAlert { Region = region, Band = band, FreqMHz = f, TargetLat = tLat, TargetLon = tLon, DistKm = D, Az = az, ShortHyp = shortKm > 0, ShortKm = shortKm };
            List<double[]> hops = new List<double[]>();     // {centro lungo la tratta (km), lunghezza del salto}
            if (shortKm > 0)
            {
                double rest = D - shortKm;
                int m = (int)Math.Ceiling(rest / MaxHopKm);
                if (rest / m < ShortMinKm) return null;
                hops.Add(new[] { shortKm / 2, shortKm });
                for (int j = 0; j < m; j++) hops.Add(new[] { shortKm + (j + 0.5) * rest / m, rest / m });
            }
            else
            {
                int n = (int)Math.Ceiling(D / MaxHopKm);
                for (int j = 0; j < n; j++) hops.Add(new[] { (j + 0.5) * D / n, D / n });
            }
            foreach (double[] h in hops)
            {
                double la, lo;
                Geo.Intermediate(Station.Lat, Station.Lon, tLat, tLon, h[0] / D, out la, out lo);
                PrecursorPoint p = new PrecursorPoint { Lat = la, Lon = lo, HopKm = h[1], FoEsNeed = f / Geo.SecantFactor(h[1], Hkm), Short = shortKm > 0 && a.Points.Count == 0 };
                foreach (Active c in act)
                {
                    double d = Geo.DistanceKm(la, lo, c.Lat, c.Lon);
                    if (d > MatchKm || c.FoEs < p.FoEsNeed) continue;
                    if (!p.Covered || c.FoEs > p.CloudFoEs) { p.Covered = true; p.CloudFoEs = c.FoEs; p.CloudKm = d; p.CloudSpot = c.Spot; }
                }
                foreach (double[] s in stations) if (Geo.DistanceKm(la, lo, s[0], s[1]) <= ObservedKm) { p.Observed = true; break; }
                a.Points.Add(p);
            }
            int cov = a.Covered, miss = a.Points.Count - cov;
            bool missUnobserved = a.Points.Where(p => !p.Covered).All(p => !p.Observed);
            // il primo salto (corto o uguale) parte dalla propria stazione: la zona e' sempre osservata dall'utente stesso
            if (miss == 0) a.Level = 2;
            else if (miss == 1 && cov >= 1 && missUnobserved) a.Level = 1;
            else a.Level = 0;
            if (f > 100 && a.Level < 2) a.Level = 0;                         // 144 MHz: solo se entrambe le nubi ci sono
            return a;
        }
    }
}
