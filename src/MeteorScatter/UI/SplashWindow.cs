// YA-MS - schermata di avvio (5 secondi): fotografia delle Perseidi, titolo del programma, autore in corsivo a destra,
// barra di avvio verde che scorre da sinistra a destra, credito della fotografia in basso a sinistra.
// Fotografia: "The 2010 Perseids over the VLT", ESO/S. Guisard (www.eso.org/public/images/potw1033a), CC BY 4.0,
// ridotta a 1280 x 800 pixel.
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;

namespace YAMS
{
    public class SplashWindow : Window
    {
        public const double Seconds = 5.0;
        public event Action Done;
        readonly Border bar;
        readonly double barMax;
        readonly DateTime t0 = DateTime.UtcNow;
        readonly DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };

        public SplashWindow()
        {
            Width = 900; Height = 562;
            CanResize = false;
            SystemDecorations = SystemDecorations.None;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ShowInTaskbar = true;
            Title = AppPaths.Title;
            Background = Brushes.Black;
            try { Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://YA-MS/src/Assets/ya-ms.png"))); } catch { }

            Grid g = new Grid();
            try
            {
                Bitmap bmp = new Bitmap(AssetLoader.Open(new Uri("avares://YA-MS/src/Assets/avvio.jpg")));
                g.Children.Add(new Image { Source = bmp, Stretch = Stretch.UniformToFill });
            }
            catch { }
            // velatura scura in alto e in basso per leggere le scritte
            g.Children.Add(new Border
            {
                Background = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative), EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
                    GradientStops = new GradientStops
                    {
                        new GradientStop(Color.FromArgb(170, 0, 0, 0), 0), new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.38),
                        new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.62), new GradientStop(Color.FromArgb(200, 0, 0, 0), 1)
                    }
                }
            });
            StackPanel top = new StackPanel { Margin = new Thickness(40, 30, 40, 0), VerticalAlignment = VerticalAlignment.Top };
            top.Children.Add(new TextBlock { Text = "YA-MS", FontSize = 64, FontWeight = FontWeight.Bold, Foreground = Brushes.White });
            top.Children.Add(new TextBlock
            {
                Text = Lang.T("Previsione e calcolo in diretta dell'efficienza degli sciami meteorici per il meteor scatter"),
                FontSize = 18, Foreground = new SolidColorBrush(Color.FromRgb(225, 232, 245)), TextWrapping = TextWrapping.Wrap, MaxWidth = 560, HorizontalAlignment = HorizontalAlignment.Left
            });
            top.Children.Add(new TextBlock { Text = Lang.T("versione") + " " + AppPaths.Version, FontSize = 14, Foreground = new SolidColorBrush(Color.FromRgb(170, 185, 210)), Margin = new Thickness(0, 6, 0, 0) });
            g.Children.Add(top);
            // autore in corsivo, allineato a destra
            g.Children.Add(new TextBlock
            {
                Text = Lang.T("a cura di") + " Costantino Cerrotta IC8TEM", FontSize = 22, FontStyle = FontStyle.Italic, FontWeight = FontWeight.SemiBold,
                Foreground = Brushes.White, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom,
                TextAlignment = TextAlignment.Right, Margin = new Thickness(40, 0, 40, 64)
            });
            // barra di avvio verde, da sinistra a destra
            barMax = Width - 80;
            Border track = new Border
            {
                Height = 10, CornerRadius = new CornerRadius(5), Background = new SolidColorBrush(Color.FromArgb(90, 255, 255, 255)),
                VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(40, 0, 40, 36)
            };
            bar = new Border
            {
                Height = 10, Width = 0, CornerRadius = new CornerRadius(5), HorizontalAlignment = HorizontalAlignment.Left,
                Background = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative), EndPoint = new RelativePoint(1, 0, RelativeUnit.Relative),
                    GradientStops = new GradientStops { new GradientStop(Color.FromRgb(22, 163, 74), 0), new GradientStop(Color.FromRgb(74, 222, 128), 1) }
                },
                VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(40, 0, 40, 36)
            };
            g.Children.Add(track);
            g.Children.Add(bar);
            // credito della fotografia, in basso a sinistra
            g.Children.Add(new TextBlock
            {
                Text = Lang.T("Foto") + ": ESO/S. Guisard · \"The 2010 Perseids over the VLT\" · CC BY 4.0", FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(200, 208, 222)), HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(40, 0, 0, 12)
            });
            Content = g;
            timer.Tick += (s, e) =>
            {
                double f = Math.Min(1, (DateTime.UtcNow - t0).TotalSeconds / Seconds);
                bar.Width = barMax * f;
                if (f >= 1) { timer.Stop(); Action a = Done; if (a != null) a(); }
            };
            Opened += (s, e) => timer.Start();
        }
    }
}
