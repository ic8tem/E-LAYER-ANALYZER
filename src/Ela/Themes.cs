// E-LAYER ANALYZER - colorazioni dell'interfaccia: "Scia meteorica" di YA-MS e tutti gli schemi di YAEC
// (chiaro, notte blu, nero, otto pastelli). Lo schema si sceglie in alto a destra e vale dal riavvio
// (il programma si riavvia da solo). I colori di YAEC sono ripresi dalla classe Tema di YAEC.cs.
using System;
using Avalonia.Media;
using YAMS;

namespace ELA
{
    public static class Themes
    {
        public static readonly string[] Keys = { "scia", "chiaro", "scuro", "nero",
            "pastello-azzurro", "pastello-arancione", "pastello-acquamarina", "pastello-verde",
            "pastello-rosa", "pastello-lilla", "pastello-indaco", "pastello-marrone" };
        public static readonly string[] Names = { "Scia meteorica (YA-MS)", "Chiaro (YAEC)", "Notte blu (YAEC)", "Nero (YAEC)",
            "Pastello azzurro", "Pastello arancione", "Pastello acquamarina", "Pastello verde",
            "Pastello rosa", "Pastello lilla", "Pastello indaco", "Pastello marrone" };
        static readonly Color[] PastelloTinta = {
            Color.FromRgb(70, 150, 230), Color.FromRgb(240, 140, 50), Color.FromRgb(40, 180, 170), Color.FromRgb(80, 170, 90),
            Color.FromRgb(230, 110, 160), Color.FromRgb(160, 120, 210), Color.FromRgb(80, 90, 190), Color.FromRgb(150, 105, 70) };

        public static int Current;
        public static bool Light { get { return Pal.Light; } }
        // colori della mappa
        public static Color MapSea = Color.FromRgb(13, 16, 44), MapLand = Color.FromRgb(126, 116, 190), MapBorder = Color.FromRgb(70, 62, 120);
        public static Color MapGrid = Color.FromArgb(70, 120, 110, 190), MapField = Color.FromArgb(70, 90, 230, 250), MapSquare = Color.FromArgb(110, 170, 165, 230);
        public static Color Overlay = Color.FromArgb(190, 14, 11, 34);

        public static int IndexOf(string key) { int i = Array.IndexOf(Keys, key ?? ""); return i < 0 ? 0 : i; }

        static Color Mix(Color t, double k) { return Color.FromRgb((byte)(255 + (t.R - 255) * k), (byte)(255 + (t.G - 255) * k), (byte)(255 + (t.B - 255) * k)); }
        static Color Rgb(int r, int g, int b) { return Color.FromRgb((byte)r, (byte)g, (byte)b); }

        // Imposta i colori di YAMS.Pal (usati da tutto il programma, YA-MS compreso) prima di costruire le finestre
        public static void Apply(int idx)
        {
            Current = Math.Max(0, Math.Min(Keys.Length - 1, idx));
            if (Current == 0) return;   // "Scia meteorica": i valori di partenza di YA-MS
            if (Current == 2)
            {
                // YAEC notte: blu scuro, cifre luminose
                Set(back: Rgb(11, 18, 32), panel: Rgb(26, 38, 64), panel2: Rgb(18, 27, 46), button: Rgb(28, 40, 66), track: Rgb(7, 12, 22), border: Rgb(36, 49, 79),
                    fore: Rgb(219, 228, 245), muted: Rgb(127, 141, 176), accent: Rgb(95, 168, 255), ok: Rgb(61, 220, 132), bad: Rgb(255, 110, 100), warn: Rgb(255, 179, 64),
                    gold: Rgb(255, 209, 102), violet: Rgb(167, 139, 250), light: false);
                return;
            }
            if (Current == 3)
            {
                // YAEC nero: sfondo nero pieno, riquadri grigio molto scuro, colori di stato accesi
                Set(back: Rgb(0, 0, 0), panel: Rgb(34, 34, 34), panel2: Rgb(18, 18, 18), button: Rgb(32, 32, 32), track: Rgb(10, 10, 10), border: Rgb(52, 52, 52),
                    fore: Rgb(235, 235, 235), muted: Rgb(140, 140, 140), accent: Rgb(80, 180, 255), ok: Rgb(60, 235, 130), bad: Rgb(255, 95, 85), warn: Rgb(255, 170, 40),
                    gold: Rgb(255, 209, 72), violet: Rgb(190, 160, 255), light: false);
                return;
            }
            // YAEC chiaro e pastelli (i pastelli sono lo schema chiaro tinto)
            Color back = Rgb(240, 240, 240), panel = Rgb(230, 232, 240), panel2 = Rgb(232, 234, 241), button = Rgb(225, 228, 236), border = Rgb(213, 220, 234), track = Rgb(214, 219, 230);
            if (Current >= 4)
            {
                Color t = PastelloTinta[Current - 4];
                back = Mix(t, 0.24); panel = Mix(t, 0.34); panel2 = Mix(t, 0.18); button = Mix(t, 0.18); border = Mix(t, 0.30); track = Mix(t, 0.42);
            }
            Set(back: back, panel: panel, panel2: panel2, button: button, track: track, border: border,
                fore: Rgb(0, 0, 0), muted: Rgb(95, 95, 105), accent: Rgb(31, 111, 209), ok: Rgb(19, 150, 80), bad: Rgb(178, 34, 34), warn: Rgb(217, 123, 0),
                gold: Rgb(181, 131, 0), violet: Rgb(91, 63, 168), light: true);
        }

