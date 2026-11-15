// E-LAYER ANALYZER - sottolinguetta NUBI Es NEL TEMPO di SPORADIC-E (proposta approvata dall'autore il 14/11/2026).
// Metodo di Chartier, Hanley ed Emmons (AMT 2022) [67], come nell'analisi del 11/05/2024 del deposito "sporadice" [70]
// (citato, codice non copiato): ogni collegamento E-sporadico da' una foEs minima nel punto di riflessione (legge della
// secante, foEs = f / sec phi; NmE = 1,24e10 * foEs^2 el/m^3); i punti si raccolgono in celle di 1, 2 o 4 gradi e a passi
// di 5, 10 o 15 minuti; ogni cella prende il valore piu' alto fra le foEs minime che ci cadono (la nube e' densa almeno cosi').
// Differenza: il programma usa i punti di riflessione di EsModel (multi-salto compreso), non solo il punto medio.
// Sorgenti: spot della sessione, un log ADIF oppure un file di spot CSV del programma.
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
    public class NubiTab
    {
        public readonly Control Root;
        readonly MapView map = new MapView { Mercator = true };
        readonly TableView tCells;
        readonly SimpleChart chart;
        readonly SelectableTextBlock txtSum;
        readonly TextBlock lblSrc, lblStep;
        readonly ComboBox cmbStep, cmbGrid;
        readonly CheckBox c10, c8, c6, c4, c2, chkTrail, chkLinks;
        readonly NumericUpDown numH;
        readonly Slider sld;
        readonly Button bPlay;
        readonly DispatcherTimer play = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(700) };
        static readonly int[] Steps = { 5, 10, 15 };
        static readonly double[] Grids = { 1, 2, 4 };
        public const double NmeK = 1.2407e10;   // el/m^3 per MHz^2 (f = 8,98 sqrt(N) Hz)
        const int TrailSteps = 3;

        // collegamenti caricati (sessione o file) e calcolo per passi
        List<Spot> src = new List<Spot>();
        string srcName = "";
        bool fromFile, fromAdif;   // fromAdif: QSO di un log, la regola degli FT8 isolati non si applica (come in BENCHMARK Es)
        int excl, exclTep, exclShort, exclFt8, exclBand;
        class Link { public Spot S; public EsPath P; public int Step; }
        class Cell { public int Step; public double Lat0, Lon0; public double FoEs; public int N; public HashSet<string> Bands = new HashSet<string>(); public Link Best; }
        List<Link> links = new List<Link>();
        Dictionary<int, List<Cell>> cells = new Dictionary<int, List<Cell>>();
        DateTime t0;
        int nSteps;
        int cur;
        Cell selCell;
        bool quiet;   // cambi del cursore fatti dal programma (nessun ridisegno doppio)

        public NubiTab()
        {
            Config cf = Station.Cfg;
            cmbStep = Ui.Combo(Steps.Select(m => Lang.F("{0} minuti", m)), Math.Max(0, Array.IndexOf(Steps, (int)cf.GetD("nubi_passo", 5))), 100);
            cmbGrid = Ui.Combo(Grids.Select(g => Lang.F("{0:F0}° x {0:F0}°", g)), Math.Max(0, Array.IndexOf(Grids, cf.GetD("nubi_cella", 2))), 90);
            c10 = Ui.C("28", cf.GetB("nubi_28", true)); c8 = Ui.C("40", cf.GetB("nubi_40", true)); c6 = Ui.C("50", cf.GetB("nubi_50", true));
            c4 = Ui.C("70", cf.GetB("nubi_70", true)); c2 = Ui.C("144", cf.GetB("nubi_144", true));
            chkTrail = Ui.C(Lang.T("Passi precedenti sbiaditi"), cf.GetB("nubi_scia", true));
            chkLinks = Ui.C(Lang.T("Collegamenti"), cf.GetB("nubi_collegamenti", true));
            numH = Ui.N(90, 130, Ui.Dm(cf.GetD("nubi_quota_km", EsModel.DefaultH)), 0, 1, 45);
            Button bSess = Ui.B(Lang.T("Spot della sessione"), true);
            Button bFile = Ui.B(Lang.T("Carica log ADIF o spot CSV"));
            Button bExp = Ui.B(Lang.T("Esporta gli spot della sessione (CSV)"));
            bSess.Click += (s, e) => UseSession();
            bFile.Click += async (s, e) => await PickFile();
            bExp.Click += async (s, e) => await ExportSession();
            foreach (CheckBox c in new[] { c10, c8, c6, c4, c2 }) c.IsCheckedChanged += (s, e) => { SaveBands(); Rebuild(); };
            cmbStep.SelectionChanged += (s, e) => { cf.Set("nubi_passo", Steps[Math.Max(0, cmbStep.SelectedIndex)]); Rebuild(); };
            cmbGrid.SelectionChanged += (s, e) => { cf.Set("nubi_cella", Grids[Math.Max(0, cmbGrid.SelectedIndex)]); Rebuild(); };
            numH.ValueChanged += (s, e) => { cf.Set("nubi_quota_km", Ui.V(numH)); Rebuild(); };
            chkTrail.IsCheckedChanged += (s, e) => { cf.Set("nubi_scia", chkTrail.IsChecked == true); Draw(); };
            chkLinks.IsCheckedChanged += (s, e) => { cf.Set("nubi_collegamenti", chkLinks.IsChecked == true); Draw(); };

            sld = new Slider { Minimum = 0, Maximum = 0, SmallChange = 1, LargeChange = 6, IsSnapToTickEnabled = true, TickFrequency = 1, MinWidth = 220, Margin = new Thickness(6, 0), VerticalAlignment = VerticalAlignment.Center };
            sld.PropertyChanged += (s, e) => { if (e.Property == Slider.ValueProperty && !quiet) { int v = (int)Math.Round(sld.Value); if (v != cur) { cur = v; selCell = null; ShowStep(); } } };
            Button bPrev = Ui.B("◀"), bNext = Ui.B("▶"), bFirst = Ui.B("⏮"), bLast = Ui.B("⏭");
            bPlay = Ui.B(Lang.T("Avvia"));
            bPrev.Click += (s, e) => Go(cur - 1); bNext.Click += (s, e) => Go(cur + 1);
            bFirst.Click += (s, e) => Go(0); bLast.Click += (s, e) => Go(nSteps - 1);
            bPlay.Click += (s, e) => { if (play.IsEnabled) StopPlay(); else { if (cur >= nSteps - 1) Go(0); play.Start(); bPlay.Content = Lang.T("Ferma"); } };
            play.Tick += (s, e) => { if (cur >= nSteps - 1) StopPlay(); else Go(cur + 1); };
            lblStep = new TextBlock { Foreground = Pal.B(Pal.Yellow), FontWeight = FontWeight.SemiBold, FontFamily = Pal.Mono, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(6, 0) };
            lblSrc = new TextBlock { Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2) };

            tCells = new TableView();
            tCells.Columns.AddRange(new[] { Lang.T("Cella"), "foEs≥", "NmE≥", Lang.T("Collegamenti"), Lang.T("Bande") });
            tCells.ColumnWidths.AddRange(new double[] { 120, 56, 86, 90, 120 });
            tCells.HeaderTips = new[] { Lang.T("Angolo sud-ovest della cella (latitudine, longitudine) e locator del centro"), Lang.T("La foEs minima più alta fra i collegamenti che riflettono nella cella (MHz)"),
                Lang.T("Densità elettronica minima dello strato, 1,24e10 x foEs^2 elettroni al metro cubo"), Lang.T("Collegamenti con un punto di riflessione nella cella in questo passo"), "" };
            tCells.RowClicked += i => { if (i >= 0 && i < tCells.Rows.Count) { selCell = tCells.Rows[i].Tag as Cell; Draw(); } };
            chart = Ui.Fit(new SimpleChart { YLabel = "MHz", Y2Label = Lang.T("celle") }, 160, 110);
            txtSum = Ui.Text(12.5);
            ScrollViewer svSum = new ScrollViewer { Content = txtSum, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };

            Grid right = new Grid { RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,Auto,*,5,Auto,*"), Background = Pal.B(Pal.Back) };
            Control r0 = Ui.Row(bSess, bFile, bExp);
            Control r1 = Ui.Row(Ui.L(Lang.T("Bande MHz")), c10, c8, c6, c4, c2, Ui.L(Lang.T("Passo")), cmbStep, Ui.L(Lang.T("Cella")), cmbGrid, Ui.L(Lang.T("Quota Es km")), numH);
            Control r2 = Ui.Row(bFirst, bPrev, bPlay, bNext, bLast, sld, lblStep);
            Control r3 = Ui.Row(chkTrail, chkLinks);
            Grid.SetRow(r0, 0); Grid.SetRow(r1, 1); Grid.SetRow(lblSrc, 2); Grid.SetRow(r2, 3); Grid.SetRow(r3, 4);
            Border cellBox = Ui.Box(Lang.T("Celle del passo (clic per i collegamenti della cella)"), Pal.Azure, tCells);
            Grid.SetRow(cellBox, 5);
            GridSplitter sp = new GridSplitter { Background = Pal.B(Pal.Track), ResizeDirection = GridResizeDirection.Rows };
            Grid.SetRow(sp, 6);
            Border chartBox = Ui.Box(Lang.T("foEs minima più alta e celle attive, passo per passo"), Pal.Yellow, chart);
            Grid.SetRow(chartBox, 7);
            Border sumBox = Ui.Box(Lang.T("Sintesi"), Pal.Green, svSum);
            Grid.SetRow(sumBox, 8);
            foreach (Control c in new Control[] { r0, r1, lblSrc, r2, r3, cellBox, sp, chartBox, sumBox }) right.Children.Add(c);

            Continents.Apply(map);
            Continents.Changed += () => { Continents.Apply(map); map.InvalidateVisual(); };
            map.Clicked += (la, lo, rb) =>
            {
                List<Cell> l; if (!cells.TryGetValue(cur, out l)) return;
                double g = Grid0;
                selCell = l.FirstOrDefault(c => la >= c.Lat0 && la < c.Lat0 + g && lo >= c.Lon0 && lo < c.Lon0 + g);
                FillTable(); Draw();
            };
            Root = Ui.MapAndData(map, right);
            UseSession();
        }

        double Grid0 { get { return Grids[Math.Max(0, cmbGrid.SelectedIndex)]; } }
        int StepMin { get { return Steps[Math.Max(0, cmbStep.SelectedIndex)]; } }

        void SaveBands()
        {
            Config cf = Station.Cfg;
            cf.Set("nubi_28", c10.IsChecked == true); cf.Set("nubi_40", c8.IsChecked == true); cf.Set("nubi_50", c6.IsChecked == true);
            cf.Set("nubi_70", c4.IsChecked == true); cf.Set("nubi_144", c2.IsChecked == true);
        }

        bool BandOn(string b)
        {
            return (b == "10m" && c10.IsChecked == true) || (b == "8m" && c8.IsChecked == true) || (b == "6m" && c6.IsChecked == true) || (b == "4m" && c4.IsChecked == true) || (b == "2m" && c2.IsChecked == true);
        }

        void StopPlay() { play.Stop(); bPlay.Content = Lang.T("Avvia"); }

        void Go(int i)
        {
            if (nSteps == 0) return;
            i = Math.Max(0, Math.Min(nSteps - 1, i));
            cur = i; selCell = null;
            quiet = true; sld.Value = i; quiet = false;
            ShowStep();
        }

        // ---------------------------------------------------------------- sorgenti

        // all'apertura della linguetta: con la sorgente "sessione" si riprendono gli spot arrivati nel frattempo
        public void OnShow() { if (!fromFile && !play.IsEnabled) UseSession(); }

        public void UseSession()
        {
            List<Spot> l;
            lock (SpotStore.All) l = new List<Spot>(SpotStore.All);
            src = l; fromFile = false; fromAdif = false;
            srcName = Lang.T("spot della sessione");
            Rebuild();
        }

        async Task PickFile()
        {
            TopLevel tl = TopLevel.GetTopLevel(Root);
            if (tl == null) return;
            IReadOnlyList<IStorageFile> files = await tl.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = Lang.T("Log ADIF o file di spot CSV del programma"),
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("ADIF / CSV") { Patterns = new[] { "*.adi", "*.adif", "*.ADI", "*.ADIF", "*.csv", "*.CSV" } } }
            });
            if (files == null || files.Count == 0) return;
            string p = files[0].TryGetLocalPath();
            if (p != null) await LoadFile(p);
        }

        public async Task LoadFile(string path)
        {
            lblSrc.Text = Lang.F("Leggo {0}...", Path.GetFileName(path));
            List<Spot> l = null; string err = null;
            await Task.Run(() =>
            {
                try { l = path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) ? FromCsv(path) : FromAdif(path); }
                catch (Exception ex) { err = ex.Message; }
            });
            if (l == null) { lblSrc.Text = Lang.F("Errore: {0}", err ?? ""); return; }
            src = l; fromFile = true; fromAdif = !path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase); srcName = Path.GetFileName(path);
            Rebuild();
        }

        static List<Spot> FromCsv(string path)
        {
            List<Spot> l = new List<Spot>();
            foreach (string line in File.ReadAllLines(path))
            {
                Spot s = SpotStore.ParseCsvLine(line);
                if (s != null) l.Add(s);
            }
            return l;
        }

        static string Field(Dictionary<string, string> f, string k) { string v; return f.TryGetValue(k, out v) && v != null ? v.Trim().ToUpperInvariant() : ""; }
        static readonly string[] OtherProp = { "TR", "MS", "FAI", "AUR", "AUE", "EME", "F2", "TEP", "RS", "SAT", "LOS", "ION", "IRL", "INTERNET", "ECH", "BS" };

        // QSO del log come "spot" (stazione del log -> corrispondente), stesse regole di BENCHMARK Es
        static List<Spot> FromAdif(string path)
        {
            List<Spot> l = new List<Spot>();
            foreach (Dictionary<string, string> r in Adif.Parse(File.ReadAllBytes(path)))
            {
                string band = LogBands.Normalize(Field(r, "BAND").ToLowerInvariant(), Field(r, "FREQ"));
                string prop = Field(r, "PROP_MODE");
                if (prop.Length > 0 && prop != "ES" && Array.IndexOf(OtherProp, prop) >= 0) continue;
                string grid = Field(r, "GRIDSQUARE");
                int cm = grid.IndexOfAny(new[] { ',', ' ', ';' }); if (cm > 0) grid = grid.Substring(0, cm);
                if (grid.Length < 4) grid = CallDb.Lookup(Field(r, "CALL")) ?? "";
                grid = CallDb.Refine(Field(r, "CALL"), grid) ?? grid;
                if (grid.Length > 6) grid = grid.Substring(0, 6);
                string my = Field(r, "MY_GRIDSQUARE"); if (my.Length > 6) my = my.Substring(0, 6);
                string myCall = Field(r, "STATION_CALLSIGN"); if (myCall.Length == 0) myCall = Field(r, "OPERATOR"); if (myCall.Length == 0) myCall = Station.Call;
                if (my.Length < 4) my = Station.Loc;
                double la, lo, mla, mlo;
                if (grid.Length < 4 || !Geo.IsLocator(grid.Substring(0, 4)) || !Geo.LocToLatLon(grid, out la, out lo)) continue;
                if (my.Length < 4 || !Geo.LocToLatLon(my, out mla, out mlo)) continue;
                double f; string fs = Field(r, "FREQ").Replace(',', '.');
                double mhz = double.TryParse(fs, NumberStyles.Float, CultureInfo.InvariantCulture, out f) && LogBands.Normalize("", fs) == band ? f : DefaultMHz(band);
                if (mhz <= 0) continue;
                string d = Field(r, "QSO_DATE"), t = Field(r, "TIME_ON");
                if (t.Length == 4) t += "00";
                DateTime dt;
                if (!DateTime.TryParseExact(d + t, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dt)) continue;
                l.Add(new Spot { Utc = dt, FreqKHz = mhz * 1000, DxCall = Field(r, "CALL"), Spotter = myCall, LocSpotter = my, LocDx = grid, LatS = mla, LonS = mlo, LatD = la, LonD = lo, Prop = prop.Length > 0 ? prop : "", DigiMode = Field(r, "MODE"), Comment = "ADIF" });
            }
            l.Sort((a, b) => a.Utc.CompareTo(b.Utc));
            return l;
        }

        static double DefaultMHz(string band)
        {
            switch (band) { case "10m": return 28.5; case "8m": return 40.68; case "6m": return 50.15; case "4m": return 70.2; case "2m": return 144.3; default: return 0; }
        }

        async Task ExportSession()
        {
            TopLevel tl = TopLevel.GetTopLevel(Root);
            if (tl == null) return;
            IStorageFile f = await tl.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = Lang.T("Esporta gli spot della sessione (CSV)"),
                SuggestedFileName = "ela_spot_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmm", CultureInfo.InvariantCulture) + ".csv",
                DefaultExtension = "csv",
                FileTypeChoices = new[] { new FilePickerFileType("CSV") { Patterns = new[] { "*.csv" } } }
            });
            if (f == null) return;
            List<Spot> l;
            lock (SpotStore.All) l = new List<Spot>(SpotStore.All);
            l.Sort((a, b) => a.Utc.CompareTo(b.Utc));
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SpotStore.CsvHeader);
            foreach (Spot s in l) sb.AppendLine(SpotStore.CsvLine(s));
            try
            {
                using (Stream st = await f.OpenWriteAsync())
                using (StreamWriter w = new StreamWriter(st, new UTF8Encoding(false)))
                    await w.WriteAsync(sb.ToString());
                lblSrc.Text = Lang.F("Esportati {0} spot in {1}.", l.Count, f.Name);
            }
            catch (Exception ex) { lblSrc.Text = Lang.F("Errore: {0}", ex.Message); }
        }

        // ---------------------------------------------------------------- calcolo

        // 50 MHz FT8/FT4 nel file: stessa regola della nube della sessione (EsModel.Ft8CloudMin / Ft8CloudKm), ma fra gli spot del file
        static bool Ft8IsolatedIn(Spot s, List<Spot> all, long[] ticks)
        {
            if (!EsModel.UseFt8CloudRule || s.Band != "6m" || !s.IsFt8Ft4) return false;
            double mla, mlo; Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out mla, out mlo);
            string pair = Pair(s);
            long w = TimeSpan.FromMinutes(EsModel.Ft8CloudMin).Ticks;
            int i0 = Array.BinarySearch(ticks, s.Utc.Ticks - w);
            if (i0 < 0) i0 = ~i0;
            while (i0 > 0 && ticks[i0 - 1] >= s.Utc.Ticks - w) i0--;
            for (int i = i0; i < all.Count && ticks[i] <= s.Utc.Ticks + w; i++)
            {
                Spot q = all[i];
                if (q == s || q.Band != "6m" || !q.HasBoth || Pair(q) == pair) continue;
                double la, lo; Geo.Intermediate(q.LatS, q.LonS, q.LatD, q.LonD, 0.5, out la, out lo);
                if (Geo.DistanceKm(mla, mlo, la, lo) <= EsModel.Ft8CloudKm) return false;
            }
            return true;
        }

        static string Pair(Spot s) { return string.CompareOrdinal(s.DxCall, s.Spotter) < 0 ? s.DxCall + "|" + s.Spotter : s.Spotter + "|" + s.DxCall; }

        public void Rebuild()
        {
            StopPlay();
            double H = Ui.V(numH);
            int step = StepMin;
            links = new List<Link>();
            excl = exclTep = exclShort = exclFt8 = exclBand = 0;
            HashSet<string> seen = new HashSet<string>();
            List<Spot> sorted = new List<Spot>(src);
            sorted.Sort((a, b) => a.Utc.CompareTo(b.Utc));
            long[] ticks = sorted.Select(x => x.Utc.Ticks).ToArray();
            foreach (Spot s in sorted)
            {
                if (!s.HasBoth) { excl++; continue; }
                if (!BandOn(s.Band)) { exclBand++; continue; }
                if (!s.EsCandidate || EsModel.IsAtlanticTropoCandidate(s)) { excl++; continue; }
                bool tep;
                using (Igrf.At(Igrf.YearOf(s.Utc))) tep = s.Prop != "ES" && s.IsTep;   // campo IGRF dell'anno dello spot
                if (tep) { exclTep++; continue; }
                bool iso = fromAdif ? false : fromFile ? Ft8IsolatedIn(s, sorted, ticks) : EsModel.Ft8Isolated50(s);
                if (iso) { exclFt8++; continue; }
                EsPath p = EsModel.Build(s, H);
                if (p == null || p.Hops == 0 || (s.Band == "2m" && p.DistKm < EsModel.MinKm144)) { exclShort++; continue; }
                // un solo collegamento per coppia di stazioni, banda e passo
                DateTime st = Floor(s.Utc, step);
                if (!seen.Add(Pair(s) + "|" + s.Band + "|" + st.Ticks)) continue;
                links.Add(new Link { S = s, P = p });
            }
            cells = new Dictionary<int, List<Cell>>();
            nSteps = 0;
            if (links.Count > 0)
            {
                t0 = Floor(links[0].S.Utc, step);
                DateTime t1 = Floor(links[links.Count - 1].S.Utc, step);
                nSteps = (int)((t1 - t0).TotalMinutes / step) + 1;
                double g = Grid0;
                Dictionary<string, Cell> map0 = new Dictionary<string, Cell>();
                foreach (Link k in links)
                {
                    k.Step = (int)((Floor(k.S.Utc, step) - t0).TotalMinutes / step);
                    HashSet<string> inLink = new HashSet<string>();
                    foreach (EsHop h in k.P.Points)
                    {
                        double la0 = Math.Floor(h.Lat / g) * g, lo0 = Math.Floor(h.Lon / g) * g;
                        string key = k.Step + "|" + la0.ToString("R", CultureInfo.InvariantCulture) + "|" + lo0.ToString("R", CultureInfo.InvariantCulture);
                        if (!inLink.Add(key)) continue;
                        Cell c;
                        if (!map0.TryGetValue(key, out c))
                        {
                            c = new Cell { Step = k.Step, Lat0 = la0, Lon0 = lo0, FoEs = double.NegativeInfinity };
                            map0[key] = c;
                            List<Cell> l; if (!cells.TryGetValue(k.Step, out l)) cells[k.Step] = l = new List<Cell>();
                            l.Add(c);
                        }
                        c.N++; c.Bands.Add(k.S.Band);
                        if (k.P.FoEsMin > c.FoEs) { c.FoEs = k.P.FoEsMin; c.Best = k; }
                    }
                }
                foreach (List<Cell> l in cells.Values) l.Sort((a, b) => b.FoEs.CompareTo(a.FoEs));
            }
            // si parte dal passo con piu' celle (il momento piu' ricco della sorgente)
            int best = 0, bestN = -1;
            foreach (KeyValuePair<int, List<Cell>> kv in cells) if (kv.Value.Count > bestN || (kv.Value.Count == bestN && kv.Key < best)) { bestN = kv.Value.Count; best = kv.Key; }
            cur = best; selCell = null;
            quiet = true;
            sld.Maximum = Math.Max(0, nSteps - 1);
            sld.Value = cur;
            quiet = false;
            lblSrc.Text = Lang.F("{0}: {1} spot o QSO, {2} collegamenti E-sporadico usati.", srcName, src.Count, links.Count)
                + " " + Lang.F("Esclusi: {0} di altre bande, {1} non E-sporadico o senza locator, {2} sotto la distanza minima, {3} FT8/FT4 isolati in 50 MHz, {4} transequatoriali.", exclBand, excl, exclShort, exclFt8, exclTep);
            FillChart();
            ShowStep();
        }

        static DateTime Floor(DateTime t, int min)
        {
            t = t.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(t, DateTimeKind.Utc) : t.ToUniversalTime();
            return new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute / min * min, 0, DateTimeKind.Utc);
        }

        void FillChart()
        {
            chart.Series.Clear();
            int n = Math.Min(nSteps, 2000);
            double[] fe = new double[n], nc = new double[n];
            string[] xl = new string[n];
            int every = Math.Max(1, n / 12);
            for (int i = 0; i < n; i++)
            {
                List<Cell> l;
                bool has = cells.TryGetValue(i, out l) && l.Count > 0;
                fe[i] = has ? l[0].FoEs : -1;
                nc[i] = has ? l.Count : 0;
                xl[i] = t0.AddMinutes(StepMin * i).ToString("HH:mm", CultureInfo.InvariantCulture);
            }
            chart.XLabels = xl; chart.XLabelEvery = every;
            chart.Title = nSteps > 0 ? t0.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " UTC" : "";
            // foEs a barre: i passi isolati (con i passi vuoti intorno) restano visibili; le celle a linea sull'asse destro
            chart.Series.Add(new ChartSeries { Name = "foEs≥", Color = Pal.Yellow, Values = fe, Bars = true });
            chart.Series.Add(new ChartSeries { Name = Lang.T("celle"), Color = Pal.Azure, Values = nc, RightAxis = true, LineWidth = 1.5 });
            chart.MarkIndex = cur;
            chart.InvalidateVisual();
        }

        void ShowStep()
        {
            if (nSteps == 0) lblStep.Text = "--:--";
            else
            {
                DateTime a = t0.AddMinutes(StepMin * cur);
                lblStep.Text = a.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) + "-" + a.AddMinutes(StepMin).ToString("HH:mm", CultureInfo.InvariantCulture) + " UTC  (" + (cur + 1) + "/" + nSteps + ")";
            }
            chart.MarkIndex = cur; chart.InvalidateVisual();
            FillTable(); Draw(); Summary();
        }

        static string CellName(Cell c, double g)
        {
            return c.Lat0.ToString("F0", CultureInfo.InvariantCulture) + "/" + c.Lon0.ToString("F0", CultureInfo.InvariantCulture) + " " + Geo.LatLonToLoc(c.Lat0 + g / 2, c.Lon0 + g / 2, 4);
        }

        static string BandList(IEnumerable<string> b)
        {
            Dictionary<string, string> mhz = new Dictionary<string, string> { { "10m", "28" }, { "8m", "40" }, { "6m", "50" }, { "4m", "70" }, { "2m", "144" } };
            string[] order = { "10m", "8m", "6m", "4m", "2m" };
            return string.Join(" ", order.Where(b.Contains).Select(x => mhz[x]));
        }

        static Color Opaque(Color c) { return Color.FromRgb(c.R, c.G, c.B); }

        void FillTable()
        {
            tCells.Rows.Clear();
            List<Cell> l;
            int si = -1;
            if (cells.TryGetValue(cur, out l))
                foreach (Cell c in l)
                {
                    tCells.Rows.Add(new TableRow
                    {
                        Cells = new[] { CellName(c, Grid0), c.FoEs.ToString("F1", CultureInfo.CurrentCulture), (NmeK * c.FoEs * c.FoEs).ToString("0.00E+0", CultureInfo.InvariantCulture), c.N.ToString(CultureInfo.InvariantCulture), BandList(c.Bands) },
                        CellFore = new Color?[] { null, Opaque(EsTab.FoEsColor(c.FoEs)), null, null, null },
                        Tag = c,
                        CellTips = new[] { "", c.Best != null ? c.Best.S.Spotter + " - " + c.Best.S.DxCall + " " + c.Best.S.FreqMHz.ToString("F3", CultureInfo.InvariantCulture) + " MHz " + c.Best.P.DistKm.ToString("F0", CultureInfo.InvariantCulture) + " km" : "", "", "", "" }
                    });
                    if (c == selCell) si = tCells.Rows.Count - 1;
                }
            tCells.SelectedIndex = si;
            tCells.Refresh();
        }

        void Draw()
        {
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear();
            map.Caption = Lang.T("NUBI Es NEL TEMPO - foEs minima per cella (metodo di Chartier 2022)");
            map.LegendTitle = Lang.T("foEs minima della cella");
            double g = Grid0;
            if (chkTrail.IsChecked == true)
                for (int k = TrailSteps; k >= 1; k--)
                {
                    List<Cell> lp;
                    if (!cells.TryGetValue(cur - k, out lp)) continue;
                    int a = 30 + (TrailSteps - k) * 25;
                    foreach (Cell c in lp)
                        map.Cells.Add(new MapCell { Lat0 = c.Lat0, Lon0 = c.Lon0, DLat = g, DLon = g, Fill = Pal.A(a, EsTab.FoEsColor(c.FoEs)), Tip = Lang.F("{0} minuti prima: foEs ≥ {1:F1} MHz", k * StepMin, c.FoEs) });
                }
            List<Cell> l;
            if (cells.TryGetValue(cur, out l))
                foreach (Cell c in l)
                    map.Cells.Add(new MapCell
                    {
                        Lat0 = c.Lat0, Lon0 = c.Lon0, DLat = g, DLon = g, Fill = Pal.A(c == selCell ? 255 : 200, EsTab.FoEsColor(c.FoEs)),
                        Tip = CellName(c, g) + "\n" + Lang.F("foEs ≥ {0:F1} MHz, NmE ≥ {1:0.00E+0} el/m³", c.FoEs, NmeK * c.FoEs * c.FoEs) + "\n" + Lang.F("{0} collegamenti, bande {1} MHz", c.N, BandList(c.Bands))
                    });
            if (chkLinks.IsChecked == true || selCell != null)
                foreach (Link k in links)
                {
                    if (k.Step != cur) continue;
                    bool inSel = selCell != null && k.P.Points.Any(h => h.Lat >= selCell.Lat0 && h.Lat < selCell.Lat0 + g && h.Lon >= selCell.Lon0 && h.Lon < selCell.Lon0 + g);
                    if (selCell != null && !inSel && chkLinks.IsChecked != true) continue;
                    Color bc = EsTab.BandColor(k.S.Band);
                    map.Lines.Add(MapView.GreatCircle(k.S.LatS, k.S.LonS, k.S.LatD, k.S.LonD, inSel ? Opaque(bc) : Pal.A(selCell != null ? 60 : 140, bc), inSel ? 2.2 : 1.1));
                    if (inSel)
                    {
                        map.Markers.Add(new MapMarker { Lat = k.S.LatS, Lon = k.S.LonS, Radius = 3, Fill = Opaque(bc), Label = k.S.Spotter, LabelColor = Opaque(bc) });
                        map.Markers.Add(new MapMarker { Lat = k.S.LatD, Lon = k.S.LonD, Radius = 3, Fill = Opaque(bc), Label = k.S.DxCall, LabelColor = Opaque(bc) });
                    }
                }
            if (Station.Valid)
                map.Markers.Add(new MapMarker { Lat = Station.Lat, Lon = Station.Lon, Radius = 5, Fill = Pal.Green, Square = true, Label = Station.Call.Length > 0 ? Station.Call : Station.Loc, LabelColor = Pal.Green });
            foreach (double f in new[] { 4.0, 6.0, 8.5, 12.0, 17.0, 25.0 })
                map.Legend.Add(new KeyValuePair<string, Color>(f < 5 ? "< 5 MHz" : f < 7 ? "5-7 MHz" : f < 10 ? "7-10 MHz" : f < 14 ? "10-14 MHz" : f < 20 ? "14-20 MHz" : "≥ 20 MHz", Opaque(EsTab.FoEsColor(f))));
            if (nSteps > 0) map.Info.Add(lblStep.Text);
            map.InvalidateVisual();
        }

        void Summary()
        {
            StringBuilder sb = new StringBuilder();
            if (links.Count == 0)
            {
                sb.AppendLine(Lang.T("Nessun collegamento E-sporadico nella sorgente scelta. Con gli spot della sessione, la mappa si riempie mentre arrivano gli spot (premi di nuovo \"Spot della sessione\" per aggiornare); in alternativa carica un log ADIF o un file di spot CSV esportato in precedenza."));
            }
            else
            {
                int active = cells.Count(kv => kv.Value.Count > 0);
                Cell peak = null;
                foreach (List<Cell> l in cells.Values) foreach (Cell c in l) if (peak == null || c.FoEs > peak.FoEs) peak = c;
                sb.AppendLine(Lang.F("Dal {0:yyyy-MM-dd HH:mm} al {1:yyyy-MM-dd HH:mm} UTC: {2} passi di {3} minuti, {4} con almeno una cella.", t0, t0.AddMinutes(StepMin * nSteps), nSteps, StepMin, active));
                if (peak != null)
                    sb.AppendLine(Lang.F("Valore più alto: foEs ≥ {0:F1} MHz (NmE ≥ {1:0.00E+0} el/m³) nella cella {2} alle {3:HH:mm} UTC.", peak.FoEs, NmeK * peak.FoEs * peak.FoEs, CellName(peak, Grid0), t0.AddMinutes(StepMin * peak.Step)));
                List<Cell> lc;
                if (cells.TryGetValue(cur, out lc) && lc.Count > 0)
                    sb.AppendLine(Lang.F("Passo mostrato: {0} celle, foEs minima più alta {1:F1} MHz.", lc.Count, lc[0].FoEs));
                else sb.AppendLine(Lang.T("Passo mostrato: nessun collegamento."));
                if (selCell != null && selCell.Best != null)
                {
                    sb.AppendLine();
                    sb.AppendLine(Lang.F("Cella {0}: il valore viene da {1} - {2}, {3:F3} MHz, {4:F0} km, {5} salti, sec φ {6:F2}.", CellName(selCell, Grid0), selCell.Best.S.Spotter, selCell.Best.S.DxCall, selCell.Best.S.FreqMHz, selCell.Best.P.DistKm, selCell.Best.P.Hops, selCell.Best.P.SecPhi));
                }
            }
            sb.AppendLine();
            sb.AppendLine(Lang.T("Metodo (Chartier, Hanley ed Emmons 2022): ogni collegamento dice che nel punto di riflessione la foEs è almeno f / sec φ; la cella prende il valore più alto del passo. È un limite inferiore: la nube può essere più densa e più estesa di quanto mostrano i collegamenti. Celle vuote vogliono dire nessun collegamento, non assenza di E-sporadico."));
            txtSum.Text = sb.ToString();
        }

        // fotografie automatiche: testo della sintesi
        public string ShotText() { return lblSrc.Text + "\n" + lblStep.Text + "\n\n" + txtSum.Text; }
    }
}
