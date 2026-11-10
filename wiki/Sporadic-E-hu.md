# SPORADIC-E

[← Home](Home-hu)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## A térkép

A térkép feletti gomb választja ki a nézetet:

- **Azimutális**: azimutális távolságtartó térkép az Ön állomására középpontozva, **nagyítás** csúszkával (300-tól 20000 km-ig a középponttól, egérgörgővel is).
- **Mercator**: a világtérkép, az Európa és Világ gombokkal.

Mindkét vetületben az állomás köré rajzolódnak **Flavio IK3XTV távolsági körei**: 500 km (egyszeres ugrás minimuma), 1500-2000 km (optimum), 2400 km (egyszeres ugrás maximuma), 5000 km (kettős ugrás maximuma) és 6900 km (hármas ugrás maximuma).

Az azimutális nézetekben a szaggatott gyűrűk a **középponttól mért km-t** mutatják (a lépésköz a nagyítással változik), a szél a legnagyobb megjelenített távolságot jelöli, az egérkurzor pedig megadja a középponttól mért távolságot és azimutot.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Mit mutat

- Az utolsó percek **spotjai** (15 perc - 12 óra) a **28, 50, 70 és 144 MHz**-es sávokon, a telnet-munkamenetekből. A lokátorok a spot megjegyzéséből (pl. `JN70CN<ES>JO62QM`) vagy a CALL3 adatbázisból származnak.
- A szövegükben **<MS>**, **<AU>** vagy **<TR>** jelölést tartalmazó spotok **elvetésre kerülnek**: nem a sporadikus E-hez tartoznak, de hasznosak a meteorszóráshoz, az aurorához vagy a tropóhoz.
- **Transzekvatoriális, a szerző szabálya (IC8TEM)**: Afrika és Dél-Amerika felé, ha a felezőpont a -20° és +20° közötti mágneses inklinációs sávba esik, az összeköttetés transzekvatoriális (TEP), nem sporadikus E, kivéve, ha a spot `<ES>` jelölést tartalmaz.
- **144 MHz, a szerző szabálya (IC8TEM)**: a sporadikus E 2 m-en **900 km felett** kezdődik (a rövidebb spotokat elveti). Egyetlen 144 MHz-es spot **önmagában nem rajzolódik ki**: akkor jelenik meg, ha **5 percen belül** érkezik egy második 144 MHz-es spot ugyanott lévő felhővel (visszaverődési pontok 300 km-en belül), vagy ha a közelben van egy **rövid ugrású 50 MHz-es** spot (1200 km-en belül, az utolsó 15 percben). A felső sáv mutatja, hány 144 MHz-es spot vár megerősítésre.
- Minden spotra: távolság, az **ugrások száma** az **IK3XTV** táblázat szerint, emelkedési szög, **minimális foEs** (szekánstörvény).
- **Mindkét állomás első ugrása**: narancs négyzet = a spotter első ugrása, lila = a DX első ugrása (egyenlő ugrások); sárga = az egyszeres ugrás felezőpontja.
- **Rövid első ugrás** (üres körök): többugrásos nyitásokban a szerző a saját QTH-jától **1200 km-en belüli** rövid első ugrást tapasztalt; a hossz beállítható.
- **Es felhők** a **Maidenhead négyzeteken** (2° x 1°), a minimális foEs szerint színezve: kék < 5, világoskék < 7, zöld < 10, sárga < 14, narancs < 20, piros > 20 MHz.
- **Es felhők** táblázata: minden négyzetre a **MUF az Ön állomására** (piros = 144 MHz lehetséges, narancs = 70, sárga = 50).

## IK3XTV táblázat

| | minimum | optimum | maximum |
|---|---|---|---|
| Egyszeres ugrás | 500-650 km | 1500-2000 km | 2200-2400 km |
| Kettős ugrás | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Hármas ugrás | – | 4800-6400 km | 6900 km |

## Használat

- **Kattintás egy spotra**: az útvonal kiemelődik, a részletekben a visszaverődési pontokkal, lokátorokkal és azimutokkal.
- **Kattintás egy spotra** a táblázatban: az útvonal felvillan a térképen (sárga fény), és ha kívül esik a nézeten, a térkép elmozdul vagy növeli a sugarát, hogy megmutassa. A spot kijelölve marad, amikor új spotok érkeznek.
- **A táblázat követi a térképet**: beállított nagyításnál a spottáblázat és a felhőtáblázat csak azt mutatja, ami a térképen látható.
- **Kattintás a térképre**: a pontot lehetséges Es felhőként kezeli, a 28, 50, 70 és 144 MHz-hez szükséges foEs-sel.

## Megjegyzés a 432 MHz-ről

A sporadikus E **432 MHz-en nem alkalmazható**: az Es MUF kb. 150-200 MHz-ig ér, és az ARRL rekordjai Es összeköttetéseket csak 50 és 144 MHz-en sorolnak fel. A 70 cm-es spotokat itt nem elemzi.

## Frekvencia szerint színezett négyzetek és villogó MUF

A visszaverődési pontok Maidenhead négyzetei a spot legmagasabb frekvenciája szerint színeződnek: **zöld** 29 MHz-ig, **sárga** 51-ig, **narancs** 71-ig, **piros** 145 MHz-ig. A négyzet foEs értékéből becsli a valószínű MUF-ot a leghosszabb ugrásra (2400 km, IK3XTV táblázat): ha az magasabb sávot ér el, a négyzet az adott sáv színében **villog** (IK3XTV és Jacobs-Leinwoll elve: rövid skip magas MUF-ot jelent).

A térkép jobb felső sarkában megjelenik az N0NBH összefoglalója (E-skip a választott kontinensre, SFI, K- és A-index).
