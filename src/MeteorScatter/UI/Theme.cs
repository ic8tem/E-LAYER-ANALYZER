// YA-MS - schemi di colori (come YAEC: Chiaro, Notte, Nero, pastelli; piu' "Scia meteorica") e caratteri
using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;

namespace YAMS
{
    // Colori dell'interfaccia: cambiano con lo schema scelto (Tema.Imposta). I nomi sono quelli della prima
    // tavolozza "Scia meteorica"; i valori vengono dagli schemi di YAEC (Chiaro, Notte, Nero, otto pastelli)
    // piu' lo schema "Scia meteorica" originale (indice 11).
    public static class Pal
    {
        public static Color Back;        // sfondo
        public static Color Panel;       // riquadri (carta)
        public static Color Panel2;      // righe dei comandi
        public static Color Panel3;      // bottoni secondari, etichette a pillola, righe dei grafici a barre
        public static Color Track;       // fondo della barra delle linguette e della barra di stato
        public static Color Border;      // bordi dei riquadri
        public static Color Fore;        // testo
        public static Color Muted;       // testo secondario
        public static Color Azure;       // accento principale
        public static Color AzureDark;   // fondo azzurrino (evidenza tenue)
        public static Color Violet;
        public static Color Green;       // stato buono
        public static Color Yellow;      // giallo leggibile sul fondo (limiti, valori notevoli)
        public static Color Orange;      // avviso
        public static Color OrangeDark;  // fondo degli avvisi
        public static Color OrangeText;  // testo degli avvisi
        public static Color Pink;
        public static Color Red;         // errore
        public static Color HotPink;     // Hot fuori dal lobo (uguale in tutti gli schemi)
        public static Color HotGrey;     // Hot dentro il lobo (uguale in tutti gli schemi)
        public static Color Silver;      // grigio chiaro (solo su fondi scuri)

        // voci aggiunte con gli schemi
        public static Color Card;        // riquadri "colpo d'occhio"
        public static Color Field;       // caselle di testo e di numeri
        public static Color FieldFore;
        public static Color ChartBg;     // fondo dei grafici
        public static Color ChartGrid;   // reticolo dei grafici
        public static Color ChartText;   // scritte degli assi
        public static Color OkBg;        // fondo verde tenue
        public static Color WarnBg;      // fondo giallo/arancio tenue
        public static Color BarFore;     // testo sulla barra scura delle linguette e sulla barra di stato
        public static Color BarMuted;    // testo secondario sulle barre scure
        public static Color BarHover;    // linguetta sotto il mouse
        public static Color AccentFore;  // testo sopra un fondo pieno di accento (linguetta scelta, bottone "Calcola")
        public static Color AccentHover; // bottone di accento sotto il mouse
        public static Color Pill;        // bottoni a pillola
        public static Color PillFore;
        public static Color PillBorder;
        public static Color PillHover;
        public static Color Select;      // riga scelta nelle tabelle
        public static Color TableHead;   // intestazione delle tabelle
        public static Color TableHeadFore;
        public static Color TableHeadLine;
        public static Color CodeBack;    // resoconto a spaziatura fissa (Geometria e attenuazione)
        public static Color CodeHead;
        public static Color CodeLabel;
        public static Color CodeText;
        public static Color Desk;        // fondo attorno al foglio della spiegazione

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

        // Colore "di dato" (serie dei grafici, scritte colorate) leggibile sul fondo dello schema:
        // negli schemi scuri resta com'e', in quelli chiari i colori troppo chiari vengono scuriti
        public static Color Ink(Color c)
        {
            if (Tema.Scuro) return c;
            double lum = (0.299 * c.R + 0.587 * c.G + 0.114 * c.B) / 255.0;
            const double target = 0.45;
            if (lum <= target) return c;
            double k = target / lum;
            return Color.FromArgb(c.A, (byte)(c.R * k), (byte)(c.G * k), (byte)(c.B * k));
        }

        // Celle della tabella oraria colorate con l'efficienza (0 - 100 %)
        public static Color HeatBack(double eff)
        {
            int a = (int)(Math.Max(0, Math.Min(100, eff)) * 1.6);
            if (Tema.Scuro) return Rgb(30 + a / 2, 30 + a / 3, 34);   // come nello schema originale
            double t = a / 160.0;   // schemi chiari: dal bianco carta all'ambra
            return Rgb((int)(Panel.R + (250 - Panel.R) * t), (int)(Panel.G + (196 - Panel.G) * t), (int)(Panel.B + (90 - Panel.B) * t));
        }

        // testo delle celle con efficienza alta (null = colore della riga)
        public static Color? HeatFore(double eff)
        {
            if (eff < 50) return null;
            return Tema.Scuro ? Yellow : Rgb(90, 40, 0);
        }

