using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using YAMS;

namespace ELA
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // lingua e schema di colori (chiavi "lingua" e "tema" di config.ini, condivise con YA-MS)
                Config cfg = null;
                try { cfg = new Config(System.IO.Path.Combine(AppPaths.DataDir, "config.ini")); } catch { }
                try { Lang.Init(YAMS.Program.ScreenshotLang ?? (cfg != null ? cfg.Get("lingua", "") : "")); } catch { }
                try { Tema.Imposta(Tema.DaChiave(Program.ThemeArg ?? (cfg != null ? cfg.Get("tema", "scia") : "scia"))); } catch { }
                Themes.FromPal();
                desktop.MainWindow = new ElaWindow();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
