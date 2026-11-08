# BENCHMARK Es

[← Home](Home-de)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Prüfstand für **Sporadic-E** und **Mehrfachsprung** an einem **ganzen ADIF-Log**, auch dem einer anderen Station: Aus den tatsächlich gefahrenen QSOs skizziert das Programm Skip und Wolken, Tag für Tag und Stunde für Stunde.

## Das Log laden

- **ADIF laden**: wählt die `.adi`/`.adif`-Datei. Das Programm merkt sich die Datei und lädt sie beim Start erneut.
- **ADIF entladen**: entfernt das Log aus dem Reiter.
- **Station in der Mitte**: Rufzeichen und Locator der Station, die das Log erstellt hat. Das Programm schlägt das häufigste `STATION_CALLSIGN` (oder `OPERATOR`) und `MY_GRIDSQUARE` vor; sie lassen sich von Hand ändern und mit **Übernehmen** bestätigen. Die Mitte hängt nicht von den allgemeinen Einstellungen ab, sodass sich auch Logs anderer Stationen untersuchen lassen.
- Sendeposition: das `MY_GRIDSQUARE` jedes QSOs (falls es fehlt, der Locator in der Mitte). Empfangspositionen: die Felder `GRIDSQUARE`.
- Verwendete QSOs: 28, 50, 70 und 144 MHz mit Locator. QSOs mit anderem `PROP_MODE` (TR, MS, FAI, AUR, EME...) und solche unter 500 km lassen sich ausschließen. 432 MHz wird nie einbezogen: Sporadic-E gilt dort nicht.

## Tage und Stunden

- **QSOs pro Tag**: Tag, Anzahl der QSOs, Bänder, größte Entfernung, Mehrfachsprünge. Beim Öffnen schlägt das Programm den Tag mit den meisten QSOs vor.
- **Stunden des Tages**: die Zeile *Ganzer Tag* und eine Zeile für jede UTC-Stunde; ein Klick zeigt nur die QSOs dieser Stunde.
- **QSO**: die Liste zur Auswahl; ein Klick auf ein QSO (oder auf der Karte in die Nähe der Gegenstation) hebt es hervor und zeigt seine Details.

## Die Karte

**Mittabstandstreue Azimutalkarte**, zentriert auf die Station, mit dem Schieberegler **Maximale Entfernung** zum Zoomen (auch mit dem Mausrad): Die Ringe zeigen die **km von der Mitte**.

- **Skip nach Sektoren** (10° Azimut): oranger Bogen = nächstes QSO des Sektors (Rand der Skip-Zone), grüner Bogen = entferntestes QSO, dazwischen der erreichte Bereich.
- **Es-Wolken** auf den Maidenhead-Feldern, gefärbt nach der geschätzten foEs.
- Mittelpunkt der Einzelsprünge (gelb), erster Sprung der Station (orange, hohler Kreis = kurzer erster Sprung), erster Sprung der Gegenstation (violett).
- Für den ganzen Tag verbindet die **rote Linie** Stunde für Stunde den Schwerpunkt der Mittelpunkte: Das ist die Drift der Wolken.

![Eine Stunde](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Vorgeschlagene Berechnungen

1. **Skip nach Sektoren**: für jeden Sektor die Skip-Entfernung und die Reichweite, mit den Rufzeichen.
2. **Geschätzte foEs** mit dem Sekantengesetz (foEs ≥ f / sec φ): Der kürzeste Einzelsprung auf jedem Band liefert die höchste Schätzung; mit dieser foEs die erwartete MUF bei 1000, 1500, 2000 und 2400 km und die Entfernung, ab der 144 MHz öffnen würde.
3. **Es-Wolken**, geschätzt auf den Feldern, mit Anzahl der QSOs und foEs.
4. **Mehrfachsprung**: der erste Sprung der Station unter den beiden Hypothesen (gleiche Sprünge sowie kurzer erster Sprung bis 1200 km, Erfahrung von IC8TEM), verglichen mit den im Einzelsprung gesehenen Wolken innerhalb von ±30 Minuten; innerhalb von 300 km gilt die Hypothese als bestätigt. Das Programm zählt die Bestätigungen jeder Hypothese.
5. **Drift der Wolken** Stunde für Stunde: km, Richtung und Geschwindigkeit.

Die Berechnungen sind Richtwerte: Ein QSO sagt, dass die Strecke offen war, nicht genau, wo die Wolke war. Je sauberer die Daten (6-stellige Locator, korrektes `PROP_MODE`), desto zuverlässiger der Skip.
