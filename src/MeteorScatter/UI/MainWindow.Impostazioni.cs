// YA-MS - linguetta "Impostazioni": tutto cio' che riguarda il funzionamento generale del programma, comprese le
// impostazioni di RMOB (richiesta dell'autore, IC8TEM, 14/11/2026). I parametri del calcolo (stazione, antenna, quota
// delle scie, Hot) restano nelle righe dei comandi sopra le linguette.
//  - Aspetto: carattere (spostato dalla prima riga), tema e lingua (gli stessi della barra in alto);
//  - Aggiornamenti del programma: controllo su GitHub e aggiornamento automatico silenzioso;
//  - Fonti in diretta: aggiornamento automatico ogni 5 minuti (spostato dalla linguetta Flusso radar);
//  - RMOB: osservatore, scarico adesso e prelievo automatico (spostati da Attuazione sperimentale previsioni);
//  - Righe dei comandi e cartella dei dati.
// Dentro E-LAYER ANALYZER tema, lingua e aggiornamenti si scelgono nella finestra di E-LAYER: qui non compaiono.
using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        Border SettingsSection(string title, Color accent, string note, params Control[] rows)
        {
            StackPanel sp = new StackPanel { Spacing = 6 };
            sp.Children.Add(new TextBlock { Text = title, FontSize = 16, FontWeight = FontWeight.Bold, Foreground = Pal.B(accent) });
            if (!string.IsNullOrEmpty(note))
                sp.Children.Add(new TextBlock { Text = note, TextWrapping = TextWrapping.Wrap, FontSize = 12.5, Foreground = Pal.B(Pal.Muted) });
            foreach (Control r in rows) if (r != null) sp.Children.Add(r);
            return new Border
            {
                Background = Pal.B(Pal.Panel), BorderBrush = Pal.B(Pal.Border), BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8), Padding = new Thickness(12, 10), Margin = new Thickness(10, 8, 10, 0), Child = sp
            };
        }

        static Control Detach(Control c)
        {
            if (c != null && c.Parent is Panel pp) pp.Children.Remove(c);
            return c;
        }

        void BuildSettingsTab()
        {
            DockPanel p = Page(Lang.T("Impostazioni"));
            StackPanel col = new StackPanel { Spacing = 0, MaxWidth = 980, HorizontalAlignment = HorizontalAlignment.Left };

            // --- Aspetto: carattere, tema e lingua
            List<Control> look = new List<Control>();
            look.Add(Row(L(Lang.T("Carattere")), Detach(sldFont), Detach(lblFont)));
            if (!Hosted)
            {
                ComboBox th = new ComboBox { ItemsSource = cmbTheme.ItemsSource, SelectedIndex = cmbTheme.SelectedIndex, MinWidth = 180 };
                th.SelectionChanged += (s, e) => { if (th.SelectedIndex >= 0 && th.SelectedIndex != cmbTheme.SelectedIndex) cmbTheme.SelectedIndex = th.SelectedIndex; };
                ComboBox lg = new ComboBox { ItemsSource = cmbLang.ItemsSource, SelectedIndex = cmbLang.SelectedIndex, MinWidth = 180 };
                lg.SelectionChanged += (s, e) => { if (lg.SelectedIndex >= 0 && lg.SelectedIndex != cmbLang.SelectedIndex) cmbLang.SelectedIndex = lg.SelectedIndex; };
                look.Add(Row(L(Lang.T("Tema")), th, L(Lang.T("Lingua dell'interfaccia")), lg));
            }
            col.Children.Add(SettingsSection(Lang.T("Aspetto"), Pal.Azure, Lang.T("Tema e lingua sono anche in alto a destra, accanto alle linguette."), look.ToArray()));

            // --- Aggiornamenti del programma (dentro E-LAYER ANALYZER li gestisce E-LAYER)
            if (!Hosted)
            {
                CheckBox chkCheck = C(Lang.T("Controlla su GitHub se c'è una versione nuova (all'avvio e ogni 30 minuti)"), cfg.GetB("controlla_aggiornamenti", true));
                CheckBox chkSilent = C(Lang.T("Aggiornamenti automatici e silenziosi: scarica, installa e riavvia senza chiedere"), cfg.GetB("aggiornamenti_automatici", true));
                chkCheck.IsCheckedChanged += (s, e) => { cfg.Set("controlla_aggiornamenti", chkCheck.IsChecked == true); chkSilent.IsEnabled = chkCheck.IsChecked == true; };
                chkSilent.IsCheckedChanged += (s, e) => cfg.Set("aggiornamenti_automatici", chkSilent.IsChecked == true);
                chkSilent.IsEnabled = chkCheck.IsChecked == true;
                Button now = B(Lang.T("Controlla adesso"));
                now.Click += (s, e) => CheckUpdatesManual();
                col.Children.Add(SettingsSection(Lang.T("Aggiornamenti del programma"), Pal.Green,
                    Lang.T("Senza aggiornamenti automatici, quando esce una versione nuova compare una finestra con «Aggiorna adesso», «Più tardi» e «Salta questa versione»."),
                    Row(chkCheck), Row(chkSilent), Row(now)));
            }

            // --- Fonti in diretta
            col.Children.Add(SettingsSection(Lang.T("Fonti in diretta"), Pal.Orange,
                Lang.T("Radar di Collm e Sodankylä, Global Meteor Network, CMOR e, se scelto, RMOB."),
                Row(Detach(chkAuto))));

            // --- RMOB
            col.Children.Add(SettingsSection("RMOB", Pal.Violet,
                Lang.T("Scrivi il nome dell'osservatore RMOB come compare nel nome del suo file (es. NOME_112026rmob.TXT -> NOME)."),
                Detach(rmobRow)));

            // --- Righe dei comandi e cartella dei dati
            CheckBox chkRows = C(Lang.T("Mostra le righe dei comandi sopra le linguette (lo stesso del pulsante «Comandi»)"), topRows == null || topRows[0].IsVisible);
            chkRows.IsCheckedChanged += (s, e) => SetRowsVisible(chkRows.IsChecked == true, true);
            SelectableTextBlock dir = new SelectableTextBlock { Text = dataDir, Foreground = Pal.B(Pal.Fore), FontFamily = Pal.Mono, FontSize = 12.5, TextWrapping = TextWrapping.Wrap };
            col.Children.Add(SettingsSection(Lang.T("Programma"), Pal.Muted, null,
                Row(chkRows), Row(L(Lang.T("Cartella dei dati")), dir)));

            col.Children.Add(new Border { Height = 12 });
            p.Children.Add(new ScrollViewer { Content = col, HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto });
        }
    }
}
