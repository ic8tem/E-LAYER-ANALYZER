# METEOR SCATTER

[← Home](Home-lv)

Šajā cilnē ir pilna **YA-MS 1.8.3** – IC8TEM meteor scatter programma: debess sfēra ar radiantiem, SKiYMET radaru meteoru skaits (Collm, Sodankylä), plūsmu efektivitātes aprēķins, prognožu eksperimentāla realizācija, ģeometrija un vājinājums, GRAVES, kvadrātu karte, **Plūsmas tiešsaistē** (Global Meteor Network izmērītie ZHR un CMOR radara radianti), **Avotu karte** (Collm, Sodankylä, GRAVES un CMOR attiecībā pret staciju) un dokuments **Efektivitātes aprēķina skaidrojums** ar formulām.

YA-MS 1.5.0 pievieno cilni **Datu vākšana** (meteor scatter spoti, filtrēti pēc MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). E-LAYER ANALYZER iekšienē tā neatver savas telnet sesijas: tā saņem DATA COLLECTION sesiju rindas, tāpēc nav dubultu pieteikšanos ar to pašu izsaukuma signālu.

YA-MS 1.6.0 pārkārto cilnes grupās **Tiešsaiste un reāllaika dati** (meteor scatter satiksme tiešsaistē ar katra sakara mākoni, radaru meteoru skaits, GRAVES, datu vākšana) un **Prognozes aprēķins** (prognoze, ģeometrija, efektivitātes aprēķins, prognožu realizācija).

YA-MS 1.7.0 un 1.8.0 pievieno radiantu salīdzinājumu ar IAU MDC katalogu, Global Meteor Network ikdienas skaitījumus pa plūsmām, no populācijas indeksiem iegūto k indeksu, meteora sliedes sākotnējo rādiusu pēc Stober u.c. (2021), pingu ilgumu starp joslām (pēc noklusējuma λ² vai NN4SA HamSCI MSQP mērījumi) un salīdzinājumu ar BRAMS detekcijām Sonneberg stacijā (Perseīdas 2026). YA-MS automātiskā atjaunināšana E-LAYER ANALYZER iekšienē ir izslēgta: E-LAYER ANALYZER atjauninās pats no GitHub Releases.

YA-MS 1.8.1 antenas daivu apstrādā kā vertikālu diagrammu (rūtiņa «antenas diagramma», pēc noklusējuma ieslēgta): atstarošanas punktus sver ar abu antenu vertikālo pastiprinājumu, un efektivitātes garās trasēs vairs nepazūd. Jaunums ir brīdinājums «nav kopīgas zonas», izcelta leņķa β rūtiņa (izvēle, kas sver visvairāk), viens aprēķina datums, «Lietošanas rokasgrāmata» un salīdzinājums ar 181 767 reāliem spotiem 50 MHz joslā no HamSCI Meteor Scatter QSO Party 2025. gada augustā: stundas un trases puses ir uzticamas, procenti ir orientējoši.

- YA-MS izmanto valodu un krāsu shēmu, kas izvēlēta E-LAYER ANALYZER augšdaļā.
- YA-MS iestatījumi tiek glabāti failā `data/config.ini`.
- Katra jauna YA-MS versija tiek pārņemta ar skriptu `tools/sync_yams.py`.

Sīkāk sk. [YA-MS repozitoriju](https://github.com/ic8tem/YA-MS).
