// E-LAYER ANALYZER - previsione dalla nube Es in formazione e modello misto E-sporadico + tropo (dalla 0.7.0, proposta
// approvata dall'autore il 28/11/2026).
// 1) Nubi: punti medi degli spot Es a singolo salto degli ultimi 30 minuti (tutte le bande 28-144 MHz: il fenomeno e' uno
//    solo, regola dell'autore del 20/11/2026) e ionosonde GIRO; foEs minima con la legge della secante; nube "in formazione"
//    se la foEs degli ultimi 10 minuti supera quella dei 20 minuti prima, o se la nube e' nata negli ultimi 10 minuti.
//    Previsione dell'Es iperdenso di IK3XTV [51]: le aperture in 144 MHz sono rare (circa il 5% delle aperture).
// 2) Ipotesi dal QTH delle impostazioni, 50 e 144 MHz trattati separatamente (regola dell'autore del 20/11/2026), in ordine di
//    priorita': a) primo salto utile (singolo salto); b) doppio salto E-sporadico, se dove atterra il primo salto c'e' un'altra
//    nube che regge la banda (EA6VQ [89]: EA3DXU-RN6BN, 2914 km, due nubi; G0ISW [65]: doppio salto anche in 144 MHz);
//    c) solo se il primo salto non c'e': tropo + E-sporadico, con una "stazione virtuale" A' raggiunta in condotto
//    (EI7GL [41][90], EA6VQ [89]: EA8-I2 oltre 2900 km). La fonte [88] e' critica sul tropo+Es e preferisce il doppio
//    salto: per questo il doppio salto viene prima.
// 3) Accoppiamento nel condotto: il raggio riflesso dall'Es arriva su A' con l'elevazione del salto; resta intrappolato solo
//    se l'elevazione non supera l'angolo critico di ITU-R P.834 [86] (i modi intrappolati stanno sotto 1 grado [87]).
//    Oltre l'angolo critico: perdita di accoppiamento 20 log10(e/alfa) dB, fino a 15 dB l'ipotesi resta (scelta del programma).
// 4) Abbattimento: salto Es con ITU-R P.534-6 [76] (spazio libero sul percorso obliquo, eq. 5, e attenuazione ionosferica
//    Gamma1, eq. 3); tratta in condotto: spazio libero sulla lunghezza totale piu' 0,05 dB/km se il condotto intrappola la
//    banda e fino a 0,15 dB/km per il condotto "morbido" (valori di partenza del programma, da tarare con le osservazioni).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using YAMS;

namespace ELA
{
    public class EsCloud
    {
        public int Id;
        public double Lat, Lon, RadiusKm = 150;
        public double FoEs = double.NaN, FoEsOld = double.NaN;   // massimo degli ultimi 10 minuti e dei 20 minuti prima
        public int Trend;          // +1 in formazione, 0 stabile, -1 in calo
        public bool New;           // nata negli ultimi 10 minuti
        public int Spots;
        public double GiroFoEs = double.NaN; public string GiroName = "";
        public HashSet<string> Bands = new HashSet<string>();
        public DateTime Last;
        public double SumW, SumLat, SumLon;
        public double Score { get { return Math.Min(1, 0.6 * Math.Min(1, Spots / 4.0) + (Bands.Count > 1 ? 0.2 : 0) + (!double.IsNaN(GiroFoEs) ? 0.2 : 0) + (Spots == 0 && !double.IsNaN(GiroFoEs) ? 0.3 : 0)); } }
        public double BestFoEs { get { return Math.Max(double.IsNaN(FoEs) ? 0 : FoEs, double.IsNaN(GiroFoEs) ? 0 : GiroFoEs); } }
        public string Loc { get { return Geo.LatLonToLoc(Lat, Lon, 4); } }
    }

    public enum HypKind { Single = 0, Double = 1, TropoEs = 2 }

