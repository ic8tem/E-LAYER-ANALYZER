# IONOSONDEN

[← Home](Home-de)

![IONOSONDEN](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Unterreiter von **SPORADIC-E** (ab Version 0.5.2): die von den Ionosonden des **GIRO**-Netzes in aller Welt **gemessene foEs**, neben der, die das Programm aus den Spots ableitet.

## Die Daten

- 62 GIRO-Ionosonden (ULCAR-Liste), mit der letzten **foEs**, **foF2** und **MUF(D)**, automatisch aus den Ionogrammen gelesen.
- Die Daten kommen beim ersten Öffnen des Reiters und danach **alle 15 Minuten** von selbst (Kontrollkästchen *Alle 15 Minuten selbst aktualisieren*); *Jetzt aktualisieren* fordert sie sofort an. Das Archiv hält die letzten 48 Stunden im Ordner `giro` der Daten.
- Daten von **GIRO** (Reinisch und Galkin 2011), Lizenz **CC BY-NC-SA 4.0**: nur nichtkommerzielle Nutzung. Der Dienst ist derselbe wie in *MUF Muncher* von DM6AX.

## Karte und Tabelle

- Jede Ionosonde ist ein farbiger Punkt mit der letzten foEs (dieselben Farben wie die Es-Wolken) und dem Wert in MHz; **grau**, wenn sie keine aktuellen Daten hat, mit dem **Ring**, wenn das letzte Ionogramm älter als 90 Minuten ist.
- Tabelle: Kennung, Ionosonde, km von Ihrer Station, UTC des letzten Ionogramms, foEs, foF2, MUF(D) und **Spot foEs≥**: die minimale foEs aus den Es-Spots der letzten 30 Minuten, die im Umkreis von 500 km um die Ionosonde reflektieren (einstellbar), mit der Zahl der Spots.

## Die gewählte Ionosonde

Klick auf eine Zeile oder neben eine Ionosonde auf der Karte:

- **Diagramm der letzten 24 Stunden**: foEs gelb, foF2 hellblau, MUF(D) violett gepunktet;
- **MUF des Sporadic-E** bei 1000, 1500, 2000 und 2400 km (MUF = foEs · sec φ, in der gewählten Es-Höhe) mit den **Bändern, die sich öffnen** (28, 40, 50, 70, 144 MHz);
- verlangen die Spots mehr als die gemessene foEs, weist das Programm darauf hin: Die Wolke kann weit von der Ionosonde entfernt sein, oder die Ionosonde hat die Grenze ihres Frequenzbereichs erreicht (Chartier 2022).
