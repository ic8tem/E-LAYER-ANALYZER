// E-LAYER ANALYZER - linguetta SPORADIC-E: mappa (65 %) degli spot con i punti di riflessione e i quadrati Maidenhead
// delle nubi Es stimate; a destra (35 %) i comandi, gli spot, le nubi e i dettagli.
// La mappa e' azimutale centrata sulla propria stazione: per il singolo salto (raggio fisso 2500 km, zone della tabella
// IK3XTV) oppure con il cursore della distanza massima, per seguire l'evoluzione del multi-salto; resta la mappa rettangolare.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class EsTab
    {
        public readonly Control Root;
        readonly MapView map = new MapView();
        readonly CheckBox c10, c8, c6, c4, c2, chkFirst, chkShort, chkCells, chkProj, chkHes;
        readonly ComboBox cmbWin;
        readonly NumericUpDown numH, numElev, numShort;
        readonly TextBlock lblSummary;
        readonly TableView tSpots, tClouds, tPre;
        List<PrecursorAlert> alerts = new List<PrecursorAlert>();
        PrecursorAlert selAlert;
        string selAlertKey;
        readonly SelectableTextBlock txtDet;
        readonly PillTabs inner = new PillTabs();
        readonly ComboBox cmbMap;
        readonly RangeBar range;
        public const double SingleHopRangeKm = 2500;
        static readonly int[] Windows = { 15, 30, 60, 120, 240, 720 };
        List<EsPath> paths = new List<EsPath>();
        List<Cloud> clouds = new List<Cloud>();
        EsPath sel;
        string selKey;            // lo spot scelto resta scelto anche quando la lista si ricostruisce con gli spot nuovi
        List<EsPath> shown = new List<EsPath>();
        double clickLat = double.NaN, clickLon = double.NaN;
        bool dirty = true;
        int short144, held144, tep, heldAtl, trEs;
        List<EsModel.EsTropo> esTropo = new List<EsModel.EsTropo>();   // spot in 144 MHz sotto i 900 km e in attesa di conferma (regola IC8TEM)

        class Cloud { public string Sq; public double Lat, Lon, FoEs, Az, Dist, Muf, MaxMHz, MufMax; public int N; public bool Open144; public List<string> Calls = new List<string>(); }

        public EsTab()
        {
            Config cf = Station.Cfg;
            c10 = Ui.C("10 m", cf.GetB("es_10m", false)); c8 = Ui.C("8 m", cf.GetB("es_8m", false)); c6 = Ui.C("6 m", cf.GetB("es_6m", true));
            c4 = Ui.C("4 m", cf.GetB("es_4m", true)); c2 = Ui.C("2 m", cf.GetB("es_2m", true));
            List<string> wn = new List<string>();
            foreach (int w in Windows) wn.Add(Lang.F("ultimi {0} min", w));
            cmbWin = Ui.Combo(wn, Math.Max(0, Array.IndexOf(Windows, (int)cf.GetD("es_minuti", 60))), 120);
            numH = Ui.N(80, 130, Ui.Dm(cf.GetD("es_quota_km2", EsModel.DefaultH)), 0, 1, 45);
            numElev = Ui.N(0, 10, Ui.Dm(cf.GetD("es_elev_min2", EsModel.DefaultElevMin)), 1, 0.5m, 45);
            chkHes = Ui.C(Lang.T("h'Es GIRO"), cf.GetB("es_quota_giro", false));
            ToolTip.SetTip(chkHes, Lang.T("Quota dello strato Es dalle ionosonde GIRO: mediana delle h'Es degli ultimi 90 minuti entro 2500 km dalla tua stazione. Senza dati recenti vale la quota scritta."));
            chkHes.IsCheckedChanged += (s, e) => { numH.IsEnabled = chkHes.IsChecked != true; dirty = true; Refresh(); };
            numH.IsEnabled = chkHes.IsChecked != true;
            chkFirst = Ui.C(Lang.T("Primi salti (salti uguali)"), cf.GetB("es_primo_salto", true));
            chkShort = Ui.C(Lang.T("Primo salto corto entro"), cf.GetB("es_salto_corto", true));
            numShort = Ui.N(400, 2400, Ui.Dm(cf.GetD("es_salto_corto_km", 1200)), 0, 50, 55);
            chkCells = Ui.C(Lang.T("Nubi Es sui quadrati Maidenhead"), cf.GetB("es_celle", true));
            chkProj = Ui.C(Lang.T("Proiezione del mio segnale in 6 m"), cf.GetB("es_proiezione_6m", true));
            Button bView = Ui.B(Lang.T("Europa"));
            bView.Click += (s, e) => { cmbMap.SelectedIndex = 1; map.SetView(-30, 60, 20, 72); };
            Button bWorld = Ui.B(Lang.T("Mondo"));
            bWorld.Click += (s, e) => { cmbMap.SelectedIndex = 1; map.SetView(-180, 180, -75, 80); };
            // proiezione: 0 = azimutale equidistante centrata sulla stazione (zoom con il cursore), 1 = Mercatore
            int oldMode = (int)cf.GetD("es_mappa", 0);   // versioni fino alla 0.4.3: 0/1 azimutale, 2 rettangolare
            int startMode = cf.Get("es_proiezione", "").Length > 0 ? (int)cf.GetD("es_proiezione", 0) : (oldMode == 2 ? 1 : 0);
            cmbMap = Ui.Combo(new[] { Lang.T("Azimutale"), Lang.T("Mercatore") }, Math.Max(0, Math.Min(1, startMode)), 160);
            range = new RangeBar(map, cf.GetD("es_raggio_km", 6000));
            range.Changed += km => { if (cmbMap.SelectedIndex == 0) { multiRange = km; Draw(); } };
            cmbMap.SelectionChanged += (s, e) => { MapMode(); Draw(); };
            map.Note = PropData.NoteEs;
            // continente scelto nella barra delle linguette e sintesi dei dati di propagazione in sovraimpressione
            PropData.Updated += () => Avalonia.Threading.Dispatcher.UIThread.Post(() => map.InvalidateVisual());
            Continents.Changed += () => { if (cmbMap.SelectedIndex == 1) Continents.Apply(map); };
            foreach (CheckBox c in new[] { c10, c8, c6, c4, c2, chkFirst, chkShort, chkCells, chkProj }) c.IsCheckedChanged += (s, e) => { dirty = true; Refresh(); };
            cmbWin.SelectionChanged += (s, e) => { dirty = true; Refresh(); };
            foreach (NumericUpDown n in new[] { numH, numElev, numShort }) n.ValueChanged += (s, e) => { if (autoSet) return; dirty = true; Refresh(); };

            lblSummary = new TextBlock { Foreground = Pal.B(Pal.Green), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4) };

            tSpots = new TableView();
            tSpots.Columns.AddRange(new[] { "UTC", "MHz", "DX", "Spotter", "km", Lang.T("Salti"), "foEs≥", "Prop" });
            tSpots.ColumnWidths.AddRange(new double[] { 46, 66, 82, 82, 52, 40, 52, 40 });
            tSpots.RowClicked += i =>
            {
                if (i < 0 || i >= tSpots.Rows.Count) return;
                sel = tSpots.Rows[i].Tag as EsPath;
                selKey = sel != null ? sel.Spot.Key : null; selAlert = null; selAlertKey = null;
                clickLat = clickLon = double.NaN;
                FocusOn(sel);
                Draw(); Details();
            };
            tClouds = new TableView();
            tClouds.Columns.AddRange(new[] { Lang.T("Quadrato"), "foEs≥", "Spot", "Az°", "km", Lang.T("MUF mia") });
            tClouds.ColumnWidths.AddRange(new double[] { 70, 56, 44, 44, 56, 70 });
            tClouds.RowClicked += i =>
            {
                if (i < 0 || i >= tClouds.Rows.Count) return;
                Cloud c = tClouds.Rows[i].Tag as Cloud;
                if (c != null) { clickLat = c.Lat; clickLon = c.Lon; sel = null; selKey = null; selAlert = null; selAlertKey = null; Draw(); Details(); }
            };
            inner.Add(Lang.T("Spot Es"), tSpots);
            inner.Add(Lang.T("Nubi Es (quadrati)"), tClouds);
            // allerta multi-salto dai precursori (nubi attive negli altri continenti sui punti di salto delle tratte trasversali)
            tPre = new TableView();
            tPre.Columns.AddRange(new[] { Lang.T("Livello"), Lang.T("Regione"), Lang.T("Banda"), Lang.T("Salti"), Lang.T("Ipotesi"), "km", "Az°" });
            tPre.ColumnWidths.AddRange(new double[] { 92, 170, 44, 50, 90, 56, 40 });
            tPre.RowClicked += i =>
            {
                if (i < 0 || i >= tPre.Rows.Count) return;
                selAlert = tPre.Rows[i].Tag as PrecursorAlert;
                selAlertKey = selAlert != null ? selAlert.Key : null;
                sel = null; selKey = null; clickLat = clickLon = double.NaN;
                Draw(); Details();
            };
            inner.Add(Lang.T("Precursori multi-salto"), tPre);

            txtDet = Ui.Text(12.5);
            ScrollViewer svDet = new ScrollViewer { Content = txtDet, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            Ui.Fit(svDet, 250, 110);

            DockPanel right = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Bande")), c10, c8, c6, c4, c2, cmbWin), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Quota Es km")), numH, chkHes, Ui.L(Lang.T("Elev. min °")), numElev, bView, bWorld), Dock.Top);
            Ui.Dock(right, Ui.Row(chkFirst, chkShort, numShort, Ui.L("km"), chkCells, chkProj), Dock.Top);
            Ui.Dock(right, lblSummary, Dock.Top);
            Ui.Dock(right, Ui.Box(Lang.T("Dettagli (clic su uno spot, su una nube o sulla mappa)"), Pal.Azure, svDet), Dock.Bottom);
            Ui.Dock(right, inner.Bar, Dock.Top);
            right.Children.Add(inner.Host);

            map.SetView(-30, 60, 20, 72);
            map.Clicked += (la, lo, rightBtn) =>
            {
                // clic vicino a uno spot (stazioni o punti di riflessione, entro 12 pixel): si sceglie lo spot; altrimenti il punto
                EsPath hit = Nearest(la, lo);
                if (hit != null) { sel = hit; selKey = hit.Spot.Key; selAlert = null; selAlertKey = null; clickLat = clickLon = double.NaN; FillTables(); Draw(); Details(); return; }
                clickLat = la; clickLon = lo; sel = null; selKey = null; selAlert = null; selAlertKey = null; Draw(); Details();
            };
            // la tabella mostra solo gli spot che si vedono sulla mappa: si aggiorna dopo zoom, spostamento o cambio del raggio
            DispatcherTimer viewTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
            viewTimer.Tick += (s, e) => { viewTimer.Stop(); FillTables(); };
            map.ViewChanged += () => { viewTimer.Stop(); viewTimer.Start(); };
            DockPanel mapHost = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(mapHost, Ui.Row(Ui.L(Lang.T("Mappa")), cmbMap, range.Root), Dock.Top);
            mapHost.Children.Add(map);
            MapMode();
            Root = Ui.MapAndData(mapHost, right);

            SpotStore.Added += s => dirty = true;
            Station.Changed += () => { MapMode(); dirty = true; Refresh(); };
            DispatcherTimer tb = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(600) };
            tb.Tick += (s, e) =>
            {
                if (blink.Count == 0) return;
                blinkOn = !blinkOn;
                foreach (KeyValuePair<MapCell, Color> kv in blink)
                    kv.Key.Fill = blinkOn ? kv.Value : Color.FromArgb(40, kv.Value.R, kv.Value.G, kv.Value.B);
                map.InvalidateVisual();
            };
            tb.Start();
            DispatcherTimer t = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            t.Tick += (s, e) => { if (dirty && Root.IsEffectivelyVisible) Refresh(); };
            t.Start();
        }

        public void Save()
        {
            Config cf = Station.Cfg;
            cf.Set("es_10m", c10.IsChecked == true); cf.Set("es_8m", c8.IsChecked == true); cf.Set("es_6m", c6.IsChecked == true); cf.Set("es_4m", c4.IsChecked == true); cf.Set("es_2m", c2.IsChecked == true);
            cf.Set("es_minuti", Windows[Math.Max(0, cmbWin.SelectedIndex)]);
            cf.Set("es_quota_km2", Ui.V(numH)); cf.Set("es_elev_min2", Ui.V(numElev)); cf.Set("es_quota_giro", chkHes.IsChecked == true);
            cf.Set("es_primo_salto", chkFirst.IsChecked == true); cf.Set("es_salto_corto", chkShort.IsChecked == true);
            cf.Set("es_salto_corto_km", Ui.V(numShort)); cf.Set("es_celle", chkCells.IsChecked == true); cf.Set("es_proiezione_6m", chkProj.IsChecked == true);
            cf.Set("es_proiezione", Math.Max(0, cmbMap.SelectedIndex));
            if (!double.IsNaN(multiRange)) cf.Set("es_raggio_km", multiRange);
        }

        // 0 = azimutale equidistante centrata sulla stazione, con il cursore dello zoom (km dal centro); 1 = Mercatore.
        // In entrambe si disegnano i cerchi delle distanze di IK3XTV attorno alla stazione.
        double multiRange = double.NaN;   // raggio scelto con il cursore (vista azimutale)
        void MapMode()
        {
            int m = Math.Max(0, cmbMap.SelectedIndex);
            if (double.IsNaN(multiRange)) multiRange = range.Km;
            map.Azimuthal = m == 0;
            map.Mercator = m == 1;
            map.WheelZoom = m == 0;
            range.Root.IsVisible = m == 0;
            if (Station.Valid) { map.AzLat = Station.Lat; map.AzLon = Station.Lon; }
            if (m == 0) range.Apply(multiRange, true);
            map.InvalidateVisual();
        }

        bool BandOn(Spot s)
        {
            string b = s.Band;
            return (b == "10m" && c10.IsChecked == true) || (b == "8m" && c8.IsChecked == true) || (b == "6m" && c6.IsChecked == true) || (b == "4m" && c4.IsChecked == true) || (b == "2m" && c2.IsChecked == true);
        }

        public static Color BandColor(string b)
        {
            switch (b)
            {
                case "10m": return Pal.Green;
                case "8m": return Pal.Yellow;
                case "6m": return Pal.Azure;
                case "4m": return Pal.Violet;
                case "2m": return Pal.Orange;
                default: return Pal.Silver;
            }
        }

        // scala dei quadrati Maidenhead per frequenza (regola dell'autore): verde fino a 29 MHz, giallo fino a 51,
        // arancio fino a 71, rosso fino a 145 MHz
        public static int FreqClass(double mhz) { return mhz <= 29 ? 0 : mhz <= 51 ? 1 : mhz <= 71 ? 2 : 3; }
        public static Color FreqClassColor(int k, byte a)
        {
            Color c = k == 0 ? Pal.Green : k == 1 ? Pal.Yellow : k == 2 ? Pal.Orange : Pal.Red;
            return Color.FromArgb(a, c.R, c.G, c.B);
        }

        // quadrati che lampeggiano: la MUF stimata per il salto piu' lungo supera la banda degli spot
        readonly List<KeyValuePair<MapCell, Color>> blink = new List<KeyValuePair<MapCell, Color>>();
        bool blinkOn;

        // colore della nube secondo la foEs minima (MHz)
        public static Color FoEsColor(double f)
        {
            if (f < 5) return Pal.Argb(120, 70, 90, 200);
            if (f < 7) return Pal.Argb(130, 56, 189, 248);
            if (f < 10) return Pal.Argb(140, 134, 239, 172);
            if (f < 14) return Pal.Argb(150, 253, 224, 71);
            if (f < 20) return Pal.Argb(160, 251, 146, 60);
            return Pal.Argb(170, 248, 80, 80);
        }

        public double H { get { return Ui.V(numH); } }

        // quota da h'Es delle ionosonde GIRO (casella accanto alla quota): aggiorna il campo; senza dati recenti resta la quota scritta
        void AutoHeight()
        {
            if (chkHes == null || chkHes.IsChecked != true) return;
            if (Giro.Ready && !Giro.Busy && (DateTime.UtcNow - Giro.LastFetch).TotalMinutes > 15) { var _ = Giro.FetchAllAsync(); }
            if (!Station.Valid) return;
            int n; string names;
            double h = Giro.EsHeight(Station.Lat, Station.Lon, 2500, out n, out names);
            if (double.IsNaN(h))
            {
                ToolTip.SetTip(numH, Giro.HesAsked ? Lang.T("Nessuna h'Es recente dalle ionosonde GIRO entro 2500 km: vale la quota scritta.") : Lang.T("Il servizio GIRO non ha dato le h'Es: vale la quota scritta."));
                return;
            }
            h = Math.Max(80, Math.Min(120, Math.Round(h)));   // 80-120 km (autore, 22/11/2026)
            ToolTip.SetTip(numH, Lang.F("h'Es GIRO: {0:F0} km, mediana di {1} ionosonde ({2})", h, n, names));
            if (Math.Abs(Ui.V(numH) - h) >= 0.5) numH.Value = (decimal)h;
        }

        // avviso della testata: apre la sottolinguetta dei precursori sull'allerta scelta
        public void ShowAlert(string key)
        {
            Refresh();
            inner.Select(2);
            PrecursorAlert a = alerts.Find(x => x.Key == key);
            if (a != null) { selAlert = a; selAlertKey = a.Key; sel = null; selKey = null; clickLat = clickLon = double.NaN; }
            FillTables(); Draw(); Details();
        }

        const double UltraKm = 11000;
        List<EsPath> ultra = new List<EsPath>();

        bool autoSet;
        // impronta dei risultati per la misura delle prestazioni (ElaWindow.Perf.cs): tratte, nubi e allerte
        public string PerfSignature()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Func<double, string> f = v => v.ToString("R", CultureInfo.InvariantCulture);
            sb.AppendLine("TRATTE " + paths.Count + " TEP " + tep + " 144 " + short144 + "/" + held144);
            foreach (EsPath p in paths.OrderBy(x => x.Spot.Key, StringComparer.Ordinal))
            {
                sb.Append(p.Spot.Key).Append(' ').Append(p.Hops).Append(' ').Append(f(p.DistKm)).Append(' ').Append(f(p.HopKm)).Append(' ').Append(f(p.FoEsMin)).Append(' ').Append(f(p.ElevDeg));
                foreach (EsHop h in p.Points) sb.Append(' ').Append(f(h.Lat)).Append(',').Append(f(h.Lon));
                sb.AppendLine();
            }
            sb.AppendLine("ALLERTE " + alerts.Count);
            foreach (PrecursorAlert a in alerts.OrderBy(x => x.Key, StringComparer.Ordinal)) sb.AppendLine(a.Key + " " + a.Level + " " + f(a.DistKm));
            return sb.ToString();
        }

        public void Refresh()
        {
            dirty = false;
            autoSet = true;
            try { AutoHeight(); } catch { } finally { autoSet = false; }
            EsModel.ShortHopKm = chkShort.IsChecked == true ? Ui.V(numShort) : 0;
            int minutes = Windows[Math.Max(0, cmbWin.SelectedIndex)];
            List<Spot> sp = SpotStore.Recent(minutes, s => s.HasBoth && BandOn(s) && s.EsCandidate && !EsModel.Ft8Isolated50(s));   // FT8/FT4 isolati in 50 MHz: meteor scatter
            tep = 0;
            sp = sp.FindAll(s => { if (s.IsTep) { tep++; return false; } return true; });   // transequatoriali: non E-sporadico (regola IC8TEM)
            // 144 MHz verso Canarie, Madeira e Azzorre oltre il singolo salto: Es + tropo, con conferma di due o piu' spot (regola IC8TEM)
            List<Spot> atl = sp.FindAll(EsModel.IsAtlanticTropoCandidate);
            sp = sp.FindAll(s => !EsModel.IsAtlanticTropoCandidate(s));
            esTropo = EsModel.AtlanticTropo(atl, out heldAtl);
            paths = new List<EsPath>();
            HashSet<string> seen = new HashSet<string>();
            foreach (Spot s in sp)
            {
                // un solo spot per coppia di stazioni e banda (il piu' recente)
                string k = s.DxCall + "|" + s.Spotter + "|" + s.Band;
                if (!seen.Add(k)) continue;
                EsPath p = EsModel.Build(s, H);
                if (p != null && p.Hops > 0) paths.Add(p);
            }
            // regola dell'autore per i 144 MHz (EsModel.Apply144Rule): conferma con un secondo spot o con un salto corto in 50 MHz
            List<EsPath> six = new List<EsPath>();
            foreach (Spot s in SpotStore.Recent(minutes + EsModel.Near50Min, x => x.HasBoth && x.Band == "6m" && x.EsCandidate && !EsModel.Ft8Isolated50(x)))
            {
                EsPath p = EsModel.Build(s, H);
                if (p != null && p.Hops == 1) six.Add(p);
            }
            paths = EsModel.Apply144Rule(paths, six, out short144, out held144);
            // modello misto generale E-sporadico + tropo (dalla 0.7.0): 144 MHz fra 2400 e 3400 km, nube nota a meta' del salto Es
            // da un'estremita' e condotto (spot <TR> o profilo Open-Meteo) dall'altra; il doppio salto con due nubi note ha la precedenza
            trEs = 0;
            if (EsForecast.UseTrEs)
            {
                List<EsCloud> cl = EsForecast.Clouds(H, DateTime.UtcNow, SpotStore.Recent(EsForecast.WindowMin, x => x.HasBoth), true);
                List<Spot> trs = TropoDuct.TropoSpots(60);
                List<EsPath> keep = new List<EsPath>();
                foreach (EsPath p in paths)
                {
                    EsModel.EsTropo et = p.Spot.Band == "2m" && p.DistKm > EsForecast.TrEsMinKm ? EsForecast.ClassifyTrEs(p.Spot, H, cl, trs) : null;
                    if (et != null) { esTropo.Add(et); trEs++; } else keep.Add(p);
                }
                paths = keep;
            }
            // notizia statistica: QSO in 50 MHz oltre gli 11000 km (EI7GL [56]: 11460 km HZ1SK-K4MOG, almeno sei salti)
            ultra = paths.FindAll(p => p.Spot.Band == "6m" && p.DistKm > UltraKm);
            sel = null;
            if (selKey != null) foreach (EsPath p in paths) if (p.Spot.Key == selKey) { sel = p; break; }
            alerts = Precursors.Compute(H);
            Chains.Notify(alerts);
            selAlert = null;
            if (selAlertKey != null) foreach (PrecursorAlert a in alerts) if (a.Key == selAlertKey) { selAlert = a; break; }
            BuildClouds();
            FillTables();
            Draw();
            Details();
        }

        void BuildClouds()
        {
            Dictionary<string, Cloud> d = new Dictionary<string, Cloud>();
            bool useShort = chkShort.IsChecked == true;
            foreach (EsPath p in paths)
            {
                List<KeyValuePair<EsHop, double>> pts = new List<KeyValuePair<EsHop, double>>();
                if (p.Hops == 1) pts.Add(new KeyValuePair<EsHop, double>(p.FirstA, p.FoEsMin));
                else if (useShort && p.ShortA != null) { pts.Add(new KeyValuePair<EsHop, double>(p.ShortA, p.ShortFoEsMin)); pts.Add(new KeyValuePair<EsHop, double>(p.ShortB, p.ShortFoEsMin)); }
                else { pts.Add(new KeyValuePair<EsHop, double>(p.FirstA, p.FoEsMin)); pts.Add(new KeyValuePair<EsHop, double>(p.FirstB, p.FoEsMin)); }
                foreach (KeyValuePair<EsHop, double> kv in pts)
                {
                    if (kv.Key == null) continue;
                    string sq = Geo.LatLonToLoc(kv.Key.Lat, kv.Key.Lon, 4);
                    Cloud c;
                    if (!d.TryGetValue(sq, out c))
                    {
                        double la, lo; Geo.LocToLatLon(sq, out la, out lo);
                        c = new Cloud { Sq = sq, Lat = la, Lon = lo };
                        d[sq] = c;
                    }
                    c.N++;
                    if (kv.Value > c.FoEs) c.FoEs = kv.Value;
                    if (p.Spot.FreqMHz > c.MaxMHz) c.MaxMHz = p.Spot.FreqMHz;
                    // regola dell'autore: salto singolo in 50 MHz tra 450 e 500 km = la nube puo' aprire i 144 MHz
                    if (p.Hops == 1 && p.Spot.Band == "6m" && p.DistKm >= EsModel.MinKm50 && p.DistKm <= EsModel.MinKm50 + EsModel.Skip144TolKm) c.Open144 = true;
                    if (c.Calls.Count < 6) c.Calls.Add(p.Spot.Spotter + "-" + p.Spot.DxCall);
                }
            }
            double maxHalf = Geo.HalfHopKm(Ui.V(numElev), H, EsModel.EsK);
            foreach (Cloud c in d.Values)
            {
                // MUF probabile della nube sul salto piu' lungo (massima del singolo salto, tabella IK3XTV): foEs x sec(phi)
                c.MufMax = c.FoEs * Geo.SecantFactor(EsModel.SingleMax[1], H);
                c.Az = c.Dist = c.Muf = double.NaN;
                if (!Station.Valid) continue;
                c.Dist = Geo.DistanceKm(Station.Lat, Station.Lon, c.Lat, c.Lon);
                c.Az = Geo.Bearing(Station.Lat, Station.Lon, c.Lat, c.Lon);
                if (c.Dist <= maxHalf && c.Dist >= EsModel.SingleMin[0] / 2) c.Muf = EsModel.MufVia(c.FoEs, c.Dist, H);
            }
            clouds = new List<Cloud>(d.Values);
            clouds.Sort((a, b) =>
            {
                double x = double.IsNaN(a.Muf) ? -1 : a.Muf, y = double.IsNaN(b.Muf) ? -1 : b.Muf;
                int k = y.CompareTo(x);
                return k != 0 ? k : b.FoEs.CompareTo(a.FoEs);
            });
        }

        void FillTables()
        {
            tSpots.Rows.Clear();
            shown = new List<EsPath>();
            foreach (EsPath p in paths) if (p == sel || Visible(p)) shown.Add(p);
            foreach (EsPath p in shown)
            {
                Spot s = p.Spot;
                TableRow r = new TableRow
                {
                    Cells = new[] { s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), s.FreqMHz.ToString("F3", CultureInfo.CurrentCulture), s.DxCall, s.Spotter,
                        p.DistKm.ToString("F0", CultureInfo.CurrentCulture), p.Hops.ToString(CultureInfo.InvariantCulture), p.FoEsMin.ToString("F1", CultureInfo.CurrentCulture), s.IsEsTep ? "ES+TEP" : s.Prop },
                    Fore = s.IsEsTep ? ElectricBlue : BandColor(s.Band),
                    Tag = p
                };
                tSpots.Rows.Add(r);
            }
            tSpots.SelectedIndex = sel != null ? shown.IndexOf(sel) : -1;
            tSpots.Refresh();
            tClouds.Rows.Clear();
            foreach (Cloud c in clouds)
            {
                if (!map.InView(c.Lat, c.Lon)) continue;
                Color fore = !double.IsNaN(c.Muf) && c.Muf >= 144 ? Pal.Red : !double.IsNaN(c.Muf) && c.Muf >= 70 ? Pal.Orange : !double.IsNaN(c.Muf) && c.Muf >= 50 ? Pal.Yellow : Pal.Fore;
                tClouds.Rows.Add(new TableRow
                {
                    Cells = new[] { c.Sq, c.FoEs.ToString("F1", CultureInfo.CurrentCulture), c.N.ToString(CultureInfo.InvariantCulture), Geo.Fmt(c.Az, 0), Geo.Fmt(c.Dist, 0), double.IsNaN(c.Muf) ? "-" : c.Muf.ToString("F0", CultureInfo.CurrentCulture) + " MHz" },
                    Fore = fore,
                    Tag = c
                });
            }
            tClouds.Refresh();
            tPre.Rows.Clear();
            foreach (PrecursorAlert a in alerts)
            {
                AprsAlert aa = a as AprsAlert;
                if (aa != null)
                {
                    tPre.Rows.Add(new TableRow
                    {
                        Cells = new[] { aa.LevelText, aa.Region, aa.Band, "1", Lang.F("APRS ripetuto ({0})", aa.N),
                            aa.DistKm.ToString("F0", CultureInfo.CurrentCulture), double.IsNaN(aa.Az) ? "-" : aa.Az.ToString("F0", CultureInfo.InvariantCulture) },
                        Fore = Pal.Red,
                        Tag = a
                    });
                    continue;
                }
                TepAlert ta = a as TepAlert;
                if (ta != null)
                {
                    tPre.Rows.Add(new TableRow
                    {
                        Cells = new[] { ta.LevelText, ta.Region, ta.Band, "1", Lang.F("{0} {1:00}h, indice {2:F0}", ta.Kind, Math.Floor(ta.LocalHour), ta.Index),
                            ta.DistKm.ToString("F0", CultureInfo.CurrentCulture), ta.Az.ToString("F0", CultureInfo.InvariantCulture) },
                        Fore = ElectricBlue,
                        Tag = a
                    });
                    continue;
                }
                ProdromeAlert pa = a as ProdromeAlert;
                if (pa != null)
                {
                    bool near = !double.IsNaN(pa.NearKm) && pa.NearKm <= Prodromes.MaxFromStationKm;
                    tPre.Rows.Add(new TableRow
                    {
                        Cells = new[] { pa.LevelText, pa.Region, pa.Band, "1", Lang.F("nube in {0}", pa.FromBand == "12 m" ? "24 MHz" : "28 MHz"),
                            double.IsNaN(pa.DistKm) ? "-" : pa.DistKm.ToString("F0", CultureInfo.CurrentCulture), double.IsNaN(pa.Az) ? "-" : pa.Az.ToString("F0", CultureInfo.InvariantCulture) },
                        Fore = Pal.Green,
                        CellFore = near ? null : new Color?[] { null, Pal.Muted, null, null, Pal.Muted, null, null },
                        Tag = a
                    });
                    continue;
                }
                ChainAlert ca = a as ChainAlert;
                if (ca != null)
                {
                    // catene con i salti corti alle estremita': salti = corto + centrali + corto
                    bool mine = ca.Kind == ChainAlert.KindQth || ca.Kind == ChainAlert.KindEqual || (!double.IsNaN(ca.NearKm) && ca.NearKm <= Chains.NearMeKm);
                    tPre.Rows.Add(new TableRow
                    {
                        Cells = new[] { ca.LevelText, ca.Region, ca.Band, ca.Kind == ChainAlert.KindSearch ? "1+?+1" : ca.Kind == ChainAlert.KindEqual || ca.Kind == ChainAlert.KindEqualCorridor ? "2 =" : "1+" + ca.Middle + "+1", ca.KindText,
                            ca.DistKm.ToString("F0", CultureInfo.CurrentCulture), ca.Az.ToString("F0", CultureInfo.InvariantCulture) },
                        Fore = ca.Level == 2 ? Pal.Red : ca.Level == 1 ? Pal.Yellow : Pal.Azure,
                        CellFore = mine ? null : new Color?[] { null, Pal.Muted, null, null, Pal.Muted, null, null },
                        Tag = a
                    });
                    continue;
                }
                tPre.Rows.Add(new TableRow
                {
                    Cells = new[] { a.LevelText, Lang.T(a.Region), a.Band, a.Covered + "/" + a.Points.Count, a.ShortHyp ? Lang.F("corto {0:F0} km", a.ShortKm) : Lang.T("uguali"),
                        a.DistKm.ToString("F0", CultureInfo.CurrentCulture), a.Az.ToString("F0", CultureInfo.InvariantCulture) },
                    Fore = a.Level == 2 ? Pal.Red : Pal.Yellow,
                    Tag = a
                });
            }
            tPre.SelectedIndex = selAlert != null ? alerts.IndexOf(selAlert) : -1;
            tPre.Refresh();
            int multi = 0; foreach (EsPath p in paths) if (p.Hops >= 2) multi++;
            Cloud best = clouds.Count > 0 && !double.IsNaN(clouds[0].Muf) ? clouds[0] : null;
            lblSummary.Text = Lang.F("{0} spot Es ({1} multi-salto), {2} quadrati con nubi stimate.", paths.Count, multi, clouds.Count)
                + (shown.Count < paths.Count ? " " + Lang.F("In tabella i {0} visibili sulla mappa.", shown.Count) : "")
                + (tep > 0 ? " " + Lang.F("{0} spot transequatoriali (TEP) esclusi.", tep) : "")
                + (short144 + held144 > 0 ? " " + Lang.F("144 MHz: {0} spot sotto i 900 km scartati, {1} in attesa di conferma (regola IC8TEM).", short144, held144) : "")
                + (esTropo.Count - trEs + heldAtl > 0 ? " " + Lang.F("144 MHz verso Canarie, Madeira e Azzorre: {0} spot Es + tropo confermati, {1} in attesa di un secondo spot sulla stessa nube.", esTropo.Count - trEs, heldAtl) : "")
                + (trEs > 0 ? " " + Lang.F("144 MHz oltre il singolo salto: {0} spot TR+ES (nube ricollocata, condotto dall'altra estremità).", trEs) : "")
                + (alerts.Count > 0 ? " " + Lang.F("Multi-salto: {0} allerte e {1} precursori (sottolinguetta Precursori multi-salto).", alerts.Count(a => a.Level == 2 && !(a is TepAlert)), alerts.Count(a => a.Level == 1 && !(a is TepAlert))) : "")
                + (alerts.Any(a => a is TepAlert) ? " " + Lang.F("TEP: {0}.", string.Join(", ", alerts.OfType<TepAlert>().Select(t => t.Region + " " + t.Index.ToString("F0", CultureInfo.CurrentCulture)))) : "")
                + (Precursors.LastStrong.Count > 0 ? " " + Lang.F("Nubi forti per il salto corto in 50 MHz: {0}.", Precursors.LastStrong.Count) : "")
                + ProjText()
                + (best != null ? " " + Lang.F("MUF più alta per {0}: {1:F0} MHz via {2} (az {3:F0}°).", Station.Call.Length > 0 ? Station.Call : Station.Loc, best.Muf, best.Sq, best.Az) : "");
        }

        void Draw()
        {
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear();
            map.Caption = "SPORADIC-E";
            blink.Clear();
            if (chkCells.IsChecked == true)
                foreach (Cloud c in clouds)
                {
                    double la0 = Math.Floor(c.Lat), lo0 = Math.Floor(c.Lon / 2) * 2;
                    int kf = FreqClass(c.MaxMHz), km = FreqClass(c.MufMax);
                    // i 144 MHz (classe 3) si segnalano solo con la regola dell'autore del salto corto in 50 MHz, non con la secante
                    if (km >= 3 && !c.Open144) km = 2;
                    if (c.Open144) km = 3;
                    MapCell mc = new MapCell { Lat0 = la0, Lon0 = lo0, DLat = 1, DLon = 2, Fill = FreqClassColor(kf, 150),
                        Tip = Lang.F("{0}: spot fino a {1:F1} MHz, foEs ≥ {2:F1} MHz, MUF probabile fino a {3:F0} MHz", c.Sq, c.MaxMHz, c.FoEs, c.MufMax)
                          + (c.Open144 ? "\n" + Lang.F("Salto in 50 MHz tra {0:F0} e {1:F0} km: la nube può aprire i 144 MHz (regola dell'autore)", EsModel.MinKm50, EsModel.MinKm50 + EsModel.Skip144TolKm) : "") };
                    map.Cells.Add(mc);
                    if (km > kf) blink.Add(new KeyValuePair<MapCell, Color>(mc, FreqClassColor(km, 210)));   // lampeggia con il colore della banda raggiunta
                }
            foreach (EsPath p in paths)
            {
                Spot s = p.Spot;
                bool isSel = p == sel;
                bool esTep = s.IsEsTep;
                Color bc = esTep ? ElectricBlue : BandColor(s.Band);
                if (esTep)
                {
                    // Es che aggancia il TEP (ombra del TEP): tratta piu' spessa in blu elettrico e scritta TEP sul punto medio
                    double mla, mlo;
                    Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out mla, out mlo);
                    map.Lines.Add(MapView.GreatCircle(s.LatS, s.LonS, s.LatD, s.LonD, Pal.A(isSel ? 255 : 220, ElectricBlue), isSel ? 3.5 : 2.2));
                    map.Markers.Add(new MapMarker { Lat = mla, Lon = mlo, Radius = 3, Fill = ElectricBlue, Stroke = ElectricBlue, Label = "TEP", LabelColor = ElectricBlue });
                }
                if (isSel) map.Lines.Add(MapView.GreatCircle(s.LatS, s.LonS, s.LatD, s.LonD, Pal.A(90, Pal.Yellow), 10));   // alone dello spot scelto
                map.Lines.Add(MapView.GreatCircle(s.LatS, s.LonS, s.LatD, s.LonD, isSel ? Pal.Yellow : Pal.A(isSel ? 255 : 140, bc), isSel ? 3.5 : 1.2));
                map.Markers.Add(new MapMarker { Lat = s.LatS, Lon = s.LonS, Radius = isSel ? 4 : 2.5, Fill = bc, Label = isSel ? s.Spotter : null });
                map.Markers.Add(new MapMarker { Lat = s.LatD, Lon = s.LonD, Radius = isSel ? 4 : 2.5, Fill = bc, Label = isSel ? s.DxCall : null });
                if (chkFirst.IsChecked == true || isSel)
                {
                    for (int i = 0; i < p.Points.Count; i++)
                    {
                        EsHop h = p.Points[i];
                        bool first = i == 0, last = i == p.Points.Count - 1;
                        Color fc = p.Hops == 1 ? Pal.Yellow : first ? Pal.Orange : last ? Pal.Violet : Pal.Silver;
                        map.Markers.Add(new MapMarker { Lat = h.Lat, Lon = h.Lon, Radius = isSel ? 5 : 3, Fill = fc, Square = true, Label = isSel ? Geo.LatLonToLoc(h.Lat, h.Lon, 4) : null });
                    }
                }
                if (chkShort.IsChecked == true && p.ShortA != null)
                {
                    map.Markers.Add(new MapMarker { Lat = p.ShortA.Lat, Lon = p.ShortA.Lon, Radius = isSel ? 7 : 5, Fill = Pal.Orange, Ring = true, Label = isSel ? Lang.T("corto ") + Geo.LatLonToLoc(p.ShortA.Lat, p.ShortA.Lon, 4) : null });
                    map.Markers.Add(new MapMarker { Lat = p.ShortB.Lat, Lon = p.ShortB.Lon, Radius = isSel ? 7 : 5, Fill = Pal.Violet, Ring = true, Label = isSel ? Lang.T("corto ") + Geo.LatLonToLoc(p.ShortB.Lat, p.ShortB.Lon, 4) : null });
                }
            }
            if (Station.Valid)
            {
                map.Lines.Add(MapView.Circle(Station.Lat, Station.Lon, Geo.HalfHopKm(Ui.V(numElev), H, EsModel.EsK), Pal.A(150, Pal.Green), 1));
                map.Markers.Add(new MapMarker { Lat = Station.Lat, Lon = Station.Lon, Radius = 5, Fill = Pal.Green, Square = true, Label = Station.Call.Length > 0 ? Station.Call : Station.Loc, LabelColor = Pal.Green });
            }
            // cerchi delle distanze di IK3XTV attorno alla stazione, in tutte e due le proiezioni
            if (map.Azimuthal) AddHopZones(map, map.AzLat, map.AzLon, true);
            else if (Station.Valid) AddHopZones(map, Station.Lat, Station.Lon, true);
            if (!double.IsNaN(clickLat))
            {
                map.Markers.Add(new MapMarker { Lat = clickLat, Lon = clickLon, Radius = 8, Fill = Pal.Red, Ring = true, Label = Geo.LatLonToLoc(clickLat, clickLon, 6) });
                if (Station.Valid) map.Lines.Add(MapView.GreatCircle(Station.Lat, Station.Lon, clickLat, clickLon, Pal.Red, 1.5, true));
            }
            // Es + tropo verso le isole atlantiche (144 MHz): salto Es pieno, nube, tratta tropo tratteggiata
            foreach (EsModel.EsTropo et in esTropo)
            {
                Spot s = et.Spot;
                bool mainS = !EsModel.InAtlanticIslands(s.LatS, s.LonS);
                double mLa = mainS ? s.LatS : s.LatD, mLo = mainS ? s.LonS : s.LonD;
                if (et.General) { mLa = et.EsLat; mLo = et.EsLon; }
                map.Lines.Add(MapView.GreatCircle(mLa, mLo, et.LandLat, et.LandLon, Pal.A(220, Pal.Orange), 1.6));
                map.Lines.Add(MapView.GreatCircle(et.LandLat, et.LandLon, et.IsleLat, et.IsleLon, Pal.A(220, Pal.Azure), 1.6, true));
                map.Markers.Add(new MapMarker { Lat = et.CloudLat, Lon = et.CloudLon, Radius = 5, Fill = Pal.Orange, Square = true, Label = (et.General ? "TR+ES " : "") + Geo.LatLonToLoc(et.CloudLat, et.CloudLon, 4) });
                map.Markers.Add(new MapMarker { Lat = s.LatS, Lon = s.LonS, Radius = 2.5, Fill = Pal.Orange, Label = s.Spotter });
                map.Markers.Add(new MapMarker { Lat = s.LatD, Lon = s.LonD, Radius = 2.5, Fill = Pal.Orange, Label = s.DxCall });
            }
            if (esTropo.Exists(x => !x.General)) map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("144 MHz Es + tropo verso Canarie, Madeira, Azzorre (tratteggio: tropo)"), Pal.Azure));
            if (esTropo.Exists(x => x.General)) map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("TR+ES: salto Es (arancio) con la nube ricollocata e tratta in condotto (tratteggio azzurro)"), Pal.Azure));
            DrawAlerts();
            if (paths.Exists(x => x.Spot.IsEsTep)) map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("blu elettrico, scritta TEP: Es che può agganciare il TEP (non si può sapere dallo spot)"), ElectricBlue));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("10 m / 6 m / 4 m / 2 m: verde, azzurro, viola, arancio"), Pal.Azure));
            if (c8.IsChecked == true) map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("8 m (40 MHz): giallo, solo entro il primo salto"), Pal.Yellow));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ giallo: punto medio (singolo salto)"), Pal.Yellow));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ primo salto dello spotter (salti uguali)"), Pal.Orange));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ primo salto del DX (salti uguali)"), Pal.Violet));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("○ primo salto corto (ipotesi IC8TEM)"), Pal.Orange));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("cerchio verde: portata del singolo salto dalla mia stazione"), Pal.Green));
            map.LegendTitle = Lang.T("Quadrati: spot fino a 29 / 51 / 71 / 145 MHz = verde, giallo, arancio, rosso; lampeggia se la MUF probabile raggiunge una banda più alta (rosso, 144 MHz: salto in 50 MHz tra 450 e 500 km)");
            map.InvalidateVisual();
        }

        EsPath Nearest(double la, double lo)
        {
            Point c = map.ToScreen(la, lo);
            EsPath best = null; double bd = 12;
            foreach (EsPath p in paths)
            {
                List<Point> pts = new List<Point> { map.ToScreen(p.Spot.LatS, p.Spot.LonS), map.ToScreen(p.Spot.LatD, p.Spot.LonD) };
                foreach (EsHop h in p.Points) pts.Add(map.ToScreen(h.Lat, h.Lon));
                foreach (Point q in pts)
                {
                    double d = Math.Sqrt((q.X - c.X) * (q.X - c.X) + (q.Y - c.Y) * (q.Y - c.Y));
                    if (d < bd) { bd = d; best = p; }
                }
            }
            return best;
        }

        // lo spot e' visibile sulla mappa? (una delle due stazioni o un punto di riflessione)
        bool Visible(EsPath p)
        {
            Spot s = p.Spot;
            if (map.InView(s.LatS, s.LonS) || map.InView(s.LatD, s.LonD)) return true;
            foreach (EsHop h in p.Points) if (map.InView(h.Lat, h.Lon)) return true;
            return false;
        }

        // porta in vista lo spot scelto in tabella: nella vista azimutale allarga il raggio, in quella rettangolare inquadra la tratta
        void FocusOn(EsPath p)
        {
            if (p == null) return;
            Spot s = p.Spot;
            if (map.InView(s.LatS, s.LonS) && map.InView(s.LatD, s.LonD)) return;
            if (map.Azimuthal)
            {
                double need = Math.Max(Geo.DistanceKm(map.AzLat, map.AzLon, s.LatS, s.LonS), Geo.DistanceKm(map.AzLat, map.AzLon, s.LatD, s.LonD)) * 1.12 + 100;
                range.Apply(Math.Max(need, map.RangeKm), true);
                return;
            }
            double lw = Math.Min(s.LonS, s.LonD), le = Math.Max(s.LonS, s.LonD), ls = Math.Min(s.LatS, s.LatD), ln = Math.Max(s.LatS, s.LatD);
            if (le - lw > 180) { map.SetView(-180, 180, -60, 80); return; }
            double padLon = Math.Max(8, (le - lw) * 0.25), padLat = Math.Max(5, (ln - ls) * 0.25);
            map.SetView(lw - padLon, le + padLon, Math.Max(-80, ls - padLat), Math.Min(84, ln + padLat));
        }

        // zone della tabella IK3XTV attorno alla stazione al centro della mappa azimutale (distanza del corrispondente):
        // singolo salto minima / ottimale / massima; con multi=true anche il limite del doppio e del triplo salto
        public static void AddHopZones(MapView map, double lat, double lon, bool multi)
        {
            Color zc = Pal.A(200, Pal.Yellow);
            Action<double, string, Color, double> ring = (km, label, c, w) =>
            {
                MapLine l = MapView.Circle(lat, lon, km, c, w); l.Dashed = true; map.Lines.Add(l);
                double la, lo; Geo.Destination(lat, lon, 200, km, out la, out lo);
                map.Markers.Add(new MapMarker { Lat = la, Lon = lo, Radius = 0, Fill = c, Stroke = c, Label = label, LabelColor = c });
            };
            ring(EsModel.SingleMin[0], Lang.F("{0:N0} km: minima del singolo salto", EsModel.SingleMin[0]), zc, 1.2);
            ring(EsModel.SingleOpt[0], Lang.F("{0:N0}-{1:N0} km: ottimale", EsModel.SingleOpt[0], EsModel.SingleOpt[1]), Pal.A(220, Pal.Green), 1.2);
            ring(EsModel.SingleOpt[1], "", Pal.A(220, Pal.Green), 1.2);
            ring(EsModel.SingleMax[1], Lang.F("{0:N0} km: massima del singolo salto", EsModel.SingleMax[1]), Pal.A(220, Pal.Orange), 1.6);
            if (multi)
            {
                ring(EsModel.DoubleMax[1], Lang.F("{0:N0} km: massima del doppio salto", EsModel.DoubleMax[1]), Pal.A(220, Pal.Violet), 1.6);
                ring(EsModel.TripleMax, Lang.F("{0:N0} km: massima del triplo salto", EsModel.TripleMax), Pal.A(220, Pal.Red), 1.6);
            }
        }

        // tratte delle allerte multi-salto: rosso = ALLERTA, giallo = PRECURSORE; punti di salto: verde = nube attiva,
        // grigio = zona senza osservatori, rosso vuoto = zona osservata senza nube
        public static readonly Color ElectricBlue = Color.FromRgb(44, 117, 255);   // blu elettrico: Es che aggancia il TEP

        // proiezione del proprio segnale in 50 MHz (Chains.Project): un punto per quadrato Maidenhead, il numero di salti minimo;
        // i punti in mare aperto o senza stazioni non si disegnano (decisione dell'autore, 20/11/2026)
        List<Chains.Landing> ProjShown(out int sea)
        {
            sea = 0;
            Dictionary<string, Chains.Landing> best = new Dictionary<string, Chains.Landing>();
            HashSet<string> wet = new HashSet<string>();
            foreach (Chains.Landing l in Chains.LastProjection)
            {
                string q = Geo.LatLonToLoc(l.Lat, l.Lon, 4);
                if (!l.Operators) { wet.Add(q); continue; }
                Chains.Landing o;
                if (!best.TryGetValue(q, out o) || l.Hops < o.Hops) best[q] = l;
            }
            foreach (string q in wet) if (!best.ContainsKey(q)) sea++;
            return best.Values.ToList();
        }

        // verifica in diretta della proiezione (dal 22/11/2026): chi ti ascolta in 50 MHz oltre i 2800 km (PSKReporter, cluster,
        // RBN: spot con il tuo nominativo come DX) cade entro 500 km da un punto rosa calcolato in quel momento?
        static readonly HashSet<string> projSeen = new HashSet<string>();
        static int projHeard, projInside;
        void ProjCheck()
        {
            if (!Station.Valid || Station.Call.Length < 3 || Chains.LastProjection.Count == 0) return;
            foreach (Spot s in SpotStore.Recent(15, x => x.HasBoth && x.Band == "6m" && string.Equals(x.DxCall, Station.Call, StringComparison.OrdinalIgnoreCase)))
            {
                if (Geo.DistanceKm(Station.Lat, Station.Lon, s.LatS, s.LonS) < Precursors.MinKm) continue;
                if (!projSeen.Add(s.Spotter + "|" + s.Utc.ToString("yyyyMMddHH", CultureInfo.InvariantCulture))) continue;   // una volta per ora e stazione
                projHeard++;
                if (Chains.LastProjection.Any(p => Geo.DistanceKm(p.Lat, p.Lon, s.LatS, s.LonS) <= 500)) projInside++;
            }
        }

        string ProjText()
        {
            try { ProjCheck(); } catch { }
            string ver = projHeard > 0 ? " " + Lang.F("Verifica della proiezione in questa sessione: {0} stazioni ti hanno ascoltato in 50 MHz oltre i 2800 km, {1} entro 500 km da un punto rosa.", projHeard, projInside) : "";
            if (chkProj.IsChecked != true || !Station.Valid || Chains.LastProjection.Count == 0) return ver;
            int sea;
            List<Chains.Landing> l = ProjShown(out sea);
            if (l.Count == 0) return " " + Lang.F("Proiezione del tuo segnale in 50 MHz: atterra solo in mare aperto o dove non ci sono stazioni ({0} quadrati).", sea) + ver;
            string zones = string.Join(", ", l.GroupBy(x => Chains.RegionName(x.Lat, x.Lon)).OrderByDescending(g => g.Max(x => x.DistKm)).Take(4)
                                       .Select(g => g.Key + " (" + (g.Min(x => x.Hops) == g.Max(x => x.Hops) ? g.Min(x => x.Hops).ToString(CultureInfo.InvariantCulture) : g.Min(x => x.Hops) + "-" + g.Max(x => x.Hops)) + ")"));
            return " " + Lang.F("Proiezione del tuo segnale in 50 MHz: {0} quadrati con stazioni, fino a {1:F0} km; zone (salti): {2}. Non mostrati {3} quadrati in mare aperto o senza stazioni.", l.Count, l.Max(x => x.DistKm), zones, sea) + ver;
        }

        void DrawProjection()
        {
            if (chkProj.IsChecked != true || !Station.Valid) return;
            int sea;
            List<Chains.Landing> l = ProjShown(out sea);
            if (l.Count == 0) return;
            foreach (Chains.Landing p in l)
                map.Markers.Add(new MapMarker { Lat = p.Lat, Lon = p.Lon, Radius = p.Hops <= 2 ? 3 : 4, Fill = Pal.A(p.Hops <= 2 ? 150 : 230, Pal.Pink), Stroke = Pal.A(120, Colors.Black) });
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("● rosa: dove atterra il tuo segnale in 50 MHz sulle nubi stimate (2-5 salti, solo dove ci sono stazioni)"), Pal.Pink));
        }

        void DrawAlerts()
        {
            DrawProjection();
            // nubi forti (reggono un salto corto in 50 MHz): rombi arancioni con la foEs
            foreach (StrongCloud c in Precursors.LastStrong)
                map.Markers.Add(new MapMarker { Lat = c.Lat, Lon = c.Lon, Radius = 5, Fill = Pal.Orange, Stroke = Colors.Black, Square = true, Label = c.FoEs.ToString("F0", CultureInfo.CurrentCulture), LabelColor = Pal.Orange });
            if (Precursors.LastStrong.Count > 0)
                map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("■ arancione: nube forte, regge un salto corto in 50 MHz (foEs in MHz)"), Pal.Orange));
            if (alerts.Count == 0) return;
            foreach (PrecursorAlert a in alerts)
            {
                bool isSel = a == selAlert;
                if (selAlert != null && !isSel) continue;
                ChainAlert ca = a as ChainAlert;
                if (ca != null) { DrawChain(ca, isSel, selAlert != null); continue; }
                AprsAlert aa = a as AprsAlert;
                if (aa != null)
                {
                    foreach (Spot s in SpotStore.Recent(AprsEs.KeepMin, x => x.Source == 4 && x.HasBoth))
                    {
                        double la, lo; Geo.Intermediate(s.LatS, s.LonS, s.LatD, s.LonD, 0.5, out la, out lo);
                        if (Geo.DistanceKm(la, lo, aa.MidLat, aa.MidLon) > 250) continue;
                        map.Lines.Add(MapView.GreatCircle(s.LatS, s.LonS, s.LatD, s.LonD, Pal.A((byte)(isSel ? 255 : 170), Pal.Red), isSel ? 2.6 : 1.4));
                    }
                    if (isSel) map.Markers.Add(new MapMarker { Lat = aa.MidLat, Lon = aa.MidLon, Radius = 0, Label = Lang.T("APRS 144 MHz"), LabelColor = Pal.Red });
                    continue;
                }
                TepAlert tp = a as TepAlert;
                if (tp != null)
                {
                    map.Lines.Add(MapView.GreatCircle(Station.Lat, Station.Lon, tp.EndLat, tp.EndLon, Pal.A((byte)(isSel ? 255 : 150), ElectricBlue), isSel ? 2.6 : 1.4, true));
                    if (isSel) map.Markers.Add(new MapMarker { Lat = tp.EndLat, Lon = tp.EndLon, Radius = 3, Fill = ElectricBlue, Stroke = ElectricBlue, Label = tp.Kind + " " + tp.Region, LabelColor = ElectricBlue });
                    continue;
                }
                ProdromeAlert pa = a as ProdromeAlert;
                if (pa != null)
                {
                    map.Lines.Add(MapView.Circle(pa.CloudLat, pa.CloudLon, Prodromes.NearKm, Pal.A((byte)(isSel ? 255 : 150), Pal.Green), isSel ? 2.4 : 1.2));
                    if (isSel) map.Markers.Add(new MapMarker { Lat = pa.CloudLat, Lon = pa.CloudLon, Radius = 0, Label = Lang.F("prodromo: nube in {0}", pa.FromBand == "12 m" ? "24 MHz" : "28 MHz"), LabelColor = Pal.Green });
                    continue;
                }
                if (!Station.Valid) continue;
                Color c = a.Level == 2 ? Pal.Red : Pal.Yellow;
                map.Lines.Add(MapView.GreatCircle(Station.Lat, Station.Lon, a.TargetLat, a.TargetLon, Pal.A(isSel ? 255 : 170, c), isSel ? 2.6 : 1.4, true));
                map.Markers.Add(new MapMarker { Lat = a.TargetLat, Lon = a.TargetLon, Radius = 0, Fill = c, Stroke = c, Label = Lang.T(a.Region) + " " + a.Band, LabelColor = c });
                foreach (PrecursorPoint p in a.Points)
                {
                    Color pc = p.Covered ? Pal.Green : !p.Observed ? Pal.BarMuted : Pal.Red;
                    map.Markers.Add(new MapMarker { Lat = p.Lat, Lon = p.Lon, Radius = isSel ? 6 : 4, Fill = pc, Ring = !p.Covered, Label = isSel ? Geo.LatLonToLoc(p.Lat, p.Lon, 4) : null });
                    if (isSel && p.Covered && p.CloudSpot != null)
                        map.Lines.Add(MapView.GreatCircle(p.CloudSpot.LatS, p.CloudSpot.LonS, p.CloudSpot.LatD, p.CloudSpot.LonD, Pal.A(200, Pal.Green), 1.2));
                }
            }
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("tratteggio rosso / giallo: allerta / precursore del multi-salto"), Pal.Red));
            if (alerts.Any(a => a is ChainAlert))
                map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("catena a salti corti: linea piena, cerchio = zona d'estremità; azzurro = cerca la nube"), Pal.Azure));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("● verde: nube attiva sul punto di salto; ○ grigio: zona senza osservatori; ○ rosso: nessuna nube"), Pal.Green));
        }

        // catena a salti corti: dal QTH e corridoi (linea piena), CERCA LA NUBE (azzurro); i corridoi che non riguardano la
        // propria stazione restano tenui finche' non si sceglie la riga
        void DrawChain(ChainAlert c, bool isSel, bool only)
        {
            bool mine = c.Kind == ChainAlert.KindQth || c.Kind == ChainAlert.KindEqual || (!double.IsNaN(c.NearKm) && c.NearKm <= Chains.NearMeKm);
            Color col = c.Level == 2 ? Pal.Red : c.Level == 1 ? Pal.Yellow : Pal.Azure;
            byte alpha = (byte)(isSel ? 255 : mine ? 200 : 80);
            double w = isSel ? 2.8 : mine ? 1.8 : 1.0;
            if (c.Kind == ChainAlert.KindSearch)
            {
                double eLa, eLo;
                Geo.Destination(c.EndALat, c.EndALon, c.Az, c.Zones.Count > 0 ? Geo.DistanceKm(c.EndALat, c.EndALon, c.Zones[c.Zones.Count - 1].Lat, c.Zones[c.Zones.Count - 1].Lon) : c.DistKm, out eLa, out eLo);
                map.Lines.Add(MapView.GreatCircle(c.EndALat, c.EndALon, eLa, eLo, Pal.A(alpha, col), w, true));
                foreach (PrecursorPoint z in c.Zones)
                {
                    map.Lines.Add(MapView.Circle(z.Lat, z.Lon, (Chains.ShortMaxKm - Chains.ShortMinKm) / 4 + Chains.LineKm / 2, Pal.A(alpha, col), w));
                    if (isSel) map.Markers.Add(new MapMarker { Lat = z.Lat, Lon = z.Lon, Radius = 0, Label = Lang.F("salto corto finale? foEs ≥ {0:F1}", z.FoEsNeed), LabelColor = col });
                }
                return;
            }
            map.Lines.Add(MapView.GreatCircle(c.EndALat, c.EndALon, c.EndBLat, c.EndBLon, Pal.A(alpha, col), w));
            if (c.Kind == ChainAlert.KindCorridor || c.Kind == ChainAlert.KindEqualCorridor) map.Lines.Add(MapView.Circle(c.EndALat, c.EndALon, Chains.LineKm, Pal.A(alpha, col), w));
            map.Lines.Add(MapView.Circle(c.EndBLat, c.EndBLon, Chains.LineKm, Pal.A(alpha, col), w));
            if (!isSel && !mine) return;
            if (c.Kind == ChainAlert.KindCorridor || c.Kind == ChainAlert.KindEqualCorridor) map.Markers.Add(new MapMarker { Lat = c.EndALat, Lon = c.EndALon, Radius = 0, Label = c.EndAName, LabelColor = col });
            map.Markers.Add(new MapMarker { Lat = c.EndBLat, Lon = c.EndBLon, Radius = 0, Label = c.EndBName + " 6 m", LabelColor = col });
            if (!isSel) return;
            foreach (StrongCloud n in new[] { c.A, c.B }.Where(x => x != null))
                map.Markers.Add(new MapMarker { Lat = n.Lat, Lon = n.Lon, Radius = 8, Fill = Pal.Orange, Stroke = Pal.Fore, Square = true, Glow = true });
            foreach (PrecursorPoint p in c.Points)
            {
                Color pc = p.Covered ? Pal.Green : !p.Observed ? Pal.BarMuted : Pal.Red;
                map.Markers.Add(new MapMarker { Lat = p.Lat, Lon = p.Lon, Radius = 6, Fill = pc, Ring = !p.Covered, Label = Geo.LatLonToLoc(p.Lat, p.Lon, 4) });
            }
            if (c.A.Spot != null) map.Lines.Add(MapView.GreatCircle(c.A.Spot.LatS, c.A.Spot.LonS, c.A.Spot.LatD, c.A.Spot.LonD, Pal.A(200, Pal.Orange), 1.2));
            if (c.B != null && c.B.Spot != null) map.Lines.Add(MapView.GreatCircle(c.B.Spot.LatS, c.B.Spot.LonS, c.B.Spot.LatD, c.B.Spot.LonD, Pal.A(200, Pal.Orange), 1.2));
        }

        static string CloudText(StrongCloud n)
        {
            return Lang.F("{0}, foEs ≥ {1:F1} MHz, {2} ({3:F0} min fa)", Geo.LatLonToLoc(n.Lat, n.Lon, 4), n.FoEs, n.Source, double.IsNaN(n.AgeMin) ? 0 : n.AgeMin);
        }

        void ChainDetails(StringBuilder sb, ChainAlert c)
        {
            if (c.Kind == ChainAlert.KindEqual || c.Kind == ChainAlert.KindEqualCorridor)
            {
                PrecursorPoint f = c.Points.Count > 0 ? c.Points[0] : null;
                sb.AppendLine(Lang.F("{0} - doppio salto a salti uguali: {1}", c.LevelText, c.Region));
                sb.AppendLine(Lang.F("Distanza {0:F0} km, azimut {1:F0}°: due salti uguali da {2:F0} km (ITU-R P.534-6)", c.DistKm, c.Az, c.MiddleKm));
                sb.AppendLine(Lang.F("1) Primo salto, punto a un quarto della tratta: nube {0}", CloudText(c.A)));
                if (f != null)
                {
                    string st = f.Covered ? Lang.F("nube con foEs ≥ {0:F1} MHz a {1:F0} km", f.CloudFoEs, f.CloudKm) : f.Observed ? Lang.T("zona osservata, nessuna nube") : Lang.T("zona senza osservatori");
                    sb.AppendLine(Lang.F("2) Secondo salto, punto a tre quarti della tratta: {0}, foEs necessaria {1:F1} MHz - {2}", Geo.LatLonToLoc(f.Lat, f.Lon, 4), f.FoEsNeed, st));
                }
                if (c.Kind == ChainAlert.KindEqual) sb.AppendLine(Lang.F("Zona d'arrivo: {0}, entro circa {1:F0} km", c.EndBName, Chains.LineKm));
                else
                {
                    sb.AppendLine(Lang.F("Zone d'estremità: {0} e {1}, entro circa {2:F0} km", c.EndAName, c.EndBName, Chains.LineKm));
                    if (!double.IsNaN(c.NearKm)) sb.AppendLine(c.NearKm <= Chains.NearMeKm ? Lang.F("La tua stazione è a {0:F0} km da una zona d'estremità: ti riguarda.", c.NearKm) : Lang.F("La tua stazione è a {0:F0} km dalla zona d'estremità più vicina.", c.NearKm));
                }
                sb.AppendLine(Lang.T("Doppio salto a salti uguali (ITU-R P.534-6): per l'autore è il caso tipico fino a circa 4700 km, per esempio verso il Kazakhstan; oltre, verso gli altri continenti, valgono le catene con i salti corti alle estremità."));
                return;
            }
            sb.AppendLine(Lang.F("{0} - catena a salti corti ({1}): {2}", c.LevelText, c.KindText, c.Region));
            if (c.Kind == ChainAlert.KindSearch)
            {
                sb.AppendLine(Lang.F("Primo salto corto di {0:F0} km dalla tua stazione su una nube forte: {1}", c.ShortKm, CloudText(c.A)));
                sb.AppendLine(Lang.F("Nessuna seconda nube forte lungo l'azimut {0:F0}°: il salto corto finale dovrebbe stare in una di queste zone (foEs ≥ {1:F1} MHz):", c.Az, Chains.StrongFoEs(H)));
                int j = 1;
                foreach (PrecursorPoint z in c.Zones)
                {
                    sb.AppendLine("  " + (j == 1 ? Lang.F("dopo 1 salto centrale: {0}", Chains.ZoneName(z.Lat, z.Lon)) : Lang.F("dopo {0} salti centrali: {1}", j, Chains.ZoneName(z.Lat, z.Lon)))
                        + (z.Observed ? "" : ", " + Lang.T("zona senza osservatori"))
                        + (double.IsNaN(z.CloudKm) ? "" : ", " + (double.IsNaN(z.CloudFoEs) ? Lang.F("ionosonda GIRO a {0:F0} km senza foEs recente", z.CloudKm) : Lang.F("ionosonda GIRO a {0:F0} km: foEs {1:F1} MHz", z.CloudKm, z.CloudFoEs))));
                    j++;
                }
            }
            else
            {
                if (c.Middle == 1) sb.AppendLine(Lang.F("Distanza {0:F0} km, azimut {1:F0}°: salto corto {2:F0} km + 1 salto centrale da {3:F0} km + salto corto {4:F0} km", c.DistKm, c.Az, 2 * c.XA, c.MiddleKm, 2 * c.XB));
                else if (c.Middle > 1) sb.AppendLine(Lang.F("Distanza {0:F0} km, azimut {1:F0}°: salto corto {2:F0} km + {3} salti centrali da {4:F0} km + salto corto {5:F0} km", c.DistKm, c.Az, 2 * c.XA, c.Middle, c.MiddleKm, 2 * c.XB));
                else sb.AppendLine(Lang.F("Distanza {0:F0} km, azimut {1:F0}°: salto corto {2:F0} km + salto corto {3:F0} km", c.DistKm, c.Az, 2 * c.XA, 2 * c.XB));
                sb.AppendLine(Lang.F("1) Primo salto corto, MUF ≥ 50 MHz: nube forte {0}", CloudText(c.A)));
                sb.AppendLine(Lang.F("2) Ultimo salto corto, MUF ≥ 50 MHz: nube forte {0}", CloudText(c.B)));
                if (c.Points.Count == 0) sb.AppendLine(Lang.T("3) Salti centrali: nessuno, le due nubi sono consecutive."));
                int k = 1;
                foreach (PrecursorPoint p in c.Points)
                {
                    bool on = p.CloudKm < OnPointKm;   // la nube sta sul punto di salto: niente "a 0 km"
                    string st = p.Covered ? (p.CloudSpot != null ? (on ? Lang.F("nube con foEs ≥ {0:F1} MHz sul punto di salto (spot {1} - {2})", p.CloudFoEs, p.CloudSpot.Spotter, p.CloudSpot.DxCall) : Lang.F("nube con foEs ≥ {0:F1} MHz a {1:F0} km (spot {2} - {3})", p.CloudFoEs, p.CloudKm, p.CloudSpot.Spotter, p.CloudSpot.DxCall))
                                                                   : (on ? Lang.F("nube con foEs ≥ {0:F1} MHz sul punto di salto (ionosonda)", p.CloudFoEs) : Lang.F("nube con foEs ≥ {0:F1} MHz a {1:F0} km (ionosonda)", p.CloudFoEs, p.CloudKm)))
                              : p.Observed ? Lang.T("zona osservata, nessuna nube") : Lang.T("zona senza osservatori");
                    sb.AppendLine(Lang.F("3) Salto centrale {0} ({1:F0} km): {2}, foEs necessaria {3:F1} MHz - {4}", k++, p.HopKm, Geo.LatLonToLoc(p.Lat, p.Lon, 4), p.FoEsNeed, st));
                }
                if (c.Kind == ChainAlert.KindQth) sb.AppendLine(Lang.F("Zona d'arrivo: {0}, entro circa {1:F0} km", c.EndBName, Chains.LineKm));
                else
                {
                    sb.AppendLine(Lang.F("Zone d'estremità: {0} e {1}, entro circa {2:F0} km", c.EndAName, c.EndBName, Chains.LineKm));
                    if (!double.IsNaN(c.NearKm)) sb.AppendLine(c.NearKm <= Chains.NearMeKm ? Lang.F("La tua stazione è a {0:F0} km da una zona d'estremità: ti riguarda.", c.NearKm) : Lang.F("La tua stazione è a {0:F0} km dalla zona d'estremità più vicina.", c.NearKm));
                }
            }
            sb.AppendLine(Lang.T("Condizioni dell'autore (IC8TEM): primo e ultimo salto corto (500-1200 km) con MUF intorno ai 50 MHz, che chiedono una foEs di 11-21 MHz; i salti centrali fino a 2200 km chiedono circa 9,3 MHz. Nubi degli ultimi 30 minuti: a parità di livello vengono prima le più recenti. Le catene valgono in tutte le direzioni, anche nord-sud; il TEP resta escluso."));
        }

        const double OnPointKm = 10;   // sotto questa distanza la nube sta sul punto di salto

        void AlertDetails(StringBuilder sb, PrecursorAlert a)
        {
            ChainAlert ca = a as ChainAlert;
            if (ca != null) { ChainDetails(sb, ca); return; }
            AprsAlert aa = a as AprsAlert;
            if (aa != null)
            {
                sb.AppendLine(Lang.F("ALLERTA 144 MHz da APRS: {0} salti radio APRS in 2 m oltre i 900 km negli ultimi 30 minuti, punto medio su {1} ({2}), ultimo alle {3:HH:mm} UTC.", aa.N, aa.Region, Geo.LatLonToLoc(aa.MidLat, aa.MidLon, 4), aa.Last));
                sb.AppendLine(Lang.F("Stazioni: {0}", aa.Calls));
                if (!double.IsNaN(aa.NearKm)) sb.AppendLine(Lang.F("Il punto medio è a {0:F0} km dalla tua stazione (azimut {1:F0}°).", aa.NearKm, aa.Az));
                sb.AppendLine(Lang.T("Regola dell'autore: un pacchetto APRS da solo non dice nulla (può essere un aereo o un meteor scatter); conta la ricorrenza a breve tempo, almeno due salti con il punto medio entro 150 km e a non più di 10 minuti l'uno dall'altro. Lettore dei pacchetti come in LIVETROPOVHF (APRS-IS in sola lettura)."));
                return;
            }
            TepAlert tp = a as TepAlert;
            if (tp != null)
            {
                sb.AppendLine(Lang.F("{0} verso {1}: {2:F0} km, azimut {3:F0}°, {4} con {5:00}:{6:00} ora solare sull'equatore magnetico; indice {7:F0} su 100.",
                    tp.LevelText, tp.Region, tp.DistKm, tp.Az, tp.Kind, Math.Floor(tp.LocalHour), Math.Floor((tp.LocalHour % 1) * 60), tp.Index));
                sb.AppendLine(Lang.F("Fattori: stagione {0:F2} (massimo agli equinozi), SFI {1} → {2:F2}, Kp {3} → {4:F2} (solo eTEP), ora {5:F2}, foF2 delle ionosonde {6:F2}.",
                    tp.FSeason, double.IsNaN(tp.Sfi) ? "-" : tp.Sfi.ToString("F0", CultureInfo.CurrentCulture), tp.FSfi,
                    double.IsNaN(tp.Kp) ? "-" : tp.Kp.ToString("F1", CultureInfo.CurrentCulture), tp.FKp, tp.FTime, tp.FGiro));
                sb.AppendLine(double.IsNaN(tp.FoF2) ? Lang.T("Nessuna ionosonda GIRO recente entro 2500 km dal punto medio: il fattore foF2 vale 1.")
                                                    : Lang.F("foF2 più alta entro 2500 km dal punto medio: {0:F1} MHz ({1}).", tp.FoF2, tp.GiroNames));
                if (tp.Spots > 0) sb.AppendLine(Lang.F("Spot TEP nell'ultima ora vicino a te o alla zona: {0}.", tp.Spots));
                sb.AppendLine(Lang.T("aTEP (pomeridiano): 15-19 ora locale, fino a circa 60 MHz; eTEP (serale): 20-23, anche 144 MHz e oltre, soprattutto agli equinozi. Il minimo solare rende il TEP raro ma non lo cancella: l'SFI pesa, non azzera (decisione dell'autore). I pesi non sono ancora tarati sui dati."));
                return;
            }
            ProdromeAlert pa = a as ProdromeAlert;
            if (pa != null)
            {
                sb.AppendLine(Lang.F("PRODROMO: nube Es in {0} su {1} ({2}), foEs ≥ {3:F1} MHz, {4} spot negli ultimi 15 minuti; in 50 MHz nessuno spot Es entro 300 km negli ultimi 30 minuti.",
                    pa.FromBand == "12 m" ? "24 MHz" : "28 MHz", pa.Region, Geo.LatLonToLoc(pa.CloudLat, pa.CloudLon, 4), pa.FoEs, pa.N));
                if (pa.Spot != null) sb.AppendLine(Lang.F("Spot: {0} - {1} {2} MHz, {3:F0} km", pa.Spot.Spotter, pa.Spot.DxCall, pa.Spot.FreqMHz.ToString("F3", CultureInfo.CurrentCulture), pa.Spot.DistanceKm));
                if (!double.IsNaN(pa.NearKm)) sb.AppendLine(Lang.F("La nube è a {0:F0} km dalla tua stazione (azimut {1:F0}°).", pa.NearKm, pa.Az));
                sb.AppendLine(Lang.T("Verifica sulle decodifiche di tre stazioni italiane (94 giorni): dopo una nube in 28 MHz i 50 MHz hanno aperto entro 300 km nell'ora dopo nel 50% dei casi, contro il 30% delle zone senza nube in 28 MHz; anticipo mediano 9-16 minuti."));
                return;
            }
            sb.AppendLine(Lang.F("{0}: {1} in {2}, {3:F0} km, azimut {4:F0}°", a.LevelText, Lang.T(a.Region), a.Band, a.DistKm, a.Az));
            sb.AppendLine(a.ShortHyp ? Lang.F("Ipotesi: primo salto corto di {0:F0} km, poi salti uguali (tabella IK3XTV).", a.ShortKm)
                                     : Lang.F("Ipotesi: {0} salti uguali di {1:F0} km (tabella IK3XTV).", a.Points.Count, a.DistKm / a.Points.Count));
            sb.AppendLine(a.ShortFound ? Lang.F("Primo salto corto (entro 1200 km): c'è una nube attiva ({0:F0} km).", a.ShortKm)
                                       : Lang.T("Primo salto corto (entro 1200 km): nessuna nube attiva trovata."));
            int k = 1;
            foreach (PrecursorPoint p in a.Points)
            {
                string st = p.Covered ? (p.CloudKm < OnPointKm ? Lang.F("nube con foEs ≥ {0:F1} MHz sul punto di salto (spot {1} - {2} {3} MHz)", p.CloudFoEs, p.CloudSpot.Spotter, p.CloudSpot.DxCall, p.CloudSpot.FreqMHz.ToString("F3", CultureInfo.CurrentCulture))
                                                       : Lang.F("nube con foEs ≥ {0:F1} MHz a {1:F0} km (spot {2} - {3} {4} MHz)", p.CloudFoEs, p.CloudKm, p.CloudSpot.Spotter, p.CloudSpot.DxCall, p.CloudSpot.FreqMHz.ToString("F3", CultureInfo.CurrentCulture)))
                          : p.Observed ? Lang.T("zona osservata, nessuna nube") : Lang.T("zona senza osservatori");
                sb.AppendLine(Lang.F("  salto {0} ({1:F0} km): {2}, foEs necessaria {3:F1} MHz - {4}", k++, p.HopKm, Geo.LatLonToLoc(p.Lat, p.Lon, 4), p.FoEsNeed, st));
            }
            if (a.FreqMHz > 100) sb.AppendLine(Lang.T("144 MHz: è solo un possibile doppio salto; le due nubi devono restare attive insieme."));
            sb.AppendLine(Lang.T("Le allerte guardano solo le tratte trasversali (est-ovest), escludono il TEP e usano gli spot in singolo salto degli ultimi 30 minuti in tutto il mondo (EI7GL, Takata)."));
        }

        void Details()
        {
            StringBuilder sb = new StringBuilder();
            double h = H;
            if (selAlert != null && sel == null && double.IsNaN(clickLat)) AlertDetails(sb, selAlert);
            else if (sel != null)
            {
                Spot s = sel.Spot;
                sb.AppendLine(Lang.F("{0} {1} MHz  {2} ({3}) -> {4} ({5})", s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture) + "Z", s.FreqMHz.ToString("F3", CultureInfo.CurrentCulture), s.Spotter, s.LocSpotter, s.DxCall, s.LocDx));
                sb.AppendLine(Lang.F("Commento: {0}", s.Comment));
                sb.AppendLine(Lang.F("Distanza {0:F0} km - {1} (tabella IK3XTV)", sel.DistKm, Lang.T(sel.Judgement)));
                if (s.IsEsTep) sb.AppendLine(Lang.T("Tratta che attraversa l'equatore magnetico lungo il meridiano magnetico, più lunga o più asimmetrica di un TEP puro: l'Es può agganciare il TEP (ombra del TEP). Dallo spot non si può sapere: la tratta resta tra gli spot Es, in blu elettrico."));
                sb.AppendLine(Lang.F("Salti uguali: {0} x {1:F0} km, elevazione {2:F1}°, sec(φ) = {3:F2}, foEs ≥ {4:F1} MHz", sel.Hops, sel.HopKm, sel.ElevDeg, sel.SecPhi, sel.FoEsMin));
                for (int i = 0; i < sel.Points.Count; i++)
                {
                    EsHop p = sel.Points[i];
                    string who = sel.Hops == 1 ? Lang.T("punto medio") : i == 0 ? Lang.F("1° salto di {0}", s.Spotter) : i == sel.Points.Count - 1 ? Lang.F("1° salto di {0}", s.DxCall) : Lang.F("salto intermedio {0}", i + 1);
                    sb.AppendLine(Lang.F("  {0}: {1}  ({2:F1}°, {3:F1}°)", who, Geo.LatLonToLoc(p.Lat, p.Lon, 6), p.Lat, p.Lon));
                }
                if (sel.ShortA != null)
                {
                    sb.AppendLine(Lang.F("Primo salto corto ({0:F0} km, esperienza IC8TEM): foEs ≥ {1:F1} MHz", EsModel.ShortHopKm, sel.ShortFoEsMin));
                    sb.AppendLine(Lang.F("  {0}: {1}   {2}: {3}", s.Spotter, Geo.LatLonToLoc(sel.ShortA.Lat, sel.ShortA.Lon, 6), s.DxCall, Geo.LatLonToLoc(sel.ShortB.Lat, sel.ShortB.Lon, 6)));
                }
                sb.AppendLine(Lang.F("Azimut: {0} -> {1}: {2:F0}°   {1} -> {0}: {3:F0}°", s.Spotter, s.DxCall, Geo.Bearing(s.LatS, s.LonS, s.LatD, s.LonD), Geo.Bearing(s.LatD, s.LonD, s.LatS, s.LonS)));
            }
            else if (!double.IsNaN(clickLat))
            {
                sb.AppendLine(Lang.F("Punto {0} ({1:F2}°, {2:F2}°) come possibile nube Es a {3:F0} km di quota", Geo.LatLonToLoc(clickLat, clickLon, 6), clickLat, clickLon, h));
                if (Station.Valid)
                {
                    double d = Geo.DistanceKm(Station.Lat, Station.Lon, clickLat, clickLon);
                    double el = Geo.ElevationDeg(d, h, Station.AltKm, EsModel.EsK);
                    double sec = Geo.SecantFactor(2 * d, h);
                    sb.AppendLine(Lang.F("Dalla mia stazione: azimut {0:F0}°, distanza {1:F0} km, elevazione {2:F1}°", Geo.Bearing(Station.Lat, Station.Lon, clickLat, clickLon), d, el));
                    sb.AppendLine(Lang.F("Salto di {0:F0} km ({1}); sec(φ) = {2:F2}", 2 * d, Lang.T(EsModel.Judge(2 * d)), sec));
                    sb.AppendLine(Lang.F("foEs necessaria: 28 MHz {0:F1}   50 MHz {1:F1}   70 MHz {2:F1}   144 MHz {3:F1} MHz", 28 / sec, 50.2 / sec, 70.2 / sec, 144.3 / sec));
                    if (el < 0) sb.AppendLine(Lang.T("Il punto è sotto l'orizzonte della stazione a questa quota."));
                }
                string sq = Geo.LatLonToLoc(clickLat, clickLon, 4);
                foreach (Cloud c in clouds) if (c.Sq == sq) sb.AppendLine(Lang.F("Nube stimata nel quadrato {0}: foEs ≥ {1:F1} MHz da {2} spot ({3})", c.Sq, c.FoEs, c.N, string.Join(", ", c.Calls)));
            }
            else
            {
                sb.AppendLine(Lang.T("Clic su uno spot della tabella, su una nube o su un punto della mappa."));
                sb.AppendLine(Lang.T("Gli spot arrivano dalle sessioni telnet della linguetta DATA COLLECTION; i locator mancanti si cercano in CALL3."));
            }
            foreach (EsPath u in ultra)
                sb.AppendLine(Lang.F("ALLERTA (solo notizia statistica): {0} - {1} in 50 MHz a {2:F0} km, oltre gli 11000 km; il QSO di riferimento è HZ1SK-K4MOG, 11460 km con almeno sei salti (EI7GL [56]).", u.Spot.Spotter, u.Spot.DxCall, u.DistKm));
            sb.AppendLine();
            sb.AppendLine(Lang.T("Tabella IK3XTV: singolo salto 500-650 / 1500-2000 / 2200-2400 km (minima / ottimale / massima); doppio 2800-3000 / 3200-4000 / 4400-5000 km; triplo ottimale 4800-6400, massima 6900 km."));
            sb.AppendLine(Lang.T("Multi-salto: l'autore (IC8TEM) ha esperienza di primo salto corto, entro i 1200 km dal proprio QTH."));
            sb.AppendLine(Lang.T("Regola dell'autore (IC8TEM) per i 144 MHz: distanza minima dell'E-sporadico sopra i 900 km; uno spot da solo non si disegna; si disegna se entro 5 minuti arriva un secondo spot con la nube nello stesso posto (punti di riflessione entro 300 km) o se nei dintorni c'è uno spot in 50 MHz di salto corto (entro 1200 km, nei 15 minuti)."));
            sb.AppendLine(Lang.T("L'E-sporadico non si applica ai 432 MHz: non esistono collegamenti Es documentati in 70 cm (record ARRL: Es solo su 50 e 144 MHz; MUF dell'Es fino a circa 150-200 MHz). Gli spot in 432 MHz non vengono analizzati qui."));
            txtDet.Text = sb.ToString();
        }

        // spot di prova per le fotografie automatiche (--screenshot): non vengono salvati
        public static void AddDemoSpots()
        {
            DateTime now = DateTime.UtcNow;
            string[][] demo =
            {
                new[] { "50313.0", "EA8DO", "IC8TEM", "IL18", "JN70CN", "ES" },
                new[] { "50313.0", "PY2XB", "IK0ABC", "GG66", "JN61", "ES" },
                new[] { "144174.0", "EA5DF", "IK8XYZ", "IM98", "JN70", "ES" },
                new[] { "50313.0", "W1JJ", "G4ABC", "FN42", "IO91", "ES" },
                new[] { "70154.0", "SV1DH", "DL1ABC", "KM18", "JO50", "ES" },
                new[] { "50313.0", "5B4AHJ", "F5XYZ", "KM64", "JN18", "ES" },
                new[] { "144174.0", "EA5XX", "IW0ABC", "IM99", "JN61", "ES" },
                new[] { "50313.0", "EA6SX", "IK7ABC", "JM19", "JN80", "ES" },
                new[] { "50313.0", "ZS6ABC", "OH2ABC", "KG44", "KP20", "ES" },
                new[] { "144300.0", "LA9ABC", "G4XYZ", "JO59", "IO70", "AU" },
                new[] { "144050.0", "OZ1ABC", "GM4XYZ", "JO65", "IO85", "AUR" }
            };
            foreach (string[] d in demo)
            {
                Spot s = new Spot
                {
                    Utc = now.AddMinutes(-5), FreqKHz = double.Parse(d[0], CultureInfo.InvariantCulture), DxCall = d[1], Spotter = d[2],
                    LocDx = d[3], LocSpotter = d[4], Prop = d[5], Source = 9, Comment = d[4] + "<" + d[5] + ">" + d[3] + " PROVA"
                };
                double la, lo;
                Geo.LocToLatLon(s.LocSpotter, out la, out lo); s.LatS = la; s.LonS = lo;
                Geo.LocToLatLon(s.LocDx, out la, out lo); s.LatD = la; s.LonD = lo;
                SpotStore.Add(s);
            }
            // catene a salti corti (dal 20/11/2026): primo salto corto verso l'Asia centrale, salto centrale coperto da uno spot in
            // 70 MHz, ultimo salto corto in Kazakhstan; corridoio Caraibi - Penisola Iberica con i salti centrali sull'Atlantico
            if (Station.Valid)
            {
                double az = Geo.Bearing(Station.Lat, Station.Lon, 43, 70);
                Action<double, double, double, double, string, string, string> add = (la, lo, f, half, c1, c2, tag) =>
                {
                    double l1, o1, l2, o2;
                    Geo.Destination(la, lo, 270, half, out l1, out o1);
                    Geo.Destination(la, lo, 90, half, out l2, out o2);
                    Spot s = new Spot
                    {
                        Utc = now.AddMinutes(-6), FreqKHz = f, DxCall = c2, Spotter = c1, LocSpotter = Geo.LatLonToLoc(l1, o1, 4), LocDx = Geo.LatLonToLoc(l2, o2, 4),
                        Prop = "ES", Source = 9, LatS = l1, LonS = o1, LatD = l2, LonD = o2
                    };
                    s.Comment = s.LocSpotter + "<ES>" + s.LocDx + " " + tag;
                    SpotStore.Add(s);
                };
                double la0, lo0;
                Geo.Destination(Station.Lat, Station.Lon, az, 450, out la0, out lo0); add(la0, lo0, 50313.0, 450, "YU1ABC", "LZ1ABC", "PROVA");
                Geo.Destination(Station.Lat, Station.Lon, az, 1800, out la0, out lo0); add(la0, lo0, 70154.0, 900, "UR5XYZ", "UA6ABC", "PROVA");
                Geo.Destination(Station.Lat, Station.Lon, az, 3300, out la0, out lo0); add(la0, lo0, 50313.0, 450, "UN7ABC", "UN8XYZ", "PROVA");
                add(18.5, -64, 50313.0, 400, "KP4ABC", "FG8ABC", "PROVA");
                add(37.5, -6.5, 50313.0, 450, "CT1ABC", "EA7ABC", "PROVA");
            }
            // nubi attive lungo la tratta verso il Giappone (precursori del multi-salto in 50 MHz): uno spot est-ovest
            // di 1400 km centrato su ogni punto dei salti uguali
            if (!Station.Valid) return;
            double D = Geo.DistanceKm(Station.Lat, Station.Lon, 36, 138);
            int n = (int)Math.Ceiling(D / Precursors.MaxHopKm);
            string[][] calls = { new[] { "UR5ABC", "4K6ABC" }, new[] { "UN7ABC", "RA9ABC" }, new[] { "UA0ABC", "BY4ABC" }, new[] { "JA7ABC", "UA0LAB" }, new[] { "JA1ABC", "BV2ABC" } };
            for (int k = 0; k < n && k < calls.Length; k++)
            {
                double la, lo, la1, lo1, la2, lo2;
                Geo.Intermediate(Station.Lat, Station.Lon, 36, 138, (k + 0.5) / n, out la, out lo);
                Geo.Destination(la, lo, 270, 700, out la1, out lo1);
                Geo.Destination(la, lo, 90, 700, out la2, out lo2);
                Spot s = new Spot
                {
                    Utc = now.AddMinutes(-4), FreqKHz = 50313.0, DxCall = calls[k][1], Spotter = calls[k][0],
                    LocSpotter = Geo.LatLonToLoc(la1, lo1, 4), LocDx = Geo.LatLonToLoc(la2, lo2, 4), Prop = "ES", Source = 9,
                    LatS = la1, LonS = lo1, LatD = la2, LonD = lo2
                };
                s.Comment = s.LocSpotter + "<ES>" + s.LocDx + " PROVA";
                SpotStore.Add(s);
            }
        }

        public string DetailsText { get { return txtDet.Text ?? ""; } }

        public void ShotChain(int kind)
        {
            Refresh();
            inner.Select(2);
            PrecursorAlert a = alerts.Where(x => x is ChainAlert && ((ChainAlert)x).Kind == kind).OrderByDescending(x => x.Level).ThenByDescending(x => x.DistKm).FirstOrDefault();
            if (a != null) { selAlert = a; selAlertKey = a.Key; sel = null; selKey = null; clickLat = clickLon = double.NaN; }
            cmbMap.SelectedIndex = 1; MapMode();
            if (kind == ChainAlert.KindCorridor) map.SetView(-95, 30, 5, 60); else map.SetView(-10, 95, 20, 65);
            FillTables(); Draw(); Details();
        }

        public void ShotPrecursors()
        {
            Refresh();
            inner.Select(2);
            PrecursorAlert a = alerts.Find(x => x.Region == "Giappone") ?? (alerts.Count > 0 ? alerts[0] : null);
            if (a != null) { selAlert = a; selAlertKey = a.Key; sel = null; selKey = null; clickLat = clickLon = double.NaN; }
            cmbMap.SelectedIndex = 1; MapMode(); map.SetView(-20, 160, -15, 75);
            FillTables(); Draw(); Details();
        }

        // mode: 0 = azimutale (km = raggio), 1 = Mercatore sull'Europa
        public void ShotMap(int mode, double km)
        {
            if (km > 0) multiRange = km;
            cmbMap.SelectedIndex = mode;
            MapMode(); Draw();
            if (mode == 1) map.SetView(-30, 60, 20, 72);
        }

        public void SelectFirst() { if (paths.Count > 0) { sel = paths[0]; selKey = sel.Spot.Key; FocusOn(sel); FillTables(); Draw(); Details(); } }
    }
}
