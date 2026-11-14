# SPORADIC-E

[← Home](Home-da)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kortet

En knap over kortet vælger visningen:

- **Azimutal**: azimutalt ækvidistant kort centreret på din station, med **zoom**-skyderen (300 til 20000 km fra centrum, også med musens scrollhjul).
- **Mercator**: verdenskortet med knapperne Europa og Verden.

I begge projektioner tegnes **afstandscirklerne fra Flavio IK3XTV** rundt om stationen: 500 km (minimum for ét hop), 1500-2000 km (optimum), 2400 km (maksimum for ét hop), 5000 km (maksimum for dobbelt hop) og 6900 km (maksimum for tredobbelt hop).

I de azimutale visninger viser de stiplede ringe **km fra centrum** (intervallet skifter med zoomen), kanten markerer den største viste afstand, og musemarkøren angiver afstand og azimut fra centrum.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Hvad den viser

- **Spots** fra de seneste minutter (15 min - 12 t) på båndene **28, 40 (8 m, slået fra som standard), 50, 70 og 144 MHz**, hentet fra telnet-sessionerne. Lokatorerne kommer fra spotkommentaren (f.eks. `JN70CN<ES>JO62QM`) eller fra CALL3-databasen.
- Spots med **<MS>**, **<AU>** eller **<TR>** i teksten **kasseres**: de vedrører ikke sporadisk E, men er nyttige til meteor scatter, nordlys eller tropo.
- **Transækvatorial, forfatterens regel (IC8TEM)**: mod Afrika og Sydamerika er forbindelsen transækvatorial (TEP) og ikke sporadisk E, hvis midtpunktet falder i båndet med magnetisk inklination mellem -15° og +15°, medmindre spottet angiver `<ES>`.
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

## Felter farvet efter frekvens og blinkende MUF

Maidenhead-felterne for refleksionspunkterne farves efter spottenes højeste frekvens: **grøn** op til 29 MHz, **gul** op til 51, **orange** op til 71, **rød** op til 145 MHz. Ud fra feltets foEs estimeres den sandsynlige MUF på det længste hop (2400 km, IK3XTV-tabellen): når den når et højere bånd, **blinker** feltet i det bånds farve (princip fra IK3XTV og Jacobs-Leinwoll: kort skip betyder høj MUF).

Øverst til højre på kortet vises sammendraget fra N0NBH (E-skip for det valgte kontinent, SFI, K- og A-indeks).

## Regler pr. bånd (version 0.4.18)

- **50 MHz**: ingen sporadisk E inden for 450 km. FT8- og FT4-spots er kun sporadisk E, hvis de hører til en sky: et andet 50 MHz-spot inden for 4 minutter med midtpunktet inden for 150 km. Et isoleret FT8/FT4-spot går til meteor scatter.
- **70 MHz**: ingen sporadisk E inden for 450 km. FT8 og FT4 går til sporadisk E, også inden for båndplanens meteor scatter-vindue (den kendte mode afgør først, derefter frekvensen).
- **40 MHz (8 m)**: minimumsafstand udledt af 50 MHz' 450 km med sekansloven (ca. 330 km); kun afstande inden for det første hop i E-lagets geometri (ca. 2440 km). Strækningens farve: gul.
- **144 MHz**: sporadisk E fra 900 km med bekræftelse; Q65- og JT65-spots er EME og kasseres, medmindre `<ES>` står i teksten.
- G0ISW (Propagation identification, 2024) ser den korteste sporadiske E over 400 km på 50 og 70 MHz og over 1400 km på 144 MHz: forfatterens regler stemmer overens på 50 og 70 MHz og er bredere på 144 MHz, men kræver en bekræftelse.
- Rækkefølgen, som alle regler anvendes i, med de værdier, der er i brug, vises i **[INDSTILLINGER](Settings-da)** › Fordeling af spots.
