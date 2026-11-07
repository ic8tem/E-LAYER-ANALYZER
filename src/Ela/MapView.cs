// E-LAYER ANALYZER - mappa del mondo (proiezione equirettangolare) con zoom (rotella), spostamento (trascina),
// oppure proiezione azimutale equidistante centrata su una stazione (raggio in km impostato dallo zoom o da un cursore),
// celle colorate, linee di cerchio massimo, segnaposti e legenda. Coste e confini: Natural Earth (dominio pubblico).
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using YAMS;

namespace ELA
{
    public class MapCell { public double Lat0, Lon0, DLat, DLon; public Color Fill; public string Tip; }
    public class MapLine { public List<Point> Pts = new List<Point>(); public Color Color; public double Width = 1.5; public bool Dashed; }   // Point(X=lon, Y=lat)
    public class MapPoly { public List<Point> Pts = new List<Point>(); public Color Fill; public Color Stroke; public double Width; }   // poligono pieno, Point(X=lon, Y=lat)
    public class MapMarker { public double Lat, Lon, Radius = 4; public Color Fill, Stroke = Colors.Black; public string Label; public bool Square, Ring; public Color LabelColor = Pal.Fore; }

    public class MapView : Control
    {
        public static List<System.Drawing.PointF[]> Coast, Borders;
        public static event Action MapLoaded;
        public static void NotifyMapLoaded() { Action a = MapLoaded; if (a != null) a(); }

        public double CenterLon = 12, CenterLat = 45, Zoom = 1;   // Zoom 1 = tutta la larghezza mostra 360 gradi
        public double MinLat = -85, MaxLat = 85;
        public readonly List<MapCell> Cells = new List<MapCell>();
        public readonly List<MapLine> Lines = new List<MapLine>();
        public readonly List<MapPoly> Polys = new List<MapPoly>();
        public readonly List<MapMarker> Markers = new List<MapMarker>();
        public readonly List<KeyValuePair<string, Color>> Legend = new List<KeyValuePair<string, Color>>();
        public string Caption = "";
        public readonly List<string> Info = new List<string>();   // righe grandi in alto a sinistra (es. Azimut / Elevazione)
        public string LegendTitle = "";
        public bool ShowLocatorGrid = true;

        // proiezione azimutale equidistante: centro (stazione) e raggio del cerchio esterno in km
        public bool Azimuthal;
        public double AzLat = 45, AzLon = 12, RangeKm = 2500, MinRangeKm = 300, MaxRangeKm = 20000;
        public bool WheelZoom = true;               // nella vista azimutale la rotella cambia il raggio
        public event Action<double> RangeChanged;   // raggio cambiato con la rotella
        double Rpx { get { return Math.Max(20, Math.Min(Bounds.Width, Bounds.Height) / 2 - 16); } }
        double Kpx { get { return Rpx / Math.Max(1, RangeKm); } }   // pixel per km

        public event Action<double, double, bool> Clicked;   // lat, lon, tasto destro

        Point? dragStart; double dragLon0, dragLat0; bool dragged;
        Point hover = new Point(-1, -1);

        static Color Sea { get { return Themes.MapSea; } }
        static Color Land { get { return Themes.MapLand; } }
        static Color Border { get { return Themes.MapBorder; } }
        static Color GridC { get { return Themes.MapGrid; } }
        static Color Ov { get { return Themes.Overlay; } }

        public MapView()
        {
            ClipToBounds = true;
            SizeChanged += (s, e) => OnSized(e);
            MapLoaded += () => Avalonia.Threading.Dispatcher.UIThread.Post(InvalidateVisual);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) { base.OnAttachedToVisualTree(e); UiScale.Changed += InvalidateVisual; }
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) { base.OnDetachedFromVisualTree(e); UiScale.Changed -= InvalidateVisual; }

        // vista: lon/lat dei bordi e pixel per grado
        double Ppd { get { return Math.Max(0.1, Bounds.Width / 360.0 * Zoom); } }

