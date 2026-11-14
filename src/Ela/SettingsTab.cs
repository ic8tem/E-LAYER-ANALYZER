// E-LAYER ANALYZER - linguetta IMPOSTAZIONI (richiesta dell'autore, 14/11/2026): le opzioni generali del programma
// e lo smistamento degli spot tra le linguette dei modi di propagazione, prima sparse tra la barra in alto,
// DATA COLLECTION e la finestra degli aggiornamenti.
//   Generali: stazione, lingua, tema, dimensione del carattere, continente, ora NTP, aggiornamenti, cartella dei dati
//   Smistamento degli spot: l'ordine in cui il programma decide dove va ogni spot, con i filtri di ciascun modo
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using YAMS;

namespace ELA
{
    public sealed class SettingsTab
    {
        public readonly Control Root;
        public readonly PillTabs Inner = new PillTabs();
        readonly ElaWindow win;
        readonly DataTab dc;
        TableView tRules;
        TextBlock lblRule;
        List<string[]> ruleRows = new List<string[]>();
        TextBlock lblNtpInfo, lblFont;

        public SettingsTab(ElaWindow w, DataTab data)
        {
            win = w; dc = data;
            Inner.Add(Lang.T("Generali"), Scroll(BuildGeneral()));
            Inner.Add(Lang.T("Smistamento degli spot"), BuildRouting());
            DockPanel p = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(p, Inner.Bar, Dock.Top);
            p.Children.Add(Inner.Host);
            Root = p;
            Filters.Changed += FillRules;
            Ntp.Changed += () => Avalonia.Threading.Dispatcher.UIThread.Post(UpdateNtp);
            FillRules();
            UpdateNtp();
        }

        static Control Scroll(Control c)
        {
            return new ScrollViewer { Content = c, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled };
        }
        static TextBlock Txt(string t, bool muted = false)
        {
            return new TextBlock { Text = t, TextWrapping = TextWrapping.Wrap, FontSize = 13, Foreground = Pal.B(muted ? Pal.Muted : Pal.Fore), Margin = new Thickness(0, 2) };
        }
        static Control Section(string title, Color c, params Control[] items)
        {
            StackPanel sp = new StackPanel { Spacing = 4, Margin = new Thickness(4) };
            foreach (Control i in items) sp.Children.Add(i);
            Border b = Ui.Box(title, c, sp);
            b.Margin = new Thickness(0, 0, 0, 8);
            return b;
        }

        // ---------------- Generali
        Control BuildGeneral()
        {
            StackPanel root = new StackPanel { Margin = new Thickness(14, 10, 14, 14), MaxWidth = 1100, HorizontalAlignment = HorizontalAlignment.Left };
            root.Children.Add(new TextBlock { Text = Lang.T("Impostazioni generali"), FontSize = 18, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure), Margin = new Thickness(0, 0, 0, 8) });

            // stazione (la stessa della riga in alto)
            TextBox tCall = Ui.T(Station.Call, 110, true), tLoc = Ui.T(Station.Loc, 90, true);
            NumericUpDown nAlt = Ui.N(0, 5000, Ui.Dm(Station.AltM), 0, 10, 70);
            Button bApply = Ui.B(Lang.T("Applica"), true);
            bApply.Click += (s, e) => win.SetStation(Ui.Up(tCall), Ui.Up(tLoc), Ui.V(nAlt));
            root.Children.Add(Section(Lang.T("Stazione"), Pal.Azure,
                Ui.Row(Ui.L(Lang.T("Nominativo")), tCall, Ui.L(Lang.T("Locator")), tLoc, Ui.L(Lang.T("Quota m")), nAlt, bApply),
                Txt(Lang.T("Sono gli stessi dati della riga in alto: valgono per tutte le linguette, anche per METEOR SCATTER."), true)));

