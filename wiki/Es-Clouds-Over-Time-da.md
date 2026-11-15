# Es-SKYER OVER TID

[← Home](Home-da)

![Es-SKYER OVER TID](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Underfane til **SPORADIC-E** (fra version 0.5.2): hvordan **skyerne af sporadisk E** opstår, flytter sig og forsvinder, trin for trin.

## Metoden

Metoden fra **Chartier, Hanley og Emmons (2022)**: hver forbindelse via sporadisk E viser, at foEs i refleksionspunktet er **mindst f / sec φ**. Punkterne samles i celler og tidstrin, og hver celle får trinnets **højeste værdi**. Den mindste elektrontæthed er NmE = 1,24·10¹⁰ · foEs² elektroner pr. kubikmeter.

Programmet bruger refleksionspunkterne fra sin egen model (alle hop), ikke kun midtpunktet. Værdien er en **nedre grænse**: en tom celle betyder kun, at der ikke er nogen forbindelser.

## Kilde og kontroller

- **Sessionens spots** (opdateres, hver gang fanen åbnes) eller **Indlæs ADIF-log eller CSV-spots**.
- **Eksportér sessionens spots (CSV)**: spotarkivet tømmes, når programmet lukkes, så på denne måde kan du se dem igen senere.
- **Bånd MHz** (28, 40, 50, 70, 144), **Trin** (5, 10 eller 15 minutter), **Celle** (1°, 2° eller 4°), **Es-højde km**.
- Tidslinje: ⏮ ◀ *Start/Stop* ▶ ⏭ og skyderen; ved åbning starter visningen ved trinnet med flest celler.
- *Tidligere trin falmet* viser de tre trin før det valgte; *Forbindelser* tegner strækningerne i båndenes farver.

## Tabel og graf

- Trinnets celler: Celle (sydvestlige hjørne og centrets lokator), foEs≥, NmE≥, Forbindelser, Bånd. Klik på en celle: dens forbindelser fremhæves.
- Graf: højeste mindste foEs (gul linje) og antal celler (søjler) for hvert trin.