        public void SetView(double lonW, double lonE, double latS, double latN)
        {
            CenterLon = (lonW + lonE) / 2; CenterLat = (latS + latN) / 2;
            double w = Math.Max(1, Bounds.Width > 1 ? Bounds.Width : 900), h = Math.Max(1, Bounds.Height > 1 ? Bounds.Height : 600);
            double zx = 360.0 / Math.Max(1, lonE - lonW);
            double zy = (h / w) * 360.0 / Math.Max(1, latN - latS);
            Zoom = Math.Max(0.8, Math.Min(zx, zy));
            pendingView = new double[] { lonW, lonE, latS, latN };
            InvalidateVisual();
        }
        double[] pendingView;

        void OnSized(SizeChangedEventArgs e)
        {
            if (pendingView != null && e.NewSize.Width > 10)
            {
                double[] v = pendingView; pendingView = null;
                SetView(v[0], v[1], v[2], v[3]); pendingView = null;
            }
        }

        public Point ToScreen(double lat, double lon)
        {
            if (Azimuthal)
            {
                double d = Geo.DistanceKm(AzLat, AzLon, lat, lon), az = Geo.Bearing(AzLat, AzLon, lat, lon) * Geo.D2R, k = Kpx;
                return new Point(Bounds.Width / 2 + d * k * Math.Sin(az), Bounds.Height / 2 - d * k * Math.Cos(az));
            }
            double p = Ppd;
            double dl = lon - CenterLon;
            if (dl > 180) dl -= 360; else if (dl < -180) dl += 360;
            return new Point(Bounds.Width / 2 + dl * p, Bounds.Height / 2 - (lat - CenterLat) * p);
        }

        public void ToLatLon(Point s, out double lat, out double lon)
        {
            if (Azimuthal)
            {
                double dx = s.X - Bounds.Width / 2, dy = s.Y - Bounds.Height / 2;
                double d = Math.Min(Math.PI * Geo.R - 1, Math.Sqrt(dx * dx + dy * dy) / Kpx);
                Geo.Destination(AzLat, AzLon, Geo.Norm360(Math.Atan2(dx, -dy) * Geo.R2D), d, out lat, out lon);
                return;
            }
            double p = Ppd;
            lon = Geo.Norm180(CenterLon + (s.X - Bounds.Width / 2) / p);
            lat = CenterLat - (s.Y - Bounds.Height / 2) / p;
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            Point pos = e.GetPosition(this);
            if (Azimuthal)
            {
                if (WheelZoom)
                {
                    RangeKm = Math.Max(MinRangeKm, Math.Min(MaxRangeKm, RangeKm * (e.Delta.Y > 0 ? 0.85 : 1 / 0.85)));
                    Action<double> rc = RangeChanged; if (rc != null) rc(RangeKm);
                    InvalidateVisual();
                }
                e.Handled = true;
                return;
            }
            double lat, lon; ToLatLon(pos, out lat, out lon);
            double f = e.Delta.Y > 0 ? 1.25 : 0.8;
            Zoom = Math.Max(0.8, Math.Min(60, Zoom * f));
            // il punto sotto il cursore resta fermo
            double p = Ppd;
            CenterLon = Geo.Norm180(lon - (pos.X - Bounds.Width / 2) / p);
            CenterLat = Math.Max(-85, Math.Min(85, lat + (pos.Y - Bounds.Height / 2) / p));
            InvalidateVisual();
            e.Handled = true;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            dragStart = e.GetPosition(this); dragLon0 = CenterLon; dragLat0 = CenterLat; dragged = false;
            e.Pointer.Capture(this);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            Point pos = e.GetPosition(this);
            hover = pos;
            if (dragStart.HasValue && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                Vector d = pos - dragStart.Value;
                if (Math.Abs(d.X) + Math.Abs(d.Y) > 4) dragged = true;
                if (dragged && !Azimuthal)
                {
                    CenterLon = Geo.Norm180(dragLon0 - d.X / Ppd);
                    CenterLat = Math.Max(-85, Math.Min(85, dragLat0 + d.Y / Ppd));
                }
            }
            InvalidateVisual();
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            e.Pointer.Capture(null);
            if (dragStart.HasValue && !dragged)
            {
                double lat, lon; ToLatLon(e.GetPosition(this), out lat, out lon);
                Action<double, double, bool> a = Clicked;
                if (a != null) a(lat, lon, e.InitialPressMouseButton == MouseButton.Right);
            }
            dragStart = null;
        }

