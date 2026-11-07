// E-LAYER ANALYZER - fumetti esplicativi (tooltip) dei comandi.
// Dopo la costruzione della finestra si percorre l'albero dei controlli: a ogni linguetta, pulsante, casella o etichetta
// che mostra una delle scritte qui sotto si aggiunge il suo fumetto; un'etichetta lo passa anche al comando che la segue.
// Le chiavi e i testi sono italiani e si traducono con Lang.T come il resto dell'interfaccia.
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using YAMS;

namespace ELA
{
    public static class Tips
    {
        static readonly Dictionary<string, string> map = new Dictionary<string, string>
        {
            // barra in alto
            { "Nominativo", "Il proprio nominativo: si usa per il login ai cluster e per riconoscere i propri spot." },
            { "Locator", "Locator della stazione (4, 6 o 8 caratteri): è il centro delle mappe e il punto di partenza dei calcoli." },
            { "Quota m", "Quota dell'antenna sul livello del mare, in metri: serve al calcolo dell'orizzonte radio." },
            { "Applica", "Conferma nominativo, locator e quota e aggiorna tutte le linguette." },
            // linguette principali
            { "SPORADIC-E", "E-sporadico in diretta dagli spot dei cluster: salti, nubi e foEs su mappa azimutale centrata sulla stazione." },
            { "BENCHMARK Es", "Analisi di un log ADIF (anche di altre stazioni) per giornate e ore: salti, settori e nubi dell'E-sporadico." },
            { "BENCHMARK 50 MHz", "Multi-salto in 50 MHz (oltre 3100 km) dai log ADIF di una o due stazioni, su mappa di Mercatore a centro libero." },
            { "F.A.I.", "Field Aligned Irregularities: scatter point di una coppia di stazioni con il campo magnetico IGRF-14." },
            { "METEOR SCATTER", "Il programma YA-MS completo: efficienza degli sciami, radar meteorici, traffico MS in diretta." },
            { "AURORA", "Aurora radio: ovale NOAA, indice Kp, scatter point e puntamento delle due stazioni." },
            { "DATA COLLECTION", "Sessioni telnet con i DX cluster, spot raccolti, archivio CALL3 e filtri degli spot di tutti i modi." },
            { "LOG ADIF", "Quadrati lavorati e confermati per banda, da un file ADIF o dal report di LoTW." },
            { "FONTI", "Articoli, pagine, modelli e dati usati dal programma, con i rimandi numerati." },
            { "SINTESI DELLE SCELTE DI CALCOLO", "Formule e scelte di calcolo, con le fonti e le regole dell'autore." },
            { "MANUALE D'USO", "Come si usa il programma, linguetta per linguetta." },
            { "RELAZIONE TECNICA", "Le scelte che hanno portato alla realizzazione del programma." },
            // SPORADIC-E
            { "Primi salti (salti uguali)", "Ipotesi dei salti tutti uguali: i punti di riflessione dividono la tratta in parti uguali." },
            { "Primo salto corto entro", "Ipotesi dell'autore: nel multi-salto il primo salto si chiude entro la distanza indicata dal proprio QTH." },
            { "Nubi Es sui quadrati Maidenhead", "Raccoglie i punti di riflessione in nubi sui quadrati di 4 caratteri e ne stima la foEs minima." },
            { "Quota Es km", "Quota dello strato E sporadico usata nella geometria dei salti (di partenza 105 km)." },
            { "Elev. min °", "Elevazione minima dell'antenna: limita la lunghezza massima di un salto." },
            { "Mappa", "Vista della mappa: azimutale per il singolo salto, azimutale con distanza massima regolabile per il multi-salto, oppure rettangolare." },
            { "Europa", "Riporta la mappa sull'Europa." },
            { "Mondo", "Mostra tutto il mondo." },
            { "Distanza massima", "Raggio della mappa azimutale: chilometri dal centro fino al bordo." },
            // BENCHMARK
            { "Carica ADIF", "Legge un log ADIF: il centro della mappa si ricava dal MY_GRIDSQUARE dei QSO." },
            { "Scarica ADIF", "Toglie il log caricato e pulisce la mappa." },
            { "Escludi i QSO sotto i 500 km (900 km in 144 MHz)", "Sotto queste distanze il collegamento è più probabilmente tropo o aircraft scatter." },
            { "Skip per settori", "Raggruppa i QSO per direzione e mostra lo skip di ogni settore." },
            { "Tratte", "Disegna le linee dei collegamenti." },
            { "Stazione al centro", "Stazione del log: il centro della mappa." },
            { "Giorno", "Giornata del log da mostrare." },
            { "dalle", "Ora UTC di inizio." },
            { "alle", "Ora UTC di fine." },
            { "foEs", "Mostra la foEs minima stimata nei punti di riflessione." },
            { "Stazione 1", "Primo log ADIF (colore azzurro)." },
            { "Stazione 2", "Secondo log ADIF, da confrontare con il primo (colore viola)." },
            { "Torna a giorno e orario", "Esce dalla vista dei dintorni e torna alla giornata e all'orario scelti." },
            { "Primo salto corto entro 1200 km", "Ipotesi dell'autore: nel multi-salto il primo salto si chiude entro 1200 km dal QTH." },
            // F.A.I. e AURORA
            { "Stazione A", "Prima stazione: nominativo e locator." },
            { "Stazione B", "Seconda stazione: nominativo e locator." },
            { "Quota km", "Quota delle irregolarità (F.A.I. circa 110 km, aurora circa 105 km)." },
            { "Vista", "Scatter trail: il punto più probabile; Iso-riflessione: verifica di un punto scelto con un clic." },
            { "Calcola", "Calcola lo scatter point per le due stazioni." },
            { "Rifrazione (raggio terrestre 4/3)", "Applica la rifrazione troposferica con il raggio terrestre equivalente di 4/3." },
            { "Verifica il punto consigliato", "Controlla la condizione di riflessione nel punto proposto." },
            { "Ovale aurorale (NOAA OVATION)", "Sfuma sulla mappa la probabilità di aurora secondo la NOAA." },
            { "Scarica NOAA adesso", "Scarica subito ovale aurorale, indice Kp e vento solare (di norma ogni 5 minuti)." },
            { "Spot <AU>/<AUR> sulla mappa con il calcolo", "Disegna gli spot di aurora recenti con il loro scatter point calcolato." },
            // DATA COLLECTION
            { "Host", "Indirizzo del cluster telnet." },
            { "Porta", "Porta telnet del cluster." },
            { "Login", "Nominativo per l'accesso; se vuoto si usa quello della barra in alto." },
            { "Comandi dopo il login", "Comandi inviati al cluster dopo l'accesso, separati da punto e virgola (per esempio i filtri di banda)." },
            { "Connetti", "Apre la sessione telnet." },
            { "Disconnetti", "Chiude la sessione telnet." },
            { "Riconnetti", "Riapre da sola la sessione dopo una caduta della linea." },
            { "Connetti all'avvio", "Apre la sessione all'avvio del programma." },
            { "Invia", "Invia al cluster il comando scritto nella casella." },
            { "Apri la cartella degli spot", "Apre la cartella con gli spot salvati (un file CSV al giorno)." },
            { "Ricarica", "Rilegge l'archivio CALL3." },
            { "Importa un nuovo CALL3.TXT...", "Sostituisce l'archivio nominativo-locator con un file CALL3.TXT più recente." },
            { "Cerca", "Cerca il nominativo nell'archivio CALL3." },
            { "Fonte", "Sessione da cui arriva lo spot: cluster, RBN o PSKReporter." },
            { "Banda", "Mostra solo gli spot di questa banda." },
            { "Ripristina le regole dell'autore", "Riporta i valori di partenza, cioè le regole dell'autore." },
            { "Distanza minima in 144 MHz (km)", "In 144 MHz l'E-sporadico vale solo oltre questa distanza (regola dell'autore: 900 km)." },
            { "entro (minuti)", "Tempo massimo tra lo spot e la sua conferma." },
            { "nube entro (km)", "Distanza massima tra i punti di riflessione perché due spot siano sulla stessa nube." },
            { "salto corto fino a (km)", "Lunghezza massima di un salto corto in 50 MHz che conferma lo spot in 144 MHz." },
            { "fascia di inclinazione ± (gradi)", "Fascia attorno all'equatore magnetico in cui il punto medio rende la tratta transequatoriale." },
            { "Spot sulla mappa degli ultimi (minuti)", "Periodo degli spot disegnati sulla mappa." },
            // LOG ADIF
            { "Apri file ADIF...", "Legge un file ADIF dal computer." },
            { "Scarica da LoTW...", "Scarica il report dei QSO da LoTW (servono nome utente e password, che non viene salvata)." },
            { "Nome utente LoTW", "Nome utente di Logbook of The World." },
            { "Password LoTW", "Password di LoTW: si usa solo per questo scaricamento e non viene mai salvata." },
            // documenti
            { "Esporta in Word (.docx)...", "Salva il documento in formato Word nella lingua scelta." },
            { "Apri in Word", "Apre il documento in Word o nel programma predefinito." },
        };

