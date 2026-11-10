# BENCHMARK 50 MHz

[← Home](Home-da)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Testbænk for **multi-hop sporadisk E på 50 MHz** med ADIF-loggen fra én station og, valgfrit, fra en anden station.

## Indlæsning af loggene

- **Station 1**: *Indlæs ADIF* / *Fjern ADIF*. Programmet tager `STATION_CALLSIGN` og `MY_GRIDSQUARE` fra loggen; sendepositionen er `MY_GRIDSQUARE` for hver QSO.
- **Station 2** (valgfri): samme knapper på anden række; afkrydsningsfeltet *Station 2* viser eller skjuler den. Strækninger for station 1 er lyseblå, for station 2 violette.
- Programmet bruger QSO'er på 50 MHz med lokator (manglende lokatorer eller lokatorer med 4 tegn hentes fra CALL3), multi-hop: over 3100 km (forfatterens regel; antallet af hop følger IK3XTV-tabellen).
- **Transækvatoriale** forbindelser udelades (forfatterens regel): mod Afrika og Sydamerika, med midtpunktet i båndet med magnetisk inklination mellem -20° og +20°, medmindre loggen har `PROP_MODE=ES`.

## Dag og tidsrum

Vælg **dagen** (ved åbning den med flest multi-hop-QSO'er) og tidsrummet **fra… til…** UTC i trin på 15 minutter. Afkrydsningsfeltet **foEs** (valgfrit) farver strækningerne efter mindste foEs og viser den i tabellen.

## Kortet

**Mercator**-kort med frit centrum: scrollhjul = zoom, træk = flyt.

![Strækning](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- En **sløret gul prik** markerer midten af hvert hop, hvor Es-skyen formodes at ligge; på strækningen under musen bliver den større.
- **Musen nær en strækning**: linjen og ikonerne forstørres, og modstationens **WWL-felt med 6 tegn** vises.
- **Venstreklik** på den strækning: en popup spørger, om **alle QSO'er i filen** (alle dage) med slutpositionen **inden for 150 km** fra det punkt skal vises. *Ja*: kortet ryddes og viser disse QSO'er inden for den orange cirkel; *Nej*: kortet forbliver uændret. *Tilbage til dag og tidsrum* vender tilbage til det tidligere udvalg.

![Nærheden](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detaljer

For den valgte strækning: afstand og vurdering efter IK3XTV-tabellen, azimut i begge retninger, lige lange hop med felterne for refleksionspunkterne, kort første hop inden for 1200 km (IC8TEM's erfaring) og, med afkrydsningsfeltet foEs, den mindste foEs.
