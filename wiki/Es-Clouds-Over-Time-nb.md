# Es-SKYER OVER TID

[← Home](Home-nb)

![Es-SKYER OVER TID](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Underfane i **SPORADIC-E** (fra versjon 0.5.2): hvordan **skyene av sporadisk E** oppstår, flytter seg og forsvinner, steg for steg.

## Metoden

Metoden til **Chartier, Hanley og Emmons (2022)**: hver forbindelse via sporadisk E viser at foEs i refleksjonspunktet er **minst f / sec φ**. Punktene samles i celler og i tidssteg, og hver celle får den **høyeste verdien** i steget. Den minste elektrontettheten er NmE = 1,24·10¹⁰ · foEs² elektroner per kubikkmeter.

Programmet bruker refleksjonspunktene fra sin egen modell (alle hopp), ikke bare midtpunktet. Verdien er en **nedre grense**: en tom celle betyr bare at det ikke finnes noen forbindelser.

## Kilde og kontroller

- **Spot fra økten** (oppdateres hver gang fanen åpnes) eller **Last inn ADIF-logg eller CSV-spot**.
- **Eksporter spotene fra økten (CSV)**: spotarkivet tømmes når programmet lukkes, så på denne måten kan du se på dem igjen senere.
- **Bånd MHz** (28, 40, 50, 70, 144), **Steg** (5, 10 eller 15 minutter), **Celle** (1°, 2° eller 4°), **Es-høyde km**.
- Tidslinje: ⏮ ◀ *Start/Stopp* ▶ ⏭ og glidebryteren; ved åpning starter visningen på steget med flest celler.
- *Tidligere steg falmet* viser de tre stegene før det valgte; *Forbindelser* tegner strekningene med båndenes farger.

## Tabell og graf

- Cellene i steget: Celle (sørvestlige hjørne og lokatoren for sentrum), foEs≥, NmE≥, Forbindelser, Bånd. Klikk på en celle: forbindelsene i den framheves.
- Graf: høyeste minste foEs (gul linje) og antall celler (søyler) for hvert steg.
