// YA-MS - volta celeste con mappa d'Europa, Hot A/B, miglior puntamento e legenda dei colori
//
// Due scale radiali sovrapposte, con lo stesso azimut:
//  - cielo: zenit al centro, orizzonte sul bordo (cerchi = elevazione del radiante);
//  - mappa: proiezione azimutale equidistante centrata sulla stazione, bordo = MapRadiusKm.
// Vista dall'alto (E a destra) come una mappa e come il rotore d'antenna; con la vista dal basso
// (E a sinistra, come swan.ms) la mappa non viene disegnata perche' risulterebbe specchiata.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Platform;

namespace YAMS
{
    // Punto di riflessione previsto (zona PREV in pianta): posizione alla quota delle scie e peso ZHR x contributo
    public class PrevSpot
    {
        public LatLon Pos;
        public double W;
        public bool Left;   // lato A (sinistra della tratta)
    }

    public class SkyView : Control
    {
        // scala del carattere per le scritte disegnate: quella scelta con il cursore, ma non oltre
        // una misura proporzionata al disegno (su una vista piccola le scritte enormi coprirebbero tutto)
        double LS { get { return Math.Max(1.0, Math.Min(UiScale.Font, Math.Min(Bounds.Width, Bounds.Height) / 2 / 210)); } }

        public PathModel Model;
        public LatLon Station;
        public List<RadiantMark> Marks = new List<RadiantMark>();
        public string Selected;
        public bool ShowHeat = false, ShowHot = true, ShowMap = true, TopView = true;
        public double AntennaAz = 0, HpbwDeg = 0;   // lobo d'antenna (HPBW = 0: non disegnato)
        public double MapRadiusKm = 2600;
        public List<System.Drawing.PointF[]> MapLines = new List<System.Drawing.PointF[]>();     // coste (lon, lat)
        public List<System.Drawing.PointF[]> MapBorders = new List<System.Drawing.PointF[]>();   // confini (lon, lat)
        public string Caption = "";
        public bool DrawCaption = true;   // false: didascalia e scala mostrate dalla finestra sotto la mappa
        // pianta: senza radianti e senza cerchi di elevazione, con la scala dei gradi; cielo piccolo: senza legenda
        public bool ShowRadiants = true, ShowSkyRings = true, ShowLegend = true, ShowDial = false;
        public List<PrevSpot> Prev = new List<PrevSpot>();   // zone PREV (punti di riflessione piu' probabili)

        WriteableBitmap heat; PathModel heatFor; bool heatTop;

        // colori di sistema usati dal vecchio programma
        static readonly Color Gainsboro = Color.FromRgb(220, 220, 220);
        static readonly Color White = Color.FromRgb(255, 255, 255);
        static readonly Color Wheat = Color.FromRgb(245, 222, 179);
        static readonly Color Cyan = Color.FromRgb(0, 255, 255);
        static readonly Color LightCyan = Color.FromRgb(224, 255, 255);

        // dimensioni dei caratteri (1 pt WinForms ~ 1.33 px)
        const double F7 = 9.3, F75 = 10, F8 = 10.7, F85 = 11.3, F9 = 12, F12 = 16;

        bool legendCollapsed;    // legenda ripiegata (clic sull'intestazione)
        Rect legendHeader;       // zona cliccabile dell'intestazione della legenda

        public SkyView()
        {
            ClipToBounds = true;
        }

        // ridisegno quando cambia la scala del carattere (iscrizione solo mentre il controllo e' visibile nell'albero)
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

        // clic sull'intestazione della legenda: ripiega / riapre la colonna
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (!ShowLegend) return;
            Point p = e.GetPosition(this);
            if (legendHeader.Width > 0 && legendHeader.Contains(p))
            {
                legendCollapsed = !legendCollapsed;
                e.Handled = true;
                InvalidateVisual();
            }
        }

        // larghezza della colonna della legenda (a destra, fuori dalla mappa)
        double LegendWidth()
        {
            if (!ShowLegend) return 0;
            if (legendCollapsed) return Ft(Lang.T("Legenda") + " ▸", F9, Pal.Fore, FontWeight.Bold).Width + 24;
            return Math.Min(420, 150 * LS);
        }

        // larghezza disponibile per la mappa / volta celeste
        double MapAreaWidth()
        {
            return ShowLegend ? Math.Max(20, Bounds.Width - LegendWidth() - 10) : Bounds.Width;
        }

        public void InvalidateHeat() { heatFor = null; InvalidateVisual(); }

        void Geom(out double cx, out double cy, out double rad)
        {
            double H = Bounds.Height, S = LS;
            double w = MapAreaWidth();   // a destra la colonna della legenda
            double s = Math.Min(w, H);
            // margine per la scala dei gradi o per le lettere N/E/S/W, che crescono con il carattere
            double pad = ShowDial ? (S <= 2 ? 30 : 26) + F9 * S : (ShowLegend ? 18 + F12 * S : 10 + F9 * S);
            rad = s / 2.0 - pad; if (rad < 20) rad = 20;
            cx = w / 2.0; cy = H / 2.0;
        }

        double Sgn { get { return TopView ? 1.0 : -1.0; } }

        Point SkyXY(double az, double el, double cx, double cy, double rad)
        {
            double r = rad * (90.0 - el) / 90.0, a = az * Astro.D2R;
            return new Point(cx + Sgn * r * Math.Sin(a), cy - r * Math.Cos(a));
        }

        Point MapXY(LatLon p, double cx, double cy, double rad)
        {
            double d = Astro.DistanceKm(Station, p), b = Astro.Bearing(Station, p) * Astro.D2R;
            double r = rad * d / MapRadiusKm;
            return new Point(cx + Sgn * r * Math.Sin(b), cy - r * Math.Cos(b));
        }

