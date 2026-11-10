# AURORA

[← Home](Home-de)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Auroraoval** NOAA OVATION Prime (schattiert), **Kp** der letzten 3 Tage, **Sonnenwind** (Bz, Geschwindigkeit, Dichte), alle 5 Minuten aktualisiert und in `data/swpc` gespeichert.
- Dieselben Ansichten wie bei F.A.I.: **Scatter trail** für ein Stationspaar und **Iso-Reflexion** zum Prüfen eines Punktes, mit einer Standardhöhe von 110 km.
- **Empfohlene Antennenrichtung für beide Stationen**: entlang der Senkrechtenlinie jeder Station (Punkte in ihrer Farbe) der Punkt mit der höchsten Oval-Wahrscheinlichkeit. Faustregel (G3WPO): bei etwa 45° beginnen, zwischen Nord und Ost.
- Beispiel IO70VJ - JO59BC: Streupunkt JP01AK vor den Shetlandinseln, Abweichung 0,97°, QTF 11° und 299°.
- **Aurora-Spots auf der Karte**: Spots mit **<AU>** oder **<AUR>** im Text sind Aurora-Spots (Regel des Autors). Mit angehaktem Kästchen *<AU>/<AUR>-Spots auf der Karte mit Berechnung* zeichnet der Reiter die Spots der letzten 2 Stunden ein (höchstens 30): Für jedes Paar Spotter-DX berechnet er den Scatter trail und zeigt den wahrscheinlichsten Streupunkt (oranger Kreis), mit QTF und Elevation beider Stationen in den Ergebnissen.
- Neben dem wahrscheinlichsten Streupunkt geben die Ergebnisse den **QTF-Bereich** beider Stationen entlang des gesamten Scatter trail an (Abweichung ≤ 1,5°).

## BENCHMARK AURORA

Der Reiter AURORA hat zwei Unterreiter: **REAL-TIME** (die oben beschriebene Berechnung) und **BENCHMARK AURORA**, einen Prüfstand des Modells mit **298 echten Aurora-QSOs** mit Uhrzeit und Locator: die veröffentlichten Logs von EI5FK (IO51RT, Irland, 2003-2005, 144 und 432 MHz) und die Logkarte von PA0O (JO33HG, Niederlande).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Für jedes QSO berechnet das Programm den Streupunkt mit demselben Modell (IGRF-14, Höhe 110 km, Refraktion 4/3, Toleranz 3°).
- Farben: **grün** = Streupunkt 370-1100 km von beiden Stationen entfernt (Lange-Hesse); **gelb** = erklärt, aber außerhalb dieses Fensters; **orange** = QSO über 2000 km; **rot** = kein Streupunkt, QSO vom Modell nicht erklärt.
- Die Zusammenfassung gibt die Prozentsätze und für jedes Log den Bereich und den Median des berechneten QTF an. Ein Klick auf eine Zeile zeichnet die Strecke und die beiden Antennenrichtungen.
- Ergebnis (Version 0.4.4): **296 von 298 QSOs erklärt (99 %)**, 243 (82 %) innerhalb des Fensters von 370-1100 km; Median-QTF von EI5FK zwischen 25° und 41°, also der von G3WPO empfohlene Nordosten. Die Logs geben die Antennenrichtung nicht an: Die Prüfung betrifft die Geometrie, nicht das QTF.
