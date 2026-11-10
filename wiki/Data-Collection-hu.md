# DATA COLLECTION

[← Home](Home-hu)

## Telnet-munkamenetek

| Munkamenet | Alapértelmezés | Megjegyzések |
|---|---|---|
| 1. Választható DX cluster | on0dxk.dyndns.org 8000 | Az ON0DXK-n a VHF spotok szűrve vannak: `set/filter dxbm/pass 6` (majd 4, 2). Másik példa: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW és RTTY, 7001 = FT8. |
| 3. Cluster PSKReporterrel | **dm4x.ddns.net 8500** | DM4X clustere; alternatívaként HamAlert (hamalert.org 7300, fiók szükséges). |

- Automatikus bejelentkezés a hívójellel (felül vagy a Login mezőben); `;` jellel elválasztott **kezdő parancsok**.
- **Újracsatlakozás** 30 s után, ha a vonal megszakad; **Csatlakozás induláskor**.
- Alapértelmezés szerint csak a **28 MHz-től felfelé** érkező spotok mentődnek.

## Összegyűjtött spotok

Az utolsó 2000 spot, sáv- és forrásszűrőkkel. Archívum: naponta egy CSV fájl a `data/spot` mappában. A program minden bezárásakor a spot-adatbázis kiürül (a memória és a `data/spot` fájljai): minden munkamenet elölről indul.

**Helyi szűrő** (opcionális négyzet): a 27990,0 kHz alatti vagy 145 MHz feletti „DX de” sorokat eldobja, nem jeleníti meg a konzolon és nem tárolja. A **pontatlan lokátorokat** (6 karakternél rövidebbek) a CALL3-ból veszi: ha a CALL3-ban 6 karakteres lokátor van, mindig az az érvényes. Ahogy a YA-MS-ben, meteorszórásos (nem Sporadic-E) spotnak számítanak a tipikus MS frekvenciaablakokba (50190-50300, 70100-70400, 144290-144399 kHz) eső, más terjedésre utaló jelölés (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) nélküli spotok, valamint a csak MS-re használt üzemmódú (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) vagy `<MS>` jelölésű spotok. Az `<AS>`/`<ACS>` (aircraft scatter) jelölésű spotokat a Sporadic-E nem használja.

## Szűrőoszlop (jobb oldali 40 %)

Az adatgyűjtés **egy az egész programra**: minden spot átmegy ezeken a szűrőkön, és továbbjut az egyes terjedési módok fülére (az E-LAYER ANALYZER-en belül a YA-MS Adatgyűjtés füle nem látszik). Felül a **helyi szűrő** jelölőnégyzete; alatta öt alfül:

| Alfül | Mit állít be | Alapértékek (a szerző szabályai) |
|---|---|---|
| Meteor scatter | a YA-MS szűrője: MS frekvenciaablakok, más terjedési jelölések, MS üzemmódok és `<MS>`, sávok | mint a YA-MS-ben |
| Sporadic-E | a spotot kizáró `< >` jelölések, 144 MHz-es szabály, transzekvatoriális szabály | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, 50 MHz rövid ugrás 1200 km/15 min; ±20° |
| F.A.I. | F.A.I. szavak, sávok, időszak a térképen | FAI, minden sáv, 24 óra |
| Aurora | aurora `< >` jelölések, sávok, időszak a térképen | `AU` `AUR` (`< >`-vel vagy anélkül), minden sáv, 2 óra |
| Szűrőteszt | illesszen be egy cluster sort, és nézze meg, hogyan sorolja be az összes szűrő | |

**A szerző szabályainak visszaállítása** gomb visszahozza az alapértékeket.

## CALL3

JTDX **hívójel → lokátor** adatbázis (több mint egymillió hívójel) a `data/call3.txt` fájlban. Gombok: **Újratöltés** és **Új CALL3.TXT importálása**. A spotok megjegyzéseiben olvasott 6 karakteres lokátorok a `data/call3_nuovi.txt` fájlba kerülnek.

## A Spot és a Terjedési adatok alfülek

A fülnek két alfüle van: **Spot** (munkamenetek, összegyűjtött spotok, CALL3 és szűrőoszlop) és **Terjedési adatok**:

- **N0NBH** (hamqsl.com): VHF szalag és napadatok, E-skip Európa és Észak-Amerika (DXrobot), aurora.
- **Bz az aurorához**: NOAA SWPC valós idejű napszél (DSCOVR/ACE, L1 pont), a legjobb forrás a Bz-hez.
- **Meteor scatter**: napi meteoraktivitás (RMOB) a Make More Miles on VHF oldalról.

Legfeljebb óránként egyszer töltődnek le. Az összefoglaló a SPORADIC-E, F.A.I. és AURORA térképeken jelenik meg. A fülsáv jobb oldalán lévő **kontinensmenü** áthelyezi a térképeket, és kiválasztja az adott kontinens adatait.
