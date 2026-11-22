// E-LAYER ANALYZER - finestra principale: stazione, lingua, linguette generali
// SPORADIC-E | BENCHMARK Es | BENCHMARK 50 MHz | F.A.I. | METEOR SCATTER (YA-MS) | AURORA | DATA COLLECTION | LOG ADIF | FONTI |
// SINTESI DELLE SCELTE DI CALCOLO | MANUALE D'USO | RELAZIONE TECNICA
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        public const string Version = "0.6.4";

        readonly PillTabs tabs = new PillTabs(true);
        TextBlock lblNtp, lblUpd, lblChain;
        string chainKey;
        static DispatcherTimer chainTimer;
        static DispatcherTimer ntpTimer;
        readonly PillTabs esTabs = new PillTabs();   // sottolinguette di SPORADIC-E
        readonly PillTabs auTabs = new PillTabs();   // sottolinguette di AURORA
        readonly TextBox txtCall, txtLoc;
        readonly NumericUpDown numAlt;
        readonly TextBlock lblClock, status;
        readonly ComboBox cmbLang;
        EsTab es; BenchTab bench; BenchMultiTab bench50; IonoTab iono; NubiTab nubi; ScatterTab fai, au; AuroraBenchTab auBench; DataTab dc; LogTab log; SettingsTab settings;
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
            if (Program.ShotW > 0) { Width = Program.ShotW; Height = Program.ShotH; }
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
            // avviso delle catene multi-salto in 50 MHz che riguardano la propria stazione (dalla 0.6.0): scritta rossa cliccabile
            lblChain = new TextBlock { FontFamily = Pal.Mono, FontSize = 12, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Red), Margin = new Thickness(0, 0, 10, 0), VerticalAlignment = VerticalAlignment.Center, IsVisible = false, Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand) };
            lblChain.PointerPressed += (s, e) => OpenChain();
            Chains.NewMine += OnChain;
            Current = this;
            Closed += (s, e) => Chains.NewMine -= OnChain;   // la finestra si ricostruisce al cambio di lingua o di tema
            if (chainTimer == null)
            {
                chainTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };
                chainTimer.Tick += (s, e) =>
                {
                    try { if (Station.Cfg.GetB("catene_avviso", true) && SpotStore.All.Count > 0) Chains.Notify(Precursors.Compute(es != null ? es.H : EsModel.DefaultH)); } catch { }
                    try { FaiTick(); } catch { }
                };
                chainTimer.Start();
            }
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
            DockPanel.SetDock(lblChain, Dock.Right); top.Children.Add(lblChain);
            DockPanel.SetDock(lblClock, Dock.Right); top.Children.Add(lblClock);
            // modalita' compatta: le scritte spariscono (restano nei fumetti dei campi), cosi' la riga non va a capo a 1366 pixel
            ToolTip.SetTip(txtCall, Lang.T("Nominativo")); ToolTip.SetTip(txtLoc, Lang.T("Locator")); ToolTip.SetTip(numAlt, Lang.T("Quota m"));
            top.Children.Add(Ui.Row(brand, Ui.HideCompact(Ui.L(Lang.T("Nominativo"))), txtCall, Ui.HideCompact(Ui.L(Lang.T("Locator"))), txtLoc, Ui.HideCompact(Ui.L(Lang.T("Quota m"))), numAlt, bApply));
            Border topB = new Border { Background = Pal.B(Pal.Panel2), Child = top };

            // --- linguette
            Filters.Load(Station.Cfg);   // filtri degli spot (colonna destra di DATA COLLECTION)
            Continents.Load();           // continente scelto (menu nella barra delle linguette)
            // campo IGRF prima delle linguette: i banchi di prova calcolano subito, in background, con l'epoca del QSO
            if (!Igrf.Loaded) Igrf.Load(AppPaths.DataDir, DateTime.UtcNow);
            es = new EsTab();
            bench = new BenchTab();
            bench50 = new BenchMultiTab();
            iono = new IonoTab();
            nubi = new NubiTab();
            fai = new ScatterTab(false);
            YAMS.MainWindow.LanguageRequested = code => Dispatcher.UIThread.Post(() => SwitchLanguage(code));
            YAMS.MainWindow.ThemeRequested = th => Dispatcher.UIThread.Post(() => SwitchTheme(th));
            ms = new YAMS.MainWindow();
            au = new ScatterTab(true);
            dc = new DataTab();
            log = new LogTab();
            // SPORADIC-E: cinque sottolinguette (REAL-TIME, BENCHMARK Es, BENCHMARK 50 MHz, IONOSONDE, NUBI Es NEL TEMPO);
            // le ultime due in coda, cosi' i numeri salvati delle prime tre non cambiano
            esTabs.Add(Lang.T("REAL-TIME"), es.Root);
            esTabs.Add(Lang.T("BENCHMARK Es"), bench.Root);
            esTabs.Add(Lang.T("BENCHMARK 50 MHz"), bench50.Root);
            esTabs.Add(Lang.T("IONOSONDE"), iono.Root);
            esTabs.Add(Lang.T("NUBI Es NEL TEMPO"), nubi.Root);
            esTabs.SelectionChanged += i => { if (i == 0) es.Refresh(); else if (i == 3) iono.Activate(); else if (i == 4) nubi.OnShow(); };
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
            // IMPOSTAZIONI: opzioni generali e smistamento degli spot (i filtri, prima nella colonna destra di DATA COLLECTION)
            settings = new SettingsTab(this, dc);
            tabs.Add(Lang.T("IMPOSTAZIONI"), settings.Root);
            tabs.Add(Lang.T("FONTI"), DocTab.Build(LocFile(dataDir, "ela_fonti", ".txt"), null, null, Lang.T("Fonti citate dal programma, linguetta per linguetta")));
            tabs.Add(Lang.T("SINTESI DELLE SCELTE DI CALCOLO"), DocTab.Build(LocFile(dataDir, "ela_documento_tecnico", ".txt"), LocFile(dataDir, "ela_documento_tecnico", ".docx"),
                "E-LAYER ANALYZER - sintesi delle scelte di calcolo.docx", Lang.T("Come il programma calcola: formule e fonti a ogni passo")));
            tabs.Add(Lang.T("MANUALE D'USO"), DocTab.Build(LocFile(dataDir, "ela_manuale", ".txt"), LocFile(dataDir, "ela_manuale", ".docx"),
                "E-LAYER ANALYZER - manuale d'uso.docx", Lang.T("Come si usa il programma, linguetta per linguetta")));
            tabs.Add(Lang.T("RELAZIONE TECNICA"), DocTab.Build(LocFile(dataDir, "ela_relazione", ".txt"), LocFile(dataDir, "ela_relazione", ".docx"),
                "E-LAYER ANALYZER - relazione tecnica.docx", Lang.T("Le scelte che hanno portato alla realizzazione del programma")));
            // ABACO DEI SALTI (dalla 0.6.3, richiesta dell'autore del 22/11/2026): documento di sintesi, 500-12000 km a passo 100 km
            tabs.Add(Lang.T("ABACO DEI SALTI"), DocTab.Build(LocFile(dataDir, "ela_abaco", ".txt"), LocFile(dataDir, "ela_abaco", ".docx"),
                "E-LAYER ANALYZER - abaco dei salti.docx", Lang.T("Quanti salti servono in 50 MHz da 500 a 12000 km, e da quali fonti")));
            tabs.SelectionChanged += i =>
            {
                if (i == 0) { if (esTabs.Selected == 0) es.Refresh(); else if (esTabs.Selected == 3) iono.Activate(); else if (esTabs.Selected == 4) nubi.OnShow(); }
                else if (i == 1) fai.Recalc();
                else if (i == 3) { if (auTabs.Selected == 0) au.Recalc(); }
                else if (i == 5) log.Refresh();
                UiScale.Apply(this);   // dimensione del carattere anche sulle pagine appena mostrate
            };
            // menu dei sei continenti, a destra nella barra delle linguette: cambia le viste delle mappe e i dati previsionali
            ComboBox cmbCont = Ui.Combo(Array.ConvertAll(Continents.Names, n => Lang.T(n)), Continents.Index, 150);
            cmbCont.VerticalAlignment = VerticalAlignment.Center;
            cmbCont.Margin = new Thickness(8, 0, 0, 0);
            ToolTip.SetTip(cmbCont, Lang.T("Continente: sposta le mappe di Mercatore sul continente scelto e sceglie i dati previsionali (E-skip di N0NBH per Europa e Nord America)."));
            cmbCont.SelectionChanged += (s2, e2) => Continents.Set(Math.Max(0, cmbCont.SelectedIndex));
            Continents.Changed += () => { if (cmbCont.SelectedIndex != Continents.Index) cmbCont.SelectedIndex = Continents.Index; };   // anche da IMPOSTAZIONI
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
            root.SizeChanged += (s, e) => EvalCompact(e.NewSize.Height);   // modalita' compatta (dalla 0.5.2)
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
                if (!replaced) { SaveAll(); ms.Shutdown(true); dc.Shutdown(); AprsEs.Stop(); SpotStore.ClearAll(); }   // a ogni chiusura il database degli spot si svuota
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

        // dalla linguetta IMPOSTAZIONI (Generali): stessi dati della riga in alto
        internal void SetStation(string call, string loc, double alt)
        {
            txtCall.Text = call; txtLoc.Text = loc; numAlt.Value = Ui.Dm(alt);
            ApplyStation();
        }
        internal void RequestLanguage(string code) { Dispatcher.UIThread.Post(() => SwitchLanguage(code)); }
        internal void RequestTheme(int schema) { Dispatcher.UIThread.Post(() => SwitchTheme(schema)); }

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

        // --- modalita' compatta per schermi bassi (dalla 0.5.2, come YA-MS 1.8.20): automatica quando la finestra e' alta meno
        //     di 850 pixel (si spegne sopra 900), oppure sempre o mai (IMPOSTAZIONI > Aspetto). Vale anche per METEOR SCATTER.
        public const double CompactOn = 850, CompactOff = 900;
        public static int CompactMode { get { return Station.Cfg != null ? (int)Station.Cfg.GetD("modalita_compatta", 0) : 0; } }
        bool compactDecided;

        void EvalCompact(double h)
        {
            if (h <= 0) return;
            int m = CompactMode;
            bool on = m == 1 || (m == 0 && (Ui.Compact ? h < CompactOff : h < CompactOn));
            if (on == Ui.Compact && compactDecided) return;
            compactDecided = true;
            Dispatcher.UIThread.Post(() => ApplyCompact(on));
        }

        void ApplyCompact(bool on)
        {
            Ui.SetCompact(on);
            try { if (ms != null) ms.HostCompact(on); } catch { }
        }

        // cambio dell'impostazione (IMPOSTAZIONI > Aspetto)
        public void CompactModeChanged()
        {
            int m = CompactMode;
            double h = Content is Control c ? c.Bounds.Height : 0;
            ApplyCompact(m == 1 || (m == 0 && h > 0 && h < CompactOn));
        }

        int chainKind;
        DateTime chainAt;

        // ALLERTA F.A.I. (FaiAlert.cs, dal 22/11/2026): la linguetta F.A.I. lampeggia; il testo va in cima alla linguetta
        static ElaWindow Current;
        static bool aprsStarted;
        static void FaiTick()
        {
            ElaWindow w = Current;
            if (w == null || w.fai == null) return;
            FaiAlert.Compute(DateTime.UtcNow);
            bool on = Station.Cfg.GetB("fai_avviso", true) && FaiAlert.Level > 0;
            w.fai.SetAlert(FaiAlert.Text);
            w.tabs.SetBlink(1, on, on ? FaiAlert.Text : "");
            // ALLERTA 144 MHz APRS (AprsEs.cs): lampeggia SPORADIC-E se il salto ripetuto e' entro 2400 km dalla propria stazione
            List<AprsAlert> ap = AprsEs.Compute().Where(a => double.IsNaN(a.NearKm) || a.NearKm <= AprsEs.MaxKm).ToList();
            w.tabs.SetBlink(0, ap.Count > 0, ap.Count > 0 ? Lang.F("ALLERTA 144 MHz: salti APRS ripetuti oltre i 900 km su {0} ({1})", ap[0].Region, ap[0].Calls) : "");
        }

        void OnChain(ChainAlert c)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (!Station.Cfg.GetB("catene_avviso", true)) return;
                // una catena dal QTH resta nella testata per un'ora: un corridoio arrivato dopo non la copre
                if (lblChain.IsVisible && chainKind == ChainAlert.KindQth && c.Kind != ChainAlert.KindQth && (DateTime.UtcNow - chainAt).TotalMinutes < 60) return;
                chainKey = c.Key; chainKind = c.Kind; chainAt = DateTime.UtcNow;
                // la zona lontana dalla propria stazione (per il corridoio: l'estremita' opposta a quella vicina)
                string reg = (c.Kind == ChainAlert.KindCorridor || c.Kind == ChainAlert.KindEqualCorridor) && Geo.DistanceKm(Station.Lat, Station.Lon, c.EndBLat, c.EndBLon) < Geo.DistanceKm(Station.Lat, Station.Lon, c.EndALat, c.EndALon) ? c.EndAName : c.EndBName;
                if (reg.Length > 30) reg = reg.Substring(0, 29) + "…";
                lblChain.Text = "⚠ 6 m " + reg;   // scritta corta: la riga della testata non va a capo
                ToolTip.SetTip(lblChain, Lang.F("ALLERTA multi-salto in 50 MHz ({0}): {1}, {2:F0} km. Clic per aprire i precursori.", c.KindText, c.Region, c.DistKm));
                lblChain.IsVisible = true;
                if (Station.Cfg.GetB("catene_finestra", false) && Program.ScreenshotDir == null) ChainWindow(c);
            });
        }

        void OpenChain()
        {
            lblChain.IsVisible = false;
            tabs.Select(0); esTabs.Select(0);
            if (chainKey != null) es.ShowAlert(chainKey);
        }

        void ChainWindow(ChainAlert c)
        {
            Button bShow = Ui.B(Lang.T("Mostra"), true), bClose = Ui.B(Lang.T("Chiudi"));
            Window w = new Window
            {
                Title = "E-LAYER ANALYZER - " + Lang.T("ALLERTA"), Width = 460, SizeToContent = SizeToContent.Height, CanResize = false, Topmost = true,
                WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = Pal.B(Pal.Back)
            };
            w.Content = new StackPanel
            {
                Margin = new Thickness(14), Spacing = 10,
                Children =
                {
                    new TextBlock { Text = Lang.T("ALLERTA multi-salto in 50 MHz"), Foreground = Pal.B(Pal.Red), FontWeight = FontWeight.Bold, FontSize = 16 },
                    new TextBlock { Text = Lang.F("{0}: {1}, {2:F0} km, azimut {3:F0}°", c.KindText, c.Region, c.DistKm, c.Az), Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap },
                    Ui.Row(bShow, bClose)
                }
            };
            bShow.Click += (s, e) => { w.Close(); OpenChain(); };
            bClose.Click += (s, e) => w.Close();
            try { w.Show(this); } catch { }
        }

        void OnOpened(object sender, EventArgs e)
        {
            UiScale.Apply(this);
            if (!startedOnce)
            {
                startedOnce = true;
                // schermo piu' piccolo della finestra (notebook da 13-15 pollici): finestra ingrandita a tutto schermo
                try
                {
                    Avalonia.Platform.Screen sc = Program.ScreenshotDir == null ? (Screens.ScreenFromVisual(this) ?? Screens.Primary) : null;
                    if (sc != null && WindowState == WindowState.Normal && (Height > sc.WorkingArea.Height / sc.Scaling || Width > sc.WorkingArea.Width / sc.Scaling))
                        WindowState = WindowState.Maximized;
                }
                catch { }
                string dataDir = AppPaths.DataDir;
                if (!Igrf.Loaded) Igrf.Load(dataDir, DateTime.UtcNow);
                if (Program.ScreenshotDir == null) Igrf.CheckUpdateSilent(dataDir, Station.Cfg);   // nuova generazione IGRF, senza chiedere
                SpotStore.Init(dataDir);
                SpotStore.Archive = Station.Cfg.GetB("spot_archivio", true);
                SpotStore.LoadRecent();
                if (!aprsStarted && Program.ScreenshotDir == null)
                {
                    // APRS-IS in sola lettura per l'E-sporadico in 144 MHz (AprsEs.cs, dal 22/11/2026): una sola volta per esecuzione
                    aprsStarted = true;
                    AprsEs.Enabled = Station.Cfg.GetB("aprs_es", true);
                    AprsEs.NewSpot += s => Dispatcher.UIThread.Post(() => SpotStore.Add(s));
                    Station.Changed += () => { if (AprsEs.Enabled) AprsEs.Start(); };
                    AprsEs.Start();
                }
                Giro.Init(dataDir);   // archivio delle ionosonde GIRO (ultime 48 ore); i dati nuovi arrivano all'apertura di IONOSONDE
                iono.Refresh();
                if (tabs.Selected == 0 && esTabs.Selected == 3) iono.Activate();   // IONOSONDE riaperta all'avvio
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
                string[] names = { "1_sporadic_e", "2_fai", "3_meteor_scatter", "4_aurora", "5_data_collection", "6_log_adif", "6b_impostazioni", "7_fonti", "8_documento_tecnico", "9_manuale", "10_relazione", "11_abaco" };
                string[] esNames = { "1_sporadic_e", "1b_benchmark_es", "1g_benchmark_50", "1m_ionosonde", "1l_nubi_es_nel_tempo" };
                await nubi.LoadFile(bdemo);   // NUBI Es NEL TEMPO: il log di prova del BENCHMARK Es
                for (int i = 0; i < tabs.Count; i++)
                {
                    tabs.Select(i);
                    if (i == 0)
                    {
                        for (int j = 0; j < esTabs.Count; j++)
                        {
                            esTabs.Select(j);
                            if (j == 0) { es.Refresh(); es.SelectFirst(); }
                            if (j == 3) { await iono.ShotWait(90); try { File.WriteAllText(Path.Combine(dir, "1m_ionosonde.txt"), iono.ShotText()); } catch { } }
                            await Task.Delay(2500);
                            if (j == 4) { try { File.WriteAllText(Path.Combine(dir, "1l_nubi_es_nel_tempo.txt"), nubi.ShotText()); } catch { } }
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
                // catene a salti corti: dal QTH (Kazakhstan) e corridoio (Caraibi - Penisola Iberica), con il testo dei dettagli
                es.ShotChain(ChainAlert.KindQth); await Task.Delay(2500); Shot(Path.Combine(dir, "1n_catena_dal_qth.png"));
                try { File.WriteAllText(Path.Combine(dir, "1n_catena_dal_qth.txt"), es.DetailsText); } catch { }
                es.ShotChain(ChainAlert.KindCorridor); await Task.Delay(2500); Shot(Path.Combine(dir, "1o_catena_corridoio.png"));
                try { File.WriteAllText(Path.Combine(dir, "1o_catena_corridoio.txt"), es.DetailsText); } catch { }
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
                // verifica delle catene a salti corti sul log di prova
                await bench50.Verify(); await Task.Delay(1500); Shot(Path.Combine(dir, "1p_benchmark_50_verifica_catene.png"));
                try { File.WriteAllText(Path.Combine(dir, "1p_benchmark_50_verifica_catene.txt"), bench50.VerifyText); } catch { }
                // F.A.I.: le verifiche richieste (scia) e l'iso-riflessione su Ginevra
                tabs.Select(1);
                fai.ShotPreset(1); await Task.Delay(2500); Shot(Path.Combine(dir, "2b_fai_scia_IC8FAX_F6DRO.png"));
                fai.ShotPreset(2); await Task.Delay(2500); Shot(Path.Combine(dir, "2c_fai_scia_IC8FAX_G4LOH.png"));
                fai.ShotIso(46.2, 6.15); await Task.Delay(2500); Shot(Path.Combine(dir, "2d_fai_isoriflessione_Ginevra.png"));
                try { File.WriteAllText(Path.Combine(dir, "2d_fai_isoriflessione_Ginevra.txt"), fai.ResultText); } catch { }
                // casi di verifica con la data: campo IGRF dell'anno (1990, 1987); risultati in testo per i documenti
                fai.ShotPreset(3); await Task.Delay(2500); Shot(Path.Combine(dir, "2f_fai_scia_IC8FAX_FA1MXE_1990.png"));
                try { File.WriteAllText(Path.Combine(dir, "2f_fai_scia_IC8FAX_FA1MXE_1990.txt"), fai.ResultText); } catch { }
                fai.ShotPreset(4); await Task.Delay(2500); Shot(Path.Combine(dir, "2g_fai_scia_DL5MCG_SV1DH_1987.png"));
                try { File.WriteAllText(Path.Combine(dir, "2g_fai_scia_DL5MCG_SV1DH_1987.txt"), fai.ResultText); } catch { }
                fai.ShotIsoPair("DL5MCG JN58KH", "SV1DH KM27AW", 47.50, 19.04); await Task.Delay(2500); Shot(Path.Combine(dir, "2h_fai_isoriflessione_Budapest_1987.png"));
                try { File.WriteAllText(Path.Combine(dir, "2h_fai_isoriflessione_Budapest_1987.txt"), fai.ResultText); } catch { }
                fai.ShotPair("IC8FAX JN70CN", "G4LOH IO70JC"); await Task.Delay(1500); fai.ShotVerify(); await Task.Delay(2500); Shot(Path.Combine(dir, "2e_fai_verifica_IC8FAX_G4LOH.png"));
                tabs.Select(4);
                for (int j = 1; j < dc.Inner.Count; j++) { dc.Inner.Select(j); await Task.Delay(1500); Shot(Path.Combine(dir, "5" + (char)('a' + j) + "_data_collection.png")); }
                dc.Inner.Select(0);
                dc.Outer.Select(1); await Task.Delay(2500); Shot(Path.Combine(dir, "5g_dati_propagazione.png"));
                dc.Outer.Select(0);
                // IMPOSTAZIONI: smistamento degli spot con i filtri di ciascun modo
                tabs.Select(6); settings.Inner.Select(1);
                string[] fn = { "ms", "es", "fai", "aurora", "prova" };
                for (int j = 0; j < dc.FilterTabs.Count; j++) { dc.FilterTabs.Select(j); await Task.Delay(1200); Shot(Path.Combine(dir, "6c_smistamento_" + fn[Math.Min(j, fn.Length - 1)] + ".png")); }
                dc.FilterTabs.Select(0); settings.Inner.Select(0);
                // AURORA: banco di prova sui QSO reali (EI5FK, PA0O)
                tabs.Select(3); auTabs.Select(1);
                for (int k = 0; k < 120 && auBench.Busy; k++) await Task.Delay(1000);
                auBench.ShotSelect(); await Task.Delay(2500); Shot(Path.Combine(dir, "4k_benchmark_aurora.png"));
                try { File.WriteAllText(Path.Combine(dir, "4k_benchmark_aurora.txt"), auBench.SummaryText); } catch { }   // riepilogo intero, per i documenti
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