        public static Color HeatColor(double eff)
        {
            // 0 % = trasparente (resta il blu del cielo), circa 50 % = verde acqua, 100 % = giallo
            double t = Math.Max(0, Math.Min(1, eff / 100.0));
            int a = (int)(170 * Math.Pow(t, 0.8));
            int r = (int)(255 * Math.Max(0, (t - 0.45) / 0.55));
            int g = (int)(120 + 135 * t);
            int b = (int)(200 * (1 - t));
            return Color.FromArgb((byte)Math.Min(255, a), (byte)Math.Min(255, r), (byte)Math.Min(255, g), (byte)Math.Max(0, b));
        }

        void BuildHeat(int size)
        {
            if (heat != null) heat.Dispose();
            heat = new WriteableBitmap(new PixelSize(size, size), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);
            heatFor = Model; heatTop = TopView;
            int[] buf = new int[size * size];
            if (Model != null && Model.Valid)
            {
                double c = size / 2.0, rad = size / 2.0;
                for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                    {
                        double dx = x + 0.5 - c, dy = y + 0.5 - c;
                        double rr = Math.Sqrt(dx * dx + dy * dy);
                        if (rr > rad) continue;
                        double el = 90.0 - 90.0 * rr / rad;
                        double az = Astro.Norm360(Math.Atan2(Sgn * dx, -dy) / Astro.D2R);
                        double lf;
                        double eff = Model.Efficiency(Astro.FromAltAz(Station, az, el), out lf);
                        if (eff > 1)
                        {
                            Color hc = HeatColor(eff);
                            int a = hc.A;
                            int r = hc.R * a / 255, g = hc.G * a / 255, b = hc.B * a / 255;   // alfa premoltiplicato
                            buf[y * size + x] = (a << 24) | (r << 16) | (g << 8) | b;
                        }
                    }
            }
            using (ILockedFramebuffer fb = heat.Lock())
            {
                for (int y = 0; y < size; y++)
                    Marshal.Copy(buf, y * size, fb.Address + y * fb.RowBytes, size);
            }
        }