    public class EsHypothesis
    {
        public HypKind Kind;
        public double FMHz;
        public EsCloud Cloud, Cloud2;
        public double Az, HopKm, Hop2Km, ElevDeg, MufMHz, Muf2MHz;
        public double LandLat, LandLon, Land2Lat = double.NaN, Land2Lon = double.NaN;
        public bool LandOnLand = true, Land2OnLand = true;
        // tropo
        public double VLat = double.NaN, VLon = double.NaN, TropoKm, CriticalDeg, CouplingDb, Soft;
        public string DuctSource = "";
        public bool TropoAtFarEnd;          // estensione tropo dopo l'atterraggio (segnale che continua in condotto)
        public double ExtLat = double.NaN, ExtLon = double.NaN, ExtKm;
        public double LossDb;               // abbattimento stimato (dB)
        public double Score;
        public string Window = "";
    }

    public static class EsForecast
    {
        public static double WindowMin = 30, RecentMin = 10;
        public static bool UseTrEs = true;   // TR+ES anche nella linguetta REAL-TIME (casella della linguetta PREVISIONE)
        public static double CouplingMaxDb = 15;
        public static double DuctDbPerKm = 0.05, SoftDbPerKm = 0.15;
        public static double TropoMinKm = 100, TropoMaxKm = 800, TropoStepKm = 50;
        public static double LineKm = 200;         // distanza massima di una nube dalla traiettoria (doppio salto)
        public static readonly double[] BandsMHz = { 50.2, 144.3 };

        public static double MaxHopKm(double H) { return 2 * Geo.HalfHopKm(EsModel.DefaultElevMin, H, EsModel.EsK); }
        public static double MinHopKm(double f) { return f > 100 ? EsModel.MinKm144 : EsModel.MinKm50; }

        // ---------------------------------------------------------------- ITU-R P.534-6 [76]

        // lunghezza del percorso obliquo di un salto (km), eq. 5 con R0 = 8500 km
        public static double SlantKm(double dKm, double H)
        {
            const double R0 = 8500;
            double x = dKm / (2 * R0);
            return 2 * Math.Sqrt(R0 * R0 + (R0 + H) * (R0 + H) - 2 * R0 * (R0 + H) * Math.Cos(x));
        }
        // attenuazione ionosferica del salto singolo (dB), eq. 3 (valida fino a 2600 km e per f/foEs tra 1 e 8)
        public static double Gamma1(double dKm, double f, double foEs)
        {
            if (foEs <= 0) return 99;
            double a = 40.0 / (1 + dKm / 130 + Math.Pow(dKm / 250, 2)) + 0.2 * Math.Pow(dKm / 2600, 2);
            double r = 1000 * f / (foEs * (1000 + dKm));
            return a * r * r + Math.Exp((dKm - 1660) / 280);
        }
        public static double Fsl(double km, double f) { return 32.45 + 20 * Math.Log10(Math.Max(1e-3, f)) + 20 * Math.Log10(Math.Max(1e-3, km)); }

        // ---------------------------------------------------------------- nubi

        static bool EsSpot(Spot x)
        {
            return x.HasBoth && x.EsCandidate && !EsModel.Ft8Isolated50(x) && !x.IsTep && !EsModel.IsAtlanticTropoCandidate(x)
                && (x.Band == "10m" || x.Band == "8m" || x.Band == "6m" || x.Band == "4m" || (x.Band == "2m" && x.DistanceKm >= EsModel.MinKm144));
        }

