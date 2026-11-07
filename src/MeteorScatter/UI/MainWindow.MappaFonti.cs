// YA-MS - linguetta "Mappa delle fonti" (subito dopo "Fonti"): dove si trovano i radar e i trasmettitori
// da cui arrivano i dati in diretta, rispetto alla stazione dell'utente.
using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        SourcesMapView srcEurope, srcWorld;
        bool srcWideLoading;

        void BuildSourcesMapTab()
        {
            DockPanel p = Page(Lang.T("Mappa dei radar europei"));
            TextBlock head = new TextBlock
            {
                Foreground = Pal.B(Pal.OrangeText), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(8, 6, 8, 4),
                Text = Lang.T("Dove si trovano le fonti dei dati in diretta. Cerchi tratteggiati = zona di cielo vista dal radar (circa 300 km intorno alla stazione, scie a 90 km): " +
                              "nessun radar guarda il cielo sopra la tua tratta, i dati dicono quanto e' attivo lo sciame. Linee punteggiate = distanza dalla tua stazione.")
            };
            DockPanel.SetDock(head, Dock.Top);
            p.Children.Add(head);

            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("58*,42*"), RowDefinitions = new RowDefinitions("*,*"), Margin = new Thickness(6) };
            srcEurope = new SourcesMapView { Title = Lang.T("Europa"), LonMin = -12, LonMax = 38, LatMin = 34, LatMax = 72, MinorLabel = Lang.T("osservatori RMOB che ascoltano GRAVES") };
            srcWorld = new SourcesMapView { Title = Lang.T("Mondo"), LonMin = -125, LonMax = 70, LatMin = 12, LatMax = 75, FontK = 0.9 };
            Border be = new Border { Child = srcEurope, Margin = new Thickness(0, 0, 6, 0) };
            Grid.SetRowSpan(be, 2);
            Border bw = new Border { Child = srcWorld, Margin = new Thickness(0, 0, 0, 6) };
            Grid.SetColumn(bw, 1);
            TextBlock gmn = new TextBlock
            {
                Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap, FontSize = 14,
                Text = Lang.T("Non è un punto sulla mappa: è una rete mondiale di telecamere (soprattutto amatoriali) sparse in molti paesi di tutti i continenti.") + "\n\n" +
                       "• " + Lang.T("misura il flusso e lo ZHR di ogni sciame attivo") + "\n" +
                       "• " + Lang.T("aggiornamento una volta al giorno") + "\n" +
                       "• " + Lang.T("dati CC BY 4.0 (Vida et al. 2021, 2022)") + "\n" +
                       "→ " + Lang.F("linguetta \"{0}\"", Lang.T("Sciami in diretta"))
            };
            StackPanel gs = new StackPanel { Spacing = 10 };
            gs.Children.Add(new TextBlock { Text = Lang.T("Global Meteor Network (GMN)"), FontSize = 17, FontWeight = FontWeight.Bold, Foreground = Pal.B(Color.FromRgb(15, 118, 110)) });
            gs.Children.Add(gmn);
            Border bg = new Border
            {
                Child = gs, Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8), Padding = new Thickness(14, 10)
            };
            Grid.SetColumn(bg, 1); Grid.SetRow(bg, 1);
            g.Children.Add(be); g.Children.Add(bw); g.Children.Add(bg);
            p.Children.Add(g);

            Color cCollm = Color.FromRgb(10, 165, 201), cSoda = Color.FromRgb(46, 158, 68), cGraves = Color.FromRgb(232, 89, 12), cCmor = Color.FromRgb(112, 72, 232);
            MapSource collmM = new MapSource { Name = "Collm", Lat = 51.31, Lon = 13.00, Color = cCollm, Line1 = Lang.T("radar SKiYMET 36,2 MHz"), Line2 = Lang.T("Univ. Lipsia / IAP Kühlungsborn"), Tab = Lang.T("Live e dati real-time") + " › " + Lang.T("Flusso radar") };
            MapSource sodaM = new MapSource { Name = "Sodankylä", Lat = 67.37, Lon = 26.63, Color = cSoda, Line1 = Lang.T("radar meteorico 36,9 MHz"), Line2 = Lang.T("SGO, Finlandia"), Tab = Lang.T("Live e dati real-time") + " › " + Lang.T("Flusso radar") };
            MapSource gravM = new MapSource { Name = "GRAVES", Lat = GravesBeam.Lat, Lon = GravesBeam.Lon, Color = cGraves, Line1 = Lang.T("trasmettitore 143,050 MHz"), Line2 = "Broye-lès-Pesmes", Tab = Lang.T("Live e dati real-time") + " › " + Lang.T("Radar GRAVES"), LabelLeft = true };
            MapSource cmorM = new MapSource { Name = "CMOR", Lat = 43.26, Lon = -80.77, Color = cCmor, CoverKm = 600, Line1 = Lang.T("radar meteorico 17-38 MHz, orbite"), Line2 = Lang.T("Tavistock, Ontario (Canada)"), Tab = Lang.T("Sciami in diretta") };
            srcEurope.Sources = new List<MapSource> { collmM, sodaM, gravM };
            srcEurope.Minor = new List<MapSource>
            {
                new MapSource { Name = "Venezia", Lat = 45.44, Lon = 12.33 },
                new MapSource { Name = "Faenza", Lat = 44.29, Lon = 11.88 },
                new MapSource { Name = "Rovereto", Lat = 45.89, Lon = 11.04 }
            };
            srcWorld.Sources = new List<MapSource> { cmorM };
            UpdateSourcesMap();
        }

        void UpdateSourcesMap()
        {
            if (srcEurope == null) return;
            LatLon home;
            if (!Astro.TryLocator(Up(txtHome), out home)) return;
            string lab = Lang.T("la tua stazione") + " " + Up(txtHome);
            srcEurope.Home = home; srcWorld.Home = home;
            srcEurope.HomeLabel = lab; srcWorld.HomeLabel = lab;
            if (sky != null && sky.MapLines != null && sky.MapLines.Count > 0)
            {
                srcEurope.MapLines = sky.MapLines; srcEurope.MapBorders = sky.MapBorders;
                // per il mondo serve anche l'America: si rilegge Natural Earth con limiti piu' larghi (una volta sola)
                if (srcWorld.MapLines == null && !srcWideLoading)
                {
                    srcWideLoading = true;
                    string dir = dataDir;
                    System.Threading.ThreadPool.QueueUserWorkItem(_ =>
                    {
                        var co = MapData.Load(MapData.CoastFile(dir), -140, 80);
                        var bo = MapData.Load(MapData.BorderFile(dir), -140, 80);
                        Dispatcher.UIThread.Post(() =>
                        {
                            srcWideLoading = false;
                            if (co.Count == 0) return;
                            srcWorld.MapLines = co; srcWorld.MapBorders = bo; srcWorld.InvalidateVisual();
                        });
                    });
                }
            }
            srcEurope.InvalidateVisual(); srcWorld.InvalidateVisual();
        }
    }
}
