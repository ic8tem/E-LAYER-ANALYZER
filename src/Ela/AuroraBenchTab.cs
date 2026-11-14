// E-LAYER ANALYZER - sottolinguetta BENCHMARK AURORA: banco di prova del modello di calcolo dell'aurora su QSO reali.
// I dati stanno in data/aurora_benchmark.txt (log pubblicati di EI5FK, IO51RT, 2003-2005, e la mappa del log di PA0O,
// JO33HG). Per ogni QSO si calcola, con lo stesso modello della linguetta AURORA (IGRF-14, condizione di Bragg sul campo,
// quota 110 km, rifrazione 4/3, tolleranza 3 gradi), la scia degli scatter point possibili e il punto piu' probabile;
// il riepilogo dice quanti QSO il modello spiega, quanti cadono nella finestra di Lange-Hesse (mobile, spostata con
// l'inclinazione IGRF-14 di ogni stazione, e fissa 370-1100 km per confronto) e quanti superano i 2000 km. I log non riportano il puntamento: il QTF calcolato non si confronta con un QTF dichiarato.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using YAMS;

namespace ELA
{
    public class AuroraBenchTab
    {
        class BSet { public string Call, Loc, Date, Source; public double MHz, Lat, Lon; public List<BQso> Qso = new List<BQso>(); public bool Done; }
        class BQso
        {
            public BSet Set; public string Time, Call, Loc; public double Lat, Lon, Km, DeclKm;
            public PerpPoint Best; public double QtfB, ElB, DistB; public int Cells; public bool InRange, InFixed;
        }

        public readonly Control Root;
        readonly MapView map = new MapView { Mercator = true };
        readonly ComboBox cmbSet;
        readonly NumericUpDown numH;
        readonly TableView tQso;
        readonly SelectableTextBlock txtSum, txtDet;
        readonly List<BSet> sets = new List<BSet>();
        BQso sel;
        bool busy;
        const double Tol = 3.0, K = 4.0 / 3.0;

        public AuroraBenchTab()
        {
            Load();
            List<string> names = new List<string> { Lang.F("Tutti i log ({0} QSO)", sets.Sum(s => s.Qso.Count)) };
            foreach (BSet s in sets) names.Add(s.Call + " " + s.Loc + "  " + (s.Date == "?" ? Lang.T("data non nota") : s.Date) + "  " + s.MHz.ToString("F0", CultureInfo.InvariantCulture) + " MHz  (" + s.Qso.Count + ")");
            cmbSet = Ui.Combo(names, 0, 300);
            cmbSet.SelectionChanged += (s, e) => { sel = null; Run(); };
            numH = Ui.N(80, 160, 110, 0, 1, 45);
            numH.ValueChanged += (s, e) => { foreach (BSet b in sets) b.Done = false; Run(); };

            tQso = new TableView();
            tQso.Columns.AddRange(new[] { Lang.T("Giornata"), "UTC", Lang.T("Nominativo"), "WWL", "km", "Scatter", "dev°", "QTF", "el°", Lang.T("QTF corr."), Lang.T("Esito") });
            tQso.ColumnWidths.AddRange(new double[] { 78, 42, 78, 58, 44, 58, 38, 38, 34, 62, 120 });
            tQso.RowClicked += i => { if (i >= 0 && i < tQso.Rows.Count) { sel = tQso.Rows[i].Tag as BQso; Draw(); Details(); } };
            txtSum = Ui.Text(12.5);
            txtDet = Ui.Text(12.5);

            DockPanel right = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Log")), cmbSet), Dock.Top);
            Ui.Dock(right, Ui.Row(Ui.L(Lang.T("Quota km")), numH, Ui.L(Lang.T("tolleranza 3°, rifrazione 4/3, IGRF-14"))), Dock.Top);
            Ui.Dock(right, Ui.Box(Lang.T("Riepilogo"), Pal.Green, new ScrollViewer { Content = txtSum, Height = 190 }), Dock.Top);
            Ui.Dock(right, Ui.Box(Lang.T("Dettagli del QSO scelto"), Pal.Azure, new ScrollViewer { Content = txtDet, Height = 150 }), Dock.Bottom);
            right.Children.Add(Ui.Box(Lang.T("QSO di aurora (dati del banco di prova)"), Pal.Azure, tQso));

            map.SetView(-25, 35, 40, 68);
            Root = Ui.MapAndData(map, right);
            Run();
        }

        static double[] LL(string loc) { double la, lo; return Geo.LocToLatLon(loc, out la, out lo) ? new[] { la, lo } : null; }

