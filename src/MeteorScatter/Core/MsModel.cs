// YA-MS - geometria del meteor scatter, Hot A/B, attenuazione, efficienza dello sciame
//
// Ipotesi di base (articolo Cerrotta-Sartori, vers. 7):
//  - lo sciame provoca scie ionizzate nello strato E; la quota tipica e' quella piu' frequente misurata
//    dal radar SKiYMET (circa 90 km);
//  - Terra sferica di raggio 6372.7955 km;
//  - Hot A / Hot B = incroci delle curve dove l'orizzonte (o il lobo elevato) di ciascuna stazione
//    incontra lo strato E.
// Efficienza: modello sperimentale a riflessione speculare (scia parallela alla direzione del radiante,
// tangente all'ellissoide con fuochi nelle due stazioni), pesata con la perdita di tratta delle scie
// sottodense (McKinley 1961) e con il seno dell'elevazione del radiante nel punto di riflessione.
using System;
using System.Collections.Generic;

namespace YAMS
{
    public class HotPoint
    {
        public string Label;
        public LatLon Pos;
        public double BearingFromHome, DistFromHome, BearingFromDx, DistFromDx, ElevFromHome;
        public double SideScatterDeg;   // scarto rispetto alla direzione diretta
        public bool InLobe;             // dentro il lobo principale dell'antenna (HPBW)
        public bool Recommended;        // lato consigliato dal calcolo dell'efficienza (disegnato in rosso)
    }

    public class PathModel
    {
        public LatLon Home, Dx, Mid;
        public double HomeAltKm, DxAltKm, LayerKm, MinElevDeg;
        public double DistKm, BearingHomeToDx, BearingDxToHome;
        public double ElevToMidDeg;          // elevazione verso il punto medio alla quota delle scie
        public HotPoint HotA, HotB;          // hot spot classici ai lati del punto medio (puntamento d'antenna)
        public HotPoint LimitA, LimitB;      // incroci delle curve d'orizzonte (articolo): limiti della zona comune
        public double HotRadiusKm;
        public static double HotSpotOffsetKm = 150;   // distanza degli hot spot dal punto medio (AMS Bulletin 203: 50-150 km)
        public bool Valid;
        public string Message = "";

        // campo di efficienza in funzione della direzione del radiante (riferimento: punto medio)
        public const double AzStep = 5.0, ElStep = 2.5;
        public int NAz = 72, NEl = 37;
        double[,] effField, leftField;
        double fieldMax = 1;
        public int SamplePoints;

        struct Sample { public Vec3 Up, N, P; public LatLon Ll; public double W; public bool Left; }
        List<Sample> samples = new List<Sample>();

        public static PathModel Build(LatLon home, double homeAltKm, LatLon dx, double dxAltKm, double layerKm, double minElevDeg)
        {
            PathModel m = new PathModel();
            m.Home = home; m.Dx = dx; m.HomeAltKm = homeAltKm; m.DxAltKm = dxAltKm;
            m.LayerKm = layerKm; m.MinElevDeg = minElevDeg;
            m.DistKm = Astro.DistanceKm(home, dx);
            if (m.DistKm < 50) { m.Message = Lang.T("Tratta troppo corta"); return m; }
            m.BearingHomeToDx = Astro.Bearing(home, dx);
            m.BearingDxToHome = Astro.Bearing(dx, home);
            m.Mid = Astro.Destination(home, m.BearingHomeToDx, m.DistKm / 2);
            m.ElevToMidDeg = MsGeometry.ElevationArticle(m.DistKm / 2, layerKm, homeAltKm);
            m.ComputeHot();
            m.BuildSamples();
            m.BuildField();
            m.Valid = true;
            return m;
        }

        void ComputeHot()
        {
            HotRadiusKm = MsGeometry.LayerRangeKm(LayerKm, MinElevDeg, HomeAltKm);
            // Hot spot classici: a HotSpotOffsetKm ai lati del punto medio, perpendicolari alla tratta (A a sinistra, B a destra)
            double bMid = Astro.Bearing(Mid, Dx);
            HotA = MakeHot(Astro.Destination(Mid, bMid - 90, HotSpotOffsetKm), "Hot A");
            HotB = MakeHot(Astro.Destination(Mid, bMid + 90, HotSpotOffsetKm), "Hot B");
            // Limiti della zona comune (articolo): incroci delle curve dove l'orizzonte di ciascuna stazione incontra lo strato E
            LatLon p1, p2;
            if (!MsGeometry.SmallCircleIntersect(Home, Dx, HotRadiusKm / Astro.R, out p1, out p2))
            {
                Message = Lang.T("Tratta piu' lunga del doppio della portata alla quota delle scie: le due stazioni non vedono una zona comune dello strato E.");
                LimitA = null; LimitB = null; return;
            }
            Vec3 h = Astro.Vec(Home, 1), d = Astro.Vec(Dx, 1);
            Vec3 nrm = h.Cross(d);
            bool p1Left = Astro.Vec(p1, 1).Dot(nrm) > 0;
            LimitA = MakeHot(p1Left ? p1 : p2, "Limite A");
            LimitB = MakeHot(p1Left ? p2 : p1, "Limite B");
        }

