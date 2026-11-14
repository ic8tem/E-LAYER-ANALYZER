# METEOR SCATTER

[← Home](Home-da)

Denne fane indeholder hele **YA-MS 1.8.12**, IC8TEM's program til meteor scatter: himmelkugle med radianterne, radarflux fra SKiYMET (Collm, Sodankylä), beregning af sværmeffektivitet, eksperimentel implementering af prognoserne, geometri og dæmpning, GRAVES, kort over felter, **Live-sværme** (ZHR målt af Global Meteor Network og radianter fra CMOR-radaren), **kort over kilderne** (Collm, Sodankylä, GRAVES og CMOR i forhold til stationen) og dokumentet **Forklaring af effektivitetsberegningen** med formlerne.

YA-MS 1.5.0 tilføjer fanen **Dataindsamling** (meteor scatter-spots filtreret efter MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Inde i E-LAYER ANALYZER åbner den ingen egne telnet-sessioner: den modtager linjerne fra sessionerne i DATA COLLECTION, så der er ingen dobbelte logins med samme kaldesignal.

YA-MS 1.6.0 omorganiserer fanerne i **Live og realtidsdata** (live meteor scatter-trafik med skyen for hver forbindelse, radarflux, GRAVES, dataindsamling) og **Prognoseberegning** (prognose, geometri, effektivitetsberegning, implementering af prognoserne).

YA-MS 1.7.0 og 1.8.0 tilføjer sammenligningen af radianterne med kataloget IAU MDC, de daglige tællinger fra Global Meteor Network pr. sværm, k-indekset ud fra populationsindekserne, sporets begyndelsesradius efter Stober m.fl. (2021), ping-varigheden mellem båndene (λ² som standard eller målingerne fra HamSCI MSQP af NN4SA) og sammenligningen med BRAMS-detektionerne i Sonneberg (Perseiderne 2026). Den automatiske opdatering af YA-MS er slået fra inde i E-LAYER ANALYZER, som selv opdateres fra Releases på GitHub.

YA-MS 1.8.1 behandler antenneloben som et lodret diagram (feltet «antennediagram», slået til som standard): refleksionspunkterne vægtes med begge antenners lodrette forstærkning, og effektiviteterne forsvinder ikke længere på lange strækninger. Nyt er advarslen «ingen fælles zone», det fremhævede felt for vinklen β (det valg, der vejer mest), én enkelt beregningsdato, «Brugervejledning» og sammenligningen med 181 767 virkelige spots på 50 MHz fra HamSCI Meteor Scatter QSO Party i august 2025: timerne og strækningens sider er pålidelige, procenterne er vejledende.

- YA-MS bruger det sprog og farveskema, der er valgt øverst i E-LAYER ANALYZER.
- YA-MS' indstillinger gemmes i `data/config.ini`.
- Hver ny version af YA-MS hentes ind med scriptet `tools/sync_yams.py`.

Detaljer findes i [YA-MS-repositoriet](https://github.com/ic8tem/YA-MS).
