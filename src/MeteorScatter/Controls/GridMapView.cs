// YA-MS - mappa dei quadrati Maidenhead da un file ADIF (nessun collegamento a LoTW)
// Mappa equirettangolare dell'Europa con i quadrati 2° x 1°; rotellina = zoom, trascinamento = spostamento,
// mouse sopra un quadrato = elenco dei QSO (suggerimento). I dati sono in AdifStore (src/Core/AdifStore.cs).
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YAMS
{
    public class GridQso
    {
        public string Band, Grid, Call, Date;
        public bool Confirmed;
    }

    // Mappa equirettangolare dell'Europa con i quadrati 2° x 1°; rotellina = zoom, trascinamento = spostamento
    public class GridMapView : Control
    {
        public string Band;
        public string HomeGrid = "";
        public List<System.Drawing.PointF[]> Coast = new List<System.Drawing.PointF[]>(), Borders = new List<System.Drawing.PointF[]>();
        double lon0 = -32, lon1 = 52, lat0 = 26, lat1 = 74;
        Point dragStart; bool dragging; double dLon0, dLat0;
        string lastTip = "";

        static readonly double CosLat = Math.Cos(50 * Astro.D2R);
        static readonly Color Gainsboro = Color.FromRgb(220, 220, 220);
        static readonly Color White = Color.FromRgb(255, 255, 255);

        public GridMapView()
        {
            ClipToBounds = true;
            Focusable = true;
        }

        // ridisegno quando cambia la scala del carattere
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

        Dictionary<string, List<GridQso>> ByGrid()
        {
            Dictionary<string, List<GridQso>> d = new Dictionary<string, List<GridQso>>();
            foreach (GridQso q in AdifStore.Qsos)
            {
                if (q.Band != Band) continue;
                List<GridQso> l;
                if (!d.TryGetValue(q.Grid, out l)) { l = new List<GridQso>(); d[q.Grid] = l; }
                l.Add(q);
            }
            return d;
        }

        double Kx { get { return Math.Max(1, Bounds.Width) / ((lon1 - lon0) * CosLat); } }

        // mantiene le proporzioni: la latitudine visibile segue l'altezza
        void SyncAspect()
        {
            if (Bounds.Width > 0 && Bounds.Height > 0) lat0 = lat1 - Bounds.Height / Kx;
        }

        Point XY(double lon, double lat)
        {
            double k = Kx;
            return new Point((lon - lon0) * CosLat * k, (lat1 - lat) * k);
        }

        void ToLonLat(Point p, out double lon, out double lat)
        {
            double k = Kx;
            lon = lon0 + p.X / (CosLat * k);
            lat = lat1 - p.Y / k;
        }

        public static string GridAt(double lon, double lat)
        {
            if (lon < -180 || lon >= 180 || lat < -90 || lat >= 90) return null;
            double x = lon + 180, y = lat + 90;
            return new string(new char[] { (char)('A' + (int)(x / 20)), (char)('A' + (int)(y / 10)), (char)('0' + (int)((x % 20) / 2)), (char)('0' + (int)(y % 10)) });
        }

        static void GridOrigin(string g, out double lon, out double lat)
        {
            lon = (g[0] - 'A') * 20 - 180 + (g[2] - '0') * 2;
            lat = (g[1] - 'A') * 10 - 90 + (g[3] - '0');
        }

        static FormattedText Ft(string s, double size, Color c, FontWeight w = FontWeight.Normal)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, FontStyle.Normal, w), size * UiScale.Font, new ImmutableSolidColorBrush(c));
        }

        static Rect RectOf(Point a, Point b)
        {
            return new Rect(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
        }

        public override void Render(DrawingContext ctx)
        {
            double Wd = Bounds.Width, Ht = Bounds.Height;
            ctx.DrawRectangle(Pal.B(SkyPal.Deep), null, new Rect(0, 0, Wd, Ht));   // mappa scura fissa (come i cieli), scritte chiare
            if (Wd < 4 || Ht < 4) return;
            SyncAspect();
            Dictionary<string, List<GridQso>> d = ByGrid();
            int conf = 0;
            // quadrati lavorati
            IBrush bConf = Pal.B(Pal.Argb(200, 60, 170, 90)), bWork = Pal.B(Pal.Argb(200, 230, 130, 30));
            foreach (KeyValuePair<string, List<GridQso>> kv in d)
            {
                if (kv.Key == null || kv.Key.Length < 4) continue;
                double lo, la; GridOrigin(kv.Key, out lo, out la);
                Point a = XY(lo, la + 1), b = XY(lo + 2, la);
                bool c = false; foreach (GridQso q in kv.Value) if (q.Confirmed) c = true;
                if (c) conf++;
                ctx.DrawRectangle(c ? bConf : bWork, null, RectOf(a, b));
            }
            // reticolo: quadrati e campi
            IPen sq = Pal.P(Pal.Argb(35, 200, 200, 220), 1);
            IPen fld = Pal.P(Pal.Argb(110, 200, 200, 230), 1);
            Color fc = Pal.Argb(120, 200, 210, 240);
            {
                double k = Kx;
                bool fine = 2 * CosLat * k > 7;
                for (int lo = -180; lo <= 180; lo += 2)
                {
                    if (lo < lon0 - 2 || lo > lon1 + 2) continue;
                    bool isF = (lo + 180) % 20 == 0;
                    if (!isF && !fine) continue;
                    Point p = XY(lo, 0); ctx.DrawLine(isF ? fld : sq, new Point(p.X, 0), new Point(p.X, Ht));
                }
                for (int la = -90; la <= 90; la++)
                {
                    if (la < lat0 - 1 || la > lat1 + 1) continue;
                    bool isF = (la + 90) % 10 == 0;
                    if (!isF && !fine) continue;
                    Point p = XY(0, la); ctx.DrawLine(isF ? fld : sq, new Point(0, p.Y), new Point(Wd, p.Y));
                }
                for (int lo = -180; lo < 180; lo += 20)
                    for (int la = -90; la < 90; la += 10)
                    {
                        if (lo + 20 < lon0 || lo > lon1 || la + 10 < lat0 || la > lat1) continue;
                        Point p = XY(lo + 10, la + 5);
                        string name = new string(new char[] { (char)('A' + (lo + 180) / 20), (char)('A' + (la + 90) / 10) });
                        FormattedText ft = Ft(name, 10.7, fc, FontWeight.Bold);
                        ctx.DrawText(ft, new Point(p.X - ft.Width / 2, p.Y - ft.Height / 2));
                    }
            }
            // coste e confini
            DrawLines(ctx, Borders, Pal.P(Pal.Argb(110, 140, 150, 180), 0.8, Pal.Dot));
            DrawLines(ctx, Coast, Pal.P(Pal.Argb(200, 120, 170, 210), 1));
            // quadrato di casa
            if (HomeGrid != null && HomeGrid.Length >= 4)
            {
                string hg = HomeGrid.ToUpperInvariant();
                if (hg[0] >= 'A' && hg[0] <= 'R' && hg[1] >= 'A' && hg[1] <= 'R' && char.IsDigit(hg[2]) && char.IsDigit(hg[3]))
                {
                    double lo, la; GridOrigin(hg, out lo, out la);
                    Point a = XY(lo, la + 1), b = XY(lo + 2, la);
                    ctx.DrawRectangle(null, Pal.P(White, 2), RectOf(a, b));
                }
            }
            // riquadro informativo
            {
                FormattedText t = Ft(Lang.F("{0}: {1} quadrati lavorati, {2} confermati", BandLabel(), d.Count, conf), 12, White, FontWeight.Bold);
                FormattedText t1 = Ft(Lang.T("lavorato"), 11, Gainsboro);
                FormattedText t2 = Ft(Lang.T("confermato (QSL o LoTW)"), 11, Gainsboro);
                // altezze ricavate dal testo: il riquadro cresce con il carattere
                double bw = Math.Max(t.Width, 30 + t1.Width + 30 + t2.Width) + 14;
                double ry = 9 + t.Height + 4, rh = Math.Max(t1.Height, 10), cyr = ry + rh / 2;
                ctx.DrawRectangle(Pal.B(Pal.Argb(200, 20, 20, 30)), null, new Rect(6, 6, bw, ry + rh + 4 - 6));
                ctx.DrawText(t, new Point(12, 9));
                ctx.DrawRectangle(Pal.B(Color.FromRgb(230, 130, 30)), null, new Rect(12, cyr - 5, 10, 10));
                ctx.DrawText(t1, new Point(26, cyr - t1.Height / 2));
                double x2 = 26 + t1.Width + 16;
                ctx.DrawRectangle(Pal.B(Color.FromRgb(60, 170, 90)), null, new Rect(x2, cyr - 5, 10, 10));
                ctx.DrawText(t2, new Point(x2 + 14, cyr - t2.Height / 2));
            }
        }

        string BandLabel()
        {
            for (int i = 0; i < AdifStore.Bands.Length; i++) if (AdifStore.Bands[i] == Band) return AdifStore.BandNames[i];
            return Band;
        }

        void DrawLines(DrawingContext ctx, List<System.Drawing.PointF[]> lines, IPen p)
        {
            if (lines == null || lines.Count == 0) return;
            // una sola geometria per tutte le linee; si saltano quelle tutte fuori dalla vista
            double m = 5;
            StreamGeometry g = new StreamGeometry();
            bool any = false;
            using (StreamGeometryContext gc = g.Open())
            {
                foreach (System.Drawing.PointF[] line in lines)
                {
                    if (line == null || line.Length < 2) continue;
                    bool inside = false;
                    foreach (System.Drawing.PointF q in line)
                        if (q.X >= lon0 - m && q.X <= lon1 + m && q.Y >= lat0 - m && q.Y <= lat1 + m) { inside = true; break; }
                    if (!inside) continue;
                    gc.BeginFigure(XY(line[0].X, line[0].Y), false);
                    for (int i = 1; i < line.Length; i++) gc.LineTo(XY(line[i].X, line[i].Y));
                    gc.EndFigure(false);
                    any = true;
                }
            }
            if (any) ctx.DrawGeometry(null, p, g);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            if (e.Delta.Y == 0) return;
            SyncAspect();
            double lon, lat; ToLonLat(e.GetPosition(this), out lon, out lat);
            double z = e.Delta.Y > 0 ? 0.8 : 1.25;
            double w = (lon1 - lon0) * z;
            e.Handled = true;
            if (w < 6 || w > 200) return;
            double fx = (lon - lon0) / (lon1 - lon0), fy = (lat1 - lat) / (lat1 - lat0);
            double h = (lat1 - lat0) * z;
            lon0 = lon - fx * w; lon1 = lon0 + w; lat1 = lat + fy * h; lat0 = lat1 - h;
            InvalidateVisual();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            Focus();
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                SyncAspect();
                dragging = true; dragStart = e.GetPosition(this); dLon0 = lon0; dLat0 = lat1;
                e.Pointer.Capture(this);
                e.Handled = true;
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (dragging) { dragging = false; e.Pointer.Capture(null); }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            Point pos = e.GetPosition(this);
            if (dragging)
            {
                double k = Kx;
                double w = lon1 - lon0, h = lat1 - lat0;
                lon0 = dLon0 - (pos.X - dragStart.X) / (CosLat * k); lon1 = lon0 + w;
                lat1 = dLat0 + (pos.Y - dragStart.Y) / k; lat0 = lat1 - h;
                InvalidateVisual(); return;
            }
            double lon, lat; ToLonLat(pos, out lon, out lat);
            string gr = GridAt(lon, lat);
            string t = "";
            if (gr != null)
            {
                List<GridQso> l = new List<GridQso>();
                foreach (GridQso q in AdifStore.Qsos) if (q.Band == Band && q.Grid == gr) l.Add(q);
                t = l.Count == 0 ? Lang.F("{0}: non lavorato", gr) : Lang.F("{0}: {1} QSO", gr, l.Count);
                for (int i = 0; i < Math.Min(6, l.Count); i++) t += "\n" + l[i].Call + "  " + l[i].Date + (l[i].Confirmed ? "  " + Lang.T("(confermato)") : "");
                if (l.Count > 6) t += "\n...";
            }
            if (t != lastTip)
            {
                lastTip = t;
                ToolTip.SetTip(this, t.Length > 0 ? t : null);
            }
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            lastTip = "";
            ToolTip.SetTip(this, null);
        }
    }
}
