# METEOR SCATTER

[← Home](Home-hu)

Ez a fül tartalmazza a teljes **YA-MS 1.6.0** programot, IC8TEM meteorszórásos programját: égi gömb a radiánsokkal, SKiYMET radarfluxus (Collm, Sodankylä), a rajok hatásfokának számítása, az előrejelzések kísérleti megvalósítása, geometria és csillapítás, GRAVES, négyzettérkép, **Élő rajok** (a Global Meteor Network által mért ZHR és a CMOR radar radiánsai), **Források térképe** (Collm, Sodankylä, GRAVES és CMOR az állomáshoz viszonyítva) és a **Hatásfok - magyarázat** dokumentum a képletekkel.

A YA-MS 1.5.0 hozzáadja az **Adatgyűjtés** fület (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>` szerint szűrt meteorszórásos spotok). Az E-LAYER ANALYZER-en belül nem nyit saját telnet-munkameneteket: a DATA COLLECTION munkameneteinek sorait kapja, így nincs kettős bejelentkezés ugyanazzal a hívójellel.

A YA-MS 1.6.0 a füleket két csoportba rendezi: **Élő és valós idejű adatok** (élő meteorszórásos forgalom az egyes összeköttetések felhőjével, radarfluxus, GRAVES, adatgyűjtés) és **Előrejelző számítás** (előrejelzés, geometria, hatásfok számítása, az előrejelzések megvalósítása).

- A YA-MS az E-LAYER ANALYZER tetején kiválasztott nyelvet és színsémát használja.
- A YA-MS beállításai a `data/config.ini` fájlban tárolódnak.
- A YA-MS minden új verzióját a `tools/sync_yams.py` szkript hozza be.

Részletek: [a YA-MS tárolója](https://github.com/ic8tem/YA-MS).
