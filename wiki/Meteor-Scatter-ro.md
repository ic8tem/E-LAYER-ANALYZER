# METEOR SCATTER

[← Home](Home-ro)

Această filă conține **YA-MS 1.8.4** complet, programul de meteor scatter al lui IC8TEM: bolta cerească cu radianții, fluxul radarelor SKiYMET (Collm, Sodankylä), calculul eficienței roiurilor, implementarea experimentală a prognozelor, geometrie și atenuare, GRAVES, harta pătratelor, **Roiuri în direct** (ZHR măsurate de Global Meteor Network și radianții radarului CMOR), **Harta surselor** (Collm, Sodankylä, GRAVES și CMOR față de stație) și documentul **Explicația calculului eficienței** cu formulele.

YA-MS 1.5.0 adaugă fila **Colectare date** (spoturi meteor scatter filtrate după MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). În E-LAYER ANALYZER nu deschide sesiuni telnet proprii: primește rândurile sesiunilor din DATA COLLECTION, astfel încât nu există login-uri duble cu același indicativ.

YA-MS 1.6.0 reorganizează filele în **Live și date în timp real** (trafic meteor scatter în direct cu norul fiecărei legături, flux radar, GRAVES, colectare date) și **Calcul previzional** (prognoză, geometrie, calculul eficienței, implementarea prognozelor).

YA-MS 1.7.0 și 1.8.0 adaugă comparația radianților cu catalogul IAU MDC, numărătorile zilnice ale Global Meteor Network pe fiecare roi, indicele k derivat din indicii de populație, raza inițială a urmei meteorice după Stober și alții (2021), durata ping-urilor între benzi (λ² implicit sau măsurătorile HamSCI MSQP ale lui NN4SA) și comparația cu detecțiile BRAMS de la Sonneberg (Perseide 2026). Actualizarea automată a YA-MS este dezactivată în E-LAYER ANALYZER, care se actualizează singur din Release-urile de pe GitHub.

YA-MS 1.8.1 tratează lobul antenei ca diagramă verticală (căsuța «diagrama antenei», activată implicit): punctele de reflexie sunt ponderate cu câștigul vertical al celor două antene, iar eficiențele nu mai dispar pe traseele lungi. Adaugă avertismentul «nicio zonă comună», evidențiază căsuța unghiului β (alegerea care cântărește cel mai mult), folosește o singură dată de calcul, aduce «Manual de utilizare» și comparația cu 181 767 de spoturi reale pe 50 MHz din Meteor Scatter QSO Party HamSCI din august 2025: orele și laturile traseului sunt de încredere, procentele sunt orientative.

- YA-MS folosește limba și schema de culori alese în partea de sus a E-LAYER ANALYZER.
- Setările YA-MS sunt salvate în `data/config.ini`.
- Fiecare versiune nouă a YA-MS este adusă cu scriptul `tools/sync_yams.py`.

Pentru detalii vezi [depozitul YA-MS](https://github.com/ic8tem/YA-MS).
