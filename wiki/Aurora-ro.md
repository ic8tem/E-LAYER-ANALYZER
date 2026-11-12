# AURORA

[← Home](Home-ro)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Ovalul auroral** NOAA OVATION Prime (estompat), **Kp** din ultimele 3 zile, **vântul solar** (Bz, viteză, densitate), actualizate la fiecare 5 minute și salvate în `data/swpc`.
- Aceleași vederi ca la F.A.I.: **Scatter trail** pentru o pereche de stații și **Izo-reflexie** pentru verificarea unui punct, cu o altitudine implicită de 110 km.
- **Orientarea recomandată a antenei pentru ambele stații**: de-a lungul liniei de perpendicularitate a fiecărei stații (puncte în culoarea ei), punctul cu cea mai mare probabilitate de oval. Regulă practică (G3WPO): începeți de la circa 45°, între nord și est.
- Exemplu IO70VJ - JO59BC: scatter point JP01AK în largul insulelor Shetland, abatere 0,97°, QTF 11° și 299°.
- **Spoturi de auroră pe hartă**: spoturile cu **<AU>** sau **<AUR>** în text sunt spoturi de auroră (regula autorului). Cu caseta *Spoturi <AU>/<AUR> pe hartă cu calculul* bifată, fila desenează spoturile din ultimele 2 ore (cel mult 30): pentru fiecare pereche spotter-DX calculează scatter trail și arată scatter point-ul cel mai probabil (cerc portocaliu), cu QTF și elevația ambelor stații în rezultate.
- Pe lângă scatter point-ul cel mai probabil, rezultatele dau **intervalul de QTF** al ambelor stații de-a lungul întregului Scatter trail (abatere ≤ 1,5°).
- **Fereastra Lange-Hesse**: scatter point-ul este preferat la 370-1100 km de fiecare stație, fereastra calculată de Lange-Hesse la 54° latitudine geomagnetică. Din versiunea 0.4.5 fereastra se deplasează după înclinația IGRF-14 a fiecărei stații: mai aproape pentru stațiile mai la sud, mai departe pentru cele mai la nord (de ex. Capri circa 260-990 km, Oslo 470-1200 km). Rezultatele arată fereastra ambelor stații.

## BENCHMARK AURORA

Fila AURORA are două sub-file: **REAL-TIME** (calculul descris mai sus) și **BENCHMARK AURORA**, un banc de probă al modelului pe **298 de QSO-uri de auroră reale** cu oră și locator: logurile publicate ale lui EI5FK (IO51RT, Irlanda, 2003-2005, 144 și 432 MHz) și harta logului lui PA0O (JO33HG, Țările de Jos).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Pentru fiecare QSO programul calculează scatter point-ul cu același model (IGRF-14, altitudine 110 km, refracție 4/3, toleranță 3°).
- Culori: **verde** = scatter point în fereastra Lange-Hesse a ambelor stații; **galben** = explicat, dar în afara acestei ferestre; **portocaliu** = QSO de peste 2000 km; **roșu** = niciun scatter point, QSO neexplicat de model.
- Rezumatul dă procentele, și cu fereastra fixă de 370-1100 km pentru comparație, și, pentru fiecare log, intervalul și mediana QTF-ului calculat. Clic pe un rând pentru a desena traseul și cele două orientări ale antenelor.
- Rezultat (versiunea 0.4.4): **296 din 298 de QSO-uri explicate (99%)**, 243 (82%) în fereastra de 370-1100 km; QTF-ul median al lui EI5FK între 25° și 41°, adică nord-estul sugerat de G3WPO. Logurile nu indică orientarea antenei: verificarea se face pe geometrie, nu pe QTF. Cu fereastra mobilă (versiunea 0.4.5) rezultatul este același, 243 QSO-uri (82%), ca și cu fereastra fixă: stațiile bancului de probă (Irlanda, Marea Britanie, Țările de Jos, Germania) au o înclinație de 66-70°, apropiată de referința Lange-Hesse (fereastra lui EI5FK 350-1080 km); diferența privește stațiile mai la sud sau mai la nord, pentru care bancul de probă nu are QSO-uri.
