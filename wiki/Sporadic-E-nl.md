# SPORADIC-E

[← Home](Home-nl)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## De kaart

Een knop boven de kaart kiest de weergave:

- **Azimutaal**: azimutale equidistante kaart, gecentreerd op je station, met de schuifregelaar **zoom** (300 tot 20000 km vanaf het midden, ook met het muiswiel).
- **Mercator**: de wereldkaart, met de knoppen Europa en Wereld.

In beide projecties worden rond het station de **afstandscirkels van Flavio IK3XTV** getekend: 500 km (minimum van de enkele sprong), 1500-2000 km (optimum), 2400 km (maximum van de enkele sprong), 5000 km (maximum van de dubbele sprong) en 6900 km (maximum van de drievoudige sprong).

In de azimutale weergaven tonen de gestippelde ringen de **km vanaf het midden** (de stap verandert met de zoom), de rand markeert de grootste getoonde afstand, en de muisaanwijzer geeft afstand en azimut vanaf het midden.

![Meervoudige sprong](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Wat het toont

- De **spots** van de laatste minuten (15 min - 12 h) op de banden **28, 40 (8 m, standaard uit), 50, 70 en 144 MHz**, uit de telnet-sessies. De locators komen uit het commentaar van de spot (bijv. `JN70CN<ES>JO62QM`) of uit de CALL3-database.
- Spots met **<MS>**, **<AU>** of **<TR>** in de tekst worden **verworpen**: ze hebben niets met sporadische E te maken, maar zijn nuttig voor meteorscatter, aurora of tropo.
- **Transequatoriaal, regel van de auteur (IC8TEM)**: richting Afrika en Zuid-Amerika is een verbinding transequatoriaal (TEP) en geen sporadische E als het middelpunt in de band van magnetische inclinatie tussen -15° en +15° valt (waarde overgenomen van Cracknell, UKSMG: TEP-zones ongeveer 5-15° van de lijn van inclinatie nul), tenzij de spot `<ES>` vermeldt.
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

## Vakken gekleurd naar frequentie en knipperende MUF

De Maidenhead-vakken van de reflectiepunten worden gekleurd naar de hoogste frequentie van de spots: **groen** tot 29 MHz, **geel** tot 51, **oranje** tot 71, **rood** tot 145 MHz. Uit de foEs van het vak wordt de waarschijnlijke MUF op de langste sprong geschat (2400 km, tabel IK3XTV): bereikt die een hogere band, dan **knippert** het vak in de kleur van die band (principe van IK3XTV en Jacobs-Leinwoll: korte skip betekent hoge MUF). Voor 144 MHz geldt de regel van de auteur: het vak knippert alleen rood als de wolk een enkele sprong op 50 MHz tussen 450 en 500 km geeft (450 km met een tolerantie van 50 km).

Rechtsboven op de kaart verschijnt de samenvatting van N0NBH (E-skip voor het gekozen continent, SFI, K- en A-index).

## Regels per band (versie 0.4.18)

- **50 MHz**: geen sporadische E binnen 450 km. FT8- en FT4-spots zijn alleen sporadische E als ze bij een wolk horen: een andere 50 MHz-spot binnen 4 minuten met het middelpunt binnen 150 km. Een losse FT8/FT4-spot gaat naar meteorscatter.
- **70 MHz**: geen sporadische E binnen 450 km. FT8 en FT4 gaan naar sporadische E, ook binnen het meteorscattervenster van het bandplan (eerst beslist de bekende mode, dan de frequentie).
- **40 MHz (8 m)**: minimale afstand met de secanswet afgeleid uit de 450 km van 50 MHz (ongeveer 330 km); alleen afstanden binnen de eerste sprong van de geometrie van de E-laag (ongeveer 2440 km). Kleur van het traject: geel.
- **144 MHz**: sporadische E vanaf 900 km, met bevestiging; Q65- en JT65-spots zijn EME en worden verworpen, behalve met `<ES>` in de tekst.
- G0ISW (Propagation identification, 2024) ziet de kortste sporadische E boven 400 km op 50 en 70 MHz en boven 1400 km op 144 MHz: de regels van de auteur komen overeen op 50 en 70 MHz en zijn ruimer op 144 MHz, maar vragen een bevestiging.
- De volgorde waarin alle regels worden toegepast, met de gebruikte waarden, staat in **[INSTELLINGEN](Settings-nl)** › Spotverdeling.

## Ketens met korte sprongen (vanaf versie 0.6.0)

![Keten vanaf de QTH](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

In de subtab **Voorboden meervoudige sprong** zoekt het programma de ketens met meervoudige sprong op 50 MHz met de drie voorwaarden van de auteur: **eerste en laatste korte sprong** (500-1200 km) met MUF rond 50 MHz, die een foEs van 11-21 MHz vragen, en **middelste sprongen** tot 2200 km, die ongeveer 9,3 MHz vragen.

- **Sterke wolken** (oranje vierkanten): Es-spots van over de hele wereld en GIRO-ionosondes met een foEs van minstens 11,3 MHz.
- **Vanaf de QTH**: eerste korte sprong vanaf je station en laatste korte sprong op een andere sterke wolk in dezelfde richting, ook op hetzelfde continent (bijvoorbeeld in Kazachstan); de cirkel is de aankomstzone.
- **Corridor**: twee willekeurige sterke wolken, in elke richting, bijvoorbeeld **Caribisch gebied - Iberisch Schiereiland** of **Europa - West-Afrika** (TEP uitgesloten); «betreft jou» als een einde binnen 1200 km van jou ligt.
- **Zoek de wolk**: er is alleen de eerste korte sprong; de lichtblauwe zones zeggen waar de wolk van de korte laatste sprong zou moeten liggen.
- **WAARSCHUWING** als de middelste sprongen door wolken gedekt zijn, **VOORBODE** als alleen zones zonder waarnemers ontbreken.
- In INSTELLINGEN staat de **melding** bovenaan (en desgewenst een venster) voor de WAARSCHUWINGEN die jou betreffen.
- Ketens die **op open zee** eindigen of waar geen stations zijn (CALL3 en spots), worden niet getoond.
- **Gelijke dubbele sprong** (vanaf versie 0.6.3, sprongen «2 =»): tot ongeveer 4700 km, bijvoorbeeld naar Kazachstan, met de wolk op een kwart van het traject zoals in de methode ITU-R P.534-6.
- De tab **SPRONGTABEL** vat samen hoeveel sprongen op 50 MHz nodig zijn van 500 tot 12000 km, met de foEs van elke sprong en de bronnen.
- **Footprint van mijn signaal op 6 m** (roze punten): waar je 50 MHz-signaal landt na 2-5 sprongen, boven de wolken van de laatste 30 minuten die geschat zijn uit de spots van alle banden (28-144 MHz) en uit de ionosondes; alleen waar stations zijn.
- **Corridor met gelijke sprongen** (vanaf versie 0.6.5): twee wolken op 1400-2350 km van elkaar verbinden twee zones op 2800-4700 km met twee gelijke sprongen; de controle op de expeditie 5U5K bevestigt de grens van ongeveer 4700 km.
- **VROEG TEKEN** (groen, vanaf versie 0.6.5): Es-wolk op 28 of 24 MHz waar 50 MHz nog niet open is; in de proeven ging 50 MHz daar binnen een uur open in 50% van de gevallen (30% zonder), 9-16 minuten vooraf.
- **Herhaalde APRS** (rood, vanaf versie 0.6.5): minstens twee APRS-sprongen op 2 m boven 900 km op dezelfde wolk binnen 10 minuten = 144 MHz-WAARSCHUWING; het tabblad knippert.
- **Wat de waarschuwingen waard zijn** (op het station van de auteur, zomers 2024-2026): WAARSCHUWINGEN geslaagd in 41-57% (meervoudige-sprong-QSO in dezelfde richting binnen een uur) tegen 5-7% zonder waarschuwing; maar 47% van de openingen komt zonder voorwaarschuwing. De footprint komt in 13,5% van de openingen binnen 500 km van het verre station, tegen 2-4% als hij gedraaid wordt.
- De wolken **worden niet verplaatst**: de proef van 22-11-2026 toont dat ze verplaatsen met hun snelheid de voorspelling op 15-45 minuten niet verbetert.
- **TEP-voorspelling** (blauw, vanaf versie 0.6.5): index 0-100 naar de zones voorbij de magnetische evenaar = seizoen × SFI × Kp (alleen eTEP) × lokale tijd × foF2 van de GIRO-ionosondes; de SFI weegt mee maar zet niet op nul, omdat TEP bij zonneminimum zeldzaam wordt maar niet verdwijnt.

![Corridor](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)

## VOORSPELLING (vanaf 0.7.0)

Het subtabblad **VOORSPELLING** herkent Es-wolken in vorming (spots van de laatste 30 minuten en GIRO-ionosondes) en controleert vanaf je station, met 50 en 144 MHz gescheiden: bruikbare eerste sprong, dan dubbele Es-sprong, dan – alleen als er geen eerste sprong is – tropo + Es, met een virtueel station A′ dat via een duct wordt bereikt (tropomodule van LIVETROPOVHF, Open-Meteo-profielen, kritische hoek volgens ITU-R P.834). In REAL-TIME kunnen 144 MHz-spots tussen 2400 en 3400 km **TR+ES** worden, met de verplaatste wolk.
