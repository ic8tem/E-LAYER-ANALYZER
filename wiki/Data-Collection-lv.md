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

**Lokālais filtrs** (neobligāta rūtiņa, cilnē IESTATĪJUMI): "DX de" rindas zem 27990,0 kHz vai virs 145 MHz tiek atmestas, netiek rādītas konsolē un netiek saglabātas; starp 430 un 440 MHz iziet tikai auroras spoti. **Neprecīzi lokatori** (mazāk par 6 rakstzīmēm) tiek ņemti no CALL3: ja CALL3 ir 6 rakstzīmju lokators, tas vienmēr ir pārāks. Tāpat kā YA-MS, meteor scatter (nevis Sporadic-E) spoti ir tie, kas ir tipiskajos MS frekvenču logos (50190-50300, 70100-70400, 144290-144399 kHz) bez citas izplatīšanās birkām (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), un tie, kuros ir tikai MS lietotas modas (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) vai `<MS>`. Izņēmums: FT8 un FT4 50 un 70 MHz joslās, kur izšķir zināmais režīms (70,154 MHz FT8 nonāk Sporadic-E pat MS logā). Spoti ar `<AS>`/`<ACS>` (aircraft scatter) netiek izmantoti Sporadic-E.

## Spotu filtri

Datu vākšana **visai programmai ir viena**: katrs spots iziet caur filtriem un nonāk katra izplatīšanās veida cilnē. Kopš versijas 0.4.18 filtri, lokālais filtrs un filtru pārbaude atrodas cilnē **[IESTATĪJUMI](Settings-lv)**, apakšcilnē **Spotu sadale**.

## CALL3

JTDX **izsaukuma signāls → lokators** arhīvs (vairāk nekā miljons izsaukuma signālu) failā `data/call3.txt`. Pogas **Pārlādēt** un **Importēt jaunu CALL3.TXT**. Spotu komentāros nolasītie 6 rakstzīmju lokatori tiek pievienoti failam `data/call3_nuovi.txt`.

Sākot ar versiju 0.4.16, visai programmai ir viena CALL3 datubāze, YA-MS Datu vākšanas (cilne METEOR SCATTER), ar tiem pašiem noteikumiem kā LIVETROPOVHF: atjaunināšana no MMMonVHF ik pēc 12 stundām, rokas ieraksti, pievienotie faili, klastera labojumi, apgūtie lokatori, G7VRD ik pēc 30 minūtēm un kā pēdējais avots JTDX slānis (data/grid_data.bin), nekad neierakstīts call3.txt; labojumi tiek ierakstīti data/call3.txt, un data/call3_nuovi.txt lokatori tiek importēti vienreiz. WSJT-X, JTDX un MSHV spoti nāk arī caur UDP (avots WSJT-X UDP, ports 2237 ar pārsūtīšanu uz 2238).

Šie filtri no YA-MS (CW, minimālais attālums, frekvenču logi, MS režīmi, joslas) attiecas tikai uz cilni METEOR SCATTER: tie nosaka, kuri spoti nonāk tās krājumā. Pārējās cilnes saņem visus spotus un izmanto savus filtrus.

## Apakšcilnes Spoti un Izplatīšanās dati

Cilnei ir divas apakšcilnes: **Spoti** (sesijas, PSKReporter MQTT, WSJT-X UDP, savāktie spoti un CALL3) un **Izplatīšanās dati**:

- **N0NBH** (hamqsl.com): VHF baneris un saules dati, Eiropas un Ziemeļamerikas E-skip (DXrobot), aurora.
- **Bz aurorai**: NOAA SWPC saules vējš reāllaikā (DSCOVR/ACE, punkts L1), labākais Bz avots.
- **Meteor scatter**: dienas meteoru aktivitāte (RMOB) no Make More Miles on VHF.

Tos lejupielādē ne biežāk kā reizi stundā. Kopsavilkums parādās SPORADIC-E, F.A.I. un AURORA kartēs. **Kontinenta izvēlne** cilņu joslas labajā pusē pārvieto kartes un izvēlas šī kontinenta datus.
