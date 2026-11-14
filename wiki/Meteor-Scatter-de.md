# METEOR SCATTER

[← Home](Home-de)

Dieser Reiter enthält das vollständige **YA-MS 1.8.11**, das Meteorscatter-Programm von IC8TEM: Himmelskugel mit den Radianten, Radarfluss SKiYMET (Collm, Sodankylä), Berechnung der Effektivität der Meteorströme, experimentelle Umsetzung der Vorhersagen, Geometrie und Dämpfung, GRAVES, Feldkarte, **Ströme live** (vom Global Meteor Network gemessene ZHR und Radianten des CMOR-Radars), **Karte der Quellen** (Collm, Sodankylä, GRAVES und CMOR relativ zur Station) und das Dokument **Erklärung der Wirkungsgradberechnung** mit den Formeln.

YA-MS 1.5.0 bringt den Reiter **Datensammlung** hinzu (Meteorscatter-Spots, gefiltert nach MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Innerhalb von E-LAYER ANALYZER öffnet er keine eigenen Telnet-Sitzungen: Er erhält die Zeilen der Sitzungen von DATA COLLECTION, sodass es keine doppelten Logins mit demselben Rufzeichen gibt.

YA-MS 1.6.0 ordnet die Reiter neu in **Live und Echtzeitdaten** (Meteorscatter-Verkehr live mit der Wolke jeder Verbindung, Radarfluss, GRAVES, Datensammlung) und **Vorhersageberechnung** (Vorhersage, Geometrie, Effektivitätsberechnung, Umsetzung der Vorhersagen).

YA-MS 1.7.0 und 1.8.0 fügen den Vergleich der Radianten mit dem Katalog IAU MDC hinzu, die täglichen Zählungen des Global Meteor Network je Strom, den Index k aus den Populationsindizes, den Anfangsradius der Meteorspur nach Stober et al. (2021), die Ping-Dauer zwischen den Bändern (standardmäßig λ² oder die Messungen HamSCI MSQP von NN4SA) und den Vergleich mit den BRAMS-Detektionen in Sonneberg (Perseiden 2026). Die automatische Aktualisierung von YA-MS ist innerhalb von E-LAYER ANALYZER abgeschaltet, das sich selbst über die GitHub-Releases aktualisiert.

YA-MS 1.8.1 behandelt die Antennenkeule als vertikales Diagramm (Kästchen «Antennendiagramm», voreingestellt an): Die Reflexionspunkte werden mit dem vertikalen Gewinn beider Antennen gewichtet, und die Wirkungsgrade verschwinden auf langen Strecken nicht mehr. Neu sind die Warnung «keine gemeinsame Zone», das hervorgehobene Kästchen für den Winkel β (die Wahl mit dem größten Gewicht), ein einziges Berechnungsdatum, das «Bedienungsanleitung» und der Vergleich mit 181 767 echten Spots auf 50 MHz aus der HamSCI Meteor Scatter QSO Party vom August 2025: Stunden und Seiten der Strecke sind verlässlich, die Prozentwerte sind Richtwerte.

- YA-MS verwendet die oben in E-LAYER ANALYZER gewählte Sprache und das dort gewählte Farbschema.
- Die Einstellungen von YA-MS werden in `data/config.ini` gespeichert.
- Jede neue Version von YA-MS wird mit dem Skript `tools/sync_yams.py` übernommen.

Einzelheiten im [YA-MS-Repository](https://github.com/ic8tem/YA-MS).
