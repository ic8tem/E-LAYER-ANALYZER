// YA-MS - finestra "Giappone: conteggi HRO mensili (IPRMO)"
// Legge i file mensili RMOJ di IPRMO (osservatori che contano gli echi dei fari HAM a 53 MHz: JA2SDA, JE3AKE, JJ1RLQ)
// e li mostra con la stessa logica di Collm: media per ora UTC del mese, traslata alla frequenza di arrivo con λ^k e K,
// piu' i totali giornalieri. Serve per la taratura e lo storico del Giappone: i file escono con settimane di ritardo.
// (richiesta dell'autore, IC8TEM, 22/11/2026)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        async Task ShowJapanHro()
        {
            Window w = new Window
            {
                Title = Lang.T("Giappone: conteggi HRO mensili (IPRMO)"), Width = 1100, Height = 760,
                WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = Pal.B(Pal.Back), Foreground = Pal.B(Pal.Fore)
            };
            ComboBox cmbMonth = new ComboBox { MinWidth = 120, Margin = new Thickness(2, 3), VerticalAlignment = VerticalAlignment.Center };
            ComboBox cmbObs = new ComboBox { MinWidth = 330, Margin = new Thickness(2, 3), VerticalAlignment = VerticalAlignment.Center };
            Button btnDl = B(Lang.T("Aggiorna da IPRMO"));
            TextBlock lblState = L("");
            TextBlock lblInfo = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4, 6, 4) };
            TextBlock lblNote = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 2, 6, 6), Foreground = Pal.B(Pal.Muted),
                Text = Lang.T("Conteggi a mano degli osservatori giapponesi (HROFFT) sugli echi dei fari HAM per l'osservazione delle meteore, pubblicati da IPRMO ogni mese con alcune settimane di ritardo: servono per la taratura e lo storico, non per la previsione in diretta. Ogni osservatore ha antenna e ricevitore diversi: confronta i mesi dello stesso osservatore, non osservatori diversi. Traslazione con le regole di SM6FHZ (λ^k, fattore K) impostate in questa linguetta.")
            };
            SimpleChart chH = new SimpleChart(), chD = new SimpleChart();
            Grid g = new Grid { RowDefinitions = new RowDefinitions("55*,45*") };
            Grid.SetRow(chH, 0); Grid.SetRow(chD, 1);
            g.Children.Add(chH); g.Children.Add(chD);
            DockPanel p = new DockPanel { Margin = new Thickness(8) };
            DockTop(p, Row(L(Lang.T("Mese")), cmbMonth, L(Lang.T("Osservatore")), cmbObs, btnDl, lblState));
            DockTop(p, lblInfo);
            DockTop(p, lblNote);
            p.Children.Add(g);
            w.Content = p;

            List<DateTime> months = new List<DateTime>();
            List<RmojObserver> obs = new List<RmojObserver>();
            DateTime curMonth = DateTime.MinValue;

            Action draw = () =>
            {
                chH.Series.Clear(); chD.Series.Clear();
                RmojObserver o = cmbObs.SelectedIndex >= 0 && cmbObs.SelectedIndex < obs.Count ? obs[cmbObs.SelectedIndex] : null;
                if (o == null || curMonth == DateTime.MinValue)
                {
                    chH.Title = Lang.T("Nessun dato: premi Aggiorna da IPRMO."); lblInfo.Text = "";
                    chH.InvalidateVisual(); chD.InvalidateVisual(); return;
                }
                int y = curMonth.Year, mo = curMonth.Month;
                double f = PrevFreqMHz, k = V(numPrevK), K = V(numPrevStation);
                double scale = MsGeometry.CountScale(o.FreqMHz, f, k) * K;
                double[] hm = Rmoj.HourlyMean(o, y, mo), tr = new double[24];
                string[] xl = new string[24];
                for (int h = 0; h < 24; h++) { xl[h] = h.ToString("00"); tr[h] = hm[h] < 0 ? -1 : hm[h] * scale; }
                ChartSeries a = new ChartSeries { Name = Lang.F("{0} - {1:F3} MHz", o.Name, o.FreqMHz), Color = Pal.Ink(Pal.Rgb(0, 230, 255)), Values = hm, Bars = true };
                ChartSeries b = new ChartSeries { Name = Lang.F("traslato a {0} MHz (scala a destra)", f.ToString("F0")), Color = Pal.Ink(Pal.Rgb(255, 170, 40)), Values = tr, RightAxis = true };
                chH.Series.Add(a); chH.Series.Add(b);
                chH.XLabels = xl; chH.XLabelEvery = 1;
                chH.Title = Lang.F("Media del mese per ora UTC - {0}", curMonth.ToString("MM/yyyy", CultureInfo.InvariantCulture));
                chH.YLabel = Lang.T("echi/ora"); chH.Y2Label = Lang.F("riflessioni/ora a {0} MHz", f.ToString("F0"));
                double[] dt = Rmoj.DailyTotals(o, y, mo);
                string[] dl = new string[dt.Length];
                for (int d = 0; d < dt.Length; d++) dl[d] = (d + 1).ToString("00");
                chD.Series.Add(new ChartSeries { Name = Lang.T("echi al giorno (UTC)"), Color = Pal.Ink(Pal.Rgb(120, 230, 120)), Values = dt, Bars = true });
                chD.XLabels = dl; chD.XLabelEvery = 2;
                chD.Title = Lang.T("Totale giornaliero (giorni con almeno 20 ore di dati, riportati a 24 ore)");
                chD.YLabel = Lang.T("echi/giorno");
                string path = "";
                if (o.HasPath)
                {
                    LatLon rx = new LatLon(o.Lat, o.Lon), tx = new LatLon(o.BeaconLat, o.BeaconLon);
                    double km = Astro.DistanceKm(tx, rx);
                    LatLon mid = Astro.Destination(tx, Astro.Bearing(tx, rx), km / 2);
                    path = Lang.F("Tratta faro → osservatore {0:F0} km, punto medio {1}.", km, Astro.ToLocator(mid));
                }
                lblInfo.Text = Lang.F("Osservatore: {0} ({1}). Faro: {2} {3} ({4:F3} MHz). Tabella in {5}, convertita in UTC.", o.Name, o.Location,
                                      o.Beacon.Length > 0 ? o.Beacon : "?", o.BeaconPlace, o.FreqMHz, o.Tz) + " " + path + "\n" +
                               Lang.F("Ore valide {0}, di cui stimate (meno di 60 minuti) {1}; ore perse o disturbate {2}; ore con sporadico E escluse {3}.",
                                      o.Counts.Count, o.Estimated, o.Missing, o.Sporadic) + " " +
                               Lang.F("Traslazione: x ({0:F3}/{1:F0})^{2:F2} x K {3:F2} = x {4:F4}.", o.FreqMHz, f, k, K, scale);
                chH.InvalidateVisual(); chD.InvalidateVisual();
            };

            Action loadMonth = () =>
            {
                obs = new List<RmojObserver>(); curMonth = DateTime.MinValue;
                if (cmbMonth.SelectedIndex >= 0 && cmbMonth.SelectedIndex < months.Count)
                {
                    curMonth = months[cmbMonth.SelectedIndex];
                    try { obs = Rmoj.Parse(Rmoj.FileFor(dataDir, curMonth.Year, curMonth.Month), curMonth.Year, curMonth.Month); }
                    catch (Exception ex) { lblState.Text = ex.Message; }
                }
                List<string> names = new List<string>();
                foreach (RmojObserver o in obs) names.Add(o.ToString() + (o.Beacon.Length > 0 ? " (" + o.Beacon + ")" : ""));
                string prevSel = cfg.Get("rmoj_osservatore", "");
                cmbObs.ItemsSource = names;
                int sel = obs.FindIndex(o => o.Name == prevSel);
                cmbObs.SelectedIndex = sel >= 0 ? sel : (obs.Count > 0 ? 0 : -1);
                draw();
            };

            Action<List<DateTime>> setMonths = list =>
            {
                months = list;
                List<string> labels = new List<string>();
                foreach (DateTime m in months) labels.Add(m.ToString("MM/yyyy", CultureInfo.InvariantCulture));
                cmbMonth.ItemsSource = labels;
                cmbMonth.SelectedIndex = months.Count > 0 ? 0 : -1;
                loadMonth();
            };

            cmbMonth.SelectionChanged += (s, e) => loadMonth();
            cmbObs.SelectionChanged += (s, e) =>
            {
                if (cmbObs.SelectedIndex >= 0 && cmbObs.SelectedIndex < obs.Count) cfg.Set("rmoj_osservatore", obs[cmbObs.SelectedIndex].Name);
                draw();
            };
            Func<Task> download = async () =>
            {
                btnDl.IsEnabled = false; lblState.Text = Lang.T("scarico...");
                string msg = "";
                List<DateTime> list = await Task.Run(() => Rmoj.Update(dataDir, 12, out msg));
                lblState.Text = msg; btnDl.IsEnabled = true;
                setMonths(list);
            };
            btnDl.Click += async (s, e) => await download();

            setMonths(Rmoj.OnDisk(dataDir));
            Window owner = TopLevel.GetTopLevel(this) as Window;
            if (owner != null) w.Show(owner); else w.Show();
            if (months.Count == 0) await download();
        }
    }
}
