# METEOR SCATTER

[← Kezdőlap](Home-hu)

Ez a fül a teljes **YA-MS 1.8.0**-t tartalmazza, IC8TEM meteor scatter programját: égbolt a radiánsokkal, SKiYMET radarfluxus (Collm, Sodankylä), a rajhatékonyság számítása, az előrejelzések kísérleti megvalósítása, geometria és csillapítás, GRAVES, négyzettérkép, **Élő rajok** (a Global Meteor Network által mért ZHR és a CMOR radar radiánsai), **Források térképe** (Collm, Sodankylä, GRAVES és CMOR az állomáshoz képest), valamint a képleteket tartalmazó **Hatékonyság - magyarázat** dokumentum.

A YA-MS 1.5.0 hozzáadja az **Adatgyűjtés** fület (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>` szerint szűrt meteor scatter spotok). Az E-LAYER ANALYZER-en belül nem nyit saját telnet-kapcsolatokat: a DATA COLLECTION kapcsolatainak sorait kapja, így nincs kettős bejelentkezés ugyanazzal a hívójellel.

A YA-MS 1.6.0 a füleket **Élő és valós idejű adatok** (élő meteor scatter forgalom az egyes összeköttetések felhőjével, radarfluxus, GRAVES, adatgyűjtés) és **Előrejelzés számítása** (előrejelzés, geometria, hatékonyság számítása, az előrejelzések megvalósítása) csoportokba rendezi át.

- A YA-MS az E-LAYER ANALYZER tetején választott nyelvet és színsémát használja.
- A YA-MS beállításai a `data/config.ini` fájlban tárolódnak.
- A YA-MS minden új verziója a `tools/sync_yams.py` szkripttel kerül be.

Részletek a [YA-MS tárolóban](https://github.com/ic8tem/YA-MS).
