# SPORADIC-E

[← Home](Home-de)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Die Karte

Eine Schaltfläche über der Karte wählt die Ansicht:

- **Azimutal - Einzelsprung**: mittabstandstreue Azimutalkarte, zentriert auf Ihre Station, fester Radius 2500 km, mit den Zonen der Tabelle IK3XTV (Einzelsprung mindestens 500 km, optimal 1500-2000 km, höchstens 2400 km).
- **Azimutal - maximale Entfernung (Mehrfachsprung)**: dieselbe Karte mit dem Schieberegler **Maximale Entfernung** (von 300 bis 20000 km, auch mit dem Mausrad); zusätzlich erscheinen die Grenzen des Doppelsprungs (5000 km) und des Dreifachsprungs (6900 km), sodass sich die Entwicklung des Mehrfachsprungs verfolgen lässt.
- **Rechteckig**: die bisherige Weltkarte mit den Schaltflächen Europa und Welt.

In den Azimutalansichten zeigen die gestrichelten Ringe die **km von der Mitte** (die Schrittweite ändert sich mit dem Zoom), der Rand markiert die größte angezeigte Entfernung, und der Mauszeiger liefert Entfernung und Azimut von der Mitte.

![Mehrfachsprung](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Was angezeigt wird

- Die **Spots** der letzten Minuten (15 min - 12 h) auf den Bändern **28, 50, 70 und 144 MHz**, aus den Telnet-Sitzungen. Die Locator stammen aus dem Kommentar des Spots (z. B. `JN70CN<ES>JO62QM`) oder aus der CALL3-Datenbank.
- Spots mit **<MS>**, **<AU>** oder **<TR>** im Text werden **verworfen**: Sie betreffen nicht Sporadic-E, sind aber für Meteorscatter, Aurora oder Tropo nützlich.
- **Transäquatorial, Regel des Autors (IC8TEM)**: Richtung Afrika und Südamerika gilt eine Verbindung als transäquatorial (TEP) und nicht als Sporadic-E, wenn der Mittelpunkt im Band der magnetischen Inklination zwischen -20° und +20° liegt, es sei denn, der Spot gibt `<ES>` an.
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
