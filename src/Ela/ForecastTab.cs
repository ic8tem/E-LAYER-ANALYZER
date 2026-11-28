// E-LAYER ANALYZER - sottolinguetta PREVISIONE di SPORADIC-E (dalla 0.7.0, proposta approvata dall'autore il 28/11/2026).
// Riconosce le nubi Es in formazione (spot degli ultimi 30 minuti e ionosonde GIRO) e, dal QTH delle impostazioni, verifica in
// ordine: primo salto utile, doppio salto Es, tropo + Es (stazione virtuale raggiunta in condotto, modulo tropo comune preso
// da LIVETROPOVHF). 50 e 144 MHz separati. Mappa 65%, dati 35%. Allerta solo nel programma (niente notifiche sul telefono).
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
    public class ForecastTab
    {
        public readonly Control Root;
        readonly MapView map = new MapView { Mercator = true };
        readonly TableView tHyp, tCl;
        readonly SelectableTextBlock txtDet;
        readonly TextBlock lblStatus, lblAlert;
        readonly ComboBox cmbBand;
        readonly CheckBox chkMeteo, chkTrEs;
        readonly NumericUpDown numH;
        List<EsCloud> clouds = new List<EsCloud>();
        List<EsHypothesis> hyps = new List<EsHypothesis>();
        EsHypothesis sel;
        int waiting;
        bool dirty = true;

        public ForecastTab()
        {
            Config cf = Station.Cfg;
            cmbBand = Ui.Combo(new[] { "50 MHz", "144 MHz" }, Math.Max(0, Math.Min(1, (int)cf.GetD("prev_banda", 1))), 100);
            chkMeteo = Ui.C(Lang.T("Profili verticali da Open-Meteo (condotti previsti)"), cf.GetB("prev_open_meteo", true));
            chkTrEs = Ui.C(Lang.T("TR+ES anche in REAL-TIME"), cf.GetB("prev_tres_realtime", true));
            numH = Ui.N(90, 130, Ui.Dm(cf.GetD("prev_quota_km", EsModel.DefaultH)), 0, 1, 45);
            TropoDuct.Enabled = chkMeteo.IsChecked == true;
            EsForecast.UseTrEs = chkTrEs.IsChecked == true;
            Button bNow = Ui.B(Lang.T("Aggiorna adesso"), true);
            bNow.Click += (s, e) => { _ = TropoDuct.FetchPendingAsync(); Refresh(); };
            cmbBand.SelectionChanged += (s, e) => { cf.Set("prev_banda", Math.Max(0, cmbBand.SelectedIndex)); sel = null; Refresh(); };
            chkMeteo.IsCheckedChanged += (s, e) => { cf.Set("prev_open_meteo", chkMeteo.IsChecked == true); TropoDuct.Enabled = chkMeteo.IsChecked == true; Refresh(); };
            chkTrEs.IsCheckedChanged += (s, e) => { cf.Set("prev_tres_realtime", chkTrEs.IsChecked == true); EsForecast.UseTrEs = chkTrEs.IsChecked == true; };
            numH.ValueChanged += (s, e) => { cf.Set("prev_quota_km", Ui.V(numH)); Refresh(); };
            lblStatus = new TextBlock { Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2) };
            lblAlert = new TextBlock { Foreground = Pal.B(Pal.Orange), FontWeight = FontWeight.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4) };

            tHyp = new TableView();
            tHyp.Columns.AddRange(new[] { Lang.T("Ipotesi"), Lang.T("Nube"), "foEs", Lang.T("Salto km"), "MUF", Lang.T("Arrivo"), "dB", "%" });
            tHyp.ColumnWidths.AddRange(new double[] { 118, 58, 44, 74, 50, 64, 44, 38 });
            tHyp.HeaderTips = new[] { Lang.T("Priorità: primo salto utile, poi doppio salto Es, poi tropo + Es"), Lang.T("Quadrato del centro della nube"), Lang.T("foEs della nube (MHz), dagli spot o dalla ionosonda"),
                Lang.T("Lunghezza dei salti Es (km)"), Lang.T("MUF del salto (MHz), legge della secante"), Lang.T("Dove arriva il segnale (quadrato)"), Lang.T("Abbattimento stimato della tratta (dB)"), Lang.T("Punteggio dell'ipotesi") };
            tHyp.RowClicked += i => { if (i >= 0 && i < tHyp.Rows.Count) { sel = tHyp.Rows[i].Tag as EsHypothesis; Draw(); Details(); } };
            tCl = new TableView();
            tCl.Columns.AddRange(new[] { Lang.T("Nube"), "foEs", Lang.T("Tendenza"), Lang.T("Spot"), Lang.T("Bande"), "km" });
            tCl.ColumnWidths.AddRange(new double[] { 58, 48, 96, 44, 110, 52 });
            txtDet = Ui.Text(12.5);
            ScrollViewer svDet = new ScrollViewer { Content = txtDet, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            TextBlock credit = new TextBlock
            {
                Text = Lang.T("Profili verticali: Open-Meteo (open-meteo.com, CC BY 4.0), modello GFS di NOAA; calcolo dei condotti di LIVETROPOVHF (ITU-R P.453, Kerr, ITU-R P.834). Abbattimento del salto Es: ITU-R P.534-6."),
                Foreground = Pal.B(Pal.Muted), FontSize = 11, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2)
            };

            Grid right = new Grid { RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,1.3*,5,*,5,1.4*,Auto"), Background = Pal.B(Pal.Back) };
            Control r0 = Ui.Row(Ui.L(Lang.T("Banda")), cmbBand, Ui.L(Lang.T("Quota Es km")), numH, bNow);
            Control r1 = Ui.Row(chkMeteo, chkTrEs);
            Border hypBox = Ui.Box(Lang.T("Ipotesi dalla mia stazione"), Pal.Orange, tHyp);
            Border clBox = Ui.Box(Lang.T("Nubi Es degli ultimi 30 minuti"), Pal.Yellow, tCl);
            Border detBox = Ui.Box(Lang.T("Dettagli dell'ipotesi"), Pal.Azure, svDet);
            GridSplitter sp1 = new GridSplitter { Background = Pal.B(Pal.Track), ResizeDirection = GridResizeDirection.Rows };
            GridSplitter sp2 = new GridSplitter { Background = Pal.B(Pal.Track), ResizeDirection = GridResizeDirection.Rows };
            Grid.SetRow(r0, 0); Grid.SetRow(r1, 1); Grid.SetRow(lblAlert, 2); Grid.SetRow(lblStatus, 3);
            Grid.SetRow(hypBox, 4); Grid.SetRow(sp1, 5); Grid.SetRow(clBox, 6); Grid.SetRow(sp2, 7); Grid.SetRow(detBox, 8); Grid.SetRow(credit, 9);
            foreach (Control c in new Control[] { r0, r1, lblAlert, lblStatus, hypBox, sp1, clBox, sp2, detBox, credit }) right.Children.Add(c);

            Continents.Apply(map);
            Continents.Changed += () => { Continents.Apply(map); map.InvalidateVisual(); };
            map.Clicked += (la, lo, rb) =>
            {
                EsHypothesis best = null; double bd = 400;
                foreach (EsHypothesis h in hyps)
                {
                    double d = Math.Min(Geo.DistanceKm(la, lo, h.Cloud.Lat, h.Cloud.Lon), Geo.DistanceKm(la, lo, h.LandLat, h.LandLon));
                    if (d < bd) { bd = d; best = h; }
                }
                if (best != null) { sel = best; FillTables(); Draw(); Details(); }
            };
            SpotStore.Added += s => dirty = true;
            TropoDuct.Updated += () => Dispatcher.UIThread.Post(() => { dirty = true; });
            Giro.Updated += () => dirty = true;
            Station.Changed += () => dirty = true;
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
            int ticks = 0;
            timer.Tick += (s, e) =>
            {
                ticks++;
                // profili in coda (anche quelli chiesti da REAL-TIME): una richiesta al minuto al massimo
                if (ticks % 2 == 0 && TropoDuct.Pending > 0) _ = TropoDuct.FetchPendingAsync();
                if (dirty && Root.IsVisible) Refresh();
            };
            timer.Start();
            Root = Ui.MapAndData(map, right);
        }

        double F { get { return EsForecast.BandsMHz[Math.Max(0, Math.Min(1, cmbBand.SelectedIndex))]; } }
        string BandLabel { get { return cmbBand.SelectedIndex == 0 ? "50 MHz" : "144 MHz"; } }

        public void OnShow() { Refresh(); if (TropoDuct.Pending > 0) _ = TropoDuct.FetchPendingAsync(); }

        public void Refresh()
        {
            dirty = false;
            double H = Ui.V(numH);
            clouds = EsForecast.Clouds(H, DateTime.UtcNow, SpotStore.Recent(EsForecast.WindowMin, x => x.HasBoth), true);
            List<Spot> trs = TropoDuct.TropoSpots(60);
            hyps = Station.Valid ? EsForecast.Hypotheses(Station.Lat, Station.Lon, F, H, clouds, trs, out waiting) : new List<EsHypothesis>();
            if (sel != null && !hyps.Contains(sel)) sel = hyps.FirstOrDefault(x => x.Kind == sel.Kind && x.Cloud.Loc == sel.Cloud.Loc);
            if (sel == null) sel = hyps.FirstOrDefault();
            // allerta: la prima ipotesi (priorita' piu' alta) con una nube in formazione o stabile
            EsHypothesis a = hyps.FirstOrDefault(x => x.Cloud.Trend >= 0);
            if (!Station.Valid) lblAlert.Text = Lang.T("Imposta il locator della stazione (IMPOSTAZIONI o barra in alto) per le ipotesi dalla tua stazione.");
            else if (a == null) lblAlert.Text = clouds.Count == 0 ? Lang.F("Nessuna nube Es negli ultimi {0:F0} minuti.", EsForecast.WindowMin) : Lang.F("{0}: nessuna ipotesi utile dalla tua stazione con le nubi attuali.", BandLabel);
            else
            {
                string tr = a.Cloud.Trend > 0 ? Lang.T("in formazione") : Lang.T("stabile");
                string head = Lang.F("Nube Es {0} su {1} (foEs {2:F1} MHz)", tr, a.Cloud.Loc, a.Cloud.BestFoEs);
                string what = a.Kind == HypKind.Single ? Lang.F("primo salto utile in {0} verso {1}", BandLabel, Geo.LatLonToLoc(a.LandLat, a.LandLon, 4))
                            : a.Kind == HypKind.Double ? Lang.F("possibile doppio salto in {0} verso {1}", BandLabel, Geo.LatLonToLoc(a.Land2Lat, a.Land2Lon, 4))
                            : Lang.F("possibile tropo + Es in {0} verso {1}", BandLabel, Geo.LatLonToLoc(a.LandLat, a.LandLon, 4));
                lblAlert.Text = "⚠ " + head + " – " + what;
            }
            lblStatus.Text = Lang.F("Nubi: {0}, di cui {1} in formazione. Ipotesi in {2}: {3}.", clouds.Count, clouds.Count(c => c.Trend > 0), BandLabel, hyps.Count)
                + (waiting > 0 && TropoDuct.Enabled ? " " + Lang.F("Profili verticali in attesa: {0}.", TropoDuct.Pending) : "")
                + (TropoDuct.Status.Length > 0 ? " " + TropoDuct.Status : "");
            FillTables(); Draw(); Details();
        }

        static string F1(double v) { return double.IsNaN(v) ? "-" : v.ToString("F1", CultureInfo.CurrentCulture); }
        static Color Opaque(Color c) { return Color.FromRgb(c.R, c.G, c.B); }
        static string TrendText(EsCloud c) { return c.Trend > 0 ? "▲ " + Lang.T("in formazione") : c.Trend < 0 ? "▼ " + Lang.T("in calo") : "● " + Lang.T("stabile"); }

        void FillTables()
        {
            tHyp.Rows.Clear();
            int si = -1;
            foreach (EsHypothesis h in hyps)
            {
                string hop = h.Kind == HypKind.Double ? h.HopKm.ToString("F0", CultureInfo.InvariantCulture) + "+" + h.Hop2Km.ToString("F0", CultureInfo.InvariantCulture)
                           : h.Kind == HypKind.TropoEs ? h.TropoKm.ToString("F0", CultureInfo.InvariantCulture) + "T+" + h.HopKm.ToString("F0", CultureInfo.InvariantCulture) : h.HopKm.ToString("F0", CultureInfo.InvariantCulture);
                double la = h.Kind == HypKind.Double ? h.Land2Lat : h.LandLat, lo = h.Kind == HypKind.Double ? h.Land2Lon : h.LandLon;
                tHyp.Rows.Add(new TableRow
                {
                    Cells = new[] { EsForecast.KindName(h.Kind) + (h.TropoAtFarEnd ? " +T" : ""), h.Cloud.Loc, F1(h.Cloud.BestFoEs), hop, F1(h.Kind == HypKind.Double ? Math.Min(h.MufMHz, h.Muf2MHz) : h.MufMHz),
                        Geo.LatLonToLoc(la, lo, 4), h.LossDb.ToString("F0", CultureInfo.InvariantCulture), (100 * h.Score).ToString("F0", CultureInfo.InvariantCulture) },
                    Fore = h.Kind == HypKind.Single ? Pal.Green : h.Kind == HypKind.Double ? Pal.Violet : Pal.Azure,
                    Tag = h
                });
                if (h == sel) si = tHyp.Rows.Count - 1;
            }
            tHyp.SelectedIndex = si; tHyp.Refresh();
            tCl.Rows.Clear();
            foreach (EsCloud c in clouds.OrderByDescending(x => x.BestFoEs))
            {
                tCl.Rows.Add(new TableRow
                {
                    Cells = new[] { c.Loc, F1(c.BestFoEs), TrendText(c), c.Spots.ToString(CultureInfo.InvariantCulture), string.Join(" ", c.Bands.OrderBy(b => b)) + (!double.IsNaN(c.GiroFoEs) ? " GIRO" : ""),
                        Station.Valid ? Geo.DistanceKm(Station.Lat, Station.Lon, c.Lat, c.Lon).ToString("F0", CultureInfo.InvariantCulture) : "-" },
                    CellFore = new Color?[] { null, Opaque(EsTab.FoEsColor(c.BestFoEs)), c.Trend > 0 ? Pal.Orange : c.Trend < 0 ? Pal.Muted : Pal.Green, null, null, null },
                    Tag = c
                });
            }
            tCl.Refresh();
        }

        void Draw()
        {
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear();
            map.Caption = Lang.F("PREVISIONE Es - {0}: nubi in formazione e ipotesi dalla mia stazione", BandLabel);
            map.LegendTitle = Lang.T("foEs della nube");
            foreach (EsCloud c in clouds)
            {
                Color fill = Opaque(EsTab.FoEsColor(c.BestFoEs));
                map.Lines.Add(MapView.Circle(c.Lat, c.Lon, c.RadiusKm, Pal.A(150, fill), c.Trend > 0 ? 2 : 1));
                map.Markers.Add(new MapMarker { Lat = c.Lat, Lon = c.Lon, Radius = 5 + Math.Min(4, c.Spots / 2.0), Fill = fill, Square = true, Glow = c.Trend > 0,
                    Label = (c.Trend > 0 ? "▲" : c.Trend < 0 ? "▼" : "") + c.BestFoEs.ToString("F1", CultureInfo.CurrentCulture), LabelColor = Pal.Fore });
            }
            foreach (EsHypothesis h in hyps)
            {
                bool isSel = h == sel;
                double w = isSel ? 3 : 1.3;
                byte al = (byte)(isSel ? 255 : 130);
                if (h.Kind == HypKind.TropoEs)
                {
                    map.Lines.Add(MapView.GreatCircle(Station.Lat, Station.Lon, h.VLat, h.VLon, Pal.A(al, Pal.Azure), w, true));
                    map.Lines.Add(MapView.GreatCircle(h.VLat, h.VLon, h.LandLat, h.LandLon, Pal.A(al, Pal.Orange), w));
                    if (isSel) map.Markers.Add(new MapMarker { Lat = h.VLat, Lon = h.VLon, Radius = 4, Fill = Pal.Azure, Label = "A′ " + Geo.LatLonToLoc(h.VLat, h.VLon, 4), LabelColor = Pal.Azure });
                }
                else map.Lines.Add(MapView.GreatCircle(Station.Lat, Station.Lon, h.LandLat, h.LandLon, Pal.A(al, h.Kind == HypKind.Single ? Pal.Green : Pal.Violet), w));
                if (h.Kind == HypKind.Double) map.Lines.Add(MapView.GreatCircle(h.LandLat, h.LandLon, h.Land2Lat, h.Land2Lon, Pal.A(al, Pal.Violet), w));
                if (h.TropoAtFarEnd) map.Lines.Add(MapView.GreatCircle(h.LandLat, h.LandLon, h.ExtLat, h.ExtLon, Pal.A(al, Pal.Azure), w, true));
                double la = h.Kind == HypKind.Double ? h.Land2Lat : h.LandLat, lo = h.Kind == HypKind.Double ? h.Land2Lon : h.LandLon;
                bool land = h.Kind == HypKind.Double ? h.Land2OnLand : h.LandOnLand;
                map.Markers.Add(new MapMarker { Lat = la, Lon = lo, Radius = isSel ? 5 : 3, Fill = land ? Pal.Yellow : Pal.Muted, Ring = !land, Label = isSel ? Geo.LatLonToLoc(la, lo, 4) : null, LabelColor = Pal.Yellow });
            }
            if (Station.Valid)
                map.Markers.Add(new MapMarker { Lat = Station.Lat, Lon = Station.Lon, Radius = 5, Fill = Pal.Green, Square = true, Label = Station.Call.Length > 0 ? Station.Call : Station.Loc, LabelColor = Pal.Green });
            foreach (double f in new[] { 4.0, 6.0, 8.5, 12.0, 17.0, 25.0 })
                map.Legend.Add(new KeyValuePair<string, Color>(f < 5 ? "< 5 MHz" : f < 7 ? "5-7 MHz" : f < 10 ? "7-10 MHz" : f < 14 ? "10-14 MHz" : f < 20 ? "14-20 MHz" : "≥ 20 MHz", Opaque(EsTab.FoEsColor(f))));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("▲ nube in formazione, ▼ in calo"), Pal.Orange));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("verde: primo salto utile; viola: doppio salto Es"), Pal.Violet));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("tratteggio azzurro: tratta in condotto (tropo + Es)"), Pal.Azure));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("giallo: arrivo sulla terra; anello grigio: arrivo in mare"), Pal.Yellow));
            map.InvalidateVisual();
        }

        void Details()
        {
            StringBuilder sb = new StringBuilder();
            EsHypothesis h = sel;
            if (h == null)
            {
                sb.AppendLine(Lang.T("Nessuna ipotesi. La linguetta cerca, dalla tua stazione e nell'ordine: primo salto utile, doppio salto Es, tropo + Es (solo se il primo salto non c'è)."));
                sb.AppendLine(Lang.T("Una nube è «in formazione» quando la foEs degli ultimi 10 minuti supera quella dei 20 minuti prima, o quando è nata negli ultimi 10 minuti con almeno due spot."));
                txtDet.Text = sb.ToString(); return;
            }
            EsCloud c = h.Cloud;
            sb.AppendLine(EsForecast.KindName(h.Kind).ToUpperInvariant() + " – " + BandLabel);
            sb.AppendLine(Lang.F("Nube {0}: foEs {1:F1} MHz, {2}, {3} spot", c.Loc, c.BestFoEs, TrendText(c), c.Spots) + (!double.IsNaN(c.GiroFoEs) ? ", " + Lang.F("ionosonda {0} {1:F1} MHz", c.GiroName, c.GiroFoEs) : "") + ".");
            sb.AppendLine(Lang.F("Quando: {0}.", h.Window));
            if (h.Kind == HypKind.TropoEs)
            {
                sb.AppendLine(Lang.F("Tratta in condotto dalla stazione ad A′ {0}: {1:F0} km ({2}).", Geo.LatLonToLoc(h.VLat, h.VLon, 6), h.TropoKm, h.DuctSource));
                sb.AppendLine(Lang.F("Il primo salto diretto non c'è: la nube è fuori dalla geometria del salto dalla tua stazione, ma è a metà di un salto di {0:F0} km da A′.", h.HopKm));
            }
            sb.AppendLine(Lang.F("Salto Es di {0:F0} km, azimut {1:F0}°, elevazione {2:F2}°, MUF {3:F1} MHz.", h.HopKm, h.Az, h.ElevDeg, h.MufMHz));
            if (h.Kind == HypKind.Double)
                sb.AppendLine(Lang.F("Secondo salto di {0:F0} km sulla nube {1} (foEs {2:F1} MHz, MUF {3:F1} MHz), arrivo a {4}{5}.", h.Hop2Km, h.Cloud2.Loc, h.Cloud2.BestFoEs, h.Muf2MHz,
                    Geo.LatLonToLoc(h.Land2Lat, h.Land2Lon, 4), h.Land2OnLand ? "" : " (" + Lang.T("in mare") + ")"));
            sb.AppendLine(Lang.F("Arrivo del primo salto a {0}{1}.", Geo.LatLonToLoc(h.LandLat, h.LandLon, 4), h.LandOnLand ? "" : " (" + Lang.T("in mare") + ")"));
            if (h.Kind == HypKind.TropoEs || h.TropoAtFarEnd)
            {
                sb.AppendLine(Lang.F("Accoppiamento nel condotto: elevazione {0:F2}° contro angolo critico {1:F2}° (ITU-R P.834), perdita di accoppiamento {2:F1} dB; condotto {3}.",
                    h.ElevDeg, h.CriticalDeg, h.CouplingDb, h.Soft >= 1 ? Lang.T("che intrappola la banda") : Lang.T("«morbido» (Kerr)")));
                if (h.TropoAtFarEnd) sb.AppendLine(Lang.F("Dopo l'arrivo il segnale può continuare in condotto per {0:F0} km fino a {1} ({2}).", h.ExtKm, Geo.LatLonToLoc(h.ExtLat, h.ExtLon, 4), h.DuctSource));
            }
            sb.AppendLine(Lang.F("Abbattimento stimato: {0:F0} dB (ITU-R P.534-6 per il salto Es{1}).", h.LossDb, h.Kind == HypKind.TropoEs ? ", " + Lang.T("più la tratta in condotto e l'accoppiamento") : ""));
            if (h.FMHz > 100) sb.AppendLine(Lang.T("In 144 MHz le aperture Es sono rare (IK3XTV): serve una nube iperdensa."));
            sb.AppendLine();
            sb.AppendLine(Lang.T("Priorità: il doppio salto Es viene prima del tropo + Es (fonte [88]); il tropo + Es si cerca solo se il primo salto utile non c'è. Valori di accoppiamento e di attenuazione nel condotto: scelte di partenza del programma, da tarare."));
            txtDet.Text = sb.ToString();
        }
    }
}
