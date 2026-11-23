// E-LAYER ANALYZER - ALLERTA 144 MHz dagli spot APRS (dal 22/11/2026, richiesta dell'autore): un salto radio APRS in 2 m oltre
// i 900 km e' un indizio di E-sporadico in 144 MHz, ma da solo non basta (un pacchetto e' brevissimo: puo' essere un aereo, un
// meteor scatter o un errore, come in LIVETROPOVHF). Conta la RICORRENZA A BREVE TEMPO: almeno due salti, in pacchetti diversi,
// con il punto medio entro 150 km e a non piu' di 10 minuti l'uno dall'altro. Allora:
//  - i salti entrano fra gli spot (fonte APRS-IS, 144,800 MHz) e si disegnano nella linguetta SPORADIC-E;
//  - nasce un'ALLERTA 144 MHz APRS nei Precursori multi-salto e la linguetta SPORADIC-E lampeggia.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELA
{
    public class AprsAlert : PrecursorAlert
    {
        public double MidLat, MidLon;
        public int N;
        public string Calls = "";
        public DateTime Last;
        public double NearKm = double.NaN;
        public override string LevelText { get { return YAMS.Lang.T("ALLERTA"); } }
        public override string Key { get { return "A|" + Math.Round(MidLat, 1) + "|" + Math.Round(MidLon, 1); } }
    }

    public static class AprsEs
    {
        public const double MinKm = 900, MaxKm = 2400, PairKm = 150, RepeatMin = 10, KeepMin = 30, RangeKm = 3000;
        public static bool Enabled = true;
        static AprsIs aprs;
        class Hop { public DateTime T; public string A, B; public double LatA, LonA, LatB, LonB, MidLat, MidLon; public bool Used; }
        static readonly List<Hop> hops = new List<Hop>();
        static readonly object lk = new object();
        public static long Packets { get { return aprs != null ? aprs.Packets : 0; } }
        public static bool Connected { get { return aprs != null && aprs.Connected; } }
        public static event Action<Spot> NewSpot;   // dal thread di APRS-IS: chi ascolta lo porta sul thread dell'interfaccia

        public static void Start()
        {
            Stop();
            if (!Enabled || !Station.Valid) return;
            aprs = new AprsIs
            {
                Call = Station.Call.Length > 0 ? Station.Call : "N0CALL",
                Filter = string.Format(System.Globalization.CultureInfo.InvariantCulture, "r/{0:F1}/{1:F1}/{2:F0}", Station.Lat, Station.Lon, RangeKm),
                MinKm = MinKm, MaxKm = MaxKm
            };
            aprs.Hop += OnHop;
            aprs.Start();
        }

        public static void Stop() { try { if (aprs != null) { aprs.Hop -= OnHop; aprs.Stop(); } } catch { } aprs = null; }

        static void OnHop(DateTime t, string band, string a, AprsPos pa, string b, AprsPos pb)
        {
            if (band != "144") return;
            double la, lo; Geo.IntermediateFast(pa.Lat, pa.Lon, pb.Lat, pb.Lon, 0.5, out la, out lo);
            Hop h = new Hop { T = t, A = a, B = b, LatA = pa.Lat, LonA = pa.Lon, LatB = pb.Lat, LonB = pb.Lon, MidLat = la, MidLon = lo };
            List<Hop> confirm = new List<Hop>();
            lock (lk)
            {
                hops.RemoveAll(x => (t - x.T).TotalMinutes > KeepMin);
                foreach (Hop x in hops)
                    if ((t - x.T).TotalMinutes <= RepeatMin && Geo.DistanceKmFast(x.MidLat, x.MidLon, la, lo) <= PairKm) confirm.Add(x);
                hops.Add(h);
                if (confirm.Count == 0) return;
                confirm.Add(h);
            }
            Action<Spot> ev = NewSpot;
            if (ev == null) return;
            foreach (Hop x in confirm)
            {
                if (x.Used) continue;
                x.Used = true;
                ev(new Spot
                {
                    Utc = x.T, FreqKHz = 144800, DxCall = x.A, Spotter = x.B, Source = 4, Prop = "ES", DigiMode = "APRS",
                    LocDx = Geo.LatLonToLoc(x.LatA, x.LonA, 6), LocSpotter = Geo.LatLonToLoc(x.LatB, x.LonB, 6),
                    LatD = x.LatA, LonD = x.LonA, LatS = x.LatB, LonS = x.LonB, Comment = "APRS 144.800 <ES>"
                });
            }
        }

        // allerte dagli spot APRS confermati degli ultimi 30 minuti (raggruppati per punto medio entro 250 km)
        public static List<AprsAlert> Compute()
        {
            List<AprsAlert> res = new List<AprsAlert>();
            foreach (Spot s in SpotStore.Recent(KeepMin, x => x.Source == 4 && x.HasBoth))
            {
                double la, lo; Geo.IntermediateFast(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                AprsAlert m = res.FirstOrDefault(x => Geo.DistanceKmFast(x.MidLat, x.MidLon, la, lo) <= 250);
                if (m != null)
                {
                    m.N++; if (s.Utc > m.Last) m.Last = s.Utc;
                    if (m.Calls.Length < 50 && !m.Calls.Contains(s.DxCall)) m.Calls += ", " + s.DxCall + "-" + s.Spotter;
                    continue;
                }
                AprsAlert a = new AprsAlert
                {
                    MidLat = la, MidLon = lo, N = 1, Last = s.Utc, Calls = s.DxCall + "-" + s.Spotter, Level = 2, Band = "2 m", FreqMHz = 144.8,
                    TargetLat = la, TargetLon = lo, DistKm = s.DistanceKm, Region = Chains.ZoneName(la, lo)
                };
                if (Station.Valid) { a.NearKm = Geo.DistanceKmFast(Station.Lat, Station.Lon, la, lo); a.Az = Geo.BearingFast(Station.Lat, Station.Lon, la, lo); }
                res.Add(a);
            }
            return res.OrderBy(a => double.IsNaN(a.NearKm) ? 1e9 : a.NearKm).ToList();
        }
    }
}
