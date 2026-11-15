# IONOSONDER

[← Home](Home-nb)

![IONOSONDER](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Underfane i **SPORADIC-E** (fra versjon 0.5.2): den **målte foEs** fra ionosondene i **GIRO**-nettet over hele verden, ved siden av den programmet utleder av spotene.

## Dataene

- 62 GIRO-ionosonder (ULCAR-listen), med siste **foEs**, **foF2** og **MUF(D)** lest automatisk fra ionogrammene.
- Dataene kommer første gang fanen åpnes, og deretter **hvert 15. minutt** av seg selv (avkrysningsboksen *Oppdater selv hvert 15. minutt*); *Oppdater nå* henter dem med en gang. Arkivet tar vare på de siste 48 timene i mappen `giro` blant dataene.
- Data fra **GIRO** (Reinisch og Galkin 2011), lisens **CC BY-NC-SA 4.0**: bare ikke-kommersiell bruk. Tjenesten er den samme som i *MUF Muncher* av DM6AX.

## Kart og tabell

- Hver ionosonde er en farget prikk med siste foEs (samme farger som Es-skyene) og verdien i MHz; **grå** hvis den ikke har nye data, med **ring** hvis det siste ionogrammet er over 90 minutter gammelt.
- Tabell: Kode, Ionosonde, km fra stasjonen din, UTC for siste ionogram, foEs, foF2, MUF(D) og **Spot foEs≥**: den minste foEs utledet av Es-spot fra de siste 30 minuttene som reflekteres innenfor 500 km fra ionosonden (kan justeres), med antall spot.

## Den valgte ionosonden

Klikk på en rad eller nær en ionosonde på kartet:

- **graf over de siste 24 timene**: foEs gul, foF2 lyseblå, MUF(D) lilla prikket;
- **MUF for sporadisk E** ved 1000, 1500, 2000 og 2400 km (MUF = foEs · sec φ, i den valgte Es-høyden) med **båndene som åpner** (28, 40, 50, 70, 144 MHz);
- hvis spotene krever mer enn den målte foEs, sier programmet fra: skyen kan ligge langt fra ionosonden, eller ionosonden har nådd grensen for frekvensområdet sitt (Chartier 2022).
