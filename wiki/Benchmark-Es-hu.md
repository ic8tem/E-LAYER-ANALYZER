# BENCHMARK Es

[← Kezdőlap](Home-hu)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Tesztpad a **sporadikus E** és a **többugrásos terjedés** vizsgálatához egy **teljes ADIF-naplón**, akár más állomásén is: a ténylegesen létrejött QSO-kból a program napról napra és óráról órára felvázolja a holtzónát és a felhőket.

## A napló betöltése

- **ADIF betöltése**: kiválasztja az `.adi`/`.adif` fájlt. A program megjegyzi a fájlt, és induláskor újratölti.
- **ADIF eltávolítása**: eltávolítja a naplót a fülről.
- **Állomás a középpontban**: a naplót készítő állomás hívójele és lokátora. A program a leggyakoribb `STATION_CALLSIGN` (vagy `OPERATOR`) és `MY_GRIDSQUARE` értéket javasolja; ezek kézzel módosíthatók, és az **Alkalmaz** gombbal megerősíthetők. A középpont nem függ az általános beállításoktól, így más állomások naplói is vizsgálhatók.
- Az adó helye: az egyes QSO-k `MY_GRIDSQUARE` mezője (ha hiányzik, a középpont lokátora). A vevők helye: a `GRIDSQUARE` mezők.
- Felhasznált QSO-k: 28, 50, 70 és 144 MHz, lokátorral. Kizárhatók a más `PROP_MODE`-ú (TR, MS, FAI, AUR, EME...) és az 500 km alatti QSO-k. A 432 MHz soha nem szerepel: a sporadikus E nem vonatkozik rá.

## Napok és órák

- **QSO-k naponta**: nap, QSO-k száma, sávok, legnagyobb távolság, többugrásos összeköttetések. Megnyitáskor a program a legtöbb QSO-t tartalmazó napot javasolja.
- **A nap órái**: az *Egész nap* sor és minden UTC-órára egy sor; egy kattintás csak az adott óra QSO-it mutatja.
- **QSO**: a kijelölés listája; egy QSO-ra (vagy a térképen a DX állomás közelébe) kattintva kiemeli, és megmutatja a részleteit.

## A térkép

Az állomásra középpontozott **azimutális távolságtartó** térkép, a nagyításhoz használható **Maximális távolság** csúszkával (az egérgörgővel is): a gyűrűk a **középponttól mért km-t** mutatják.

- **Holtzóna szektoronként** (10° azimut): narancs ív = a szektor legközelebbi QSO-ja (a holtzóna széle), zöld ív = a legtávolabbi QSO, a kettő között az elért terület.
- **Es-felhők** a Maidenhead-négyzeteken, a becsült foEs szerint színezve.
- Az egyugrásos összeköttetések felezőpontja (sárga), az állomás első ugrása (narancs, üres kör = rövid első ugrás), a DX állomás első ugrása (lila).
- Egy **elmosódott sárga pötty** jelöli minden ugrás közepét, ahol az Es-felhőt feltételezzük (a rövid első ugrás közepét is, ha ez a hipotézis be van kapcsolva).
- Az egész napra a **piros vonal** óráról órára összeköti a felezőpontok súlypontjait: ez a felhők vándorlása.

![Egy óra](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Javasolt számítások

1. **Holtzóna szektoronként**: minden szektorra a holtzóna távolsága és a hatótávolság, a hívójelekkel.
2. **Becsült foEs** a szekánstörvénnyel (foEs ≥ f / sec φ): minden sávon a legrövidebb egyugrásos összeköttetés adja a legmagasabb becslést; ezzel a foEs-sel a várható MUF 1000, 1500, 2000 és 2400 km-en, valamint az a távolság, amelyen túl a 144 MHz nyitna.
3. A négyzeteken becsült **Es-felhők**, a QSO-k számával és a foEs-sel.
4. **Többugrásos terjedés**: az állomás első ugrása a két hipotézis szerint (egyenlő ugrások, illetve 1200 km-en belüli rövid első ugrás, IC8TEM tapasztalata), összevetve a ±30 percen belül egyugrásosan látott felhőkkel; 300 km-en belül a hipotézis megerősítést nyer. A program megszámolja az egyes hipotézisek megerősítéseit.
5. A **felhők vándorlása** óráról órára: km, irány és sebesség.

A számítások tájékoztató jellegűek: egy QSO azt mondja meg, hogy az útvonal nyitva volt, nem pontosan azt, hol volt a felhő. Minél tisztábbak az adatok (6 karakteres lokátorok, helyes `PROP_MODE`), annál megbízhatóbb a holtzóna.
