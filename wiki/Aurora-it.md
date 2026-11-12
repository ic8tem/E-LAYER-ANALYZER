# AURORA

[← Home](Home-it)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Ovale aurorale** NOAA OVATION Prime (sfumato), **Kp** degli ultimi 3 giorni, **vento solare** (Bz, velocità, densità), aggiornati ogni 5 minuti e salvati in `data/swpc`.
- Le stesse viste del F.A.I.: **Scatter trail** per una coppia di stazioni e **Iso-riflessione** per verificare un punto, con quota di partenza 110 km.
- **Puntamento consigliato per entrambe le stazioni**: lungo la linea di perpendicolarità di ogni stazione (puntini del suo colore), il punto con la probabilità d'ovale più alta. Regola pratica (G3WPO): partire da circa 45°, tra nord ed est.
- Esempio IO70VJ - JO59BC: scatter point JP01AK al largo delle Shetland, deviazione 0,97°, QTF 11° e 299°.
- **Spot di aurora sulla mappa**: gli spot con **<AU>** o **<AUR>** nel testo sono di aurora (regola dell'autore). Con la casella *Spot <AU>/<AUR> sulla mappa con il calcolo*, la linguetta disegna gli spot delle ultime 2 ore (al massimo 30): per ogni coppia spotter-DX calcola la scatter trail e mostra lo scatter point più probabile (cerchio arancio), con QTF ed elevazione delle due stazioni nei risultati.
- Oltre allo scatter point più probabile, i risultati riportano l'**intervallo di QTF** delle due stazioni lungo tutta la Scatter trail (deviazione ≤ 1,5°).
- **Finestra di Lange-Hesse**: lo scatter point si preferisce entro 370-1100 km da ciascuna stazione, la finestra calcolata da Lange-Hesse a 54° di latitudine geomagnetica. Dalla versione 0.4.5 la finestra si sposta con l'inclinazione IGRF-14 di ogni stazione: più vicina per le stazioni più a sud, più lontana per quelle più a nord (per esempio Capri circa 260-990 km, Oslo 470-1200 km). I risultati riportano la finestra di entrambe le stazioni.

## BENCHMARK AURORA

La linguetta AURORA ha due sottolinguette: **REAL-TIME** (il calcolo descritto sopra) e **BENCHMARK AURORA**, un banco di prova del modello su **298 QSO di aurora reali** con ora e locator: i log pubblicati di EI5FK (IO51RT, Irlanda, 2003-2005, 144 e 432 MHz) e la mappa del log di PA0O (JO33HG, Paesi Bassi).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Per ogni QSO il programma calcola lo scatter point con lo stesso modello (IGRF-14, quota 110 km, rifrazione 4/3, tolleranza 3°).
- Colori: **verde** = scatter point dentro la finestra di Lange-Hesse di entrambe le stazioni; **giallo** = spiegato ma fuori da questa finestra; **arancio** = QSO oltre 2000 km; **rosso** = nessuno scatter point, QSO non spiegato dal modello.
- Il riepilogo dà le percentuali, anche con la finestra fissa 370-1100 km per confronto, e, per ogni log, l'intervallo e la mediana del QTF calcolato. Il clic su una riga disegna la tratta e i due puntamenti.
- Risultato (versione 0.4.4): **296 QSO spiegati su 298 (99%)**, 243 (82%) dentro la finestra 370-1100 km; QTF mediano di EI5FK tra 25° e 41°, cioè il nord-est consigliato da G3WPO. I log non riportano il puntamento: il confronto è sulla geometria, non sul QTF. Con la finestra mobile (versione 0.4.5) il risultato è lo stesso, 243 QSO (82%), come con la finestra fissa: le stazioni del banco di prova (Irlanda, Gran Bretagna, Paesi Bassi, Germania) hanno un'inclinazione di 66-70°, vicina al riferimento di Lange-Hesse (finestra di EI5FK 350-1080 km); la differenza riguarda le stazioni più a sud o più a nord, per le quali il banco di prova non ha QSO.
