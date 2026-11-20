// E-LAYER ANALYZER - catene del multi-salto in 50 MHz con i salti corti alle estremita' (proposta dell'autore, IC8TEM,
// approvata il 20/11/2026). Le tre condizioni dell'autore per l'E-sporadico multi-salto:
//   1) primo salto corto (500-1200 km) con MUF intorno ai 50 MHz;
//   2) ultimo salto corto (500-1200 km) con MUF intorno ai 50 MHz;
//   3) salti centrali (fino a circa 2200 km), che chiedono molta meno foEs (circa 9,3 MHz contro 11-21 MHz dei salti corti,
//      legge della secante del programma a 110 km di quota).
// "Nube forte": nube che regge un salto corto in 50 MHz; viene dagli spot Es (50, 70, 144 MHz e anche 28 MHz molto corti)
// e dalle ionosonde GIRO con foEs recente. Tre tipi di catena:
//   - DAL QTH: primo salto corto dalla propria stazione su una nube forte A, ultimo salto corto su una nube forte B lungo
//     lo stesso cerchio massimo (anche nello stesso continente, per esempio in Kazakhstan), zona d'arrivo oltre B;
//   - CORRIDOIO: due nubi forti A e B in qualunque parte del mondo; le due zone d'estremita' sono le
//     stazioni che possono collegarsi (per esempio Caraibi - Europa); se la propria stazione e' in una zona d'estremita' o vicina,
//     la riga lo dice;
//   - CERCA LA NUBE: c'e' solo il primo salto corto; si mostrano le zone dove dovrebbe stare la nube del salto corto finale,
//     con la ionosonda GIRO piu' vicina.
// Direzioni: a differenza dei precursori a regioni, le catene valgono in tutte le direzioni, anche nord-sud (decisione
// dell'autore del 20/11/2026: le aperture Europa - Niger della spedizione 5U5K, giugno 2024, sono doppio e triplo salto);
// resta l'esclusione del TEP.
// Livelli: ALLERTA = salti centrali tutti coperti da nubi; PRECURSORE = mancano solo punti centrali senza osservatori
// (oceano, zone senza stazioni negli spot); le catene con un punto centrale osservato e senza nube non si mostrano.
// Fonti: tabella IK3XTV [3]; esperienza IC8TEM del primo salto corto [4]; DF5AI (UKSMG 1999) sul modo a salto corto e i suoi
// multipli; EI7GL (UN7MBH, 70 MHz) per il Kazakhstan.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELA
{
    // nube forte (raggruppata entro ClusterKm)
    public class StrongCloud
    {
        public double Lat, Lon, FoEs;
        public int N;
        public DateTime Newest = DateTime.MinValue;
        public double Age = double.NaN;            // minuti dallo spot piu' recente all'istante del calcolo
        public string Source = "";                 // nominativi degli spot o codice della ionosonda GIRO
        public bool Ionosonde;
        public Spot Spot;
        public double AgeMin { get { return Age; } }
    }

    public class ChainAlert : PrecursorAlert
    {
        public const int KindQth = 1, KindCorridor = 2, KindSearch = 3;
        public int Kind;
        public StrongCloud A, B;
        public double XA, XB;                      // distanza al suolo fra la stazione d'estremita' e la nube (meta' del salto corto)
        public double EndALat, EndALon, EndBLat, EndBLon;   // estremita' (QTH: A = propria stazione, B = zona d'arrivo)
        public string EndAName = "", EndBName = "";
        public double NearKm = double.NaN;         // CORRIDOIO: distanza della propria stazione dalla zona d'estremita' piu' vicina
        public int Middle;                         // numero dei salti centrali
        public double MiddleKm;                    // lunghezza di ogni salto centrale
        public List<PrecursorPoint> Zones = new List<PrecursorPoint>();   // CERCA LA NUBE: zone del salto corto finale
        public override string LevelText { get { return Level == 2 ? YAMS.Lang.T("ALLERTA") : Level == 1 ? YAMS.Lang.T("PRECURSORE") : YAMS.Lang.T("CERCA LA NUBE"); } }
        public override string Key { get { return "C" + Kind + "|" + Math.Round(A.Lat, 1) + "|" + Math.Round(A.Lon, 1) + "|" + (B != null ? Math.Round(B.Lat, 1) + "|" + Math.Round(B.Lon, 1) : ""); } }
        public string KindText
        {
            get
            {
                if (Kind == KindQth) return YAMS.Lang.T("dal QTH");
                if (Kind == KindSearch) return YAMS.Lang.T("cerca la nube");
                if (!double.IsNaN(NearKm) && NearKm <= Chains.NearMeKm) return YAMS.Lang.T("corridoio, ti riguarda");
                return YAMS.Lang.T("corridoio");
            }
        }
    }

    public static class Chains
    {
        public const double F50 = 50.2;
        public const double ShortMinKm = 500, ShortMaxKm = 1200;           // salto corto (IC8TEM, DF5AI: picco intorno a 950 km)
        public const double MiddleMaxKm = 2200, MiddleMinKm = 500;          // salti centrali: ottimale-massimo del singolo salto (IK3XTV)
        public const double ClusterKm = 250, LineKm = 300, NearMeKm = 1200;
        public const double CorridorMinKm = 1000, CorridorMaxKm = 9000, MaxPathKm = 13000;
        public static bool Enabled = true, Corridors = true, Search = true;
        public static int MaxCorridors = 40;

        // --- avviso (IMPOSTAZIONI > Generali): ALLERTA nuova di una catena che riguarda la propria stazione (dal QTH, oppure un
        //     corridoio con un'estremita' entro 1200 km); la stessa catena non si ripete prima di 60 minuti
        public static event Action<ChainAlert> NewMine;
        static readonly Dictionary<string, DateTime> told = new Dictionary<string, DateTime>();
        public static bool Mine(ChainAlert c) { return c.Kind == ChainAlert.KindQth || (c.Kind == ChainAlert.KindCorridor && !double.IsNaN(c.NearKm) && c.NearKm <= NearMeKm); }
        public static void Notify(IEnumerable<PrecursorAlert> list)
        {
            DateTime now = DateTime.UtcNow;
            foreach (PrecursorAlert a in list)
            {
                ChainAlert c = a as ChainAlert;
                if (c == null || c.Level != 2 || !Mine(c)) continue;
                string k = c.Kind + "|" + c.Region;
                DateTime t;
                if (told.TryGetValue(k, out t) && (now - t).TotalMinutes < 60) continue;
                told[k] = now;
                if (NewMine != null) NewMine(c);
            }
        }

        // foEs minima per un salto corto in 50 MHz alla lunghezza massima (1200 km): sotto, la nube non e' "forte"
        public static double StrongFoEs(double Hkm) { return F50 / Geo.SecantFactor(ShortMaxKm, Hkm); }

        // meta' salto corto piu' lunga (250-600 km) che la nube regge in 50 MHz, oppure NaN
        public static double ShortHalf(double foEs, double Hkm)
        {
            for (double x = ShortMaxKm / 2; x >= ShortMinKm / 2 - 0.1; x -= 25)
                if (foEs * Geo.SecantFactor(2 * x, Hkm) >= F50) return x;
            return double.NaN;
        }

        // distanza dal cerchio massimo (km) e distanza lungo il cerchio massimo (km, negativa se dietro) del punto P
        public static void Track(double lat0, double lon0, double az, double lat, double lon, out double cross, out double along)
        {
            double d13 = Geo.DistanceKm(lat0, lon0, lat, lon) / Geo.R;
            double t13 = Geo.Bearing(lat0, lon0, lat, lon) * Math.PI / 180, t12 = az * Math.PI / 180;
            double xt = Math.Asin(Math.Max(-1, Math.Min(1, Math.Sin(d13) * Math.Sin(t13 - t12))));
            double at = Math.Acos(Math.Max(-1, Math.Min(1, Math.Cos(d13) / Math.Max(1e-9, Math.Cos(xt)))));
            cross = Math.Abs(xt) * Geo.R;
            along = at * Geo.R * (Math.Cos(t13 - t12) >= 0 ? 1 : -1);
        }


        // nome della zona: regione piu' vicina entro 1500 km, altrimenti il campo Maidenhead
        public static string ZoneName(double lat, double lon)
        {
            string best = null; double bd = 1500;
            foreach (var r in Precursors.Regions) { double d = Geo.DistanceKm(lat, lon, r.Lat, r.Lon); if (d < bd) { bd = d; best = r.Name; } }
            return best != null ? YAMS.Lang.T(best) + " (" + Geo.LatLonToLoc(lat, lon, 4) + ")" : Geo.LatLonToLoc(lat, lon, 4);
        }

        // ------------------------------------------------------------ nubi forti
        // spot Es utili alle catene (stesse esclusioni della linguetta REAL-TIME); isolated = regola degli FT8/FT4 isolati in 50 MHz
        static bool Usable(Spot x, Func<Spot, bool> isolated)
        {
            return x.HasBoth && x.EsCandidate && !x.IsTep && !isolated(x) && !EsModel.IsAtlanticTropoCandidate(x);
        }

        public static List<StrongCloud> Strong(double Hkm, IEnumerable<Spot> spots, DateTime now, bool giro, Func<Spot, bool> isolated)
        {
            double need = StrongFoEs(Hkm);
            List<StrongCloud> raw = new List<StrongCloud>();
            foreach (Spot s in spots)
            {
                if (!Usable(s, isolated)) continue;
                double d = s.DistanceKm;
                if (d < 400 || d > Precursors.MaxHopKm) continue;
                if (s.Band == "2m" && d < EsModel.MinKm144) continue;
                double foEs = s.FreqMHz / Geo.SecantFactor(d, Hkm);
                if (foEs < need) continue;
                double la, lo;
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                raw.Add(new StrongCloud { Lat = la, Lon = lo, FoEs = foEs, N = 1, Newest = s.Utc, Source = s.Spotter + "-" + s.DxCall, Spot = s });
            }
            // ionosonde GIRO con foEs recente sufficiente (solo in diretta)
            if (giro) foreach (GiroStation g in Giro.Stations)
            {
                DateTime when;
                double f = g.LastValue(r => r.FoEs, Giro.FreshMin, out when);
                if (double.IsNaN(f) || f < need) continue;
                raw.Add(new StrongCloud { Lat = g.Lat, Lon = g.Lon, FoEs = f, N = 1, Newest = when, Source = "GIRO " + g.Code, Ionosonde = true });
            }
            // raggruppamento: la nube piu' forte tiene la posizione, le altre entro ClusterKm si sommano
            raw.Sort((a, b) => b.FoEs.CompareTo(a.FoEs));
            List<StrongCloud> res = new List<StrongCloud>();
            foreach (StrongCloud c in raw)
            {
                StrongCloud m = res.FirstOrDefault(x => Geo.DistanceKm(x.Lat, x.Lon, c.Lat, c.Lon) <= ClusterKm);
                if (m == null) { res.Add(c); continue; }
                m.N++;
                if (c.Newest > m.Newest) m.Newest = c.Newest;
                if (m.Source.Length < 60 && !m.Source.Contains(c.Source)) m.Source += ", " + c.Source;
                if (c.Ionosonde) m.Ionosonde = true;
            }
            foreach (StrongCloud c in res) c.Age = (now - c.Newest).TotalMinutes;
            return res;
        }

        // ------------------------------------------------------------ catene
        class Act { public double Lat, Lon, FoEs; public Spot Spot; }

        // in diretta: spot degli ultimi windowMin minuti, ionosonde GIRO, propria stazione
        public static List<ChainAlert> Compute(double Hkm, double windowMin, out List<StrongCloud> strong)
        {
            strong = new List<StrongCloud>();
            if (!Enabled) return new List<ChainAlert>();
            List<Spot> win = SpotStore.Recent(windowMin, x => x.HasBoth).ToList();
            List<Spot> hour = SpotStore.Recent(60, x => x.HasBoth).ToList();
            return Core(Hkm, win, hour, DateTime.UtcNow, Station.Valid, Station.Lat, Station.Lon, true, Corridors, Search, x => EsModel.Ft8Isolated50(x), out strong);
        }

        // calcolo vero e proprio, anche su dati del passato (BENCHMARK 50 MHz): win = spot della finestra, hour = spot dell'ultima ora
        public static List<ChainAlert> Core(double Hkm, List<Spot> win, List<Spot> hour, DateTime now, bool hasStation, double sLat, double sLon,
                                            bool giro, bool corridors, bool search, Func<Spot, bool> isolated, out List<StrongCloud> strong)
        {
            List<ChainAlert> res = new List<ChainAlert>();
            strong = Strong(Hkm, win, now, giro, isolated);
            if (strong.Count == 0) return res;
            // tutte le nubi Es (anche deboli) per i salti centrali, e le stazioni presenti negli spot (zone osservate)
            List<Act> act = new List<Act>();
            foreach (Spot s in win)
            {
                if (!(s.HasBoth && s.EsCandidate && !s.IsTep && !isolated(s))) continue;
                double d = s.DistanceKm;
                if (d > Precursors.MaxHopKm || d < 400) continue;
                double la, lo;
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                act.Add(new Act { Lat = la, Lon = lo, FoEs = s.FreqMHz / Geo.SecantFactor(d, Hkm), Spot = s });
            }
            foreach (StrongCloud c in strong) if (c.Ionosonde) act.Add(new Act { Lat = c.Lat, Lon = c.Lon, FoEs = c.FoEs });
            List<double[]> stations = new List<double[]>();
            foreach (Spot s in hour) { stations.Add(new[] { s.LatS, s.LonS }); stations.Add(new[] { s.LatD, s.LonD }); }

            if (hasStation) res.AddRange(FromQth(sLat, sLon, Hkm, strong, act, stations, search, giro));
            if (corridors) res.AddRange(Corridor(Hkm, strong, act, stations, res, hasStation, sLat, sLon));
            return res;
        }

        // salti centrali fra due punti a distanza along1..along2 lungo il cerchio massimo (lat0, lon0, az); null = catena smentita
        static List<PrecursorPoint> MiddlePoints(double lat0, double lon0, double az, double g1, double g2, double Hkm, List<Act> act, List<double[]> stations, out int k, out double hop)
        {
            List<PrecursorPoint> pts = new List<PrecursorPoint>();
            double lm = g2 - g1;
            k = 0; hop = 0;
            if (lm <= MiddleMinKm) return pts;                        // i due salti corti sono vicini: niente salti centrali
            k = (int)Math.Ceiling(lm / MiddleMaxKm);
            hop = lm / k;
            for (int j = 0; j < k; j++)
            {
                double la, lo;
                Geo.Destination(lat0, lon0, az, g1 + (j + 0.5) * hop, out la, out lo);
                PrecursorPoint p = new PrecursorPoint { Lat = la, Lon = lo, HopKm = hop, FoEsNeed = F50 / Geo.SecantFactor(hop, Hkm) };
                foreach (Act c in act)
                {
                    double d = Geo.DistanceKm(la, lo, c.Lat, c.Lon);
                    if (d > Precursors.MatchKm || c.FoEs < p.FoEsNeed) continue;
                    if (!p.Covered || c.FoEs > p.CloudFoEs) { p.Covered = true; p.CloudFoEs = c.FoEs; p.CloudKm = d; p.CloudSpot = c.Spot; }
                }
                foreach (double[] s in stations) if (Geo.DistanceKm(la, lo, s[0], s[1]) <= Precursors.ObservedKm) { p.Observed = true; break; }
                pts.Add(p);
            }
            return pts;
        }

        static int LevelOf(List<PrecursorPoint> mid)
        {
            if (mid.All(p => p.Covered)) return 2;
            if (mid.Where(p => !p.Covered).All(p => !p.Observed)) return 1;
            return 0;
        }

        // primo salto corto dalla propria stazione (A), ultimo salto corto su B lungo lo stesso cerchio massimo
        static List<ChainAlert> FromQth(double sLat, double sLon, double Hkm, List<StrongCloud> strong, List<Act> act, List<double[]> stations, bool search, bool giro)
        {
            List<ChainAlert> res = new List<ChainAlert>();
            foreach (StrongCloud a in strong)
            {
                double dA = Geo.DistanceKm(sLat, sLon, a.Lat, a.Lon);
                if (dA < ShortMinKm / 2 || dA > ShortMaxKm / 2) continue;
                if (a.FoEs * Geo.SecantFactor(2 * dA, Hkm) < F50) continue;               // condizione 1
                double az = Geo.Bearing(sLat, sLon, a.Lat, a.Lon);   // anche nord-sud (Europa - Africa occidentale, decisione dell'autore 20/11/2026)
                ChainAlert best = null;
                foreach (StrongCloud b in strong)
                {
                    if (b == a) continue;
                    double cross, along;
                    Track(sLat, sLon, az, b.Lat, b.Lon, out cross, out along);
                    if (cross > LineKm || along < 2 * dA + ShortMinKm / 2) continue;
                    double xb = ShortHalf(b.FoEs, Hkm);                                    // condizione 2
                    if (double.IsNaN(xb)) continue;
                    double D = along + xb;
                    if (D > MaxPathKm || D < Precursors.MinKm) continue;
                    double tLat, tLon;
                    Geo.Destination(sLat, sLon, az, D, out tLat, out tLon);
                    if (EsModel.IsTransEquatorialWorld(sLat, sLon, tLat, tLon)) continue;
                    int k; double hop;
                    List<PrecursorPoint> mid = MiddlePoints(sLat, sLon, az, 2 * dA, along - xb, Hkm, act, stations, out k, out hop);   // condizione 3
                    int lev = LevelOf(mid);
                    if (lev == 0) continue;
                    ChainAlert c = new ChainAlert
                    {
                        Kind = ChainAlert.KindQth, A = a, B = b, XA = dA, XB = xb, Level = lev, Band = "6 m", FreqMHz = F50,
                        EndALat = sLat, EndALon = sLon, EndBLat = tLat, EndBLon = tLon, TargetLat = tLat, TargetLon = tLon,
                        DistKm = D, Az = az, ShortHyp = true, ShortFound = true, ShortKm = 2 * dA, Middle = k, MiddleKm = hop, Points = mid
                    };
                    c.EndBName = ZoneName(tLat, tLon); c.Region = c.EndBName;
                    if (best == null || c.Level > best.Level || (c.Level == best.Level && c.DistKm > best.DistKm)) best = c;
                }
                if (best != null) { res.Add(best); continue; }
                if (!search) continue;
                // CERCA LA NUBE: zone del salto corto finale dopo 1, 2 o 3 salti centrali
                ChainAlert s = new ChainAlert
                {
                    Kind = ChainAlert.KindSearch, A = a, XA = dA, Level = 0, Band = "6 m", FreqMHz = F50, EndALat = sLat, EndALon = sLon,
                    Az = az, ShortHyp = true, ShortFound = true, ShortKm = 2 * dA
                };
                for (int j = 1; j <= 3; j++)
                {
                    double along = 2 * dA + j * MiddleMaxKm + (ShortMinKm + ShortMaxKm) / 4;
                    if (along + ShortMaxKm / 2 > MaxPathKm) break;
                    double la, lo;
                    Geo.Destination(sLat, sLon, az, along, out la, out lo);
                    PrecursorPoint z = new PrecursorPoint { Lat = la, Lon = lo, HopKm = j, FoEsNeed = StrongFoEs(Hkm) };
                    foreach (double[] st in stations) if (Geo.DistanceKm(la, lo, st[0], st[1]) <= Precursors.ObservedKm) { z.Observed = true; break; }
                    // ionosonda GIRO piu' vicina (entro 1000 km): foEs recente
                    GiroStation gb = null; double gd = 1000;
                    if (giro) foreach (GiroStation g in Giro.Stations) { double d = Geo.DistanceKm(la, lo, g.Lat, g.Lon); if (d < gd) { gd = d; gb = g; } }
                    if (gb != null)
                    {
                        DateTime when;
                        z.CloudFoEs = gb.LastValue(r => r.FoEs, Giro.FreshMin, out when);
                        z.CloudKm = gd;
                    }
                    if (j == 1) { s.TargetLat = la; s.TargetLon = lo; s.DistKm = along + ShortMaxKm / 2; }
                    s.Zones.Add(z);
                }
                if (s.Zones.Count == 0) continue;
                s.Region = ZoneName(s.Zones[0].Lat, s.Zones[0].Lon);
                double eLa, eLo;
                Geo.Destination(sLat, sLon, az, s.DistKm, out eLa, out eLo);
                if (EsModel.IsTransEquatorialWorld(sLat, sLon, eLa, eLo)) continue;
                res.Add(s);
            }
            return res;
        }

        // CORRIDOIO: due nubi forti qualunque, in ogni direzione (TEP escluso)
        static List<ChainAlert> Corridor(double Hkm, List<StrongCloud> strong, List<Act> act, List<double[]> stations, List<ChainAlert> fromQth, bool hasStation, double sLat, double sLon)
        {
            List<ChainAlert> res = new List<ChainAlert>();
            for (int i = 0; i < strong.Count; i++)
                for (int j = i + 1; j < strong.Count; j++)
                {
                    StrongCloud a = strong[i], b = strong[j];
                    double dAB = Geo.DistanceKm(a.Lat, a.Lon, b.Lat, b.Lon);
                    if (dAB < CorridorMinKm || dAB > CorridorMaxKm) continue;
                    double az = Geo.Bearing(a.Lat, a.Lon, b.Lat, b.Lon);
                    double xa = ShortHalf(a.FoEs, Hkm), xb = ShortHalf(b.FoEs, Hkm);
                    if (double.IsNaN(xa) || double.IsNaN(xb)) continue;
                    double eaLat, eaLon, ebLat, ebLon;
                    Geo.Destination(a.Lat, a.Lon, Geo.Norm360(az + 180), xa, out eaLat, out eaLon);
                    Geo.Destination(b.Lat, b.Lon, Geo.Norm360(Geo.Bearing(b.Lat, b.Lon, a.Lat, a.Lon) + 180), xb, out ebLat, out ebLon);
                    if (EsModel.IsTransEquatorialWorld(eaLat, eaLon, ebLat, ebLon)) continue;
                    int k; double hop;
                    List<PrecursorPoint> mid = MiddlePoints(a.Lat, a.Lon, az, xa, dAB - xb, Hkm, act, stations, out k, out hop);
                    int lev = LevelOf(mid);
                    if (lev == 0) continue;
                    ChainAlert c = new ChainAlert
                    {
                        Kind = ChainAlert.KindCorridor, A = a, B = b, XA = xa, XB = xb, Level = lev, Band = "6 m", FreqMHz = F50,
                        EndALat = eaLat, EndALon = eaLon, EndBLat = ebLat, EndBLon = ebLon, TargetLat = ebLat, TargetLon = ebLon,
                        DistKm = xa + dAB + xb, Az = az, Middle = k, MiddleKm = hop, Points = mid
                    };
                    c.EndAName = ZoneName(eaLat, eaLon); c.EndBName = ZoneName(ebLat, ebLon);
                    c.Region = c.EndAName + " ↔ " + c.EndBName;
                    if (hasStation)
                    {
                        c.NearKm = Math.Min(Geo.DistanceKm(sLat, sLon, eaLat, eaLon), Geo.DistanceKm(sLat, sLon, ebLat, ebLon));
                        // la propria stazione e' gia' l'estremita' di una catena DAL QTH con la stessa coppia di nubi: niente doppione
                        if (fromQth.Any(q => (q.A == a && q.B == b) || (q.A == b && q.B == a))) continue;
                    }
                    res.Add(c);
                }
            // prima le catene che riguardano la propria stazione, poi le ALLERTE, poi le nubi piu' recenti
            return res.OrderBy(c => !double.IsNaN(c.NearKm) && c.NearKm <= NearMeKm ? 0 : 1).ThenByDescending(c => c.Level)
                      .ThenBy(c => Math.Max(c.A.AgeMin, c.B.AgeMin)).Take(MaxCorridors).ToList();
        }
    }
}
