# F.A.I.

[← Home](Home-pl)

F.A.I. (Field Aligned Irregularities) to rozpraszanie na nieregularnościach zorientowanych wzdłuż pola geomagnetycznego, typowe dla letnich wieczorów po sporadycznej warstwie E. Rozproszony sygnał wychodzi po powierzchni **stożka**, którego oś biegnie wzdłuż pola: dwie stacje mogą nawiązać łączność, jeśli leżą na **konturach uzupełniających** (a1 + a2 = 180°, artykuł IC8FAX-IC8TEM; kontury ±c według K9SE). Pole magnetyczne jest obliczane modelem **IGRF-14**.

## Widok Scatter trail (para A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Wpisz **Stację A** i **Stację B** (znak wyszukiwany w CALL3 i/lub lokator). Puste A = twoja stacja.
- Mapa pokazuje **scatter trail**: wszystkie punkty na wysokości warstwy, widoczne z obu stacji, które spełniają warunek odbicia z dokładnością do 2,5° (klasy 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Ramka w prawym górnym rogu pokazuje **QTF i elewację** A (czerwony) i B (zielony) w kierunku każdego punktu.
- Program proponuje **najbardziej prawdopodobny punkt rozpraszania** (minimalne odchylenie, najkrótsza trasa).
- **Refrakcja 4/3**: domyślnie włączona, poszerza horyzonty.

## Widok Izorefleksja (linie izorefleksji)

![Izorefleksja](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

To konstrukcja geometryczna służąca do **sprawdzenia, czy punkt odbicia jest prawidłowy**.

- **Lewy klik** na mapie: wybierasz punkt do sprawdzenia. **Prawy klik**: przesuwa stację B.
- **Sprawdź zalecany punkt**: otwiera wykres w punkcie zaproponowanym przez program.
- Poziomice przy **0, ±1, ±2, ±4, ±8, ±16°**: kolorowe pasy w ciemnych schematach, **grube linie** w jasnych schematach.
- Ocena pary: **zadowalająca** do 1°, **akceptowalna** do 2,5°, **niezadowalająca** powyżej.
- Jeśli punkt leży poniżej horyzontu jednej ze stacji, pojawia się okno **Obliczenie niemożliwe** i nic więcej.

## Sprawdzenia

| Łączność | Punkt rozpraszania | Odchylenie | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (nad Genuą) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (płd.-zach. Niemcy, 28 km od Brukseli) | 0,88° | 338° / 87° |
| DB0FAI - Dresden via Sheffield (DL5MCG) | – | 0,3° | – |

Znane punkty rozpraszania na liście: Genewa, Bruksela, Budapeszt, JN92, JN41/42.
