# F.A.I.

[← Home](Home-ro)

F.A.I. (Field Aligned Irregularities) este împrăștierea pe neregularități aliniate cu câmpul geomagnetic, tipică serilor de vară după E sporadic. Semnalul împrăștiat pleacă pe un **con** a cărui axă este de-a lungul câmpului: două stații pot lucra între ele dacă se află pe **curbe suplementare** (a1 + a2 = 180°, articolul IC8FAX-IC8TEM; curbele ±c ale lui K9SE). Câmpul magnetic este calculat cu modelul **IGRF-14**.

## Vederea Scatter trail (perechea A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Introduceți **Stația A** și **Stația B** (indicativ căutat în CALL3 și/sau locator). A lăsată goală = propria stație.
- Harta arată **scatter trail-ul**: toate punctele la altitudinea stratului, văzute de ambele stații, care îndeplinesc condiția de reflexie în limita a 2,5° (clase 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Caseta din dreapta sus arată **QTF-ul și elevația** lui A (roșu) și B (verde) spre fiecare punct.
- Programul sugerează **punctul de împrăștiere cel mai probabil** (abatere minimă, traseu cel mai scurt).
- **Refracție 4/3**: activă implicit, extinde orizonturile.

## Vederea Izoreflexie (curbe de izoreflexie)

![Isoflection](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Este construcția geometrică folosită pentru a **verifica dacă un punct de reflexie este corect**.

- **Clic stânga** pe hartă: alegeți punctul de verificat. **Clic dreapta**: mută stația B.
- **Verifică punctul recomandat**: deschide diagrama pe punctul sugerat de program.
- Curbe de nivel la **0, ±1, ±2, ±4, ±8, ±16°**: benzi colorate în schemele întunecate, **linii groase** în schemele luminoase.
- Verdict asupra perechii: **satisfăcător** în limita a 1°, **acceptabil** în limita a 2,5°, **nesatisfăcător** dincolo.
- Dacă punctul este sub orizontul uneia dintre stații, apare fereastra **Calcul imposibil** și nimic altceva.

## Verificări

| Legătură | Punct de împrăștiere | Abatere | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (deasupra Genovei) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (SV Germaniei, la 28 km de Bruxelles) | 0,88° | 338° / 87° |
| DB0FAI - Dresda via Sheffield (DL5MCG) | – | 0,3° | – |

Puncte de împrăștiere cunoscute din listă: Geneva, Bruxelles, Budapesta, JN92, JN41/42.