        protected override void OnPointerExited(PointerEventArgs e) { base.OnPointerExited(e); hover = new Point(-1, -1); InvalidateVisual(); }

        static FormattedText Ft(string s, double size, Color c, FontWeight w = FontWeight.Normal)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily.Default, FontStyle.Normal, w), size * UiScale.Font, new SolidColorBrush(c));
        }

        void DrawPolyLines(DrawingContext ctx, List<System.Drawing.PointF[]> lines, IPen pen, bool fillLand)
        {
            if (lines == null) return;
            double W = Bounds.Width, H = Bounds.Height;
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext gc = g.Open())
            {
                foreach (System.Drawing.PointF[] pl in lines)
                {
                    bool open = false; Point prev = new Point();
                    for (int i = 0; i < pl.Length; i++)
                    {
                        Point p = ToScreen(pl[i].Y, pl[i].X);
                        bool jump = open && Jump(p, prev);
                        bool vis = p.X > -50 && p.X < W + 50 && p.Y > -50 && p.Y < H + 50;
                        if (!open || jump || (!vis && !(prev.X > -50 && prev.X < W + 50 && prev.Y > -50 && prev.Y < H + 50)))
                        {
                            if (open) gc.EndFigure(false);
                            gc.BeginFigure(p, false); open = true;
                        }
                        else gc.LineTo(p);
                        prev = p;
                    }
                    if (open) gc.EndFigure(false);
                }
            }
            ctx.DrawGeometry(null, pen, g);
        }

        // salto tra due punti consecutivi (bordo della mappa o antipodo nella vista azimutale)
        bool Jump(Point a, Point b)
        {
            if (Azimuthal) return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) > Rpx * 0.5 * Math.Max(1, RangeKm / 10000.0);
            return Math.Abs(a.X - b.X) > Bounds.Width / 2;
        }

        public override void Render(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            if (Azimuthal) { RenderAzimuthal(ctx); return; }
            ctx.FillRectangle(Pal.B(Sea), new Rect(0, 0, W, H));
            double p = Ppd;
            RenderBody(ctx);
        }

        void RenderBody(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            double p = Ppd;

            // reticolo Maidenhead: campi 20 x 10 gradi con le lettere grandi (come AuroReflex), quadrati 2 x 1 gradi con le cifre
            if (ShowLocatorGrid && !Azimuthal)
            {
                IPen gp = Pal.P(GridC, 1.2);
                bool squares = p * 2 > 26;
                IPen sp = Pal.P(Themes.MapGrid, 0.7);
                double lonL, latT, lonR, latB;
                ToLatLon(new Point(0, 0), out latT, out lonL); ToLatLon(new Point(W, H), out latB, out lonR);
                if (squares)
                {
                    for (int lon = -180; lon <= 180; lon += 2) { Point a = ToScreen(0, lon); if (a.X >= 0 && a.X <= W && lon % 20 != 0) ctx.DrawLine(sp, new Point(a.X, 0), new Point(a.X, H)); }
                    for (int lat = -90; lat <= 90; lat += 1) { Point a = ToScreen(lat, CenterLon); if (a.Y >= 0 && a.Y <= H && lat % 10 != 0) ctx.DrawLine(sp, new Point(0, a.Y), new Point(W, a.Y)); }
                    if (p * 2 > 34)
                        for (int lat = (int)Math.Floor(latB); lat <= (int)Math.Ceiling(latT); lat++)
                            for (double lon = Math.Floor(CenterLon - W / 2 / p / 2) * 2; lon <= CenterLon + W / 2 / p + 2; lon += 2)
                            {
                                string loc = Geo.LatLonToLoc(lat + 0.5, lon + 1, 4);
                                Point c = ToScreen(lat + 0.5, lon + 1);
                                if (c.X < 0 || c.X > W || c.Y < 0 || c.Y > H) continue;
                                FormattedText t = Ft(loc.Substring(2, 2), 8.5, Themes.MapSquare);
                                ctx.DrawText(t, new Point(c.X - t.Width / 2, c.Y - t.Height / 2));
                            }
                }
                for (int lon = -180; lon <= 180; lon += 20) { Point a = ToScreen(0, lon); if (a.X >= 0 && a.X <= W) ctx.DrawLine(gp, new Point(a.X, 0), new Point(a.X, H)); }
                for (int lat = -90; lat <= 90; lat += 10) { Point a = ToScreen(lat, CenterLon); if (a.Y >= 0 && a.Y <= H) ctx.DrawLine(gp, new Point(0, a.Y), new Point(W, a.Y)); }
                double fs = Math.Max(11, Math.Min(90, p * 4.2)) / UiScale.Font;
                for (int fx = 0; fx < 18; fx++)
                    for (int fy = 0; fy < 18; fy++)
                    {
                        Point c = ToScreen(-90 + fy * 10 + 5, -180 + fx * 20 + 10);
                        if (c.X < -60 || c.X > W + 60 || c.Y < -60 || c.Y > H + 60) continue;
                        FormattedText t = Ft("" + (char)('A' + fx) + (char)('A' + fy), fs, Themes.MapField, FontWeight.Bold);
                        ctx.DrawText(t, new Point(c.X - t.Width / 2, c.Y - t.Height / 2));
                    }
            }

            // celle colorate (sotto le coste)
            foreach (MapCell c in Cells)
            {
                if (Azimuthal)
                {
                    StreamGeometry cg = new StreamGeometry();
                    using (StreamGeometryContext gc = cg.Open())
                    {
                        gc.BeginFigure(ToScreen(c.Lat0, c.Lon0), true);
                        gc.LineTo(ToScreen(c.Lat0 + c.DLat, c.Lon0)); gc.LineTo(ToScreen(c.Lat0 + c.DLat, c.Lon0 + c.DLon)); gc.LineTo(ToScreen(c.Lat0, c.Lon0 + c.DLon));
                        gc.EndFigure(true);
                    }
                    ctx.DrawGeometry(Pal.B(c.Fill), null, cg);
                    continue;
                }
                Point a = ToScreen(c.Lat0 + c.DLat, c.Lon0), b = ToScreen(c.Lat0, c.Lon0 + c.DLon);
                if (b.X < a.X) continue;   // attraversa il bordo della vista
                if (b.X < 0 || a.X > W || b.Y < 0 || a.Y > H) continue;
                ctx.FillRectangle(Pal.B(c.Fill), new Rect(a, b));
            }

            // poligoni pieni (es. settori raggiunti e zona di skip)
            foreach (MapPoly pg in Polys)
            {
                if (pg.Pts.Count < 3) continue;
                StreamGeometry g = new StreamGeometry();
                bool bad = false;
                using (StreamGeometryContext gc = g.Open())
                {
                    Point first = ToScreen(pg.Pts[0].Y, pg.Pts[0].X), prev = first;
                    gc.BeginFigure(first, true);
                    for (int i = 1; i < pg.Pts.Count; i++)
                    {
                        Point q = ToScreen(pg.Pts[i].Y, pg.Pts[i].X);
                        if (Jump(q, prev)) bad = true;
                        gc.LineTo(q); prev = q;
                    }
                    gc.EndFigure(true);
                }
                if (!bad) ctx.DrawGeometry(Pal.B(pg.Fill), pg.Width > 0 ? Pal.P(pg.Stroke, pg.Width) : null, g);
            }

            DrawPolyLines(ctx, Borders, Pal.P(Border, 0.8), false);
            DrawPolyLines(ctx, Coast, Pal.P(Land, 1.1), false);

            // linee
            foreach (MapLine l in Lines)
            {
                if (l.Pts.Count < 2) continue;
                IPen pen = l.Dashed ? Pal.P(l.Color, l.Width, Pal.Dash) : Pal.P(l.Color, l.Width);
                Point prev = ToScreen(l.Pts[0].Y, l.Pts[0].X);
                for (int i = 1; i < l.Pts.Count; i++)
                {
                    Point q = ToScreen(l.Pts[i].Y, l.Pts[i].X);
                    if (!Jump(q, prev)) ctx.DrawLine(pen, prev, q);
                    prev = q;
                }
            }

            // segnaposti
            foreach (MapMarker m in Markers)
            {
                Point c = ToScreen(m.Lat, m.Lon);
                if (c.X < -10 || c.X > W + 10 || c.Y < -10 || c.Y > H + 10) continue;
                double r = m.Radius;
                if (m.Ring) ctx.DrawEllipse(null, Pal.P(m.Fill, 2), c, r, r);
                else if (m.Square) ctx.DrawRectangle(Pal.B(m.Fill), Pal.P(m.Stroke, 1), new Rect(c.X - r, c.Y - r, 2 * r, 2 * r));
                else ctx.DrawEllipse(Pal.B(m.Fill), Pal.P(m.Stroke, 1), c, r, r);
                if (!string.IsNullOrEmpty(m.Label))
                {
                    FormattedText t = Ft(m.Label, 11, m.LabelColor, FontWeight.SemiBold);
                    Rect tr = new Rect(c.X + r + 3, c.Y - t.Height / 2, t.Width + 4, t.Height);
                    ctx.FillRectangle(Pal.B(Ov), tr);
                    ctx.DrawText(t, new Point(tr.X + 2, tr.Y));
                }
            }

            // legenda in basso a sinistra
            double y = H - 8;
            for (int i = Legend.Count - 1; i >= 0; i--)
            {
                FormattedText t = Ft(Legend[i].Key, 11.5, Pal.Fore);
                y -= t.Height + 3;
                ctx.FillRectangle(Pal.B(Ov), new Rect(6, y - 1, t.Width + 30, t.Height + 2));
                ctx.FillRectangle(Pal.B(Legend[i].Value), new Rect(10, y + t.Height / 2 - 5, 12, 10));
                ctx.DrawText(t, new Point(28, y));
            }
            if (LegendTitle.Length > 0 && Legend.Count > 0)
            {
                FormattedText t = Ft(LegendTitle, 11.5, Pal.Yellow, FontWeight.SemiBold);
                y -= t.Height + 3;
                ctx.FillRectangle(Pal.B(Ov), new Rect(6, y - 1, t.Width + 12, t.Height + 2));
                ctx.DrawText(t, new Point(10, y));
            }
            double iy = string.IsNullOrEmpty(Caption) ? 8 : 34;
            foreach (string line in Info)
            {
                FormattedText t = Ft(line, 16, Pal.Fore, FontWeight.SemiBold);
                ctx.FillRectangle(Pal.B(Ov), new Rect(6, iy - 2, t.Width + 12, t.Height + 4));
                ctx.DrawText(t, new Point(12, iy));
                iy += t.Height + 4;
            }
            if (!string.IsNullOrEmpty(Caption))
            {
                FormattedText t = Ft(Caption, 12, Pal.Yellow, FontWeight.SemiBold);
                ctx.FillRectangle(Pal.B(Ov), new Rect(6, 6, t.Width + 12, t.Height + 6));
                ctx.DrawText(t, new Point(12, 9));
            }

            // coordinate del cursore in alto a destra
            if (hover.X >= 0)
            {
                double la, lo; ToLatLon(hover, out la, out lo);
                string s = la.ToString("F2", CultureInfo.CurrentCulture) + "°  " + lo.ToString("F2", CultureInfo.CurrentCulture) + "°  " + Geo.LatLonToLoc(la, lo, 6);
                if (Azimuthal)
                    s += "   " + Lang.F("{0:F0} km, az {1:F0}° dal centro", Geo.DistanceKm(AzLat, AzLon, la, lo), Geo.Bearing(AzLat, AzLon, la, lo));
                FormattedText t = Ft(s, 12, Pal.Azure, FontWeight.SemiBold);
                ctx.FillRectangle(Pal.B(Ov), new Rect(W - t.Width - 18, 6, t.Width + 12, t.Height + 6));
                ctx.DrawText(t, new Point(W - t.Width - 12, 9));
            }
            if (Coast == null || Coast.Count == 0)
            {
                FormattedText t = Ft(Lang.T("Carico la mappa (Natural Earth)..."), 13, Pal.Muted);
                ctx.DrawText(t, new Point(W / 2 - t.Width / 2, H / 2));
            }
        }

        // passo "tondo" degli anelli di distanza (km) per il raggio mostrato
        public static double RingStep(double range)
        {
            double[] steps = { 50, 100, 200, 250, 500, 1000, 2000, 2500, 5000 };
            foreach (double st in steps) if (range / st <= 6) return st;
            return 5000;
        }

        void GeoPolyline(DrawingContext ctx, IPen pen, IList<Point> lonLat)
        {
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext gc = g.Open())
            {
                bool open = false; Point prev = new Point();
                foreach (Point ll in lonLat)
                {
                    Point q = ToScreen(ll.Y, ll.X);
                    if (!open || Jump(q, prev)) { if (open) gc.EndFigure(false); gc.BeginFigure(q, false); open = true; }
                    else gc.LineTo(q);
                    prev = q;
                }
                if (open) gc.EndFigure(false);
            }
            ctx.DrawGeometry(null, pen, g);
        }

        // vista azimutale equidistante: disco centrato sulla stazione, anelli di distanza in km, azimut sul bordo
        void RenderAzimuthal(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height, R = Rpx;
            Point C = new Point(W / 2, H / 2);
            ctx.FillRectangle(Pal.B(Pal.Back), new Rect(0, 0, W, H));
            ctx.DrawEllipse(Pal.B(Sea), null, C, R, R);
            using (ctx.PushGeometryClip(new EllipseGeometry(new Rect(C.X - R, C.Y - R, 2 * R, 2 * R))))
            {
                if (ShowLocatorGrid)
                {
                    // campi Maidenhead (20 x 10 gradi) e, da vicino, quadrati (2 x 1 grado)
                    bool squares = RangeKm <= 3000;
                    double span = Math.Min(180, RangeKm / 111.0 * 1.6 + 2);
                    IPen sp = Pal.P(GridC, 0.6), gp = Pal.P(GridC, 1.2);
                    if (squares)
                    {
                        double la0 = Math.Max(-89, Math.Floor(AzLat - span)), la1 = Math.Min(89, Math.Ceiling(AzLat + span));
                        double lspan = Math.Min(180, span / Math.Max(0.15, Math.Cos(Math.Min(80, Math.Abs(AzLat) + span) * Geo.D2R)));
                        double lo0 = Math.Floor((AzLon - lspan) / 2) * 2, lo1 = AzLon + lspan;
                        for (double lo = lo0; lo <= lo1; lo += 2)
                        {
                            if (((int)Math.Round(Geo.Norm180(lo)) % 20 + 20) % 20 == 0) continue;
                            List<Point> m = new List<Point>(); for (double la = la0; la <= la1; la += 0.5) m.Add(new Point(lo, la));
                            GeoPolyline(ctx, sp, m);
                        }
                        for (double la = la0; la <= la1; la += 1)
                        {
                            if (((int)la % 10 + 10) % 10 == 0) continue;
                            List<Point> m = new List<Point>(); for (double lo = lo0; lo <= lo1; lo += 1) m.Add(new Point(lo, la));
                            GeoPolyline(ctx, sp, m);
                        }
                        if (RangeKm <= 1600)
                            for (double la = la0; la < la1; la += 1)
                                for (double lo = lo0; lo < lo1; lo += 2)
                                {
                                    Point c = ToScreen(la + 0.5, lo + 1);
                                    if (c.X < 0 || c.X > W || c.Y < 0 || c.Y > H) continue;
                                    FormattedText t = Ft(Geo.LatLonToLoc(la + 0.5, lo + 1, 4).Substring(2, 2), 8.5, Themes.MapSquare);
                                    ctx.DrawText(t, new Point(c.X - t.Width / 2, c.Y - t.Height / 2));
                                }
                    }
                    for (int lo = -180; lo < 180; lo += 20)
                    {
                        List<Point> m = new List<Point>(); for (double la = -90; la <= 90; la += 1) m.Add(new Point(lo, la));
                        GeoPolyline(ctx, gp, m);
                    }
                    for (int la = -80; la <= 80; la += 10)
                    {
                        List<Point> m = new List<Point>(); for (double lo = -180; lo <= 180; lo += 1) m.Add(new Point(lo, la));
                        GeoPolyline(ctx, gp, m);
                    }
                    double fs = Math.Max(11, Math.Min(70, 2200.0 / RangeKm * 18)) / UiScale.Font;
                    for (int fx = 0; fx < 18; fx++)
                        for (int fy = 0; fy < 18; fy++)
                        {
                            double fla = -90 + fy * 10 + 5, flo = -180 + fx * 20 + 10;
                            if (Geo.DistanceKm(AzLat, AzLon, fla, flo) > RangeKm * 1.05) continue;
                            Point c = ToScreen(fla, flo);
                            FormattedText t = Ft("" + (char)('A' + fx) + (char)('A' + fy), fs, Themes.MapField, FontWeight.Bold);
                            ctx.DrawText(t, new Point(c.X - t.Width / 2, c.Y - t.Height / 2));
                        }
                }
                RenderBody(ctx);
            }
            // anelli di distanza dal centro (km) e azimut sul bordo
            double step = RingStep(RangeKm);
            IPen rp = Pal.P(Pal.A(170, Pal.Azure), 1, Pal.Dash);
            for (double d = step; d < RangeKm * 0.999; d += step)
            {
                double r = d * Kpx;
                ctx.DrawEllipse(null, rp, C, r, r);
                FormattedText t = Ft(d.ToString("N0", CultureInfo.CurrentCulture) + " km", 10.5, Pal.Azure, FontWeight.SemiBold);
                Rect tr = new Rect(C.X + r * 0.7071 + 2, C.Y - r * 0.7071 - t.Height - 1, t.Width + 6, t.Height + 2);
                ctx.FillRectangle(Pal.B(Ov), tr);
                ctx.DrawText(t, new Point(tr.X + 3, tr.Y + 1));
            }
            ctx.DrawEllipse(null, Pal.P(Pal.Azure, 1.6), C, R, R);
            for (int az = 0; az < 360; az += 30)
            {
                double a = az * Geo.D2R;
                Point o = new Point(C.X + R * Math.Sin(a), C.Y - R * Math.Cos(a)), i2 = new Point(C.X + (R - 8) * Math.Sin(a), C.Y - (R - 8) * Math.Cos(a));
                ctx.DrawLine(Pal.P(Pal.Azure, 1.4), i2, o);
                FormattedText t = Ft(az == 0 ? "N" : az + "°", 10, Pal.Azure, FontWeight.SemiBold);
                Point lp = new Point(C.X + (R + 9) * Math.Sin(a) - t.Width / 2, C.Y - (R + 9) * Math.Cos(a) - t.Height / 2);
                ctx.DrawText(t, lp);
            }
            ctx.DrawLine(Pal.P(Pal.A(120, Pal.Azure), 0.8), new Point(C.X - 6, C.Y), new Point(C.X + 6, C.Y));
            ctx.DrawLine(Pal.P(Pal.A(120, Pal.Azure), 0.8), new Point(C.X, C.Y - 6), new Point(C.X, C.Y + 6));
            // raggio del cerchio esterno (in alto a destra, sotto le coordinate)
            FormattedText rt = Ft(Lang.F("Raggio {0:N0} km dal centro ({1})", RangeKm, Geo.LatLonToLoc(AzLat, AzLon, 6)), 12.5, Pal.Yellow, FontWeight.SemiBold);
            Rect rr = new Rect(W - rt.Width - 18, H - rt.Height - 12, rt.Width + 12, rt.Height + 6);
            ctx.FillRectangle(Pal.B(Ov), rr);
            ctx.DrawText(rt, new Point(rr.X + 6, rr.Y + 3));
        }

        // cerchio di raggio rKm attorno a un punto (es. orizzonte radio alla quota dello strato)
        public static MapLine Circle(double lat, double lon, double rKm, Color c, double w)
        {
            MapLine l = new MapLine { Color = c, Width = w };
            for (int az = 0; az <= 360; az += 3)
            {
                double la, lo; Geo.Destination(lat, lon, az, rKm, out la, out lo);
                l.Pts.Add(new Point(lo, la));
            }
            return l;
        }

        // cerchio massimo come MapLine (n punti)
        public static MapLine GreatCircle(double lat1, double lon1, double lat2, double lon2, Color c, double w, bool dashed = false)
        {
            MapLine l = new MapLine { Color = c, Width = w, Dashed = dashed };
            double d = Geo.DistanceKm(lat1, lon1, lat2, lon2);
            int n = Math.Max(2, Math.Min(200, (int)(d / 50)));
            for (int i = 0; i <= n; i++)
            {
                double la, lo; Geo.Intermediate(lat1, lon1, lat2, lon2, (double)i / n, out la, out lo);
                l.Pts.Add(new Point(lo, la));
            }
            return l;
        }
    }
}