        static Pal() { Load(0); }

        static Color Mix(Color tinta, float k)
        {
            // come in YAEC: verso il bianco, troncando
            return Color.FromRgb((byte)(int)(255 + (tinta.R - 255) * k), (byte)(int)(255 + (tinta.G - 255) * k), (byte)(int)(255 + (tinta.B - 255) * k));
        }

        // Riempie la tavolozza con lo schema indicato (0 chiaro, 1 notte, 2 nero, 3-10 pastelli, 11 scia meteorica)
        internal static void Load(int schema)
        {
            HotPink = Rgb(240, 120, 240);
            HotGrey = Rgb(150, 150, 150);
            Silver = Rgb(192, 192, 192);
            if (schema == Tema.SchemaScia)
            {
                // "Scia meteorica": la tavolozza originale di YA-MS
                Back = Rgb(22, 18, 48); Panel = Rgb(28, 23, 64); Panel2 = Rgb(34, 28, 71); Panel3 = Rgb(43, 33, 96);
                Track = Rgb(14, 11, 34); Border = Rgb(58, 50, 112);
                Fore = Rgb(238, 235, 250); Muted = Rgb(179, 173, 214);
                Azure = Rgb(56, 189, 248); AzureDark = Rgb(11, 34, 51); Violet = Rgb(167, 139, 250); Green = Rgb(134, 239, 172);
                Yellow = Rgb(253, 224, 71); Orange = Rgb(251, 146, 60); OrangeDark = Rgb(58, 35, 18); OrangeText = Rgb(254, 215, 170);
                Pink = Rgb(240, 171, 252); Red = Rgb(248, 113, 113);
                Card = Panel3; Field = Back; FieldFore = Fore;
                ChartBg = Back; ChartGrid = Border; ChartText = Rgb(220, 220, 220);
                OkBg = Rgb(20, 64, 44); WarnBg = OrangeDark;
                BarFore = Fore; BarMuted = Muted; BarHover = Rgb(43, 33, 96);
                AccentFore = Rgb(14, 11, 34); AccentHover = Rgb(125, 211, 252);
                Pill = Rgb(43, 33, 96); PillFore = Fore; PillBorder = Rgb(58, 50, 112); PillHover = Rgb(59, 47, 134);
                Select = Panel3;
                TableHead = Rgb(244, 243, 250); TableHeadFore = Rgb(0, 0, 0); TableHeadLine = Rgb(200, 198, 215);
                CodeBack = Rgb(16, 20, 32); CodeHead = Rgb(255, 190, 90); CodeLabel = Rgb(130, 180, 235); CodeText = Rgb(235, 238, 245);
                Desk = Track;
                return;
            }
            if (schema == 1 || schema == 2)
            {
                bool nero = schema == 2;
                // valori di YAEC (Tema.Imposta): Back, Fore, Muted, Field, Section, Accent, OkFg, BadFg, WarnFg, OkBg, Warn2, SnBg,
                // ChartBg, ChartGrid, ChartText, Button, Carta, Linea, Limiti
                Color back = nero ? Rgb(0, 0, 0) : Rgb(11, 18, 32);
                Color fore = nero ? Rgb(235, 235, 235) : Rgb(219, 228, 245);
                Color muted = nero ? Rgb(140, 140, 140) : Rgb(127, 141, 176);
                Color field = nero ? Rgb(18, 18, 18) : Rgb(18, 27, 46);
                Color section = nero ? Rgb(34, 34, 34) : Rgb(26, 38, 64);
                Color accent = nero ? Rgb(80, 180, 255) : Rgb(95, 168, 255);
                Color okFg = nero ? Rgb(60, 235, 130) : Rgb(61, 220, 132);
                Color badFg = nero ? Rgb(255, 95, 85) : Rgb(255, 110, 100);
                Color warnFg = nero ? Rgb(255, 170, 40) : Rgb(255, 179, 64);
                Color okBg = nero ? Rgb(14, 60, 32) : Rgb(20, 64, 44);
                Color warn2 = nero ? Rgb(70, 52, 14) : Rgb(74, 58, 26);
                Color snBg = nero ? Rgb(14, 30, 56) : Rgb(22, 40, 70);
                Color chartBg = nero ? Rgb(8, 8, 8) : Rgb(14, 22, 40);
                Color chartGrid = nero ? Rgb(48, 48, 48) : Rgb(40, 52, 80);
                Color chartText = nero ? Rgb(175, 175, 175) : Rgb(160, 175, 200);
                Color button = nero ? Rgb(32, 32, 32) : Rgb(28, 40, 66);
                Color carta = nero ? Rgb(18, 18, 18) : Rgb(18, 27, 46);
                Color linea = nero ? Rgb(52, 52, 52) : Rgb(36, 49, 79);
                Color limiti = nero ? Rgb(255, 209, 72) : Rgb(255, 209, 102);

                Back = back; Panel = carta; Panel2 = section; Panel3 = button;
                Track = nero ? Rgb(12, 12, 12) : Rgb(6, 11, 22);   // tono piu' scuro dello schema
                Border = linea; Fore = fore; Muted = muted;
                Azure = accent; AzureDark = snBg;
                Violet = nero ? Rgb(180, 150, 255) : Rgb(167, 139, 250);
                Green = okFg; Yellow = limiti; Orange = warnFg;
                OrangeDark = warn2; OrangeText = Rgb(254, 215, 170);
                Pink = Rgb(240, 171, 252); Red = badFg;
                Card = carta; Field = field; FieldFore = fore;
                ChartBg = chartBg; ChartGrid = chartGrid; ChartText = chartText;
                OkBg = okBg; WarnBg = warn2;
                BarFore = fore; BarMuted = muted; BarHover = section;
                AccentFore = back; AccentHover = nero ? Rgb(130, 205, 255) : Rgb(140, 192, 255);
                Pill = button; PillFore = fore; PillBorder = linea; PillHover = nero ? Rgb(52, 52, 52) : Rgb(40, 56, 92);
                Select = snBg;
                TableHead = section; TableHeadFore = fore; TableHeadLine = linea;
                CodeBack = field; CodeHead = warnFg; CodeLabel = accent; CodeText = fore;
                Desk = Track;
                return;
            }
            // chiaro (0) e pastelli (3-10): i pastelli sono lo schema chiaro tinto
            Color cBack = Rgb(240, 240, 240), cSection = Rgb(230, 232, 240), cField = Rgb(255, 255, 255), cCarta = Rgb(255, 255, 255);
            Color cChartBg = Rgb(255, 255, 255), cChartGrid = Rgb(225, 228, 235), cLinea = Rgb(213, 220, 234), cButton = Rgb(240, 240, 240);
            Color cSnBg = Rgb(235, 245, 255);
            if (schema >= 3 && schema < 3 + Tema.PastelloTinta.Length)
            {
                Color t = Tema.PastelloTinta[schema - 3];
                cBack = Mix(t, 0.24f); cSection = Mix(t, 0.34f); cField = Mix(t, 0.05f); cCarta = Mix(t, 0.08f);
                cChartBg = Mix(t, 0.06f); cChartGrid = Mix(t, 0.22f); cLinea = Mix(t, 0.30f); cButton = Mix(t, 0.18f);
                cSnBg = Mix(t, 0.12f);
            }
            Back = cBack; Panel = cCarta; Panel2 = cSection; Panel3 = cButton;
            Track = Rgb(30, 41, 59);   // barra scura ardesia (come nel modello GRAVES di YAEC)
            Border = cLinea;
            Fore = Rgb(0, 0, 0); Muted = Rgb(109, 109, 109);   // SystemColors.ControlText / GrayText
            Azure = Rgb(31, 111, 209); AzureDark = cSnBg;
            Violet = Rgb(124, 58, 237);
            Green = Rgb(19, 150, 80); Yellow = Rgb(181, 131, 0); Orange = Rgb(217, 123, 0);
            OrangeDark = Rgb(255, 245, 220); OrangeText = Rgb(180, 83, 9);
            Pink = Rgb(190, 50, 150); Red = Rgb(178, 34, 34);   // Firebrick
            Card = cCarta; Field = cField; FieldFore = Rgb(0, 0, 0);
            ChartBg = cChartBg; ChartGrid = cChartGrid; ChartText = Rgb(80, 90, 105);
            OkBg = Rgb(214, 245, 214); WarnBg = Rgb(255, 245, 220);
            BarFore = Rgb(226, 232, 240); BarMuted = Rgb(148, 163, 184); BarHover = Rgb(44, 56, 78);
            AccentFore = Rgb(255, 255, 255); AccentHover = Rgb(59, 134, 226);
            Pill = cCarta; PillFore = Rgb(37, 84, 160); PillBorder = cLinea; PillHover = cSnBg;   // ColPillola di YAEC
            Select = cSnBg;
            TableHead = cSection; TableHeadFore = Rgb(0, 0, 0); TableHeadLine = cLinea;
            CodeBack = cField; CodeHead = Rgb(180, 83, 9); CodeLabel = Rgb(31, 111, 209); CodeText = Rgb(0, 0, 0);
            Desk = cSection;
        }
    }

