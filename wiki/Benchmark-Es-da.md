# BENCHMARK Es

[← Home](Home-da)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Testbænk for **sporadisk E** og **multi-hop** på en **hel ADIF-log**, også fra en anden station: ud fra de QSO'er, der faktisk er kørt, skitserer programmet skip og skyer, dag for dag og time for time.

## Indlæsning af loggen

- **Indlæs ADIF**: vælger `.adi`/`.adif`-filen. Programmet husker filen og genindlæser den ved opstart.
- **Fjern ADIF**: fjerner loggen fra fanen.
- **Station i centrum**: kaldesignal og lokator for den station, der har lavet loggen. Programmet foreslår den hyppigste `STATION_CALLSIGN` (eller `OPERATOR`) og `MY_GRIDSQUARE`; de kan ændres manuelt og bekræftes med **Anvend**. Centrum afhænger ikke af de generelle indstillinger, så logs fra andre stationer kan undersøges.
- Sendeposition: `MY_GRIDSQUARE` for hver QSO (hvis den mangler, lokatoren i centrum). Modtagepositioner: felterne `GRIDSQUARE`.
- Anvendte QSO'er: 28, 50, 70 og 144 MHz med lokator. QSO'er med en anden `PROP_MODE` (TR, MS, FAI, AUR, EME...) og dem under 500 km kan udelades. 432 MHz medtages aldrig: sporadisk E gælder ikke.

## Dage og timer

- **QSO'er pr. dag**: dag, antal QSO'er, bånd, maksimal afstand, multi-hop. Ved åbning foreslår programmet dagen med flest QSO'er.
- **Dagens timer**: rækken *Hele dagen* og én række for hver UTC-time; et klik viser kun QSO'erne i den time.
- **QSO**: listen for udvalget; et klik på en QSO (eller nær modstationen på kortet) fremhæver den og viser detaljerne.

## Kortet

**Azimutalt ækvidistant** kort centreret på stationen med skyderen **Maksimal afstand** til zoom (også med musens scrollhjul): ringene viser **km fra centrum**.

- **Skip pr. sektor** (10° azimut): orange bue = nærmeste QSO i sektoren (kanten af den døde zone), grøn bue = fjerneste QSO, imellem det opnåede område.
- **Es-skyer** på Maidenhead-felterne, farvet efter den anslåede foEs.
- Midtpunkt for enkelte hop (gul), stationens første hop (orange, hul cirkel = kort første hop), modstationens første hop (violet).
- En **sløret gul prik** markerer midten af hvert hop, hvor Es-skyen formodes at ligge (også midten af det korte første hop, når den hypotese er aktiv).
- For hele dagen forbinder den **røde linje** tyngdepunktet for midtpunkterne time for time: det er skyernes vandring.

![Én time](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Foreslåede beregninger

1. **Skip pr. sektor**: for hver sektor skipafstand og rækkevidde, med kaldesignalerne.
2. **Anslået foEs** med sekantloven (foEs ≥ f / sec φ): det korteste enkelte hop på hvert bånd giver det højeste skøn; med denne foEs den forventede MUF ved 1000, 1500, 2000 og 2400 km og den afstand, ud over hvilken 144 MHz ville åbne.
3. **Es-skyer** anslået på felterne med antal QSO'er og foEs.
4. **Multi-hop**: stationens første hop under de to hypoteser (lige lange hop og kort første hop inden for 1200 km, IC8TEM's erfaring) sammenlignet med skyerne set ved ét hop inden for ±30 minutter; inden for 300 km er hypotesen bekræftet. Programmet tæller bekræftelserne for hver hypotese.
5. **Skyernes vandring** time for time: km, retning og hastighed.

Beregningerne er vejledende: en QSO viser, at strækningen var åben, ikke præcis hvor skyen var. Jo renere data (6-tegns lokatorer, korrekt `PROP_MODE`), desto mere pålideligt skip.
