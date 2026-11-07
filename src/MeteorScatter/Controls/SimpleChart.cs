// YA-MS - grafico semplice a linee/barre (sfondo scuro) e dati dei radianti per la volta celeste
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YAMS
{
    public class RadiantMark
    {
        public string Code;
        public double Az, El, Eff, LeftFrac, Zhr;
        public List<System.Drawing.PointF> Track = new List<System.Drawing.PointF>();  // (az, el) ogni 10 minuti nelle 24 ore
        public List<int> TrackHours = new List<int>();   // ora UTC dei punti a ora intera (-1 altrimenti)
    }

    public class ChartSeries
    {
        public string Name;
        public Color Color;
        public double[] Values;   // valori < 0 = mancanti
        public bool Bars;
        public bool RightAxis;
        public double LineWidth = 2;
        public bool Dotted;       // linea a puntini (es. confronto DH9GCD)
    }

    // Grafico semplice a linee/barre, sfondo scuro
    public class SimpleChart : Control
    {
        public string Title = "", YLabel = "", Y2Label = "";
        public string[] XLabels = new string[0];
        public int XLabelEvery = 1;
        // seconda riga sotto l'asse x (es. ora locale) con le didascalie delle due righe a sinistra
        public string[] XLabels2 = null;
        public string XRow1Name = "", XRow2Name = "";
        public List<ChartSeries> Series = new List<ChartSeries>();
        public double YMaxFixed = 0, Y2MaxFixed = 0;
        public int MarkIndex = -1;   // linea verticale (es. ora attuale)

        // scritte degli assi e della legenda: colore dello schema scelto
        static Color TextCol { get { return Pal.ChartText; } }
        const double F8 = 10.7, F10 = 13.3;

        public SimpleChart()
        {
            ClipToBounds = true;
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

        static double NiceMax(double v)
        {
            if (v <= 0) return 1;
            double p = Math.Pow(10, Math.Floor(Math.Log10(v)));
            double[] k = { 1, 1.2, 1.5, 2, 2.5, 3, 4, 5, 6, 8, 10 };
            foreach (double x in k) if (x * p >= v) return x * p;
            return 10 * p;
        }

        static FormattedText Ft(string s, double size, Color c, FontWeight w = FontWeight.Normal)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, FontStyle.Normal, w), size * UiScale.Font, new ImmutableSolidColorBrush(c));
        }

        // testo ruotato di 90° (si legge dal basso verso l'alto), centrato verticalmente su cy
        static void VText(DrawingContext ctx, FormattedText ft, double x, double cy)
        {
            using (ctx.PushTransform(Matrix.CreateRotation(-Math.PI / 2) * Matrix.CreateTranslation(x, cy)))
                ctx.DrawText(ft, new Point(-ft.Width / 2, 0));
        }

        public override void Render(DrawingContext ctx)
        {
            double Wd = Bounds.Width, Ht = Bounds.Height;
            ctx.DrawRectangle(Pal.B(Pal.ChartBg), null, new Rect(0, 0, Wd, Ht));
            int n = XLabels != null ? XLabels.Length : 0;
            string[] xl = XLabels ?? new string[0];
            List<ChartSeries> series = Series ?? new List<ChartSeries>();
            foreach (ChartSeries s in series) if (s.Values != null) n = Math.Max(n, s.Values.Length);
            double ymax = 0, y2max = 0;
            bool any2 = false;
            foreach (ChartSeries s in series)
            {
                if (s.RightAxis) any2 = true;
                if (s.Values != null)
                    foreach (double v in s.Values) { if (s.RightAxis) y2max = Math.Max(y2max, v); else ymax = Math.Max(ymax, v); }
            }
            ymax = YMaxFixed > 0 ? YMaxFixed : NiceMax(ymax);
            y2max = Y2MaxFixed > 0 ? Y2MaxFixed : NiceMax(y2max);
            // margini che si allargano con il carattere: titolo, scritte degli assi, etichette ruotate
            FormattedText titleFt = Ft(Title, F10, Tema.Schema == Tema.SchemaScia ? Pal.ChartText : Pal.Fore, FontWeight.SemiBold);
            double lh = Ft("0", F8, TextCol).Height;
            double yw = 0, y2w = 0;
            for (int i = 0; i <= 5; i++)
            {
                yw = Math.Max(yw, Ft(Fmt(ymax * i / 5), F8, TextCol).Width);
                if (any2) y2w = Math.Max(y2w, Ft(Fmt(y2max * i / 5), F8, TextCol).Width);
            }
            double ml = Math.Max(58, 4 + (string.IsNullOrEmpty(YLabel) ? 0 : lh + 4) + yw + 8);
            double mr = Math.Max(58, 4 + (string.IsNullOrEmpty(Y2Label) ? 0 : lh + 4) + y2w + 8);
            double mt = Math.Max(30, titleFt.Height + 12);
            bool row2 = XLabels2 != null && XLabels2.Length > 0;
            double mb = Math.Max(42, lh + 28) + (row2 ? lh + 2 : 0);
            double pw = Wd - ml - mr, ph = Ht - mt - mb;
            if (pw < 50 || ph < 30) return;
            Rect pr = new Rect(ml, mt, pw, ph);

            IPen grid = Pal.P(Pal.ChartGrid, 1);
            IPen axis = Pal.P(Color.FromRgb(150, 150, 150), 1);
            ctx.DrawText(titleFt, new Point(pr.Left, 6));
            for (int i = 0; i <= 5; i++)
            {
                double y = pr.Bottom - pr.Height * i / 5.0;
                ctx.DrawLine(grid, new Point(pr.Left, y), new Point(pr.Right, y));
                FormattedText lab = Ft(Fmt(ymax * i / 5), F8, TextCol);
                ctx.DrawText(lab, new Point(pr.Left - lab.Width - 6, y - lab.Height / 2));
                if (any2)
                {
                    FormattedText l2 = Ft(Fmt(y2max * i / 5), F8, TextCol);
                    ctx.DrawText(l2, new Point(pr.Right + 6, y - l2.Height / 2));
                }
            }
            // etichette degli assi verticali, ruotate
            if (!string.IsNullOrEmpty(YLabel))
                VText(ctx, Ft(YLabel, F8, TextCol), 4, pr.Top + pr.Height / 2);
            if (!string.IsNullOrEmpty(Y2Label))
            {
                FormattedText l2 = Ft(Y2Label, F8, TextCol);
                VText(ctx, l2, Wd - l2.Height - 4, pr.Top + pr.Height / 2);
            }
            if (n == 0) { ctx.DrawRectangle(null, axis, pr); return; }
            double bw = pr.Width / n;
            // meno scritte sull'asse x se si sovrapporrebbero (caratteri grandi o grafico stretto)
            double xw = 0;
            foreach (string t in xl) if (t != null) xw = Math.Max(xw, Ft(t, F8, TextCol).Width);
            int every = Math.Max(1, XLabelEvery);
            while (every < n && bw * every < xw + 6) every += Math.Max(1, XLabelEvery);
            for (int i = 0; i < n; i++)
                if (i < xl.Length && (i % every == 0))
                {
                    double x = pr.Left + bw * (i + 0.5);
                    ctx.DrawLine(grid, new Point(x, pr.Top), new Point(x, pr.Bottom));
                    FormattedText lab = Ft(xl[i], F8, TextCol);
                    ctx.DrawText(lab, new Point(x - lab.Width / 2, pr.Bottom + 6));
                    if (row2 && i < XLabels2.Length && !string.IsNullOrEmpty(XLabels2[i]))
                    {
                        FormattedText l2 = Ft(XLabels2[i], F8, Pal.OrangeText, FontWeight.SemiBold);
                        ctx.DrawText(l2, new Point(x - l2.Width / 2, pr.Bottom + 8 + lh));
                    }
                }
            if (row2)
            {
                // didascalie delle due righe, allineate a destra subito prima dell'asse
                if (!string.IsNullOrEmpty(XRow1Name))
                {
                    FormattedText c1 = Ft(XRow1Name, F8, TextCol);
                    ctx.DrawText(c1, new Point(pr.Left - c1.Width - 10, pr.Bottom + 6));
                }
                if (!string.IsNullOrEmpty(XRow2Name))
                {
                    FormattedText c2 = Ft(XRow2Name, F8, Pal.OrangeText, FontWeight.SemiBold);
                    ctx.DrawText(c2, new Point(pr.Left - c2.Width - 10, pr.Bottom + 8 + lh));
                }
            }

            using (ctx.PushClip(pr.Inflate(1)))
            {
                // prima le barre, poi le linee sopra
                foreach (ChartSeries s in series)
                {
                    if (s.Values == null || !s.Bars) continue;
                    double top = s.RightAxis ? y2max : ymax;
                    IBrush b = Pal.B(Pal.A(170, s.Color));
                    for (int i = 0; i < s.Values.Length; i++)
                    {
                        double v = s.Values[i];
                        if (v < 0) continue;
                        double y = pr.Bottom - pr.Height * Math.Min(1.0, v / top);
                        ctx.DrawRectangle(b, null, new Rect(pr.Left + bw * i + 1 + bw * 0.1, y, Math.Max(1, bw * 0.8 - 2), Math.Max(0, pr.Bottom - y)));
                    }
                }
                foreach (ChartSeries s in series)
                {
                    if (s.Values == null || s.Bars) continue;
                    double top = s.RightAxis ? y2max : ymax;
                    StreamGeometry g = new StreamGeometry();
                    using (StreamGeometryContext gc = g.Open())
                    {
                        bool open = false;
                        for (int i = 0; i < s.Values.Length; i++)
                        {
                            double v = s.Values[i];
                            if (v < 0) { if (open) gc.EndFigure(false); open = false; continue; }
                            Point pt = new Point(pr.Left + bw * (i + 0.5), pr.Bottom - pr.Height * Math.Min(1.0, v / top));
                            if (!open) { gc.BeginFigure(pt, false); open = true; }
                            else gc.LineTo(pt);
                        }
                        if (open) gc.EndFigure(false);
                    }
                    ctx.DrawGeometry(null, SeriesPen(s), g);
                }
            }
            ctx.DrawRectangle(null, axis, pr);
            if (MarkIndex >= 0 && MarkIndex < n)
            {
                double x = pr.Left + bw * (MarkIndex + 0.5);
                ctx.DrawLine(Pal.P(Pal.A(200, Pal.Fore), 1, Pal.Dot), new Point(x, pr.Top), new Point(x, pr.Bottom));
            }
            DrawLegend(ctx, series, pr);
        }

        static IPen SeriesPen(ChartSeries s)
        {
            IImmutableBrush br = new ImmutableSolidColorBrush(s.Color);
            if (s.Dotted) return new ImmutablePen(br, s.LineWidth, new ImmutableDashStyle(new double[] { 1, 2 }, 0), PenLineCap.Flat, PenLineJoin.Round);
            return new ImmutablePen(br, s.LineWidth, null, PenLineCap.Round, PenLineJoin.Round);
        }

        // Legenda in un riquadro scuro in alto a sinistra: campione di linea (o quadratino per le barre) e nome
        void DrawLegend(DrawingContext ctx, List<ChartSeries> series, Rect pr)
        {
            List<ChartSeries> items = new List<ChartSeries>();
            foreach (ChartSeries s in series) if (s.Values != null && !string.IsNullOrEmpty(s.Name)) items.Add(s);
            if (items.Count == 0) return;
            double S = UiScale.Font;
            double sample = 22 * S, gap = 18 * S, pad = 6, rowH = Math.Max(18, Ft("Ag", F8, TextCol).Height + 4);
            double maxW = pr.Width - 8;
            // disposizione su una o piu' righe
            List<FormattedText> fts = new List<FormattedText>();
            List<Point> pos = new List<Point>();
            double x = 0, y = 0, boxW = 0;
            foreach (ChartSeries s in items)
            {
                FormattedText ft = Ft(s.Name, F8, TextCol);
                double w = sample + 6 + ft.Width;
                if (x > 0 && x + w > maxW - 2 * pad) { x = 0; y += rowH; }
                fts.Add(ft); pos.Add(new Point(x, y));
                x += w + gap;
                boxW = Math.Max(boxW, x - gap);
            }
            double bx = pr.Left + 4, by = pr.Top + 4;
            Rect box = new Rect(bx, by, boxW + 2 * pad, y + rowH + 2 * pad - 4);
            ctx.DrawRectangle(Pal.B(Pal.A(235, Pal.Card)), Pal.P(Pal.Border, 1), box, 3, 3);
            for (int i = 0; i < items.Count; i++)
            {
                ChartSeries s = items[i];
                double ix = bx + pad + pos[i].X, iy = by + pad + pos[i].Y;
                double cyl = iy + rowH / 2 - 2;
                if (s.Bars)
                    ctx.DrawRectangle(Pal.B(Pal.A(170, s.Color)), null, new Rect(ix + sample / 2 - 5, cyl - 5, 10, 10));
                else
                    ctx.DrawLine(SeriesPen(s), new Point(ix, cyl), new Point(ix + sample, cyl));
                ctx.DrawText(fts[i], new Point(ix + sample + 6, cyl - fts[i].Height / 2));
            }
        }

        static string Fmt(double v)
        {
            if (v >= 100) return v.ToString("F0");
            if (v >= 10) return v.ToString("F0");
            if (v >= 1) return v.ToString("F1");
            return v.ToString("F2");
        }
    }
}
