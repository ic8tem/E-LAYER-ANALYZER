# NUBI Es NEL TEMPO

[← Home](Home-it)

![NUBI Es NEL TEMPO](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Sottolinguetta di **SPORADIC-E** (dalla versione 0.5.2): come nascono, si spostano e si spengono le **nubi di E-sporadico**, passo per passo.

## Il metodo

Metodo di **Chartier, Hanley ed Emmons (2022)**: ogni collegamento E-sporadico dice che nel punto di riflessione la foEs è **almeno f / sec φ**. I punti si raccolgono in celle e in passi di tempo, e ogni cella prende il **valore più alto** del passo. La densità elettronica minima è NmE = 1,24·10¹⁰ · foEs² elettroni al metro cubo.

Il programma usa i punti di riflessione del proprio modello (tutti i salti), non solo il punto medio. Il valore è un **limite inferiore**: una cella vuota vuol dire soltanto nessun collegamento.

## Sorgente e comandi

- **Spot della sessione** (si aggiornano a ogni apertura della linguetta) oppure **Carica log ADIF o spot CSV**.
- **Esporta gli spot della sessione (CSV)**: l'archivio degli spot si svuota alla chiusura del programma, così li puoi rivedere più tardi.
- **Bande MHz** (28, 40, 50, 70, 144), **Passo** (5, 10 o 15 minuti), **Cella** (1°, 2° o 4°), **Quota Es km**.
- Barra del tempo: ⏮ ◀ *Avvia/Ferma* ▶ ⏭ e il cursore; all'apertura si parte dal passo con più celle.
- *Passi precedenti sbiaditi* mostra i tre passi prima di quello scelto; *Collegamenti* disegna le tratte con i colori delle bande.

## Tabella e grafico

- Celle del passo: Cella (angolo sud-ovest e locator del centro), foEs≥, NmE≥, Collegamenti, Bande. Clic su una cella: ne evidenzia i collegamenti.
- Grafico: foEs minima più alta (linea gialla) e numero di celle (barre) per ogni passo.
