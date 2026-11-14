// YA-MS - sottolinguetta "Traffico in diretta" (in "Live e volta celeste"): tutto il traffico meteor scatter raccolto
// dalla linguetta "Raccolta dati" su una mappa azimutale centrata sulla stazione.
// Per ogni contatto la "nuvola" della maggiore concentrazione di scie probabili: per la tratta tra le due stazioni e
// l'ora dello spot si sommano i contributi di riflessione speculare di tutti gli sciami attivi (ZHR x contributo,
// stesso calcolo delle zone PREV della volta celeste) e per ciascun lato della tratta si disegna l'ellisse con centro
// e assi dati dalla media e dalla dispersione pesata dei punti di riflessione (1,5 deviazioni standard).
// Senza sciami utili (solo sporadiche) le nuvole sono gli hot spot classici, tratteggiate.
// Colori per banda; spot con una stazione fuori dalla mappa scartati; spot arrivati da meno di un minuto e
// non piu' vecchi di 3 minuti lampeggiano.
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace YAMS
{
    public partial class MainWindow
    {
        TrafficView traffic;
        TableView tTraffic;
        TextBlock lblTrafficInfo, lblTrafficCount, lblTrafficRadius;
        Slider sldTrafficRadius;
        ComboBox cmbTrafficWindow;
        CheckBox chkTrafficListeners, chkPskMonitors;
        static readonly int[] TrafficMinutes = { 3, 15, 30, 60, 180, 1440 };   // minimo 3 minuti (richiesta dell'autore, 14/11/2026)
        readonly Dictionary<MsSpot, List<TrafficCloud>> cloudCache = new Dictionary<MsSpot, List<TrafficCloud>>();
        int trafficSpotCount = -1;

        public static Color BandColor(string band)
        {
            switch (band)
            {
                case "28": return Color.FromRgb(161, 98, 7);
                case "50": return Color.FromRgb(22, 163, 74);
                case "70": return Color.FromRgb(8, 145, 178);
                case "144": return Color.FromRgb(234, 88, 12);
                case "222": return Color.FromRgb(219, 39, 119);
                case "432": return Color.FromRgb(124, 58, 237);
                default: return Color.FromRgb(100, 116, 139);
            }
        }

        string trafficCloudTip;
        // FT8 lunghi e isolati (possibile meteor scatter): verde chiaro
        static readonly Color Ft8IsoColor = Color.FromRgb(134, 239, 172);
        // messaggi rivolti a me sentiti da altri utenti di YA-MS (scambio cifrato): viola
        static readonly Color SharedColor = Color.FromRgb(167, 139, 250);

        void BuildTrafficTab()
        {
            DockPanel p = Page(Lang.T("Traffico in diretta"));
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("68*,32*") };
            p.Children.Add(g);
            traffic = new TrafficView { RadiusKm = Math.Max(600, Math.Min(3500, cfg.GetD("traffico_raggio", 2500))) };
            traffic.SpotClicked += t => ShowTrafficInfo(t);
            traffic.ListenerClicked += l => { if (lblTrafficInfo != null) lblTrafficInfo.Text = l.Info; };
            Grid.SetColumn(traffic, 0); g.Children.Add(traffic);

            StackPanel side = new StackPanel { Spacing = 6, Margin = new Thickness(10, 8, 10, 8) };
            side.Children.Add(BuildMyStationBox());
            side.Children.Add(new TextBlock { Text = Lang.T("Traffico meteor scatter in diretta"), FontSize = 17, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure) });
            side.Children.Add(new TextBlock
            {
                TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Fore), FontSize = 12.5,
                Text = Lang.T("Spot della linguetta \"Raccolta dati\". Per ogni contatto la zona dello sciame indica dove si concentrano le scie più probabili per quella tratta e quell'ora (riflessione speculare, somma degli sciami attivi); zone tratteggiate = solo meteore sporadiche. Gli spot con una stazione fuori dalla mappa non si disegnano; quelli appena arrivati (meno di 3 minuti) lampeggiano per un minuto.")
            });
            side.Children.Add(new TextBlock
            {
                TextWrapping = TextWrapping.Wrap, Foreground = Pal.B(Pal.Muted), FontSize = 12,
                Text = Lang.T("Si vedono gli spot di tutte le bande, anche di quelle non scelte nella Raccolta dati (quelli non si memorizzano). Gli spot CW e quelli tra stazioni troppo vicine non si vedono, se sono scartati nella Raccolta dati.")
            });
            sldTrafficRadius = new Slider { Minimum = 600, Maximum = 3500, Value = traffic.RadiusKm, Width = 220, VerticalAlignment = VerticalAlignment.Center };
            lblTrafficRadius = new TextBlock { VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeight.SemiBold, Foreground = Pal.B(Pal.Fore) };
            sldTrafficRadius.ValueChanged += (s, e) => { traffic.RadiusKm = Math.Round(sldTrafficRadius.Value / 50) * 50; RefreshTraffic(true); };
            side.Children.Add(Row(L(Lang.T("Raggio della mappa")), sldTrafficRadius, lblTrafficRadius));
            ComboBox cmbProj = new ComboBox { ItemsSource = new[] { Lang.T("azimutale (centrata sulla mia stazione)"), Lang.T("Mercatore") }, MinWidth = 220, VerticalAlignment = VerticalAlignment.Center };
            traffic.Mercator = cfg.GetB("traffico_mercatore", false);
            cmbProj.SelectedIndex = traffic.Mercator ? 1 : 0;
            cmbProj.SelectionChanged += (s, e) => { traffic.Mercator = cmbProj.SelectedIndex == 1; cfg.Set("traffico_mercatore", traffic.Mercator); RefreshTraffic(true); };
            side.Children.Add(Row(L(Lang.T("Proiezione della mappa")), cmbProj));
            List<string> wn = new List<string>();
            foreach (int m in TrafficMinutes) wn.Add(m < 60 ? Lang.F("ultimi {0} minuti", m) : m == 60 ? Lang.T("ultima ora") : m < 1440 ? Lang.F("ultime {0} ore", m / 60) : Lang.T("ultime 24 ore"));
            cmbTrafficWindow = new ComboBox { ItemsSource = wn, MinWidth = 170, VerticalAlignment = VerticalAlignment.Center };
            int wi = Array.IndexOf(TrafficMinutes, (int)cfg.GetD("traffico_minuti", 60));
            cmbTrafficWindow.SelectedIndex = wi < 0 ? Array.IndexOf(TrafficMinutes, 60) : wi;
            cmbTrafficWindow.SelectionChanged += (s, e) => RefreshTraffic(true);
            side.Children.Add(Row(L(Lang.T("Spot da mostrare")), cmbTrafficWindow));
            chkTrafficListeners = C(Lang.T("Mostra chi è in ascolto in MSK144, FSK441, PSK2K e JTMS (PSKReporter MQTT, triangoli)"), cfg.GetB("traffico_ascolto", true));
            if (chkTrafficListeners.Content is string tls) chkTrafficListeners.Content = new TextBlock { Text = tls, TextWrapping = TextWrapping.Wrap };
            chkTrafficListeners.IsCheckedChanged += (s, e) => { cfg.Set("traffico_ascolto", chkTrafficListeners.IsChecked == true); RefreshTraffic(true); };
            side.Children.Add(chkTrafficListeners);
            // anche i monitor attivi della mappa di PSKReporter, pure senza spot ricevuti (interrogazione periodica)
            chkPskMonitors = C(Lang.T("Anche i monitor attivi della mappa di PSKReporter, pure senza spot ricevuti (triangoli vuoti, aggiornati ogni 5 minuti)"), cfg.GetB("psk_monitor_attivi", true));
            if (chkPskMonitors.Content is string pms) chkPskMonitors.Content = new TextBlock { Text = pms, TextWrapping = TextWrapping.Wrap };
            chkPskMonitors.IsCheckedChanged += (s, e) => { PskActive.Enabled = chkPskMonitors.IsChecked == true; cfg.Set("psk_monitor_attivi", PskActive.Enabled); RefreshTraffic(true); };
            side.Children.Add(chkPskMonitors);
            // FT8 lunghi e isolati: possibile meteor scatter (regola dell'autore, 14/11/2026)
            CheckBox chkFt8 = C(Lang.T("FT8 oltre i 1100 km senza tratte parallele nei 3 minuti (verde chiaro, la riga lampeggia sempre)"), cfg.GetB("ft8_isolati", true));
            if (chkFt8.Content is string f8) chkFt8.Content = new TextBlock { Text = f8, TextWrapping = TextWrapping.Wrap };
            chkFt8.IsCheckedChanged += (s, e) => { MsCollector.Ft8Enabled = chkFt8.IsChecked == true; cfg.Set("ft8_isolati", MsCollector.Ft8Enabled); RefreshTraffic(true); };
            side.Children.Add(chkFt8);
            lblTrafficCount = new TextBlock { Foreground = Pal.B(Pal.Green), TextWrapping = TextWrapping.Wrap };
            side.Children.Add(lblTrafficCount);
            lblTrafficInfo = new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 12.5, Foreground = Pal.B(Pal.Fore) };
            side.Children.Add(new Border { Background = Pal.B(Pal.Panel2), CornerRadius = new CornerRadius(8), Padding = new Thickness(8, 6), Child = lblTrafficInfo });

            tTraffic = MakeTable(new[] { "UTC", "MHz", Lang.T("Stazione"), Lang.T("Corrispondente"), "km", Lang.T("% sciame") }, new double[] { 46, 64, 82, 82, 50, 90 });
            tTraffic.FitWidth = true;
            trafficCloudTip = Lang.T("% sciame: quota delle riflessioni utili di quella tratta, a quell'ora, che cade nella zona dello sciame disegnata sulla mappa (riflessione speculare, somma degli sciami attivi). Più è alta, più le scie buone sono concentrate in un'unica zona verso cui puntare l'antenna. \"sporadiche\" = nessuno sciame attivo dà riflessioni utili: restano solo le meteore sporadiche, ai due lati della tratta (zone tratteggiate).");
            tTraffic.HeaderTips = new[] { "", "", "", "", "", trafficCloudTip };
            tTraffic.RowClicked += i =>
            {
                if (i < 0 || i >= tTraffic.Rows.Count) return;
                TrafficSpot t = tTraffic.Rows[i].Tag as TrafficSpot;
                traffic.Selected = t; traffic.InvalidateVisual(); ShowTrafficInfo(t);
            };
            DockPanel right = new DockPanel();
            DockTop(right, side);
            right.Children.Add(tTraffic);
            Border rb = new Border { Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1, 0, 0, 0), Child = right };
            Grid.SetColumn(rb, 1); g.Children.Add(rb);
            ShowTrafficInfo(null);
        }

        // riquadro "La mia stazione" in cima alla colonna destra (prima era la prima riga dei comandi)
        Control BuildMyStationBox()
        {
            WrapPanel w1 = new WrapPanel { Orientation = Orientation.Horizontal };
            foreach (Control c in new Control[] { L(Lang.T("Il mio locator")), txtHome, L(Lang.T("quota m")), numHomeAlt })
            { c.VerticalAlignment = VerticalAlignment.Center; w1.Children.Add(c); }
            WrapPanel w2 = new WrapPanel { Orientation = Orientation.Horizontal };
            foreach (Control c in new Control[] { L(Lang.T("Banda MHz")), cmbBand, btnCalc })
            { c.VerticalAlignment = VerticalAlignment.Center; w2.Children.Add(c); }
            StackPanel sp = new StackPanel { Spacing = 2 };
            string ttl = Lang.T("la mia stazione"); if (ttl.Length > 0) ttl = char.ToUpper(ttl[0]) + ttl.Substring(1);
            sp.Children.Add(new TextBlock { Text = ttl, FontWeight = FontWeight.SemiBold, FontSize = 14, Foreground = Pal.B(Pal.Red) });
            sp.Children.Add(w1); sp.Children.Add(w2);
            return new Border
            {
                Background = Pal.B(Pal.Panel2), BorderBrush = Pal.B(Pal.Red), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8), Padding = new Thickness(8, 6), Margin = new Thickness(0, 0, 0, 4), Child = sp
            };
        }

        void ShowTrafficInfo(TrafficSpot t)
        {
            if (lblTrafficInfo == null) return;
            lblTrafficInfo.Text = t == null ? Lang.T("Clic su una tratta della mappa o su una riga della tabella per i dettagli del contatto e della sua zona dello sciame.") : t.Info;
        }

        // nuvole della concentrazione di scie per la tratta A-B all'ora t (in cache per ogni spot)
        List<TrafficCloud> CloudsFor(MsSpot s, LatLon a, LatLon b)
        {
            List<TrafficCloud> res;
            if (cloudCache.TryGetValue(s, out res)) return res;
            res = new List<TrafficCloud>();
            try
            {
                PathModel pm = PathModel.Build(a, 0.05, b, 0.05, numLayer != null ? V(numLayer) : 90, 0);
                if (pm != null && pm.Valid && pm.SamplePoints > 0)
                {
                    double[] acc = new double[pm.SamplePoints];
                    double tot = 0;
                    foreach (Shower sh in ActiveShowers(s.Utc))
                    {
                        double zhr;
                        Vec3 v = RadiantVec(sh, s.Utc, out zhr);
                        if (zhr < 0.1) continue;
                        double[] c = pm.Contributions(v);
                        for (int i = 0; i < c.Length && i < acc.Length; i++) { acc[i] += zhr * c[i]; tot += zhr * c[i]; }
                    }
                    if (tot > 1e-6)
                    {
                        foreach (bool left in new[] { true, false })
                        {
                            double sw = 0, la = 0, lo = 0;
                            for (int i = 0; i < acc.Length; i++)
                            {
                                if (pm.SampleLeft(i) != left || acc[i] <= 0) continue;
                                LatLon q = pm.SampleLatLon(i);
                                sw += acc[i]; la += acc[i] * q.Lat; lo += acc[i] * q.Lon;
                            }
                            if (sw < 0.15 * tot) continue;
                            la /= sw; lo /= sw;
                            double k = Math.Cos(la * Math.PI / 180) * 111.2, sxx = 0, syy = 0, sxy = 0;
                            for (int i = 0; i < acc.Length; i++)
                            {
                                if (pm.SampleLeft(i) != left || acc[i] <= 0) continue;
                                LatLon q = pm.SampleLatLon(i);
                                double e = (q.Lon - lo) * k, n = (q.Lat - la) * 111.2;
                                sxx += acc[i] * e * e; syy += acc[i] * n * n; sxy += acc[i] * e * n;
                            }
                            sxx /= sw; syy /= sw; sxy /= sw;
                            double tr = sxx + syy, det = sxx * syy - sxy * sxy, disc = Math.Sqrt(Math.Max(0, tr * tr / 4 - det));
                            double l1 = tr / 2 + disc, l2 = Math.Max(0, tr / 2 - disc);
                            // direzione dell'asse maggiore (x = est, y = nord) e suo azimut
                            double ang = 0.5 * Math.Atan2(2 * sxy, sxx - syy);
                            double az = Astro.Norm360(90 - ang * 180 / Math.PI);
                            res.Add(new TrafficCloud
                            {
                                Centre = new LatLon(la, lo), MajorKm = Math.Max(30, 1.5 * Math.Sqrt(l1)), MinorKm = Math.Max(20, 1.5 * Math.Sqrt(l2)),
                                AngleDeg = az, Share = sw / tot
                            });
                        }
                    }
                    else
                        foreach (HotPoint h in new[] { pm.HotA, pm.HotB })
                            if (h != null) res.Add(new TrafficCloud { Centre = h.Pos, MajorKm = 70, MinorKm = 70, Share = 0.5, Sporadic = true });
                }
            }
            catch { }
            if (cloudCache.Count > 3000) cloudCache.Clear();
            cloudCache[s] = res;
            return res;
        }

        // ogni 2 s dalla raccolta dati (e subito quando cambiano raggio o finestra di tempo)
        void RefreshTraffic(bool force)
        {
            if (traffic == null) return;
            if (lblTrafficRadius != null) lblTrafficRadius.Text = Lang.F("{0:F0} km", traffic.RadiusKm);
            bool visible = traffic.IsEffectivelyVisible;
            if (!force && !visible) return;
            LatLon home;
            if (!Astro.TryLocator(Up(txtHome), out home)) return;
            traffic.Home = home;
            traffic.HomeLabel = (txtRdLogin != null && Up(txtRdLogin).Length > 0 ? Up(txtRdLogin) + " " : "") + Up(txtHome);
            if (sky != null) { traffic.MapLines = sky.MapLines; traffic.MapBorders = sky.MapBorders; }
            DateTime now = DateTime.UtcNow;
            int mins = TrafficMinutes[Math.Max(0, cmbTrafficWindow.SelectedIndex)];
            DateTime lim = now.AddMinutes(-mins);
            List<TrafficSpot> list = new List<TrafficSpot>();
            int outside = 0, noLoc = 0;
            Dictionary<string, int> perBand = new Dictionary<string, int>();
            List<MsSpot> all = MsCollector.MapSpots();   // tutte le bande, senza CW e spot troppo vicini
            // in piu': FT8 oltre i 1100 km senza tratte parallele (verde chiaro, riga che lampeggia sempre) e spot WSJT-X UDP
            // che riguardano la mia stazione (tratteggio che lampeggia sempre)
            HashSet<MsSpot> ft8Iso = new HashSet<MsSpot>(), mine = new HashSet<MsSpot>();
            HashSet<string> seen = new HashSet<string>();
            foreach (MsSpot s in all) seen.Add(s.Key);
            if (MsCollector.Ft8Enabled)
                foreach (MsCollector.Ft8Path fp in MsCollector.Ft8Long)
                    if (fp.Show && fp.S.Utc >= lim) { ft8Iso.Add(fp.S); if (seen.Add(fp.S.Key)) all.Add(fp.S); }
            foreach (MsSpot s in MsCollector.MyUdp)
                if (s.Utc >= lim) { mine.Add(s); if (seen.Add(s.Key)) all.Add(s); }
            if (ft8Iso.Count > 0 || mine.Count > 0)
            {
                all.Sort((x, y) => x.Utc.CompareTo(y.Utc));
                HashSet<string> mk = new HashSet<string>(), fk = new HashSet<string>();
                foreach (MsSpot s in mine) mk.Add(s.Key);
                foreach (MsSpot s in ft8Iso) fk.Add(s.Key);
                foreach (MsSpot s in all) { if (mk.Contains(s.Key)) mine.Add(s); if (fk.Contains(s.Key)) ft8Iso.Add(s); }
            }
            for (int i = all.Count - 1; i >= 0 && list.Count < 400; i--)
            {
                MsSpot s = all[i];
                if (s.Utc < lim) continue;
                LatLon a, b;
                if (!Astro.TryLocator(s.LocSpotter, out a) || !Astro.TryLocator(s.LocDx, out b)) { noLoc++; continue; }
                if (!traffic.Inside(a) || !traffic.Inside(b)) { outside++; continue; }
                bool iso = ft8Iso.Contains(s);
                TrafficSpot t = new TrafficSpot
                {
                    A = a, B = b, CallA = s.Spotter, CallB = s.DxCall, Band = s.Band, Color = s.Shared ? SharedColor : iso ? Ft8IsoColor : BandColor(s.Band), Tag = s,
                    Blink = (now - s.Utc).TotalMinutes <= 3 && (now - s.Received).TotalSeconds <= 60,
                    Dashed = mine.Contains(s)
                };
                t.Clouds = CloudsFor(s, a, b);
                string cl = "";
                foreach (TrafficCloud c in t.Clouds)
                {
                    if (cl.Length > 0) cl += "\n";
                    cl += c.Sporadic ? Lang.F("hot spot (solo sporadiche) {0}", Astro.ToLocator(c.Centre))
                                     : Lang.F("zona dello sciame {0}: {1:F0} % delle riflessioni, {2:F0} × {3:F0} km, asse a {4:F0}°", Astro.ToLocator(c.Centre), 100 * c.Share, 2 * c.MajorKm, 2 * c.MinorKm, c.AngleDeg);
                }
                t.Info = s.DxCall + " " + s.LocDx + "  ⇄  " + s.Spotter + " " + s.LocSpotter + "\n" +
                         Lang.F("{0} UTC · {1:F1} kHz · {2:F0} km", s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), s.FreqKHz, Astro.DistanceKm(a, b)) + "\n" +
                         (cl.Length > 0 ? cl : Lang.T("zona dello sciame non calcolabile per questa tratta (troppo corta o troppo lunga)")) +
                         (string.IsNullOrEmpty(s.Comment) ? "" : "\n" + s.Comment) +
                         (iso ? "\n" + Lang.T("FT8 oltre i 1100 km senza tratte parallele nei 3 minuti: non ha l'aspetto dell'E-sporadico o della tropo, può essere meteor scatter") : "") +
                         (s.Shared ? "\n" + Lang.F("Sentito da {0} (YA-MS): il messaggio è rivolto alla mia stazione", s.Spotter)
                                   : t.Dashed ? "\n" + Lang.T("WSJT-X UDP: la decodifica riguarda la mia stazione") : "");
                list.Add(t);
                int nb; perBand.TryGetValue(s.Band, out nb); perBand[s.Band] = nb + 1;
            }
            list.Sort((x, y) => ((MsSpot)x.Tag).Utc.CompareTo(((MsSpot)y.Tag).Utc));   // i piu' recenti disegnati sopra (e in cima alla tabella)
            if (traffic.Selected != null && !list.Contains(traffic.Selected))
            {
                TrafficSpot keep = null;
                foreach (TrafficSpot t in list) if (t.Tag == traffic.Selected.Tag) keep = t;
                traffic.Selected = keep;
            }
            traffic.Spots = list;
            // stazioni in ascolto nei modi MS (PSKReporter MQTT) nella stessa finestra di tempo, 28-50-70-144-432 MHz
            List<TrafficListener> lis = new List<TrafficListener>();
            int nLis = 0, nMon = 0;
            if (chkTrafficListeners != null && chkTrafficListeners.IsChecked == true)
                foreach (MsListener ml in MsCollector.Listeners.Values)
                {
                    bool monOnly = ml.Count == 0;   // monitor attivo di PSKReporter che non ha ancora ricevuto spot MS
                    if (monOnly && !PskActive.Enabled) continue;
                    if ((monOnly ? ml.SeenUtc : ml.Latest) < (monOnly && lim > now.AddMinutes(-11) ? now.AddMinutes(-11) : lim)) continue;   // i monitor si rileggono ogni 5 minuti: restano almeno 11 minuti   // tutte le bande, anche quelle non scelte nella Raccolta dati
                    LatLon q;
                    if (!Astro.TryLocator(ml.Loc, out q) || !traffic.Inside(q)) continue;
                    nLis++;
                    if (monOnly) nMon++;
                    if (lis.Count >= 1500) continue;
                    lis.Add(new TrafficListener
                    {
                        Pos = q, Call = ml.Call, Color = BandColor(ml.Band), Hollow = monOnly,
                        Info = Lang.F("{0} {1} in ascolto su {2} MHz ({3})", ml.Call, ml.Loc, ml.Band, ml.Modes) + "\n" +
                               (monOnly ? Lang.F("monitor attivo sulla mappa di PSKReporter alle {0} UTC, nessuno spot ricevuto", ml.SeenUtc.ToString("HH:mm", CultureInfo.InvariantCulture))
                                        : Lang.F("ultimo spot ricevuto {0} UTC · {1} spot", ml.LastUtc.ToString("HH:mm", CultureInfo.InvariantCulture), ml.Count)) + "\n" +
                               (ml.Soft.Length > 0 ? ml.Soft + "\n" : "") +
                               Lang.F("{0:F0} km · azimut {1:F0}°", Astro.DistanceKm(traffic.Home, q), Astro.Bearing(traffic.Home, q))
                    });
                }
            traffic.Listeners = lis;
            traffic.Caption = Lang.F("Traffico MS in diretta · {0}", cmbTrafficWindow.SelectedItem as string);
            traffic.Legend.Clear();
            foreach (string bn in MsBands.Names)
            {
                int n; perBand.TryGetValue(bn, out n);
                traffic.Legend.Add(new KeyValuePair<string, Color>(Lang.F("{0} MHz: {1}", bn, n), BandColor(bn)));
            }
            traffic.InvalidateVisual();
            lblTrafficCount.Text = Lang.F("{0} contatti sulla mappa · {1} fuori dalla mappa · {2} senza locator", list.Count, outside, noLoc)
                                   + (chkTrafficListeners != null && chkTrafficListeners.IsChecked == true
                                      ? "\n" + Lang.F("{0} stazioni in ascolto (PSKReporter MQTT)", nLis)
                                        + (PskActive.Enabled ? " · " + Lang.F("{0} solo monitor attivi", nMon) + (PskActive.LastError.Length > 0 ? " · " + Lang.F("mappa di PSKReporter non raggiungibile: {0}", PskActive.LastError) : "") : "")
                                        + (PskMqtt.Connected ? "" : " · " + Lang.T("MQTT non connesso: si connette dalla linguetta \"Raccolta dati\"")) : "");
            int sig = list.Count; foreach (TrafficSpot t in list) if (t.Color == Ft8IsoColor) sig += 100000;
            if (sig != trafficSpotCount || force)
            {
                trafficSpotCount = sig;
                List<TableRow> rows = new List<TableRow>();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    TrafficSpot t = list[i]; MsSpot s = (MsSpot)t.Tag;
                    string side = "";
                    foreach (TrafficCloud c in t.Clouds) if (!c.Sporadic && c.Share >= 0.5) side = (100 * c.Share).ToString("F0") + " %";
                    if (t.Clouds.Count > 0 && t.Clouds[0].Sporadic) side = Lang.T("sporadiche");
                    rows.Add(new TableRow
                    {
                        Cells = new[] { s.Utc.ToString("HH:mm", CultureInfo.InvariantCulture), (s.FreqKHz / 1000).ToString("F3", CultureInfo.CurrentCulture), s.Spotter, s.DxCall,
                                        Astro.DistanceKm(t.A, t.B).ToString("F0", CultureInfo.CurrentCulture), side },
                        Fore = t.Color == Ft8IsoColor && !Tema.Scuro ? Color.FromRgb(21, 128, 61) : t.Color, Tag = t, CellTips = new[] { "", "", "", "", "", trafficCloudTip },
                        Blink = t.Color == Ft8IsoColor, Back = t.Color == Ft8IsoColor ? Ft8IsoColor : (Color?)null
                    });
                }
                tTraffic.Rows = rows;
                tTraffic.Refresh();
            }
            if (traffic.Selected != null) ShowTrafficInfo(traffic.Selected);
        }

        // spot di esempio per le fotografie di controllo (solo con --screenshot)
        void AddDemoTraffic()
        {
            DateTime now = DateTime.UtcNow;
            string[][] d =
            {
                new[] { "IK7XXX", "JN80QK", "DK5EW", "JN48MB", "144360", "2", "1" },
                new[] { "IC8TEM", "JN70CN", "OK1TEH", "JO70KA", "144361", "6", "0" },
                new[] { "IZ7ZZZ", "JN81EK", "G4LOH", "IO70OJ", "144362", "11", "0" },
                new[] { "9A2AA", "JN75OS", "EA3XU", "JN11CK", "50280", "14", "0" },
                new[] { "S51ZZ", "JN76HD", "F6DRO", "JN03RQ", "144370", "19", "0" },
                new[] { "SV1BTR", "KM17VX", "OK2KKW", "JN99CL", "144360", "24", "0" },
                new[] { "YO2IS", "KN05PS", "I4XCC", "JN54QR", "144363", "29", "0" },
                new[] { "HB9COF", "JN47PH", "SV2DCD", "KN10LO", "50290", "33", "0" },
                new[] { "LZ1ZP", "KN12PQ", "DL7APV", "JO62QM", "144365", "38", "0" },
                new[] { "OE5JFL", "JN78DK", "EA6VQ", "JM19HN", "70174", "44", "0" },
                new[] { "IT9BLB", "JM78SB", "PA2M", "JO21FM", "144360", "51", "0" },
                new[] { "SP6GZ", "JO81LB", "IW0HK", "JN61FV", "144362", "1", "1" },
            };
            foreach (string[] r in d)
            {
                MsSpot s = new MsSpot
                {
                    Spotter = r[0], LocSpotter = r[1], DxCall = r[2], LocDx = r[3], FreqKHz = double.Parse(r[4], CultureInfo.InvariantCulture),
                    Utc = now.AddMinutes(-int.Parse(r[5], CultureInfo.InvariantCulture)), Source = 2, Match = "MSK144", Comment = "MSK144 (esempio)"
                };
                if (r[6] == "1") s.Received = now;
                LatLon a, b;
                if (Astro.TryLocator(s.LocSpotter, out a) && Astro.TryLocator(s.LocDx, out b)) s.DistKm = Astro.DistanceKm(a, b);
                MsCollector.AddDemo(s);
            }
            // stazioni in ascolto di esempio (PSKReporter MQTT)
            string[][] ls = { new[] { "OH5IY", "KP30KX", "144" }, new[] { "EA5BB", "IM98WN", "50" }, new[] { "LA1VNA", "JO59FX", "144" },
                              new[] { "OZ1BEF", "JO55WM", "70" }, new[] { "YO3JW", "KN34BK", "432" }, new[] { "G3WOS", "IO91IL", "28" } };
            foreach (string[] r in ls)
                MsCollector.Listeners[r[0] + "|" + r[2]] = new MsListener { Call = r[0], Loc = r[1], Band = r[2], Modes = "MSK144", LastUtc = now.AddMinutes(-5), Count = 3 };
            // FT8 lungo e isolato di esempio (verde chiaro, riga che lampeggia) e spot UDP che mi riguarda (tratteggio)
            MsSpot f8 = new MsSpot { Spotter = "EA4EOZ", LocSpotter = "IN80DJ", DxCall = "OK1TEH", LocDx = "JO70FD", FreqKHz = 144174, Utc = now.AddMinutes(-1), Source = 3, Mode = "FT8", Comment = "FT8 -18 dB", Received = now };
            LatLon fa, fb;
            if (Astro.TryLocator(f8.LocSpotter, out fa) && Astro.TryLocator(f8.LocDx, out fb)) f8.DistKm = Astro.DistanceKm(fa, fb);
            MsCollector.Ft8Long.Add(new MsCollector.Ft8Path { S = f8 });
            MsCollector.MyUdp.Add(new MsSpot { Spotter = "IC8TEM", LocSpotter = "JN70ON", DxCall = "9A2AE", LocDx = "JN95GM", FreqKHz = 144360, Utc = now.AddMinutes(-2), Source = 4, Mode = "MSK144", Comment = "MSK144 +3 dB  IC8TEM 9A2AE R+03  (WSJT-X UDP)", Received = now });
            // monitor attivi di esempio (mappa di PSKReporter, nessuno spot ricevuto: triangoli vuoti)
            string[][] ms = { new[] { "IK0XYZ", "JN61HT", "144" }, new[] { "S51DI", "JN76HD", "50" }, new[] { "SM6FHZ", "JO57XQ", "144" } };
            foreach (string[] r in ms)
                MsCollector.Listeners[r[0] + "|" + r[2]] = new MsListener { Call = r[0], Loc = r[1], Band = r[2], Modes = "MSK144", SeenUtc = now.AddMinutes(-4), Soft = "WSJT-X v2.7.0" };
            rdDirty = true;
        }
    }
}
