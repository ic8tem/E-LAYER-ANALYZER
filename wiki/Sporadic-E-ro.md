# SPORADIC-E

[← Home](Home-ro)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Harta

Un buton deasupra hărții alege vederea:

- **Azimutală**: hartă azimutală echidistantă centrată pe stația ta, cu cursorul de **zoom** (de la 300 la 20000 km de la centru, și cu rotița mouse-ului).
- **Mercator**: harta lumii, cu butoanele Europa și Lume.

În ambele proiecții se desenează în jurul stației **cercurile distanțelor lui Flavio IK3XTV**: 500 km (minimul saltului unic), 1500-2000 km (optim), 2400 km (maximul saltului unic), 5000 km (maximul saltului dublu) și 6900 km (maximul saltului triplu).

În vederile azimutale inelele punctate arată **km de la centru** (pasul se schimbă cu zoomul), marginea indică distanța maximă afișată, iar cursorul mouse-ului dă distanța și azimutul de la centru.

![Multi-salt](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ce arată

- **Spoturile** din ultimele minute (15 min - 12 h) pe benzile **28, 40 (8 m, dezactivată implicit), 50, 70 și 144 MHz**, preluate din sesiunile telnet. Locatorii provin din comentariul spotului (ex. `JN70CN<ES>JO62QM`) sau din arhiva CALL3.
- Spoturile cu **<MS>**, **<AU>** sau **<TR>** în text sunt **respinse**: nu privesc E-sporadic, dar sunt utile pentru meteor scatter, auroră sau tropo.
- **Transecuatorial, regula autorului (IC8TEM)**: spre Africa și America de Sud, dacă punctul median cade în banda de înclinație magnetică între -20° și +20°, legătura este transecuatorială (TEP) și nu E-sporadic, cu excepția cazului în care spotul declară `<ES>`.
- **144 MHz, regula autorului (IC8TEM)**: E-sporadic pe 2 m începe **de la peste 900 km** (spoturile mai scurte sunt respinse). Un singur spot pe 144 MHz **nu se desenează singur**: apare când în **5 minute** sosește un al doilea spot pe 144 MHz cu norul în același loc (puncte de reflexie la cel mult 300 km) sau când în apropiere există un spot pe **50 MHz cu salt scurt** (până la 1200 km, în ultimele 15 minute). Bara de sus arată câte spoturi pe 144 MHz așteaptă confirmarea.
- Pentru fiecare spot: distanța, **numărul de salturi** conform tabelului **IK3XTV**, elevația, **foEs minim** (legea secantei).
- **Primul salt al ambelor stații**: pătrat portocaliu = primul salt al spotterului, violet = primul salt al DX-ului (salturi egale); galben = punctul median al unui salt unic.
- **Primul salt scurt** (cercuri goale): în deschiderile multi-salt autorul a constatat un prim salt scurt până la **1200 km** de propriul QTH; lungimea poate fi setată.
- **Nori Es** pe **pătratele Maidenhead** (2° x 1°), colorați după foEs minim: albastru < 5, bleu < 7, verde < 10, galben < 14, portocaliu < 20, roșu > 20 MHz.
- Tabelul **Nori Es**: pentru fiecare pătrat, **MUF-ul pentru stația ta** (roșu = 144 MHz posibil, portocaliu = 70, galben = 50).

## Tabelul IK3XTV

| | minim | optim | maxim |
|---|---|---|---|
| Salt unic | 500-650 km | 1500-2000 km | 2200-2400 km |
| Salt dublu | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Salt triplu | – | 4800-6400 km | 6900 km |

## Cum se folosește

- **Clic pe un spot**: traseul este evidențiat, cu punctele de reflexie, locatorii și azimuturile în detalii.
- **Clic pe un spot** în tabel: traseul se luminează pe hartă (halou galben) și, dacă nu este în vedere, harta se mută sau își mărește raza pentru a-l arăta. Spotul rămâne selectat când sosesc spoturi noi.
- **Tabelul urmează harta**: cu un zoom setat, tabelul spoturilor și tabelul norilor arată doar ce este vizibil pe hartă.
- **Clic pe hartă**: punctul este tratat ca posibil nor Es, cu foEs necesar pentru 28, 50, 70 și 144 MHz.

## Notă despre 432 MHz

E-sporadic **nu se aplică la 432 MHz**: MUF-ul Es ajunge la circa 150-200 MHz, iar recordurile ARRL indică legături Es doar pe 50 și 144 MHz. Spoturile pe 70 cm nu sunt analizate aici.

## Pătrate colorate după frecvență și MUF care clipește

Pătratele Maidenhead ale punctelor de reflexie sunt colorate după cea mai mare frecvență a spoturilor: **verde** până la 29 MHz, **galben** până la 51, **portocaliu** până la 71, **roșu** până la 145 MHz. Din foEs al pătratului se estimează MUF-ul probabil pe cel mai lung salt (2400 km, tabelul IK3XTV): dacă acesta atinge o bandă mai înaltă, pătratul **clipește** cu culoarea acelei benzi (principiul lui IK3XTV și Jacobs-Leinwoll: skip scurt înseamnă MUF ridicat).

În dreapta sus pe hartă apare sinteza N0NBH (E-skip pentru continentul ales, SFI, indicii K și A).

## Reguli pe bandă (versiunea 0.4.18)

- **50 MHz**: în limita a 450 km nu este E-sporadic. Spoturile FT8 și FT4 sunt E-sporadic doar dacă fac parte dintr-un nor: un alt spot pe 50 MHz în 4 minute cu punctul median în limita a 150 km. Un spot FT8/FT4 izolat merge la meteor scatter.
- **70 MHz**: în limita a 450 km nu este E-sporadic. FT8 și FT4 merg la E-sporadic chiar și în fereastra meteor scatter din bandplan (decide întâi modul cunoscut, apoi frecvența).
- **40 MHz (8 m)**: distanța minimă obținută din cei 450 km de pe 50 MHz cu legea secantei (circa 330 km); doar distanțele din primul salt al geometriei stratului E (circa 2440 km). Culoarea traseului: galben.
- **144 MHz**: E-sporadic de la 900 km, cu confirmare; spoturile Q65 și JT65 sunt EME și sunt respinse, cu excepția `<ES>` în text.
- G0ISW (Propagation identification, 2024) vede cel mai scurt E-sporadic peste 400 km pe 50 și 70 MHz și peste 1400 km pe 144 MHz: regulile autorului concordă pe 50 și 70 MHz și sunt mai largi pe 144 MHz, dar cer o confirmare.
- Ordinea în care se aplică toate regulile, cu valorile în uz, este afișată în **[SETĂRI](Settings-ro)** › Distribuirea spoturilor.
