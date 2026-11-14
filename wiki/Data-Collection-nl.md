# DATA COLLECTION

[← Home](Home-nl)

## Telnet-sessies

| Sessie | Standaard | Opmerkingen |
|---|---|---|
| 1. DX-cluster naar keuze | on0dxk.dyndns.org 8000 | Op ON0DXK zijn de VHF-spots gefilterd: `set/filter dxbm/pass 6` (daarna 4, 2). Een ander voorbeeld: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW en RTTY, 7001 = FT8. |
| 3. Cluster met PSKReporter | **dm4x.ddns.net 8500** | Cluster van DM4X; als alternatief HamAlert (hamalert.org 7300, account vereist). |

- Automatisch inloggen met de roepnaam (bovenaan of in het veld Login); **commando's na het inloggen**, gescheiden door `;`.
- **Opnieuw verbinden** na 30 s als de verbinding wegvalt; **Verbinden bij opstarten**.
- Standaard worden alleen spots **vanaf 28 MHz** opgeslagen.

## Verzamelde spots

De laatste 2000 spots, met filters op band en bron. Archief: één CSV-bestand per dag in `data/spot`. Telkens als het programma wordt afgesloten, wordt de spotdatabase leeggemaakt (geheugen en de bestanden in `data/spot`): elke sessie begint opnieuw.

**Lokaal filter** (optioneel vakje, in INSTELLINGEN): "DX de"-regels onder 27990,0 kHz of boven 145 MHz worden verworpen, niet in de console getoond en niet opgeslagen; tussen 430 en 440 MHz komen alleen aurora-spots door. **Onnauwkeurige locators** (minder dan 6 tekens) worden uit CALL3 gehaald: als CALL3 een locator van 6 tekens heeft, gaat die altijd voor. Net als in YA-MS gelden als meteorscatter-spots (geen sporadische E) die in de typische MS-frequentievensters (50190-50300, 70100-70400, 144290-144399 kHz) zonder aanduiding van een andere propagatie (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), en die met modes die alleen voor MS worden gebruikt (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) of met `<MS>`. Uitzondering: FT8 en FT4 op 50 en 70 MHz, waar een bekende mode beslist (de FT8 van 70,154 MHz gaat naar sporadische E, ook binnen het MS-venster). Spots met `<AS>`/`<ACS>` (aircraft scatter) worden niet voor sporadische E gebruikt.

## Spotfilters

Er is **maar één gegevensverzameling voor het hele programma**: elke spot gaat door de filters en naar het tabblad van de betreffende propagatiemode. Vanaf versie 0.4.18 staan de filters, het lokale filter en de filtertest in het tabblad **[INSTELLINGEN](Settings-nl)**, subtab **Spotverdeling**.

## CALL3

JTDX-database **roepnaam → locator** (ruim een miljoen roepnamen) in `data/call3.txt`. Knoppen **Herladen** en **Nieuw CALL3.TXT importeren**. Locators van 6 tekens die in het commentaar van spots worden gelezen, worden toegevoegd aan `data/call3_nuovi.txt`.

Vanaf versie 0.4.16 is er één CALL3-database voor het hele programma, die van de Gegevensverzameling van YA-MS (tabblad METEOR SCATTER), met dezelfde regels als LIVETROPOVHF: bijwerken vanaf MMMonVHF elke 12 uur, handmatige invoer, toegevoegde bestanden, clustercorrecties, geleerde locators, G7VRD elke 30 minuten en, als laatste bron, de JTDX-laag (data/grid_data.bin), nooit in call3.txt geschreven; correcties worden in data/call3.txt geschreven en de locators uit data/call3_nuovi.txt worden één keer overgenomen. Spots van WSJT-X, JTDX en MSHV komen ook via UDP binnen (bron WSJT-X UDP, poort 2237 doorgestuurd naar 2238).

Deze filters uit YA-MS (CW, minimumafstand, frequentievensters, MS-modi, banden) gelden alleen voor het tabblad METEOR SCATTER: ze bepalen welke spots in de verzameling komen. De andere tabbladen krijgen alle spots en gebruiken hun eigen filters.

## Subtabs Spot en Propagatiegegevens

Het tabblad heeft twee subtabs: **Spot** (sessies, PSKReporter MQTT, WSJT-X UDP, verzamelde spots en CALL3) en **Propagatiegegevens**:

- **N0NBH** (hamqsl.com): VHF-banner en zonnegegevens, E-skip Europa en Noord-Amerika (DXrobot), aurora.
- **Bz voor aurora**: realtime zonnewind van NOAA SWPC (DSCOVR/ACE, punt L1), de beste bron voor de Bz.
- **Meteorscatter**: dagelijkse meteoractiviteit (RMOB) van Make More Miles on VHF.

Ze worden hoogstens eenmaal per uur gedownload. De samenvatting verschijnt op de kaarten SPORADIC-E, F.A.I. en AURORA. Het **continentmenu**, rechts in de tabbladbalk, verplaatst de kaarten en kiest de gegevens van dat continent.
