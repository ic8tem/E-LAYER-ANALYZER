// YA-MS - linguetta "Attuazione sperimentale previsioni"
// Traslazione del flusso del radar di Collm (36,2 MHz) a una frequenza radioamatoriale (default 144 MHz)
// con le regole sperimentali di SM6FHZ (numero ~ lambda^k con k = 3, durata ~ lambda^2),
// fattore di stazione K e confronto facoltativo con un file RMOB (es. un osservatore GRAVES 143,050 MHz).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace YAMS
{
    public partial class MainWindow
    {
        NumericUpDown numPrevFreq, numPrevK, numPrevStation;
        SimpleChart chartPrev48, chartPrev24;
        TableView gridPrev;
        TextBlock lblPrevNote, lblRmob;
        Dictionary<DateTime, double> rmobCounts;
        string rmobName = "";
        TextBox txtRmobObs;
        CheckBox chkRmobAuto;
        DateTime rmobLastMod = DateTime.MinValue;
        public const string RmobUrlDefault = "https://rmob.org/index.php/livedata/live_datas/{NOME}_{MMAAAA}rmob.TXT";

        // Indirizzo del file di testo RMOB del mese in corso per l'osservatore scelto (modificabile in config.ini, chiave rmob_url)
        string RmobUrl(DateTime utc)
        {
            return cfg.Get("rmob_url", RmobUrlDefault).Replace("{NOME}", (txtRmobObs.Text ?? "").Trim())
                      .Replace("{MMAAAA}", utc.ToString("MMyyyy", CultureInfo.InvariantCulture));
        }

        string RmobFile(DateTime utc)
        {
            string safe = Regex.Replace((txtRmobObs.Text ?? "").Trim(), @"[^A-Za-z0-9_\-]", "_");
            return Path.Combine(Path.Combine(dataDir, "radar"), "rmob_" + safe + "_" + utc.ToString("MMyyyy", CultureInfo.InvariantCulture) + ".txt");
        }

        void BuildPrevTab()
        {
            DockPanel p = Page(Lang.T("Attuazione sperimentale previsioni"));
            numPrevFreq = N(28, 1300, Dm(cfg.GetD("prev_frequenza_MHz", 144)), 0, 1, 65);
            numPrevK = N(1, 4, Dm(cfg.GetD("prev_esponente", 3)), 2, 0.05m, 60);
            numPrevStation = N(0.01m, 100, Dm(cfg.GetD("prev_fattore_stazione", 1)), 2, 0.1m, 60);
            Button btnRmob = B(Lang.T("Carica file RMOB dal disco..."));
            btnRmob.Click += async (s, e) => await LoadRmobFile();
            Button btnK = B(Lang.T("Ricava l'esponente dal file RMOB"));
            btnK.Click += async (s, e) => await FitExponentFromRmob();
            Button btnKr = B(Lang.T("k dagli indici di popolazione"));
            ToolTip.SetTip(btnKr, Lang.T("Calcola k = 3,75·log10 r per ogni sciame attivo adesso (r dalla tabella del GMN, altrimenti dal catalogo IMO) e ne fa la media pesata con lo ZHR.\n" +
                "Teoria: con indice di massa s = 1 + 2,5·log10 r, il numero di scie sopra una soglia fissa scala con λ^(1,5·(s − 1)) (IZ5TEK, OH5IY).\n" +
                "Il valore di partenza resta k = 3 (regola sperimentale di SM6FHZ), più prudente: con k più basso si prevedono più riflessioni alle frequenze alte.\n" +
                "Attenzione: i conteggi di Collm sono per lo più meteore sporadiche, di cui il GMN non pubblica r."));
            btnKr.Click += async (s, e) => await ExponentFromPopIndex();
            numPrevFreq.ValueChanged += (s, e) => UpdatePrevTab();
            numPrevK.ValueChanged += (s, e) => UpdatePrevTab();
            numPrevStation.ValueChanged += (s, e) => UpdatePrevTab();
            lblRmob = L(Lang.T("nessun file RMOB caricato"));
            DockTop(p, Row(L(Lang.T("Frequenza di arrivo MHz")), numPrevFreq, L(Lang.T("Esponente λ^k")), numPrevK,
                           L(Lang.T("Fattore di stazione K")), numPrevStation, btnRmob, btnK, btnKr, lblRmob));

            txtRmobObs = T(cfg.Get("rmob_osservatore", ""), 170, false);
            chkRmobAuto = C(Lang.T("prelievo automatico ogni 5 minuti (attivare solo con il permesso di RMOB)"), cfg.GetB("rmob_automatico", false));
            Button btnRmobNow = B(Lang.T("Scarica RMOB adesso"));
            btnRmobNow.Click += async (s, e) =>
            {
                if ((txtRmobObs.Text ?? "").Trim().Length == 0)
                {
                    await Dialogs.Message(Win, "RMOB", Lang.T("Scrivi il nome dell'osservatore RMOB come compare nel nome del suo file (es. NOME_112026rmob.TXT -> NOME)."));
                    return;
                }
                StartFetch(true);
            };
            // impostazioni di RMOB: nella linguetta Impostazioni (richiesta dell'autore, 14/11/2026)
            rmobRow = Row(L(Lang.T("Osservatore RMOB")), txtRmobObs, btnRmobNow, chkRmobAuto);

            lblPrevNote = new TextBlock { Foreground = Pal.B(Pal.OrangeText), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(6, 4, 6, 4) };
            DockTop(p, lblPrevNote);

            Grid tl = new Grid { ColumnDefinitions = new ColumnDefinitions("60*,40*"), RowDefinitions = new RowDefinitions("50*,50*") };
            chartPrev48 = new SimpleChart();
            chartPrev24 = new SimpleChart();
            gridPrev = MakeTable(new[] { Lang.T("Ora UTC"), Lang.T("Collm stessa ora"), Lang.T("Traslato"), Lang.T("Indice sciami tratta"), Lang.T("Previsione /ora"), Lang.T("Giudizio") },
                                 new double[] { 110, 120, 100, 140, 130, 80 });
            gridPrev.Selectable = false;
            Grid.SetColumn(chartPrev48, 0); Grid.SetRow(chartPrev48, 0);
            Grid.SetColumn(chartPrev24, 0); Grid.SetRow(chartPrev24, 1);
            Grid.SetColumn(gridPrev, 1); Grid.SetRow(gridPrev, 0); Grid.SetRowSpan(gridPrev, 2);
            tl.Children.Add(chartPrev48); tl.Children.Add(chartPrev24); tl.Children.Add(gridPrev);
            p.Children.Add(tl);
        }

        Border rmobRow;

        void SavePrevConfig()
        {
            if (numPrevFreq == null) return;
            cfg.Set("prev_frequenza_MHz", V(numPrevFreq));
            cfg.Set("prev_esponente", V(numPrevK));
            cfg.Set("prev_fattore_stazione", V(numPrevStation));
            cfg.Set("rmob_osservatore", (txtRmobObs.Text ?? "").Trim());
            cfg.Set("rmob_automatico", chkRmobAuto.IsChecked == true);
            if (cfg.Get("rmob_url", "") == "") cfg.Set("rmob_url", RmobUrlDefault);
        }

        double RadarFreq { get { return collmSrc != null && collmSrc.FreqMHz > 0 ? collmSrc.FreqMHz : 36.2; } }

        // frequenza di arrivo della previsione (MHz)
        double PrevFreqMHz { get { return numPrevFreq != null ? V(numPrevFreq) : 144; } }

        double Translate(double n36)
        {
            if (n36 < 0) return -1;
            return n36 * MsGeometry.CountScale(RadarFreq, V(numPrevFreq), V(numPrevK)) * V(numPrevStation);
        }

        void UpdatePrevTab()
        {
            if (chartPrev48 == null) return;
            double f = V(numPrevFreq), k = V(numPrevK), K = V(numPrevStation), fr = RadarFreq;
            double scale = MsGeometry.CountScale(fr, f, k);
            double dur = MsGeometry.DurationScale(fr, f);
            lblPrevNote.Text = Lang.F(
                "N({0:F0} MHz) = N(Collm {1:F1} MHz) x ({1:F1}/{0:F0})^{2:F2} x K = N x {3:F5} x {4:F2}.  Durata delle riflessioni x ({1:F1}/{0:F0})^2 = x {5:F4}: " +
                "un eco di 1 s a {1:F1} MHz dura circa {6:F0} ms a {0:F0} MHz (ping massimo IARU a {0:F0} MHz: {7:F0} ms).\n" +
                "Regole sperimentali di SM6FHZ (VUSHF 2025). Il fattore K tiene conto della stazione ricevente (potenza, antenne): si ricava confrontando con un osservatore RMOB sulla stessa banda.",
                f, fr, k, scale, K, Math.Pow(fr / f, 2), 1000 * Math.Pow(fr / f, 2), MsGeometry.PingMaxMs(f));
            if (MsGeometry.DurationObserved)
                lblPrevNote.Text += "\n" + Lang.F("Durata secondo le misure HamSCI MSQP (NN4SA, Perseidi 2026) invece di λ²: x {0:F3}, quindi un eco di 1 s a {1:F1} MHz dura circa {2:F0} ms a {3:F0} MHz.",
                    dur, fr, 1000 * dur, f);
            // forward scatter: la durata cresce anche con sec^2(phi) (McKinley; IZ5TEK; OH5IY), Collm misura in backscatter (phi = 0)
            if (model != null && model.Valid)
            {
                double phi = model.HalfScatterAngleDeg(model.Mid), sec2 = 1.0 / Math.Pow(Math.Cos(Math.Min(85, phi) * Astro.D2R), 2);
                lblPrevNote.Text += "\n" + Lang.F("In forward scatter la durata cresce anche con sec²φ (φ = {0:F0}° al punto medio della tratta; Collm misura in backscatter, φ = 0): x {1:F1}, quindi un eco di 1 s a Collm dura circa {2:F0} ms sulla tratta.",
                    phi, sec2, 1000 * dur * sec2);
            }

            chartPrev48.Series.Clear(); chartPrev24.Series.Clear();
            gridPrev.Columns = new List<string> { Lang.T("Ora UTC"), Lang.T("Collm stessa ora"), Lang.F("Traslato {0}", f.ToString("F0")), Lang.T("Indice sciami tratta"), Lang.F("Previsione {0} /ora", f.ToString("F0")), Lang.T("Giudizio") };
            gridPrev.Rows = new List<TableRow>();
            if (collm == null)
            {
                chartPrev48.Title = Lang.T("Nessun dato del radar: scaricalo dalla sottolinguetta Live e dati real-time › Flusso radar.");
                chartPrev48.InvalidateVisual(); chartPrev24.InvalidateVisual(); gridPrev.Refresh(); return;
            }
            DateTime start = collm.DayToday.AddDays(-1);
            string[] xl = new string[48];
            double[] tr = new double[48], rm = new double[48];
            bool anyRmob = false;
            for (int i = 0; i < 48; i++)
            {
                xl[i] = (i % 24).ToString("00");
                tr[i] = Translate(collm.Valid[i]);
                rm[i] = -1;
                double c;
                if (rmobCounts != null && rmobCounts.TryGetValue(start.AddHours(i), out c)) { rm[i] = c; anyRmob = true; }
            }
            ChartSeries a = new ChartSeries(); a.Name = "Collm " + fr.ToString("F1") + " MHz"; a.Color = Pal.Ink(Pal.Rgb(0, 230, 255)); a.Values = collm.Valid; a.Bars = true;
            ChartSeries b = new ChartSeries(); b.Name = Lang.F("traslato a {0} MHz (scala a destra)", f.ToString("F0")); b.Color = Pal.Ink(Pal.Rgb(255, 170, 40)); b.Values = tr; b.RightAxis = true;
            chartPrev48.Series.Add(a); chartPrev48.Series.Add(b);
            if (anyRmob)
            {
                ChartSeries r = new ChartSeries(); r.Name = Lang.F("RMOB {0} (scala a destra)", rmobName); r.Color = Pal.Ink(Pal.Rgb(120, 230, 120)); r.Values = rm; r.RightAxis = true;
                chartPrev48.Series.Add(r);
            }
            chartPrev48.XLabels = xl; chartPrev48.XLabelEvery = 3;
            SetLocalRow(chartPrev48, start, 48);
            chartPrev48.Title = Lang.F("Misurato a Collm e traslato a {0} MHz - ieri {1} e oggi {2} (UTC)", f.ToString("F0"),
                                start.ToString("dd/MM", CultureInfo.InvariantCulture), collm.DayToday.ToString("dd/MM", CultureInfo.InvariantCulture));
            chartPrev48.YLabel = Lang.T("meteore/ora radar"); chartPrev48.Y2Label = Lang.F("riflessioni/ora a {0} MHz", f.ToString("F0"));
            int nowIdx = (int)Math.Floor((DateTime.UtcNow - start).TotalHours);
            chartPrev48.MarkIndex = nowIdx >= 0 && nowIdx < 48 ? nowIdx : -1;
            chartPrev48.InvalidateVisual();

            // previsione per le prossime 24 ore: persistenza (stessa ora dell'ultimo giorno disponibile)
            // corretta con la variazione dell'attivita' degli sciami attesa per la tratta
            DateTime nu = DateTime.UtcNow;
            DateTime h0 = new DateTime(nu.Year, nu.Month, nu.Day, nu.Hour, 0, 0, DateTimeKind.Utc);
            double[] fc = new double[24], idx = new double[24];
            string[] xf = new string[24];
            double sum = 0; int nsum = 0;
            for (int i = 0; i < 24; i++)
            {
                DateTime t = h0.AddHours(i);
                xf[i] = t.Hour.ToString("00");
                double past = -1;
                for (int back = 1; back <= 2 && past < 0; back++)
                {
                    int j = (int)Math.Round((t.AddDays(-back) - start).TotalHours);
                    if (j >= 0 && j < 48 && collm.Valid[j] >= 0) past = collm.Valid[j];
                }
                idx[i] = ShowerIndex(t.AddMinutes(30));
                double idxPast = ShowerIndex(t.AddDays(-1).AddMinutes(30));
                double trp = Translate(past);
                // correzione: variazione dell'indice degli sciami rispetto al giorno prima (fondo sporadico invariato)
                double corr = (idx[i] + 10.0) / (idxPast + 10.0);
                fc[i] = trp < 0 ? -1 : trp * corr;
                if (fc[i] >= 0) { sum += fc[i]; nsum++; }
            }
            double mean = nsum > 0 ? sum / nsum : 0;
            List<TableRow> rows = new List<TableRow>();
            for (int i = 0; i < 24; i++)
            {
                DateTime t = h0.AddHours(i);
                int j = (int)Math.Round((t.AddDays(-1) - start).TotalHours);
                double past = j >= 0 && j < 48 ? collm.Valid[j] : -1;
                int gq = fc[i] < 0 ? -1 : fc[i] >= 1.3 * mean ? 2 : fc[i] >= 0.8 * mean ? 1 : 0;   // giudizio: 2 buono, 1 medio, 0 scarso
                string giud = gq < 0 ? "-" : gq == 2 ? Lang.T("buono") : gq == 1 ? Lang.T("medio") : Lang.T("scarso");
                TableRow row = new TableRow
                {
                    Cells = new[] {
                        t.ToString("dd/MM HH:00", CultureInfo.InvariantCulture),
                        past < 0 ? "-" : past.ToString("F0"), past < 0 ? "-" : Translate(past).ToString("F1"),
                        idx[i].ToString("F1"), fc[i] < 0 ? "-" : fc[i].ToString("F1"), giud }
                };
                if (gq == 2) row.Fore = Pal.Yellow;
                else if (gq == 0) row.Fore = Pal.Rgb(128, 128, 128);
                rows.Add(row);
            }
            gridPrev.Rows = rows;
            gridPrev.Refresh();
            ChartSeries pf = new ChartSeries(); pf.Name = Lang.F("previsione {0} MHz /ora", f.ToString("F0")); pf.Color = Pal.Ink(Pal.Rgb(255, 170, 40)); pf.Values = fc; pf.Bars = true;
            ChartSeries pi = new ChartSeries(); pi.Name = Lang.T("indice sciami tratta (scala a destra)"); pi.Color = Pal.Ink(Pal.Rgb(240, 110, 230)); pi.Values = idx; pi.RightAxis = true;
            chartPrev24.Series.Add(pf); chartPrev24.Series.Add(pi);
            chartPrev24.XLabels = xf; chartPrev24.XLabelEvery = 2;
            SetLocalRow(chartPrev24, h0, 24);
            chartPrev24.Title = Lang.F("Previsione sperimentale delle prossime 24 ore a {0} MHz per {1}", f.ToString("F0"), PathLabel());
            chartPrev24.YLabel = Lang.T("riflessioni/ora"); chartPrev24.Y2Label = Lang.T("Σ ZHR x effic.");
            chartPrev24.MarkIndex = -1;
            chartPrev24.InvalidateVisual();
        }

        // Previsione sperimentale per l'ora che inizia a t: valore traslato della stessa ora del giorno prima (o di due giorni prima),
        // corretto con la variazione dell'indice degli sciami. -1 = nessun dato radar per quell'ora.
        double ForecastAt(DateTime hourStartUtc)
        {
            if (collm == null || numPrevFreq == null) return -1;
            DateTime start = collm.DayToday.AddDays(-1);
            double past = -1;
            for (int back = 1; back <= 2 && past < 0; back++)
            {
                int j = (int)Math.Round((hourStartUtc.AddDays(-back) - start).TotalHours);
                if (j >= 0 && j < 48 && collm.Valid[j] >= 0) past = collm.Valid[j];
            }
            if (past < 0) return -1;
            double idxNow = ShowerIndex(hourStartUtc.AddMinutes(30)), idxPast = ShowerIndex(hourStartUtc.AddDays(-1).AddMinutes(30));
            return Translate(past) * (idxNow + 10.0) / (idxPast + 10.0);
        }

        // Somma ZHR x efficienza della tratta per tutti gli sciami attivi all'istante t
        double ShowerIndex(DateTime t)
        {
            if (model == null || !model.Valid) return 0;
            double s = 0;
            foreach (Shower sh in Catalog.Showers)
            {
                if (!sh.IsActive(t)) continue;
                double z; Vec3 v = RadiantVec(sh, t, out z);
                if (z <= 0) continue;
                double lf; s += z * model.Efficiency(v, out lf) / 100.0;
            }
            return s;
        }

        // ---------------- file RMOB ----------------
        async Task LoadRmobFile()
        {
            try
            {
                IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = Lang.T("File RMOB (conteggi orari del mese)"),
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType(Lang.T("File RMOB")) { Patterns = new[] { "*.txt", "*.TXT", "*.dat", "*.DAT" } },
                        new FilePickerFileType(Lang.T("Tutti i file")) { Patterns = new[] { "*" } }
                    }
                });
                if (files == null || files.Count == 0) return;
                string path = files[0].TryGetLocalPath();
                if (path == null) { await Dialogs.Message(Win, Lang.T("File RMOB"), Lang.T("Il file scelto non e' un file locale.")); return; }
                string msg;
                Dictionary<DateTime, double> r = ParseRmob(path, out msg);
                if (r == null || r.Count == 0)
                {
                    await Dialogs.Message(Win, Lang.T("File RMOB"), Lang.T("Non ho riconosciuto il formato del file.") + "\n" + msg);
                    return;
                }
                rmobCounts = r; rmobName = Path.GetFileNameWithoutExtension(path);
                lblRmob.Text = Lang.F("{0}: {1} ore lette.", rmobName, r.Count) + " " + msg;
                UpdatePrevTab();
            }
            catch (Exception ex) { await Dialogs.Message(Win, Lang.T("File RMOB"), ex.Message); }
        }

        // Ricava l'esponente k (con K = 1) dalla mediana dei rapporti RMOB / Collm sulle ore comuni
        async Task ExponentFromPopIndex()
        {
            DateTime now = DateTime.UtcNow;
            double lam = Astro.SolarLongitude(Astro.JulianDay(now)), sw = 0, sk = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Shower sh in Catalog.Showers)
            {
                if (!sh.IsActive(now)) continue;
                double z = sh.ZhrAt(lam, now);
                double r; bool g = GmnPopIndex.R.TryGetValue(sh.Code, out r);
                if (!g) r = sh.PopIndex;
                double k = MsGeometry.KFromPopIndex(r);
                if (z < 0.5 || double.IsNaN(k)) continue;
                sw += z; sk += z * k;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}  ZHR {1:F0}  r {2:F2} ({3})  k {4:F2}", sh.Code, z, r, g ? "GMN" : "IMO", k));
            }
            if (sw <= 0) { await Dialogs.Message(Win, Lang.T("Esponente"), Lang.T("Nessuno sciame attivo con un indice di popolazione valido.")); return; }
            double km = sk / sw;
            string txt = sb.ToString() + "\n" + Lang.F("Media pesata con lo ZHR: k = {0:F2} (regola SM6FHZ: 3, più prudente).\n\nApplico questo esponente?", km);
            if (await Dialogs.Confirm(Win, Lang.T("k dagli indici di popolazione"), txt))
                numPrevK.Value = Dm(Math.Max(1, Math.Min(4, Math.Round(km, 2))));
        }

        async Task FitExponentFromRmob()
        {
            if (collm == null || rmobCounts == null) { await Dialogs.Message(Win, Lang.T("Esponente"), Lang.T("Servono sia i dati di Collm sia un file RMOB delle stesse ore.")); return; }
            List<double> ratios = new List<double>();
            DateTime start = collm.DayToday.AddDays(-1);
            for (int i = 0; i < 48; i++)
            {
                double c;
                if (collm.Valid[i] > 0 && rmobCounts.TryGetValue(start.AddHours(i), out c) && c > 0) ratios.Add(c / collm.Valid[i]);
            }
            if (ratios.Count < 3) { await Dialogs.Message(Win, Lang.T("Esponente"), Lang.F("Troppe poche ore in comune ({0}) tra Collm e il file RMOB.", ratios.Count)); return; }
            ratios.Sort();
            double med = ratios[ratios.Count / 2];
            double f = V(numPrevFreq);
            double kEmp = -Math.Log(med) / Math.Log(f / RadarFreq);
            if (double.IsNaN(kEmp) || double.IsInfinity(kEmp)) { await Dialogs.Message(Win, Lang.T("Esponente"), Lang.T("Esponente non calcolabile con questi dati.")); return; }
            string txt = Lang.F("Ore in comune: {0}. Rapporto mediano RMOB/Collm = {1:F4}.\nEsponente empirico (con K = 1): k = {2:F2}  (regola SM6FHZ: 3).\n\n" +
                "Applico questo esponente? (In alternativa si lascia k = 3 e si regola il fattore K = {3:F2}.)",
                ratios.Count, med, kEmp, med / MsGeometry.CountScale(RadarFreq, f, 3));
            if (await Dialogs.Confirm(Win, Lang.T("Esponente dal confronto RMOB"), txt))
            {
                numPrevStation.Value = 1;
                numPrevK.Value = Dm(Math.Max(1, Math.Min(4, Math.Round(kEmp, 2))));
            }
        }

        // Lettura tollerante dei file RMOB:
        //  formato A (file mensile Colorgramme/Spectrum Lab): righe "giorno| n| n| ... (24 ore)", intestazione "Mmm| 00h| 01h| ..."
        //  formato B (tabella del bollettino): intestazione " UT |  1   2   3 ...", righe "  0h|175 166 ..." a colonne di 4 caratteri
        // Mese e anno dal nome del file (..._MMAAAArmob...) o dall'intestazione; altrimenti il mese corrente.
        public static Dictionary<DateTime, double> ParseRmob(string file, out string msg)
        {
            msg = "";
            Dictionary<DateTime, double> res = new Dictionary<DateTime, double>();
            string[] lines = File.ReadAllLines(file);
            int month = 0, year = 0;
            Match m = Regex.Match(Path.GetFileName(file), @"(\d{2})(\d{4})rmob", RegexOptions.IgnoreCase);
            if (m.Success) { month = int.Parse(m.Groups[1].Value); year = int.Parse(m.Groups[2].Value); }
            if (month < 1 || month > 12) month = 0;
            if (month == 0)
                foreach (string l in lines)
                {
                    Match h = Regex.Match(l, @"(January|February|March|April|May|June|July|August|September|October|November|December)\s+(\d{4})", RegexOptions.IgnoreCase);
                    if (h.Success)
                    {
                        string mon = h.Groups[1].Value.Substring(0, 1).ToUpperInvariant() + h.Groups[1].Value.Substring(1, 2).ToLowerInvariant();
                        month = DateTime.ParseExact(mon, "MMM", CultureInfo.InvariantCulture).Month; year = int.Parse(h.Groups[2].Value); break;
                    }
                }
            if (month == 0) { month = DateTime.UtcNow.Month; year = DateTime.UtcNow.Year; msg += Lang.T("Mese non indicato: uso il mese corrente.") + " "; }
            int dim = DateTime.DaysInMonth(year, month);

            // formato A
            int nA = 0;
            foreach (string raw in lines)
            {
                string[] c = raw.Split('|');
                if (c.Length < 25) continue;
                int day;
                if (!int.TryParse(c[0].Trim(), out day) || day < 1 || day > dim) continue;
                for (int hr = 0; hr < 24; hr++)
                {
                    double v;
                    if (double.TryParse(c[hr + 1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out v) && v >= 0)
                    { res[new DateTime(year, month, day, hr, 0, 0, DateTimeKind.Utc)] = v; nA++; }
                }
            }
            if (nA > 0) { msg += Lang.F("Formato file mensile, {0}/{1}.", month.ToString("00"), year); return res; }

            // formato B
            int[] days = null;
            foreach (string raw in lines)
            {
                int bar = raw.IndexOf('|');
                if (bar < 0) continue;
                string left = raw.Substring(0, bar).Trim().ToLowerInvariant();
                string right = raw.Substring(bar + 1);
                if (left == "ut")
                {
                    List<int> d = new List<int>();
                    foreach (string tok in right.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) { int x; if (int.TryParse(tok, out x)) d.Add(x); }
                    days = d.ToArray(); continue;
                }
                if (days == null) continue;
                int hrv;
                if (!int.TryParse(left.TrimEnd('h'), out hrv) || hrv < 0 || hrv > 23) continue;
                for (int k = 0; k < days.Length; k++)
                {
                    int p0 = k * 4;
                    if (p0 >= right.Length) break;
                    string cell = right.Substring(p0, Math.Min(4, right.Length - p0)).Trim();
                    double v;
                    if (cell.Length > 0 && double.TryParse(cell, NumberStyles.Float, CultureInfo.InvariantCulture, out v) && days[k] >= 1 && days[k] <= dim)
                        res[new DateTime(year, month, days[k], hrv, 0, 0, DateTimeKind.Utc)] = v;
                }
            }
            if (res.Count > 0) msg += Lang.F("Formato tabella del bollettino ({0}/{1}): se il file contiene piu' osservatori vengono letti di seguito, usa un file con un solo osservatore.", month.ToString("00"), year);
            return res;
        }
    }
}