    // Colori fissi dei "cieli" (volta celeste, pianta, sezione, cielo di GRAVES, mappa dei quadrati): restano scuri
    // in tutti gli schemi, come le viste del cielo di YAEC, cosi' radianti, coste e scie restano visibili.
    public static class SkyPal
    {
        public static readonly Color Back = Color.FromRgb(22, 18, 48);
        public static readonly Color Deep = Color.FromRgb(14, 11, 34);
        public static readonly Color Fore = Color.FromRgb(238, 235, 250);
        public static readonly Color Muted = Color.FromRgb(179, 173, 214);
        public static readonly Color Azure = Color.FromRgb(56, 189, 248);
        public static readonly Color Violet = Color.FromRgb(167, 139, 250);
        public static readonly Color Green = Color.FromRgb(134, 239, 172);
        public static readonly Color Yellow = Color.FromRgb(253, 224, 71);
        public static readonly Color Orange = Color.FromRgb(251, 146, 60);
        public static readonly Color Pink = Color.FromRgb(240, 171, 252);
        public static readonly Color Red = Color.FromRgb(248, 113, 113);
        public static readonly Color Silver = Color.FromRgb(192, 192, 192);
        public static readonly Color Halo = Color.FromArgb(184, 14, 11, 34);   // alone delle etichette
    }

