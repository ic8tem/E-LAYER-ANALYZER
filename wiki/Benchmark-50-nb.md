# BENCHMARK 50 MHz

[← Home](Home-nb)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Testbenk for **multihopp sporadisk E på 50 MHz** med ADIF-loggen fra én stasjon og, valgfritt, fra en andre stasjon.

## Laste inn loggene

- **Stasjon 1**: *Last inn ADIF* / *Fjern ADIF*. Programmet henter `STATION_CALLSIGN` og `MY_GRIDSQUARE` fra loggen; sendeposisjonen er `MY_GRIDSQUARE` for hver QSO.
- **Stasjon 2** (valgfri): samme knapper på andre rad; avkrysningsboksen *Stasjon 2* viser eller skjuler den. Strekninger for stasjon 1 er lyseblå, for stasjon 2 fiolette.
- Programmet bruker QSO-er på 50 MHz med lokator (manglende lokatorer eller lokatorer med 4 tegn hentes fra CALL3), multihopp: over 3100 km (forfatterens regel; antall hopp følger IK3XTV-tabellen).
- **Transekvatoriale** forbindelser utelates (forfatterens regel): mot Afrika og Sør-Amerika, med midtpunktet i beltet med magnetisk inklinasjon mellom -20° og +20°, med mindre loggen har `PROP_MODE=ES`.

## Dag og tid

Velg **dagen** (ved åpning den med flest multihopp-QSO-er) og tidsrommet **fra… til…** UTC, i steg på 15 minutter. Avkrysningsboksen **foEs** (valgfri) farger strekningene etter minste foEs og viser den i tabellen.

## Kartet

**Mercator**-kart med fritt sentrum: hjul = zoom, dra = flytt.

![Strekning](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Musen nær en strekning**: linjen og ikonene forstørres, og motstasjonens **WWL-rute med 6 tegn** vises.
- **Venstreklikk** på den strekningen: et sprettoppvindu spør om **alle QSO-ene i filen** (alle dager) med sluttposisjonen **innenfor 150 km** fra det punktet skal vises. *Ja*: kartet tømmes og viser disse QSO-ene innenfor den oransje sirkelen; *Nei*: kartet forblir som det er. *Tilbake til dag og tid* går tilbake til det forrige utvalget.

![Nærheten](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detaljer

For valgt strekning: avstand og vurdering etter IK3XTV-tabellen, asimut i begge retninger, like hopp med rutene for refleksjonspunktene, kort første hopp innenfor 1200 km (IC8TEMs erfaring) og, med avkrysningsboksen foEs, minste foEs.
