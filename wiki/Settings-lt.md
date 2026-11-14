# NUSTATYMAI

[← Home](Home-lt)

Nuo 0.4.18 versijos kortelėje **NUSTATYMAI** surinktos bendrosios programos parinktys ir spotų skirstymas tarp kortelių. Ji turi dvi pakorteles.

## Bendrieji

| Skyrius | Ką nustatote |
|---|---|
| Stotis | Šaukinys, lokatorius ir aukštis, tie patys kaip viršutinėje juostoje; **Taikyti** juos patvirtina visoms kortelėms. |
| Išvaizda | Kalba, spalvų schema ir **visos programos šrifto dydis** (85-140 %, kaip YA-MS). Kalba ir spalvos perkuria langą: nustatymai ir spotai išlieka. |
| Žemėlapiai ir prognozių duomenys | Žemynas, tas pats meniu kaip kortelių juostoje. |
| Laikas | NTP būsena ir serverių laiko bei kompiuterio laikrodžio skirtumas. |
| Atnaujinimai | Patikra GitHub paleidžiant ir kas 6 valandas, automatinis ir tylus atnaujinimas, mygtukas **Tikrinti dabar**. Automatiniai atnaujinimai netaikomi sistemos paketams (.deb, .rpm, Arch, Slackware). |
| Duomenys | Mygtukas, atveriantis aplanką `data`. Bureau of Meteorology API raktas lieka DATA COLLECTION › Sklidimo duomenys. |

## Spotų skirstymas

Kairėje lentelėje išvardytos taisyklės, pagal kurias programa nusprendžia, kur patenka kiekvienas spotas, **ta tvarka, kuria jos taikomos**: pirmoji tinkanti taisyklė nusprendžia kortelę. Rodomos reikšmės yra nustatytos filtruose.

| # | Jei spotas... | patenka į |
|---|---|---|
| 1 | yra už vietinio filtro ribų (žemiau 27990 kHz arba aukščiau 145 MHz; 430-440 MHz tik pašvaistė) | atmestas |
| 2 | turi AUE, AU-E arba AURORAL E | AURORA (pašvaistė-E) |
| 3 | turi pašvaistės žodį (pagal nutylėjimą AU, AUR) | AURORA |
| 4 | turi F.A.I. žodį (pagal nutylėjimą FAI; juostos 6, 4 ir 2 m: F.A.I. nesiekia 432 MHz) | F.A.I. |
| 5 | turi meteor scatter režimą arba `<MS>` | METEOR SCATTER |
| 6 | patenka į juostų plano MS langą (50 MHz juostoje neišskirtinis; FT8 ir FT4 50 ir 70 MHz juostose išskyrus) | METEOR SCATTER |
| 7 | yra FT8 arba FT4 50 MHz juostoje be kito spoto per 4 min su vidurio tašku iki 150 km | meteor scatter (ne sporadinis E) |
| 8 | yra Q65 arba JT65 144 MHz juostoje (nebent `<ES>`) | EME: atmestas |
| 9 | turi kito sklidimo žymą | ne sporadinis E |
| 10 | yra transekvatorinis | ne sporadinis E (TEP) |
| 11-15 | yra 28, 40, 50, 70 arba 144 MHz juostoje atstumo taisyklių ribose | SPORADIC-E |
| 16 | neatitinka jokios taisyklės (tropo, 432 MHz, HF...) | jokia kortelė (lieka Surinktuose spotuose) |

Dešinėje yra **vietinis filtras** ir kiekvieno būdo filtrai penkiose pakortelėse: Meteor scatter (YA-MS filtras nustatomas kortelėje METEOR SCATTER), Sporadinis E, F.A.I., Pašvaistė ir **Filtrų bandymas**, kur įklijuojate klasterio eilutę ir matote, kaip ji klasifikuojama. Mygtukas **Atkurti autoriaus taisykles** grąžina pradines reikšmes.
