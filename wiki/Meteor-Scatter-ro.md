# METEOR SCATTER

[← Home](Home-ro)

Această filă conține **YA-MS 1.8.0** complet, programul de meteor scatter al lui IC8TEM: bolta cerească cu radianții, fluxul radarelor SKiYMET (Collm, Sodankylä), calculul eficienței roiurilor, implementarea experimentală a prognozelor, geometrie și atenuare, GRAVES, harta pătratelor, **Roiuri în direct** (ZHR măsurate de Global Meteor Network și radianții radarului CMOR), **Harta surselor** (Collm, Sodankylä, GRAVES și CMOR față de stație) și documentul **Eficiență - explicație** cu formulele.

YA-MS 1.5.0 adaugă fila **Colectare date** (spoturi meteor scatter filtrate după MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). În E-LAYER ANALYZER nu deschide sesiuni telnet proprii: primește rândurile sesiunilor din DATA COLLECTION, astfel încât nu există login-uri duble cu același indicativ.

YA-MS 1.6.0 reorganizează filele în **Live și date în timp real** (trafic meteor scatter în direct cu norul fiecărei legături, flux radar, GRAVES, colectare date) și **Calcul previzional** (prognoză, geometrie, calculul eficienței, implementarea prognozelor).

- YA-MS folosește limba și schema de culori alese în partea de sus a E-LAYER ANALYZER.
- Setările YA-MS sunt salvate în `data/config.ini`.
- Fiecare versiune nouă a YA-MS este adusă cu scriptul `tools/sync_yams.py`.

Pentru detalii vezi [depozitul YA-MS](https://github.com/ic8tem/YA-MS).
