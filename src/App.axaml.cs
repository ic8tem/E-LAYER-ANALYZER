using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using YAMS;

namespace ELA
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            // colorazione scelta (data/ela.ini, chiave schema_colori), applicata prima di costruire le finestre
            string key = "scia";
            try { key = new Config(System.IO.Path.Combine(AppPaths.DataDir, "ela.ini")).Get("schema_colori", "scia"); } catch { }
            if (Program.ThemeArg != null) key = Program.ThemeArg;
            Themes.Apply(Themes.IndexOf(key));
            Themes.FillResources(Resources);
            RequestedThemeVariant = Pal.Light ? ThemeVariant.Light : ThemeVariant.Dark;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // lingua dell'interfaccia (chiave "lingua" di config.ini, condivisa con YA-MS)
                try { Lang.Init(YAMS.Program.ScreenshotLang ?? new Config(System.IO.Path.Combine(AppPaths.DataDir, "config.ini")).Get("lingua", "")); } catch { }
                desktop.MainWindow = new ElaWindow();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
