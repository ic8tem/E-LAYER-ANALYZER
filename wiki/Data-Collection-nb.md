# DATA COLLECTION

[← Home](Home-nb)

## Telnet-økter

| Økt | Standard | Merknader |
|---|---|---|
| 1. Valgfri DX-cluster | on0dxk.dyndns.org 8000 | På ON0DXK er VHF-spot filtrert: `set/filter dxbm/pass 6` (deretter 4, 2). Et annet eksempel: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW og RTTY, 7001 = FT8. |
| 3. Cluster med PSKReporter | **dm4x.ddns.net 8500** | DM4X-cluster; alternativt HamAlert (hamalert.org 7300, krever konto). |

- Automatisk innlogging med kallesignalet (øverst eller i Login-feltet); **startkommandoer** skilt med `;`.
- **Koble til på nytt** etter 30 s hvis linjen faller ut; **Koble til ved oppstart**.
- Som standard lagres bare spot fra **28 MHz og oppover**.

## Innsamlede spot

De siste 2000 spottene, med filter for bånd og kilde. Arkiv: én CSV-fil per dag i `data/spot`. Hver gang programmet lukkes, tømmes spotdatabasen (minnet og filene i `data/spot`): hver økt starter på nytt.

**Lokalt filter** (valgfri avkrysningsboks): «DX de»-linjer under 27990,0 kHz eller over 145 MHz forkastes, vises ikke i konsollen og lagres ikke. **Upresise lokatorer** (færre enn 6 tegn) hentes fra CALL3: har CALL3 en lokator med 6 tegn, går den alltid foran. Som i YA-MS er meteor scatter-spot (ikke sporadisk E) de som ligger i de typiske MS-frekvensvinduene (50190-50300, 70100-70400, 144290-144399 kHz) uten tagger for en annen utbredelse (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), og de med modi som bare brukes til MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) eller med `<MS>`. Spot med `<AS>`/`<ACS>` (aircraft scatter) brukes ikke for sporadisk E.

## CALL3

JTDX-databasen **kallesignal → lokator** (over en million kallesignaler) i `data/call3.txt`. Knappene **Last inn på nytt** og **Importer en ny CALL3.TXT**. Lokatorer med 6 tegn som leses i spotkommentarene, legges til i `data/call3_nuovi.txt`.

Fra versjon 0.4.16 finnes én CALL3-database for hele programmet, den i YA-MS Datainnsamling (fanen METEOR SCATTER), med samme regler som LIVETROPOVHF: oppdatering fra MMMonVHF, håndoppføringer, tillagte filer, klyngekorreksjoner, lærte lokatorer og G7VRD hvert 30. minutt; korreksjonene skrives inn i data/call3.txt og lokatorene i data/call3_nuovi.txt importeres én gang. Spotter fra WSJT-X, JTDX og MSHV kommer også via UDP (kilde WSJT-X UDP, port 2237 videresendt til 2238).

Disse filtrene fra YA-MS (CW, minsteavstand, frekvensvinduer, MS-moduser, bånd) gjelder bare fanen METEOR SCATTER: de avgjør hvilke spotter som kommer inn i innsamlingen. De andre fanene får alle spotter og bruker sine egne filtre.
