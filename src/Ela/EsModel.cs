// E-LAYER ANALYZER - modello dei salti di E-sporadico
// Numero di salti e giudizio sulla distanza: tabella di Flavio Egano IK3XTV (singolo, doppio, triplo salto).
// Punti di riflessione: salti uguali lungo il cerchio massimo; primo salto di ciascuna stazione a D/(2n) da essa.
// Ipotesi "primo salto corto" (esperienza dell'autore IC8TEM): nel multi-salto il primo salto di una stazione puo'
// essere corto, entro ~1200 km dal proprio QTH; il resto della tratta si divide in salti uguali.
// foEs minima: legge della secante sulla Terra sferica, foEs >= f / sec(phi) (phi = angolo di incidenza sullo strato).
using System;
using System.Collections.Generic;

namespace ELA
{
    public class EsHop { public double Lat, Lon; public int Index; }

    public class EsPath
    {
        public Spot Spot;
        public double DistKm;
        public int Hops;            // 0 = troppo corto per l'E-sporadico
        public double HopKm, SecPhi, FoEsMin, ElevDeg;
        public string Judgement = "";
        public List<EsHop> Points = new List<EsHop>();
        public EsHop ShortA, ShortB;     // ipotesi del primo salto corto (punto di riflessione) per spotter e DX
        public double ShortFoEsMin;      // foEs minima per il salto corto
        public EsHop FirstA { get { return Points.Count > 0 ? Points[0] : null; } }               // primo salto dello spotter
        public EsHop FirstB { get { return Points.Count > 0 ? Points[Points.Count - 1] : null; } } // primo salto del DX
    }

    public static class EsModel
    {
        // Tabella IK3XTV (km)
        public static readonly double[] SingleMin = { 500, 650 }, SingleOpt = { 1500, 2000 }, SingleMax = { 2200, 2400 };
        public static readonly double[] DoubleMin = { 2800, 3000 }, DoubleOpt = { 3200, 4000 }, DoubleMax = { 4400, 5000 };
        public static readonly double[] TripleOpt = { 4800, 6400 }; public const double TripleMax = 6900;

        public static int HopsFor(double d)
        {
            if (d < SingleMin[0]) return 0;
            if (d <= SingleMax[1]) return 1;
            if (d <= DoubleMax[1]) return 2;
            if (d <= TripleMax) return 3;
            return (int)Math.Ceiling(d / 2300.0);
        }

        public static string Judge(double d)
        {
            if (d < SingleMin[0]) return "sotto la distanza minima dell'Es (tropo o Es a MUF molto alta)";
            if (d < SingleMin[1]) return "singolo salto, vicino alla distanza minima";
            if (d < SingleOpt[0]) return "singolo salto";
            if (d <= SingleOpt[1]) return "singolo salto, distanza ottimale";
            if (d <= SingleMax[1]) return "singolo salto, vicino alla distanza massima";
            if (d < DoubleMin[0]) return "tra singolo e doppio salto (zona di passaggio)";
            if (d < DoubleOpt[0]) return "doppio salto";
            if (d <= DoubleOpt[1]) return "doppio salto, distanza ottimale";
            if (d <= DoubleMax[1]) return "doppio salto, vicino alla distanza massima";
            if (d < TripleOpt[0]) return "triplo salto";
            if (d <= TripleOpt[1]) return "triplo salto, distanza ottimale";
            if (d <= TripleMax) return "triplo salto, vicino alla distanza massima";
            return "oltre il triplo salto (ipotesi di salti multipli o Es + F2)";
        }