        HotPoint MakeHot(LatLon p, string label)
        {
            HotPoint hp = new HotPoint();
            hp.Label = label; hp.Pos = p;
            hp.BearingFromHome = Astro.Bearing(Home, p); hp.DistFromHome = Astro.DistanceKm(Home, p);
            hp.BearingFromDx = Astro.Bearing(Dx, p); hp.DistFromDx = Astro.DistanceKm(Dx, p);
            hp.ElevFromHome = MsGeometry.ElevationArticle(hp.DistFromHome, LayerKm, HomeAltKm);
            hp.SideScatterDeg = Astro.Norm180(hp.BearingFromHome - BearingHomeToDx);
            return hp;
        }

        void BuildSamples()
        {
            samples.Clear();
            double step = 15.0;
            Vec3 pa = Astro.Vec(Home, Astro.R + HomeAltKm), pb = Astro.Vec(Dx, Astro.R + DxAltKm);
            Vec3 ua = pa.Unit, ub = pb.Unit;
            double bm = Astro.Bearing(Mid, Dx);
            double maxCross = Math.Min(1500, HotRadiusKm + 100);
            for (double u = -DistKm / 2 - 900; u <= DistKm / 2 + 900; u += step)
            {
                LatLon q = u >= 0 ? Astro.Destination(Mid, bm, u) : Astro.Destination(Mid, bm + 180, -u);
                // direzione locale della tratta nel punto q (verso il corrispondente)
                double bq = u < DistKm / 2 - 1 ? Astro.Bearing(q, Dx) : Astro.Norm360(Astro.Bearing(q, Home) + 180);
                for (double v = -maxCross; v <= maxCross; v += step)
                {
                    LatLon p = v >= 0 ? Astro.Destination(q, bq + 90, v) : Astro.Destination(q, bq - 90, -v);
                    Vec3 P = Astro.Vec(p, Astro.R + LayerKm);
                    Vec3 ra = P - pa, rb = P - pb;
                    double Ra = ra.Length, Rb = rb.Length;
                    double ea = Math.Asin(ra.Dot(ua) / Ra) / Astro.D2R;
                    double eb = Math.Asin(rb.Dot(ub) / Rb) / Astro.D2R;
                    if (ea < MinElevDeg || eb < MinElevDeg) continue;
                    Vec3 na = ra * (-1.0 / Ra), nb = rb * (-1.0 / Rb);
                    Vec3 n = (na + nb).Unit;
                    double phi = Math.Acos(Math.Max(-1, Math.Min(1, na.Dot(nb)))) / 2.0;
                    phi = Math.Min(phi, 85 * Astro.D2R);
                    double cp = Math.Cos(phi);
                    Sample s = new Sample();
                    s.Up = P.Unit; s.N = n; s.P = P; s.Ll = p;
                    s.W = 1e9 / (Ra * Rb * (Ra + Rb) * cp * cp);
                    s.Left = v < 0;
                    samples.Add(s);
                }
            }
            SamplePoints = samples.Count;
        }

        double Score(Vec3 s, out double left)
        {
            const double sig2 = 2 * 0.05 * 0.05;
            double tot = 0; left = 0;
            for (int i = 0; i < samples.Count; i++)
            {
                Sample sm = samples[i];
                double sel = s.Dot(sm.Up);
                if (sel <= 0) continue;
                double k = s.Dot(sm.N);
                double c = sm.W * Math.Exp(-k * k / sig2) * sel;
                tot += c;
                if (sm.Left) left += c;
            }
            return tot;
        }

        // ---- accesso ai singoli punti di riflessione (zone PREV in pianta e vista in sezione) ----
        public LatLon SampleLatLon(int i) { return samples[i].Ll; }
        public Vec3 SamplePos(int i) { return samples[i].P; }       // posizione ECEF alla quota delle scie (km)
        public Vec3 SampleUp(int i) { return samples[i].Up; }
        public bool SampleLeft(int i) { return samples[i].Left; }

