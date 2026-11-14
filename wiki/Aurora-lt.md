# AURORA

[← Home](Home-lt)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **pašvaistės ovalas** (užtamsintas), paskutinių 3 dienų **Kp**, **Saulės vėjas** (Bz, greitis, tankis), atnaujinami kas 5 minutes ir saugomi aplanke `data/swpc`.
- Tie patys vaizdai kaip F.A.I.: **Scatter trail** stočių porai ir **Izoatspindys** taškui patikrinti, numatytasis aukštis 110 km.
- **Rekomenduojamas antenos nukreipimas abiem stotims**: ant kiekvienos stoties statmenumo linijos (taškeliai jos spalva) – taškas, kuriame ovalo tikimybė didžiausia. Praktinė taisyklė (G3WPO): pradėkite nuo maždaug 45°, tarp šiaurės ir rytų.
- Pavyzdys IO70VJ - JO59BC: sklaidos taškas JP01AK prie Šetlando salų, nuokrypis 0,97°, QTF 11° ir 299°.
- **Pašvaistės spotai žemėlapyje**: spotai, kurių tekste yra **<AU>** arba **<AUR>**, yra pašvaistės spotai (autoriaus taisyklė). Pažymėjus langelį *<AU>/<AUR> spotai žemėlapyje su skaičiavimu*, kortelė rodo paskutinių 2 valandų spotus (ne daugiau kaip 30): kiekvienai spoterio ir DX porai apskaičiuojamas Scatter trail ir rodomas labiausiai tikėtinas sklaidos taškas (oranžinis apskritimas), o rezultatuose – abiejų stočių QTF ir elevacija.
- Be labiausiai tikėtino sklaidos taško, rezultatuose nurodomas abiejų stočių **QTF intervalas** per visą Scatter trail (nuokrypis ≤ 1,5°).
- **Lange-Hesse langas**: pirmenybė teikiama sklaidos taškui 370-1100 km atstumu nuo kiekvienos stoties – tai langas, kurį Lange-Hesse apskaičiavo ties 54° geomagnetine platuma. Nuo 0.4.5 versijos langas slenkasi pagal kiekvienos stoties IGRF-14 inklinaciją: arčiau piečiau esančioms stotims, toliau šiauriau esančioms (pvz., Kapris apie 260-990 km, Oslas 470-1200 km). Rezultatuose nurodomas abiejų stočių langas.

## BENCHMARK AURORA

Kortelė AURORA turi dvi vidines korteles: **REAL-TIME** (aukščiau aprašytas skaičiavimas) ir **BENCHMARK AURORA**, modelio bandymų stendą su **298 realiais pašvaistės QSO** su laiku ir lokatoriumi: paskelbti EI5FK žurnalai (IO51RT, Airija, 2003-2005, 144 ir 432 MHz) ir PA0O žurnalo žemėlapis (JO33HG, Nyderlandai).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Kiekvienam QSO programa apskaičiuoja sklaidos tašką tuo pačiu modeliu (IGRF-14, aukštis 110 km, refrakcija 4/3, tolerancija 3°).
- Spalvos: **žalia** = sklaidos taškas abiejų stočių Lange-Hesse lange; **geltona** = paaiškintas, bet už šio lango ribų; **oranžinė** = QSO ilgesnis nei 2000 km; **raudona** = sklaidos taško nėra, modelis QSO nepaaiškina.
- Suvestinė pateikia procentus, taip pat su fiksuotu 370-1100 km langu palyginimui, ir kiekvienam žurnalui apskaičiuoto QTF intervalą bei medianą. Spustelėjus eilutę nubraižoma trasa ir abu antenų nukreipimai.
- Rezultatas (versija 0.4.18, kiekvieno QSO metų IGRF-14 laukas): **paaiškinti 297 iš 298 QSO (100%)**, 267 (90%) abiejų stočių Lange-Hesse lange (275, t. y. 92%, su fiksuotu 370-1100 km langu); apskaičiuoto EI5FK QTF mediana nuo 32° iki 53°, t. y. G3WPO rekomenduojami šiaurės rytai. Žurnaluose antenų nukreipimas nenurodytas: tikrinama geometrija, o ne QTF.
- **Pataisa**: nuo 0.4.4 iki 0.4.17 versijos bandymų stendas pasileisdavo prieš įkeliant IGRF-14 ir skaičiavo su dipoliu ir fiksuotu langu; tada paskelbti rezultatai (243 QSO, 82%, QTF mediana 25-41°) buvo dipolio. Pataisyta 0.4.18 versijoje.