        public static EsPath Build(Spot s, double Hkm)
        {
            if (s == null || !s.HasBoth) return null;
            EsPath p = new EsPath { Spot = s };
            p.DistKm = s.DistanceKm;
            p.Hops = HopsFor(p.DistKm);
            p.Judgement = Judge(p.DistKm);
            int n = Math.Max(1, p.Hops);
            p.HopKm = p.DistKm / n;
            p.SecPhi = Geo.SecantFactor(p.HopKm, Hkm);
            p.FoEsMin = s.FreqMHz / p.SecPhi;
            p.ElevDeg = Geo.ElevationDeg(p.HopKm / 2, Hkm, 0);
            for (int k = 1; k <= n; k++)
            {
                double la, lo;
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, (2.0 * k - 1) / (2.0 * n), out la, out lo);
                p.Points.Add(new EsHop { Lat = la, Lon = lo, Index = k });
            }
            if (p.Hops >= 2 && ShortHopKm > 0 && ShortHopKm < p.HopKm)
            {
                double f = (ShortHopKm / 2) / p.DistKm, la, lo;
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, f, out la, out lo);
                p.ShortA = new EsHop { Lat = la, Lon = lo, Index = 1 };
                Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 1 - f, out la, out lo);
                p.ShortB = new EsHop { Lat = la, Lon = lo, Index = n };
                p.ShortFoEsMin = s.FreqMHz / Geo.SecantFactor(ShortHopKm, Hkm);
            }
            return p;
        }

        // ---- Regola dell'autore (IC8TEM) per i 144 MHz ----
        // 1) in 144 MHz la distanza minima dell'E-sporadico e' sopra i 900 km: gli spot piu' corti non sono Es;
        // 2) uno spot in 144 MHz da solo non si disegna;
        // 3) si disegna se entro 5 minuti arriva un secondo spot in 144 MHz (altra coppia di stazioni) con la nube
        //    nello stesso posto, oppure 4) se nei dintorni c'e' almeno uno spot in 50 MHz di salto corto.
        // Scelte del programma per "stesso posto", "dintorni" e "salto corto": punti di riflessione entro 300 km,
        // spot a 50 MHz in singolo salto entro ShortHopKm (1200 km) e entro 15 minuti dallo spot in 144 MHz.
        // valori di partenza = regola dell'autore; si regolano nei filtri di DATA COLLECTION (Filters.cs)
        public static double MinKm144 = 900, Confirm144Min = 5, CloudMatchKm = 300, Near50Min = 15, Short50Km = 1200;
        public static bool Use144Rule = true;
        public static double MinKm50 = 450;   // regola dell'autore: in 50 MHz entro 450 km non e' E-sporadico
        // 28 MHz: si segue IK3XTV (lo skip si accorcia al diminuire della frequenza): stessa nube (stessa foEs), quindi
        // la distanza minima dei 28 MHz e' quella in cui sec(phi) vale sec(phi a MinKm50) x 28,5 / 50,2 (legge della secante)
        public static double MinKm28 { get { return ScaledMinKm(MinKm50, 50.2, 28.5, 105); } }
        public static double ScaledMinKm(double km, double fFrom, double fTo, double h)
        {
            double target = Geo.SecantFactor(km, h) * fTo / fFrom;
            if (target <= 1.0) return 0;
            double lo = 0, hi = km;
            for (int i = 0; i < 40; i++) { double m = (lo + hi) / 2; if (Geo.SecantFactor(m, h) < target) lo = m; else hi = m; }
            return Math.Round((lo + hi) / 2 / 10) * 10;
        }

        static IEnumerable<EsHop> RefPoints(EsPath p)
        {
            foreach (EsHop h in p.Points) yield return h;
            if (p.ShortA != null) { yield return p.ShortA; yield return p.ShortB; }
        }

        // distanza minima tra i punti di riflessione di due tratte (km)
        public static double RefDistKm(EsPath a, EsPath b)
        {
            double best = double.MaxValue;
            foreach (EsHop x in RefPoints(a))
                foreach (EsHop y in RefPoints(b))
                    best = Math.Min(best, Geo.DistanceKm(x.Lat, x.Lon, y.Lat, y.Lon));
            return best;
        }

        static bool SamePair(Spot a, Spot b)
        {
            return (a.DxCall == b.DxCall && a.Spotter == b.Spotter) || (a.DxCall == b.Spotter && a.Spotter == b.DxCall);
        }

        // Applica la regola dei 144 MHz. paths: tratte da disegnare (tutte le bande); six: tratte a 50 MHz per la conferma.
        // Restituisce le tratte da disegnare; short = spot sotto i 900 km scartati, held = spot in attesa di conferma.
        public static List<EsPath> Apply144Rule(List<EsPath> paths, List<EsPath> six, out int shortCount, out int held)
        {
            shortCount = 0; held = 0;
            if (!Use144Rule) return new List<EsPath>(paths);   // regola spenta nei filtri
            List<EsPath> two = new List<EsPath>();
            foreach (EsPath p in paths) if (p.Spot.Band == "2m" && p.DistKm >= MinKm144) two.Add(p);
            List<EsPath> res = new List<EsPath>();
            foreach (EsPath p in paths)
            {
                if (p.Spot.Band != "2m") { res.Add(p); continue; }
                if (p.DistKm < MinKm144) { shortCount++; continue; }
                bool ok = false;
                foreach (EsPath q in two)
                {
                    if (q == p || SamePair(p.Spot, q.Spot)) continue;
                    if (Math.Abs((q.Spot.Utc - p.Spot.Utc).TotalMinutes) > Confirm144Min) continue;
                    if (RefDistKm(p, q) <= CloudMatchKm) { ok = true; break; }
                }
                if (!ok && six != null)
                    foreach (EsPath r in six)
                    {
                        if (r.Hops != 1 || r.DistKm > Short50Km) continue;
                        if (Math.Abs((r.Spot.Utc - p.Spot.Utc).TotalMinutes) > Near50Min) continue;
                        if (RefDistKm(p, r) <= CloudMatchKm) { ok = true; break; }
                    }
                if (ok) res.Add(p); else held++;
            }
            return res;
        }

        // ---- Regola dell'autore (IC8TEM): propagazione transequatoriale (TEP) ----
        // Per le tratte verso l'Africa e il Sud America, se il punto medio del collegamento cade nella fascia di
        // inclinazione magnetica tra -20° e +20° (equatore magnetico; carta dell'inclinazione WMM 2015 fornita
        // dall'autore, calcolo con IGRF-14), il collegamento non e' E-sporadico ma transequatoriale, salvo che lo
        // spot (o il PROP_MODE del log) dichiari esplicitamente ES.
        public static double TepInclinationDeg = 20;
        public static bool UseTepRule = true;

        // Africa (con il Madagascar; senza Canarie, Madeira, Capo Verde e penisola arabica) e Sud America: riquadri geografici approssimati
        public static bool InAzzorre(double lat, double lon) { return lat >= 36.8 && lat <= 39.9 && lon >= -31.5 && lon <= -24.7; }
        public static bool InAtlanticIslands(double lat, double lon) { return InCanarie(lat, lon) || InMadeira(lat, lon) || InAzzorre(lat, lon); }

        // Regola dell'autore (IC8TEM) per i 144 MHz verso Canarie, Madeira e Azzorre: oltre il singolo salto la tratta e' un
        // E-sporadico seguito da un condotto troposferico sull'Atlantico (come per le aperture VK-ZL in 144 MHz [41]).
        // Lo spot vale solo se due o piu' spot indicano la stessa nube Es; non si applica ai 50 e 70 MHz.
        public class EsTropo { public Spot Spot; public double CloudLat, CloudLon, LandLat, LandLon, IsleLat, IsleLon, EsKm, TropoKm; }

        public static bool IsAtlanticTropoCandidate(Spot s)
        {
            if (s.Band != "2m" || !s.HasBoth) return false;
            bool a = InAtlanticIslands(s.LatS, s.LonS), b = InAtlanticIslands(s.LatD, s.LonD);
            return a != b && s.DistanceKm > SingleMax[1];
        }

        // punti candidati della nube: punto medio di un salto Es di 1000-2300 km partito dalla stazione sul continente verso l'isola
        static List<double[]> CloudCandidates(Spot s, out double mLat, out double mLon, out double iLat, out double iLon)
        {
            bool sIsle = InAtlanticIslands(s.LatS, s.LonS);
            mLat = sIsle ? s.LatD : s.LatS; mLon = sIsle ? s.LonD : s.LonS;
            iLat = sIsle ? s.LatS : s.LatD; iLon = sIsle ? s.LonS : s.LonD;
            double brg = Geo.Bearing(mLat, mLon, iLat, iLon);
            List<double[]> l = new List<double[]>();
            for (double hop = 1000; hop <= 2300; hop += 100)
            {
                double la, lo; Geo.Destination(mLat, mLon, brg, hop / 2, out la, out lo);
                l.Add(new[] { la, lo, hop });
            }
            return l;
        }

        // conferma con due o piu' spot sulla stessa nube (entro CloudMatchKm e Confirm144Min)
        public static List<EsTropo> AtlanticTropo(List<Spot> cands, out int held)
        {
            List<EsTropo> res = new List<EsTropo>();
            held = 0;
            for (int i = 0; i < cands.Count; i++)
            {
                double mLa, mLo, iLa, iLo;
                List<double[]> ci = CloudCandidates(cands[i], out mLa, out mLo, out iLa, out iLo);
                double best = double.MaxValue; double[] bp = null;
                for (int j = 0; j < cands.Count; j++)
                {
                    if (j == i || Math.Abs((cands[i].Utc - cands[j].Utc).TotalMinutes) > Confirm144Min) continue;
                    if (cands[i].Spotter == cands[j].Spotter && cands[i].DxCall == cands[j].DxCall) continue;
                    double a, b, c, d;
                    List<double[]> cj = CloudCandidates(cands[j], out a, out b, out c, out d);
                    foreach (double[] p in ci) foreach (double[] q in cj)
                    {
                        double km = Geo.DistanceKm(p[0], p[1], q[0], q[1]);
                        if (km <= CloudMatchKm && km < best) { best = km; bp = p; }
                    }
                }
                if (bp == null) { held++; continue; }
                double brg = Geo.Bearing(mLa, mLo, iLa, iLo), lLa, lLo;
                Geo.Destination(mLa, mLo, brg, bp[2], out lLa, out lLo);
                res.Add(new EsTropo { Spot = cands[i], CloudLat = bp[0], CloudLon = bp[1], LandLat = lLa, LandLon = lLo, IsleLat = iLa, IsleLon = iLo,
                                      EsKm = bp[2], TropoKm = Geo.DistanceKm(lLa, lLo, iLa, iLo) });
            }
            return res;
        }

        public static bool InCanarie(double lat, double lon) { return lat >= 27.4 && lat <= 29.6 && lon >= -18.4 && lon <= -13.2; }
        public static bool InMadeira(double lat, double lon) { return lat >= 29.9 && lat <= 33.3 && lon >= -17.5 && lon <= -15.7; }   // Madeira e Selvagens

        public static bool InAfrica(double lat, double lon)
        {
            if (InCanarie(lat, lon) || InMadeira(lat, lon)) return false;   // Canarie e Madeira escluse (regola dell'autore); Capo Verde e' gia' fuori dai riquadri
            if (lat >= -35 && lat <= 37.5 && lon >= -18.5 && lon <= 11) return true;
            if (lat >= -35 && lat <= 32 && lon > 11 && lon <= 35) return !(lon > 34 && lat > 27.5);   // fino al Sinai, senza la costa arabica del golfo di Aqaba
            if (lat >= -27 && lat <= 12.5 && lon > 35 && lon <= 52) return !(lat > 11.8 && lon > 43.3);   // Corno d'Africa, Africa orientale, Madagascar; penisola arabica esclusa (Aden)
            return false;
        }

        public static bool InSouthAmerica(double lat, double lon) { return lat >= -56 && lat <= 13 && lon >= -82 && lon <= -34; }

        // vero se la tratta va verso l'Africa o il Sud America e il punto medio sta nella fascia |inclinazione| <= 20°
        public static bool IsTransEquatorial(double latA, double lonA, double latB, double lonB)
        {
            if (!UseTepRule) return false;
            bool south = InAfrica(latA, lonA) || InAfrica(latB, lonB) || InSouthAmerica(latA, lonA) || InSouthAmerica(latB, lonB);
            if (!south) return false;
            double la, lo;
            Geo.Intermediate(latA, lonA, latB, lonB, 0.5, out la, out lo);
            return Math.Abs(Geo.InclinationDeg(la, lo)) <= TepInclinationDeg;
        }

        // Regola generale per tutto il mondo (Kennewell e Wilkinson, IPS [49]): il TEP richiede un percorso che attraversa
        // l'equatore magnetico in modo quasi simmetrico, entro circa 15 gradi dalla direzione nord-sud magnetica, su 3000-8000 km.
        // Restano fuori le zone escluse dall'autore (Canarie, Madeira, Capo Verde, penisola arabica).
        public static double TepMinKm = 3000, TepMaxKm = 8000, TepMeridianDeg = 15;
        static bool InCapoVerde(double lat, double lon) { return lat >= 14.5 && lat <= 17.5 && lon >= -25.7 && lon <= -22.3; }
        static bool InArabia(double lat, double lon) { return lat >= 12 && lat <= 32.5 && lon >= 34.5 && lon <= 60; }
        static bool TepExcluded(double lat, double lon) { return InCanarie(lat, lon) || InMadeira(lat, lon) || InCapoVerde(lat, lon) || InArabia(lat, lon); }

        public static bool IsTransEquatorialWorld(double latA, double lonA, double latB, double lonB)
        {
            if (!UseTepRule) return false;
            if (TepExcluded(latA, lonA) || TepExcluded(latB, lonB)) return false;
            double d = Geo.DistanceKm(latA, lonA, latB, lonB);
            if (d < TepMinKm || d > TepMaxKm) return false;
            double incA = Geo.InclinationDeg(latA, lonA), incB = Geo.InclinationDeg(latB, lonB);
            if (Math.Sign(incA) == Math.Sign(incB)) return false;                 // non attraversa l'equatore magnetico
            double la, lo;
            Geo.Intermediate(latA, lonA, latB, lonB, 0.5, out la, out lo);
            if (Math.Abs(Geo.InclinationDeg(la, lo)) > TepInclinationDeg) return false;   // punto medio lontano dall'equatore magnetico
            double inc, dec;
            Igrf.IncDec(la, lo, out inc, out dec);
            double brg = Geo.Bearing(la, lo, latB, lonB);
            double dev = Math.Abs(((brg - dec) % 180 + 270) % 180 - 90);          // scarto dal meridiano magnetico (0-90 gradi)
            return dev <= TepMeridianDeg;
        }

        // lunghezza del primo salto corto (km); 0 = ipotesi spenta
        public static double ShortHopKm = 1200;

        // MUF della propria stazione attraverso una nube di foEs nota posta a distanza dKm (salto di lunghezza 2d)
        public static double MufVia(double foEs, double dKm, double Hkm) { return foEs * Geo.SecantFactor(2 * dKm, Hkm); }
    }
}
