# Quellen und Dokumente

[← Home](Home-de)

- Der Reiter **QUELLEN** listet alle Quellen auf, nummeriert [1]-[63].
- Der Reiter **ZUSAMMENFASSUNG DER BERECHNUNGSENTSCHEIDUNGEN** (früher *Technisches Dokument*) erklärt jede Berechnung mit ihren Formeln und den Kennzeichen **QUELLE** (Schritt aus einer Quelle übernommen), **PRÜFUNG** (am Programm oder an einem realen Fall verifiziert) und **WAHL** (eine Entscheidung des Programms, im Funkbetrieb zu bestätigen).
- Der Reiter **BEDIENUNGSANLEITUNG** ist die ausführliche Bedienungsanleitung des Programms, Reiter für Reiter.
- Der Reiter **TECHNISCHER BERICHT** legt die Entscheidungen dar, die zur Entwicklung des Programms geführt haben: Architektur, physikalische Modelle, Datenerkennung, Prüfungen, Grenzen und künftige Entwicklungen.
- Die drei Dokumente haben dieselbe Gestaltung, sind in jede Sprache übersetzt und lassen sich nach Word exportieren.

Wichtigste Formeln:

- Elevation zu einem Punkt der Schicht: `e = atan2((r+H)·cos x − (r+h), (r+H)·sin x)`, mit x = d / r und Refraktion k = 4/3;
- Sekantengesetz: `MUF = foEs · sec φ`;
- Reflexionsbedingung F.A.I./Aurora: `c(A) + c(B) = 0`, wobei c = Winkel zwischen der Sichtlinie und der Ebene senkrecht zum IGRF-14-Feld.