        // ---------- testo (dimensioni moltiplicate per la scala del carattere) ----------
        static FormattedText Ft(string s, double size, Color c, FontWeight w = FontWeight.Normal)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, FontStyle.Normal, w), size * drawScale, new ImmutableSolidColorBrush(c));
        }

        static void Txt(DrawingContext ctx, string s, double size, Color c, double x, double y, FontWeight w = FontWeight.Normal)
        {
            ctx.DrawText(Ft(s, size, c, w), new Point(x, y));
        }

        static IPen RoundPen(Color c, double w)
        {
            return new ImmutablePen(new ImmutableSolidColorBrush(c), w, null, PenLineCap.Round, PenLineJoin.Round);
        }

        // direzione sullo schermo (0 = su, 90 = destra) di un azimut: con la vista dal basso E e W si scambiano
        double ScreenDir(double az) { return Sgn * az; }

        static Point PA(double screenDeg, double r, double cx, double cy)
        {
            double a = screenDeg * Astro.D2R;
            return new Point(cx + r * Math.Sin(a), cy - r * Math.Cos(a));
        }

        // ---------- etichette: raccolte durante il disegno, piazzate alla fine senza sovrapposizioni ----------
        sealed class Lab
        {
            public string Text; public double Size; public Color Col; public bool Bold;
            public Point Anchor; public double Pref; public bool Side; public bool Optional;
            public int Pri, Seq, MaxRing = 6; public bool Halo = true;
            public bool Flow; public double FlowAz;          // scritta dello sciame lungo la sua linea
            public bool Fixed; public Point FixedAt;        // posizione esatta (angolo in alto a sinistra del testo), solo se libera
        }

        List<Lab> labs = new List<Lab>();
        LabelPlacer lp;

        Lab AddLab(int pri, string text, double size, Color col, bool bold, Point anchor, double pref, bool side = false, bool optional = false)
        {
            Lab l = new Lab { Pri = pri, Seq = labs.Count, Text = text, Size = size, Col = col, Bold = bold, Anchor = anchor, Pref = pref, Side = side, Optional = optional };
            labs.Add(l);
            return l;
        }

        void PlaceLabels(DrawingContext ctx, double cx, double cy, double rad)
        {
            labs.Sort((a, b) => a.Pri != b.Pri ? a.Pri.CompareTo(b.Pri) : a.Seq.CompareTo(b.Seq));
            foreach (Lab l in labs)
            {
                FormattedText ft = Ft(l.Text, l.Size, l.Col, l.Bold ? FontWeight.Bold : FontWeight.Normal);
                if (l.Fixed)
                {
                    Size bs = LabelPlacer.BoxSize(ft);
                    Rect r = new Rect(l.FixedAt.X - 4, l.FixedAt.Y - 1, bs.Width, bs.Height);
                    if (lp.Free(r)) lp.Put(ctx, ft, r, l.Halo);
                    continue;
                }
                if (l.Flow && PlaceFlow(ctx, ft, l.FlowAz, cx, cy, rad)) continue;
                lp.Place(ctx, ft, l.Anchor, l.Pref, l.Side, l.Optional, l.Col, l.Halo, l.MaxRing);
            }
            labs.Clear();
        }

        // Scritta dello sciame: orizzontale, lungo la sua linea (dal bordo verso il centro, ai due capi e sui due lati),
        // scostata dalla linea inclinata quanto basta per non attraversarla
        bool PlaceFlow(DrawingContext ctx, FormattedText ft, double az, double cx, double cy, double rad)
        {
            Size bs = LabelPlacer.BoxSize(ft);
            double sa = ScreenDir(az) * Astro.D2R;
            double d = bs.Width / 2 * Math.Abs(Math.Cos(sa)) + bs.Height / 2 * Math.Abs(Math.Sin(sa)) + 5;
            foreach (double rr in new double[] { 0.85, 0.75, 0.65, 0.55, 0.45, 0.35, 0.25 })
                foreach (double end in new double[] { az, az + 180 })
                    foreach (double side in new double[] { 90, -90 })
                    {
                        Point an = PA(ScreenDir(end), rad * rr, cx, cy);
                        double e = (ScreenDir(end) + side) * Astro.D2R;
                        Rect r = LabelPlacer.At(ft, an.X + Math.Sin(e) * d, an.Y - Math.Cos(e) * d);
                        if (lp.Free(r)) { lp.Put(ctx, ft, r, true); return true; }
                    }
            return false;
        }

        // scala usata dai metodi statici che creano i testi (impostata all'inizio di ogni disegno)
        static double drawScale = 1.0;

        public override void Render(DrawingContext ctx)
        {
            drawScale = LS;
            double W = Bounds.Width, H = Bounds.Height;
            ctx.DrawRectangle(Pal.B(Pal.Back), null, new Rect(0, 0, W, H));
            if (W < 10 || H < 10) return;
            double S = LS;
            double cx, cy, rad; Geom(out cx, out cy, out rad);
            Rect dome = new Rect(cx - rad, cy - rad, 2 * rad, 2 * rad);
            bool map = ShowMap && TopView;
            lp = new LabelPlacer(new Rect(0, 0, MapAreaWidth(), H));
            labs.Clear();

            RadialGradientBrush pb = new RadialGradientBrush();
            pb.GradientStops.Add(new GradientStop(map ? Color.FromRgb(22, 36, 110) : Color.FromRgb(32, 52, 160), 0));
            pb.GradientStops.Add(new GradientStop(Color.FromRgb(8, 12, 48), 1));
            ctx.DrawEllipse(pb, null, dome);
            if (map)
            {
                using (ctx.PushGeometryClip(new EllipseGeometry(dome)))
                    DrawMap(ctx, cx, cy, rad);
                // scritte dei cerchi delle distanze (le meno importanti: omesse se non c'e' posto)
                int step = S <= 2 ? 500 : 1000;
                Color lc = Pal.Argb(190, 255, 200, 120);
                for (int km = step; km < MapRadiusKm; km += step)
                {
                    double r = rad * km / MapRadiusKm;
                    AddLab(9, km + " km", F75, lc, false, new Point(cx + 26, cy + r), 0, false, true);
                }
            }
            if (ShowHeat && Model != null && Model.Valid)
            {
                if (heatFor != Model || heat == null || heatTop != TopView) BuildHeat(240);
                if (map)
                {
                    using (ctx.PushOpacity(0.72))
                        ctx.DrawImage(heat, dome);
                }
                else ctx.DrawImage(heat, dome);
            }

            IPen ring = Pal.P(Pal.Argb(130, 220, 220, 230), 1);
            IPen dash = Pal.P(Pal.Argb(80, 220, 220, 230), 1, Pal.Dash);
            Color tc = Pal.Argb(200, 230, 230, 230);
            if (!map || ShowSkyRings)
                for (int el = 0; el < 90; el += 10)
                {
                    double r = rad * (90 - el) / 90.0;
                    ctx.DrawEllipse(null, el % 20 == 0 ? ring : dash, new Point(cx, cy), r, r);
                    if (el % 20 == 0 && el > 0)
                    {
                        Lab l = AddLab(6, el + "°", ShowLegend || map ? F8 : F7, tc, false, new Point(cx, cy - r), 0, false, true);
                        l.Fixed = true; l.Halo = false; l.FixedAt = new Point(cx + 2, cy - r - 1);
                    }
                }
            if (map && ShowDial) DrawDial(ctx, cx, cy, rad);
            ctx.DrawLine(dash, new Point(cx - rad, cy), new Point(cx + rad, cy));
            ctx.DrawLine(dash, new Point(cx, cy - rad), new Point(cx, cy + rad));
            if (map && DrawCaption)
            {
                FormattedText mt = Ft(Lang.F("mappa: bordo = {0} km dalla stazione", MapRadiusKm.ToString("F0")), F8, tc);
                ctx.DrawText(mt, new Point(6, 6));   // in alto a sinistra: non copre la didascalia
                lp.Block(new Rect(4, 4, mt.Width + 4, mt.Height + 4));
            }

            if (!(map && ShowDial))
            {
                double fs = ShowLegend || map ? F12 : F9;
                Color oc = Color.FromRgb(240, 90, 40);
                FormattedText n = Ft("N", fs, oc), s = Ft("S", fs, oc);
                FormattedText w = Ft(TopView ? "W" : "E", fs, oc), e = Ft(TopView ? "E" : "W", fs, oc);
                Point pn = new Point(cx - n.Width / 2, cy - rad - n.Height - 4), ps = new Point(cx - s.Width / 2, cy + rad + 4);
                Point pw = new Point(cx - rad - w.Width - 8, cy - w.Height / 2), pe = new Point(cx + rad + 8, cy - e.Height / 2);
                ctx.DrawText(n, pn); ctx.DrawText(s, ps); ctx.DrawText(w, pw); ctx.DrawText(e, pe);
                lp.Block(new Rect(pn, new Size(n.Width, n.Height))); lp.Block(new Rect(ps, new Size(s.Width, s.Height)));
                lp.Block(new Rect(pw, new Size(w.Width, w.Height))); lp.Block(new Rect(pe, new Size(e.Width, e.Height)));
            }

            DrawLobe(ctx, cx, cy, rad, map);
            if (map) DrawPrev(ctx, cx, cy, rad);
            if (Model != null && Model.Valid) DrawPath(ctx, cx, cy, rad, map);
            if (map) DrawFlow(ctx, cx, cy, rad); else DrawBestPointing(ctx, cx, cy, rad);
            if (ShowRadiants || !map) DrawRadiants(ctx, cx, cy, rad);
            PlaceLabels(ctx, cx, cy, rad);
            if (ShowLegend) DrawSymbolLegend(ctx, W, H);
            if (DrawCaption && !string.IsNullOrEmpty(Caption))
            {
                FormattedText ft = Ft(Caption, F85, Pal.Silver);
                ft.MaxTextWidth = Math.Max(100, Bounds.Width - 12);   // va a capo se la finestra e' stretta
                ctx.DrawText(ft, new Point(6, H - ft.Height - 4));
            }
        }

        void DrawMap(DrawingContext ctx, double cx, double cy, double rad)
        {
            EnsureMapGeometry(cx, cy, rad);
            IPen coast = Pal.P(Pal.Argb(200, 120, 170, 200), 1.1);
            IPen border = Pal.P(Pal.Argb(120, 120, 140, 170), 0.8, Pal.Dot);
            if (borderGeo != null) ctx.DrawGeometry(null, border, borderGeo);
            if (coastGeo != null) ctx.DrawGeometry(null, coast, coastGeo);
            IPen dr = Pal.P(Pal.Argb(60, 255, 200, 120), 1, Pal.DashDot);
            for (int km = 500; km < MapRadiusKm; km += 500)
            {
                double r = rad * km / MapRadiusKm;
                ctx.DrawEllipse(null, dr, new Point(cx, cy), r, r);   // le scritte "km" sono piazzate con le etichette
            }
        }

        // proiezione delle linee della mappa in coordinate normalizzate (raggio 1 = MapRadiusKm), rifatta solo se cambia
        // la stazione (o la vista, o i dati); la geometria in pixel si rifa' solo se cambiano anche centro o raggio
        List<Point[]> projCoast = new List<Point[]>(), projBorders = new List<Point[]>();
        LatLon projFor; bool projTop; int projCount = -1; double projRadiusKm;
        List<System.Drawing.PointF[]> projLinesRef, projBordersRef;
        StreamGeometry coastGeo, borderGeo;
        double geoCx = double.NaN, geoCy, geoRad;

        List<Point[]> Project(List<System.Drawing.PointF[]> lines)
        {
            List<Point[]> outl = new List<Point[]>();
            if (lines == null) return outl;
            double maxDeg = (MapRadiusKm + 300) / 111.0;
            List<Point> cur = new List<Point>();
            foreach (System.Drawing.PointF[] line in lines)
            {
                cur.Clear();
                if (line == null) continue;
                for (int i = 0; i < line.Length; i++)
                {
                    LatLon q = new LatLon(line[i].Y, line[i].X);
                    bool ok = Math.Abs(q.Lat - Station.Lat) <= maxDeg;
                    double d = ok ? Astro.DistanceKm(Station, q) : 0;
                    if (!ok || d > MapRadiusKm + 200) { if (cur.Count > 1) outl.Add(cur.ToArray()); cur.Clear(); continue; }
                    double r = d / MapRadiusKm, b = Astro.Bearing(Station, q) * Astro.D2R;
                    cur.Add(new Point(Sgn * r * Math.Sin(b), -r * Math.Cos(b)));
                }
                if (cur.Count > 1) outl.Add(cur.ToArray());
            }
            return outl;
        }

        public void InvalidateMap() { projCount = -1; coastGeo = null; borderGeo = null; InvalidateVisual(); }

        void EnsureMapGeometry(double cx, double cy, double rad)
        {
            int cnt = (MapLines != null ? MapLines.Count : 0) + (MapBorders != null ? MapBorders.Count : 0);
            if (projCount != cnt || projFor.Lat != Station.Lat || projFor.Lon != Station.Lon || projTop != TopView
                || projRadiusKm != MapRadiusKm || !ReferenceEquals(projLinesRef, MapLines) || !ReferenceEquals(projBordersRef, MapBorders))
            {
                projCoast = Project(MapLines); projBorders = Project(MapBorders);
                projFor = Station; projTop = TopView; projCount = cnt; projRadiusKm = MapRadiusKm;
                projLinesRef = MapLines; projBordersRef = MapBorders;
                coastGeo = null; borderGeo = null;
            }
            if (coastGeo == null || borderGeo == null || geoCx != cx || geoCy != cy || geoRad != rad)
            {
                coastGeo = BuildGeo(projCoast, cx, cy, rad);
                borderGeo = BuildGeo(projBorders, cx, cy, rad);
                geoCx = cx; geoCy = cy; geoRad = rad;
            }
        }

        static StreamGeometry BuildGeo(List<Point[]> lines, double cx, double cy, double rad)
        {
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext c = g.Open())
            {
                foreach (Point[] line in lines)
                {
                    if (line.Length < 2) continue;
                    c.BeginFigure(new Point(cx + rad * line[0].X, cy + rad * line[0].Y), false);
                    for (int i = 1; i < line.Length; i++) c.LineTo(new Point(cx + rad * line[i].X, cy + rad * line[i].Y));
                    c.EndFigure(false);
                }
            }
            return g;
        }

        static Color HotColor(HotPoint h)
        {
            // rosso = consigliato dall'efficienza, grigio = gia' nel lobo, rosa = fuori dal lobo
            return h.Recommended ? Pal.Red : h.InLobe ? Pal.HotGrey : Pal.HotPink;
        }

        void DrawPath(DrawingContext ctx, double cx, double cy, double rad, bool map)
        {
            IPen pp = Pal.P(Pal.A(230, Pal.Green), 2);
            IBrush gb = Pal.B(Pal.Green);
            double left = ScreenDir(Model.BearingHomeToDx - 90);   // a sinistra della tratta (lato A)
            if (map)
            {
                Point pd = MapXY(Model.Dx, cx, cy, rad);
                ctx.DrawLine(pp, new Point(cx, cy), pd);
                ctx.DrawEllipse(gb, null, pd, 4, 4);
                lp.BlockLine(new Point(cx, cy), pd);
                lp.BlockPoint(pd, 6);
                AddLab(0, "DX", F9, Pal.Green, true, pd, left);
                Point pm = MapXY(Model.Mid, cx, cy, rad);
                ctx.DrawEllipse(null, Pal.P(Pal.Green, 1), pm, 3, 3);
                DrawCommonZone(ctx, cx, cy, rad, left);
            }
            else
            {
                Point p = SkyXY(Model.BearingHomeToDx, 0, cx, cy, rad);
                ctx.DrawLine(pp, new Point(cx, cy), p);
                lp.BlockLine(new Point(cx, cy), p);
            }
            ctx.DrawEllipse(Pal.B(White), null, new Point(cx, cy), 3, 3);
            lp.BlockPoint(new Point(cx, cy), 6);
            if (!ShowHot) return;
            HotPoint[] hs = { Model.HotA, Model.HotB };
            foreach (HotPoint h in hs)
            {
                if (h == null) continue;
                Color hc = HotColor(h);
                IPen hp = Pal.P(Pal.A(220, hc), 1.5, Pal.Dash);
                Point rim = SkyXY(h.BearingFromHome, 0, cx, cy, rad);
                ctx.DrawLine(hp, new Point(cx, cy), rim);
                string lab = (h.Label ?? "").Replace("Hot ", "");
                bool isA = h == Model.HotA;
                if (map)
                {
                    Point p = MapXY(h.Pos, cx, cy, rad);
                    ctx.DrawEllipse(Pal.B(hc), null, p, 5, 5);
                    lp.BlockPoint(p, 6);
                    // Hot A a sinistra della tratta, Hot B a destra (vista dalla stazione verso il DX)
                    AddLab(h.Recommended ? 1 : 2, isA ? Lang.T("Hot A") : Lang.T("Hot B"), F9, hc, true, p, isA ? left : left + 180, true);
                }
                else
                {
                    // lettera appena fuori dal bordo, lungo la direzione
                    AddLab(0, lab, F85, hc, true, rim, ScreenDir(h.BearingFromHome));
                }
            }
        }

        // Zona dello strato E vista da entrambe le stazioni (articolo): due cerchi di raggio pari alla portata
        // alla quota delle scie, attorno alla stazione e al corrispondente; i loro incroci sono i Limiti A e B
        void DrawCommonZone(DrawingContext ctx, double cx, double cy, double rad, double left)
        {
            if (Model.HotRadiusKm <= 0) return;
            Color lc = Color.FromRgb(150, 150, 210);
            IPen zp = Pal.P(Pal.A(110, lc), 1, Pal.Dot);
            foreach (LatLon c in new LatLon[] { Station, Model.Dx })
            {
                StreamGeometry g = new StreamGeometry();
                using (StreamGeometryContext gc = g.Open())
                {
                    gc.BeginFigure(MapXY(Astro.Destination(c, 0, Model.HotRadiusKm), cx, cy, rad), false);
                    for (int i = 1; i <= 72; i++) gc.LineTo(MapXY(Astro.Destination(c, i * 5, Model.HotRadiusKm), cx, cy, rad));
                    gc.EndFigure(false);
                }
                ctx.DrawGeometry(null, zp, g);
            }
            IPen lpen = Pal.P(lc, 1.3);
            foreach (HotPoint h in new HotPoint[] { Model.LimitA, Model.LimitB })
            {
                if (h == null) continue;
                Point p = MapXY(h.Pos, cx, cy, rad);
                ctx.DrawEllipse(null, lpen, p, 4, 4);
                lp.BlockPoint(p, 5);
                AddLab(8, h == Model.LimitA ? Lang.T("lim. A") : Lang.T("lim. B"), F75, lc, false, p, h == Model.LimitA ? left : left + 180);
            }
        }

        // Lobo principale dell'antenna: settore di +-HPBW/2 attorno al puntamento
        void DrawLobe(DrawingContext ctx, double cx, double cy, double rad, bool map)
        {
            if (HpbwDeg <= 0) return;
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext gc = g.Open())
            {
                gc.BeginFigure(new Point(cx, cy), true);
                for (double a = -HpbwDeg / 2; a <= HpbwDeg / 2 + 0.01; a += Math.Max(0.5, HpbwDeg / 40))
                    gc.LineTo(SkyXY(AntennaAz + a, 0, cx, cy, rad));
                gc.EndFigure(true);
            }
            ctx.DrawGeometry(Pal.B(Pal.Argb(45, 255, 240, 200)), Pal.P(Pal.Argb(140, 255, 240, 200), 1), g);
            Point t = PA(ScreenDir(AntennaAz), rad * 0.93, cx, cy);
            AddLab(map ? 7 : 4, Lang.F("lobo {0}°", HpbwDeg.ToString("F0")), F8, Wheat, false, t, ScreenDir(AntennaAz));
        }

        // Da un punto del controllo alla posizione geografica (proiezione azimutale equidistante inversa)
        public bool MapToLatLon(Point pt, out LatLon p)
        {
            p = new LatLon();
            double cx, cy, rad; Geom(out cx, out cy, out rad);
            double dx = pt.X - cx, dy = pt.Y - cy;
            double r = Math.Sqrt(dx * dx + dy * dy) / rad;
            if (r > 1 || !TopView) return false;
            double brg = Astro.Norm360(Math.Atan2(Sgn * dx, -dy) / Astro.D2R);
            p = Astro.Destination(Station, brg, r * MapRadiusKm);
            return true;
        }

        // Miglior puntamento: ortogonale alla direzione di arrivo dello sciame selezionato
        void DrawBestPointing(DrawingContext ctx, double cx, double cy, double rad)
        {
            RadiantMark m = null;
            if (Marks != null) foreach (RadiantMark x in Marks) if (x.Code == Selected) m = x;
            if (m == null || m.El <= 0) return;
            Point a = SkyXY(m.Az + 90, 0, cx, cy, rad), b = SkyXY(m.Az - 90, 0, cx, cy, rad);
            IPen p = Pal.P(Pal.A(230, Pal.Orange), 2.5);
            IPen w = Pal.P(Pal.A(70, Pal.Orange), 1);
            Color tc = Color.FromRgb(255, 170, 60);
            ctx.DrawLine(p, a, b);
            lp.BlockLine(a, b);
            // tolleranza +-45° (SM6FHZ: +-40-50°)
            foreach (double off in new double[] { 45, -45 })
            {
                Point a2 = SkyXY(m.Az + 90 + off, 0, cx, cy, rad), b2 = SkyXY(m.Az - 90 + off, 0, cx, cy, rad);
                ctx.DrawLine(w, a2, b2);
            }
            AddLab(1, Astro.Norm360(m.Az + 90).ToString("F0") + "°", F85, tc, true, a, ScreenDir(m.Az + 90));
            AddLab(1, Astro.Norm360(m.Az - 90).ToString("F0") + "°", F85, tc, true, b, ScreenDir(m.Az - 90));
            // direzione di arrivo dello sciame: linea a puntini con freccia verso il centro
            Point r = SkyXY(m.Az, 0, cx, cy, rad);
            Color dc = Pal.A(150, Pal.Orange);
            ctx.DrawLine(Pal.P(dc, 1, Pal.Dot), r, new Point(cx, cy));
            double vx = cx - r.X, vy = cy - r.Y, len = Math.Sqrt(vx * vx + vy * vy);
            if (len > 1)
            {
                vx /= len; vy /= len;
                Point tip = new Point(cx, cy);
                Point bl = new Point(cx - 9 * vx + 4 * vy, cy - 9 * vy - 4 * vx);
                Point br = new Point(cx - 9 * vx - 4 * vy, cy - 9 * vy + 4 * vx);
                StreamGeometry g = new StreamGeometry();
                using (StreamGeometryContext gc = g.Open())
                {
                    gc.BeginFigure(tip, true); gc.LineTo(bl); gc.LineTo(br); gc.EndFigure(true);
                }
                ctx.DrawGeometry(Pal.B(dc), null, g);
            }
        }

        // Scala dei gradi attorno al bordo della mappa (come il rotore d'antenna)
        void DrawDial(DrawingContext ctx, double cx, double cy, double rad)
        {
            IPen tk = Pal.P(Pal.Argb(150, 200, 200, 210), 1);
            IPen rim = Pal.P(Pal.Argb(170, 200, 200, 210), 1.5);
            Color oc = Color.FromRgb(240, 120, 60);
            int every = LS <= 2 ? 30 : 60;   // caratteri grandi: meno scritte
            double lr = rad + 14 + 0.75 * F9 * LS;
            ctx.DrawEllipse(null, rim, new Point(cx, cy), rad + 3, rad + 3);
            for (int a = 0; a < 360; a += 5)
            {
                double ar = a * Astro.D2R;
                double l = a % 30 == 0 ? 9 : (a % 10 == 0 ? 6 : 3);
                double sx = Sgn * Math.Sin(ar), sy = -Math.Cos(ar);
                ctx.DrawLine(tk, new Point(cx + (rad + 3) * sx, cy + (rad + 3) * sy), new Point(cx + (rad + 3 + l) * sx, cy + (rad + 3 + l) * sy));
                if (a % every == 0)
                {
                    FormattedText ft = Ft(a.ToString(), F9, oc);
                    lp.Put(ctx, ft, LabelPlacer.At(ft, cx + lr * sx, cy + lr * sy), false);   // ostacolo fisso per le etichette
                }
            }
        }

        // Zone PREV: dove cadono le scie piu' probabili (per ciascun lato della tratta un'ellisse
        // con centro e assi dati dalla media e dalla dispersione pesata dei punti di riflessione)
        void DrawPrev(DrawingContext ctx, double cx, double cy, double rad)
        {
            if (Prev == null || Prev.Count == 0) return;
            double tot = 0; foreach (PrevSpot sp in Prev) tot += sp.W;
            if (tot <= 0) return;
            foreach (bool left in new bool[] { true, false })
            {
                double sw = 0, mx = 0, my = 0;
                List<Point> pts = new List<Point>(); List<double> ws = new List<double>();
                foreach (PrevSpot sp in Prev)
                {
                    if (sp.Left != left || sp.W <= 0) continue;
                    Point p = MapXY(sp.Pos, cx, cy, rad);
                    pts.Add(p); ws.Add(sp.W);
                    sw += sp.W; mx += sp.W * p.X; my += sp.W * p.Y;
                }
                if (sw < 0.05 * tot || pts.Count < 3) continue;
                mx /= sw; my /= sw;
                double sxx = 0, syy = 0, sxy = 0;
                for (int i = 0; i < pts.Count; i++)
                {
                    double dx = pts[i].X - mx, dy = pts[i].Y - my;
                    sxx += ws[i] * dx * dx; syy += ws[i] * dy * dy; sxy += ws[i] * dx * dy;
                }
                sxx /= sw; syy /= sw; sxy /= sw;
                // autovalori e direzione dell'asse maggiore della matrice di covarianza
                double tr = sxx + syy, det = sxx * syy - sxy * sxy;
                double disc = Math.Sqrt(Math.Max(0, tr * tr / 4 - det));
                double l1 = tr / 2 + disc, l2 = Math.Max(0, tr / 2 - disc);
                double ang = 0.5 * Math.Atan2(2 * sxy, sxx - syy);   // radianti
                double a1 = Math.Max(6, 2 * Math.Sqrt(l1)), a2 = Math.Max(5, 2 * Math.Sqrt(l2));
                using (ctx.PushTransform(Matrix.CreateRotation(ang) * Matrix.CreateTranslation(mx, my)))
                {
                    ctx.DrawEllipse(Pal.B(Pal.Argb(95, 235, 225, 190)), Pal.P(Pal.Argb(170, 245, 235, 200), 1.2), new Point(0, 0), a1, a2);
                }
                string lab = "PREV " + (left ? "A" : "B") + " " + (100 * sw / tot).ToString("F0") + "%";
                double side = Model != null && Model.Valid ? ScreenDir(Model.BearingHomeToDx - 90) : 270;
                AddLab(left ? 3 : 4, lab, F9, Color.FromRgb(250, 240, 210), true, new Point(mx, my), left ? side : side + 180, true);
            }
        }

        // Direzione di scorrimento dello sciame (linea gialla per la stazione, lungo l'azimut del radiante):
        // le riflessioni migliori stanno ai due lati, in direzione ortogonale (punti arancioni sul bordo)
        void DrawFlow(DrawingContext ctx, double cx, double cy, double rad)
        {
            RadiantMark m = FlowMark();
            if (m == null) return;
            Point a = SkyXY(m.Az, 0, cx, cy, rad), b = SkyXY(m.Az + 180, 0, cx, cy, rad);
            ctx.DrawLine(Pal.P(Pal.Argb(200, 120, 90, 20), 6), a, b);
            ctx.DrawLine(Pal.P(Pal.Argb(240, 255, 210, 90), 2.5), a, b);
            lp.BlockLine(a, b);
            Color fc = Color.FromRgb(255, 220, 110);
            Lab fl = AddLab(5, Lang.F("{0} (dal radiante, az. {1}°)", m.Code, m.Az.ToString("F0")), F9, fc, true, PA(ScreenDir(m.Az), rad * 0.80, cx, cy), ScreenDir(m.Az + 90));
            fl.Flow = true; fl.FlowAz = m.Az;
            IBrush ob = Pal.B(Pal.Orange);
            foreach (double off in new double[] { 90, -90 })
            {
                Point q = SkyXY(m.Az + off, 0, cx, cy, rad);
                ctx.DrawEllipse(ob, null, q, 5, 5);
                lp.BlockPoint(q, 6);
                AddLab(6, Astro.Norm360(m.Az + off).ToString("F0") + "°", F9, Pal.Orange, true, q, ScreenDir(m.Az + off + 180));
            }
        }

        // Sciame per la linea di scorrimento: quello selezionato, altrimenti il piu' favorevole (ZHR x efficienza)
        RadiantMark FlowMark()
        {
            RadiantMark best = null; double bs = 0;
            if (Marks == null) return null;
            foreach (RadiantMark x in Marks)
            {
                if (x.El <= 0) continue;
                if (x.Code == Selected) return x;
                double sc = x.Zhr * x.Eff;
                if (sc > bs) { bs = sc; best = x; }
            }
            return best;
        }

        void DrawRadiants(DrawingContext ctx, double cx, double cy, double rad)
        {
            if (Marks == null) return;
            IBrush cyan = Pal.B(Cyan);
            foreach (RadiantMark m in Marks)
            {
                if (m.Track == null) continue;
                bool sel = m.Code == Selected;
                IPen tp = RoundPen(sel ? Pal.A(240, Pal.Azure) : Pal.A(140, Pal.Azure), sel ? 2.2 : 1.3);
                StreamGeometry g = new StreamGeometry();
                List<Point> hours = new List<Point>(); List<int> hv = new List<int>();
                using (StreamGeometryContext gc = g.Open())
                {
                    bool open = false;
                    for (int i = 0; i < m.Track.Count; i++)
                    {
                        System.Drawing.PointF t = m.Track[i];
                        if (t.Y < 0) { if (open) gc.EndFigure(false); open = false; continue; }
                        Point p = SkyXY(t.X, t.Y, cx, cy, rad);
                        if (!open) { gc.BeginFigure(p, false); open = true; }
                        else gc.LineTo(p);
                        if (sel && m.TrackHours != null && i < m.TrackHours.Count && m.TrackHours[i] >= 0)
                        {
                            hours.Add(p); hv.Add(m.TrackHours[i]);
                        }
                    }
                    if (open) gc.EndFigure(false);
                }
                ctx.DrawGeometry(null, tp, g);
                for (int i = 0; i < hours.Count; i++)
                {
                    ctx.DrawEllipse(cyan, null, hours[i], 2, 2);
                    lp.BlockPoint(hours[i], 2);
                    Lab hl = AddLab(5, hv[i].ToString("00"), F7, LightCyan, false, hours[i], 45, false, true);
                    hl.MaxRing = 1;   // le ore restano vicine al loro punto, altrimenti si omettono
                }
            }
            IPen selPen = Pal.P(White, 1);
            List<RadiantMark> codes = new List<RadiantMark>(); List<Point> codePts = new List<Point>();
            foreach (RadiantMark m in Marks)
            {
                if (m.El < 0) continue;
                Point p = SkyXY(m.Az, m.El, cx, cy, rad);
                Color c;
                if (m.Zhr < 1) c = Color.FromRgb(130, 130, 130);
                else if (m.Eff >= 50 && m.Zhr >= 2) c = Pal.Yellow;
                else if (m.Eff >= 20) c = Pal.Orange;
                else c = Color.FromRgb(194, 65, 12);
                ctx.DrawEllipse(Pal.B(c), null, p, 7, 7);
                if (m.Code == Selected) ctx.DrawEllipse(null, selPen, p, 9, 9);
                lp.BlockPoint(p, 7);
                codes.Add(m); codePts.Add(p);
            }
            // sigle: prima quella selezionata, poi le piu' favorevoli (ZHR x efficienza)
            List<int> ord = new List<int>();
            for (int i = 0; i < codes.Count; i++) ord.Add(i);
            ord.Sort((x, y) =>
            {
                bool sx = codes[x].Code == Selected, sy = codes[y].Code == Selected;
                if (sx != sy) return sx ? -1 : 1;
                return (codes[y].Zhr * codes[y].Eff).CompareTo(codes[x].Zhr * codes[x].Eff);
            });
            foreach (int i in ord)
                AddLab(codes[i].Code == Selected ? 2 : 3, codes[i].Code, F85, White, true, codePts[i], 45);
        }

        // Legenda dei simboli: colonna a destra con riquadro arrotondato; clic sull'intestazione = ripiega / riapre
        void DrawSymbolLegend(DrawingContext ctx, double W, double H)
        {
            double lw = LegendWidth();
            double x0 = W - lw;
            Rect panel = new Rect(x0, 6, Math.Max(10, lw - 6), Math.Max(10, H - 12));
            ctx.DrawRectangle(Pal.B(Pal.Panel), Pal.P(Pal.Border, 1), panel, 10, 10);
            FormattedText hf = Ft(Lang.T("Legenda") + (legendCollapsed ? " ▸" : " ▾"), F9, Pal.Fore, FontWeight.Bold);
            ctx.DrawText(hf, new Point(x0 + 12, 14));
            if (legendCollapsed) { legendHeader = panel; return; }   // striscia stretta: tutta cliccabile
            legendHeader = new Rect(x0, 6, panel.Width, hf.Height + 16);
            double x = x0 + 12, yy = 14 + hf.Height + 10;
            using (ctx.PushClip(panel))
            {
                Item(ctx, x, ref yy, Pal.Green, Lang.T("tratta / DX"));
                Item(ctx, x, ref yy, Pal.Red, Lang.T("Hot consigliato"));
                Item(ctx, x, ref yy, Pal.HotPink, Lang.T("Hot fuori lobo"));
                Item(ctx, x, ref yy, Color.FromRgb(150, 150, 210), Lang.T("limiti zona comune"));
                Item(ctx, x, ref yy, Pal.HotGrey, Lang.T("Hot nel lobo"));
                Item(ctx, x, ref yy, Color.FromRgb(255, 240, 200), Lang.T("lobo antenna"));
                Item(ctx, x, ref yy, Color.FromRgb(255, 210, 90), Lang.T("scorrimento sciame"));
                Item(ctx, x, ref yy, Pal.Orange, Lang.T("miglior punt."));
                Item(ctx, x, ref yy, Color.FromRgb(235, 225, 190), Lang.T("zona PREV"));
                if (ShowRadiants || !(ShowMap && TopView))
                {
                    Item(ctx, x, ref yy, Pal.Azure, Lang.T("percorso rad."));
                    yy += 8;
                    FormattedText rt = Ft(Lang.T("Radiante:"), F75, Gainsboro, FontWeight.Bold);
                    ctx.DrawText(rt, new Point(x, yy)); yy += rt.Height + 6;
                    Item(ctx, x, ref yy, Pal.Yellow, Lang.T("effic. >= 50%"));
                    Item(ctx, x, ref yy, Pal.Orange, Lang.T("effic. >= 20%"));
                    Item(ctx, x, ref yy, Color.FromRgb(194, 65, 12), Lang.T("effic. < 20%"));
                    Item(ctx, x, ref yy, Color.FromRgb(130, 130, 130), Lang.T("quasi inattivo"));
                }
                yy += 8;
                string[] note = { Lang.T("Mappa:"), Lang.T("clic sinistro ="), Lang.T("corrispondente"), Lang.T("clic destro ="), Lang.T("la mia stazione") };
                foreach (string n in note)
                {
                    FormattedText nt = Ft(n, F75, Pal.Silver);
                    ctx.DrawText(nt, new Point(x, yy)); yy += Math.Max(16, nt.Height + 3);
                }
            }
        }

        static void Item(DrawingContext ctx, double x, ref double y, Color c, string text)
        {
            FormattedText ft = Ft(text, F75, Gainsboro);
            ctx.DrawEllipse(Pal.B(c), null, new Point(x + 4.5, y + ft.Height / 2), 4.5, 4.5);
            ctx.DrawText(ft, new Point(x + 13, y));
            y += Math.Max(18, ft.Height + 5);
        }

        public string HitTest(Point pt)
        {
            if (!ShowRadiants && ShowMap && TopView) return null;   // in pianta i radianti non sono disegnati
            if (Marks == null) return null;
            double cx, cy, rad; Geom(out cx, out cy, out rad);
            string best = null; double bd = 15;
            foreach (RadiantMark m in Marks)
            {
                if (m.El < 0) continue;
                Point p = SkyXY(m.Az, m.El, cx, cy, rad);
                double d = Math.Sqrt((p.X - pt.X) * (p.X - pt.X) + (p.Y - pt.Y) * (p.Y - pt.Y));
                if (d < bd) { bd = d; best = m.Code; }
            }
            return best;
        }
    }
}
