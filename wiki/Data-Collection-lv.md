# DATA COLLECTION

[← Home](Home-lv)

## Telnet sesijas

| Sesija | Noklusējums | Piezīmes |
|---|---|---|
| 1. DX klasteris pēc izvēles | on0dxk.dyndns.org 8000 | ON0DXK VHF spoti ir filtrēti: `set/filter dxbm/pass 6` (tad 4, 2). Cits piemērs: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW un RTTY, 7001 = FT8. |
| 3. Klasteris ar PSKReporter | **dm4x.ddns.net 8500** | DM4X klasteris; alternatīvi HamAlert (hamalert.org 7300, vajadzīgs konts). |

- Automātiska pieteikšanās ar izsaukuma signālu (augšā vai laukā Login); **sākuma komandas**, atdalītas ar `;`.
- **Savienot vēlreiz** pēc 30 s, ja savienojums pārtrūkst; **Savienot startējot**.
- Noklusējumā tiek saglabāti tikai spoti **no 28 MHz uz augšu**.

## Savāktie spoti

Pēdējie 2000 spoti ar joslu un avotu filtriem. Arhīvs: viens CSV fails dienā mapē `data/spot`. Katru reizi, kad programma tiek aizvērta, spotu datubāze tiek iztukšota (atmiņa un faili mapē `data/spot`): katra sesija sākas no nulles.

**Lokālais filtrs** (neobligāta rūtiņa): "DX de" rindas zem 27990,0 kHz vai virs 145 MHz tiek atmestas, netiek rādītas konsolē un netiek saglabātas. **Neprecīzi lokatori** (mazāk par 6 rakstzīmēm) tiek ņemti no CALL3: ja CALL3 ir 6 rakstzīmju lokators, tas vienmēr ir pārāks. Tāpat kā YA-MS, meteor scatter (nevis Sporadic-E) spoti ir tie, kas ir tipiskajos MS frekvenču logos (50190-50300, 70100-70400, 144290-144399 kHz) bez citas izplatīšanās birkām (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), un tie, kuros ir tikai MS lietotas modas (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) vai `<MS>`. Spoti ar `<AS>`/`<ACS>` (aircraft scatter) netiek izmantoti Sporadic-E.

## CALL3

JTDX **izsaukuma signāls → lokators** arhīvs (vairāk nekā miljons izsaukuma signālu) failā `data/call3.txt`. Pogas **Pārlādēt** un **Importēt jaunu CALL3.TXT**. Spotu komentāros nolasītie 6 rakstzīmju lokatori tiek pievienoti failam `data/call3_nuovi.txt`.
