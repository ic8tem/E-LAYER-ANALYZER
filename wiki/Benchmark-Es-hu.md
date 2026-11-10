# BENCHMARK Es

[← Home](Home-hu)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Próbapad a **sporadikus E-hez** és a **többugrásos** terjedéshez egy **teljes ADIF naplón**, akár más állomásén is: a ténylegesen létrejött QSO-kból a program felvázolja a skipet és a felhőket, napról napra és óráról órára.

## A napló betöltése

- **ADIF betöltése**: kiválasztja az `.adi`/`.adif` fájlt. A program megjegyzi a fájlt, és induláskor újra betölti.
- **ADIF eltávolítása**: eltávolítja a naplót a fülről.
- **Állomás a középpontban**: a naplót készítő állomás hívójele és lokátora. A program a leggyakoribb `STATION_CALLSIGN` (vagy `OPERATOR`) és `MY_GRIDSQUARE` értéket javasolja; ezek kézzel módosíthatók és az **Alkalmaz** gombbal megerősíthetők. A középpont nem függ az általános beállításoktól, így más állomások naplói is vizsgálhatók.
- Adási hely: minden QSO `MY_GRIDSQUARE` mezője (ha hiányzik, a középpont lokátora). Vételi helyek: a `GRIDSQUARE` mezők.
- Felhasznált QSO-k: 28, 50, 70 és 144 MHz lokátorral. A más `PROP_MODE` értékű QSO-k (TR, MS, FAI, AUR, EME...) és az 500 km alattiak kizárhatók. A 432 MHz soha nem szerepel: ott a sporadikus E nem alkalmazható.

## Napok és órák

- **QSO-k naponta**: nap, QSO-k száma, sávok, legnagyobb távolság, többugrásosak. Megnyitáskor a program a legtöbb QSO-t tartalmazó napot javasolja.
- **A nap órái**: az *Egész nap* sor és minden UTC órához egy sor; kattintásra csak az adott óra QSO-i látszanak.
- **QSO**: a kijelölés listája; egy QSO-ra (vagy a térképen a DX állomás közelébe) kattintva kiemeli és megmutatja a részleteit.

## A térkép

**Azimutális távolságtartó** térkép az állomásra középpontozva, a **Legnagyobb távolság** csúszkával a nagyításhoz (egérgörgővel is): a gyűrűk a **középponttól mért km-t** mutatják.

- **Skip szektoronként** (10° azimut): narancs ív = a szektor legközelebbi QSO-ja (a holt zóna széle), zöld ív = a legtávolabbi QSO, a kettő között az elért terület.
- **Es felhők** a Maidenhead négyzeteken, a becsült foEs szerint színezve.
- Az egyszeres ugrások felezőpontja (sárga), az állomás első ugrása (narancs, üres kör = rövid első ugrás), a DX állomás első ugrása (lila).
- Az egész napra a **piros vonal** óráról órára összeköti a felezőpontok súlypontját: ez a felhők sodródása.

![One hour](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Javasolt számítások

1. **Skip szektoronként**: minden szektorra a skiptávolság és a tartomány, a hívójelekkel.
2. **Becsült foEs** a szekánstörvénnyel (foEs ≥ f / sec φ): az egyes sávok legrövidebb egyszeres ugrása adja a legmagasabb becslést; ezzel az foEs-sel a várható MUF 1000, 1500, 2000 és 2400 km-en, valamint az a távolság, amelyen túl a 144 MHz megnyílna.
3. **Es felhők** becslése a négyzeteken, a QSO-k számával és az foEs-sel.
4. **Többugrásos**: az állomás első ugrása a két hipotézis szerint (egyenlő ugrások, illetve rövid első ugrás 1200 km-en belül, IC8TEM tapasztalata), összevetve a ±30 percen belül egyszeres ugrásban látott felhőkkel; 300 km-en belül a hipotézis megerősítést nyer. A program megszámolja az egyes hipotézisek megerősítéseit.
5. **A felhők sodródása** óráról órára: km, irány és sebesség.

A számítások tájékoztató jellegűek: egy QSO azt mondja meg, hogy az útvonal nyitva volt, nem pontosan azt, hol volt a felhő. Minél tisztábbak az adatok (6 karakteres lokátorok, helyes `PROP_MODE`), annál megbízhatóbb a skip.
