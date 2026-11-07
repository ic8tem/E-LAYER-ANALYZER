// E-LAYER ANALYZER - linguetta BENCHMARK Es: banco di prova dello sporadico E e del multi-salto su un log ADIF intero,
// anche di un'altra stazione. Dal log: posizione di trasmissione (MY_GRIDSQUARE, o il locator scritto nel riquadro),
// posizioni di ricezione (GRIDSQUARE), QSO per giornata e per ora. Per la selezione: skip abbozzato per settori di
// azimut (distanza minima e massima raggiunte), nubi Es sui quadrati Maidenhead, foEs stimata con la legge della
// secante, verifica delle ipotesi del multi-salto (salti uguali / primo salto corto, IC8TEM) contro le nubi viste in
// singolo salto nella stessa ora, spostamento orario delle nubi. Mappa azimutale centrata sulla stazione, con lo zoom.
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
    public class BenchQso
    {
        public DateTime Utc;
        public string Call = "", Grid = "", MyCall = "", MyGrid = "", Band = "", Mode = "", Prop = "";
        public double FreqMHz, LatTx = double.NaN, LonTx = double.NaN, LatRx, LonRx;
        public EsPath Path;          // salti secondo la tabella IK3XTV (e primo salto corto)
        public string Day { get { return Utc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); } }
    }

    public class BenchTab
    {
        public readonly Control Root;
        readonly MapView map = new MapView { Azimuthal = true };
        readonly RangeBar range;
        readonly TextBox txtCall, txtLoc;
        readonly TextBlock lblFile;
        readonly CheckBox c10, c6, c4, c2, chkProp, chkMin, chkShort, chkSkip, chkPaths;
        readonly NumericUpDown numH, numShort;
        readonly TableView tDays, tHours, tQso;
        readonly SelectableTextBlock txtCalc;
        readonly PillTabs inner = new PillTabs();
        static readonly string[] EsBands = { "10m", "6m", "4m", "2m" };

        List<BenchQso> all = new List<BenchQso>();
        List<BenchQso> cur = new List<BenchQso>();
        string file = "";
        int ignored;
        string selDay;
        int selHour = -1;            // -1 = tutta la giornata
        BenchQso selQso;
        double cLat = double.NaN, cLon = double.NaN;

        public BenchTab()
        {
            Config cf = Station.Cfg;
            Button bLoad = Ui.B(Lang.T("Carica ADIF"), true);
            bLoad.Click += async (s, e) => await PickFile();
            Button bUnload = Ui.B(Lang.T("Scarica ADIF"));
            bUnload.Click += (s, e) => Unload();
            lblFile = new TextBlock { Foreground = Pal.B(Pal.Muted), VerticalAlignment = VerticalAlignment.Center, TextWrapping = TextWrapping.Wrap, MaxWidth = 380 };

            txtCall = Ui.T("", 95, true);
            txtLoc = Ui.T("", 80, true);
            Button bCentre = Ui.B(Lang.T("Applica"));
            bCentre.Click += (s, e) => { SetCentre(); Analyze(); };

            c10 = Ui.C("10 m", cf.GetB("bench_10m", true)); c6 = Ui.C("6 m", cf.GetB("bench_6m", true));
            c4 = Ui.C("4 m", cf.GetB("bench_4m", true)); c2 = Ui.C("2 m", cf.GetB("bench_2m", true));
            numH = Ui.N(90, 130, Ui.Dm(cf.GetD("bench_quota_km", 105)), 0, 1, 45);
            chkProp = Ui.C("", cf.GetB("bench_prop", true));
            chkProp.Content = new TextBlock { Text = Lang.T("Escludi i QSO con un altro PROP_MODE (TR, MS, FAI, AUR, EME...)"), TextWrapping = TextWrapping.Wrap };   // testo semplice: "_" non e' un tasto di scelta rapida
            chkMin = Ui.C(Lang.T("Escludi i QSO sotto i 500 km"), cf.GetB("bench_min", true));
            chkShort = Ui.C(Lang.T("Primo salto corto entro"), cf.GetB("bench_corto", true));
            numShort = Ui.N(400, 2400, Ui.Dm(cf.GetD("bench_corto_km", 1200)), 0, 50, 55);
            chkSkip = Ui.C(Lang.T("Skip per settori"), cf.GetB("bench_skip", true));
            chkPaths = Ui.C(Lang.T("Tratte"), cf.GetB("bench_tratte", true));
            foreach (CheckBox c in new[] { c10, c6, c4, c2, chkProp, chkMin, chkShort }) c.IsCheckedChanged += (s, e) => Rebuild();
            foreach (CheckBox c in new[] { chkSkip, chkPaths }) c.IsCheckedChanged += (s, e) => Draw();
            foreach (NumericUpDown n in new[] { numH, numShort }) n.ValueChanged += (s, e) => Rebuild();

            tDays = new TableView();
            tDays.Columns.AddRange(new[] { Lang.T("Giornata"), "QSO", Lang.T("Bande"), Lang.T("km max"), Lang.T("Multi-salto") });
            tDays.ColumnWidths.AddRange(new double[] { 92, 46, 120, 64, 70 });
            tDays.RowClicked += i => { if (i >= 0 && i < tDays.Rows.Count) { selDay = tDays.Rows[i].Tag as string; selHour = -1; selQso = null; FillHours(); Analyze(); } };
            tHours = new TableView();
            tHours.Columns.AddRange(new[] { Lang.T("Ora UTC"), "QSO", Lang.T("km min - max"), Lang.T("Multi-salto"), Lang.T("Bande") });
            tHours.ColumnWidths.AddRange(new double[] { 130, 46, 100, 70, 100 });
            tHours.RowClicked += i => { if (i >= 0 && i < tHours.Rows.Count) { selHour = (int)tHours.Rows[i].Tag; selQso = null; Analyze(); } };
            tQso = new TableView();
            tQso.Columns.AddRange(new[] { "UTC", Lang.T("Nominativo"), "Locator", "MHz", "km", Lang.T("Salti"), "foEs≥" });
            tQso.ColumnWidths.AddRange(new double[] { 46, 84, 64, 62, 52, 40, 52 });
            tQso.RowClicked += i => { if (i >= 0 && i < tQso.Rows.Count) { selQso = tQso.Rows[i].Tag as BenchQso; Draw(); Calc(); inner.Select(0); } };

            txtCalc = Ui.Text(12.5);
            ScrollViewer svCalc = new ScrollViewer { Content = txtCalc, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            inner.Add(Lang.T("Calcoli proposti"), svCalc);
            inner.Add("QSO", tQso);

            Border boxDays = Ui.Box(Lang.T("QSO per giornata"), Pal.Azure, tDays); boxDays.Height = 175;
            Border boxHours = Ui.Box(Lang.T("Orari della giornata (clic: QSO di quell'ora)"), Pal.Azure, tHours); boxHours.Height = 175;

            DockPanel right = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(right, Ui.Row(bLoad, bUnload, lblFile), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Stazione al centro")), txtCall, txtLoc, bCentre), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Bande")), c10, c6, c4, c2, Ui.L(Lang.T("Quota Es km")), numH), Dock.Top);
            Ui.Dock(right, Ui.Row(chkShort, numShort, Ui.L("km"), chkMin), Dock.Top);
            Ui.Dock(right, Ui.Row(chkProp), Dock.Top);
            Ui.Dock(right, boxDays, Dock.Top);
            Ui.Dock(right, boxHours, Dock.Top);
            Ui.Dock(right, inner.Bar, Dock.Top);
            right.Children.Add(inner.Host);

            range = new RangeBar(map, cf.GetD("bench_raggio_km", 3000));
            range.Changed += km => Draw();
            map.Clicked += (la, lo, rb) => ClickMap(la, lo);
            DockPanel mapHost = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(mapHost, Ui.Row(range.Root, chkSkip, chkPaths), Dock.Top);
            mapHost.Children.Add(map);
            Root = Ui.MapAndData(mapHost, right);

            string f = cf.Get("bench_file", "");
            if (f.Length > 0 && File.Exists(f)) _ = LoadFile(f);
            else ShowEmpty();
        }

        public void Save()
        {
            Config cf = Station.Cfg;
            cf.Set("bench_10m", c10.IsChecked == true); cf.Set("bench_6m", c6.IsChecked == true); cf.Set("bench_4m", c4.IsChecked == true); cf.Set("bench_2m", c2.IsChecked == true);
            cf.Set("bench_quota_km", Ui.V(numH)); cf.Set("bench_prop", chkProp.IsChecked == true); cf.Set("bench_min", chkMin.IsChecked == true);
            cf.Set("bench_corto", chkShort.IsChecked == true); cf.Set("bench_corto_km", Ui.V(numShort));
            cf.Set("bench_skip", chkSkip.IsChecked == true); cf.Set("bench_tratte", chkPaths.IsChecked == true);
            cf.Set("bench_raggio_km", range.Km); cf.Set("bench_file", file);
        }

        double H { get { return Ui.V(numH); } }

        // ---------------------------------------------------------------- caricamento

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
            List<Dictionary<string, string>> recs = null;
            string err = null;
            await Task.Run(() => { try { recs = Adif.Parse(File.ReadAllBytes(path)); } catch (Exception ex) { err = ex.Message; } });
            if (recs == null) { lblFile.Text = Lang.F("Errore: {0}", err ?? ""); return; }
            file = path;
            raw = recs;
            // stazione: il MY_GRIDSQUARE e lo STATION_CALLSIGN (o OPERATOR) piu' frequenti del log
            string g = Most(recs, "MY_GRIDSQUARE"), c = Most(recs, "STATION_CALLSIGN");
            if (c.Length == 0) c = Most(recs, "OPERATOR");
            txtLoc.Text = g.Length >= 4 ? g : Station.Loc;
            txtCall.Text = c.Length > 0 ? c : g.Length >= 4 ? "" : Station.Call;
            SetCentre();
            selDay = null; selHour = -1; selQso = null;
            Rebuild();
        }

        List<Dictionary<string, string>> raw = new List<Dictionary<string, string>>();

        static string Field(Dictionary<string, string> f, string k) { string v; return f.TryGetValue(k, out v) && v != null ? v.Trim().ToUpperInvariant() : ""; }

        static string Most(List<Dictionary<string, string>> recs, string k)
        {
            Dictionary<string, int> n = new Dictionary<string, int>();
            foreach (Dictionary<string, string> r in recs)
            {
                string v = Field(r, k);
                if (v.Length == 0) continue;
                int x; n.TryGetValue(v, out x); n[v] = x + 1;
            }
            string best = ""; int bn = 0;
            foreach (KeyValuePair<string, int> kv in n) if (kv.Value > bn || (kv.Value == bn && kv.Key.Length > best.Length)) { best = kv.Key; bn = kv.Value; }
            return best;
        }

        void Unload()
        {
            file = ""; raw = new List<Dictionary<string, string>>(); all = new List<BenchQso>(); cur = new List<BenchQso>();
            selDay = null; selHour = -1; selQso = null;
            ShowEmpty();
        }

        void ShowEmpty()
        {
            lblFile.Text = Lang.T("Nessun log caricato");
            tDays.Rows.Clear(); tDays.Refresh(); tHours.Rows.Clear(); tHours.Refresh(); tQso.Rows.Clear(); tQso.Refresh();
            if (Station.Valid && double.IsNaN(cLat)) { txtCall.Text = Station.Call; txtLoc.Text = Station.Loc; SetCentre(); }
            txtCalc.Text = Lang.T("Carica un log ADIF (anche di un'altra stazione): il programma trova la posizione di trasmissione (MY_GRIDSQUARE, oppure il locator scritto qui sopra) e quelle di ricezione (GRIDSQUARE), divide i QSO per giornata e per ora e propone i calcoli dello sporadico E e del multi-salto.");
            Draw();
        }

        void SetCentre()
        {
            double la, lo;
            string loc = Ui.Up(txtLoc);
            if (Geo.IsLocator(loc) && Geo.LocToLatLon(loc, out la, out lo)) { cLat = la; cLon = lo; }
            else if (Station.Valid) { cLat = Station.Lat; cLon = Station.Lon; }
            else { cLat = 45; cLon = 12; }
            map.AzLat = cLat; map.AzLon = cLon;
        }

        static double DefaultMHz(string band)
        {
            switch (band) { case "10m": return 28.5; case "6m": return 50.15; case "4m": return 70.2; case "2m": return 144.3; default: return 0; }
        }

        static readonly string[] OtherProp = { "TR", "MS", "FAI", "AUR", "AUE", "EME", "F2", "TEP", "RS", "SAT", "LOS", "ION", "IRL", "INTERNET", "ECH", "BS" };

        // ricostruisce i QSO utili dai record ADIF con le opzioni correnti
        void Rebuild()
        {
            if (raw.Count == 0) { if (file.Length == 0) ShowEmpty(); return; }
            SetCentre();
            double oldShort = EsModel.ShortHopKm;
            EsModel.ShortHopKm = chkShort.IsChecked == true ? Ui.V(numShort) : 0;
            List<BenchQso> list = new List<BenchQso>();
            ignored = 0;
            int noGrid = 0, otherBand = 0, otherProp = 0, tooShort = 0;
            try
            {
                foreach (Dictionary<string, string> r in raw)
                {
                    string band = LogBands.Normalize(Field(r, "BAND").ToLowerInvariant(), Field(r, "FREQ"));
                    if (Array.IndexOf(EsBands, band) < 0 || !BandOn(band)) { otherBand++; continue; }
                    string prop = Field(r, "PROP_MODE");
                    if (chkProp.IsChecked == true && prop.Length > 0 && prop != "ES" && Array.IndexOf(OtherProp, prop) >= 0) { otherProp++; continue; }
                    string grid = Field(r, "GRIDSQUARE");
                    if (grid.Length > 6) grid = grid.Substring(0, 6);
                    double la, lo;
                    if (grid.Length < 4 || !Geo.IsLocator(grid.Substring(0, 4)) || !Geo.LocToLatLon(grid, out la, out lo)) { noGrid++; continue; }
                    BenchQso q = new BenchQso { Call = Field(r, "CALL"), Grid = grid, Band = band, Mode = Field(r, "MODE"), Prop = prop, LatRx = la, LonRx = lo };
                    q.MyGrid = Field(r, "MY_GRIDSQUARE"); if (q.MyGrid.Length > 6) q.MyGrid = q.MyGrid.Substring(0, 6);
                    q.MyCall = Field(r, "STATION_CALLSIGN"); if (q.MyCall.Length == 0) q.MyCall = Field(r, "OPERATOR"); if (q.MyCall.Length == 0) q.MyCall = Ui.Up(txtCall);
                    double mla, mlo;
                    if (q.MyGrid.Length >= 4 && Geo.LocToLatLon(q.MyGrid, out mla, out mlo)) { q.LatTx = mla; q.LonTx = mlo; }
                    else { q.LatTx = cLat; q.LonTx = cLon; q.MyGrid = Ui.Up(txtLoc); }
                    double f;
                    string fs = Field(r, "FREQ").Replace(',', '.');
                    q.FreqMHz = double.TryParse(fs, NumberStyles.Float, CultureInfo.InvariantCulture, out f) && LogBands.Normalize("", fs) == band ? f : DefaultMHz(band);
                    string d = Field(r, "QSO_DATE"), t = Field(r, "TIME_ON");
                    DateTime dt;
                    if (t.Length == 4) t += "00";
                    if (!DateTime.TryParseExact(d + t, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dt)) { ignored++; continue; }
                    q.Utc = dt;
                    Spot s = new Spot { Utc = dt, FreqKHz = q.FreqMHz * 1000, DxCall = q.Call, Spotter = q.MyCall, LocSpotter = q.MyGrid, LocDx = q.Grid, LatS = q.LatTx, LonS = q.LonTx, LatD = la, LonD = lo, Prop = "ES" };
                    q.Path = EsModel.Build(s, H);
                    if (q.Path == null) { ignored++; continue; }
                    if (chkMin.IsChecked == true && q.Path.Hops == 0) { tooShort++; continue; }
                    list.Add(q);
                }
            }
            finally { EsModel.ShortHopKm = oldShort; }
            list.Sort((a, b) => a.Utc.CompareTo(b.Utc));
            all = list;
            ignored += noGrid + otherBand + otherProp + tooShort;
            lblFile.Text = Lang.F("{0}: {1} record, {2} QSO analizzati; esclusi: {3} senza locator, {4} di altre bande (432 MHz compreso: lo sporadico E non si applica), {5} con altro PROP_MODE, {6} sotto i 500 km.",
                Path.GetFileName(file), raw.Count, all.Count, noGrid, otherBand, otherProp, tooShort);
            FillDays();
            if (selDay == null || !all.Any(q => q.Day == selDay))
            {
                // giornata proposta: quella con piu' QSO
                selDay = all.GroupBy(q => q.Day).OrderByDescending(gr => gr.Count()).Select(gr => gr.Key).FirstOrDefault();
                selHour = -1;
            }
            FillDays();
            FillHours();
            Analyze();
        }

        bool BandOn(string b)
        {
            return (b == "10m" && c10.IsChecked == true) || (b == "6m" && c6.IsChecked == true) || (b == "4m" && c4.IsChecked == true) || (b == "2m" && c2.IsChecked == true);
        }

        static string BandList(IEnumerable<BenchQso> qs)
        {
            List<string> b = qs.Select(q => q.Band).Distinct().OrderBy(x => Array.IndexOf(EsBands, x)).ToList();
            return string.Join(" ", b);
        }

        void FillDays()
        {
            tDays.Rows.Clear();
            foreach (IGrouping<string, BenchQso> g in all.GroupBy(q => q.Day))
            {
                tDays.Rows.Add(new TableRow
                {
                    Cells = new[] { g.Key, g.Count().ToString(CultureInfo.InvariantCulture), BandList(g), g.Max(q => q.Path.DistKm).ToString("F0", CultureInfo.CurrentCulture), g.Count(q => q.Path.Hops >= 2).ToString(CultureInfo.InvariantCulture) },
                    Fore = g.Any(q => q.Band == "2m") ? Pal.Orange : g.Any(q => q.Path.Hops >= 2) ? Pal.Violet : Pal.Fore,
                    Tag = g.Key
                });
            }
            tDays.SelectedIndex = tDays.Rows.FindIndex(r => (r.Tag as string) == selDay);
            tDays.Refresh();
        }

        void FillHours()
        {
            tHours.Rows.Clear();
            List<BenchQso> day = all.Where(q => q.Day == selDay).ToList();
            if (day.Count > 0)
            {
                tHours.Rows.Add(new TableRow
                {
                    Cells = new[] { Lang.T("Tutta la giornata"), day.Count.ToString(CultureInfo.InvariantCulture), Km(day), day.Count(q => q.Path.Hops >= 2).ToString(CultureInfo.InvariantCulture), BandList(day) },
                    Bold = true, Tag = -1
                });
                foreach (IGrouping<int, BenchQso> g in day.GroupBy(q => q.Utc.Hour).OrderBy(x => x.Key))
                    tHours.Rows.Add(new TableRow
                    {
                        Cells = new[] { g.Key.ToString("00", CultureInfo.InvariantCulture) + ":00-" + g.Key.ToString("00", CultureInfo.InvariantCulture) + ":59", g.Count().ToString(CultureInfo.InvariantCulture), Km(g), g.Count(q => q.Path.Hops >= 2).ToString(CultureInfo.InvariantCulture), BandList(g) },
                        Fore = g.Any(q => q.Band == "2m") ? Pal.Orange : g.Any(q => q.Path.Hops >= 2) ? Pal.Violet : Pal.Fore,
                        Tag = g.Key
                    });
            }
            tHours.SelectedIndex = tHours.Rows.FindIndex(r => (int)r.Tag == selHour);
            tHours.Refresh();
        }

        static string Km(IEnumerable<BenchQso> qs)
        {
            return qs.Min(q => q.Path.DistKm).ToString("F0", CultureInfo.CurrentCulture) + " - " + qs.Max(q => q.Path.DistKm).ToString("F0", CultureInfo.CurrentCulture);
        }

        // ---------------------------------------------------------------- analisi della selezione

        class Sector { public int Az; public BenchQso Min, Max; public int N; }
        class Cloud { public string Sq; public double Lat, Lon, FoEs; public int N; public List<string> Calls = new List<string>(); }
        List<Sector> sectors = new List<Sector>();
        List<Cloud> clouds = new List<Cloud>();
        const int SectorDeg = 10;

        void Analyze()
        {
            cur = all.Where(q => q.Day == selDay && (selHour < 0 || q.Utc.Hour == selHour)).ToList();
            tHours.SelectedIndex = tHours.Rows.FindIndex(r => (int)r.Tag == selHour);
            tHours.Refresh();
            tQso.Rows.Clear();
            foreach (BenchQso q in cur)
                tQso.Rows.Add(new TableRow
                {
                    Cells = new[] { q.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), q.Call, q.Grid, q.FreqMHz.ToString("F3", CultureInfo.CurrentCulture), q.Path.DistKm.ToString("F0", CultureInfo.CurrentCulture),
                        q.Path.Hops.ToString(CultureInfo.InvariantCulture), q.Path.FoEsMin.ToString("F1", CultureInfo.CurrentCulture) },
                    Fore = EsTab.BandColor(q.Band),
                    Tag = q
                });
            tQso.SelectedIndex = selQso != null ? cur.IndexOf(selQso) : -1;
            tQso.Refresh();

            // skip per settori di azimut dal centro: distanza minima e massima raggiunte
            Dictionary<int, Sector> sec = new Dictionary<int, Sector>();
            foreach (BenchQso q in cur)
            {
                int a = (int)(Geo.Bearing(cLat, cLon, q.LatRx, q.LonRx) / SectorDeg) * SectorDeg;
                Sector s;
                if (!sec.TryGetValue(a, out s)) { s = new Sector { Az = a }; sec[a] = s; }
                s.N++;
                if (s.Min == null || q.Path.DistKm < s.Min.Path.DistKm) s.Min = q;
                if (s.Max == null || q.Path.DistKm > s.Max.Path.DistKm) s.Max = q;
            }
            sectors = sec.Values.OrderBy(x => x.Az).ToList();

            // nubi: punto medio dei singoli salti, primo salto (lato della stazione) dei multi-salto
            Dictionary<string, Cloud> d = new Dictionary<string, Cloud>();
            foreach (BenchQso q in cur)
                foreach (KeyValuePair<EsHop, double> kv in ReflectionPoints(q))
                {
                    string sq = Geo.LatLonToLoc(kv.Key.Lat, kv.Key.Lon, 4);
                    Cloud c;
                    if (!d.TryGetValue(sq, out c)) { double la, lo; Geo.LocToLatLon(sq, out la, out lo); c = new Cloud { Sq = sq, Lat = la, Lon = lo }; d[sq] = c; }
                    c.N++;
                    if (kv.Value > c.FoEs) c.FoEs = kv.Value;
                    if (c.Calls.Count < 5 && !c.Calls.Contains(q.Call)) c.Calls.Add(q.Call);
                }
            clouds = d.Values.OrderByDescending(c => c.N).ThenByDescending(c => c.FoEs).ToList();
            Draw();
            Calc();
        }

        List<KeyValuePair<EsHop, double>> ReflectionPoints(BenchQso q)
        {
            List<KeyValuePair<EsHop, double>> pts = new List<KeyValuePair<EsHop, double>>();
            EsPath p = q.Path;
            if (p.Hops == 1) pts.Add(new KeyValuePair<EsHop, double>(p.FirstA, p.FoEsMin));
            else if (p.Hops >= 2)
            {
                if (chkShort.IsChecked == true && p.ShortA != null) pts.Add(new KeyValuePair<EsHop, double>(p.ShortA, p.ShortFoEsMin));
                else pts.Add(new KeyValuePair<EsHop, double>(p.FirstA, p.FoEsMin));
            }
            return pts;
        }

        // poligono del settore di corona tra d1 e d2 km, azimut a1..a2 dal centro
        MapPoly SectorPoly(double a1, double a2, double d1, double d2, Color fill)
        {
            MapPoly pg = new MapPoly { Fill = fill };
            d1 = Math.Max(1, d1);
            int nr = Math.Max(1, (int)Math.Ceiling((d2 - d1) / 150.0));   // lati radiali in piu' punti (la mappa spezza i salti lunghi)
            for (double a = a1; a <= a2 + 0.01; a += 1) { double la, lo; Geo.Destination(cLat, cLon, a, d2, out la, out lo); pg.Pts.Add(new Point(lo, la)); }
            for (int k = 1; k < nr; k++) { double la, lo; Geo.Destination(cLat, cLon, a2, d2 - (d2 - d1) * k / nr, out la, out lo); pg.Pts.Add(new Point(lo, la)); }
            for (double a = a2; a >= a1 - 0.01; a -= 1) { double la, lo; Geo.Destination(cLat, cLon, a, d1, out la, out lo); pg.Pts.Add(new Point(lo, la)); }
            for (int k = 1; k < nr; k++) { double la, lo; Geo.Destination(cLat, cLon, a1, d1 + (d2 - d1) * k / nr, out la, out lo); pg.Pts.Add(new Point(lo, la)); }
            return pg;
        }

        MapLine Arc(double a1, double a2, double dkm, Color c, double w)
        {
            MapLine l = new MapLine { Color = c, Width = w };
            for (double a = a1; a <= a2 + 0.01; a += 1) { double la, lo; Geo.Destination(cLat, cLon, a, dkm, out la, out lo); l.Pts.Add(new Point(lo, la)); }
            return l;
        }

        void Draw()
        {
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear(); map.Polys.Clear();
            map.Caption = Lang.T("BENCHMARK Es") + (selDay != null ? "  " + selDay + (selHour >= 0 ? "  " + selHour.ToString("00", CultureInfo.InvariantCulture) + ":00-" + selHour.ToString("00", CultureInfo.InvariantCulture) + ":59 UTC" : "") : "");
            if (double.IsNaN(cLat)) SetCentre();
            if (chkSkip.IsChecked == true)
                foreach (Sector s in sectors)
                {
                    double dmin = s.Min.Path.DistKm, dmax = s.Max.Path.DistKm;
                    map.Polys.Add(SectorPoly(s.Az, s.Az + SectorDeg, dmin, Math.Max(dmax, dmin + 30), Pal.A(55, Pal.Green)));
                    map.Polys.Add(SectorPoly(s.Az, s.Az + SectorDeg, 0, dmin, Pal.A(28, Pal.Silver)));
                    map.Lines.Add(Arc(s.Az, s.Az + SectorDeg, dmin, Pal.Orange, 2.6));
                    map.Lines.Add(Arc(s.Az, s.Az + SectorDeg, dmax, Pal.Green, 1.6));
                }
            foreach (Cloud c in clouds)
            {
                double la0 = Math.Floor(c.Lat), lo0 = Math.Floor(c.Lon / 2) * 2;
                map.Cells.Add(new MapCell { Lat0 = la0, Lon0 = lo0, DLat = 1, DLon = 2, Fill = EsTab.FoEsColor(c.FoEs) });
            }
            EsTab.AddHopZones(map, cLat, cLon, range.Km > 3500);
            foreach (BenchQso q in cur)
            {
                bool isSel = q == selQso;
                Color bc = EsTab.BandColor(q.Band);
                if (chkPaths.IsChecked == true || isSel)
                    map.Lines.Add(MapView.GreatCircle(q.LatTx, q.LonTx, q.LatRx, q.LonRx, isSel ? Pal.Yellow : Pal.A(150, bc), isSel ? 3 : 1.2));
                map.Markers.Add(new MapMarker { Lat = q.LatRx, Lon = q.LonRx, Radius = isSel ? 5 : 3, Fill = bc, Label = isSel || cur.Count <= 25 ? q.Call : null });
                EsPath p = q.Path;
                if (p.Hops == 1) map.Markers.Add(new MapMarker { Lat = p.FirstA.Lat, Lon = p.FirstA.Lon, Radius = isSel ? 5 : 3, Fill = Pal.Yellow, Square = true });
                else if (p.Hops >= 2)
                {
                    map.Markers.Add(new MapMarker { Lat = p.FirstA.Lat, Lon = p.FirstA.Lon, Radius = isSel ? 5 : 3, Fill = Pal.Orange, Square = true, Label = isSel ? Geo.LatLonToLoc(p.FirstA.Lat, p.FirstA.Lon, 4) : null });
                    map.Markers.Add(new MapMarker { Lat = p.FirstB.Lat, Lon = p.FirstB.Lon, Radius = isSel ? 5 : 3, Fill = Pal.Violet, Square = true });
                    if (p.ShortA != null && chkShort.IsChecked == true)
                        map.Markers.Add(new MapMarker { Lat = p.ShortA.Lat, Lon = p.ShortA.Lon, Radius = isSel ? 7 : 5, Fill = Pal.Orange, Ring = true, Label = isSel ? Lang.T("corto ") + Geo.LatLonToLoc(p.ShortA.Lat, p.ShortA.Lon, 4) : null });
                }
            }
            // spostamento orario delle nubi (baricentro dei punti medi dei singoli salti), solo sulla giornata intera
            if (selHour < 0)
            {
                List<KeyValuePair<int, Point>> tr = HourCentroids(cur);
                if (tr.Count >= 2)
                {
                    MapLine l = new MapLine { Color = Pal.Red, Width = 2.2 };
                    foreach (KeyValuePair<int, Point> kv in tr) l.Pts.Add(kv.Value);
                    map.Lines.Add(l);
                }
                foreach (KeyValuePair<int, Point> kv in tr)
                    map.Markers.Add(new MapMarker { Lat = kv.Value.Y, Lon = kv.Value.X, Radius = 4, Fill = Pal.Red, Label = kv.Key.ToString("00", CultureInfo.InvariantCulture) + "Z", LabelColor = Pal.Red });
            }
            string me = Ui.Up(txtCall); if (me.Length == 0) me = Ui.Up(txtLoc);
            map.Markers.Add(new MapMarker { Lat = cLat, Lon = cLon, Radius = 5, Fill = Pal.Green, Square = true, Label = me, LabelColor = Pal.Green });
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("10 m / 6 m / 4 m / 2 m: verde, azzurro, viola, arancio"), Pal.Azure));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ giallo: punto medio (singolo salto)"), Pal.Yellow));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ primo salto della stazione / ○ primo salto corto"), Pal.Orange));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ primo salto del corrispondente"), Pal.Violet));
            if (chkSkip.IsChecked == true)
            {
                map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("arco arancio: skip (QSO più vicino del settore)"), Pal.Orange));
                map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("settore verde: distanze raggiunte"), Pal.A(140, Pal.Green)));
            }
            if (selHour < 0) map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("rosso: spostamento orario delle nubi (baricentro dei punti medi)"), Pal.Red));
            map.LegendTitle = Lang.T("Nubi: foEs minima <5 / 7 / 10 / 14 / 20 / >20 MHz = blu, azzurro, verde, giallo, arancio, rosso");
            map.InvalidateVisual();
        }

        // baricentro dei punti medi dei singoli salti, ora per ora
        static List<KeyValuePair<int, Point>> HourCentroids(List<BenchQso> qs)
        {
            List<KeyValuePair<int, Point>> r = new List<KeyValuePair<int, Point>>();
            foreach (IGrouping<int, BenchQso> g in qs.Where(q => q.Path.Hops == 1).GroupBy(q => q.Utc.Hour).OrderBy(k => k.Key))
            {
                double x = 0, y = 0, z = 0;
                foreach (BenchQso q in g)
                {
                    double la = q.Path.FirstA.Lat * Geo.D2R, lo = q.Path.FirstA.Lon * Geo.D2R;
                    x += Math.Cos(la) * Math.Cos(lo); y += Math.Cos(la) * Math.Sin(lo); z += Math.Sin(la);
                }
                double lat = Math.Atan2(z, Math.Sqrt(x * x + y * y)) * Geo.R2D, lon = Math.Atan2(y, x) * Geo.R2D;
                r.Add(new KeyValuePair<int, Point>(g.Key, new Point(lon, lat)));
            }
            return r;
        }

        void ClickMap(double la, double lo)
        {
            // il QSO piu' vicino al clic (posizione del corrispondente o punto di riflessione)
            BenchQso best = null; double bd = double.MaxValue;
            foreach (BenchQso q in cur)
            {
                double d = Math.Min(Geo.DistanceKm(la, lo, q.LatRx, q.LonRx), Geo.DistanceKm(la, lo, q.Path.FirstA.Lat, q.Path.FirstA.Lon));
                if (d < bd) { bd = d; best = q; }
            }
            if (best != null && bd < Math.Max(150, range.Km / 15)) { selQso = best; tQso.SelectedIndex = cur.IndexOf(best); tQso.Refresh(); Draw(); Calc(); inner.Select(0); }
        }

        // ---------------------------------------------------------------- calcoli proposti

        void Calc()
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = CultureInfo.CurrentCulture;
            if (cur.Count == 0)
            {
                txtCalc.Text = all.Count == 0 ? txtCalc.Text : Lang.T("Nessun QSO nella selezione.");
                if (all.Count > 0) txtCalc.Text = Lang.T("Nessun QSO nella selezione.");
                return;
            }
            double h = H;
            string who = Ui.Up(txtCall).Length > 0 ? Ui.Up(txtCall) : Ui.Up(txtLoc);
            sb.AppendLine(Lang.F("{0} da {1} ({2}), quota Es {3:F0} km", selHour < 0 ? Lang.F("Giornata {0}", selDay) : Lang.F("Giornata {0}, ore {1:00}:00-{1:00}:59 UTC", selDay, selHour), who, Ui.Up(txtLoc), h));
            int n1 = cur.Count(q => q.Path.Hops == 1), n2 = cur.Count(q => q.Path.Hops == 2), n3 = cur.Count(q => q.Path.Hops == 3), n4 = cur.Count(q => q.Path.Hops > 3);
            sb.AppendLine(Lang.F("{0} QSO: {1} singolo salto, {2} doppio, {3} triplo, {4} oltre (tabella IK3XTV). Bande: {5}.", cur.Count, n1, n2, n3, n4, BandList(cur)));

            if (selQso != null)
            {
                BenchQso q = selQso; EsPath p = q.Path;
                sb.AppendLine();
                sb.AppendLine(Lang.F("QSO scelto: {0} {1} - {2} ({3}) {4} MHz", q.Utc.ToString("HH:mm", CultureInfo.InvariantCulture) + "Z", q.MyCall, q.Call, q.Grid, q.FreqMHz.ToString("F3", ci)));
                sb.AppendLine(Lang.F("Distanza {0:F0} km - {1} (tabella IK3XTV)", p.DistKm, Lang.T(p.Judgement)));
                sb.AppendLine(Lang.F("Salti uguali: {0} x {1:F0} km, elevazione {2:F1}°, sec(φ) = {3:F2}, foEs ≥ {4:F1} MHz", p.Hops, p.HopKm, p.ElevDeg, p.SecPhi, p.FoEsMin));
                if (p.ShortA != null) sb.AppendLine(Lang.F("Primo salto corto ({0:F0} km, esperienza IC8TEM): foEs ≥ {1:F1} MHz", Ui.V(numShort), p.ShortFoEsMin));
                sb.AppendLine(Lang.F("Azimut: {0} -> {1}: {2:F0}°   {1} -> {0}: {3:F0}°", q.MyCall, q.Call, Geo.Bearing(q.LatTx, q.LonTx, q.LatRx, q.LonRx), Geo.Bearing(q.LatRx, q.LonRx, q.LatTx, q.LonTx)));
            }

            // 1. skip per settori
            sb.AppendLine();
            sb.AppendLine(Lang.T("1. Skip per settori di azimut (10°): QSO più vicino = bordo della zona di silenzio, più lontano = portata"));
            foreach (Sector s in sectors)
                sb.AppendLine(Lang.F("  az {0:000}-{1:000}°: skip {2,5:F0} km ({3}, {4} MHz)   max {5,5:F0} km ({6})   {7} QSO",
                    s.Az, s.Az + SectorDeg, s.Min.Path.DistKm, s.Min.Call, s.Min.FreqMHz.ToString("F1", ci), s.Max.Path.DistKm, s.Max.Call, s.N));

            // 2. foEs dalla legge della secante: il QSO in singolo salto piu' corto di ogni banda
            sb.AppendLine();
            sb.AppendLine(Lang.T("2. foEs stimata (legge della secante, foEs ≥ f / sec φ): il singolo salto più corto di ogni banda dà la stima più alta"));
            double foBest = 0; BenchQso foQ = null;
            foreach (string b in EsBands)
            {
                BenchQso q = cur.Where(x => x.Band == b && x.Path.Hops == 1).OrderBy(x => x.Path.DistKm).FirstOrDefault();
                if (q == null) continue;
                sb.AppendLine(Lang.F("  {0}: {1} a {2:F0} km, sec(φ) = {3:F2} -> foEs ≥ {4:F1} MHz", b, q.Call, q.Path.DistKm, q.Path.SecPhi, q.Path.FoEsMin));
                if (q.Path.FoEsMin > foBest) { foBest = q.Path.FoEsMin; foQ = q; }
            }
            if (foQ != null)
            {
                sb.AppendLine(Lang.F("  Stima per la selezione: foEs ≥ {0:F1} MHz ({1}). MUF attesa con questa foEs: {2:F0} MHz a 1000 km, {3:F0} MHz a 1500 km, {4:F0} MHz a 2000 km, {5:F0} MHz a 2400 km.",
                    foBest, foQ.Call, foBest * Geo.SecantFactor(1000, h), foBest * Geo.SecantFactor(1500, h), foBest * Geo.SecantFactor(2000, h), foBest * Geo.SecantFactor(2400, h)));
                double dmin144 = double.NaN;
                for (double d = 400; d <= 2400; d += 20) if (foBest * Geo.SecantFactor(d, h) >= 144.3) { dmin144 = d; break; }
                sb.AppendLine(double.IsNaN(dmin144) ? Lang.T("  Con questa foEs i 144 MHz non si aprono in singolo salto.")
                    : Lang.F("  Con questa foEs i 144 MHz si aprono in singolo salto oltre circa {0:F0} km (skip in 2 m).", dmin144));
            }
            else sb.AppendLine(Lang.T("  Nessun QSO in singolo salto nella selezione."));

            // 3. nubi sui quadrati
            sb.AppendLine();
            sb.AppendLine(Lang.T("3. Nubi Es stimate (quadrati Maidenhead dei punti di riflessione)"));
            foreach (Cloud c in clouds.Take(12))
                sb.AppendLine(Lang.F("  {0}: {1} QSO, foEs ≥ {2:F1} MHz, az {3:F0}°, {4:F0} km ({5})", c.Sq, c.N, c.FoEs, Geo.Bearing(cLat, cLon, c.Lat, c.Lon), Geo.DistanceKm(cLat, cLon, c.Lat, c.Lon), string.Join(", ", c.Calls)));

            // 4. multi-salto: quale ipotesi del primo salto trova una nube vista in singolo salto nella stessa ora (+/- 30 min)
            List<BenchQso> multi = cur.Where(q => q.Path.Hops >= 2).ToList();
            if (multi.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine(Lang.T("4. Multi-salto: primo salto della stazione confrontato con le nubi viste in singolo salto entro ±30 min (entro 300 km = confermato)"));
                List<BenchQso> day = all.Where(q => q.Day == selDay && q.Path.Hops == 1).ToList();
                int okEq = 0, okSh = 0;
                foreach (BenchQso q in multi)
                {
                    List<BenchQso> near = day.Where(x => Math.Abs((x.Utc - q.Utc).TotalMinutes) <= 30).ToList();
                    double dEq = near.Count > 0 ? near.Min(x => Geo.DistanceKm(x.Path.FirstA.Lat, x.Path.FirstA.Lon, q.Path.FirstA.Lat, q.Path.FirstA.Lon)) : double.NaN;
                    double dSh = near.Count > 0 && q.Path.ShortA != null ? near.Min(x => Geo.DistanceKm(x.Path.FirstA.Lat, x.Path.FirstA.Lon, q.Path.ShortA.Lat, q.Path.ShortA.Lon)) : double.NaN;
                    if (!double.IsNaN(dEq) && dEq <= 300) okEq++;
                    if (!double.IsNaN(dSh) && dSh <= 300) okSh++;
                    sb.AppendLine(Lang.F("  {0} {1} {2:F0} km, {3} salti: salti uguali {4} ({5}), primo salto corto {6} ({7})",
                        q.Utc.ToString("HH:mm", CultureInfo.InvariantCulture) + "Z", q.Call, q.Path.DistKm, q.Path.Hops,
                        Geo.LatLonToLoc(q.Path.FirstA.Lat, q.Path.FirstA.Lon, 4), double.IsNaN(dEq) ? Lang.T("nessuna nube vicina") : Lang.F("nube a {0:F0} km", dEq),
                        q.Path.ShortA != null ? Geo.LatLonToLoc(q.Path.ShortA.Lat, q.Path.ShortA.Lon, 4) : "-", double.IsNaN(dSh) ? Lang.T("nessuna nube vicina") : Lang.F("nube a {0:F0} km", dSh)));
                }
                sb.AppendLine(Lang.F("  Confermati: salti uguali {0} su {1}, primo salto corto {2} su {1}.", okEq, multi.Count, okSh));
            }

            // 5. spostamento orario delle nubi
            List<KeyValuePair<int, Point>> tr = HourCentroids(all.Where(q => q.Day == selDay).ToList());
            if (tr.Count >= 2)
            {
                sb.AppendLine();
                sb.AppendLine(Lang.T("5. Spostamento delle nubi ora per ora (baricentro dei punti medi dei singoli salti)"));
                for (int i = 1; i < tr.Count; i++)
                {
                    Point a = tr[i - 1].Value, b = tr[i].Value;
                    double dkm = Geo.DistanceKm(a.Y, a.X, b.Y, b.X), hrs = Math.Max(1, tr[i].Key - tr[i - 1].Key);
                    sb.AppendLine(Lang.F("  {0:00}Z -> {1:00}Z: {2} -> {3}, {4:F0} km verso {5:F0}°, {6:F0} km/h ({7:F0} m/s)",
                        tr[i - 1].Key, tr[i].Key, Geo.LatLonToLoc(a.Y, a.X, 4), Geo.LatLonToLoc(b.Y, b.X, 4), dkm, Geo.Bearing(a.Y, a.X, b.Y, b.X), dkm / hrs, dkm / hrs / 3.6));
                }
            }
            sb.AppendLine();
            sb.AppendLine(Lang.T("Tabella IK3XTV: singolo salto 500-650 / 1500-2000 / 2200-2400 km (minima / ottimale / massima); doppio 2800-3000 / 3200-4000 / 4400-5000 km; triplo ottimale 4800-6400, massima 6900 km."));
            sb.AppendLine(Lang.T("Multi-salto: l'autore (IC8TEM) ha esperienza di primo salto corto, entro i 1200 km dal proprio QTH."));
            sb.AppendLine(Lang.T("I calcoli sono indicativi: un QSO dice che la tratta era aperta, non dove stava esattamente la nube."));
            txtCalc.Text = sb.ToString();
        }

        // ---------------------------------------------------------------- fotografie automatiche

        public void ShotSelect(string day, int hour, double km)
        {
            range.Apply(km, true);
            selDay = day; selHour = hour; selQso = null;
            FillDays(); FillHours(); Analyze();
        }

        // log di prova (una giornata di sporadico E da JN70CN con singoli e multi-salti)
        public static string DemoAdif()
        {
            string[][] q =
            {
                new[] { "1005", "EA7KW", "IM76", "50.313" }, new[] { "1012", "EA5ABC", "IM98", "50.313" }, new[] { "1020", "EA6VQ", "JM19", "144.174" },
                new[] { "1031", "CT1HZE", "IM58", "50.313" }, new[] { "1044", "EA8DO", "IL18", "50.313" }, new[] { "1058", "EA3XYZ", "JN11", "70.154" },
                new[] { "1105", "F4ABC", "IN94", "50.313" }, new[] { "1115", "G4DEF", "IO91", "50.313" }, new[] { "1122", "F6XYZ", "IN88", "144.174" },
                new[] { "1140", "W1JJ", "FN42", "50.313" }, new[] { "1152", "VE1SKY", "FN74", "50.313" }, new[] { "1210", "OH1ABC", "KP10", "50.313" },
                new[] { "1225", "SM5DEF", "JO89", "50.313" }, new[] { "1240", "4X1AB", "KM72", "50.313" }, new[] { "1255", "LA9XYZ", "JP20", "28.074" },
                new[] { "1610", "PY2XB", "GG66", "50.313" }, new[] { "1630", "LU5FF", "GF05", "50.313" }, new[] { "1700", "IW2ABC", "JN45", "432.200" }
            };
            StringBuilder sb = new StringBuilder("<adif_ver:5>3.1.4<eoh>\n");
            foreach (string[] r in q)
                sb.Append("<station_callsign:6>IC8TEM<my_gridsquare:6>JN70CN<call:" + r[1].Length + ">" + r[1] + "<gridsquare:4>" + r[2] + "<freq:" + r[3].Length + ">" + r[3]
                    + "<mode:3>FT8<qso_date:8>20250615<time_on:4>" + r[0] + "<eor>\n");
            sb.Append("<station_callsign:6>IC8TEM<my_gridsquare:6>JN70CN<call:5>EA1AB<gridsquare:4>IN73<freq:6>50.313<mode:3>FT8<qso_date:8>20250616<time_on:4>0915<eor>\n");
            return sb.ToString();
        }
    }
}
