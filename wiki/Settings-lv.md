# IESTATĪJUMI

[← Home](Home-lv)

Kopš versijas 0.4.18 cilne **IESTATĪJUMI** apkopo programmas vispārīgās opcijas un spotu sadali starp cilnēm. Tai ir divas apakšcilnes.

## Vispārīgi

| Sadaļa | Ko iestata |
|---|---|
| Stacija | Izsaukuma signāls, lokators un augstums, tie paši kā augšējā joslā; **Lietot** tos apstiprina visām cilnēm. |
| Izskats | Valoda, krāsu shēma (YAEC un YA-MS shēmas un, kopš versijas 0.5.0, sešas IP-TRX-CTRL tēmas: Gaiša, Nakts, Melna, Zila, Violeta, Zaļa) un **visas programmas fonta lielums** (85-140 %, tāpat kā YA-MS). Valoda un krāsas pārbūvē logu: iestatījumi un spoti saglabājas. |
| Kartes un prognožu dati | Kontinents, tā pati izvēlne kā cilņu joslā. |
| Laiks | NTP stāvoklis un starpība starp serveru laiku un datora pulksteni. |
| Atjauninājumi | Pārbaude GitHub startējot un ik pēc 6 stundām, automātiska un klusa atjaunināšana, poga **Pārbaudīt tagad**. Automātiskie atjauninājumi neattiecas uz sistēmas pakotnēm (.deb, .rpm, Arch, Slackware). |
| Dati | Poga, kas atver mapi `data`. Bureau of Meteorology API atslēga paliek DATA COLLECTION › Izplatīšanās dati. |

## Spotu sadale

Kreisajā pusē tabula uzskaita noteikumus, pēc kuriem programma izlemj, kur nonāk katrs spots, **secībā, kādā tos piemēro**: pirmais derīgais noteikums izlemj cilni. Parādītās vērtības ir tās, kas iestatītas filtros.

| # | Ja spots... | nonāk |
|---|---|---|
| 1 | ir ārpus lokālā filtra (zem 27990 kHz vai virs 145 MHz; 430-440 MHz tikai aurora) | atmests |
| 2 | satur AUE, AU-E vai AURORAL E | AURORA (aurora-E) |
| 3 | satur auroras vārdu (sākotnēji AU, AUR) | AURORA |
| 4 | satur F.A.I. vārdu (sākotnēji FAI; joslas 6, 4 un 2 m: F.A.I. nesasniedz 432 MHz) | F.A.I. |
| 5 | ir meteor scatter režīmā vai ar `<MS>` | METEOR SCATTER |
| 6 | iekrīt joslu plāna MS logā (50 MHz joslā neizslēdzošs; FT8 un FT4 50 un 70 MHz joslās izņemti) | METEOR SCATTER |
| 7 | ir FT8 vai FT4 50 MHz joslā bez cita spota 4 min laikā ar viduspunktu 150 km robežās | meteor scatter (nevis Sporadic-E) |
| 8 | ir Q65 vai JT65 144 MHz joslā (ja vien nav `<ES>`) | EME: atmests |
| 9 | satur citas izplatīšanās atzīmi | nav Sporadic-E |
| 10 | ir transekvatoriāls | nav Sporadic-E (TEP) |
| 11-15 | ir 28, 40, 50, 70 vai 144 MHz joslā attāluma noteikumu robežās | SPORADIC-E |
| 16 | neatbilst nevienam noteikumam (tropo, 432 MHz, HF...) | neviena cilne (paliek Savāktajos spotos) |

Labajā pusē ir **lokālais filtrs** un katra veida filtri piecās apakšcilnēs: Meteor scatter (YA-MS filtru iestata cilnē METEOR SCATTER), Sporādiskais E, F.A.I., Aurora un **Filtru pārbaude**, kur ielīmē klastera rindu un redz, kā tā tiek klasificēta. Poga **Atjaunot autora noteikumus** atjauno sākotnējās vērtības.
