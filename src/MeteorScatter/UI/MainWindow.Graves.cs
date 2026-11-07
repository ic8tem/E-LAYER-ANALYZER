// YA-MS - linguetta "Radar GRAVES" (dopo "Calcolo dell'efficienza")
//
// GRAVES (143,050 MHz CW, JN27SI) e' il trasmettitore di una tratta in forward scatter GRAVES -> mia stazione:
// le scie meteoriche nella zona comune riflettono GRAVES verso di me (cosi' contano le meteore gli osservatori RMOB).
// Si costruisce un PathModel a parte con le stesse funzioni della tratta principale (stessa quota delle scie,
// stesso lobo minimo) e si calcolano allo stesso modo efficienza degli sciami, Hot A/B consigliato e zone PREV.
// Il modello ha la mia stazione come "Home" e GRAVES come "Dx": cosi' Hot A/Hot B hanno lo stesso verso della
// tratta principale quando il corrispondente e' GRAVES (A = a sinistra guardando da me verso GRAVES).
// Il fascio a passi (PE1ITR): vedi GravesBeam in src/Controls/GravesView.cs.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        // durata tipica di una scia sottodensa a 143 MHz (s): ordine di grandezza dei ping (qualche decimo di secondo).
        // Nel programma esiste la regola SM6FHZ della durata (MsGeometry.DurationScale, ~ lambda^2) ma non una durata
        // di partenza misurata: si usa quindi un valore fisso.
        const double GravesTrailSec = 0.3;

        PathModel gravesModel;           // tratta GRAVES -> mia stazione (Home = io, Dx = GRAVES)
        string gravesKey = "";           // parametri con cui e' stato costruito il modello
        int gravesGen;                   // generazione del calcolo in corso (scarta i risultati vecchi)
        DockPanel gravesPage;
        ComboBox cmbGravesSeq;
        GravesView gravesView;
        GravesTimeline gravesLine;
        Grid gravesMid;
        TextBlock gvA, gvAsub, gvB, gvBsub, gvC, gvCsub, gvD, gvDsub;
        TextBlock lblGravesHead2, lblGravesNote;
        int gravesSector = -1, gravesStep = -1;   // settore (0..3) e passo (0..5) che illuminano l'hot spot consigliato
        DateTime gravesDay = DateTime.MinValue;

        // durata del passo del fascio (s): 0,8 (PE1ITR 2021) oppure 3,2 (PE1ITR 2007)
        double GravesStepSec { get { return cmbGravesSeq != null && cmbGravesSeq.SelectedIndex == 1 ? 3.2 : 0.8; } }

        // riquadro con titolo, valore grande e testo sotto (come i riquadri del "Calcolo dell'efficienza")
        Border GCard(string title, Color accent, out TextBlock big, out TextBlock sub)
        {
            TextBlock t = new TextBlock { Text = title, Foreground = Pal.B(accent), FontWeight = FontWeight.Bold, FontSize = 11.5, TextWrapping = TextWrapping.Wrap };
            big = new TextBlock { Text = "-", Foreground = Pal.B(accent), FontWeight = FontWeight.Bold, FontSize = 22, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 0) };
            sub = new TextBlock { Text = "", Foreground = Pal.B(Pal.Fore), FontSize = 12.5, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 0) };
            StackPanel sp = new StackPanel();
            sp.Children.Add(t); sp.Children.Add(big); sp.Children.Add(sub);
            Border bar = new Border { Background = Pal.B(accent), Width = 4, CornerRadius = new CornerRadius(2), Margin = new Thickness(0, 0, 10, 0) };
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*") };
            Grid.SetColumn(bar, 0); Grid.SetColumn(sp, 1);
            g.Children.Add(bar); g.Children.Add(sp);
            return new Border
            {
                Background = Pal.B(Pal.Card), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8), Padding = new Thickness(8, 6), Margin = new Thickness(4), Child = g
            };
        }

        static TextBlock GHead(string t)
        {
            return new TextBlock { Text = t, Foreground = Pal.B(Pal.Violet), FontWeight = FontWeight.Bold, FontSize = 13, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 6) };
        }

        static Border GPanel(Control child)
        {
            return new Border
            {
                Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10), Padding = new Thickness(10, 8), Margin = new Thickness(6), Child = child
            };
        }

        void BuildGravesTab()
        {
            DockPanel p = Page(Lang.T("Radar GRAVES"));
            gravesPage = p;

            // ---- barra in alto
            TextBlock title = new TextBlock { Text = Lang.T("Radar GRAVES"), FontWeight = FontWeight.Bold, FontSize = 14, Foreground = Pal.B(Pal.Fore), Margin = new Thickness(6, 0, 4, 0) };
            TextBlock info = L(Lang.T("· 143,050 MHz CW · JN27SI (Broye-lès-Pesmes, 47,348° N 5,515° E)"));
            Button btnNow = B(Lang.T("Adesso"));
            btnNow.Classes.Add("accent");
            ToolTip.SetTip(btnNow, Lang.T("Riporta la volta celeste al tempo reale"));
            btnNow.Click += (s, e) => { trackTime.Value = 0; UpdateSky(); };
            Button btnDx = B(Lang.T("Metti GRAVES come DX"));
            btnDx.Background = Pal.B(Pal.Violet); btnDx.Foreground = Pal.B(Pal.AccentFore); btnDx.FontWeight = FontWeight.SemiBold;
            ToolTip.SetTip(btnDx, Lang.T("Il corrispondente diventa GRAVES (JN27SI, quota 200 m) e la tratta principale viene ricalcolata"));
            btnDx.Click += (s, e) =>
            {
                txtDx.Text = GravesBeam.Locator;
                numDxAlt.Value = 200;
                Recalc();
            };
            List<string> seq = new List<string>
            {
                Lang.T("0,8 s per passo · ciclo 4,8 s (PE1ITR 2021)"),
                Lang.T("3,2 s per passo · ciclo 19,2 s (PE1ITR 2007)")
            };
            cmbGravesSeq = new ComboBox { ItemsSource = seq, Margin = new Thickness(2, 3), VerticalAlignment = VerticalAlignment.Center };
            cmbGravesSeq.SelectedIndex = cfg.GetD("graves_sequenza", 0) >= 0.5 ? 1 : 0;
            cmbGravesSeq.SelectionChanged += (s, e) =>
            {
                cfg.Set("graves_sequenza", (double)Math.Max(0, cmbGravesSeq.SelectedIndex));
                try { cfg.Save(); } catch { }
                UpdateGravesSky();   // anche le eco attese dipendono dalla durata del passo
            };
            DockTop(p, Row(title, info, btnNow, btnDx, L(Lang.T("Sequenza del fascio")), cmbGravesSeq));

            StackPanel body = new StackPanel { Orientation = Orientation.Vertical };

            // ---- quattro riquadri
            Grid cards = new Grid { ColumnDefinitions = new ColumnDefinitions("*,*,*,*"), MinHeight = 110, Margin = new Thickness(6, 6, 6, 2) };
            Border c0 = GCard(Lang.T("ZONA UTILE NEL FASCIO DI GRAVES"), Pal.Green, out gvA, out gvAsub);
            Border c1 = GCard(Lang.T("EFFICIENZA DELLA TRATTA GRAVES → TE ADESSO"), Pal.Azure, out gvB, out gvBsub);
            Border c2 = GCard(Lang.T("TRATTA"), Pal.Orange, out gvC, out gvCsub);
            Border c3 = GCard(Lang.T("PROSSIMO PASSAGGIO DEL FASCIO SULLA ZONA"), Pal.Violet, out gvD, out gvDsub);
            Grid.SetColumn(c0, 0); Grid.SetColumn(c1, 1); Grid.SetColumn(c2, 2); Grid.SetColumn(c3, 3);
            cards.Children.Add(c0); cards.Children.Add(c1); cards.Children.Add(c2); cards.Children.Add(c3);
            body.Children.Add(cards);

            // ---- al centro: cielo visto da GRAVES (sinistra) e ricostruzione cronologica (destra)
            gravesMid = new Grid { ColumnDefinitions = new ColumnDefinitions("40*,60*") };
            gravesView = new GravesView();
            DockPanel left = new DockPanel();
            DockTop(left, GHead(Lang.T("IL CIELO VISTO DA GRAVES")));
            TextBlock leftText = new TextBlock
            {
                Foreground = Pal.B(Pal.Muted), FontSize = 12, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 6, 0, 0),
                Text = Lang.T("Quattro schiere a fasi coprono il cielo a sud, da 90° a 270° di azimut, in quattro settori di 45° (D, C, B, A); " +
                              "ogni settore è diviso in 6 passi da 7,5°, illuminati uno dopo l'altro, nello stesso momento in tutti e quattro i settori " +
                              "(passi numerati da 1 a 6 a partire dall'azimut più basso). Il fascio è largo circa 25° in elevazione: alla quota delle scie " +
                              "copre la corona colorata tra i due cerchi tratteggiati. Punti arancio/rosa = zone PREV della tratta GRAVES → te, " +
                              "linea verde = direzione della tua stazione, linea rossa = dalla tua stazione all'hot spot consigliato (PE1ITR).")
            };
            DockPanel.SetDock(leftText, Dock.Bottom);
            left.Children.Add(leftText);
            left.Children.Add(gravesView);
            Border lp = GPanel(left);
            Grid.SetColumn(lp, 0);
            gravesMid.Children.Add(lp);

            gravesLine = new GravesTimeline { TrailSec = GravesTrailSec };
            DockPanel right = new DockPanel();
            lblGravesHead2 = GHead(Lang.F("RICOSTRUZIONE CRONOLOGICA · {0} UTC", DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            DockTop(right, lblGravesHead2);
            StackPanel noteBox = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(0, 6, 0, 0) };
            lblGravesNote = new TextBlock { Foreground = Pal.B(Pal.Fore), FontSize = 12, TextWrapping = TextWrapping.Wrap };
            TextBlock pillText = new TextBlock { Text = Lang.T("orologio del PC sincronizzato"), Foreground = Pal.B(Pal.Yellow), FontWeight = FontWeight.Bold, FontSize = 12 };
            Border pill = new Border
            {
                Background = Pal.B(Pal.A(50, Pal.Yellow)), BorderBrush = Pal.B(Pal.A(140, Pal.Yellow)), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10), Padding = new Thickness(8, 2), Margin = new Thickness(0, 5, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left, Child = pillText
            };
            ToolTip.SetTip(pill, Lang.T("La sequenza del fascio si calcola con l'orologio del PC: sincronizzalo con un server NTP (errore di pochi decimi di secondo al massimo)."));
            noteBox.Children.Add(lblGravesNote);
            noteBox.Children.Add(pill);
            DockPanel.SetDock(noteBox, Dock.Bottom);
            right.Children.Add(noteBox);
            right.Children.Add(gravesLine);
            Border rp = GPanel(right);
            Grid.SetColumn(rp, 1);
            gravesMid.Children.Add(rp);
            body.Children.Add(gravesMid);
            GravesFontScale();

            // ---- in basso: come ascoltare e fonti
            Grid bottom = new Grid { ColumnDefinitions = new ColumnDefinitions("50*,50*") };
            StackPanel howTo = new StackPanel { Orientation = Orientation.Vertical };
            howTo.Children.Add(GHead(Lang.T("COME ASCOLTARE I PING DI GRAVES")));
            string[][] rows =
            {
                new[] { Lang.T("Passo"), Lang.T("Che cosa fare") },
                new[] { Lang.T("Ricevitore"), Lang.T("USB su 143,049 MHz (nota a circa 1 kHz) oppure 143,050 MHz, AGC lento o spento, banda audio di almeno 2,4 kHz") },
                new[] { Lang.T("Antenna"), Lang.T("Verso l'hot spot consigliato della tratta GRAVES → te (riquadro Tratta), con bassa elevazione") },
                new[] { Lang.T("Waterfall"), Lang.T("I ping sono righe brevi vicino alla portante; le scie lunghe (overdense) sono più larghe e mostrano il Doppler") },
                new[] { Lang.T("Conteggio"), Lang.T("RMOB usa GRAVES per i conteggi orari delle meteore: confronta i tuoi ping con gli osservatori su 143,050 MHz") }
            };
            Grid tbl = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*") };
            for (int i = 0; i < rows.Length; i++)
            {
                tbl.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                for (int j = 0; j < 2; j++)
                {
                    TextBlock tc = new TextBlock
                    {
                        Text = rows[i][j], TextWrapping = j == 1 ? TextWrapping.Wrap : TextWrapping.NoWrap, FontSize = 12.5,
                        Foreground = Pal.B(i == 0 ? Pal.Muted : Pal.Fore), FontWeight = i == 0 || j == 0 ? FontWeight.SemiBold : FontWeight.Normal
                    };
                    Border cell = new Border
                    {
                        BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(0, 0, 0, 1),
                        Padding = new Thickness(6, 4, 12, 4), Child = tc
                    };
                    Grid.SetRow(cell, i); Grid.SetColumn(cell, j);
                    tbl.Children.Add(cell);
                }
            }
            howTo.Children.Add(tbl);
            Border bl = GPanel(howTo);
            Grid.SetColumn(bl, 0);
            bottom.Children.Add(bl);

            StackPanel src = new StackPanel { Orientation = Orientation.Vertical };
            src.Children.Add(GHead(Lang.T("FONTI")));
            src.Children.Add(new SelectableTextBlock
            {
                FontSize = 12.5, Foreground = Pal.B(Pal.Fore), TextWrapping = TextWrapping.Wrap,
                Text = "[1] " + Lang.T("Rob Hardenberg PE1ITR, \"The 143.050 MHz Graves Radar, a VHF Beacon\" (DUBUS 4/2007) e \"Graves Radar Update April 2021\" – pe1itr.com/graves") + "\n" +
                       "[2] " + Lang.T("Bolidozor wiki, \"Graves\" – schiere, settori e tempi del fascio") + "\n" +
                       "[3] " + Lang.T("Wikipedia, \"GRAVES (system)\" – trasmettitore a Broye-lès-Pesmes (47,348° N 5,515° E)") + "\n" +
                       "[4] " + Lang.T("RMOB – osservatori su 143,050 MHz") + "\n" +
                       "[5] " + Lang.T("YAEC di IC8TEM – linguetta Radar GRAVES (bozzetto da cui è derivata)")
            });
            Border br = GPanel(src);
            Grid.SetColumn(br, 1);
            bottom.Children.Add(br);
            body.Children.Add(bottom);

            p.Children.Add(new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = body
            });
            UpdateGravesNote();
        }

        // altezza della parte centrale: cresce con il carattere (scritte del disegno e testi sotto)
        void GravesFontScale()
        {
            if (gravesMid != null) gravesMid.Height = 500 * (0.55 + 0.45 * UiScale.Font);
        }

        void SaveGravesConfig()
        {
            if (cmbGravesSeq != null) cfg.Set("graves_sequenza", (double)Math.Max(0, cmbGravesSeq.SelectedIndex));
        }

        // Costruisce in un thread secondario il modello della tratta GRAVES -> mia stazione (stesse funzioni della
        // tratta principale; il campionamento richiede qualche istante)
        void StartGravesModel()
        {
            LatLon home;
            if (!Astro.TryLocator(Up(txtHome), out home)) return;
            double homeAlt = V(numHomeAlt) / 1000.0, layer = V(numLayer), elev = V(numElev), hot = PathModel.HotSpotOffsetKm;
            string key = string.Format(CultureInfo.InvariantCulture, "{0:F4};{1:F4};{2:F3};{3:F2};{4:F2};{5:F1}", home.Lat, home.Lon, homeAlt, layer, elev, hot);
            if (key == gravesKey && gravesModel != null) return;   // stessa tratta: il modello c'e' gia'
            if (key != gravesKey) gravesModel = null;   // parametri cambiati: il modello vecchio non vale piu'
            gravesKey = key;
            int gen = ++gravesGen;
            Task.Run(() =>
            {
                PathModel m = null;
                try { m = PathModel.Build(home, homeAlt, GravesBeam.Pos, GravesBeam.AltKm, layer, elev); }
                catch { m = null; }
                Dispatcher.UIThread.Post(() =>
                {
                    if (gen != gravesGen || replaced) return;   // nel frattempo e' partito un altro calcolo
                    gravesModel = m;
                    UpdateGravesSky();
                    UpdateGraves24h();
                });
            });
        }

        // Somma ZHR x efficienza della tratta GRAVES -> te all'istante t, sciame migliore, pesi dei lati A/B e (se acc
        // non e' null) contributi dei punti di riflessione per le zone PREV
        double GravesPathIndex(PathModel gm, DateTime t, double[] acc, out Shower best, out double bestZ, out double bestE, out double wA, out double wB)
        {
            best = null; bestZ = 0; bestE = 0; wA = 0; wB = 0;
            if (gm == null || !gm.Valid) return 0;
            double idx = 0, bw = 0;
            foreach (Shower s in ActiveShowers(t))
            {
                double z; Vec3 v = RadiantVec(s, t, out z);
                if (z <= 0) continue;
                double lf; double e = gm.Efficiency(v, out lf);
                double w = z * e / 100.0;
                idx += w; wA += w * lf; wB += w * (1 - lf);
                if (w > bw) { bw = w; best = s; bestZ = z; bestE = e; }
                if (acc != null && z > 0.1)
                {
                    double[] c = gm.Contributions(v);
                    for (int i = 0; i < c.Length && i < acc.Length; i++) acc[i] += z * c[i];
                }
            }
            return idx;
        }

        // Riquadri (a)-(c), cielo visto da GRAVES: all'ora della volta celeste (come la linguetta "Volta celeste")
        void UpdateGravesSky()
        {
            if (gravesView == null) return;
            DateTime t = SkyTimeUtc;
            LatLon home;
            bool okHome = Astro.TryLocator(Up(txtHome), out home);
            gravesView.MapLines = sky.MapLines; gravesView.MapBorders = sky.MapBorders;
            gravesView.LayerKm = V(numLayer);
            gravesView.HasMe = okHome; gravesView.Me = home;
            gravesView.MeLabel = Lang.F("tu {0}", Up(txtHome));
            gravesView.Marks.Clear(); gravesView.Prev.Clear();
            gravesSector = -1; gravesStep = -1;
            PathModel gm = gravesModel;
            if (gm == null || !gm.Valid)
            {
                string msg = gm == null ? Lang.T("calcolo della tratta GRAVES → te in corso...") : Lang.F("Calcolo non riuscito: {0}", gm.Message);
                if (!okHome) msg = Lang.T("Locator della stazione non valido");
                gvA.Text = "-"; gvAsub.Text = msg; gvA.Foreground = Pal.B(Pal.Muted);
                gvB.Text = "-"; gvBsub.Text = msg;
                gvC.Text = "-"; gvCsub.Text = msg;
                gravesView.HotSector = -1; gravesView.HotStep = -1;
                gravesView.InvalidateVisual();
                UpdateGravesTick();
                return;
            }
            double layer = gm.LayerKm;
            double[] acc = new double[gm.SamplePoints];
            Shower best; double bz, be, wA, wB;
            double idx = GravesPathIndex(gm, t, acc, out best, out bz, out be, out wA, out wB);

            // hot spot consigliato della tratta GRAVES -> te: il lato da cui arriva la maggior parte delle riflessioni
            HotPoint rh = null;
            bool fallback = false;
            if (gm.HotA != null && gm.HotB != null)
            {
                gm.HotA.Recommended = false; gm.HotB.Recommended = false;
                if (wA + wB > 0.05) rh = wA >= wB ? gm.HotA : gm.HotB;
                else
                {
                    // nessuno sciame utile: si mostra l'hot spot meglio coperto dal fascio (dentro i settori, piu' vicino a sud)
                    fallback = true;
                    int s0, p0, s1, p1;
                    bool inA = GravesBeam.Locate(gm.HotA.BearingFromDx, out s0, out p0);
                    bool inB = GravesBeam.Locate(gm.HotB.BearingFromDx, out s1, out p1);
                    if (inA && !inB) rh = gm.HotA;
                    else if (inB && !inA) rh = gm.HotB;
                    else rh = Math.Abs(Astro.Norm180(gm.HotA.BearingFromDx - 180)) <= Math.Abs(Astro.Norm180(gm.HotB.BearingFromDx - 180)) ? gm.HotA : gm.HotB;
                }
                rh.Recommended = !fallback;
            }

            // (a) zona utile nel fascio di GRAVES
            if (rh != null)
            {
                double azG = rh.BearingFromDx;
                double elG = MsGeometry.ElevationArticle(rh.DistFromDx, layer, GravesBeam.AltKm);
                int sec, st;
                bool inSec = GravesBeam.Locate(azG, out sec, out st);
                bool inBeam = elG >= 0 && elG <= GravesBeam.BeamElevDeg;
                if (inSec && inBeam)
                {
                    gravesSector = sec; gravesStep = st;
                    gvA.Text = Lang.F("Sì · settore {0}", GravesBeam.Sectors[sec]);
                    gvA.Foreground = Pal.B(Pal.Green);
                    gvAsub.Text = Lang.F("{0} da GRAVES: az {1:F1}° · el {2:F1}° (passo {3} di 6)", Lang.T(rh.Label), azG, elG, st + 1);
                }
                else
                {
                    gvA.Text = !inSec ? Lang.T("No: fuori dai settori") : elG < 0 ? Lang.T("No: sotto l'orizzonte di GRAVES") : Lang.T("No: sopra il fascio (circa 25°)");
                    gvA.Foreground = Pal.B(Pal.Red);
                    gvAsub.Text = Lang.F("{0} da GRAVES: az {1:F1}° · el {2:F1}° (settori da 90° a 270°, fascio fino a circa 25°)", Lang.T(rh.Label), azG, elG);
                }
                if (fallback) gvAsub.Text += "\n" + Lang.T("nessuno sciame utile adesso: hot spot più adatto al fascio");
            }
            else
            {
                gvA.Text = "-"; gvA.Foreground = Pal.B(Pal.Muted);
                gvAsub.Text = gm.Message;
            }

            // (b) efficienza della tratta GRAVES -> te
            gvB.Text = Lang.F("Σ {0:F1}", idx);
            StringBuilder sb = new StringBuilder();
            if (best != null)
            {
                Shower s = best;
                sb.Append(Lang.F("sciame migliore {0} {1}: ZHR {2:F0} × eff. {3:F0} %", s.Code, Lang.T(s.Name), bz, be));
                sb.Append('\n').Append(Lang.F("lato prevalente {0} ({1:F0}%)", wA >= wB ? "A" : "B", 100 * Math.Max(wA, wB) / Math.Max(1e-9, wA + wB)));
            }
            else sb.Append(Lang.T("nessuno sciame utile in questo momento"));
            sb.Append('\n').Append(TrackValue == 0 ? Lang.F("{0} UTC (tempo reale)", t.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture))
                                                   : Lang.F("{0} UTC (ora della volta celeste)", t.ToString("dd/MM HH:mm", CultureInfo.InvariantCulture)));
            // eco attese all'ora (sperimentale): previsione della linguetta "Attuazione sperimentale previsioni" (tratta
            // principale) portata a 143,050 MHz con la regola SM6FHZ, corretta con il rapporto degli indici degli sciami
            // (come fa ForecastAt) e moltiplicata per la probabilita' che il fascio a passi illumini la scia
            DateTime hr = new DateTime(t.Year, t.Month, t.Day, t.Hour, 0, 0, DateTimeKind.Utc);
            double fc = ForecastAt(hr);
            if (fc >= 0 && numPrevK != null)
            {
                Shower b2; double z2, e2, a2, b2w;
                double idxG = GravesPathIndex(gm, hr.AddMinutes(30), null, out b2, out z2, out e2, out a2, out b2w);
                double idxM = ShowerIndex(hr.AddMinutes(30));
                double f = GravesBeam.CatchFactor(GravesStepSec, GravesTrailSec);
                double n = fc * MsGeometry.CountScale(PrevFreqMHz, GravesBeam.FreqMHz, V(numPrevK)) * (idxG + 10.0) / (idxM + 10.0) * f;
                sb.Append('\n').Append(Lang.F("eco attese ≈ {0:F0}/ora (sperimentale: previsione × fascio {1:F0} %)", n, 100 * f));
            }
            gvBsub.Text = sb.ToString();

            // (c) tratta
            gvC.Text = Lang.F("{0:F0} km", gm.DistKm);
            if (rh != null)
                gvCsub.Text = Lang.F("antenna verso {0}: az {1:F0}° · el {2:F1}°\nGRAVES in linea diretta: az {3:F0}°", Lang.T(rh.Label), rh.BearingFromHome, rh.ElevFromHome, gm.BearingHomeToDx);
            else
                gvCsub.Text = Lang.F("GRAVES in linea diretta: az {0:F0}°", gm.BearingHomeToDx);

            // cielo visto da GRAVES
            foreach (HotPoint h in new HotPoint[] { gm.HotA, gm.HotB })
            {
                if (h == null) continue;
                bool main = h == rh;
                gravesView.Marks.Add(new GravesMark { Pos = h.Pos, Label = Lang.T(h.Label), Color = main && !fallback ? SkyPal.Red : Pal.HotPink, Main = main });
            }
            double amax = 0; foreach (double a in acc) amax = Math.Max(amax, a);
            if (amax > 0)
                for (int i = 0; i < acc.Length; i++)
                    if (acc[i] >= 0.03 * amax)
                    {
                        PrevSpot sp = new PrevSpot(); sp.Pos = gm.SampleLatLon(i); sp.W = acc[i]; sp.Left = gm.SampleLeft(i);
                        gravesView.Prev.Add(sp);
                    }
            gravesView.HotSector = gravesSector; gravesView.HotStep = gravesStep;
            gravesView.InvalidateVisual();
            UpdateGravesTick();
        }

        // Ricostruzione cronologica del giorno scelto (stesso giorno della tabella 24 h): indice orario della tratta
        // GRAVES -> te (valori a meta' ora) e sciame piu' favorevole per ora
        void UpdateGraves24h()
        {
            if (gravesLine == null) return;
            DateTime day = SelectedDayUtc;
            gravesDay = day.Date;
            lblGravesHead2.Text = Lang.F("RICOSTRUZIONE CRONOLOGICA · {0} UTC", day.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            PathModel gm = gravesModel;
            double[] idx = new double[24];
            string[] best = new string[24];
            bool mv = gm != null && gm.Valid;
            if (mv)
                for (int h = 0; h < 24; h++)
                {
                    Shower bs; double z, e, wA, wB;
                    idx[h] = GravesPathIndex(gm, day.AddHours(h + 0.5), null, out bs, out z, out e, out wA, out wB);
                    if (bs != null && z * e / 100.0 >= 0.05) best[h] = bs.Code;
                }
            gravesLine.Index = idx;
            gravesLine.Best = best;
            gravesLine.Valid = mv;
            gravesLine.Message = gm == null ? Lang.T("calcolo della tratta GRAVES → te in corso...") : gm.Message;
            DateTime nowU = DateTime.UtcNow;
            gravesLine.NowHour = nowU.Date == gravesDay ? nowU.TimeOfDay.TotalHours : -1;
            gravesLine.InvalidateVisual();
        }

        // nota sotto la cronologia (dipende dalla sequenza scelta)
        void UpdateGravesNote()
        {
            if (lblGravesNote == null) return;
            double step = GravesStepSec, cyc = GravesBeam.Steps * step;
            lblGravesNote.Text = Lang.F("La sequenza è legata all'ora UTC (PE1ITR): dal 2021 i passi durano 0,8 s e il ciclo di un settore 4,8 s; nel 2007 erano 3,2 s e 19,2 s. " +
                "La stazione di comando a volte sposta la sequenza di qualche passo: il dettaglio è una previsione da confermare con l'ascolto. " +
                "Una scia si sente solo se il fascio la illumina mentre dura: probabilità ≈ min(1, (passo + scia) / ciclo) = ({0:F1} + {1:F1}) / {2:F1} ≈ {3:F0} % " +
                "(durata tipica di una scia sottodensa a 143 MHz ≈ {1:F1} s).",
                step, GravesTrailSec, cyc, 100 * GravesBeam.CatchFactor(step, GravesTrailSec));
        }

        // Ogni secondo (timer della finestra): passo attivo, prossimi passaggi del fascio sulla zona, dettaglio dei prossimi secondi
        void UpdateGravesTick()
        {
            if (gravesView == null || gravesLine == null) return;
            DateTime now = DateTime.UtcNow;
            double step = GravesStepSec, cyc = GravesBeam.Steps * step;
            int act = GravesBeam.ActiveStep(now, step);
            if (sky != null && !ReferenceEquals(gravesView.MapLines, sky.MapLines)) { gravesView.MapLines = sky.MapLines; gravesView.MapBorders = sky.MapBorders; }
            gravesView.ActiveStep = act;
            gravesLine.NowUtc = now;
            gravesLine.StepSec = step;
            gravesLine.TrailSec = GravesTrailSec;
            gravesLine.Sector = gravesSector;
            gravesLine.HotStep = gravesStep;
            if (gravesDay != DateTime.MinValue) gravesLine.NowHour = now.Date == gravesDay ? now.TimeOfDay.TotalHours : -1;
            UpdateGravesNote();
            double f = GravesBeam.CatchFactor(step, GravesTrailSec);
            if (gravesSector < 0)
            {
                gvD.Text = "-";
                gvDsub.Text = Lang.F("nessun passo del fascio illumina la zona utile\nogni scia colta ≈ {0:F0} % ({1:F1} + {2:F1}) / {3:F1}", 100 * f, step, GravesTrailSec, cyc);
            }
            else
            {
                double wait = GravesBeam.WaitSec(now, gravesStep, step);
                bool lit = act == gravesStep;
                double left = (gravesStep + 1) * step - GravesBeam.Phase(now, step);
                gvD.Text = lit ? Lang.F("adesso · ancora {0:F1} s", Math.Max(0, left)) : Lang.F("tra {0:F1} s", wait);
                List<string> times = new List<string>();
                DateTime first = now.AddSeconds(wait);
                for (int i = 0; i < 3; i++) times.Add(GravesBeam.SecLabel(first.AddSeconds(i * cyc)));
                gvDsub.Text = Lang.F("passo {0}{1} ogni {2:F1} s · alle {3} ...\nscia colta ≈ {4:F0} % = ({5:F1} + {6:F1}) / {2:F1}",
                    GravesBeam.Sectors[gravesSector], gravesStep + 1, cyc, string.Join(" · ", times), 100 * f, step, GravesTrailSec);
            }
            if (gravesPage != null && gravesPage.IsVisible) { gravesView.InvalidateVisual(); gravesLine.InvalidateVisual(); }
        }
    }
}
