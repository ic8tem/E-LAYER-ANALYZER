# METEOR SCATTER

[← Home](Home-sv)

Den här fliken innehåller hela **YA-MS 1.8.0**, IC8TEM:s program för meteorscatter: himmelssfär med radianterna, radarflöde från SKiYMET (Collm, Sodankylä), beräkning av svärmarnas effektivitet, experimentell implementering av prognoserna, geometri och dämpning, GRAVES, rutkarta, **Svärmar live** (ZHR uppmätt av Global Meteor Network och radianter från CMOR-radarn), **källkarta** (Collm, Sodankylä, GRAVES och CMOR i förhållande till stationen) och dokumentet **Effektivitet - förklaring** med formlerna.

YA-MS 1.5.0 lägger till fliken **Datainsamling** (meteorscatter-spots filtrerade på MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Inuti E-LAYER ANALYZER öppnar den inga egna telnetsessioner: den tar emot raderna från sessionerna i DATA COLLECTION, så det blir inga dubbla inloggningar med samma anropssignal.

- YA-MS använder det språk och färgschema som valts högst upp i E-LAYER ANALYZER.
- YA-MS inställningar sparas i `data/config.ini`.
- Varje ny version av YA-MS förs in med skriptet `tools/sync_yams.py`.

För detaljer, se [YA-MS-förrådet](https://github.com/ic8tem/YA-MS).
