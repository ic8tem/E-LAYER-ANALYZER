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
        TextBlock lblAntAz, lblAnt;
        TextBlock lblLobeWarn, lblHighRad, lblAB;
        Border boxLobeWarn, boxHighRad;
        double antGainDbi, antHpbw, antHpbwV;   // lobo orizzontale (E) e verticale (H)
        string recommendText = "";
        string recommendShort = "";   // consiglio senza la parte tra parentesi (riquadro "Adesso")
        TextBlock glBest, glShower, glNow, glFore;
        CheckBox chkHot, chkMap, chkTopView;
        Button btnCalc, btnReal;
        TextBlock lblClock, lblSkyTime;
        Slider trackTime;
        int lastTrackValue;
        Slider sldFont;              // "Carattere": 100-300 % di tutto il testo
        TextBlock lblFont;
        bool fontReady;              // la scala dei controlli standard si applica dopo l'apertura (stili gia' applicati)
        TextBlock lblMapCaption;     // didascalia sotto la pianta
        CalendarDatePicker dtDay;

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
            Border sb = new Border { Background = Pal.B(Pal.Track), Padding = new Thickness(8, 3), Child = status };
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
            public int Selected = -1;
            public int Count { get { return buttons.Count; } }   // numero di linguette

            public PillTabs()
            {
                Line.Children.Add(strip);
                Bar = new Border { Background = Pal.B(Pal.Track), Padding = new Thickness(8, 5), Child = Line };
            }

            public void Add(string title, Control page)
            {
                int idx = buttons.Count;
                ToggleButton b = new ToggleButton { Content = title, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 1, 4, 1) };
                b.Classes.Add("tab");
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
            btnCalc = B(Lang.T("Calcola")); btnCalc.Classes.Add("accent");
            btnCalc.Click += (s, e) => Recalc();
            cmbBand.SelectionChanged += (s, e) => { if (model != null) { ApplyAntenna(); UpdateSky(); UpdateFluxTab(); UpdateGeometry(); } };

            // l'antenna e' sempre puntata verso il corrispondente (azimut della tratta)
            Color wheat = Pal.Ink(Pal.Rgb(245, 222, 179));   // color grano: scurito negli schemi chiari
            lblAntAz = L(Lang.T("Antenna az: -")); lblAntAz.Foreground = Pal.B(wheat);
            numBoom = N(0.5m, 30, Dm(cfg.GetD("boom_m", 4.6)), 2, 0.1m, 60);
            lblAnt = L(""); lblAnt.Foreground = Pal.B(wheat);
            numBoom.ValueChanged += (s, e) => { if (model != null) { ApplyAntenna(); UpdateSky(); UpdateGeometry(); } };
            numHotKm = N(30, 400, Dm(cfg.GetD("hot_spot_km", 150)), 0, 10, 55);
            PathModel.HotSpotOffsetKm = V(numHotKm);
            numHotKm.ValueChanged += (s, e) => { PathModel.HotSpotOffsetKm = V(numHotKm); if (model != null) Recalc(); };

            // riga 2: strato E, lobo e antenna
            Border row2 = Row(L(Lang.T("Quota scie km")), numLayer, chkLayerRadar,
                              L(Lang.T("Elevazione lobo °")), numElev,
                              L(Lang.T("Antenna Yagi: boom m")), numBoom, lblAnt,
                              L(Lang.T("Hot spot dal punto medio km")), numHotKm);

            // riga 3: orologio, ora della volta celeste, giorno della tabella 24 h
            lblClock = L("--:--:-- UTC"); lblClock.FontFamily = Pal.Mono; lblClock.FontSize = 15; lblClock.FontWeight = FontWeight.Bold; lblClock.Foreground = Pal.B(Pal.Yellow);
            trackTime = new Slider { Minimum = -144, Maximum = 144, Value = 0, SmallChange = 1, LargeChange = 4, TickFrequency = 12, Width = 360, Margin = new Thickness(4, 0) };
            WhenChanged(trackTime, RangeBase.ValueProperty, () =>
            {
                int v = TrackValue;
                if (v == lastTrackValue) return;
                lastTrackValue = v; UpdateSky();
            });
            lblSkyTime = L(Lang.T("Volta celeste: tempo reale")); lblSkyTime.Foreground = Pal.B(Pal.Azure);
            btnReal = B(Lang.T("Tempo reale"));
            btnReal.Background = Brushes.Transparent; btnReal.BorderBrush = Pal.B(Pal.Violet); btnReal.Foreground = Pal.B(Pal.Violet);
            btnReal.Click += (s, e) => { trackTime.Value = 0; UpdateSky(); };
            dtDay = new CalendarDatePicker { SelectedDate = DateTime.UtcNow.Date, Width = 150, Margin = new Thickness(2, 3) };
            WhenChanged(dtDay, CalendarDatePicker.SelectedDateProperty, () => Update24h());
            // dimensione del carattere di tutto il programma (solo il testo), a passi del 10 %
            double pct0 = Math.Max(100, Math.Min(300, Math.Round(cfg.GetD("carattere_pct", 100) / 10) * 10));
            lblFont = L(pct0.ToString("F0", CultureInfo.InvariantCulture) + " %"); lblFont.Foreground = Pal.B(Pal.Azure);
            sldFont = new Slider { Minimum = 100, Maximum = 300, Value = pct0, SmallChange = 10, LargeChange = 10, TickFrequency = 10,
                                   IsSnapToTickEnabled = true, Width = 160, Margin = new Thickness(4, 0) };
            UiScale.Set(pct0 / 100.0);
            WhenChanged(sldFont, RangeBase.ValueProperty, () => SetFontPercent(sldFont.Value));
            Border row3 = Row(L(Lang.T("Ora")), lblClock, L(Lang.T("Ora della volta celeste (±36 h)")), trackTime,
                              btnReal, lblSkyTime, L(Lang.T("Giorno per la tabella 24 h (UTC)")), dtDay);
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
            GravesFontScale();
            UpdateGeometry();   // il resoconto usa caratteri ricavati da quello della casella
        }

        DockPanel Page(string t)
        {
            DockPanel p = new DockPanel { Background = Pal.B(Pal.Back) };
            tabs.Add(t, p);
            return p;
        }

        void BuildTabs()
        {
            // --- Volta celeste
            DockPanel p1 = Page(Lang.T("Volta celeste"));
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
            // lato destro: in alto elenco degli sciami, al centro volta celeste + testo del calcolo, in basso la sezione
            Grid rt = new Grid { RowDefinitions = new RowDefinitions("31*,37*,32*") };
            DockPanel rTop = new DockPanel();
            DockTop(rTop, opt); DockTop(rTop, boxLobeWarn); DockTop(rTop, boxHighRad);
            rTop.Children.Add(lvSky);
            Grid rMid = new Grid { ColumnDefinitions = new ColumnDefinitions("42*,58*") };
            Grid.SetColumn(skySmall, 0); Grid.SetColumn(lblPathInfo, 1);
            rMid.Children.Add(skySmall); rMid.Children.Add(lblPathInfo);
            section.Margin = new Thickness(0, 4, 0, 0);
            Grid.SetRow(rTop, 0); Grid.SetRow(rMid, 1); Grid.SetRow(section, 2);
            rt.Children.Add(rTop); rt.Children.Add(rMid); rt.Children.Add(section);
            Grid.SetColumn(rt, 1);
            sc.Children.Add(rt);

            // --- Flusso radar (subito dopo "Volta celeste")
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

            // --- Sciami in diretta: GMN e CMOR (MainWindow.Diretta.cs)
            BuildLiveTab();

            // --- Calcolo dell'efficienza (ex "Efficienza 24 h")
            DockPanel p2 = Page(Lang.T("Calcolo dell'efficienza"));
            // colpo d'occhio: quattro riquadri riassuntivi sopra il grafico
            // altezza minima 100: con caratteri grandi i riquadri crescono
            Grid glance = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*,*,*"), MinHeight = 100, Margin = new Thickness(6, 6, 6, 2) };
            Border c0 = Card(Lang.T("ORA MIGLIORE DEL GIORNO"), Pal.Azure, out glBest);
            Border c1 = Card(Lang.T("SCIAME PIÙ FAVOREVOLE"), Pal.Yellow, out glShower);
            Border c2 = Card(Lang.T("ADESSO"), Pal.Green, out glNow);
            Border c3 = Card(Lang.T("PREVISIONE SPERIMENTALE"), Pal.Orange, out glFore);
            Grid.SetColumn(c0, 0); Grid.SetColumn(c1, 1); Grid.SetColumn(c2, 2); Grid.SetColumn(c3, 3);
            glance.Children.Add(c0); glance.Children.Add(c1); glance.Children.Add(c2); glance.Children.Add(c3);
            DockTop(p2, glance);
            lblAB = new TextBlock
            {
                Foreground = Pal.B(Pal.Fore), FontSize = 13, TextWrapping = TextWrapping.Wrap,
                Text = Lang.T("Le lettere accanto alla percentuale dicono da quale lato della tratta arriva la maggior parte delle riflessioni. " +
                       "A = a SINISTRA guardando dalla tua stazione verso il corrispondente (zona Hot A); B = a DESTRA (zona Hot B). " +
                       "Conviene spostare l'antenna di 10-30° verso quel lato (side-scatter). Nessuna lettera = efficienza sotto il 5 %. " +
                       "Riga Σ: indice della tratta = somma di ZHR x efficienza di tutti gli sciami attivi. " +
                       "Righe azzurre: confronto con la formula di DH9GCD (DF5HC 2018). ↑ = radiante sopra 70° dalla tua stazione (troppo alto).")
            };
            DockTop(p2, new Border { Background = Pal.B(Pal.Panel3), Padding = new Thickness(8, 5, 8, 4), Child = lblAB });
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

            // --- Radar GRAVES (MainWindow.Graves.cs), subito dopo "Calcolo dell'efficienza"
            BuildGravesTab();

            // --- Attuazione sperimentale previsioni (MainWindow.Previsioni.cs)
            BuildPrevTab();

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

            // --- Sciami
            DockPanel p5 = Page(Lang.T("Sciami"));
            // giustificata sulla larghezza: larghezze di base compatte, caratteri e righe crescono con la finestra
            gridShowers = MakeTable(new[] { Lang.T("Sigla"), Lang.T("Sciame"), Lang.T("Attività"), "λ max", Lang.T("Prossimo massimo (UTC)"), Lang.T("Fonte del massimo"), Lang.T("A.R."), Lang.T("Decl."), "v km/s", "r", "ZHR", Lang.T("Tipo"), Lang.T("Attivo oggi") },
                                    new double[] { 44, 170, 96, 56, 170, 130, 50, 50, 44, 36, 40, 90, 70 });
            gridShowers.Selectable = false;
            gridShowers.FitWidth = true;
            p5.Children.Add(gridShowers);

            // --- Fonti
            DockPanel p6 = Page(Lang.T("Fonti"));
            txtSources = new SelectableTextBlock { FontSize = 13.5, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap, Text = SourcesText() };
            p6.Children.Add(new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = new Border { Padding = new Thickness(14, 10), Child = txtSources }
            });

            // --- Mappa delle fonti (MainWindow.MappaFonti.cs)
            BuildSourcesMapTab();

            // --- Raccolta dati: spot MS da cluster, RBN e PSKReporter (MainWindow.Raccolta.cs)
            BuildCollectTab();

            // --- Efficienza - spiegazione (MainWindow.Spiegazione.cs)
            BuildExplainTab();

            // --- Mappa quadrati da ADIF (MainWindow.Grid.cs)
            BuildGridTab();
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

        DateTime SkyTimeUtc { get { return DateTime.UtcNow.AddMinutes(TrackValue * 15); } }

        void Recalc()
        {
            LatLon home, dx;
            if (!Astro.TryLocator(Up(txtHome), out home)) { status.Text = Lang.T("Locator della stazione non valido"); return; }
            if (!Astro.TryLocator(Up(txtDx), out dx)) { status.Text = Lang.T("Locator del corrispondente non valido"); return; }
            DateTime t0 = DateTime.Now;
            model = PathModel.Build(home, V(numHomeAlt) / 1000.0, dx, V(numDxAlt) / 1000.0, V(numLayer), V(numElev));
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
            lblSkyTime.Text = TrackValue == 0 ? Lang.T("Volta celeste: tempo reale")
                : Lang.F("Volta celeste: {0} UTC", t.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture));
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
                else recommendShort = recommendText = Lang.T("Nessun Hot consigliato: sciami attivi quasi assenti in questo momento");
            }
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
                sb.Append(Lang.F("Hot A/B: hot spot a {0} km dal punto medio (AMS 203, W8WN, MSHV).", PathModel.HotSpotOffsetKm.ToString("F0"))).Append('\n');
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
                DateTime d = dtDay.SelectedDate ?? DateTime.UtcNow;
                return DateTime.SpecifyKind(d.Date, DateTimeKind.Utc);
            }
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
                    row[3 + h] = (e < 1 ? "" : e.ToString("F0") + (e >= 5 ? sides[h] : "")) + (tooHigh[h] ? "↑" : "");
                    if (e >= 5) tips[3 + h] = HotTip(s, t, e, lf);
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
            glBest.Text = index[hb] > 0
                ? Lang.F("{0:00}:00-{0:00}:59 UTC\nindice della tratta {1:F1}\nlato prevalente {2} ({3:F0}%)", hb, index[hb], wA[hb] >= wB[hb] ? "A" : "B", 100 * Math.Max(wA[hb], wB[hb]) / Math.Max(1e-9, wA[hb] + wB[hb]))
                : Lang.T("nessuno sciame utile in questo giorno");
            glShower.Text = bestShower;
            DateTime nowU = DateTime.UtcNow;
            if (nowU.Date == day.Date)
            {
                int hn = nowU.Hour;
                glNow.Text = Lang.F("{0:00}:{1:00} UTC - indice {2:F1}\nlato prevalente {3}\n{4}", hn, nowU.Minute, index[hn],
                    index[hn] > 0.05 ? (wA[hn] >= wB[hn] ? "A" : "B") : "-", recommendShort.Trim());
            }
            else glNow.Text = Lang.T("il giorno scelto non è oggi");
            DateTime hNow = new DateTime(nowU.Year, nowU.Month, nowU.Day, nowU.Hour, 0, 0, DateTimeKind.Utc);
            double f1 = ForecastAt(hNow);
            double f2 = ForecastAt(hNow.AddHours(1));
            double f3 = ForecastAt(hNow.AddHours(2));
            glFore.Text = f1 < 0 ? Lang.T("nessun dato radar: vedi Flusso radar")
                : Lang.F("{0:F0} MHz, riflessioni/ora previste\nora {1:F0} - tra 1 h {2} - tra 2 h {3}\nsperimentale (Collm, regole SM6FHZ)", PrevFreqMHz, f1,
                    f2 < 0 ? "-" : f2.ToString("F0"), f3 < 0 ? "-" : f3.ToString("F0"));
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
            if (model.ElevToMidDeg < 0) line("  " + Lang.T("-> punto medio sotto l'orizzonte alla quota delle scie: tratta oltre la portata geometrica del singolo salto"));
            sb.Append("\n");
            head(Lang.T("HOT A / HOT B (hot spot classici, per il puntamento d'antenna)"));
            line(Lang.F("Punti a {0:F0} km ai lati del punto medio, perpendicolari alla tratta (AMS Bulletin 203 e W8WN: 50-150 km; come MSHV)", PathModel.HotSpotOffsetKm));
            foreach (HotPoint h in new HotPoint[] { model.HotA, model.HotB })
            {
                if (h == null) continue;
                line(Lang.F("{0}: {1}  az. da me {2:F1}° (scarto {3:+0.0;-0.0}° dalla direzione diretta), dal corrispondente {4:F0}°, distanza {5:F0} km, elevazione {6:F1}°",
                    Lang.T(h.Label), Astro.ToLocator(h.Pos), h.BearingFromHome, h.SideScatterDeg, h.BearingFromDx, h.DistFromHome, h.ElevFromHome));
            }
            sb.Append("\n");
            head(Lang.T("LIMITI DELLA ZONA COMUNE (incroci delle curve dove l'orizzonte di ciascuna stazione incontra lo strato E, articolo)"));
            line(Lang.F("Portata alla quota delle scie con lobo a {0:F1}°: {1:F0} km", model.MinElevDeg, model.HotRadiusKm));
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
            head(Lang.T("ATTENUAZIONE DI TRATTA (scia sottodensa, McKinley 1961; guadagni d'antenna esclusi)"));
            line(Lang.F("Densita' della scia q = {0:E1} elettroni/m. Riferimenti dell'articolo a 144 MHz: 182 dB minimo (GM8JBJ), 200 dB medio (OH5IY)", q));
            line(Lang.T("Punto di riflessione").PadRight(30) + "28 MHz   50 MHz   70 MHz  144 MHz  222 MHz  432 MHz");
            List<KeyValuePair<string, LatLon>> pts = new List<KeyValuePair<string, LatLon>>();
            pts.Add(new KeyValuePair<string, LatLon>(Lang.T("Punto medio (forward)"), model.Mid));
            if (model.HotA != null && model.HotB != null) { pts.Add(new KeyValuePair<string, LatLon>(Lang.T("Hot A"), model.HotA.Pos)); pts.Add(new KeyValuePair<string, LatLon>(Lang.T("Hot B"), model.HotB.Pos)); }
            foreach (KeyValuePair<string, LatLon> kv in pts)
            {
                sb.Append(kv.Key.PadRight(28));
                foreach (double fb in new double[] { 28, 50, 70, 144, 222, 432 })
                {
                    double Ldb = MsGeometry.PathLossDb(model.Home, model.HomeAltKm, model.Dx, model.DxAltKm, kv.Value, model.LayerKm, fb, q);
                    sb.Append(Ldb.ToString("F1").PadLeft(9));
                }
                sb.Append("\n");
            }
            double lossMid = MsGeometry.PathLossDb(model.Home, model.HomeAltKm, model.Dx, model.DxAltKm, model.Mid, model.LayerKm, f, q);
            double gain = V(numGain), sens = V(numSens), power = V(numPower);
            double pdbm = 10 * Math.Log10(power * 1000.0);
            double eirp = pdbm + gain - 2.15;
            double rx = eirp - lossMid + gain - 2.15;
            sb.Append("\n");
            head(Lang.F("BILANCIO A {0:F0} MHz (punto medio)", f));
            line(Lang.F("Potenza {0:F0} W = {1:F1} dBm, EIRP = {2:F1} dBm (guadagno in dBd = dBi - 2,15)", power, pdbm, eirp));
            line(Lang.F("Attenuazione {0:F1} dB -> segnale in antenna {1:F1} dBm, minimo ricevibile {2:F0} dBm, margine {3:+0.0;-0.0} dB",
                lossMid, rx, sens, rx - sens));
            line(Lang.F("Con l'attenuazione media di OH5IY (200 dB a 144 MHz, scalata con λ³: {0:F1} dB a {1:F0} MHz) il margine sarebbe {2:+0.0;-0.0} dB",
                200 + 30 * Math.Log10(f / 144.0), f, eirp - (200 + 30 * Math.Log10(f / 144.0)) + gain - 2.15 - sens));
            sb.Append("\n");
            head(Lang.T("REGOLE SPERIMENTALI TRA LE BANDE (SM6FHZ, VUSHF 2025)"));
            line(Lang.F("Rispetto a 144 MHz: numero/intensità x {0:F3} ({1:+0.0;-0.0} dB), durata x {2:F2}. Ping massimo IARU a {3:F0} MHz: {4:F0} ms",
                MsGeometry.CountScale(144, f, 3), 10 * Math.Log10(MsGeometry.CountScale(144, f, 3)), MsGeometry.DurationScale(144, f), f, MsGeometry.PingMaxMs(f)));
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
                    s.Type == "R" ? Lang.T("radio, diurno") : Lang.T("visuale"), s.IsActive(now) ? Lang.T("sì") : "" };
                rows.Add(new TableRow { Cells = cells, Tag = s.Code });
            }
            gridShowers.Rows = rows;
            gridShowers.Refresh();
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
                if (TrackValue == 0) UpdateSky();
                if (now.Minute == 0 && (dtDay.SelectedDate ?? now).Date == now.AddMinutes(-1).Date) dtDay.SelectedDate = now.Date;
                if (now.Minute == 0 && now.Hour == 0) UpdateShowerGrid();   // nuovo giorno: aggiorna i prossimi massimi
            }
            if (chkAuto.IsChecked == true && !fetching && (now - lastFetchUtc).TotalMinutes >= 5)
                StartFetch();   // ogni 5 minuti: ogni fonte viene riscaricata solo se e' cambiata
        }

        void SaveConfig()
        {
            SaveCollectConfig();
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
"  " + Lang.T("Da qui: scie nello strato E a circa 90 km (radar SKiYMET), formula dell'elevazione, Hot A / Hot B, side-scatter, attenuazione 182-200 dB, durata dei ping IARU.") + "\n\n" +
Lang.T("CALCOLO E TEORIA DELL'EFFICIENZA") + "\n" +
"  " + Lang.T("Ilkka Yrjola OH5IY - MS-Soft (concetto di efficienza dello sciame), www.kolumbus.fi/oh5iy (sito storico)") + "\n" +
"  Wilhelm Rauh DL5MCG - MCG_MS, MS Efficiency Calculator, qsl.net/dl5mcg\n" +
"  " + Lang.T("Ingolf SM6FHZ - \"432 MHz Meteorscatter, Challenges and Techniques\", 45th Nordic VUSHF, maggio 2025 (regole lambda^3 e lambda^2, punto medio, direzione migliore)") + "\n" +
"  " + Lang.T("D.W.R. McKinley - Meteor Science and Engineering, 1961 (equazione delle scie sottodense)") + "\n" +
"  " + Lang.T("John GM8JBJ - Meteor scatter path loss, hamradio.engineering (da Kaiser & Closs 1952)") + "\n" +
"  " + Lang.T("Shelby W8WN - qsl.net/w8wn/hscw (elevazione e offset ottimali)") + "\n" +
"  " + Lang.T("Klaus von der Heide DJ5HG - QTFGUI (aurora, FAI e meteor scatter) e manuale \"QTFGUI Help\" 2007: legge di diffusione a cono,\n" +
"    caso di prova Amburgo - Barcellona nelle Quadrantidi, usato per verificare il modello") + "\n" +
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
"  " + Lang.T("Philip G0ISW - www.qsl.net/g0isw/g0iswms.htm (tassi orari radio massimi)") + "\n" +
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
