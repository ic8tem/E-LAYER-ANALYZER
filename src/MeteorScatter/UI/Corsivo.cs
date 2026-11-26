using Avalonia.Media;

namespace YAMS
{
    // Corsivo e scritture da destra a sinistra (arabo, ebraico, urdu): senza un carattere corsivo per quelle lettere
    // il testo usciva a quadratini; in quelle lingue il corsivo diventa normale (stessa correzione di E-LAYER ANALYZER 0.6.3).
    static class Corsivo
    {
        public static FontStyle Per(string text, FontStyle st = FontStyle.Italic)
        {
            if (st != FontStyle.Italic || text == null) return st;
            foreach (char ch in text)
                if ((ch >= '֐' && ch <= 'ࣿ') || (ch >= 'יִ' && ch <= 'ﻼ')) return FontStyle.Normal;
            return st;
        }
    }
}
