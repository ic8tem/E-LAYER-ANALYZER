// YA-MS - mappa azimutale del traffico meteor scatter in diretta (sottolinguetta "Traffico in diretta").
// Proiezione azimutale equidistante centrata sulla stazione: la distanza dal centro e' quella vera (km), l'angolo
// e' l'azimut. Per ogni spot raccolto: tratta tra le due stazioni (colore della banda), stazioni agli estremi e la
// "nuvola" dove si concentrano le scie piu' probabili per quella tratta e quell'ora (calcolata in MainWindow.Traffico.cs
// con lo stesso modello di riflessione speculare della volta celeste). Gli spot con una stazione fuori dalla mappa
// non si disegnano. Gli spot arrivati da meno di un minuto (e non piu' vecchi di 3 minuti) lampeggiano.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Threading;

namespace YAMS
{
    public class TrafficCloud
    {
        public LatLon Centre;
        public double MajorKm, MinorKm, AngleDeg;   // semiassi e azimut dell'asse maggiore
        public double Share = 1;                    // frazione delle riflessioni (0..1)
        public bool Sporadic;                       // nessuno sciame utile: hot spot classici
    }

    public class TrafficSpot
    {
        public LatLon A, B;
        public string CallA = "", CallB = "", Band = "", Info = "";
        public Color Color;
        public bool Blink;
        public bool Dashed;    // spot WSJT-X UDP che riguarda la mia stazione: tratteggio che lampeggia sempre
        public List<TrafficCloud> Clouds = new List<TrafficCloud>();
        public object Tag;
    }

    // stazione in ascolto nei modi MS (PSKReporter MQTT): triangolo del colore della banda
    public class TrafficListener
    {
        public LatLon Pos;
        public string Call = "", Info = "";
        public Color Color;
        public bool Hollow;   // monitor attivo di PSKReporter senza spot ricevuti: triangolo vuoto
    }

    public class TrafficView : Control
    {
        public List<TrafficListener> Listeners = new List<TrafficListener>();
        public event Action<TrafficListener> ListenerClicked;
        public LatLon Home;
        public string HomeLabel = "";
        public double RadiusKm = 2500;
        // proiezione di Mercatore (rettangolo, nord in alto, meridiani verticali) al posto dell'azimutale equidistante
        // centrata sulla stazione; il raggio indica quanti km stanno tra il centro e il bordo piu' vicino
        public bool Mercator;
        // Mercatore spostabile (richiesta dell'autore, 14/11/2026): si trascina con il mouse; doppio clic = torna
        // centrata sulla stazione. L'azimutale resta fissa sulla stazione.
        double panLat = double.NaN, panLon;
        public event Action ViewChanged;
        LatLon C { get { return Mercator && !double.IsNaN(panLat) ? new LatLon(panLat, panLon) : Home; } }
        public void ResetPan() { panLat = double.NaN; InvalidateVisual(); ViewChanged?.Invoke(); }
        Point? dragStart; Point dragLast; bool dragged;
        double hw, hh;   // mezza larghezza e mezza altezza dell'area della mappa (Mercatore)
        public List<System.Drawing.PointF[]> MapLines, MapBorders;
        public List<TrafficSpot> Spots = new List<TrafficSpot>();
        public TrafficSpot Selected;
        public string Caption = "";
        public List<KeyValuePair<string, Color>> Legend = new List<KeyValuePair<string, Color>>();
        public event Action<TrafficSpot> SpotClicked;

        readonly DispatcherTimer blink = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        bool phase = true;
        double cx, cy, rad;

