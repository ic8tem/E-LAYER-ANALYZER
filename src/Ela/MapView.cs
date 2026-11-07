// E-LAYER ANALYZER - mappa del mondo (proiezione equirettangolare) con zoom (rotella), spostamento (trascina),
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
    public class MapMarker { public double Lat, Lon, Radius = 4; public Color Fill, Stroke = Colors.Black; public string Label; public bool Square, Ring; public Color LabelColor = Colors.White; }

    public class MapView : Control
    {
        public static List<System.Drawing.PointF[]> Coast, Borders;
        public static event Action MapLoaded;
        public static void NotifyMapLoaded() { Action a = MapLoaded; if (a != null) a(); }

        public double CenterLon = 12, CenterLat = 45, Zoom = 1;   // Zoom 1 = tutta la larghezza mostra 360 gradi
        public double MinLat = -85, MaxLat = 85;
        public readonly List<MapCell> Cells = new List<MapCell>();
        public readonly List<MapLine> Lines = new List<MapLine>();
        public readonly List<MapMarker> Markers = new List<MapMarker>();
        public readonly List<KeyValuePair<string, Color>> Legend = new List<KeyValuePair<string, Color>>();
        public string Caption = "";
        public readonly List<string> Info = new List<string>();   // righe grandi in alto a sinistra (es. Azimut / Elevazione)
        public string LegendTitle = "";
        public bool ShowLocatorGrid = true;

        public event Action<double, double, bool> Clicked;   // lat, lon, tasto destro

        Point? dragStart; double dragLon0, dragLat0; bool dragged;
        Point hover = new Point(-1, -1);

        static readonly Color Sea = Color.FromRgb(13, 16, 44);
        static readonly Color Land = Color.FromRgb(126, 116, 190);
        static readonly Color Border = Color.FromRgb(70, 62, 120);
        static readonly Color GridC = Color.FromArgb(70, 120, 110, 190);

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
            double p = Ppd;
            double dl = lon - CenterLon;
            if (dl > 180) dl -= 360; else if (dl < -180) dl += 360;
            return new Point(Bounds.Width / 2 + dl * p, Bounds.Height / 2 - (lat - CenterLat) * p);
        }

        public void ToLatLon(Point s, out double lat, out double lon)
        {
            double p = Ppd;
            lon = Geo.Norm180(CenterLon + (s.X - Bounds.Width / 2) / p);
            lat = CenterLat - (s.Y - Bounds.Height / 2) / p;
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            Point pos = e.GetPosition(this);
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
                if (dragged)
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
                        bool jump = open && Math.Abs(p.X - prev.X) > W / 2;
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

        public override void Render(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            ctx.FillRectangle(Pal.B(Sea), new Rect(0, 0, W, H));
            double p = Ppd;

            // reticolo Maidenhead: campi 20 x 10 gradi con le lettere grandi (come AuroReflex), quadrati 2 x 1 gradi con le cifre
            if (ShowLocatorGrid)
            {
                IPen gp = Pal.P(GridC, 1.2);
                bool squares = p * 2 > 26;
                IPen sp = Pal.P(Color.FromArgb(55, 150, 140, 210), 0.7);
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
                                FormattedText t = Ft(loc.Substring(2, 2), 8.5, Color.FromArgb(110, 170, 165, 230));
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
                        FormattedText t = Ft("" + (char)('A' + fx) + (char)('A' + fy), fs, Color.FromArgb(70, 90, 230, 250), FontWeight.Bold);
                        ctx.DrawText(t, new Point(c.X - t.Width / 2, c.Y - t.Height / 2));
                    }
            }

            // celle colorate (sotto le coste)
            foreach (MapCell c in Cells)
            {
                Point a = ToScreen(c.Lat0 + c.DLat, c.Lon0), b = ToScreen(c.Lat0, c.Lon0 + c.DLon);
                if (b.X < a.X) continue;   // attraversa il bordo della vista
                if (b.X < 0 || a.X > W || b.Y < 0 || a.Y > H) continue;
                ctx.FillRectangle(Pal.B(c.Fill), new Rect(a, b));
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
                    if (Math.Abs(q.X - prev.X) < W / 2) ctx.DrawLine(pen, prev, q);
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
                    ctx.FillRectangle(Pal.B(Color.FromArgb(150, 10, 8, 30)), tr);
                    ctx.DrawText(t, new Point(tr.X + 2, tr.Y));
                }
            }

            // legenda in basso a sinistra
            double y = H - 8;
            for (int i = Legend.Count - 1; i >= 0; i--)
            {
                FormattedText t = Ft(Legend[i].Key, 11.5, Pal.Fore);
                y -= t.Height + 3;
                ctx.FillRectangle(Pal.B(Color.FromArgb(190, 14, 11, 34)), new Rect(6, y - 1, t.Width + 30, t.Height + 2));
                ctx.FillRectangle(Pal.B(Legend[i].Value), new Rect(10, y + t.Height / 2 - 5, 12, 10));
                ctx.DrawText(t, new Point(28, y));
            }
            if (LegendTitle.Length > 0 && Legend.Count > 0)
            {
                FormattedText t = Ft(LegendTitle, 11.5, Pal.Yellow, FontWeight.SemiBold);
                y -= t.Height + 3;
                ctx.FillRectangle(Pal.B(Color.FromArgb(190, 14, 11, 34)), new Rect(6, y - 1, t.Width + 12, t.Height + 2));
                ctx.DrawText(t, new Point(10, y));
            }
            double iy = string.IsNullOrEmpty(Caption) ? 8 : 34;
            foreach (string line in Info)
            {
                FormattedText t = Ft(line, 16, Pal.Fore, FontWeight.SemiBold);
                ctx.FillRectangle(Pal.B(Color.FromArgb(170, 14, 11, 34)), new Rect(6, iy - 2, t.Width + 12, t.Height + 4));
                ctx.DrawText(t, new Point(12, iy));
                iy += t.Height + 4;
            }
            if (!string.IsNullOrEmpty(Caption))
            {
                FormattedText t = Ft(Caption, 12, Pal.Yellow, FontWeight.SemiBold);
                ctx.FillRectangle(Pal.B(Color.FromArgb(190, 14, 11, 34)), new Rect(6, 6, t.Width + 12, t.Height + 6));
                ctx.DrawText(t, new Point(12, 9));
            }

            // coordinate del cursore in alto a destra
            if (hover.X >= 0)
            {
                double la, lo; ToLatLon(hover, out la, out lo);
                string s = la.ToString("F2", CultureInfo.CurrentCulture) + "°  " + lo.ToString("F2", CultureInfo.CurrentCulture) + "°  " + Geo.LatLonToLoc(la, lo, 6);
                FormattedText t = Ft(s, 12, Pal.Azure, FontWeight.SemiBold);
                ctx.FillRectangle(Pal.B(Color.FromArgb(200, 14, 11, 34)), new Rect(W - t.Width - 18, 6, t.Width + 12, t.Height + 6));
                ctx.DrawText(t, new Point(W - t.Width - 12, 9));
            }
            if (Coast == null || Coast.Count == 0)
            {
                FormattedText t = Ft(Lang.T("Carico la mappa (Natural Earth)..."), 13, Pal.Muted);
                ctx.DrawText(t, new Point(W / 2 - t.Width / 2, H / 2));
            }
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