        public static List<EsCloud> Clouds(double H, DateTime now, IEnumerable<Spot> spots, bool giro)
        {
            List<EsCloud> res = new List<EsCloud>();
            List<Tuple<EsPath, Spot>> pts = new List<Tuple<EsPath, Spot>>();
            HashSet<string> seen = new HashSet<string>();
            foreach (Spot s in spots.Where(EsSpot).OrderByDescending(x => x.Utc))
            {
                if (!seen.Add(s.DxCall + "|" + s.Spotter + "|" + s.Band)) continue;
                EsPath p = EsModel.Build(s, H);
                if (p == null || p.Hops != 1) continue;   // solo il singolo salto dice dove sta la nube
                pts.Add(Tuple.Create(p, s));
            }
            pts.Sort((a, b) => b.Item1.FoEsMin.CompareTo(a.Item1.FoEsMin));
            foreach (var t in pts)
            {
                EsHop m = t.Item1.Points[0];
                EsCloud c = res.FirstOrDefault(x => Geo.DistanceKmFast(x.Lat, x.Lon, m.Lat, m.Lon) <= EsModel.CloudMatchKm);
                if (c == null) { c = new EsCloud { Id = res.Count + 1, Lat = m.Lat, Lon = m.Lon }; res.Add(c); }
                double w = Math.Max(0.5, t.Item1.FoEsMin);
                c.SumW += w; c.SumLat += w * m.Lat; c.SumLon += w * m.Lon;
                c.Lat = c.SumLat / c.SumW; c.Lon = c.SumLon / c.SumW;
                c.RadiusKm = Math.Max(c.RadiusKm, Math.Min(EsModel.CloudMatchKm, Geo.DistanceKmFast(c.Lat, c.Lon, m.Lat, m.Lon)));
                c.Spots++; c.Bands.Add(t.Item2.Band);
                if (t.Item2.Utc > c.Last) c.Last = t.Item2.Utc;
                bool recent = (now - t.Item2.Utc).TotalMinutes <= RecentMin;
                if (recent) c.FoEs = double.IsNaN(c.FoEs) ? t.Item1.FoEsMin : Math.Max(c.FoEs, t.Item1.FoEsMin);
                else c.FoEsOld = double.IsNaN(c.FoEsOld) ? t.Item1.FoEsMin : Math.Max(c.FoEsOld, t.Item1.FoEsMin);
            }
            foreach (EsCloud c in res)
            {
                c.New = double.IsNaN(c.FoEsOld) && !double.IsNaN(c.FoEs);
                if (double.IsNaN(c.FoEs)) { c.FoEs = c.FoEsOld; c.Trend = -1; }
                else if (c.New) c.Trend = c.Spots >= 2 ? 1 : 0;
                else c.Trend = c.FoEs > c.FoEsOld + 0.5 ? 1 : c.FoEs < c.FoEsOld - 0.5 ? -1 : 0;
            }
            if (giro)
                foreach (GiroStation st in Giro.Stations)
                {
                    DateTime when;
                    double fo = st.LastValue(r => r.FoEs, Giro.FreshMin, out when);
                    if (double.IsNaN(fo) || fo < 5) continue;   // sotto 5 MHz la nube non regge nemmeno i 28 MHz a 2000 km
                    EsCloud c = res.OrderBy(x => Geo.DistanceKmFast(x.Lat, x.Lon, st.Lat, st.Lon)).FirstOrDefault(x => Geo.DistanceKmFast(x.Lat, x.Lon, st.Lat, st.Lon) <= 400);
                    if (c == null) { c = new EsCloud { Id = res.Count + 1, Lat = st.Lat, Lon = st.Lon, RadiusKm = 200, Last = when }; res.Add(c); }
                    if (double.IsNaN(c.GiroFoEs) || fo > c.GiroFoEs) { c.GiroFoEs = fo; c.GiroName = st.Name; }
                }
            return res;
        }

        // ---------------------------------------------------------------- ipotesi dal QTH