        static void Set(Color back, Color panel, Color panel2, Color button, Color track, Color border, Color fore, Color muted,
            Color accent, Color ok, Color bad, Color warn, Color gold, Color violet, bool light)
        {
            Pal.Back = back; Pal.Panel = panel; Pal.Panel2 = panel2; Pal.Panel3 = button; Pal.Track = track; Pal.Border = border;
            Pal.Fore = fore; Pal.Muted = muted; Pal.Azure = accent; Pal.Green = ok; Pal.Red = bad; Pal.Orange = warn; Pal.Yellow = gold; Pal.Violet = violet;
            Pal.Light = light;
            Pal.ChartText = light ? Rgb(60, 66, 80) : Rgb(200, 205, 220);
            if (light)
            {
                Pal.AzureDark = Mix(accent, 0.15); Pal.OrangeDark = Mix(warn, 0.18); Pal.OrangeText = Rgb(120, 60, 0);
                Pal.Pink = Rgb(170, 60, 160); Pal.Silver = Rgb(110, 110, 120);
                MapSea = Rgb(226, 236, 248); MapLand = Rgb(70, 90, 140); MapBorder = Rgb(160, 170, 200);
                MapGrid = Color.FromArgb(90, 90, 110, 160); MapField = Color.FromArgb(70, 40, 110, 200); MapSquare = Color.FromArgb(120, 80, 90, 140);
                Overlay = Color.FromArgb(215, 255, 255, 255);
            }
            else
            {
                Pal.AzureDark = Color.FromArgb(255, (byte)(accent.R / 6), (byte)(accent.G / 6), (byte)(accent.B / 5));
                Pal.OrangeDark = Color.FromArgb(255, (byte)(warn.R / 5), (byte)(warn.G / 6), (byte)(warn.B / 8));
                MapSea = Color.FromRgb((byte)Math.Max(0, back.R - 4), (byte)Math.Max(0, back.G - 2), (byte)Math.Min(255, back.B + 8));
                MapLand = Mix(accent, 0.55); MapBorder = border;
                MapGrid = Color.FromArgb(80, border.R, border.G, border.B); MapField = Color.FromArgb(70, accent.R, accent.G, accent.B);
                MapSquare = Color.FromArgb(110, muted.R, muted.G, muted.B);
                Overlay = Color.FromArgb(200, back.R, back.G, back.B);
            }
        }

        // pennelli per le risorse dello stile dell'applicazione (App.axaml)
        public static void FillResources(Avalonia.Controls.IResourceDictionary r)
        {
            r["ElaBack"] = new SolidColorBrush(Pal.Back);
            r["ElaFore"] = new SolidColorBrush(Pal.Fore);
            r["ElaPill"] = new SolidColorBrush(Pal.Panel3);
            r["ElaPillOver"] = new SolidColorBrush(Pal.Light ? Mix(Pal.Azure, 0.25) : Pal.Border);
            r["ElaPillBorder"] = new SolidColorBrush(Pal.Border);
            r["ElaAccent"] = new SolidColorBrush(Pal.Azure);
            r["ElaAccentOver"] = new SolidColorBrush(Pal.Light ? Mix(Pal.Azure, 0.75) : Mix(Pal.Azure, 0.6));
            r["ElaAccentFore"] = new SolidColorBrush(Pal.Light ? Colors.White : Pal.Track);
            r["ElaMuted"] = new SolidColorBrush(Pal.Muted);
        }
    }
}