        public TrafficView()
        {
            ClipToBounds = true;
            blink.Tick += (s, e) =>
            {
                bool any = false; foreach (TrafficSpot t in Spots) if (t.Blink || t.Dashed) { any = true; break; }
                if (!any) { phase = true; return; }
                phase = !phase;
                if (IsEffectivelyVisible) InvalidateVisual();
            };
            PointerPressed += (s, e) =>
            {
                if (Mercator) { dragStart = dragLast = e.GetPosition(this); dragged = false; e.Pointer.Capture(this); if (e.ClickCount == 2) ResetPan(); return; }
                ClickAt(e.GetPosition(this));
            };
            PointerMoved += (s, e) =>
            {
                if (!Mercator || dragStart == null) return;
                Point p = e.GetPosition(this);
                if (!dragged && Math.Abs(p.X - dragStart.Value.X) + Math.Abs(p.Y - dragStart.Value.Y) < 5) return;
                dragged = true;
                double k = MercK();
                if (k <= 0) return;
                LatLon c = C;
                double lon = c.Lon - (p.X - dragLast.X) / k * 180 / Math.PI;
                double my = MercY(c.Lat) + (p.Y - dragLast.Y) / k;
                double lat = (2 * Math.Atan(Math.Exp(my)) - Math.PI / 2) * 180 / Math.PI;
                while (lon > 180) lon -= 360; while (lon < -180) lon += 360;
                panLat = Math.Max(-80, Math.Min(80, lat)); panLon = lon;
                dragLast = p;
                InvalidateVisual();
            };
            PointerReleased += (s, e) =>
            {
                if (!Mercator || dragStart == null) return;
                e.Pointer.Capture(null);
                Point p0 = dragStart.Value; dragStart = null;
                if (dragged) { ViewChanged?.Invoke(); return; }
                ClickAt(p0);
            };
        }

        void ClickAt(Point p)
        {
            {
                TrafficSpot best = null; double bd = 14;
                foreach (TrafficSpot t in Spots)
                {
                    double d = SegDist(p, P(t.A), P(t.B));
                    if (d < bd) { bd = d; best = t; }
                }
                if (best == null)
                {
                    TrafficListener bl = null; double ld = 10;
                    foreach (TrafficListener l in Listeners)
                    {
                        Point q = P(l.Pos); double d = Math.Sqrt((q.X - p.X) * (q.X - p.X) + (q.Y - p.Y) * (q.Y - p.Y));
                        if (d < ld) { ld = d; bl = l; }
                    }
                    if (bl != null) { Action<TrafficListener> la = ListenerClicked; if (la != null) la(bl); return; }
                }
                Selected = best;
                InvalidateVisual();
                Action<TrafficSpot> a = SpotClicked; if (a != null) a(best);
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            blink.Start();
            UiScale.Changed += InvalidateVisual;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            blink.Stop();
            UiScale.Changed -= InvalidateVisual;
        }

        public bool Inside(LatLon p)
        {
            if (!Mercator || rad <= 0) return Astro.DistanceKm(Home, p) <= RadiusKm;
            Point q = P(p);
            return Math.Abs(q.X - cx) <= hw && Math.Abs(q.Y - cy) <= hh && Astro.DistanceKm(C, p) <= RadiusKm * 3;
        }

        // scala del Mercatore: vera alla latitudine della stazione (non cambia spostando la vista)
        double MercK() { return rad / RadiusKm * 6371.0 * Math.Cos(Home.Lat * Math.PI / 180); }

        static double MercY(double latDeg)
        {
            double la = Math.Max(-85, Math.Min(85, latDeg)) * Math.PI / 180;
            return Math.Log(Math.Tan(Math.PI / 4 + la / 2));
        }

        Point P(LatLon p)
        {
            if (Mercator)
            {
                // scala vera alla latitudine della stazione: 1 radiante di Mercatore = R cos(lat) km
                LatLon c = C;
                double k = MercK();
                double dl = p.Lon - c.Lon;
                while (dl > 180) dl -= 360;
                while (dl < -180) dl += 360;
                return new Point(cx + k * dl * Math.PI / 180, cy - k * (MercY(p.Lat) - MercY(c.Lat)));
            }
            double d = Astro.DistanceKm(Home, p), b = Astro.Bearing(Home, p) * Math.PI / 180;
            double r = d / RadiusKm * rad;
            return new Point(cx + r * Math.Sin(b), cy - r * Math.Cos(b));
        }

        static double SegDist(Point p, Point a, Point b)
        {
            double vx = b.X - a.X, vy = b.Y - a.Y, l2 = vx * vx + vy * vy;
            double t = l2 <= 0 ? 0 : Math.Max(0, Math.Min(1, ((p.X - a.X) * vx + (p.Y - a.Y) * vy) / l2));
            double dx = a.X + t * vx - p.X, dy = a.Y + t * vy - p.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        static FormattedText Ft(string s, double size, Color c, FontWeight w = FontWeight.Normal)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, FontStyle.Normal, w), size * UiScale.Font, new ImmutableSolidColorBrush(c));
        }

