# BENCHMARK 50 MHz

[← Home](Home-ro)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Banc de probă pentru **E-sporadic multi-salt pe 50 MHz** folosind logul ADIF al unei stații și, opțional, al unei a doua stații.

## Încărcarea logurilor

- **Stația 1**: *Încarcă ADIF* / *Descarcă ADIF*. Programul preia `STATION_CALLSIGN` și `MY_GRIDSQUARE` din log; poziția de emisie este `MY_GRIDSQUARE` al fiecărui QSO.
- **Stația 2** (opțional): aceleași butoane pe al doilea rând; caseta *Stația 2* o arată sau o ascunde. Traseele stației 1 sunt bleu, cele ale stației 2 violet.
- Programul folosește QSO-urile pe 50 MHz cu locator (locatorii lipsă sau cu 4 caractere se iau din CALL3), multi-salt: peste 3100 km (regula autorului; numărul de salturi urmează tabelul IK3XTV).
- Legăturile **transecuatoriale** sunt excluse (regula autorului): spre Africa și America de Sud, cu punctul median în banda de înclinație magnetică între -20° și +20°, cu excepția cazului în care logul are `PROP_MODE=ES`.

## Ziua și ora

Alege **ziua** (la deschidere, cea cu cele mai multe QSO-uri multi-salt) și intervalul orar **de la… până la…** UTC, în pași de 15 minute. Caseta **foEs** (opțională) colorează traseele după foEs minim și îl afișează în tabel.

## Harta

Hartă **Mercator** cu centru liber: rotița = zoom, tragere = deplasare.

![Traseu](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- Un **punct galben estompat** marchează centrul fiecărui salt, unde se presupune că se află norul Es; devine mai mare pe traseul de sub mouse.
- **Mouse lângă un traseu**: linia și pictogramele se măresc și apare **pătratul WWL cu 6 caractere** al stației DX.
- **Clic stânga** pe acel traseu: o fereastră popup întreabă dacă să fie afișate **toate QSO-urile din fișier** (toate zilele) cu poziția terminală **la cel mult 150 km** de acel punct. *Da*: harta se golește și arată acele QSO-uri în cercul portocaliu; *Nu*: harta rămâne neschimbată. *Înapoi la zi și interval orar* revine la selecția anterioară.

![Împrejurimi](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detalii

Pentru traseul ales: distanța și încadrarea în tabelul IK3XTV, azimutul în ambele direcții, salturile egale cu pătratele punctelor de reflexie, primul salt scurt până la 1200 km (experiența IC8TEM) și, cu caseta foEs, foEs minim.
