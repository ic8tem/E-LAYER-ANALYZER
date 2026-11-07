// E-LAYER ANALYZER - linguetta LOG ADIF: quadrati Maidenhead lavorati e confermati banda per banda
// (dai 28 MHz in su, ogni banda per conto suo), dal report LoTW e dai file ADIF del log. Porting di LoTW Grid Map v2.
// Colori: arancione = lavorato, blu = confermato con QSL, verde = confermato via LoTW.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class LogTab
    {
        public readonly Control Root;
        readonly MapView map = new MapView();
        readonly WrapPanel bandStrip = new WrapPanel { Orientation = Orientation.Horizontal };
        readonly List<ToggleButton> bandButtons = new List<ToggleButton>();
        readonly ComboBox cmbProp;
        readonly TextBlock lblStats;
        readonly TableView tSq;
        readonly SelectableTextBlock txtSq;
        string band = "6m";
        string selGrid;
        Dictionary<string, SquareInfo> squares = new Dictionary<string, SquareInfo>();
        static readonly string[] PropKeys = { "", "ES", "FAI", "AU", "MS", "TR", "EME", "F2", "-" };
        static readonly string[] PropNames = { "Tutti i modi di propagazione", "ES - E-sporadico", "FAI", "AUR / AUE - aurora", "MS - meteor scatter", "TR - tropo", "EME", "F2", "senza PROP_MODE" };
        public static readonly Color ColWorked = Color.FromArgb(150, 255, 140, 0), ColQsl = Color.FromArgb(150, 30, 90, 255), ColLotw = Color.FromArgb(150, 0, 175, 0);

        public LogTab()
        {
            Config cf = Station.Cfg;
            band = cf.Get("log_banda", "6m");
            string files = cf.Get("log_file_adif", "");
            foreach (string f in files.Split('|')) if (f.Trim().Length > 0) LogStore.Files.Add(f.Trim());

            Button bOpen = Ui.B(Lang.T("Apri file ADIF..."), true);
            bOpen.Click += async (s, e) => await OpenAdif();
            Button bLotw = Ui.B(Lang.T("Scarica da LoTW..."));
            bLotw.Click += async (s, e) => await LotwDialog.Show(TopLevel.GetTopLevel(Root) as Window);
            Button bClear = Ui.B(Lang.T("Cancella memoria"));
            bClear.Click += async (s, e) =>
            {
                if (!await Dialogs.Confirm(TopLevel.GetTopLevel(Root) as Window, Lang.T("Cancella memoria"), Lang.T("Togliere tutti i file ADIF aperti e il report LoTW scaricato?"))) return;
                lock (LogStore.Files) LogStore.Files.Clear();
                try { if (File.Exists(LogStore.LotwFile)) File.Delete(LogStore.LotwFile); } catch { }
                StartReload();
            };
            cmbProp = Ui.Combo(PropNames, Math.Max(0, Array.IndexOf(PropKeys, cf.Get("log_prop", ""))), 200);
            cmbProp.SelectionChanged += (s, e) => Refresh();
            lblStats = new TextBlock { Foreground = Pal.B(Pal.Green), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4) };
            tSq = new TableView();
            tSq.Columns.AddRange(new[] { Lang.T("Quadrato"), "QSO", Lang.T("Stato"), Lang.T("Nominativi"), "Prop" });
            tSq.ColumnWidths.AddRange(new double[] { 70, 44, 70, 170, 70 });
            tSq.RowClicked += i =>
            {
                if (i < 0 || i >= tSq.Rows.Count) return;
                SquareInfo sq = tSq.Rows[i].Tag as SquareInfo;
                if (sq == null) return;
                selGrid = sq.Grid;
                double la, lo; Geo.LocToLatLon(sq.Grid, out la, out lo);
                map.CenterLat = la; map.CenterLon = lo;
                Draw(); Detail();
            };
            txtSq = Ui.Text(12.5);
            ScrollViewer svSq = new ScrollViewer { Content = txtSq, Height = 210, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };

            DockPanel right = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(right, Ui.Row(bOpen, bLotw, bClear), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Propagazione")), cmbProp), Dock.Top);
            Ui.Dock(right, lblStats, Dock.Top);
            Ui.Dock(right, Ui.Box(Lang.T("Quadrato scelto (clic sulla mappa o sulla tabella)"), Pal.Azure, svSq), Dock.Bottom);
            right.Children.Add(Ui.Box(Lang.T("Quadrati della banda"), Pal.Orange, tSq));

            DockPanel left = new DockPanel();
            Border strip = new Border { Background = Pal.B(Pal.Panel2), Padding = new Thickness(6, 3), Child = bandStrip };
            Ui.Dock(left, strip, Dock.Top);
            left.Children.Add(map);
            map.SetView(-30, 60, 25, 72);
            map.Clicked += (la, lo, rb) => { selGrid = Geo.LatLonToLoc(la, lo, 4); Draw(); Detail(); };
            Root = Ui.MapAndData(left, right);

            LogStore.Changed += () => Dispatcher.UIThread.Post(Refresh);
            Station.Changed += () => Draw();
        }

        public void Save()
        {
            Config cf = Station.Cfg;
            cf.Set("log_banda", band);
            cf.Set("log_prop", PropKeys[Math.Max(0, cmbProp.SelectedIndex)]);
            lock (LogStore.Files) cf.Set("log_file_adif", string.Join("|", LogStore.Files));
        }

        public static void StartReload()
        {
            Task.Run(() => { LogStore.Reload(); LogStore.RaiseChanged(); });
        }

        async Task OpenAdif()
        {
            try
            {
                TopLevel tl = TopLevel.GetTopLevel(Root);
                if (tl == null) return;
                IReadOnlyList<IStorageFile> files = await tl.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = Lang.T("File ADIF del log"),
                    AllowMultiple = true,
                    FileTypeFilter = new[] { new FilePickerFileType("ADIF") { Patterns = new[] { "*.adi", "*.adif", "*.ADI", "*.ADIF" } } }
                });
                if (files == null) return;
                foreach (IStorageFile f in files)
                {
                    string p = f.TryGetLocalPath();
                    if (p == null) continue;
                    lock (LogStore.Files) if (!LogStore.Files.Contains(p)) LogStore.Files.Add(p);
                }
                StartReload();
            }
            catch (Exception ex) { await Dialogs.Message(TopLevel.GetTopLevel(Root) as Window, "ADIF", ex.Message); }
        }

        bool PropOk(QsoRecord q)
        {
            string k = PropKeys[Math.Max(0, cmbProp.SelectedIndex)];
            if (k == "") return true;
            if (k == "-") return q.PropMode.Length == 0;
            if (k == "AU") return q.PropMode == "AUR" || q.PropMode == "AUE";
            return q.PropMode == k;
        }

        public void Refresh()
        {
            // linguette delle bande presenti nel log (sempre 28, 50, 70, 144 e 432 MHz)
            HashSet<string> present = new HashSet<string> { "10m", "6m", "4m", "2m", "70cm" };
            foreach (QsoRecord q in LogStore.Qsos.Values) present.Add(q.Band);
            bandStrip.Children.Clear(); bandButtons.Clear();
            for (int i = 0; i < LogBands.Names.Length; i++)
            {
                string b = LogBands.Names[i];
                if (!present.Contains(b)) continue;
                Dictionary<string, SquareInfo> sq = LogStore.Squares(b, PropOk);
                int conf = 0; foreach (SquareInfo s in sq.Values) if (s.Status > 0) conf++;
                ToggleButton t = new ToggleButton { Content = LogBands.Labels[i] + "  (" + sq.Count + "/" + conf + ")", Margin = new Thickness(0, 1, 4, 1), IsChecked = b == band };
                t.Classes.Add("tab");
                string bb = b;
                t.Click += (s, e) => { band = bb; Refresh(); };
                bandButtons.Add(t);
                bandStrip.Children.Add(t);
            }
            squares = LogStore.Squares(band, PropOk);
            int nConf = 0, nLotw = 0, nQso = 0;
            foreach (QsoRecord q in LogStore.Qsos.Values) if (q.Band == band && PropOk(q)) nQso++;
            List<SquareInfo> list = new List<SquareInfo>(squares.Values);
            list.Sort((a, b) => string.CompareOrdinal(a.Grid, b.Grid));
            tSq.Rows.Clear();
            foreach (SquareInfo s in list)
            {
                if (s.Status > 0) nConf++;
                if (s.Lotw) nLotw++;
                HashSet<string> calls = new HashSet<string>(), props = new HashSet<string>();
                foreach (QsoRecord q in s.Qsos) { calls.Add(q.Call); if (q.PropMode.Length > 0) props.Add(q.PropMode); }
                tSq.Rows.Add(new TableRow
                {
                    Cells = new[] { s.Grid, s.Qsos.Count.ToString(CultureInfo.InvariantCulture), s.Lotw ? "LoTW" : s.Qsl ? "QSL" : Lang.T("lavorato"), string.Join(" ", calls), string.Join(" ", props) },
                    Fore = s.Lotw ? Pal.Green : s.Qsl ? Pal.Azure : Pal.Orange,
                    Tag = s
                });
            }
            tSq.Refresh();
            int idx = LogBands.Index(band);
            lblStats.Text = Lang.F("{0}: {1} QSO, {2} quadrati lavorati, {3} confermati ({4} via LoTW). File aperti: {5}; report LoTW: {6}.",
                idx >= 0 ? LogBands.Labels[idx] : band, nQso, squares.Count, nConf, nLotw, LogStore.Files.Count, File.Exists(LogStore.LotwFile) ? Lang.T("sì") : Lang.T("no"))
                + (LogStore.LastError.Length > 0 ? "\n" + LogStore.LastError : "")
                + (band == "70cm" ? "\n" + Lang.T("Nota: l'E-sporadico non si applica ai 432 MHz (vedi Sintesi delle scelte di calcolo).") : "");
            Draw(); Detail();
        }

        void Draw()
        {
            map.Cells.Clear(); map.Markers.Clear(); map.Lines.Clear(); map.Legend.Clear(); map.Info.Clear();
            int idx = LogBands.Index(band);
            map.Caption = "LOG ADIF - " + (idx >= 0 ? LogBands.Labels[idx] : band) + " - " + PropNames[Math.Max(0, cmbProp.SelectedIndex)];
            foreach (SquareInfo s in squares.Values)
            {
                double lo0 = (s.Grid[0] - 'A') * 20 - 180 + (s.Grid[2] - '0') * 2, la0 = (s.Grid[1] - 'A') * 10 - 90 + (s.Grid[3] - '0');
                map.Cells.Add(new MapCell { Lat0 = la0, Lon0 = lo0, DLat = 1, DLon = 2, Fill = s.Lotw ? ColLotw : s.Qsl ? ColQsl : ColWorked });
            }
            if (selGrid != null && Geo.IsLocator(selGrid))
            {
                double lo0 = (selGrid[0] - 'A') * 20 - 180 + (selGrid[2] - '0') * 2, la0 = (selGrid[1] - 'A') * 10 - 90 + (selGrid[3] - '0');
                MapLine l = new MapLine { Color = Pal.Red, Width = 2.5 };
                l.Pts.Add(new Point(lo0, la0)); l.Pts.Add(new Point(lo0 + 2, la0)); l.Pts.Add(new Point(lo0 + 2, la0 + 1)); l.Pts.Add(new Point(lo0, la0 + 1)); l.Pts.Add(new Point(lo0, la0));
                map.Lines.Add(l);
            }
            if (Station.Valid)
                map.Markers.Add(new MapMarker { Lat = Station.Lat, Lon = Station.Lon, Radius = 5, Fill = Pal.Red, Square = true, Label = Station.Call.Length > 0 ? Station.Call : Station.Loc });
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("lavorato, non confermato"), ColWorked));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("confermato con QSL"), ColQsl));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("confermato via LoTW"), ColLotw));
            map.InvalidateVisual();
        }

        void Detail()
        {
            StringBuilder sb = new StringBuilder();
            SquareInfo s;
            if (selGrid == null) sb.AppendLine(Lang.T("Clic su un quadrato della mappa o della tabella."));
            else if (!squares.TryGetValue(selGrid, out s)) sb.AppendLine(Lang.F("{0}: nessun QSO in questa banda.", selGrid));
            else
            {
                double la, lo; Geo.LocToLatLon(selGrid, out la, out lo);
                sb.Append(Lang.F("{0}: {1} QSO - {2}", selGrid, s.Qsos.Count, s.Lotw ? Lang.T("confermato via LoTW") : s.Qsl ? Lang.T("confermato con QSL") : Lang.T("lavorato, non confermato")));
                if (Station.Valid) sb.Append(Lang.F("  ({0:F0} km, az {1:F0}°)", Geo.DistanceKm(Station.Lat, Station.Lon, la, lo), Geo.Bearing(Station.Lat, Station.Lon, la, lo)));
                sb.AppendLine();
                List<QsoRecord> qs = new List<QsoRecord>(s.Qsos);
                qs.Sort((a, b) => string.CompareOrdinal(a.Date + a.Time, b.Date + b.Time));
                foreach (QsoRecord q in qs)
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0} {1}  {2,-10} {3,-7} {4,-4} {5}", q.DateText, q.Time.Length >= 4 ? q.Time.Substring(0, 4) : q.Time, q.Call, q.Mode, q.PropMode, q.Lotw ? "LoTW" : q.Qsl ? "QSL" : ""));
            }
            txtSq.Text = sb.ToString();
        }
    }

    // Finestra di scaricamento del report ADIF da LoTW (la password non viene mai salvata)
    public static class LotwDialog
    {
        public static async Task Show(Window owner)
        {
            Config cf = Station.Cfg;
            Window w = new Window
            {
                Title = Lang.T("Scarica da LoTW"), Width = 520, SizeToContent = SizeToContent.Height, CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = Pal.B(Pal.Back)
            };
            TextBox user = Ui.T(cf.Get("lotw_utente", ""), 250, false);
            TextBox pass = Ui.T("", 250, false); pass.PasswordChar = '•';
            TextBox own = Ui.T(cf.Get("lotw_nominativo", ""), 250, true);
            CheckBox rem = Ui.C(Lang.T("Ricorda il nome utente (la password non viene mai salvata)"), cf.GetB("lotw_ricorda", true));
            TextBlock info = new TextBlock
            {
                Text = Lang.T("Vengono scaricati tutti i QSO caricati su LoTW, con i dettagli delle conferme (locator del corrispondente). Con log grandi LoTW può impiegare qualche minuto."),
                TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Muted), Margin = new Thickness(8)
            };
            Button ok = Ui.B(Lang.T("Scarica"), true), cancel = Ui.B(Lang.T("Annulla"));
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*"), RowDefinitions = new RowDefinitions("Auto,Auto,Auto"), Margin = new Thickness(10) };
            Control[] cells = { Ui.L(Lang.T("Nome utente LoTW")), user, Ui.L(Lang.T("Password LoTW")), pass, Ui.L(Lang.T("Solo nominativo (facoltativo)")), own };
            for (int i = 0; i < 6; i++) { Grid.SetRow(cells[i], i / 2); Grid.SetColumn(cells[i], i % 2); g.Children.Add(cells[i]); }
            StackPanel sp = new StackPanel();
            sp.Children.Add(g); sp.Children.Add(rem); sp.Children.Add(info);
            StackPanel btns = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(8) };
            btns.Children.Add(ok); btns.Children.Add(cancel);
            sp.Children.Add(btns);
            w.Content = sp;
            bool cancelled = false;
            cancel.Click += (s, e) => { cancelled = true; w.Close(); };
            ok.Click += async (s, e) =>
            {
                string u = (user.Text ?? "").Trim(), p = pass.Text ?? "", oc = Ui.Up(own);
                if (u.Length == 0 || p.Length == 0) { info.Text = Lang.T("Inserisci nome utente e password di LoTW."); return; }
                cf.Set("lotw_ricorda", rem.IsChecked == true);
                cf.Set("lotw_utente", rem.IsChecked == true ? u : "");
                cf.Set("lotw_nominativo", oc);
                Station.Save();
                string url = "https://lotw.arrl.org/lotwuser/lotwreport.adi?login=" + Uri.EscapeDataString(u) + "&password=" + Uri.EscapeDataString(p)
                    + "&qso_query=1&qso_qsl=no&qso_qsldetail=yes&qso_mydetail=yes&qso_withown=yes" + (oc.Length > 0 ? "&qso_owncall=" + Uri.EscapeDataString(oc) : "");
                ok.IsEnabled = false;
                info.Text = Lang.T("Collegamento a LoTW in corso...");
                string msg = await Task.Run(async () =>
                {
                    try
                    {
                        using (HttpClient h = new HttpClient { Timeout = TimeSpan.FromMinutes(15) })
                        {
                            h.DefaultRequestHeaders.UserAgent.ParseAdd("E-LAYER-ANALYZER/0.2 (programma per radioamatori)");
                            byte[] data = await h.GetByteArrayAsync(url);
                            string head = Encoding.ASCII.GetString(data, 0, Math.Min(data.Length, 8000));
                            if (head.IndexOf("<eoh>", StringComparison.OrdinalIgnoreCase) < 0)
                                return head.IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0 || head.IndexOf("username", StringComparison.OrdinalIgnoreCase) >= 0
                                    ? Lang.T("LoTW ha rifiutato l'accesso: controlla nome utente e password.") : Lang.T("Risposta inattesa da LoTW (non è un file ADIF). Riprova più tardi.");
                            File.WriteAllBytes(LogStore.LotwFile, data);
                            return null;
                        }
                    }
                    catch (Exception ex) { return Lang.F("Errore di collegamento: {0}", ex.Message); }
                });
                if (cancelled) return;
                if (msg != null) { info.Text = msg; ok.IsEnabled = true; return; }
                LogTab.StartReload();
                w.Close();
            };
            w.Opened += (s, e) => UiScale.Apply(w);
            if (owner != null) await w.ShowDialog(owner); else w.Show();
        }
    }
}