        public override void Render(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            bool dark = Tema.Scuro;
            Color page = dark ? Color.FromRgb(16, 22, 36) : Color.FromRgb(244, 246, 250);
            Color sea = dark ? Color.FromRgb(20, 30, 52) : Color.FromRgb(228, 236, 246);
            Color coast = dark ? Color.FromRgb(130, 146, 175) : Color.FromRgb(105, 115, 132);
            Color border = dark ? Color.FromRgb(70, 82, 110) : Color.FromRgb(180, 188, 200);
            Color ring = dark ? Color.FromArgb(120, 160, 175, 200) : Color.FromArgb(140, 140, 150, 170);
            Color ink = dark ? Color.FromRgb(225, 230, 240) : Color.FromRgb(31, 41, 55);
            Color box = dark ? Color.FromArgb(215, 12, 18, 32) : Color.FromArgb(225, 255, 255, 255);
            ctx.DrawRectangle(new ImmutableSolidColorBrush(page), null, new Rect(0, 0, W, H));
            if (W < 80 || H < 80) return;
            double fs = 12;
            double margin = 28 + 10 * UiScale.Font;
            rad = Math.Max(20, Math.Min(W, H) / 2 - margin);
            cx = W / 2; cy = H / 2;
            Point c0 = new Point(cx, cy);
            if (Mercator) { rad = Math.Max(20, Math.Min(W, H) / 2 - 6); hw = W / 2 - 6; hh = H / 2 - 6; }
            Rect mrect = new Rect(cx - hw, cy - hh, 2 * hw, 2 * hh);
            if (Mercator) ctx.DrawRectangle(new ImmutableSolidColorBrush(sea), null, mrect);
            else ctx.DrawEllipse(new ImmutableSolidColorBrush(sea), null, c0, rad, rad);
            using (ctx.PushGeometryClip(Mercator ? (Geometry)new RectangleGeometry(mrect) : new EllipseGeometry(new Rect(cx - rad, cy - rad, 2 * rad, 2 * rad))))
            {
                if (Mercator) DrawGraticule(ctx, new Pen(new ImmutableSolidColorBrush(ring), 1, new ImmutableDashStyle(new double[] { 4, 4 }, 0)), fs, ring, W, H);
                DrawLines(ctx, MapBorders, new Pen(new ImmutableSolidColorBrush(border), 0.6));
                DrawLines(ctx, MapLines, new Pen(new ImmutableSolidColorBrush(coast), 0.9));
                // cerchi di distanza ogni 500 km
                IPen rp = new Pen(new ImmutableSolidColorBrush(ring), 1, new ImmutableDashStyle(new double[] { 4, 4 }, 0));
                double step = RadiusKm > 3000 ? 1000 : 500;
                for (double r = step; r < RadiusKm && !Mercator; r += step)
                {
                    double rr = r / RadiusKm * rad;
                    ctx.DrawEllipse(null, rp, c0, rr, rr);
                    FormattedText t = Ft(r.ToString("F0", CultureInfo.InvariantCulture) + " km", fs * 0.8, ring);
                    ctx.DrawText(t, new Point(cx + rr * 0.707 + 2, cy - rr * 0.707 - t.Height));
                }
                // nuvole, poi tratte e stazioni (gli spot selezionati e lampeggianti sopra gli altri)
                foreach (TrafficSpot s in Spots) DrawClouds(ctx, s, false);
                foreach (TrafficListener l in Listeners) if (l.Hollow) DrawListener(ctx, l, box, Listeners.Count <= 40);
                foreach (TrafficListener l in Listeners) if (!l.Hollow) DrawListener(ctx, l, box, Listeners.Count <= 40);
                labeled.Clear();
                foreach (TrafficSpot s in Spots) DrawPath(ctx, s, box);
                if (Selected != null && Spots.Contains(Selected)) { DrawClouds(ctx, Selected, true); DrawPath(ctx, Selected, box); }
            }
            // bordo e scala dei gradi (come il rotore d'antenna)
            IPen rim = new Pen(new ImmutableSolidColorBrush(coast), 1.5);
            if (Mercator) ctx.DrawRectangle(null, rim, mrect);
            else ctx.DrawEllipse(null, rim, c0, rad, rad);
            for (int a = 0; a < 360 && !Mercator; a += 10)
            {
                double ar = a * Math.PI / 180, l = a % 30 == 0 ? 8 : 4;
                ctx.DrawLine(rim, new Point(cx + rad * Math.Sin(ar), cy - rad * Math.Cos(ar)), new Point(cx + (rad + l) * Math.Sin(ar), cy - (rad + l) * Math.Cos(ar)));
                if (a % 30 == 0)
                {
                    FormattedText t = Ft(a.ToString(CultureInfo.InvariantCulture) + "°", fs * 0.85, Color.FromRgb(234, 88, 12));
                    double lr = rad + 12 + t.Height * 0.6;
                    ctx.DrawText(t, new Point(cx + lr * Math.Sin(ar) - t.Width / 2, cy - lr * Math.Cos(ar) - t.Height / 2));
                }
            }
            // stazione al centro (nel Mercatore spostato: dove cade sulla mappa)
            Point hp = Mercator ? P(Home) : c0;
            if (!Mercator || (Math.Abs(hp.X - cx) <= hw && Math.Abs(hp.Y - cy) <= hh))
            {
                Star(ctx, hp, 10, Color.FromRgb(225, 29, 72));
                FormattedText hl = Ft(HomeLabel, fs, Color.FromRgb(225, 29, 72), FontWeight.Bold);
                Label(ctx, hl, hp.X + 12, hp.Y + 4, box);
            }
            // didascalia e legenda
            if (!string.IsNullOrEmpty(Caption))
            {
                FormattedText t = Ft(Caption, fs * 1.05, ink, FontWeight.Bold);
                Label(ctx, t, 8, 6, box);
            }
            double ly = H - 8;
            for (int i = Legend.Count - 1; i >= 0; i--)
            {
                FormattedText t = Ft(Legend[i].Key, fs * 0.9, ink);
                ly -= t.Height + 3;
                ctx.DrawRectangle(new ImmutableSolidColorBrush(box), null, new Rect(6, ly - 1, t.Width + 26, t.Height + 2), 3, 3);
                ctx.DrawRectangle(new ImmutableSolidColorBrush(Legend[i].Value), null, new Rect(10, ly + t.Height / 2 - 4, 12, 8), 2, 2);
                ctx.DrawText(t, new Point(28, ly));
            }
        }

