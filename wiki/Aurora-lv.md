# AURORA

[← Home](Home-lv)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **aurorālais ovāls** (iekrāsots), pēdējo 3 dienu **Kp**, **Saules vējš** (Bz, ātrums, blīvums), atjaunināti ik pēc 5 minūtēm un saglabāti mapē `data/swpc`.
- Tie paši skati kā F.A.I.: **Scatter trail** staciju pārim un **Izoatstarošana** punkta pārbaudei, ar noklusējuma augstumu 110 km.
- **Ieteicamais antenas virziens abām stacijām**: gar katras stacijas perpendikularitātes līniju (punkti tās krāsā) – punkts ar augstāko ovāla varbūtību. Praktisks likums (G3WPO): sāc ar apmēram 45°, starp ziemeļiem un austrumiem.
- Piemērs IO70VJ - JO59BC: scatter point JP01AK pie Šetlendas salām, novirze 0,97°, QTF 11° un 299°.
- **Auroras spoti kartē**: spoti ar **<AU>** vai **<AUR>** tekstā ir auroras spoti (autora likums). Ja atzīmēta rūtiņa *<AU>/<AUR> spoti kartē ar aprēķinu*, cilne attēlo pēdējo 2 stundu spotus (ne vairāk kā 30): katram spotera-DX pārim tā aprēķina scatter trail un parāda varbūtīgāko scatter point (oranžs aplis), rezultātos norādot abu staciju QTF un elevāciju.
- Papildus varbūtīgākajam scatter point rezultātos norādīts abu staciju **QTF intervāls** visā Scatter trail garumā (novirze ≤ 1,5°).

## BENCHMARK AURORA

Cilnei AURORA ir divas apakšcilnes: **REAL-TIME** (iepriekš aprakstītais aprēķins) un **BENCHMARK AURORA**, modeļa pārbaudes stends uz **298 reāliem auroras QSO** ar laiku un lokatoru: EI5FK publicētie žurnāli (IO51RT, Īrija, 2003-2005, 144 un 432 MHz) un PA0O žurnāla karte (JO33HG, Nīderlande).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Katram QSO programma aprēķina scatter point ar to pašu modeli (IGRF-14, augstums 110 km, refrakcija 4/3, pielaide 3°).
- Krāsas: **zaļš** = scatter point 370-1100 km no abām stacijām (Lange-Hesse); **dzeltens** = izskaidrots, bet ārpus šī loga; **oranžs** = QSO virs 2000 km; **sarkans** = nav scatter point, modelis QSO neizskaidro.
- Kopsavilkums dod procentus un katram žurnālam aprēķinātā QTF intervālu un mediānu. Klikšķis uz rindas uzzīmē trasi un abus antenu virzienus.
- Rezultāts (versija 0.4.4): **izskaidroti 296 no 298 QSO (99%)**, 243 (82%) 370-1100 km logā; EI5FK QTF mediāna no 25° līdz 41°, t.i., G3WPO ieteiktie ziemeļaustrumi. Žurnālos antenu virziens nav norādīts: pārbauda ģeometriju, nevis QTF.
