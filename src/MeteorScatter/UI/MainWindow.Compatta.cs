// YA-MS - modalita' compatta per schermi bassi, per esempio i notebook da 13-15 pollici (richiesta dell'autore, IC8TEM,
// 15/11/2026: su 1366x768 righe dei comandi, riga dell'orario e fascia degli orizzonti prendevano quasi 300 pixel).
// Impostazioni > Aspetto > "Modalita' compatta": automatica (default), sempre, mai.
//  - automatica: si accende quando l'area della finestra e' alta meno di 850 pixel e si spegne sopra 900 (isteresi);
//  - all'avvio in modalita' compatta le righe dei comandi partono ripiegate (si riaprono con "Comandi");
//  - fascia degli orizzonti su due righe brevi (ora e indice; lato, sciame, riflessioni/ora), il testo completo nel
//    fumetto; "ora migliore" e "sciame piu' favorevole" al massimo due righe;
//  - Traffico in diretta: le descrizioni lunghe della colonna destra spariscono (restano nel fumetto del titolo), cosi'
//    la tabella prende tutta l'altezza che resta.
using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace YAMS
{
    public partial class MainWindow
    {
        public const double CompactOn = 850, CompactOff = 900;
        bool compact, compactDecided;
        readonly List<Control> compactHidden = new List<Control>();

        int CompactMode { get { return (int)cfg.GetD("modalita_compatta", 0); } }   // 0 automatica, 1 sempre, 2 mai

        Control compactRoot;

        void InitCompact(Control root)
        {
            compactRoot = root;
            root.SizeChanged += (s, e) => EvalCompact(e.NewSize.Height);
        }

        // E-LAYER ANALYZER (tools/sync_yams.py): dentro E-LAYER la modalita' compatta la decide la finestra di E-LAYER
        // (altezza della finestra intera, impostazione in IMPOSTAZIONI > Aspetto) e arriva qui
        public void HostCompact(bool on)
        {
            bool first = !compactDecided;
            compactDecided = true;
            if (first && on) SetRowsVisible(false, false);   // solo la prima volta: poi decide "Comandi"
            if (on != compact || first) ApplyCompact(on);
        }

        void EvalCompact(double h)
        {
            if (h <= 0) return;
            int m = CompactMode;
            bool on = m == 1 ? true : m == 2 ? false : (compact ? h < CompactOff : h < CompactOn);
            bool first = !compactDecided;
            compactDecided = true;
            if (first && on && m != 2) SetRowsVisible(false, false);   // solo all'avvio: poi decide "Comandi"
            if (on != compact || first) Dispatcher.UIThread.Post(() => ApplyCompact(on));
        }

        void ApplyCompact(bool on)
        {
            compact = on;
            foreach (Control c in compactHidden) c.IsVisible = !on;
            foreach (TextBlock t in new[] { glBest, glShower })
                if (t != null) { t.MaxLines = on ? 2 : 0; t.TextTrimming = on ? TextTrimming.CharacterEllipsis : TextTrimming.None; }
            UpdateHorizons();
        }
    }
}