        void DrawClouds(DrawingContext ctx, TrafficSpot s, bool sel)
        {
            if (s.Blink && !phase && !sel) return;
            foreach (TrafficCloud cl in s.Clouds)
            {
                StreamGeometry g = new StreamGeometry();
                using (StreamGeometryContext c = g.Open())
                {
                    double ang = cl.AngleDeg * Math.PI / 180;
                    for (int k = 0; k <= 36; k++)
                    {
                        double t = k * Math.PI / 18;
                        double u = cl.MajorKm * Math.Cos(t), v = cl.MinorKm * Math.Sin(t);
                        // u lungo l'asse maggiore (azimut AngleDeg), v ortogonale
                        double north = u * Math.Cos(ang) - v * Math.Sin(ang), east = u * Math.Sin(ang) + v * Math.Cos(ang);
                        double d = Math.Sqrt(north * north + east * east), b = Math.Atan2(east, north) * 180 / Math.PI;
                        Point p = P(Astro.Destination(cl.Centre, b, d));
                        if (k == 0) c.BeginFigure(p, true); else c.LineTo(p);
                    }
                    c.EndFigure(true);
                }
                byte fa = (byte)Math.Max(25, Math.Min(110, (sel ? 110 : 70) * (0.35 + 0.65 * cl.Share)));
                IPen pen = cl.Sporadic ? new Pen(new ImmutableSolidColorBrush(Color.FromArgb(170, s.Color.R, s.Color.G, s.Color.B)), sel ? 2 : 1, new ImmutableDashStyle(new double[] { 3, 3 }, 0))
                                       : new Pen(new ImmutableSolidColorBrush(Color.FromArgb(200, s.Color.R, s.Color.G, s.Color.B)), sel ? 2.2 : 1.2);
                ctx.DrawGeometry(new ImmutableSolidColorBrush(Color.FromArgb(fa, s.Color.R, s.Color.G, s.Color.B)), pen, g);
            }
        }

