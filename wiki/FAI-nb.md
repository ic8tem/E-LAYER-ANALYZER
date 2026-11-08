# F.A.I.

[← Home](Home-nb)

F.A.I. (Field Aligned Irregularities) er spredning fra uregelmessigheter som er rettet langs det geomagnetiske feltet, typisk for sommerkvelder etter sporadisk E. Det spredte signalet går ut på en **kjegle** med aksen langs feltet: to stasjoner kan kjøre hverandre hvis de ligger på **supplementære konturer** (a1 + a2 = 180°, artikkelen av IC8FAX-IC8TEM; ±c-konturene til K9SE). Magnetfeltet beregnes med modellen **IGRF-14**.

## Visningen Scatter trail (par A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Skriv inn **Stasjon A** og **Stasjon B** (kallesignal slått opp i CALL3 og/eller lokator). Tom A = din egen stasjon.
- Kartet viser **scatter trail**: alle punkter i lagets høyde, sett av begge stasjonene, som oppfyller refleksjonsbetingelsen innenfor 2,5° (klasser 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Feltet øverst til høyre viser **QTF og elevasjon** for A (rød) og B (grønn) mot hvert punkt.
- Programmet foreslår det **mest sannsynlige scatter-punktet** (minste avvik, korteste strekning).
- **Refraksjon 4/3**: på som standard; utvider horisontene.

## Visningen Isorefleksjon (isorefleksjonskurver)

![Isorefleksjon](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Dette er den geometriske konstruksjonen som brukes til å **kontrollere om et refleksjonspunkt er riktig**.

- **Venstreklikk** på kartet: du velger punktet som skal kontrolleres. **Høyreklikk**: flytter stasjon B.
- **Kontroller det anbefalte punktet**: åpner diagrammet på punktet programmet foreslår.
- Konturlinjer ved **0, ±1, ±2, ±4, ±8, ±16°**: fargede bånd i de mørke skjemaene, **tykke linjer** i de lyse skjemaene.
- Vurdering av paret: **tilfredsstillende** innenfor 1°, **akseptabel** innenfor 2,5°, **ikke tilfredsstillende** utover det.
- Hvis punktet er under horisonten for én av stasjonene, vises sprettoppvinduet **Beregning ikke mulig** og ingenting annet.

## Kontroller

| Forbindelse | Scatter-punkt | Avvik | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (over Genova) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (sørvestlige Tyskland, 28 km fra Brussel) | 0,88° | 338° / 87° |
| DB0FAI - Dresden via Sheffield (DL5MCG) | – | 0,3° | – |

Kjente scatter-punkter i listen: Genève, Brussel, Budapest, JN92, JN41/42.
