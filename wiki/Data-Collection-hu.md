# DATA COLLECTION

[← Kezdőlap](Home-hu)

## Telnet-kapcsolatok

| Kapcsolat | Alapértelmezés | Megjegyzések |
|---|---|---|
| 1. Választott DX cluster | on0dxk.dyndns.org 8000 | Az ON0DXK-n a VHF-spotok szűrve vannak: `set/filter dxbm/pass 6` (majd 4, 2). Másik példa: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW és RTTY, 7001 = FT8. |
| 3. Cluster PSKReporterrel | **dm4x.ddns.net 8500** | DM4X clustere; alternatívaként HamAlert (hamalert.org 7300, fiók szükséges). |

- Automatikus bejelentkezés a hívójellel (felül vagy a Bejelentkezés mezőben); `;`-vel elválasztott **kezdeti parancsok**.
- **Újracsatlakozás** 30 s után, ha a vonal megszakad; **Csatlakozás induláskor**.
- Alapértelmezésben csak a **28 MHz feletti** spotokat menti.

## Gyűjtött spotok

Az utolsó 2000 spot, sáv- és forrásszűrővel. Archívum: naponta egy CSV-fájl a `data/spot` mappában. A program minden bezárásakor kiüríti a spot-adatbázist (a memóriát és a `data/spot` fájljait): minden munkamenet elölről kezdődik.

**Helyi szűrő** (opcionális jelölőnégyzet): a 27990,0 kHz alatti vagy 145 MHz feletti "DX de" sorokat elveti, ezek nem jelennek meg a konzolon, és nem tárolódnak. A **pontatlan lokátorokat** (6 karakternél rövidebbeket) a CALL3-ból veszi: ha a CALL3-ban 6 karakteres lokátor van, az mindig elsőbbséget élvez. A YA-MS-hez hasonlóan meteor scatter (nem sporadikus E) spotnak számítanak az MS-re jellemző frekvenciaablakokba (50190-50300, 70100-70400, 144290-144399 kHz) eső, más terjedési mód címkéit (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) nem tartalmazó spotok, valamint a kizárólag MS-üzemmódúak (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) vagy `<MS>` jelölésűek. Az `<AS>`/`<ACS>` (repülőgép-szórás, aircraft scatter) jelölésű spotokat nem használja a sporadikus E-hez.

## Szűrőoszlop (jobb oldali 40%)

Az adatgyűjtés **az egész programnak egy**: minden spot ezeken a szűrőkön halad át, majd az egyes terjedési módok fülére kerül (az E-LAYER ANALYZER-en belül a YA-MS Adatgyűjtés fül nem látható). Felül a **helyi szűrő** jelölőnégyzete; alatta öt allap:

| Allap | Mit állít be | Alapértékek (a szerző szabályai) |
|---|---|---|
| Meteor scatter | a YA-MS szűrője: MS-frekvenciaablakok, más terjedési módok címkéi, MS-üzemmódok és `<MS>`, sávok | mint a YA-MS-ben |
| Sporadikus E | a spotot kizáró `< >` címkék, 144 MHz-es szabály, transzekvatoriális szabály | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 perc, 300 km, 50 MHz-es rövid ugrás 1200 km/15 perc; ±20° |
| F.A.I. | F.A.I.-szavak, sávok, időtartam a térképen | FAI, minden sáv, 24 óra |
| Aurora | aurora `< >` címkék, sávok, időtartam a térképen | `AU` `AUR` (`< >` között vagy anélkül), minden sáv, 2 óra |
| Szűrőpróba | illesszen be egy cluster-sort, és nézze meg, hogyan sorolja be az összes szűrő | |

A **szerző szabályainak visszaállítása** gomb visszahozza az alapértékeket.

## CALL3

A JTDX **hívójel → lokátor** adatbázisa (több mint egymillió hívójel) a `data/call3.txt` fájlban. Gombok: **Újratöltés** és **Új CALL3.TXT importálása**. A spotok megjegyzéseiben olvasott 6 karakteres lokátorok a `data/call3_nuovi.txt` fájlba kerülnek.

A 0.4.16-os verziótól az egész programnak egyetlen CALL3 adatbázisa van, a YA-MS Adatgyűjtéséé (METEOR SCATTER fül), a LIVETROPOVHF szabályaival: frissítés az MMMonVHF-ről, kézi bejegyzések, hozzáadott fájlok, klaszterjavítások, tanult lokátorok és G7VRD 6 óránként; a javítások a data/call3.txt fájlba íródnak, a data/call3_nuovi.txt lokátorai egyszer átkerülnek. A WSJT-X, JTDX és MSHV spotjai UDP-n is érkeznek (forrás: WSJT-X UDP, 2237-es port, továbbítás a 2238-ra).

Ezek a YA-MS-ből származó szűrők (CW, legkisebb távolság, frekvenciaablakok, MS módok, sávok) csak a METEOR SCATTER fülre vonatkoznak: ők döntik el, mely spotok kerülnek a gyűjteményébe. A többi fül minden spotot megkap, és a saját szűrőit használja.

## Spotok és Terjedési adatok allapok

A fülnek két allapja van: **Spotok** (kapcsolatok, gyűjtött spotok, CALL3 és szűrőoszlop) és **Terjedési adatok**:

- **N0NBH** (hamqsl.com): VHF-banner és napadatok, E-skip Európa és Észak-Amerika (DXrobot), aurora.
- **Bz aurorához**: NOAA SWPC valós idejű napszél (DSCOVR/ACE, L1 pont), a Bz legjobb forrása.
- **Meteor scatter**: napi meteoraktivitás (RMOB) a Make More Miles on VHF-től.

Legfeljebb óránként egyszer töltődnek le. Az összefoglaló a SPORADIC-E, F.A.I. és AURORA térképeken jelenik meg. A fülsáv jobb oldalán lévő **kontinensmenü** a térképeket az adott kontinensre viszi, és kiválasztja annak adatait.
