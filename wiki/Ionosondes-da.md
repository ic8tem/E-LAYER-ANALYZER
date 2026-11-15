# IONOSONDER

[← Home](Home-da)

![IONOSONDER](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Underfane til **SPORADIC-E** (fra version 0.5.2): den **målte foEs** fra ionosonderne i **GIRO**-nettet over hele verden, ved siden af den foEs, programmet udleder af spottene.

## Dataene

- 63 GIRO-ionosonder (ULCAR-listen) med den seneste **foEs**, **foF2** og **MUF(D)** aflæst automatisk fra ionogrammerne.
- Dataene kommer, første gang fanen åbnes, og derefter **hvert 15. minut** af sig selv (afkrydsningsfeltet *Opdater selv hvert 15. minut*); *Opdater nu* henter dem straks. Arkivet gemmer de seneste 48 timer i mappen `giro` blandt dataene.
- Data fra **GIRO** (Reinisch og Galkin 2011), licens **CC BY-NC-SA 4.0**: kun ikke-kommerciel brug. Tjenesten er den samme som i *MUF Muncher* af DM6AX.

## Kort og tabel

- Hver ionosonde er en farvet prik med den seneste foEs (samme farver som Es-skyerne) og værdien i MHz; **grå**, hvis den ikke har nye data, med **ring**, hvis det seneste ionogram er over 90 minutter gammelt.
- Tabel: Kode, Ionosonde, km fra din station, UTC for det seneste ionogram, foEs, foF2, MUF(D) og **Spots foEs≥**: den mindste foEs udledt af Es-spots fra de seneste 30 minutter, der reflekteres inden for 500 km fra ionosonden (kan justeres), med antallet af spots.

## Den valgte ionosonde

Klik på en række eller nær en ionosonde på kortet:

- **graf over de seneste 24 timer**: foEs gul, foF2 lyseblå, MUF(D) lilla prikket;
- **MUF for sporadisk E** ved 1000, 1500, 2000 og 2400 km (MUF = foEs · sec φ i den valgte Es-højde) med de **bånd, der åbner** (28, 40, 50, 70, 144 MHz);
- hvis spottene kræver mere end den målte foEs, gør programmet opmærksom på det: skyen kan ligge langt fra ionosonden, eller ionosonden har nået grænsen for sit frekvensområde (Chartier 2022).
