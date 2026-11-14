# STILLINGAR

[← Home](Home-is)

Frá útgáfu 0.4.18 safnar flipinn **STILLINGAR** saman almennum valkostum forritsins og flokkun spota milli flipanna. Hann hefur tvo undirflipa.

## Almennt

| Hluti | Hvað þú stillir |
|---|---|
| Stöð | Kallmerki, locator og hæð, þau sömu og í efri stikunni; **Nota** staðfestir þau fyrir alla flipa. |
| Útlit | Tungumál, litaþema (stef YAEC og YA-MS og, frá útgáfu 0.5.0, sex þemu IP-TRX-CTRL: Ljóst, Nótt, Svart, Blátt, Fjólublátt, Grænt) og **leturstærð alls forritsins** (85-140 %, eins og í YA-MS). Tungumál og litir endurbyggja gluggann: stillingar og spot haldast. |
| Kort og spágögn | Heimsálfan, sama valmynd og í flipastikunni. |
| Tími | Staða NTP og munurinn á tíma þjónanna og klukku tölvunnar. |
| Uppfærslur | Athugun á GitHub við ræsingu og á 30 mínútna fresti, sjálfvirk og hljóðlaus uppfærsla, hnappurinn **Athuga núna**. Sjálfvirkar uppfærslur gilda ekki fyrir kerfispakka (.deb, .rpm, Arch, Slackware). |
| Gögn | Hnappur sem opnar möppuna `data`. API-lykill Bureau of Meteorology er áfram í DATA COLLECTION › Útbreiðslugögn. |

## Flokkun spota

Til vinstri telur tafla upp reglurnar sem forritið notar til að ákveða hvert hvert spot fer, **í þeirri röð sem þeim er beitt**: fyrsta reglan sem á við ræður flipanum. Gildin sem sýnd eru eru þau sem stillt eru í síunum.

| # | Ef spotið... | fer á |
|---|---|---|
| 1 | er utan staðbundnu síunnar (undir 27990 kHz eða yfir 145 MHz; 430-440 MHz aðeins norðurljós) | hent |
| 2 | inniheldur AUE, AU-E eða AURORAL E | AURORA (norðurljós-E) |
| 3 | inniheldur norðurljósaorð (sjálfgefið AU, AUR) | AURORA |
| 4 | inniheldur F.A.I.-orð (sjálfgefið FAI; tíðnisvið 6, 4 og 2 m: F.A.I. nær ekki upp í 432 MHz) | F.A.I. |
| 5 | hefur meteor scatter-hátt eða `<MS>` | METEOR SCATTER |
| 6 | fellur í MS-glugga tíðniskipulagsins (á 50 MHz ekki útilokandi; FT8 og FT4 á 50 og 70 MHz undanskilin) | METEOR SCATTER |
| 7 | er FT8 eða FT4 á 50 MHz án annars spots innan 4 mín. með miðpunkt innan 150 km | meteor scatter (ekki sporadic-E) |
| 8 | er Q65 eða JT65 á 144 MHz (nema `<ES>`) | EME: hent |
| 9 | inniheldur merkingu annarrar útbreiðslu | ekki sporadic-E |
| 10 | er þvermiðbaugs | ekki sporadic-E (TEP) |
| 11-15 | er á 28, 40, 50, 70 eða 144 MHz innan fjarlægðarreglnanna | SPORADIC-E |
| 16 | fellur ekki undir neina reglu (tropo, 432 MHz, HF...) | enginn flipi (verður áfram í Söfnuð spot) |

Til hægri eru **staðbundna sían** og síur hverrar aðferðar, í fimm undirflipum: Meteor scatter (sía YA-MS er stillt í flipanum METEOR SCATTER), Sporadic-E, F.A.I., Norðurljós og **Prófun sía**, þar sem þú límir inn klasalínu og sérð hvernig hún er flokkuð. Hnappurinn **Endurheimta reglur höfundarins** setur sjálfgefnu gildin aftur.
