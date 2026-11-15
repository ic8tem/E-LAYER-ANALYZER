// E-LAYER ANALYZER - sottolinguetta IONOSONDE di SPORADIC-E (proposta approvata dall'autore il 14/11/2026).
// foEs, foF2 e MUF(D) in diretta dalle ionosonde GIRO [68] (servizio usato da MUF Muncher di DM6AX [69]): mappa del mondo con
// le ionosonde colorate secondo l'ultima foEs (stessi colori delle nubi Es), grafico di 24 ore della stazione scelta,
// MUF dell'E-sporadico a 1000/1500/2000/2400 km con la legge della secante del programma e bande che si aprono,
// confronto con la foEs minima che il programma ricava dagli spot Es entro 500 km dalla ionosonda.
// Aggiornamento silenzioso ogni 15 minuti, dal primo momento in cui si apre la linguetta.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class IonoTab
    {
        public readonly Control Root;
        readonly MapView map = new MapView { Mercator = true };
        readonly TableView tSt;
        readonly SimpleChart chart;
        readonly SelectableTextBlock txtDet;
        readonly TextBlock lblStatus;
        readonly CheckBox chkAuto, chkOld;
        readonly NumericUpDown numH, numRadius;
        readonly DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(15) };
        GiroStation sel;
        bool started;
        public const double SpotRadiusKm = 500;
        static readonly double[] Dists = { 1000, 1500, 2000, 2400 };
        static readonly double[] BandMHz = { 28.5, 40.68, 50.2, 70.2, 144.3 };
        static readonly string[] BandName = { "28 MHz", "40 MHz", "50 MHz", "70 MHz", "144 MHz" };

        public IonoTab()
        {
            Config cf = Station.Cfg;
            chkAuto = Ui.C(Lang.T("Aggiorna da solo ogni 15 minuti"), cf.GetB("iono_auto", true));
            chkOld = Ui.C(Lang.T("Mostra anche le ionosonde senza dati recenti"), cf.GetB("iono_vecchie", true));
            numH = Ui.N(90, 130, Ui.Dm(cf.GetD("iono_quota_km", EsModel.DefaultH)), 0, 1, 45);
            numRadius = Ui.N(100, 1500, Ui.Dm(cf.GetD("iono_raggio_km", SpotRadiusKm)), 0, 50, 60);
            Button bNow = Ui.B(Lang.T("Aggiorna adesso"), true);
            bNow.Click += (s, e) => _ = Fetch();
            chkAuto.IsCheckedChanged += (s, e) => { cf.Set("iono_auto", chkAuto.IsChecked == true); if (chkAuto.IsChecked == true) timer.Start(); else timer.Stop(); };
            chkOld.IsCheckedChanged += (s, e) => { cf.Set("iono_vecchie", chkOld.IsChecked == true); Refresh(); };
            numH.ValueChanged += (s, e) => { cf.Set("iono_quota_km", Ui.V(numH)); Details(); };
            numRadius.ValueChanged += (s, e) => { cf.Set("iono_raggio_km", Ui.V(numRadius)); Refresh(); };
            timer.Tick += (s, e) => { if (chkAuto.IsChecked == true) _ = Fetch(); };
            lblStatus = new TextBlock { Foreground = Pal.B(Pal.Green), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4) };

            tSt = new TableView();
            tSt.Columns.AddRange(new[] { Lang.T("Codice"), Lang.T("Ionosonda"), "km", "UTC", "foEs", "foF2", "MUF(D)", Lang.T("Spot foEs≥") });
            tSt.ColumnWidths.AddRange(new double[] { 60, 150, 52, 46, 48, 48, 58, 76 });
            tSt.HeaderTips = new[] { "", "", Lang.T("Distanza dalla propria stazione"), Lang.T("Ora dell'ultimo ionogramma"), Lang.T("Frequenza critica dello strato E sporadico (MHz)"),
                Lang.T("Frequenza critica dello strato F2 (MHz)"), Lang.T("MUF a 3000 km dello strato F2 (MHz)"), Lang.T("foEs minima ricavata dagli spot Es degli ultimi 30 minuti con un punto di riflessione vicino alla ionosonda") };
            tSt.RowClicked += i =>
            {
                if (i < 0 || i >= tSt.Rows.Count) return;
                sel = tSt.Rows[i].Tag as GiroStation;
                if (sel != null) cf.Set("iono_scelta", sel.Code);
                Draw(); Details();
            };
            chart = Ui.Fit(new SimpleChart { Title = "", YLabel = "MHz" }, 190, 120);
            txtDet = Ui.Text(12.5);
            ScrollViewer svDet = new ScrollViewer { Content = txtDet, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            TextBlock credit = new TextBlock
            {
                Text = Lang.T("Dati: GIRO, Global Ionosphere Radio Observatory (Reinisch e Galkin 2011), licenza CC BY-NC-SA 4.0, solo uso non commerciale. Servizio lgdc.uml.edu/fastchar/getbest, come in MUF Muncher di DM6AX."),
                Foreground = Pal.B(Pal.Muted), FontSize = 11, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2)
            };

            Grid bottom = new Grid { RowDefinitions = new RowDefinitions("Auto,*") };
            Border chartBox = Ui.Box(Lang.T("Ultime 24 ore della ionosonda scelta"), Pal.Azure, chart);
            Grid.SetRow(chartBox, 0); bottom.Children.Add(chartBox);
            Border detBox = Ui.Box(Lang.T("MUF dell'E-sporadico e bande (legge della secante)"), Pal.Yellow, svDet);
            Grid.SetRow(detBox, 1); bottom.Children.Add(detBox);

            Grid right = new Grid { RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,*,5,1.6*,Auto"), Background = Pal.B(Pal.Back) };
            Control r0 = Ui.Row(bNow, chkAuto, chkOld);
            Control r1 = Ui.Row(Ui.L(Lang.T("Quota Es km")), numH, Ui.L(Lang.T("Spot entro km")), numRadius);
            Grid.SetRow(r0, 0); Grid.SetRow(r1, 1); Grid.SetRow(lblStatus, 2);
            TextBlock hint = new TextBlock { Text = Lang.T("Clic su una ionosonda (tabella o mappa) per il grafico e la MUF dell'E-sporadico."), Foreground = Pal.B(Pal.Muted), Margin = new Thickness(6, 0, 6, 2), TextWrapping = TextWrapping.Wrap };
            Grid.SetRow(Ui.HideCompact(hint), 3);
            Grid.SetRow(tSt, 4);
            GridSplitter sp = new GridSplitter { Background = Pal.B(Pal.Track), ResizeDirection = GridResizeDirection.Rows };
            Grid.SetRow(sp, 5);
            Grid.SetRow(bottom, 6); Grid.SetRow(credit, 7);
            foreach (Control c in new Control[] { r0, r1, lblStatus, hint, tSt, sp, bottom, credit }) right.Children.Add(c);

            Continents.Apply(map);
            Continents.Changed += () => { Continents.Apply(map); map.InvalidateVisual(); };
            map.Clicked += (la, lo, rb) =>
            {
                GiroStation best = null; double bd = 400;
                foreach (GiroStation s in Giro.Stations) { double d = Geo.DistanceKm(la, lo, s.Lat, s.Lon); if (d < bd) { bd = d; best = s; } }
                if (best != null) { sel = best; cf.Set("iono_scelta", sel.Code); FillTable(); Draw(); Details(); }
            };
            Giro.Updated += () => Dispatcher.UIThread.Post(Refresh);
            SpotStore.Added += s => spotsDirty = true;
            DispatcherTimer spotTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
            spotTimer.Tick += (s, e) => { if (spotsDirty && Root.IsVisible) { spotsDirty = false; Refresh(); } };
            spotTimer.Start();
            sel = Giro.Find(cf.Get("iono_scelta", ""));
            Root = Ui.MapAndData(map, right);
            Refresh();
        }

        bool spotsDirty;

        // primo ingresso nella linguetta: carica l'archivio e scarica i dati nuovi; poi ogni 15 minuti, in silenzio
        public void Activate()
        {
            if (!Giro.Ready) return;   // l'archivio si carica all'apertura della finestra (OnOpened), che poi richiama Activate
            if (!started)
            {
                started = true;
                if (chkAuto.IsChecked == true) timer.Start();
                _ = Fetch();
            }
            Refresh();
        }

        public async Task Fetch()
        {
            if (Giro.Busy) return;
            lblStatus.Text = Lang.T("Scarico i dati delle ionosonde GIRO...");
            await Giro.FetchAllAsync();
        }

        public bool Busy { get { return Giro.Busy; } }
        public string StatusText { get { return lblStatus.Text ?? ""; } }

        // ---------------------------------------------------------------- dati

        class Row { public GiroStation St; public double Km = double.NaN, FoEs = double.NaN, FoF2 = double.NaN, Muf = double.NaN, SpotFoEs = double.NaN; public int SpotN; public DateTime When; }
        List<Row> rows = new List<Row>();

        static double V(GiroStation s, Func<GiroRec, double> f, out DateTime when) { return s.LastValue(f, Giro.FreshMin, out when); }

        public void Refresh()
        {
            // foEs minima dagli spot Es recenti, per punto di riflessione (stesse regole della linguetta REAL-TIME)
            List<EsPath> paths = new List<EsPath>();
            HashSet<string> seen = new HashSet<string>();
            foreach (Spot s in SpotStore.Recent(30, x => x.HasBoth && x.EsCandidate && !EsModel.Ft8Isolated50(x) && !x.IsTep && !EsModel.IsAtlanticTropoCandidate(x)
                     && (x.Band == "10m" || x.Band == "8m" || x.Band == "6m" || x.Band == "4m" || (x.Band == "2m" && x.DistanceKm >= EsModel.MinKm144))))
            {
                if (!seen.Add(s.DxCall + "|" + s.Spotter + "|" + s.Band)) continue;   // un solo spot per coppia e banda, come in REAL-TIME
                EsPath p = EsModel.Build(s, Ui.V(numH));
                if (p != null && p.Hops > 0) paths.Add(p);
            }
            double rad = Ui.V(numRadius);
            rows = new List<Row>();
            foreach (GiroStation st in Giro.Stations)
            {
                Row r = new Row { St = st };
                if (Station.Valid) r.Km = Geo.DistanceKm(Station.Lat, Station.Lon, st.Lat, st.Lon);
                DateTime w1, w2, w3;
                r.FoEs = V(st, x => x.FoEs, out w1); r.FoF2 = V(st, x => x.FoF2, out w2); r.Muf = V(st, x => x.Muf, out w3);
                GiroRec last = st.Last; r.When = last != null ? last.Utc : DateTime.MinValue;
                foreach (EsPath p in paths)
                    foreach (EsHop h in p.Points)
                        if (Geo.DistanceKm(h.Lat, h.Lon, st.Lat, st.Lon) <= rad)
                        {
                            r.SpotN++;
                            if (double.IsNaN(r.SpotFoEs) || p.FoEsMin > r.SpotFoEs) r.SpotFoEs = p.FoEsMin;
                            break;
                        }
                if (!st.Fresh && chkOld.IsChecked != true && r.SpotN == 0) continue;
                rows.Add(r);
            }
            rows.Sort((a, b) => (double.IsNaN(a.Km) ? 0 : a.Km).CompareTo(double.IsNaN(b.Km) ? 0 : b.Km));
            if (sel == null && rows.Count > 0)
            {
                Row first = rows.FirstOrDefault(x => x.St.Fresh) ?? rows[0];
                sel = first.St;
            }
            int fresh = Giro.Stations.Count(x => x.Fresh);
            if (Giro.Busy) lblStatus.Text = Lang.T("Scarico i dati delle ionosonde GIRO...");
            else if (Giro.LastFetch == DateTime.MinValue && fresh == 0) lblStatus.Text = Lang.T("Nessun dato ancora: i dati arrivano alla prima apertura della linguetta e poi ogni 15 minuti.");
            else
            {
                string[] p = (Giro.Status ?? "").Split('|');
                lblStatus.Text = Lang.F("Ionosonde con un ionogramma negli ultimi 90 minuti: {0} su {1}.", fresh, Giro.Stations.Count)
                    + (p.Length >= 4 ? " " + Lang.F("Ultimo aggiornamento {0}.", p[0]) + (p[3] != "0" ? " " + Lang.F("Senza risposta: {0}.", p[3]) : "") : "");
            }
            FillTable(); Draw(); Details();
        }

        static string F1(double v) { return double.IsNaN(v) ? "-" : v.ToString("F1", CultureInfo.CurrentCulture); }

        void FillTable()
        {
            tSt.Rows.Clear();
            int si = -1;
            foreach (Row r in rows)
            {
                bool fresh = r.St.Fresh;
                Color? fe = double.IsNaN(r.FoEs) ? (Color?)null : Opaque(EsTab.FoEsColor(r.FoEs));
                tSt.Rows.Add(new TableRow
                {
                    Cells = new[] { r.St.Code, r.St.Name, double.IsNaN(r.Km) ? "-" : r.Km.ToString("F0", CultureInfo.InvariantCulture),
                        r.When == DateTime.MinValue ? "-" : r.When.ToString("HH:mm", CultureInfo.InvariantCulture), F1(r.FoEs), F1(r.FoF2), F1(r.Muf),
                        r.SpotN > 0 ? F1(r.SpotFoEs) + " (" + r.SpotN + ")" : "" },
                    Fore = fresh ? (Color?)null : Pal.Muted,
                    CellFore = new Color?[] { null, null, null, null, fe, null, null, r.SpotN > 0 ? (Color?)Pal.Yellow : null },
                    Tag = r.St,
                    CellTips = new[] { "", r.St.Error.Length > 0 ? Lang.F("Ultimo errore: {0}", r.St.Error) : "", "", r.When == DateTime.MinValue ? "" : r.When.ToString("yyyy-MM-dd HH:mm 'UTC'", CultureInfo.InvariantCulture), "", "", "", "" }
                });
                if (r.St == sel) si = tSt.Rows.Count - 1;
            }
            tSt.SelectedIndex = si;
            tSt.Refresh();
        }

        static Color Opaque(Color c) { return Color.FromRgb(c.R, c.G, c.B); }

        void Draw()
        {
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear();
            map.Caption = Lang.T("IONOSONDE GIRO - ultima foEs (MHz)");
            map.LegendTitle = Lang.T("foEs della ionosonda");
            foreach (Row r in rows)
            {
                bool isSel = r.St == sel;
                bool has = !double.IsNaN(r.FoEs);
                Color fill = has ? Opaque(EsTab.FoEsColor(r.FoEs)) : r.St.Fresh ? Pal.Silver : Pal.Muted;
                string lab = isSel ? r.St.Code + (has ? " " + r.FoEs.ToString("F1", CultureInfo.CurrentCulture) : "") : has ? r.FoEs.ToString("F1", CultureInfo.CurrentCulture) : null;
                map.Markers.Add(new MapMarker { Lat = r.St.Lat, Lon = r.St.Lon, Radius = isSel ? 7 : has ? 5.5 : 3.5, Fill = fill, Stroke = isSel ? Pal.Fore : Colors.Black, Ring = !r.St.Fresh, Label = lab, LabelColor = has || isSel ? Pal.Fore : Pal.Muted, Glow = isSel });   // scritta chiara: i blu della scala foEs si leggono male sul fondo
                if (isSel) map.Lines.Add(MapView.Circle(r.St.Lat, r.St.Lon, Ui.V(numRadius), Pal.A(160, Pal.Yellow), 1.2));
            }
            if (Station.Valid)
                map.Markers.Add(new MapMarker { Lat = Station.Lat, Lon = Station.Lon, Radius = 5, Fill = Pal.Green, Square = true, Label = Station.Call.Length > 0 ? Station.Call : Station.Loc, LabelColor = Pal.Green });
            foreach (double f in new[] { 4.0, 6.0, 8.5, 12.0, 17.0, 25.0 })
                map.Legend.Add(new KeyValuePair<string, Color>(f < 5 ? "< 5 MHz" : f < 7 ? "5-7 MHz" : f < 10 ? "7-10 MHz" : f < 14 ? "10-14 MHz" : f < 20 ? "14-20 MHz" : "≥ 20 MHz", Opaque(EsTab.FoEsColor(f))));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("grigio: nessun dato recente (anello: oltre 90 minuti)"), Pal.Silver));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.F("cerchio giallo: spot entro {0:F0} km dalla ionosonda scelta", Ui.V(numRadius)), Pal.Yellow));
            map.InvalidateVisual();
        }

        void Details()
        {
            chart.Series.Clear();
            if (sel == null) { chart.Title = ""; chart.InvalidateVisual(); txtDet.Text = Lang.T("Nessuna ionosonda scelta."); return; }
            // 24 ore a passi di 15 minuti (96 colonne), valori mancanti < 0
            DateTime end = DateTime.UtcNow, start = end.AddHours(-24);
            start = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute / 15 * 15, 0, DateTimeKind.Utc);
            double[] fe = new double[96], f2 = new double[96], mu = new double[96];
            for (int i = 0; i < 96; i++) { fe[i] = -1; f2[i] = -1; mu[i] = -1; }
            lock (sel.Recs)
                foreach (GiroRec r in sel.Recs)
                {
                    int k = (int)Math.Floor((r.Utc - start).TotalMinutes / 15);
                    if (k < 0 || k >= 96) continue;
                    if (!double.IsNaN(r.FoEs)) fe[k] = Math.Max(fe[k], r.FoEs);
                    if (!double.IsNaN(r.FoF2)) f2[k] = Math.Max(f2[k], r.FoF2);
                    if (!double.IsNaN(r.Muf)) mu[k] = Math.Max(mu[k], r.Muf);
                }
            string[] xl = new string[96];
            for (int i = 0; i < 96; i++) { DateTime t = start.AddMinutes(15 * i); xl[i] = t.Minute == 0 && t.Hour % 3 == 0 ? t.ToString("HH", CultureInfo.InvariantCulture) : ""; }
            chart.XLabels = xl; chart.XLabelEvery = 1;
            chart.Title = sel.Code + " " + sel.Name + " (UTC)";
            chart.Series.Add(new ChartSeries { Name = "foEs", Color = Pal.Yellow, Values = fe, LineWidth = 2.5 });
            chart.Series.Add(new ChartSeries { Name = "foF2", Color = Pal.Azure, Values = f2, LineWidth = 1.5 });
            chart.Series.Add(new ChartSeries { Name = "MUF(D)", Color = Pal.Violet, Values = mu, LineWidth = 1.5, Dotted = true });
            chart.InvalidateVisual();

            StringBuilder sb = new StringBuilder();
            Row row = rows.FirstOrDefault(x => x.St == sel);
            DateTime when;
            double foEs = sel.LastValue(x => x.FoEs, Giro.FreshMin, out when);
            sb.AppendLine(sel.Code + "  " + sel.Name + "  " + Geo.LatLonToLoc(sel.Lat, sel.Lon, 4) + (row != null && !double.IsNaN(row.Km) ? "  " + Lang.F("{0:F0} km dalla stazione", row.Km) : ""));
            if (double.IsNaN(foEs))
            {
                sb.AppendLine(Lang.T("Nessuna foEs negli ultimi 90 minuti: niente E-sporadico sopra la ionosonda, oppure la ionosonda non trasmette."));
            }
            else
            {
                double H = Ui.V(numH);
                sb.AppendLine(Lang.F("foEs {0:F1} MHz alle {1:HH:mm} UTC.", foEs, when));
                sb.AppendLine(Lang.F("MUF dell'E-sporadico per un salto con la riflessione sopra la ionosonda (quota {0:F0} km):", H));
                foreach (double d in Dists)
                {
                    double muf = foEs * Geo.SecantFactor(d, H);
                    List<string> open = new List<string>();
                    for (int i = 0; i < BandMHz.Length; i++) if (muf >= BandMHz[i]) open.Add(BandName[i]);
                    sb.AppendLine("  " + Lang.F("{0:F0} km: MUF {1:F1} MHz", d, muf) + (open.Count > 0 ? " → " + string.Join(", ", open) : " → " + Lang.T("nessuna banda")));
                }
                double need50 = 50.2 / Geo.SecantFactor(2000, H);
                sb.AppendLine(Lang.F("Per i 50 MHz a 2000 km serve una foEs di almeno {0:F1} MHz; per i 144 MHz a 2000 km almeno {1:F1} MHz.", need50, 144.3 / Geo.SecantFactor(2000, H)));
            }
            if (row != null && row.SpotN > 0)
            {
                sb.AppendLine();
                sb.AppendLine(Lang.F("Spot Es degli ultimi 30 minuti con un punto di riflessione entro {0:F0} km: {1}; foEs minima ricavata dagli spot {2:F1} MHz.", Ui.V(numRadius), row.SpotN, row.SpotFoEs));
                if (!double.IsNaN(foEs) && row.SpotFoEs > foEs + 0.5)
                    sb.AppendLine(Lang.T("Gli spot chiedono più della foEs misurata: la nube può essere lontana dalla verticale della ionosonda, oppure la ionosonda è arrivata al limite del suo campo di frequenza (6,5-10 MHz secondo Chartier 2022) e la foEs vera è più alta."));
            }
            sb.AppendLine();
            sb.AppendLine(Lang.T("La foEs della ionosonda vale per la verticale della stazione; i valori sono letti in automatico dagli ionogrammi (ARTIST) e possono avere errori. La MUF usa la stessa legge della secante della linguetta REAL-TIME."));
            txtDet.Text = sb.ToString();
        }

        // fotografie automatiche: aspetta i dati (al massimo 'seconds' secondi) e sceglie la ionosonda piu' vicina con la foEs
        public async Task ShotWait(int seconds)
        {
            Activate();
            for (int i = 0; i < seconds * 2 && (Giro.Busy || Giro.LastFetch == DateTime.MinValue); i++) await Task.Delay(500);
            Row r = rows.FirstOrDefault(x => !double.IsNaN(x.FoEs)) ?? rows.FirstOrDefault(x => x.St.Fresh);
            if (r != null) { sel = r.St; }
            Refresh();
        }

        public string ShotText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(StatusText);
            foreach (GiroStation s in Giro.Stations)
            {
                GiroRec l = s.Last;
                sb.AppendLine(s.Code + ";" + s.Name + ";" + s.Recs.Count + ";" + (l != null ? l.Utc.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) + ";" + S(l.FoEs) + ";" + S(l.FoF2) + ";" + S(l.Muf) : ";;;") + ";" + s.Error);
            }
            sb.AppendLine();
            sb.AppendLine(txtDet.Text);
            return sb.ToString();
        }

        static string S(double v) { return double.IsNaN(v) ? "" : v.ToString("0.##", CultureInfo.InvariantCulture); }
    }
}
