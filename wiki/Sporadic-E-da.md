# SPORADIC-E

[← Home](Home-da)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kortet

En knap over kortet vælger visningen:

- **Azimutal - ét hop**: azimutalt ækvidistant kort centreret på din station, fast radius 2500 km, med zonerne fra IK3XTV-tabellen (500 km minimum, 1500-2000 km optimum, 2400 km maksimum for ét hop).
- **Azimutal - maksimal afstand (multi-hop)**: samme kort med skyderen **Maksimal afstand** (fra 300 til 20000 km, også med musens scrollhjul); grænserne for dobbelt hop (5000 km) og tredobbelt hop (6900 km) vises også, så man kan følge udviklingen af multi-hop.
- **Rektangulær**: det tidligere verdenskort med knapperne Europa og Verden.

I de azimutale visninger viser de stiplede ringe **km fra centrum** (intervallet skifter med zoomen), kanten markerer den største viste afstand, og musemarkøren angiver afstand og azimut fra centrum.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Hvad den viser

- **Spots** fra de seneste minutter (15 min - 12 t) på båndene **28, 50, 70 og 144 MHz**, taget fra telnet-sessionerne. Lokatorerne kommer fra spottets kommentar (fx `JN70CN<ES>JO62QM`) eller fra CALL3-databasen.
- Spots med **<MS>**, **<AU>** eller **<TR>** i teksten **kasseres**: de vedrører ikke sporadisk E, men er nyttige til meteor scatter, nordlys eller tropo.
- **Transækvatorial, forfatterens regel (IC8TEM)**: mod Afrika og Sydamerika er forbindelsen transækvatorial (TEP) og ikke sporadisk E, hvis midtpunktet falder i båndet med magnetisk inklination mellem -20° og +20°, medmindre spottet angiver `<ES>`.
- **144 MHz, forfatterens regel (IC8TEM)**: sporadisk E på 2 m begynder **over 900 km** (kortere spots kasseres). Et enkelt 144 MHz-spot **tegnes ikke alene**: det vises, når der inden for **5 minutter** kommer et andet 144 MHz-spot med skyen på samme sted (refleksionspunkter inden for 300 km), eller når der i nærheden er et **50 MHz-spot med kort hop** (inden for 1200 km, inden for de seneste 15 minutter). Bjælken øverst viser, hvor mange 144 MHz-spots der afventer bekræftelse.
- For hvert spot: afstand, **antal hop** ifølge **IK3XTV**-tabellen, elevation, **mindste foEs** (sekantloven).
- **Første hop for begge stationer**: orange firkant = spotterens første hop, violet = DX'ens første hop (lige lange hop); gul = midtpunkt for ét hop.
- **Kort første hop** (hule cirkler): ved multi-hop-åbninger har forfatteren oplevet et kort første hop inden for **1200 km** fra sit eget QTH; længden kan indstilles.
- **Es-skyer** på **Maidenhead-felter** (2° x 1°), farvet efter mindste foEs: blå < 5, lyseblå < 7, grøn < 10, gul < 14, orange < 20, rød > 20 MHz.
- Tabellen **Es-skyer**: for hvert felt **MUF for din station** (rød = 144 MHz mulig, orange = 70, gul = 50).

## IK3XTV-tabellen

| | minimum | optimum | maksimum |
|---|---|---|---|
| Ét hop | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dobbelt hop | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Tredobbelt hop | – | 4800-6400 km | 6900 km |

## Sådan bruges den

- **Klik på et spot**: strækningen fremhæves med refleksionspunkter, lokatorer og azimut i detaljerne.
- **Klik på et spot** i tabellen: strækningen lyser op på kortet (gult skær), og hvis den er uden for synsfeltet, flytter kortet sig eller udvider radius for at vise den. Spottet forbliver valgt, når nye spots kommer ind.
- **Tabellen følger kortet**: når der er zoomet, viser spot-tabellen og sky-tabellen kun det, der er synligt på kortet.
- **Klik på kortet**: punktet behandles som en mulig Es-sky med den foEs, der kræves for 28, 50, 70 og 144 MHz.

## Bemærkning om 432 MHz

Sporadisk E **gælder ikke for 432 MHz**: Es-MUF når op på ca. 150-200 MHz, og ARRL-rekorderne opregner kun Es-forbindelser på 50 og 144 MHz. Spots på 70 cm analyseres ikke her.