        readonly HashSet<string> labeled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void DrawPath(DrawingContext ctx, TrafficSpot s, Color box)
        {
            bool sel = s == Selected;
            if ((s.Blink || s.Dashed) && !phase && !sel) return;
            Point a = P(s.A), b = P(s.B);
            if (sel) Seg(ctx, new Pen(new ImmutableSolidColorBrush(Color.FromArgb(120, 255, 255, 255)), 7), s.A, s.B);
            if (s.Dashed)
                Seg(ctx, new Pen(new ImmutableSolidColorBrush(s.Color), sel ? 3.5 : 3, new ImmutableDashStyle(new double[] { 3, 2 }, 0)), s.A, s.B);
            else
                Seg(ctx, new Pen(new ImmutableSolidColorBrush(s.Color), sel ? 3.5 : (s.Blink ? 3 : 1.8)), s.A, s.B);
            foreach (Point p in new[] { a, b }) ctx.DrawEllipse(new ImmutableSolidColorBrush(s.Color), new Pen(Brushes.White, 1.5), p, sel ? 5.5 : 4, sel ? 5.5 : 4);
            if (s.Blink)
            {
                ctx.DrawEllipse(null, new Pen(new ImmutableSolidColorBrush(s.Color), 2), b, 10, 10);
                ctx.DrawEllipse(null, new Pen(new ImmutableSolidColorBrush(s.Color), 2), a, 10, 10);
            }
            if (sel || s.Blink || s.Dashed || Spots.Count <= 25)
            {
                // nominativi di entrambe le stazioni (chi ascolta e chi e' ascoltato), una sola volta per stazione;
                // la stazione di casa ha gia' la sua scritta
                Color ink = Tema.Scuro ? Color.FromRgb(225, 230, 240) : Color.FromRgb(31, 41, 55);
                foreach (var e in new[] { (s.CallA, a), (s.CallB, b) })
                {
                    if (string.IsNullOrEmpty(e.Item1) || (Math.Abs(e.Item2.X - P(Home).X) < 3 && Math.Abs(e.Item2.Y - P(Home).Y) < 3)) continue;
                    if (!labeled.Add(e.Item1) && !sel) continue;
                    FormattedText t = Ft(e.Item1, 9, ink, sel ? FontWeight.Bold : FontWeight.Normal);
                    Label(ctx, t, e.Item2.X + 6, e.Item2.Y - t.Height - 2, box);
                }
            }
        }

