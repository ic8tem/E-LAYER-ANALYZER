# SPORADIC-E

[← Home](Home-nb)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kartet

En knapp over kartet velger visningen:

- **Asimutal**: asimutalt ekvidistant kart sentrert på stasjonen din, med glidebryteren for **zoom** (300 til 20000 km fra sentrum, også med musehjulet).
- **Mercator**: verdenskartet, med knappene Europa og Verden.

I begge projeksjonene tegnes **avstandssirklene til Flavio IK3XTV** rundt stasjonen: 500 km (minimum for enkelthopp), 1500-2000 km (optimum), 2400 km (maksimum for enkelthopp), 5000 km (maksimum for dobbelthopp) og 6900 km (maksimum for trippelhopp).

I de asimutale visningene viser de stiplede ringene **km fra sentrum** (intervallet endres med zoomen), kanten markerer den største avstanden som vises, og musepekeren viser avstand og asimut fra sentrum.

![Multihopp](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Hva den viser

- **Spot** fra de siste minuttene (15 min - 12 t) på båndene **28, 40 (8 m, slått av fra start), 50, 70 og 144 MHz**, hentet fra telnet-øktene. Lokatorene kommer fra spotkommentaren (f.eks. `JN70CN<ES>JO62QM`) eller fra CALL3-databasen.
- Spot med **<MS>**, **<AU>** eller **<TR>** i teksten **forkastes**: de gjelder ikke sporadisk E, men er nyttige for meteor scatter, nordlys eller tropo.
- **Transekvatorial, forfatterens regel (IC8TEM)**: mot Afrika og Sør-Amerika er forbindelsen transekvatorial (TEP) og ikke sporadisk E hvis midtpunktet ligger i beltet med magnetisk inklinasjon mellom -20° og +20°, med mindre spottet oppgir `<ES>`.
- **144 MHz, forfatterens regel (IC8TEM)**: sporadisk E på 2 m begynner **over 900 km** (kortere spot forkastes). Et enkelt 144 MHz-spot **tegnes ikke alene**: det vises når det innen **5 minutter** kommer et andre 144 MHz-spot med skyen på samme sted (refleksjonspunkter innenfor 300 km), eller når det finnes et **50 MHz-spot med kort hopp** i nærheten (innenfor 1200 km, de siste 15 minuttene). Linjen øverst viser hvor mange 144 MHz-spot som venter på bekreftelse.
- For hvert spot: avstand, **antall hopp** ifølge **IK3XTV**-tabellen, elevasjon, **minste foEs** (sekantloven).
- **Første hopp for begge stasjonene**: oransje firkant = spotterens første hopp, fiolett = DX-stasjonens første hopp (like hopp); gul = midtpunkt for enkelthopp.
- **Kort første hopp** (hule sirkler): ved multihoppåpninger har forfatteren opplevd et kort første hopp innenfor **1200 km** fra eget QTH; lengden kan stilles inn.
- **Es-skyer** på **Maidenhead-ruter** (2° x 1°), farget etter minste foEs: blå < 5, lyseblå < 7, grønn < 10, gul < 14, oransje < 20, rød > 20 MHz.
- Tabellen **Es-skyer**: for hver rute **MUF for din stasjon** (rød = 144 MHz mulig, oransje = 70, gul = 50).

## IK3XTV-tabellen

| | minimum | optimal | maksimum |
|---|---|---|---|
| Enkelthopp | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dobbelthopp | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Trippelhopp | – | 4800-6400 km | 6900 km |

## Slik bruker du den

- **Klikk på et spot**: strekningen utheves, med refleksjonspunkter, lokatorer og asimut i detaljene.
- **Klikk på et spot** i tabellen: strekningen lyser opp på kartet (gul glød), og hvis den er utenfor synsfeltet, flytter kartet seg eller utvider radien for å vise den. Spottet forblir valgt når nye spot kommer inn.
- **Tabellen følger kartet**: når det er zoomet inn, viser spot-tabellen og sky-tabellen bare det som er synlig på kartet.
- **Klikk på kartet**: punktet behandles som en mulig Es-sky, med foEs som kreves for 28, 50, 70 og 144 MHz.

## Merknad om 432 MHz

Sporadisk E **gjelder ikke for 432 MHz**: Es-MUF når ca. 150-200 MHz, og ARRL-rekordene oppgir Es-forbindelser bare på 50 og 144 MHz. Spot på 70 cm analyseres ikke her.

## Ruter farget etter frekvens og blinkende MUF

Maidenhead-rutene for refleksjonspunktene farges etter den høyeste frekvensen blant spotene: **grønn** opp til 29 MHz, **gul** opp til 51, **oransje** opp til 71, **rød** opp til 145 MHz. Ut fra rutens foEs anslås den sannsynlige MUF-en på det lengste hoppet (2400 km, IK3XTV-tabellen): når den når et høyere bånd, **blinker** ruten i fargen til det båndet (prinsipp fra IK3XTV og Jacobs-Leinwoll: kort skip betyr høy MUF).

Øverst til høyre på kartet vises sammendraget fra N0NBH (E-skip for det valgte kontinentet, SFI, K- og A-indeks).

## Regler per bånd (versjon 0.4.18)

- **50 MHz**: ingen sporadisk E innenfor 450 km. FT8- og FT4-spot er bare sporadisk E hvis de hører til en sky: et annet 50 MHz-spot innen 4 minutter med midtpunktet innenfor 150 km. Et isolert FT8/FT4-spot går til meteor scatter.
- **70 MHz**: ingen sporadisk E innenfor 450 km. FT8 og FT4 går til sporadisk E, også innenfor båndplanens meteor scatter-vindu (den kjente modusen avgjør først, deretter frekvensen).
- **40 MHz (8 m)**: minsteavstand utledet av 450 km på 50 MHz med sekantloven (ca. 330 km); bare avstander innenfor det første hoppet i E-lagets geometri (ca. 2440 km). Farge på strekningen: gul.
- **144 MHz**: sporadisk E fra 900 km, med bekreftelse; Q65- og JT65-spot er EME og forkastes, med mindre `<ES>` står i teksten.
- G0ISW (Propagation identification, 2024) ser den korteste sporadiske E over 400 km på 50 og 70 MHz og over 1400 km på 144 MHz: forfatterens regler stemmer på 50 og 70 MHz og er videre på 144 MHz, men krever en bekreftelse.
- Rekkefølgen som alle reglene brukes i, med verdiene som er i bruk, vises i **[INNSTILLINGER](Settings-nb)** › Fordeling av spot.