            // aspetto: lingua, tema, carattere
            List<string> langNames = new List<string>();
            foreach (string[] lg in Lang.Languages) langNames.Add(lg[1]);
            ComboBox cLang = Ui.Combo(langNames, Math.Max(0, Lang.IndexOf(Lang.Code)), 160);
            cLang.SelectionChanged += (s, e) =>
            {
                int li = cLang.SelectedIndex;
                if (li >= 0 && li < Lang.Languages.Length && Lang.Languages[li][0] != Lang.Code) win.RequestLanguage(Lang.Languages[li][0]);
            };
            List<string> tn = new List<string>();
            foreach (string t in Tema.Names) tn.Add(Lang.T(t));
            ComboBox cTheme = Ui.Combo(tn, Tema.Schema, 170);
            cTheme.SelectionChanged += (s, e) => { if (cTheme.SelectedIndex >= 0 && cTheme.SelectedIndex != Tema.Schema) win.RequestTheme(cTheme.SelectedIndex); };
            double pct0 = Math.Round(UiScale.Font * 100 / 5) * 5;
            Slider sFont = new Slider { Minimum = 85, Maximum = 140, Value = pct0, SmallChange = 5, LargeChange = 5, TickFrequency = 5, IsSnapToTickEnabled = true, Width = 200, Margin = new Thickness(4, 0), VerticalAlignment = VerticalAlignment.Center };
            lblFont = new TextBlock { Text = pct0.ToString("F0", CultureInfo.InvariantCulture) + " %", Foreground = Pal.B(Pal.Azure), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(6, 0) };
            sFont.PropertyChanged += (s, e) => { if (e.Property == RangeBase.ValueProperty) SetFont(sFont.Value); };
            root.Children.Add(Section(Lang.T("Aspetto"), Pal.Violet,
                Ui.Row(Ui.L(Lang.T("Lingua")), cLang, Ui.L(Lang.T("Tema")), cTheme),
                Ui.Row(Ui.L(Lang.T("Carattere")), sFont, lblFont),
                Txt(Lang.T("La dimensione del carattere vale per tutto il programma, dall'85 al 140 %, come in YA-MS. Lingua e tema ricostruiscono la finestra: le impostazioni e gli spot restano."), true)));

            // continente
            ComboBox cCont = Ui.Combo(Array.ConvertAll(Continents.Names, n => Lang.T(n)), Continents.Index, 170);
            cCont.SelectionChanged += (s, e) => Continents.Set(Math.Max(0, cCont.SelectedIndex));
            Continents.Changed += () => { if (cCont.SelectedIndex != Continents.Index) cCont.SelectedIndex = Continents.Index; };
            root.Children.Add(Section(Lang.T("Mappe e dati previsionali"), Pal.Green,
                Ui.Row(Ui.L(Lang.T("Continente")), cCont),
                Txt(Lang.T("Sposta le mappe di Mercatore sul continente scelto e sceglie i dati previsionali (E-skip di N0NBH per Europa e Nord America). È lo stesso menu della barra delle linguette."), true)));

            // ora
            lblNtpInfo = Txt("");
            root.Children.Add(Section(Lang.T("Ora"), Pal.Yellow, lblNtpInfo,
                Txt(Lang.T("Il programma controlla l'ora con i server NTP all'avvio e ogni 5 minuti e usa l'ora corretta nei calcoli. Su Windows, con i diritti di amministratore, corregge anche l'orologio del PC. Se la scritta in alto è NTP:FAILED, la rete blocca la porta UDP 123."), true)));

            // aggiornamenti
            CheckBox cChk = Ui.C(Lang.T("Controlla gli aggiornamenti su GitHub (all'avvio e ogni 6 ore)"), Station.Cfg.GetB("agg_controlla", true));
            CheckBox cAuto = Ui.C(Lang.T("Aggiornamento automatico e silenzioso: scarica e installa la nuova versione senza chiedere"), Station.Cfg.GetB("agg_automatici", true));
            cChk.IsCheckedChanged += (s, e) => { Station.Cfg.Set("agg_controlla", cChk.IsChecked == true); cAuto.IsEnabled = cChk.IsChecked == true; Station.Save(); };
            cAuto.IsCheckedChanged += (s, e) => { Station.Cfg.Set("agg_automatici", cAuto.IsChecked == true); Station.Save(); };
            cAuto.IsEnabled = cChk.IsChecked == true;
            Button bNow = Ui.B(Lang.T("Controlla adesso"));
            bNow.Click += async (s, e) => { await Updater.CheckAsync(); await Updater.ShowDialog(win); };
            root.Children.Add(Section(Lang.T("Aggiornamenti"), Pal.Orange,
                Ui.Row(Ui.L(Lang.F("Versione in uso: {0}", ElaWindow.Version)), bNow), cChk, cAuto,
                Txt(Lang.T("L'aggiornamento automatico vale per l'installatore e lo zip di Windows, l'app del Mac, gli archivi .tar.gz e l'AppImage. Con i pacchetti di sistema (.deb, .rpm, Arch, Slackware) il programma non si aggiorna da solo: segnala la versione nuova, che si installa scaricando il pacchetto nuovo dalla pagina delle Release. Se la cartella del programma non è scrivibile, il programma lo dice e la nuova versione va scaricata a mano."), true)));

