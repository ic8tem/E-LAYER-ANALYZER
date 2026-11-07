// YA-MS - mappa delle fonti dei dati in diretta (linguetta "Mappa delle fonti")
// Mappa equirettangolare (longitudine compressa con cos(latitudine media)) con coste e confini di Natural Earth,
// la stazione dell'utente, i radar/trasmettitori, la zona di cielo vista da ciascun radar (cerchio di circa 300 km:
// scie a 90 km di quota) e la linea di cerchio massimo con la distanza dalla stazione.
// Si usa in due istanze: Europa (tutte le fonti europee) e Mondo (per CMOR in Canada).
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YAMS
{
    public class MapSource
    {
        public string Name, Line1, Line2, Tab;
        public double Lat, Lon;
        public Color Color;
        public double CoverKm = 300;
        public bool LabelLeft;
    }

    public class SourcesMapView : Control
    {
        public double LonMin = -12, LonMax = 38, LatMin = 34, LatMax = 72;
        public string Title = "";
        public List<System.Drawing.PointF[]> MapLines, MapBorders;
        public List<MapSource> Sources = new List<MapSource>();
        public List<MapSource> Minor = new List<MapSource>();   // punti secondari (osservatori RMOB), solo triangoli
        public string MinorLabel = "";
        public LatLon Home;
        public string HomeLabel = "";
        public double FontK = 1;

        public SourcesMapView()
        {
            ClipToBounds = true;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            UiScale.Changed += InvalidateVisual;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            UiScale.Changed -= InvalidateVisual;
        }

        static FormattedText Ft(string s, double size, Color c, FontWeight w = FontWeight.Normal, FontStyle st = FontStyle.Normal)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, st, w), size * UiScale.Font, new ImmutableSolidColorBrush(c));
        }

        public override void Render(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            bool dark = Tema.Scuro;
            Color sea = dark ? Color.FromRgb(18, 26, 44) : Color.FromRgb(232, 238, 247);
            Color coast = dark ? Color.FromRgb(130, 146, 175) : Color.FromRgb(95, 105, 122);
            Color border = dark ? Color.FromRgb(70, 82, 110) : Color.FromRgb(175, 182, 195);
            Color ink = dark ? Color.FromRgb(225, 230, 240) : Color.FromRgb(31, 41, 55);
            Color muted = dark ? Color.FromRgb(160, 170, 190) : Color.FromRgb(107, 114, 128);
            Color box = dark ? Color.FromArgb(215, 12, 18, 32) : Color.FromArgb(225, 255, 255, 255);
            ctx.DrawRectangle(new ImmutableSolidColorBrush(sea), null, new Rect(0, 0, W, H));
            if (W < 60 || H < 60) return;

            // proiezione: scala uguale in x e y (x compressa con cos della latitudine media), centrata
            double k = Math.Cos((LatMin + LatMax) / 2 * Math.PI / 180);
            double sx = W / ((LonMax - LonMin) * k), sy = H / (LatMax - LatMin);
            double s = Math.Min(sx, sy);
            double cx = (LonMin + LonMax) / 2, cy = (LatMin + LatMax) / 2;
            Func<double, double, Point> P = (lat, lon) => new Point(W / 2 + (lon - cx) * k * s, H / 2 - (lat - cy) * s);

            IPen pc = new Pen(new ImmutableSolidColorBrush(coast), 0.8);
            IPen pb = new Pen(new ImmutableSolidColorBrush(border), 0.5);
            DrawLines(ctx, MapBorders, pb, P);
            DrawLines(ctx, MapLines, pc, P);

            double fs = 13.5 * FontK;
            // zone di cielo viste dai radar e linee verso la stazione
            foreach (MapSource m in Sources)
            {
                if (m.CoverKm > 0)
                {
                    StreamGeometry g = new StreamGeometry();
                    using (StreamGeometryContext c = g.Open())
                    {
                        LatLon o = new LatLon(m.Lat, m.Lon);
                        for (int b = 0; b <= 360; b += 5)
                        {
                            LatLon q = Astro.Destination(o, b, m.CoverKm);
                            Point pt = P(q.Lat, q.Lon);
                            if (b == 0) c.BeginFigure(pt, true); else c.LineTo(pt);
                        }
                        c.EndFigure(true);
                    }
                    ctx.DrawGeometry(new ImmutableSolidColorBrush(Color.FromArgb(45, m.Color.R, m.Color.G, m.Color.B)),
                        new Pen(new ImmutableSolidColorBrush(m.Color), 1, new ImmutableDashStyle(new double[] { 4, 3 }, 0)), g);
                }
                GreatCircle(ctx, Home, new LatLon(m.Lat, m.Lon), m.Color, P);
            }
            // osservatori secondari
            foreach (MapSource m in Minor)
            {
                Point p = P(m.Lat, m.Lon);
                StreamGeometry g = new StreamGeometry();
                using (StreamGeometryContext c = g.Open())
                {
                    c.BeginFigure(new Point(p.X, p.Y - 6), true); c.LineTo(new Point(p.X + 5.5, p.Y + 4)); c.LineTo(new Point(p.X - 5.5, p.Y + 4)); c.EndFigure(true);
                }
                ctx.DrawGeometry(new ImmutableSolidColorBrush(Color.FromRgb(250, 204, 21)), new Pen(new ImmutableSolidColorBrush(Color.FromRgb(133, 77, 14)), 1), g);
            }
            if (Minor.Count > 0 && !string.IsNullOrEmpty(MinorLabel))
            {
                double mx = 0, my = 0; foreach (MapSource m in Minor) { mx = Math.Max(mx, m.Lon); my += m.Lat; }
                Point p = P(my / Minor.Count, mx);
                FormattedText t = Ft(MinorLabel, fs * 0.85, dark ? Color.FromRgb(250, 204, 21) : Color.FromRgb(133, 77, 14));
                Label(ctx, t, p.X + 10, p.Y - t.Height / 2, box);
            }
            // marcatori ed etichette delle fonti
            foreach (MapSource m in Sources)
            {
                Point p = P(m.Lat, m.Lon);
                ctx.DrawEllipse(new ImmutableSolidColorBrush(m.Color), new Pen(Brushes.White, 2), p, 6.5, 6.5);
                double d = Astro.DistanceKm(Home, new LatLon(m.Lat, m.Lon));
                FormattedText tn = Ft(m.Name, fs * 1.2, m.Color, FontWeight.Bold);
                FormattedText t1 = Ft(m.Line1 + "\n" + m.Line2 + "\n" + Lang.F("{0} km dalla tua stazione", d.ToString("N0", CultureInfo.CurrentCulture)), fs * 0.9, ink);
                FormattedText t2 = Ft("→ " + Lang.F("linguetta \"{0}\"", m.Tab), fs * 0.8, muted, FontWeight.Normal, FontStyle.Italic);
                double wmax = Math.Max(tn.Width, Math.Max(t1.Width, t2.Width));
                double x = m.LabelLeft ? p.X - 12 - wmax : p.X + 12;
                if (x + wmax > W - 4) x = p.X - 12 - wmax;
                if (x < 4) x = p.X + 12;
                double y = p.Y - tn.Height / 2 - 2;
                if (y + tn.Height + t1.Height + t2.Height + 6 > H - 4) y = H - 4 - (tn.Height + t1.Height + t2.Height + 6);
                if (y < 4) y = 4;
                ctx.DrawRectangle(new ImmutableSolidColorBrush(box), null, new Rect(x - 4, y - 2, wmax + 8, tn.Height + t1.Height + t2.Height + 8), 4, 4);
                ctx.DrawText(tn, new Point(x < p.X ? x + wmax - tn.Width : x, y));
                ctx.DrawText(t1, new Point(x < p.X ? x + wmax - t1.Width : x, y + tn.Height + 2));
                ctx.DrawText(t2, new Point(x < p.X ? x + wmax - t2.Width : x, y + tn.Height + t1.Height + 4));
            }
            // la stazione dell'utente
            {
                Point p = P(Home.Lat, Home.Lon);
                Star(ctx, p, 11, Color.FromRgb(225, 29, 72));
                FormattedText t = Ft(HomeLabel, fs * 1.05, Color.FromRgb(225, 29, 72), FontWeight.Bold);
                double x = p.X + 12, y = p.Y + 4;
                if (x + t.Width > W - 4) x = p.X - 12 - t.Width;
                if (y + t.Height > H - 4) y = p.Y - 8 - t.Height;
                Label(ctx, t, x, y, box);
            }
            if (!string.IsNullOrEmpty(Title))
            {
                FormattedText t = Ft(Title, fs * 1.1, muted, FontWeight.Bold);
                ctx.DrawText(t, new Point(8, H - t.Height - 6));
            }
            ctx.DrawRectangle(null, new Pen(new ImmutableSolidColorBrush(border), 1), new Rect(0.5, 0.5, W - 1, H - 1));
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

        static void DrawLines(DrawingContext ctx, List<System.Drawing.PointF[]> lines, IPen pen, Func<double, double, Point> P)
        {
            if (lines == null) return;
            foreach (System.Drawing.PointF[] l in lines)
            {
                if (l == null || l.Length < 2) continue;
                StreamGeometry g = new StreamGeometry();
                using (StreamGeometryContext c = g.Open())
                {
                    c.BeginFigure(P(l[0].Y, l[0].X), false);
                    for (int i = 1; i < l.Length; i++) c.LineTo(P(l[i].Y, l[i].X));
                    c.EndFigure(false);
                }
                ctx.DrawGeometry(null, pen, g);
            }
        }

        // arco di cerchio massimo tra due punti (punteggiato)
        static void GreatCircle(DrawingContext ctx, LatLon a, LatLon b, Color col, Func<double, double, Point> P)
        {
            double d = Astro.DistanceKm(a, b);
            if (d < 1) return;
            double brg = Astro.Bearing(a, b);
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext c = g.Open())
            {
                c.BeginFigure(P(a.Lat, a.Lon), false);
                for (int i = 1; i <= 60; i++)
                {
                    LatLon q = Astro.Destination(a, brg, d * i / 60.0);
                    c.LineTo(P(q.Lat, q.Lon));
                }
                c.EndFigure(false);
            }
            ctx.DrawGeometry(null, new Pen(new ImmutableSolidColorBrush(col), 1.3, new ImmutableDashStyle(new double[] { 1, 3 }, 0)), g);
        }
    }
}