        // Contributo di ciascun punto all'efficienza per un radiante in direzione s:
        // la somma dei valori restituiti e' l'efficienza in % (stessa normalizzazione di Efficiency).
        public double[] Contributions(Vec3 s)
        {
            double[] c = new double[samples.Count];
            if (!Valid) return c;
            const double sig2 = 2 * 0.05 * 0.05;
            for (int i = 0; i < samples.Count; i++)
            {
                Sample sm = samples[i];
                double sel = s.Dot(sm.Up);
                if (sel <= 0) continue;
                double k = s.Dot(sm.N);
                c[i] = 100.0 * sm.W * Math.Exp(-k * k / sig2) * sel / fieldMax;
            }
            return c;
        }

        void BuildField()
        {
            effField = new double[NAz, NEl]; leftField = new double[NAz, NEl];
            fieldMax = 1e-30;
            for (int i = 0; i < NAz; i++)
                for (int j = 0; j < NEl; j++)
                {
                    Vec3 s = Astro.FromAltAz(Mid, i * AzStep, j * ElStep);
                    double l;
                    double v = Score(s, out l);
                    effField[i, j] = v; leftField[i, j] = l;
                    if (v > fieldMax) fieldMax = v;
                }
        }

        // Efficienza 0..100 % per un radiante in direzione s (versore terra fissa).
        // leftFrac = frazione delle riflessioni dal lato A (sinistra della tratta).
        public double Efficiency(Vec3 s, out double leftFrac)
        {
            leftFrac = 0.5;
            if (!Valid) return 0;
            double az, el; Astro.AltAz(Mid, s, out az, out el);
            if (el <= 0) return 0;
            double fi = az / AzStep, fj = el / ElStep;
            int i0 = (int)Math.Floor(fi), j0 = (int)Math.Floor(fj);
            double ti = fi - i0, tj = fj - j0;
            int i1 = (i0 + 1) % NAz; i0 = i0 % NAz;
            int j1 = Math.Min(j0 + 1, NEl - 1); j0 = Math.Min(j0, NEl - 1);
            double v = Bil(effField, i0, i1, j0, j1, ti, tj);
            double l = Bil(leftField, i0, i1, j0, j1, ti, tj);
            leftFrac = v > 0 ? l / v : 0.5;
            return Math.Max(0, Math.Min(100, 100.0 * v / fieldMax));
        }

        static double Bil(double[,] f, int i0, int i1, int j0, int j1, double ti, double tj)
        {
            return f[i0, j0] * (1 - ti) * (1 - tj) + f[i1, j0] * ti * (1 - tj) + f[i0, j1] * (1 - ti) * tj + f[i1, j1] * ti * tj;
        }
    }

    public static class MsGeometry
    {
        public const double ElectronRadius = 2.8179403e-15;  // m

        // Formula dell'elevazione dell'articolo (Immagine 7):
        // e = 90 - 57,3 * arctan[ (r tan x + (H - r(1-cos x)/cos x) sin x) / (H - r(1-cos x)/cos x - h sin x) ]
        // x = distanza dallo scatter point / r. Si usa atan2 per avere valori negativi oltre l'orizzonte.
        public static double ElevationArticle(double distKm, double scatterHeightKm, double stationAltKm)
        {
            double r = Astro.R, x = distKm / r;
            double a = scatterHeightKm - r * (1 - Math.Cos(x)) / Math.Cos(x);
            double num = r * Math.Tan(x) + a * Math.Sin(x);
            double den = a - stationAltKm * Math.Sin(x);
            return 90.0 - Math.Atan2(num, den) / Astro.D2R;
        }

        // Distanza a terra (km) alla quale un raggio con elevazione elevDeg raggiunge la quota H
        public static double LayerRangeKm(double layerKm, double elevDeg, double stationAltKm)
        {
            double e = elevDeg * Astro.D2R;
            double arg = (Astro.R + stationAltKm) * Math.Cos(e) / (Astro.R + layerKm);
            double x = Math.PI / 2 - e - Math.Asin(Math.Max(-1, Math.Min(1, arg)));
            return x * Astro.R;
        }

        // Intersezione di due cerchi minori di uguale raggio angolare centrati in a e b
        public static bool SmallCircleIntersect(LatLon a, LatLon b, double radiusRad, out LatLon p1, out LatLon p2)
        {
            p1 = new LatLon(); p2 = new LatLon();
            Vec3 va = Astro.Vec(a, 1), vb = Astro.Vec(b, 1);
            double c = Math.Cos(radiusRad), ab = va.Dot(vb), den = 1 - ab * ab;
            if (den < 1e-12) return false;
            double x = (c - c * ab) / den, y = (c - c * ab) / den;
            Vec3 p0 = va * x + vb * y;
            Vec3 n = va.Cross(vb);
            double q = 1 - p0.Dot(p0);
            if (q < 0) return false;
            double t = Math.Sqrt(q / n.Dot(n));
            p1 = Astro.ToLatLon(p0 + n * t);
            p2 = Astro.ToLatLon(p0 - n * t);
            return true;
        }

