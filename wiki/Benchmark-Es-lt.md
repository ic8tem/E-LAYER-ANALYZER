# BENCHMARK Es

[← Home](Home-lt)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Bandymų stendas **sporadiniam E** ir **keliems šuoliams** su **visu ADIF žurnalu**, net ir kitos stoties: pagal faktiškai užmegztus QSO programa nubrėžia skip ir debesis diena po dienos ir valanda po valandos.

## Žurnalo įkėlimas

- **Įkelti ADIF**: pasirenka `.adi`/`.adif` failą. Programa įsimena failą ir įkelia jį paleidžiant.
- **Pašalinti ADIF**: pašalina žurnalą iš kortelės.
- **Stotis centre**: žurnalą pildžiusios stoties šaukinys ir lokatorius. Programa siūlo dažniausius `STATION_CALLSIGN` (arba `OPERATOR`) ir `MY_GRIDSQUARE`; juos galima pakeisti ranka ir patvirtinti mygtuku **Taikyti**. Centras nepriklauso nuo bendrųjų nustatymų, todėl galima nagrinėti ir kitų stočių žurnalus.
- Siuntimo vieta: kiekvieno QSO `MY_GRIDSQUARE` (jei jo nėra – centre esančios stoties lokatorius). Priėmimo vietos: laukai `GRIDSQUARE`.
- Naudojami QSO: 28, 50, 70 ir 144 MHz su lokatoriumi. QSO su kitu `PROP_MODE` (TR, MS, FAI, AUR, EME...) ir trumpesnius nei 500 km galima neįtraukti. 432 MHz niekada neįtraukiama: sporadinis E jai netaikomas.

## Dienos ir valandos

- **QSO pagal dienas**: diena, QSO skaičius, juostos, didžiausias atstumas, keli šuoliai. Atidarius programa siūlo dieną, kurioje daugiausia QSO.
- **Dienos valandos**: eilutė *Visa diena* ir po vieną eilutę kiekvienai UTC valandai; spustelėjus rodomi tik tos valandos QSO.
- **QSO**: pasirinkimo sąrašas; spustelėjus QSO (arba žemėlapyje prie DX stoties) jis paryškinamas ir rodomos jo detalės.

## Žemėlapis

**Azimutinis vienodų atstumų** žemėlapis, kurio centras – stotis, su slankikliu **Maksimalus atstumas** masteliui keisti (taip pat pelės ratuku): apskritimai rodo **km nuo centro**.

- **Skip pagal sektorius** (10° azimuto): oranžinis lankas = artimiausias sektoriaus QSO (tylos zonos kraštas), žalias lankas = toliausias QSO, tarp jų – pasiekta sritis.
- **Es debesys** Maidenhead kvadratuose, nuspalvinti pagal įvertintą foEs.
- Vieno šuolio vidurio taškas (geltonas), stoties pirmas šuolis (oranžinis, tuščias apskritimas = trumpas pirmas šuolis), DX stoties pirmas šuolis (violetinis).
- **Išsiliejęs geltonas taškelis** pažymi kiekvieno šuolio centrą, kur spėjama esant Es debesiui (taip pat trumpo pirmo šuolio centrą, jei ši hipotezė įjungta).
- Visai dienai **raudona linija** jungia vidurio taškų svorio centrus valanda po valandos: tai debesų slinkimas.

![Viena valanda](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Siūlomi skaičiavimai

1. **Skip pagal sektorius**: kiekvieno sektoriaus skip atstumas ir nuotolis, su šaukiniais.
2. **Įvertinta foEs** pagal sekanto dėsnį (foEs ≥ f / sec φ): trumpiausias kiekvienos juostos vieno šuolio ryšys duoda didžiausią įvertį; su šia foEs – tikėtina MUF ties 1000, 1500, 2000 ir 2400 km ir atstumas, už kurio atsivertų 144 MHz.
3. Kvadratuose įvertinti **Es debesys**, su QSO skaičiumi ir foEs.
4. **Keli šuoliai**: stoties pirmas šuolis pagal dvi hipotezes (vienodi šuoliai ir trumpas pirmas šuolis iki 1200 km, IC8TEM patirtis) palyginamas su debesimis, matytais vienu šuoliu per ±30 minučių; iki 300 km hipotezė patvirtinama. Programa skaičiuoja kiekvienos hipotezės patvirtinimus.
5. **Debesų slinkimas** valanda po valandos: km, kryptis ir greitis.

Skaičiavimai yra orientaciniai: QSO rodo, kad trasa buvo atvira, bet ne tiksliai, kur buvo debesis. Kuo švaresni duomenys (6 simbolių lokatoriai, teisingas `PROP_MODE`), tuo patikimesnis skip.
