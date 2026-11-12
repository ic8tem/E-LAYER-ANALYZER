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
        public static double HotSpotOffsetKm = 150;   // distanza degli hot spot dal punto medio scritta a mano (km)
        public static bool HotSpotAuto = true;        // distanza dalla tabella AMS (J. Richardson, riportata da DJ5HG)
        // Opzione (DJ5HG, "Die Geometrie von Meteorscatterverbindungen", 2004): con il radiante basso cadono meno
        // meteore per km2 (fattore sin El) ma le scie sono piu' lunghe (fino a 100 km utili) e soddisfano piu' spesso la
        // legge di riflessione. Con l'opzione accesa il peso del radiante diventa min(sin El, TrailDh/TrailMax).
        public static bool TrailLength = false;
        // Angolo beta tra la scia e il piano di propagazione (McKinley 1961, ITU-R P.843 eq. 3): il peso di ogni punto vale
        // 1 / [R1 R2 (R1+R2) (1 - cos^2(beta) sin^2(phi))]; beta = 0 (scia nel piano) da' il vecchio 1/cos^2(phi), le scie che
        // attraversano il piano pesano fino a 1/cos^2(phi) volte meno (10-15 dB sulle tratte di 600-2000 km).
        // Opzione, accesa di partenza (calcolo prudente: nessuna scia sopravvalutata); con beta i radianti lungo la tratta
        // prevalgono su quelli perpendicolari.
        public static bool BetaFactor = true;
        public const double TrailDhKm = 15, TrailMaxKm = 100;

        // Scarto d'antenna degli hot spot dalla direzione diretta in funzione della distanza (J. Richardson, AMS,
        // "Some Notes and Equations for Forward Scatter", riportata da DJ5HG): 13 gradi a 1000 km, 11 a 1250, 10 a 1500
        // e a 2000 km; sotto 1000 km 13 gradi, sopra 2000 km 10 gradi. La distanza dal punto medio e' (D/2) tan(scarto).
        static readonly double[] AmsKm = { 1000, 1250, 1500, 2000 }, AmsDeg = { 13, 11, 10, 10 };
        public static double AmsOffsetDeg(double distKm)
        {
            if (distKm <= AmsKm[0]) return AmsDeg[0];
            for (int i = 1; i < AmsKm.Length; i++)
                if (distKm <= AmsKm[i]) return AmsDeg[i - 1] + (AmsDeg[i] - AmsDeg[i - 1]) * (distKm - AmsKm[i - 1]) / (AmsKm[i] - AmsKm[i - 1]);
            return AmsDeg[AmsDeg.Length - 1];
        }
        public static double HotOffsetFor(double distKm)
        {
            if (!HotSpotAuto) return HotSpotOffsetKm;
            return Math.Max(20, distKm / 2 * Math.Tan(AmsOffsetDeg(distKm) * Astro.D2R));
        }
        public double HotOffsetKm;   // distanza usata per questa tratta

        // peso della direzione del radiante nel punto di riflessione (sel = sin dell'elevazione locale)
        static double RadiantWeight(double sel)
        {
            if (sel <= 0) return 0;
            return TrailLength ? Math.Min(sel, TrailDhKm / TrailMaxKm) : sel;
        }
        public bool Valid;
        public string Message = "";

        // campo di efficienza in funzione della direzione del radiante (riferimento: punto medio)
        public const double AzStep = 5.0, ElStep = 2.5;
        public int NAz = 72, NEl = 37;
        double[,] effField, leftField;
        double fieldMax = 1;
        public int SamplePoints;

        struct Sample { public Vec3 Up, N, P, Pn; public LatLon Ll; public double W, Wb, S2; public bool Left; }
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
            HotOffsetKm = HotOffsetFor(DistKm);
            HotA = MakeHot(Astro.Destination(Mid, bMid - 90, HotOffsetKm), "Hot A");
            HotB = MakeHot(Astro.Destination(Mid, bMid + 90, HotOffsetKm), "Hot B");
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
                    s.Wb = 1e9 / (Ra * Rb * (Ra + Rb)); s.S2 = 1 - cp * cp;
                    Vec3 pn = na.Cross(nb); double pl = pn.Length;
                    s.Pn = pl > 1e-12 ? pn * (1.0 / pl) : n;
                    s.Left = v < 0;
                    samples.Add(s);
                }
            }
            SamplePoints = samples.Count;
        }

        // peso del punto per una scia diretta come s (verso il radiante): con BetaFactor dipende dall'angolo tra la scia e il piano A-P-B
        static double Weight(Sample sm, Vec3 s)
        {
            if (!BetaFactor) return sm.W;
            double sb = s.Dot(sm.Pn);                  // seno di beta
            double den = 1 - (1 - sb * sb) * sm.S2;    // 1 - cos^2(beta) sin^2(phi)
            return sm.Wb / Math.Max(den, 1e-6);
        }

        double Score(Vec3 s, out double left)
        {
            const double sig2 = 2 * 0.05 * 0.05;
            double tot = 0; left = 0;
            for (int i = 0; i < samples.Count; i++)
            {
                Sample sm = samples[i];
                double sel = RadiantWeight(s.Dot(sm.Up));
                if (sel <= 0) continue;
                double k = s.Dot(sm.N);
                double c = Weight(sm, s) * Math.Exp(-k * k / sig2) * sel;
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
                double sel = RadiantWeight(s.Dot(sm.Up));
                if (sel <= 0) continue;
                double k = s.Dot(sm.N);
                c[i] = 100.0 * Weight(sm, s) * Math.Exp(-k * k / sig2) * sel / fieldMax;
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

        // Meta' dell'angolo di scattering (gradi) in un punto alla quota delle scie: 0 = backscatter (radar), verso 90 = radente
        public double HalfScatterAngleDeg(LatLon p)
        {
            Vec3 pa = Astro.Vec(Home, Astro.R + HomeAltKm), pb = Astro.Vec(Dx, Astro.R + DxAltKm), P = Astro.Vec(p, Astro.R + LayerKm);
            Vec3 na = (pa - P).Unit, nb = (pb - P).Unit;
            return Math.Acos(Math.Max(-1, Math.Min(1, na.Dot(nb)))) / 2.0 / Astro.D2R;
        }

        // Efficienza in dB riferita al riferimento fisso di DJ5HG (MS-Geometrie, 2004): 0 dB = massimo di una tratta
        // nord-sud di 1000 km a cavallo dell'equatore con il radiante sull'equatore celeste (0 h, 0 gradi) all'equinozio,
        // cioe' il radiante che dal punto medio percorre il cerchio est-ovest per lo zenit. Stessa quota e stessi pesi della tratta.
        static double refRaw = double.NaN; static string refKey = "";
        double ReferenceRaw()
        {
            string key = LayerKm.ToString("F2") + "|" + MinElevDeg.ToString("F2") + "|" + TrailLength + "|" + BetaFactor;
            if (refKey == key && !double.IsNaN(refRaw)) return refRaw;
            PathModel r = new PathModel();
            r.Home = new LatLon(-4.4966, 0); r.Dx = new LatLon(4.4966, 0); r.HomeAltKm = 0; r.DxAltKm = 0;
            r.LayerKm = LayerKm; r.MinElevDeg = MinElevDeg; r.DistKm = Astro.DistanceKm(r.Home, r.Dx);
            r.BearingHomeToDx = Astro.Bearing(r.Home, r.Dx); r.Mid = new LatLon(0, 0);
            r.HotRadiusKm = MsGeometry.LayerRangeKm(LayerKm, MinElevDeg, 0);
            r.BuildSamples();
            double best = 1e-30, l;
            for (double el = 1; el <= 90; el += 1)
            {
                best = Math.Max(best, r.Score(Astro.FromAltAz(r.Mid, 90, el), out l));
                best = Math.Max(best, r.Score(Astro.FromAltAz(r.Mid, 270, el), out l));
            }
            refRaw = best; refKey = key;
            return refRaw;
        }

        public double AbsoluteDb(Vec3 s)
        {
            if (!Valid) return double.NaN;
            double l, v = Score(s, out l);
            if (v <= 0) return double.NegativeInfinity;
            return 10 * Math.Log10(v / ReferenceRaw());
        }

        // direzione d'antenna dalla mia stazione: media dei rilevamenti dei punti di riflessione pesata con il loro contributo
        public double WeightedBearingFromHome(Vec3 s)
        {
            double[] c = Contributions(s);
            double sx = 0, sy = 0;
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] <= 0) continue;
                double b = Astro.Bearing(Home, samples[i].Ll) * Astro.D2R;
                sx += c[i] * Math.Sin(b); sy += c[i] * Math.Cos(b);
            }
            if (sx == 0 && sy == 0) return double.NaN;
            return Astro.Norm360(Math.Atan2(sx, sy) / Astro.D2R);
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

        // Attenuazione di tratta (dB) per una scia sottodensa, McKinley (1961) e ITU-R P.843, sin(alfa) = 1:
        // L = 10 log10[ 16 pi^2 R1 R2 (R1+R2) (1 - cos^2(beta) sin^2(phi)) / (lambda^3 q^2 re^2) ] - 10 log10(a1)  (guadagni d'antenna esclusi)
        // beta = angolo tra la scia e il piano di propagazione (0 = scia nel piano, caso piu' favorevole);
        // a1 = exp[-8 pi^2 r0^2 / (lambda^2 sec^2 phi)] = fattore del raggio iniziale della scia (P.843 eq. 7), r0 dalla quota (eq. 4).
        public static double PathLossDb(LatLon home, double homeAltKm, LatLon dx, double dxAltKm, LatLon scatter, double layerKm, double freqMHz, double qPerM)
        {
            return PathLossDb(home, homeAltKm, dx, dxAltKm, scatter, layerKm, freqMHz, qPerM, 0, true);
        }

        public static double PathLossDb(LatLon home, double homeAltKm, LatLon dx, double dxAltKm, LatLon scatter, double layerKm, double freqMHz, double qPerM,
                                        double betaDeg, bool initialRadius)
        {
            Vec3 pa = Astro.Vec(home, Astro.R + homeAltKm), pb = Astro.Vec(dx, Astro.R + dxAltKm);
            Vec3 P = Astro.Vec(scatter, Astro.R + layerKm);
            Vec3 ra = pa - P, rb = pb - P;
            double R1 = ra.Length * 1000, R2 = rb.Length * 1000;
            double phi = Math.Acos(Math.Max(-1, Math.Min(1, ra.Unit.Dot(rb.Unit)))) / 2.0;
            phi = Math.Min(phi, 85 * Astro.D2R);
            double lambda = 299.792458 / freqMHz;
            double sp = Math.Sin(phi), cb = Math.Cos(betaDeg * Astro.D2R);
            double lin = 16 * Math.PI * Math.PI * R1 * R2 * (R1 + R2) * (1 - cb * cb * sp * sp)
                         / (Math.Pow(lambda, 3) * qPerM * qPerM * ElectronRadius * ElectronRadius);
            double L = 10 * Math.Log10(lin);
            if (initialRadius) L += InitialRadiusLossDb(ItuMeanHeightKm(freqMHz), freqMHz, phi / Astro.D2R);
            return L;
        }

        // ITU-R P.843 eq. 15: quota media delle scie che danno eco alla frequenza f (MHz): h = 124 - 17 log10 f (km).
        // Il raggio iniziale si valuta a questa quota: e' li' che arrivano gli echi a quella frequenza (a 102 km fissi
        // il fattore a 432 MHz varrebbe decine di dB, cioe' nessun eco).
        public static double ItuMeanHeightKm(double freqMHz) { return 124 - 17 * Math.Log10(freqMHz); }

        // ITU-R P.843 eq. 4: raggio iniziale della scia (m) dalla quota (km): log10 r0 = 0,035 h - 3,45
        public static double InitialRadiusItuM(double hKm) { return Math.Pow(10, 0.035 * hKm - 3.45); }

        // moltiplicatore del raggio iniziale scelto dall'utente: 2 = Stober et al. 2021 (CMOR, 85-95 km: r0 di 0,5-2 m,
        // per lo piu' 1-1,5 m, circa il doppio delle misure precedenti; valore prudente, di partenza), 1 = ITU-R P.843, 0 = escluso
        public static double InitialRadiusFactor = 2.0;

        // raggio iniziale usato nel calcolo (m)
        public static double InitialRadiusM(double hKm) { return InitialRadiusFactor * InitialRadiusItuM(hKm); }

        // perdita (dB, positiva) dovuta al raggio iniziale: -10 log10 a1, a1 = exp[-8 pi^2 r0^2 cos^2(phi) / lambda^2]
        public static double InitialRadiusLossDb(double hKm, double freqMHz, double phiDeg)
        {
            double r0 = InitialRadiusM(hKm), lambda = 299.792458 / freqMHz, c = Math.Cos(Math.Min(phiDeg, 85) * Astro.D2R);
            return 10 * Math.Log10(Math.E) * 8 * Math.PI * Math.PI * r0 * r0 * c * c / (lambda * lambda);
        }

        // quota minima delle scie (km) perche' il punto medio di una tratta di distKm sia sopra l'orizzonte delle due stazioni
        public static double MinLayerForPathKm(double distKm, double stationAltKm)
        {
            double lo = 0, hi = 2000;
            if (ElevationArticle(distKm / 2, hi, stationAltKm) < 0) return double.NaN;
            for (int i = 0; i < 60; i++) { double m = (lo + hi) / 2; if (ElevationArticle(distKm / 2, m, stationAltKm) >= 0) hi = m; else lo = m; }
            return hi;
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
        // giudizio classico della geometria (regola dei radioamatori: radiante alto e di traverso alla tratta), al punto medio.
        // 0 = radiante sotto l'orizzonte, 1 = marginale (sotto 15° o sopra 70°, oppure a meno di 30° dalla direzione della tratta),
        // 2 = buono, 3 = migliore (15-70° e a piu' di 60° dalla direzione della tratta). Soglie come in meteor-mcp (qso-graph, 2026).
        public static int ClassicVerdict(double radAzDeg, double radElDeg, double pathBearingDeg)
        {
            if (radElDeg <= 0) return 0;
            double off = Math.Abs(Astro.Norm180(radAzDeg - pathBearingDeg));
            if (off > 90) off = 180 - off;
            bool elOk = radElDeg >= 15 && radElDeg <= 70;
            if (!elOk || off < 30) return 1;
            return off > 60 ? 3 : 2;
        }

        // esponente k della traslazione di frequenza dall'indice di popolazione r: s = 1 + 2,5 log10 r, k = 1,5 (s - 1) = 3,75 log10 r
        // (numero di scie sopra soglia fissa ~ lambda^(1,5 (s-1)); IZ5TEK, OH5IY). Fuori da 1,5-4 il valore di r non e' plausibile: NaN.
        public static double KFromPopIndex(double r)
        {
            if (double.IsNaN(r) || r < 1.5 || r > 4) return double.NaN;
            return 3.75 * Math.Log10(r);
        }

        public static double CountScale(double fromMHz, double toMHz, double exponent)
        {
            return Math.Pow(fromMHz / toMHz, exponent);
        }

        // durata dei ping: false = teorica, lambda^2 (McKinley; SM6FHZ), valore prudente di partenza;
        // true = osservata: mediane delle costanti di decadimento (potenza) degli echi sottodensi misurate da NN4SA (EM64)
        // durante le Perseidi 2026 (HamSCI MSQP, R. Suggs NN4NT, gruppo di lavoro del 4/11/2026): 148 ms a 28,145 MHz (41 echi),
        // 78 ms a 50,260 MHz (1198), 53 ms a 144,150 MHz (59). Interpolazione lineare in scala log-log; sotto 28 MHz si prolunga
        // il tratto 10-6 m, sopra 144 MHz (nessuna misura) si torna a lambda^2, che da' durate piu' brevi (prudente).
        public static bool DurationObserved = false;
        static readonly double[] ObsF = { 28.145, 50.260, 144.150 };
        static readonly double[] ObsMs = { 148, 78, 53 };

        // durata tipica osservata (ms) alla frequenza f secondo le misure MSQP
        public static double ObservedDurationMs(double fMHz)
        {
            if (fMHz >= ObsF[2]) return ObsMs[2] * Math.Pow(ObsF[2] / fMHz, 2.0);
            int i = fMHz < ObsF[1] ? 0 : 1;
            double p = Math.Log(ObsMs[i + 1] / ObsMs[i]) / Math.Log(ObsF[i + 1] / ObsF[i]);
            return ObsMs[i] * Math.Pow(fMHz / ObsF[i], p);
        }

        public static double DurationScale(double fromMHz, double toMHz)
        {
            if (DurationObserved) return ObservedDurationMs(toMHz) / ObservedDurationMs(fromMHz);
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
