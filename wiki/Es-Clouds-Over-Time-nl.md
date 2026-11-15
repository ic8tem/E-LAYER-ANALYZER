# Es-WOLKEN IN DE TIJD

[← Home](Home-nl)

![Es-WOLKEN IN DE TIJD](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Subtab van **SPORADIC-E** (vanaf versie 0.5.2): hoe de **wolken van sporadische E** stap voor stap ontstaan, verschuiven en uitdoven.

## De methode

Methode van **Chartier, Hanley en Emmons (2022)**: elke verbinding via sporadische E zegt dat de foEs in het reflectiepunt **minstens f / sec φ** is. De punten worden verzameld in cellen en tijdstappen, en elke cel neemt de **hoogste waarde** van de stap. De minimale elektronendichtheid is NmE = 1,24·10¹⁰ · foEs² elektronen per kubieke meter.

Het programma gebruikt de reflectiepunten van zijn eigen model (alle sprongen), niet alleen het middelpunt. De waarde is een **ondergrens**: een lege cel betekent alleen dat er geen verbinding is.

## Bron en bediening

- **Spots van de sessie** (bijgewerkt bij elk openen van het tabblad) of **ADIF-log of CSV-spots laden**.
- **Spots van de sessie exporteren (CSV)**: het spotarchief wordt geleegd bij het sluiten van het programma, zo kun je ze later terugzien.
- **Banden MHz** (28, 40, 50, 70, 144), **Stap** (5, 10 of 15 minuten), **Cel** (1°, 2° of 4°), **Es-hoogte km**.
- Tijdbalk: ⏮ ◀ *Starten/Stoppen* ▶ ⏭ en de schuifregelaar; bij het openen begint hij bij de stap met de meeste cellen.
- *Eerdere stappen vervaagd* toont de drie stappen vóór de gekozen stap; *Verbindingen* tekent de trajecten in de kleuren van de banden.

## Tabel en grafiek

- Cellen van de stap: Cel (zuidwestelijke hoek en locator van het midden), foEs≥, NmE≥, Verbindingen, Banden. Klik op een cel: de verbindingen ervan worden gemarkeerd.
- Grafiek: hoogste minimale foEs (gele lijn) en aantal cellen (staven) voor elke stap.
