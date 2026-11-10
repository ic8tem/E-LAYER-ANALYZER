# BENCHMARK Es

[← Home](Home-lv)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Pārbaudes stends **sporādiskajam E** un **vairāklēcienu** sakariem **veselā ADIF žurnālā**, arī citas stacijas: no reāli veiktajiem QSO programma ieskicē skip zonu un mākoņus, dienu pa dienai un stundu pa stundai.

## Žurnāla ielāde

- **Ielādēt ADIF**: izvēlas `.adi`/`.adif` failu. Programma atceras failu un ielādē to no jauna startējot.
- **Izņemt ADIF**: noņem žurnālu no cilnes.
- **Stacija centrā**: tās stacijas izsaukuma signāls un lokators, kas veidojusi žurnālu. Programma piedāvā biežāko `STATION_CALLSIGN` (vai `OPERATOR`) un `MY_GRIDSQUARE`; tos var mainīt ar roku un apstiprināt ar **Lietot**. Centrs nav atkarīgs no vispārējiem iestatījumiem, tāpēc var pētīt arī citu staciju žurnālus.
- Raidīšanas vieta: katra QSO `MY_GRIDSQUARE` (ja trūkst – lokators centrā). Uztveršanas vietas: lauki `GRIDSQUARE`.
- Izmantotie QSO: 28, 50, 70 un 144 MHz ar lokatoru. QSO ar citu `PROP_MODE` (TR, MS, FAI, AUR, EME...) un tos, kas īsāki par 500 km, var izslēgt. 432 MHz nekad netiek iekļauti: sporādiskais E uz tiem neattiecas.

## Dienas un stundas

- **QSO pa dienām**: diena, QSO skaits, joslas, maksimālais attālums, vairāklēcienu sakari. Atverot programma piedāvā dienu ar visvairāk QSO.
- **Dienas stundas**: rinda *Visa diena* un pa vienai rindai katrai UTC stundai; klikšķis rāda tikai tās stundas QSO.
- **QSO**: atlases saraksts; klikšķis uz QSO (vai kartē pie korespondenta) to izceļ un parāda tā detaļas.

## Karte

**Azimutāla ekvidistanta** karte ar centru stacijā, ar slīdni **Maksimālais attālums** tālummaiņai (arī ar peles ritenīti): gredzeni rāda **km no centra**.

- **Skip pa sektoriem** (10° azimuta): oranžs loks = sektora tuvākais QSO (skip zonas mala), zaļš loks = tālākais QSO, starp tiem – sasniegtā zona.
- **Es mākoņi** Maidenhead kvadrātos, iekrāsoti pēc novērtētās foEs.
- Viena lēciena viduspunkts (dzeltens), stacijas pirmais lēciens (oranžs, tukšs aplis = īss pirmais lēciens), korespondenta pirmais lēciens (violets).
- **Izplūdis dzeltens punkts** atzīmē katra lēciena centru, kur tiek pieņemts Es mākonis (arī īsā pirmā lēciena centru, ja šī hipotēze ir ieslēgta).
- Visai dienai **sarkanā līnija** stundu pa stundai savieno viduspunktu smaguma centrus: tā ir mākoņu pārvietošanās.

![Viena stunda](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Ieteiktie aprēķini

1. **Skip pa sektoriem**: katram sektoram skip attālums un sniedzamība, ar izsaukuma signāliem.
2. **Novērtētā foEs** pēc sekanses likuma (foEs ≥ f / sec φ): katras joslas īsākais viena lēciena sakars dod augstāko novērtējumu; ar šo foEs – gaidāmā MUF 1000, 1500, 2000 un 2400 km attālumā un attālums, virs kura atvērtos 144 MHz.
3. **Es mākoņi**, novērtēti kvadrātos, ar QSO skaitu un foEs.
4. **Vairāklēcienu sakari**: stacijas pirmais lēciens pēc abām hipotēzēm (vienādi lēcieni un īss pirmais lēciens līdz 1200 km, IC8TEM pieredze), salīdzināts ar mākoņiem, kas redzēti viena lēciena sakaros ±30 minūšu robežās; 300 km robežās hipotēze tiek apstiprināta. Programma saskaita katras hipotēzes apstiprinājumus.
5. **Mākoņu pārvietošanās** pa stundām: km, virziens un ātrums.

Aprēķini ir orientējoši: QSO apliecina, ka trase bija atvērta, bet ne precīzi to, kur bija mākonis. Jo tīrāki dati (6 rakstzīmju lokatori, pareizs `PROP_MODE`), jo uzticamāka skip zona.
