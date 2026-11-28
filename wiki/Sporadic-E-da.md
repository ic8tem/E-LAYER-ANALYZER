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
- **Transækvatorial, forfatterens regel (IC8TEM)**: mod Afrika og Sydamerika er forbindelsen transækvatorial (TEP) og ikke sporadisk E, hvis midtpunktet falder i båndet med magnetisk inklination mellem -15° og +15° (værdi hentet fra Cracknell, UKSMG: TEP-zoner ca. 5-15° fra linjen med inklination nul), medmindre spottet angiver `<ES>`.
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

Maidenhead-felterne for refleksionspunkterne farves efter spottenes højeste frekvens: **grøn** op til 29 MHz, **gul** op til 51, **orange** op til 71, **rød** op til 145 MHz. Ud fra feltets foEs estimeres den sandsynlige MUF på det længste hop (2400 km, IK3XTV-tabellen): når den når et højere bånd, **blinker** feltet i det bånds farve (princip fra IK3XTV og Jacobs-Leinwoll: kort skip betyder høj MUF). For 144 MHz gælder forfatterens regel: feltet blinker kun rødt, hvis skyen giver ét hop på 50 MHz mellem 450 og 500 km (450 km med en tolerance på 50 km).

Øverst til højre på kortet vises sammendraget fra N0NBH (E-skip for det valgte kontinent, SFI, K- og A-indeks).

## Regler pr. bånd (version 0.4.18)

- **50 MHz**: ingen sporadisk E inden for 450 km. FT8- og FT4-spots er kun sporadisk E, hvis de hører til en sky: et andet 50 MHz-spot inden for 4 minutter med midtpunktet inden for 150 km. Et isoleret FT8/FT4-spot går til meteor scatter.
- **70 MHz**: ingen sporadisk E inden for 450 km. FT8 og FT4 går til sporadisk E, også inden for båndplanens meteor scatter-vindue (den kendte mode afgør først, derefter frekvensen).
- **40 MHz (8 m)**: minimumsafstand udledt af 50 MHz' 450 km med sekansloven (ca. 330 km); kun afstande inden for det første hop i E-lagets geometri (ca. 2440 km). Strækningens farve: gul.
- **144 MHz**: sporadisk E fra 900 km med bekræftelse; Q65- og JT65-spots er EME og kasseres, medmindre `<ES>` står i teksten.
- G0ISW (Propagation identification, 2024) ser den korteste sporadiske E over 400 km på 50 og 70 MHz og over 1400 km på 144 MHz: forfatterens regler stemmer overens på 50 og 70 MHz og er bredere på 144 MHz, men kræver en bekræftelse.
- Rækkefølgen, som alle regler anvendes i, med de værdier, der er i brug, vises i **[INDSTILLINGER](Settings-da)** › Fordeling af spots.

## Kæder med korte hop (fra version 0.6.0)

![Kæde fra QTH](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

I underfanen **Multi-hop-forløbere** søger programmet efter kæder af multi-hop på 50 MHz med forfatterens tre betingelser: **første og sidste korte hop** (500-1200 km) med MUF omkring 50 MHz, som kræver en foEs på 11-21 MHz, og **midterhop** op til 2200 km, som kræver ca. 9,3 MHz.

- **Stærke skyer** (orange firkanter): Es-spots fra hele verden og GIRO-ionosonder med foEs på mindst 11,3 MHz.
- **Fra QTH**: første korte hop fra din station og sidste korte hop via en anden stærk sky i samme retning, også på samme kontinent (for eksempel i Kasakhstan); cirklen er ankomstzonen.
- **Korridor**: to vilkårlige stærke skyer i alle retninger, for eksempel **Caribien - Den Iberiske Halvø** eller **Europa - Vestafrika** (TEP udelukket); «angår dig», hvis en ende er inden for 1200 km fra dig.
- **Find skyen**: der er kun det første korte hop; de lyseblå zoner viser, hvor skyen for det sidste korte hop burde ligge.
- **ALARM**, hvis midterhoppene er dækket af skyer, **FORLØBER**, hvis der kun mangler zoner uden observatører.
- I INDSTILLINGER findes **advarslen** øverst (og valgfrit et vindue) for de ALARMER, der angår dig.
- Kæder, der ender **på åbent hav** eller hvor der ikke er stationer (CALL3 og spots), vises ikke.
- **Ens dobbelthop** (fra version 0.6.3, hop »2 =«): op til omkring 4700 km, for eksempel mod Kasakhstan, med skyen ved en fjerdedel af strækningen som i metoden ITU-R P.534-6.
- Fanen **HOPTABEL** opsummerer, hvor mange hop der skal til på 50 MHz fra 500 til 12000 km, med hvert hops foEs og kilderne.
- **Hvor mit signal lander på 6 m** (lyserøde punkter): hvor dit signal på 50 MHz lander efter 2-5 hop, over de skyer fra de seneste 30 minutter, der er anslået ud fra spots på alle bånd (28-144 MHz) og fra ionosonderne; kun hvor der er stationer.
- **Korridor med lige hop** (fra version 0.6.5): to skyer 1400-2350 km fra hinanden forbinder to zoner på 2800-4700 km med to lige hop; kontrollen på ekspeditionen 5U5K bekræfter grænsen på omkring 4700 km.
- **TIDLIGT TEGN** (grøn, fra version 0.6.5): Es-sky på 28 eller 24 MHz, hvor 50 MHz endnu ikke er åben; i prøverne åbnede 50 MHz der inden for en time i 50 % af tilfældene (30 % uden), 9-16 minutter før.
- **Gentaget APRS** (rød, fra version 0.6.5): mindst to APRS-hop på 2 m over 900 km på samme sky inden for 10 minutter = 144 MHz-ALARM; fanen blinker.
- **Hvad alarmerne er værd** (på forfatterens station, somrene 2024-2026): ALARMER vellykkede i 41-57 % (multi-hop-QSO i samme retning inden for en time) mod 5-7 % uden alarm; men 47 % af åbningerne kommer uden varsel. Mønsteret for hvor signalet lander når inden for 500 km fra den fjerne station i 13,5 % af åbningerne, mod 2-4 % når det drejes.
- Skyerne **flyttes ikke**: prøven fra 22-11-2026 viser, at det ikke forbedrer forudsigelsen på 15-45 minutter at flytte dem med deres hastighed.
- **TEP-prognose** (blå, fra version 0.6.5): indeks 0-100 mod zonerne hinsides den magnetiske ækvator = årstid × SFI × Kp (kun eTEP) × lokal tid × foF2 fra GIRO-ionosonderne; SFI vejer, men nulstiller ikke, fordi TEP ved solminimum bliver sjælden, men ikke forsvinder.

![Korridor](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)

## PROGNOSE (fra 0.7.0)

Underfanen **PROGNOSE** genkender Es-skyer under dannelse (spots fra de sidste 30 minutter og GIRO-ionosonder) og tjekker fra din station, med 50 og 144 MHz adskilt: brugbart første hop, så Es-dobbelthop, så – kun hvis der ikke er noget første hop – tropo + Es, med en virtuel station A′ nået gennem en dukt (tropomodul fra LIVETROPOVHF, Open-Meteo-profiler, kritisk vinkel efter ITU-R P.834). I REAL-TIME kan 144 MHz-spots mellem 2400 og 3400 km blive til **TR+ES**, med den flyttede sky.
