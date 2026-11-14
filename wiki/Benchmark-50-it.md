# BENCHMARK 50 MHz

[← Home](Home-it)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Banco di prova dell'**E-sporadico multi-salto in 50 MHz** sul log ADIF di una stazione e, se si vuole, di una seconda stazione.

## Caricare i log

- **Stazione 1**: *Carica ADIF* / *Scarica ADIF*. Il programma prende `STATION_CALLSIGN` e `MY_GRIDSQUARE` dal log; la posizione di trasmissione è il `MY_GRIDSQUARE` di ogni QSO.
- **Stazione 2** (facoltativa): stessi pulsanti nella seconda riga; la casella *Stazione 2* la mostra o la nasconde. Le tratte della stazione 1 sono azzurre, quelle della stazione 2 viola.
- Si usano i QSO in 50 MHz con locator (i locator mancanti o a 4 caratteri si prendono da CALL3), multi-salto: oltre i 3100 km (regola dell'autore; il numero dei salti segue la tabella IK3XTV).
- I collegamenti **transequatoriali** si escludono (regola dell'autore): verso l'Africa e il Sud America, con il punto medio nella fascia di inclinazione magnetica tra -15° e +15°, salvo `PROP_MODE=ES` nel log.

## Giorno e orario

Si sceglie il **giorno** (all'apertura quello con più QSO multi-salto) e l'orario **dalle… alle…** UTC, a passi di 15 minuti. La casella **foEs** (facoltativa) colora le tratte con la foEs minima e la riporta in tabella.

## La mappa

Mappa di **Mercatore** a centro libero: rotella = zoom, trascina = sposta.

![Tratta](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- Un **pallino giallo sfocato** segna il centro di ogni salto, dove si presume la nube Es; sulla tratta sotto il mouse diventa più grande.
- **Mouse vicino a una tratta**: linea e icone si ingrandiscono e compare il **quadrato WWL a 6 caratteri** del corrispondente.
- **Clic sinistro** su quella tratta: un popup chiede se vedere **tutti i QSO del file** (tutte le giornate) con la posizione terminale **entro 150 km** da quel punto. *Sì*: la mappa si svuota e mostra quei QSO dentro il cerchio arancio; *No*: la mappa resta com'è. *Torna a giorno e orario* riporta alla scelta precedente.

![Dintorni](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Dettagli

Per la tratta scelta: distanza e giudizio della tabella IK3XTV, azimut nei due sensi, salti uguali con i quadrati dei punti di riflessione, primo salto corto entro 1200 km (esperienza IC8TEM) e, con la casella foEs, la foEs minima.
