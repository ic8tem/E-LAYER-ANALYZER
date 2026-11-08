# SPORADIC-E

[← Home](Home-nb)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kartet

En knapp over kartet velger visningen:

- **Asimutal - enkelthopp**: asimutalt ekvidistant kart sentrert på stasjonen din, fast radius 2500 km, med sonene fra IK3XTV-tabellen (500 km minimum, 1500-2000 km optimal, 2400 km maksimum for enkelthopp).
- **Asimutal - maksimal avstand (multihopp)**: samme kart med glidebryteren **Maksimal avstand** (fra 300 til 20000 km, også med musehjulet); grensene for dobbelthopp (5000 km) og trippelhopp (6900 km) vises også, slik at utviklingen av multihopp kan følges.
- **Rektangulær**: det tidligere verdenskartet, med knappene Europa og Verden.

I de asimutale visningene viser de stiplede ringene **km fra sentrum** (intervallet endres med zoomen), kanten markerer største viste avstand, og musemarkøren gir avstand og asimut fra sentrum.

![Multihopp](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Hva den viser

- **Spot** fra de siste minuttene (15 min - 12 t) på båndene **28, 50, 70 og 144 MHz**, hentet fra telnet-øktene. Lokatorene kommer fra kommentaren i spottet (f.eks. `JN70CN<ES>JO62QM`) eller fra CALL3-databasen.
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
