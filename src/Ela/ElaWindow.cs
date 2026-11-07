// E-LAYER ANALYZER - finestra principale: stazione, lingua, linguette generali
// SPORADIC-E | F.A.I. | METEOR SCATTER (YA-MS) | AURORA | DATA COLLECTION | FONTI | DOCUMENTO TECNICO
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class ElaWindow : Window
    {
        public const string AppName = "E-LAYER ANALYZER";
        public const string Version = "0.1.0";

        readonly PillTabs tabs = new PillTabs(true);
        readonly TextBox txtCall, txtLoc;
        readonly NumericUpDown numAlt;
        readonly TextBlock lblClock, status;
        readonly ComboBox cmbLang;
        EsTab es; ScatterTab fai, au; DataTab dc;
        YAMS.MainWindow ms;
        bool replaced;
        static bool startedOnce;

        public ElaWindow()
        {
            string dataDir = AppPaths.DataDir;
            if (Station.Cfg == null) Station.Load(dataDir);
            Title = AppName + " " + Version + " - " + Lang.T("a cura di Costantino IC8TEM") + (Station.Call.Length > 0 && Station.Call != "IC8TEM" ? " " + Lang.F("per {0}", Station.Call) : "");
            Width = 1600; Height = 980;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Background = Pal.B(Pal.Back);
            Foreground = Pal.B(Pal.Fore);
            FontSize = 13;
            try { Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://E-LAYER-ANALYZER/src/Assets/ela.png"))); } catch { }

            // --- riga della stazione
            txtCall = Ui.T(Station.Call, 110, true);
            txtLoc = Ui.T(Station.Loc, 90, true);
            numAlt = Ui.N(0, 5000, Ui.Dm(Station.AltM), 0, 10, 55);
            Button bApply = Ui.B(Lang.T("Applica"), true);
            bApply.Click += (s, e) => ApplyStation();
            txtLoc.KeyUp += (s, e) => { if (e.Key == Avalonia.Input.Key.Enter) ApplyStation(); };
            lblClock = new TextBlock { Foreground = Pal.B(Pal.Yellow), FontFamily = Pal.Mono, FontSize = 15, FontWeight = FontWeight.SemiBold, Margin = new Thickness(12, 0), VerticalAlignment = VerticalAlignment.Center };
            List<string> langNames = new List<string>();
            foreach (string[] lg in Lang.Languages) langNames.Add(lg[1]);
            cmbLang = new ComboBox { ItemsSource = langNames, MinWidth = 120, Margin = new Thickness(6, 1, 0, 1), VerticalAlignment = VerticalAlignment.Center };
            cmbLang.SelectedIndex = Math.Max(0, Lang.IndexOf(Lang.Code));
            cmbLang.SelectionChanged += (s, e) =>
            {
                int li = cmbLang.SelectedIndex;
                if (li < 0 || li >= Lang.Languages.Length || Lang.Languages[li][0] == Lang.Code) return;
                string code = Lang.Languages[li][0];
                Dispatcher.UIThread.Post(() => SwitchLanguage(code));
            };
            TextBlock brand = new TextBlock { Text = "E-LAYER ANALYZER", Foreground = Pal.B(Pal.Azure), FontWeight = FontWeight.Bold, FontSize = 17, Margin = new Thickness(6, 0, 14, 0), VerticalAlignment = VerticalAlignment.Center };
            DockPanel top = new DockPanel();
            DockPanel.SetDock(cmbLang, Dock.Right); top.Children.Add(cmbLang);
            DockPanel.SetDock(lblClock, Dock.Right); top.Children.Add(lblClock);
            top.Children.Add(Ui.Row(brand, Ui.L(Lang.T("Nominativo")), txtCall, Ui.L(Lang.T("Locator")), txtLoc, Ui.L(Lang.T("Quota m")), numAlt, bApply));
            Border topB = new Border { Background = Pal.B(Pal.Panel2), Child = top };

            // --- linguette
            es = new EsTab();
            fai = new ScatterTab(false);
            YAMS.MainWindow.LanguageRequested = code => Dispatcher.UIThread.Post(() => SwitchLanguage(code));
            ms = new YAMS.MainWindow();
            au = new ScatterTab(true);
            dc = new DataTab();
            tabs.Add("SPORADIC-E", es.Root);
            tabs.Add("F.A.I.", fai.Root);
            tabs.Add("METEOR SCATTER", ms);
            tabs.Add("AURORA", au.Root);
            tabs.Add("DATA COLLECTION", dc.Root);
            tabs.Add(Lang.T("FONTI"), DocTab.Build(Path.Combine(dataDir, "ela_fonti.txt"), null, null, Lang.T("Fonti citate dal programma, linguetta per linguetta")));
            tabs.Add(Lang.T("DOCUMENTO TECNICO"), DocTab.Build(Path.Combine(dataDir, "ela_documento_tecnico.txt"), Path.Combine(dataDir, "ela_documento_tecnico.docx"),
                "E-LAYER ANALYZER - documento tecnico.docx", Lang.T("Come il programma calcola: formule e fonti a ogni passo")));
            tabs.SelectionChanged += i =>
            {
                if (i == 0) es.Refresh();
                else if (i == 1) fai.Recalc();
                else if (i == 3) au.Recalc();
            };
            int sel = (int)Station.Cfg.GetD("linguetta", 0);

            status = new TextBlock { Text = Lang.T("Pronto"), Foreground = Pal.B(Pal.Muted), TextTrimming = TextTrimming.CharacterEllipsis };
            Border sb = new Border { Background = Pal.B(Pal.Track), Padding = new Thickness(8, 3), Child = status };

            DockPanel root = new DockPanel();
            DockPanel.SetDock(topB, Dock.Top); root.Children.Add(topB);
            DockPanel.SetDock(tabs.Bar, Dock.Top); root.Children.Add(tabs.Bar);
            DockPanel.SetDock(sb, Dock.Bottom); root.Children.Add(sb);
            root.Children.Add(tabs.Host);
            Content = root;
            tabs.Select(Math.Max(0, Math.Min(tabs.Count - 1, sel)));

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) =>
            {
                lblClock.Text = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) + " UTC";
                status.Text = Lang.F("CALL3: {0:N0} nominativi{1}   Spot in memoria: {2}   Campo: {3}", CallDb.Count, CallDb.Loading ? " (" + Lang.T("caricamento") + ")" : "", SpotStore.All.Count, Igrf.Info);
            };
            timer.Start();

            Opened += OnOpened;
            Closing += (s, e) =>
            {
                timer.Stop();
                if (!replaced) { SaveAll(); ms.Shutdown(true); dc.Shutdown(); }
            };
        }

        void ApplyStation()
        {
            Station.Set(Ui.Up(txtCall), Ui.Up(txtLoc), Ui.V(numAlt));
            if (!Station.Valid) status.Text = Lang.T("Locator non valido (4, 6 o 8 caratteri, es. JN70CN).");
        }

        void SaveAll()
        {
            try
            {
                Station.Cfg.Set("linguetta", Math.Max(0, tabs.Selected));
                es.Save(); fai.Save(); au.Save(); dc.Save();
                Station.Set(Ui.Up(txtCall), Ui.Up(txtLoc), Ui.V(numAlt), false);
                Station.Save();
            }
            catch { }
        }

        void OnOpened(object sender, EventArgs e)
        {
            UiScale.Apply(this);
            if (!startedOnce)
            {
                startedOnce = true;
                string dataDir = AppPaths.DataDir;
                Igrf.Load(dataDir, DateTime.UtcNow);
                SpotStore.Init(dataDir);
                SpotStore.LoadRecent();
                CallDb.StartLoad(dataDir);
                SpaceWeather.LoadCached(dataDir);
                StartMapLoad(dataDir);
            }
            if (Program.ScreenshotDir == null) dc.ConnectSaved();
            es.Refresh(); fai.Recalc(); au.Recalc();
            if (Program.ScreenshotDir != null) _ = RunScreenshots(Program.ScreenshotDir);
        }

        static void StartMapLoad(string dataDir)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    string msg;
                    MapData.EnsureFiles(dataDir, out msg);
                    MapView.Coast = MapData.LoadAll(MapData.CoastFile(dataDir));
                    MapView.Borders = MapData.LoadAll(MapData.BorderFile(dataDir));
                }
                catch { }
                MapView.NotifyMapLoaded();
            });
        }

        // Cambio di lingua: salva tutto e ricostruisce la finestra (anche il pannello YA-MS)
        void SwitchLanguage(string code)
        {
            if (replaced || code == Lang.Code) return;
            try
            {
                SaveAll();
                ms.Shutdown(true);
                dc.Shutdown();   // la nuova finestra riapre le sessioni con "Connetti all'avvio"
                Lang.Load(code);
                Lang.ApplyCulture();
                try { Config c = new Config(Path.Combine(AppPaths.DataDir, "config.ini")); c.Set("lingua", code); c.Save(); } catch { }
                replaced = true;
                ElaWindow w = new ElaWindow();
                w.WindowStartupLocation = WindowStartupLocation.Manual;
                w.Position = Position;
                if (WindowState == WindowState.Normal) { w.Width = Width; w.Height = Height; }
                w.WindowState = WindowState == WindowState.Minimized ? WindowState.Normal : WindowState;
                if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    desktop.MainWindow = w;
                w.Show();
                Close();
            }
            catch (Exception ex) { replaced = false; status.Text = ex.Message; }
        }

        // --screenshot <cartella>: fotografa ogni linguetta (con spot di prova) e chiude il programma
        async Task RunScreenshots(string dir)
        {
            try
            {
                Directory.CreateDirectory(dir);
                if (!string.IsNullOrEmpty(Program.ScreenshotLoc)) { txtLoc.Text = Program.ScreenshotLoc; txtCall.Text = "IC8TEM"; ApplyStation(); }
                for (int k = 0; k < 60 && (MapView.Coast == null || MapView.Coast.Count == 0); k++) await Task.Delay(1000);
                for (int k = 0; k < 30 && CallDb.Loading; k++) await Task.Delay(1000);
                EsTab.AddDemoSpots();
                au.StartFetch();
                await Task.Delay(8000);
                string[] names = { "1_sporadic_e", "2_fai", "3_meteor_scatter", "4_aurora", "5_data_collection", "6_fonti", "7_documento_tecnico" };
                for (int i = 0; i < tabs.Count; i++)
                {
                    tabs.Select(i);
                    if (i == 0) { es.Refresh(); es.SelectFirst(); }
                    await Task.Delay(2500);
                    Shot(Path.Combine(dir, names[i] + ".png"));
                }
                // F.A.I.: le verifiche richieste (scia) e l'iso-riflessione su Ginevra
                tabs.Select(1);
                fai.ShotPreset(1); await Task.Delay(2500); Shot(Path.Combine(dir, "2b_fai_scia_IC8FAX_F6DRO.png"));
                fai.ShotPreset(2); await Task.Delay(2500); Shot(Path.Combine(dir, "2c_fai_scia_IC8FAX_G4LOH.png"));
                fai.ShotIso(46.2, 6.15); await Task.Delay(2500); Shot(Path.Combine(dir, "2d_fai_isoriflessione_Ginevra.png"));
                tabs.Select(4);
                for (int j = 1; j < dc.Inner.Count; j++) { dc.Inner.Select(j); await Task.Delay(1500); Shot(Path.Combine(dir, "5" + (char)('a' + j) + "_data_collection.png")); }
                // tutte le linguette di YA-MS
                tabs.Select(2);
                await Task.Delay(1500);
                await ms.RunScreenshots(Path.Combine(dir, "ya-ms"), "IN78UO", 0, this);
            }
            catch (Exception ex)
            {
                try { File.WriteAllText(Path.Combine(dir, "errore.txt"), ex.ToString()); } catch { }
            }
            Close();
        }

        void Shot(string file)
        {
            PixelSize ps = new PixelSize(Math.Max(100, (int)Bounds.Width), Math.Max(100, (int)Bounds.Height));
            using (RenderTargetBitmap rtb = new RenderTargetBitmap(ps, new Vector(96, 96)))
            {
                rtb.Render(this);
                rtb.Save(file);
            }
        }
    }
}