        public static List<EsHypothesis> Hypotheses(double sLat, double sLon, double f, double H, List<EsCloud> clouds, List<Spot> tropoSpots, out int waiting)
        {
            waiting = 0;
            List<EsHypothesis> res = new List<EsHypothesis>();
            double hMax = MaxHopKm(H), hMin = MinHopKm(f);
            foreach (EsCloud c in clouds)
            {
                double fo = c.BestFoEs;
                if (fo <= 0) continue;
                double d = Geo.DistanceKm(sLat, sLon, c.Lat, c.Lon);
                double az = Geo.Bearing(sLat, sLon, c.Lat, c.Lon);
                double lo = Math.Max(hMin, 2 * (d - c.RadiusKm)), hi = Math.Min(hMax, 2 * (d + c.RadiusKm));
                bool geomOk = hi >= lo;
                bool mufOk = geomOk && fo * Geo.SecantFactor(hi, H) >= f;
                if (geomOk && mufOk)
                {
                    // salto piu' corto che regge la banda (meno perdite), dentro la nube
                    double hop = hi;
                    for (double x = lo; x <= hi; x += 25) if (fo * Geo.SecantFactor(x, H) >= f) { hop = x; break; }
                    EsHypothesis h = MakeSingle(sLat, sLon, az, hop, f, fo, c, H);
                    EsHypothesis dbl = DoubleHop(h, clouds, f, H);
                    FarEndTropo(h, f, H, tropoSpots, ref waiting);
                    res.Add(h);
                    if (dbl != null) res.Add(dbl);
                    continue;
                }
                // niente primo salto utile: tropo + Es con la stazione virtuale A'
                EsHypothesis tr = TropoEs(sLat, sLon, d, az, f, fo, c, H, tropoSpots, ref waiting);
                if (tr != null) res.Add(tr);
            }
            res.Sort((a, b) => a.Kind != b.Kind ? a.Kind.CompareTo(b.Kind) : b.Score.CompareTo(a.Score));
            return res;
        }

        static string Window(EsCloud c)
        {
            return c.Trend > 0 ? Lang.T("in formazione: possibile nei prossimi 15-30 minuti") : c.Trend < 0 ? Lang.T("in calo: la nube si sta chiudendo") : Lang.T("stabile: possibile adesso");
        }

        static double MufFactor(double muf, double f) { return Math.Max(0.2, Math.Min(1, 0.5 + (muf - f) / (0.2 * f))); }

        static EsHypothesis MakeSingle(double sLat, double sLon, double az, double hop, double f, double fo, EsCloud c, double H)
        {
            EsHypothesis h = new EsHypothesis { Kind = HypKind.Single, FMHz = f, Cloud = c, Az = az, HopKm = hop };
            h.ElevDeg = Geo.ElevationDeg(hop / 2, H, 0, EsModel.EsK);
            h.MufMHz = fo * Geo.SecantFactor(hop, H);
            Geo.Destination(sLat, sLon, az, hop, out h.LandLat, out h.LandLon);
            h.LandOnLand = Chains.HasOperators(h.LandLat, h.LandLon, 150);
            h.LossDb = Fsl(SlantKm(hop, H), f) + Gamma1(hop, f, fo);
            h.Score = c.Score * MufFactor(h.MufMHz, f);
            h.Window = Window(c);
            return h;
        }

        // doppio salto: un'altra nube lungo la traiettoria, oltre l'atterraggio del primo salto, che regge la banda
        static EsHypothesis DoubleHop(EsHypothesis h1, List<EsCloud> clouds, double f, double H)
        {
            double hMax = MaxHopKm(H), hMin = MinHopKm(f);
            EsHypothesis best = null;
            foreach (EsCloud c2 in clouds)
            {
                if (c2 == h1.Cloud) continue;
                double fo2 = c2.BestFoEs;
                double dl = Geo.DistanceKm(h1.LandLat, h1.LandLon, c2.Lat, c2.Lon);
                double az2 = Geo.Bearing(h1.LandLat, h1.LandLon, c2.Lat, c2.Lon);
                double off = Math.Abs(Geo.Norm180(az2 - Geo.Bearing(h1.LandLat, h1.LandLon, h1.Cloud.Lat, h1.Cloud.Lon) - 180));
                // la nube deve stare avanti, vicino al proseguimento della traiettoria
                double xt = dl * Math.Sin(off * Math.PI / 180);
                if (off > 90 || xt > LineKm + c2.RadiusKm) continue;
                double hop2 = 2 * dl * Math.Cos(off * Math.PI / 180);
                if (hop2 < hMin || hop2 > hMax) continue;
                double muf2 = fo2 * Geo.SecantFactor(hop2, H);
                if (muf2 < f) continue;
                double azOut = Geo.Bearing(h1.LandLat, h1.LandLon, c2.Lat, c2.Lon);
                EsHypothesis h = new EsHypothesis
                {
                    Kind = HypKind.Double, FMHz = f, Cloud = h1.Cloud, Cloud2 = c2, Az = h1.Az, HopKm = h1.HopKm, Hop2Km = hop2,
                    ElevDeg = h1.ElevDeg, MufMHz = h1.MufMHz, Muf2MHz = muf2, LandLat = h1.LandLat, LandLon = h1.LandLon, LandOnLand = h1.LandOnLand
                };
                Geo.Destination(h1.LandLat, h1.LandLon, azOut, hop2, out h.Land2Lat, out h.Land2Lon);
                h.Land2OnLand = Chains.HasOperators(h.Land2Lat, h.Land2Lon, 150);
                h.LossDb = Fsl(SlantKm(h1.HopKm, H) + SlantKm(hop2, H), f) + Gamma1(h1.HopKm, f, h1.Cloud.BestFoEs) + Gamma1(hop2, f, fo2);
                h.Score = 0.6 * Math.Min(h1.Cloud.Score, c2.Score) * Math.Min(MufFactor(h1.MufMHz, f), MufFactor(muf2, f));
                h.Window = Window(c2.Trend > h1.Cloud.Trend ? c2 : h1.Cloud);
                if (best == null || h.Score > best.Score) best = h;
            }
            return best;
        }

