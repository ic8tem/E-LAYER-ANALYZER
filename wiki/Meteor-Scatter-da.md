# METEOR SCATTER

[← Home](Home-da)

Denne fane indeholder hele **YA-MS 1.5.1**, IC8TEM's program til meteor scatter: himmelkugle med radianterne, radarflux fra SKiYMET (Collm, Sodankylä), beregning af sværmeffektivitet, eksperimentel implementering af prognoserne, geometri og dæmpning, GRAVES, kort over felter, **Live-sværme** (ZHR målt af Global Meteor Network og radianter fra CMOR-radaren), **kort over kilderne** (Collm, Sodankylä, GRAVES og CMOR i forhold til stationen) og dokumentet **Effektivitet - forklaring** med formlerne.

YA-MS 1.5.0 tilføjer fanen **Dataindsamling** (meteor scatter-spots filtreret efter MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Inde i E-LAYER ANALYZER åbner den ingen egne telnet-sessioner: den modtager linjerne fra sessionerne i DATA COLLECTION, så der er ingen dobbelte logins med samme kaldesignal.

- YA-MS bruger det sprog og farveskema, der er valgt øverst i E-LAYER ANALYZER.
- YA-MS' indstillinger gemmes i `data/config.ini`.
- Hver ny version af YA-MS hentes ind med scriptet `tools/sync_yams.py`.

Detaljer findes i [YA-MS-repositoriet](https://github.com/ic8tem/YA-MS).
