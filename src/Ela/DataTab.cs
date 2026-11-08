// E-LAYER ANALYZER - linguetta DATA COLLECTION: tre sessioni telnet (cluster a scelta, RBN, cluster con PSKReporter),
// spot raccolti e archivio CALL3 (nominativo -> locator).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class DataTab
    {
        public readonly Control Root;
        public readonly PillTabs Inner = new PillTabs();
        readonly List<SessionPanel> sessions = new List<SessionPanel>();
        readonly TableView tAll;
        readonly ComboBox cmbBand, cmbSrc;
        readonly CheckBox chkVhfOnly;
        readonly TextBlock lblCount, lblCall3;
        readonly TextBox txtSearch;
        readonly TextBlock lblSearch;
        bool dirty = true;
        public static bool VhfOnly = true;
        public const double MinKHz = 27990.0, MaxKHz = 145000.0;   // limiti del filtro locale delle righe del cluster (kHz; massimo 145 MHz, regola dell'autore)
        static readonly System.Text.RegularExpressions.Regex rxDxFreq = new System.Text.RegularExpressions.Regex(@"DX de\s+\S+?:?\s+(\d+(?:\.\d+)?)",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        // frequenza (kHz) di una riga "DX de ..."; NaN se la riga non e' uno spot
        public static double LineFreqKHz(string l)
        {
            System.Text.RegularExpressions.Match m = rxDxFreq.Match(l ?? "");
            double f;
            return m.Success && double.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out f) ? f : double.NaN;
        }

        // valori di partenza delle tre sessioni: nome, host, porta, comandi iniziali, nota
        static readonly string[][] Defaults =
        {
            new[] { "DX cluster a scelta", "on0dxk.dyndns.org", "8000", "set/filter dxbm/pass 6", "Cluster scelto dall'utente. Su ON0DXK le bande VHF sono filtrate: abilitale con set/filter dxbm/pass 6 (poi 4, 2...). Altro esempio: hamqth.com 7300." },
            new[] { "Reverse Beacon Network", "telnet.reversebeacon.net", "7001", "", "RBN: porta 7000 = CW e RTTY, porta 7001 = FT8. Login con il solo nominativo." },
            new[] { "Cluster con PSKReporter", "dm4x.ddns.net", "8500", "", "Cluster di DM4X con gli spot di PSKReporter (dm4x.ddns.net, porta 8500). In alternativa HamAlert (hamalert.org 7300, serve un account con i trigger)." }
        };

        public DataTab()
        {
            Config cf = Station.Cfg;
            VhfOnly = cf.GetB("dc_solo_vhf", true);

            // --- sessioni telnet
            StackPanel sp = new StackPanel();
            for (int i = 0; i < 3; i++)
            {
                SessionPanel p = new SessionPanel(i, Defaults[i]);
                p.SpotArrived += () => dirty = true;
                sessions.Add(p);
                sp.Children.Add(p.Root);
            }
            chkVhfOnly = Ui.C(Lang.T("Filtro locale: scarta le righe del cluster sotto 27990,0 kHz e sopra 145 MHz (non mostrate e non memorizzate)"), VhfOnly);
            chkVhfOnly.IsCheckedChanged += (s, e) => VhfOnly = chkVhfOnly.IsChecked == true;
            DockPanel sessPage = new DockPanel();   // il filtro locale delle righe sta nella colonna dei filtri (40 %)
            sessPage.Children.Add(new ScrollViewer { Content = sp, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto });
            Inner.Add(Lang.T("Sessioni telnet"), sessPage);

            // --- spot raccolti
            cmbBand = Ui.Combo(new[] { Lang.T("Tutte"), "10m", "8m", "6m", "4m", "2m", "70cm", "HF" }, 0);
            cmbSrc = Ui.Combo(new[] { Lang.T("Tutte le fonti"), "Cluster", "RBN", "PSKReporter" }, 0, 120);
            cmbBand.SelectionChanged += (s, e) => { dirty = true; FillAll(); };
            cmbSrc.SelectionChanged += (s, e) => { dirty = true; FillAll(); };
            lblCount = new TextBlock { Foreground = Pal.B(Pal.Green), Margin = new Thickness(8, 0) };
            Button bOpen = Ui.B(Lang.T("Apri la cartella degli spot"));
            bOpen.Click += (s, e) => { try { Dialogs.OpenWithDefaultApp(SpotStore.Dir); } catch { } };
            tAll = new TableView();
            tAll.Columns.AddRange(new[] { "UTC", Lang.T("Fonte"), "MHz", Lang.T("Banda"), "DX", Lang.T("Loc DX"), "Spotter", Lang.T("Loc spotter"), "km", "Prop", Lang.T("Commento") });
            tAll.ColumnWidths.AddRange(new double[] { 46, 80, 76, 50, 86, 64, 96, 74, 54, 40, 260 });
            DockPanel spotPage = new DockPanel();
            Ui.Dock(spotPage, Ui.Row(Ui.L(Lang.T("Banda")), cmbBand, Ui.L(Lang.T("Fonte")), cmbSrc, lblCount, bOpen), Dock.Top);
            spotPage.Children.Add(tAll);
            Inner.Add(Lang.T("Spot raccolti"), spotPage);

            // --- CALL3
            lblCall3 = new TextBlock { Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(8), FontFamily = Pal.Mono, FontSize = 12.5 };
            txtSearch = Ui.T("", 140, true);
            lblSearch = new TextBlock { Foreground = Pal.B(Pal.Yellow), Margin = new Thickness(8, 0), FontFamily = Pal.Mono, FontSize = 14, VerticalAlignment = VerticalAlignment.Center };
            Button bFind = Ui.B(Lang.T("Cerca"), true);
            bFind.Click += (s, e) => Search();
            txtSearch.KeyUp += (s, e) => { if (e.Key == Avalonia.Input.Key.Enter) Search(); };
            Button bReload = Ui.B(Lang.T("Ricarica"));
            bReload.Click += (s, e) => { CallDb.StartLoad(AppPaths.DataDir); UpdateCall3(); };
            Button bImport = Ui.B(Lang.T("Importa un nuovo CALL3.TXT..."));
            bImport.Click += async (s, e) => await ImportCall3();
            DockPanel callPage = new DockPanel();
            Ui.Dock(callPage, Ui.Row(Ui.L(Lang.T("Nominativo")), txtSearch, bFind, lblSearch), Dock.Top);
            Ui.Dock(callPage, Ui.Row(bReload, bImport), Dock.Top);
            callPage.Children.Add(new ScrollViewer { Content = lblCall3 });
            Inner.Add("CALL3", callPage);

            // 60 % a sinistra: sessioni, spot raccolti, CALL3; 40 % a destra: i filtri di tutti i modi di propagazione.
            // La raccolta dei dati e' una sola per tutto il programma: anche YA-MS (METEOR SCATTER) riceve le righe da qui.
            DockPanel left = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(left, Inner.Bar, Dock.Top);
            left.Children.Add(Inner.Host);
            Grid root = new Grid { ColumnDefinitions = new ColumnDefinitions("60*,40*"), Background = Pal.B(Pal.Back) };
            Grid.SetColumn(left, 0); root.Children.Add(left);
            Control right = BuildFilters();
            Grid.SetColumn(right, 1); root.Children.Add(right);
            Root = root;

            CallDb.Loaded += () => Dispatcher.UIThread.Post(UpdateCall3);
            UpdateCall3();
            DispatcherTimer t = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            t.Tick += (s, e) =>
            {
                foreach (SessionPanel p in sessions) p.FlushConsole();
                if (dirty && Root.IsEffectivelyVisible) FillAll();
            };
            t.Start();
        }

        // ---------------- colonna dei filtri (40 %)
        public readonly PillTabs FilterTabs = new PillTabs();
        TextBox txtProva;
        TextBlock lblProva;

        static StackPanel FPanel(string title, Color c)
        {
            StackPanel fp = new StackPanel { Spacing = 6, Margin = new Thickness(10, 8, 10, 8) };
            fp.Children.Add(new TextBlock { Text = title, FontSize = 17, FontWeight = FontWeight.Bold, Foreground = Pal.B(c), TextWrapping = TextWrapping.Wrap });
            return fp;
        }
        static TextBlock FText(string t, bool muted = false)
        {
            return new TextBlock { Text = t, TextWrapping = TextWrapping.Wrap, FontSize = 13, Foreground = Pal.B(muted ? Pal.Muted : Pal.Fore) };
        }
        static TextBlock FHead(string t)
        {
            return new TextBlock { Text = t, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(0, 8, 0, 0), TextWrapping = TextWrapping.Wrap };
        }
        static CheckBox FCheck(string t, bool v)
        {
            CheckBox c = Ui.C("", v);
            c.Content = new TextBlock { Text = t, TextWrapping = TextWrapping.Wrap };
            return c;
        }
        static TextBox FBox(string v)
        {
            TextBox t = Ui.T(v, 300, false);
            t.HorizontalAlignment = HorizontalAlignment.Stretch; t.Width = double.NaN;
            return t;
        }
        static Control Scroll(Control c)
        {
            return new ScrollViewer { Content = c, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                                      HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled };
        }
        void Changed() { Filters.NotifyChanged(); TestFilters(); }

        Control BuildFilters()
        {
            // filtro generale delle righe del cluster (vale per tutto il programma)
            StackPanel head = new StackPanel { Spacing = 4, Margin = new Thickness(10, 8, 10, 4) };
            head.Children.Add(new TextBlock { Text = Lang.T("Filtri degli spot"), FontSize = 18, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure) });
            head.Children.Add(FText(Lang.T("La raccolta dei dati è una sola per tutto il programma: ogni spot ricevuto dalle sessioni telnet passa per questi filtri e va alle linguette di ciascun modo di propagazione."), true));
            if (chkVhfOnly.Content is string cs) chkVhfOnly.Content = new TextBlock { Text = cs, TextWrapping = TextWrapping.Wrap };
            head.Children.Add(chkVhfOnly);

            // 1. meteor scatter: il filtro di YA-MS
            Control ms = YAMS.MainWindow.MsFilterPanel;
            if (ms == null) ms = FText(Lang.T("Il filtro del meteor scatter è nella linguetta METEOR SCATTER."), true);
            FilterTabs.Add(Lang.T("Meteor scatter"), Scroll(ms));
            FilterTabs.Add(Lang.T("E-sporadico"), Scroll(BuildEsFilter()));
            FilterTabs.Add("F.A.I.", Scroll(BuildFaiFilter()));
            FilterTabs.Add(Lang.T("Aurora"), Scroll(BuildAuFilter()));
            FilterTabs.Add(Lang.T("Prova dei filtri"), Scroll(BuildTestPanel()));

            DockPanel dp = new DockPanel();
            Ui.Dock(dp, head, Dock.Top);
            Ui.Dock(dp, FilterTabs.Bar, Dock.Top);
            dp.Children.Add(FilterTabs.Host);
            return new Border { Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1, 0, 0, 0), Child = dp };
        }

        Control BuildEsFilter()
        {
            StackPanel fp = FPanel(Lang.T("Filtro degli spot di E-sporadico"), Pal.Azure);
            fp.Children.Add(FHead(Lang.T("Scritte di altra propagazione")));
            fp.Children.Add(FText(Lang.T("Gli spot che contengono una di queste scritte tra < > non entrano nell'E-sporadico: servono al meteor scatter, all'aurora o al tropo.")));
            TextBox tags = FBox(Filters.EsTags);
            tags.TextChanged += (s, e) => { Filters.SetEsTags(tags.Text); Changed(); };
            fp.Children.Add(tags);
            fp.Children.Add(FText(Lang.T("Sono esclusi anche gli spot riconosciuti come meteor scatter dal filtro della sottolinguetta Meteor scatter, e quelli con il modo dichiarato MS, FAI, AU, TR, EME o TEP."), true));

            fp.Children.Add(FHead(Lang.T("Regole dei 28 e 50 MHz (regole dell'autore)")));
            NumericUpDown n50 = Ui.N(0, 1500, Ui.Dm(EsModel.MinKm50), 0, 50, 90);
            fp.Children.Add(Ui.Row(Ui.L(Lang.T("Distanza minima in 28 e 50 MHz (km)")), n50));
            CheckBox cBp = FCheck(Lang.T("Il bandplan non è un criterio esclusivo: in 50 MHz uno spot nella finestra del meteor scatter resta nell'E-sporadico, salvo modo digitale MS o <MS>"), Filters.Es50WindowNotExclusive);
            fp.Children.Add(cBp);

            fp.Children.Add(FHead(Lang.T("Regola dei 144 MHz (regola dell'autore)")));
            CheckBox c144 = FCheck(Lang.T("Applica la regola: in 144 MHz l'E-sporadico vale oltre la distanza minima, e uno spot da solo non si disegna finché non è confermato"), EsModel.Use144Rule);
            fp.Children.Add(c144);
            NumericUpDown nMin = Ui.N(300, 3000, Ui.Dm(EsModel.MinKm144), 0, 50, 90);
            NumericUpDown nConf = Ui.N(1, 60, Ui.Dm(EsModel.Confirm144Min), 0, 1, 70);
            NumericUpDown nCloud = Ui.N(50, 1500, Ui.Dm(EsModel.CloudMatchKm), 0, 50, 90);
            NumericUpDown nShort = Ui.N(300, 2400, Ui.Dm(EsModel.Short50Km), 0, 100, 90);
            NumericUpDown nNear = Ui.N(1, 120, Ui.Dm(EsModel.Near50Min), 0, 1, 70);
            fp.Children.Add(Ui.Row(Ui.L(Lang.T("Distanza minima in 144 MHz (km)")), nMin));
            fp.Children.Add(FText(Lang.T("Conferma con un secondo spot in 144 MHz sulla stessa nube:")));
            fp.Children.Add(Ui.Row(Ui.L(Lang.T("entro (minuti)")), nConf, Ui.L(Lang.T("nube entro (km)")), nCloud));
            fp.Children.Add(FText(Lang.T("Oppure con uno spot in 50 MHz di salto corto nei dintorni:")));
            fp.Children.Add(Ui.Row(Ui.L(Lang.T("salto corto fino a (km)")), nShort, Ui.L(Lang.T("entro (minuti)")), nNear));

            fp.Children.Add(FHead(Lang.T("Regola transequatoriale (regola dell'autore)")));
            CheckBox cTep = FCheck(Lang.T("Verso l'Africa (Madagascar compreso; senza Canarie, Madeira, Capo Verde e penisola arabica) e il Sud America, se il punto medio cade nella fascia di inclinazione magnetica indicata il collegamento è transequatoriale, salvo che lo spot dichiari ES"), EsModel.UseTepRule);
            fp.Children.Add(cTep);
            NumericUpDown nTep = Ui.N(5, 45, Ui.Dm(EsModel.TepInclinationDeg), 0, 1, 70);
            fp.Children.Add(Ui.Row(Ui.L(Lang.T("fascia di inclinazione ± (gradi)")), nTep));

            Action read = () =>
            {
                EsModel.MinKm50 = Ui.V(n50); Filters.Es50WindowNotExclusive = cBp.IsChecked == true;
                EsModel.Use144Rule = c144.IsChecked == true; EsModel.MinKm144 = Ui.V(nMin); EsModel.Confirm144Min = Ui.V(nConf);
                EsModel.CloudMatchKm = Ui.V(nCloud); EsModel.Short50Km = Ui.V(nShort); EsModel.Near50Min = Ui.V(nNear);
                EsModel.UseTepRule = cTep.IsChecked == true; EsModel.TepInclinationDeg = Ui.V(nTep);
                Changed();
            };
            c144.IsCheckedChanged += (s, e) => read(); cTep.IsCheckedChanged += (s, e) => read(); cBp.IsCheckedChanged += (s, e) => read();
            foreach (NumericUpDown n in new[] { n50, nMin, nConf, nCloud, nShort, nNear, nTep }) n.ValueChanged += (s, e) => read();
            Button bReset = Ui.B(Lang.T("Ripristina le regole dell'autore"));
            bReset.Click += (s, e) =>
            {
                Filters.ResetEs();
                n50.Value = Ui.Dm(EsModel.MinKm50); cBp.IsChecked = Filters.Es50WindowNotExclusive;
                tags.Text = Filters.EsTags; c144.IsChecked = EsModel.Use144Rule; cTep.IsChecked = EsModel.UseTepRule;
                nMin.Value = Ui.Dm(EsModel.MinKm144); nConf.Value = Ui.Dm(EsModel.Confirm144Min); nCloud.Value = Ui.Dm(EsModel.CloudMatchKm);
                nShort.Value = Ui.Dm(EsModel.Short50Km); nNear.Value = Ui.Dm(EsModel.Near50Min); nTep.Value = Ui.Dm(EsModel.TepInclinationDeg);
                Changed();
            };
            fp.Children.Add(Ui.Row(bReset));
            fp.Children.Add(FText(Lang.T("I filtri valgono per la linguetta SPORADIC-E e per i due BENCHMARK; i valori si salvano nelle impostazioni."), true));
            return fp;
        }

        static WrapPanel BandChecks(HashSet<string> set, Action changed, List<CheckBox> list)
        {
            WrapPanel w = new WrapPanel();
            foreach (string b in Filters.BandNames)
            {
                string bn = b;
                CheckBox c = Ui.C(bn, set.Contains(bn));
                c.IsCheckedChanged += (s, e) => { if (c.IsChecked == true) set.Add(bn); else set.Remove(bn); changed(); };
                w.Children.Add(c); list.Add(c);
            }
            return w;
        }

        Control BuildFaiFilter()
        {
            StackPanel fp = FPanel(Lang.T("Filtro degli spot di F.A.I."), Pal.Violet);
            fp.Children.Add(FText(Lang.T("Uno spot è di F.A.I. se il modo dichiarato è FAI oppure se il testo contiene una di queste parole (anche senza < >, in maiuscolo o minuscolo):")));
            TextBox tags = FBox(Filters.FaiTags);
            tags.TextChanged += (s, e) => { Filters.SetFaiTags(tags.Text); Changed(); };
            fp.Children.Add(tags);
            fp.Children.Add(FHead(Lang.T("Bande")));
            List<CheckBox> bl = new List<CheckBox>();
            fp.Children.Add(BandChecks(Filters.FaiBands, Changed, bl));
            NumericUpDown nMin = Ui.N(15, 2880, Ui.Dm(Filters.FaiMinutes), 0, 15, 90);
            nMin.ValueChanged += (s, e) => { Filters.FaiMinutes = Ui.V(nMin); Changed(); };
            fp.Children.Add(Ui.Row(Ui.L(Lang.T("Spot sulla mappa degli ultimi (minuti)")), nMin));
            Button bReset = Ui.B(Lang.T("Ripristina le regole dell'autore"));
            bReset.Click += (s, e) =>
            {
                Filters.ResetFai(); tags.Text = Filters.FaiTags; nMin.Value = Ui.Dm(Filters.FaiMinutes);
                foreach (CheckBox c in bl) c.IsChecked = true;
                Changed();
            };
            fp.Children.Add(Ui.Row(bReset));
            fp.Children.Add(FText(Lang.T("Gli spot di F.A.I. si disegnano nella linguetta F.A.I. insieme al calcolo dello scatter point."), true));
            return fp;
        }

        Control BuildAuFilter()
        {
            StackPanel fp = FPanel(Lang.T("Filtro degli spot di aurora"), Pal.Green);
            fp.Children.Add(FText(Lang.T("Uno spot è di aurora se il modo dichiarato è AU oppure se il testo contiene una di queste parole, con o senza < >, in maiuscolo o minuscolo (regola dell'autore: AU e AUR):")));
            TextBox tags = FBox(Filters.AuTags);
            tags.TextChanged += (s, e) => { Filters.SetAuTags(tags.Text); Changed(); };
            fp.Children.Add(tags);
            fp.Children.Add(FHead(Lang.T("Bande")));
            List<CheckBox> bl = new List<CheckBox>();
            fp.Children.Add(BandChecks(Filters.AuBands, Changed, bl));
            NumericUpDown nMin = Ui.N(15, 1440, Ui.Dm(Filters.AuMinutes), 0, 15, 90);
            nMin.ValueChanged += (s, e) => { Filters.AuMinutes = Ui.V(nMin); Changed(); };
            fp.Children.Add(Ui.Row(Ui.L(Lang.T("Spot sulla mappa degli ultimi (minuti)")), nMin));
            Button bReset = Ui.B(Lang.T("Ripristina le regole dell'autore"));
            bReset.Click += (s, e) =>
            {
                Filters.ResetAu(); tags.Text = Filters.AuTags; nMin.Value = Ui.Dm(Filters.AuMinutes);
                foreach (CheckBox c in bl) c.IsChecked = true;
                Changed();
            };
            fp.Children.Add(Ui.Row(bReset));
            fp.Children.Add(FText(Lang.T("Gli spot di aurora non entrano nell'E-sporadico e si disegnano nella linguetta AURORA con il calcolo dello scatter point e il puntamento delle due stazioni."), true));
            return fp;
        }

        Control BuildTestPanel()
        {
            StackPanel fp = FPanel(Lang.T("Prova dei filtri"), Pal.Orange);
            fp.Children.Add(FText(Lang.T("Scrivi o incolla una riga del cluster: il programma mostra come la classificano i filtri.")));
            txtProva = FBox("DX de IK0XYZ:     50313.0  EA8ABC       JN61<ES>IL18 FT8 -12 dB    1532Z");
            txtProva.FontSize = 12;
            lblProva = new TextBlock { FontFamily = Pal.Mono, FontSize = 12.5, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap };
            txtProva.TextChanged += (s, e) => TestFilters();
            fp.Children.Add(txtProva);
            fp.Children.Add(lblProva);
            TestFilters();
            return fp;
        }

        void TestFilters()
        {
            if (lblProva == null) return;
            Spot s = SpotParser.Parse(txtProva.Text ?? "", 9, DateTime.UtcNow);
            if (s == null) { lblProva.Text = Lang.T("La riga non è uno spot (deve iniziare con DX de)."); return; }
            Func<bool, string> yn = b => b ? Lang.T("sì") : Lang.T("no");
            double f = s.FreqKHz;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Lang.F("Banda: {0}   frequenza: {1:F1} kHz", s.Band, f));
            if (VhfOnly && (f < MinKHz || f > MaxKHz)) sb.AppendLine(Lang.T("Filtro locale: riga scartata (fuori dalla gamma 27990 kHz - 145 MHz)"));
            sb.AppendLine(Lang.F("Locator: {0} - {1}", s.LocSpotter.Length > 0 ? s.LocSpotter : "?", s.LocDx.Length > 0 ? s.LocDx : "?"));
            if (s.HasBoth) sb.AppendLine(Lang.F("Distanza: {0:F0} km", s.DistanceKm));
            sb.AppendLine(Lang.F("Propagazione dichiarata: {0}", s.Prop.Length > 0 ? s.Prop : "-"));
            sb.AppendLine();
            string msTxt = s.IsMeteorScatterSure ? Lang.T("sì") : s.IsMeteorScatter ? Lang.T("possibile (finestra MS del bandplan)") : Lang.T("no");
            sb.AppendLine(Lang.F("Meteor scatter: {0}", msTxt));
            sb.AppendLine(Lang.F("Aurora: {0}", yn(Filters.IsAuroraSpot(s))));
            sb.AppendLine(Lang.F("F.A.I.: {0}", yn(Filters.IsFaiSpot(s))));
            bool tep = s.IsTep;
            sb.AppendLine(Lang.F("Transequatoriale: {0}", yn(tep)));
            bool es = s.EsCandidate && !tep;
            string esNote = "";
            if (!es && s.TooShortForEs && !tep) esNote = " " + Lang.F("(scartato: entro {0:F0} km in 28 e 50 MHz)", EsModel.MinKm50);
            if (es && s.Band == "2m" && EsModel.Use144Rule)
                esNote = s.HasBoth && s.DistanceKm < EsModel.MinKm144 ? " " + Lang.F("(scartato: sotto {0:F0} km in 144 MHz)", EsModel.MinKm144)
                                                                      : " " + Lang.T("(in 144 MHz si disegna solo con una conferma)");
            sb.AppendLine(Lang.F("E-sporadico: {0}", yn(es)) + esNote);
            lblProva.Text = sb.ToString().TrimEnd();
        }

        public void ConnectSaved()
        {
            foreach (SessionPanel p in sessions) if (p.ConnectAtStart) p.Connect();
        }

        public void Save()
        {
            Station.Cfg.Set("dc_solo_vhf", VhfOnly);
            Filters.Save(Station.Cfg);
            foreach (SessionPanel p in sessions) p.Save();
        }

        public void Shutdown() { foreach (SessionPanel p in sessions) p.Session.Disconnect(); }

        void FillAll()
        {
            dirty = false;
            string band = cmbBand.SelectedIndex > 0 ? (string)cmbBand.SelectedItem : null;
            int src = cmbSrc.SelectedIndex - 1;
            tAll.Rows.Clear();
            int n = 0;
            for (int i = SpotStore.All.Count - 1; i >= 0 && n < 2000; i--)
            {
                Spot s = SpotStore.All[i];
                if (band != null && s.Band != band) continue;
                if (src >= 0 && s.Source != src) continue;
                n++;
                tAll.Rows.Add(new TableRow
                {
                    Cells = new[] { s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), s.Source == 9 ? Lang.T("prova") : SpotStore.SourceNames[Math.Max(0, Math.Min(2, s.Source))],
                        s.FreqMHz.ToString("F3", CultureInfo.CurrentCulture), s.Band, s.DxCall, s.LocDx, s.Spotter, s.LocSpotter, Geo.Fmt(s.DistanceKm, 0), s.Prop, s.Comment },
                    Fore = EsTab.BandColor(s.Band)
                });
            }
            tAll.Refresh();
            lblCount.Text = Lang.F("{0} spot in memoria, {1} mostrati", SpotStore.All.Count, n);
        }

        void Search()
        {
            string c = Ui.Up(txtSearch);
            if (c.Length == 0) return;
            string l = CallDb.Lookup(c);
            if (l == null) { lblSearch.Text = Lang.T("non trovato"); return; }
            double la, lo; Geo.LocToLatLon(l, out la, out lo);
            string extra = "";
            if (Station.Valid) extra = Lang.F("  {0:F0} km, az {1:F0}°", Geo.DistanceKm(Station.Lat, Station.Lon, la, lo), Geo.Bearing(Station.Lat, Station.Lon, la, lo));
            lblSearch.Text = c + " = " + l + extra;
        }

        void UpdateCall3()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Lang.T("Archivio CALL3 (formato JTDX/WSJT: NOMINATIVO,LOCATOR,,,)"));
            sb.AppendLine(Lang.F("File: {0}", CallDb.FilePath));
            if (CallDb.Loading) sb.AppendLine(Lang.T("Caricamento in corso..."));
            else
            {
                sb.AppendLine(Lang.F("Nominativi caricati: {0:N0}", CallDb.Count));
                if (CallDb.FileDate != DateTime.MinValue) sb.AppendLine(Lang.F("Data del file: {0}", CallDb.FileDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture)));
                sb.AppendLine(Lang.F("Locator imparati dagli spot (data/call3_nuovi.txt): {0}", CallDb.LearnedCount));
                if (CallDb.LastError.Length > 0) sb.AppendLine(Lang.F("Errore: {0}", CallDb.LastError));
            }
            sb.AppendLine();
            sb.AppendLine(Lang.T("Intestazione del file:"));
            sb.AppendLine(CallDb.Header);
            sb.AppendLine(Lang.T("Gli spot senza locator nel commento prendono il locator da questo archivio; i locator a 6 caratteri letti nei commenti (es. JN70CN<ES>JO62QM) vengono aggiunti a call3_nuovi.txt."));
            lblCall3.Text = sb.ToString();
        }

        async System.Threading.Tasks.Task ImportCall3()
        {
            try
            {
                TopLevel tl = TopLevel.GetTopLevel(Root);
                if (tl == null) return;
                IReadOnlyList<IStorageFile> files = await tl.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = Lang.T("Nuovo file CALL3.TXT"),
                    AllowMultiple = false,
                    FileTypeFilter = new[] { new FilePickerFileType("CALL3") { Patterns = new[] { "*.txt", "*.TXT" } } }
                });
                if (files == null || files.Count == 0) return;
                string path = files[0].TryGetLocalPath();
                if (path == null) return;
                CallDb.Replace(path, AppPaths.DataDir);
                UpdateCall3();
            }
            catch (Exception ex) { await Dialogs.Message(TopLevel.GetTopLevel(Root) as Window, "CALL3", ex.Message); }
        }
    }

    // Una sessione telnet con la sua console
    public class SessionPanel
    {
        public readonly Control Root;
        public readonly TelnetSession Session = new TelnetSession();
        public event Action SpotArrived;
        public bool ConnectAtStart;
        readonly int index;
        readonly TextBox txtHost, txtLogin, txtCmds, txtSend;
        readonly NumericUpDown numPort;
        readonly CheckBox chkAuto, chkStart;
        readonly TextBlock lblState, lblCounts;
        readonly SelectableTextBlock console;
        readonly ScrollViewer sv;
        readonly Queue<string> lines = new Queue<string>();
        readonly Queue<string> pending = new Queue<string>();
        readonly object plk = new object();
        int spots, dropped;

        public SessionPanel(int idx, string[] def)
        {
            index = idx;
            string k = "dc" + (idx + 1) + "_";
            Config cf = Station.Cfg;
            txtHost = Ui.T(cf.Get(k + "host", def[1]), 220, false);
            numPort = Ui.N(1, 65535, Ui.Dm(cf.GetD(k + "porta", double.Parse(def[2], CultureInfo.InvariantCulture))), 0, 1, 60);
            txtLogin = Ui.T(cf.Get(k + "login", ""), 110, true);
            txtLogin.Watermark = Lang.T("nominativo");
            txtCmds = Ui.T(cf.Get(k + "comandi", def[3]), 260, false);
            txtCmds.Watermark = Lang.T("comandi dopo il login, separati da ;");
            chkAuto = Ui.C(Lang.T("Riconnetti"), cf.GetB(k + "riconnetti", true));
            chkStart = Ui.C(Lang.T("Connetti all'avvio"), cf.GetB(k + "avvio", false));
            ConnectAtStart = chkStart.IsChecked == true;
            Button bConn = Ui.B(Lang.T("Connetti"), true);
            Button bDisc = Ui.B(Lang.T("Disconnetti"));
            bConn.Click += (s, e) => Connect();
            bDisc.Click += (s, e) => Session.Disconnect();
            lblState = new TextBlock { Text = Lang.T("Non connesso"), Foreground = Pal.B(Pal.Muted), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            lblCounts = new TextBlock { Foreground = Pal.B(Pal.Green), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            console = new SelectableTextBlock { FontFamily = Pal.Mono, FontSize = 11.5, Foreground = Pal.B(Pal.CodeText), TextWrapping = TextWrapping.NoWrap };
            sv = new ScrollViewer { Content = console, Height = 170, Background = Pal.B(Pal.CodeBack), HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            txtSend = Ui.T("", 300, false);
            txtSend.Watermark = Lang.T("comando da inviare (es. sh/dx 20)");
            Button bSend = Ui.B(Lang.T("Invia"));
            bSend.Click += (s, e) => { Session.Send(txtSend.Text ?? ""); txtSend.Text = ""; };
            txtSend.KeyUp += (s, e) => { if (e.Key == Avalonia.Input.Key.Enter) { Session.Send(txtSend.Text ?? ""); txtSend.Text = ""; } };

            StackPanel sp = new StackPanel();
            sp.Children.Add(Ui.Row(Ui.L(Lang.T("Host")), txtHost, Ui.L(Lang.T("Porta")), numPort, Ui.L(Lang.T("Login")), txtLogin, Ui.L(Lang.T("Comandi dopo il login")), txtCmds));
            sp.Children.Add(Ui.Row(bConn, bDisc, chkAuto, chkStart, lblState, lblCounts));
            sp.Children.Add(new TextBlock { Text = Lang.T(def[4]), Foreground = Pal.B(Pal.Muted), FontSize = 11.5, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2) });
            sp.Children.Add(sv);
            sp.Children.Add(Ui.Row(txtSend, bSend));
            Color[] cols = { Pal.Azure, Pal.Violet, Pal.Orange };
            Root = Ui.Box((idx + 1) + ". " + Lang.T(def[0]), cols[idx], sp);

            Session.Name = def[0];
            Session.LineReceived += l => { lock (plk) pending.Enqueue(l); };
            Session.StateChanged += st => Dispatcher.UIThread.Post(() =>
            {
                lblState.Text = Lang.T(st);
                lblState.Foreground = Pal.B(Session.Connected ? Pal.Green : Pal.Orange);
            });
        }

        public void Connect()
        {
            Session.Host = (txtHost.Text ?? "").Trim();
            Session.Port = (int)Ui.V(numPort);
            string login = Ui.Up(txtLogin);
            Session.Login = login.Length > 0 ? login : Station.Call;
            Session.InitCommands = txtCmds.Text ?? "";
            Session.AutoReconnect = chkAuto.IsChecked == true;
            if (string.IsNullOrEmpty(Session.Login)) lblState.Text = Lang.T("Scrivi il nominativo (in alto o nel campo Login) prima di connettere.");
            else Session.Connect();
        }

        // passa le righe ricevute alla console e all'archivio degli spot (thread dell'interfaccia)
        public void FlushConsole()
        {
            List<string> got = new List<string>();
            lock (plk) { while (pending.Count > 0) got.Add(pending.Dequeue()); }
            if (got.Count == 0) return;
            DateTime now = DateTime.UtcNow;
            foreach (string l in got) YAMS.MsCollector.Feed(index, l);   // la Raccolta dati di YA-MS filtra da queste righe gli spot meteor scatter
            foreach (string l in got)
            {
                // filtro locale (facoltativo): le righe "DX de" sotto 27990,0 kHz o sopra 145 MHz si scartano, non vanno in console ne' in memoria
                double fk = DataTab.LineFreqKHz(l);
                if (DataTab.VhfOnly && !double.IsNaN(fk) && (fk < DataTab.MinKHz || fk > DataTab.MaxKHz)) { dropped++; continue; }
                lines.Enqueue(l);
                Spot s = SpotParser.Parse(l, index, now);
                if (s != null && (!DataTab.VhfOnly || (s.FreqKHz >= DataTab.MinKHz && s.FreqKHz <= DataTab.MaxKHz)))
                {
                    if (SpotStore.Add(s)) spots++;
                }
            }
            while (lines.Count > 300) lines.Dequeue();
            console.Text = string.Join("\n", lines);
            sv.ScrollToEnd();
            lblCounts.Text = Lang.F("righe {0}, spot salvati {1}", Session.LinesIn, spots) + (dropped > 0 ? ", " + Lang.F("scartate fuori da 27990 kHz - 145 MHz: {0}", dropped) : "");
            if (SpotArrived != null) SpotArrived();
        }

        public void Save()
        {
            string k = "dc" + (index + 1) + "_";
            Config cf = Station.Cfg;
            cf.Set(k + "host", txtHost.Text ?? ""); cf.Set(k + "porta", Ui.V(numPort)); cf.Set(k + "login", txtLogin.Text ?? "");
            cf.Set(k + "comandi", txtCmds.Text ?? ""); cf.Set(k + "riconnetti", chkAuto.IsChecked == true); cf.Set(k + "avvio", chkStart.IsChecked == true);
        }
    }
}
