# DATA COLLECTION

[← Home](Home-ro)

## Sesiuni telnet

| Sesiune | Implicit | Note |
|---|---|---|
| 1. DX cluster la alegere | on0dxk.dyndns.org 8000 | Pe ON0DXK spoturile VHF sunt filtrate: `set/filter dxbm/pass 6` (apoi 4, 2). Alt exemplu: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW și RTTY, 7001 = FT8. |
| 3. Cluster cu PSKReporter | **dm4x.ddns.net 8500** | Clusterul DM4X; ca alternativă HamAlert (hamalert.org 7300, este necesar un cont). |

- Login automat cu indicativul (sus sau în câmpul Login); **comenzi inițiale** separate prin `;`.
- **Reconectează** după 30 s dacă legătura cade; **Conectare la pornire**.
- Implicit se salvează doar spoturile **de la 28 MHz în sus**.

## Spoturi colectate

Ultimele 2000 de spoturi, cu filtre pe bandă și sursă. Arhivă: câte un fișier CSV pe zi în `data/spot`. La fiecare închidere a programului baza de date a spoturilor se golește (memoria și fișierele din `data/spot`): fiecare sesiune pornește de la zero.

**Filtru local** (casetă opțională): rândurile „DX de” sub 27990,0 kHz sau peste 145 MHz sunt eliminate, nu apar în consolă și nu se memorează. **Locatorii impreciși** (sub 6 caractere) se iau din CALL3: dacă CALL3 are un locator cu 6 caractere, acesta prevalează întotdeauna. Ca în YA-MS, sunt spoturi de meteor scatter (nu de E-sporadic) cele din ferestrele de frecvență tipice pentru MS (50190-50300, 70100-70400, 144290-144399 kHz) fără inscripții de altă propagare (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) și cele cu moduri exclusiv MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) sau cu `<MS>`. Spoturile cu `<AS>`/`<ACS>` (aircraft scatter) nu se folosesc pentru E-sporadic.

## Coloana filtrelor (40% în dreapta)

Colectarea datelor este **una singură pentru tot programul**: fiecare spot trece prin aceste filtre și ajunge la fila fiecărui mod de propagare (în E-LAYER ANALYZER fila Colectare date din YA-MS nu este afișată). Sus se află caseta **filtrului local**; dedesubt, cinci sub-file:

| Sub-filă | Ce se setează | Implicit (regulile autorului) |
|---|---|---|
| Meteor scatter | filtrul YA-MS: ferestrele de frecvență MS, inscripțiile de altă propagare, modurile MS și `<MS>`, benzile | ca în YA-MS |
| E-sporadic | inscripțiile `< >` care exclud spotul, regula pentru 144 MHz, regula transecuatorială | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, salt scurt 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | cuvintele F.A.I., benzile, perioada pe hartă | FAI, toate benzile, 24 de ore |
| Auroră | inscripțiile `< >` de auroră, benzile, perioada pe hartă | `AU` `AUR` (cu sau fără `< >`), toate benzile, 2 ore |
| Testul filtrelor | lipește un rând din cluster și vezi cum îl clasifică toate filtrele | |

Butonul **Restabilește regulile autorului** readuce valorile implicite.

## CALL3

Arhiva JTDX **indicativ → locator** (peste un milion de indicative) în `data/call3.txt`. Butoanele **Reîncarcă** și **Importă un nou CALL3.TXT**. Locatorii cu 6 caractere citiți în comentariile spoturilor sunt adăugați în `data/call3_nuovi.txt`.

Începând cu versiunea 0.4.16 există o singură bază CALL3 pentru tot programul, cea din Colectarea datelor YA-MS (fila METEOR SCATTER), cu aceleași reguli ca LIVETROPOVHF: actualizare de la MMMonVHF, intrări manuale, fișiere adăugate, corecturi din cluster, locatoare învățate și G7VRD la fiecare 30 de minute; corecturile se scriu în data/call3.txt, iar locatoarele din data/call3_nuovi.txt se importă o singură dată. Spoturile din WSJT-X, JTDX și MSHV sosesc și prin UDP (sursa WSJT-X UDP, portul 2237 cu redirecționare la 2238).

Aceste filtre, venite din YA-MS (CW, distanța minimă, ferestre de frecvență, moduri MS, benzi), se aplică doar filei METEOR SCATTER: ele decid ce spoturi intră în colecția ei. Celelalte file primesc toate spoturile și își folosesc propriile filtre.

## Sub-filele Spoturi și Date de propagare

Fila are două sub-file: **Spoturi** (sesiuni, spoturi colectate, CALL3 și coloana filtrelor) și **Date de propagare**:

- **N0NBH** (hamqsl.com): bannerul VHF și datele solare, E-skip Europa și America de Nord (DXrobot), auroră.
- **Bz pentru auroră**: vântul solar în timp real NOAA SWPC (DSCOVR/ACE, punctul L1), cea mai bună sursă pentru Bz.
- **Meteor scatter**: activitatea meteorică zilnică (RMOB) de la Make More Miles on VHF.

Se descarcă cel mult o dată pe oră. Sinteza apare pe hărțile SPORADIC-E, F.A.I. și AURORA. **Meniul continentului**, în dreapta barei filelor, mută hărțile și selectează datele acelui continent.
