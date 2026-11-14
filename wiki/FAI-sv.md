# F.A.I.

[← Home](Home-sv)

F.A.I. (Field Aligned Irregularities) är scatter från irreguljariteter som är orienterade längs jordmagnetfältet, typiskt för sommarkvällar efter sporadiskt E. Den spridda signalen lämnar på en **kon** vars axel ligger längs fältet: två stationer kan köra varandra om de ligger på **supplementära konturer** (a1 + a2 = 180°, artikeln av IC8FAX-IC8TEM; ±c-konturerna hos K9SE). Magnetfältet beräknas med modellen **IGRF-14**.

## Vyn Scatter trail (par A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Ange **Station A** och **Station B** (anropssignal som slås upp i CALL3 och/eller lokator). A lämnad tom = din egen station.
- Kartan visar **scatter trail**: alla punkter på skiktets höjd, sedda av båda stationerna, som uppfyller reflexionsvillkoret inom 2,5° (klasserna 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Rutan uppe till höger visar **QTF och elevation** för A (röd) och B (grön) mot varje punkt.
- Programmet föreslår den **mest sannolika scatterpunkten** (minsta avvikelse, kortaste sträcka).
- **Refraktion 4/3**: på som standard, utvidgar horisonterna.

## Vyn Isoreflexion (isoreflexionslinjer)

![Isoreflexion](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Detta är den geometriska konstruktion som används för att **kontrollera om en reflexionspunkt är rätt**.

- **Vänsterklick** på kartan: du väljer punkten som ska kontrolleras. **Högerklick**: flyttar station B.
- **Kontrollera den rekommenderade punkten**: öppnar diagrammet för den punkt som programmet föreslår.
- Konturlinjer vid **0, ±1, ±2, ±4, ±8, ±16°**: färgade band i de mörka färgschemana, **tjocka linjer** i de ljusa.
- Bedömning av paret: **tillfredsställande** inom 1°, **godtagbar** inom 2,5°, **inte tillfredsställande** därutöver.
- Om punkten ligger under horisonten för en av stationerna visas dialogrutan **Beräkning inte möjlig** och inget annat.

## Kontroller

| Förbindelse | Scatterpunkt | Avvikelse | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (ovanför Genua) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (sydvästra Tyskland, 28 km från Bryssel) | 0,88° | 338° / 87° |
| Fyren DB0FAI (305°) - Dresden via Sheffield (DL5MCG; Dresden riktade antennen mot 290° och rapporterade aurora) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, IGRF-fält från 1990) | JN45MV, Scatter trail 40 km från Genève (Genève: 4,10°) | 0,76° | 326° / 85° |
| DL5MCG - SV1DH (test från 1987 via Budapest, 1569 km, IGRF-fält från 1987) | Budapest vid 2,53°, strax över 2,5° (2,16° med dagens fält); Scatter trail 26 km bort; mest sannolik KN06RV | 2,53° (KN06RV 0,93°) | 95° / 341° |

Från version 0.4.18 använder de två fallen med ett datum (1987 och 1990) IGRF-14-fältet från det året, så länge stationerna är fallets.

Kända scatterpunkter i listan: Genève, Bryssel, Budapest, JN92, JN41/42.

Menyn **Kontroller...** läser in testparen IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE och DL5MCG-SV1DH.

**Utrustning** (DL5MCG): F.A.I. på 144 MHz kräver minst en lång yagi och 100 W (SV1DH har även körts med en 8-elements yagi och 50 W). Rikta antennen mot den scatterpunkt som programmet anger, inte mot motstationen.
