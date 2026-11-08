# SPORADIC-E

[← Home](Home-lv)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Karte

Poga virs kartes izvēlas skatu:

- **Azimutālā - viens lēciens**: azimutāla ekvidistanta karte ar centru tavā stacijā, fiksēts rādiuss 2500 km, ar IK3XTV tabulas zonām (500 km minimālais, 1500-2000 km optimālais, 2400 km maksimālais viena lēciena attālums).
- **Azimutālā - maksimālais attālums (vairāklēcienu)**: tā pati karte ar slīdni **Maksimālais attālums** (no 300 līdz 20000 km, arī ar peles ritenīti); parādās arī dubultā (5000 km) un trīskāršā lēciena (6900 km) robežas, lai varētu sekot vairāklēcienu sakaru attīstībai.
- **Taisnstūra**: iepriekšējā pasaules karte ar pogām Eiropa un Pasaule.

Azimutālajos skatos raustītie gredzeni rāda **km no centra** (solis mainās līdz ar tālummaiņu), mala apzīmē maksimālo parādīto attālumu, un peles kursors rāda attālumu un azimutu no centra.

![Vairāklēcienu](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ko tā rāda

- Pēdējo minūšu (15 min - 12 h) **spotus** joslās **28, 50, 70 un 144 MHz**, ņemtus no telnet sesijām. Lokatori tiek ņemti no spota komentāra (piem. `JN70CN<ES>JO62QM`) vai no CALL3 arhīva.
- Spoti ar **<MS>**, **<AU>** vai **<TR>** tekstā tiek **atmesti**: tie neattiecas uz sporādisko E, bet noder meteor scatter, aurorai vai tropo.
- **Transekvatoriālie, autora likums (IC8TEM)**: virzienā uz Āfriku un Dienvidameriku, ja viduspunkts krīt magnētiskās inklinācijas joslā starp -20° un +20°, sakars ir transekvatoriāls (TEP), nevis sporādiskais E, ja vien spotā nav norādīts `<ES>`.
- **144 MHz, autora likums (IC8TEM)**: sporādiskais E 2 m joslā sākas **virs 900 km** (īsāki spoti tiek atmesti). Viens 144 MHz spots **netiek zīmēts atsevišķi**: tas parādās, kad **5 minūšu** laikā pienāk otrs 144 MHz spots ar mākoni tajā pašā vietā (atstarošanas punkti 300 km robežās) vai kad tuvumā ir **īsa lēciena 50 MHz** spots (1200 km robežās, pēdējās 15 minūtēs). Josla augšā rāda, cik 144 MHz spotu gaida apstiprinājumu.
- Katram spotam: attālums, **lēcienu skaits** pēc **IK3XTV** tabulas, elevācija, **minimālā foEs** (sekanses likums).
- **Abu staciju pirmais lēciens**: oranžs kvadrāts = spotera pirmais lēciens, violets = DX pirmais lēciens (vienādi lēcieni); dzeltens = viena lēciena viduspunkts.
- **Īss pirmais lēciens** (tukši apļi): vairāklēcienu atvēršanās laikā autors ir pieredzējis īsu pirmo lēcienu **1200 km** robežās no sava QTH; garumu var iestatīt.
- **Es mākoņi** **Maidenhead kvadrātos** (2° x 1°), iekrāsoti pēc minimālās foEs: zils < 5, gaiši zils < 7, zaļš < 10, dzeltens < 14, oranžs < 20, sarkans > 20 MHz.
- Tabula **Es mākoņi**: katram kvadrātam **MUF tavai stacijai** (sarkans = iespējami 144 MHz, oranžs = 70, dzeltens = 50).

## IK3XTV tabula

| | minimālais | optimālais | maksimālais |
|---|---|---|---|
| Viens lēciens | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dubults lēciens | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Trīskāršs lēciens | – | 4800-6400 km | 6900 km |

## Kā to lietot

- **Klikšķis uz spota**: trase tiek izcelta, detaļās redzami atstarošanas punkti, lokatori un azimuti.
- **Klikšķis uz spota** tabulā: trase kartē iedegas (dzeltens mirdzums), un, ja tā ir ārpus redzamās daļas, karte pārvietojas vai palielina rādiusu, lai to parādītu. Spots paliek izvēlēts, arī pienākot jauniem spotiem.
- **Tabula seko kartei**: ja iestatīta tālummaiņa, spotu tabula un mākoņu tabula rāda tikai to, kas redzams kartē.
- **Klikšķis uz kartes**: punkts tiek uzskatīts par iespējamu Es mākoni, ar foEs, kas vajadzīga 28, 50, 70 un 144 MHz.

## Piezīme par 432 MHz

Sporādiskais E **neattiecas uz 432 MHz**: Es MUF sasniedz ap 150-200 MHz, un ARRL rekordos Es sakari ir tikai 50 un 144 MHz. 70 cm spoti šeit netiek analizēti.
