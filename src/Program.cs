// E-LAYER ANALYZER - avvio (Windows, Linux, Mac)
using System;
using System.IO;
using Avalonia;

namespace ELA
{
    internal static class Program
    {
        public static string ScreenshotDir, ScreenshotLoc, ThemeArg, AuroraA, AuroraB;
        public static int ShotW, ShotH;   // --dimensione LxA: finestra di prova (fotografie della modalita' compatta)

        [STAThread]
        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "--screenshot") { ScreenshotDir = args[i + 1]; YAMS.Program.ScreenshotDir = args[i + 1]; }
                if (args[i] == "--loc") ScreenshotLoc = args[i + 1];
                if (args[i] == "--aurora" && i + 2 < args.Length) { AuroraA = args[i + 1]; AuroraB = args[i + 2]; }
                if (args[i] == "--colori") ThemeArg = args[i + 1];
                if (args[i] == "--lingua") YAMS.Program.ScreenshotLang = args[i + 1];
                if (args[i] == "--dimensione")
                {
                    string[] p = args[i + 1].ToLowerInvariant().Split('x');
                    int w, h;
                    if (p.Length == 2 && int.TryParse(p[0], out w) && int.TryParse(p[1], out h)) { ShotW = w; ShotH = h; }
                }
            }
            try
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                try { File.WriteAllText(Path.Combine(YAMS.AppPaths.DataDir, "errore_avvio.txt"), ex.ToString()); } catch { }
                throw;
            }
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();
        }
    }
}

namespace YAMS
{
    // opzioni di YA-MS (fotografie automatiche) usate dal pannello METEOR SCATTER
    public static class Program
    {
        public static string ScreenshotDir, ScreenshotDx, ScreenshotLang, ScreenshotTheme;
        public static double ScreenshotHours;
        public static double ScreenshotFontPct;
    }
}