        void DrawListener(DrawingContext ctx, TrafficListener l, Color box, bool label)
        {
            Point c = P(l.Pos);
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext s = g.Open())
            {
                s.BeginFigure(new Point(c.X, c.Y - 6), true);
                s.LineTo(new Point(c.X + 5.5, c.Y + 4)); s.LineTo(new Point(c.X - 5.5, c.Y + 4));
                s.EndFigure(true);
            }
            ctx.DrawGeometry(l.Hollow ? null : new ImmutableSolidColorBrush(Color.FromArgb(150, l.Color.R, l.Color.G, l.Color.B)), new Pen(new ImmutableSolidColorBrush(l.Color), l.Hollow ? 1.2 : 1.4), g);
            if (label)
            {
                Color ink = Tema.Scuro ? Color.FromRgb(200, 208, 222) : Color.FromRgb(71, 85, 105);
                FormattedText t = Ft(l.Call, 8, ink);
                Label(ctx, t, c.X + 6, c.Y - 2, Color.FromArgb((byte)(box.A * 0.8), box.R, box.G, box.B));
            }
        }

        static void Label(DrawingContext ctx, FormattedText t, double x, double y, Color box)
        {
            ctx.DrawRectangle(new ImmutableSolidColorBrush(box), null, new Rect(x - 3, y - 1, t.Width + 6, t.Height + 2), 3, 3);
            ctx.DrawText(t, new Point(x, y));
        }

        static void Star(DrawingContext ctx, Point c, double r, Color col)
        {
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext s = g.Open())
            {
                for (int i = 0; i < 10; i++)
                {
                    double a = -Math.PI / 2 + i * Math.PI / 5, rr = i % 2 == 0 ? r : r * 0.45;
                    Point p = new Point(c.X + rr * Math.Cos(a), c.Y + rr * Math.Sin(a));
                    if (i == 0) s.BeginFigure(p, true); else s.LineTo(p);
                }
                s.EndFigure(true);
            }
            ctx.DrawGeometry(new ImmutableSolidColorBrush(col), new Pen(Brushes.White, 1.5), g);
        }

        // tratta sul cerchio massimo: retta nell'azimutale centrata sulla stazione, curva in Mercatore
        void Seg(DrawingContext ctx, IPen pen, LatLon a, LatLon b)
        {
            if (!Mercator) { ctx.DrawLine(pen, P(a), P(b)); return; }
            double d = Astro.DistanceKm(a, b), br = Astro.Bearing(a, b);
            int n = Math.Max(2, Math.Min(32, (int)(d / 100)));
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext c = g.Open())
            {
                c.BeginFigure(P(a), false);
                for (int i = 1; i <= n; i++) c.LineTo(P(i == n ? b : Astro.Destination(a, br, d * i / n)));
                c.EndFigure(false);
            }
            ctx.DrawGeometry(null, pen, g);
        }

        // reticolo di paralleli e meridiani (Mercatore), con i gradi sul bordo
        void DrawGraticule(DrawingContext ctx, IPen pen, double fs, Color ring, double W, double H)
        {
            double k = MercK();
            LatLon c0g = C;
            double degW = hw / k * 180 / Math.PI;
            double step = degW > 40 ? 20 : degW > 15 ? 10 : 5;
            for (double lo = Math.Floor((c0g.Lon - degW) / step) * step; lo <= c0g.Lon + degW; lo += step)
            {
                double x = P(new LatLon(c0g.Lat, lo)).X;
                ctx.DrawLine(pen, new Point(x, 0), new Point(x, H));
                double l = lo > 180 ? lo - 360 : lo < -180 ? lo + 360 : lo;
                FormattedText t = Ft(Math.Abs(l).ToString("F0", CultureInfo.InvariantCulture) + "°" + (l < 0 ? "W" : l > 0 ? "E" : ""), fs * 0.8, ring);
                ctx.DrawText(t, new Point(x + 2, cy + hh - t.Height - 2));
            }
            for (double la = -80; la <= 80; la += step)
            {
                double y = P(new LatLon(la, c0g.Lon)).Y;
                if (y < 0 || y > H) continue;
                ctx.DrawLine(pen, new Point(0, y), new Point(W, y));
                FormattedText t = Ft(Math.Abs(la).ToString("F0", CultureInfo.InvariantCulture) + "°" + (la < 0 ? "S" : la > 0 ? "N" : ""), fs * 0.8, ring);
                ctx.DrawText(t, new Point(cx + hw - t.Width - 4, y - t.Height - 1));
            }
        }

        void DrawLines(DrawingContext ctx, List<System.Drawing.PointF[]> lines, IPen pen)
        {
            if (lines == null) return;
            double lim = Mercator ? RadiusKm * 3.2 : RadiusKm * 1.08;
            foreach (System.Drawing.PointF[] l in lines)
            {
                if (l == null || l.Length < 2) continue;
                StreamGeometry g = new StreamGeometry();
                bool any = false;
                using (StreamGeometryContext c = g.Open())
                {
                    bool open = false; double lastX = 0;
                    foreach (System.Drawing.PointF q in l)
                    {
                        LatLon ll = new LatLon(q.Y, q.X);
                        if (Astro.DistanceKm(Mercator ? C : Home, ll) > lim) { if (open) { c.EndFigure(false); open = false; } continue; }
                        Point p = P(ll);
                        if (Mercator && open && Math.Abs(p.X - lastX) > hw) { c.EndFigure(false); open = false; }   // salto dell'antimeridiano
                        lastX = p.X;
                        if (!open) { c.BeginFigure(p, false); open = true; any = true; } else c.LineTo(p);
                    }
                    if (open) c.EndFigure(false);
                }
                if (any) ctx.DrawGeometry(null, pen, g);
            }
        }
    }
}
