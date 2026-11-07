// YA-MS - radar GRAVES (143,050 MHz CW, JN27SI): settori e passi del fascio, cielo visto da GRAVES, cronologia
//
// Fonti: Rob Hardenberg PE1ITR, "The 143.050 MHz Graves Radar, a VHF Beacon" (DUBUS 4/2007) e
// "Graves Radar Update April 2021" (pe1itr.com/graves); Bolidozor wiki "Graves".
// Quattro schiere a fasi coprono il cielo a sud (azimut 90-270°) in quattro settori di 45°:
// D (90-135°), C (135-180°), B (180-225°), A (225-270°). Ogni settore e' diviso in 6 passi da 7,5°
// illuminati uno dopo l'altro, nello stesso momento in tutti e quattro i settori. Fascio largo circa 25°
// in elevazione (elevazioni basse). Dal 2021 ogni passo dura 0,8 s (ciclo 4,8 s); nel 2007 3,2 s (ciclo 19,2 s).
// La sequenza e' legata all'ora UTC: passo = floor(((secondi UTC dalle 00:00) mod ciclo) / passo).
// La stazione di comando a volte la sposta di qualche passo: e' una previsione da confermare con l'ascolto.
// Numerazione dei passi (convenzione di YA-MS): 1-6 dentro ogni settore a partire dall'azimut piu' basso.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YAMS
{
    public static class GravesBeam
    {
        public const double Lat = 47.348, Lon = 5.515;   // trasmettitore (Broye-lès-Pesmes)
        public const double AltKm = 0.2;                 // quota del trasmettitore, circa 200 m
        public const double FreqMHz = 143.050;
        public const string Locator = "JN27SI";
        public const double AzMin = 90, AzMax = 270, SectorDeg = 45, StepDeg = 7.5;
        public const double BeamElevDeg = 25;            // larghezza del fascio in elevazione (circa)
        public const int Steps = 6;
        public static readonly string[] Sectors = { "D", "C", "B", "A" };   // indice 0 = 90-135°, ... 3 = 225-270°

        public static LatLon Pos { get { return new LatLon(Lat, Lon); } }

        // Settore (0..3) e passo (0..5) che illuminano l'azimut az visto da GRAVES; false se fuori da 90-270°
        public static bool Locate(double az, out int sector, out int step)
        {
            sector = -1; step = -1;
            az = Astro.Norm360(az);
            if (az < AzMin || az >= AzMax) return false;
            double x = az - AzMin;
            sector = Math.Max(0, Math.Min(3, (int)Math.Floor(x / SectorDeg)));
            step = Math.Max(0, Math.Min(Steps - 1, (int)Math.Floor((x - sector * SectorDeg) / StepDeg)));
            return true;
        }

        // posizione nel ciclo (s): secondi UTC dalle 00:00 modulo il ciclo (86400 e' multiplo sia di 4,8 sia di 19,2)
        public static double Phase(DateTime utc, double stepSec)
        {
            double c = Steps * stepSec;
            double p = utc.TimeOfDay.TotalSeconds % c;
            return p < 0 ? p + c : p;
        }

        // passo illuminato adesso (0..5), uguale in tutti i settori
        public static int ActiveStep(DateTime utc, double stepSec)
        {
            return Math.Max(0, Math.Min(Steps - 1, (int)Math.Floor(Phase(utc, stepSec) / stepSec)));
        }

        // secondi che mancano al prossimo inizio del passo step (0..5)
        public static double WaitSec(DateTime utc, int step, double stepSec)
        {
            double c = Steps * stepSec;
            double d = step * stepSec - Phase(utc, stepSec);
            while (d < 0) d += c;
            return d;
        }

        // Una scia si sente solo se il fascio la illumina mentre dura: probabilita' di coglierla
        // ~ min(1, (durata del passo + durata della scia) / ciclo)
        public static double CatchFactor(double stepSec, double trailSec)
        {
            return Math.Min(1.0, (stepSec + trailSec) / (Steps * stepSec));
        }

        // secondi dentro il minuto, a decimi (es. ":41,9")
        public static string SecLabel(DateTime t)
        {
            double s = Math.Floor((t.Second + t.Millisecond / 1000.0) * 10) / 10;
            return ":" + s.ToString("00.0", CultureInfo.CurrentCulture);
        }

        public static Color SectorColor(int i)
        {
            switch (i)
            {
                // colori fissi (SkyPal): il cielo di GRAVES resta scuro in tutti gli schemi
                case 0: return SkyPal.Pink;     // D
                case 1: return SkyPal.Azure;    // C
                case 2: return SkyPal.Violet;   // B
                default: return SkyPal.Orange;  // A
            }
        }
    }

    // punto da segnare nel cielo visto da GRAVES (Hot A / Hot B della tratta GRAVES -> stazione)
    public class GravesMark
    {
        public LatLon Pos;
        public string Label = "";
        public Color Color;
        public bool Main;   // hot spot consigliato: linea tratteggiata dalla stazione
    }

    // Il cielo visto da GRAVES: pianta azimutale equidistante centrata sul trasmettitore (N in alto, E a destra).
    // La corona colorata e' la parte dello strato delle scie coperta dal fascio (elevazione 0-25° vista da GRAVES).
    public class GravesView : Control
    {
        public LatLon Graves = GravesBeam.Pos;
        public bool HasMe;
        public LatLon Me;
        public string MeLabel = "";
        public List<GravesMark> Marks = new List<GravesMark>();
        public List<PrevSpot> Prev = new List<PrevSpot>();
        public double LayerKm = 90;
        public int ActiveStep = -1;              // passo illuminato adesso (0..5)
        public int HotSector = -1, HotStep = -1; // passo che illumina l'hot spot consigliato
        public List<System.Drawing.PointF[]> MapLines, MapBorders;

        // mappa proiettata (km attorno a GRAVES), ricalcolata solo se cambiano i dati o il raggio
        StreamGeometry coastGeo, borderGeo;
        object coastFor, borderFor;
        int coastCount = -1, borderCount = -1;
        double geoLimit = -1;

        double ls = 1.0;
        double LS { get { return Math.Max(1.0, Math.Min(UiScale.Font, Math.Min(Bounds.Width, Bounds.Height) / 2 / 170)); } }

        public GravesView()
        {
            ClipToBounds = true;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            UiScale.Changed += OnScaleChanged;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            UiScale.Changed -= OnScaleChanged;
        }

        void OnScaleChanged() { InvalidateMeasure(); InvalidateVisual(); }

        FormattedText Ft(string s, double size, Color c, bool bold = false)
        {
            return LabelPlacer.Text(s, size * ls, c, bold);
        }

        // spicchio di corona circolare tra i raggi r0 e r1 (px) e gli azimut az0-az1 (gradi)
        static StreamGeometry Wedge(Point c, double r0, double r1, double az0, double az1)
        {
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext gc = g.Open())
            {
                int n = Math.Max(2, (int)Math.Ceiling((az1 - az0) / 1.5));
                for (int i = 0; i <= n; i++)
                {
                    double a = (az0 + (az1 - az0) * i / n) * Astro.D2R;
                    Point p = new Point(c.X + r1 * Math.Sin(a), c.Y - r1 * Math.Cos(a));
                    if (i == 0) gc.BeginFigure(p, true); else gc.LineTo(p);
                }
                for (int i = n; i >= 0; i--)
                {
                    double a = (az0 + (az1 - az0) * i / n) * Astro.D2R;
                    gc.LineTo(new Point(c.X + r0 * Math.Sin(a), c.Y - r0 * Math.Cos(a)));
                }
                gc.EndFigure(true);
            }
            return g;
        }

        // linee della mappa in km attorno a GRAVES (x verso est, y verso sud); oltre limitKm la linea si interrompe
        StreamGeometry Project(List<System.Drawing.PointF[]> lines, double limitKm)
        {
            if (lines == null || lines.Count == 0) return null;
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext gc = g.Open())
            {
                foreach (System.Drawing.PointF[] line in lines)
                {
                    if (line == null) continue;
                    bool open = false;
                    foreach (System.Drawing.PointF p in line)
                    {
                        LatLon ll = new LatLon(p.Y, p.X);
                        double d = Astro.DistanceKm(Graves, ll);
                        if (d > limitKm)
                        {
                            if (open) { gc.EndFigure(false); open = false; }
                            continue;
                        }
                        double a = Astro.Bearing(Graves, ll) * Astro.D2R;
                        Point q = new Point(d * Math.Sin(a), -d * Math.Cos(a));
                        if (!open) { gc.BeginFigure(q, false); open = true; }
                        else gc.LineTo(q);
                    }
                    if (open) gc.EndFigure(false);
                }
            }
            return g;
        }

        void EnsureMap(double limitKm)
        {
            bool same = Math.Abs(limitKm - geoLimit) < 1;
            if (!same || !ReferenceEquals(coastFor, MapLines) || coastCount != (MapLines == null ? -1 : MapLines.Count))
            {
                coastGeo = Project(MapLines, limitKm);
                coastFor = MapLines; coastCount = MapLines == null ? -1 : MapLines.Count;
            }
            if (!same || !ReferenceEquals(borderFor, MapBorders) || borderCount != (MapBorders == null ? -1 : MapBorders.Count))
            {
                borderGeo = Project(MapBorders, limitKm);
                borderFor = MapBorders; borderCount = MapBorders == null ? -1 : MapBorders.Count;
            }
            geoLimit = limitKm;
        }

        public override void Render(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            ctx.DrawRectangle(Pal.B(Pal.Panel), null, new Rect(0, 0, W, H));
            ls = LS;
            FormattedText probe = Ft("N", 12, Pal.Fore, true);
            double pad = probe.Height + 8;
            double R = Math.Min(W, H) / 2 - pad;
            if (R < 40) return;
            Point c = new Point(W / 2, H / 2);

            // corona del fascio alla quota delle scie: da 25° di elevazione (raggio interno) all'orizzonte (raggio esterno)
            double layer = LayerKm > 0 ? LayerKm : 90;
            double rInKm = MsGeometry.LayerRangeKm(layer, GravesBeam.BeamElevDeg, GravesBeam.AltKm);
            double rOutKm = MsGeometry.LayerRangeKm(layer, 0, GravesBeam.AltKm);
            double maxKm = rOutKm * 1.08;
            if (HasMe) maxKm = Math.Max(maxKm, Astro.DistanceKm(Graves, Me) * 1.08);
            foreach (GravesMark m in Marks) maxKm = Math.Max(maxKm, Astro.DistanceKm(Graves, m.Pos) * 1.08);
            maxKm = Math.Min(6000, Math.Ceiling(maxKm / 250) * 250);
            double k = R / maxKm;   // px per km
            Func<LatLon, Point> P = ll =>
            {
                double d = Astro.DistanceKm(Graves, ll) * k;
                double a = Astro.Bearing(Graves, ll) * Astro.D2R;
                return new Point(c.X + d * Math.Sin(a), c.Y - d * Math.Cos(a));
            };

            Rect disc = new Rect(c.X - R, c.Y - R, 2 * R, 2 * R);
            ctx.DrawEllipse(Pal.B(SkyPal.Back), null, c, R, R);   // cielo scuro fisso
            using (ctx.PushGeometryClip(new EllipseGeometry(disc)))
            {
                // settori e passi (il passo illuminato adesso e' piu' chiaro, quello dell'hot spot in giallo)
                for (int s = 0; s < 4; s++)
                {
                    Color col = GravesBeam.SectorColor(s);
                    for (int st = 0; st < GravesBeam.Steps; st++)
                    {
                        double a0 = GravesBeam.AzMin + s * GravesBeam.SectorDeg + st * GravesBeam.StepDeg;
                        bool act = st == ActiveStep;
                        bool hot = s == HotSector && st == HotStep;
                        Color fill = hot ? Pal.A(act ? 235 : 150, SkyPal.Yellow) : Pal.A(act ? 125 : 42, col);
                        ctx.DrawGeometry(Pal.B(fill), Pal.P(Pal.A(80, col), 0.6), Wedge(c, rInKm * k, rOutKm * k, a0, a0 + GravesBeam.StepDeg));
                    }
                    // bordo del settore fino al cerchio esterno (settore anche oltre l'orizzonte)
                    double sa = (GravesBeam.AzMin + s * GravesBeam.SectorDeg) * Astro.D2R;
                    ctx.DrawLine(Pal.P(Pal.A(120, col), 1), c, new Point(c.X + R * Math.Sin(sa), c.Y - R * Math.Cos(sa)));
                }
                double ea = GravesBeam.AzMax * Astro.D2R;
                ctx.DrawLine(Pal.P(Pal.A(120, GravesBeam.SectorColor(3)), 1), c, new Point(c.X + R * Math.Sin(ea), c.Y - R * Math.Cos(ea)));

                // coste e confini
                EnsureMap(maxKm * 1.45);
                using (ctx.PushTransform(Matrix.CreateScale(k, k) * Matrix.CreateTranslation(c.X, c.Y)))
                {
                    if (borderGeo != null) ctx.DrawGeometry(null, Pal.P(Pal.A(70, SkyPal.Silver), 0.7 / k), borderGeo);
                    if (coastGeo != null) ctx.DrawGeometry(null, Pal.P(Pal.A(140, SkyPal.Silver), 0.9 / k), coastGeo);
                }

                // cerchi delle distanze
                double ringStep = maxKm <= 1500 ? 250 : maxKm <= 3000 ? 500 : 1000;
                for (double rk = ringStep; rk < maxKm; rk += ringStep)
                    ctx.DrawEllipse(null, Pal.P(Pal.A(45, SkyPal.Fore), 0.8), c, rk * k, rk * k);
                ctx.DrawLine(Pal.P(Pal.A(40, SkyPal.Fore), 0.8), new Point(c.X, c.Y - R), new Point(c.X, c.Y + R));
                ctx.DrawLine(Pal.P(Pal.A(40, SkyPal.Fore), 0.8), new Point(c.X - R, c.Y), new Point(c.X + R, c.Y));
                // limiti del fascio alla quota delle scie
                ctx.DrawEllipse(null, Pal.P(Pal.A(170, SkyPal.Yellow), 1, Pal.Dash), c, rOutKm * k, rOutKm * k);
                ctx.DrawEllipse(null, Pal.P(Pal.A(120, SkyPal.Yellow), 1, Pal.Dash), c, rInKm * k, rInKm * k);

                // zone PREV della tratta GRAVES -> stazione
                double wmax = 0;
                foreach (PrevSpot p in Prev) wmax = Math.Max(wmax, p.W);
                if (wmax > 0)
                    foreach (PrevSpot p in Prev)
                    {
                        double rel = p.W / wmax;
                        Color pc = p.Left ? SkyPal.Orange : SkyPal.Pink;
                        double rr = 1.5 + 2.5 * rel;
                        ctx.DrawEllipse(Pal.B(Pal.A((int)(25 + 140 * rel), pc)), null, P(p.Pos), rr, rr);
                    }

                // tratta GRAVES -> stazione (retta: in questa proiezione i cerchi massimi dal centro sono rette)
                if (HasMe) ctx.DrawLine(Pal.P(Pal.A(200, SkyPal.Green), 1.4, Pal.Dash), c, P(Me));
                // dalla stazione verso l'hot spot consigliato (cerchio massimo campionato)
                if (HasMe)
                    foreach (GravesMark m in Marks)
                    {
                        if (!m.Main) continue;
                        double d = Astro.DistanceKm(Me, m.Pos), b = Astro.Bearing(Me, m.Pos);
                        Point prev = P(Me);
                        for (int i = 1; i <= 24; i++)
                        {
                            Point q = P(Astro.Destination(Me, b, d * i / 24));
                            ctx.DrawLine(Pal.P(Pal.A(220, m.Color), 1.4, Pal.Dash), prev, q);
                            prev = q;
                        }
                    }
            }
            ctx.DrawEllipse(null, Pal.P(Pal.Border, 1.2), c, R, R);

            // simboli dei punti
            ctx.DrawEllipse(Pal.B(SkyPal.Yellow), Pal.P(SkyPal.Deep, 1), c, 4.5, 4.5);
            Point pMe = HasMe ? P(Me) : c;
            if (HasMe) ctx.DrawEllipse(Pal.B(SkyPal.Green), Pal.P(SkyPal.Deep, 1), pMe, 5, 5);
            List<Point> mp = new List<Point>();
            foreach (GravesMark m in Marks)
            {
                Point q = P(m.Pos);
                mp.Add(q);
                ctx.DrawEllipse(Pal.B(m.Color), Pal.P(SkyPal.Deep, 1), q, m.Main ? 6 : 4.5, m.Main ? 6 : 4.5);
            }

            // ---- etichette senza sovrapposizioni ----
            LabelPlacer lp = new LabelPlacer(new Rect(0, 0, W, H));
            lp.BlockPoint(c, 6);
            if (HasMe) lp.BlockPoint(pMe, 7);
            foreach (Point q in mp) lp.BlockPoint(q, 7);
            // punti cardinali fuori dal cerchio
            Color cc = Pal.Fore;
            FormattedText fn = Ft(Lang.T("N"), 12, cc, true), fe = Ft(Lang.T("E"), 12, cc, true), fs = Ft(Lang.T("S"), 12, cc, true), fo = Ft(Lang.T("O"), 12, cc, true);
            lp.Put(ctx, fn, LabelPlacer.At(fn, c.X, c.Y - R - fn.Height / 2 - 2), false);
            lp.Put(ctx, fs, LabelPlacer.At(fs, c.X, c.Y + R + fs.Height / 2 + 2), false);
            lp.Put(ctx, fe, LabelPlacer.At(fe, c.X + R + fe.Width / 2 + 6, c.Y), false);
            lp.Put(ctx, fo, LabelPlacer.At(fo, c.X - R - fo.Width / 2 - 6, c.Y), false);
            // lettere dei settori al centro della corona
            for (int s = 0; s < 4; s++)
            {
                double a = (GravesBeam.AzMin + (s + 0.5) * GravesBeam.SectorDeg) * Astro.D2R;
                double rr = (rInKm + rOutKm) / 2 * k;
                Point sp = new Point(c.X + rr * Math.Sin(a), c.Y - rr * Math.Cos(a));
                FormattedText ft = Ft(GravesBeam.Sectors[s], 15, GravesBeam.SectorColor(s), true);
                lp.Put(ctx, ft, LabelPlacer.At(ft, sp.X, sp.Y), true);
            }
            // nomi dei punti
            lp.Place(ctx, Ft("GRAVES", 11.3, SkyPal.Yellow, true), c, 0, false, false, SkyPal.Yellow);
            if (HasMe) lp.Place(ctx, Ft(MeLabel, 11.3, SkyPal.Green, true), pMe, 0, false, false, SkyPal.Green);
            for (int i = 0; i < Marks.Count; i++)
                lp.Place(ctx, Ft(Marks[i].Label, 11.3, Marks[i].Color, true), mp[i], Marks[i].Main ? 90 : -90, false, false, Marks[i].Color);
            // distanze dei cerchi (lungo il nord, facoltative) e limiti del fascio
            double rs = maxKm <= 1500 ? 250 : maxKm <= 3000 ? 500 : 1000;
            for (double rk = rs; rk < maxKm; rk += rs)
                lp.Place(ctx, Ft(rk.ToString("F0") + " km", 9.5, SkyPal.Muted), new Point(c.X, c.Y - rk * k), 45, false, true, SkyPal.Muted, true, 1);
            double ah = 315 * Astro.D2R;
            lp.Place(ctx, Ft(Lang.T("orizzonte del fascio"), 9.5, SkyPal.Yellow), new Point(c.X + rOutKm * k * Math.Sin(ah), c.Y - rOutKm * k * Math.Cos(ah)), -45, false, true, SkyPal.Yellow, true, 3);
        }
    }

    // Ricostruzione cronologica: 24 ore della tratta GRAVES -> stazione e dettaglio dei prossimi secondi del fascio
    public class GravesTimeline : Control
    {
        public double[] Index = new double[24];   // indice orario della tratta (somma ZHR x efficienza)
        public string[] Best = new string[24];    // sigla dello sciame piu' favorevole per ora
        public bool Valid;
        public double NowHour = -1;               // ora attuale (frazione) se il giorno mostrato e' oggi
        public double WindowFrac = 0.3;           // finestra utile: ore con indice >= 30 % del massimo del giorno
        public int Sector = -1, HotStep = -1;     // settore e passo che illuminano l'hot spot consigliato
        public double StepSec = 0.8, TrailSec = 0.3;
        public DateTime NowUtc = DateTime.UtcNow;
        public string Message = "";

        double ls = 1.0;
        double LS { get { return Math.Max(1.0, Math.Min(UiScale.Font, Bounds.Height / 230)); } }

        static readonly Color[] ShowerPal = { SkyPal.Yellow, SkyPal.Azure, SkyPal.Pink, SkyPal.Green, SkyPal.Orange, SkyPal.Violet };   // fissi: testo scuro sopra

        public GravesTimeline()
        {
            ClipToBounds = true;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            UiScale.Changed += OnScaleChanged;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            UiScale.Changed -= OnScaleChanged;
        }

        void OnScaleChanged() { InvalidateMeasure(); InvalidateVisual(); }

        FormattedText Ft(string s, double size, Color c, bool bold = false)
        {
            return LabelPlacer.Text(s, size * ls, c, bold);
        }

        static Color ShowerColor(string code)
        {
            int h = 0;
            foreach (char ch in code) h = h * 31 + ch;
            return ShowerPal[(h & 0x7fffffff) % ShowerPal.Length];
        }

        public override void Render(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            ctx.DrawRectangle(Pal.B(Pal.Panel), null, new Rect(0, 0, W, H));
            ls = LS;
            string[] names = { Lang.T("Efficienza tratta GRAVES → te"), Lang.T("Sciame più favorevole"), Lang.T("Finestra utile") };
            FormattedText[] rn = new FormattedText[3];
            double lw = 0;
            for (int i = 0; i < 3; i++) { rn[i] = Ft(names[i], 11.3, i == 2 ? Pal.Fore : Pal.Muted, i == 2); lw = Math.Max(lw, rn[i].Width); }
            lw = Math.Min(lw + 14, W * 0.35);
            for (int i = 0; i < 3; i++) rn[i].MaxTextWidth = Math.Max(30, lw - 8);
            double lh = Ft("0", 11.3, Pal.Fore).Height;
            double x0 = lw, x1 = W - 12, pw = x1 - x0;
            if (pw < 100) return;
            Func<double, double> X = h => x0 + h / 24.0 * pw;

            double yTop = 4 + lh + 4;   // spazio per la scritta "adesso"
            double y = yTop;
            double[] rowH = { Math.Max(rn[0].Height, lh * 1.8) + 8, Math.Max(rn[1].Height, lh) + 8, Math.Max(rn[2].Height, lh) + 8 };
            double[] rowY = new double[3];
            for (int r = 0; r < 3; r++)
            {
                rowY[r] = y;
                ctx.DrawText(rn[r], new Point(4, y + (rowH[r] - rn[r].Height) / 2));
                ctx.DrawRectangle(Pal.B(Pal.Panel3), null, new Rect(x0, y + 2, pw, rowH[r] - 4), 4, 4);
                y += rowH[r] + 4;
            }
            double max = 0; int hmax = 0;
            if (Valid && Index != null)
                for (int h = 0; h < 24 && h < Index.Length; h++) if (Index[h] > max) { max = Index[h]; hmax = h; }
            if (!Valid)
                ctx.DrawText(Ft(string.IsNullOrEmpty(Message) ? Lang.T("tratta GRAVES → te non calcolata") : Message, 10.5, Pal.Muted), new Point(x0 + 8, rowY[0] + 6));
            else if (max <= 0)
                ctx.DrawText(Ft(Lang.T("nessuno sciame utile in questo giorno"), 10.5, Pal.Muted), new Point(x0 + 8, rowY[0] + 6));
            else
            {
                // riga 1: barre dell'indice orario
                double bh = rowH[0] - 6;
                for (int h = 0; h < 24 && h < Index.Length; h++)
                {
                    double f = Index[h] / max;
                    if (f <= 0) continue;
                    ctx.DrawRectangle(Pal.B(Pal.A(210, Pal.Azure)), null, new Rect(X(h) + 1, rowY[0] + 3 + bh * (1 - f), Math.Max(1, pw / 24 - 2), bh * f), 2, 2);
                }
                FormattedText mt = Ft(Lang.F("max Σ {0:F1} alle {1:00}:30", max, hmax), 9.5, Pal.Fore);
                double mx = X(hmax + 1) + 4;
                if (mx + mt.Width > x1) mx = X(hmax) - mt.Width - 4;
                ctx.DrawRectangle(Pal.B(Pal.A(220, Pal.Panel)), null, new Rect(mx - 3, rowY[0] + 3, mt.Width + 6, mt.Height), 4, 4);
                ctx.DrawText(mt, new Point(mx, rowY[0] + 3));

                // riga 2: sciame piu' favorevole (ore consecutive con lo stesso sciame unite)
                int h0 = 0;
                while (h0 < 24)
                {
                    string code = Best != null && h0 < Best.Length ? Best[h0] : null;
                    int h1 = h0 + 1;
                    while (h1 < 24 && Best != null && h1 < Best.Length && Best[h1] == code) h1++;
                    if (!string.IsNullOrEmpty(code))
                    {
                        Rect sr = new Rect(X(h0) + 1, rowY[1] + 3, X(h1) - X(h0) - 2, rowH[1] - 6);
                        ctx.DrawRectangle(Pal.B(Pal.A(185, ShowerColor(code))), null, sr, 3, 3);
                        FormattedText ct = Ft(code, 10.5, SkyPal.Deep, true);
                        if (ct.Width + 4 < sr.Width) ctx.DrawText(ct, new Point(sr.X + (sr.Width - ct.Width) / 2, sr.Y + (sr.Height - ct.Height) / 2));
                    }
                    h0 = h1;
                }

                // riga 3: finestra utile
                h0 = 0;
                while (h0 < 24)
                {
                    if (Index[h0] < WindowFrac * max) { h0++; continue; }
                    int h1 = h0 + 1;
                    while (h1 < 24 && Index[h1] >= WindowFrac * max) h1++;
                    Rect wr = new Rect(X(h0) + 1, rowY[2] + 3, X(h1) - X(h0) - 2, rowH[2] - 6);
                    ctx.DrawRectangle(Pal.B(Pal.Rgb(22, 163, 74)), null, wr, 3, 3);
                    Color wf = Pal.Rgb(255, 255, 255);   // testo bianco sulla barra verde, in tutti gli schemi
                    FormattedText wt = Ft(Lang.F("{0:00}:00 → {1:00}:00 UTC · {2} h", h0, h1, h1 - h0), 10.5, wf, true);
                    if (wt.Width + 6 >= wr.Width) wt = Ft(Lang.F("{0} h", h1 - h0), 10.5, wf, true);
                    if (wt.Width + 6 < wr.Width) ctx.DrawText(wt, new Point(wr.X + 4, wr.Y + (wr.Height - wt.Height) / 2));
                    h0 = h1;
                }
            }
            // ore
            for (int h = 0; h <= 24; h += 3)
            {
                FormattedText t = Ft(h.ToString("00"), 9.5, Pal.Muted);
                ctx.DrawText(t, new Point(Math.Min(x1 - t.Width, Math.Max(0, X(h) - t.Width / 2)), y));
            }
            double yAxis = y;
            y += lh + 6;
            // linea "adesso"
            if (NowHour >= 0 && NowHour <= 24)
            {
                double xn = X(NowHour);
                ctx.DrawLine(Pal.P(Pal.Red, 2), new Point(xn, yTop - 2), new Point(xn, yAxis - 2));
                FormattedText nt = Ft(Lang.F("adesso {0}", NowUtc.ToString("HH:mm", CultureInfo.InvariantCulture)), 10, Pal.Red, true);
                double nx = xn + 3;
                if (nx + nt.Width > W - 2) nx = xn - nt.Width - 3;
                ctx.DrawText(nt, new Point(nx, 3));
            }

            // ---- dettaglio dei prossimi secondi: passi del settore che contiene l'hot spot ----
            y += 4;
            double step = StepSec > 0 ? StepSec : 0.8;
            double cyc = GravesBeam.Steps * step;
            double span = Math.Max(10.0, cyc + step);   // almeno un ciclo intero piu' un passo
            string letter = Sector >= 0 && Sector < 4 ? GravesBeam.Sectors[Sector] : "";
            FormattedText dt = Sector >= 0
                ? Ft(Lang.F("Dettaglio dei prossimi {0:F0} s: passi del settore {1} ({2:F1} s ciascuno) e passo che illumina la zona utile", span, letter, step), 11.3, Pal.Fore, true)
                : Ft(Lang.F("Dettaglio dei prossimi {0:F0} s: la zona utile non è in nessun passo del fascio", span), 11.3, Pal.Fore, true);
            dt.MaxTextWidth = Math.Max(50, W - 8);
            ctx.DrawText(dt, new Point(4, y));
            y += dt.Height + 6;
            if (Sector < 0) return;

            double ph = GravesBeam.Phase(NowUtc, step);
            int act = GravesBeam.ActiveStep(NowUtc, step);
            double inStep = ph - act * step;
            DateTime t0 = NowUtc.AddSeconds(-inStep);   // inizio del passo in corso
            Func<double, double> XS = s => x0 + s / span * pw;
            FormattedText sl = Ft(Lang.T("passi"), 10.5, Pal.Muted);
            double boxH = lh + 8;
            ctx.DrawText(sl, new Point(4, y + (boxH - sl.Height) / 2));
            int nb = (int)Math.Ceiling(span / step);
            Color secCol = GravesBeam.SectorColor(Sector);
            List<double> hotStarts = new List<double>();
            for (int i = 0; i < nb; i++)
            {
                double s0 = i * step, s1 = Math.Min(span, s0 + step);
                int st = (act + i) % GravesBeam.Steps;
                bool hot = st == HotStep;
                if (hot) hotStarts.Add(s0);
                Rect br = new Rect(XS(s0) + 1, y, Math.Max(1, XS(s1) - XS(s0) - 2), boxH);
                ctx.DrawRectangle(Pal.B(hot ? SkyPal.Yellow : Pal.A(80, secCol)), null, br, 3, 3);
                string lab = letter + (st + 1).ToString(CultureInfo.InvariantCulture);
                FormattedText bt = Ft(hot ? Lang.F("{0} · zona", lab) : lab, 10.5, hot ? SkyPal.Deep : Pal.Fore, hot);
                if (bt.Width + 4 > br.Width) bt = Ft(lab, 10.5, hot ? SkyPal.Deep : Pal.Fore, hot);
                if (bt.Width + 2 < br.Width) ctx.DrawText(bt, new Point(br.X + (br.Width - bt.Width) / 2, br.Y + (br.Height - bt.Height) / 2));
            }
            double xnow = XS(inStep);
            ctx.DrawLine(Pal.P(Pal.Red, 2), new Point(xnow, y - 4), new Point(xnow, y + boxH + 4));
            y += boxH + 5;

            // eco possibile: dal passo che illumina la zona fino alla fine del passo piu' la durata della scia
            FormattedText el = Ft(Lang.T("eco"), 10.5, Pal.Muted);
            ctx.DrawText(el, new Point(4, y));
            bool labDone = false;
            foreach (double s0 in hotStarts)
            {
                double s1 = Math.Min(span, s0 + step + TrailSec);
                Rect er = new Rect(XS(s0) + 1, y + 1, Math.Max(1, XS(s1) - XS(s0) - 2), lh * 0.8);
                ctx.DrawRectangle(Pal.B(Pal.Rgb(22, 163, 74)), null, er, 3, 3);
                if (labDone) continue;
                FormattedText et = Ft(Lang.F("eco possibile (+{0:F1} s di scia)", TrailSec), 10.5, Pal.Green, true);
                double ex = er.Right + 4;
                if (ex + et.Width > W - 2) ex = er.X - et.Width - 4;
                if (ex >= x0) { ctx.DrawText(et, new Point(ex, y + 1 + (lh * 0.8 - et.Height) / 2)); labDone = true; }
            }
            y += lh + 4;
            // tempi (secondi del minuto) all'inizio dei passi, diradati se non c'e' spazio
            double lastR = -1e9;
            for (int i = 0; i <= nb; i++)
            {
                double s0 = i * step;
                if (s0 > span + 1e-6) break;
                FormattedText tl = Ft(GravesBeam.SecLabel(t0.AddSeconds(s0)), 9.5, Pal.Muted);
                double tx = Math.Min(x1 - tl.Width, Math.Max(x0, XS(s0) - tl.Width / 2));
                if (tx <= lastR + 6) continue;
                ctx.DrawText(tl, new Point(tx, y));
                lastR = tx + tl.Width;
            }
        }
    }
}
