// YA-MS - sezione: le scie piu' probabili viste dall'antenna puntata verso il corrispondente
//
// Asse orizzontale = azimut rispetto alla direzione del corrispondente (A a sinistra, B a destra),
// asse verticale = elevazione vista dalla stazione. Ogni scia e' un segmento centrato nel punto di
// riflessione (quota delle scie) e orientato lungo la direzione di arrivo dello sciame (verso il radiante),
// da circa 10 km sopra a 10 km sotto la quota: cosi' si vede l'inclinazione con cui le scie attraversano
// il lobo. Le scie sono quelle che soddisfano la condizione di riflessione speculare (capitolo 5 del
// manuale): se lo sciame scorre da nord a sud, le riflessioni utili cadono a est e a ovest.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YAMS
{
    public class TrailSeg
    {
        public Vec3 P;       // punto di riflessione (ECEF, km)
        public Vec3 S;       // direzione del radiante (versore)
        public double W;     // peso: ZHR x contributo all'efficienza
        public string Code;
        public bool Left;
    }

    public class SectionView : Control
    {
        // scala del carattere per le scritte disegnate: quella scelta con il cursore, ma non oltre
        // una misura proporzionata al disegno (su una vista piccola le scritte enormi coprirebbero tutto)
        double LS { get { return Math.Max(1.0, Math.Min(UiScale.Font, Bounds.Height / 170)); } }

        public PathModel Model;
        public List<TrailSeg> Trails = new List<TrailSeg>();
        public double AntennaAz = 0, HpbwDeg = 0, HpbwVDeg = 0, LobeElevDeg = 0;   // lobo orizzontale e verticale
        double VLobe { get { return HpbwVDeg > 0 ? HpbwVDeg : HpbwDeg; } }
        public string Note = "";


        public SectionView()
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

        // azimut (relativo al corrispondente) ed elevazione di un punto ECEF visto dalla stazione
        bool View(Vec3 q, out double daz, out double el)
        {
            Vec3 pa = Astro.Vec(Model.Home, Astro.R + Model.HomeAltKm);
            Vec3 r = q - pa;
            double az;
            Astro.AltAz(Model.Home, r.Unit, out az, out el);
            daz = Astro.Norm180(az - AntennaAz);
            return el > -5;
        }

        static FormattedText Ft(string s, double size, Color c, FontWeight w = FontWeight.Normal)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, FontStyle.Normal, w), size * drawScale, new ImmutableSolidColorBrush(c));
        }

        // scala usata dai metodi statici che creano i testi (impostata all'inizio di ogni disegno)
        static double drawScale = 1.0;

        public override void Render(DrawingContext ctx)
        {
            drawScale = LS;
            double Wd = Bounds.Width, Ht = Bounds.Height;
            ctx.DrawRectangle(Pal.B(Pal.Back), null, new Rect(0, 0, Wd, Ht));
            FormattedText title = Ft(Lang.F("SEZIONE - le scie più probabili viste dall'antenna verso il corrispondente (az. {0}°)", AntennaAz.ToString("F0")),
                12.7, Pal.Fore, FontWeight.SemiBold);
            title.MaxTextWidth = Math.Max(50, Wd - 12);   // va a capo se lo spazio non basta
            ctx.DrawText(title, new Point(6, 4));
            if (Model == null || !Model.Valid) return;

            const double fs = 10;   // 7.5 pt
            Color tb = Tema.Scuro ? Pal.Argb(200, 225, 225, 235) : Pal.Muted;   // scritte degli assi, fuori dal cielo
            // margini che crescono con il carattere: a sinistra le elevazioni, sotto due righe di scritte
            FormattedText probe = Ft("+60°", fs, tb);
            double left = Math.Max(44, Ft("55°", fs, tb).Width + 14), top = Math.Max(28, title.Height + 10), bottom = Math.Max(40, 2 * probe.Height + 14);
            double pw = Wd - left - 14, ph = Ht - top - bottom;
            if (pw < 80 || ph < 40) return;
            Rect pr = new Rect(left, top, pw, ph);
            double xr = Math.Max(60, Math.Ceiling(HpbwDeg * 0.9 / 10) * 10);   // ± gradi di azimut mostrati
            double ymin = -3, ymax = 25;
            foreach (HotPoint h in new HotPoint[] { Model.HotA, Model.HotB })
                if (h != null) ymax = Math.Max(ymax, h.ElevFromHome + 8);
            ymax = Math.Max(ymax, LobeElevDeg + VLobe / 2 + 3);
            ymax = Math.Min(60, Math.Ceiling(ymax / 5) * 5);

            Func<double, double> X = a => pr.Left + (a + xr) / (2 * xr) * pr.Width;
            Func<double, double> Y = v => pr.Bottom - (v - ymin) / (ymax - ymin) * pr.Height;

            // cielo e terreno
            LinearGradientBrush sky = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative)
            };
            sky.GradientStops.Add(new GradientStop(Color.FromRgb(14, 22, 70), 0));
            sky.GradientStops.Add(new GradientStop(Color.FromRgb(30, 50, 140), 1));
            ctx.DrawRectangle(sky, null, pr);
            ctx.DrawRectangle(Pal.B(Color.FromRgb(40, 34, 30)), null, new Rect(pr.Left, Y(0), pr.Width, pr.Bottom - Y(0)));

            IPen grid = Pal.P(Pal.Argb(55, 200, 200, 230), 1);
            IPen hor = Pal.P(Pal.Argb(160, 210, 190, 150), 1.2);
            // con caratteri grandi le scritte degli assi si diradano per non sovrapporsi
            double px10 = pr.Width * 10 / (2 * xr);
            int kx = 1; while (kx < 12 && kx * px10 < probe.Width + 6) kx++;
            double px5 = pr.Height * 5 / (ymax - ymin);
            int ky = 1; while (ky < 12 && ky * px5 < probe.Height + 2) ky++;
            for (double a = -xr; a <= xr + 0.1; a += 10)
            {
                ctx.DrawLine(grid, new Point(X(a), pr.Top), new Point(X(a), pr.Bottom));
                if (Math.Round(a / 10) % kx != 0) continue;
                FormattedText t = Ft((a > 0 ? "+" : "") + a.ToString("F0") + "°", fs, tb);
                ctx.DrawText(t, new Point(X(a) - t.Width / 2, pr.Bottom + 2));
            }
            int iv = 0;
            for (double v = 0; v <= ymax + 0.1; v += 5, iv++)
            {
                ctx.DrawLine(grid, new Point(pr.Left, Y(v)), new Point(pr.Right, Y(v)));
                if (iv % ky != 0) continue;
                FormattedText t = Ft(v.ToString("F0") + "°", fs, tb);
                ctx.DrawText(t, new Point(pr.Left - t.Width - 5, Y(v) - t.Height / 2));
            }
            ctx.DrawLine(hor, new Point(pr.Left, Y(0)), new Point(pr.Right, Y(0)));
            double row2 = pr.Bottom + 4 + probe.Height;
            FormattedText sa = Ft(Lang.T("← A  (sinistra)"), fs, tb);
            ctx.DrawText(sa, new Point(pr.Left + 4, row2));
            FormattedText sb = Ft(Lang.T("(destra)  B →"), fs, tb);
            ctx.DrawText(sb, new Point(pr.Right - sb.Width - 4, row2));
            FormattedText sc = Ft(Lang.T("azimut rispetto al corrispondente"), fs, tb);
            // la scritta centrale solo se c'e' spazio tra le due laterali
            if (sc.Width + sa.Width + sb.Width + 24 < pr.Width)
                ctx.DrawText(sc, new Point(pr.Left + pr.Width / 2 - sc.Width / 2, row2));

            using (ctx.PushClip(pr))
            {
                // lobo dell'antenna (HPBW, cerchio a -3 dB attorno al puntamento)
                if (HpbwDeg > 0)
                {
                    double lx0 = X(-HpbwDeg / 2), lx1 = X(HpbwDeg / 2), ly0 = Y(LobeElevDeg + VLobe / 2), ly1 = Y(LobeElevDeg - VLobe / 2);
                    ctx.DrawEllipse(Pal.B(Pal.Argb(40, 255, 240, 200)), Pal.P(Pal.Argb(150, 255, 240, 200), 1),
                        new Rect(lx0, ly0, lx1 - lx0, ly1 - ly0));
                }

                // scie piu' probabili
                double wmax = 0;
                if (Trails != null) foreach (TrailSeg t in Trails) wmax = Math.Max(wmax, t.W);
                if (wmax > 0)
                {
                    List<TrailSeg> ord = new List<TrailSeg>(Trails);
                    ord.Sort((a, b) => a.W.CompareTo(b.W));   // le piu' forti sopra
                    foreach (TrailSeg t in ord)
                    {
                        double rel = t.W / wmax;
                        if (rel < 0.04) continue;
                        double up = Math.Abs(t.S.Dot(t.P.Unit));
                        double half = Math.Min(60, 10 / Math.Max(0.17, up));   // km lungo la scia per +-10 km di quota
                        Vec3 q1 = t.P + t.S * half, q2 = t.P - t.S * half;
                        double a0, e0, a1, e1, am, em;
                        if (!View(t.P, out am, out em)) continue;
                        View(q1, out a1, out e1); View(q2, out a0, out e0);
                        int alpha = (int)(60 + 195 * rel);
                        IPen sp = new ImmutablePen(new ImmutableSolidColorBrush(Pal.Argb(alpha, 255, 170, 60)), 1.2 + 2.3 * rel, null, PenLineCap.Round);
                        double r = 4 + 10 * rel;
                        ctx.DrawEllipse(Pal.B(Pal.Argb((int)(40 * rel), 255, 230, 160)), null, new Point(X(am), Y(em)), r, r);
                        ctx.DrawLine(sp, new Point(X(a0), Y(e0)), new Point(X(a1), Y(e1)));
                    }
                }
                else
                    ctx.DrawText(Ft(Lang.T("Nessuna scia prevista: sciami attivi sotto l'orizzonte o con efficienza nulla per questa tratta."), 12, SkyPal.Silver),
                        new Point(pr.Left + 10, pr.Top + 10));

            }
            ctx.DrawRectangle(null, Pal.P(Tema.Scuro ? Pal.Argb(120, 200, 200, 220) : Pal.Border, 1), pr);
            // etichette senza sovrapposizioni (con alone): prima i punti e la nota come ostacoli
            LabelPlacer lp = new LabelPlacer(pr);
            if (!string.IsNullOrEmpty(Note))
            {
                FormattedText ft = Ft(Note, 10, SkyPal.Silver);
                ft.MaxTextWidth = Math.Max(50, pr.Width - 8);
                Point np = new Point(Math.Max(pr.Left + 4, pr.Right - ft.Width - 4), pr.Top + 3);
                ctx.DrawText(ft, np);
                lp.Block(new Rect(np.X - 2, np.Y - 2, ft.Width + 4, ft.Height + 4));
            }
            // corrispondente (punto medio) e Hot A / Hot B
            const double fb = 11.3;   // 8.5 pt grassetto
            using (ctx.PushClip(pr))
            {
                Point pd = new Point(X(0), Y(Model.ElevToMidDeg));
                ctx.DrawEllipse(Pal.B(SkyPal.Green), null, pd, 4, 4);
                lp.BlockPoint(pd, 6);
                List<HotPoint> hl = new List<HotPoint>(); List<Point> hp = new List<Point>(); List<Color> hcs = new List<Color>();
                foreach (HotPoint h in new HotPoint[] { Model.HotA, Model.HotB })
                {
                    if (h == null) continue;
                    Color hc = h.Recommended ? SkyPal.Red : h.InLobe ? Pal.HotGrey : Pal.HotPink;
                    Point p = new Point(X(Astro.Norm180(h.BearingFromHome - AntennaAz)), Y(h.ElevFromHome));
                    ctx.DrawEllipse(Pal.B(hc), null, p, 5, 5);
                    lp.BlockPoint(p, 6);
                    hl.Add(h); hp.Add(p); hcs.Add(hc);
                }
                lp.Place(ctx, Ft("DX", fb, SkyPal.Green, FontWeight.Bold), pd, 0, false, false, SkyPal.Green);
                for (int i = 0; i < hl.Count; i++)
                    lp.Place(ctx, Ft(hl[i].Label, fb, hcs[i], FontWeight.Bold), hp[i], hl[i] == Model.HotA ? -45 : 45, false, false, hcs[i]);
            }
        }
    }
}
