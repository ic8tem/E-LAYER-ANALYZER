// YA-MS - finestra principale (porting di MainForm.cs da WinForms ad Avalonia; interfaccia costruita in codice)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow : UserControl
    {
        // E-LAYER ANALYZER: YA-MS e' un pannello (UserControl) nella linguetta METEOR SCATTER (tools/sync_yams.py)
        Window Win { get { return TopLevel.GetTopLevel(this) as Window; } }
        IStorageProvider StorageProvider { get { TopLevel t = TopLevel.GetTopLevel(this); return t != null ? t.StorageProvider : null; } }
        public static Action<string> LanguageRequested;   // cambio di lingua: lo esegue la finestra di E-LAYER ANALYZER
        public static Action<int> ThemeRequested;         // cambio dello schema di colori: idem
        public static bool Hosted = true;                 // dentro E-LAYER ANALYZER: le righe telnet arrivano da DATA COLLECTION
        bool openedOnce;
        public void SelectTab(int i) { if (tabs != null) tabs.Select(i); }
        public void SaveSettings() { SaveConfig(); }
        public void Shutdown(bool save)
        {
            if (timer != null) timer.Stop();
            if (save && !replaced) SaveConfig();
            replaced = true;
        }

        string dataDir;
        Config cfg;
        PathModel model;
        List<RadarSource> sources;
        CollmData collm;
        RadarSource collmSrc, sodaSrc;
        static bool fetching;   // statico: dopo un cambio di lingua la nuova finestra non scarica insieme alla vecchia
        bool offlineNotified;
        DateTime lastFetchUtc = DateTime.MinValue;

        // comandi in alto
        TextBox txtHome, txtDx;
        NumericUpDown numHomeAlt, numDxAlt, numLayer, numElev;
        ComboBox cmbBand;
        CheckBox chkLayerRadar;
        NumericUpDown numBoom, numHotKm;
        CheckBox chkHotAuto, chkTrailLen;
        TextBlock lblAntAz, lblAnt;
        TextBlock lblLobeWarn, lblHighRad, lblAB;
        Border boxLobeWarn, boxHighRad;
        double antGainDbi, antHpbw, antHpbwV;   // lobo orizzontale (E) e verticale (H)
        string recommendText = "";
        string recommendShort = "";   // consiglio senza la parte tra parentesi (riquadro "Adesso")
        TextBlock glBest, glShower, glNow, glFore;
        Border cardBest, cardShower, cardNow, cardFore;   // riquadri del colpo d'occhio (per i fumetti)
        Border boxNoZone, boxNoZoneSky; TextBlock lblNoZone, lblNoZoneSky;   // avviso: nessuna zona comune della tratta
        CheckBox chkHot, chkMap, chkTopView;
        Button btnCalc, btnReal;
        TextBlock lblClock, lblSkyTime;
        Slider trackTime;
        CalendarDatePicker dtCalc;   // data del calcolo (UTC): base del momento del calcolo
        int lastTrackValue;
        Slider sldFont;              // "Carattere": 100-300 % di tutto il testo
        TextBlock lblFont;
        bool fontReady;              // la scala dei controlli standard si applica dopo l'apertura (stili gia' applicati)
        TextBlock lblMapCaption;     // didascalia sotto la pianta

        // linguette
        PillTabs tabs;
        SkyView sky, skySmall;   // sky = pianta (mappa); skySmall = volta celeste con i radianti
        SectionView section;     // sezione vista dall'antenna
        TableView lvSky;
        TextBlock lblPathInfo;
        SimpleChart chart24;
        const double Chart24Height = 300;
        TableView grid24;
        const double Grid24Scale = 1.5;   // caratteri della tabella oraria al 150 %
        SimpleChart chartFlux, chartHeight;
        Image picCollm, picSoda;
        TextBlock lblRadar, lblScaleNote;
        CheckBox chkAuto;
        NumericUpDown numExp;
        Button btnFetch;
        SelectableTextBlock txtGeo;
        SimpleChart chartElev;
        NumericUpDown numPower, numGain, numSens, numQ;
        TableView gridShowers;
        SelectableTextBlock txtSources;
        TextBlock status;
        DispatcherTimer timer;

        public MainWindow()
        {
            dataDir = AppPaths.DataDir;
            cfg = new Config(Path.Combine(dataDir, "config.ini"));
            Catalog.Load(dataDir);
            IauMdc.Load(dataDir); GmnDaily.Load(dataDir); GmnPopIndex.Load(Path.Combine(dataDir, "radar", "diretta", "gmn_flux.html"));
            sources = RadarFlux.LoadSources(dataDir);
            foreach (RadarSource s in sources)
            {
                if (s.Type == "SKIYMET_COLLM" && collmSrc == null) collmSrc = s;
                else if (s.Name.StartsWith("Sodankyla") && sodaSrc == null) sodaSrc = s;
            }

            Background = Pal.B(Pal.Back);
            Foreground = Pal.B(Pal.Fore);
            FontSize = 13;

            DockPanel root = new DockPanel();
            BuildTop(root);
            tabs = new PillTabs();
            // pulsante "Comandi": ripiega le tre righe dei comandi per lasciare spazio ai grafici (utile con caratteri grandi)
            btnRows = new Button { Content = Lang.T("Comandi") + " ▴", VerticalAlignment = VerticalAlignment.Center };
            btnRows.Classes.Add("pill");
            btnRows.Click += (s, e) => SetRowsVisible(!topRows[0].IsVisible, true);
            DockPanel.SetDock(btnRows, Dock.Right);
            tabs.Line.Children.Insert(0, btnRows);
            // scelta della lingua, in alto a destra (a destra del pulsante "Comandi")
            List<string> langNames = new List<string>();
            foreach (string[] lg in Lang.Languages) langNames.Add(lg[1]);
            cmbLang = new ComboBox { ItemsSource = langNames, MinWidth = 120, Margin = new Thickness(6, 1, 0, 1), VerticalAlignment = VerticalAlignment.Center };
            cmbLang.SelectedIndex = Math.Max(0, Lang.IndexOf(Lang.Code));
            ToolTip.SetTip(cmbLang, Lang.T("Lingua dell'interfaccia"));
            cmbLang.SelectionChanged += (s, e) =>
            {
                int li = cmbLang.SelectedIndex;
                if (li < 0 || li >= Lang.Languages.Length || Lang.Languages[li][0] == Lang.Code) return;
                string code = Lang.Languages[li][0];
                Dispatcher.UIThread.Post(() => SwitchLanguage(code));   // fuori dall'evento della casella
            };
            DockPanel.SetDock(cmbLang, Dock.Right);
            tabs.Line.Children.Insert(0, cmbLang);
            cmbLang.IsVisible = false;
            // scelta dello schema di colori, subito a sinistra della lingua
            List<string> themeNames = new List<string>();
            foreach (string tn in Tema.Names) themeNames.Add(Lang.T(tn));
            cmbTheme = new ComboBox { ItemsSource = themeNames, MinWidth = 150, Margin = new Thickness(6, 1, 0, 1), VerticalAlignment = VerticalAlignment.Center };
            cmbTheme.SelectedIndex = Tema.Schema;
            ToolTip.SetTip(cmbTheme, Lang.T("Tema"));
            cmbTheme.SelectionChanged += (s, e) =>
            {
                int ti = cmbTheme.SelectedIndex;
                if (ti < 0 || ti >= Tema.Count || ti == Tema.Schema) return;
                Dispatcher.UIThread.Post(() => SwitchTheme(ti));   // fuori dall'evento della casella
            };
            DockPanel.SetDock(cmbTheme, Dock.Right);
            tabs.Line.Children.Insert(1, cmbTheme);
            TextBlock lblTheme = new TextBlock { Text = Lang.T("Tema"), Foreground = Pal.B(Pal.BarMuted), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10, 0, 0, 0) };
            DockPanel.SetDock(lblTheme, Dock.Right);
            tabs.Line.Children.Insert(2, lblTheme);
            cmbTheme.IsVisible = false; lblTheme.IsVisible = false;
            DockTop(root, tabs.Bar);
            SetRowsVisible(cfg.GetD("comandi_visibili", 1) != 0, false);
            status = new TextBlock { Text = Lang.T("Pronto"), Foreground = Pal.B(Pal.BarMuted), TextTrimming = TextTrimming.CharacterEllipsis };
            DockPanel sbLine = new DockPanel();
            btnUpdate = new Button { Content = Lang.T("Aggiornamenti"), Padding = new Thickness(8, 0), Margin = new Thickness(8, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };
            btnUpdate.Classes.Add("pill");
            ToolTip.SetTip(btnUpdate, Lang.T("Controlla su GitHub se c'è una versione più nuova di YA-MS e, se c'è, la installa; controlla anche che non manchino file del programma."));
            btnUpdate.Click += (s, e) => CheckUpdatesManual();
            DockPanel.SetDock(btnUpdate, Dock.Right);
            btnUpdate.IsVisible = false;   // E-LAYER ANALYZER: aggiornamento dalla finestra principale
            sbLine.Children.Add(btnUpdate);
            sbLine.Children.Add(status);
            Border sb = new Border { Background = Pal.B(Pal.Track), Padding = new Thickness(8, 3), Child = sbLine };
            DockPanel.SetDock(sb, Dock.Bottom);
            root.Children.Add(sb);
            BuildTabs();
            root.Children.Add(tabs.Host);   // riempie lo spazio restante
            Content = root;

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += OnTick;
            timer.Start();

            AttachedToVisualTree += (s, e) =>
            {
                if (openedOnce) return;
                openedOnce = true;
                OnOpened(s, EventArgs.Empty);
            };
        }

        ComboBox cmbLang, cmbTheme;
        bool replaced;                 // finestra sostituita da una nuova (cambio di lingua o di schema di colori)
        static bool catalogShown;      // avviso del catalogo degli sciami mostrato una sola volta

        // Cambio di lingua: salva le impostazioni, carica la lingua e ricostruisce l'interfaccia in una nuova finestra
        // con la stessa posizione, dimensione, stato e linguetta; poi chiude questa.
        void SwitchLanguage(string code)
        {
            if (replaced || code == Lang.Code) return;
            if (LanguageRequested != null) LanguageRequested(code);
        }

        // Cambio dello schema di colori: stessa strada del cambio di lingua (tutti i colori sono letti
        // da Pal quando i controlli vengono creati, quindi si ricostruisce la finestra)
        void SwitchTheme(int schema)
        {
            if (replaced || schema == Tema.Schema) return;
            if (ThemeRequested != null) ThemeRequested(schema);
        }

        // nuova finestra con la stessa posizione, dimensione, stato e linguetta; poi chiude questa


        async void OnOpened(object sender, EventArgs e)
        {
            try
            {
                sky.ShowMap = chkMap.IsChecked == true; sky.TopView = chkTopView.IsChecked == true; skySmall.TopView = chkTopView.IsChecked == true;
                fontReady = true;
                ApplyFontScale();
                LoadCachedRadar();
                Recalc();
                StartMapLoad();
                RdAutoStart();
                if (chkAuto.IsChecked == true) StartFetch(); else lastFetchUtc = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(Catalog.LoadMessage) && !catalogShown)
                {
                    catalogShown = true;
                    await Dialogs.Message(Win, Lang.T("Catalogo sciami"), Catalog.LoadMessage);
                }
                await StartupUpdateChecks();
            }
            catch (Exception ex) { status.Text = Lang.F("Errore all'avvio: {0}", ex.Message); }
        }

        // ------------------------------------------------------------ barra delle linguette a pillola
        // Bordo scuro con i bottoni a pillola; le pagine stanno in una Grid e se ne vede una sola.
        sealed class PillTabs
        {
            public readonly Border Bar;
            public readonly Grid Host = new Grid();
            readonly WrapPanel strip = new WrapPanel { Orientation = Orientation.Horizontal };   // con caratteri grandi le linguette vanno a capo
            public readonly DockPanel Line = new DockPanel();
            readonly List<ToggleButton> buttons = new List<ToggleButton>();
            readonly List<Control> pages = new List<Control>();
            public readonly List<PillTabs> Subs = new List<PillTabs>();   // sottolinguette della pagina (null = pagina semplice)
            public int Selected = -1;
            public int Count { get { return buttons.Count; } }   // numero di linguette

            public PillTabs() : this(false) { }

            // sub = true: barra delle sottolinguette, piu' chiara e sottile, sotto quella principale
            public PillTabs(bool sub)
            {
                Line.Children.Add(strip);
                Bar = sub ? new Border { Background = Pal.B(Pal.Panel2), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(0, 0, 0, 1), Padding = new Thickness(8, 3), Child = Line }
                          : new Border { Background = Pal.B(Pal.Track), Padding = new Thickness(8, 5), Child = Line };
                IsSub = sub;
            }
            public readonly bool IsSub;

            public void Add(string title, Control page) { Add(title, page, null); }

            public void Add(string title, Control page, PillTabs sub)
            {
                int idx = buttons.Count;
                Subs.Add(sub);
                ToggleButton b = new ToggleButton { Content = title, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 1, 4, 1) };
                b.Classes.Add(IsSub ? "subtab" : "tab");
                b.Click += (s, e) => Select(idx);
                buttons.Add(b);
                strip.Children.Add(b);
                page.IsVisible = false;
                pages.Add(page);
                Host.Children.Add(page);
                if (idx == 0) Select(0);
            }

            public void Select(int i)
            {
                Selected = i;
                for (int k = 0; k < buttons.Count; k++)
                {
                    buttons[k].IsChecked = k == i;   // sempre esattamente una linguetta attiva
                    pages[k].IsVisible = k == i;
                }
            }
        }

        // ------------------------------------------------------------ interfaccia
        static void DockTop(DockPanel p, Control c)
        {
            DockPanel.SetDock(c, Dock.Top);
            p.Children.Add(c);
        }

        TextBlock L(string t)
        {
            TextBlock l = new TextBlock { Text = t };
            l.Classes.Add("label");
            return l;
        }

        static decimal Dm(double v)
        {
            if (double.IsNaN(v) || double.IsInfinity(v) || Math.Abs(v) > 1e9) return 0;
            return (decimal)v;
        }

        // valore di un NumericUpDown come double (il valore puo' essere null se la casella e' vuota)
        static double V(NumericUpDown n) { return (double)(n.Value ?? n.Minimum); }

        // testo di una casella in maiuscolo (locator)
        static string Up(TextBox t) { return (t.Text ?? "").Trim().ToUpperInvariant(); }

        NumericUpDown N(decimal min, decimal max, decimal val, int dec, decimal inc, double width)
        {
            NumericUpDown n = new NumericUpDown
            {
                Minimum = min, Maximum = max, Increment = inc,
                FormatString = "F" + dec.ToString(CultureInfo.InvariantCulture),
                NumberFormat = CultureInfo.CurrentCulture.NumberFormat,   // separatore decimale della lingua scelta (anche dopo un cambio di lingua)
                ClipValueToMinMax = true,
                Width = width + 80,   // spazio per i due pulsanti su/giu'
                Margin = new Thickness(2, 3),
                VerticalAlignment = VerticalAlignment.Center,
                Background = Pal.B(Pal.Field),
                Foreground = Pal.B(Pal.FieldFore),
                CornerRadius = new CornerRadius(12)
            };
            n.Value = Math.Max(min, Math.Min(max, val));
            return n;
        }

        TextBox T(string val, double width, bool upper)
        {
            TextBox t = new TextBox
            {
                Text = val, Width = width,
                FontFamily = Pal.Mono, FontSize = 14,
                Margin = new Thickness(2, 3),
                VerticalAlignment = VerticalAlignment.Center,
                Background = Pal.B(Pal.Field),
                Foreground = Pal.B(Pal.FieldFore),
                CornerRadius = new CornerRadius(12)
            };
            if (upper)
                t.LostFocus += (s, e) => { string u = (t.Text ?? "").ToUpperInvariant(); if (u != t.Text) t.Text = u; };
            return t;
        }

        Button B(string t)
        {
            Button b = new Button { Content = t, Margin = new Thickness(8, 2, 2, 2), VerticalAlignment = VerticalAlignment.Center };
            b.Classes.Add("pill");
            return b;
        }

        CheckBox C(string t, bool val)
        {
            return new CheckBox { Content = t, IsChecked = val, Margin = new Thickness(8, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center, Foreground = Pal.B(Pal.Fore) };
        }

        // riga di comandi (fondo Panel2): gli elementi vanno a capo se la finestra e' stretta
        // bordino in rilievo attorno a una casella da tenere d'occhio (diagramma d'antenna, angolo beta)
        static Border Raised(Control c)
        {
            return new Border
            {
                Child = c,
                Background = Pal.B(Pal.Card),
                BorderBrush = Pal.B(Pal.Border),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(6, 1, 8, 1),
                Margin = new Thickness(4, 2),
                VerticalAlignment = VerticalAlignment.Center,
                BoxShadow = new BoxShadows(new BoxShadow { OffsetX = 1, OffsetY = 2, Blur = 4, Color = Pal.Argb(110, 0, 0, 0) })
            };
        }

        static Border Row(params Control[] items)
        {
            WrapPanel wp = new WrapPanel { Orientation = Orientation.Horizontal };
            foreach (Control c in items) { c.VerticalAlignment = VerticalAlignment.Center; wp.Children.Add(c); }
            return new Border { Background = Pal.B(Pal.Panel2), Padding = new Thickness(6, 2), Child = wp };
        }

        // esegue a quando cambia la proprieta' p (piu' sicuro degli eventi specifici dei singoli controlli)
        static void WhenChanged(AvaloniaObject o, AvaloniaProperty p, Action a)
        {
            o.PropertyChanged += (s, e) => { if (e.Property == p) a(); };
        }

        static Border Warn(Color back, Color fore, out TextBlock text)
        {
            text = new TextBlock { Foreground = Pal.B(fore), FontWeight = FontWeight.Bold, TextWrapping = TextWrapping.Wrap };
            return new Border { Background = Pal.B(back), Padding = new Thickness(6, 4), Child = text, IsVisible = false };
        }

        Border Card(string title, Color accent, out TextBlock value)
        {
            TextBlock t = new TextBlock { Text = title, Foreground = Pal.B(accent), FontWeight = FontWeight.Bold, FontSize = 11.5 };
            value = new TextBlock { Text = "-", Foreground = Pal.B(Pal.Fore), FontSize = 13, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 4, 0, 0) };
            StackPanel sp = new StackPanel();
            sp.Children.Add(t); sp.Children.Add(value);
            Border bar = new Border { Background = Pal.B(accent), Width = 4, CornerRadius = new CornerRadius(2), Margin = new Thickness(0, 0, 10, 0) };
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*") };
            Grid.SetColumn(bar, 0); Grid.SetColumn(sp, 1);
            g.Children.Add(bar); g.Children.Add(sp);
            return new Border
            {
                Background = Pal.B(Pal.Card), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8), Padding = new Thickness(8, 6), Margin = new Thickness(4), Child = g
            };
        }

        static TableView MakeTable(string[] cols, double[] widths)
        {
            TableView t = new TableView();
            t.Columns = new List<string>(cols);
            t.ColumnWidths = new List<double>(widths);
            t.Rows = new List<TableRow>();
            return t;
        }

        static Image MakePicture()
        {
            return new Image { Stretch = Stretch.Uniform };
        }

        static Border Framed(Control c, Color back)
        {
            return new Border { Background = Pal.B(back), Child = c };
        }

        void BuildTop(DockPanel root)
        {
            txtHome = T(cfg.Get("mio_locator", "JN70CN"), 90, true);
            numHomeAlt = N(0, 5000, Dm(cfg.GetD("mia_quota_m", 62)), 0, 10, 60);
            txtDx = T(cfg.Get("dx_locator", "JO62QM"), 90, true);
            numDxAlt = N(0, 5000, Dm(cfg.GetD("dx_quota_m", 50)), 0, 10, 60);
            string[] bands = { "28", "50", "70", "144", "222", "432" };
            cmbBand = new ComboBox { ItemsSource = bands, Width = 90, Margin = new Thickness(2, 3), VerticalAlignment = VerticalAlignment.Center };
            cmbBand.SelectedIndex = Array.IndexOf(bands, cfg.Get("banda_MHz", "144"));
            if (cmbBand.SelectedIndex < 0) cmbBand.SelectedIndex = 3;
            numLayer = N(70, 130, Dm(cfg.GetD("quota_scie_km", 90)), 1, 0.5m, 60);
            chkLayerRadar = C(Lang.T("dal radar"), cfg.GetB("quota_da_radar", true));
            numElev = N(0, 30, Dm(cfg.GetD("elevazione_lobo_gradi", 0)), 1, 0.5m, 55);
            ToolTip.SetTip(numElev, Lang.T("Elevazione del massimo del lobo verticale dell'antenna. Una direttiva fissata a 0° ha comunque il lobo principale qualche grado sopra l'orizzonte, per la riflessione sul terreno (di solito 3-8°, secondo l'altezza dell'antenna in lunghezze d'onda).\n" +
                "Con \"diagramma d'antenna\" acceso questo valore è il massimo del lobo: i punti sotto pesano sempre meno fino a zero all'orizzonte, quelli sopra seguono il lobo verticale calcolato dal boom. Con la casella spenta è un limite netto: i punti sotto non contano."));
            // casella: lobo come diagramma d'antenna (attenuazione) invece del limite netto
            chkLobe = C(Lang.T("diagramma d'antenna"), cfg.GetB("lobo_diagramma", true));
            PathModel.LobePattern = chkLobe.IsChecked == true;
            ToolTip.SetTip(chkLobe, Lang.T("Come conta l'elevazione del lobo.\n" +
                "Accesa (di partenza): i punti di riflessione pesano con il diagramma verticale dell'antenna di ciascuna stazione. Il lobo ha il massimo all'elevazione scritta accanto e la larghezza verticale calcolata dal boom (curva DL6WU, Kraus); " +
                "sotto il massimo il terreno fa scendere il guadagno fino a zero all'orizzonte. Un punto basso conta meno, ma conta: su una tratta lunga l'efficienza non sparisce. È accesa di partenza perché descrive l'antenna reale: JN70CN – JO76QP (1791 km), lobo a 3°, scie a 88,5 km, Geminidi del 14/12/2026: 7 % con la casella accesa, nessun valore con la casella spenta.\n" +
                "Spenta: l'elevazione del lobo è un limite netto (come fino alla versione 1.8.0). I punti visti sotto quell'elevazione da una delle due stazioni non contano; su una tratta lunga con il lobo a qualche grado tutte le efficienze possono valere 0."));
            WhenChanged(chkLobe, ToggleButton.IsCheckedProperty, () =>
            {
                PathModel.LobePattern = chkLobe.IsChecked == true;
                cfg.Set("lobo_diagramma", PathModel.LobePattern);
                if (model != null) Recalc();
            });
            btnCalc = B(Lang.T("Calcola")); btnCalc.Classes.Add("accent");
            btnCalc.Click += (s, e) => Recalc();
            cmbBand.SelectionChanged += (s, e) => { if (model != null) { if (PathModel.LobePattern) Recalc(); else { ApplyAntenna(); UpdateSky(); UpdateFluxTab(); UpdateGeometry(); } } };

            // l'antenna e' sempre puntata verso il corrispondente (azimut della tratta)
            Color wheat = Pal.Ink(Pal.Rgb(245, 222, 179));   // color grano: scurito negli schemi chiari
            lblAntAz = L(Lang.T("Antenna az: -")); lblAntAz.Foreground = Pal.B(wheat);
            numBoom = N(0.5m, 30, Dm(cfg.GetD("boom_m", 4.6)), 2, 0.1m, 60);
            lblAnt = L(""); lblAnt.Foreground = Pal.B(wheat);
            numBoom.ValueChanged += (s, e) => { if (model != null) { if (PathModel.LobePattern) Recalc(); else { ApplyAntenna(); UpdateSky(); UpdateGeometry(); } } };
            numHotKm = N(30, 400, Dm(cfg.GetD("hot_spot_km", 150)), 0, 10, 55);
            PathModel.HotSpotOffsetKm = V(numHotKm);
            numHotKm.ValueChanged += (s, e) => { if (!PathModel.HotSpotAuto) { PathModel.HotSpotOffsetKm = V(numHotKm); if (model != null) Recalc(); } };
            // distanza degli hot spot: automatica dalla tabella AMS (J. Richardson, riportata da DJ5HG) o scritta a mano
            chkHotAuto = C(Lang.T("auto (AMS)"), cfg.GetB("hot_spot_auto", true));
            PathModel.HotSpotAuto = chkHotAuto.IsChecked == true;
            numHotKm.IsEnabled = !PathModel.HotSpotAuto;
            ToolTip.SetTip(chkHotAuto, Lang.T("Scarto d'antenna degli hot spot dalla tabella AMS di J. Richardson riportata da DJ5HG: 13° a 1000 km, 11° a 1250 km, 10° a 1500 e 2000 km. Togli la spunta per scrivere la distanza a mano (MSHV usa 150 km)."));
            WhenChanged(chkHotAuto, ToggleButton.IsCheckedProperty, () =>
            {
                PathModel.HotSpotAuto = chkHotAuto.IsChecked == true;
                numHotKm.IsEnabled = !PathModel.HotSpotAuto;
                if (!PathModel.HotSpotAuto) PathModel.HotSpotOffsetKm = V(numHotKm);
                if (model != null) Recalc();
            });

            // riga 2: strato E, lobo e antenna
            Border row2 = Row(L(Lang.T("Quota scie km")), numLayer, chkLayerRadar,
                              L(Lang.T("Elevazione lobo °")), numElev, chkLobe,
                              L(Lang.T("Antenna Yagi: boom m")), numBoom, lblAnt,
                              L(Lang.T("Hot spot dal punto medio km")), numHotKm, chkHotAuto);

            // riga 3: orologio, ora della volta celeste, giorno della tabella 24 h
            lblClock = L("--:--:-- UTC"); lblClock.FontFamily = Pal.Mono; lblClock.FontSize = 15; lblClock.FontWeight = FontWeight.Bold; lblClock.Foreground = Pal.B(Pal.Yellow);
            trackTime = new Slider { Minimum = -48, Maximum = 144, Value = 0, SmallChange = 1, LargeChange = 4, TickFrequency = 12, Width = 240, Margin = new Thickness(4, 0) };
            WhenChanged(trackTime, RangeBase.ValueProperty, () =>
            {
                int v = TrackValue;
                if (v == lastTrackValue) return;
                lastTrackValue = v; UpdateSky();
            });
            lblSkyTime = L(Lang.T("Calcolo per: adesso")); lblSkyTime.Foreground = Pal.B(Pal.Azure);
            btnReal = B(Lang.T("Tempo reale"));
            btnReal.Background = Brushes.Transparent; btnReal.BorderBrush = Pal.B(Pal.Violet); btnReal.Foreground = Pal.B(Pal.Violet);
            btnReal.Click += (s, e) => ResetCalcTime();
            // data del calcolo: con l'ora attuale e lo spostamento del cursore (da -12 a +36 h) da' il momento del calcolo
            dtCalc = new CalendarDatePicker { SelectedDate = DateTime.UtcNow.Date, Width = 150, Margin = new Thickness(2, 3), FlowDirection = FlowDirection.LeftToRight };   // date sempre da sinistra a destra (arabo, ebraico, urdu)
            // una sola data: vale per il calcolo del momento e per la tabella delle 24 ore (Calcolo dell'efficienza)
            WhenChanged(dtCalc, CalendarDatePicker.SelectedDateProperty, () => { UpdateSky(); Update24h(); });
            // dimensione del carattere di tutto il programma (solo il testo), a passi del 10 %
            double pct0 = Math.Max(100, Math.Min(300, Math.Round(cfg.GetD("carattere_pct", 100) / 10) * 10));
            lblFont = L(pct0.ToString("F0", CultureInfo.InvariantCulture) + " %"); lblFont.Foreground = Pal.B(Pal.Azure);
            sldFont = new Slider { Minimum = 100, Maximum = 300, Value = pct0, SmallChange = 10, LargeChange = 10, TickFrequency = 10,
                                   IsSnapToTickEnabled = true, Width = 160, Margin = new Thickness(4, 0) };
            UiScale.Set(pct0 / 100.0);
            WhenChanged(sldFont, RangeBase.ValueProperty, () => SetFontPercent(sldFont.Value));
            Border row3 = Row(L(Lang.T("Ora")), lblClock, L(Lang.T("Data del calcolo (UTC)")), dtCalc, L(Lang.T("Ore (da −12 a +36 h)")), trackTime,
                              btnReal, lblSkyTime);
            // riga 1: stazioni e banda, azimut d'antenna e cursore del carattere (la prima riga ha spazio)
            Border row1 = Row(L(Lang.T("Il mio locator")), txtHome, L(Lang.T("quota m")), numHomeAlt,
                       L(Lang.T("Corrispondente")), txtDx, L(Lang.T("quota m")), numDxAlt,
                       L(Lang.T("Banda MHz")), cmbBand, btnCalc, lblAntAz, L(Lang.T("Carattere")), sldFont, lblFont);

            DockTop(root, row1);
            DockTop(root, row2);
            DockTop(root, row3);
            topRows = new[] { row1, row2, row3 };
        }

        Border[] topRows;
        Button btnRows;

        void SetRowsVisible(bool v, bool save)
        {
            if (topRows == null) return;
            foreach (Border b in topRows) b.IsVisible = v;
            if (btnRows != null) btnRows.Content = Lang.T("Comandi") + (v ? " ▴" : " ▾");
            if (save) { cfg.Set("comandi_visibili", v); try { cfg.Save(); } catch { } }
        }

        // cursore "Carattere": nuova scala, poi controlli standard e testi costruiti con dimensioni proprie
        void SetFontPercent(double pct)
        {
            pct = Math.Max(100, Math.Min(300, Math.Round(pct / 10) * 10));
            lblFont.Text = pct.ToString("F0", CultureInfo.InvariantCulture) + " %";
            UiScale.Set(pct / 100.0);   // i controlli disegnati si ridisegnano da soli (evento UiScale.Changed)
            ApplyFontScale();
        }

        // Applica la scala del carattere ai controlli standard di tutta la finestra (TextBlock, caselle, bottoni, ...).
        // Da richiamare dopo aver creato o sostituito controlli; i valori di base restano memorizzati.
        void ApplyFontScale()
        {
            if (!fontReady) return;
            UiScale.Apply(this);
            if (chart24 != null) chart24.Height = Chart24Height * (0.5 + 0.5 * UiScale.Font);   // spazio per scritte piu' grandi
            if (explainSheet != null) explainSheet.MaxWidth = 860 * Math.Min(2.0, UiScale.Font);
            if (manualSheet != null) manualSheet.MaxWidth = 860 * Math.Min(2.0, UiScale.Font);
            GravesFontScale();
            UpdateGeometry();   // il resoconto usa caratteri ricavati da quello della casella
        }

        // linguetta principale con sottolinguette: le Page() seguenti vanno nel gruppo finche' non si chiama EndGroup()
        PillTabs curGroup;

        DockPanel Page(string t)
        {
            DockPanel p = new DockPanel { Background = Pal.B(Pal.Back) };
            (curGroup ?? tabs).Add(t, p);
            return p;
        }

        void Group(string t)
        {
            PillTabs g = new PillTabs(true);
            DockPanel p = new DockPanel { Background = Pal.B(Pal.Back) };
            DockTop(p, g.Bar);
            p.Children.Add(g.Host);
            tabs.Add(t, p, g);
            curGroup = g;
        }

        void EndGroup() { curGroup = null; }

        void BuildTabs()
        {
            // ===== 1. Live e dati real-time: traffico in diretta, flusso radar, radar GRAVES, raccolta dati
            Group(Lang.T("Live e dati real-time"));

            // --- Traffico in diretta: spot MS della raccolta dati sulla mappa azimutale (MainWindow.Traffico.cs)
            BuildTrafficTab();

            // --- Flusso radar
            DockPanel p3 = Page(Lang.T("Flusso radar"));
            btnFetch = B(Lang.T("Scarica adesso")); btnFetch.Click += (s, e) => StartFetch();
            chkAuto = C(Lang.T("Aggiornamento automatico di tutte le fonti ogni 5 minuti"), cfg.GetB("aggiornamento_automatico", true));
            numExp = N(1, 4, Dm(cfg.GetD("esponente_lambda", 3)), 1, 0.1m, 50);
            numExp.ValueChanged += (s, e) => UpdateFluxTab();
            lblRadar = L("");
            DockTop(p3, Row(btnFetch, chkAuto, L(Lang.T("Esponente λ (SM6FHZ)")), numExp, lblRadar));
            lblScaleNote = new TextBlock { Foreground = Pal.B(Pal.OrangeText), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4, 6, 4) };
            DockTop(p3, lblScaleNote);
            Grid tl = new Grid { ColumnDefinitions = new ColumnDefinitions("62*,38*"), RowDefinitions = new RowDefinitions("50*,50*") };
            chartFlux = new SimpleChart();
            chartHeight = new SimpleChart();
            picCollm = MakePicture(); picSoda = MakePicture();
            Border fCollm = Framed(picCollm, Pal.ChartBg), fSoda = Framed(picSoda, Pal.ChartBg);
            Grid.SetColumn(chartFlux, 0); Grid.SetRow(chartFlux, 0);
            Grid.SetColumn(fCollm, 1); Grid.SetRow(fCollm, 0);
            Grid.SetColumn(chartHeight, 0); Grid.SetRow(chartHeight, 1);
            Grid.SetColumn(fSoda, 1); Grid.SetRow(fSoda, 1);
            tl.Children.Add(chartFlux); tl.Children.Add(fCollm); tl.Children.Add(chartHeight); tl.Children.Add(fSoda);
            p3.Children.Add(tl);

            // --- Radar GRAVES (MainWindow.Graves.cs)
            BuildGravesTab();

            // --- Raccolta dati: spot MS da cluster, RBN e PSKReporter (MainWindow.Raccolta.cs)
            BuildCollectTab();

            EndGroup();

            // ===== 2. Calcolo previsionale: volta celeste e calcolo, geometria e attenuazione, calcolo dell'efficienza,
            //          attuazione sperimentale previsioni
            Group(Lang.T("Calcolo previsionale"));

            // --- Calcolo previsionale (ex "Volta celeste")
            DockPanel p1 = Page(Lang.T("Calcolo previsionale"));   // ex linguetta "Volta celeste"
            Grid sc = new Grid { ColumnDefinitions = new ColumnDefinitions("60*,40*") };
            p1.Children.Add(sc);
            // pianta: mappa con tratta, Hot, lobo, zone PREV, linea di scorrimento dello sciame e scala dei gradi
            sky = new SkyView { ShowRadiants = false, ShowSkyRings = false, ShowDial = true, DrawCaption = false };
            EventHandler<PointerReleasedEventArgs> onSky = (s, e) => OnSkyClick(e);
            sky.AddHandler(InputElement.PointerReleasedEvent, onSky, RoutingStrategies.Bubble, true);
            // didascalia sotto la pianta (non piu' disegnata dentro la mappa), va a capo
            lblMapCaption = new TextBlock { Foreground = Pal.B(Pal.Muted), FontSize = 11.3, TextWrapping = TextWrapping.Wrap };
            Border capBox = new Border { Background = Pal.B(Pal.Panel), Padding = new Thickness(8, 5), Child = lblMapCaption };
            DockPanel skyCol = new DockPanel();
            // fascia sopra la pianta: i dati di efficienza del momento di calcolo scelto con il cursore
            lblEffStrip = new TextBlock { Foreground = Pal.B(Pal.Fore), FontSize = 13, FontWeight = FontWeight.SemiBold, TextWrapping = TextWrapping.Wrap };
            Border effStrip = new Border
            {
                Background = Pal.B(Pal.Panel3), BorderBrush = Pal.B(Pal.Azure), BorderThickness = new Thickness(0, 0, 0, 2),
                Padding = new Thickness(10, 6), Child = lblEffStrip
            };
            DockPanel.SetDock(effStrip, Dock.Top);
            skyCol.Children.Add(effStrip);
            DockPanel.SetDock(capBox, Dock.Bottom);
            skyCol.Children.Add(capBox);
            skyCol.Children.Add(sky);   // riempie lo spazio restante
            Grid.SetColumn(skyCol, 0);
            sc.Children.Add(skyCol);
            // volta celeste piccola: radianti e percorsi (clic su un radiante = selezione)
            skySmall = new SkyView { ShowMap = false, ShowLegend = false };
            skySmall.Marks = sky.Marks;
            EventHandler<PointerReleasedEventArgs> onSmall = (s, e) =>
            {
                if (e.InitialPressMouseButton != MouseButton.Left) return;
                string c = skySmall.HitTest(e.GetPosition(skySmall));
                if (c != null) { SelectShower(c); SelectInList(c); }
            };
            skySmall.AddHandler(InputElement.PointerReleasedEvent, onSmall, RoutingStrategies.Bubble, true);
            section = new SectionView();
            section.Note = Lang.T("segmenti = scie (±10 km attorno alla quota) orientate verso il radiante; più spessi = più probabili");
            lvSky = MakeTable(new[] { Lang.T("Sciame"), Lang.T("Sigla"), Lang.T("ZHR ora"), Lang.T("Azimut"), Lang.T("Elev."), Lang.T("Effic. %"), Lang.T("Lato"), Lang.T("Punt. migliore"), Lang.T("Sorge"), Lang.T("Tramonta") },
                              new double[] { 118, 42, 54, 52, 46, 54, 62, 98, 48, 56 });
            lvSky.RowClicked += i => { if (i >= 0 && i < lvSky.Rows.Count) SelectShower(lvSky.Rows[i].Cells[1]); };
            chkHot = C(Lang.T("Direzioni Hot A / Hot B"), true);
            WhenChanged(chkHot, ToggleButton.IsCheckedProperty, () =>
            {
                sky.ShowHot = chkHot.IsChecked == true; skySmall.ShowHot = chkHot.IsChecked == true;
                sky.InvalidateVisual(); skySmall.InvalidateVisual();
            });
            chkMap = C(Lang.T("Mappa d'Europa"), cfg.GetB("mostra_mappa", true));
            WhenChanged(chkMap, ToggleButton.IsCheckedProperty, () =>
            {
                sky.ShowMap = chkMap.IsChecked == true;
                if (chkMap.IsChecked == true) chkTopView.IsChecked = true;
                sky.InvalidateVisual();
            });
            chkTopView = C(Lang.T("Vista dall'alto (E a destra)"), cfg.GetB("vista_alto", true));
            WhenChanged(chkTopView, ToggleButton.IsCheckedProperty, () =>
            {
                sky.TopView = chkTopView.IsChecked == true; skySmall.TopView = chkTopView.IsChecked == true;
                if (chkTopView.IsChecked != true) chkMap.IsChecked = false;
                sky.InvalidateHeat(); skySmall.InvalidateVisual();
            });
            Border opt = Row(chkHot, chkMap, chkTopView);
            lblPathInfo = new TextBlock { FontFamily = Pal.Mono, FontSize = 11, Foreground = Pal.B(Pal.Fore), Padding = new Thickness(6), TextWrapping = TextWrapping.Wrap, ClipToBounds = true };
            boxLobeWarn = Warn(Pal.OrangeDark, Pal.OrangeText, out lblLobeWarn);
            // avviso radiante troppo alto (sopra 70°, DF5HC / DH9GCD)
            boxHighRad = Warn(Pal.Panel3, Pal.Yellow, out lblHighRad);
            // avviso radiante basso lungo la tratta (DJ5HG 2004)
            boxLowAlong = Warn(Pal.Panel3, Pal.Azure, out lblLowAlong);
            // avviso tratta oltre l'orizzonte della scia (punto medio sotto l'orizzonte alla quota delle scie)
            boxHorizon = Warn(Pal.OrangeDark, Pal.OrangeText, out lblHorizon);
            boxNoZoneSky = Warn(Pal.OrangeDark, Pal.OrangeText, out lblNoZoneSky);
            // lato destro: in alto elenco degli sciami, al centro volta celeste + testo del calcolo, in basso la sezione
            Grid rt = new Grid { RowDefinitions = new RowDefinitions("31*,37*,32*") };
            DockPanel rTop = new DockPanel();
            DockTop(rTop, opt); DockTop(rTop, boxLobeWarn); DockTop(rTop, boxNoZoneSky); DockTop(rTop, boxHorizon); DockTop(rTop, boxHighRad); DockTop(rTop, boxLowAlong);
            rTop.Children.Add(lvSky);
            Grid rMid = new Grid { ColumnDefinitions = new ColumnDefinitions("42*,58*") };
            Grid.SetColumn(skySmall, 0); Grid.SetColumn(lblPathInfo, 1);
            rMid.Children.Add(skySmall); rMid.Children.Add(lblPathInfo);
            section.Margin = new Thickness(0, 4, 0, 0);
            Grid.SetRow(rTop, 0); Grid.SetRow(rMid, 1); Grid.SetRow(section, 2);
            rt.Children.Add(rTop); rt.Children.Add(rMid); rt.Children.Add(section);
            Grid.SetColumn(rt, 1);
            sc.Children.Add(rt);

            // --- Geometria e attenuazione
            DockPanel p4 = Page(Lang.T("Geometria e attenuazione"));
            numPower = N(1, 2000, Dm(cfg.GetD("potenza_W", 400)), 0, 10, 65);
            numGain = N(0, 40, Dm(cfg.GetD("guadagno_dBi", 14)), 1, 0.5m, 55);
            numSens = N(-160, -90, Dm(cfg.GetD("sensibilita_dBm", -130)), 0, 1, 60);
            numQ = N(12, 16, Dm(cfg.GetD("densita_esponente_q", 14)), 1, 0.5m, 50);
            DockTop(p4, Row(L(Lang.T("Potenza W")), numPower, L(Lang.T("Guadagno antenna dBi (tx = rx)")), numGain,
                            L(Lang.T("Segnale minimo dBm")), numSens, L(Lang.T("Densita' scia q = 10^")), numQ, L(Lang.T("elettroni/m"))));
            numPower.ValueChanged += (s, e) => UpdateGeometry();
            numGain.ValueChanged += (s, e) => UpdateGeometry();
            numSens.ValueChanged += (s, e) => UpdateGeometry();
            numQ.ValueChanged += (s, e) => UpdateGeometry();
            // scelte con fonti: raggio iniziale della scia (Stober et al. 2021 / ITU-R P.843) e durata dei ping (lambda^2 / misure HamSCI MSQP)
            cmbR0 = new ComboBox
            {
                ItemsSource = new List<string> { Lang.T("doppio dell'ITU (Stober 2021, prudente)"), Lang.T("ITU-R P.843"), Lang.T("escluso") },
                Margin = new Thickness(2, 3), VerticalAlignment = VerticalAlignment.Center
            };
            cmbR0.SelectedIndex = Math.Max(0, Math.Min(2, (int)Math.Round(cfg.GetD("raggio_iniziale", 0))));
            MsGeometry.InitialRadiusFactor = new double[] { 2, 1, 0 }[cmbR0.SelectedIndex];
            ToolTip.SetTip(cmbR0, Lang.T("Raggio iniziale della scia r0: più è grande, più si perde alle frequenze alte.\n" +
                "Doppio dell'ITU (prudente, di partenza): il radar CMOR a 17, 29 e 38 MHz trova r0 di 0,5-2 m tra 85 e 95 km, per lo più 1-1,5 m, con dispersione di un fattore due; circa il doppio delle misure precedenti (Stober et al. 2021).\n" +
                "ITU-R P.843: log10 r0 = 0,035 h − 3,45 (0,5 m a 90 km), valore medio.\n" +
                "Escluso: senza il fattore del raggio iniziale."));
            cmbR0.SelectionChanged += (s, e) =>
            {
                MsGeometry.InitialRadiusFactor = new double[] { 2, 1, 0 }[Math.Max(0, cmbR0.SelectedIndex)];
                cfg.Set("raggio_iniziale", (double)Math.Max(0, cmbR0.SelectedIndex));
                UpdateGeometry();
            };
            cmbDur = new ComboBox
            {
                ItemsSource = new List<string> { Lang.T("teorica λ² (McKinley, SM6FHZ; prudente)"), Lang.T("osservata (HamSCI MSQP, NN4SA 2026)") },
                Margin = new Thickness(2, 3), VerticalAlignment = VerticalAlignment.Center
            };
            cmbDur.SelectedIndex = cfg.GetD("durata_ping", 0) >= 0.5 ? 1 : 0;
            MsGeometry.DurationObserved = cmbDur.SelectedIndex == 1;
            ToolTip.SetTip(cmbDur, Lang.T("Come cambia la durata dei ping da una banda all'altra.\n" +
                "Teorica (prudente, di partenza): durata proporzionale a λ² (McKinley 1961; regola di SM6FHZ): da 50 a 144 MHz i ping si accorciano di 8 volte.\n" +
                "Osservata: mediane misurate da NN4SA (EM64) durante le Perseidi 2026 nel MSQP di HamSCI (R. Suggs NN4NT): 148 ms a 28 MHz, 78 ms a 50 MHz, 53 ms a 144 MHz; da 50 a 144 MHz i ping si accorciano solo di 1,5 volte. " +
                "Una sola stazione ricevente, analisi preliminare; sopra 144 MHz non ci sono misure e si usa λ²."));
            cmbDur.SelectionChanged += (s, e) =>
            {
                MsGeometry.DurationObserved = cmbDur.SelectedIndex == 1;
                cfg.Set("durata_ping", (double)Math.Max(0, cmbDur.SelectedIndex));
                UpdateGeometry();
                UpdatePrevTab();
            };
            DockTop(p4, Row(L(Lang.T("Raggio iniziale della scia")), cmbR0, L(Lang.T("Durata dei ping tra le bande")), cmbDur));
            Grid sg = new Grid { ColumnDefinitions = new ColumnDefinitions("55*,45*") };
            txtGeo = new SelectableTextBlock { FontFamily = Pal.Mono, FontSize = 12.5, Foreground = Pal.B(Pal.CodeText), TextWrapping = TextWrapping.Wrap };
            ScrollViewer geoScroll = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = new Border { Padding = new Thickness(12), Child = txtGeo }
            };
            Border geoBox = Framed(geoScroll, Pal.CodeBack);
            chartElev = new SimpleChart();
            Grid.SetColumn(geoBox, 0); Grid.SetColumn(chartElev, 1);
            sg.Children.Add(geoBox); sg.Children.Add(chartElev);
            p4.Children.Add(sg);

            // --- Calcolo dell'efficienza (ex "Efficienza 24 h")
            DockPanel p2 = Page(Lang.T("Calcolo dell'efficienza"));
            // colpo d'occhio: quattro riquadri riassuntivi sopra il grafico
            // altezza minima 100: con caratteri grandi i riquadri crescono
            Grid glance = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*,*,*"), MinHeight = 100, Margin = new Thickness(6, 6, 6, 2) };
            Border c0 = cardBest = Card(Lang.T("ORA MIGLIORE DEL GIORNO"), Pal.Azure, out glBest);
            Border c1 = cardShower = Card(Lang.T("SCIAME PIÙ FAVOREVOLE"), Pal.Yellow, out glShower);
            Border c2 = cardNow = Card(Lang.T("ADESSO"), Pal.Green, out glNow);
            Border c3 = cardFore = Card(Lang.T("PREVISIONE SPERIMENTALE"), Pal.Orange, out glFore);
            Grid.SetColumn(c0, 0); Grid.SetColumn(c1, 1); Grid.SetColumn(c2, 2); Grid.SetColumn(c3, 3);
            glance.Children.Add(c0); glance.Children.Add(c1); glance.Children.Add(c2); glance.Children.Add(c3);
            DockTop(p2, glance);
            boxNoZone = Warn(Pal.OrangeDark, Pal.OrangeText, out lblNoZone);
            boxNoZone.Margin = new Thickness(6, 0, 6, 4);
            DockTop(p2, boxNoZone);
            lblAB = new TextBlock
            {
                Foreground = Pal.B(Pal.Fore), FontSize = 13, TextWrapping = TextWrapping.Wrap,
                Text = Lang.T("Le lettere accanto alla percentuale dicono da quale lato della tratta arriva la maggior parte delle riflessioni. " +
                       "A = a SINISTRA guardando dalla tua stazione verso il corrispondente (zona Hot A); B = a DESTRA (zona Hot B). " +
                       "Conviene spostare l'antenna di 10-30° verso quel lato (side-scatter). Nessuna lettera = efficienza sotto l'1 %. " +
                       "Riga Σ: indice della tratta = somma di ZHR x efficienza di tutti gli sciami attivi. " +
                       "Righe azzurre: confronto con la formula di DH9GCD (DF5HC 2018). ↑ = radiante sopra 70° dalla tua stazione (troppo alto).")
            };
            DockTop(p2, new Border { Background = Pal.B(Pal.Panel3), Padding = new Thickness(8, 5, 8, 4), Child = lblAB });
            // opzione: lunghezza delle scie con il radiante basso (DJ5HG 2004), spenta di partenza (calcolo prudente)
            chkTrailLen = C(Lang.T("Scie più lunghe con il radiante basso (DJ5HG)"), cfg.GetB("lunghezza_scie", false));
            PathModel.TrailLength = chkTrailLen.IsChecked == true;
            ToolTip.SetTip(chkTrailLen, Lang.T("Spenta (prudente): ogni scia pesa con il seno dell'elevazione del radiante, perché con il radiante basso cadono meno meteore per km².\n" +
                "Accesa: come DJ5HG (2004), con il radiante basso le scie sono più lunghe (si conta lo spessore utile dello strato, 15 km, diviso il seno dell'elevazione, fino a 100 km) " +
                "e soddisfano più spesso la legge di riflessione. Le due cose si compensano sopra circa 9° di elevazione: l'efficienza con il radiante basso sale, quella con il radiante alto non cambia."));
            WhenChanged(chkTrailLen, ToggleButton.IsCheckedProperty, () =>
            {
                PathModel.TrailLength = chkTrailLen.IsChecked == true;
                SyncProfile();
                if (!applyingProfile && model != null) Recalc();
            });
            // opzione: angolo beta tra la scia e il piano della tratta (McKinley completo, ITU-R P.843)
            chkBeta = C(Lang.T("Angolo β tra scia e tratta (McKinley completo)"), cfg.GetB("angolo_beta", true));
            PathModel.BetaFactor = chkBeta.IsChecked == true;
            ToolTip.SetTip(chkBeta, Lang.T("Due tesi sulla geometria, con fonti diverse.\n" +
                "Accesa (prudente, di partenza): formula completa di McKinley (1961) e dell'ITU-R P.843, 1 / (1 − cos²β sin²φ), dove β è l'angolo tra la scia e il piano della tratta. " +
                "Le scie che attraversano il piano valgono 10-15 dB meno di quelle che vi giacciono e nessuna scia è sopravvalutata: prevalgono i radianti allineati alla tratta. " +
                "Con questa scelta YA-MS segue meglio la forma delle curve orarie di DJ5HG (2004).\n" +
                "Spenta: ogni scia pesa come se stesse nel piano della tratta (β = 0), come fino alla versione 1.6. Vale la regola classica dei radioamatori: il radiante migliore è alto 15-70° e di traverso alla tratta, " +
                "quello allineato è marginale (SM6FHZ, DH9GCD, meteor-mcp di qso-graph).\n" +
                "Un primo confronto con i conteggi BRAMS di Sonneberg (Perseidi 2026, 471 km) non le distingue: la casella \"Giudizio classico\" mostra il verdetto della regola classica sotto l'efficienza, ora per ora.\n" +
                "Quanto pesa: è la scelta che cambia di più i risultati. Su JN70CN – JO76QP (1791 km), Geminidi del 14/12/2026, l'efficienza massima passa dal 98 % (spenta) al 13 % (accesa), un fattore 7. Gli altri affinamenti sono molto più piccoli: raggio iniziale e durata dei ping valgono pochi dB e solo sopra 144 MHz, il diagramma d'antenna conta sulle tratte vicine al limite dell'orizzonte, la quota delle scie varia di ±7 km da un'ora all'altra. Le ore migliori e il lato restano invece quasi gli stessi con le due scelte."));
            WhenChanged(chkBeta, ToggleButton.IsCheckedProperty, () =>
            {
                PathModel.BetaFactor = chkBeta.IsChecked == true;
                SyncProfile();
                if (!applyingProfile && model != null) Recalc();
            });
            // casella: giudizio classico (radiante di traverso alla tratta) mostrato sotto l'efficienza; non cambia il calcolo
            chkClassic = C(Lang.T("Giudizio classico"), cfg.GetB("giudizio_classico", false));
            ToolTip.SetTip(chkClassic, Lang.T("Aggiunge sotto ogni sciame la riga \"classico\": il verdetto della regola dei radioamatori, calcolato al punto medio della tratta.\n" +
                "✗ radiante sotto l'orizzonte; ◐ marginale (radiante sotto 15° o sopra 70°, oppure a meno di 30° dalla direzione della tratta); ● buono; ★ migliore (radiante tra 15° e 70° e a più di 60° dalla direzione della tratta).\n" +
                "Soglie come in meteor-mcp (qso-graph, 2026); stessa idea della formula di DH9GCD e delle regole di SM6FHZ. Non cambia il calcolo: serve a confrontare le due tesi sull'angolo β."));
            WhenChanged(chkClassic, ToggleButton.IsCheckedProperty, () => { cfg.Set("giudizio_classico", chkClassic.IsChecked == true); if (model != null) Recalc(); });
            // impostazioni pronte del calcolo: prudente (quota dal radar, scie normali) o come DJ5HG (102 km, scie lunghe)
            rbProfPrud = new RadioButton { Content = Lang.T("Prudente"), GroupName = "profilo_calcolo", Margin = new Thickness(8, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center, Foreground = Pal.B(Pal.Fore) };
            rbProfDj = new RadioButton { Content = Lang.T("Come DJ5HG"), GroupName = "profilo_calcolo", Margin = new Thickness(8, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center, Foreground = Pal.B(Pal.Fore) };
            rbProfPers = new RadioButton { Content = Lang.T("Personalizzata"), GroupName = "profilo_calcolo", Margin = new Thickness(8, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center, Foreground = Pal.B(Pal.Fore) };
            ToolTip.SetTip(rbProfPrud, Lang.T("Valori più prudenti, di partenza: quota delle scie misurata dal radar di Collm (casella \"dal radar\"), scie normali, angolo β acceso.\n" +
                "Confronto con le curve di DJ5HG (Quadrantidi, tratte da Amburgo): in media 7 dB sotto, con lo stesso andamento orario (scarto di forma circa 3 dB)."));
            ToolTip.SetTip(rbProfDj, Lang.T("Quota fissa di 102 km, scie più lunghe con il radiante basso e angolo β, come in \"Die Geometrie von Meteorscatterverbindungen\" di DJ5HG (2004).\n" +
                "Confronto con le sue curve: in media 2 dB sotto, scarto tipico 5 dB; prevalgono i radianti allineati alla tratta."));
            ToolTip.SetTip(rbProfPers, Lang.T("Quota delle scie e opzioni scelte a mano. Si seleziona da sola quando cambi la quota, la casella \"dal radar\", le scie lunghe o l'angolo β."));
            WhenChanged(rbProfPrud, ToggleButton.IsCheckedProperty, () => { if (rbProfPrud.IsChecked == true) ApplyProfile(false); });
            WhenChanged(rbProfDj, ToggleButton.IsCheckedProperty, () => { if (rbProfDj.IsChecked == true) ApplyProfile(true); });
            WhenChanged(chkLayerRadar, ToggleButton.IsCheckedProperty, SyncProfile);
            numLayer.ValueChanged += (s, e) => SyncProfile();
            SyncProfile();
            // casella dell'angolo beta in evidenza: bordino in rilievo (cambia molto i valori sulle tratte lunghe)
            chkBeta.FontWeight = FontWeight.SemiBold;
            StackPanel betaInner = new StackPanel { Orientation = Orientation.Horizontal };
            betaInner.Children.Add(chkBeta);
            betaInner.Children.Add(new TextBlock
            {
                Text = "⚠ " + Lang.T("la scelta che pesa di più"), Foreground = Pal.B(Pal.OrangeText), FontWeight = FontWeight.Bold,
                VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(6, 0, 0, 0)
            });
            Border betaBox = Raised(betaInner);
            betaBox.Background = Pal.B(Pal.A(55, Pal.Orange));
            betaBox.BorderBrush = Pal.B(Pal.Orange);
            betaBox.BorderThickness = new Thickness(1.5);
            ToolTip.SetTip(betaInner, ToolTip.GetTip(chkBeta));
            DockTop(p2, Row(L(Lang.T("Impostazione del calcolo:")), rbProfPrud, rbProfDj, rbProfPers, chkTrailLen, betaBox, chkClassic));
            chart24 = new SimpleChart { Height = Chart24Height };
            DockTop(p2, chart24);
            List<string> cols24 = new List<string> { Lang.T("Sigla"), Lang.T("Sciame"), "ZHR" };
            // tabella oraria con caratteri al 150 % (richiesta dell'autore) e giustificata sulla larghezza della finestra:
            // larghezze di base compatte, poi colonne, righe e caratteri crescono insieme fino a riempire la larghezza
            List<double> w24 = new List<double> { 56, 200, 50 };
            for (int h = 0; h < 24; h++) { cols24.Add(h.ToString("00")); w24.Add(50); }
            grid24 = MakeTable(cols24.ToArray(), w24.ToArray());
            grid24.Selectable = false;
            grid24.FitWidth = true;
            grid24.RowHeight = 22 * Grid24Scale;
            grid24.HeaderHeight = 24 * Grid24Scale;
            grid24.HeaderFontSize = 12 * Grid24Scale;
            grid24.StretchLastColumn = false;
            List<int> al24 = new List<int> { 0, 0, 1 };
            for (int h = 0; h < 24; h++) al24.Add(1);   // ore centrate come nell'anteprima
            grid24.ColumnAlign = al24;
            p2.Children.Add(grid24);   // nessuno scorrimento orizzontale: la tabella e' giustificata

            // --- Attuazione sperimentale previsioni (MainWindow.Previsioni.cs)
            BuildPrevTab();

            EndGroup();

            // ===== 3. le linguette senza sottolinguette
            // --- Sciami in diretta: GMN e CMOR (MainWindow.Diretta.cs)
            BuildLiveTab();

            // --- Sciami
            DockPanel p5 = Page(Lang.T("Sciami"));
            DockTop(p5, new Border
            {
                Background = Pal.B(Pal.A(70, Pal.Yellow)), Padding = new Thickness(10, 4), Margin = new Thickness(4, 4, 4, 2), CornerRadius = new CornerRadius(4),
                Child = new TextBlock { Text = Lang.T("Le righe evidenziate sono gli sciami attivi oggi (UTC)"), Foreground = Pal.B(Pal.Fore), FontWeight = FontWeight.SemiBold }
            });
            // giustificata sulla larghezza: larghezze di base compatte, caratteri e righe crescono con la finestra
            gridShowers = MakeTable(new[] { Lang.T("Sigla"), Lang.T("Sciame"), Lang.T("Attività"), "λ max", Lang.T("Prossimo massimo (UTC)"), Lang.T("Fonte del massimo"), Lang.T("A.R."), Lang.T("Decl."), "v km/s", "r", "ZHR", Lang.T("Tipo"),
                                    "k", Lang.T("IAU MDC: soluzioni"), Lang.T("Δ radiante"), "Δλ", "Δv", Lang.T("GMN oggi") },
                                    new double[] { 44, 170, 96, 56, 170, 130, 50, 50, 44, 36, 40, 90, 52, 70, 70, 50, 44, 60 });
            gridShowers.Selectable = false;
            gridShowers.FitWidth = true;
            DockTop(p5, new TextBlock
            {
                Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 0, 6, 4),
                Text = Lang.T("k = esponente della traslazione di frequenza dall'indice di popolazione r (GMN se disponibile, altrimenti IMO): k = 3,75·log10 r. " +
                              "IAU MDC: soluzioni pubblicate nel catalogo dell'IAU Meteor Data Center; Δ = catalogo IMO del programma meno mediana delle soluzioni MDC " +
                              "(radiante confrontato alla stessa longitudine solare). GMN oggi: traiettorie confermate nell'ultimo file giornaliero del Global Meteor Network (conteggio, non flusso).")
            });
            p5.Children.Add(gridShowers);

            // --- Mappa quadrati da ADIF (MainWindow.Grid.cs)
            BuildGridTab();

            // ===== ultima: Documenti tecnici: fonti, mappa dei radar europei, efficienza - spiegazione
            Group(Lang.T("Documenti tecnici"));

            // --- Manuale d'uso (MainWindow.Spiegazione.cs)
            BuildManualTab();

            // --- Fonti
            DockPanel p6 = Page(Lang.T("Fonti"));
            txtSources = new SelectableTextBlock { FontSize = 13.5, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap, Text = SourcesText() };
            p6.Children.Add(new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = new Border { Padding = new Thickness(14, 10), Child = txtSources }
            });

            // --- Mappa dei radar europei (ex "Mappa delle fonti", MainWindow.MappaFonti.cs)
            BuildSourcesMapTab();

            // --- Spiegazione del calcolo dell'efficienza (MainWindow.Spiegazione.cs)
            BuildExplainTab();

            EndGroup();

        }

        // Titoli delle sezioni (le righe elencate in heads) in evidenza, etichette in azzurro tenue, valori in chiaro
        static void StyleReport(SelectableTextBlock rt, string text, HashSet<string> heads)
        {
            IBrush head = Pal.B(Pal.CodeHead);
            IBrush lab = Pal.B(Pal.CodeLabel);
            IBrush val = Pal.B(Pal.CodeText);
            InlineCollection inl = new InlineCollection();
            string[] lines = text.Replace("\r\n", "\n").Split('\n');
            bool first = true;
            foreach (string line in lines)
            {
                // titolo di sezione: riga indicata da chi costruisce il testo (vale per tutte le lingue)
                bool isHead = heads != null && heads.Contains(line);
                if (isHead)
                {
                    if (!first) inl.Add(new LineBreak());
                    inl.Add(new Run(line) { Foreground = head, FontWeight = FontWeight.Bold, FontSize = rt.FontSize * 14.5 / 12.5 });   // segue la scala del carattere
                    inl.Add(new LineBreak());
                }
                else
                {
                    int colon = line.IndexOf(':');
                    if (colon > 0 && colon < 34 && !line.StartsWith(" "))
                    {
                        inl.Add(new Run(line.Substring(0, colon + 1)) { Foreground = lab });
                        inl.Add(new Run(line.Substring(colon + 1)) { Foreground = val });
                    }
                    else inl.Add(new Run(line) { Foreground = val });
                    inl.Add(new LineBreak());
                }
                first = false;
            }
            rt.Inlines = inl;
        }

        // ------------------------------------------------------------ calcoli
        double BandMHz
        {
            get
            {
                double f;
                double.TryParse(cmbBand.SelectedItem as string, NumberStyles.Float, CultureInfo.InvariantCulture, out f);
                return f > 0 ? f : 144;
            }
        }

        int TrackValue { get { return (int)Math.Round(trackTime.Value); } }

        // momento del calcolo: data scelta + ora attuale + spostamento del cursore (passi di 15 minuti)
        DateTime SkyTimeUtc
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                DateTime d = dtCalc != null && dtCalc.SelectedDate.HasValue ? dtCalc.SelectedDate.Value.Date : now.Date;
                return DateTime.SpecifyKind(d, DateTimeKind.Utc).Add(now.TimeOfDay).AddMinutes(TrackValue * 15);
            }
        }

        // tempo reale: data di oggi e cursore a zero
        bool IsRealTime { get { return TrackValue == 0 && (dtCalc == null || !dtCalc.SelectedDate.HasValue || dtCalc.SelectedDate.Value.Date == DateTime.UtcNow.Date); } }

        void ResetCalcTime()
        {
            if (dtCalc != null) dtCalc.SelectedDate = DateTime.UtcNow.Date;
            trackTime.Value = 0;
            UpdateSky();
        }

        void Recalc()
        {
            LatLon home, dx;
            if (!Astro.TryLocator(Up(txtHome), out home)) { status.Text = Lang.T("Locator della stazione non valido"); return; }
            if (!Astro.TryLocator(Up(txtDx), out dx)) { status.Text = Lang.T("Locator del corrispondente non valido"); return; }
            DateTime t0 = DateTime.Now;
            PathModel.VerticalHpbwDeg = MsGeometry.HpbwVerticalDeg(MsGeometry.YagiGainDbi(V(numBoom), BandMHz));   // lobo verticale dal boom
            model = PathModel.Build(home, V(numHomeAlt) / 1000.0, dx, V(numDxAlt) / 1000.0, V(numLayer), V(numElev));
            if (PathModel.HotSpotAuto && model.Valid) numHotKm.Value = (decimal)Math.Round(Math.Max(30, Math.Min(400, model.HotOffsetKm)));   // mostra la distanza calcolata
            StartGravesModel();   // tratta GRAVES -> stazione, in un thread secondario
            sky.Model = model; sky.Station = home; sky.InvalidateHeat();
            skySmall.Model = model; skySmall.Station = home; section.Model = model;
            ApplyAntenna();
            status.Text = model.Valid
                ? Lang.F("Tratta calcolata: {0:F0} km, {1} punti di riflessione campionati in {2:F1} s. {3}",
                    model.DistKm, model.SamplePoints, (DateTime.Now - t0).TotalSeconds, model.Message)
                : Lang.F("Calcolo non riuscito: {0}", model.Message);
            UpdateSky(); Update24h(); UpdateGeometry(); UpdateShowerGrid(); UpdateFluxTab(); RefreshGrids(); UpdateSourcesMap();
            SaveConfig();
        }

        // Lobo dell'antenna: guadagno dalla lunghezza del boom (DL6WU), HPBW dal guadagno (Kraus corretto
        // con le simulazioni NEC di W4RNL: orizzontale 0,93 x Kraus, verticale Kraus).
        // Gli Hot che cadono entro +-HPBW/2 dal puntamento diventano grigi e la GUI avvisa.
        void ApplyAntenna()
        {
            double f = BandMHz;
            antGainDbi = MsGeometry.YagiGainDbi(V(numBoom), f);
            antHpbw = MsGeometry.HpbwFromGainDeg(antGainDbi);
            antHpbwV = MsGeometry.HpbwVerticalDeg(antGainDbi);
            double bl = V(numBoom) * f / 299.792458;
            lblAnt.Text = Lang.F("= {0:F1} λ  G {1:F1} dBi  lobo {2:F0}° oriz. / {3:F0}° vert.{4}", bl, antGainDbi, antHpbw, antHpbwV, bl < 2 ? " " + Lang.T("(stima poco precisa sotto 2 λ)") : "");
            decimal gd = Dm(Math.Round(antGainDbi * 2) / 2.0);
            if (numGain != null && gd >= numGain.Minimum && gd <= numGain.Maximum) numGain.Value = gd;
            if (model == null || !model.Valid) return;
            double point = model.BearingHomeToDx;   // antenna sempre verso il DX
            lblAntAz.Text = Lang.F("Antenna az: {0}° (verso il DX)", point.ToString("F0"));
            sky.AntennaAz = point; sky.HpbwDeg = antHpbw;
            skySmall.AntennaAz = point; skySmall.HpbwDeg = antHpbw;
            section.AntennaAz = point; section.HpbwDeg = antHpbw; section.HpbwVDeg = antHpbwV; section.LobeElevDeg = V(numElev);
            List<string> inside = new List<string>();
            foreach (HotPoint h in new HotPoint[] { model.HotA, model.HotB })
            {
                if (h == null) continue;
                h.InLobe = Math.Abs(Astro.Norm180(h.BearingFromHome - point)) <= antHpbw / 2;
                if (h.InLobe) inside.Add(Lang.T(h.Label));
            }
            if (inside.Count > 0)
            {
                lblLobeWarn.Text = inside.Count > 1
                    ? Lang.F("ATTENZIONE: {0} e {1} entrano nel lobo dell'antenna ({2}° a -3 dB attorno a {3}°): con questo puntamento sono gia' coperti. Segnati in grigio sulla mappa.",
                        inside[0], inside[1], antHpbw.ToString("F0"), point.ToString("F0"))
                    : Lang.F("ATTENZIONE: {0} entra nel lobo dell'antenna ({1}° a -3 dB attorno a {2}°): con questo puntamento e' gia' coperto. Segnati in grigio sulla mappa.",
                        inside[0], antHpbw.ToString("F0"), point.ToString("F0"));
                boxLobeWarn.IsVisible = true;
                status.Text = lblLobeWarn.Text;
            }
            else boxLobeWarn.IsVisible = false;
            sky.InvalidateVisual(); skySmall.InvalidateVisual(); section.InvalidateVisual();
        }

        void OnSkyClick(PointerReleasedEventArgs e)
        {
            Point pt = e.GetPosition(sky);
            MouseButton btn = e.InitialPressMouseButton;
            if (btn == MouseButton.Left)
            {
                string c = sky.HitTest(pt);
                if (c != null) { SelectShower(c); SelectInList(c); return; }
            }
            LatLon p;
            if (!sky.ShowMap || !sky.MapToLatLon(pt, out p)) return;
            string loc = Astro.ToLocator(p);
            if (btn == MouseButton.Right)
            {
                txtHome.Text = loc;
                status.Text = Lang.F("Nuovo locator della stazione dalla mappa: {0}", loc);
                Recalc();
            }
            else if (btn == MouseButton.Left)
            {
                txtDx.Text = loc;
                status.Text = Lang.F("Nuovo locator del corrispondente dalla mappa: {0}", loc);
                Recalc();
            }
        }

        string PathLabel()
        {
            return Up(txtHome) + " - " + Up(txtDx);
        }

        Vec3 RadiantVec(Shower s, DateTime t, out double zhr)
        {
            double jd = Astro.JulianDay(t);
            double lam = Astro.SolarLongitude(jd);
            zhr = s.ZhrAt(lam, t);
            double ra, dec; s.RadiantAt(lam, t, out ra, out dec);
            return Astro.RaDecToEcef(ra, dec, jd);
        }

        List<Shower> ActiveShowers(DateTime t)
        {
            List<Shower> l = new List<Shower>();
            foreach (Shower s in Catalog.Showers) if (s.IsActive(t)) l.Add(s);
            return l;
        }

        void UpdateSky()
        {
            if (sky == null) return;
            DateTime t = SkyTimeUtc;
            lblSkyTime.Text = IsRealTime ? Lang.T("Calcolo per: adesso")
                : Lang.F("Calcolo per: {0} UTC", t.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture));
            LatLon home; if (!Astro.TryLocator(Up(txtHome), out home)) return;
            sky.Station = home;
            sky.Marks.Clear();
            List<TableRow> rows = new List<TableRow>();
            DateTime day0 = t.Date;
            List<string> high = new List<string>();
            bool mv = model != null && model.Valid;
            double[] acc = mv ? new double[model.SamplePoints] : null;
            List<TrailSeg> trails = new List<TrailSeg>();
            foreach (Shower s in ActiveShowers(t))
            {
                double zhr;
                Vec3 v = RadiantVec(s, t, out zhr);
                RadiantMark m = new RadiantMark();
                m.Code = s.Code; m.Zhr = zhr;
                double maz, mel; Astro.AltAz(home, v, out maz, out mel);
                m.Az = maz; m.El = mel;
                double lf = 0.5;
                m.Eff = mv ? model.Efficiency(v, out lf) : 0;
                m.LeftFrac = lf;
                if (mv && zhr > 0.1) AddPrevision(v, zhr, s.Code, acc, trails);
                string rise = "", set = "";
                double prevEl = double.NaN; bool anyUp = false, anyDown = false;
                for (int k = 0; k <= 144; k++)
                {
                    DateTime tk = day0.AddMinutes(10 * k);
                    double z2; Vec3 vk = RadiantVec(s, tk, out z2);
                    double az, el; Astro.AltAz(home, vk, out az, out el);
                    m.Track.Add(new System.Drawing.PointF((float)az, (float)el));
                    m.TrackHours.Add(k % 6 == 0 && k < 144 ? k / 6 : -1);
                    if (el > 0) anyUp = true; else anyDown = true;
                    if (!double.IsNaN(prevEl))
                    {
                        if (prevEl <= 0 && el > 0 && rise == "") rise = tk.AddMinutes(-5).ToString("HH:mm", CultureInfo.InvariantCulture);
                        if (prevEl > 0 && el <= 0 && set == "") set = tk.AddMinutes(-5).ToString("HH:mm", CultureInfo.InvariantCulture);
                    }
                    prevEl = el;
                }
                if (!anyDown) { rise = Lang.T("circumpolare"); set = ""; }
                if (!anyUp) { rise = Lang.T("sempre sotto"); set = ""; }
                sky.Marks.Add(m);
                string[] cells = new string[10];
                cells[0] = Lang.T(s.Name);
                cells[1] = s.Code;
                cells[2] = zhr >= 10 ? zhr.ToString("F0") : zhr.ToString("F1");
                cells[3] = m.Az.ToString("F0");
                cells[4] = m.El.ToString("F0") + (m.El > HighRadiantDeg ? " ↑" : "");
                if (m.El > HighRadiantDeg && zhr >= 1) high.Add(s.Code + " " + m.El.ToString("F0") + "°");
                cells[5] = m.El > 0 ? m.Eff.ToString("F0") : "-";
                cells[6] = m.El > 0 && m.Eff > 1 ? (lf >= 0.5 ? "A" : "B") + " " + (100 * Math.Max(lf, 1 - lf)).ToString("F0") + "%" : "-";
                // miglior puntamento: ortogonale alla direzione di arrivo dello sciame (azimut del radiante +-90°)
                cells[7] = m.El > 0 ? Astro.Norm360(m.Az + 90).ToString("F0") + "° / " + Astro.Norm360(m.Az - 90).ToString("F0") + "°" : "-";
                cells[8] = rise; cells[9] = set;
                TableRow row = new TableRow { Cells = cells, Tag = s.Code };
                if (m.El > 0 && m.Eff >= 50 && zhr >= 2) row.Fore = Pal.Yellow;
                else if (m.El <= 0 || zhr < 1) row.Fore = Pal.Rgb(128, 128, 128);
                rows.Add(row);
            }
            lvSky.Rows = rows;
            lvSky.SelectedIndex = -1;
            lvSky.Refresh();
            // zone PREV in pianta: punti di riflessione con almeno il 3 % del massimo
            sky.Prev.Clear();
            if (mv)
            {
                double amax = 0; foreach (double a in acc) amax = Math.Max(amax, a);
                if (amax > 0)
                    for (int i = 0; i < acc.Length; i++)
                        if (acc[i] >= 0.03 * amax)
                        {
                            PrevSpot sp = new PrevSpot(); sp.Pos = model.SampleLatLon(i); sp.W = acc[i]; sp.Left = model.SampleLeft(i);
                            sky.Prev.Add(sp);
                        }
            }
            section.Trails = trails;
            skySmall.InvalidateVisual(); section.InvalidateVisual();
            if (high.Count > 0)
            {
                lblHighRad.Text = Lang.F("RADIANTE TROPPO ALTO (sopra {0}°): {1}. Le scie scendono quasi verticali e riflettono poco verso la tratta (DF5HC, DH9GCD).",
                    HighRadiantDeg.ToString("F0"), string.Join(", ", high.ToArray()));
                boxHighRad.IsVisible = true;
            }
            else boxHighRad.IsVisible = false;
            // radiante basso (sotto 20°) quasi allineato alla tratta (entro 30°, nei due versi): l'efficienza calcolata resta
            // bassa (prudente), ma DJ5HG prevede in questa situazione riflessioni rare e forti (Amburgo-Narvik, Quadrantidi)
            List<string> lowAlong = new List<string>();
            if (mv)
                foreach (RadiantMark rm in sky.Marks)
                {
                    if (rm.El <= 0 || rm.El > LowRadiantDeg || rm.Zhr < 1) continue;
                    double d = Math.Abs(Astro.Norm180(rm.Az - model.BearingHomeToDx));
                    if (d <= 30 || d >= 150) lowAlong.Add(rm.Code + " " + rm.El.ToString("F0") + "°");
                }
            if (lowAlong.Count > 0)
            {
                lblLowAlong.Text = Lang.F("RADIANTE BASSO LUNGO LA TRATTA: {0}. L'efficienza calcolata resta bassa, ma con scie radenti allineate alla tratta DJ5HG prevede riflessioni rare e forti: vale la pena ascoltare.",
                    string.Join(", ", lowAlong.ToArray()));
                boxLowAlong.IsVisible = true;
            }
            else boxLowAlong.IsVisible = false;
            // tratta oltre l'orizzonte della scia: a quella quota il punto medio non e' visto da nessuna delle due stazioni
            if (mv && model.ElevToMidDeg < 0)
            {
                double hMin = MsGeometry.MinLayerForPathKm(model.DistKm, Math.Min(model.HomeAltKm, model.DxAltKm));
                lblHorizon.Text = Lang.F("TRATTA OLTRE L'ORIZZONTE DELLA SCIA: con le scie a {0:F1} km il punto medio è {1:F1}° sotto l'orizzonte. Servono scie ad almeno {2:F0} km di quota: collegamento possibile solo con le scie più alte, efficienze e attenuazioni sono al limite del modello.",
                    model.LayerKm, -model.ElevToMidDeg, hMin);
                boxHorizon.IsVisible = true;
            }
            else boxHorizon.IsVisible = false;
            string noZone = mv ? NoZoneText() : null;
            lblNoZoneSky.Text = noZone ?? ""; boxNoZoneSky.IsVisible = noZone != null;
            // Hot consigliato dall'efficienza: il lato (A o B) da cui arriva la maggior parte delle riflessioni,
            // sommando su tutti gli sciami attivi ZHR x efficienza x frazione del lato
            double wA = 0, wB = 0;
            foreach (RadiantMark rm in sky.Marks)
                if (rm.El > 0 && rm.Zhr > 0) { double w = rm.Zhr * rm.Eff / 100.0; wA += w * rm.LeftFrac; wB += w * (1 - rm.LeftFrac); }
            recommendText = ""; recommendShort = "";
            if (mv && model.HotA != null && model.HotB != null)
            {
                model.HotA.Recommended = false; model.HotB.Recommended = false;
                if (wA + wB > 0.05)
                {
                    HotPoint rh = wA >= wB ? model.HotA : model.HotB;
                    rh.Recommended = true;
                    recommendText = Lang.F("{0} consigliato ora: az. {1}° ({2}% delle riflessioni previste)", Lang.T(rh.Label), rh.BearingFromHome.ToString("F0"),
                        (100 * Math.Max(wA, wB) / (wA + wB)).ToString("F0"));
                    recommendShort = Lang.F("{0} consigliato ora: az. {1}°", Lang.T(rh.Label), rh.BearingFromHome.ToString("F0"));
                }
                else if (model.SamplePoints == 0) recommendShort = recommendText = Lang.T("Nessun Hot consigliato: le due stazioni non vedono una zona comune dello strato (vedi l'avviso)");
                else recommendShort = recommendText = Lang.T("Nessun Hot consigliato: sciami attivi quasi assenti in questo momento");
            }
            UpdateEffStrip(t, mv);
            sky.Caption = Lang.F("Pianta: linea gialla = scorrimento dello sciame selezionato (o del più favorevole), ellissi PREV = dove cadono ora le scie più probabili. " +
                "Volta celeste a destra (Sorge/Tramonta in UTC del {0}): giallo = efficienza >= 50 % e ZHR >= 2, arancio >= 20 %, grigio = quasi inattivo.",
                day0.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            lblMapCaption.Text = Lang.F("Mappa: bordo = {0} km dalla stazione. {1}", sky.MapRadiusKm.ToString("F0"), sky.Caption);
            sky.InvalidateVisual();
            if (mv)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Lang.F("Tratta {0}: {1:F0} km, az. {2:F0}°", PathLabel(), model.DistKm, model.BearingHomeToDx)).Append('\n');
                sb.Append(Lang.F("Punto medio {0}, elev. per {1:F1} km: {2:F1}°", Astro.ToLocator(model.Mid), model.LayerKm, model.ElevToMidDeg)).Append('\n');
                if (model.HotA != null)
                {
                    sb.Append(Lang.F("Hot A: az. {0:F0}° ({1:+0;-0}°)  {2}", model.HotA.BearingFromHome, model.HotA.SideScatterDeg, Astro.ToLocator(model.HotA.Pos))).Append('\n');
                    sb.Append(Lang.F("Hot B: az. {0:F0}° ({1:+0;-0}°)  {2}", model.HotB.BearingFromHome, model.HotB.SideScatterDeg, Astro.ToLocator(model.HotB.Pos))).Append('\n');
                    sb.Append(Lang.F("{0} (in rosso sulla mappa)", recommendText)).Append('\n');
                }
                if (model.LimitA != null)
                    sb.Append(Lang.F("Limiti zona comune: A {0:F0}° ({1:+0;-0}°), B {2:F0}° ({3:+0;-0}°)", model.LimitA.BearingFromHome, model.LimitA.SideScatterDeg, model.LimitB.BearingFromHome, model.LimitB.SideScatterDeg)).Append('\n');
                else sb.Append(Lang.F("Nessuna zona comune dello strato E ({0})", model.Message)).Append('\n');
                sb.Append(Lang.T("Lato A = a sinistra della tratta, B = a destra")).Append('\n');
                sb.Append((PathModel.HotSpotAuto ? Lang.F("Hot A/B: hot spot a {0} km dal punto medio, scarto {1}° (tabella AMS di J. Richardson, DJ5HG).", model.HotOffsetKm.ToString("F0"), PathModel.AmsOffsetDeg(model.DistKm).ToString("F1")) : Lang.F("Hot A/B: hot spot a {0} km dal punto medio (valore scritto a mano).", model.HotOffsetKm.ToString("F0")))).Append('\n');
                sb.Append(Lang.T("Miglior puntamento per lo sciame: ORTOGONALE alla direzione\n" +
                                 "di arrivo (azimut del radiante +-90°, tolleranza +-40-50°,\n" +
                                 "SM6FHZ). In pianta: linea gialla = scorrimento\n" +
                                 "dello sciame, punti arancioni = puntamenti, ellissi\n" +
                                 "PREV = scie piu' probabili adesso.\n" +
                                 "Clic su un radiante: percorso con le ore UTC"));
                lblPathInfo.Text = sb.ToString();
            }
            if (sky.Selected != null) SelectInList(sky.Selected);
            UpdateGravesSky();
        }

        TextBlock lblEffStrip;

        // Fascia sopra la pianta: momento del calcolo, indice della tratta, sciame piu' utile, Hot consigliato
        void UpdateEffStrip(DateTime t, bool mv)
        {
            if (lblEffStrip == null) return;
            double dh = Math.Round((t - DateTime.UtcNow).TotalHours * 4) / 4;
            string when = Lang.F("Calcolo per {0} UTC", t.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)) + " · " +
                          (IsRealTime ? Lang.T("adesso") : Lang.F("{0} h rispetto ad adesso", dh.ToString("+0.##;-0.##", CultureInfo.CurrentCulture)));
            if (!mv) { lblEffStrip.Text = when; return; }
            double idx = 0, best = 0; RadiantMark bm = null; int up = 0;
            foreach (RadiantMark rm in sky.Marks)
            {
                if (rm.El <= 0) continue;
                up++;
                double w = rm.Zhr * rm.Eff / 100.0;
                idx += w;
                if (w > best) { best = w; bm = rm; }
            }
            StringBuilder sb = new StringBuilder(when);
            sb.Append("   |   ").Append(Lang.F("Indice della tratta Σ ZHR×η: {0}", idx.ToString("F1", CultureInfo.CurrentCulture)));
            sb.Append("   |   ");
            if (bm != null && best >= 0.05)
            {
                sb.Append(Lang.F("Sciame più utile: {0} (ZHR {1}, efficienza {2} %)", bm.Code,
                    bm.Zhr.ToString(bm.Zhr >= 10 ? "F0" : "F1", CultureInfo.CurrentCulture), bm.Eff.ToString("F0", CultureInfo.CurrentCulture)));
                LatLon hm; double db = double.NaN;
                if (Astro.TryLocator(Up(txtHome), out hm)) db = model.AbsoluteDb(Astro.FromAltAz(hm, bm.Az, bm.El));
                if (!double.IsNaN(db) && !double.IsInfinity(db))
                    sb.Append(" · ").Append(Lang.F("{0} dB rispetto al riferimento di DJ5HG", db.ToString("+0.0;-0.0", CultureInfo.CurrentCulture)));
            }
            else sb.Append(Lang.T("Nessuno sciame utile in questo momento"));
            sb.Append("   |   ").Append(Lang.F("Sciami sopra l'orizzonte: {0}", up));
            if (!string.IsNullOrEmpty(recommendShort)) sb.Append("   |   ").Append(recommendShort);
            lblEffStrip.Text = sb.ToString();
        }

        // Contributo di uno sciame alle zone PREV (pianta) e alle scie della sezione:
        // peso di ogni punto di riflessione = ZHR x contributo all'efficienza (condizione speculare)
        void AddPrevision(Vec3 v, double zhr, string code, double[] acc, List<TrailSeg> trails)
        {
            double[] c = model.Contributions(v);
            for (int i = 0; i < c.Length && i < acc.Length; i++) acc[i] += zhr * c[i];
            // per la sezione: i 14 punti piu' forti di questo sciame
            bool[] used = new bool[c.Length];
            for (int k = 0; k < 14; k++)
            {
                int bi = -1; double bv = 0;
                for (int i = 0; i < c.Length; i++) if (!used[i] && c[i] > bv) { bv = c[i]; bi = i; }
                if (bi < 0) break;
                used[bi] = true;
                TrailSeg ts = new TrailSeg(); ts.P = model.SamplePos(bi); ts.S = v; ts.W = zhr * bv; ts.Code = code; ts.Left = model.SampleLeft(bi);
                trails.Add(ts);
            }
        }

        void SelectShower(string code)
        {
            sky.Selected = code; skySmall.Selected = code;
            sky.InvalidateVisual(); skySmall.InvalidateVisual();
        }

        void SelectInList(string code)
        {
            for (int i = 0; i < lvSky.Rows.Count; i++)
                if (lvSky.Rows[i].Cells.Length > 1 && lvSky.Rows[i].Cells[1] == code)
                {
                    lvSky.SelectedIndex = i;
                    lvSky.EnsureVisible(i);
                    lvSky.Refresh();
                    return;
                }
        }

        // Radiante troppo alto secondo DF5HC (tabella Geminidi 2017): sopra 70° di elevazione
        public const double HighRadiantDeg = 70.0;
        // radiante basso per l'avviso "lungo la tratta" (DJ5HG 2004: radiante tiefe am Horizont)
        public const double LowRadiantDeg = 20.0;
        Border boxLowAlong; TextBlock lblLowAlong;
        Border boxHorizon; TextBlock lblHorizon;
        RadioButton rbProfPrud, rbProfDj, rbProfPers;
        ComboBox cmbR0, cmbDur;
        CheckBox chkBeta, chkClassic, chkLobe;
        bool applyingProfile;
        public const double Dj5hgLayerKm = 102.0;

        // impostazione pronta: prudente = quota dal radar e scie normali; DJ5HG = 102 km fissi e scie lunghe
        void ApplyProfile(bool dj5hg)
        {
            if (applyingProfile) return;
            applyingProfile = true;
            try
            {
                if (dj5hg)
                {
                    chkLayerRadar.IsChecked = false;
                    numLayer.Value = Dm(Dj5hgLayerKm);
                    chkTrailLen.IsChecked = true;
                    chkBeta.IsChecked = true;
                }
                else
                {
                    chkLayerRadar.IsChecked = true;
                    if (!ApplyRadarLayer() && V(numLayer) == Dj5hgLayerKm) numLayer.Value = Dm(cfg.GetD("quota_scie_km_prudente", 90));
                    chkTrailLen.IsChecked = false;
                    chkBeta.IsChecked = true;
                }
                PathModel.TrailLength = chkTrailLen.IsChecked == true;
                PathModel.BetaFactor = chkBeta.IsChecked == true;
            }
            finally { applyingProfile = false; }
            SyncProfile();
            if (model != null) Recalc();
        }

        // seleziona la casella che corrisponde alle impostazioni attuali
        void SyncProfile()
        {
            if (applyingProfile || rbProfPrud == null || chkLayerRadar == null || chkTrailLen == null || chkBeta == null) return;
            applyingProfile = true;
            try
            {
                bool radar = chkLayerRadar.IsChecked == true, trail = chkTrailLen.IsChecked == true, beta = chkBeta.IsChecked == true;
                if (!radar && trail && beta && Math.Abs(V(numLayer) - Dj5hgLayerKm) < 0.01) rbProfDj.IsChecked = true;
                else if (radar && !trail && beta) rbProfPrud.IsChecked = true;
                else rbProfPers.IsChecked = true;
            }
            finally { applyingProfile = false; }
        }

        // Formula della migliore direzione di DH9GCD (DF5HC, "Meteorscatter auf UKW", DARC 2018):
        // E = sin(El) cos(El) sin(P - Az); massimo 0,5 con El = 45° e P - Az = 90°. Restituita in % del massimo.
        public static double Dh9gcdPercent(double radAz, double radEl, double pathAz)
        {
            if (radEl <= 0) return 0;
            double el = radEl * Astro.D2R;
            double e = Math.Sin(el) * Math.Cos(el) * Math.Abs(Math.Sin((pathAz - radAz) * Astro.D2R));
            return 200.0 * e;
        }

        DateTime SelectedDayUtc
        {
            get
            {
                DateTime d = dtCalc.SelectedDate ?? DateTime.UtcNow;
                return DateTime.SpecifyKind(d.Date, DateTimeKind.Utc);
            }
        }

        // Avviso quando nessun punto dello strato e' visto da entrambe le stazioni sopra l'elevazione del lobo:
        // il modello scarta i punti sotto quell'elevazione, quindi tutte le efficienze valgono 0. null = zona comune presente.
        string NoZoneText()
        {
            if (model == null || !model.Valid || model.SamplePoints > 0) return null;
            double alt = Math.Min(model.HomeAltKm, model.DxAltKm), half = model.DistKm / 2;
            if (PathModel.LobePattern)
            {
                double hor = MsGeometry.LayerRangeKm(model.LayerKm, 0, alt);
                string t = Lang.F("NESSUNA ZONA COMUNE: con le scie a {0:F1} km ogni stazione vede lo strato solo fino all'orizzonte, a {1:F0} km, ma il punto medio della tratta ({2:F0} km) è a {3:F0} km. Nessun punto di riflessione è visibile da entrambe le stazioni, quindi tutte le efficienze valgono 0.",
                    model.LayerKm, hor, model.DistKm, half);
                double hh = -1;
                if (MsGeometry.LayerRangeKm(200, 0, alt) > half)
                {
                    double lo = model.LayerKm, hi = 200;
                    for (int i = 0; i < 40; i++) { double m = (lo + hi) / 2; if (MsGeometry.LayerRangeKm(m, 0, alt) > half) hi = m; else lo = m; }
                    hh = Math.Ceiling(hi);
                }
                if (hh > 0) t += " " + Lang.F("Servono scie ad almeno {0:F0} km.", hh);
                return t + " " + Lang.T("La formula di DH9GCD (righe azzurre) non tiene conto dell'orizzonte né del lobo, per questo dà valori anche qui.");
            }
            double reach = MsGeometry.LayerRangeKm(model.LayerKm, model.MinElevDeg, alt);
            string s = Lang.F("NESSUNA ZONA COMUNE: con le scie a {0:F1} km e il lobo d'antenna a {1:F1}° ogni stazione vede lo strato fino a {2:F0} km, ma il punto medio della tratta ({3:F0} km) è a {4:F0} km. " +
                "Il calcolo tiene solo i punti visti da entrambe le stazioni almeno a quell'elevazione: qui non ce n'è nessuno, quindi tutte le efficienze valgono 0.",
                model.LayerKm, model.MinElevDeg, reach, model.DistKm, half);
            // elevazione massima del lobo e quota minima delle scie che aprono la zona comune
            double needE = -1;
            if (MsGeometry.LayerRangeKm(model.LayerKm, 0, alt) > half)
            {
                double lo = 0, hi = Math.Max(0.1, model.MinElevDeg);
                for (int i = 0; i < 40; i++) { double m = (lo + hi) / 2; if (MsGeometry.LayerRangeKm(model.LayerKm, m, alt) > half) lo = m; else hi = m; }
                needE = Math.Floor(lo * 10) / 10;
            }
            double needH = -1;
            if (MsGeometry.LayerRangeKm(200, model.MinElevDeg, alt) > half)
            {
                double lo = model.LayerKm, hi = 200;
                for (int i = 0; i < 40; i++) { double m = (lo + hi) / 2; if (MsGeometry.LayerRangeKm(m, model.MinElevDeg, alt) > half) hi = m; else lo = m; }
                needH = Math.Ceiling(hi);
            }
            if (needE >= 0 && needH > 0) s += " " + Lang.F("Per avere un calcolo: elevazione del lobo di {0:F1}° o meno, oppure scie ad almeno {1:F0} km.", needE, needH);
            else if (needE >= 0) s += " " + Lang.F("Per avere un calcolo: elevazione del lobo di {0:F1}° o meno.", needE);
            else if (needH > 0) s += " " + Lang.F("Anche con il lobo a 0° non basta: servono scie ad almeno {0:F0} km.", needH);
            s += " " + Lang.T("La formula di DH9GCD (righe azzurre) non tiene conto dell'orizzonte né del lobo, per questo dà valori anche qui.");
            return s;
        }

        void Update24h()
        {
            UpdateGraves24h();   // cronologia della linguetta Radar GRAVES (non dipende dalla tratta principale)
            if (model == null || !model.Valid || grid24 == null) return;
            DateTime day = SelectedDayUtc;
            List<Shower> act = new List<Shower>();
            foreach (Shower s in Catalog.Showers)
            {
                double z; RadiantVec(s, day.AddHours(12), out z);
                if (s.IsActive(day) && z >= 1) act.Add(s);
            }
            act.Sort((a, b) =>
            {
                double za, zb; RadiantVec(a, day.AddHours(12), out za); RadiantVec(b, day.AddHours(12), out zb);
                return zb.CompareTo(za);
            });
            List<TableRow> rows = new List<TableRow>();
            double[] index = new double[24], wA = new double[24], wB = new double[24];
            double bestScore = 0; string bestShower = "-";
            chart24.Series.Clear();
            Color[] pal = { Pal.Ink(Pal.Rgb(255, 210, 40)), Pal.Ink(Pal.Rgb(0, 220, 255)), Pal.Ink(Pal.Rgb(240, 110, 230)), Pal.Ink(Pal.Rgb(120, 230, 120)), Pal.Ink(Pal.Rgb(255, 140, 60)) };
            int ns = 0;
            foreach (Shower s in act)
            {
                string[] row = new string[27];
                row[0] = s.Code; row[1] = Lang.T(s.Name);
                double zmax = 0;
                double[] effs = new double[24];
                double[] dh = new double[24];
                bool[] tooHigh = new bool[24];
                string[] sides = new string[24];
                string[] tips = new string[27];
                for (int h = 0; h < 24; h++)
                {
                    DateTime t = day.AddHours(h + 0.5);
                    double z; Vec3 v = RadiantVec(s, t, out z);
                    zmax = Math.Max(zmax, z);
                    double lf; double e = model.Efficiency(v, out lf);
                    effs[h] = e; sides[h] = lf >= 0.5 ? "A" : "B";
                    double hAz, hEl; Astro.AltAz(model.Home, v, out hAz, out hEl);
                    dh[h] = Dh9gcdPercent(hAz, hEl, model.BearingHomeToDx);
                    tooHigh[h] = hEl > HighRadiantDeg;
                    index[h] += z * e / 100.0;
                    wA[h] += z * e / 100.0 * lf; wB[h] += z * e / 100.0 * (1 - lf);
                    if (z * e / 100.0 > bestScore)
                    {
                        bestScore = z * e / 100.0;
                        double raz, rel; Astro.AltAz(model.Home, v, out raz, out rel);
                        bestShower = s.Code + " " + Lang.T(s.Name) + "\n" + Lang.F("efficienza {0}% alle {1}:30, lato {2}", e.ToString("F0"), h.ToString("00"), sides[h]) +
                            "\n" + Lang.F("puntamento migliore {0}° / {1}°", Astro.Norm360(raz + 90).ToString("F0"), Astro.Norm360(raz - 90).ToString("F0"));
                    }
                    row[3 + h] = (e < 1 ? "" : e.ToString("F0") + sides[h]) + (tooHigh[h] ? "↑" : "");   // lettera del lato Hot su ogni valore
                    if (e >= 1) tips[3 + h] = HotTip(s, t, e, lf);
                }
                row[2] = zmax.ToString("F0");
                TableRow tr = new TableRow { Cells = row, CellBack = new Color?[27], CellFore = new Color?[27], Tag = s.Code, FontSize = 12 * Grid24Scale, CellTips = tips };
                for (int h = 0; h < 24; h++)
                {
                    double e = effs[h];
                    tr.CellBack[3 + h] = Pal.HeatBack(e);
                    tr.CellFore[3 + h] = Pal.HeatFore(e);
                }
                rows.Add(tr);
                // riga di confronto: formula di DH9GCD (riportata da DF5HC, 2018), in % del suo massimo
                string[] drow = new string[27];
                drow[0] = ""; drow[1] = "   DH9GCD %"; drow[2] = "";
                for (int h = 0; h < 24; h++) drow[3 + h] = dh[h] < 1 ? "" : dh[h].ToString("F0");
                rows.Add(new TableRow { Cells = drow, Fore = Pal.Azure, Italic = true, FontSize = 11 * Grid24Scale });
                if (chkClassic != null && chkClassic.IsChecked == true)
                {
                    // riga del giudizio classico (radiante di traverso), calcolato al punto medio della tratta
                    string[] crow = new string[27];
                    crow[0] = ""; crow[1] = "   " + Lang.T("classico"); crow[2] = "";
                    string[] sym = { "✗", "◐", "●", "★" };
                    for (int h = 0; h < 24; h++)
                    {
                        double z; Vec3 v = RadiantVec(s, day.AddHours(h + 0.5), out z);
                        double maz, mel; Astro.AltAz(model.Mid, v, out maz, out mel);
                        crow[3 + h] = sym[MsGeometry.ClassicVerdict(maz, mel, Astro.Bearing(model.Mid, model.Dx))];
                    }
                    rows.Add(new TableRow { Cells = crow, Fore = Pal.Muted, FontSize = 11 * Grid24Scale });
                }
                if (ns < 5)
                {
                    ChartSeries cs = new ChartSeries(); cs.Name = s.Code + " " + Lang.T("effic. %"); cs.Color = pal[ns]; cs.Values = effs;
                    chart24.Series.Add(cs);
                    // stessa curva con la formula di DH9GCD, a puntini
                    ChartSeries cd = new ChartSeries(); cd.Name = s.Code + " DH9GCD %"; cd.Color = pal[ns]; cd.Values = dh; cd.Dotted = true; cd.LineWidth = 1.4f;
                    chart24.Series.Add(cd); ns++;
                }
            }
            string[] irow = new string[27];
            irow[0] = "Σ"; irow[1] = Lang.T("Indice tratta (ZHR×eff.)"); irow[2] = "";
            for (int h = 0; h < 24; h++) irow[3 + h] = index[h].ToString("F1");
            rows.Add(new TableRow { Cells = irow, FontSize = 12 * Grid24Scale });

            // riga della previsione sperimentale (solo per le ore in cui esiste: oggi e in parte domani)
            bool anyFc = false;
            string[] frow = new string[27];
            frow[0] = "~"; frow[1] = Lang.F("Previsione {0} MHz (rifl./ora)", PrevFreqMHz.ToString("F0")); frow[2] = "";
            for (int h = 0; h < 24; h++)
            {
                double fc = ForecastAt(day.AddHours(h));
                frow[3 + h] = fc < 0 ? "" : fc.ToString("F0");
                if (fc >= 0) anyFc = true;
            }
            if (anyFc) rows.Add(new TableRow { Cells = frow, Fore = Pal.Orange, Italic = true, FontSize = 12 * Grid24Scale });
            for (int i = 0; i < rows.Count; i++)
                for (int c = 0; c < rows[i].Cells.Length; c++) if (rows[i].Cells[c] == null) rows[i].Cells[c] = "";
            grid24.Rows = rows;
            grid24.Refresh();

            // colpo d'occhio
            int hb = 0; for (int h = 1; h < 24; h++) if (index[h] > index[hb]) hb = h;
            // perche' non c'e' nessun valore: zona comune assente, nessuno sciame attivo, radianti sfavorevoli tutto il giorno
            string noZone = NoZoneText();
            string why = noZone != null ? Lang.T("nessun valore: le due stazioni non vedono una zona comune dello strato E (vedi l'avviso qui sotto)")
                : act.Count == 0 ? Lang.T("nessun valore: in questo giorno non c'è nessuno sciame attivo con ZHR di almeno 1 (catalogo IMO)")
                : Lang.F("nessun valore: i {0} sciami attivi hanno per tutto il giorno il radiante sotto l'orizzonte o in posizione sfavorevole per questa tratta (efficienza sotto l'1 %)", act.Count);
            lblNoZone.Text = noZone ?? ""; boxNoZone.IsVisible = noZone != null;
            glBest.Text = index[hb] > 0
                ? Lang.F("{0:00}:00-{0:00}:59 UTC\nindice della tratta {1:F1}\nlato prevalente {2} ({3:F0}%)", hb, index[hb], wA[hb] >= wB[hb] ? "A" : "B", 100 * Math.Max(wA[hb], wB[hb]) / Math.Max(1e-9, wA[hb] + wB[hb]))
                : why;
            glShower.Text = bestScore > 0 ? bestShower : why;
            string tipWhy = index[hb] > 0 ? "" : "\n\n" + (noZone ?? why);
            ToolTip.SetTip(cardBest, Lang.T("L'ora del giorno scelto con l'indice della tratta più alto (somma di ZHR × efficienza di tutti gli sciami attivi) e il lato della tratta da cui arriva la maggior parte delle riflessioni.") + tipWhy);
            ToolTip.SetTip(cardShower, Lang.T("Lo sciame che dà il contributo più alto (ZHR × efficienza) nel giorno scelto, con l'ora, il lato e le due direzioni d'antenna migliori.") + (bestScore > 0 ? "" : "\n\n" + (noZone ?? why)));
            DateTime nowU = DateTime.UtcNow;
            if (nowU.Date == day.Date)
            {
                int hn = nowU.Hour;
                glNow.Text = Lang.F("{0:00}:{1:00} UTC - indice {2:F1}\nlato prevalente {3}\n{4}", hn, nowU.Minute, index[hn],
                    index[hn] > 0.05 ? (wA[hn] >= wB[hn] ? "A" : "B") : "-", recommendShort.Trim());
            }
            else glNow.Text = Lang.T("il giorno scelto non è oggi");
            ToolTip.SetTip(cardNow, Lang.T("L'indice della tratta nell'ora attuale, il lato prevalente e l'Hot consigliato. Compare solo se il giorno scelto è oggi.") +
                (nowU.Date == day.Date && index[nowU.Hour] <= 0 ? "\n\n" + (noZone ?? why) : ""));
            DateTime hNow = new DateTime(nowU.Year, nowU.Month, nowU.Day, nowU.Hour, 0, 0, DateTimeKind.Utc);
            double f1 = ForecastAt(hNow);
            double f2 = ForecastAt(hNow.AddHours(1));
            double f3 = ForecastAt(hNow.AddHours(2));
            glFore.Text = f1 < 0 ? Lang.T("nessun dato radar: vedi Live e dati real-time › Flusso radar")
                : Lang.F("{0:F0} MHz, riflessioni/ora previste\nora {1:F0} - tra 1 h {2} - tra 2 h {3}\nsperimentale (Collm, regole SM6FHZ)", PrevFreqMHz, f1,
                    f2 < 0 ? "-" : f2.ToString("F0"), f3 < 0 ? "-" : f3.ToString("F0"));
            ToolTip.SetTip(cardFore, Lang.T("Riflessioni all'ora previste sulla banda scelta, ottenute traslando i conteggi del radar di Collm con le regole di SM6FHZ. Serve il dato del radar (Live e dati real-time › Flusso radar). È indipendente dalla geometria della tratta."));
            ChartSeries ix = new ChartSeries(); ix.Name = Lang.T("indice tratta"); ix.Color = Pal.Ink(Pal.Rgb(90, 110, 200)); ix.Values = index; ix.Bars = true; ix.RightAxis = true;
            chart24.Series.Insert(0, ix);
            chart24.Title = Lang.F("Efficienza oraria per {0} del {1} (UTC, valori a metà ora)", PathLabel(), day.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            chart24.YLabel = Lang.T("efficienza %"); chart24.Y2Label = Lang.T("indice"); chart24.YMaxFixed = 100;
            string[] xl = new string[24]; for (int h = 0; h < 24; h++) xl[h] = h.ToString("00");
            chart24.XLabels = xl;
            SetLocalRow(chart24, day.Date, 24);
            chart24.MarkIndex = day.Date == DateTime.UtcNow.Date ? DateTime.UtcNow.Hour : -1;
            chart24.InvalidateVisual();
        }

        void UpdateGeometry()
        {
            if (model == null || !model.Valid || txtGeo == null) return;
            double f = BandMHz;
            double q = Math.Pow(10, V(numQ));
            StringBuilder sb = new StringBuilder();
            HashSet<string> heads = new HashSet<string>();   // righe dei titoli di sezione (evidenziate)
            Action<string> head = t => { heads.Add(t); sb.Append(t).Append('\n'); };
            Action<string> line = t => sb.Append(t).Append('\n');
            head(Lang.T("GEOMETRIA DELLA TRATTA (Terra sferica, R = 6372,7955 km)"));
            line(Lang.F("Stazione        {0}   quota {1:F0} m", Up(txtHome), V(numHomeAlt)));
            line(Lang.F("Corrispondente  {0}   quota {1:F0} m", Up(txtDx), V(numDxAlt)));
            line(Lang.F("Distanza        {0:F0} km   azimut {1:F1}°   (dal corrispondente {2:F1}°)", model.DistKm, model.BearingHomeToDx, model.BearingDxToHome));
            line(Lang.F("Punto medio     {0}  ({1:F2}N {2:F2}E)", Astro.ToLocator(model.Mid), model.Mid.Lat, model.Mid.Lon));
            line(Lang.F("Quota scie      {0:F1} km (strato E){1}", model.LayerKm, collm != null && chkLayerRadar.IsChecked == true ? "  " + Lang.T("<- moda misurata dal radar Collm") : ""));
            line(Lang.F("Elevazione d'antenna verso il punto medio: {0:F1}°   (formula dell'articolo, Immagine 7)", model.ElevToMidDeg));
            if (model.ElevToMidDeg < 0) line("  " + Lang.F("-> punto medio sotto l'orizzonte alla quota delle scie: tratta oltre la portata geometrica del singolo salto (servono scie ad almeno {0:F0} km)",
                MsGeometry.MinLayerForPathKm(model.DistKm, Math.Min(model.HomeAltKm, model.DxAltKm))));
            sb.Append("\n");
            head(Lang.T("HOT A / HOT B (hot spot classici, per il puntamento d'antenna)"));
            line(PathModel.HotSpotAuto ? Lang.F("Punti a {0:F0} km ai lati del punto medio, perpendicolari alla tratta: scarto {1:F1}° dalla tabella AMS di J. Richardson (13° a 1000 km, 11° a 1250, 10° a 1500 e 2000), riportata da DJ5HG", model.HotOffsetKm, PathModel.AmsOffsetDeg(model.DistKm))
                                         : Lang.F("Punti a {0:F0} km ai lati del punto medio, perpendicolari alla tratta (valore scritto a mano; MSHV usa 150 km)", model.HotOffsetKm));
            foreach (HotPoint h in new HotPoint[] { model.HotA, model.HotB })
            {
                if (h == null) continue;
                line(Lang.F("{0}: {1}  az. da me {2:F1}° (scarto {3:+0.0;-0.0}° dalla direzione diretta), dal corrispondente {4:F0}°, distanza {5:F0} km, elevazione {6:F1}°",
                    Lang.T(h.Label), Astro.ToLocator(h.Pos), h.BearingFromHome, h.SideScatterDeg, h.BearingFromDx, h.DistFromHome, h.ElevFromHome));
            }
            sb.Append("\n");
            head(Lang.T("LIMITI DELLA ZONA COMUNE (incroci delle curve dove l'orizzonte di ciascuna stazione incontra lo strato E, articolo)"));
            if (PathModel.LobePattern)
                line(Lang.F("Portata alla quota delle scie fino all'orizzonte: {0:F0} km. Lobo d'antenna con il massimo a {1:F1}° e larghezza verticale {2:F0}° a -3 dB: i punti fuori dal lobo pesano meno, quelli sotto il massimo scendono a zero all'orizzonte",
                    model.HotRadiusKm, model.MinElevDeg, PathModel.VerticalHpbwDeg));
            else line(Lang.F("Portata alla quota delle scie con lobo a {0:F1}°: {1:F0} km", model.MinElevDeg, model.HotRadiusKm));
            if (model.LimitA != null)
            {
                foreach (HotPoint h in new HotPoint[] { model.LimitA, model.LimitB })
                {
                    if (h == null) continue;
                    line(Lang.F("{0}: {1}  az. da me {2:F0}° (scarto {3:+0;-0}°), dal corrispondente {4:F0}°, distanza {5:F0} km",
                        Lang.T(h.Label), Astro.ToLocator(h.Pos), h.BearingFromHome, h.SideScatterDeg, h.BearingFromDx, h.DistFromHome));
                }
                line(Lang.T("Sono i bordi piu' lontani del cielo visto da entrambe le stazioni, non i punti da puntare."));
            }
            else line(model.Message);
            line(Lang.F("Antenna Yagi: boom {0:F2} m = {1:F2} lambda a {2:F0} MHz -> guadagno {3:F1} dBi (curva DL6WU),\n" +
                        "  lobo a -3 dB {4:F0}° orizzontale / {5:F0}° verticale (Kraus corretto con le simulazioni NEC di W4RNL)",
                V(numBoom), V(numBoom) * f / 299.792458, f, antGainDbi, antHpbw, antHpbwV));
            if (model.HotA != null && model.HotB != null)
                line(Lang.F("Puntamento {0:F0}°: Hot A {1}, Hot B {2}", sky.AntennaAz,
                    model.HotA.InLobe ? Lang.T("DENTRO il lobo") : Lang.T("fuori dal lobo"), model.HotB.InLobe ? Lang.T("DENTRO il lobo") : Lang.T("fuori dal lobo")));
            sb.Append("\n");
            head(Lang.T("ATTENUAZIONE DI TRATTA (scia sottodensa, McKinley 1961 e ITU-R P.843; guadagni d'antenna esclusi)"));
            line(Lang.F("Densita' della scia q = {0:E1} elettroni/m. Riferimenti: 172-190 dB (calcoli di G4BAO citati da GM8JBJ), 200 dB medio a 144 MHz (OH5IY)", q));
            line(Lang.T("Valori prudenti: scia che attraversa il piano della tratta (β = 90°, caso più sfavorevole), compreso il raggio iniziale della scia (ITU-R P.843) alla quota media degli echi per ciascuna frequenza"));
            line(Lang.T("Punto di riflessione").PadRight(30) + "28 MHz   50 MHz   70 MHz  144 MHz  222 MHz  432 MHz");
            List<KeyValuePair<string, LatLon>> pts = new List<KeyValuePair<string, LatLon>>();
            pts.Add(new KeyValuePair<string, LatLon>(Lang.T("Punto medio (forward)"), model.Mid));
            if (model.HotA != null && model.HotB != null) { pts.Add(new KeyValuePair<string, LatLon>(Lang.T("Hot A"), model.HotA.Pos)); pts.Add(new KeyValuePair<string, LatLon>(Lang.T("Hot B"), model.HotB.Pos)); }
            foreach (KeyValuePair<string, LatLon> kv in pts)
            {
                sb.Append(kv.Key.PadRight(28));
                foreach (double fb in new double[] { 28, 50, 70, 144, 222, 432 })
                {
                    double Ldb = MsGeometry.PathLossDb(model.Home, model.HomeAltKm, model.Dx, model.DxAltKm, kv.Value, model.LayerKm, fb, q, 90, true);
                    sb.Append(Ldb.ToString("F1").PadLeft(9));
                }
                sb.Append("\n");
            }
            // valore prudente: beta = 90 gradi (scia che attraversa il piano della tratta); il caso migliore (beta = 0) si mostra a parte
            double lossMid = MsGeometry.PathLossDb(model.Home, model.HomeAltKm, model.Dx, model.DxAltKm, model.Mid, model.LayerKm, f, q, 90, true);
            double loss0 = MsGeometry.PathLossDb(model.Home, model.HomeAltKm, model.Dx, model.DxAltKm, model.Mid, model.LayerKm, f, q, 0, true);
            double phiMid = model.HalfScatterAngleDeg(model.Mid);
            double hItu = MsGeometry.ItuMeanHeightKm(f);
            double lossR0 = MsGeometry.InitialRadiusLossDb(hItu, f, phiMid);
            double gain = V(numGain), sens = V(numSens), power = V(numPower);
            double pdbm = 10 * Math.Log10(power * 1000.0);
            double eirp = pdbm + gain;   // guadagno in dBi: la perdita di McKinley e' riferita ad antenne isotrope
            double rx = eirp - lossMid + gain;
            sb.Append("\n");
            head(Lang.F("BILANCIO A {0:F0} MHz (punto medio)", f));
            line(Lang.F("Potenza {0:F0} W = {1:F1} dBm, EIRP = {2:F1} dBm (guadagno in dBi)", power, pdbm, eirp));
            line(Lang.F("Attenuazione {0:F1} dB -> segnale in antenna {1:F1} dBm, minimo ricevibile {2:F0} dBm, margine {3:+0.0;-0.0} dB",
                lossMid, rx, sens, rx - sens));
            line(Lang.F("Nell'attenuazione (β = 90°, prudente): raggio iniziale della scia {0:F1} dB (r0 = {1:F2} m alla quota media ITU di {2:F1} km per {3:F0} MHz). Con la scia nel piano della tratta (β = 0°, caso migliore) l'attenuazione scende a {4:F1} dB ({5:F1} dB) e il margine sale a {6:+0.0;-0.0} dB",
                lossR0, MsGeometry.InitialRadiusM(hItu), hItu, f, loss0, loss0 - lossMid, eirp - loss0 + gain - sens));
            if (MsGeometry.InitialRadiusFactor != 1)
            {
                double rItu = MsGeometry.InitialRadiusItuM(hItu), lItu = 10 * Math.Log10(Math.E) * 8 * Math.PI * Math.PI * rItu * rItu * Math.Pow(Math.Cos(Math.Min(phiMid, 85) * Astro.D2R), 2) / Math.Pow(299.792458 / f, 2);
                line(Lang.F("Raggio iniziale: con il valore medio ITU-R P.843 (r0 = {0:F2} m) il fattore varrebbe {1:F1} dB; scelta attuale {2}. Misure CMOR: r0 di 0,5-2 m tra 85 e 95 km, dispersione di un fattore due (Stober et al. 2021)",
                    rItu, lItu, MsGeometry.InitialRadiusFactor == 2 ? Lang.T("doppio dell'ITU (Stober 2021, prudente)") : Lang.T("escluso")));
            }
            line(Lang.F("Con l'attenuazione media di OH5IY (200 dB a 144 MHz, scalata con λ³: {0:F1} dB a {1:F0} MHz) il margine sarebbe {2:+0.0;-0.0} dB",
                200 + 30 * Math.Log10(f / 144.0), f, eirp - (200 + 30 * Math.Log10(f / 144.0)) + gain - sens));
            sb.Append("\n");
            head(Lang.T("REGOLE SPERIMENTALI TRA LE BANDE (SM6FHZ, VUSHF 2025)"));
            line(Lang.F("Rispetto a 144 MHz: numero/intensità x {0:F3} ({1:+0.0;-0.0} dB), durata x {2:F2}. Ping massimo IARU a {3:F0} MHz: {4:F0} ms",
                MsGeometry.CountScale(144, f, 3), 10 * Math.Log10(MsGeometry.CountScale(144, f, 3)), Math.Pow(144 / f, 2), f, MsGeometry.PingMaxMs(f)));
            line(Lang.F("Durata osservata (HamSCI MSQP, NN4SA, Perseidi 2026; mediane 148 / 78 / 53 ms a 28 / 50 / 144 MHz): circa {0:F0} ms a {1:F0} MHz, x {2:F2} rispetto a 144 MHz{3}",
                MsGeometry.ObservedDurationMs(f), f, MsGeometry.ObservedDurationMs(f) / MsGeometry.ObservedDurationMs(144),
                f > 144.15 ? " " + Lang.T("(sopra 144 MHz nessuna misura: λ²)") : ""));
            StyleReport(txtGeo, sb.ToString(), heads);

            // curve dell'elevazione (come Immagine 8 dell'articolo)
            chartElev.Series.Clear();
            int n = 27; string[] xl = new string[n];
            double[] hs = { model.LayerKm, 100, 120 };
            Color[] cs = { Pal.Ink(Pal.Rgb(255, 210, 40)), Pal.Ink(Pal.Rgb(0, 200, 255)), Pal.Ink(Pal.Rgb(240, 110, 230)) };
            for (int k = 0; k < 3; k++)
            {
                double[] v = new double[n];
                for (int i = 0; i < n; i++) { double d = i * 50; v[i] = d == 0 ? 90 : MsGeometry.ElevationArticle(d, hs[k], model.HomeAltKm); if (v[i] < 0) v[i] = -1; xl[i] = (i * 50).ToString(); }
                ChartSeries s = new ChartSeries(); s.Name = "H = " + hs[k].ToString("F1") + " km"; s.Color = cs[k]; s.Values = v;
                chartElev.Series.Add(s);
            }
            chartElev.XLabels = xl; chartElev.XLabelEvery = 4; chartElev.YMaxFixed = 90;
            chartElev.Title = Lang.T("Elevazione d'antenna in funzione della distanza dallo scatter point");
            chartElev.YLabel = Lang.T("gradi"); chartElev.InvalidateVisual();
        }

        void UpdateShowerGrid()
        {
            if (gridShowers == null) return;
            List<TableRow> rows = new List<TableRow>();
            DateTime now = DateTime.UtcNow;
            foreach (Shower s in Catalog.Showers)
            {
                string peak = Lang.T("varie (tab. 6 IMO)"), src = "IMO";
                if (!s.IsAntihelion) s.NextPeak(now, out peak, out src);
                string[] cells = {
                    s.Code, Lang.T(s.Name),
                    string.Format("{0:00}/{1:00} - {2:00}/{3:00}", s.StartDay, s.StartMonth, s.EndDay, s.EndMonth),
                    s.IsAntihelion ? "-" : s.LambdaMax.ToString("F2"), peak ?? "", src ?? "",
                    s.IsAntihelion ? Lang.T("mobile") : s.Ra.ToString("F0") + "°", s.IsAntihelion ? Lang.T("mobile") : s.Dec.ToString("+0;-0") + "°",
                    s.V.ToString("F0"), s.PopIndex.ToString("F1"), s.Zhr > 0 ? s.Zhr.ToString("F0") : Lang.T("var."),
                    s.Type == "R" ? Lang.T("radio, diurno") : Lang.T("visuale"), "", "", "", "", "", "" };
                string[] tips = new string[cells.Length];
                double rG; bool gmnR = GmnPopIndex.R.TryGetValue(s.Code, out rG);
                double kk = MsGeometry.KFromPopIndex(gmnR ? rG : s.PopIndex);
                cells[12] = double.IsNaN(kk) ? "-" : kk.ToString("F2") + (gmnR ? " G" : "");
                tips[12] = Lang.F("k = 3,75·log10 r = {0}, con r = {1} ({2}). La regola sperimentale di SM6FHZ usa k = 3, più prudente.",
                    double.IsNaN(kk) ? "-" : kk.ToString("F2"), (gmnR ? rG : s.PopIndex).ToString("F2"), gmnR ? "GMN" : "IMO");
                IauMdc.Comparison mc = IauMdc.Compare(s);
                if (mc != null)
                {
                    cells[13] = mc.N.ToString();
                    cells[14] = double.IsNaN(mc.SepMed) ? "-" : mc.SepMed.ToString("F1") + "°";
                    cells[15] = double.IsNaN(mc.DLambda) ? "-" : mc.DLambda.ToString("+0.0;-0.0");
                    cells[16] = double.IsNaN(mc.DV) ? "-" : mc.DV.ToString("+0.0;-0.0");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine(Lang.F("IAU MDC ({0}): {1} soluzioni. Mediane: λ {2:F1}°, A.R. {3:F1}°, decl. {4:+0.0;-0.0}°, v {5:F1} km/s.",
                        IauMdc.Edition, mc.N, mc.LoSMed, mc.RaMed, mc.DeMed, mc.VgMed));
                    sb.AppendLine(Lang.F("Distanza tra il radiante del programma e quelli MDC: mediana {0:F1}°, massima {1:F1}°.", mc.SepMed, mc.SepMax));
                    int shown = 0;
                    foreach (MdcSolution x in IauMdc.ByCode[s.Code])
                    {
                        if (++shown > 12) { sb.AppendLine("…"); break; }
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}  λ {1:F1}  {2:F1}/{3:+0.0;-0.0}  v {4:F1}  {5}", x.AdNo, x.LoS, x.Ra, x.De, x.Vg, x.Reference));
                    }
                    tips[13] = tips[14] = tips[15] = tips[16] = sb.ToString().TrimEnd();
                }
                int gc;
                if (GmnDaily.Total > 0)
                {
                    cells[17] = GmnDaily.Counts.TryGetValue(s.Code, out gc) ? gc.ToString() : "0";
                    tips[17] = Lang.F("Global Meteor Network: {0} traiettorie confermate dal {1} al {2} UTC, di cui {3} sporadiche. Conteggio delle telecamere, non flusso. Dati CC BY 4.0.",
                        GmnDaily.Total, GmnDaily.FirstUtc.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture), GmnDaily.LastUtc.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture), GmnDaily.Sporadic);
                }
                TableRow tr = new TableRow { Cells = cells, Tag = s.Code, CellTips = tips };
                // velatura sugli sciami attivi oggi (al posto della vecchia colonna "Attivo oggi")
                if (s.IsActive(now)) { tr.Back = Pal.A(70, Pal.Yellow); tr.Bold = true; }
                rows.Add(tr);
            }
            gridShowers.Rows = rows;
            gridShowers.Refresh();
            // la tabella si apre sul primo sciame attivo oggi (in autunno e in inverno sarebbero fuori vista)
            int firstActive = rows.FindIndex(r => r.Back.HasValue);
            if (firstActive > 0) gridShowers.ScrollToRow(Math.Max(0, firstActive - 1));
        }

        // ------------------------------------------------------------ radar
        void LoadCachedRadar()
        {
            if (collmSrc != null && File.Exists(collmSrc.LocalFile))
            {
                collmSrc.LastModifiedUtc = File.GetLastWriteTimeUtc(collmSrc.LocalFile);
                collmSrc.Status = Lang.F("copia locale del {0} UTC", collmSrc.LastModifiedUtc.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture));
                try { collm = RadarFlux.ParseCollm(collmSrc.LocalFile, collmSrc.LastModifiedUtc); } catch { collm = null; }
                ShowImage(picCollm, collmSrc.LocalFile);
                ApplyRadarLayer();
            }
            if (sodaSrc != null && File.Exists(sodaSrc.LocalFile)) ShowImage(picSoda, sodaSrc.LocalFile);
            UpdateFluxTab();
        }

        static void ShowImage(Image pb, string file)
        {
            try
            {
                byte[] b = File.ReadAllBytes(file);
                using (MemoryStream ms = new MemoryStream(b))
                    pb.Source = new Bitmap(ms);
            }
            catch { }
        }

        void StartFetch() { StartFetch(false); }

        // Controlla tutte le fonti esterne (radar e, se attivo, RMOB). Ogni fonte viene riscaricata solo se cambiata.
        // Se tutte le richieste falliscono per mancanza di rete compare un avviso (una sola volta finche' la rete non torna).
        void StartFetch(bool forceRmob)
        {
            if (fetching) return;
            StartLiveFetch(forceRmob);   // GMN e CMOR: al massimo ogni 30 minuti, o subito con "Scarica adesso"
            fetching = true;
            lastFetchUtc = DateTime.UtcNow;
            btnFetch.IsEnabled = false;
            status.Text = Lang.T("Controllo le fonti di dati esterne...");
            string obs = (txtRmobObs.Text ?? "").Trim();
            bool doRmob = (forceRmob || chkRmobAuto.IsChecked == true) && obs.Length > 0;
            string rmobUrl = doRmob ? RmobUrl(DateTime.UtcNow) : null;
            string rmobFile = doRmob ? RmobFile(DateTime.UtcNow) : null;
            bool haveCollm = collm != null;
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                string msg = "";
                CollmData parsed = null;
                int attempts = 0, offlineCount = 0;
                Dictionary<DateTime, double> rm = null;
                try
                {
                    foreach (RadarSource s in sources)
                    {
                        if (!s.Active) continue;
                        attempts++;
                        int r = RadarFlux.Download(s);
                        if (r < 0 && s.Offline) offlineCount++;
                        msg += s.Name.Split(' ')[0] + ": " + s.Status + "   ";
                        if (s == collmSrc && (r == 1 || (r == 0 && !haveCollm)))
                        {
                            try
                            {
                                parsed = RadarFlux.ParseCollm(s.LocalFile, s.LastModifiedUtc);
                                RadarFlux.Archive(dataDir, parsed, s.FreqMHz);
                            }
                            catch (Exception ex) { msg += Lang.F("(lettura Collm: {0})", ex.Message) + " "; }
                        }
                    }
                    if (doRmob)
                    {
                        attempts++;
                        bool off; string err; string rmMsg = "";
                        int r = RadarFlux.Fetch(rmobUrl, rmobFile, ref rmobLastMod, out off, out err);
                        if (r < 0 && off) offlineCount++;
                        if (r >= 0 && File.Exists(rmobFile))
                        {
                            try { rm = ParseRmob(rmobFile, out rmMsg); } catch (Exception ex) { rmMsg = ex.Message; }
                        }
                        msg += "RMOB: " + (r == 1 ? Lang.T("nuovo dato") : r == 0 ? Lang.T("invariato") : Lang.F("errore {0}", err));
                    }
                }
                catch (Exception ex) { msg += Lang.F("errore: {0}", ex.Message); }
                bool offline = attempts > 0 && offlineCount == attempts;
                Dispatcher.UIThread.Post(() => FetchDone(parsed, rm, obs, offline, msg));
            });
        }

        // Fine dello scaricamento, sul thread dell'interfaccia
        void FetchDone(CollmData parsed, Dictionary<DateTime, double> rm, string obs, bool offline, string msg)
        {
            fetching = false; btnFetch.IsEnabled = true;
            if (replaced) return;   // finestra gia' sostituita (cambio di lingua): la nuova scarica da se'
            if (parsed != null) collm = parsed;
            if (rm != null && rm.Count > 0)
            {
                rmobCounts = rm; rmobName = obs;
                lblRmob.Text = Lang.F("RMOB {0}: {1} ore lette alle {2} UTC", rmobName, rm.Count, DateTime.UtcNow.ToString("HH:mm", CultureInfo.InvariantCulture));
            }
            if (collmSrc != null && File.Exists(collmSrc.LocalFile)) ShowImage(picCollm, collmSrc.LocalFile);
            if (sodaSrc != null && File.Exists(sodaSrc.LocalFile)) ShowImage(picSoda, sodaSrc.LocalFile);
            if (offline)
            {
                status.Text = Lang.T("CONNESSIONE INTERNET ASSENTE: dati esterni non aggiornati (nuovo tentativo tra 5 minuti)");
                status.Foreground = Pal.B(Tema.Scuro ? Pal.Red : SkyPal.Red);   // sulla barra scura
                if (!offlineNotified)
                {
                    offlineNotified = true;
                    _ = Dialogs.Message(Win, Lang.T("YA-MS - nessuna connessione"),
                        Lang.T("Connessione internet assente.\n\nI dati dei radar e di RMOB non possono essere aggiornati: il programma usa gli ultimi dati salvati e riprova da solo ogni 5 minuti."));
                }
                return;
            }
            status.Foreground = Pal.B(Pal.BarMuted);
            status.Text = (offlineNotified ? Lang.T("Connessione ripristinata.") + " " : "") + msg;
            offlineNotified = false;
            bool changed = ApplyRadarLayer();
            UpdateFluxTab();
            if (changed) Recalc(); else { UpdateGeometry(); Update24h(); }
        }

        // usa la quota piu' frequente misurata dal radar; true se cambiata
        bool ApplyRadarLayer()
        {
            if (collm == null || chkLayerRadar.IsChecked != true || collm.HeightTotal <= 0) return false;
            decimal v = Dm(Math.Round(collm.HeightMode * 2) / 2.0);
            v = Math.Max(numLayer.Minimum, Math.Min(numLayer.Maximum, v));
            if (numLayer.Value == v) return false;
            numLayer.Value = v;
            return true;
        }

        // ore locali (fuso orario del computer, con l'ora legale) delle ore UTC che iniziano a startUtc
        // fumetto delle celle della tabella oraria: quale hot spot (A o B) e' indicato dalla lettera e dove puntare
        string HotTip(Shower s, DateTime tUtc, double eff, double lf)
        {
            bool left = lf >= 0.5;
            HotPoint hp = left ? model.HotA : model.HotB;
            string loc = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(tUtc, DateTimeKind.Utc), TimeZoneInfo.Local).ToString("HH:mm", CultureInfo.InvariantCulture);
            string head = Lang.F("{0} {1} - {2:F0} % alle {3} UTC ({4} ora locale)", s.Code, Lang.T(s.Name), eff,
                tUtc.ToString("HH:mm", CultureInfo.InvariantCulture), loc);
            string side = left ? Lang.T("Hot A") + " - " + Lang.T("a SINISTRA guardando verso il corrispondente")
                               : Lang.T("Hot B") + " - " + Lang.T("a DESTRA guardando verso il corrispondente");
            string pct = Lang.F("{0:F0} % delle riflessioni arriva da questo lato", 100 * Math.Max(lf, 1 - lf));
            if (hp == null) return head + "\n" + side + "\n" + pct;
            string where = Lang.F("puntare a {0:F0}° ({1:+0;-0}° dal corrispondente), elevazione {2:F1}°, hot spot a {3:F0} km",
                hp.BearingFromHome, Astro.Norm180(hp.BearingFromHome - model.BearingHomeToDx), hp.ElevFromHome, hp.DistFromHome);
            return head + "\n" + side + "\n" + pct + "\n" + where;
        }

        static string[] LocalHours(DateTime startUtc, int n)
        {
            string[] r = new string[n];
            DateTime s = DateTime.SpecifyKind(startUtc, DateTimeKind.Utc);
            for (int i = 0; i < n; i++)
                r[i] = TimeZoneInfo.ConvertTimeFromUtc(s.AddHours(i), TimeZoneInfo.Local).ToString("HH:mm", CultureInfo.InvariantCulture).Replace(":00", "");
            return r;
        }

        static void SetLocalRow(SimpleChart c, DateTime startUtc, int n)
        {
            c.XLabels2 = LocalHours(startUtc, n);
            c.XRow1Name = "UTC";
            c.XRow2Name = Lang.T("locale");
        }

        void UpdateFluxTab()
        {
            if (chartFlux == null) return;
            double f = BandMHz, k = V(numExp);
            double fr = collmSrc != null && collmSrc.FreqMHz > 0 ? collmSrc.FreqMHz : 36.2;
            lblRadar.Text = (collmSrc != null ? "Collm: " + collmSrc.Status : Lang.T("Nessuna fonte Collm in fonti_radar.csv"))
                + (collm != null && collm.ScaleNote.Length > 0 ? " - " + collm.ScaleNote : "");
            double scale = MsGeometry.CountScale(fr, f, k);
            lblScaleNote.Text = Lang.F(
                "Stima sperimentale (regole SM6FHZ): meteore/ora a {0:F0} MHz = Collm {1:F1} MHz x ({1:F1}/{0:F0})^{2:F1} = x {3:F4}; durata dei ping x {4:F3}. " +
                "Ping massimo IARU a {0:F0} MHz: {5:F0} ms. Valori indicativi, da confrontare con l'esperienza in banda.{6}",
                f, fr, k, scale, MsGeometry.DurationScale(fr, f), MsGeometry.PingMaxMs(f),
                collm != null && !string.IsNullOrEmpty(collm.Problem) ? "\n" + Lang.F("ATTENZIONE: {0}", collm.Problem) : "");
            chartFlux.Series.Clear(); chartHeight.Series.Clear();
            if (collm == null)
            {
                chartFlux.Title = Lang.T("Flusso radar: nessun dato. Premi \"Scarica adesso\".");
                chartFlux.InvalidateVisual(); chartHeight.InvalidateVisual(); UpdatePrevTab(); return;
            }
            string[] xl = new string[48];
            for (int i = 0; i < 48; i++) xl[i] = (i % 24).ToString("00");
            double[] est = new double[48];
            for (int i = 0; i < 48; i++) est[i] = collm.Valid[i] < 0 ? -1 : collm.Valid[i] * scale;
            ChartSeries a = new ChartSeries(); a.Name = Lang.F("Collm {0} MHz (validi)", fr.ToString("F1")); a.Color = Pal.Ink(Pal.Rgb(0, 230, 255)); a.Values = collm.Valid; a.Bars = true;
            ChartSeries r = new ChartSeries(); r.Name = Lang.T("scartati"); r.Color = Pal.Ink(Pal.Rgb(230, 40, 40)); r.Values = collm.Rejected; r.Bars = true;
            ChartSeries b = new ChartSeries(); b.Name = Lang.F("stima a {0} MHz (scala a destra)", f.ToString("F0")); b.Color = Pal.Ink(Pal.Rgb(255, 170, 40)); b.Values = est; b.RightAxis = true; b.LineWidth = 6f;   // spessore triplo
            chartFlux.Series.Add(a); chartFlux.Series.Add(r); chartFlux.Series.Add(b);
            chartFlux.XLabels = xl; chartFlux.XLabelEvery = 3;
            SetLocalRow(chartFlux, collm.DayToday.AddDays(-1), 48);
            chartFlux.Title = Lang.F("Meteore/ora - ieri {0} e oggi {1} (UTC)", collm.DayToday.AddDays(-1).ToString("dd/MM", CultureInfo.InvariantCulture), collm.DayToday.ToString("dd/MM", CultureInfo.InvariantCulture));
            chartFlux.YLabel = Lang.T("meteore/ora radar"); chartFlux.Y2Label = Lang.T("meteore/ora stimate");
            int nowIdx = (int)Math.Floor((DateTime.UtcNow - collm.DayToday.AddDays(-1)).TotalHours);
            chartFlux.MarkIndex = nowIdx >= 0 && nowIdx < 48 ? nowIdx : -1;
            chartFlux.InvalidateVisual();

            string[] hl = new string[40]; for (int i = 0; i < 40; i++) hl[i] = (70 + i).ToString();
            ChartSeries h = new ChartSeries(); h.Name = Lang.T("meteore per km di quota"); h.Color = Pal.Ink(Pal.Rgb(255, 230, 0)); h.Values = collm.HeightCounts; h.Bars = true;
            chartHeight.Series.Add(h); chartHeight.XLabels = hl; chartHeight.XLabelEvery = 5;
            chartHeight.Title = Lang.F("Quote delle scie (Collm): più frequente {0:F1} km, media {1:F1}, mediana {2:F1}, dev. std {3:F1} km",
                collm.HeightMode, collm.HeightMean, collm.HeightMedian, collm.HeightSd);
            chartHeight.YLabel = Lang.T("meteore"); chartHeight.InvalidateVisual();
            UpdatePrevTab();
        }

        // ------------------------------------------------------------ orologio e aggiornamento automatico
        void OnTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.UtcNow;
            lblClock.Text = now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + " UTC";
            UpdateGravesTick();   // fascio di GRAVES: passo attivo e prossimi passaggi
            RefreshCollect(false);   // raccolta dati: righe telnet nuove, contatori, tabella
            if (now.Second == 0)
            {
                if (IsRealTime) UpdateSky();
                else if (now.Minute % 15 == 0) UpdateSky();   // la data scelta segue l'ora attuale
                // a mezzanotte la data del calcolo segue il giorno nuovo, se era quella di oggi
                if (now.Minute == 0 && (dtCalc.SelectedDate ?? now).Date == now.AddMinutes(-1).Date) dtCalc.SelectedDate = now.Date;
                if (now.Minute == 0 && now.Hour == 0) UpdateShowerGrid();   // nuovo giorno: aggiorna i prossimi massimi
            }
            if (chkAuto.IsChecked == true && !fetching && (now - lastFetchUtc).TotalMinutes >= 5)
                StartFetch();   // ogni 5 minuti: ogni fonte viene riscaricata solo se e' cambiata
        }

        void SaveConfig()
        {
            SaveCollectConfig();
            if (traffic != null) { cfg.Set("traffico_raggio", traffic.RadiusKm); cfg.Set("traffico_minuti", TrafficMinutes[Math.Max(0, cmbTrafficWindow.SelectedIndex)]); }
            cfg.Set("mio_locator", Up(txtHome));
            cfg.Set("mia_quota_m", V(numHomeAlt));
            cfg.Set("dx_locator", Up(txtDx));
            cfg.Set("dx_quota_m", V(numDxAlt));
            cfg.Set("banda_MHz", (cmbBand.SelectedItem as string) ?? "144");
            cfg.Set("quota_scie_km", V(numLayer));
            cfg.Set("quota_da_radar", chkLayerRadar.IsChecked == true);
            cfg.Set("elevazione_lobo_gradi", V(numElev));
            cfg.Set("aggiornamento_automatico", chkAuto.IsChecked == true);
            cfg.Set("esponente_lambda", V(numExp));
            cfg.Set("potenza_W", V(numPower));
            cfg.Set("guadagno_dBi", V(numGain));
            cfg.Set("sensibilita_dBm", V(numSens));
            cfg.Set("densita_esponente_q", V(numQ));
            cfg.Set("mostra_mappa", chkMap.IsChecked == true);
            cfg.Set("vista_alto", chkTopView.IsChecked == true);
            cfg.Set("boom_m", V(numBoom));
            cfg.Set("hot_spot_km", V(numHotKm));
            cfg.Set("hot_spot_auto", chkHotAuto.IsChecked == true);
            if (chkTrailLen != null) cfg.Set("lunghezza_scie", chkTrailLen.IsChecked == true);
            if (chkBeta != null) cfg.Set("angolo_beta", chkBeta.IsChecked == true);
            if (chkLobe != null) cfg.Set("lobo_diagramma", chkLobe.IsChecked == true);
            cfg.Set("carattere_pct", Math.Round(sldFont.Value / 10) * 10);
            cfg.Set("lingua", Lang.Code);
            cfg.Set("tema", Tema.Chiave);
            SavePrevConfig();
            SaveGravesConfig();
            cfg.Save();
        }

        static string SourcesText()
        {
            return
Lang.T("FONTE PRINCIPALE") + "\n" +
"  " + Lang.T("Costantino Cerrotta IC8TEM, Gianluigi \"Gigi\" Sartori IV3GTH - \"METEOR-SCATTER. Ovvero come collegare l'Europa in VHF tutto l'anno senza tante pretese\" (articolo, versione 7).") + "\n" +
"  " + Lang.T("Da qui: scie nello strato E a circa 90 km (radar SKiYMET), formula dell'elevazione, Hot A / Hot B, side-scatter, attenuazione di tratta, durata dei ping IARU.") + "\n\n" +
Lang.T("CALCOLO E TEORIA DELL'EFFICIENZA") + "\n" +
"  " + Lang.T("Ilkka Yrjola OH5IY - MS-Soft (concetto di efficienza dello sciame), www.kolumbus.fi/oh5iy (sito storico)") + "\n" +
"  Wilhelm Rauh DL5MCG - MCG_MS, MS Efficiency Calculator, qsl.net/dl5mcg\n" +
"  " + Lang.T("Ingolf SM6FHZ - \"432 MHz Meteorscatter, Challenges and Techniques\", 45th Nordic VUSHF, maggio 2025 (regole lambda^3 e lambda^2, punto medio, direzione migliore)") + "\n" +
"  " + Lang.T("D.W.R. McKinley - Meteor Science and Engineering, 1961 (equazione delle scie sottodense)") + "\n" +
"  " + Lang.T("John GM8JBJ - Meteor scatter path loss, hamradio.engineering (da Kaiser & Closs 1952; cita i calcoli di John Worsnop G4BAO, 172-190 dB)") + "\n" +
"  " + Lang.T("Raccomandazione ITU-R P.843-1 (1997): angolo beta tra scia e piano della tratta, raggio iniziale della scia, quota media in funzione della frequenza") + "\n" +
"  G. Stober, P. Brown, M. Campbell-Brown, R.J. Weryk - \"Triple-frequency meteor radar full wave scattering\", A&A 2021, arXiv:2108.07647\n" +
"  J.A. Weitzen, W.T. Ralston - \"Meteor Scatter: An Overview\", IEEE Trans. Antennas Propag. 36(12), 1813-1819, 1988\n" +
"  B. Xia, L. Li, Y. Zheng - \"Modeling and Simulation of the Meteor Burst Channel\", Telecommunications Science 2016 (9)\n" +
"  Rob Suggs NN4NT, HamSCI Meteor Scatter Working Group - MSQP, NN4SA 2026 Perseids, 4 Nov 2026, github.com/HamSCI/MSQP\n" +
"  " + Lang.T("Shelby W8WN - qsl.net/w8wn/hscw (elevazione e offset ottimali)") + "\n" +
"  " + Lang.T("Klaus von der Heide DJ5HG - QTFGUI (aurora, FAI e meteor scatter) e manuale \"QTFGUI Help\" 2007: legge di diffusione a cono,\n" +
"    caso di prova Amburgo - Barcellona nelle Quadrantidi, usato per verificare il modello") + "\n" +
"  " + Lang.T("Klaus von der Heide DJ5HG - \"Die Geometrie von Meteorscatterverbindungen\" (2004): riferimento fisso in dB, scie lunghe con il radiante basso,\n" +
"    curve delle tratte da Amburgo e da Monaco; tabella AMS di J. Richardson per la distanza degli hot spot") + "\n" +
"  " + Lang.T("Jens DF5HC - \"Meteorscatter auf UKW\", conferenza DARC, gennaio 2018: formula della migliore direzione\n" +
"    di DH9GCD (Sin(El) x Cos(El) x Sin(P-Az)), diffusione a cono e scelta HOT-A / HOT-B di DJ5HG,\n" +
"    radiante utile tra 20 e 70 gradi, distanza migliore 1300-1500 km, meteore sporadiche di KB0VUK") + "\n" +
"  " + Lang.T("Gunter Hoch DL6WU - curva guadagno/lunghezza del boom delle Yagi lunghe, G(dBi) = 7,773 log(L/lambda) + 11,43, riportata da W8IO (w8io.com)") + "\n" +
"  " + Lang.T("J.D. Kraus - Antennas: guadagno e apertura del lobo, G = 41253 / (thetaE x thetaH)") + "\n" +
"  " + Lang.T("L.B. Cebik W4RNL - Long-Boom Yagi Rules of Thumb: A Comparison with Modeling Data (verifica delle due regole);\n" +
"    Appreciating DL6WU Wide-Band Long-Boom Yagi Design e 70-CM Yagi Stacks Part 2 (Yagi VK3AUU di David Tanner):\n" +
"    simulazioni NEC usate per verificare la curva del guadagno e correggere il lobo orizzontale (0,93 x Kraus)") + "\n" +
"  " + Lang.T("John Drew VK5DJ - Yagi Calculator: stime del guadagno per le varianti VK3AUU, WA2PHW e DJ9BV") + "\n\n" +
Lang.T("DATI DEGLI SCIAMI") + "\n" +
"  " + Lang.T("International Meteor Organization - Meteor Shower Calendar 2026, www.imo.net (tabelle 5, 6 e 7)") + "\n" +
"  Rossano IZ5TEK - www.aripisa.it/images/pdf/scattertek.pdf\n\n" +
Lang.T("DATI RADAR IN DIRETTA") + "\n" +
"  " + Lang.T("Radar meteorico di Collm, Universita' di Lipsia (LIM) e IAP Kuehlungsborn - wetterdaten.meteo.uni-leipzig.de/radar_flux.php") + "\n" +
"  Sodankyla Geophysical Observatory - sgo.fi/Data/RealTime/meteorRadar.php\n" +
"  " + Lang.T("Global Meteor Network (GMN) - globalmeteornetwork.org/flux: flusso e ZHR misurati degli sciami. Dati CC BY 4.0; Vida et al. 2021, MNRAS 506(4), e Vida et al. 2022, MNRAS 515(2)") + "\n" +
"  " + Lang.T("CMOR, Canadian Meteor Orbit Radar (University of Western Ontario, NASA Meteoroid Environment Office) - fireballs.ndc.nasa.gov/cmor-radiants: mappe dei radianti") + "\n" +
"  " + Lang.T("Swan Meteor Skyview di DL1DBC - swan.ms (riferimento per la grafica della volta celeste)") + "\n" +
"  " + Lang.T("Ingolf SM6FHZ - RadiantCalc (grafico polare con la direzione migliore)") + "\n" +
"  " + Lang.T("Natural Earth - coste e confini 1:50 milioni, dominio pubblico, www.naturalearthdata.com (mappa d'Europa)") + "\n" +
"  " + Lang.T("RMOB, Radio Meteor Observatories On Line (Pierre Terrier) - www.rmob.org; bollettini mensili rmob.org/rmobtext (osservatori GRAVES 143,050 MHz: Planetario di Venezia, Faenza, Rovereto e altri)") + "\n\n" +
Lang.T("RADAR GRAVES (linguetta Radar GRAVES)") + "\n" +
"  " + Lang.T("Rob Hardenberg PE1ITR, \"The 143.050 MHz Graves Radar, a VHF Beacon\" (DUBUS 4/2007) e \"Graves Radar Update April 2021\" – pe1itr.com/graves") + "\n" +
"  " + Lang.T("Bolidozor wiki, \"Graves\" – schiere, settori e tempi del fascio") + "\n" +
"  " + Lang.T("Wikipedia, \"GRAVES (system)\" – trasmettitore a Broye-lès-Pesmes (47,348° N 5,515° E)") + "\n\n" +
Lang.T("ALTRI RIFERIMENTI DELL'ARTICOLO") + "\n" +
"  " + Lang.T("G3WZT, DH8BQA, geekshed.co.uk, manuale WSJT-X, WB2FKO (pings), IU6GIR (attivita' 143,050 MHz), IARU Regione 1 VHF Handbook") + "\n\n" +
Lang.T("Nota: i dati dei radar appartengono ai rispettivi enti. Prima di distribuire il programma va chiesto il permesso di prelevarli automaticamente.") + "\n";
        }
    }
}
