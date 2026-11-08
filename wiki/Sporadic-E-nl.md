# SPORADIC-E

[← Home](Home-nl)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## De kaart

Een knop boven de kaart kiest de weergave:

- **Azimutaal - enkele sprong**: azimutale equidistante kaart gecentreerd op jouw station, vaste straal 2500 km, met de zones van de tabel IK3XTV (enkele sprong minimaal 500 km, optimaal 1500-2000 km, maximaal 2400 km).
- **Azimutaal - maximale afstand (meervoudige sprong)**: dezelfde kaart met de schuifregelaar **Maximale afstand** (van 300 tot 20000 km, ook met het scrollwiel); ook de grenzen van de dubbele (5000 km) en drievoudige sprong (6900 km) verschijnen, zodat je het verloop van meervoudige sprongen kunt volgen.
- **Rechthoekig**: de vroegere wereldkaart, met de knoppen Europa en Wereld.

In de azimutale weergaven tonen de gestippelde ringen de **km vanaf het midden** (de stap verandert met de zoom), de rand markeert de grootste getoonde afstand, en de muisaanwijzer geeft afstand en azimut vanaf het midden.

![Meervoudige sprong](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Wat het toont

- De **spots** van de laatste minuten (15 min - 12 h) op de banden **28, 50, 70 en 144 MHz**, uit de telnet-sessies. De locators komen uit het commentaar van de spot (bijv. `JN70CN<ES>JO62QM`) of uit de CALL3-database.
- Spots met **<MS>**, **<AU>** of **<TR>** in de tekst worden **verworpen**: ze hebben niets met sporadische E te maken, maar zijn nuttig voor meteorscatter, aurora of tropo.
- **Transequatoriaal, regel van de auteur (IC8TEM)**: richting Afrika en Zuid-Amerika is een verbinding transequatoriaal (TEP) en geen sporadische E als het middelpunt in de band van magnetische inclinatie tussen -20° en +20° valt, tenzij de spot `<ES>` vermeldt.
- **144 MHz, regel van de auteur (IC8TEM)**: sporadische E op 2 m begint **voorbij 900 km** (kortere spots worden verworpen). Een enkele 144 MHz-spot **wordt niet op zichzelf getekend**: hij verschijnt als er binnen **5 minuten** een tweede 144 MHz-spot binnenkomt met de wolk op dezelfde plek (reflectiepunten binnen 300 km), of als er in de buurt een **50 MHz-spot met korte sprong** is (binnen 1200 km, in de laatste 15 minuten). De balk bovenaan toont hoeveel 144 MHz-spots op bevestiging wachten.
- Voor elke spot: afstand, **aantal sprongen** volgens de tabel **IK3XTV**, elevatie, **minimale foEs** (secanswet).
- **Eerste sprong van beide stations**: oranje vierkant = eerste sprong van de spotter, paars = eerste sprong van de DX (gelijke sprongen); geel = middelpunt van een enkele sprong.
- **Korte eerste sprong** (open cirkels): bij openingen met meervoudige sprongen heeft de auteur een korte eerste sprong binnen **1200 km** van zijn eigen QTH ervaren; de lengte is instelbaar.
- **Es-wolken** op **Maidenhead-vakken** (2° x 1°), gekleurd naar minimale foEs: blauw < 5, lichtblauw < 7, groen < 10, geel < 14, oranje < 20, rood > 20 MHz.
- Tabel **Es-wolken**: voor elk vak de **MUF voor jouw station** (rood = 144 MHz mogelijk, oranje = 70, geel = 50).

## Tabel IK3XTV

| | minimum | optimum | maximum |
|---|---|---|---|
| Enkele sprong | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dubbele sprong | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Drievoudige sprong | – | 4800-6400 km | 6900 km |

## Gebruik

- **Klik op een spot**: het traject wordt gemarkeerd, met reflectiepunten, locators en azimuts in de details.
- **Klik op een spot** in de tabel: het traject licht op op de kaart (gele gloed) en als het buiten beeld ligt, verschuift de kaart of vergroot ze haar straal om het te tonen. De spot blijft geselecteerd als er nieuwe spots binnenkomen.
- **De tabel volgt de kaart**: met een ingestelde zoom tonen de spottabel en de wolkentabel alleen wat op de kaart zichtbaar is.
- **Klik op de kaart**: het punt wordt behandeld als mogelijke Es-wolk, met de foEs die nodig is voor 28, 50, 70 en 144 MHz.

## Opmerking over 432 MHz

Sporadische E **is niet van toepassing op 432 MHz**: de Es-MUF reikt tot ongeveer 150-200 MHz en de ARRL-recordlijsten vermelden Es-verbindingen alleen op 50 en 144 MHz. Spots op 70 cm worden hier niet geanalyseerd.
