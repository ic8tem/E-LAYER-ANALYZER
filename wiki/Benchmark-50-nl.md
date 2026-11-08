# BENCHMARK 50 MHz

[← Home](Home-nl)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Testbank voor **sporadische E met meervoudige sprongen op 50 MHz** op basis van het ADIF-log van één station en, optioneel, van een tweede station.

## De logs laden

- **Station 1**: *ADIF laden* / *ADIF verwijderen*. Het programma haalt `STATION_CALLSIGN` en `MY_GRIDSQUARE` uit het log; de zendpositie is de `MY_GRIDSQUARE` van elke QSO.
- **Station 2** (optioneel): dezelfde knoppen op de tweede regel; het vakje *Station 2* toont of verbergt hem. Trajecten van station 1 zijn lichtblauw, die van station 2 paars.
- Het programma gebruikt QSO's op 50 MHz met een locator (ontbrekende locators of locators van 4 tekens worden uit CALL3 gehaald), met meervoudige sprong: voorbij 3100 km (regel van de auteur; het aantal sprongen volgt de tabel IK3XTV).
- **Transequatoriale** verbindingen worden uitgesloten (regel van de auteur): richting Afrika en Zuid-Amerika, met het middelpunt in de band van magnetische inclinatie tussen -20° en +20°, tenzij het log `PROP_MODE=ES` heeft.

## Dag en tijd

Kies de **dag** (bij het openen die met de meeste QSO's met meervoudige sprong) en de tijd **van… tot…** UTC, in stappen van 15 minuten. Het vakje **foEs** (optioneel) kleurt de trajecten naar minimale foEs en toont die in de tabel.

## De kaart

**Mercator**-kaart met vrij midden: scrollwiel = zoom, slepen = verschuiven.

![Traject](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Muis bij een traject**: lijn en pictogrammen worden vergroot en het **WWL-vak van 6 tekens** van het tegenstation verschijnt.
- **Linksklik** op dat traject: een venster vraagt of **alle QSO's in het bestand** (alle dagen) moeten worden getoond waarvan het andere eind **binnen 150 km** van dat punt ligt. *Ja*: de kaart wordt leeggemaakt en toont die QSO's binnen de oranje cirkel; *Nee*: de kaart blijft zoals ze is. *Terug naar dag en tijd* herstelt de vorige selectie.

![Omgeving](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Details

Voor het gekozen traject: afstand en beoordeling volgens de tabel IK3XTV, azimut in beide richtingen, gelijke sprongen met de vakken van de reflectiepunten, korte eerste sprong binnen 1200 km (ervaring IC8TEM) en, met het vakje foEs, de minimale foEs.
