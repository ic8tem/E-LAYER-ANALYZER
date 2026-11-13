// YA-MS - linguetta "Raccolta dati": spot meteor scatter da DX cluster, Reverse Beacon Network e PSKReporter (telnet),
// come la linguetta DATA COLLECTION di E-LAYER ANALYZER.
// 60 % della finestra (sinistra): sessioni telnet e spot MS raccolti; 40 % (destra): impostazione del filtro.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace YAMS
{
    public partial class MainWindow
    {
        public static Control MsFilterPanel;   // E-LAYER ANALYZER: filtro MS mostrato in DATA COLLECTION
        TextBox txtRdLogin, txtRdTest;
        TextBlock lblRdStats, lblRdTest, lblRdCall3, lblRdCount;
        TableView tRd;
        StackPanel rdWordList;
        readonly TextBox[] rdHost = new TextBox[3], rdCmds = new TextBox[3], rdSend = new TextBox[3];
        readonly NumericUpDown[] rdPort = new NumericUpDown[3];
        readonly CheckBox[] rdAuto = new CheckBox[3], rdStart = new CheckBox[3];
        readonly TextBlock[] rdState = new TextBlock[3], rdCounts = new TextBlock[3];
        readonly SelectableTextBlock[] rdConsole = new SelectableTextBlock[3];
        readonly ScrollViewer[] rdSv = new ScrollViewer[3];
        readonly TextBlock[] rdDot = new TextBlock[3];
        WrapPanel rdStrip;
        readonly CheckBox[] rdBand = new CheckBox[MsBands.Names.Length];
        bool rdDirty = true;
        int rdTick;

        void BuildCollectTab()
        {
            MsCollector.Init(dataDir, cfg);
            if (CallDb.Count == 0 && !CallDb.Loading) CallDb.StartLoad(dataDir);
            CallDb.Loaded += () => Avalonia.Threading.Dispatcher.UIThread.Post(UpdateCall3Label);

            DockPanel p = Hosted ? new DockPanel() : Page(Lang.T("Raccolta dati"));   // dentro E-LAYER ANALYZER: raccolta unica in DATA COLLECTION
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("60*,40*") };
            p.Children.Add(g);

            // ---------------- sinistra (60 %): sessioni e spot raccolti
            DockPanel left = new DockPanel();
            txtRdLogin = T(cfg.Get("rd_login", ""), 120, true);
            txtRdLogin.Watermark = Lang.T("nominativo");
            lblRdCall3 = new TextBlock { Foreground = Pal.B(Pal.Muted), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            Button bImp = B(Lang.T("Importa CALL3.TXT..."));
            bImp.Click += async (s, e) => await ImportCall3();
            DockTop(left, Row(L(Lang.T("Nominativo per il login")), txtRdLogin, bImp, lblRdCall3));

            Color[] cols = { Pal.Azure, Pal.Violet, Pal.Orange };

            DockPanel spotPane = new DockPanel { Margin = new Thickness(4, 4, 4, 0) };
            lblRdCount = new TextBlock { Foreground = Pal.B(Pal.Green), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            Button bOpen = B(Lang.T("Apri la cartella degli spot"));
            bOpen.Click += (s, e) => { try { Dialogs.OpenWithDefaultApp(MsCollector.Dir); } catch { } };
            Button bClr = B(Lang.T("Svuota la tabella"));
            bClr.Click += (s, e) => { MsCollector.ClearSpots(); rdDirty = true; FillRdTable(); };
            TextBlock th = new TextBlock { Text = Lang.T("Spot meteor scatter raccolti (i più recenti in alto)"), FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Fore), VerticalAlignment = VerticalAlignment.Center };
            DockTop(spotPane, Row(th, lblRdCount, bOpen, bClr));
            tRd = MakeTable(new[] { "UTC", Lang.T("Fonte"), "MHz", "DX", Lang.T("Loc DX"), "Spotter", Lang.T("Loc spotter"), "km", Lang.T("Filtro"), Lang.T("Commento") },
                            new double[] { 50, 90, 80, 90, 66, 100, 76, 56, 70, 260 });
            tRd.Selectable = false;
            spotPane.Children.Add(tRd);
            // le tre sessioni (cluster, RBN, DM4X/PSKReporter) tutte visibili in una linguetta, gli spot in un'altra;
            // in cima una striscia con lo stato delle tre sessioni, sempre visibile
            rdStrip = new WrapPanel { Margin = new Thickness(6, 2, 6, 2) };
            for (int i = 0; i < 3; i++)
            {
                rdDot[i] = new TextBlock { Text = (i + 1) + ". " + Lang.T(MsCollector.Defaults[i][0]) + " ●", FontWeight = FontWeight.SemiBold, Foreground = Pal.B(Pal.Muted), Margin = new Thickness(0, 0, 18, 0) };
                rdStrip.Children.Add(rdDot[i]);
            }
            mqDot = new TextBlock { Text = "4. " + Lang.T("PSKReporter MQTT") + " ●", FontWeight = FontWeight.SemiBold, Foreground = Pal.B(Pal.Muted), Margin = new Thickness(0, 0, 18, 0) };
            rdStrip.Children.Add(mqDot);
            DockTop(left, rdStrip);
            // una sottolinguetta per ciascuno dei tre flussi di cluster (richiesta IC8TEM), poi gli spot raccolti
            PillTabs inner = new PillTabs(true);
            string[] shortName = { Lang.T("DX cluster"), "RBN", Lang.T("DM4X (PSKReporter)") };
            // i riquadri si creano sempre: i loro campi servono all'aggiornamento e al salvataggio
            for (int i = 0; i < 3; i++) { Border sbx = SessionBox(i, cols[i]); if (!Hosted) inner.Add((i + 1) + ". " + shortName[i], sbx); }
            inner.Add("4. " + Lang.T("PSKReporter MQTT"), MqttBox(Pal.Green));
            inner.Add(Lang.T("Spot MS raccolti"), spotPane);
            DockTop(left, inner.Bar);
            left.Children.Add(inner.Host);
            Grid.SetColumn(left, 0); g.Children.Add(left);

            // ---------------- destra (40 %): filtro
            StackPanel fp = new StackPanel { Spacing = 6, Margin = new Thickness(10, 8, 10, 8) };
            fp.Children.Add(new TextBlock { Text = Lang.T("Filtro degli spot meteor scatter"), FontSize = 17, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure) });
            // 1° criterio: finestre di frequenza tipiche del MS
            CheckBox chkWin = C(Lang.T("1° criterio: frequenze tipiche degli spot meteor scatter (kHz)"), MsCollector.UseWindows);
            chkWin.FontWeight = FontWeight.Bold; chkWin.Margin = new Thickness(0, 2, 0, 0);
            chkWin.IsCheckedChanged += (s, e) => { MsCollector.UseWindows = chkWin.IsChecked == true; TestFilter(); };
            fp.Children.Add(chkWin);
            TextBox txtWin = T(MsCollector.WindowsText, 300, false);
            txtWin.HorizontalAlignment = HorizontalAlignment.Stretch; txtWin.Width = double.NaN;
            txtWin.TextChanged += (s, e) => { MsCollector.SetWindows(txtWin.Text); TestFilter(); };
            fp.Children.Add(txtWin);
            fp.Children.Add(new TextBlock
            {
                TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Fore), FontSize = 13,
                Text = Lang.T("Uno spot in queste finestre si accetta anche senza il nome del modo, tranne quando il commento dichiara un'altra propagazione con una di queste scritte:")
            });
            TextBox txtOther = T(MsCollector.OtherTagsText, 300, false);
            txtOther.HorizontalAlignment = HorizontalAlignment.Stretch; txtOther.Width = double.NaN;
            txtOther.TextChanged += (s, e) => { MsCollector.SetOtherTags(txtOther.Text); TestFilter(); };
            fp.Children.Add(txtOther);

            // 2° criterio: modi digitali MS e <MS>
            fp.Children.Add(new TextBlock { Text = Lang.T("2° criterio: modi digitali MS e <MS>"), FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(0, 8, 0, 0) });
            fp.Children.Add(new TextBlock
            {
                TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Fore), FontSize = 13,
                Text = Lang.T("Vale per gli spot fuori dalle finestre e per quelli con una scritta di altra propagazione: si tengono se contengono una delle parole accese, in maiuscolo, minuscolo o misto. " +
                              "MSK144, FSK441, PSK2K, JTMS e JT6M sono modi digitali usati solo per il meteor scatter: lo spot è MS di sicuro. " +
                              "<MS> indica genericamente che il collegamento è avvenuto via meteor scatter.")
            });
            rdWordList = new StackPanel { Spacing = 2 };
            fp.Children.Add(new Border { Background = Pal.B(Pal.Panel2), CornerRadius = new CornerRadius(8), Padding = new Thickness(8, 6), Child = rdWordList });
            TextBox txtNew = T("", 140, false);
            txtNew.Watermark = Lang.T("nuova parola");
            Button bAdd = B(Lang.T("Aggiungi"));
            bAdd.Click += (s, e) =>
            {
                string w = (txtNew.Text ?? "").Trim();
                if (w.Length == 0) return;
                foreach (MsFilterWord x in MsCollector.Words) if (string.Equals(x.Text, w, StringComparison.OrdinalIgnoreCase)) return;
                MsCollector.Words.Add(new MsFilterWord(w, true, false));
                txtNew.Text = "";
                FillWordList();
            };
            fp.Children.Add(Row(txtNew, bAdd));

            fp.Children.Add(new TextBlock { Text = Lang.T("Bande (MHz)"), FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(0, 6, 0, 0) });
            WrapPanel bw = new WrapPanel();
            for (int i = 0; i < MsBands.Names.Length; i++)
            {
                string bn = MsBands.Names[i];
                rdBand[i] = C(bn, MsCollector.Bands.Contains(bn));
                rdBand[i].IsCheckedChanged += (s, e) =>
                {
                    if (((CheckBox)s).IsChecked == true) MsCollector.Bands.Add(bn); else MsCollector.Bands.Remove(bn);
                };
                bw.Children.Add(rdBand[i]);
            }
            fp.Children.Add(bw);
            CheckBox chkLow = C(Lang.T("Scarta subito le righe del cluster sotto 27990,0 kHz (non compaiono nella console e non si memorizzano)"), MsCollector.DropLow);
            ((Control)chkLow).Margin = new Thickness(0, 4, 0, 0);
            if (chkLow.Content is string cs) chkLow.Content = new TextBlock { Text = cs, TextWrapping = TextWrapping.Wrap };
            chkLow.IsCheckedChanged += (s, e) => MsCollector.DropLow = chkLow.IsChecked == true;
            chkLow.IsVisible = !Hosted;   // in E-LAYER ANALYZER c'e' gia' il filtro locale di DATA COLLECTION
            fp.Children.Add(chkLow);
            CheckBox chkCw = C(Lang.T("Scarta subito gli spot CW (CW o WPM nel commento, oppure 28000-28070 kHz): non compaiono nella console e non si memorizzano. Restano quelli segnati <MS>."), MsCollector.DropCw);
            if (chkCw.Content is string cws) chkCw.Content = new TextBlock { Text = cws, TextWrapping = TextWrapping.Wrap };
            chkCw.IsCheckedChanged += (s, e) => MsCollector.DropCw = chkCw.IsChecked == true;
            fp.Children.Add(chkCw);

            fp.Children.Add(new TextBlock { Text = Lang.T("Prova del filtro"), FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(0, 6, 0, 0) });
            txtRdTest = T("DX de DK5EW-#:  144360.0  IC8TEM   JN48<>JN70 -12 dB   2133Z", 420, false);
            txtRdTest.FontSize = 12;
            txtRdTest.HorizontalAlignment = HorizontalAlignment.Stretch; txtRdTest.Width = double.NaN;
            lblRdTest = new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 13 };
            txtRdTest.TextChanged += (s, e) => TestFilter();
            fp.Children.Add(txtRdTest);
            fp.Children.Add(lblRdTest);

            fp.Children.Add(new TextBlock { Text = Lang.T("Contatori"), FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(0, 6, 0, 0) });
            lblRdStats = new TextBlock { FontFamily = Pal.Mono, FontSize = 12.5, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap };
            fp.Children.Add(lblRdStats);
            fp.Children.Add(new TextBlock
            {
                TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Muted), FontSize = 12,
                Text = Lang.T("Gli spot accettati si salvano in data/spot_ms, un file CSV al giorno. I locator mancanti si prendono dall'archivio CALL3 (data/call3.txt).") + " " +
                       Lang.T("Anche i locator imprecisi (solo 4 caratteri) si prendono da CALL3, quando CALL3 ha il locator a 6 caratteri.")
            });
            Border right = new Border
            {
                Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1, 0, 0, 0),
                Child = new ScrollViewer { Content = fp, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled }
            };
            Grid.SetColumn(right, 1); g.Children.Add(right);
            if (Hosted) { ((ScrollViewer)right.Child).Content = null; MsFilterPanel = fp; }

            FillWordList();
            TestFilter();
            UpdateCall3Label();
            RefreshCollect(true);
        }

        Border SessionBox(int i, Color tc)
        {
            string[] def = MsCollector.Defaults[i];
            string k = "rd" + (i + 1) + "_";
            rdHost[i] = T(cfg.Get(k + "host", def[1]), 200, false);
            rdPort[i] = N(1, 65535, Dm(cfg.GetD(k + "porta", double.Parse(def[2], CultureInfo.InvariantCulture))), 0, 1, 60);
            string cmds = cfg.Get(k + "comandi", def[3]);
            if (i == 0 && cmds.Trim() == MsCollector.OldClusterCommands) cmds = def[3];   // comandi di partenza vecchi: si aggiungono 28 e 70 MHz
            rdCmds[i] = T(cmds, 240, false);
            rdCmds[i].Watermark = Lang.T("comandi dopo il login, separati da ;");
            rdAuto[i] = C(Lang.T("Riconnetti"), cfg.GetB(k + "riconnetti", true));
            rdStart[i] = C(Lang.T("Connetti all'avvio"), cfg.GetB(k + "avvio", false));
            Button bc = B(Lang.T("Connetti")); bc.Classes.Add("accent");
            Button bd = B(Lang.T("Disconnetti"));
            int idx = i;
            bc.Click += (s, e) => RdConnect(idx);
            bd.Click += (s, e) => MsCollector.Sessions[idx].Disconnect();
            rdState[i] = new TextBlock { Foreground = Pal.B(Pal.Muted), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            rdCounts[i] = new TextBlock { Foreground = Pal.B(Pal.Green), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            rdConsole[i] = new SelectableTextBlock { FontFamily = Pal.Mono, FontSize = 11.5, Foreground = Pal.B(Pal.CodeText), TextWrapping = TextWrapping.NoWrap };
            rdSv[i] = new ScrollViewer { Content = rdConsole[i], Background = Pal.B(Pal.CodeBack), HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
            rdSend[i] = T("", 260, false);
            rdSend[i].Watermark = Lang.T("comando da inviare (es. sh/dx 20)");
            Button bs = B(Lang.T("Invia"));
            Action send = () => { MsCollector.Sessions[idx].Send(rdSend[idx].Text ?? ""); rdSend[idx].Text = ""; };
            bs.Click += (s, e) => send();
            rdSend[i].KeyUp += (s, e) => { if (e.Key == Avalonia.Input.Key.Enter) send(); };

            // ogni flusso ha la sua sottolinguetta: comandi in alto, console che riempie lo spazio
            DockPanel sp = new DockPanel();
            TextBlock title = new TextBlock { Text = (i + 1) + ". " + Lang.T(def[0]), FontWeight = FontWeight.Bold, FontSize = 15, Foreground = Pal.B(tc), Margin = new Thickness(4, 2, 12, 2) };
            DockTop(sp, title);
            DockTop(sp, new TextBlock { Text = Lang.T(def[4]), Foreground = Pal.B(Pal.Muted), FontSize = 12, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 0, 6, 4) });
            DockTop(sp, Row(L(Lang.T("Host")), rdHost[i], L(Lang.T("Porta")), rdPort[i], L(Lang.T("Comandi")), rdCmds[i]));
            DockTop(sp, Row(bc, bd, rdAuto[i], rdStart[i], rdState[i], rdCounts[i]));
            Border sendRow = Row(rdSend[i], bs);
            DockPanel.SetDock(sendRow, Dock.Bottom);
            sp.Children.Add(sendRow);
            sp.Children.Add(rdSv[i]);
            return new Border
            {
                Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10), Padding = new Thickness(6, 3), Margin = new Thickness(4), Child = sp
            };
        }

        // quarta fonte: PSKReporter via MQTT (mqtt.pskreporter.info), spot gia' strutturati con i locator di chi trasmette e di chi riceve
        TextBox mqHost, mqTopics;
        NumericUpDown mqPort;
        CheckBox mqAuto, mqStart;
        TextBlock mqState, mqCounts, mqDot;
        SelectableTextBlock mqConsole;
        ScrollViewer mqSv;

        Border MqttBox(Color tc)
        {
            mqHost = T(cfg.Get("rd4_host", PskMqtt.DefaultHost), 200, false);
            mqPort = N(1, 65535, Dm(cfg.GetD("rd4_porta", PskMqtt.DefaultPort)), 0, 1, 60);
            string topics = cfg.Get("rd4_argomenti", PskMqtt.DefaultTopics);
            if (topics.Trim() == PskMqtt.OldDefaultTopics) topics = PskMqtt.DefaultTopics;   // argomenti di partenza vecchi: si aggiornano
            mqTopics = T(topics, 300, false);
            mqTopics.AcceptsReturn = true; mqTopics.TextWrapping = TextWrapping.Wrap; mqTopics.Height = 74;
            mqTopics.HorizontalAlignment = HorizontalAlignment.Stretch; mqTopics.Width = double.NaN;
            mqAuto = C(Lang.T("Riconnetti"), cfg.GetB("rd4_riconnetti", true));
            mqStart = C(Lang.T("Connetti all'avvio"), cfg.GetB("rd4_avvio", false));
            Button bc = B(Lang.T("Connetti")); bc.Classes.Add("accent");
            Button bd = B(Lang.T("Disconnetti"));
            Button bdef = B(Lang.T("Argomenti di partenza"));
            bc.Click += (s, e) => MqConnect();
            bd.Click += (s, e) => PskMqtt.Disconnect();
            bdef.Click += (s, e) => mqTopics.Text = PskMqtt.DefaultTopics;
            mqState = new TextBlock { Foreground = Pal.B(Pal.Muted), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            mqCounts = new TextBlock { Foreground = Pal.B(Pal.Green), Margin = new Thickness(8, 0), VerticalAlignment = VerticalAlignment.Center };
            mqConsole = new SelectableTextBlock { FontFamily = Pal.Mono, FontSize = 11.5, Foreground = Pal.B(Pal.CodeText), TextWrapping = TextWrapping.NoWrap };
            mqSv = new ScrollViewer { Content = mqConsole, Background = Pal.B(Pal.CodeBack), HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
            DockPanel sp = new DockPanel();
            DockTop(sp, new TextBlock { Text = "4. " + Lang.T("PSKReporter MQTT"), FontWeight = FontWeight.Bold, FontSize = 15, Foreground = Pal.B(tc), Margin = new Thickness(4, 2, 12, 2) });
            DockTop(sp, new TextBlock
            {
                Text = Lang.T("Spot di PSKReporter in diretta via MQTT (mqtt.pskreporter.info, servizio di M0LTE): nessun login, arrivano gia' con i locator di chi trasmette e di chi riceve. Un argomento per riga o separati da ;  formato pskr/filter/v2/banda/modo/call tx/call rx/... (+ = qualsiasi, # = il resto)."),
                Foreground = Pal.B(Pal.Muted), FontSize = 12, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 0, 6, 4)
            });
            DockTop(sp, Row(L(Lang.T("Host")), mqHost, L(Lang.T("Porta")), mqPort, bdef));
            DockTop(sp, new Border { Padding = new Thickness(6, 2), Child = mqTopics });
            DockTop(sp, Row(bc, bd, mqAuto, mqStart, mqState, mqCounts));
            sp.Children.Add(mqSv);
            return new Border
            {
                Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10), Padding = new Thickness(6, 3), Margin = new Thickness(4), Child = sp
            };
        }

        void MqConnect()
        {
            PskMqtt.Host = (mqHost.Text ?? "").Trim();
            PskMqtt.Port = (int)V(mqPort);
            PskMqtt.Topics = mqTopics.Text ?? "";
            PskMqtt.AutoReconnect = mqAuto.IsChecked == true;
            PskMqtt.Connect();
        }

        void RdConnect(int i)
        {
            TelnetSession s = MsCollector.Sessions[i];
            s.Host = (rdHost[i].Text ?? "").Trim();
            s.Port = (int)V(rdPort[i]);
            s.Login = Up(txtRdLogin);
            s.InitCommands = rdCmds[i].Text ?? "";
            s.AutoReconnect = rdAuto[i].IsChecked == true;
            if (s.Login.Length == 0) { rdState[i].Text = Lang.T("Scrivi il nominativo per il login prima di connettere."); rdState[i].Foreground = Pal.B(Pal.Orange); return; }
            s.Connect();
        }

        // all'apertura del programma: sessioni con "Connetti all'avvio" (una sola volta, non dopo il cambio di lingua)
        void RdAutoStart()
        {
            if (Hosted || MsCollector.Started) return;
            MsCollector.Started = true;
            for (int i = 0; i < 3; i++) if (rdStart[i].IsChecked == true) RdConnect(i);
            if (mqStart != null && mqStart.IsChecked == true) MqConnect();
        }

        void FillWordList()
        {
            rdWordList.Children.Clear();
            foreach (MsFilterWord w in MsCollector.Words)
            {
                MsFilterWord ww = w;
                CheckBox cb = C(w.Text, w.Enabled);
                cb.FontFamily = Pal.Mono; cb.FontSize = 14; cb.FontWeight = FontWeight.Bold;
                cb.IsCheckedChanged += (s, e) => { ww.Enabled = cb.IsChecked == true; TestFilter(); };
                TextBlock note = new TextBlock
                {
                    Text = w.Text == "<MS>" ? Lang.T("collegamento via meteor scatter") : w.BuiltIn ? Lang.T("modo digitale MS") : Lang.T("parola aggiunta"),
                    Foreground = Pal.B(Pal.Muted), FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(6, 0)
                };
                TextBlock cnt = new TextBlock { Text = w.Count.ToString(CultureInfo.CurrentCulture), Foreground = Pal.B(Pal.Green), FontFamily = Pal.Mono, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(6, 0), Tag = ww };
                Grid row = new Grid { ColumnDefinitions = new ColumnDefinitions("150,*,60,Auto") };
                Grid.SetColumn(cb, 0); Grid.SetColumn(note, 1); Grid.SetColumn(cnt, 2);
                row.Children.Add(cb); row.Children.Add(note); row.Children.Add(cnt);
                if (!w.BuiltIn)
                {
                    Button del = B("✕");
                    del.Click += (s, e) => { MsCollector.Words.Remove(ww); FillWordList(); };
                    Grid.SetColumn(del, 3); row.Children.Add(del);
                }
                rdWordList.Children.Add(row);
            }
            TestFilter();
        }

        void TestFilter()
        {
            if (lblRdTest == null) return;
            string line = txtRdTest.Text ?? "";
            MsSpot s = MsCollector.Parse(line, 0, DateTime.UtcNow);
            if (s == null)
            {
                MsFilterWord w0 = MsCollector.FindWord(line);
                lblRdTest.Text = w0 != null ? Lang.F("La parola \"{0}\" c'è, ma la riga non è uno spot \"DX de ...\" valido.", w0.Text)
                                            : Lang.T("La riga non è uno spot \"DX de ...\" valido.");
                lblRdTest.Foreground = Pal.B(Pal.Orange);
                return;
            }
            MsFilterWord w; string other;
            string why = MsCollector.Decide(s, out w, out other);
            if (MsCollector.DropLow && s.FreqKHz < MsCollector.MinKHz)
            {
                lblRdTest.Text = Lang.T("SCARTATO: frequenza sotto 27990,0 kHz.");
                lblRdTest.Foreground = Pal.B(Pal.Red);
            }
            else if (why == null)
            {
                lblRdTest.Text = other != null ? Lang.F("SCARTATO: il commento dichiara un'altra propagazione ({0}) e non c'è un modo MS.", other)
                                               : Lang.T("SCARTATO: fuori dalle finestre MS e nessuna parola del filtro.");
                lblRdTest.Foreground = Pal.B(Pal.Red);
            }
            else if (s.Band.Length == 0 || !MsCollector.Bands.Contains(s.Band))
            {
                lblRdTest.Text = Lang.F("SCARTATO: \"{0}\" trovato, ma la frequenza {1:F1} kHz non è in una banda scelta.", why, s.FreqKHz);
                lblRdTest.Foreground = Pal.B(Pal.Orange);
            }
            else
            {
                s.LocSpotter = MsCollector.Refine(s.LocSpotter, s.Spotter); s.LocDx = MsCollector.Refine(s.LocDx, s.DxCall);
                string crit = w == null ? Lang.F("finestra {0} kHz", why) : why;
                lblRdTest.Text = Lang.F("ACCETTATO per \"{0}\": {1} da {2}, {3} MHz", crit, s.DxCall, s.Spotter, s.Band) +
                    (s.LocSpotter.Length > 0 || s.LocDx.Length > 0 ? "  (" + s.LocSpotter + " → " + s.LocDx + ")" : "");
                lblRdTest.Foreground = Pal.B(Pal.Green);
            }
        }

        void UpdateCall3Label()
        {
            if (lblRdCall3 == null) return;
            lblRdCall3.Text = CallDb.Loading ? Lang.T("CALL3: caricamento...")
                : CallDb.Count > 0 ? Lang.F("CALL3: {0:N0} nominativi", CallDb.Count) : Lang.T("CALL3: nessun archivio (data/call3.txt)");
        }

        async System.Threading.Tasks.Task ImportCall3()
        {
            try
            {
                IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "CALL3.TXT", AllowMultiple = false,
                    FileTypeFilter = new[] { new FilePickerFileType("CALL3") { Patterns = new[] { "*.txt", "*.TXT" } } }
                });
                if (files == null || files.Count == 0) return;
                string path = files[0].TryGetLocalPath();
                if (path == null) return;
                CallDb.Replace(path, dataDir);
                UpdateCall3Label();
            }
            catch (Exception ex) { await Dialogs.Message(Win, "CALL3", ex.Message); }
        }

        // ogni secondo dall'orologio: righe nuove, console, contatori, tabella (ogni 2 s)
        void RefreshCollect(bool force)
        {
            if (tRd == null) return;
            if (MsCollector.Process()) rdDirty = true;
            if (!force && ++rdTick % 2 != 0) return;
            for (int i = 0; i < 3; i++)
            {
                TelnetSession s = MsCollector.Sessions[i];
                rdState[i].Text = Lang.T(MsCollector.StateText[i]);
                rdState[i].Foreground = Pal.B(s.Connected ? Pal.Green : Pal.Muted);
                rdDot[i].Foreground = Pal.B(s.Connected ? Pal.Green : Pal.Muted);
                ToolTip.SetTip(rdDot[i], rdState[i].Text);
                rdCounts[i].Text = Lang.F("righe {0}, spot letti {1}, MS {2}", MsCollector.Lines[i], MsCollector.Parsed[i], MsCollector.Accepted[i]);
                if (rdSv[i].IsEffectivelyVisible)   // console aggiornata solo se la sua sottolinguetta e' aperta
                {
                    string txt = string.Join("\n", MsCollector.ConsoleLines[i]);
                    if (rdConsole[i].Text != txt) { rdConsole[i].Text = txt; rdSv[i].ScrollToEnd(); }
                }
            }
            if (mqState != null)
            {
                mqState.Text = Lang.F(PskMqtt.StateKey, PskMqtt.StateArg);
                mqState.Foreground = Pal.B(PskMqtt.Connected ? Pal.Green : Pal.Muted);
                mqDot.Foreground = Pal.B(PskMqtt.Connected ? Pal.Green : Pal.Muted);
                ToolTip.SetTip(mqDot, mqState.Text);
                mqCounts.Text = Lang.F("righe {0}, spot letti {1}, MS {2}", MsCollector.Lines[3], MsCollector.Parsed[3], MsCollector.Accepted[3]);
                if (mqSv.IsEffectivelyVisible)
                {
                    string txt = string.Join("\n", MsCollector.ConsoleLines[3]);
                    if (mqConsole.Text != txt) { mqConsole.Text = txt; mqSv.ScrollToEnd(); }
                }
            }
            int lines = 0, parsed = 0, acc = 0;
            for (int i = 0; i < 4; i++) { lines += MsCollector.Lines[i]; parsed += MsCollector.Parsed[i]; acc += MsCollector.Accepted[i]; }
            int lastHour = 0; DateTime lim = DateTime.UtcNow.AddHours(-1);
            foreach (MsSpot s in MsCollector.Spots) if (s.Utc >= lim) lastHour++;
            lblRdStats.Text = Lang.F("righe ricevute      {0}\nspot letti          {1}\nspot MS accettati   {2}\nscartati (parola)   {3}\nscartati (banda)    {4}\nspot MS ultima ora  {5}",
                lines, parsed, acc, MsCollector.RejectedNoWord, MsCollector.RejectedBand, lastHour)
                + "\n" + Lang.F("righe sotto 27990 kHz scartate: {0}", MsCollector.DroppedLow)
                + "\n" + Lang.F("spot CW scartati: {0}", MsCollector.DroppedCw)
                + "\n" + Lang.F("accettati per finestra di frequenza: {0}", MsCollector.AcceptedWindow)
                + "\n" + Lang.F("scartati per altra propagazione: {0}", MsCollector.RejectedOther);
            foreach (Control c in rdWordList.Children)
                if (c is Grid gr) foreach (Control x in gr.Children) if (x is TextBlock tb && tb.Tag is MsFilterWord fw) tb.Text = fw.Count.ToString(CultureInfo.CurrentCulture);
            if (rdDirty) FillRdTable();
            RefreshTraffic(false);   // traffico in diretta (solo se la sottolinguetta e' aperta)
        }

        bool pages_visible_collect() { return tRd != null && tRd.IsEffectivelyVisible; }

        void FillRdTable()
        {
            rdDirty = false;
            List<TableRow> rows = new List<TableRow>();
            for (int i = MsCollector.Spots.Count - 1; i >= 0 && rows.Count < 1500; i--)
            {
                MsSpot s = MsCollector.Spots[i];
                rows.Add(new TableRow
                {
                    Cells = new[] { s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), MsCollector.SourceNames[s.Source], s.FreqKHz.ToString("F1", CultureInfo.CurrentCulture),
                        s.DxCall, s.LocDx, s.Spotter, s.LocSpotter, double.IsNaN(s.DistKm) ? "" : s.DistKm.ToString("F0", CultureInfo.CurrentCulture), s.Match, s.Comment },
                    Fore = s.Match == "<MS>" ? Pal.Yellow : Pal.Fore
                });
            }
            tRd.Rows = rows;
            tRd.Refresh();
            lblRdCount.Text = Lang.F("{0} spot MS in memoria", MsCollector.Spots.Count);
        }

        void SaveCollectConfig()
        {
            if (txtRdLogin == null) return;
            cfg.Set("rd_login", txtRdLogin.Text ?? "");
            for (int i = 0; i < 3; i++)
            {
                string k = "rd" + (i + 1) + "_";
                cfg.Set(k + "host", rdHost[i].Text ?? ""); cfg.Set(k + "porta", V(rdPort[i])); cfg.Set(k + "comandi", rdCmds[i].Text ?? "");
                cfg.Set(k + "riconnetti", rdAuto[i].IsChecked == true); cfg.Set(k + "avvio", rdStart[i].IsChecked == true);
            }
            if (mqHost != null)
            {
                cfg.Set("rd4_host", mqHost.Text ?? ""); cfg.Set("rd4_porta", V(mqPort)); cfg.Set("rd4_argomenti", mqTopics.Text ?? "");
                cfg.Set("rd4_riconnetti", mqAuto.IsChecked == true); cfg.Set("rd4_avvio", mqStart.IsChecked == true);
            }
            MsCollector.Save(cfg);
        }
    }
}
