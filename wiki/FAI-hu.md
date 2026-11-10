# F.A.I.

[← Kezdőlap](Home-hu)

Az F.A.I. (Field Aligned Irregularities) a geomágneses tér mentén rendeződött irregularitásokon történő szórás, amely a nyári estékre jellemző, a sporadikus E után. A szórt jel egy **kúpon** távozik, amelynek tengelye a tér irányába esik: két állomás akkor tud összeköttetést létesíteni, ha **kiegészítő kontúrokon** helyezkedik el (a1 + a2 = 180°, IC8FAX-IC8TEM cikk; K9SE ±c kontúrjai). A mágneses teret az **IGRF-14** modellel számolja.

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
| IC8FAX - F6DRO | JN44GH (Genova felett) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (DNy-Németország, 28 km-re Brüsszeltől) | 0,88° | 338° / 87° |
| DB0FAI - Drezda Sheffielden át (DL5MCG) | – | 0,3° | – |

A listában szereplő ismert szórási pontok: Genf, Brüsszel, Budapest, JN92, JN41/42.