            // dati
            Button bData = Ui.B(Lang.T("Apri la cartella dei dati"));
            bData.Click += (s, e) => { try { Dialogs.OpenWithDefaultApp(AppPaths.DataDir); } catch { } };
            root.Children.Add(Section(Lang.T("Dati"), Pal.Silver,
                Ui.Row(bData, Ui.L(AppPaths.DataDir)),
                Txt(Lang.T("Tutti i dati del programma stanno nella cartella data accanto al programma (niente in AppData). Gli spot raccolti si cancellano alla chiusura del programma. La chiave API del Bureau of Meteorology si inserisce in DATA COLLECTION, Dati di propagazione."), true)));
            return root;
        }

        void SetFont(double pct)
        {
            pct = Math.Max(85, Math.Min(140, Math.Round(pct / 5) * 5));
            if (lblFont != null) lblFont.Text = pct.ToString("F0", CultureInfo.InvariantCulture) + " %";
            UiScale.Set(pct / 100.0);
            UiScale.Apply(win);
            try { Config c = new Config(Path.Combine(AppPaths.DataDir, "config.ini")); c.Set("carattere_pct", pct); c.Save(); } catch { }
        }

        void UpdateNtp()
        {
            if (lblNtpInfo == null) return;
            if (Ntp.Ok == null) { lblNtpInfo.Text = Lang.T("Controllo dell'ora in corso"); return; }
            lblNtpInfo.Text = Ntp.Ok == true
                ? Lang.F("NTP:OK - differenza tra l'ora dei server e quella del PC: {0:+0.00;-0.00;0.00} s", Ntp.OffsetSec) + (Ntp.SystemClockSet ? " " + Lang.T("L'orologio del PC è stato corretto.") : "")
                : Lang.T("NTP:FAILED - nessun server NTP raggiungibile: il programma usa l'orologio del PC.");
        }

        // ---------------- Smistamento degli spot
        Control BuildRouting()
        {
            StackPanel head = new StackPanel { Spacing = 4, Margin = new Thickness(10, 8, 10, 4) };
            head.Children.Add(new TextBlock { Text = Lang.T("Smistamento degli spot"), FontSize = 18, FontWeight = FontWeight.Bold, Foreground = Pal.B(Pal.Azure) });
            head.Children.Add(Txt(Lang.T("Ogni spot ricevuto in DATA COLLECTION (cluster, RBN, PSKReporter, WSJT-X) si controlla con le regole nell'ordine della tabella: la prima che vale decide la linguetta. A destra i filtri di ciascun modo, con i valori modificabili e la prova di una riga del cluster."), true));
            tRules = new TableView { Selectable = true };
            tRules.Columns.AddRange(new[] { "#", Lang.T("Se lo spot..."), Lang.T("Valori"), Lang.T("va a") });
            tRules.ColumnWidths.AddRange(new double[] { 28, 240, 330, 190 });
            // la riga scelta si legge per intero sotto la tabella (le celle lunghe sono tagliate)
            lblRule = Txt(Lang.T("Clicca una riga per leggerla per intero."), true);
            lblRule.Margin = new Thickness(10, 6, 10, 8);
            tRules.RowClicked += i => ShowRule(i);
            DockPanel left = new DockPanel { Background = Pal.B(Pal.Back) };
            Ui.Dock(left, head, Dock.Top);
            Ui.Dock(left, lblRule, Dock.Bottom);
            left.Children.Add(tRules);
            Control right = dc.BuildFilters();
            Grid g = new Grid { ColumnDefinitions = new ColumnDefinitions("55*,45*"), Background = Pal.B(Pal.Back) };
            Grid.SetColumn(left, 0); g.Children.Add(left);
            Grid.SetColumn(right, 1); g.Children.Add(right);
            return g;
        }

