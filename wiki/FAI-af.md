# F.A.I.

[← Home](Home-af)

F.A.I. (Field Aligned Irregularities) is verstrooiing deur onreëlmatighede wat met die geomagnetiese veld in lyn is, tipies vir someraande ná sporadiese E. Die verstrooide sein vertrek op 'n **keël** waarvan die as langs die veld lê: twee stasies kan mekaar werk as hulle op **supplementêre kontoere** lê (a1 + a2 = 180°, IC8FAX-IC8TEM-artikel; ±c-kontoere van K9SE). Die magnetiese veld word met die **IGRF-14**-model bereken.

## Aansig Scatter trail (A-B-paar)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Voer **Stasie A** en **Stasie B** in (roepnaam word in CALL3 opgesoek en/of lokator). A leeg gelaat = jou eie stasie.
- Die kaart wys die **scatter trail**: al die punte op laaghoogte, deur albei stasies gesien, wat binne 2,5° aan die refleksievoorwaarde voldoen (klasse 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Die blokkie regs bo wys die **QTF en elevasie** van A (rooi) en B (groen) na elke punt.
- Die program stel die **mees waarskynlike scatter point** voor (minimum afwyking, kortste pad).
- **Refraksie 4/3**: by verstek aan, dit vergroot die horisonne.

## Aansig Iso-refleksie (iso-refleksielokusse)

![Iso-refleksie](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Dit is die geometriese konstruksie wat gebruik word om **na te gaan of 'n refleksiepunt reg is**.

- **Linksklik** op die kaart: jy kies die punt wat nagegaan moet word. **Regsklik**: skuif stasie B.
- **Kontroleer die aanbevole punt**: open die grafiek op die punt wat die program voorstel.
- Kontoerlyne by **0, ±1, ±2, ±4, ±8, ±16°**: gekleurde bande in die donker skemas, **dik lyne** in die ligte skemas.
- Uitspraak oor die paar: **bevredigend** binne 1°, **aanvaarbaar** binne 2,5°, **nie bevredigend nie** daarbuite.
- As die punt onder die horison van een stasie is, verskyn die opspringvenster **Berekening nie moontlik nie** en niks anders nie.

## Kontroles

| Kontak | Scatter point | Afwyking | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (bo Genua) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (suidwes-Duitsland, 28 km van Brussel) | 0,88° | 338° / 87° |
| Baken DB0FAI (305°) - Dresden via Sheffield (DL5MCG; Dresden het op 290° gerig en aurora aangemeld) | – | 0,3° | – |
| DL5MCG - SV1DH (toets van 1987 via Boedapest, 1569 km) | Boedapest op die spoor; selle onder 1° vanaf 35 km | 2,16° | 95° / 341° |

Bekende scatter points in die lys: Genève, Brussel, Boedapest, JN92, JN41/42.

Die kieslys **Kontroles...** laai die toetspare IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE en DL5MCG-SV1DH.

**Toerusting** (DL5MCG): F.A.I. op 144 MHz vereis minstens 'n lang Yagi en 100 W (SV1DH is ook met 'n 8-element-Yagi en 50 W gewerk). Rig die antenna op die scatter point wat die program gee, nie op die ander stasie nie.
