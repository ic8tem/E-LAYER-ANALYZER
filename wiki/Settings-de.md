# EINSTELLUNGEN

[← Home](Home-de)

Ab Version 0.4.18 fasst der Reiter **EINSTELLUNGEN** die allgemeinen Optionen des Programms und die Verteilung der Spots auf die Reiter zusammen. Er hat zwei Unterreiter.

## Allgemein

| Bereich | Was man einstellt |
|---|---|
| Station | Rufzeichen, Locator und Höhe, dieselben wie in der oberen Leiste; **Übernehmen** bestätigt sie für alle Reiter. |
| Darstellung | Sprache, Farbschema (die Schemata von YAEC und YA-MS und ab Version 0.5.0 die sechs Themen von IP-TRX-CTRL: Hell, Nacht, Schwarz, Blau, Violett, Grün) und **Schriftgröße des ganzen Programms** (85-140 %, wie in YA-MS). Sprache und Farben bauen das Fenster neu auf: Einstellungen und Spots bleiben erhalten. |
| Karten und Vorhersagedaten | Der Kontinent, dasselbe Menü wie in der Reiterleiste. |
| Zeit | NTP-Status und Differenz zwischen der Serverzeit und der PC-Uhr. |
| Aktualisierungen | Prüfung auf GitHub beim Start und alle 30 Minuten, automatische und stille Aktualisierung, Schaltfläche **Jetzt prüfen**. Die automatische Aktualisierung gilt nicht für Systempakete (.deb, .rpm, Arch, Slackware). |
| Daten | Schaltfläche, die den Ordner `data` öffnet. Der API-Schlüssel des Bureau of Meteorology bleibt in DATA COLLECTION › Ausbreitungsdaten. |

## Spot-Verteilung

Links listet eine Tabelle die Regeln auf, mit denen das Programm entscheidet, wohin jeder Spot geht, **in der Reihenfolge, in der sie angewandt werden**: Die erste zutreffende Regel entscheidet über den Reiter. Die angezeigten Werte sind die in den Filtern eingestellten.

| # | Wenn der Spot... | geht an |
|---|---|---|
| 1 | außerhalb des lokalen Filters liegt (unter 27990 kHz oder über 145 MHz; 430-440 MHz nur Aurora) | verworfen |
| 2 | AUE, AU-E oder AURORAL E enthält | AURORA (Aurora-E) |
| 3 | ein Aurora-Wort enthält (voreingestellt AU, AUR) | AURORA |
| 4 | ein F.A.I.-Wort enthält (voreingestellt FAI; Bänder 6, 4 und 2 m: F.A.I. reicht nicht bis 432 MHz) | F.A.I. |
| 5 | eine Meteorscatter-Betriebsart oder `<MS>` hat | METEOR SCATTER |
| 6 | in ein MS-Fenster des Bandplans fällt (auf 50 MHz nicht ausschließlich; FT8 und FT4 auf 50 und 70 MHz ausgenommen) | METEOR SCATTER |
| 7 | FT8 oder FT4 auf 50 MHz ist, ohne anderen Spot innerhalb von 4 min mit Mittelpunkt innerhalb von 150 km | Meteorscatter (nicht Sporadic-E) |
| 8 | Q65 oder JT65 auf 144 MHz ist (außer bei `<ES>`) | EME: verworfen |
| 9 | eine Kennung einer anderen Ausbreitung enthält | nicht Sporadic-E |
| 10 | transäquatorial ist | nicht Sporadic-E (TEP) |
| 11-15 | auf 28, 40, 50, 70 oder 144 MHz innerhalb der Entfernungsregeln liegt | SPORADIC-E |
| 16 | unter keine Regel fällt (Tropo, 432 MHz, KW...) | kein Reiter (bleibt in Gesammelte Spots) |

Rechts stehen der **lokale Filter** und die Filter jeder Betriebsart, in fünf Unterreitern: Meteorscatter (der Filter von YA-MS wird im Reiter METEOR SCATTER eingestellt), Sporadic-E, F.A.I., Aurora und **Filtertest**, in den man eine Cluster-Zeile einfügt, um zu sehen, wie sie eingeordnet wird. Die Schaltfläche **Regeln des Autors wiederherstellen** stellt die Ausgangswerte wieder her.
