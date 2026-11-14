# F.A.I.

[← Home](Home-de)

F.A.I. (Field Aligned Irregularities) ist die Streuung an Irregularitäten, die entlang des Erdmagnetfelds ausgerichtet sind, typisch für Sommerabende nach Sporadic-E. Das gestreute Signal verlässt die Irregularität auf einem **Kegel**, dessen Achse entlang des Feldes liegt: Zwei Stationen können miteinander arbeiten, wenn sie auf **supplementären Konturen** liegen (a1 + a2 = 180°, Artikel IC8FAX-IC8TEM; ±c-Konturen von K9SE). Das Magnetfeld wird mit dem Modell **IGRF-14** berechnet.

## Ansicht Scatter trail (Paar A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Geben Sie **Station A** und **Station B** ein (Rufzeichen, in CALL3 nachgeschlagen, und/oder Locator). A leer = Ihre eigene Station.
- Die Karte zeigt den **Scatter trail**: alle Punkte in Schichthöhe, die von beiden Stationen gesehen werden und die Reflexionsbedingung innerhalb von 2,5° erfüllen (Klassen 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Das Feld oben rechts zeigt **QTF und Elevation** von A (rot) und B (grün) zu jedem Punkt.
- Das Programm schlägt den **wahrscheinlichsten Streupunkt** vor (minimale Abweichung, kürzeste Strecke).
- **Refraktion 4/3**: standardmäßig eingeschaltet, erweitert sie die Horizonte.

## Ansicht Iso-Reflexion (Iso-Reflexionslinien)

![Iso-Reflexion](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Das ist die geometrische Konstruktion, mit der **geprüft wird, ob ein Reflexionspunkt stimmt**.

- **Linksklick** auf die Karte: wählt den zu prüfenden Punkt. **Rechtsklick**: verschiebt Station B.
- **Empfohlenen Punkt prüfen**: öffnet das Diagramm auf dem vom Programm vorgeschlagenen Punkt.
- Höhenlinien bei **0, ±1, ±2, ±4, ±8, ±16°**: farbige Bänder in den dunklen Schemata, **dicke Linien** in den hellen Schemata.
- Urteil über das Paar: **zufriedenstellend** innerhalb 1°, **akzeptabel** innerhalb 2,5°, **nicht zufriedenstellend** darüber.
- Liegt der Punkt unter dem Horizont einer Station, erscheint nur die Meldung **Berechnung nicht möglich**.

## Prüfungen

| Verbindung | Streupunkt | Abweichung | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (über Genua) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (Südwestdeutschland, 28 km von Brüssel) | 0,88° | 338° / 87° |
| Bake DB0FAI (305°) - Dresden über Sheffield (DL5MCG; Dresden richtete auf 290° und meldete Aurora) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, IGRF-Feld von 1990) | JN45MV, Spur 40 km von Genf (Genf: 4,10°) | 0,76° | 326° / 85° |
| DL5MCG - SV1DH (Test von 1987 über Budapest, 1569 km, IGRF-Feld von 1987) | Budapest bei 2,53°, knapp über 2,5° (2,16° mit dem heutigen Feld); Spur 26 km entfernt; wahrscheinlichster Punkt KN06RV | 2,53° (KN06RV 0,93°) | 95° / 341° |

Ab Version 0.4.18 verwenden die beiden Fälle mit Datum (1987 und 1990) das IGRF-14-Feld jenes Jahres, solange die Stationen die des Falls bleiben.

Bekannte Streupunkte in der Liste: Genf, Brüssel, Budapest, JN92, JN41/42.

Das Menü **Prüfungen...** lädt die Testpaare IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE und DL5MCG-SV1DH.

**Ausrüstung** (DL5MCG): F.A.I. auf 144 MHz erfordert mindestens eine lange Yagi und 100 W (SV1DH wurde auch mit einer 8-Element-Yagi und 50 W gearbeitet). Richten Sie die Antenne auf den vom Programm angegebenen Streupunkt, nicht auf die Gegenstation.
