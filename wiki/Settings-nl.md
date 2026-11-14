# INSTELLINGEN

[← Home](Home-nl)

Vanaf versie 0.4.18 verzamelt het tabblad **INSTELLINGEN** de algemene opties van het programma en de verdeling van de spots over de tabbladen. Het heeft twee subtabs.

## Algemeen

| Onderdeel | Wat je instelt |
|---|---|
| Station | Roepnaam, locator en hoogte, dezelfde als in de bovenste balk; **Toepassen** bevestigt ze voor alle tabbladen. |
| Weergave | Taal, kleurenschema (de schema's van YAEC en YA-MS en, vanaf versie 0.5.0, de zes thema's van IP-TRX-CTRL: Licht, Nacht, Zwart, Blauw, Paars, Groen) en **lettergrootte van het hele programma** (85-140 %, zoals in YA-MS). Taal en kleuren bouwen het venster opnieuw op: instellingen en spots blijven behouden. |
| Kaarten en voorspellingsgegevens | Het continent, hetzelfde menu als in de tabbladbalk. |
| Tijd | NTP-status en het verschil tussen de tijd van de servers en de pc-klok. |
| Updates | Controle op GitHub bij het opstarten en elke 30 minuten, automatische en stille update, knop **Nu controleren**. Automatische updates gelden niet voor systeempakketten (.deb, .rpm, Arch, Slackware). |
| Gegevens | Knop die de map `data` opent. De API-sleutel van het Bureau of Meteorology blijft in DATA COLLECTION › Propagatiegegevens. |

## Spotverdeling

Links somt een tabel de regels op waarmee het programma beslist waar elke spot naartoe gaat, **in de volgorde waarin ze worden toegepast**: de eerste regel die geldt, bepaalt het tabblad. De getoonde waarden zijn die welke in de filters zijn ingesteld.

| # | Als de spot... | gaat naar |
|---|---|---|
| 1 | buiten het lokale filter valt (onder 27990 kHz of boven 145 MHz; 430-440 MHz alleen aurora) | verworpen |
| 2 | AUE, AU-E of AURORAL E bevat | AURORA (aurora-E) |
| 3 | een aurorawoord bevat (standaard AU, AUR) | AURORA |
| 4 | een F.A.I.-woord bevat (standaard FAI; banden 6, 4 en 2 m: F.A.I. reikt niet tot 432 MHz) | F.A.I. |
| 5 | een meteorscatter-mode of `<MS>` heeft | METEOR SCATTER |
| 6 | in een MS-venster van het bandplan valt (op 50 MHz niet exclusief; FT8 en FT4 op 50 en 70 MHz uitgezonderd) | METEOR SCATTER |
| 7 | FT8 of FT4 op 50 MHz is zonder andere spot binnen 4 min met het middelpunt binnen 150 km | meteorscatter (geen sporadische E) |
| 8 | Q65 of JT65 op 144 MHz is (behalve met `<ES>`) | EME: verworpen |
| 9 | een aanduiding van een andere propagatie bevat | geen sporadische E |
| 10 | transequatoriaal is | geen sporadische E (TEP) |
| 11-15 | op 28, 40, 50, 70 of 144 MHz binnen de afstandsregels valt | SPORADIC-E |
| 16 | onder geen enkele regel valt (tropo, 432 MHz, HF...) | geen tabblad (blijft in Verzamelde spots) |

Rechts staan het **lokale filter** en de filters van elke mode, in vijf subtabs: Meteorscatter (het filter van YA-MS wordt ingesteld in het tabblad METEOR SCATTER), Sporadische E, F.A.I., Aurora en **Filtertest**, waarin je een clusterregel plakt om te zien hoe die wordt ingedeeld. De knop **Regels van de auteur herstellen** zet de standaardwaarden terug.
