# METEOR SCATTER

[← Home](Home-et)

See sakk sisaldab täielikku **YA-MS 1.8.0**, IC8TEM-i meteor scatteri programmi: taevavõlv radiantidega, SKiYMET-radarite voog (Collm, Sodankylä), meteoorivoolude efektiivsuse arvutus, prognooside eksperimentaalne teostus, geomeetria ja sumbumine, GRAVES, ruutude kaart, **Meteoorivoolud otse** (Global Meteor Networki mõõdetud ZHR ja CMOR-radari radiandid), **Allikate kaart** (Collm, Sodankylä, GRAVES ja CMOR jaama suhtes) ning valemitega dokument **Efektiivsus - selgitus**.

YA-MS 1.5.0 lisab saki **Andmete kogumine** (meteor scatteri spotid, filtreeritud MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>` järgi). E-LAYER ANALYZERi sees ei ava see oma telneti seansse: see saab read saki DATA COLLECTION seanssidelt, nii et sama kutsungiga topeltsisselogimisi ei teki.

- YA-MS kasutab E-LAYER ANALYZERi ülaosas valitud keelt ja värviskeemi.
- YA-MS-i seaded salvestatakse faili `data/config.ini`.
- Iga uus YA-MS-i versioon tuuakse sisse skriptiga `tools/sync_yams.py`.

Üksikasjad leiad [YA-MS-i repositooriumist](https://github.com/ic8tem/YA-MS).
