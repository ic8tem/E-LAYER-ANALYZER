# METEOR SCATTER

[← Kezdőlap](Home-hu)

Ez a fül a teljes **YA-MS 1.8.3**-t tartalmazza, IC8TEM meteor scatter programját: égbolt a radiánsokkal, SKiYMET radarfluxus (Collm, Sodankylä), a rajhatékonyság számítása, az előrejelzések kísérleti megvalósítása, geometria és csillapítás, GRAVES, négyzettérkép, **Élő rajok** (a Global Meteor Network által mért ZHR és a CMOR radar radiánsai), **Források térképe** (Collm, Sodankylä, GRAVES és CMOR az állomáshoz képest), valamint a képleteket tartalmazó **A hatásfok számításának magyarázata** dokumentum.

A YA-MS 1.5.0 hozzáadja az **Adatgyűjtés** fület (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>` szerint szűrt meteor scatter spotok). Az E-LAYER ANALYZER-en belül nem nyit saját telnet-kapcsolatokat: a DATA COLLECTION kapcsolatainak sorait kapja, így nincs kettős bejelentkezés ugyanazzal a hívójellel.

A YA-MS 1.6.0 a füleket **Élő és valós idejű adatok** (élő meteor scatter forgalom az egyes összeköttetések felhőjével, radarfluxus, GRAVES, adatgyűjtés) és **Előrejelzés számítása** (előrejelzés, geometria, hatékonyság számítása, az előrejelzések megvalósítása) csoportokba rendezi át.

A YA-MS 1.7.0 és 1.8.0 hozzáadja a radiánsok összevetését az IAU MDC katalógussal, a Global Meteor Network rajonkénti napi számlálásait, a populációs indexekből levezetett k indexet, a nyom kezdeti sugarát Stober és mtsai (2021) szerint, a pingek időtartamát a sávok között (alapértelmezésben λ² vagy NN4SA HamSCI MSQP mérései) és az összevetést a Sonneberg állomás BRAMS észleléseivel (Perseidák 2026). A YA-MS automatikus frissítése az E-LAYER ANALYZER-en belül ki van kapcsolva; a program magától frissül a GitHub Releases oldaláról.

A YA-MS 1.8.1 az antenna nyalábját függőleges iránykarakterisztikaként kezeli («antenna iránykarakterisztika» jelölőnégyzet, alapértelmezésben bekapcsolva): a visszaverődési pontokat a két antenna függőleges nyeresége súlyozza, és a hatásfokok hosszú útvonalakon már nem tűnnek el. Új a «nincs közös zóna» figyelmeztetés, a kiemelt β-szög jelölőnégyzet (a legnagyobb súlyú választás), az egyetlen számítási dátum, a «Felhasználói kézikönyv» és az összevetés a HamSCI Meteor Scatter QSO Party 2025. augusztusi 181 767 valódi 50 MHz-es spotjával: az órák és az útvonal oldalai megbízhatók, a százalékok tájékoztató jellegűek.

- A YA-MS az E-LAYER ANALYZER tetején választott nyelvet és színsémát használja.
- A YA-MS beállításai a `data/config.ini` fájlban tárolódnak.
- A YA-MS minden új verziója a `tools/sync_yams.py` szkripttel kerül be.

Részletek a [YA-MS tárolóban](https://github.com/ic8tem/YA-MS).
