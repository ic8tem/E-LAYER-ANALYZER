# F.A.I.

[← Home](Home-nb)

F.A.I. (Field Aligned Irregularities) er spredning fra uregelmessigheter som er rettet langs det geomagnetiske feltet, typisk for sommerkvelder etter sporadisk E. Det spredte signalet går ut på en **kjegle** med aksen langs feltet: to stasjoner kan kjøre hverandre hvis de ligger på **supplementære konturer** (a1 + a2 = 180°, artikkelen av IC8FAX-IC8TEM; ±c-konturene til K9SE). Magnetfeltet beregnes med modellen **IGRF-14**.

**F.A.I.-alarm** (fra versjon 0.6.5): fra mai til september, mellom 18 og 24 lokal tid, etter en Es-sky 300-1300 km fra stasjonen din **blinker** fanen, og en rød linje øverst sier hvor F.A.I. er mulig (reglene fra artikkelen IC8FAX-IC8TEM); med nye F.A.I.-spot sier den at den pågår. Klikk på linjen: visningen Isorefleksjon på punktet.

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
| IC8FAX - F6DRO | JN44EH (over Genova) | 0,96° | 313° / 77° |
| IC8FAX - G4LOH | JN49BR (sørvestlige Tyskland, 18 km fra Brussel) | 0,93° | 337° / 88° |
| Fyr DB0FAI (305°) - Dresden via Sheffield (DL5MCG; Dresden hadde antennen mot 290° og meldte nordlys) | – | 0,3° | – |
| IC8FAX - FA1MXE (1990, IGRF-felt fra 1990) | JN45QR, Scatter trail 35 km fra Genève (Genève: 3,79°) | 0,92° | 327° / 87° |
| DL5MCG - SV1DH (test fra 1987 via Budapest, 1569 km, IGRF-felt fra 1987) | Budapest ved 2,22°, innenfor 2,5° (1,85° med dagens felt); Scatter trail 21 km unna; mest sannsynlig KN06TQ | 2,22° (KN06TQ 0,92°) | 95° / 341° |

Fra versjon 0.4.18 bruker de to tilfellene med en dato (1987 og 1990) IGRF-14-feltet fra det året, så lenge stasjonene er de i tilfellet.

Kjente scatter-punkter i listen: Genève, Brussel, Budapest, JN92, JN41/42.

Menyen **Kontroller...** laster inn testparene IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE og DL5MCG-SV1DH.

**Utstyr** (DL5MCG): F.A.I. på 144 MHz krever minst en lang yagi og 100 W (SV1DH er også blitt kjørt med en 8-elementers yagi og 50 W). Rett antennen mot scatter-punktet som programmet angir, ikke mot motstasjonen.
