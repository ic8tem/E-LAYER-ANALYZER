# BENCHMARK Es

[← Home](Home-nb)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Testbenk for **sporadisk E** og **multihopp** på en **hel ADIF-logg**, også fra en annen stasjon: ut fra QSO-ene som faktisk er kjørt, skisserer programmet skip og skyer, dag for dag og time for time.

## Laste inn loggen

- **Last inn ADIF**: velger `.adi`/`.adif`-filen. Programmet husker filen og laster den inn på nytt ved oppstart.
- **Fjern ADIF**: fjerner loggen fra fanen.
- **Stasjon i sentrum**: kallesignal og lokator for stasjonen som har laget loggen. Programmet foreslår den hyppigste `STATION_CALLSIGN` (eller `OPERATOR`) og `MY_GRIDSQUARE`; de kan endres for hånd og bekreftes med **Bruk**. Sentrum er uavhengig av de generelle innstillingene, så logger fra andre stasjoner kan studeres.
- Sendeposisjon: `MY_GRIDSQUARE` for hver QSO (hvis den mangler, lokatoren i sentrum). Mottaksposisjoner: feltene `GRIDSQUARE`.
- QSO-er som brukes: 28, 50, 70 og 144 MHz med lokator. QSO-er med en annen `PROP_MODE` (TR, MS, FAI, AUR, EME...) og de under 500 km kan utelates. 432 MHz tas aldri med: sporadisk E gjelder ikke.

## Dager og timer

- **QSO-er per dag**: dag, antall QSO-er, bånd, maksimal avstand, multihopp. Ved åpning foreslår programmet dagen med flest QSO-er.
- **Timer på dagen**: raden *Hele dagen* og én rad for hver UTC-time; et klikk viser bare QSO-ene i den timen.
- **QSO**: listen for utvalget; et klikk på en QSO (eller nær motstasjonen på kartet) uthever den og viser detaljene.

## Kartet

**Asimutalt ekvidistant** kart sentrert på stasjonen, med glidebryteren **Maksimal avstand** for zoom (også med musehjulet): ringene viser **km fra sentrum**.

- **Skip per sektor** (10° asimut): oransje bue = nærmeste QSO i sektoren (kanten av den døde sonen), grønn bue = fjerneste QSO, imellom det oppnådde området.
- **Es-skyer** på Maidenhead-rutene, farget etter estimert foEs.
- Midtpunkt for enkelthopp (gul), stasjonens første hopp (oransje, hul sirkel = kort første hopp), motstasjonens første hopp (fiolett).
- For hele dagen binder den **røde linjen** sammen tyngdepunktet for midtpunktene time for time: det er skyenes forflytning.

![Én time](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Foreslåtte beregninger

1. **Skip per sektor**: for hver sektor skip-avstanden og rekkevidden, med kallesignalene.
2. **Estimert foEs** med sekantloven (foEs ≥ f / sec φ): det korteste enkelthoppet på hvert bånd gir det høyeste estimatet; med denne foEs forventet MUF på 1000, 1500, 2000 og 2400 km og avstanden der 144 MHz ville åpnet.
3. **Es-skyer** estimert på rutene, med antall QSO-er og foEs.
4. **Multihopp**: stasjonens første hopp under de to hypotesene (like hopp, og kort første hopp innenfor 1200 km, IC8TEMs erfaring) sammenlignet med skyene sett ved enkelthopp innenfor ±30 minutter; innenfor 300 km er hypotesen bekreftet. Programmet teller bekreftelsene for hver hypotese.
5. **Skyenes forflytning** time for time: km, retning og hastighet.

Beregningene er veiledende: en QSO viser at strekningen var åpen, ikke nøyaktig hvor skyen var. Jo renere data (6-tegns lokatorer, riktig `PROP_MODE`), desto mer pålitelig skip.