        static double CouplingDb(double elev, double crit)
        {
            if (crit <= 0) return 99;
            return elev <= crit ? 0 : Math.Min(40, 20 * Math.Log10(elev / crit));
        }

        // condotto fra due punti: osservato (spot <TR>) o previsto (Open-Meteo); null se manca
        static DuctInfo Duct(double la1, double lo1, double la2, double lo2, List<Spot> tr, ref int waiting, out string src)
        {
            src = "";
            Spot obs = TropoDuct.ObservedAlong(tr, la1, lo1, la2, lo2, 150);
            bool wait;
            DuctInfo d = TropoDuct.Along(la1, lo1, la2, lo2, out wait);
            if (wait) waiting++;
            if (obs != null)
            {
                src = Lang.F("spot tropo {0}-{1} ({2:F0} km)", obs.Spotter, obs.DxCall, obs.DistanceKm);
                // lo spot dice che il condotto c'e' e intrappola la banda; se il profilo manca, deficit tipico di 20 M (angolo 0,36°)
                if (d == null || !d.HasDuct) d = new DuctInfo { Ok = true, HasDuct = true, Deficit = 20, LambdaMax = 10, Source = src };
                return d;
            }
            if (d != null && d.HasDuct) src = d.Source;
            return d != null && d.HasDuct ? d : null;
        }

