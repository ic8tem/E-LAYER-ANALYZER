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
        public virtual string LevelText { get { return Level == 2 ? YAMS.Lang.T("ALLERTA") : YAMS.Lang.T("PRECURSORE"); } }
        public virtual string Key { get { return Region + "|" + Band; } }
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

        // Indice geografico delle nubi attive (ottimizzazione del 28/11/2026): per ogni punto della tratta si guardano solo
        // le nubi nelle celle di 1 grado che possono stare entro il raggio, con un margine rigoroso (formula dell'emisenoverso:
        // sin^2(d/2R) >= sin^2(dlat/2) e >= cos(lat1) cos(lat2) sin^2(dlon/2)), nello stesso ordine di prima e con la stessa
        // distanza: i risultati sono identici, cambia solo quante nubi si confrontano.
        sealed class GeoIdx
        {
            readonly Dictionary<int, List<int>> cells = new Dictionary<int, List<int>>();
            static int Key(int y, int x) { return (y + 90) * 360 + (((x + 180) % 360) + 360) % 360; }
            public GeoIdx(int n, Func<int, double> lat, Func<int, double> lon)
            {
                for (int i = 0; i < n; i++)
                {
                    int k = Key(Math.Max(-90, Math.Min(89, (int)Math.Floor(lat(i)))), (int)Math.Floor(lon(i)));
                    List<int> l;
                    if (!cells.TryGetValue(k, out l)) { l = new List<int>(); cells[k] = l; }
                    l.Add(i);
                }
            }
            public void Query(double lat, double lon, double km, List<int> res)
            {
                res.Clear();
                double dLat = km / (Math.PI * Geo.R / 180) + 0.5;                       // gradi, con mezzo grado di margine
                double hi = Math.Min(90, Math.Max(Math.Abs(lat - dLat), Math.Abs(lat + dLat)));
                double den = Math.Cos(lat * Geo.D2R) * Math.Cos(hi * Geo.D2R), sh = Math.Sin(km / (2 * Geo.R));
                double dLon = den <= 1e-9 || sh * sh >= den * 0.999 ? 180 : 2 * Math.Asin(sh / Math.Sqrt(den)) * Geo.R2D + 0.5;
                int y0 = Math.Max(-90, (int)Math.Floor(lat - dLat)), y1 = Math.Min(89, (int)Math.Floor(lat + dLat));
                int x0, x1;
                if (dLon >= 180) { x0 = -180; x1 = 179; } else { x0 = (int)Math.Floor(lon - dLon); x1 = (int)Math.Floor(lon + dLon); if (x1 - x0 >= 359) { x0 = -180; x1 = 179; } }
                for (int y = y0; y <= y1; y++)
                    for (int x = x0; x <= x1; x++)
                    {
                        List<int> l;
                        if (cells.TryGetValue(Key(y, x), out l)) res.AddRange(l);
                    }
                res.Sort();
            }
        }

        public static List<PrecursorAlert> Last = new List<PrecursorAlert>();
        public static List<StrongCloud> LastStrong = new List<StrongCloud>();

        public static List<PrecursorAlert> Compute(double Hkm)
        {
            List<PrecursorAlert> res = new List<PrecursorAlert>();
            if (!Station.Valid)
            {
                List<StrongCloud> st1;
                List<PrecursorAlert> cor = new List<PrecursorAlert>(Chains.Compute(Hkm, WindowMin, out st1));   // solo i corridoi
                try { cor.InsertRange(0, AprsEs.Compute()); cor.AddRange(Prodromes.Compute(Hkm)); } catch { }
                LastStrong = st1; Last = cor; return cor;
            }
            // nubi attive: punti medi degli spot Es in singolo salto (stazioni qualunque, tutto il mondo)
            List<Active> act = new List<Active>();
            foreach (Spot s in SpotStore.Recent(WindowMin, x => x.HasBoth && x.EsCandidate && !x.IsTep && !EsModel.Ft8Isolated50(x)))
            {
                double d = s.DistanceKm;
                if (d > MaxHopKm || d < 400) continue;
                double la, lo;
                Geo.IntermediateFast(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                act.Add(new Active { Lat = la, Lon = lo, FoEs = s.FreqMHz / Geo.SecantFactor(d, Hkm), Spot = s });
            }
            // stazioni presenti negli spot dell'ultima ora (tutti i modi): dicono dove c'e' chi ascolta
            List<double[]> stations = new List<double[]>();
            foreach (Spot s in SpotStore.Recent(60, x => x.HasBoth)) { stations.Add(new[] { s.LatS, s.LonS }); stations.Add(new[] { s.LatD, s.LonD }); }
            if (act.Count == 0)
            {
                List<StrongCloud> st0;
                List<PrecursorAlert> only = new List<PrecursorAlert>(Chains.Compute(Hkm, WindowMin, out st0));
                try { only.InsertRange(0, AprsEs.Compute()); only.AddRange(Prodromes.Compute(Hkm)); only.AddRange(TepForecast.Compute()); } catch { }
                LastStrong = st0; Last = only; return only;
            }

            GeoIdx idx = new GeoIdx(act.Count, i => act[i].Lat, i => act[i].Lon);
            GeoIdx sidx = new GeoIdx(stations.Count, i => stations[i][0], i => stations[i][1]);
            List<int> buf = new List<int>();
            foreach (var r in Regions)
            {
                double D = Geo.DistanceKmFast(Station.Lat, Station.Lon, r.Lat, r.Lon);
                if (D < MinKm || D > MaxKm) continue;
                double az = Geo.BearingFast(Station.Lat, Station.Lon, r.Lat, r.Lon);
                double dev = Math.Abs(((az % 180) + 180) % 180 - 90);          // 90 = est-ovest, 0 = nord-sud
                if (90 - dev < MinDevFromMeridian) continue;                    // tratta troppo vicina al nord-sud
                if (EsModel.IsTransEquatorialWorld(Station.Lat, Station.Lon, r.Lat, r.Lon)) continue;   // TEP, non Es
                foreach (var band in new[] { ("10 m", 28.5), ("6 m", 50.2), ("2 m", 144.3) })
                {
                    PrecursorAlert best = null;
                    // salti uguali
                    int n = (int)Math.Ceiling(D / MaxHopKm);
                    if (band.Item2 > 100 && n != 2) continue;                    // 144 MHz: solo il doppio salto
                    best = Better(best, Evaluate(r.Name, band.Item1, band.Item2, r.Lat, r.Lon, D, az, 0, Hkm, act, stations, idx, sidx, buf));
                    // primo salto corto: si verifica sempre (500-1200 km), con il resto della tratta a salti uguali
                    PrecursorAlert shortBest = null;
                    if (band.Item2 < 100)
                        for (double S = ShortMinKm; S <= ShortMaxKm + 1; S += 100)
                            shortBest = Better(shortBest, Evaluate(r.Name, band.Item1, band.Item2, r.Lat, r.Lon, D, az, S, Hkm, act, stations, idx, sidx, buf));
                    bool shortFound = shortBest != null && shortBest.Points.Count > 0 && shortBest.Points[0].Covered;
                    PrecursorAlert pick = Better(best, shortBest);
                    if (pick == null || pick.Level == 0) continue;
                    pick.ShortFound = shortFound;
                    if (shortBest != null) pick.ShortKm = shortBest.ShortKm;
                    res.Add(pick);
                }
            }
            res = res.OrderByDescending(a => a.Level).ThenByDescending(a => (double)a.Covered / a.Points.Count).ThenBy(a => a.DistKm).ToList();
            // catene con i salti corti alle estremita' (Chains.cs, dal 20/11/2026): prima quelle dal QTH, poi le regioni,
            // poi i corridoi (prima quelli che riguardano la propria stazione), infine CERCA LA NUBE
            List<StrongCloud> strong;
            List<ChainAlert> ch = Chains.Compute(Hkm, WindowMin, out strong);
            LastStrong = strong;
            List<PrecursorAlert> all = new List<PrecursorAlert>();
            try { all.AddRange(AprsEs.Compute()); } catch { }   // ALLERTA 144 MHz dagli spot APRS ripetuti (dal 22/11/2026)
            all.AddRange(ch.Where(c => c.Kind == ChainAlert.KindQth).OrderByDescending(c => c.Level));
            all.AddRange(ch.Where(c => c.Kind == ChainAlert.KindEqual));
            all.AddRange(res);
            all.AddRange(ch.Where(c => c.Kind == ChainAlert.KindCorridor || c.Kind == ChainAlert.KindEqualCorridor).OrderBy(c => Chains.Mine(c) ? 0 : 1).ThenByDescending(c => c.Level));
            all.AddRange(ch.Where(c => c.Kind == ChainAlert.KindSearch));
            try { all.AddRange(Prodromes.Compute(Hkm)); } catch { }   // prodromi in 24 e 28 MHz (dal 22/11/2026)
            try { all.AddRange(TepForecast.Compute()); } catch { }    // previsione del TEP (dal 22/11/2026)
            Last = all;
            return all;
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
                                       double Hkm, List<Active> act, List<double[]> stations, GeoIdx idx, GeoIdx sidx, List<int> buf)
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
                Geo.IntermediateFast(Station.Lat, Station.Lon, tLat, tLon, h[0] / D, out la, out lo);
                PrecursorPoint p = new PrecursorPoint { Lat = la, Lon = lo, HopKm = h[1], FoEsNeed = f / Geo.SecantFactor(h[1], Hkm), Short = shortKm > 0 && a.Points.Count == 0 };
                idx.Query(la, lo, MatchKm, buf);
                foreach (int ci in buf)
                {
                    Active c = act[ci];
                    double d = Geo.DistanceKmFast(la, lo, c.Lat, c.Lon);
                    if (d > MatchKm || c.FoEs < p.FoEsNeed) continue;
                    if (!p.Covered || c.FoEs > p.CloudFoEs) { p.Covered = true; p.CloudFoEs = c.FoEs; p.CloudKm = d; p.CloudSpot = c.Spot; }
                }
                sidx.Query(la, lo, ObservedKm, buf);
                foreach (int si in buf) { double[] s = stations[si]; if (Geo.DistanceKmFast(la, lo, s[0], s[1]) <= ObservedKm) { p.Observed = true; break; } }
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
