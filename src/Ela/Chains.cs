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
        public const int KindQth = 1, KindCorridor = 2, KindSearch = 3, KindEqual = 4, KindEqualCorridor = 5;   // 4 = doppio salto a salti uguali (ITU-R P.534-6) dal QTH, 5 = fra due nubi qualunque
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
                if (Kind == KindEqual) return YAMS.Lang.T("doppio salto uguale");
                if (Kind == KindEqualCorridor) return !double.IsNaN(NearKm) && NearKm <= Chains.NearMeKm ? YAMS.Lang.T("corridoio a salti uguali, ti riguarda") : YAMS.Lang.T("corridoio a salti uguali");
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
        public static bool Mine(ChainAlert c) { return c.Kind == ChainAlert.KindQth || c.Kind == ChainAlert.KindEqual || ((c.Kind == ChainAlert.KindCorridor || c.Kind == ChainAlert.KindEqualCorridor) && !double.IsNaN(c.NearKm) && c.NearKm <= NearMeKm); }
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
            double d13 = Geo.DistanceKmFast(lat0, lon0, lat, lon) / Geo.R;
            double t13 = Geo.BearingFast(lat0, lon0, lat, lon) * Math.PI / 180, t12 = az * Math.PI / 180;
            double xt = Math.Asin(Math.Max(-1, Math.Min(1, Math.Sin(d13) * Math.Sin(t13 - t12))));
            double at = Math.Acos(Math.Max(-1, Math.Min(1, Math.Cos(d13) / Math.Max(1e-9, Math.Cos(xt)))));
            cross = Math.Abs(xt) * Geo.R;
            along = at * Geo.R * (Math.Cos(t13 - t12) >= 0 ? 1 : -1);
        }


        // solo la regione (piu' vicina entro 1500 km), altrimenti il campo Maidenhead di 2 caratteri
        public static string RegionName(double lat, double lon)
        {
            string best = null; double bd = 1500;
            foreach (var r in Precursors.Regions) { double d = Geo.DistanceKmFast(lat, lon, r.Lat, r.Lon); if (d < bd) { bd = d; best = r.Name; } }
            return best != null ? YAMS.Lang.T(best) : Geo.LatLonToLoc(lat, lon, 4).Substring(0, 2);
        }

        // nome della zona: regione piu' vicina entro 1500 km, altrimenti il campo Maidenhead
        public static string ZoneName(double lat, double lon)
        {
            string best = null; double bd = 1500;
            foreach (var r in Precursors.Regions) { double d = Geo.DistanceKmFast(lat, lon, r.Lat, r.Lon); if (d < bd) { bd = d; best = r.Name; } }
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
                Geo.IntermediateFast(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
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
                StrongCloud m = res.FirstOrDefault(x => Geo.DistanceKmFast(x.Lat, x.Lon, c.Lat, c.Lon) <= ClusterKm);
                if (m == null) { res.Add(c); continue; }
                m.N++;
                if (c.Newest > m.Newest) m.Newest = c.Newest;
                if (m.Source.Length < 60 && !m.Source.Contains(c.Source)) m.Source += ", " + c.Source;
                if (c.Ionosonde) m.Ionosonde = true;
            }
            foreach (StrongCloud c in res) c.Age = (now - c.Newest).TotalMinutes;
            return res;
        }

        // ------------------------------------------------------------ dove c'e' chi ascolta
        // quadrati Maidenhead di 4 caratteri con almeno una stazione nota (CALL3 e stazioni degli spot): un segnale che atterra
        // lontano da tutti (mare aperto, deserti, ghiacci) non serve a nessuno e non si segnala (decisione dell'autore, IC8TEM,
        // 20/11/2026: «il segnale nostro del multisalto, se cade a mare, è inutile segnalarlo»)
        public const double OperatorsKm = 300;
        static bool[] opsSq;
        static int opsFrom = -1;
        static volatile bool opsAny;
        static readonly object opsLk = new object();
        static int Sq(double lat, double lon)
        {
            double l = ((lon + 180) % 360 + 360) % 360;
            int x = Math.Min(179, Math.Max(0, (int)Math.Floor(l / 2))), y = Math.Min(179, Math.Max(0, (int)Math.Floor(lat + 90)));
            return y * 180 + x;
        }
        static bool[] Ops()
        {
            string[] locs = YAMS.CallDb.Locators;
            lock (opsLk)
            {
                if (opsSq != null && opsFrom == locs.Length) return opsSq;
                bool[] m = opsSq != null ? (bool[])opsSq.Clone() : new bool[180 * 180];
                foreach (string l in locs)
                {
                    if (l == null || l.Length < 4) continue;
                    string q = l.Substring(0, 4).ToUpperInvariant();
                    if (q == "RR73" || q == "AA00" || q == "JJ00" || q == "RR99") continue;   // saluto finale o locator di comodo
                    double la, lo;
                    if (Geo.LocToLatLon(q, out la, out lo)) m[Sq(la, lo)] = true;
                }
                opsSq = m; opsFrom = locs.Length; opsAny = m.Any(x => x);
                return m;
            }
        }
        // stazione vista negli spot: il suo quadrato conta come abitato
        public static void AddOperator(double lat, double lon)
        {
            if (double.IsNaN(lat) || double.IsNaN(lon)) return;
            bool[] m = Ops();
            lock (opsLk) m[Sq(lat, lon)] = true;
            opsAny = true;
        }
        // c'e' almeno un quadrato con stazioni entro km dal punto? (senza CALL3 caricato non si scarta niente)
        public static bool HasOperators(double lat, double lon, double km)
        {
            bool[] m = Ops();
            if (!opsAny) return true;
            double dLat = km / 111.0, dLon = Math.Min(180, km / (111.0 * Math.Max(0.05, Math.Cos(lat * Math.PI / 180))));
            for (double y = Math.Floor(lat - dLat); y <= lat + dLat; y += 1)
            {
                if (y < -90 || y >= 90) continue;
                for (double x = Math.Floor((lon - dLon) / 2) * 2; x <= lon + dLon; x += 2)
                {
                    if (!m[Sq(y + 0.5, x + 1)]) continue;
                    if (Geo.DistanceKmFast(lat, lon, y + 0.5, x + 1) <= km + 60) return true;   // 60 km: meta' quadrato, la stazione puo' stare sul bordo
                }
            }
            return false;
        }

        // ------------------------------------------------------------ proiezione del proprio segnale
        // dove atterra il segnale in 50 MHz della propria stazione dopo 2-5 salti, sopra le nubi stimate degli ultimi 30 minuti
        // (spot di tutte le bande, 28-144 MHz, convertiti in foEs: il fenomeno e' uno solo, proposta dell'autore del 20/11/2026):
        // primo salto corto (500-1200 km), poi salti di 500-2200 km, ognuno su una nube che lo regge; ogni 2° di azimut.
        // Prova sulle decodifiche JTDX dell'autore (409 aperture multi-salto, estati 2024-2026): nei 30 minuti prima dell'apertura
        // la proiezione andava verso la stazione lontana (entro 10°) nel 49% dei casi contro il 20% degli stessi orari senza apertura.
        public class Landing { public double Lat, Lon, Az, DistKm; public int Hops; public bool Operators; }
        public static List<Landing> LastProjection = new List<Landing>();
        public const double ProjStepDeg = 2;
        public const int ProjMaxHops = 5;

        static List<Landing> Project(double sLat, double sLon, double Hkm, List<Act> act)
        {
            List<Landing> res = new List<Landing>();
            int n = act.Count;
            if (n == 0) return res;
            double[] d13 = new double[n], th13 = new double[n], fo = new double[n];
            for (int i = 0; i < n; i++)
            {
                d13[i] = Geo.DistanceKmFast(sLat, sLon, act[i].Lat, act[i].Lon) / Geo.R;
                th13[i] = Geo.BearingFast(sLat, sLon, act[i].Lat, act[i].Lon) * Math.PI / 180;
                fo[i] = act[i].FoEs;
            }
            List<double[]> on = new List<double[]>();
            for (double az = 0; az < 360; az += ProjStepDeg)
            {
                double t12 = az * Math.PI / 180;
                on.Clear();
                for (int i = 0; i < n; i++)
                {
                    double xt = Math.Asin(Math.Max(-1, Math.Min(1, Math.Sin(d13[i]) * Math.Sin(th13[i] - t12))));
                    if (Math.Abs(xt) * Geo.R > LineKm || Math.Cos(th13[i] - t12) <= 0) continue;
                    double at = Math.Acos(Math.Max(-1, Math.Min(1, Math.Cos(d13[i]) / Math.Max(1e-9, Math.Cos(xt))))) * Geo.R;
                    on.Add(new[] { at, fo[i] });
                }
                if (on.Count == 0) continue;
                HashSet<int> from = new HashSet<int> { 0 }, seen = new HashSet<int>();
                // doppio salto a salti uguali (fino a circa 4700 km): nube a un quarto e a tre quarti della tratta
                double hMax = EqualMaxHopKm(Hkm);
                foreach (double[] c1 in on)
                {
                    double h = 2 * c1[0];
                    if (h < Precursors.MinKm / 2 || h > hMax || c1[1] * Geo.SecantFactor(h, Hkm) < F50) continue;
                    if (!on.Any(c2 => Math.Abs(c2[0] - 3 * c1[0]) <= LineKm / 2 && c2[1] * Geo.SecantFactor(h, Hkm) >= F50)) continue;
                    int L2 = (int)(Math.Round(4 * c1[0] / 100.0) * 100);
                    if (!seen.Add(L2)) continue;
                    double la, lo;
                    Geo.DestinationFast(sLat, sLon, az, L2, out la, out lo);
                    if (EsModel.IsTransEquatorialWorld(sLat, sLon, la, lo)) continue;
                    res.Add(new Landing { Lat = la, Lon = lo, Az = az, DistKm = L2, Hops = 2, Operators = HasOperators(la, lo, OperatorsKm) });
                }
                for (int k = 1; k <= ProjMaxHops && from.Count > 0; k++)
                {
                    HashSet<int> next = new HashSet<int>();
                    foreach (int L in from)
                        foreach (double[] c in on)
                        {
                            double half = c[0] - L;
                            if (half < ShortMinKm / 2 || half > (k == 1 ? ShortMaxKm / 2 : MiddleMaxKm / 2)) continue;
                            if (c[1] * Geo.SecantFactor(2 * half, Hkm) < F50) continue;
                            int L2 = (int)(Math.Round((2 * c[0] - L) / 100.0) * 100);
                            if (L2 <= MaxPathKm) next.Add(L2);
                        }
                    if (k >= 2)
                        foreach (int L2 in next)
                        {
                            if (!seen.Add(L2)) continue;
                            double la, lo;
                            Geo.DestinationFast(sLat, sLon, az, L2, out la, out lo);
                            if (EsModel.IsTransEquatorialWorld(sLat, sLon, la, lo)) continue;
                            res.Add(new Landing { Lat = la, Lon = lo, Az = az, DistKm = L2, Hops = k, Operators = HasOperators(la, lo, OperatorsKm) });
                        }
                    from = next;
                }
            }
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
            foreach (Spot s in hour) { AddOperator(s.LatS, s.LonS); AddOperator(s.LatD, s.LonD); }
            strong = Strong(Hkm, win, now, giro, isolated);
            // tutte le nubi Es (anche deboli) per i salti centrali, e le stazioni presenti negli spot (zone osservate)
            List<Act> act = new List<Act>();
            foreach (Spot s in win)
            {
                if (!(s.HasBoth && s.EsCandidate && !s.IsTep && !isolated(s))) continue;
                double d = s.DistanceKm;
                if (d > Precursors.MaxHopKm || d < 400) continue;
                double la, lo;
                Geo.IntermediateFast(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                act.Add(new Act { Lat = la, Lon = lo, FoEs = s.FreqMHz / Geo.SecantFactor(d, Hkm), Spot = s });
            }
            foreach (StrongCloud c in strong) if (c.Ionosonde) act.Add(new Act { Lat = c.Lat, Lon = c.Lon, FoEs = c.FoEs });
            List<double[]> stations = new List<double[]>();
            foreach (Spot s in hour) { stations.Add(new[] { s.LatS, s.LonS }); stations.Add(new[] { s.LatD, s.LonD }); }

            if (giro) LastProjection = hasStation ? Project(sLat, sLon, Hkm, act) : new List<Landing>();   // solo in diretta
            if (hasStation) res.AddRange(EqualDouble(sLat, sLon, Hkm, act, stations, now));   // non chiede nubi forti
            if (corridors) res.AddRange(EqualCorridor(Hkm, act, stations, hasStation, sLat, sLon, now));
            if (strong.Count == 0) return res;
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
                Geo.DestinationFast(lat0, lon0, az, g1 + (j + 0.5) * hop, out la, out lo);
                PrecursorPoint p = new PrecursorPoint { Lat = la, Lon = lo, HopKm = hop, FoEsNeed = F50 / Geo.SecantFactor(hop, Hkm) };
                foreach (Act c in act)
                {
                    double d = Geo.DistanceKmFast(la, lo, c.Lat, c.Lon);
                    if (d > Precursors.MatchKm || c.FoEs < p.FoEsNeed) continue;
                    if (!p.Covered || c.FoEs > p.CloudFoEs) { p.Covered = true; p.CloudFoEs = c.FoEs; p.CloudKm = d; p.CloudSpot = c.Spot; }
                }
                foreach (double[] s in stations) if (Geo.DistanceKmFast(la, lo, s[0], s[1]) <= Precursors.ObservedKm) { p.Observed = true; break; }
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

        // punto di riflessione lungo il cerchio massimo (lat0, lon0, az) a distanza along, per un salto di hopKm
        static PrecursorPoint PointAt(double lat0, double lon0, double az, double along, double hopKm, double Hkm, List<Act> act, List<double[]> stations)
        {
            double la, lo;
            Geo.DestinationFast(lat0, lon0, az, along, out la, out lo);
            PrecursorPoint p = new PrecursorPoint { Lat = la, Lon = lo, HopKm = hopKm, FoEsNeed = F50 / Geo.SecantFactor(hopKm, Hkm) };
            foreach (Act c in act)
            {
                double d = Geo.DistanceKmFast(la, lo, c.Lat, c.Lon);
                if (d > Precursors.MatchKm || c.FoEs < p.FoEsNeed) continue;
                if (!p.Covered || c.FoEs > p.CloudFoEs) { p.Covered = true; p.CloudFoEs = c.FoEs; p.CloudKm = d; p.CloudSpot = c.Spot; }
            }
            foreach (double[] s in stations) if (Geo.DistanceKmFast(la, lo, s[0], s[1]) <= Precursors.ObservedKm) { p.Observed = true; break; }
            return p;
        }

        // ------------------------------------------------------------ doppio salto a salti uguali (dal 22/11/2026)
        // Geometria di ITU-R P.534-6 [76]: due salti uguali, punti di riflessione a un quarto e tre quarti della tratta. Per
        // l'autore (IC8TEM, 22/11/2026) e' l'occorrenza tipica fino a circa 4700 km (per esempio Europa - Kazakhstan); oltre,
        // verso gli altri continenti, valgono le catene con i salti corti alle estremita'. Prova sulle decodifiche JTDX
        // dell'autore (aperture di 3100-4800 km): nei 30 minuti prima una nube a un quarto della tratta c'era nel 74% dei casi
        // contro il 44% degli stessi orari senza apertura, meglio del solo primo salto corto (82% contro 61%).
        // 4700 km = due salti da 2350 km, il salto piu' lungo possibile con la nube a 110 km (elevazione 0°): valore scelto il
        // 22/11/2026 su richiesta dell'autore al posto dei suoi «circa 5100 km», che chiederebbero nubi oltre i 120 km
        public const double EqualMaxKm = 4700;
        public static double EqualMaxHopKm(double Hkm) { return Math.Min(EqualMaxKm / 2, 2 * Geo.R * Math.Acos(Geo.R / (Geo.R + Hkm))); }

        static List<ChainAlert> EqualDouble(double sLat, double sLon, double Hkm, List<Act> act, List<double[]> stations, DateTime now)
        {
            Dictionary<int, ChainAlert> best = new Dictionary<int, ChainAlert>();
            double hMax = EqualMaxHopKm(Hkm), hMin = Precursors.MinKm / 2;
            foreach (Act a in act)
            {
                double x = Geo.DistanceKmFast(sLat, sLon, a.Lat, a.Lon);          // meta' del salto
                if (2 * x < hMin || 2 * x > hMax) continue;
                if (a.FoEs * Geo.SecantFactor(2 * x, Hkm) < F50) continue;      // la nube regge il primo salto
                double az = Geo.BearingFast(sLat, sLon, a.Lat, a.Lon), D = 4 * x, tLat, tLon;
                Geo.DestinationFast(sLat, sLon, az, D, out tLat, out tLon);
                if (EsModel.IsTransEquatorialWorld(sLat, sLon, tLat, tLon)) continue;
                if (!HasOperators(tLat, tLon, LineKm)) continue;
                PrecursorPoint far = PointAt(sLat, sLon, az, 3 * x, 2 * x, Hkm, act, stations);
                int lev = far.Covered ? 2 : !far.Observed ? 1 : 0;
                if (lev == 0) continue;
                StrongCloud A = new StrongCloud { Lat = a.Lat, Lon = a.Lon, FoEs = a.FoEs, N = 1, Spot = a.Spot, Source = a.Spot != null ? a.Spot.Spotter + " - " + a.Spot.DxCall : "GIRO" };
                if (a.Spot != null) { A.Newest = a.Spot.Utc; A.Age = (now - a.Spot.Utc).TotalMinutes; }
                StrongCloud B = null;
                if (far.Covered) B = new StrongCloud { Lat = far.Lat, Lon = far.Lon, FoEs = far.CloudFoEs, N = 1, Spot = far.CloudSpot, Source = far.CloudSpot != null ? far.CloudSpot.Spotter + " - " + far.CloudSpot.DxCall : "GIRO" };
                ChainAlert c = new ChainAlert
                {
                    Kind = ChainAlert.KindEqual, A = A, B = B, XA = x, XB = x, Level = lev, Band = "6 m", FreqMHz = F50,
                    EndALat = sLat, EndALon = sLon, EndBLat = tLat, EndBLon = tLon, TargetLat = tLat, TargetLon = tLon,
                    DistKm = D, Az = az, Middle = 0, MiddleKm = 2 * x, Points = new List<PrecursorPoint> { far }
                };
                c.EndBName = ZoneName(tLat, tLon); c.Region = c.EndBName;
                int sec = (int)(az / 10);                                       // una sola per settore di 10°: la migliore, poi la piu' lunga
                ChainAlert o;
                if (!best.TryGetValue(sec, out o) || c.Level > o.Level || (c.Level == o.Level && c.DistKm > o.DistKm)) best[sec] = c;
            }
            return best.Values.OrderByDescending(c => c.Level).ThenByDescending(c => c.DistKm).ToList();
        }

        // CORRIDOIO A SALTI UGUALI (dal 22/11/2026, richiesta dell'autore): due nubi qualunque a distanza h fra loro (h = un
        // salto, fino a EqualMaxHopKm) che reggono entrambe un salto di h in 50 MHz sono i punti a un quarto e tre quarti di una
        // tratta di 2h (ITU-R P.534-6 [76]); le estremita' stanno a h/2 prima della prima nube e dopo la seconda. Solo ALLERTE
        // (le due nubi ci sono), dove ci sono stazioni, TEP escluso; tratta di almeno Precursors.MinKm (sotto non e' multi-salto).
        // Verifica sulle decodifiche di 5U5K (Niger, giugno 2024, uso interno): stazioni europee attive collegate o sentite il
        // 10% a 3000-4000 km, il 4% a 4000-4500 km, l'1% a 4500-5500 km: il calo oltre i 4500 km conferma il limite di 4700 km.
        public const int MaxEqualCorridors = 20;
        static List<ChainAlert> EqualCorridor(double Hkm, List<Act> act, List<double[]> stations, bool hasStation, double sLat, double sLon, DateTime now)
        {
            double hMax = EqualMaxHopKm(Hkm), hMin = Precursors.MinKm / 2;
            // una nube per zona di 150 km: la piu' forte
            List<Act> reps = new List<Act>();
            foreach (Act a in act.OrderByDescending(x => x.FoEs))
            {
                if (reps.Count >= 250) break;
                if (a.FoEs * Geo.SecantFactor(hMin, Hkm) < F50) break;              // ordinate per foEs: le altre non reggono nemmeno il salto minimo
                if (reps.Any(r => Geo.DistanceKmFast(r.Lat, r.Lon, a.Lat, a.Lon) <= 150)) continue;
                reps.Add(a);
            }
            Dictionary<string, ChainAlert> best = new Dictionary<string, ChainAlert>();
            for (int i = 0; i < reps.Count; i++)
                for (int j = i + 1; j < reps.Count; j++)
                {
                    Act a = reps[i], b = reps[j];
                    double h = Geo.DistanceKmFast(a.Lat, a.Lon, b.Lat, b.Lon);
                    if (h < hMin || h > hMax) continue;
                    double sec = Geo.SecantFactor(h, Hkm);
                    if (a.FoEs * sec < F50 || b.FoEs * sec < F50) continue;
                    double az = Geo.BearingFast(a.Lat, a.Lon, b.Lat, b.Lon), eaLat, eaLon, ebLat, ebLon;
                    Geo.DestinationFast(a.Lat, a.Lon, Geo.Norm360(az + 180), h / 2, out eaLat, out eaLon);
                    Geo.DestinationFast(b.Lat, b.Lon, Geo.Norm360(Geo.BearingFast(b.Lat, b.Lon, a.Lat, a.Lon) + 180), h / 2, out ebLat, out ebLon);
                    if (EsModel.IsTransEquatorialWorld(eaLat, eaLon, ebLat, ebLon)) continue;
                    if (!HasOperators(eaLat, eaLon, LineKm) || !HasOperators(ebLat, ebLon, LineKm)) continue;
                    double near = double.NaN;
                    if (hasStation)
                    {
                        near = Math.Min(Geo.DistanceKmFast(sLat, sLon, eaLat, eaLon), Geo.DistanceKmFast(sLat, sLon, ebLat, ebLon));
                        if (near <= LineKm) continue;                                   // e' il doppio salto uguale dal QTH, gia' cercato
                    }
                    PrecursorPoint pb = new PrecursorPoint { Lat = b.Lat, Lon = b.Lon, HopKm = h, FoEsNeed = F50 / sec, Covered = true, CloudFoEs = b.FoEs, CloudKm = 0, CloudSpot = b.Spot, Observed = true };
                    StrongCloud A = new StrongCloud { Lat = a.Lat, Lon = a.Lon, FoEs = a.FoEs, N = 1, Spot = a.Spot, Source = a.Spot != null ? a.Spot.Spotter + " - " + a.Spot.DxCall : "GIRO" };
                    StrongCloud B = new StrongCloud { Lat = b.Lat, Lon = b.Lon, FoEs = b.FoEs, N = 1, Spot = b.Spot, Source = b.Spot != null ? b.Spot.Spotter + " - " + b.Spot.DxCall : "GIRO" };
                    if (a.Spot != null) { A.Newest = a.Spot.Utc; A.Age = (now - a.Spot.Utc).TotalMinutes; }
                    if (b.Spot != null) { B.Newest = b.Spot.Utc; B.Age = (now - b.Spot.Utc).TotalMinutes; }
                    ChainAlert c = new ChainAlert
                    {
                        Kind = ChainAlert.KindEqualCorridor, A = A, B = B, XA = h / 2, XB = h / 2, Level = 2, Band = "6 m", FreqMHz = F50,
                        EndALat = eaLat, EndALon = eaLon, EndBLat = ebLat, EndBLon = ebLon, TargetLat = ebLat, TargetLon = ebLon,
                        DistKm = 2 * h, Az = az, Middle = 0, MiddleKm = h, Points = new List<PrecursorPoint> { pb }, NearKm = near
                    };
                    c.EndAName = ZoneName(eaLat, eaLon); c.EndBName = ZoneName(ebLat, ebLon);
                    c.Region = c.EndAName + " ↔ " + c.EndBName;
                    // una sola per coppia di zone d'estremita' (celle di 3°): la piu' lunga
                    string k1 = Math.Round(eaLat / 3) + "," + Math.Round(eaLon / 3), k2 = Math.Round(ebLat / 3) + "," + Math.Round(ebLon / 3);
                    string key = string.CompareOrdinal(k1, k2) < 0 ? k1 + "|" + k2 : k2 + "|" + k1;
                    ChainAlert o;
                    if (!best.TryGetValue(key, out o) || c.DistKm > o.DistKm) best[key] = c;
                }
            return best.Values.OrderBy(c => !double.IsNaN(c.NearKm) && c.NearKm <= NearMeKm ? 0 : 1)
                       .ThenBy(c => Math.Max(double.IsNaN(c.A.AgeMin) ? 0 : c.A.AgeMin, double.IsNaN(c.B.AgeMin) ? 0 : c.B.AgeMin))
                       .Take(MaxEqualCorridors).ToList();
        }

        // primo salto corto dalla propria stazione (A), ultimo salto corto su B lungo lo stesso cerchio massimo
        static List<ChainAlert> FromQth(double sLat, double sLon, double Hkm, List<StrongCloud> strong, List<Act> act, List<double[]> stations, bool search, bool giro)
        {
            List<ChainAlert> res = new List<ChainAlert>();
            foreach (StrongCloud a in strong)
            {
                double dA = Geo.DistanceKmFast(sLat, sLon, a.Lat, a.Lon);
                if (dA < ShortMinKm / 2 || dA > ShortMaxKm / 2) continue;
                if (a.FoEs * Geo.SecantFactor(2 * dA, Hkm) < F50) continue;               // condizione 1
                double az = Geo.BearingFast(sLat, sLon, a.Lat, a.Lon);   // anche nord-sud (Europa - Africa occidentale, decisione dell'autore 20/11/2026)
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
                    Geo.DestinationFast(sLat, sLon, az, D, out tLat, out tLon);
                    if (EsModel.IsTransEquatorialWorld(sLat, sLon, tLat, tLon)) continue;
                    if (!HasOperators(tLat, tLon, LineKm)) continue;                       // atterra dove non c'e' nessuno
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
                    Geo.DestinationFast(sLat, sLon, az, along, out la, out lo);
                    if (!HasOperators(la, lo, LineKm)) continue;                           // zona in mare aperto: inutile cercarci la nube
                    PrecursorPoint z = new PrecursorPoint { Lat = la, Lon = lo, HopKm = j, FoEsNeed = StrongFoEs(Hkm) };
                    foreach (double[] st in stations) if (Geo.DistanceKmFast(la, lo, st[0], st[1]) <= Precursors.ObservedKm) { z.Observed = true; break; }
                    // ionosonda GIRO piu' vicina (entro 1000 km): foEs recente
                    GiroStation gb = null; double gd = 1000;
                    if (giro) foreach (GiroStation g in Giro.Stations) { double d = Geo.DistanceKmFast(la, lo, g.Lat, g.Lon); if (d < gd) { gd = d; gb = g; } }
                    if (gb != null)
                    {
                        DateTime when;
                        z.CloudFoEs = gb.LastValue(r => r.FoEs, Giro.FreshMin, out when);
                        z.CloudKm = gd;
                    }
                    if (s.Zones.Count == 0) { s.TargetLat = la; s.TargetLon = lo; s.DistKm = along + ShortMaxKm / 2; }
                    s.Zones.Add(z);
                }
                if (s.Zones.Count == 0) continue;
                s.Region = ZoneName(s.Zones[0].Lat, s.Zones[0].Lon);
                double eLa, eLo;
                Geo.DestinationFast(sLat, sLon, az, s.DistKm, out eLa, out eLo);
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
                    double dAB = Geo.DistanceKmFast(a.Lat, a.Lon, b.Lat, b.Lon);
                    if (dAB < CorridorMinKm || dAB > CorridorMaxKm) continue;
                    double az = Geo.BearingFast(a.Lat, a.Lon, b.Lat, b.Lon);
                    double xa = ShortHalf(a.FoEs, Hkm), xb = ShortHalf(b.FoEs, Hkm);
                    if (double.IsNaN(xa) || double.IsNaN(xb)) continue;
                    if (xa + dAB + xb < Precursors.MinKm) continue;                          // sotto i 2800 km non e' multi-salto (IK3XTV)
                    double eaLat, eaLon, ebLat, ebLon;
                    Geo.DestinationFast(a.Lat, a.Lon, Geo.Norm360(az + 180), xa, out eaLat, out eaLon);
                    Geo.DestinationFast(b.Lat, b.Lon, Geo.Norm360(Geo.BearingFast(b.Lat, b.Lon, a.Lat, a.Lon) + 180), xb, out ebLat, out ebLon);
                    if (EsModel.IsTransEquatorialWorld(eaLat, eaLon, ebLat, ebLon)) continue;
                    if (!HasOperators(eaLat, eaLon, LineKm) || !HasOperators(ebLat, ebLon, LineKm)) continue;   // un'estremita' in mare aperto
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
                        c.NearKm = Math.Min(Geo.DistanceKmFast(sLat, sLon, eaLat, eaLon), Geo.DistanceKmFast(sLat, sLon, ebLat, ebLon));
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
