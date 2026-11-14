# F.A.I.

[← Home](Home-lv)

F.A.I. (Field Aligned Irregularities) ir izkliede no neregularitātēm, kas orientētas gar ģeomagnētisko lauku; tā raksturīga vasaras vakariem pēc sporādiskā E. Izkliedētais signāls aiziet pa **konusu**, kura ass ir vērsta gar lauku: divas stacijas var sazināties, ja tās atrodas uz **papildinošām kontūrām** (a1 + a2 = 180°, IC8FAX-IC8TEM raksts; K9SE ±c kontūras). Magnētiskais lauks tiek aprēķināts ar modeli **IGRF-14**.

## Skats Scatter trail (pāris A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Ievadi **Stacija A** un **Stacija B** (izsaukuma signāls tiek meklēts CALL3 un/vai lokators). Tukšs A = tava stacija.
- Karte rāda **scatter trail**: visus slāņa augstuma punktus, ko redz abas stacijas un kas atbilst atstarošanas nosacījumam 2,5° robežās (klases 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Lodziņš augšā pa labi rāda A (sarkans) un B (zaļš) **QTF un elevāciju** uz katru punktu.
- Programma iesaka **varbūtīgāko scatter point** (minimālā novirze, īsākā trase).
- **Refrakcija 4/3**: noklusējumā ieslēgta, tā paplašina horizontus.

## Skats Izoatstarošana (izoatstarošanas līknes)

![Izoatstarošana](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Tā ir ģeometriska konstrukcija, ko izmanto, lai **pārbaudītu, vai atstarošanas punkts ir pareizs**.

- **Kreisais klikšķis** uz kartes: izvēlas pārbaudāmo punktu. **Labais klikšķis**: pārvieto staciju B.
- **Pārbaudīt ieteikto punktu**: atver grafiku programmas ieteiktajā punktā.
- Kontūrlīnijas pie **0, ±1, ±2, ±4, ±8, ±16°**: krāsainas joslas tumšajās shēmās, **biezas līnijas** gaišajās shēmās.
- Pāra vērtējums: **apmierinošs** līdz 1°, **pieņemams** līdz 2,5°, **neapmierinošs** virs tā.
- Ja punkts ir zem kādas stacijas horizonta, parādās uznirstošais logs **Aprēķins nav iespējams**, un nekas vairāk.

## Pārbaudes

| Sakars | Scatter point | Novirze | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (virs Dženovas) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (DR Vācija, 28 km no Briseles) | 0,88° | 338° / 87° |
| Bāka DB0FAI (305°) - Drēzdene caur Šefīldu (DL5MCG; Drēzdene tēmēja uz 290° un pieteica auroru) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, 1990. gada IGRF lauks) | JN45MV, sliede 40 km no Ženēvas (Ženēva: 4,10°) | 0,76° | 326° / 85° |
| DL5MCG - SV1DH (1987. gada tests caur Budapeštu, 1569 km, 1987. gada IGRF lauks) | Budapešta pie 2,53°, nedaudz virs 2,5° (2,16° ar šodienas lauku); sliede 26 km attālumā; varbūtīgākais KN06RV | 2,53° (KN06RV 0,93°) | 95° / 341° |

Kopš versijas 0.4.18 abi gadījumi ar datumu (1987 un 1990) izmanto tā gada IGRF-14 lauku, kamēr stacijas ir gadījuma stacijas.

Sarakstā esošie zināmie scatter point: Ženēva, Brisele, Budapešta, JN92, JN41/42.

Izvēlne **Pārbaudes...** ielādē testa pārus IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE un DL5MCG-SV1DH.

**Aprīkojums** (DL5MCG): F.A.I. 144 MHz joslā vajag vismaz garu yagi un 100 W (ar SV1DH strādāja arī ar 8 elementu yagi un 50 W). Vērs antenu uz programmas norādīto scatter point, nevis uz otru staciju.
