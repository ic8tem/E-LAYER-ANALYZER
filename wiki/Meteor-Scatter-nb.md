# METEOR SCATTER

[← Home](Home-nb)

Denne fanen inneholder hele **YA-MS 1.8.4**, meteor scatter-programmet til IC8TEM: himmelkule med radiantene, radarfluks fra SKiYMET (Collm, Sodankylä), beregning av svermeffektivitet, eksperimentell implementering av prognosene, geometri og demping, GRAVES, rutekart, **Svermer direkte** (ZHR målt av Global Meteor Network og radianter fra CMOR-radaren), **kart over kildene** (Collm, Sodankylä, GRAVES og CMOR i forhold til stasjonen) og dokumentet **Forklaring av effektivitetsberegningen** med formlene.

YA-MS 1.5.0 legger til fanen **Datainnsamling** (meteor scatter-spot filtrert etter MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Inne i E-LAYER ANALYZER åpner den ingen egne telnet-økter: den mottar linjene fra øktene i DATA COLLECTION, så det blir ingen doble innlogginger med samme kallesignal.

YA-MS 1.6.0 omorganiserer fanene i **Direkte og sanntidsdata** (meteor scatter-trafikk direkte med skyen for hver forbindelse, radarfluks, GRAVES, datainnsamling) og **Prognoseberegning** (prognose, geometri, effektivitetsberegning, implementering av prognosene).

YA-MS 1.7.0 og 1.8.0 legger til sammenligningen av radiantene med katalogen IAU MDC, de daglige tellingene fra Global Meteor Network per sverm, k-indeksen ut fra populasjonsindeksene, sporets startradius etter Stober m.fl. (2021), ping-varigheten mellom båndene (λ² som standard eller målingene fra HamSCI MSQP av NN4SA) og sammenligningen med BRAMS-deteksjonene i Sonneberg (Perseidene 2026). Den automatiske oppdateringen av YA-MS er slått av inne i E-LAYER ANALYZER, som oppdaterer seg selv fra Releases på GitHub.

YA-MS 1.8.1 behandler antenneloben som et vertikalt diagram (boksen «antennediagram», på som standard): refleksjonspunktene vektes med den vertikale gevinsten til begge antennene, og effektivitetene forsvinner ikke lenger på lange strekninger. Nytt er varselet «ingen felles sone», den uthevede boksen for vinkelen β (valget som veier mest), én enkelt beregningsdato, «Brukerveiledning» og sammenligningen med 181 767 virkelige spotter på 50 MHz fra HamSCI Meteor Scatter QSO Party i august 2025: timene og strekningens sider er pålitelige, prosentene er veiledende.

- YA-MS bruker språket og fargeskjemaet som er valgt øverst i E-LAYER ANALYZER.
- Innstillingene for YA-MS lagres i `data/config.ini`.
- Hver nye versjon av YA-MS hentes inn med skriptet `tools/sync_yams.py`.

Detaljer finner du i [YA-MS-repositoriet](https://github.com/ic8tem/YA-MS).
