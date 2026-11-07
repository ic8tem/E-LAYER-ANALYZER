// YA-MS - tavolozza "Scia meteorica" (azzurro, viola, verde chiaro, giallo, arancione) e caratteri
using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YAMS
{
    public static class Pal
    {
        public static Color Back = Color.FromRgb(22, 18, 48);       // sfondo
        public static Color Panel = Color.FromRgb(28, 23, 64);      // riquadri
        public static Color Panel2 = Color.FromRgb(34, 28, 71);     // righe dei comandi
        public static Color Panel3 = Color.FromRgb(43, 33, 96);     // bottoni secondari, etichette a pillola
        public static Color Track = Color.FromRgb(14, 11, 34);      // fondo della barra delle linguette
        public static Color Border = Color.FromRgb(58, 50, 112);
        public static Color Fore = Color.FromRgb(238, 235, 250);
        public static Color Muted = Color.FromRgb(179, 173, 214);
        public static Color Azure = Color.FromRgb(56, 189, 248);
        public static Color AzureDark = Color.FromRgb(11, 34, 51);
        public static Color Violet = Color.FromRgb(167, 139, 250);
        public static Color Green = Color.FromRgb(134, 239, 172);
        public static Color Yellow = Color.FromRgb(253, 224, 71);
        public static Color Orange = Color.FromRgb(251, 146, 60);
        public static Color OrangeDark = Color.FromRgb(58, 35, 18);
        public static Color OrangeText = Color.FromRgb(254, 215, 170);
        public static Color Pink = Color.FromRgb(240, 171, 252);
        public static Color Red = Color.FromRgb(248, 113, 113);
        public static Color HotPink = Color.FromRgb(240, 120, 240);     // Hot fuori dal lobo
        public static Color HotGrey = Color.FromRgb(150, 150, 150);     // Hot dentro il lobo
        public static Color Silver = Color.FromRgb(192, 192, 192);
        public static bool Light;   // schema chiaro (E-LAYER ANALYZER: colorazioni di YAEC)
        public static Color ChartText = Color.FromRgb(220, 220, 220);   // scritte dei grafici

        public static Color A(int alpha, Color c) { return Color.FromArgb((byte)alpha, c.R, c.G, c.B); }
        public static Color Rgb(int r, int g, int b) { return Color.FromRgb((byte)r, (byte)g, (byte)b); }
        public static Color Argb(int a, int r, int g, int b) { return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b); }
        public static IBrush B(Color c) { return new ImmutableSolidColorBrush(c); }
        public static IPen P(Color c, double w = 1) { return new ImmutablePen(new ImmutableSolidColorBrush(c), w); }
        public static IPen P(Color c, double w, ImmutableDashStyle dash) { return new ImmutablePen(new ImmutableSolidColorBrush(c), w, dash); }
        public static readonly ImmutableDashStyle Dash = new ImmutableDashStyle(new double[] { 4, 3 }, 0);
        public static readonly ImmutableDashStyle Dot = new ImmutableDashStyle(new double[] { 1, 2 }, 0);
        public static readonly ImmutableDashStyle DashDot = new ImmutableDashStyle(new double[] { 5, 2, 1, 2 }, 0);

        // carattere a spaziatura fissa: il primo disponibile sul sistema
        public static readonly FontFamily Mono = new FontFamily("Cascadia Mono, Consolas, DejaVu Sans Mono, Menlo, Liberation Mono, monospace");
        public static readonly FontFamily Ui = FontFamily.Default;
    }

    // Dimensione del carattere di tutto il programma (cursore "Carattere", 100-300 %): solo il testo,
    // linee, cerchi e mappe restano delle stesse dimensioni.
    public static class UiScale
    {
        public static double Font = 1.0;
        public static event Action Changed;

        public static void Set(double f)
        {
            if (double.IsNaN(f) || double.IsInfinity(f)) f = 1.0;
            f = Math.Max(1.0, Math.Min(3.0, f));
            if (Math.Abs(f - Font) < 1e-6) return;
            Font = f;
            Changed?.Invoke();
        }

        // valori di partenza (al 100 %) di ogni controllo, ricordati la prima volta che lo si incontra;
        // tabella "debole": i controlli eliminati (es. finestre di messaggio chiuse) non restano in memoria
        sealed class BaseVals { public double Font = double.NaN; public double Width = double.NaN; public double LineH = double.NaN; }
        static readonly ConditionalWeakTable<Control, BaseVals> bases = new ConditionalWeakTable<Control, BaseVals>();

        // Applica la scala ai controlli standard di una finestra (o di un ramo): percorre l'albero logico,
        // ricorda la FontSize di base e imposta FontSize = base x Font; le caselle di testo/numeri si allargano.
        public static void Apply(Control root)
        {
            if (root == null) return;
            One(root);
            foreach (ILogical l in root.GetLogicalDescendants())
            {
                Control c = l as Control;
                if (c != null) One(c);
            }
        }

        static bool HasText(Control c)
        {
            return c is TextBlock || c is TemplatedControl;   // TemplatedControl: Button, CheckBox, TextBox, NumericUpDown, ComboBox, ...
        }

        static void One(Control c)
        {
            if (!HasText(c)) return;
            BaseVals b;
            if (!bases.TryGetValue(c, out b))
            {
                b = new BaseVals();
                double cur = c is TextBlock ? ((TextBlock)c).FontSize : ((TemplatedControl)c).FontSize;
                // se il valore e' ereditato da un antenato gia' scalato, la base e' quella dell'antenato
                b.Font = cur;
                StyledElement p = c.Parent;
                while (p != null)
                {
                    Control pc = p as Control;
                    BaseVals pb;
                    if (pc != null && HasText(pc) && bases.TryGetValue(pc, out pb))
                    {
                        double pcur = pc is TextBlock ? ((TextBlock)pc).FontSize : ((TemplatedControl)pc).FontSize;
                        if (Math.Abs(pcur - cur) < 0.01 && !double.IsNaN(pb.Font)) b.Font = pb.Font;
                        break;
                    }
                    p = p.Parent;
                }
                if (c is TextBox || c is NumericUpDown || c is ComboBox || c is CalendarDatePicker || c is TextBlock)
                    b.Width = c.Width;   // NaN se la larghezza e' automatica
                if (c is TextBlock) b.LineH = ((TextBlock)c).LineHeight;   // NaN = interlinea automatica
                bases.Add(c, b);
            }
            if (!double.IsNaN(b.Font) && b.Font > 0)
            {
                double target = b.Font * Font;
                if (c is TextBlock)
                {
                    TextBlock t = (TextBlock)c;
                    if (Math.Abs(t.FontSize - target) > 0.01) t.FontSize = target;
                    if (!double.IsNaN(b.LineH) && b.LineH > 0 && Math.Abs(t.LineHeight - b.LineH * Font) > 0.01) t.LineHeight = b.LineH * Font;
                }
                else
                {
                    TemplatedControl t = (TemplatedControl)c;
                    if (Math.Abs(t.FontSize - target) > 0.01) t.FontSize = target;
                }
            }
            if (!double.IsNaN(b.Width) && b.Width > 0)
            {
                double tw = b.Width * Font;
                if (Math.Abs(c.Width - tw) > 0.5) c.Width = tw;
            }
        }
    }
}
