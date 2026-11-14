# SPORADIC-E

[← Home](Home-sv)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kartan

En knapp ovanför kartan väljer vy:

- **Azimutal**: azimutal ekvidistant karta centrerad på din station, med reglaget för **zoom** (300 till 20000 km från mitten, även med mushjulet).
- **Mercator**: världskartan, med knapparna Europa och Världen.

I båda projektionerna ritas **avståndscirklarna enligt Flavio IK3XTV** runt stationen: 500 km (minimum för enkelhopp), 1500-2000 km (optimum), 2400 km (maximum för enkelhopp), 5000 km (maximum för dubbelhopp) och 6900 km (maximum för trippelhopp).

I de azimutala vyerna visar de streckade ringarna **km från mitten** (steget ändras med zoomen), kanten markerar det största visade avståndet, och muspekaren anger avstånd och azimut från mitten.

![Multihopp](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Vad den visar

- **Spotsen** från de senaste minuterna (15 min - 12 h) på banden **28, 40 (8 m, avstängt från början), 50, 70 och 144 MHz**, hämtade från telnetsessionerna. Lokatorerna kommer från spotkommentaren (t.ex. `JN70CN<ES>JO62QM`) eller från CALL3-databasen.
- Spots med **<MS>**, **<AU>** eller **<TR>** i texten **kasseras**: de gäller inte sporadiskt E men är användbara för meteorscatter, aurora eller tropo.
- **Transekvatoriell utbredning, författarens regel (IC8TEM)**: mot Afrika och Sydamerika, om mittpunkten hamnar i bältet med magnetisk inklination mellan -15° och +15° (värde hämtat från Cracknell, UKSMG: TEP-zoner cirka 5-15° från linjen med inklination noll), är förbindelsen transekvatoriell (TEP) och inte sporadiskt E, såvida spoten inte anger `<ES>`.
- **144 MHz, författarens regel (IC8TEM)**: sporadiskt E på 2 m börjar **bortom 900 km** (kortare spots kasseras). En ensam spot på 144 MHz **ritas inte ut för sig själv**: den visas när det inom **5 minuter** kommer en andra spot på 144 MHz med molnet på samma ställe (reflexionspunkter inom 300 km), eller när det finns en spot med **kort hopp på 50 MHz** i närheten (inom 1200 km, under de senaste 15 minuterna). Raden högst upp visar hur många spots på 144 MHz som väntar på bekräftelse.
- För varje spot: avstånd, **antal hopp** enligt **IK3XTV**-tabellen, elevation, **minsta foEs** (sekantlagen).
- **Båda stationernas första hopp**: orange ruta = spotterns första hopp, lila = DX-stationens första hopp (lika hopp); gul = mittpunkt för ett enkelhopp.
- **Kort första hopp** (ihåliga cirklar): vid multihoppsöppningar har författaren erfarit ett kort första hopp inom **1200 km** från sitt QTH; längden kan ställas in.
- **Es-moln** på **Maidenhead-rutor** (2° x 1°), färgade efter minsta foEs: blå < 5, ljusblå < 7, grön < 10, gul < 14, orange < 20, röd > 20 MHz.
- Tabellen **Es-moln**: för varje ruta **MUF för din station** (röd = 144 MHz möjligt, orange = 70, gul = 50).

## IK3XTV-tabellen

| | minimum | optimum | maximum |
|---|---|---|---|
| Enkelhopp | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dubbelhopp | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Trippelhopp | – | 4800-6400 km | 6900 km |

## Så använder du den

- **Klicka på en spot**: sträckan markeras, med reflexionspunkter, lokatorer och azimut i detaljerna.
- **Klicka på en spot** i tabellen: sträckan lyser upp på kartan (gult sken) och, om den ligger utanför vyn, flyttas kartan eller vidgas radien för att visa den. Spoten förblir vald när nya spots kommer in.
- **Tabellen följer kartan**: med en inställd zoom visar spottabellen och molntabellen bara det som syns på kartan.
- **Klicka på kartan**: punkten behandlas som ett möjligt Es-moln, med den foEs som krävs för 28, 50, 70 och 144 MHz.

## Anmärkning om 432 MHz

Sporadiskt E **gäller inte 432 MHz**: Es-MUF når omkring 150-200 MHz och ARRL:s rekordlistor tar bara upp Es-förbindelser på 50 och 144 MHz. Spots på 70 cm analyseras inte här.

## Rutor färgade efter frekvens och blinkande MUF

Maidenhead-rutorna för reflexionspunkterna färgas efter spotsens högsta frekvens: **grönt** upp till 29 MHz, **gult** upp till 51, **orange** upp till 71, **rött** upp till 145 MHz. Ur rutans foEs uppskattas den troliga MUF:en på det längsta hoppet (2400 km, IK3XTV-tabellen): om den når ett högre band **blinkar** rutan i det bandets färg (princip enligt IK3XTV och Jacobs-Leinwoll: kort skip betyder hög MUF). För 144 MHz gäller författarens regel: rutan blinkar rött bara om molnet ger ett enkelhopp på 50 MHz mellan 450 och 500 km (450 km med en tolerans på 50 km).

Uppe till höger på kartan visas sammanfattningen från N0NBH (E-skip för den valda kontinenten, SFI, K- och A-index).

## Regler per band (version 0.4.18)

- **50 MHz**: inget sporadiskt E inom 450 km. FT8- och FT4-spots är sporadiskt E bara om de ingår i ett moln: en annan 50 MHz-spot inom 4 minuter med mittpunkten inom 150 km. En isolerad FT8/FT4-spot går till meteor scatter.
- **70 MHz**: inget sporadiskt E inom 450 km. FT8 och FT4 går till sporadiskt E även inom bandplanens meteor scatter-fönster (den kända moden avgör först, sedan frekvensen).
- **40 MHz (8 m)**: minsta avstånd härlett ur 50 MHz-gränsen 450 km med sekantlagen (cirka 330 km); bara avstånd inom det första hoppet i E-skiktets geometri (cirka 2440 km). Sträckans färg: gul.
- **144 MHz**: sporadiskt E från 900 km, med bekräftelse; Q65- och JT65-spots är EME och förkastas, utom med `<ES>` i texten.
- G0ISW (Propagation identification, 2024) ser det kortaste sporadiska E över 400 km på 50 och 70 MHz och över 1400 km på 144 MHz: författarens regler stämmer på 50 och 70 MHz och är vidare på 144 MHz, men kräver en bekräftelse.
- Ordningen som alla regler tillämpas i, med de värden som används, visas i **[INSTÄLLNINGAR](Settings-sv)** › Fördelning av spots.
