# SETĂRI

[← Home](Home-ro)

Din versiunea 0.4.18 fila **SETĂRI** reunește opțiunile generale ale programului și distribuirea spoturilor între file. Are două sub-file.

## Generale

| Secțiune | Ce se setează |
|---|---|
| Stație | Indicativul, locatorul și altitudinea, aceleași ca în bara de sus; **Aplică** le confirmă pentru toate filele. |
| Aspect | Limba, schema de culori și **dimensiunea fontului întregului program** (85-140 %, ca în YA-MS). Limba și culorile reconstruiesc fereastra: setările și spoturile se păstrează. |
| Hărți și date de prognoză | Continentul, același meniu ca în bara filelor. |
| Ora | Starea NTP și diferența dintre ora serverelor și ceasul PC-ului. |
| Actualizări | Verificarea pe GitHub la pornire și la fiecare 6 ore, actualizarea automată și silențioasă, butonul **Verifică acum**. Actualizările automate nu sunt valabile pentru pachetele de sistem (.deb, .rpm, Arch, Slackware). |
| Date | Butonul care deschide dosarul `data`. Cheia API a Bureau of Meteorology rămâne în DATA COLLECTION › Date de propagare. |

## Distribuirea spoturilor

În stânga un tabel enumeră regulile cu care programul decide unde merge fiecare spot, **în ordinea în care se aplică**: prima regulă care se potrivește decide fila. Valorile afișate sunt cele setate în filtre.

| # | Dacă spotul... | merge la |
|---|---|---|
| 1 | este în afara filtrului local (sub 27990 kHz sau peste 145 MHz; 430-440 MHz doar auroră) | respins |
| 2 | conține AUE, AU-E sau AURORAL E | AURORA (aurora-E) |
| 3 | conține un cuvânt de auroră (implicit AU, AUR) | AURORA |
| 4 | conține un cuvânt F.A.I. (implicit FAI; benzile 6, 4 și 2 m: F.A.I. nu ajunge la 432 MHz) | F.A.I. |
| 5 | are un mod de meteor scatter sau `<MS>` | METEOR SCATTER |
| 6 | cade într-o fereastră MS din bandplan (neexclusivă pe 50 MHz; FT8 și FT4 pe 50 și 70 MHz excluse) | METEOR SCATTER |
| 7 | este FT8 sau FT4 pe 50 MHz fără alt spot în 4 min cu punctul median în limita a 150 km | meteor scatter (nu E-sporadic) |
| 8 | este Q65 sau JT65 pe 144 MHz (cu excepția `<ES>`) | EME: respins |
| 9 | conține un cuvânt de altă propagare | nu E-sporadic |
| 10 | este transecuatorial | nu E-sporadic (TEP) |
| 11-15 | este pe 28, 40, 50, 70 sau 144 MHz în limitele regulilor de distanță | SPORADIC-E |
| 16 | nu se încadrează în nicio regulă (tropo, 432 MHz, HF...) | nicio filă (rămâne în Spoturi colectate) |

În dreapta se află **filtrul local** și filtrele fiecărui mod, în cinci sub-file: Meteor scatter (filtrul YA-MS se setează în fila METEOR SCATTER), E-sporadic, F.A.I., Auroră și **Testul filtrelor**, unde lipești un rând din cluster și vezi cum este clasificat. Butonul **Restabilește regulile autorului** readuce valorile inițiale.
