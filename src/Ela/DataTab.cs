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
            chkVhfOnly = Ui.C(Lang.T("Salva solo gli spot da 28 MHz in su (consigliato con RBN e PSKReporter)"), VhfOnly);
            chkVhfOnly.IsCheckedChanged += (s, e) => VhfOnly = chkVhfOnly.IsChecked == true;
            DockPanel sessPage = new DockPanel();
            Ui.Dock(sessPage, Ui.Row(chkVhfOnly), Dock.Top);
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

            DockPanel root = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(root, Inner.Bar, Dock.Top);
            root.Children.Add(Inner.Host);
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

        public void ConnectSaved()
        {
            foreach (SessionPanel p in sessions) if (p.ConnectAtStart) p.Connect();
        }

        public void Save()
        {
            Station.Cfg.Set("dc_solo_vhf", VhfOnly);
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
        int spots;

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
            sp.Children.Add(Ui.Row(Ui.L(Lang.T("Host")), txtHost, Ui.L(Lang.T("Porta")), numPort, Ui.L(Lang.T("Login")), txtLogin, Ui.L(Lang.T("Comandi")), txtCmds));
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
            foreach (string l in got)
            {
                lines.Enqueue(l);
                Spot s = SpotParser.Parse(l, index, now);
                if (s != null && (!DataTab.VhfOnly || s.FreqKHz >= 28000))
                {
                    if (SpotStore.Add(s)) spots++;
                }
            }
            while (lines.Count > 300) lines.Dequeue();
            console.Text = string.Join("\n", lines);
            sv.ScrollToEnd();
            lblCounts.Text = Lang.F("righe {0}, spot salvati {1}", Session.LinesIn, spots);
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
