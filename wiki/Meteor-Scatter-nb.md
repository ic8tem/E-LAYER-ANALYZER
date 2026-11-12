# METEOR SCATTER

[← Home](Home-nb)

Denne fanen inneholder hele **YA-MS 1.8.0**, meteor scatter-programmet til IC8TEM: himmelkule med radiantene, radarfluks fra SKiYMET (Collm, Sodankylä), beregning av svermeffektivitet, eksperimentell implementering av prognosene, geometri og demping, GRAVES, rutekart, **Svermer direkte** (ZHR målt av Global Meteor Network og radianter fra CMOR-radaren), **kart over kildene** (Collm, Sodankylä, GRAVES og CMOR i forhold til stasjonen) og dokumentet **Effektivitet - forklaring** med formlene.

YA-MS 1.5.0 legger til fanen **Datainnsamling** (meteor scatter-spot filtrert etter MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Inne i E-LAYER ANALYZER åpner den ingen egne telnet-økter: den mottar linjene fra øktene i DATA COLLECTION, så det blir ingen doble innlogginger med samme kallesignal.

- YA-MS bruker språket og fargeskjemaet som er valgt øverst i E-LAYER ANALYZER.
- Innstillingene for YA-MS lagres i `data/config.ini`.
- Hver nye versjon av YA-MS hentes inn med skriptet `tools/sync_yams.py`.

Detaljer finner du i [YA-MS-repositoriet](https://github.com/ic8tem/YA-MS).