        // Attenuazione di tratta (dB) per una scia sottodensa, McKinley (1961), beta = 0, sin(alfa) = 1:
        // L = 10 log10[ 16 pi^2 R1 R2 (R1+R2) cos^2(phi) / (lambda^3 q^2 re^2) ]  (guadagni d'antenna esclusi)
        public static double PathLossDb(LatLon home, double homeAltKm, LatLon dx, double dxAltKm, LatLon scatter, double layerKm, double freqMHz, double qPerM)
        {
            Vec3 pa = Astro.Vec(home, Astro.R + homeAltKm), pb = Astro.Vec(dx, Astro.R + dxAltKm);
            Vec3 P = Astro.Vec(scatter, Astro.R + layerKm);
            Vec3 ra = pa - P, rb = pb - P;
            double R1 = ra.Length * 1000, R2 = rb.Length * 1000;
            double phi = Math.Acos(Math.Max(-1, Math.Min(1, ra.Unit.Dot(rb.Unit)))) / 2.0;
            phi = Math.Min(phi, 85 * Astro.D2R);
            double lambda = 299.792458 / freqMHz;
            double cp = Math.Cos(phi);
            double lin = 16 * Math.PI * Math.PI * R1 * R2 * (R1 + R2) * cp * cp
                         / (Math.Pow(lambda, 3) * qPerM * qPerM * ElectronRadius * ElectronRadius);
            return 10 * Math.Log10(lin);
        }

        // ---- Antenna Yagi ----
        // Guadagno da lunghezza del boom: curva DL6WU, G(dBi) = 7,773 log10(L/lambda) + 11,43
        // (riportata da W8IO; valida per Yagi lunghe, boom da circa 2 lambda in su).
        public static double YagiGainDbi(double boomM, double freqMHz)
        {
            double lam = 299.792458 / freqMHz;
            double bl = Math.Max(0.3, boomM / lam);
            return 7.773 * Math.Log10(bl) + 11.43;
        }

        // Apertura a -3 dB dal guadagno. Regola di Kraus G = 41253 / (thetaE x thetaH); con lobo circolare
        // thetaK = sqrt(41253/G). Le simulazioni NEC delle Yagi DL6WU (W4RNL, "Appreciating DL6WU Wide-Band
        // Long-Boom Yagi Design" e "Long-Boom Yagi Rules of Thumb") mostrano che Kraus allarga il lobo del 5-10 %:
        // piano orizzontale (E, azimut con polarizzazione orizzontale) = 0,93 x thetaK. Per il piano verticale (H)
        // si tiene thetaK senza correzione: sulle Yagi lunghe H e' di poco piu' largo di E (14 lambda: 18,4/18,8 gradi
        // simulati, Kraus 19,5). Regole approssimate, ricavate da poche antenne DL6WU.
        public const double HpbwECorrection = 0.93;

        public static double HpbwKrausDeg(double gainDbi)
        {
            return Math.Sqrt(41253.0 / Math.Pow(10, gainDbi / 10.0));
        }

        // lobo orizzontale (piano E): quello usato per il puntamento in azimut e per gli Hot nel lobo
        public static double HpbwFromGainDeg(double gainDbi)
        {
            return HpbwECorrection * HpbwKrausDeg(gainDbi);
        }

        // lobo verticale (piano H): Kraus senza correzione
        public static double HpbwVerticalDeg(double gainDbi)
        {
            return HpbwKrausDeg(gainDbi);
        }

        // Regole sperimentali SM6FHZ (VUSHF 2025): numero/intensita' ~ lambda^3, durata ~ lambda^2
        public static double CountScale(double fromMHz, double toMHz, double exponent)
        {
            return Math.Pow(fromMHz / toMHz, exponent);
        }

        public static double DurationScale(double fromMHz, double toMHz)
        {
            return Math.Pow(fromMHz / toMHz, 2.0);
        }

        // Durata massima di un ping secondo IARU Regione 1 (citata nell'articolo), ms
        public static double PingMaxMs(double fMHz)
        {
            if (fMHz < 60) return 1000;
            if (fMHz < 100) return 500;
            if (fMHz < 300) return 100;
            return 13;
        }
    }
}
