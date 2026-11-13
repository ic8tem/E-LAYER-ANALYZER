# METEOR SCATTER

[← Home](Home-sv)

Den här fliken innehåller hela **YA-MS 1.8.4**, IC8TEM:s program för meteorscatter: himmelssfär med radianterna, radarflöde från SKiYMET (Collm, Sodankylä), beräkning av svärmarnas effektivitet, experimentell implementering av prognoserna, geometri och dämpning, GRAVES, rutkarta, **Svärmar live** (ZHR uppmätt av Global Meteor Network och radianter från CMOR-radarn), **källkarta** (Collm, Sodankylä, GRAVES och CMOR i förhållande till stationen) och dokumentet **Förklaring av effektivitetsberäkningen** med formlerna.

YA-MS 1.5.0 lägger till fliken **Datainsamling** (meteorscatter-spots filtrerade på MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Inuti E-LAYER ANALYZER öppnar den inga egna telnetsessioner: den tar emot raderna från sessionerna i DATA COLLECTION, så det blir inga dubbla inloggningar med samma anropssignal.

YA-MS 1.6.0 omorganiserar flikarna i **Live och realtidsdata** (meteorscatter-trafik live med molnet för varje förbindelse, radarflöde, GRAVES, datainsamling) och **Prognosberäkning** (prognos, geometri, effektivitetsberäkning, implementering av prognoserna).

YA-MS 1.7.0 och 1.8.0 lägger till jämförelsen av radianterna med katalogen IAU MDC, de dagliga räkningarna från Global Meteor Network per svärm, k-indexet utifrån populationsindexen, spårets initiala radie enligt Stober m.fl. (2021), pingarnas varaktighet mellan banden (λ² som standard eller mätningarna från HamSCI MSQP av NN4SA) och jämförelsen med BRAMS-detektionerna i Sonneberg (Perseiderna 2026). Den automatiska uppdateringen av YA-MS är avstängd inuti E-LAYER ANALYZER, som uppdaterar sig själv från Releases på GitHub.

YA-MS 1.8.1 behandlar antennloben som ett vertikalt diagram (rutan «antenndiagram», på som standard): reflexpunkterna vägs med båda antennernas vertikala förstärkning och effektiviteterna försvinner inte längre på långa sträckor. Nytt är varningen «ingen gemensam zon», den framhävda rutan för vinkeln β (valet som väger tyngst), ett enda beräkningsdatum, «Bruksanvisning» och jämförelsen med 181 767 verkliga spots på 50 MHz från HamSCI Meteor Scatter QSO Party i augusti 2025: timmarna och sträckans sidor är tillförlitliga, procentsatserna är vägledande.

- YA-MS använder det språk och färgschema som valts högst upp i E-LAYER ANALYZER.
- YA-MS inställningar sparas i `data/config.ini`.
- Varje ny version av YA-MS förs in med skriptet `tools/sync_yams.py`.

För detaljer, se [YA-MS-förrådet](https://github.com/ic8tem/YA-MS).
