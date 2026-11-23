# F.A.I.

[← Kezdőlap](Home-hu)

Az F.A.I. (Field Aligned Irregularities) a geomágneses tér mentén rendeződött irregularitásokon történő szórás, amely a nyári estékre jellemző, a sporadikus E után. A szórt jel egy **kúpon** távozik, amelynek tengelye a tér irányába esik: két állomás akkor tud összeköttetést létesíteni, ha **kiegészítő kontúrokon** helyezkedik el (a1 + a2 = 180°, IC8FAX-IC8TEM cikk; K9SE ±c kontúrjai). A mágneses teret az **IGRF-14** modellel számolja.

**F.A.I.-figyelmeztetés** (a 0.6.5-ös verziótól): májustól szeptemberig, helyi idő szerint 18 és 24 óra között, egy az állomásától 300-1300 km-re lévő Es-felhő után a fül **villog**, és felül egy piros sor megmondja, hol lehetséges az F.A.I. (az IC8FAX-IC8TEM cikk szabályai); friss F.A.I.-spotokkal azt írja, hogy folyamatban van. Kattintás a sorra: Izoflexió nézet a pontra.

## Scatter trail nézet (A-B pár)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Adja meg az **A állomást** és a **B állomást** (a CALL3-ban keresett hívójel és/vagy lokátor). Üresen hagyott A = a saját állomás.
- A térkép a **Scatter trailt** mutatja: a réteg magasságában lévő összes olyan pontot, amelyet mindkét állomás lát, és amely 2,5°-on belül teljesíti a visszaverődési feltételt (osztályok: 0-1, 1-1,5, 1,5-2, 2-2,5°).
- A jobb felső mező A (piros) és B (zöld) **QTF-jét és elevációját** mutatja az egyes pontok felé.
- A program javasolja a **legvalószínűbb szórási pontot** (legkisebb eltérés, legrövidebb útvonal).
- **4/3-os refrakció**: alapértelmezésben bekapcsolva, kiterjeszti a horizontokat.

## Izoflexió nézet (izoflexiós helyek)

![Izoflexió](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Ez az a geometriai szerkesztés, amellyel **ellenőrizhető, hogy egy visszaverődési pont helyes-e**.

- **Bal kattintás** a térképen: kiválasztja az ellenőrizendő pontot. **Jobb kattintás**: a B állomást mozgatja.
- **Az ajánlott pont ellenőrzése**: megnyitja a diagramot a program által javasolt ponton.
- Szintvonalak **0, ±1, ±2, ±4, ±8, ±16°**-nál: színes sávok a sötét sémákban, **vastag vonalak** a világos sémákban.
- Értékelés a párra: **kielégítő** 1°-on belül, **elfogadható** 2,5°-on belül, **nem kielégítő** azon túl.
- Ha a pont az egyik állomás horizontja alatt van, **A számítás nem lehetséges** felugró ablak jelenik meg, és semmi más.

## Ellenőrzések

| Összeköttetés | Szórási pont | Eltérés | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44EH (Genova felett) | 0,96° | 313° / 77° |
| IC8FAX - G4LOH | JN49BR (DNy-Németország, 18 km-re Brüsszeltől) | 0,93° | 337° / 88° |
| DB0FAI jeladó (305°) - Drezda Sheffielden át (DL5MCG; Drezda 290°-ra irányzott és aurorát jelentett) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, az 1990-es IGRF-tér) | JN45QR, a nyom 35 km-re Genftől (Genf: 3,79°) | 0,92° | 327° / 87° |
| DL5MCG - SV1DH (1987-es teszt Budapesten át, 1569 km, az 1987-es IGRF-tér) | Budapest 2,22°-on, a 2,5°-on belül (a mai térrel 1,85°); a nyom 21 km-re; legvalószínűbb KN06TQ | 2,22° (KN06TQ 0,92°) | 95° / 341° |

A 0.4.18-as verziótól a két dátummal rendelkező eset (1987 és 1990) az adott év IGRF-14 terét használja, amíg az állomások az esetéi maradnak.

A listában szereplő ismert szórási pontok: Genf, Brüsszel, Budapest, JN92, JN41/42.

Az **Ellenőrzések...** menü betölti az IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE és DL5MCG-SV1DH tesztpárokat.

**Felszerelés** (DL5MCG): a 144 MHz-es F.A.I.-hoz legalább egy hosszú yagi és 100 W kell (SV1DH-t egy 8 elemes yagival és 50 W-tal is sikerült elérni). Az antennát a program által megadott szórási pontra irányozza, ne az ellenállomásra.