        void Load()
        {
            string f = Path.Combine(AppPaths.DataDir, "aurora_benchmark.txt");
            if (!File.Exists(f)) f = Path.Combine(AppContext.BaseDirectory, "data", "aurora_benchmark.txt");
            if (!File.Exists(f)) return;
            BSet cur = null;
            foreach (string raw in File.ReadAllLines(f))
            {
                string l = raw.Trim();
                if (l.Length == 0) continue;
                if (l.StartsWith("#"))
                {
                    string[] p = l.Substring(1).Trim().Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);
                    if (p.Length < 4 || p[0].Contains("E-LAYER") || p[0].StartsWith("Riga") || p[0].StartsWith("Locator")) { continue; }
                    double[] ll = LL(p[1]);
                    double mhz;
                    if (ll == null || !double.TryParse(p[3], NumberStyles.Float, CultureInfo.InvariantCulture, out mhz)) { cur = null; continue; }
                    cur = new BSet { Call = p[0], Loc = p[1], Date = p[2], MHz = mhz, Lat = ll[0], Lon = ll[1], Source = p.Length > 4 ? p[4] : "" };
                    sets.Add(cur);
                    continue;
                }
                if (cur == null) continue;
                string[] q = l.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (q.Length < 3) continue;
                double[] c = LL(q[2]);
                if (c == null) continue;
                double decl = 0;
                if (q.Length > 3) double.TryParse(q[3], NumberStyles.Float, CultureInfo.InvariantCulture, out decl);
                cur.Qso.Add(new BQso { Set = cur, Time = q[0], Call = q[1], Loc = q[2], Lat = c[0], Lon = c[1], DeclKm = decl, Km = Geo.DistanceKm(cur.Lat, cur.Lon, c[0], c[1]) });
            }
        }

        List<BSet> Shown()
        {
            int i = cmbSet.SelectedIndex;
            return i <= 0 ? sets : new List<BSet> { sets[i - 1] };
        }

        // calcolo in background (stesso punteggio degli spot di aurora della linguetta AURORA, senza l'ovale)
        async void Run()
        {
            if (busy) return;
            List<BSet> todo = Shown().Where(s => !s.Done).ToList();
            if (todo.Count > 0)
            {
                busy = true;
                txtSum.Text = Lang.T("Calcolo in corso…");
                double h = Ui.V(numH);
                await Task.Run(() => { foreach (BSet s in todo) { Compute(s, h); s.Done = true; } });
                busy = false;
            }
            Fill(); Draw(); Details();
        }

        static void Compute(BSet s, double h)
        {
            using (Igrf.At(Igrf.YearOf(s.Date))) ComputeAt(s, h);   // campo IGRF dell'anno del QSO (data non nota: campo di oggi)
        }

        static void ComputeAt(BSet s, double h)
        {
            double maxD = Geo.HalfHopKm(0, h, K);
            foreach (BQso q in s.Qso)
            {
                q.Best = null; q.InRange = false; q.InFixed = false; q.Cells = 0;
                List<PerpPoint> loc = FieldAligned.PairLocus(s.Lat, s.Lon, q.Lat, q.Lon, h, Tol, 0, maxD, 0.5, K);
                q.Cells = loc.Count;
                double best = double.MaxValue, bestF = double.MaxValue;
                foreach (PerpPoint p in loc)
                {
                    double dB = Geo.DistanceKm(q.Lat, q.Lon, p.Lat, p.Lon);
                    bool inR = ScatterTab.InAuWindow(s.Lat, s.Lon, p.DistKm, q.Lat, q.Lon, dB), inF = ScatterTab.InAuFixed(p.DistKm, dB);
                    double sc0 = (p.Off <= 1 ? 0 : 100 + p.Off * 10) + (p.DistKm * p.DistKm + dB * dB) / 1e6;
                    if (sc0 + (inR ? 0 : 3) < best) { best = sc0 + (inR ? 0 : 3); q.Best = p; q.InRange = inR; q.DistB = dB; }
                    // stesso calcolo con la finestra fissa di Lange-Hesse, solo per il confronto nel riepilogo
                    if (sc0 + (inF ? 0 : 3) < bestF) { bestF = sc0 + (inF ? 0 : 3); q.InFixed = inF; }
                }
                if (q.Best != null)
                {
                    q.QtfB = Geo.Bearing(q.Lat, q.Lon, q.Best.Lat, q.Best.Lon);
                    q.ElB = Geo.ElevationDeg(q.DistB, h, 0, K);
                    q.Best.Elev = Geo.ElevationDeg(q.Best.DistKm, h, 0, K);
                }
            }
        }

