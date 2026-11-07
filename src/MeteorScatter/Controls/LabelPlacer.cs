// YA-MS - posizionamento automatico delle etichette senza sovrapposizioni (pianta, volta celeste, sezione)
//
// Elenco di rettangoli occupati: prima gli ostacoli fissi (scritte della scala, simboli dei punti, linee
// campionate ogni ~6 px), poi le etichette in ordine di importanza. Per ogni etichetta si provano piu'
// direzioni attorno al punto (quella preferita per prima) a distanze crescenti: vince il primo rettangolo
// dentro l'area utile che non tocca nulla. Se l'etichetta finisce lontana, una lineetta la collega al punto.
// Ogni etichetta ha uno sfondo scuro semitrasparente arrotondato (alone) per restare leggibile.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YAMS
{
    public class LabelPlacer
    {
        public readonly List<Rect> Occ = new List<Rect>();
        public Rect Area;   // l'etichetta deve stare tutta qui dentro
        public static readonly Color Halo = Pal.Argb(184, 14, 11, 34);
        static readonly double[] Rings = { 0.6, 1.4, 2.6, 4.0, 5.6, 7.5, 10 };

        public LabelPlacer(Rect area) { Area = area; }

        public static FormattedText Text(string s, double size, Color c, bool bold)
        {
            return new FormattedText(s ?? "", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily.Default, FontStyle.Normal, bold ? FontWeight.Bold : FontWeight.Normal), size, new ImmutableSolidColorBrush(c));
        }

        // dimensioni del riquadro dell'etichetta (testo + margine dell'alone)
        public static Size BoxSize(FormattedText ft) { return new Size(ft.Width + 8, ft.Height + 2); }

        public void Block(Rect r) { Occ.Add(r); }

        // simbolo puntiforme: quadrato di lato 2 x half
        public void BlockPoint(Point p, double half) { Occ.Add(new Rect(p.X - half, p.Y - half, 2 * half, 2 * half)); }

        // linea campionata ogni step px come piccoli quadrati
        public void BlockLine(Point a, Point b, double step = 6, double half = 2.5)
        {
            double len = Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
            int n = Math.Max(2, (int)(len / step));
            for (int i = 0; i <= n; i++)
            {
                double x = a.X + (b.X - a.X) * i / n, y = a.Y + (b.Y - a.Y) * i / n;
                Occ.Add(new Rect(x - half, y - half, 2 * half, 2 * half));
            }
        }

        static bool Overlap(Rect a, Rect b)
        {
            return a.X < b.X + b.Width && b.X < a.X + a.Width && a.Y < b.Y + b.Height && b.Y < a.Y + a.Height;
        }

        public bool Free(Rect r)
        {
            if (r.X < Area.X + 2 || r.Y < Area.Y + 2 || r.X + r.Width > Area.X + Area.Width - 2 || r.Y + r.Height > Area.Y + Area.Height - 2) return false;
            foreach (Rect o in Occ) if (Overlap(r, o)) return false;
            return true;
        }

        // disegna l'etichetta nel rettangolo r (centrata) e lo segna come occupato
        public void Put(DrawingContext ctx, FormattedText ft, Rect r, bool halo)
        {
            Occ.Add(r);
            if (halo) ctx.DrawRectangle(new ImmutableSolidColorBrush(Halo), null, r, r.Height / 2.6, r.Height / 2.6);
            ctx.DrawText(ft, new Point(r.X + (r.Width - ft.Width) / 2, r.Y + (r.Height - ft.Height) / 2));
        }

        // rettangolo centrato in (x, y)
        public static Rect At(FormattedText ft, double x, double y)
        {
            Size s = BoxSize(ft);
            return new Rect(x - s.Width / 2, y - s.Height / 2, s.Width, s.Height);
        }

        // Cerca un posto libero attorno ad anchor. prefDeg: direzione preferita sullo schermo (0 = su, 90 = destra).
        // sideOnly: solo pref, +-30, +-60, +-85 (resta da un lato di una linea). maxRing: distanza massima (indice).
        // optional: se non c'e' posto l'etichetta non viene disegnata; altrimenti va nella posizione preferita.
        public bool Place(DrawingContext ctx, FormattedText ft, Point anchor, double prefDeg, bool sideOnly, bool optional,
                          Color lineColor, bool halo = true, int maxRing = 6)
        {
            Size s = BoxSize(ft);
            double w = s.Width, h = s.Height;
            double[] offs = sideOnly ? new double[] { 0, 30, -30, 60, -60, 85, -85 } : new double[] { 0, 45, -45, 90, -90, 135, -135, 180 };
            for (int ring = 0; ring < Rings.Length && ring <= maxRing; ring++)
            {
                double d = Rings[ring] * h * 0.6;
                foreach (double o in offs)
                {
                    double a = (prefDeg + o) * Math.PI / 180.0;
                    double sx = Math.Sin(a), sy = -Math.Cos(a);
                    double x = anchor.X + sx * (w / 2 + 8 + d), y = anchor.Y + sy * (h / 2 + 6 + d);
                    Rect r = new Rect(x - w / 2, y - h / 2, w, h);
                    if (!Free(r)) continue;
                    if (ring >= 2)
                    {
                        // lineetta di collegamento dal punto al bordo vicino dell'etichetta
                        IPen lp = new ImmutablePen(new ImmutableSolidColorBrush(Color.FromArgb(200, lineColor.R, lineColor.G, lineColor.B)), 0.8);
                        ctx.DrawLine(lp, anchor, new Point(x - sx * w / 2 * 0.9, y - sy * h / 2 * 0.9));
                    }
                    Put(ctx, ft, r, halo);
                    return true;
                }
            }
            if (optional) return false;
            // nessun posto libero: posizione preferita comunque (l'informazione non va persa)
            {
                double a = prefDeg * Math.PI / 180.0;
                double x = anchor.X + Math.Sin(a) * (w / 2 + 8), y = anchor.Y - Math.Cos(a) * (h / 2 + 6);
                Put(ctx, ft, new Rect(x - w / 2, y - h / 2, w, h), halo);
            }
            return true;
        }
    }
}