        void FillRules()
        {
            if (tRules == null) return;
            CultureInfo ci = CultureInfo.CurrentCulture;
            string es = "SPORADIC-E", ms = "METEOR SCATTER", none = Lang.T("nessuna linguetta");
            List<string[]> r = new List<string[]>();
            Color[] col = new Color[20]; int k = 0;
            Action<string, string, string, Color> add = (a, b, c, cc) => { r.Add(new[] { (r.Count + 1).ToString(ci), a, b, c }); col[Math.Min(19, k++)] = cc; };
            add(Lang.T("è fuori dalla gamma del filtro locale"), DataTab.VhfOnly ? Lang.T("sotto 27990 kHz o sopra 145 MHz; in 430-440 MHz solo aurora") : Lang.T("filtro locale spento"), Lang.T("scartato"), Pal.Muted);
            add(Lang.T("contiene AUE, AU-E o AURORAL E"), "", "AURORA (aurora-E)", Pal.Green);
            add(Lang.T("contiene una parola dell'aurora (o modo AU)"), Filters.AuTags + " · " + string.Join(" ", Filters.BandNames).Replace(" ", ", ") , "AURORA", Pal.Green);
            add(Lang.T("contiene una parola del F.A.I. (o modo FAI)"), Filters.FaiTags + " · " + string.Join(", ", Filters.FaiBandNames), "F.A.I.", Pal.Violet);
            add(Lang.T("ha un modo del meteor scatter o <MS>"), "MSK144, FSK441, JTMS, JT6M, PSK2K", ms, Pal.Orange);
            add(Lang.T("cade in una finestra MS del bandplan"), MsCollector.WindowsText + (Filters.Es50WindowNotExclusive ? " · " + Lang.T("in 50 MHz non esclusiva") : "") + " · " + Lang.T("FT8 e FT4 in 50 e 70 MHz esclusi"), ms, Pal.Orange);
            add(Lang.T("è FT8 o FT4 in 50 MHz senza nube"), EsModel.UseFt8CloudRule ? Lang.F("nessun altro spot entro {0:F0} min con il punto medio entro {1:F0} km", EsModel.Ft8CloudMin, EsModel.Ft8CloudKm) : Lang.T("regola spenta"), Lang.T("meteor scatter (non E-sporadico)"), Pal.Orange);
            add(Lang.T("è Q65 o JT65 in 144 MHz"), Lang.T("salvo <ES> nel testo"), Lang.T("EME: scartato"), Pal.Muted);
            add(Lang.T("contiene una scritta di altra propagazione"), Filters.EsTags, Lang.T("non E-sporadico"), Pal.Muted);
            add(Lang.T("è transequatoriale"), EsModel.UseTepRule ? Lang.F("fascia di inclinazione ± {0:F0}°", EsModel.TepInclinationDeg) : Lang.T("regola spenta"), Lang.T("non E-sporadico (TEP)"), Pal.Muted);
            add(Lang.T("è in 28 MHz"), Lang.F("da {0:F0} km", EsModel.MinKm28), es, EsTab.BandColor("10m"));
            add(Lang.T("è in 40 MHz"), Lang.F("da {0:F0} a {1:F0} km (primo salto)", EsModel.MinKm40, EsModel.MaxKm40), es, EsTab.BandColor("8m"));
            add(Lang.T("è in 50 MHz"), Lang.F("da {0:F0} km", EsModel.MinKm50), es, EsTab.BandColor("6m"));
            add(Lang.T("è in 70 MHz"), Lang.F("da {0:F0} km", EsModel.MinKm70), es, EsTab.BandColor("4m"));
            add(Lang.T("è in 144 MHz"), EsModel.Use144Rule ? Lang.F("oltre {0:F0} km, con conferma", EsModel.MinKm144) : Lang.T("regola spenta"), es, EsTab.BandColor("2m"));
            add(Lang.T("non rientra in nessuna regola (tropo, 432 MHz, HF...)"), "", none + " (" + Lang.T("resta in Spot raccolti") + ")", Pal.Muted);
            tRules.Rows.Clear();
            for (int i = 0; i < r.Count; i++) tRules.Rows.Add(new TableRow { Cells = r[i], Fore = col[i], CellTips = new[] { "", r[i][1], r[i][2], r[i][3] } });
            ruleRows = r;
            tRules.Refresh();
            ShowRule(tRules.SelectedIndex);
        }

        void ShowRule(int i)
        {
            if (lblRule == null) return;
            if (i < 0 || i >= ruleRows.Count) { lblRule.Text = Lang.T("Clicca una riga per leggerla per intero."); return; }
            string[] c = ruleRows[i];
            lblRule.Text = c[0] + ". " + Lang.T("Se lo spot...").TrimEnd('.', '…') + " " + c[1] + (c[2] != "" ? " (" + c[2] + ")" : "") + " → " + c[3];
        }
    }
}
