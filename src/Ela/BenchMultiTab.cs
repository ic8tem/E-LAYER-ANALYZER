// E-LAYER ANALYZER - linguetta BENCHMARK 50 MHz: banco di prova dell'E-sporadico multi-salto in 50 MHz su un log ADIF
// (per ora una stazione e un log). Mappa di Mercatore a centro libero (rotella = zoom, trascina = sposta). Si sceglie il
// giorno e l'orario di inizio e fine; la foEs e' facoltativa (casella). Avvicinando il mouse a una tratta, icone e linea
// si ingrandiscono e compare il quadrato WWL a 6 caratteri del corrispondente; il clic sinistro su quella tratta apre
// un popup che chiede se vedere tutti i QSO del file con la posizione terminale entro 150 km da quel punto.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class BenchMultiTab
    {
        public readonly Control Root;
        readonly MapView map = new MapView { Mercator = true };
        readonly TextBlock lblFile, lblSel, lblArea;
        readonly ComboBox cmbDay, cmbFrom, cmbTo;
        readonly CheckBox chkFoEs, chkShort;
        readonly NumericUpDown numH;
        readonly Button bAll;
        readonly TableView tQso;
        readonly SelectableTextBlock txtDet;
        public const double AreaKm = 150;
        static readonly string[] Times = BuildTimes();

        List<Dictionary<string, string>> raw = new List<Dictionary<string, string>>();
        List<BenchQso> all = new List<BenchQso>();     // QSO in 50 MHz del log (tutte le giornate)
        List<BenchQso> cur = new List<BenchQso>();     // QSO mostrati
        string file = "", myCall = "", myGrid = "";
        BenchQso hover, sel;
        bool areaMode; double areaLat, areaLon; string areaName = "";
        readonly Dictionary<BenchQso, List<Point>> geo = new Dictionary<BenchQso, List<Point>>();
        bool busyFill;

        static string[] BuildTimes()
        {
            List<string> t = new List<string>();
            for (int m = 0; m <= 24 * 60; m += 15) t.Add((m / 60).ToString("00", CultureInfo.InvariantCulture) + ":" + (m % 60).ToString("00", CultureInfo.InvariantCulture));
            return t.ToArray();
        }

        public BenchMultiTab()
        {
            Config cf = Station.Cfg;
            Button bLoad = Ui.B(Lang.T("Carica ADIF"), true);
            bLoad.Click += async (s, e) => await PickFile();
            Button bUnload = Ui.B(Lang.T("Scarica ADIF"));
            bUnload.Click += (s, e) => Unload();
            lblFile = new TextBlock { Foreground = Pal.B(Pal.Muted), VerticalAlignment = VerticalAlignment.Center, TextWrapping = TextWrapping.Wrap, MaxWidth = 380 };
            cmbDay = Ui.Combo(new string[0], -1, 120);
            cmbFrom = Ui.Combo(Times, 0, 80);
            cmbTo = Ui.Combo(Times, Times.Length - 1, 80);
            foreach (ComboBox c in new[] { cmbDay, cmbFrom, cmbTo }) c.SelectionChanged += (s, e) => { if (!busyFill) { areaMode = false; Fill(); } };
            chkFoEs = Ui.C(Lang.T("foEs"), cf.GetB("b50_foes", false));
            chkShort = Ui.C(Lang.T("Primo salto corto entro 1200 km"), cf.GetB("b50_corto", true));
            numH = Ui.N(90, 130, Ui.Dm(cf.GetD("b50_quota_km", 105)), 0, 1, 45);
            chkFoEs.IsCheckedChanged += (s, e) => Fill(false);
            chkShort.IsCheckedChanged += (s, e) => Rebuild();
            numH.ValueChanged += (s, e) => Rebuild();
            lblSel = new TextBlock { Foreground = Pal.B(Pal.Green), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4) };
            lblArea = new TextBlock { Foreground = Pal.B(Pal.Orange), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2), FontWeight = FontWeight.SemiBold };
            bAll = Ui.B(Lang.T("Torna a giorno e orario"));
            bAll.Click += (s, e) => { areaMode = false; Fill(); };

            tQso = new TableView();
            tQso.Columns.AddRange(new[] { Lang.T("Giornata"), "UTC", Lang.T("Nominativo"), "WWL", "km", Lang.T("Salti"), "foEs≥" });
            tQso.ColumnWidths.AddRange(new double[] { 84, 46, 84, 70, 54, 40, 52 });
            tQso.RowClicked += i => { if (i >= 0 && i < tQso.Rows.Count) { sel = tQso.Rows[i].Tag as BenchQso; hover = sel; Draw(); Details(); } };
            txtDet = Ui.Text(12.5);
            ScrollViewer svDet = new ScrollViewer { Content = txtDet, Height = 230, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };

            DockPanel right = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(right, Ui.Row(bLoad, bUnload, lblFile), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Giorno")), cmbDay, Ui.L(Lang.T("dalle")), cmbFrom, Ui.L(Lang.T("alle")), cmbTo, Ui.L("UTC")), Dock.Top);
            Ui.Dock(right, Ui.Row(chkFoEs, Ui.L(Lang.T("Quota Es km")), numH, chkShort), Dock.Top);
            Ui.Dock(right, lblSel, Dock.Top);
            StackPanel areaRow = new StackPanel { Orientation = Orientation.Horizontal };
            areaRow.Children.Add(lblArea); areaRow.Children.Add(bAll);
            Ui.Dock(right, areaRow, Dock.Top);
            Ui.Dock(right, Ui.Box(Lang.T("Dettagli (mouse vicino a una tratta, clic sinistro = QSO nei dintorni)"), Pal.Azure, svDet), Dock.Bottom);
            right.Children.Add(Ui.Box(Lang.T("QSO multi-salto in 50 MHz"), Pal.Azure, tQso));

            map.SetView(-130, 60, -45, 72);
            map.PointerMoved += (s, e) => OnHover(e.GetPosition(map));
            map.PointerExited += (s, e) => { if (hover != null && hover != sel) { hover = sel; Draw(); } };
            map.Clicked += async (la, lo, rb) => { if (!rb && hover != null) await AskArea(hover); };
            Root = Ui.MapAndData(map, right);

            string f = cf.Get("b50_file", "");
            if (f.Length > 0 && File.Exists(f)) _ = LoadFile(f);
            else Empty();
        }

        public void Save()
        {
            Config cf = Station.Cfg;
            cf.Set("b50_foes", chkFoEs.IsChecked == true); cf.Set("b50_corto", chkShort.IsChecked == true);
            cf.Set("b50_quota_km", Ui.V(numH)); cf.Set("b50_file", file);
        }

        // ---------------------------------------------------------------- caricamento del log

        async Task PickFile()
        {
            TopLevel tl = TopLevel.GetTopLevel(Root);
            if (tl == null) return;
            IReadOnlyList<IStorageFile> files = await tl.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = Lang.T("Log ADIF da analizzare (anche di un'altra stazione)"),
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("ADIF") { Patterns = new[] { "*.adi", "*.adif", "*.ADI", "*.ADIF" } } }
            });
            if (files == null || files.Count == 0) return;
            string p = files[0].TryGetLocalPath();
            if (p != null) await LoadFile(p);
        }

        public async Task LoadFile(string path)
        {
            lblFile.Text = Lang.F("Leggo {0}...", Path.GetFileName(path));
            List<Dictionary<string, string>> recs = null; string err = null;
            await Task.Run(() => { try { recs = Adif.Parse(File.ReadAllBytes(path)); } catch (Exception ex) { err = ex.Message; } });
            if (recs == null) { lblFile.Text = Lang.F("Errore: {0}", err ?? ""); return; }
            file = path; raw = recs; areaMode = false; hover = sel = null;
            Rebuild();
        }

        void Unload()
        {
            file = ""; raw = new List<Dictionary<string, string>>(); all = new List<BenchQso>(); cur = new List<BenchQso>();
            hover = sel = null; areaMode = false; geo.Clear();
            Empty();
        }

        void Empty()
        {
            lblFile.Text = Lang.T("Nessun log caricato");
            busyFill = true; cmbDay.ItemsSource = new string[0]; busyFill = false;
            tQso.Rows.Clear(); tQso.Refresh(); lblSel.Text = ""; lblArea.Text = ""; bAll.IsVisible = false;
            txtDet.Text = Lang.T("Carica il log ADIF di una stazione: la linguetta mostra i QSO in 50 MHz oltre il singolo salto (più di 2400 km, tabella IK3XTV), giorno per giorno e nell'orario scelto, sulla mappa di Mercatore.");
            Draw();
        }

        static string F(Dictionary<string, string> r, string k) { string v; return r.TryGetValue(k, out v) && v != null ? v.Trim().ToUpperInvariant() : ""; }

        static string Most(IEnumerable<string> v)
        {
            return v.Where(x => x.Length > 0).GroupBy(x => x).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key.Length).Select(g => g.Key).FirstOrDefault() ?? "";
        }

        // QSO in 50 MHz con locator, salti secondo la tabella IK3XTV
        void Rebuild()
        {
            if (raw.Count == 0) { if (file.Length == 0) Empty(); return; }
            myGrid = Most(raw.Select(r => F(r, "MY_GRIDSQUARE")));
            myCall = Most(raw.Select(r => F(r, "STATION_CALLSIGN"))); if (myCall.Length == 0) myCall = Most(raw.Select(r => F(r, "OPERATOR")));
            if (myGrid.Length < 4) myGrid = Station.Loc;
            if (myCall.Length == 0) myCall = Station.Call;
            double old = EsModel.ShortHopKm;
            EsModel.ShortHopKm = chkShort.IsChecked == true ? 1200 : 0;
            List<BenchQso> list = new List<BenchQso>();
            int noGrid = 0, other = 0;
            try
            {
                foreach (Dictionary<string, string> r in raw)
                {
                    if (LogBands.Normalize(F(r, "BAND").ToLowerInvariant(), F(r, "FREQ")) != "6m") { other++; continue; }
                    string prop = F(r, "PROP_MODE");
                    if (prop.Length > 0 && prop != "ES") { other++; continue; }
                    string call = F(r, "CALL"), grid = F(r, "GRIDSQUARE");
                    int cm = grid.IndexOfAny(new[] { ',', ' ', ';' }); if (cm > 0) grid = grid.Substring(0, cm);
                    if (grid.Length > 6) grid = grid.Substring(0, 6);
                    grid = CallDb.Refine(call, grid) ?? grid;
                    if (grid.Length > 6) grid = grid.Substring(0, 6);
                    double la, lo;
                    if (grid.Length < 4 || !Geo.IsLocator(grid.Substring(0, 4)) || !Geo.LocToLatLon(grid, out la, out lo)) { noGrid++; continue; }
                    string mg = F(r, "MY_GRIDSQUARE"); if (mg.Length > 6) mg = mg.Substring(0, 6); if (mg.Length < 4) mg = myGrid;
                    double tla, tlo;
                    if (!Geo.LocToLatLon(mg, out tla, out tlo)) { noGrid++; continue; }
                    string t = F(r, "TIME_ON"); if (t.Length == 4) t += "00";
                    DateTime dt;
                    if (!DateTime.TryParseExact(F(r, "QSO_DATE") + t, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dt)) continue;
                    double f; string fs = F(r, "FREQ").Replace(',', '.');
                    double mhz = double.TryParse(fs, NumberStyles.Float, CultureInfo.InvariantCulture, out f) && f >= 50 && f <= 54 ? f : 50.15;
                    BenchQso q = new BenchQso { Utc = dt, Call = call, Grid = grid, MyCall = myCall, MyGrid = mg, Band = "6m", FreqMHz = mhz, LatTx = tla, LonTx = tlo, LatRx = la, LonRx = lo, Prop = prop };
                    q.Path = EsModel.Build(new Spot { Utc = dt, FreqKHz = mhz * 1000, DxCall = call, Spotter = myCall, LocSpotter = mg, LocDx = grid, LatS = tla, LonS = tlo, LatD = la, LonD = lo, Prop = "ES" }, Ui.V(numH));
                    if (q.Path != null) list.Add(q);
                }
            }
            finally { EsModel.ShortHopKm = old; }
            list.Sort((a, b) => a.Utc.CompareTo(b.Utc));
            all = list;
            geo.Clear();
            foreach (BenchQso q in all)
            {
                MapLine l = MapView.GreatCircle(q.LatTx, q.LonTx, q.LatRx, q.LonRx, Pal.Azure, 1);
                geo[q] = l.Pts;
            }
            int multi = all.Count(q => q.Path.Hops >= 2);
            lblFile.Text = Lang.F("{0}: {1} ({2}), {3} QSO in 50 MHz, {4} multi-salto; esclusi: {5} senza locator, {6} di altre bande o con altro PROP_MODE.",
                Path.GetFileName(file), myCall, myGrid, all.Count, multi, noGrid, other);
            // giornate con multi-salto in 50 MHz (proposta: quella con piu' QSO)
            List<string> days = all.Where(q => q.Path.Hops >= 2).Select(q => q.Day).Distinct().OrderBy(d => d).ToList();
            string keep = cmbDay.SelectedItem as string;
            busyFill = true;
            cmbDay.ItemsSource = days;
            string best = all.Where(q => q.Path.Hops >= 2).GroupBy(q => q.Day).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
            cmbDay.SelectedIndex = keep != null && days.Contains(keep) ? days.IndexOf(keep) : best != null ? days.IndexOf(best) : -1;
            busyFill = false;
            Fill();
        }

        // QSO da mostrare: giornata e orario scelti, oppure (dopo il popup) tutti quelli con il corrispondente entro 150 km
        void Fill() { Fill(true); }

        void Fill(bool frame)
        {
            if (areaMode)
                cur = all.Where(q => Geo.DistanceKm(areaLat, areaLon, q.LatRx, q.LonRx) <= AreaKm).ToList();
            else
            {
                string day = cmbDay.SelectedItem as string;
                int a = Minutes(cmbFrom.SelectedItem as string, 0), b = Minutes(cmbTo.SelectedItem as string, 24 * 60);
                cur = all.Where(q => q.Path.Hops >= 2 && q.Day == day && Mins(q) >= a && Mins(q) <= b).ToList();
            }
            if (sel != null && !cur.Contains(sel)) sel = null;
            if (hover != null && !cur.Contains(hover)) hover = sel;
            bool fo = chkFoEs.IsChecked == true;
            tQso.Rows.Clear();
            foreach (BenchQso q in cur)
                tQso.Rows.Add(new TableRow
                {
                    Cells = new[] { q.Day, q.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), q.Call, q.Grid, q.Path.DistKm.ToString("F0", CultureInfo.CurrentCulture),
                        q.Path.Hops.ToString(CultureInfo.InvariantCulture), fo ? q.Path.FoEsMin.ToString("F1", CultureInfo.CurrentCulture) : "" },
                    Tag = q
                });
            tQso.SelectedIndex = sel != null ? cur.IndexOf(sel) : -1;
            tQso.Refresh();
            lblSel.Text = areaMode ? Lang.F("{0} QSO del file con il corrispondente entro {1:F0} km da {2}.", cur.Count, AreaKm, areaName)
                : Lang.F("{0} QSO multi-salto in 50 MHz nel giorno e nell'orario scelti.", cur.Count);
            lblArea.Text = areaMode ? Lang.F("Dintorni di {0} (tutte le giornate)", areaName) : "";
            bAll.IsVisible = areaMode;
            // la mappa inquadra i QSO mostrati
            if (frame && cur.Count > 0)
            {
                double w = cur.Min(q => Math.Min(q.LonTx, q.LonRx)), e = cur.Max(q => Math.Max(q.LonTx, q.LonRx));
                double s = cur.Min(q => Math.Min(q.LatTx, q.LatRx)), n = cur.Max(q => Math.Max(q.LatTx, q.LatRx));
                if (e - w < 300) map.SetView(w - 10, e + 10, Math.Max(-75, s - 8), Math.Min(80, n + 8));
            }
            Draw(); Details();
        }

        static int Mins(BenchQso q) { return q.Utc.Hour * 60 + q.Utc.Minute; }
        static int Minutes(string t, int def)
        {
            if (string.IsNullOrEmpty(t) || t.Length < 5) return def;
            return int.Parse(t.Substring(0, 2), CultureInfo.InvariantCulture) * 60 + int.Parse(t.Substring(3, 2), CultureInfo.InvariantCulture);
        }

        // ---------------------------------------------------------------- mouse: tratta vicina, ingrandimento, popup

        void OnHover(Point pos)
        {
            BenchQso best = null; double bd = 8;
            foreach (BenchQso q in cur)
            {
                List<Point> g; if (!geo.TryGetValue(q, out g)) continue;
                Point prev = map.ToScreen(g[0].Y, g[0].X);
                for (int i = 1; i < g.Count; i++)
                {
                    Point p = map.ToScreen(g[i].Y, g[i].X);
                    if (Math.Abs(p.X - prev.X) < map.Bounds.Width / 2)
                    {
                        double d = SegDist(pos, prev, p);
                        if (d < bd) { bd = d; best = q; }
                    }
                    prev = p;
                }
                Point rx = map.ToScreen(q.LatRx, q.LonRx);
                double dr = Math.Sqrt((rx.X - pos.X) * (rx.X - pos.X) + (rx.Y - pos.Y) * (rx.Y - pos.Y));
                if (dr < bd + 4) { bd = dr; best = q; }
            }
            if (best == null) best = sel;
            if (best != hover) { hover = best; Draw(); Details(); }
        }

        static double SegDist(Point p, Point a, Point b)
        {
            double dx = b.X - a.X, dy = b.Y - a.Y, l2 = dx * dx + dy * dy;
            double t = l2 < 1e-9 ? 0 : Math.Max(0, Math.Min(1, ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / l2));
            double x = a.X + t * dx - p.X, y = a.Y + t * dy - p.Y;
            return Math.Sqrt(x * x + y * y);
        }

        async Task AskArea(BenchQso q)
        {
            string who = q.Call + " " + q.Grid;
            bool yes = Program.ScreenshotDir != null || await Dialogs.Confirm(TopLevel.GetTopLevel(Root) as Window, Lang.T("QSO nei dintorni"),
                Lang.F("Vuoi vedere tutti i QSO del file ADIF che hanno la posizione terminale entro {0:F0} km da {1}?", AreaKm, who));
            if (!yes) return;   // "No": la mappa resta com'e'
            ApplyArea(q);
        }

        void ApplyArea(BenchQso q)
        {
            areaMode = true; areaLat = q.LatRx; areaLon = q.LonRx; areaName = q.Grid + " (" + q.Call + ")";
            sel = hover = null;
            Fill();
        }

        // ---------------------------------------------------------------- disegno

        void Draw()
        {
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear(); map.Polys.Clear();
            map.Caption = Lang.T("BENCHMARK 50 MHz") + (areaMode ? "  " + Lang.F("dintorni di {0}", areaName) : cmbDay.SelectedItem is string d ? "  " + d + "  " + (cmbFrom.SelectedItem as string) + "-" + (cmbTo.SelectedItem as string) + " UTC" : "");
            bool fo = chkFoEs.IsChecked == true;
            if (areaMode) map.Lines.Add(MapView.Circle(areaLat, areaLon, AreaKm, Pal.Orange, 1.8));
            foreach (BenchQso q in cur)
            {
                if (q == hover) continue;
                Color c = fo ? EsTab.FoEsColor(q.Path.FoEsMin) : Pal.Azure;
                MapLine l = new MapLine { Color = Pal.A(fo ? 230 : 170, c), Width = 1.4 }; l.Pts.AddRange(geo[q]); map.Lines.Add(l);
                map.Markers.Add(new MapMarker { Lat = q.LatRx, Lon = q.LonRx, Radius = 3, Fill = c });
                if (q.Path.FirstA != null) map.Markers.Add(new MapMarker { Lat = q.Path.FirstA.Lat, Lon = q.Path.FirstA.Lon, Radius = 2.5, Fill = Pal.Orange, Square = true });
            }
            if (hover != null)
            {
                // tratta ingrandita: alone, linea spessa, icone grandi, WWL a 6 caratteri del corrispondente
                BenchQso q = hover;
                MapLine glow = new MapLine { Color = Pal.A(90, Pal.Yellow), Width = 11 }; glow.Pts.AddRange(geo[q]); map.Lines.Add(glow);
                MapLine l = new MapLine { Color = Pal.Orange, Width = 3.6 }; l.Pts.AddRange(geo[q]); map.Lines.Add(l);
                for (int i = 0; i < q.Path.Points.Count; i++)
                {
                    EsHop h = q.Path.Points[i];
                    map.Markers.Add(new MapMarker { Lat = h.Lat, Lon = h.Lon, Radius = 6, Fill = i == 0 ? Pal.Orange : i == q.Path.Points.Count - 1 ? Pal.Violet : Pal.Silver, Square = true,
                        Label = fo ? Lang.F("foEs ≥ {0:F1}", q.Path.FoEsMin) : null });
                }
                if (chkShort.IsChecked == true && q.Path.ShortA != null)
                    map.Markers.Add(new MapMarker { Lat = q.Path.ShortA.Lat, Lon = q.Path.ShortA.Lon, Radius = 9, Fill = Pal.Orange, Ring = true, Label = Lang.T("corto ") + Geo.LatLonToLoc(q.Path.ShortA.Lat, q.Path.ShortA.Lon, 4) });
                string wwl = q.Grid.Length >= 6 ? q.Grid.Substring(0, 6) : q.Grid + " " + Lang.T("(4 caratteri: manca in CALL3)");
                map.Markers.Add(new MapMarker { Lat = q.LatRx, Lon = q.LonRx, Radius = 9, Fill = Pal.Yellow, Stroke = Pal.Fore, Label = q.Call + "  " + wwl, LabelColor = Pal.Yellow });
                map.Info.Add(Lang.F("{0}  {1}  {2:F0} km, {3} salti", q.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) + "Z", q.Call + " " + wwl, q.Path.DistKm, q.Path.Hops));
            }
            double tla, tlo;
            if (myGrid.Length >= 4 && Geo.LocToLatLon(myGrid, out tla, out tlo))
                map.Markers.Add(new MapMarker { Lat = tla, Lon = tlo, Radius = 6, Fill = Pal.Green, Square = true, Label = myCall + " " + myGrid, LabelColor = Pal.Green });
            map.Legend.Add(new KeyValuePair<string, Color>(fo ? Lang.T("tratte colorate dalla foEs minima") : Lang.T("tratte multi-salto in 50 MHz"), fo ? Pal.Yellow : Pal.Azure));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ primo salto della stazione / ○ primo salto corto"), Pal.Orange));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("mouse vicino a una tratta: ingrandita con il WWL; clic sinistro: QSO entro 150 km"), Pal.Yellow));
            if (areaMode) map.Legend.Add(new KeyValuePair<string, Color>(Lang.F("cerchio arancio: {0:F0} km attorno al punto scelto", AreaKm), Pal.Orange));
            map.LegendTitle = fo ? Lang.T("Nubi: foEs minima <5 / 7 / 10 / 14 / 20 / >20 MHz = blu, azzurro, verde, giallo, arancio, rosso") : "";
            map.InvalidateVisual();
        }

        void Details()
        {
            BenchQso q = hover ?? sel;
            if (q == null)
            {
                if (all.Count > 0) txtDet.Text = Lang.T("Avvicina il mouse a una tratta per ingrandirla e leggere il quadrato WWL a 6 caratteri; con il clic sinistro puoi vedere tutti i QSO del file con il corrispondente entro 150 km.");
                return;
            }
            EsPath p = q.Path;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Lang.F("{0}  {1} ({2}) - {3} ({4})  {5} MHz", q.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) + "Z", q.MyCall, q.MyGrid, q.Call, q.Grid, q.FreqMHz.ToString("F3", CultureInfo.CurrentCulture)));
            sb.AppendLine(Lang.F("Distanza {0:F0} km - {1} (tabella IK3XTV)", p.DistKm, Lang.T(p.Judgement)));
            sb.AppendLine(Lang.F("Azimut: {0} -> {1}: {2:F0}°   {1} -> {0}: {3:F0}°", q.MyCall, q.Call, Geo.Bearing(q.LatTx, q.LonTx, q.LatRx, q.LonRx), Geo.Bearing(q.LatRx, q.LonRx, q.LatTx, q.LonTx)));
            if (chkFoEs.IsChecked == true)
                sb.AppendLine(Lang.F("Salti uguali: {0} x {1:F0} km, elevazione {2:F1}°, sec(φ) = {3:F2}, foEs ≥ {4:F1} MHz", p.Hops, p.HopKm, p.ElevDeg, p.SecPhi, p.FoEsMin));
            else sb.AppendLine(Lang.F("Salti uguali: {0} x {1:F0} km", p.Hops, p.HopKm));
            for (int i = 0; i < p.Points.Count; i++)
                sb.AppendLine(Lang.F("  salto {0}: {1}", i + 1, Geo.LatLonToLoc(p.Points[i].Lat, p.Points[i].Lon, 6)));
            if (p.ShortA != null && chkShort.IsChecked == true)
                sb.AppendLine(chkFoEs.IsChecked == true ? Lang.F("Primo salto corto (1200 km, esperienza IC8TEM): {0}, foEs ≥ {1:F1} MHz", Geo.LatLonToLoc(p.ShortA.Lat, p.ShortA.Lon, 6), p.ShortFoEsMin)
                    : Lang.F("Primo salto corto (1200 km, esperienza IC8TEM): {0}", Geo.LatLonToLoc(p.ShortA.Lat, p.ShortA.Lon, 6)));
            txtDet.Text = sb.ToString();
        }

        // ---------------------------------------------------------------- fotografie automatiche

        public void ShotHover(int i) { if (i >= 0 && i < cur.Count) { hover = sel = cur[i]; Draw(); Details(); } }
        public void ShotArea(int i) { if (i >= 0 && i < cur.Count) ApplyArea(cur[i]); }
        public void ShotFoEs(bool on) { chkFoEs.IsChecked = on; }

        // log di prova: due giornate di multi-salto in 50 MHz da JN70CN (Nord America, Caraibi, Sud America, Giappone)
        public static string DemoAdif()
        {
            string[][] q =
            {
                new[] { "20250621", "1612", "W1JJ", "FN42ET" }, new[] { "20250621", "1618", "K1TOL", "FN43" }, new[] { "20250621", "1625", "W2XYZ", "FN31PR" },
                new[] { "20250621", "1631", "N1ABC", "FN42HN" }, new[] { "20250621", "1640", "VE1SKY", "FN74MP" }, new[] { "20250621", "1702", "KP4EU", "FK68VJ" },
                new[] { "20250621", "1715", "W4ABC", "EM95" }, new[] { "20250621", "1730", "VE3XYZ", "FN03" }, new[] { "20250621", "1810", "PY2XB", "GG66RO" },
                new[] { "20250621", "1825", "LU5FF", "GF05" }, new[] { "20250621", "0540", "JA1ABC", "PM95VR" }, new[] { "20250621", "0602", "JA3XYZ", "PM74" },
                new[] { "20250622", "1530", "W1AW", "FN31PR" }, new[] { "20250622", "1548", "K2ABC", "FN30" }, new[] { "20250622", "1601", "VO1XYZ", "GN37" },
                new[] { "20250622", "1820", "9Y4ABC", "FK90" }, new[] { "20250621", "1100", "EA8DO", "IL18QI" }, new[] { "20250621", "1210", "TF3ABC", "HP94" }
            };
            StringBuilder sb = new StringBuilder("<adif_ver:5>3.1.4<eoh>\n");
            foreach (string[] r in q)
                sb.Append("<station_callsign:6>IC8TEM<my_gridsquare:6>JN70CN<call:" + r[2].Length + ">" + r[2] + "<gridsquare:" + r[3].Length + ">" + r[3]
                    + "<band:2>6m<freq:6>50.313<mode:3>FT8<qso_date:8>" + r[0] + "<time_on:4>" + r[1] + "<eor>\n");
            return sb.ToString();
        }
    }
}
