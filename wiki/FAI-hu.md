# F.A.I.

[← Home](Home-hu)

Az F.A.I. (Field Aligned Irregularities) a geomágneses tér mentén rendeződött irregularitásokon létrejövő szórás, amely a nyári estékre jellemző a sporadikus E után. A szórt jel egy **kúp** mentén távozik, amelynek tengelye a tér irányába esik: két állomás akkor tud összeköttetést létesíteni, ha **kiegészítő görbéken** fekszenek (a1 + a2 = 180°, IC8FAX-IC8TEM cikk; K9SE ±c görbéi). A mágneses teret az **IGRF-14** modell számítja.

## Scatter trail nézet (A-B pár)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Adja meg az **A állomást** és a **B állomást** (hívójel a CALL3-ban keresve és/vagy lokátor). Üresen hagyott A = a saját állomása.
- A térkép a **scatter trailt** mutatja: a réteg magasságában lévő összes pontot, amely mindkét állomásról látható és 2,5°-on belül teljesíti a visszaverődési feltételt (osztályok: 0-1, 1-1,5, 1,5-2, 2-2,5°).
- A jobb felső doboz A (piros) és B (zöld) **QTF-jét és emelkedési szögét** mutatja az egyes pontok felé.
- A program javasolja a **legvalószínűbb szórási pontot** (legkisebb eltérés, legrövidebb útvonal).
- **4/3 refrakció**: alapértelmezésként bekapcsolva, kiterjeszti a horizontokat.

## Izoreflexiós nézet (izoreflexiós görbék)

![Isoflection](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Ez az a geometriai szerkesztés, amellyel **ellenőrizhető, hogy egy visszaverődési pont helyes-e**.

- **Bal kattintás** a térképen: kiválasztja az ellenőrizendő pontot. **Jobb kattintás**: áthelyezi a B állomást.
- **Az ajánlott pont ellenőrzése**: megnyitja a diagramot a program által javasolt ponton.
- Szintvonalak **0, ±1, ±2, ±4, ±8, ±16°** értéknél: színes sávok a sötét sémákban, **vastag vonalak** a világos sémákban.
- Ítélet a párról: **kielégítő** 1°-on belül, **elfogadható** 2,5°-on belül, **nem kielégítő** azon túl.
- Ha a pont valamelyik állomás horizontja alatt van, megjelenik **A számítás nem lehetséges** ablak, és semmi más.

## Ellenőrzések

| Összeköttetés | Szórási pont | Eltérés | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (Genova felett) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (DNy-Németország, 28 km-re Brüsszeltől) | 0,88° | 338° / 87° |
| DB0FAI - Drezda Sheffielden keresztül (DL5MCG) | – | 0,3° | – |

Ismert szórási pontok a listában: Genf, Brüsszel, Budapest, JN92, JN41/42.
