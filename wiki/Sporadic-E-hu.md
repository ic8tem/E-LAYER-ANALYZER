# SPORADIC-E

[← Kezdőlap](Home-hu)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## A térkép

A térkép feletti gomb választja ki a nézetet:

- **Azimutális**: a saját állomásra középpontozott azimutális távolságtartó térkép, **nagyítási** csúszkával (a középponttól 300 és 20000 km között, az egérgörgővel is).
- **Mercator**: a világtérkép, az Európa és a Világ gombbal.

Mindkét vetületben az állomás köré rajzolja **Flavio IK3XTV távolságköreit**: 500 km (egyugrásos minimum), 1500-2000 km (optimum), 2400 km (egyugrásos maximum), 5000 km (kétugrásos maximum) és 6900 km (háromugrásos maximum).

Az azimutális nézetekben a szaggatott gyűrűk a **középponttól mért km-t** mutatják (a lépésköz a nagyítással változik), a szél a megjelenített legnagyobb távolságot jelöli, az egérkurzor pedig a középponttól mért távolságot és azimutot adja meg.

![Többugrásos terjedés](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Mit mutat

- Az utolsó néhány perc (15 perc - 12 óra) **spotjait** a **28, 40 (8 m, alapértelmezésben kikapcsolva), 50, 70 és 144 MHz-es** sávokon, a telnet-kapcsolatokból. A lokátorok a spot megjegyzéséből (pl. `JN70CN<ES>JO62QM`) vagy a CALL3-adatbázisból származnak.
- A szövegükben **<MS>**, **<AU>** vagy **<TR>** jelölést tartalmazó spotokat **elveti**: ezek nem a sporadikus E-hez tartoznak, de a meteor scatterhez, az aurorához vagy a tropóhoz hasznosak.
- **Transzekvatoriális, a szerző (IC8TEM) szabálya**: Afrika és Dél-Amerika felé, ha a felezőpont a -15° és +15° közötti mágneses inklinációs sávba esik, az összeköttetés transzekvatoriális (TEP) és nem sporadikus E, hacsak a spot nem jelez `<ES>`-t.
- **144 MHz, a szerző (IC8TEM) szabálya**: a 2 m-es sporadikus E **900 km felett** kezdődik (a rövidebb spotokat elveti). Egyetlen 144 MHz-es spot **önmagában nem jelenik meg**: akkor jelenik meg, ha **5 percen belül** érkezik egy második 144 MHz-es spot ugyanott lévő felhővel (visszaverődési pontok 300 km-en belül), vagy ha a közelben van egy **rövid ugrású 50 MHz-es** spot (1200 km-en belül, az utolsó 15 percben). A felső sáv mutatja, hány 144 MHz-es spot vár megerősítésre.
- Minden spotra: távolság, az **ugrások száma** az **IK3XTV**-táblázat szerint, eleváció, **minimális foEs** (szekánstörvény).
- **Mindkét állomás első ugrása**: narancs négyzet = a spottoló első ugrása, lila = a DX első ugrása (egyenlő ugrások); sárga = egyugrásos terjedés felezőpontja.
- **Rövid első ugrás** (üres körök): többugrásos nyitásoknál a szerző tapasztalt rövid első ugrást saját QTH-jától **1200 km-en** belül; a hossz beállítható.
- **Es-felhők** a **Maidenhead-négyzeteken** (2° x 1°), a minimális foEs szerint színezve: kék < 5, világoskék < 7, zöld < 10, sárga < 14, narancs < 20, piros > 20 MHz.
- **Es-felhők** táblázat: minden négyzetre a **MUF a saját állomásra** (piros = 144 MHz lehetséges, narancs = 70, sárga = 50).

## IK3XTV-táblázat

| | minimum | optimum | maximum |
|---|---|---|---|
| Egyugrásos | 500-650 km | 1500-2000 km | 2200-2400 km |
| Kétugrásos | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Háromugrásos | – | 4800-6400 km | 6900 km |

## Használat

- **Kattintás egy spotra**: az útvonal kiemelődik, a részletekben a visszaverődési pontokkal, lokátorokkal és azimutokkal.
- **Kattintás egy spotra** a táblázatban: az útvonal felvillan a térképen (sárga fénykoszorú), és ha nem látható, a térkép odamozdul vagy növeli a sugarát, hogy megmutassa. A spot kiválasztva marad, amikor új spotok érkeznek.
- **A táblázat követi a térképet**: beállított nagyításnál a spot- és a felhőtáblázat csak azt mutatja, ami a térképen látható.
- **Kattintás a térképre**: a pontot lehetséges Es-felhőként kezeli, a 28, 50, 70 és 144 MHz-hez szükséges foEs-sel.

## Megjegyzés a 432 MHz-ről

A sporadikus E **nem vonatkozik a 432 MHz-re**: az Es-MUF kb. 150-200 MHz-ig ér, és az ARRL-rekordok csak 50 és 144 MHz-en sorolnak fel Es-összeköttetéseket. A 70 cm-es spotokat itt nem elemzi.

## Frekvencia szerint színezett négyzetek és villogó MUF

A visszaverődési pontok Maidenhead-négyzeteit a spotok legmagasabb frekvenciája szerint színezi: **zöld** 29 MHz-ig, **sárga** 51-ig, **narancs** 71-ig, **piros** 145 MHz-ig. A négyzet foEs-éből becsüli a valószínű MUF-ot a leghosszabb ugrásra (2400 km, IK3XTV-táblázat): ha egy magasabb sávot is elér, a négyzet annak a sávnak a színében **villog** (IK3XTV és Jacobs-Leinwoll elve: rövid holtzóna = magas MUF).

A térkép jobb felső sarkában megjelenik az N0NBH összefoglalója (E-skip a választott kontinensre, SFI, K- és A-index).

## Sávonkénti szabályok (0.4.18-as verzió)

- **50 MHz**: 450 km-en belül nem sporadikus E. Az FT8 és FT4 spotok csak akkor sporadikus E-k, ha egy felhőhöz tartoznak: egy másik 50 MHz-es spot 4 percen belül, 150 km-en belüli felezőponttal. Az elszigetelt FT8/FT4 spot a meteor scatterhez kerül.
- **70 MHz**: 450 km-en belül nem sporadikus E. Az FT8 és az FT4 a sporadikus E-hez kerül akkor is, ha a sávterv meteor scatter ablakába esik (először az ismert üzemmód dönt, utána a frekvencia).
- **40 MHz (8 m)**: a minimális távolság az 50 MHz-es 450 km-ből a szekánstörvénnyel származik (kb. 330 km); csak az E-réteg geometriája szerinti első ugráson belüli távolságok (kb. 2440 km). Az útvonal színe: sárga.
- **144 MHz**: sporadikus E 900 km-től, megerősítéssel; a Q65 és JT65 spotok EME-k, és elveti őket, kivéve, ha a szövegben `<ES>` szerepel.
- G0ISW (Propagation identification, 2024) a legrövidebb sporadikus E-t 50 és 70 MHz-en 400 km felett, 144 MHz-en 1400 km felett látja: a szerző szabályai 50 és 70 MHz-en egyeznek vele, 144 MHz-en tágabbak, de megerősítést kérnek.
- Az összes szabály alkalmazásának sorrendjét a használt értékekkel a **[BEÁLLÍTÁSOK](Settings-hu)** › Spotok szétosztása mutatja.
