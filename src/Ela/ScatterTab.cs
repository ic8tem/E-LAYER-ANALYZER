// E-LAYER ANALYZER - linguette F.A.I. e AURORA: scatter da irregolarita' allineate al campo geomagnetico (IGRF-14).
// Due viste sulla stessa mappa (65 %):
//  - "Scia": luogo degli scatter point possibili per la coppia di stazioni A-B alla quota H (stile del programma
//    Aurora QTF mostrato dall'utente), con il riquadro dei QTF/elevazioni di A (rosso) e B (verde);
//  - "Iso-riflessione": clic sul punto di riflessione, curve di deviazione dalla condizione di riflessione per la stazione A
//    (stile AuroReflex / isoflection lines di SE-PROP K9SE).
// Condizione: c(A) + c(G) = 0, con c(X) = angolo (con segno) tra la linea di vista X -> SP e il piano perpendicolare a B in SP
// (contorni supplementari di K9SE/K4GFG; a1 + a2 = 180 gradi nell'articolo IC8FAX-IC8TEM).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class ScatterTab
    {
        public readonly Control Root;
        readonly bool aurora;
        readonly string pfx;
        readonly MapView map = new MapView();
        readonly QtfChart qtf = new QtfChart();
        readonly ComboBox cmbMode, cmbPreset, cmbSp;
        readonly TextBox txtA, txtB;
        readonly NumericUpDown numH, numElev;
        readonly CheckBox chkOval, chkRefr, chkAuSpots;
        readonly SelectableTextBlock txtRes;
        readonly TableView tSpots;
        readonly TextBlock lblSw;
        readonly SimpleChart kpChart;
        double aLat = double.NaN, aLon, bLat = double.NaN, bLon, spLat = double.NaN, spLon;
        string aName = "", bName = "";

        // stazioni di verifica (IC8FAX: pagina F.A.I. e articolo 2011)
        static readonly string[][] FaiPresets =
        {
            new[] { "IC8FAX - F6DRO", "IC8FAX", "JN70CN", "F6DRO", "JN03TJ" },
            new[] { "IC8FAX - G4LOH", "IC8FAX", "JN70CN", "G4LOH", "IO70JC" },
            new[] { "IC8FAX - FA1MXE (1° QSO F.A.I., 1990)", "IC8FAX", "JN70CN", "FA1MXE", "JN05GQ" }
        };
        // scatter point noti (pagina F.A.I. di IC8FAX, articolo IC8FAX-IC8TEM, DL5MCG)
        static readonly string[][] KnownSp =
        {
            new[] { "Ginevra (JN36)", "46.20", "6.15" },
            new[] { "Bruxelles (JO20)", "50.85", "4.35" },
            new[] { "Budapest (JN97)", "47.50", "19.04" },
            new[] { "JN92", "42.50", "19.00" },
            new[] { "JN41/JN42", "42.00", "9.00" }
        };

        public ScatterTab(bool aurora)
        {
            this.aurora = aurora;
            map.Note = aurora ? (Func<List<string>>)PropData.NoteAurora : PropData.NoteFai;
            // continente scelto nella barra delle linguette e sintesi dei dati di propagazione in sovraimpressione
            PropData.Updated += () => Avalonia.Threading.Dispatcher.UIThread.Post(() => map.InvalidateVisual());
            Continents.Changed += () => Continents.Apply(map);
            pfx = aurora ? "au_" : "fai_";
            Config cf = Station.Cfg;
            cmbMode = Ui.Combo(new[] { Lang.T("Scia (coppia A-B)"), Lang.T("Iso-riflessione (punto)") }, (int)cf.GetD(pfx + "modo", 0), 170);
            List<string> pr = new List<string> { Lang.T("Verifiche...") };
            if (!aurora) foreach (string[] p in FaiPresets) pr.Add(p[0]);
            cmbPreset = Ui.Combo(pr, 0, 150);
            List<string> sps = new List<string> { Lang.T("Scatter noti...") };
            foreach (string[] k in KnownSp) sps.Add(k[0]);
            cmbSp = Ui.Combo(sps, 0, 140);
            txtA = Ui.T(cf.Get(pfx + "a", ""), 120, true);
            txtB = Ui.T(cf.Get(pfx + "b", aurora ? "" : "F6DRO"), 120, true);
            numH = Ui.N(80, 160, Ui.Dm(cf.GetD(pfx + "quota_km", 110)), 0, 1, 45);   // aurora: eco a circa 110 km (100-120 km, Lange-Hesse [12])
            numElev = Ui.N(0, 10, Ui.Dm(cf.GetD(pfx + "elev_min", 0)), 1, 0.5m, 45);
            chkOval = Ui.C(Lang.T("Ovale aurorale (NOAA OVATION)"), cf.GetB(pfx + "ovale", true));
            chkRefr = Ui.C(Lang.T("Rifrazione (raggio terrestre 4/3)"), cf.GetB(pfx + "rifrazione", true));
            chkAuSpots = Ui.C(Lang.T("Spot <AU>/<AUR> sulla mappa con il calcolo"), cf.GetB(pfx + "spot_mappa", true));
            chkAuSpots.IsCheckedChanged += (s, e) => Recalc();
            chkRefr.IsCheckedChanged += (s, e) => Recalc();
            Button bCalc = Ui.B(Lang.T("Calcola"), true);
            bCalc.Click += (s, e) => Recalc(true);
            Button bEu = Ui.B(Lang.T("Europa"));
            bEu.Click += (s, e) => DefaultView();
            txtA.LostFocus += (s, e) => Recalc();
            txtB.LostFocus += (s, e) => Recalc();
            numH.ValueChanged += (s, e) => Recalc();
            numElev.ValueChanged += (s, e) => Recalc();
            cmbMode.SelectionChanged += (s, e) => Recalc();
            chkOval.IsCheckedChanged += (s, e) => Recalc();
            cmbPreset.SelectionChanged += (s, e) =>
            {
                int i = cmbPreset.SelectedIndex - 1;
                if (i < 0 || i >= FaiPresets.Length) return;
                txtA.Text = FaiPresets[i][1] + " " + FaiPresets[i][2];
                txtB.Text = FaiPresets[i][3] + " " + FaiPresets[i][4];
                cmbMode.SelectedIndex = 0;
                Recalc(true);
            };
            cmbSp.SelectionChanged += (s, e) =>
            {
                int i = cmbSp.SelectedIndex - 1;
                if (i < 0 || i >= KnownSp.Length) return;
                spLat = double.Parse(KnownSp[i][1], CultureInfo.InvariantCulture); spLon = double.Parse(KnownSp[i][2], CultureInfo.InvariantCulture);
                cmbMode.SelectedIndex = 1;
                Recalc(true);
            };

            txtRes = Ui.Text(12.5);
            tSpots = new TableView();
            tSpots.Columns.AddRange(new[] { "UTC", "MHz", "DX", "Spotter", "km", Lang.T("Commento") });
            tSpots.ColumnWidths.AddRange(new double[] { 46, 70, 80, 80, 50, 160 });
            tSpots.RowClicked += i =>
            {
                if (i < 0 || i >= tSpots.Rows.Count) return;
                Spot s = tSpots.Rows[i].Tag as Spot;
                if (s == null) return;
                txtA.Text = s.Spotter + " " + s.LocSpotter; txtB.Text = s.DxCall + " " + s.LocDx;
                cmbMode.SelectedIndex = 0;
                Recalc(true);
            };

            DockPanel right = new DockPanel { Background = Pal.B(Pal.Back) };
            Button bVerify = Ui.B(Lang.T("Verifica il punto consigliato"));
            bVerify.Click += (s, e) =>
            {
                // calcola la scia, prende lo scatter point consigliato e ne mostra le linee di iso-riflessione
                cmbMode.SelectedIndex = 0; Recalc(false);
                if (LastBest == null) { Recalc(true); return; }
                spLat = LastBest.Lat; spLon = LastBest.Lon;
                cmbMode.SelectedIndex = 1; Recalc(true);
            };
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Vista")), cmbMode, bCalc, bEu, bVerify), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Stazione A")), txtA, Ui.L(Lang.T("Stazione B")), txtB), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Quota km")), numH, Ui.L(Lang.T("Elev. min °")), numElev, aurora ? (Control)chkOval : cmbPreset, cmbSp, chkRefr), Dock.Top);
            TextBlock help = new TextBlock
            {
                Text = Lang.T("A e B: nominativo (cercato in CALL3) e/o locator; A vuoto = la mia stazione. Clic sinistro sulla mappa = punto di riflessione da verificare (iso-riflessione); clic destro = posizione di B. \"Verifica il punto consigliato\" mostra le linee di iso-riflessione sullo scatter point proposto dal programma."),
                Foreground = Pal.B(Pal.Muted), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 3), FontSize = 11.5
            };
            Ui.Dock(right, help, Dock.Top);
            if (aurora)
            {
                lblSw = new TextBlock { Foreground = Pal.B(Pal.Green), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 3) };
                kpChart = new SimpleChart { Height = 120, Title = "Kp" };
                Button bFetch = Ui.B(Lang.T("Scarica NOAA adesso"));
                bFetch.Click += (s, e) => StartFetch();
                StackPanel sw = new StackPanel();
                sw.Children.Add(lblSw); sw.Children.Add(kpChart); sw.Children.Add(bFetch);
                Ui.Dock(right, Ui.Box(Lang.T("Meteorologia spaziale (NOAA SWPC)"), Pal.Green, sw), Dock.Top);
                Ui.Dock(right, Ui.Row(chkAuSpots), Dock.Top);
            }
            ScrollViewer svRes = new ScrollViewer { Content = txtRes, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            Border spotBox = Ui.Box(aurora ? Lang.T("Spot aurora (AU)") : Lang.T("Spot F.A.I."), Pal.Orange, tSpots);
            spotBox.Height = 190;
            Ui.Dock(right, spotBox, Dock.Bottom);
            right.Children.Add(Ui.Box(Lang.T("Risultati"), Pal.Azure, svRes));

            // mappa con il riquadro dei QTF in alto a destra
            Grid mg = new Grid();
            mg.Children.Add(map);
            qtf.Width = 330; qtf.Height = 170; qtf.HorizontalAlignment = HorizontalAlignment.Right; qtf.VerticalAlignment = VerticalAlignment.Top; qtf.Margin = new Thickness(0, 34, 8, 0);
            mg.Children.Add(qtf);
            // clic sinistro = punto di riflessione da verificare (iso-riflessione); clic destro = posizione della stazione B
            map.Clicked += (la, lo, rb) =>
            {
                if (rb) txtB.Text = Geo.LatLonToLoc(la, lo, 6);
                else { spLat = la; spLon = lo; if (cmbMode.SelectedIndex != 1) cmbMode.SelectedIndex = 1; }
                Recalc(true);
            };
            Root = Ui.MapAndData(mg, right);
            DefaultView();

            // si ridisegna solo per gli spot che riguardano questa linguetta (aurora: <AU>/<AUR>; F.A.I.: FAI)
            SpotStore.Added += s => { if (aurora ? Filters.IsAuroraSpot(s) : Filters.IsFaiSpot(s)) spotsDirty = true; };
            Station.Changed += () => Recalc();
            DispatcherTimer t = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            t.Tick += (s, e) =>
            {
                if (spotsDirty && Root.IsEffectivelyVisible) FillSpots();
                if (aurora && autoFetch && (DateTime.UtcNow - SpaceWeather.LastFetch).TotalMinutes >= 5 && !fetching) StartFetch();
            };
            t.Start();
        }

        bool spotsDirty = true;
        static bool fetching;   // condiviso: una sola richiesta NOAA anche con piu' finestre (cambio di lingua o di tema)
        public static bool autoFetch = true;

        // per le fotografie automatiche
        public void ShotPreset(int i) { cmbPreset.SelectedIndex = i; }
        public PerpPoint LastBest;
        public void ShotPair(string a, string b) { txtA.Text = a; txtB.Text = b; cmbMode.SelectedIndex = 0; Recalc(); }
        public void ShotIsoA(string a, double lat, double lon) { txtA.Text = a; spLat = lat; spLon = lon; cmbMode.SelectedIndex = 1; Recalc(); }
        public void View(double w, double e, double s, double n) { map.SetView(w, e, s, n); }
        public void ShotVerify() { if (LastBest == null) return; spLat = LastBest.Lat; spLon = LastBest.Lon; cmbMode.SelectedIndex = 1; Recalc(); }
        public void ShotIso(double lat, double lon) { txtA.Text = "IC8FAX JN70CN"; txtB.Text = "FA1MXE JN05GQ"; spLat = lat; spLon = lon; cmbMode.SelectedIndex = 1; Recalc(); }

        void DefaultView()
        {
            if (aurora) map.SetView(-35, 55, 38, 80);
            else map.SetView(-15, 35, 34, 60);
        }

        public void Save()
        {
            Config cf = Station.Cfg;
            cf.Set(pfx + "modo", Math.Max(0, cmbMode.SelectedIndex));
            cf.Set(pfx + "a", txtA.Text ?? ""); cf.Set(pfx + "b", txtB.Text ?? "");
            cf.Set(pfx + "quota_km", Ui.V(numH)); cf.Set(pfx + "elev_min", Ui.V(numElev));
            cf.Set(pfx + "ovale", chkOval.IsChecked == true);
            cf.Set(pfx + "rifrazione", chkRefr.IsChecked == true);
            cf.Set(pfx + "spot_mappa", chkAuSpots.IsChecked == true);
        }

        // "NOMINATIVO LOCATOR", "LOCATOR" o "NOMINATIVO" -> coordinate
        static bool Parse(string text, out double lat, out double lon, out string name)
        {
            lat = lon = double.NaN; name = "";
            string[] parts = (text ?? "").Trim().ToUpperInvariant().Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            string loc = null;
            foreach (string p in parts) { if (Geo.IsLocator(p) && loc == null) loc = p; else if (name.Length == 0) name = p; }
            if (loc == null && name.Length > 0) loc = CallDb.Lookup(name);
            if (loc == null) return false;
            if (name.Length == 0) name = loc;
            return Geo.LocToLatLon(loc, out lat, out lon);
        }

        double H { get { return Ui.V(numH); } }

        // Calcolo. user = chiesto dall'utente (pulsante, clic, scelta da elenco): se il modello non da' soluzione
        // compare il popup "Calcolo non possibile" e non si mostra nessun altro risultato.
        string impossible;
        string pairKey = "";

        public void Recalc() { Recalc(false); }

        public void Recalc(bool user)
        {
            Geo.K = chkRefr.IsChecked == true ? 4.0 / 3.0 : 1.0;
            try { RecalcCore(user); }
            finally { Geo.K = 1.0; }
        }

        void RecalcCore(bool user)
        {
            impossible = null;
            string n;
            if (!Parse(txtA.Text, out aLat, out aLon, out n))
            {
                aLat = Station.Lat; aLon = Station.Lon; n = Station.Call.Length > 0 ? Station.Call : Station.Loc;
            }
            aName = n;
            if (!Parse(txtB.Text, out bLat, out bLon, out n)) { bLat = double.NaN; n = ""; }
            bName = n;
            string pk = aLat + "|" + aLon + "|" + bLat + "|" + bLon + "|" + H;
            if (pk != pairKey) { pairKey = pk; LastBest = null; }   // lo scatter consigliato vale solo per la coppia con cui e' stato calcolato
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear(); map.LegendTitle = "";
            qtf.Clear();
            map.Caption = (aurora ? "AURORA" : "F.A.I.") + " - " + Igrf.Info + " - " + Lang.F("quota {0:F0} km", H);
            if (aurora && chkOval.IsChecked == true) DrawOval();
            StringBuilder sb = new StringBuilder();
            if (double.IsNaN(aLat)) impossible = Lang.T("Manca la stazione A: imposta il locator della stazione in alto o scrivilo nella casella Stazione A.");
            else if (cmbMode.SelectedIndex == 1) Iso(sb);
            else Scia(sb);
            if (aurora && impossible == null) AuroraInfo(sb);
            if (aurora && impossible == null && chkAuSpots.IsChecked == true) AuroraSpots(sb);
            if (impossible != null)
            {
                // nessun altro risultato: mappa pulita, nessun testo
                map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear(); map.LegendTitle = "";
                qtf.Clear();
                LastBest = null;
                txtRes.Text = "";
                if (user && Program.ScreenshotDir == null)
                {
                    string msg = impossible;
                    Dispatcher.UIThread.Post(async () => await Dialogs.Message(TopLevel.GetTopLevel(Root) as Window, Lang.T("Calcolo non possibile"), msg));
                }
            }
            else txtRes.Text = sb.ToString();
            qtf.IsVisible = cmbMode.SelectedIndex == 0 && qtf.HasData && impossible == null;
            qtf.InvalidateVisual();
            map.InvalidateVisual();
            if (aurora) UpdateSw();
        }

        static Color Dev(double d, bool green)
        {
            // classi di deviazione come Aurora QTF / AuroReflex
            byte a = d <= 1 ? (byte)230 : d <= 1.5 ? (byte)190 : d <= 2 ? (byte)140 : d <= 2.5 ? (byte)95 : d <= 4 ? (byte)70 : d <= 8 ? (byte)45 : (byte)25;
            return green ? Color.FromArgb(a, 40, 220, 60) : Color.FromArgb(a, 255, 30, 30);
        }

        void StationMarks()
        {
            double hr = Geo.HalfHopKm(0, H);
            map.Lines.Add(MapView.Circle(aLat, aLon, hr, Pal.Green, 1.5));
            map.Markers.Add(new MapMarker { Lat = aLat, Lon = aLon, Radius = 6, Fill = Color.FromRgb(30, 200, 60), Stroke = Colors.DarkRed, Label = aName });
            if (!double.IsNaN(bLat))
            {
                map.Lines.Add(MapView.Circle(bLat, bLon, hr, Pal.Green, 1.5));
                map.Markers.Add(new MapMarker { Lat = bLat, Lon = bLon, Radius = 6, Fill = Color.FromRgb(30, 200, 60), Label = bName });
            }
        }

        // ---------------------------------------------------------------- Scia: luogo degli scatter point per A-B
        void Scia(StringBuilder sb)
        {
            StationMarks();
            if (double.IsNaN(bLat)) { sb.AppendLine(Lang.T("Imposta la stazione B (nominativo, locator o clic sulla mappa).")); return; }
            double h = H, maxD = Geo.HalfHopKm(Ui.V(numElev), h);
            double step = 0.2;
            List<PerpPoint> loc = FieldAligned.PairLocus(aLat, aLon, bLat, bLon, h, Tol, Ui.V(numElev), maxD, step);
            Func<double, double, double> w = null;
            if (aurora && chkOval.IsChecked == true && SpaceWeather.Oval != null) w = (la, lo) => SpaceWeather.OvalAt(la, lo) / 30.0;
            PerpPoint best = null; double bestScore = double.MaxValue;
            LastBest = null;
            foreach (PerpPoint p in loc)
            {
                map.Cells.Add(new MapCell { Lat0 = p.Lat - step / 2, Lon0 = p.Lon - step / 2, DLat = step, DLon = step, Fill = Dev(p.Off, false) });
                double dB = Geo.DistanceKm(bLat, bLon, p.Lat, p.Lon);
                double eB = Geo.ElevationDeg(dB, h, 0);
                double azB = Geo.Bearing(bLat, bLon, p.Lat, p.Lon);
                qtf.Add(p.Az, p.Elev, azB, eB, p.Off);
                double score = (p.Off <= 1 ? 0 : 100 + p.Off * 10) + (p.DistKm * p.DistKm + dB * dB) / 1e6;
                if (w != null) score += 5 * (1 - Math.Max(0, Math.Min(1, w(p.Lat, p.Lon))));
                if (!InAuRange(p, bLat, bLon)) score += 3;   // aurora: preferenza per 370-1100 km da ciascuna stazione (Lange-Hesse)
                if (score < bestScore) { bestScore = score; best = p; }
            }
            map.Legend.Add(new KeyValuePair<string, Color>("0° - 1°", Dev(0.5, false)));
            map.Legend.Add(new KeyValuePair<string, Color>("1° - 1,5°", Dev(1.2, false)));
            map.Legend.Add(new KeyValuePair<string, Color>("1,5° - 2°", Dev(1.8, false)));
            map.Legend.Add(new KeyValuePair<string, Color>("2° - 2,5°", Dev(2.2, false)));
            map.LegendTitle = Lang.T("Deviazione dalla condizione di riflessione");
            double dAB = Geo.DistanceKm(aLat, aLon, bLat, bLon);
            sb.AppendLine(Lang.F("Scia degli scatter point possibili {0} - {1} ({2:F0} km), quota {3:F0} km", aName, bName, dAB, h));
            sb.AppendLine(Lang.F("Celle del luogo entro 2,5°: {0} (passo {1}°); entrambe le stazioni vedono il punto sopra {2:F1}° di elevazione{3}.", loc.Count, step.ToString(CultureInfo.CurrentCulture), Ui.V(numElev), Geo.K > 1 ? Lang.T(", con la rifrazione (4/3)") : ""));
            if (best == null)
            {
                impossible = Lang.F("Per il modello nessun punto alla quota di {0:F0} km, visto da entrambe le stazioni ({1} e {2}), soddisfa la condizione di riflessione entro 2,5°.", h, aName, bName);
                return;
            }
            LastBest = best;
            double dBb = Geo.DistanceKm(bLat, bLon, best.Lat, best.Lon);
            sb.AppendLine();
            sb.AppendLine(Lang.T("Scatter point più probabile (deviazione minima e tratta più corta):"));
            sb.AppendLine(Lang.F("  {0}  ({1:F2}°, {2:F2}°)  deviazione {3:F2}°", Geo.LatLonToLoc(best.Lat, best.Lon, 6), best.Lat, best.Lon, best.Off));
            sb.AppendLine(Lang.F("  {0}: QTF {1:F0}°, elevazione {2:F1}°, distanza {3:F0} km", aName, best.Az, best.Elev, best.DistKm));
            sb.AppendLine(Lang.F("  {0}: QTF {1:F0}°, elevazione {2:F1}°, distanza {3:F0} km", bName, Geo.Bearing(bLat, bLon, best.Lat, best.Lon), Geo.ElevationDeg(dBb, h, 0), dBb));
            sb.AppendLine(Lang.F("  Inclinazione magnetica nel punto: {0:F1}°", Geo.InclinationDeg(best.Lat, best.Lon)));
            map.Markers.Add(new MapMarker { Lat = best.Lat, Lon = best.Lon, Radius = 9, Fill = Pal.Yellow, Ring = true, Label = Geo.LatLonToLoc(best.Lat, best.Lon, 4), LabelColor = Pal.Yellow });
            map.Lines.Add(MapView.GreatCircle(aLat, aLon, best.Lat, best.Lon, Pal.Yellow, 1.2, true));
            map.Lines.Add(MapView.GreatCircle(bLat, bLon, best.Lat, best.Lon, Pal.Yellow, 1.2, true));
            map.Info.Add(Lang.F("{0}: QTF {1:F0}°  el {2:F0}°", aName, best.Az, best.Elev));
            map.Info.Add(Lang.F("{0}: QTF {1:F0}°  el {2:F0}°", bName, Geo.Bearing(bLat, bLon, best.Lat, best.Lon), Geo.ElevationDeg(dBb, h, 0)));
            if (aurora) QtfRange(sb, loc, best);
            if (aurora) AuroraNotes(sb, best);
            // scatter noti vicini alla scia
            foreach (string[] k in KnownSp)
            {
                double la = double.Parse(k[1], CultureInfo.InvariantCulture), lo = double.Parse(k[2], CultureInfo.InvariantCulture);
                double bestD = double.MaxValue;
                foreach (PerpPoint p in loc) if (p.Off <= 1.5) bestD = Math.Min(bestD, Geo.DistanceKm(la, lo, p.Lat, p.Lon));
                if (bestD < 150) sb.AppendLine(Lang.F("  La scia passa a {0:F0} km dallo scatter noto {1}.", bestD, k[0]));
            }
            sb.AppendLine();
            sb.AppendLine(Lang.T("Riquadro in alto a destra: QTF ed elevazione verso ogni punto della scia, rosso = A, verde = B (più intenso = deviazione minore)."));
        }

        // ---------------------------------------------------------------- Iso-riflessione per un punto di riflessione
        // Linee di iso-riflessione (isoflection loci, K9SE / AuroReflex): per ogni punto al suolo G la somma
        // S = c(A) + c(G) dei contorni; S = 0 = condizione di riflessione esatta. Schemi scuri: fasce colorate;
        // schemi chiari: linee spesse (curve di livello 0, ±1, ±2, ±4, ±8, ±16 gradi).
        static readonly double[] IsoLevels = { 0, 1, 2, 4, 8, 16 };
        static Color IsoColor(int k, bool light)
        {
            Color[] dark = { Color.FromRgb(255, 40, 40), Color.FromRgb(255, 140, 30), Color.FromRgb(250, 225, 60), Color.FromRgb(90, 220, 90), Color.FromRgb(60, 200, 230), Color.FromRgb(110, 120, 255) };
            Color[] lite = { Color.FromRgb(190, 0, 0), Color.FromRgb(230, 90, 0), Color.FromRgb(170, 130, 0), Color.FromRgb(20, 140, 40), Color.FromRgb(0, 120, 160), Color.FromRgb(50, 60, 190) };
            return (light ? lite : dark)[Math.Max(0, Math.Min(5, k))];
        }
        static double IsoWidth(int k) { double[] w = { 5, 4, 3.2, 2.6, 2.1, 1.7 }; return w[Math.Max(0, Math.Min(5, k))]; }

        static double Contour(Geo.V3 sp, Geo.V3 b, Geo.V3 x) { return Math.Asin(Math.Max(-1, Math.Min(1, (sp - x).Unit.Dot(b)))) * Geo.R2D; }

        void Iso(StringBuilder sb)
        {
            StationMarks();
            double h = H;
            if (double.IsNaN(spLat))
            {
                sb.AppendLine(Lang.T("Clic sinistro sulla mappa per scegliere il punto di riflessione da verificare, oppure \"Verifica il punto consigliato\"."));
                return;
            }
            Geo.V3 sp = Geo.Ecef(spLat, spLon, h);
            Geo.V3 b = Geo.FieldDir(sp);
            double cA = Contour(sp, b, Geo.Ecef(aLat, aLon, Station.AltKm));   // contorno di A (K9SE)
            double dA = Geo.DistanceKm(aLat, aLon, spLat, spLon), eA = Geo.ElevationDeg(dA, h, Station.AltKm), azA = Geo.Bearing(aLat, aLon, spLat, spLon);
            if (eA < Ui.V(numElev)) { impossible = Lang.F("Il punto di riflessione {0} è sotto l'orizzonte di {1} alla quota di {2:F0} km (elevazione {3:F1}°).", Geo.LatLonToLoc(spLat, spLon, 6), aName, h, eA); return; }
            double dB = double.NaN, eB = double.NaN, azB = double.NaN, devB = double.NaN;
            if (!double.IsNaN(bLat))
            {
                dB = Geo.DistanceKm(bLat, bLon, spLat, spLon); eB = Geo.ElevationDeg(dB, h, 0); azB = Geo.Bearing(bLat, bLon, spLat, spLon);
                devB = Math.Abs(cA + Contour(sp, b, Geo.Ecef(bLat, bLon, 0)));
                if (eB < Ui.V(numElev)) { impossible = Lang.F("Il punto di riflessione {0} è sotto l'orizzonte di {1} alla quota di {2:F0} km (elevazione {3:F1}°).", Geo.LatLonToLoc(spLat, spLon, 6), bName, h, eB); return; }
            }
            double hr = Geo.HalfHopKm(0, h);
            map.Lines.Add(MapView.Circle(spLat, spLon, hr, Pal.Red, 1.5));

            // griglia al suolo entro l'orizzonte del punto: valore con segno S = c(A) + c(G)
            double dl = hr / 111.0, cl = Math.Max(0.15, Math.Cos(spLat * Geo.D2R)), dlo = dl / cl;
            double stLa = 0.15, stLo = 0.15 / Math.Max(0.3, cl);
            int nLa = (int)(2 * dl / stLa) + 2, nLo = (int)(2 * dlo / stLo) + 2;
            double la0 = spLat - dl, lo0 = spLon - dlo;
            double[,] S = new double[nLa, nLo];
            for (int i = 0; i < nLa; i++)
                for (int j = 0; j < nLo; j++)
                {
                    double la = la0 + i * stLa, lo = lo0 + j * stLo;
                    if (la > 89 || la < -89 || Geo.DistanceKm(spLat, spLon, la, lo) > hr) { S[i, j] = double.NaN; continue; }
                    S[i, j] = cA + Contour(sp, b, Geo.Ecef(la, lo, 0));
                }
            bool light = Themes.Light;
            if (!light)
            {
                // fasce colorate
                for (int i = 0; i < nLa; i++)
                    for (int j = 0; j < nLo; j++)
                    {
                        double v = Math.Abs(S[i, j]);
                        if (double.IsNaN(v) || v > 16) continue;
                        int k = v <= 0.5 ? 0 : v <= 1 ? 1 : v <= 2 ? 2 : v <= 4 ? 3 : v <= 8 ? 4 : 5;
                        Color c = IsoColor(k, false);
                        byte a = (byte)(k == 0 ? 230 : k == 1 ? 190 : k == 2 ? 150 : k == 3 ? 115 : k == 4 ? 85 : 60);
                        map.Cells.Add(new MapCell { Lat0 = la0 + i * stLa - stLa / 2, Lon0 = lo0 + j * stLo - stLo / 2, DLat = stLa, DLon = stLo, Fill = Color.FromArgb(a, c.R, c.G, c.B) });
                    }
            }
            else
            {
                // linee spesse: curve di livello con il metodo dei quadrati marcianti
                for (int k = 0; k < IsoLevels.Length; k++)
                    foreach (double lev in k == 0 ? new[] { 0.0 } : new[] { IsoLevels[k], -IsoLevels[k] })
                    {
                        Color c = IsoColor(k, true); double w = IsoWidth(k);
                        bool labelled = false;
                        for (int i = 0; i < nLa - 1; i++)
                            for (int j = 0; j < nLo - 1; j++)
                            {
                                double v00 = S[i, j] - lev, v01 = S[i, j + 1] - lev, v10 = S[i + 1, j] - lev, v11 = S[i + 1, j + 1] - lev;
                                if (double.IsNaN(v00) || double.IsNaN(v01) || double.IsNaN(v10) || double.IsNaN(v11)) continue;
                                List<Point> pts = new List<Point>(4);
                                double y0 = la0 + i * stLa, x0 = lo0 + j * stLo;
                                if ((v00 < 0) != (v01 < 0)) pts.Add(new Point(x0 + stLo * v00 / (v00 - v01), y0));
                                if ((v10 < 0) != (v11 < 0)) pts.Add(new Point(x0 + stLo * v10 / (v10 - v11), y0 + stLa));
                                if ((v00 < 0) != (v10 < 0)) pts.Add(new Point(x0, y0 + stLa * v00 / (v00 - v10)));
                                if ((v01 < 0) != (v11 < 0)) pts.Add(new Point(x0 + stLo, y0 + stLa * v01 / (v01 - v11)));
                                for (int q = 0; q + 1 < pts.Count; q += 2)
                                {
                                    MapLine l = new MapLine { Color = c, Width = w };
                                    l.Pts.Add(pts[q]); l.Pts.Add(pts[q + 1]);
                                    map.Lines.Add(l);
                                }
                                if (!labelled && pts.Count >= 2 && (i + j) % 7 == 0)
                                {
                                    labelled = true;
                                    map.Markers.Add(new MapMarker { Lat = pts[0].Y, Lon = pts[0].X, Radius = 0.1, Fill = c, Stroke = c, Label = lev.ToString("+0;-0;0", CultureInfo.InvariantCulture) + "°", LabelColor = c });
                                }
                            }
                    }
            }
            map.Markers.Add(new MapMarker { Lat = spLat, Lon = spLon, Radius = 10, Fill = Pal.Red, Ring = true, Label = Geo.LatLonToLoc(spLat, spLon, 6), LabelColor = Pal.Red });
            map.Lines.Add(MapView.GreatCircle(aLat, aLon, spLat, spLon, Pal.Yellow, 1.4, true));
            if (!double.IsNaN(bLat)) map.Lines.Add(MapView.GreatCircle(bLat, bLon, spLat, spLon, Pal.Yellow, 1.4, true));
            map.Info.Add(Lang.F("{0}: azimut {1:F0}°  elevazione {2:F0}°", aName, azA, eA));
            if (!double.IsNaN(bLat)) map.Info.Add(Lang.F("{0}: azimut {1:F0}°  elevazione {2:F0}°", bName, azB, eB));
            for (int k = 0; k < IsoLevels.Length; k++)
                map.Legend.Add(new KeyValuePair<string, Color>((k == 0 ? "0°" : "±" + IsoLevels[k].ToString(CultureInfo.InvariantCulture) + "°"), IsoColor(k, light)));
            map.LegendTitle = Lang.T("Iso-riflessione: deviazione dalla condizione di riflessione");

            // verdetto sul punto scelto
            sb.AppendLine(Lang.F("Punto di riflessione {0} ({1:F2}°, {2:F2}°) a {3:F0} km di quota", Geo.LatLonToLoc(spLat, spLon, 6), spLat, spLon, h));
            sb.AppendLine(Lang.F("{0}: azimut {1:F0}°, elevazione {2:F1}°, distanza {3:F0} km, contorno K9SE {4:+0.0;-0.0}°", aName, azA, eA, dA, cA));
            if (!double.IsNaN(bLat))
            {
                sb.AppendLine(Lang.F("{0}: azimut {1:F0}°, elevazione {2:F1}°, distanza {3:F0} km", bName, azB, eB, dB));
                string verdict = devB <= 1 ? Lang.T("SODDISFACENTE (entro 1°)") : devB <= 2.5 ? Lang.T("ACCETTABILE (entro 2,5°, tolleranza di Lange-Hesse ±2-3°)") : Lang.T("NON SODDISFACENTE (oltre 2,5°)");
                sb.AppendLine(Lang.F("Deviazione della coppia {0} - {1}: {2:F2}°  ->  {3}", aName, bName, devB, verdict));
                if (LastBest != null) sb.AppendLine(Lang.F("Scatter point consigliato dal programma: {0} (deviazione {1:F2}°), a {2:F0} km da questo punto.", Geo.LatLonToLoc(LastBest.Lat, LastBest.Lon, 6), LastBest.Off, Geo.DistanceKm(LastBest.Lat, LastBest.Lon, spLat, spLon)));
            }
            else sb.AppendLine(Lang.T("Imposta la stazione B (o clic destro sulla mappa) per il verdetto sulla coppia."));
            sb.AppendLine(Lang.F("Inclinazione magnetica nel punto: {0:F1}°", Geo.InclinationDeg(spLat, spLon)));
            sb.AppendLine();
            sb.AppendLine(Lang.T("Ricevono la stazione A attraverso questo punto le stazioni sulla linea 0° (rossa); le linee ±1, ±2, ±4, ±8, ±16° dicono di quanto si scostano le altre. Clic sinistro su un altro punto per verificarlo."));
            sb.AppendLine(Lang.T("Cerchio verde: orizzonte delle stazioni alla quota dello strato; cerchio rosso: zona da cui il punto di riflessione è visibile."));
        }

        // ---------------------------------------------------------------- aurora
        // ovale OVATION sfumato: interpolazione bilineare su celle di 0,5 gradi
        void DrawOval()
        {
            if (SpaceWeather.Oval == null) return;
            for (double lon = -180; lon < 180; lon += 0.5)
                for (double lat = 35; lat <= 89; lat += 0.5)
                {
                    double v = OvalSmooth(lat + 0.25, lon + 0.25);
                    if (v < 2) continue;
                    byte a = (byte)Math.Min(160, 20 + v * 3);
                    double t = Math.Min(1, v / 60.0);
                    // verde -> giallo -> rosso al crescere della probabilita'
                    byte r = (byte)(t < 0.5 ? 80 + 346 * t : 253);
                    byte g = (byte)(t < 0.5 ? 230 : 230 - 300 * (t - 0.5));
                    byte bl = (byte)(t < 0.5 ? 120 - 100 * t : 70);
                    map.Cells.Add(new MapCell { Lat0 = lat, Lon0 = lon, DLat = 0.5, DLon = 0.5, Fill = Color.FromArgb(a, r, g, bl) });
                }
        }

        static double OvalSmooth(double lat, double lon)
        {
            double la0 = Math.Floor(lat), lo0 = Math.Floor(lon), fa = lat - la0, fo = lon - lo0;
            double v00 = SpaceWeather.OvalAt(la0, lo0), v01 = SpaceWeather.OvalAt(la0, lo0 + 1), v10 = SpaceWeather.OvalAt(la0 + 1, lo0), v11 = SpaceWeather.OvalAt(la0 + 1, lo0 + 1);
            return v00 * (1 - fa) * (1 - fo) + v01 * (1 - fa) * fo + v10 * fa * (1 - fo) + v11 * fa * fo;
        }

        void AuroraInfo(StringBuilder sb)
        {
            if (double.IsNaN(aLat)) return;
            sb.AppendLine();
            AuroraStation(sb, aLat, aLon, Station.AltKm, aName, Pal.Azure);
            if (!double.IsNaN(bLat)) AuroraStation(sb, bLat, bLon, 0, bName, Pal.Violet);
        }

        // Puntamento consigliato di una stazione: lungo la sua linea di perpendicolarita' (eco con raggio perpendicolare a B)
        // il punto con la probabilita' d'ovale piu' alta; regola pratica di partenza (G3WPO): circa 45 gradi, tra nord ed est.
        void AuroraStation(StringBuilder sb, double lat, double lon, double hKm, string name, Color c)
        {
            sb.AppendLine(Lang.F("{0}: latitudine geomagnetica (dipolo) {1:F1}°", name, Geo.GeomagLat(lat, lon)));
            double bnd = SpaceWeather.EquatorBoundary(lon, 10);
            if (!double.IsNaN(bnd)) sb.AppendLine(Lang.F("  confine equatoriale dell'ovale (≥10 %) alla sua longitudine: {0:F0}° N", bnd));
            List<PerpPoint> line = FieldAligned.PerpLine(lat, lon, hKm, H, 1.5, Ui.V(numElev), 1800, 2);
            PerpPoint best = null; double bv = 0;
            foreach (PerpPoint p in line)
            {
                map.Markers.Add(new MapMarker { Lat = p.Lat, Lon = p.Lon, Radius = 2, Fill = c, Stroke = c });
                double v = OvalSmooth(p.Lat, p.Lon);
                if (v > bv) { bv = v; best = p; }
            }
            if (best != null && bv >= 5)
            {
                sb.AppendLine(Lang.F("  puntamento consigliato: azimut {0:F0}°, elevazione {1:F1}°, distanza {2:F0} km, probabilità ovale {3:F0} %", best.Az, best.Elev, best.DistKm, bv));
                map.Markers.Add(new MapMarker { Lat = best.Lat, Lon = best.Lon, Radius = 8, Fill = c, Ring = true, Label = Lang.F("{0} QTF {1:F0}°", name, best.Az), LabelColor = c });
            }
            else sb.AppendLine(Lang.F("  la linea di perpendicolarità (puntini) non entra nell'ovale: aurora radio improbabile adesso (regola pratica: partire da circa 45°, tra nord ed est)."));
        }

        void UpdateSw()
        {
            if (lblSw == null) return;
            double kp = SpaceWeather.LatestKp;
            lblSw.Text = Lang.F("Kp {0}   Bz {1} nT   vento {2} km/s   densità {3} p/cm³", Geo.Fmt(kp, 1), Geo.Fmt(SpaceWeather.Bz, 1), Geo.Fmt(SpaceWeather.Speed, 0), Geo.Fmt(SpaceWeather.Density, 1))
                + "\n" + Lang.F("Ovale: {0}   ultimo scaricamento: {1}", SpaceWeather.OvalTime, SpaceWeather.LastFetch == DateTime.MinValue ? "-" : SpaceWeather.LastFetch.ToString("HH:mm", CultureInfo.InvariantCulture) + " UTC")
                + (SpaceWeather.LastError.Length > 0 ? "\n" + SpaceWeather.LastError : "");
            List<KeyValuePair<DateTime, double>> kps;
            lock (SpaceWeather.Kp) kps = new List<KeyValuePair<DateTime, double>>(SpaceWeather.Kp);
            int n = Math.Min(24, kps.Count);
            double[] v = new double[n]; string[] x = new string[n];
            for (int i = 0; i < n; i++) { KeyValuePair<DateTime, double> k = kps[kps.Count - n + i]; v[i] = k.Value; x[i] = k.Key.ToString("dd HH", CultureInfo.InvariantCulture); }
            kpChart.Series.Clear();
            kpChart.Series.Add(new ChartSeries { Name = "Kp", Color = Pal.Green, Values = v, Bars = true });
            kpChart.XLabels = x; kpChart.XLabelEvery = 4; kpChart.YMaxFixed = 9;
            kpChart.Title = Lang.T("Kp planetario (ultimi 3 giorni, ogni 3 ore)");
            kpChart.InvalidateVisual();
        }

        public void StartFetch()
        {
            if (fetching) return;
            fetching = true;
            string dd = AppPaths.DataDir;
            System.Threading.Tasks.Task.Run(async () =>
            {
                try { await SpaceWeather.FetchAsync(dd); } catch { }
                Dispatcher.UIThread.Post(() => { fetching = false; if (SpaceWeather.LastFetch == DateTime.MinValue) SpaceWeather.LastFetch = DateTime.UtcNow; Recalc(); });
            });
        }

        void FillSpots()
        {
            spotsDirty = false;
            List<Spot> l = SpotStore.Recent(aurora ? Filters.AuMinutes : Filters.FaiMinutes, s => s.HasBoth && (aurora ? Filters.IsAuroraSpot(s) : Filters.IsFaiSpot(s)));
            tSpots.Rows.Clear();
            foreach (Spot s in l)
                tSpots.Rows.Add(new TableRow
                {
                    Cells = new[] { s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), s.FreqMHz.ToString("F3", CultureInfo.CurrentCulture), s.DxCall, s.Spotter, Geo.Fmt(s.DistanceKm, 0), s.Comment },
                    Fore = EsTab.BandColor(s.Band), Tag = s
                });
            tSpots.Refresh();
            if (aurora && chkAuSpots.IsChecked == true) Recalc();   // nuovi spot di aurora: si ridisegnano con il calcolo
        }

        // ---------------------------------------------------------------- spot di aurora sulla mappa, con il calcolo
        // Regola dell'autore: gli spot con <AU> o <AUR> nel testo sono di aurora. Per ogni spot (ultime 2 ore, al massimo 30)
        // si calcola la scia degli scatter point della coppia spotter-DX (stesso modello della vista Scatter trail, passo 0,5°)
        // e si disegna lo scatter point piu' probabile, con le direzioni di puntamento delle due stazioni.
        class AuSol { public PerpPoint Best; public double QtfB, ElB; }
        readonly Dictionary<string, AuSol> auCache = new Dictionary<string, AuSol>();

        void AuroraSpots(StringBuilder sb)
        {
            List<Spot> l = SpotStore.Recent(Filters.AuMinutes, s => s.HasBoth && Filters.IsAuroraSpot(s));
            if (l.Count > 30) l = l.GetRange(0, 30);
            if (l.Count == 0) return;
            double h = H, minEl = Ui.V(numElev), maxD = Geo.HalfHopKm(minEl, h);
            bool oval = chkOval.IsChecked == true && SpaceWeather.Oval != null;
            Func<double, double, double> w = null;
            if (oval) w = (la, lo) => SpaceWeather.OvalAt(la, lo) / 30.0;
            if (auCache.Count > 500) auCache.Clear();
            int ok = 0;
            sb.AppendLine();
            sb.AppendLine(Lang.F("Spot di aurora delle ultime {0} ore sulla mappa ({1}):", Math.Round(Filters.AuMinutes / 60.0, 1), l.Count));
            HashSet<string> seen = new HashSet<string>();
            foreach (Spot s in l)
            {
                if (!seen.Add(s.Spotter + "|" + s.DxCall)) continue;
                string key = s.Key + "|" + h + "|" + minEl + "|" + Geo.K + "|" + oval;
                AuSol sol;
                if (!auCache.TryGetValue(key, out sol))
                {
                    sol = new AuSol();
                    List<PerpPoint> loc = FieldAligned.PairLocus(s.LatS, s.LonS, s.LatD, s.LonD, h, Tol, minEl, maxD, 0.5);
                    double bestScore = double.MaxValue;
                    foreach (PerpPoint p in loc)
                    {
                        double dB = Geo.DistanceKm(s.LatD, s.LonD, p.Lat, p.Lon);
                        double score = (p.Off <= 1 ? 0 : 100 + p.Off * 10) + (p.DistKm * p.DistKm + dB * dB) / 1e6;
                        if (w != null) score += 5 * (1 - Math.Max(0, Math.Min(1, w(p.Lat, p.Lon))));
                        if (!InAuRange(p, s.LatD, s.LonD)) score += 3;
                        if (score < bestScore) { bestScore = score; sol.Best = p; }
                    }
                    if (sol.Best != null)
                    {
                        double dB = Geo.DistanceKm(s.LatD, s.LonD, sol.Best.Lat, sol.Best.Lon);
                        sol.QtfB = Geo.Bearing(s.LatD, s.LonD, sol.Best.Lat, sol.Best.Lon); sol.ElB = Geo.ElevationDeg(dB, h, 0);
                    }
                    auCache[key] = sol;
                }
                Color bc = EsTab.BandColor(s.Band);
                map.Markers.Add(new MapMarker { Lat = s.LatS, Lon = s.LonS, Radius = 3.5, Fill = bc, Label = s.Spotter });
                map.Markers.Add(new MapMarker { Lat = s.LatD, Lon = s.LonD, Radius = 3.5, Fill = bc, Label = s.DxCall });
                if (sol.Best == null)
                {
                    map.Lines.Add(MapView.GreatCircle(s.LatS, s.LonS, s.LatD, s.LonD, Pal.A(110, Pal.Silver), 1, true));
                    sb.AppendLine(Lang.F("  {0} {1} - {2}: nessuno scatter point entro 2,5° per il modello", s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture) + "Z", s.Spotter, s.DxCall));
                    continue;
                }
                ok++;
                PerpPoint b = sol.Best;
                map.Lines.Add(MapView.GreatCircle(s.LatS, s.LonS, b.Lat, b.Lon, Pal.A(200, bc), 1.4, true));
                map.Lines.Add(MapView.GreatCircle(s.LatD, s.LonD, b.Lat, b.Lon, Pal.A(200, bc), 1.4, true));
                map.Markers.Add(new MapMarker { Lat = b.Lat, Lon = b.Lon, Radius = 6, Fill = Pal.Orange, Ring = true, Label = Geo.LatLonToLoc(b.Lat, b.Lon, 4), LabelColor = Pal.Orange });
                sb.AppendLine(Lang.F("  {0} {1} - {2} {3} MHz: scatter {4} (dev. {5:F1}°), {1} QTF {6:F0}° el {7:F0}°, {2} QTF {8:F0}° el {9:F0}°",
                    s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture) + "Z", s.Spotter, s.DxCall, s.FreqMHz.ToString("F3", CultureInfo.CurrentCulture),
                    Geo.LatLonToLoc(b.Lat, b.Lon, 6), b.Off, b.Az, b.Elev, sol.QtfB, sol.ElB)
                    + (s.DistanceKm > AuMaxKm ? "  " + Lang.F("dubbio: {0:F0} km, oltre i {1:F0} km tipici dell'aurora", s.DistanceKm, AuMaxKm) : ""));
            }
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("○ arancio: scatter point calcolato per uno spot <AU>/<AUR>"), Pal.Orange));
            AuroraESpots(sb);
        }

        // ---------------------------------------------------------------- parametri dell'aurora dalle fonti approvate
        // [12] Lange-Hesse (144 MHz): perpendicolarita' al campo entro +-2-3 gradi, eco a ~110 km, scatter point a 370-1100 km
        // da ciascuna stazione; nessun collegamento con Kp 0-2. [14] G3WPO e [33] G0ISW: QTF tipico N-NE (0-50 gradi),
        // distanze tipiche 250-1300 km, eccezionali 2000 km; Doppler fino a 2 kHz in 144 MHz; 50 MHz favorito.
        // [34] K7CW: in aurora FT8 spesso non decodifica, meglio CW/SSB o Q65 con sottomodi larghi; [61] PE1ITR: Q65-30D
        // decodificato in 144 MHz l'11 maggio 2024, segnali quasi invisibili sul waterfall di WSJT-X, serve RIT o split.
        double Tol { get { return aurora ? 3.0 : 2.5; } }
        public const double AuMinRangeKm = 370, AuMaxRangeKm = 1100, AuMaxKm = 2000;
        bool InAuRange(PerpPoint p, double bLa, double bLo)
        {
            if (!aurora) return true;
            double dB = Geo.DistanceKm(bLa, bLo, p.Lat, p.Lon);
            return p.DistKm >= AuMinRangeKm && p.DistKm <= AuMaxRangeKm && dB >= AuMinRangeKm && dB <= AuMaxRangeKm;
        }

        // probabilita' dell'aurora nello scatter point: Kp (Lange-Hesse) e ovale NOAA OVATION (al posto di una soglia di Kp per latitudine)
        public static string AuroraChance(double la, double lo, out double ov)
        {
            double kp = SpaceWeather.LatestKp;
            ov = SpaceWeather.Oval != null ? SpaceWeather.OvalAt(la, lo) : double.NaN;
            if (!double.IsNaN(kp) && kp <= 2 && !(PropData.Dst <= -50)) return Lang.T("improbabile (Kp ≤ 2)");
            // emisfero sud: allerta aurora del Bureau of Meteorology (Australia) [44]
            if (la < 0 && PropData.BomAlert.Length > 0) return Lang.T("probabile (allerta aurora BoM)");
            // tempesta geomagnetica (Dst di Kyoto [46]): l'ovale scende verso sud
            if (PropData.Dst <= -100 && !double.IsNaN(ov) && ov >= 1) return Lang.T("probabile (tempesta, Dst ≤ -100 nT)");
            if (PropData.Dst <= -50 && !double.IsNaN(ov) && ov >= 1) return Lang.T("possibile (tempesta, Dst ≤ -50 nT)");
            if (!double.IsNaN(ov) && ov >= 10) return Lang.T("probabile");
            if ((!double.IsNaN(ov) && ov >= 3) || (!double.IsNaN(kp) && kp >= 5)) return Lang.T("possibile");
            return Lang.T("improbabile");
        }

        // intervallo di QTF lungo tutta la scia (scelta dell'autore): oltre al punto piu' probabile,
        // i puntamenti di tutti i punti con deviazione <= 1,5 gradi, di preferenza a 370-1100 km da entrambe le stazioni
        void QtfRange(StringBuilder sb, List<PerpPoint> loc, PerpPoint best)
        {
            List<PerpPoint> good = loc.FindAll(p => p.Off <= 1.5 && InAuRange(p, bLat, bLon));
            bool inRange = good.Count > 0;
            if (!inRange) good = loc.FindAll(p => p.Off <= 1.5);
            if (good.Count < 2) return;
            double azB0 = Geo.Bearing(bLat, bLon, best.Lat, best.Lon);
            double aMin = 0, aMax = 0, bMin = 0, bMax = 0;
            foreach (PerpPoint p in good)
            {
                double da = Geo.Norm180(p.Az - best.Az), db = Geo.Norm180(Geo.Bearing(bLat, bLon, p.Lat, p.Lon) - azB0);
                aMin = Math.Min(aMin, da); aMax = Math.Max(aMax, da); bMin = Math.Min(bMin, db); bMax = Math.Max(bMax, db);
            }
            Func<double, string> f = v => Geo.Norm360(v).ToString("F0", CultureInfo.InvariantCulture) + "°";
            sb.AppendLine(inRange ? Lang.T("Intervallo di QTF lungo la scia (deviazione ≤ 1,5°, 370-1100 km da entrambe le stazioni):")
                                  : Lang.T("Intervallo di QTF lungo la scia (deviazione ≤ 1,5°):"));
            sb.AppendLine(Lang.F("  {0}: da {1} a {2}   {3}: da {4} a {5}", aName, f(best.Az + aMin), f(best.Az + aMax), bName, f(azB0 + bMin), f(azB0 + bMax)));
            sb.AppendLine(Lang.T("  Il punto più probabile privilegia la tratta più corta; l'aurora può arrivare da tutto l'intervallo."));
            map.Info.Add(Lang.F("QTF {0} {1}-{2}", aName, f(best.Az + aMin), f(best.Az + aMax)));
        }

        void AuroraNotes(StringBuilder sb, PerpPoint best)
        {
            double ov;
            string ch = AuroraChance(best.Lat, best.Lon, out ov);
            sb.AppendLine();
            sb.AppendLine(Lang.F("Probabilità di aurora nello scatter point: {0} (ovale NOAA {1} %, Kp {2})", ch, Geo.Fmt(ov, 0), Geo.Fmt(SpaceWeather.LatestKp, 1)));
            map.Info.Add(Lang.F("Aurora: {0}", ch));
            double direct = Geo.Bearing(aLat, aLon, bLat, bLon);
            double dev = Math.Abs(((best.Az - direct) % 360 + 540) % 360 - 180);
            bool south = aLat < 0;
            sb.AppendLine(south ? Lang.F("QTF tipico verso S-SE (emisfero sud; in 50 MHz fino a circa 2000 km sulle tratte est-ovest, ANVDG [55]); scarto dal percorso diretto {0:F0}°", dev)
                                : Lang.F("QTF tipico tra N e NE (0-50°), che si sposta verso N durante l'evento (G3WPO, G0ISW); scarto dal percorso diretto {0:F0}°", dev));
            sb.AppendLine(Lang.T("Doppler fino a 2 kHz in 144 MHz, minore in 50 MHz (G3WPO); i 50 MHz sono favoriti rispetto ai 144 (G0ISW). FT8 spesso non decodifica: meglio CW/SSB o Q65 con sottomodi larghi (K7CW; Q65-30D in 144 MHz, PE1ITR [61]); il segnale è largo e spostato, serve il RIT o lo split."));
            sb.AppendLine(Lang.F("Scatter point preferito a {0:F0}-{1:F0} km da ciascuna stazione, quota circa 110 km, perpendicolarità entro ±3° (Lange-Hesse).", AuMinRangeKm, AuMaxRangeKm));
            // peso orario (Reeve [52]): la maggior parte delle riflessioni aurorali cade entro circa 4 ore dalla mezzanotte solare locale
            double lst = ((DateTime.UtcNow.TimeOfDay.TotalHours + best.Lon / 15.0) % 24 + 24) % 24;
            double fromMidnight = Math.Min(lst, 24 - lst);
            string hh = TimeSpan.FromHours(lst).ToString(@"hh\:mm");
            sb.AppendLine(fromMidnight <= 4
                ? Lang.F("Ora solare nello scatter point {0}: dentro le 4 ore dalla mezzanotte locale, la fascia con la maggior parte delle riflessioni aurorali (Reeve [52]).", hh)
                : Lang.F("Ora solare nello scatter point {0}: fuori dalle 4 ore dalla mezzanotte locale (Reeve [52]); l'aurora resta possibile nel pomeriggio e in serata durante le tempeste (G3WPO).", hh));
            if (!InAuRange(best, bLat, bLon)) sb.AppendLine(Lang.T("Lo scatter point proposto è fuori dall'intervallo 370-1100 km di Lange-Hesse per almeno una stazione: è possibile, ma meno probabile."));
        }

        // aurora-E: sul percorso diretto (forward scatter), senza Doppler, segnali piu' deboli e stabili dopo l'aurora
        // (G0ISW [33], SM2CEW-SM0FOB [54]); si disegna la tratta diretta
        void AuroraESpots(StringBuilder sb)
        {
            List<Spot> l = SpotStore.Recent(Filters.AuMinutes, s => s.HasBoth && s.IsAuroraE);
            if (l.Count == 0) return;
            sb.AppendLine();
            sb.AppendLine(Lang.F("Spot di aurora-E (Au-E) sul percorso diretto, segnali più deboli e stabili dopo l'aurora (G0ISW [33], SM2CEW-SM0FOB [54]): {0}", l.Count));
            foreach (Spot s in l.GetRange(0, Math.Min(30, l.Count)))
            {
                map.Lines.Add(MapView.GreatCircle(s.LatS, s.LonS, s.LatD, s.LonD, Pal.A(220, Pal.Green), 1.6));
                sb.AppendLine(Lang.F("  {0} {1} - {2} {3} MHz, {4:F0} km", s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture) + "Z", s.Spotter, s.DxCall, s.FreqMHz.ToString("F3", CultureInfo.CurrentCulture), s.DistanceKm));
            }
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("verde: aurora-E (Au-E) sul percorso diretto"), Pal.Green));
        }
    }

    // Riquadro dei QTF (stile Aurora QTF): ascissa = azimut centrato sulla direzione media, ordinata = elevazione
    public class QtfChart : Control
    {
        readonly List<double[]> pts = new List<double[]>();   // azA, elA, azB, elB, dev
        public bool HasData { get { return pts.Count > 0; } }
        public void Clear() { pts.Clear(); }
        public void Add(double azA, double elA, double azB, double elB, double dev) { pts.Add(new[] { azA, elA, azB, elB, dev }); }

        static double Mean(List<double[]> p, int k)
        {
            double sx = 0, sy = 0;
            foreach (double[] v in p) { sx += Math.Sin(v[k] * Geo.D2R); sy += Math.Cos(v[k] * Geo.D2R); }
            return Geo.Norm360(Math.Atan2(sx, sy) * Geo.R2D);
        }

        public override void Render(DrawingContext ctx)
        {
            double W = Bounds.Width, H = Bounds.Height;
            ctx.FillRectangle(Pal.B(Color.FromArgb(225, 248, 248, 252)), new Rect(0, 0, W, H));
            ctx.DrawRectangle(null, Pal.P(Colors.Gray, 1), new Rect(0, 0, W, H));
            if (pts.Count == 0) return;
            double cA = Mean(pts, 0), cB = Mean(pts, 2);
            double c = Geo.Norm360(Math.Atan2(Math.Sin(cA * Geo.D2R) + Math.Sin(cB * Geo.D2R), Math.Cos(cA * Geo.D2R) + Math.Cos(cB * Geo.D2R)) * Geo.R2D);
            c = Math.Round(c / 30) * 30;
            double maxEl = 10;
            foreach (double[] v in pts) maxEl = Math.Max(maxEl, Math.Max(v[1], v[3]));
            maxEl = Math.Min(90, Math.Ceiling(maxEl / 5) * 5);
            double l = 28, r = W - 8, t = 22, b = H - 20;
            Func<double, double> X = az => { double d = az - c; while (d > 180) d -= 360; while (d < -180) d += 360; return l + (d + 90) / 180 * (r - l); };
            Func<double, double> Y = el => b - Math.Max(0, el) / maxEl * (b - t);
            IPen gp = Pal.P(Color.FromArgb(70, 0, 0, 0), 0.6, Pal.Dot);
            Typeface tf = new Typeface(FontFamily.Default);
            for (int k = -90; k <= 90; k += 30)
            {
                double x = l + (k + 90) / 180.0 * (r - l);
                ctx.DrawLine(gp, new Point(x, t), new Point(x, b));
                FormattedText ft = new FormattedText(Geo.Norm360(c + k).ToString("F0", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, tf, 10, Brushes.Black);
                ctx.DrawText(ft, new Point(x - ft.Width / 2, b + 3));
            }
            for (double e = 0; e <= maxEl; e += 5)
            {
                ctx.DrawLine(gp, new Point(l, Y(e)), new Point(r, Y(e)));
                FormattedText ft = new FormattedText(e.ToString("F0", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, tf, 10, Brushes.Black);
                ctx.DrawText(ft, new Point(l - ft.Width - 3, Y(e) - ft.Height / 2));
            }
            ctx.DrawRectangle(null, Pal.P(Colors.Black, 0.8), new Rect(l, t, r - l, b - t));
            List<double[]> sorted = new List<double[]>(pts);
            sorted.Sort((p, q) => q[4].CompareTo(p[4]));   // prima i peggiori, sopra i migliori
            foreach (double[] v in sorted)
            {
                ctx.DrawEllipse(Pal.B(Dev(v[4], true)), null, new Point(X(v[2]), Y(v[3])), 3.5, 3.5);
                ctx.DrawEllipse(Pal.B(Dev(v[4], false)), null, new Point(X(v[0]), Y(v[1])), 4.5, 4.5);
            }
            FormattedText ta = new FormattedText(Lang.T("QTF A: rosso"), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily.Default, FontStyle.Normal, FontWeight.Bold), 11, Brushes.Red);
            ctx.DrawText(ta, new Point(l + 4, 4));
            FormattedText tb = new FormattedText(Lang.T("QTF B: verde"), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily.Default, FontStyle.Normal, FontWeight.Bold), 11, new SolidColorBrush(Color.FromRgb(20, 160, 40)));
            ctx.DrawText(tb, new Point(r - tb.Width - 4, 4));
        }

        static Color Dev(double d, bool green)
        {
            byte a = d <= 1 ? (byte)235 : d <= 1.5 ? (byte)170 : d <= 2 ? (byte)110 : (byte)60;
            return green ? Color.FromArgb(a, 30, 200, 50) : Color.FromArgb(a, 240, 20, 20);
        }
    }
}
