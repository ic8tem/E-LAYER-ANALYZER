# METEOR SCATTER

[← Home](Home-et)

See sakk sisaldab täielikku **YA-MS 1.8.13**, IC8TEM-i meteor scatteri programmi: taevavõlv radiantidega, SKiYMET-radarite voog (Collm, Sodankylä), meteoorivoolude efektiivsuse arvutus, prognooside eksperimentaalne teostus, geomeetria ja sumbumine, GRAVES, ruutude kaart, **Meteoorivoolud otse** (Global Meteor Networki mõõdetud ZHR ja CMOR-radari radiandid), **Allikate kaart** (Collm, Sodankylä, GRAVES ja CMOR jaama suhtes) ning valemitega dokument **Tõhususe arvutuse selgitus**.

YA-MS 1.5.0 lisab saki **Andmete kogumine** (meteor scatteri spotid, filtreeritud MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>` järgi). E-LAYER ANALYZERi sees ei ava see oma telneti seansse: see saab read saki DATA COLLECTION seanssidelt, nii et sama kutsungiga topeltsisselogimisi ei teki.

YA-MS 1.6.0 koondab sakid rühmadesse **Otse ja reaalajas andmed** (meteor scatteri liiklus otse iga side pilvega, radarite voog, GRAVES, andmete kogumine) ja **Prognoosarvutus** (prognoos, geomeetria, efektiivsuse arvutus, prognooside teostus).

YA-MS 1.7.0 ja 1.8.0 lisavad radiantide võrdluse IAU MDC kataloogiga, Global Meteor Networki päevased loendused meteoorivoolude kaupa, populatsiooniindeksitest tuletatud k-indeksi, Stober jt (2021) jälje algraadiuse, pingide kestuse sagedusalade vahel (vaikimisi λ² või NN4SA HamSCI MSQP mõõtmised) ning võrdluse Sonnebergi BRAMS-i tuvastustega (Perseiidid 2026). YA-MS-i automaatne uuendamine on E-LAYER ANALYZERi sees välja lülitatud; E-LAYER ANALYZER uuendab end ise GitHubi Release'idest.

YA-MS 1.8.1 käsitleb antenni keila vertikaalse suunadiagrammina (kast «antenni suunadiagramm», vaikimisi sees): peegelduspunkte kaalutakse mõlema antenni vertikaalse võimendusega ja tõhusused ei kao enam pikkadel teekondadel. Uued on hoiatus «ühist tsooni pole», esile tõstetud nurga β kast (kõige kaalukam valik), üksainus arvutuse kuupäev, «Kasutusjuhend» ja võrdlus 181 767 tegeliku 50 MHz spotiga HamSCI Meteor Scatter QSO Partylt 2025. aasta augustist: tunnid ja teekonna küljed on usaldusväärsed, protsendid on suunavad.

- YA-MS kasutab E-LAYER ANALYZERi ülaosas valitud keelt ja värviskeemi.
- YA-MS-i seaded salvestatakse faili `data/config.ini`.
- Iga uus YA-MS-i versioon tuuakse sisse skriptiga `tools/sync_yams.py`.

Üksikasjad leiad [YA-MS-i repositooriumist](https://github.com/ic8tem/YA-MS).
