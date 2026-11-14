# DATA COLLECTION

[← Home](Home-sv)

## Telnetsessioner

| Session | Standard | Anmärkningar |
|---|---|---|
| 1. DX cluster som du väljer | on0dxk.dyndns.org 8000 | På ON0DXK är VHF-spotsen filtrerade: `set/filter dxbm/pass 6` (sedan 4, 2). Ett annat exempel: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW och RTTY, 7001 = FT8. |
| 3. Kluster med PSKReporter | **dm4x.ddns.net 8500** | DM4X:s kluster; alternativt HamAlert (hamalert.org 7300, konto krävs). |

- Automatisk inloggning med anropssignalen (högst upp eller i fältet Inloggning); **startkommandon** åtskilda med `;`.
- **Återanslut** efter 30 s om förbindelsen bryts; **Anslut vid start**.
- Som standard sparas bara spots från **28 MHz och uppåt**.

## Insamlade spots

De senaste 2000 spotsen, med filter för band och källa. Arkiv: en CSV-fil per dag i `data/spot`. Varje gång programmet stängs töms spotdatabasen (minnet och filerna i `data/spot`): varje session börjar från noll.

**Lokalt filter** (valfri kryssruta, i INSTÄLLNINGAR): "DX de"-rader under 27990,0 kHz eller över 145 MHz kasseras, visas inte i konsolen och sparas inte; mellan 430 och 440 MHz släpps bara auroraspots igenom. **Oprecisa lokatorer** (färre än 6 tecken) hämtas från CALL3: om CALL3 har en lokator med 6 tecken har den alltid företräde. Som i YA-MS är meteor scatter-spots (inte sporadiskt E) de som ligger i de typiska MS-frekvensfönstren (50190-50300, 70100-70400, 144290-144399 kHz) utan märkningar för annan utbredning (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), och de med moder som bara används för MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) eller med `<MS>`. Undantag: FT8 och FT4 på 50 och 70 MHz, där en känd mod avgör (FT8 på 70,154 MHz går till sporadiskt E även inom MS-fönstret). Spots med `<AS>`/`<ACS>` (aircraft scatter) används inte för sporadiskt E.

## Spotfilter

Det finns **bara en datainsamling för hela programmet**: varje spot passerar filtren och går vidare till fliken för varje utbredningssätt. Från version 0.4.18 finns filtren, det lokala filtret och testet av filtren i fliken **[INSTÄLLNINGAR](Settings-sv)**, underfliken **Fördelning av spots**.

## CALL3

JTDX-databas för **anropssignal → lokator** (över en miljon anropssignaler) i `data/call3.txt`. Knapparna **Läs om** och **Importera en ny CALL3.TXT**. Lokatorer med 6 tecken som läses i spotkommentarer läggs till i `data/call3_nuovi.txt`.

Från version 0.4.16 finns en enda CALL3-databas för hela programmet, den i YA-MS Datainsamling (fliken METEOR SCATTER), med samma regler som LIVETROPOVHF: uppdatering från MMMonVHF var 12:e timme, handposter, tillagda filer, klusterkorrigeringar, inlärda lokatorer, G7VRD var 30:e minut och, som sista källa, JTDX-lagret (data/grid_data.bin), som aldrig skrivs i call3.txt; korrigeringarna skrivs in i data/call3.txt och lokatorerna i data/call3_nuovi.txt importeras en gång. Spottar från WSJT-X, JTDX och MSHV kommer också via UDP (källa WSJT-X UDP, port 2237 vidarebefordrad till 2238).

Dessa filter från YA-MS (CW, minsta avstånd, frekvensfönster, MS-moder, band) gäller bara fliken METEOR SCATTER: de avgör vilka spottar som kommer in i dess insamling. De andra flikarna får alla spottar och använder sina egna filter.

## Underflikarna Spots och Utbredningsdata

Fliken har två underflikar: **Spots** (sessioner, PSKReporter MQTT, WSJT-X UDP, insamlade spots och CALL3) och **Utbredningsdata**:

- **N0NBH** (hamqsl.com): VHF-banner och soldata, E-skip Europa och Nordamerika (DXrobot), aurora.
- **Bz för aurora**: solvind i realtid från NOAA SWPC (DSCOVR/ACE, punkten L1), den bästa källan för Bz.
- **Meteor scatter**: dagens meteoraktivitet (RMOB) från Make More Miles on VHF.

De hämtas högst en gång i timmen. Sammanfattningen visas på kartorna i SPORADIC-E, F.A.I. och AURORA. **Kontinentmenyn**, till höger i flikraden, flyttar kartorna och väljer data för den kontinenten.
