# SPORADIC-E

[← Home](Home-it)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Cosa mostra

- Gli **spot** degli ultimi minuti (15 min - 12 h) sulle bande **28, 50, 70 e 144 MHz**, presi dalle sessioni telnet. I locator vengono dal commento dello spot (es. `JN70CN<ES>JO62QM`) o dall'archivio CALL3.
- Per ogni spot: distanza, **numero di salti** secondo la tabella di **IK3XTV**, elevazione, **foEs minima** (legge della secante).
- **Primo salto di entrambe le stazioni**: quadratino arancio = primo salto dello spotter, viola = primo salto del DX (salti uguali); giallo = punto medio del singolo salto.
- **Primo salto corto** (cerchi vuoti): l'autore ha esperienza, nel multi-salto, di un primo salto corto entro i **1200 km** dal proprio QTH; la lunghezza si imposta.
- **Nubi Es** sui **quadrati Maidenhead** (2° x 1°), colorate dalla foEs minima: blu < 5, azzurro < 7, verde < 10, giallo < 14, arancio < 20, rosso > 20 MHz.
- Tabella **Nubi Es**: per ogni quadrato la **MUF per la tua stazione** (rosso = 144 MHz possibile, arancio = 70, giallo = 50).

## Tabella di IK3XTV

| | minima | ottimale | massima |
|---|---|---|---|
| Singolo salto | 500-650 km | 1500-2000 km | 2200-2400 km |
| Doppio salto | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Triplo salto | – | 4800-6400 km | 6900 km |

## Uso

- **Clic su uno spot**: tratta evidenziata, punti di riflessione con locator e azimut nei dettagli.
- **Clic sulla mappa**: il punto come possibile nube Es, con la foEs necessaria per 28, 50, 70 e 144 MHz.

## Nota sui 432 MHz

Lo sporadico E **non si applica ai 432 MHz**: la MUF dell'Es arriva a circa 150-200 MHz e i record ARRL riportano collegamenti in Es solo su 50 e 144 MHz. Gli spot in 70 cm non vengono analizzati qui.
