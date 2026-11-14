# INSTÄLLNINGAR

[← Home](Home-sv)

Från version 0.4.18 samlar fliken **INSTÄLLNINGAR** programmets allmänna alternativ och fördelningen av spots mellan flikarna. Den har två underflikar.

## Allmänt

| Avsnitt | Vad du ställer in |
|---|---|
| Station | Anropssignal, lokator och höjd, samma som i fältet högst upp; **Verkställ** bekräftar dem för alla flikar. |
| Utseende | Språk, färgschema (schemana från YAEC och YA-MS och, från version 0.5.0, de sex temana från IP-TRX-CTRL: Ljust, Natt, Svart, Blå, Lila, Grön) och **teckenstorlek för hela programmet** (85-140 %, som i YA-MS). Språk och färger bygger om fönstret: inställningar och spots behålls. |
| Kartor och prognosdata | Kontinenten, samma meny som i flikraden. |
| Tid | NTP-status och skillnaden mellan servertiden och datorns klocka. |
| Uppdateringar | Kontroll på GitHub vid start och var 30:e minut, automatisk och tyst uppdatering, knappen **Kontrollera nu**. Automatisk uppdatering gäller inte systempaket (.deb, .rpm, Arch, Slackware). |
| Data | Knapp som öppnar mappen `data`. API-nyckeln till Bureau of Meteorology stannar i DATA COLLECTION › Utbredningsdata. |

## Fördelning av spots

Till vänster listar en tabell de regler som programmet använder för att avgöra vart varje spot ska, **i den ordning de tillämpas**: den första regel som stämmer avgör fliken. De visade värdena är de som ställts in i filtren.

| # | Om spoten... | går till |
|---|---|---|
| 1 | ligger utanför det lokala filtret (under 27990 kHz eller över 145 MHz; 430-440 MHz bara aurora) | förkastad |
| 2 | innehåller AUE, AU-E eller AURORAL E | AURORA (aurora-E) |
| 3 | innehåller ett auroraord (standard AU, AUR) | AURORA |
| 4 | innehåller ett F.A.I.-ord (standard FAI; band 6, 4 och 2 m: F.A.I. når inte upp till 432 MHz) | F.A.I. |
| 5 | har en meteor scatter-mod eller `<MS>` | METEOR SCATTER |
| 6 | faller i ett MS-fönster i bandplanen (inte uteslutande på 50 MHz; FT8 och FT4 på 50 och 70 MHz undantagna) | METEOR SCATTER |
| 7 | är FT8 eller FT4 på 50 MHz utan någon annan spot inom 4 min och mittpunkt inom 150 km | meteor scatter (inte sporadiskt E) |
| 8 | är Q65 eller JT65 på 144 MHz (utom med `<ES>`) | EME: förkastad |
| 9 | innehåller en märkning för annan utbredning | inte sporadiskt E |
| 10 | är transekvatoriell | inte sporadiskt E (TEP) |
| 11-15 | är på 28, 40, 50, 70 eller 144 MHz inom avståndsreglerna | SPORADIC-E |
| 16 | stämmer inte med någon regel (tropo, 432 MHz, HF...) | ingen flik (stannar i Insamlade spots) |

Till höger finns det **lokala filtret** och filtren för varje utbredningssätt, i fem underflikar: Meteor scatter (YA-MS-filtret ställs in i fliken METEOR SCATTER), Sporadiskt E, F.A.I., Aurora och **Test av filtren**, där du klistrar in en klusterrad och ser hur den klassificeras. Knappen **Återställ författarens regler** återställer standardvärdena.
