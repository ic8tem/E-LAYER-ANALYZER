# F.A.I.

[← Home](Home-it)

Il F.A.I. (Field Aligned Irregularities) è lo scatter da irregolarità allineate al campo geomagnetico, tipico delle sere d'estate dopo l'E-sporadico. Il segnale diffuso esce su un **cono** con asse lungo il campo: due stazioni si collegano se stanno su **contorni supplementari** (a1 + a2 = 180°, articolo IC8FAX-IC8TEM; contorni ±c di K9SE). Il campo magnetico è calcolato con il modello **IGRF-14**.

## Vista Scatter trail (coppia A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Scrivi **Stazione A** e **Stazione B** (nominativo cercato in CALL3 e/o locator). A vuoto = la tua stazione.
- La mappa mostra lo **scatter trail**: tutti i punti alla quota dello strato visti da entrambe le stazioni che soddisfano la condizione di riflessione entro 2,5° (classi 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Il riquadro in alto a destra mostra **QTF ed elevazione** di A (rosso) e B (verde) verso ogni punto.
- Il programma propone lo **scatter point più probabile** (deviazione minima, tratta più corta).
- **Rifrazione 4/3**: attiva di partenza, allunga gli orizzonti.

## Vista Iso-riflessione (isoflection loci)

![Iso-riflessione](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

È la costruzione geometrica per **verificare se un punto di riflessione è giusto**.

- **Clic sinistro** sulla mappa: scegli tu il punto da verificare. **Clic destro**: sposti la stazione B.
- **Verifica il punto consigliato**: apre il grafico sul punto proposto dal programma.
- Curve di livello **0, ±1, ±2, ±4, ±8, ±16°**: fasce colorate negli schemi scuri, **linee spesse** negli schemi chiari.
- Verdetto sulla coppia: **soddisfacente** entro 1°, **accettabile** entro 2,5°, **non soddisfacente** oltre.
- Se il punto è sotto l'orizzonte di una stazione compare il popup **Calcolo non possibile** e nient'altro.

## Verifiche

| Collegamento | Scatter point | Deviazione | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (sopra Genova) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (Germania SW, 28 km da Bruxelles) | 0,88° | 338° / 87° |
| Beacon DB0FAI (305°) - Dresda via Sheffield (DL5MCG; Dresda puntava a 290° e aveva dichiarato aurora) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, campo IGRF del 1990) | JN45MV, scia a 40 km da Ginevra (Ginevra: 4,10°) | 0,76° | 326° / 85° |
| DL5MCG - SV1DH (test del 1987 via Budapest, 1569 km, campo IGRF del 1987) | Budapest a 2,53°, appena oltre i 2,5° (2,16° con il campo di oggi); scia a 26 km; più probabile KN06RV | 2,53° (KN06RV 0,93°) | 95° / 341° |

Dalla versione 0.4.18 i due casi con una data (1987 e 1990) usano il campo IGRF-14 di quell'anno, finché le stazioni restano quelle del caso.

Scatter point noti nell'elenco: Ginevra, Bruxelles, Budapest, JN92, JN41/42.

Il menu **Verifiche...** carica le coppie di prova IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE e DL5MCG-SV1DH.

**Attrezzatura** (DL5MCG): il F.A.I. in 144 MHz richiede almeno una yagi lunga e 100 W (SV1DH è stato lavorato anche con una 8 elementi e 50 W). Punta l'antenna verso lo scatter point indicato dal programma, non verso il corrispondente.
