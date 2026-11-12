# METEOR SCATTER

[← Home](Home-it)

La linguetta contiene **YA-MS 1.8.3** completo, il programma di IC8TEM per il meteor scatter: volta celeste con i radianti, flusso dei radar SKiYMET (Collm, Sodankylä), calcolo dell'efficienza degli sciami, attuazione sperimentale delle previsioni, geometria e attenuazione, GRAVES, mappa dei quadrati, **Sciami in diretta** (ZHR misurati dal Global Meteor Network e radianti del radar CMOR), **Mappa delle fonti** (Collm, Sodankylä, GRAVES e CMOR rispetto alla stazione) e il documento **Spiegazione del calcolo dell'efficienza** con le formule.

YA-MS 1.5.0 aggiunge la linguetta **Raccolta dati** (spot meteor scatter filtrati con MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro E-LAYER ANALYZER non apre sessioni telnet proprie: riceve le righe delle sessioni di DATA COLLECTION, così non ci sono doppi login con lo stesso nominativo.

YA-MS 1.6.0 riorganizza le linguette in **Live e dati real-time** (traffico meteor scatter in diretta con la nuvola di ogni contatto, flusso radar, GRAVES, raccolta dati) e **Calcolo previsionale** (calcolo previsionale, geometria, calcolo dell'efficienza, attuazione delle previsioni).

YA-MS 1.7.0 e 1.8.0 aggiungono il confronto dei radianti con il catalogo IAU MDC, i conteggi giornalieri del Global Meteor Network per sciame, l'indice k ricavato dagli indici di popolazione, il raggio iniziale della scia di Stober e altri (2021), la durata dei ping tra le bande (λ² di partenza o misure HamSCI MSQP di NN4SA) e il confronto con i rilevamenti BRAMS di Sonneberg (Perseidi 2026). L'aggiornamento automatico di YA-MS è spento dentro E-LAYER ANALYZER, che si aggiorna da sé dalle Release di GitHub.

YA-MS 1.8.1 tratta il lobo d'antenna come diagramma verticale (casella «diagramma d'antenna», accesa di partenza): i punti di riflessione pesano con il guadagno verticale delle due antenne e le efficienze non spariscono più sulle tratte lunghe. Aggiunge l'avviso «nessuna zona comune», mette in evidenza la casella dell'angolo β (la scelta che pesa di più), usa una sola data del calcolo, porta il «Manuale d'uso» e il confronto con 181 767 spot reali su 50 MHz del Meteor Scatter QSO Party HamSCI dell'agosto 2025: le ore e i lati della tratta sono affidabili, le percentuali sono indicative.

- La lingua e lo schema di colori di YA-MS sono quelli scelti in alto in E-LAYER ANALYZER.
- Le impostazioni di YA-MS stanno in `data/config.ini`.
- Ogni nuova versione di YA-MS si porta dentro con lo script `tools/sync_yams.py`.

Per i dettagli vedi il [repository di YA-MS](https://github.com/ic8tem/YA-MS).
