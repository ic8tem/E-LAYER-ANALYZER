// E-LAYER ANALYZER - colorazioni: si usano gli schemi di YA-MS (classe Tema: Chiaro, Notte, Nero, otto pastelli,
// Scia meteorica), che riprendono quelli di YAEC. Qui si ricavano solo i colori della mappa del mondo.
using System;
using Avalonia.Media;
using YAMS;

namespace ELA
{
    public static class Themes
    {
        public static Color MapSea, MapLand, MapBorder, MapGrid, MapField, MapSquare, Overlay;
        public static bool Light { get { return !Tema.Scuro; } }

        static Color Mix(Color t, double k) { return Color.FromRgb((byte)(255 + (t.R - 255) * k), (byte)(255 + (t.G - 255) * k), (byte)(255 + (t.B - 255) * k)); }

        // da chiamare dopo Tema.Imposta
        public static void FromPal()
        {
            if (Tema.Schema == Tema.SchemaScia)
            {
                MapSea = Color.FromRgb(13, 16, 44); MapLand = Color.FromRgb(126, 116, 190); MapBorder = Color.FromRgb(70, 62, 120);
                MapGrid = Color.FromArgb(70, 120, 110, 190); MapField = Color.FromArgb(70, 90, 230, 250); MapSquare = Color.FromArgb(110, 170, 165, 230);
                Overlay = Color.FromArgb(190, 14, 11, 34);
            }
            else if (Tema.Scuro)
            {
                Color b = Pal.Back;
                MapSea = Color.FromRgb((byte)Math.Max(0, b.R - 4), (byte)Math.Max(0, b.G - 2), (byte)Math.Min(255, b.B + 8));
                MapLand = Mix(Pal.Azure, 0.55); MapBorder = Pal.Border;
                MapGrid = Pal.A(80, Pal.Border); MapField = Pal.A(70, Pal.Azure); MapSquare = Pal.A(110, Pal.Muted);
                Overlay = Pal.A(200, Pal.Back);
            }
            else
            {
                MapSea = Color.FromRgb(226, 236, 248); MapLand = Color.FromRgb(70, 90, 140); MapBorder = Color.FromRgb(160, 170, 200);
                MapGrid = Color.FromArgb(90, 90, 110, 160); MapField = Color.FromArgb(70, 40, 110, 200); MapSquare = Color.FromArgb(120, 80, 90, 140);
                Overlay = Color.FromArgb(215, 255, 255, 255);
            }
        }
    }
}
