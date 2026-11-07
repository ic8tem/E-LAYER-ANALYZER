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
                // lingua dell'interfaccia (chiave "lingua" di config.ini, condivisa con YA-MS)
                try { Lang.Init(YAMS.Program.ScreenshotLang ?? new Config(System.IO.Path.Combine(AppPaths.DataDir, "config.ini")).Get("lingua", "")); } catch { }
                desktop.MainWindow = new ElaWindow();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
