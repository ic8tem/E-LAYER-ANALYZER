# F.A.I.

[← Home](Home)

F.A.I. (Field Aligned Irregularities) is scatter from irregularities aligned with the geomagnetic field, typical of summer evenings after sporadic-E. The scattered signal leaves on a **cone** whose axis lies along the field: two stations can work each other if they lie on **supplementary contours** (a1 + a2 = 180°, IC8FAX-IC8TEM article; ±c contours of K9SE). The magnetic field is computed with the **IGRF-14** model.

## Scatter trail view (A-B pair)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Enter **Station A** and **Station B** (callsign looked up in CALL3 and/or locator). A left empty = your own station.
- The map shows the **scatter trail**: all points at layer height, seen by both stations, that meet the reflection condition within 2.5° (classes 0-1, 1-1.5, 1.5-2, 2-2.5°).
- The box at the top right shows **QTF and elevation** of A (red) and B (green) towards each point.
- The program suggests the **most likely scatter point** (minimum deviation, shortest path).
- **Refraction 4/3**: on by default, it extends the horizons.

## Isoflection view (isoflection loci)

![Isoflection](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

This is the geometric construction used to **check whether a reflection point is right**.

- **Left-click** on the map: you choose the point to check. **Right-click**: moves station B.
- **Check the recommended point**: opens the chart on the point suggested by the program.
- Contour lines at **0, ±1, ±2, ±4, ±8, ±16°**: coloured bands in the dark schemes, **thick lines** in the light schemes.
- Verdict on the pair: **satisfactory** within 1°, **acceptable** within 2.5°, **not satisfactory** beyond.
- If the point is below the horizon of one station, the **Calculation not possible** popup appears and nothing else.

## Checks

| Contact | Scatter point | Deviation | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (above Genoa) | 0.97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (SW Germany, 28 km from Brussels) | 0.88° | 338° / 87° |
| DB0FAI beacon (305°) - Dresden via Sheffield (DL5MCG; Dresden beamed 290° and reported aurora) | – | 0.3° | – |
| DL5MCG - SV1DH (1987 test via Budapest, 1569 km) | Budapest on the trail; cells below 1° from 35 km | 2.16° | 95° / 341° |

Known scatter points in the list: Geneva, Brussels, Budapest, JN92, JN41/42.

The **Checks...** menu loads the test pairs IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE and DL5MCG-SV1DH.

**Equipment** (DL5MCG): F.A.I. on 144 MHz needs at least a long yagi and 100 W (SV1DH was also worked with an 8-element yagi and 50 W). Point the antenna at the scatter point given by the program, not at the other station.
