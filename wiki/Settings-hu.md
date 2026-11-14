# BEÁLLÍTÁSOK

[← Kezdőlap](Home-hu)

A 0.4.18-as verziótól a **BEÁLLÍTÁSOK** fül a program általános beállításait és a spotok fülek közötti szétosztását tartalmazza. Két allapja van.

## Általános

| Szakasz | Mit állít be |
|---|---|
| Állomás | Hívójel, lokátor és magasság, ugyanazok, mint a felső sávban; az **Alkalmaz** gomb minden fülre érvényesíti őket. |
| Megjelenés | Nyelv, színséma (a YAEC és a YA-MS sémái, a 0.5.0-s verziótól pedig az IP-TRX-CTRL hat témája: Világos, Éjszaka, Fekete, Kék, Lila, Zöld) és **az egész program betűmérete** (85-140 %, mint a YA-MS-ben). A nyelv és a színek újraépítik az ablakot: a beállítások és a spotok megmaradnak. |
| Térképek és előrejelzési adatok | A kontinens, ugyanaz a menü, mint a fülsávban. |
| Idő | Az NTP állapota és a szerverek ideje és a PC órája közötti különbség. |
| Frissítések | Ellenőrzés a GitHubon indításkor és 6 óránként, automatikus és csendes frissítés, **Ellenőrzés most** gomb. Az automatikus frissítés a rendszercsomagokra (.deb, .rpm, Arch, Slackware) nem vonatkozik. |
| Adatok | A `data` mappát megnyitó gomb. A Bureau of Meteorology API-kulcsa a DATA COLLECTION › Terjedési adatok részben marad. |

## Spotok szétosztása

Bal oldalon egy táblázat sorolja fel azokat a szabályokat, amelyek alapján a program eldönti, hová kerül egy spot, **az alkalmazás sorrendjében**: az első illeszkedő szabály dönti el a fület. A megjelenített értékek a szűrőkben beállított értékek.

| # | Ha a spot... | ide kerül |
|---|---|---|
| 1 | a helyi szűrőn kívül esik (27990 kHz alatt vagy 145 MHz felett; 430-440 MHz csak aurora) | elvetve |
| 2 | AUE, AU-E vagy AURORAL E szerepel benne | AURORA (aurora-E) |
| 3 | aurorát jelző szó szerepel benne (alapértelmezésben AU, AUR) | AURORA |
| 4 | F.A.I.-t jelző szó szerepel benne (alapértelmezésben FAI; sávok 6, 4 és 2 m: az F.A.I. nem éri el a 432 MHz-et) | F.A.I. |
| 5 | meteor scatter üzemmódú, vagy `<MS>` szerepel benne | METEOR SCATTER |
| 6 | a sávterv egy MS-ablakába esik (50 MHz-en nem kizárólagos; az FT8 és az FT4 50 és 70 MHz-en kizárva) | METEOR SCATTER |
| 7 | FT8 vagy FT4 50 MHz-en, és 4 percen belül nincs másik spot 150 km-en belüli felezőponttal | meteor scatter (nem sporadikus E) |
| 8 | Q65 vagy JT65 144 MHz-en (kivéve `<ES>`) | EME: elvetve |
| 9 | más terjedési mód címkéje szerepel benne | nem sporadikus E |
| 10 | transzekvatoriális | nem sporadikus E (TEP) |
| 11-15 | 28, 40, 50, 70 vagy 144 MHz-en van, a távolsági szabályokon belül | SPORADIC-E |
| 16 | egyik szabály alá sem esik (tropo, 432 MHz, HF...) | egyik fülre sem (a Gyűjtött spotok között marad) |

Jobb oldalon a **helyi szűrő** és az egyes módok szűrői vannak, öt allapon: Meteor scatter (a YA-MS szűrője a METEOR SCATTER fülön állítható), Sporadikus E, F.A.I., Aurora és **Szűrőpróba**, ahová beilleszthet egy cluster-sort, és megnézheti, hogyan sorolja be a program. **A szerző szabályainak visszaállítása** gomb visszahozza az alapértékeket.
