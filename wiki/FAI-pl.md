# F.A.I.

[← Strona główna](Home-pl)

F.A.I. (Field Aligned Irregularities) to rozpraszanie na nieregularnościach zorientowanych wzdłuż pola geomagnetycznego, typowe dla letnich wieczorów po E-sporadycznym. Rozproszony sygnał wychodzi na **stożku**, którego oś leży wzdłuż pola: dwie stacje mogą nawiązać łączność, jeśli leżą na **konturach dopełniających** (a1 + a2 = 180°, artykuł IC8FAX-IC8TEM; kontury ±c wg K9SE). Pole magnetyczne jest obliczane modelem **IGRF-14**.

## Widok Scatter trail (para A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Wpisz **Stację A** i **Stację B** (znak wywoławczy wyszukiwany w CALL3 i/lub lokator). Puste A = własna stacja.
- Mapa pokazuje **scatter trail**: wszystkie punkty na wysokości warstwy, widziane przez obie stacje, które spełniają warunek odbicia w granicach 2,5° (klasy 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Ramka w prawym górnym rogu pokazuje **QTF i elewację** A (czerwony) i B (zielony) w stronę każdego punktu.
- Program proponuje **najbardziej prawdopodobny scatter point** (minimalne odchylenie, najkrótsza trasa).
- **Refrakcja 4/3**: domyślnie włączona, poszerza horyzonty.

## Widok Izorefleksja (isoflection loci)

![Izorefleksja](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

To konstrukcja geometryczna służąca do **sprawdzenia, czy punkt odbicia jest właściwy**.

- **Lewy klik** na mapie: wybierasz punkt do sprawdzenia. **Prawy klik**: przesuwa stację B.
- **Sprawdź zalecany punkt**: otwiera wykres dla punktu zaproponowanego przez program.
- Poziomice na **0, ±1, ±2, ±4, ±8, ±16°**: kolorowe pasy w schematach ciemnych, **grube linie** w schematach jasnych.
- Werdykt dla pary: **zadowalający** w granicach 1°, **dopuszczalny** w granicach 2,5°, **niezadowalający** powyżej.
- Jeśli punkt jest poniżej horyzontu jednej ze stacji, pojawia się okienko **Obliczenie niemożliwe** i nic więcej.

## Weryfikacje

| Łączność | Scatter point | Odchylenie | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (nad Genuą) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (płd.-zach. Niemcy, 28 km od Brukseli) | 0,88° | 338° / 87° |
| DB0FAI - Drezno przez Sheffield (DL5MCG) | – | 0,3° | – |

Znane scatter pointy na liście: Genewa, Bruksela, Budapeszt, JN92, JN41/42.
