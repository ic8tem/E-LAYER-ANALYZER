# IONOSONDE

[← Home](Home-it)

![IONOSONDE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Sottolinguetta di **SPORADIC-E** (dalla versione 0.5.2): la **foEs misurata** dalle ionosonde della rete **GIRO** in tutto il mondo, accanto a quella che il programma ricava dagli spot.

## I dati

- 62 ionosonde GIRO (elenco ULCAR), con l'ultima **foEs**, **foF2** e **MUF(D)** lette in automatico dagli ionogrammi.
- I dati arrivano la prima volta che si apre la linguetta e poi **ogni 15 minuti**, da soli (casella *Aggiorna da solo ogni 15 minuti*); *Aggiorna adesso* li chiede subito. L'archivio tiene le ultime 48 ore nella cartella `giro` dei dati.
- Dati di **GIRO** (Reinisch e Galkin 2011), licenza **CC BY-NC-SA 4.0**: solo uso non commerciale. Il servizio è lo stesso di *MUF Muncher* di DM6AX.

## Mappa e tabella

- Ogni ionosonda è un pallino colorato con l'ultima foEs (stessi colori delle nubi Es) e il valore in MHz; **grigia** se non ha dati recenti, con l'**anello** se l'ultimo ionogramma ha più di 90 minuti.
- Tabella: Codice, Ionosonda, km dalla tua stazione, UTC dell'ultimo ionogramma, foEs, foF2, MUF(D) e **Spot foEs≥**: la foEs minima ricavata dagli spot Es degli ultimi 30 minuti che riflettono entro 500 km dalla ionosonda (regolabile), con il numero degli spot.
- **h'Es GIRO** (dalla versione 0.6.5, casella in REAL-TIME): la quota Es diventa la mediana delle h'Es recenti delle ionosonde entro 2500 km (80-120 km).

## La ionosonda scelta

Clic su una riga o vicino a una ionosonda sulla mappa:

- **grafico delle ultime 24 ore**: foEs gialla, foF2 azzurra, MUF(D) viola a puntini;
- **MUF dell'E-sporadico** a 1000, 1500, 2000 e 2400 km (MUF = foEs · sec φ, alla quota Es scelta) con le **bande che si aprono** (28, 40, 50, 70, 144 MHz);
- se gli spot chiedono più della foEs misurata, il programma lo segnala: la nube può essere lontana dalla ionosonda, oppure la ionosonda è arrivata al limite del suo campo di frequenza (Chartier 2022).
