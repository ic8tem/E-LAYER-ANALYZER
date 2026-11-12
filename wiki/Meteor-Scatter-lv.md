# METEOR SCATTER

[← Home](Home-lv)

Šajā cilnē ir pilna **YA-MS 1.8.0** – IC8TEM meteor scatter programma: debess sfēra ar radiantiem, SKiYMET radaru meteoru skaits (Collm, Sodankylä), plūsmu efektivitātes aprēķins, prognožu eksperimentāla realizācija, ģeometrija un vājinājums, GRAVES, kvadrātu karte, **Plūsmas tiešsaistē** (Global Meteor Network izmērītie ZHR un CMOR radara radianti), **Avotu karte** (Collm, Sodankylä, GRAVES un CMOR attiecībā pret staciju) un dokuments **Efektivitāte - skaidrojums** ar formulām.

YA-MS 1.5.0 pievieno cilni **Datu vākšana** (meteor scatter spoti, filtrēti pēc MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). E-LAYER ANALYZER iekšienē tā neatver savas telnet sesijas: tā saņem DATA COLLECTION sesiju rindas, tāpēc nav dubultu pieteikšanos ar to pašu izsaukuma signālu.

- YA-MS izmanto valodu un krāsu shēmu, kas izvēlēta E-LAYER ANALYZER augšdaļā.
- YA-MS iestatījumi tiek glabāti failā `data/config.ini`.
- Katra jauna YA-MS versija tiek pārņemta ar skriptu `tools/sync_yams.py`.

Sīkāk sk. [YA-MS repozitoriju](https://github.com/ic8tem/YA-MS).