        // tropo + E-sporadico: A' in condotto dalla stazione, poi il salto Es da A' sulla nube
        static EsHypothesis TropoEs(double sLat, double sLon, double d, double az, double f, double fo, EsCloud c, double H, List<Spot> tr, ref int waiting)
        {
            double hMax = MaxHopKm(H), hMin = MinHopKm(f);
            EsHypothesis best = null;
            for (double t = TropoMinKm; t <= TropoMaxKm; t += TropoStepKm)
            {
                foreach (int dir in new[] { 1, -1 })   // verso la nube (nube troppo lontana) o all'indietro (nube troppo vicina)
                {
                    double hop = 2 * (d - dir * t);
                    if (hop < hMin || hop > hMax) continue;
                    double muf = fo * Geo.SecantFactor(hop, H);
                    if (muf < f) continue;
                    double vla, vlo;
                    Geo.Destination(sLat, sLon, dir > 0 ? az : Geo.Norm360(az + 180), t, out vla, out vlo);
                    string src;
                    DuctInfo du = Duct(sLat, sLon, vla, vlo, tr, ref waiting, out src);
                    if (du == null) continue;
                    double soft = du.Traps(f) ? 1 : du.Soft(f);
                    if (soft < 0.5) continue;
                    double elev = Geo.ElevationDeg(hop / 2, H, 0, EsModel.EsK);
                    double cpl = CouplingDb(elev, du.CriticalDeg);
                    if (cpl > CouplingMaxDb) continue;
                    EsHypothesis h = new EsHypothesis
                    {
                        Kind = HypKind.TropoEs, FMHz = f, Cloud = c, Az = Geo.Bearing(vla, vlo, c.Lat, c.Lon), HopKm = hop, ElevDeg = elev, MufMHz = muf,
                        VLat = vla, VLon = vlo, TropoKm = t, CriticalDeg = du.CriticalDeg, CouplingDb = cpl, Soft = soft, DuctSource = src
                    };
                    Geo.Destination(vla, vlo, h.Az, hop, out h.LandLat, out h.LandLon);
                    h.LandOnLand = Chains.HasOperators(h.LandLat, h.LandLon, 150);
                    h.LossDb = Fsl(SlantKm(hop, H) + t, f) + Gamma1(hop, f, fo) + (soft >= 1 ? DuctDbPerKm : SoftDbPerKm) * t + cpl;
                    h.Score = 0.4 * c.Score * MufFactor(muf, f) * soft * (1 - cpl / (CouplingMaxDb + 5));
                    h.Window = Window(c);
                    if (best == null || h.LossDb < best.LossDb) best = h;
                }
            }
            return best;
        }

        // dopo l'atterraggio del primo salto il segnale puo' continuare in condotto (es. sul mare, EI7GL [90]: Es fino alla costa
        // turca, poi condotto fino a Israele): si cerca il primo tratto di 100-800 km con condotto e accoppiamento accettabile
        static void FarEndTropo(EsHypothesis h, double f, double H, List<Spot> tr, ref int waiting)
        {
            double azOut = Geo.Bearing(h.LandLat, h.LandLon, h.Cloud.Lat, h.Cloud.Lon) + 180;
            for (double t = TropoMaxKm; t >= TropoMinKm; t -= 100)
            {
                double ela, elo;
                Geo.Destination(h.LandLat, h.LandLon, Geo.Norm360(azOut), t, out ela, out elo);
                string src;
                DuctInfo du = Duct(h.LandLat, h.LandLon, ela, elo, tr, ref waiting, out src);
                if (du == null) continue;
                double soft = du.Traps(f) ? 1 : du.Soft(f);
                if (soft < 0.5) continue;
                double cpl = CouplingDb(h.ElevDeg, du.CriticalDeg);
                if (cpl > CouplingMaxDb) continue;
                h.TropoAtFarEnd = true; h.ExtLat = ela; h.ExtLon = elo; h.ExtKm = t; h.CriticalDeg = du.CriticalDeg; h.CouplingDb = cpl; h.Soft = soft; h.DuctSource = src;
                return;
            }
        }

        // ---------------------------------------------------------------- REAL-TIME: spot in 144 MHz oltre il singolo salto

