# DATA COLLECTION

[← Home](Home-da)

## Telnet-sessioner

| Session | Standard | Bemærkninger |
|---|---|---|
| 1. Valgfri DX-cluster | on0dxk.dyndns.org 8000 | På ON0DXK er VHF-spots filtreret: `set/filter dxbm/pass 6` (derefter 4, 2). Andet eksempel: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW og RTTY, 7001 = FT8. |
| 3. Cluster med PSKReporter | **dm4x.ddns.net 8500** | DM4X' cluster; alternativt HamAlert (hamalert.org 7300, kræver en konto). |

- Automatisk login med kaldesignalet (øverst eller i feltet Login); **startkommandoer** adskilt af `;`.
- **Genforbind** efter 30 s, hvis forbindelsen falder ud; **Forbind ved opstart**.
- Som standard gemmes kun spots fra **28 MHz og op**.

## Indsamlede spots

De seneste 2000 spots med filtre for bånd og kilde. Arkiv: én CSV-fil pr. dag i `data/spot`. Hver gang programmet lukkes, tømmes spotdatabasen (hukommelsen og filerne i `data/spot`): hver session starter forfra.

**Lokalt filter** (valgfrit afkrydsningsfelt): "DX de"-linjer under 27990,0 kHz eller over 145 MHz kasseres, vises ikke i konsollen og gemmes ikke. **Upræcise lokatorer** (færre end 6 tegn) hentes fra CALL3: hvis CALL3 har en lokator med 6 tegn, har den altid forrang. Som i YA-MS er meteor scatter-spots (ikke sporadisk E) dem, der ligger i de typiske MS-frekvensvinduer (50190-50300, 70100-70400, 144290-144399 kHz) uden tags for en anden udbredelse (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), og dem med modes, der kun bruges til MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M), eller med `<MS>`. Spots med `<AS>`/`<ACS>` (aircraft scatter) bruges ikke til sporadisk E.

## CALL3

JTDX-databasen **kaldesignal → lokator** (over en million kaldesignaler) i `data/call3.txt`. Knapperne **Genindlæs** og **Importér en ny CALL3.TXT**. Lokatorer med 6 tegn, der læses i spotkommentarerne, føjes til `data/call3_nuovi.txt`.

Fra version 0.4.16 er der én CALL3-database for hele programmet, den i YA-MS Dataindsamling (fanen METEOR SCATTER), med samme regler som LIVETROPOVHF: opdatering fra MMMonVHF, håndindtastninger, tilføjede filer, cluster-rettelser, lærte locatorer og G7VRD hver 6. time; rettelserne skrives ind i data/call3.txt og locatorerne i data/call3_nuovi.txt importeres én gang. Spots fra WSJT-X, JTDX og MSHV kommer også via UDP (kilde WSJT-X UDP, port 2237 videresendt til 2238).
