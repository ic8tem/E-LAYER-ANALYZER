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

**Lokaal filter** (optioneel vakje): "DX de"-regels onder 27990,0 kHz of boven 145 MHz worden verworpen, niet in de console getoond en niet opgeslagen. **Onnauwkeurige locators** (minder dan 6 tekens) worden uit CALL3 gehaald: als CALL3 een locator van 6 tekens heeft, gaat die altijd voor. Net als in YA-MS gelden als meteorscatter-spots (geen sporadische E) die in de typische MS-frequentievensters (50190-50300, 70100-70400, 144290-144399 kHz) zonder aanduiding van een andere propagatie (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), en die met modes die alleen voor MS worden gebruikt (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) of met `<MS>`. Spots met `<AS>`/`<ACS>` (aircraft scatter) worden niet voor sporadische E gebruikt.

## CALL3

JTDX-database **roepnaam → locator** (ruim een miljoen roepnamen) in `data/call3.txt`. Knoppen **Herladen** en **Nieuw CALL3.TXT importeren**. Locators van 6 tekens die in het commentaar van spots worden gelezen, worden toegevoegd aan `data/call3_nuovi.txt`.

Vanaf versie 0.4.16 is er één CALL3-database voor het hele programma, die van de Gegevensverzameling van YA-MS (tabblad METEOR SCATTER), met dezelfde regels als LIVETROPOVHF: bijwerken vanaf MMMonVHF, handmatige invoer, toegevoegde bestanden, clustercorrecties, geleerde locators en G7VRD elke 6 uur; correcties worden in data/call3.txt geschreven en de locators uit data/call3_nuovi.txt worden één keer overgenomen. Spots van WSJT-X, JTDX en MSHV komen ook via UDP binnen (bron WSJT-X UDP, poort 2237 doorgestuurd naar 2238).
