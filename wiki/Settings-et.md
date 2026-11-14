# SEADED

[← Home](Home-et)

Alates versioonist 0.4.18 koondab sakk **SEADED** programmi üldised valikud ja spottide suunamise sakkide vahel. Sellel on kaks alamsakki.

## Üldine

| Jaotis | Mida seadistad |
|---|---|
| Jaam | Kutsung, lokaator ja kõrgus, samad mis ülemisel ribal; **Rakenda** kinnitab need kõigile sakkidele. |
| Välimus | Keel, värviskeem (YAEC-i ja YA-MS-i skeemid ning alates versioonist 0.5.0 kuus IP-TRX-CTRL-i teemat: Hele, Öö, Must, Sinine, Lilla, Roheline) ja **kogu programmi kirjasuurus** (85-140 %, nagu YA-MS-is). Keel ja värvid ehitavad akna uuesti üles: seaded ja spotid säilivad. |
| Kaardid ja prognoosiandmed | Kontinent, sama menüü mis sakiribal. |
| Aeg | NTP olek ning serverite aja ja arvuti kella erinevus. |
| Uuendused | Kontroll GitHubis käivitamisel ja iga 30 minuti järel, automaatne ja vaikne uuendamine, nupp **Kontrolli kohe**. Automaatsed uuendused ei kehti süsteemipakettide (.deb, .rpm, Arch, Slackware) kohta. |
| Andmed | Nupp, mis avab kausta `data`. Bureau of Meteorology API võti jääb DATA COLLECTION › Levi andmed alla. |

## Spottide suunamine

Vasakul loetleb tabel reeglid, millega programm otsustab, kuhu iga spot läheb, **nende rakendamise järjekorras**: esimene kehtiv reegel otsustab saki. Näidatud väärtused on filtrites seatud väärtused.

| # | Kui spot... | läheb |
|---|---|---|
| 1 | on väljaspool kohalikku filtrit (alla 27990 kHz või üle 145 MHz; 430-440 MHz ainult aurora) | kõrvale jäetud |
| 2 | sisaldab AUE, AU-E või AURORAL E | AURORA (aurora-E) |
| 3 | sisaldab aurora sõna (vaikimisi AU, AUR) | AURORA |
| 4 | sisaldab F.A.I. sõna (vaikimisi FAI; sagedusalad 6, 4 ja 2 m: F.A.I. ei ulatu 432 MHz-ni) | F.A.I. |
| 5 | on meteoorhajumise viisiga või `<MS>` | METEOR SCATTER |
| 6 | langeb sagedusplaani MS-aknasse (50 MHz-l mitte välistav; FT8 ja FT4 50 ja 70 MHz-l välja arvatud) | METEOR SCATTER |
| 7 | on FT8 või FT4 50 MHz-l ilma teise spotita 4 min jooksul keskpunktiga 150 km piires | meteoorhajumine (mitte sporaadiline E) |
| 8 | on Q65 või JT65 144 MHz-l (välja arvatud `<ES>`) | EME: kõrvale jäetud |
| 9 | sisaldab muu levi märget | mitte sporaadiline E |
| 10 | on transekvatoriaalne | mitte sporaadiline E (TEP) |
| 11-15 | on 28, 40, 50, 70 või 144 MHz-l kaugusreeglite piires | SPORADIC-E |
| 16 | ei vasta ühelegi reeglile (tropo, 432 MHz, HF...) | mitte ükski sakk (jääb Kogutud spottide hulka) |

Paremal on **kohalik filter** ja iga leviviisi filtrid viiel alamsakil: Meteoorhajumine (YA-MS-i filter seadistatakse sakil METEOR SCATTER), Sporaadiline E, F.A.I., Aurora ja **Filtrite proov**, kuhu kleebid klastri rea ja näed, kuidas see liigitatakse. Nupp **Taasta autori reeglid** taastab vaikeväärtused.
