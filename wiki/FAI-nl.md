# F.A.I.

[← Home](Home-nl)

F.A.I. (Field Aligned Irregularities) is verstrooiing aan onregelmatigheden die langs het aardmagnetisch veld zijn uitgelijnd, typisch voor zomeravonden na sporadische E. Het verstrooide signaal vertrekt op een **kegel** waarvan de as langs het veld ligt: twee stations kunnen elkaar werken als ze op **supplementaire contouren** liggen (a1 + a2 = 180°, artikel IC8FAX-IC8TEM; ±c-contouren van K9SE). Het magnetisch veld wordt berekend met het model **IGRF-14**.

## Weergave Scatter trail (paar A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Vul **Station A** en **Station B** in (roepnaam, opgezocht in CALL3, en/of locator). A leeg = je eigen station.
- De kaart toont de **scatter trail**: alle punten op laaghoogte die door beide stations worden gezien en binnen 2,5° aan de reflectievoorwaarde voldoen (klassen 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Het vak rechtsboven toont **QTF en elevatie** van A (rood) en B (groen) naar elk punt.
- Het programma stelt het **meest waarschijnlijke scatterpunt** voor (minimale afwijking, kortste traject).
- **Refractie 4/3**: standaard aan, vergroot de horizonten.

## Weergave Iso-reflectie (iso-reflectielijnen)

![Iso-reflectie](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Dit is de geometrische constructie waarmee je **controleert of een reflectiepunt klopt**.

- **Linksklik** op de kaart: je kiest het punt dat je wilt controleren. **Rechtsklik**: verplaatst station B.
- **Aanbevolen punt controleren**: opent de grafiek op het punt dat het programma voorstelt.
- Contourlijnen bij **0, ±1, ±2, ±4, ±8, ±16°**: gekleurde banden in de donkere schema's, **dikke lijnen** in de lichte schema's.
- Oordeel over het paar: **voldoende** binnen 1°, **aanvaardbaar** binnen 2,5°, **onvoldoende** daarboven.
- Ligt het punt onder de horizon van een van de stations, dan verschijnt alleen de melding **Berekening niet mogelijk**.

## Controles

| Verbinding | Scatterpunt | Afwijking | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (boven Genua) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (Zuidwest-Duitsland, 28 km van Brussel) | 0,88° | 338° / 87° |
| DB0FAI - Dresden via Sheffield (DL5MCG) | – | 0,3° | – |

Bekende scatterpunten in de lijst: Genève, Brussel, Boedapest, JN92, JN41/42.
