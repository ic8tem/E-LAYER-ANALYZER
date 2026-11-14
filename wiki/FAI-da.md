# F.A.I.

[← Home](Home-da)

F.A.I. (Field Aligned Irregularities) er spredning fra uregelmæssigheder orienteret langs det geomagnetiske felt, typisk for sommeraftener efter sporadisk E. Det spredte signal udgår på en **kegle**, hvis akse ligger langs feltet: to stationer kan forbinde hinanden, hvis de ligger på **supplerende konturer** (a1 + a2 = 180°, artiklen af IC8FAX-IC8TEM; K9SE's ±c-konturer). Magnetfeltet beregnes med modellen **IGRF-14**.

## Visningen Scatter trail (A-B-par)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Indtast **Station A** og **Station B** (kaldesignal slået op i CALL3 og/eller lokator). Tom A = din egen station.
- Kortet viser **scatter trail**: alle punkter i lagets højde, som begge stationer ser, og som opfylder refleksionsbetingelsen inden for 2,5° (klasser 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Boksen øverst til højre viser **QTF og elevation** for A (rød) og B (grøn) mod hvert punkt.
- Programmet foreslår det **mest sandsynlige scatterpunkt** (mindste afvigelse, korteste strækning).
- **Refraktion 4/3**: slået til som standard; den udvider horisonterne.

## Visningen Iso-refleksion (iso-refleksionskurver)

![Iso-refleksion](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Det er den geometriske konstruktion, der bruges til at **kontrollere, om et refleksionspunkt er rigtigt**.

- **Venstreklik** på kortet: du vælger det punkt, der skal kontrolleres. **Højreklik**: flytter station B.
- **Kontrollér det anbefalede punkt**: åbner diagrammet på det punkt, programmet foreslår.
- Konturlinjer ved **0, ±1, ±2, ±4, ±8, ±16°**: farvede bånd i de mørke skemaer, **tykke linjer** i de lyse skemaer.
- Vurdering af parret: **tilfredsstillende** inden for 1°, **acceptabel** inden for 2,5°, **ikke tilfredsstillende** derudover.
- Hvis punktet er under horisonten for en af stationerne, vises popup'en **Beregning ikke mulig** og intet andet.

## Kontroller

| Forbindelse | Scatterpunkt | Afvigelse | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (over Genova) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (det sydvestlige Tyskland, 28 km fra Bruxelles) | 0,88° | 338° / 87° |
| Fyret DB0FAI (305°) - Dresden via Sheffield (DL5MCG; Dresden havde antennen mod 290° og meldte nordlys) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, IGRF-felt fra 1990) | JN45MV, Scatter trail 40 km fra Genève (Genève: 4,10°) | 0,76° | 326° / 85° |
| DL5MCG - SV1DH (test fra 1987 via Budapest, 1569 km, IGRF-felt fra 1987) | Budapest ved 2,53°, lige over 2,5° (2,16° med dagens felt); Scatter trail 26 km væk; mest sandsynlige KN06RV | 2,53° (KN06RV 0,93°) | 95° / 341° |

Fra version 0.4.18 bruger de to tilfælde med en dato (1987 og 1990) IGRF-14-feltet fra det år, så længe stationerne er tilfældets.

Kendte scatterpunkter på listen: Genève, Bruxelles, Budapest, JN92, JN41/42.

Menuen **Kontroller...** indlæser testparrene IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE og DL5MCG-SV1DH.

**Udstyr** (DL5MCG): F.A.I. på 144 MHz kræver mindst en lang yagi og 100 W (SV1DH er også blevet kørt med en 8-elementers yagi og 50 W). Ret antennen mod det scatterpunkt, programmet angiver, ikke mod modstationen.
