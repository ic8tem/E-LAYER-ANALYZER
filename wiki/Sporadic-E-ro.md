# SPORADIC-E

[← Home](Home-ro)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Harta

Un buton deasupra hărții alege vederea:

- **Azimutală**: hartă azimutală echidistantă centrată pe stația dumneavoastră, cu cursorul de **zoom** (de la 300 la 20000 km de la centru, și cu rotița mouse-ului).
- **Mercator**: harta lumii, cu butoanele Europa și Lume.

În ambele proiecții sunt desenate în jurul stației **cercurile de distanță ale lui Flavio IK3XTV**: 500 km (minimul saltului simplu), 1500-2000 km (optim), 2400 km (maximul saltului simplu), 5000 km (maximul saltului dublu) și 6900 km (maximul saltului triplu).

În vederile azimutale cercurile punctate arată **km de la centru** (pasul se schimbă cu zoomul), marginea marchează distanța maximă afișată, iar cursorul mouse-ului dă distanța și azimutul de la centru.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ce arată

- **Spoturile** din ultimele minute (15 min - 12 h) pe benzile de **28, 50, 70 și 144 MHz**, preluate din sesiunile telnet. Locatoarele provin din comentariul spotului (de ex. `JN70CN<ES>JO62QM`) sau din baza de date CALL3.
- Spoturile cu **<MS>**, **<AU>** sau **<TR>** în text sunt **eliminate**: nu privesc E sporadic, dar sunt utile pentru meteor scatter, aurora sau tropo.
- **Transecuatorial, regula autorului (IC8TEM)**: spre Africa și America de Sud, dacă punctul de mijloc cade în banda de înclinație magnetică între -20° și +20°, legătura este transecuatorială (TEP) și nu E sporadic, cu excepția cazului în care spotul indică `<ES>`.
- **144 MHz, regula autorului (IC8TEM)**: E sporadic pe 2 m începe **dincolo de 900 km** (spoturile mai scurte sunt eliminate). Un singur spot pe 144 MHz **nu este desenat de unul singur**: apare când în **5 minute** sosește un al doilea spot pe 144 MHz cu norul în același loc (puncte de reflexie în limita a 300 km), sau când în apropiere există un spot cu **salt scurt pe 50 MHz** (în limita a 1200 km, în ultimele 15 minute). Bara de sus arată câte spoturi pe 144 MHz așteaptă confirmarea.
- Pentru fiecare spot: distanța, **numărul de salturi** după tabelul **IK3XTV**, elevația, **foEs minim** (legea secantei).
- **Primul salt al ambelor stații**: pătrat portocaliu = primul salt al spotterului, violet = primul salt al DX-ului (salturi egale); galben = punctul de mijloc al unui salt simplu.
- **Primul salt scurt** (cercuri goale): în deschiderile cu salturi multiple autorul a constatat un prim salt scurt în limita a **1200 km** de propriul QTH; lungimea poate fi setată.
- **Nori Es** pe **pătratele Maidenhead** (2° x 1°), colorați după foEs minim: albastru < 5, albastru deschis < 7, verde < 10, galben < 14, portocaliu < 20, roșu > 20 MHz.
- Tabelul **norilor Es**: pentru fiecare pătrat, **MUF-ul pentru stația dumneavoastră** (roșu = 144 MHz posibil, portocaliu = 70, galben = 50).

## Tabelul IK3XTV

| | minim | optim | maxim |
|---|---|---|---|
| Salt simplu | 500-650 km | 1500-2000 km | 2200-2400 km |
| Salt dublu | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Salt triplu | – | 4800-6400 km | 6900 km |

## Cum se folosește

- **Clic pe un spot**: traseul este evidențiat, cu punctele de reflexie, locatoarele și azimuturile în detalii.
- **Clic pe un spot** în tabel: traseul se aprinde pe hartă (strălucire galbenă) și, dacă este în afara vederii, harta se deplasează sau își lărgește raza pentru a-l arăta. Spotul rămâne selectat când sosesc spoturi noi.
- **Tabelul urmează harta**: cu un zoom setat, tabelul spoturilor și tabelul norilor arată doar ce este vizibil pe hartă.
- **Clic pe hartă**: punctul este tratat ca un posibil nor Es, cu foEs necesar pentru 28, 50, 70 și 144 MHz.

## Notă despre 432 MHz

E sporadic **nu se aplică pe 432 MHz**: MUF-ul Es ajunge la circa 150-200 MHz, iar recordurile ARRL enumeră legături Es doar pe 50 și 144 MHz. Spoturile pe 70 cm nu sunt analizate aici.

## Pătrate colorate după frecvență și MUF intermitent

Pătratele Maidenhead ale punctelor de reflexie sunt colorate după cea mai mare frecvență a spotului: **verde** până la 29 MHz, **galben** până la 51, **portocaliu** până la 71, **roșu** până la 145 MHz. Din foEs-ul pătratului se estimează MUF-ul probabil pe saltul cel mai lung (2400 km, tabelul IK3XTV): dacă atinge o bandă mai înaltă, pătratul **clipește** în culoarea acelei benzi (principiul lui IK3XTV și Jacobs-Leinwoll: skip scurt înseamnă MUF ridicat).

În dreapta sus a hărții apare rezumatul N0NBH (E-skip pentru continentul ales, SFI, indicii K și A).