    // Schemi di colori (come YAEC): 0 Chiaro, 1 Notte, 2 Nero, 3-10 pastelli, 11 Scia meteorica (tavolozza originale)
    public static class Tema
    {
        public static int Schema;
        public static bool Scuro;
        public const int SchemaScia = 11;
        public static readonly string[] PastelloChiave = { "azzurro", "arancione", "acquamarina", "verde", "rosa", "lilla", "indaco", "marrone" };
        public static readonly string[] PastelloNome = { "Pastello azzurro", "Pastello arancione", "Pastello acquamarina", "Pastello verde", "Pastello rosa", "Pastello lilla", "Pastello indaco", "Pastello marrone" };
        public static readonly Color[] PastelloTinta = {
            Color.FromRgb(70, 150, 230), Color.FromRgb(240, 140, 50), Color.FromRgb(40, 180, 170), Color.FromRgb(80, 170, 90),
            Color.FromRgb(230, 110, 160), Color.FromRgb(160, 120, 210), Color.FromRgb(80, 90, 190), Color.FromRgb(150, 105, 70) };

        public static int Count { get { return SchemaScia + 1; } }

        // nomi italiani (da tradurre con Lang.T) nell'ordine degli indici
        public static string[] Names
        {
            get
            {
                string[] n = new string[Count];
                n[0] = "Chiaro"; n[1] = "Notte"; n[2] = "Nero";
                for (int i = 0; i < PastelloNome.Length; i++) n[3 + i] = PastelloNome[i];
                n[SchemaScia] = "Scia meteorica";
                return n;
            }
        }

        // chiave salvata in config.ini (tema = ...)
        public static string Chiave
        {
            get
            {
                if (Schema == SchemaScia) return "scia";
                if (Schema == 2) return "nero";
                if (Schema == 1) return "scuro";
                if (Schema >= 3 && Schema < 3 + PastelloChiave.Length) return "pastello-" + PastelloChiave[Schema - 3];
                return "chiaro";
            }
        }

        public static int DaChiave(string k)
        {
            k = (k ?? "").Trim().ToLowerInvariant();
            if (k == "scia") return SchemaScia;
            if (k == "nero") return 2;
            if (k == "scuro") return 1;
            if (k.StartsWith("pastello-")) { int i = Array.IndexOf(PastelloChiave, k.Substring(9)); if (i >= 0) return 3 + i; }
            return 0;
        }

        public static void Imposta(int schema)
        {
            Schema = Math.Max(0, Math.Min(SchemaScia, schema));
            Scuro = Schema == 1 || Schema == 2 || Schema == SchemaScia;
            Pal.Load(Schema);
            ApplicaStili();
        }

        static void Res(string key, Color c)
        {
            Application.Current.Resources[key] = new SolidColorBrush(c);
        }

        // Risorse usate dagli stili di App.axaml ({DynamicResource ...}) e variante chiara/scura dei controlli Fluent
        public static void ApplicaStili()
        {
            if (Application.Current == null) return;
            Application.Current.RequestedThemeVariant = Scuro ? ThemeVariant.Dark : ThemeVariant.Light;
            Res("YamsBack", Pal.Back);
            Res("YamsFore", Pal.Fore);
            Res("YamsMuted", Pal.Muted);
            Res("YamsPill", Pal.Pill);
            Res("YamsPillFore", Pal.PillFore);
            Res("YamsPillBorder", Pal.PillBorder);
            Res("YamsPillHover", Pal.PillHover);
            Res("YamsAccent", Pal.Azure);
            Res("YamsAccentFore", Pal.AccentFore);
            Res("YamsAccentHover", Pal.AccentHover);
            Res("YamsTabFore", Pal.BarFore);
            Res("YamsTabHover", Pal.BarHover);
        }
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
