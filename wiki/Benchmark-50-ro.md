# BENCHMARK 50 MHz

[← Home](Home-ro)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Banc de probă pentru **E sporadic cu salturi multiple pe 50 MHz** folosind logul ADIF al unei stații și, opțional, al unei a doua stații.

## Încărcarea logurilor

- **Stația 1**: *Încarcă ADIF* / *Descarcă ADIF*. Programul preia din log `STATION_CALLSIGN` și `MY_GRIDSQUARE`; poziția de emisie este `MY_GRIDSQUARE` al fiecărui QSO.
- **Stația 2** (opțională): aceleași butoane pe al doilea rând; caseta *Stația 2* o afișează sau o ascunde. Traseele stației 1 sunt albastru deschis, cele ale stației 2 violet.
- Programul folosește QSO-urile pe 50 MHz cu locator (locatoarele lipsă sau de 4 caractere se iau din CALL3), cu salturi multiple: peste 3100 km (regula autorului; numărul de salturi urmează tabelul IK3XTV).
- Legăturile **transecuatoriale** sunt excluse (regula autorului): spre Africa și America de Sud, cu punctul de mijloc în banda de înclinație magnetică între -20° și +20°, cu excepția cazului în care logul are `PROP_MODE=ES`.

## Zi și oră

Alegeți **ziua** (la deschidere, cea cu cele mai multe QSO-uri cu salturi multiple) și ora **de la… la…** UTC, în pași de 15 minute. Caseta **foEs** (opțională) colorează traseele după foEs minim și îl afișează în tabel.

## Harta

Hartă **Mercator** cu centru liber: rotiță = zoom, tragere = deplasare.

![Path](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Mouse lângă un traseu**: linia și pictogramele se măresc și apare **pătratul WWL de 6 caractere** al stației DX.
- **Clic stânga** pe acel traseu: o fereastră întreabă dacă să afișeze **toate QSO-urile din fișier** (toate zilele) cu poziția capătului îndepărtat **pe o rază de 150 km** de acel punct. *Da*: harta se golește și arată acele QSO-uri în interiorul cercului portocaliu; *Nu*: harta rămâne așa cum este. *Înapoi la zi și oră* revine la selecția anterioară.

![Surroundings](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detalii

Pentru traseul ales: distanța și evaluarea după tabelul IK3XTV, azimutul în ambele direcții, salturi egale cu pătratele punctelor de reflexie, primul salt scurt în limita a 1200 km (experiența IC8TEM) și, cu caseta foEs bifată, foEs minim.
