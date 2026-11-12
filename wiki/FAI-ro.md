# F.A.I.

[← Home](Home-ro)

F.A.I. (Field Aligned Irregularities) este scatter-ul pe neregularități aliniate cu câmpul geomagnetic, tipic serilor de vară după E-sporadic. Semnalul difuzat pleacă pe un **con** a cărui axă este orientată de-a lungul câmpului: două stații pot lucra între ele dacă se află pe **contururi suplementare** (a1 + a2 = 180°, articolul IC8FAX-IC8TEM; contururile ±c ale lui K9SE). Câmpul magnetic este calculat cu modelul **IGRF-14**.

## Vederea Scatter trail (perechea A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Introdu **Stația A** și **Stația B** (indicativ căutat în CALL3 și/sau locator). A lăsat gol = propria stație.
- Harta arată **scatter trail**: toate punctele de la altitudinea stratului, văzute de ambele stații, care satisfac condiția de reflexie în limita a 2,5° (clasele 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Caseta din dreapta sus arată **QTF și elevația** lui A (roșu) și B (verde) spre fiecare punct.
- Programul propune **scatter point-ul cel mai probabil** (abatere minimă, traseu cel mai scurt).
- **Refracție 4/3**: activă implicit, extinde orizonturile.

## Vederea Izo-reflexie (isoflection loci)

![Izo-reflexie](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Este construcția geometrică folosită pentru a **verifica dacă un punct de reflexie este corect**.

- **Clic stânga** pe hartă: alegi punctul de verificat. **Clic dreapta**: mută stația B.
- **Verifică punctul recomandat**: deschide diagrama pe punctul propus de program.
- Curbe de nivel la **0, ±1, ±2, ±4, ±8, ±16°**: benzi colorate în schemele întunecate, **linii groase** în schemele luminoase.
- Verdictul asupra perechii: **satisfăcător** în limita a 1°, **acceptabil** în limita a 2,5°, **nesatisfăcător** peste.
- Dacă punctul este sub orizontul unei stații, apare fereastra popup **Calcul imposibil** și nimic altceva.

## Verificări

| Legătură | Scatter point | Abatere | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (deasupra Genovei) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (sud-vestul Germaniei, la 28 km de Bruxelles) | 0,88° | 338° / 87° |
| Beaconul DB0FAI (305°) - Dresda via Sheffield (DL5MCG; Dresda avea antena la 290° și a declarat auroră) | – | 0,3° | – |
| DL5MCG - SV1DH (testul din 1987 via Budapesta, 1569 km) | Budapesta pe scatter trail; celule sub 1° de la 35 km | 2,16° | 95° / 341° |

Scatter point-uri cunoscute în listă: Geneva, Bruxelles, Budapesta, JN92, JN41/42.

Meniul **Verificări...** încarcă perechile de test IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE și DL5MCG-SV1DH.

**Echipament** (DL5MCG): F.A.I. pe 144 MHz necesită cel puțin o yagi lungă și 100 W (SV1DH a fost lucrat și cu o yagi cu 8 elemente și 50 W). Orientați antena spre scatter point-ul indicat de program, nu spre corespondent.
