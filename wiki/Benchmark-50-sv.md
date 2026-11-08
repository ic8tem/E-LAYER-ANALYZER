# BENCHMARK 50 MHz

[← Home](Home-sv)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Testbänk för **sporadiskt E med multihopp på 50 MHz** utifrån ADIF-loggen från en station och, om du vill, från en andra station.

## Läsa in loggarna

- **Station 1**: *Läs in ADIF* / *Ta bort ADIF*. Programmet hämtar `STATION_CALLSIGN` och `MY_GRIDSQUARE` ur loggen; sändarpositionen är `MY_GRIDSQUARE` för varje QSO.
- **Station 2** (valfri): samma knappar på den andra raden; kryssrutan *Station 2* visar eller döljer den. Sträckorna för station 1 är ljusblå, för station 2 lila.
- Programmet använder QSO på 50 MHz med lokator (saknade lokatorer eller lokatorer med 4 tecken hämtas från CALL3), med multihopp: bortom 3100 km (författarens regel; antalet hopp följer IK3XTV-tabellen).
- **Transekvatoriella** förbindelser utesluts (författarens regel): mot Afrika och Sydamerika, med mittpunkten i bältet med magnetisk inklination mellan -20° och +20°, såvida inte loggen har `PROP_MODE=ES`.

## Dag och tid

Välj **dag** (när fliken öppnas den med flest QSO med multihopp) och tid **från… till…** UTC, i steg om 15 minuter. Kryssrutan **foEs** (valfri) färgar sträckorna efter minsta foEs och visar den i tabellen.

## Kartan

**Mercatorkarta** med fri mittpunkt: hjul = zoom, dra = panorera.

![Sträcka](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Musen nära en sträcka**: linjen och ikonerna förstoras och motstationens **WWL-ruta med 6 tecken** visas.
- **Vänsterklick** på den sträckan: en dialogruta frågar om alla **QSO i filen** (alla dagar) ska visas där den andra ändens position ligger **inom 150 km** från den punkten. *Ja*: kartan rensas och visar de QSO:na inom den orange cirkeln; *Nej*: kartan förblir som den är. *Tillbaka till dag och tid* återgår till det tidigare urvalet.

![Närheten](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detaljer

För den valda sträckan: avstånd och bedömning enligt IK3XTV-tabellen, azimut i båda riktningarna, lika hopp med rutorna för reflexionspunkterna, kort första hopp inom 1200 km (IC8TEM:s erfarenhet) och, med kryssrutan foEs, minsta foEs.
