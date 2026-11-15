# Es-WOLKEN IM ZEITVERLAUF

[← Home](Home-de)

![Es-WOLKEN IM ZEITVERLAUF](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Unterreiter von **SPORADIC-E** (ab Version 0.5.2): wie die **Sporadic-E-Wolken** Schritt für Schritt entstehen, wandern und erlöschen.

## Die Methode

Methode von **Chartier, Hanley und Emmons (2022)**: Jede Sporadic-E-Verbindung besagt, dass die foEs am Reflexionspunkt **mindestens f / sec φ** beträgt. Die Punkte werden in Zellen und Zeitschritten gesammelt, und jede Zelle übernimmt den **höchsten Wert** des Schritts. Die minimale Elektronendichte ist NmE = 1,24·10¹⁰ · foEs² Elektronen pro Kubikmeter.

Das Programm verwendet die Reflexionspunkte seines eigenen Modells (alle Sprünge), nicht nur den Mittelpunkt. Der Wert ist eine **Untergrenze**: Eine leere Zelle bedeutet nur, dass es keine Verbindung gibt.

## Quelle und Bedienelemente

- **Spots der Sitzung** (bei jedem Öffnen des Reiters aktualisiert) oder **ADIF-Log oder CSV-Spots laden**.
- **Spots der Sitzung exportieren (CSV)**: Das Spot-Archiv wird beim Schließen des Programms geleert, so können Sie sie später erneut ansehen.
- **Bänder MHz** (28, 40, 50, 70, 144), **Schritt** (5, 10 oder 15 Minuten), **Zelle** (1°, 2° oder 4°), **Es-Höhe km**.
- Zeitleiste: ⏮ ◀ *Starten/Stoppen* ▶ ⏭ und der Schieberegler; beim Öffnen beginnt sie mit dem Schritt mit den meisten Zellen.
- *Frühere Schritte verblasst* zeigt die drei Schritte vor dem gewählten; *Verbindungen* zeichnet die Strecken in den Farben der Bänder.

## Tabelle und Diagramm

- Zellen des Schritts: Zelle (Südwestecke und Locator der Mitte), foEs≥, NmE≥, Verbindungen, Bänder. Klick auf eine Zelle: Ihre Verbindungen werden hervorgehoben.
- Diagramm: höchste minimale foEs (gelbe Linie) und Zahl der Zellen (Balken) für jeden Schritt.
