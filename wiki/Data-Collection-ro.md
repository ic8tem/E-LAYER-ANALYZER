# DATA COLLECTION

[← Home](Home-ro)

## Sesiuni telnet

| Sesiune | Implicit | Note |
|---|---|---|
| 1. DX cluster la alegere | on0dxk.dyndns.org 8000 | Pe ON0DXK spoturile VHF sunt filtrate: `set/filter dxbm/pass 6` (apoi 4, 2). Alt exemplu: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW și RTTY, 7001 = FT8. |
| 3. Cluster cu PSKReporter | **dm4x.ddns.net 8500** | Clusterul lui DM4X; alternativ HamAlert (hamalert.org 7300, necesită cont). |

- Autentificare automată cu indicativul (sus sau în câmpul Login); **comenzi inițiale** separate prin `;`.
- **Reconectare** după 30 s dacă linia cade; **Conectare la pornire**.
- Implicit se salvează doar spoturile **de la 28 MHz în sus**.

## Spoturi colectate

Ultimele 2000 de spoturi, cu filtre după bandă și sursă. Arhivă: un fișier CSV pe zi în `data/spot`. De fiecare dată când programul se închide, baza de date a spoturilor se golește (memoria și fișierele din `data/spot`): fiecare sesiune începe de la zero.

**Filtru local** (casetă opțională): liniile „DX de” sub 27990,0 kHz sau peste 145 MHz sunt aruncate, nu sunt afișate în consolă și nu sunt stocate. **Locatoarele imprecise** (sub 6 caractere) se iau din CALL3: dacă CALL3 are un locator de 6 caractere, acesta prevalează întotdeauna. Ca în YA-MS, spoturile de meteor scatter (nu Sporadic-E) sunt cele din ferestrele de frecvență tipice MS (50190-50300, 70100-70400, 144290-144399 kHz) fără etichete ale altei propagări (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), precum și cele cu moduri exclusiv MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) sau cu `<MS>`. Spoturile cu `<AS>`/`<ACS>` (aircraft scatter) nu sunt folosite pentru Sporadic-E.

## Coloana de filtre (dreapta, 40 %)

Colectarea datelor este **una singură pentru tot programul**: fiecare spot trece prin aceste filtre și ajunge la fila fiecărui mod de propagare (în interiorul E-LAYER ANALYZER fila Colectare date a YA-MS nu este afișată). Sus caseta **filtrului local**; dedesubt, cinci sub-file:

| Sub-filă | Ce setați | Valori implicite (regulile autorului) |
|---|---|---|
| Meteor scatter | filtrul YA-MS: ferestre de frecvență MS, etichete ale altei propagări, moduri MS și `<MS>`, benzi | ca în YA-MS |
| Sporadic-E | etichetele `< >` care exclud spotul, regula 144 MHz, regula transecuatorială | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, salt scurt 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | cuvinte F.A.I., benzi, perioada pe hartă | FAI, toate benzile, 24 de ore |
| Aurora | etichete `< >` de aurora, benzi, perioada pe hartă | `AU` `AUR` (cu sau fără `< >`), toate benzile, 2 ore |
| Test filtre | lipiți o linie de cluster și vedeți cum o clasifică toate filtrele | |

Butonul **Restabilește regulile autorului** readuce valorile implicite.

## CALL3

Baza de date JTDX **indicativ → locator** (peste un milion de indicative) în `data/call3.txt`. Butoanele **Reîncarcă** și **Importă un CALL3.TXT nou**. Locatoarele de 6 caractere citite în comentariile spoturilor sunt adăugate în `data/call3_nuovi.txt`.

## Sub-filele Spot și Date de propagare

Fila are două sub-file: **Spot** (sesiuni, spoturi colectate, CALL3 și coloana de filtre) și **Date de propagare**:

- **N0NBH** (hamqsl.com): banner VHF și date solare, E-skip Europa și America de Nord (DXrobot), aurora.
- **Bz pentru aurora**: vântul solar în timp real NOAA SWPC (DSCOVR/ACE, punctul L1), cea mai bună sursă pentru Bz.
- **Meteor scatter**: activitatea meteorică zilnică (RMOB) de la Make More Miles on VHF.

Se descarcă cel mult o dată pe oră. Rezumatul apare pe hărțile SPORADIC-E, F.A.I. și AURORA. **Meniul continentelor**, în dreapta barei de file, mută hărțile și selectează datele acelui continent.
