# METEOR SCATTER

[← Home](Home-is)

Þessi flipi inniheldur allt **YA-MS 1.8.8**, meteor scatter-forrit IC8TEM: himinkúla með geislapunktunum, ratsjárflæði SKiYMET (Collm, Sodankylä), útreikningur á nýtni drífa, tilraunaútfærsla spánna, rúmfræði og deyfing, GRAVES, reitakort, **Drífur í beinni** (ZHR mælt af Global Meteor Network og geislapunktar frá CMOR-ratsjánni), **kort af lindum** (Collm, Sodankylä, GRAVES og CMOR miðað við stöðina) og skjalið **Skýring á útreikningi nýtninnar** með formúlunum.

YA-MS 1.5.0 bætir við flipanum **Gagnasöfnun** (meteor scatter-spot síuð eftir MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Innan E-LAYER ANALYZER opnar hann engar eigin telnet-lotur: hann tekur við línunum frá lotunum í DATA COLLECTION, svo engar tvöfaldar innskráningar verða með sama kallmerki.

YA-MS 1.6.0 endurskipuleggur flipana í **Beint og rauntímagögn** (meteor scatter-umferð í beinni með skýi hvers sambands, ratsjárflæði, GRAVES, gagnasöfnun) og **Spáreikningur** (spá, rúmfræði, útreikningur á nýtni, útfærsla spánna).

YA-MS 1.7.0 og 1.8.0 bæta við samanburði geislapunktanna við IAU MDC-skrána, daglegum talningum Global Meteor Network fyrir hverja drífu, k-stuðlinum sem leiddur er af fjöldastuðlunum, upphafsradíus slóðarinnar samkvæmt Stober o.fl. (2021), lengd pinga milli tíðnisviða (λ² sjálfgefið eða HamSCI MSQP-mælingar NN4SA) og samanburði við BRAMS-greiningar í Sonneberg (Perseidar 2026). Sjálfvirk uppfærsla YA-MS er óvirk innan E-LAYER ANALYZER, sem uppfærir sig sjálft frá Releases á GitHub.

YA-MS 1.8.1 meðhöndlar geisla loftnetsins sem lóðrétt geislamynstur (reiturinn «geislamynstur loftnets», sjálfgefið kveiktur): endurvarpspunktar eru vegnir með lóðréttri mögnun beggja loftneta og nýtnin hverfur ekki lengur á löngum leiðum. Nýtt er viðvörunin «ekkert sameiginlegt svæði», áberandi reitur fyrir hornið β (valið sem vegur mest), ein útreikningsdagsetning, «Notendahandbók» og samanburður við 181 767 raunveruleg spot á 50 MHz úr Meteor Scatter QSO Party HamSCI í ágúst 2025: tímarnir og hliðar leiðarinnar eru áreiðanlegar, prósenturnar eru leiðbeinandi.

- YA-MS notar tungumálið og litaþemað sem valið er efst í E-LAYER ANALYZER.
- Stillingar YA-MS eru geymdar í `data/config.ini`.
- Hver ný útgáfa YA-MS er færð inn með skriftunni `tools/sync_yams.py`.

Nánari upplýsingar eru í [hugbúnaðarsafni YA-MS](https://github.com/ic8tem/YA-MS).