        string Outcome(BQso q)
        {
            if (q.Best == null) return Lang.T("non spiegato");
            if (q.Km > ScatterTab.AuMaxKm) return Lang.T("spiegato, oltre 2000 km");
            return q.InRange ? Lang.T("spiegato, nella finestra") : Lang.T("spiegato, fuori finestra");
        }

        void Fill()
        {
            List<BQso> all = Shown().SelectMany(s => s.Qso).ToList();
            tQso.Rows.Clear();
            foreach (BQso q in all)
            {
                bool ok = q.Best != null;
                tQso.Rows.Add(new TableRow
                {
                    Cells = new[] { q.Set.Date, q.Time, q.Call, q.Loc, q.Km.ToString("F0", CultureInfo.InvariantCulture),
                        ok ? Geo.LatLonToLoc(q.Best.Lat, q.Best.Lon, 4) : "-", ok ? q.Best.Off.ToString("F1", CultureInfo.CurrentCulture) : "-",
                        ok ? q.Best.Az.ToString("F0", CultureInfo.InvariantCulture) : "-", ok ? q.Best.Elev.ToString("F1", CultureInfo.CurrentCulture) : "-",
                        ok ? q.QtfB.ToString("F0", CultureInfo.InvariantCulture) : "-", Outcome(q) },
                    Fore = !ok ? Pal.Red : q.Km > ScatterTab.AuMaxKm ? Pal.Orange : q.InRange ? Pal.Green : Pal.Yellow,
                    Tag = q
                });
            }
            tQso.SelectedIndex = sel != null ? all.IndexOf(sel) : -1;
            tQso.Refresh();

            int n = all.Count, ok2 = all.Count(q => q.Best != null), inr = all.Count(q => q.Best != null && q.InRange), inf = all.Count(q => q.Best != null && q.InFixed), far = all.Count(q => q.Km > ScatterTab.AuMaxKm);
            StringBuilder sb = new StringBuilder();
            if (n == 0) { txtSum.Text = Lang.T("Nessun dato: manca il file data/aurora_benchmark.txt."); return; }
            sb.AppendLine(Lang.F("QSO: {0}; spiegati dal modello (scatter point entro 3°): {1} ({2:F0}%)", n, ok2, 100.0 * ok2 / n));
            sb.AppendLine(Lang.F("con lo scatter point nella finestra di Lange-Hesse di entrambe le stazioni, spostata con l'inclinazione IGRF-14: {0} ({1:F0}%)", inr, 100.0 * inr / n));
            sb.AppendLine(Lang.F("con la finestra fissa 370-1100 km (Lange-Hesse, 54° geomagnetici), per confronto: {0} ({1:F0}%)", inf, 100.0 * inf / n));
            sb.AppendLine(Lang.F("oltre i 2000 km (dubbi per il programma): {0}", far));
            List<BQso> sol = all.Where(q => q.Best != null).ToList();
            if (sol.Count > 0)
            {
                foreach (BSet s in Shown())
                {
                    List<double> az = s.Qso.Where(q => q.Best != null).Select(q => q.Best.Az).OrderBy(a => a).ToList();
                    if (az.Count == 0) continue;
                    sb.AppendLine(Lang.F("{0} {1} ({2}): QTF calcolato della stazione da {3:F0}° a {4:F0}°, mediana {5:F0}°", s.Call, s.Date == "?" ? Lang.T("data non nota") : s.Date, s.Qso.Count, az.First(), az.Last(), az[az.Count / 2]));
                }
                sb.AppendLine(Lang.F("Distanza dichiarata e calcolata dai locator: scarto massimo {0:F0} km.", all.Where(q => q.DeclKm > 0).Select(q => Math.Abs(q.DeclKm - q.Km)).DefaultIfEmpty(0).Max()));
            }
            sb.AppendLine(Lang.T("I log non riportano il puntamento: il confronto è sulla geometria (esiste lo scatter point?), non sul QTF."));
            sb.AppendLine(Igrf.HasHistory
                ? Lang.T("Campo magnetico: IGRF-14 dell'anno di ciascun QSO (epoche storiche 1900-2030); con la data non nota, il campo di oggi.")
                : Lang.T("Campo magnetico: quello di oggi (manca il file data/igrf14_storico.csv)."));
            foreach (BSet s in Shown()) sb.AppendLine(Lang.F("Fonte: {0} {1}, {2}", s.Call, s.Loc, s.Source));
            txtSum.Text = sb.ToString();
        }

