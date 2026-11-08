# SPORADIC-E

[← Home](Home-sv)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kartan

En knapp ovanför kartan väljer vy:

- **Azimutal - enkelt hopp**: azimutal ekvidistant karta centrerad på din station, fast radie 2500 km, med zonerna i IK3XTV-tabellen (500 km minimum, 1500-2000 km optimum, 2400 km maximum för enkelhopp).
- **Azimutal - största avstånd (multihopp)**: samma karta med reglaget **Största avstånd** (från 300 till 20000 km, även med mushjulet); gränserna för dubbelhopp (5000 km) och trippelhopp (6900 km) visas också, så att multihoppets utveckling kan följas.
- **Rektangulär**: den tidigare världskartan, med knapparna Europa och Världen.

I de azimutala vyerna visar de streckade ringarna **km från mitten** (steget ändras med zoomen), kanten markerar det största visade avståndet, och muspekaren anger avstånd och azimut från mitten.

![Multihopp](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Vad den visar

- **Spotsen** från de senaste minuterna (15 min - 12 h) på banden **28, 50, 70 och 144 MHz**, hämtade från telnetsessionerna. Lokatorerna kommer från spotens kommentar (t.ex. `JN70CN<ES>JO62QM`) eller från databasen CALL3.
- Spots med **<MS>**, **<AU>** eller **<TR>** i texten **kasseras**: de gäller inte sporadiskt E men är användbara för meteorscatter, aurora eller tropo.
- **Transekvatoriell utbredning, författarens regel (IC8TEM)**: mot Afrika och Sydamerika, om mittpunkten hamnar i bältet med magnetisk inklination mellan -20° och +20°, är förbindelsen transekvatoriell (TEP) och inte sporadiskt E, såvida spoten inte anger `<ES>`.
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
