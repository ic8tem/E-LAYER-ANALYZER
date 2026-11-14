# INSTELLINGS

[← Home](Home-af)

Vanaf weergawe 0.4.18 bevat die oortjie **INSTELLINGS** die algemene opsies van die program en die verdeling van die spots tussen die oortjies. Dit het twee suboortjies.

## Algemeen

| Afdeling | Wat jy instel |
|---|---|
| Stasie | Roepnaam, lokator en hoogte, dieselfde as op die boonste balk; **Pas toe** bevestig hulle vir al die oortjies. |
| Voorkoms | Taal, kleurskema (die skemas van YAEC en YA-MS en, vanaf weergawe 0.5.0, die ses temas van IP-TRX-CTRL: Lig, Nag, Swart, Blou, Pers, Groen) en **lettergrootte van die hele program** (85-140 %, soos in YA-MS). Taal en kleure bou die venster weer op: instellings en spots bly behoue. |
| Kaarte en voorspellingsdata | Die kontinent, dieselfde kieslys as op die oortjiebalk. |
| Tyd | NTP-status en die verskil tussen die bedienertyd en die rekenaar se klok. |
| Opdaterings | Kontrole op GitHub by opstart en elke 30 minute, outomatiese en stil opdatering, knoppie **Kyk nou**. Outomatiese opdaterings geld nie vir stelselpakkette (.deb, .rpm, Arch, Slackware) nie. |
| Data | Knoppie wat die `data`-gids oopmaak. Die API-sleutel van die Bureau of Meteorology bly in DATA COLLECTION › Voortplantingsdata. |

## Spotverdeling

Links lys 'n tabel die reëls waarmee die program besluit waarheen elke spot gaan, **in die volgorde waarin hulle toegepas word**: die eerste reël wat pas, besluit die oortjie. Die gewysde waardes is dié wat in die filters ingestel is.

| # | As die spot... | gaan na |
|---|---|---|
| 1 | buite die plaaslike filter is (onder 27990 kHz of bo 145 MHz; 430-440 MHz slegs aurora) | weggegooi |
| 2 | AUE, AU-E of AURORAL E bevat | AURORA (aurora-E) |
| 3 | 'n aurorawoord bevat (by verstek AU, AUR) | AURORA |
| 4 | 'n F.A.I.-woord bevat (by verstek FAI; bande 6, 4 en 2 m: F.A.I. bereik nie 432 MHz nie) | F.A.I. |
| 5 | 'n meteor scatter-modus of `<MS>` het | METEOR SCATTER |
| 6 | in 'n MS-venster van die bandplan val (op 50 MHz nie uitsluitend nie; FT8 en FT4 op 50 en 70 MHz uitgesluit) | METEOR SCATTER |
| 7 | FT8 of FT4 op 50 MHz is sonder 'n ander spot binne 4 min met middelpunt binne 150 km | meteor scatter (nie Sporadic-E nie) |
| 8 | Q65 of JT65 op 144 MHz is (tensy `<ES>`) | EME: weggegooi |
| 9 | 'n etiket van 'n ander voortplanting bevat | nie Sporadic-E nie |
| 10 | transekwatoriaal is | nie Sporadic-E nie (TEP) |
| 11-15 | op 28, 40, 50, 70 of 144 MHz binne die afstandreëls is | SPORADIC-E |
| 16 | by geen reël pas nie (tropo, 432 MHz, HF...) | geen oortjie nie (bly in Versamelde spots) |

Regs is die **plaaslike filter** en die filters van elke modus, in vyf suboortjies: Meteor scatter (die YA-MS-filter word in die oortjie METEOR SCATTER ingestel), Sporadic-E, F.A.I., Aurora en **Toets die filters**, waar jy 'n klusterreël plak en sien hoe dit geklassifiseer word. Die knoppie **Herstel die reëls van die outeur** bring die beginwaardes terug.
