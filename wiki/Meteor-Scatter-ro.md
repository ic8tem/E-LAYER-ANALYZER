# METEOR SCATTER

[← Home](Home-ro)

Această filă conține programul complet **YA-MS 1.6.0**, programul de meteor scatter al lui IC8TEM: sfera cerească cu radianții, fluxul radarelor SKiYMET (Collm, Sodankylä), calculul eficienței roiurilor, implementarea experimentală a prognozelor, geometrie și atenuare, GRAVES, harta pătratelor, **Roiuri în direct** (ZHR măsurat de Global Meteor Network și radianți de la radarul CMOR), **Harta surselor** (Collm, Sodankylä, GRAVES și CMOR față de stație) și documentul **Eficiență - explicație** cu formulele.

YA-MS 1.5.0 adaugă fila **Colectare date** (spoturi de meteor scatter filtrate după MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). În interiorul E-LAYER ANALYZER nu deschide sesiuni telnet proprii: primește liniile sesiunilor din DATA COLLECTION, deci nu există autentificări duble cu același indicativ.

YA-MS 1.6.0 reorganizează filele în **Live și date în timp real** (trafic de meteor scatter în direct cu norul fiecărei legături, flux radar, GRAVES, colectare date) și **Calcul de prognoză** (prognoză, geometrie, calculul eficienței, implementarea prognozelor).

- YA-MS folosește limba și schema de culori alese în partea de sus a E-LAYER ANALYZER.
- Setările YA-MS sunt păstrate în `data/config.ini`.
- Fiecare versiune nouă a YA-MS este adusă cu scriptul `tools/sync_yams.py`.

Pentru detalii vedeți [depozitul YA-MS](https://github.com/ic8tem/YA-MS).
