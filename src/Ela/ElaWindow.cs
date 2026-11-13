// E-LAYER ANALYZER - finestra principale: stazione, lingua, linguette generali
// SPORADIC-E | BENCHMARK Es | BENCHMARK 50 MHz | F.A.I. | METEOR SCATTER (YA-MS) | AURORA | DATA COLLECTION | LOG ADIF | FONTI |
// SINTESI DELLE SCELTE DI CALCOLO | MANUALE D'USO | RELAZIONE TECNICA
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
        public const string Version = "0.4.10";

        readonly PillTabs tabs = new PillTabs(true);
        TextBlock lblNtp, lblUpd;
        static DispatcherTimer ntpTimer;
        readonly PillTabs esTabs = new PillTabs();   // sottolinguette di SPORADIC-E
        readonly PillTabs auTabs = new PillTabs();   // sottolinguette di AURORA
        readonly TextBox txtCall, txtLoc;
        readonly NumericUpDown numAlt;
        readonly TextBlock lblClock, status;
        readonly ComboBox cmbLang;
        EsTab es; BenchTab bench; BenchMultiTab bench50; ScatterTab fai, au; AuroraBenchTab auBench; DataTab dc; LogTab log;
        readonly ComboBox cmbTheme;
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
            lblNtp = new TextBlock { FontFamily = Pal.Mono, FontSize = 12, FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 0, 10, 0), VerticalAlignment = VerticalAlignment.Center };
            UpdateNtp();
            Ntp.Changed += OnNtp;
            // versione in uso; diventa un avviso cliccabile quando su GitHub esce una versione nuova
            lblUpd = new TextBlock { FontFamily = Pal.Mono, FontSize = 12, FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 0, 10, 0), VerticalAlignment = VerticalAlignment.Center, Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) };
            lblUpd.PointerPressed += async (s, e) => { await Updater.CheckAsync(); await Updater.ShowDialog(this); };
            UpdateUpd();
            Updater.Changed += OnUpd;
            if (ntpTimer == null)
            {
                // allineatore dell'orologio: elemento nascosto, un controllo all'avvio e poi ogni 5 minuti
                ntpTimer = new DispatcherTimer { Interval = Ntp.Interval };
                ntpTimer.Tick += (s, e) => { _ = Ntp.CheckAsync(); };
                ntpTimer.Start();
                _ = Ntp.CheckAsync();
            }
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
            cmbTheme = new ComboBox { MinWidth = 170, Margin = new Thickness(6, 1, 0, 1), VerticalAlignment = VerticalAlignment.Center };
            List<string> tn = new List<string>();
            foreach (string t in Tema.Names) tn.Add(Lang.T(t));
            cmbTheme.ItemsSource = tn;
            cmbTheme.SelectedIndex = Tema.Schema;
            ToolTip.SetTip(cmbTheme, Lang.T("Tema"));
            cmbTheme.SelectionChanged += (s, e) =>
            {
                if (cmbTheme.SelectedIndex < 0 || cmbTheme.SelectedIndex == Tema.Schema) return;
                int ti = cmbTheme.SelectedIndex;
                Dispatcher.UIThread.Post(() => SwitchTheme(ti));
            };
            TextBlock brand = new TextBlock { Text = "E-LAYER ANALYZER", Foreground = Pal.B(Pal.Azure), FontWeight = FontWeight.Bold, FontSize = 17, Margin = new Thickness(6, 0, 14, 0), VerticalAlignment = VerticalAlignment.Center };
            DockPanel top = new DockPanel();
            DockPanel.SetDock(cmbLang, Dock.Right); top.Children.Add(cmbLang);
            DockPanel.SetDock(cmbTheme, Dock.Right); top.Children.Add(cmbTheme);
            DockPanel.SetDock(lblNtp, Dock.Right); top.Children.Add(lblNtp);
            DockPanel.SetDock(lblUpd, Dock.Right); top.Children.Add(lblUpd);
            DockPanel.SetDock(lblClock, Dock.Right); top.Children.Add(lblClock);
            top.Children.Add(Ui.Row(brand, Ui.L(Lang.T("Nominativo")), txtCall, Ui.L(Lang.T("Locator")), txtLoc, Ui.L(Lang.T("Quota m")), numAlt, bApply));
            Border topB = new Border { Background = Pal.B(Pal.Panel2), Child = top };

            // --- linguette
            Filters.Load(Station.Cfg);   // filtri degli spot (colonna destra di DATA COLLECTION)
            Continents.Load();           // continente scelto (menu nella barra delle linguette)
            es = new EsTab();
            bench = new BenchTab();
            bench50 = new BenchMultiTab();
            fai = new ScatterTab(false);
            YAMS.MainWindow.LanguageRequested = code => Dispatcher.UIThread.Post(() => SwitchLanguage(code));
            YAMS.MainWindow.ThemeRequested = th => Dispatcher.UIThread.Post(() => SwitchTheme(th));
            ms = new YAMS.MainWindow();
            au = new ScatterTab(true);
            dc = new DataTab();
            log = new LogTab();
            // SPORADIC-E: tre sottolinguette (REAL-TIME, BENCHMARK Es, BENCHMARK 50 MHz)
            esTabs.Add(Lang.T("REAL-TIME"), es.Root);
            esTabs.Add(Lang.T("BENCHMARK Es"), bench.Root);
            esTabs.Add(Lang.T("BENCHMARK 50 MHz"), bench50.Root);
            esTabs.SelectionChanged += i => { if (i == 0) es.Refresh(); };
            DockPanel esPage = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(esPage, esTabs.Bar, Dock.Top);
            esPage.Children.Add(esTabs.Host);
            tabs.Add(Lang.T("SPORADIC-E"), esPage);
            tabs.Add(Lang.T("F.A.I."), fai.Root);
            tabs.Add(Lang.T("METEOR SCATTER"), ms);
            // AURORA: sottolinguette REAL-TIME (calcolo e spot in diretta) e BENCHMARK AURORA (QSO reali come banco di prova)
            auBench = new AuroraBenchTab();
            auTabs.Add(Lang.T("REAL-TIME"), au.Root);
            auTabs.Add(Lang.T("BENCHMARK AURORA"), auBench.Root);
            auTabs.SelectionChanged += i => { if (i == 0) au.Recalc(); };
            DockPanel auPage = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(auPage, auTabs.Bar, Dock.Top);
            auPage.Children.Add(auTabs.Host);
            tabs.Add(Lang.T("AURORA"), auPage);
            tabs.Add(Lang.T("DATA COLLECTION"), dc.Root);
            tabs.Add(Lang.T("LOG ADIF"), log.Root);
            tabs.Add(Lang.T("FONTI"), DocTab.Build(LocFile(dataDir, "ela_fonti", ".txt"), null, null, Lang.T("Fonti citate dal programma, linguetta per linguetta")));
            tabs.Add(Lang.T("SINTESI DELLE SCELTE DI CALCOLO"), DocTab.Build(LocFile(dataDir, "ela_documento_tecnico", ".txt"), LocFile(dataDir, "ela_documento_tecnico", ".docx"),
                "E-LAYER ANALYZER - sintesi delle scelte di calcolo.docx", Lang.T("Come il programma calcola: formule e fonti a ogni passo")));
            tabs.Add(Lang.T("MANUALE D'USO"), DocTab.Build(LocFile(dataDir, "ela_manuale", ".txt"), LocFile(dataDir, "ela_manuale", ".docx"),
                "E-LAYER ANALYZER - manuale d'uso.docx", Lang.T("Come si usa il programma, linguetta per linguetta")));
            tabs.Add(Lang.T("RELAZIONE TECNICA"), DocTab.Build(LocFile(dataDir, "ela_relazione", ".txt"), LocFile(dataDir, "ela_relazione", ".docx"),
                "E-LAYER ANALYZER - relazione tecnica.docx", Lang.T("Le scelte che hanno portato alla realizzazione del programma")));
            tabs.SelectionChanged += i =>
            {
                if (i == 0) { if (esTabs.Selected == 0) es.Refresh(); }
                else if (i == 1) fai.Recalc();
                else if (i == 3) { if (auTabs.Selected == 0) au.Recalc(); }
                else if (i == 5) log.Refresh();
            };
            // menu dei sei continenti, a destra nella barra delle linguette: cambia le viste delle mappe e i dati previsionali
            ComboBox cmbCont = Ui.Combo(Array.ConvertAll(Continents.Names, n => Lang.T(n)), Continents.Index, 150);
            cmbCont.VerticalAlignment = VerticalAlignment.Center;
            cmbCont.Margin = new Thickness(8, 0, 0, 0);
            ToolTip.SetTip(cmbCont, Lang.T("Continente: sposta le mappe di Mercatore sul continente scelto e sceglie i dati previsionali (E-skip di N0NBH per Europa e Nord America)."));
            cmbCont.SelectionChanged += (s2, e2) => Continents.Set(Math.Max(0, cmbCont.SelectedIndex));
            DockPanel.SetDock(cmbCont, Dock.Right);
            tabs.Line.Children.Insert(0, cmbCont);
            int sel = (int)Station.Cfg.GetD("linguetta2", 0);   // "linguetta2": numerazione con le sottolinguette di SPORADIC-E
            esTabs.Select(Math.Max(0, Math.Min(esTabs.Count - 1, (int)Station.Cfg.GetD("linguetta_es", 0))));

            status = new TextBlock { Text = Lang.T("Pronto"), Foreground = Pal.B(Pal.BarMuted), TextTrimming = TextTrimming.CharacterEllipsis };
            Border sb = new Border { Background = Pal.B(Pal.Track), Padding = new Thickness(8, 3), Child = status };

            DockPanel root = new DockPanel();
            DockPanel.SetDock(topB, Dock.Top); root.Children.Add(topB);
            DockPanel.SetDock(tabs.Bar, Dock.Top); root.Children.Add(tabs.Bar);
            DockPanel.SetDock(sb, Dock.Bottom); root.Children.Add(sb);
            root.Children.Add(tabs.Host);
            Content = root;
            Tips.Apply(root);   // fumetti esplicativi su linguette, pulsanti, caselle ed etichette
            tabs.Select(Math.Max(0, Math.Min(tabs.Count - 1, sel)));

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) =>
            {
                lblClock.Text = Ntp.UtcNow.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) + " UTC";
                status.Text = Lang.F("CALL3: {0:N0} nominativi{1}   Spot in memoria: {2}   Modello magnetico: {3}", CallDb.Count, CallDb.Loading ? " (" + Lang.T("caricamento") + ")" : "", SpotStore.All.Count, Igrf.Info);
            };
            timer.Start();

            Opened += OnOpened;
            Closing += (s, e) =>
            {
                timer.Stop();
                Ntp.Changed -= OnNtp;
                Updater.Changed -= OnUpd;
                if (!replaced) { SaveAll(); ms.Shutdown(true); dc.Shutdown(); SpotStore.ClearAll(); }   // a ogni chiusura il database degli spot si svuota
            };
        }

        void OnNtp() { Dispatcher.UIThread.Post(UpdateNtp); }
        void OnUpd() { Dispatcher.UIThread.Post(UpdateUpd); }

        // "v0.4.4" in grigio; con una versione nuova su GitHub: "▲ 0.4.5" in arancio, cliccabile
        void UpdateUpd()
        {
            if (Updater.Newer)
            {
                lblUpd.Text = "▲ " + Updater.LatestVersion;
                lblUpd.Foreground = Pal.B(Pal.Orange);
                ToolTip.SetTip(lblUpd, Lang.F("È disponibile la versione {0} (in uso: {1}): clic per aggiornare.", Updater.LatestVersion, Version));
            }
            else
            {
                lblUpd.Text = "v" + Version;
                lblUpd.Foreground = Pal.B(Pal.BarMuted);
                ToolTip.SetTip(lblUpd, Lang.T("Versione in uso: clic per controllare gli aggiornamenti su GitHub."));
            }
        }

        // NTP:OK in verde, NTP:FAILED in rosso; il fumetto riporta server, scarto e ora del controllo
        void UpdateNtp()
        {
            if (Ntp.Ok == null) { lblNtp.Text = "NTP:…"; lblNtp.Foreground = Pal.B(Pal.BarMuted); ToolTip.SetTip(lblNtp, Lang.T("Controllo dell'ora in corso")); return; }
            bool ok = Ntp.Ok == true;
            lblNtp.Text = ok ? "NTP:OK" : "NTP:FAILED";
            lblNtp.Foreground = Pal.B(ok ? Pal.Green : Pal.Red);
            string when = Ntp.LastCheckUtc.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + " UTC";
            ToolTip.SetTip(lblNtp, ok
                ? Lang.F("Ora allineata con {0} (scarto del PC {1:+0.000;-0.000} s, controllo delle {2}); si ricontrolla ogni 5 minuti.", Ntp.LastServers, Ntp.OffsetSec, when)
                  + (Ntp.SystemClockSet ? " " + Lang.T("L'orologio del PC è stato corretto.") : "")
                : Lang.F("Nessun server NTP ha risposto (controllo delle {0}); si riprova tra 5 minuti con altri server.", when));
        }

        // documento nella lingua scelta (data/<nome>_<codice>.txt), altrimenti quello italiano
        static string LocFile(string dataDir, string name, string ext)
        {
            if (Lang.Code != "it")
            {
                string f = Path.Combine(dataDir, name + "_" + Lang.Code + ext);
                if (File.Exists(f)) return f;
                string g = Path.Combine(AppContext.BaseDirectory, "data", name + "_" + Lang.Code + ext);
                if (File.Exists(g)) return g;
            }
            return Path.Combine(dataDir, name + ext);
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
                Station.Cfg.Set("linguetta2", Math.Max(0, tabs.Selected)); Station.Cfg.Set("linguetta_es", Math.Max(0, esTabs.Selected));
                es.Save(); bench.Save(); bench50.Save(); fai.Save(); au.Save(); dc.Save(); log.Save();
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
                LogStore.Init(dataDir);
                LogTab.StartReload();
                StartMapLoad(dataDir);
            }
            if (Program.ScreenshotDir == null) dc.ConnectSaved();
            es.Refresh(); fai.Recalc(); au.Recalc();
            if (Program.ScreenshotDir != null) _ = RunScreenshots(Program.ScreenshotDir);
            else if (!checkedFiles)
            {
                checkedFiles = true;
                Dispatcher.UIThread.Post(async () => { await Task.Delay(3000); await Updater.CheckMissing(this); Updater.Start(this); });
            }
        }
        static bool checkedFiles;

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

        // Cambio dello schema di colori (Tema di YA-MS): salva e ricostruisce la finestra come per la lingua
        void SwitchTheme(int schema)
        {
            if (replaced || schema == Tema.Schema) return;
            int old = Tema.Schema;
            try
            {
                SaveAll();
                ms.Shutdown(true);
                dc.Shutdown();
                Tema.Imposta(schema);
                Themes.FromPal();
                try { Config c = new Config(Path.Combine(AppPaths.DataDir, "config.ini")); c.Set("tema", Tema.Chiave); c.Save(); } catch { }
                Rebuild();
            }
            catch (Exception ex) { replaced = false; Tema.Imposta(old); Themes.FromPal(); status.Text = ex.Message; }
        }

        void Rebuild()
        {
            replaced = true;
            int sel = tabs.Selected, esSel = esTabs.Selected;
            ElaWindow w = new ElaWindow();
            w.WindowStartupLocation = WindowStartupLocation.Manual;
            w.Position = Position;
            if (WindowState == WindowState.Normal) { w.Width = Width; w.Height = Height; }
            w.WindowState = WindowState == WindowState.Minimized ? WindowState.Normal : WindowState;
            if (esSel >= 0) w.esTabs.Select(esSel);
            if (sel >= 0) w.tabs.Select(sel);
            if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = w;
            w.Show();
            Close();
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
                Rebuild();
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
                if (Program.AuroraA != null)
                {
                    // bozzetto del calcolo aurora per una coppia di locator (--aurora A B)
                    tabs.Select(3);
                    au.StartFetch();
                    await Task.Delay(12000);
                    au.View(-25, 35, 45, 75);
                    au.ShotPair(Program.AuroraA, Program.AuroraB); await Task.Delay(3000);
                    Shot(Path.Combine(dir, "aurora_scia_" + Program.AuroraA + "_" + Program.AuroraB + ".png"));
                    if (au.LastBest != null)
                    {
                        au.ShotIsoA(Program.AuroraA, au.LastBest.Lat, au.LastBest.Lon); await Task.Delay(3000);
                        Shot(Path.Combine(dir, "aurora_isoriflessione_" + Program.AuroraA + ".png"));
                    }
                    Close();
                    return;
                }
                EsTab.AddDemoSpots();
                // log di prova per la linguetta LOG ADIF
                string demo = Path.Combine(LogStore.Dir, "prova.adi");
                File.WriteAllText(demo, "<eoh>\n" +
                    "<call:5>EA8DO<band:2>6m<mode:3>FT8<qso_date:8>20250612<time_on:4>1012<gridsquare:6>IL18QI<prop_mode:2>ES<lotw_qsl_rcvd:1>Y<eor>\n" +
                    "<call:6>F5XYZ1<band:2>2m<mode:2>CW<qso_date:8>20240620<time_on:4>1930<gridsquare:4>JN18<prop_mode:3>FAI<qsl_rcvd:1>Y<eor>\n" +
                    "<call:5>G4LOH<band:2>2m<mode:3>SSB<qso_date:8>20230615<time_on:4>2012<gridsquare:6>IO70JC<prop_mode:3>FAI<eor>\n" +
                    "<call:5>SV1DH<band:2>4m<mode:3>FT8<qso_date:8>20250701<time_on:4>1100<gridsquare:4>KM17<prop_mode:2>ES<lotw_qsl_rcvd:1>Y<eor>\n" +
                    "<call:4>W1JJ<band:2>6m<mode:3>FT8<qso_date:8>20250618<time_on:4>1730<gridsquare:4>FN42<prop_mode:2>ES<eor>\n" +
                    "<call:5>OH5IY<band:2>2m<mode:6>MSK144<qso_date:8>20251213<time_on:4>0230<gridsquare:4>KP30<prop_mode:2>MS<lotw_qsl_rcvd:1>Y<eor>\n");
                lock (LogStore.Files) if (!LogStore.Files.Contains(demo)) LogStore.Files.Add(demo);
                LogTab.StartReload();
                au.StartFetch();
                await Task.Delay(8000);
                // log di prova per la linguetta BENCHMARK Es
                string bdemo = Path.Combine(LogStore.Dir, "benchmark_prova.adi");
                File.WriteAllText(bdemo, BenchTab.DemoAdif());
                await bench.LoadFile(bdemo);
                string b50demo = Path.Combine(LogStore.Dir, "benchmark50_prova.adi");
                File.WriteAllText(b50demo, BenchMultiTab.DemoAdif());
                await bench50.LoadFile(0, b50demo);
                string b50demo2 = Path.Combine(LogStore.Dir, "benchmark50_prova2.adi");
                File.WriteAllText(b50demo2, BenchMultiTab.DemoAdif("IK0XYZ", "JN61FW", 1));
                await bench50.LoadFile(1, b50demo2);   // seconda stazione (casella "Stazione 2" accesa)
                string[] names = { "1_sporadic_e", "2_fai", "3_meteor_scatter", "4_aurora", "5_data_collection", "6_log_adif", "7_fonti", "8_documento_tecnico", "9_manuale", "10_relazione" };
                string[] esNames = { "1_sporadic_e", "1b_benchmark_es", "1g_benchmark_50" };
                for (int i = 0; i < tabs.Count; i++)
                {
                    tabs.Select(i);
                    if (i == 0)
                    {
                        for (int j = 0; j < esTabs.Count; j++)
                        {
                            esTabs.Select(j);
                            if (j == 0) { es.Refresh(); es.SelectFirst(); }
                            await Task.Delay(2500);
                            Shot(Path.Combine(dir, esNames[j] + ".png"));
                        }
                        esTabs.Select(0);
                        continue;
                    }
                    await Task.Delay(2500);
                    Shot(Path.Combine(dir, names[i] + ".png"));
                }
                // SPORADIC-E: azimutale allargata (multi-salto) e Mercatore, con i cerchi delle distanze di IK3XTV
                tabs.Select(0); esTabs.Select(0);
                es.ShotMap(0, 8000); await Task.Delay(2500); Shot(Path.Combine(dir, "1c_sporadic_e_multisalto.png"));
                es.ShotMap(1, 0); await Task.Delay(2500); Shot(Path.Combine(dir, "1d_sporadic_e_mercatore.png"));
                es.ShotPrecursors(); await Task.Delay(2500); Shot(Path.Combine(dir, "1k_sporadic_e_precursori.png"));
                es.ShotMap(0, 3000);
                // BENCHMARK Es: un'ora della giornata e lo zoom
                esTabs.Select(1);
                bench.ShotSelect("2025-06-15", 11, 9000); await Task.Delay(2500); Shot(Path.Combine(dir, "1e_benchmark_es_ora.png"));
                bench.ShotSelect("2025-06-15", -1, 1500); await Task.Delay(2500); Shot(Path.Combine(dir, "1f_benchmark_es_zoom.png"));
                // BENCHMARK 50 MHz: tratta ingrandita (mouse vicino), foEs, dintorni di 150 km dopo il popup
                esTabs.Select(2);
                bench50.ShotHover(4); await Task.Delay(2000); Shot(Path.Combine(dir, "1h_benchmark_50_tratta.png"));
                bench50.ShotFoEs(true); await Task.Delay(2000); Shot(Path.Combine(dir, "1i_benchmark_50_foes.png"));
                bench50.ShotFoEs(false); bench50.ShotHover(4); bench50.ShotArea(4); await Task.Delay(2500); Shot(Path.Combine(dir, "1j_benchmark_50_dintorni.png"));
                // F.A.I.: le verifiche richieste (scia) e l'iso-riflessione su Ginevra
                tabs.Select(1);
                fai.ShotPreset(1); await Task.Delay(2500); Shot(Path.Combine(dir, "2b_fai_scia_IC8FAX_F6DRO.png"));
                fai.ShotPreset(2); await Task.Delay(2500); Shot(Path.Combine(dir, "2c_fai_scia_IC8FAX_G4LOH.png"));
                fai.ShotIso(46.2, 6.15); await Task.Delay(2500); Shot(Path.Combine(dir, "2d_fai_isoriflessione_Ginevra.png"));
                fai.ShotPair("IC8FAX JN70CN", "G4LOH IO70JC"); await Task.Delay(1500); fai.ShotVerify(); await Task.Delay(2500); Shot(Path.Combine(dir, "2e_fai_verifica_IC8FAX_G4LOH.png"));
                tabs.Select(4);
                for (int j = 1; j < dc.Inner.Count; j++) { dc.Inner.Select(j); await Task.Delay(1500); Shot(Path.Combine(dir, "5" + (char)('a' + j) + "_data_collection.png")); }
                dc.Inner.Select(0);
                string[] fn = { "ms", "es", "fai", "aurora", "prova" };
                for (int j = 1; j < dc.FilterTabs.Count; j++) { dc.FilterTabs.Select(j); await Task.Delay(1200); Shot(Path.Combine(dir, "5f_filtri_" + fn[Math.Min(j, fn.Length - 1)] + ".png")); }
                dc.FilterTabs.Select(0);
                dc.Outer.Select(1); await Task.Delay(2500); Shot(Path.Combine(dir, "5g_dati_propagazione.png"));
                dc.Outer.Select(0);
                // AURORA: banco di prova sui QSO reali (EI5FK, PA0O)
                tabs.Select(3); auTabs.Select(1);
                for (int k = 0; k < 120 && auBench.Busy; k++) await Task.Delay(1000);
                auBench.ShotSelect(); await Task.Delay(2500); Shot(Path.Combine(dir, "4k_benchmark_aurora.png"));
                auTabs.Select(0);
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
