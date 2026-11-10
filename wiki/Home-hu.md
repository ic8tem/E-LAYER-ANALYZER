# E-LAYER ANALYZER

*készítette: Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Português (Brasil)](Home-pt-br) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Polski](Home-pl) · [Čeština](Home-cs) · **Magyar** · [Română](Home-ro) · [Ελληνικά](Home-el) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [ไทย](Home-th) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · [中文 (繁體)](Home-zh-tw)

Az **E-LAYER ANALYZER** ingyenes program rádióamatőrök számára, amely az ionoszféra **E-rétegét** érintő összes VHF terjedési módot elemzi: **sporadikus E** (az interkontinentális többugrásos terjedéssel együtt), **F.A.I.** (field-aligned irregularities, az erővonalak mentén rendeződött irregularitások), **meteor scatter** (a beépített YA-MS programmal) és **rádióaurora**. **Windowson (64 és 32 bites), Linuxon, Raspberry Pi-n és Macen** fut, **36 nyelven**, és semmit sem ír a saját mappáján kívülre.

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## A fülek

| Fül | Mit csinál |
|---|---|
| [SPORADIC-E](Sporadic-E-hu) | Es-spotok a térképen, mindkét állomás első ugrása, Es-felhők Maidenhead-négyzeteken, MUF a saját állomásra |
| [BENCHMARK Es](Benchmark-Es-hu) | Tesztpad egy teljes ADIF-naplón (akár más állomásén is): QSO-k naponta és óránként, holtzóna szektoronként, becsült foEs, többugrásos ellenőrzés, a felhők vándorlása |
| [BENCHMARK 50 MHz](Benchmark-50-hu) | Többugrásos sporadikus E 50 MHz-en egy vagy két ADIF-naplóból: Mercator-térkép, nap és időpont, opcionális foEs, kinagyított útvonal a WWL-lel, QSO-k 150 km-en belül |
| [F.A.I.](FAI-hu) | Egy állomáspár szórási pontjainak Scatter trailje, valamint izoflexiós helyek egy pont ellenőrzéséhez |
| [METEOR SCATTER](Meteor-Scatter-hu) | A teljes YA-MS program (rajhatékonyság, radarok, előrejelzések) |
| [AURORA](Aurora-hu) | Aurora-ovális, NOAA Kp és napszél, aurorás Scatter trail és izoflexió, ajánlott antennairány |
| [DATA COLLECTION](Data-Collection-hu) | Három telnet-kapcsolat (DX cluster, RBN, DM4X PSKReporterrel), gyűjtött spotok, CALL3-adatbázis |
| [LOG ADIF](Log-ADIF-hu) | Forgalmazott és megerősített négyzetek sávonként 28 MHz-től felfelé, saját ADIF-ből vagy a LoTW-ből |
| [FORRÁSOK, A SZÁMÍTÁSI VÁLASZTÁSOK ÖSSZEFOGLALÓJA, FELHASZNÁLÓI KÉZIKÖNYV, MŰSZAKI JELENTÉS](Sources-and-Technical-Document-hu) | Források, képletek és számítási választások, részletes felhasználói kézikönyv, műszaki jelentés: Wordbe exportálhatók |

## Első lépések

1. [Telepítés](Installation-hu)
2. [Első lépések](Getting-Started-hu)
3. [Színek és nyelvek](Colours-and-Languages-hu)
4. [Adatmappa](Data-Folder-hu)
5. [Gyakori kérdések](FAQ-hu)

## Fő források

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV, a sporadikus E távolságtáblázata ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)).
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (a YA-MS forrása).

A teljes lista a program FORRÁSOK fülén található.