        // tratta 2 m fra 2400 e 3400 km (fuori dalla regola atlantica): se non c'e' un doppio salto con due nubi note e invece c'e'
        // una nube nota a meta' di un salto Es da una estremita' e un condotto dall'altra, la tratta e' TR+ES (nube ricollocata)
        public const double TrEsMinKm = 2400, TrEsMaxKm = 3400;
        public static EsModel.EsTropo ClassifyTrEs(Spot s, double H, List<EsCloud> clouds, List<Spot> tr)
        {
            if (s.Band != "2m" || !s.HasBoth || s.DistanceKm <= TrEsMinKm || s.DistanceKm > TrEsMaxKm) return null;
            double D = s.DistanceKm, hMax = MaxHopKm(H), f = s.FreqMHz;
            // doppio salto con due nubi note: ha la precedenza (fonte [88])
            List<EsCloud> on = clouds.Where(c => c.BestFoEs > 0 && OffTrackKm(s, c) <= LineKm).ToList();
            foreach (EsCloud a in on)
                foreach (EsCloud b in on)
                    if (a != b && Math.Abs(AlongKm(s, a) - AlongKm(s, b)) >= EsModel.MinKm144 / 2) return null;
            int waiting = 0;
            EsModel.EsTropo best = null; double bestLoss = double.MaxValue;
            for (int end = 0; end < 2; end++)
            {
                double xLa = end == 0 ? s.LatS : s.LatD, xLo = end == 0 ? s.LonS : s.LonD;   // estremita' del salto Es
                double yLa = end == 0 ? s.LatD : s.LatS, yLo = end == 0 ? s.LonD : s.LonS;   // estremita' del condotto
                for (double t = TropoMinKm; t <= TropoMaxKm; t += TropoStepKm)
                {
                    double hop = D - t;
                    if (hop > hMax || hop < EsModel.MinKm144) continue;
                    double vLa, vLo; Geo.IntermediateFast(xLa, xLo, yLa, yLo, hop / D, out vLa, out vLo);
                    double cLa, cLo; Geo.IntermediateFast(xLa, xLo, vLa, vLo, 0.5, out cLa, out cLo);
                    EsCloud c = clouds.FirstOrDefault(x => Geo.DistanceKmFast(x.Lat, x.Lon, cLa, cLo) <= EsModel.CloudMatchKm && x.BestFoEs * Geo.SecantFactor(hop, H) >= f * 0.9);
                    if (c == null) continue;
                    string src;
                    DuctInfo du = Duct(vLa, vLo, yLa, yLo, tr, ref waiting, out src);
                    if (du == null) continue;
                    double soft = du.Traps(f) ? 1 : du.Soft(f);
                    if (soft < 0.5) continue;
                    double cpl = CouplingDb(Geo.ElevationDeg(hop / 2, H, 0, EsModel.EsK), du.CriticalDeg);
                    if (cpl > CouplingMaxDb) continue;
                    double loss = Fsl(SlantKm(hop, H) + t, f) + Gamma1(hop, f, c.BestFoEs) + (soft >= 1 ? DuctDbPerKm : SoftDbPerKm) * t + cpl;
                    if (loss < bestLoss)
                    {
                        bestLoss = loss;
                        best = new EsModel.EsTropo { Spot = s, CloudLat = cLa, CloudLon = cLo, LandLat = vLa, LandLon = vLo, IsleLat = yLa, IsleLon = yLo, EsKm = hop, TropoKm = t,
                                                     EsLat = xLa, EsLon = xLo, General = true, LossDb = loss, DuctSource = src };
                    }
                }
            }
            return best;
        }
        // distanza lungo la tratta e fuori tratta di una nube (km); fuori tratta = MaxValue se la nube sta dietro o oltre la tratta
        static void Track(Spot s, EsCloud c, out double along, out double off)
        {
            double d13 = Geo.DistanceKmFast(s.LatS, s.LonS, c.Lat, c.Lon) / Geo.R;
            double dth = (Geo.BearingFast(s.LatS, s.LonS, c.Lat, c.Lon) - Geo.BearingFast(s.LatS, s.LonS, s.LatD, s.LonD)) * Math.PI / 180;
            double xt = Math.Asin(Math.Max(-1, Math.Min(1, Math.Sin(d13) * Math.Sin(dth))));
            along = Math.Acos(Math.Max(-1, Math.Min(1, Math.Cos(d13) / Math.Max(1e-9, Math.Cos(xt))))) * Geo.R;
            off = Math.Abs(xt) * Geo.R;
            if (Math.Cos(dth) <= 0 || along > s.DistanceKm) off = double.MaxValue;
        }
        static double AlongKm(Spot s, EsCloud c) { double a, o; Track(s, c, out a, out o); return a; }
        static double OffTrackKm(Spot s, EsCloud c) { double a, o; Track(s, c, out a, out o); return o; }

        public static string KindName(HypKind k)
        {
            return k == HypKind.Single ? Lang.T("primo salto utile") : k == HypKind.Double ? Lang.T("doppio salto Es") : Lang.T("tropo + Es");
        }
    }
}