        void Draw()
        {
            map.Cells.Clear(); map.Lines.Clear(); map.Markers.Clear(); map.Legend.Clear(); map.Info.Clear(); map.Polys.Clear();
            foreach (BSet s in Shown())
            {
                foreach (BQso q in s.Qso)
                {
                    bool isSel = q == sel;
                    Color c = q.Best == null ? Pal.Red : q.Km > ScatterTab.AuMaxKm ? Pal.Orange : q.InRange ? Pal.Green : Pal.Yellow;
                    map.Markers.Add(new MapMarker { Lat = q.Lat, Lon = q.Lon, Radius = isSel ? 5 : 3, Fill = Pal.Azure, Label = isSel ? q.Call : null });
                    if (q.Best != null) map.Markers.Add(new MapMarker { Lat = q.Best.Lat, Lon = q.Best.Lon, Radius = isSel ? 7 : 3.5, Fill = c, Ring = isSel });
                    if (isSel)
                    {
                        map.Lines.Add(MapView.GreatCircle(s.Lat, s.Lon, q.Lat, q.Lon, Pal.A(140, Pal.Silver), 1, true));
                        if (q.Best != null)
                        {
                            map.Lines.Add(MapView.GreatCircle(s.Lat, s.Lon, q.Best.Lat, q.Best.Lon, Pal.Orange, 2.2));
                            map.Lines.Add(MapView.GreatCircle(q.Lat, q.Lon, q.Best.Lat, q.Best.Lon, Pal.Orange, 2.2));
                        }
                    }
                }
                map.Markers.Add(new MapMarker { Lat = s.Lat, Lon = s.Lon, Radius = 6, Fill = Pal.Green, Square = true, Label = s.Call, LabelColor = Pal.Green });
            }
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("● azzurro: corrispondenti; ■ verde: stazione del log"), Pal.Azure));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("scatter point: verde nella finestra di Lange-Hesse, giallo fuori finestra, arancio QSO oltre 2000 km"), Pal.Green));
            map.Legend.Add(new KeyValuePair<string, Color>(Lang.T("rosso: nessuno scatter point entro 3° (QSO non spiegato dal modello)"), Pal.Red));
            map.InvalidateVisual();
        }

        void Details()
        {
            if (sel == null) { txtDet.Text = Lang.T("Clic su un QSO della tabella."); return; }
            BQso q = sel;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Lang.F("{0} {1}Z  {2} {3} - {4} {5}, {6:F0} km (dichiarati {7:F0})", q.Set.Date, q.Time, q.Set.Call, q.Set.Loc, q.Call, q.Loc, q.Km, q.DeclKm));
            if (q.Best == null) sb.AppendLine(Lang.T("Il modello non trova scatter point entro 3°: QSO non spiegato (tratta troppo corta o geometria sfavorevole)."));
            else
            {
                sb.AppendLine(Lang.F("Scatter point {0} (dev. {1:F1}°), celle del luogo {2}", Geo.LatLonToLoc(q.Best.Lat, q.Best.Lon, 6), q.Best.Off, q.Cells));
                sb.AppendLine(Lang.F("  {0}: QTF {1:F0}° el {2:F1}°, {3:F0} km", q.Set.Call, q.Best.Az, q.Best.Elev, q.Best.DistKm));
                sb.AppendLine(Lang.F("  {0}: QTF {1:F0}° el {2:F1}°, {3:F0} km", q.Call, q.QtfB, q.ElB, q.DistB));
                sb.AppendLine(Lang.F("Finestra di Lange-Hesse: {0}; {1}", ScatterTab.AuWindowText(q.Set.Call, q.Set.Lat, q.Set.Lon), ScatterTab.AuWindowText(q.Call, q.Lat, q.Lon)));
                sb.AppendLine(Lang.F("Percorso diretto {0:F0}°: scarto del QTF {1:F0}°", Geo.Bearing(q.Set.Lat, q.Set.Lon, q.Lat, q.Lon), Math.Abs(Geo.Norm180(q.Best.Az - Geo.Bearing(q.Set.Lat, q.Set.Lon, q.Lat, q.Lon)))));
            }
            txtDet.Text = sb.ToString();
        }

        public void ShotSelect() { if (tQso.Rows.Count > 0) { sel = tQso.Rows[0].Tag as BQso; Draw(); Details(); } }
        public bool Busy { get { return busy; } }
        public string SummaryText { get { return txtSum.Text ?? ""; } }
    }
}
