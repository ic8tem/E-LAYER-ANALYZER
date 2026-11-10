# BENCHMARK Es

[← Home](Home-ro)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Banc de probă pentru **E sporadic** și **salturi multiple** pe un **întreg log ADIF**, chiar și al altei stații: din QSO-urile efectiv realizate, programul schițează skip-ul și norii, zi de zi și oră de oră.

## Încărcarea logului

- **Încarcă ADIF**: alege fișierul `.adi`/`.adif`. Programul ține minte fișierul și îl reîncarcă la pornire.
- **Descarcă ADIF**: elimină logul din filă.
- **Stația din centru**: indicativul și locatorul stației care a făcut logul. Programul propune `STATION_CALLSIGN` (sau `OPERATOR`) și `MY_GRIDSQUARE` cel mai frecvent; pot fi schimbate manual și confirmate cu **Aplică**. Centrul nu depinde de setările generale, deci se pot studia și logurile altor stații.
- Poziția de emisie: `MY_GRIDSQUARE` al fiecărui QSO (dacă lipsește, locatorul din centru). Pozițiile de recepție: câmpurile `GRIDSQUARE`.
- QSO-uri folosite: 28, 50, 70 și 144 MHz cu locator. QSO-urile cu alt `PROP_MODE` (TR, MS, FAI, AUR, EME...) și cele sub 500 km pot fi excluse. 432 MHz nu este inclus niciodată: E sporadic nu se aplică.

## Zile și ore

- **QSO-uri pe zi**: ziua, numărul de QSO-uri, benzile, distanța maximă, salturile multiple. La deschidere, programul propune ziua cu cele mai multe QSO-uri.
- **Orele zilei**: rândul *Toată ziua* și câte un rând pentru fiecare oră UTC; un clic arată doar QSO-urile din acea oră.
- **QSO**: lista pentru selecție; un clic pe un QSO (sau lângă stația DX pe hartă) îl evidențiază și îi arată detaliile.

## Harta

Hartă **azimutală echidistantă** centrată pe stație, cu cursorul **Distanța maximă** pentru zoom (și cu rotița mouse-ului): cercurile arată **km de la centru**.

- **Skip pe sectoare** (10° de azimut): arc portocaliu = cel mai apropiat QSO din sector (marginea zonei de skip), arc verde = cel mai îndepărtat QSO, între ele zona atinsă.
- **Nori Es** pe pătratele Maidenhead, colorați după foEs estimat.
- Punctul de mijloc al salturilor simple (galben), primul salt al stației (portocaliu, cerc gol = primul salt scurt), primul salt al stației DX (violet).
- Pentru toată ziua, **linia roșie** unește oră de oră centroidul punctelor de mijloc: este deriva norilor.

![One hour](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Calcule propuse

1. **Skip pe sectoare**: pentru fiecare sector distanța de skip și raza de acoperire, cu indicativele.
2. **foEs estimat** cu legea secantei (foEs ≥ f / sec φ): cel mai scurt salt simplu pe fiecare bandă dă estimarea cea mai mare; cu acest foEs, MUF-ul așteptat la 1000, 1500, 2000 și 2400 km și distanța dincolo de care s-ar deschide 144 MHz.
3. **Nori Es** estimați pe pătrate, cu numărul de QSO-uri și foEs.
4. **Salturi multiple**: primul salt al stației în cele două ipoteze (salturi egale și prim salt scurt în limita a 1200 km, experiența IC8TEM) comparat cu norii văzuți în salt simplu în intervalul de ±30 de minute; în limita a 300 km ipoteza este confirmată. Programul numără confirmările fiecărei ipoteze.
5. **Deriva norilor** oră de oră: km, direcție și viteză.

Calculele sunt orientative: un QSO spune că traseul era deschis, nu exact unde era norul. Cu cât datele sunt mai curate (locatoare de 6 caractere, `PROP_MODE` corect), cu atât skip-ul este mai fiabil.
