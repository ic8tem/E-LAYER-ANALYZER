# BENCHMARK Es

[← Home](Home-it)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Banco di prova dello **E-sporadico** e del **multi-salto** su un **log ADIF intero**, anche di un'altra stazione: dai QSO fatti davvero il programma abbozza lo skip e le nubi, giornata per giornata e ora per ora.

## Caricare il log

- **Carica ADIF**: sceglie il file `.adi`/`.adif`. Il programma ricorda il file e lo ricarica all'avvio.
- **Scarica ADIF**: toglie il log dalla linguetta.
- **Stazione al centro**: nominativo e locator della stazione che ha fatto il log. Il programma propone lo `STATION_CALLSIGN` (o `OPERATOR`) e il `MY_GRIDSQUARE` più frequenti; si possono cambiare a mano e confermare con **Applica**. Il centro non dipende dalle impostazioni generali, così si possono studiare i log di altre stazioni.
- Posizione di trasmissione: il `MY_GRIDSQUARE` di ogni QSO (se manca, il locator al centro). Posizioni di ricezione: i `GRIDSQUARE`.
- QSO usati: 28, 50, 70 e 144 MHz con locator. Si possono escludere i QSO con un altro `PROP_MODE` (TR, MS, FAI, AUR, EME...) e quelli sotto i 500 km. I 432 MHz non entrano mai: l'E-sporadico non si applica.

## Giornate e orari

- **QSO per giornata**: giornata, numero di QSO, bande, distanza massima, multi-salti. All'apertura il programma propone la giornata con più QSO.
- **Orari della giornata**: la riga *Tutta la giornata* e una riga per ogni ora UTC; il clic mostra solo i QSO di quell'ora.
- **QSO**: l'elenco della selezione; il clic su un QSO (o vicino al corrispondente sulla mappa) lo evidenzia e ne dà i dettagli.

## La mappa

Mappa **azimutale equidistante** centrata sulla stazione, con il cursore **Distanza massima** per lo zoom (anche con la rotella): gli anelli riportano i **km dal centro**.

- **Skip per settori** (10° di azimut): arco arancio = QSO più vicino del settore (bordo della zona di silenzio), arco verde = QSO più lontano, in mezzo il settore raggiunto.
- **Nubi Es** sui quadrati Maidenhead, colorate dalla foEs stimata.
- Punto medio dei singoli salti (giallo), primo salto della stazione (arancio, cerchio vuoto = primo salto corto), primo salto del corrispondente (viola).
- Un **pallino giallo sfocato** segna il centro di ogni salto, dove si presume la nube Es (anche il centro del primo salto corto, se l'ipotesi è attiva).
- Sulla giornata intera, la **linea rossa** unisce il baricentro dei punti medi ora per ora: è lo spostamento delle nubi.

![Un'ora](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Calcoli proposti

1. **Skip per settori**: per ogni settore la distanza dello skip e la portata, con i nominativi.
2. **foEs stimata** con la legge della secante (foEs ≥ f / sec φ): il singolo salto più corto di ogni banda dà la stima più alta; con questa foEs la MUF attesa a 1000, 1500, 2000 e 2400 km e la distanza oltre cui si aprirebbero i 144 MHz.
3. **Nubi Es** stimate sui quadrati, con il numero di QSO e la foEs.
4. **Multi-salto**: il primo salto della stazione nelle due ipotesi (salti uguali e primo salto corto entro 1200 km, esperienza IC8TEM) confrontato con le nubi viste in singolo salto entro ±30 minuti; entro 300 km l'ipotesi è confermata. Il programma conta le conferme di ciascuna ipotesi.
5. **Spostamento delle nubi** ora per ora: km, direzione e velocità.

I calcoli sono indicativi: un QSO dice che la tratta era aperta, non dove stava esattamente la nube. Più i dati sono puliti (locator a 6 caratteri, `PROP_MODE` corretto), più lo skip è sicuro.
