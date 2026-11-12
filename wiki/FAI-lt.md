# F.A.I.

[← Home](Home-lt)

F.A.I. (Field Aligned Irregularities) – tai sklaida nuo išilgai geomagnetinio lauko išsidėsčiusių netolygumų, būdinga vasaros vakarams po sporadinio E. Išsklaidytas signalas sklinda **kūgiu**, kurio ašis nukreipta išilgai lauko: dvi stotys gali užmegzti ryšį, jei jos yra ant **papildomų kontūrų** (a1 + a2 = 180°, IC8FAX ir IC8TEM straipsnis; K9SE ±c kontūrai). Magnetinis laukas skaičiuojamas pagal modelį **IGRF-14**.

## Vaizdas Scatter trail (pora A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Įveskite **Stotis A** ir **Stotis B** (šaukinys ieškomas CALL3 ir (arba) lokatorius). Tuščias A = jūsų stotis.
- Žemėlapis rodo **Scatter trail**: visus sluoksnio aukštyje esančius taškus, matomus iš abiejų stočių, kurie tenkina atspindžio sąlygą 2,5° ribose (klasės 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Langelis viršuje dešinėje rodo A (raudona) ir B (žalia) **QTF ir elevaciją** į kiekvieną tašką.
- Programa siūlo **labiausiai tikėtiną sklaidos tašką** (mažiausias nuokrypis, trumpiausia trasa).
- **Refrakcija 4/3**: įjungta pagal nutylėjimą, išplečia horizontus.

## Vaizdas Izoatspindys (izoatspindžio linijos)

![Izoatspindys](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Tai geometrinė konstrukcija, naudojama **patikrinti, ar atspindžio taškas teisingas**.

- **Kairysis spustelėjimas** žemėlapyje: pasirenkate tikrinamą tašką. **Dešinysis spustelėjimas**: perkelia stotį B.
- **Patikrinti rekomenduojamą tašką**: atveria diagramą su programos siūlomu tašku.
- Kontūrinės linijos ties **0, ±1, ±2, ±4, ±8, ±16°**: tamsiose schemose spalvotos juostos, šviesiose schemose **storos linijos**.
- Poros vertinimas: **tenkina** iki 1°, **priimtina** iki 2,5°, **netenkina** daugiau.
- Jei taškas yra žemiau vienos stoties horizonto, pasirodo iššokantis langas **Skaičiuoti negalima** ir nieko daugiau.

## Patikros

| Ryšys | Sklaidos taškas | Nuokrypis | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (virš Genujos) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (pietvakarių Vokietija, 28 km nuo Briuselio) | 0,88° | 338° / 87° |
| Švyturys DB0FAI (305°) - Drezdenas per Šefildą (DL5MCG; Drezdenas buvo nukreipęs anteną 290° ir nurodė pašvaistę) | – | 0,3° | – |
| DL5MCG - SV1DH (1987 m. bandymas per Budapeštą, 1569 km) | Budapeštas ant pėdsako; langeliai žemiau 1° nuo 35 km | 2,16° | 95° / 341° |

Žinomi sklaidos taškai sąraše: Ženeva, Briuselis, Budapeštas, JN92, JN41/42.

Meniu **Patikros...** įkelia bandomąsias poras IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE ir DL5MCG-SV1DH.

**Įranga** (DL5MCG): F.A.I. 144 MHz juostoje reikia bent ilgos yagi ir 100 W (su SV1DH buvo dirbta ir su 8 elementų yagi bei 50 W). Nukreipkite anteną į programos nurodytą sklaidos tašką, o ne į kitą stotį.