        public static IEnumerable<string> Keys { get { return map.Keys; } }   // per l'estrazione delle chiavi

        static Dictionary<string, string> byShown;

        static string Shown(Control c)
        {
            if (c is TextBlock tb) return tb.Text;
            if (c is ContentControl cc)
            {
                if (cc.Content is string s) return s;
                if (cc.Content is TextBlock t) return t.Text;
            }
            return null;
        }

        static bool IsInput(Control c)
        {
            return c is NumericUpDown || c is TextBox || c is ComboBox || c is CheckBox || c is Slider;
        }

        // aggiunge i fumetti a tutto l'albero (YA-MS escluso: ha i suoi)
        public static void Apply(Control root)
        {
            byShown = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kv in map)
            {
                string shown = Lang.T(kv.Key);
                if (!byShown.ContainsKey(shown)) byShown[shown] = Lang.T(kv.Value);
            }
            Walk(root);
        }

        static void Walk(ILogical n)
        {
            if (n is YAMS.MainWindow) return;
            if (n is Control c)
            {
                string s = Shown(c), tip;
                if (s != null && byShown.TryGetValue(s.Trim(), out tip) && ToolTip.GetTip(c) == null)
                {
                    if (c is TextBlock && !(c.Parent is ContentControl))
                    {
                        ToolTip.SetTip(c, tip);
                        // l'etichetta passa il fumetto al comando che la segue
                        if (c.Parent is Panel p)
                        {
                            int i = p.Children.IndexOf(c);
                            if (i >= 0 && i + 1 < p.Children.Count && IsInput(p.Children[i + 1]) && ToolTip.GetTip(p.Children[i + 1]) == null)
                                ToolTip.SetTip(p.Children[i + 1], tip);
                        }
                    }
                    else if (c is ContentControl || c is ToggleButton) ToolTip.SetTip(c, tip);
                }
                else if (s != null && c is TextBlock && c.Parent is ContentControl owner && ToolTip.GetTip(owner) == null && byShown.TryGetValue(s.Trim(), out tip))
                    ToolTip.SetTip(owner, tip);
            }
            foreach (ILogical ch in n.LogicalChildren) Walk(ch);
        }
    }
}
