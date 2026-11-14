# SPORADIC-E

[← Home](Home-de)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Die Karte

Eine Schaltfläche über der Karte wählt die Ansicht:

- **Azimutal**: mittabstandstreue Azimutalkarte, zentriert auf Ihre Station, mit dem Schieberegler **Zoom** (300 bis 20000 km von der Mitte, auch mit dem Mausrad).
- **Mercator**: die Weltkarte, mit den Schaltflächen Europa und Welt.

In beiden Projektionen werden um die Station die **Entfernungskreise von Flavio IK3XTV** gezeichnet: 500 km (Minimum des Einzelsprungs), 1500-2000 km (Optimum), 2400 km (Maximum des Einzelsprungs), 5000 km (Maximum des Doppelsprungs) und 6900 km (Maximum des Dreifachsprungs).

In den Azimutalansichten zeigen die gestrichelten Ringe die **km von der Mitte** (die Schrittweite ändert sich mit dem Zoom), der Rand markiert die größte angezeigte Entfernung, und der Mauszeiger liefert Entfernung und Azimut von der Mitte.

![Mehrfachsprung](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Was angezeigt wird

- Die **Spots** der letzten Minuten (15 min - 12 h) auf den Bändern **28, 40 (8 m, standardmäßig aus), 50, 70 und 144 MHz**, aus den Telnet-Sitzungen. Die Locator stammen aus dem Kommentar des Spots (z. B. `JN70CN<ES>JO62QM`) oder aus der CALL3-Datenbank.
- Spots mit **<MS>**, **<AU>** oder **<TR>** im Text werden **verworfen**: Sie betreffen nicht Sporadic-E, sind aber für Meteorscatter, Aurora oder Tropo nützlich.
- **Transäquatorial, Regel des Autors (IC8TEM)**: Richtung Afrika und Südamerika gilt eine Verbindung als transäquatorial (TEP) und nicht als Sporadic-E, wenn der Mittelpunkt im Band der magnetischen Inklination zwischen -15° und +15° liegt, es sei denn, der Spot gibt `<ES>` an.
- **144 MHz, Regel des Autors (IC8TEM)**: Sporadic-E auf 2 m beginnt **jenseits von 900 km** (kürzere Spots werden verworfen). Ein einzelner 144-MHz-Spot **wird nicht allein eingezeichnet**: Er erscheint, wenn innerhalb von **5 Minuten** ein zweiter 144-MHz-Spot mit der Wolke an derselben Stelle eintrifft (Reflexionspunkte innerhalb von 300 km) oder wenn es in der Nähe einen **50-MHz-Spot mit kurzem Sprung** gibt (innerhalb von 1200 km, in den letzten 15 Minuten). Die Leiste oben zeigt, wie viele 144-MHz-Spots auf Bestätigung warten.
- Für jeden Spot: Entfernung, **Anzahl der Sprünge** nach der Tabelle **IK3XTV**, Elevation, **minimale foEs** (Sekantengesetz).
- **Erster Sprung beider Stationen**: oranges Quadrat = erster Sprung des Spotters, violett = erster Sprung des DX (gleiche Sprünge); gelb = Mittelpunkt eines Einzelsprungs.
- **Kurzer erster Sprung** (hohle Kreise): Bei Mehrfachsprung-Öffnungen hat der Autor einen kurzen ersten Sprung innerhalb von **1200 km** um sein QTH erlebt; die Länge ist einstellbar.
- **Es-Wolken** auf **Maidenhead-Feldern** (2° x 1°), gefärbt nach minimaler foEs: blau < 5, hellblau < 7, grün < 10, gelb < 14, orange < 20, rot > 20 MHz.
- Tabelle **Es-Wolken**: für jedes Feld die **MUF für Ihre Station** (rot = 144 MHz möglich, orange = 70, gelb = 50).

## Tabelle IK3XTV

| | Minimum | Optimum | Maximum |
|---|---|---|---|
| Einzelsprung | 500-650 km | 1500-2000 km | 2200-2400 km |
| Doppelsprung | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Dreifachsprung | – | 4800-6400 km | 6900 km |

## Bedienung

- **Klick auf einen Spot**: Die Strecke wird hervorgehoben, mit Reflexionspunkten, Locatorn und Azimuten in den Details.
- **Klick auf einen Spot** in der Tabelle: Die Strecke leuchtet auf der Karte auf (gelber Schein) und, falls sie außerhalb der Ansicht liegt, verschiebt sich die Karte oder vergrößert ihren Radius, um sie zu zeigen. Der Spot bleibt ausgewählt, wenn neue Spots eintreffen.
- **Die Tabelle folgt der Karte**: Bei eingestelltem Zoom zeigen die Spot-Tabelle und die Wolkentabelle nur, was auf der Karte sichtbar ist.
- **Klick auf die Karte**: Der Punkt wird als mögliche Es-Wolke behandelt, mit der für 28, 50, 70 und 144 MHz nötigen foEs.

## Hinweis zu 432 MHz

Sporadic-E **gilt nicht für 432 MHz**: Die Es-MUF erreicht etwa 150-200 MHz, und die ARRL-Rekordlisten führen Es-Verbindungen nur auf 50 und 144 MHz. 70-cm-Spots werden hier nicht analysiert.

## Nach Frequenz gefärbte Felder und blinkende MUF

Die Maidenhead-Felder der Reflexionspunkte werden nach der höchsten Frequenz der Spots gefärbt: **grün** bis 29 MHz, **gelb** bis 51, **orange** bis 71, **rot** bis 145 MHz. Aus der foEs des Feldes wird die wahrscheinliche MUF auf dem längsten Sprung (2400 km, Tabelle IK3XTV) geschätzt: Erreicht sie ein höheres Band, **blinkt** das Feld in der Farbe dieses Bandes (Prinzip von IK3XTV und Jacobs-Leinwoll: kurzer Skip bedeutet hohe MUF).

Oben rechts auf der Karte erscheint die Zusammenfassung von N0NBH (E-skip für den gewählten Kontinent, SFI, K- und A-Index).

## Regeln je Band (Version 0.4.18)

- **50 MHz**: kein Sporadic-E innerhalb von 450 km. FT8- und FT4-Spots sind nur dann Sporadic-E, wenn sie zu einer Wolke gehören: ein anderer 50-MHz-Spot innerhalb von 4 Minuten mit Mittelpunkt innerhalb von 150 km. Ein einzelner FT8/FT4-Spot geht zu Meteorscatter.
- **70 MHz**: kein Sporadic-E innerhalb von 450 km. FT8 und FT4 gehen zu Sporadic-E, auch innerhalb des Meteorscatter-Fensters des Bandplans (zuerst entscheidet die bekannte Betriebsart, dann die Frequenz).
- **40 MHz (8 m)**: Mindestentfernung aus den 450 km von 50 MHz mit dem Sekantengesetz abgeleitet (etwa 330 km); nur Entfernungen innerhalb des ersten Sprungs der Geometrie der E-Schicht (etwa 2440 km). Farbe der Strecke: gelb.
- **144 MHz**: Sporadic-E ab 900 km, mit Bestätigung; Q65- und JT65-Spots sind EME und werden verworfen, außer bei `<ES>` im Text.
- G0ISW (Propagation identification, 2024) sieht das kürzeste Sporadic-E über 400 km auf 50 und 70 MHz und über 1400 km auf 144 MHz: Die Regeln des Autors stimmen auf 50 und 70 MHz überein und sind auf 144 MHz weiter gefasst, verlangen aber eine Bestätigung.
- Die Reihenfolge, in der alle Regeln angewandt werden, mit den verwendeten Werten, zeigt **[EINSTELLUNGEN](Settings-de)** › Spot-Verteilung.
