# DATA COLLECTION

[← Home](Home-af)

## Telnet-sessies

| Sessie | Verstek | Opmerkings |
|---|---|---|
| 1. DX-cluster na keuse | on0dxk.dyndns.org 8000 | Op ON0DXK word die VHF-spots gefiltreer: `set/filter dxbm/pass 6` (dan 4, 2). Nog 'n voorbeeld: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW en RTTY, 7001 = FT8. |
| 3. Cluster met PSKReporter | **dm4x.ddns.net 8500** | DM4X se cluster; alternatiewelik HamAlert (hamalert.org 7300, rekening nodig). |

- Outomatiese aanmelding met die roepnaam (bo-aan of in die Aanmelding-veld); **beginopdragte** geskei deur `;`.
- **Herkoppel** ná 30 s as die lyn wegval; **Koppel by opstart**.
- By verstek word slegs spots vanaf **28 MHz en hoër** gestoor.

## Versamelde spots

Die laaste 2000 spots, met band- en bronfilters. Argief: een CSV-lêer per dag in `data/spot`. Elke keer as die program sluit, word die spotdatabasis leeggemaak (geheue en die lêers in `data/spot`): elke sessie begin van voor af.

**Plaaslike filter** (opsionele blokkie, in INSTELLINGS): "DX de"-reëls onder 27990,0 kHz of bo 145 MHz word weggegooi, nie in die konsole gewys nie en nie gestoor nie; tussen 430 en 440 MHz gaan net aurora-spots deur. **Onakkurate lokators** (minder as 6 karakters) word uit CALL3 geneem: as CALL3 'n lokator van 6 karakters het, kry dit altyd voorrang. Soos in YA-MS is meteor scatter-spots (nie Sporadic-E nie) dié in die tipiese MS-frekwensievensters (50190-50300, 70100-70400, 144290-144399 kHz) sonder etikette van 'n ander voortplanting (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), en dié met modusse wat slegs vir MS gebruik word (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) of met `<MS>`. Uitsondering: FT8 en FT4 op 50 en 70 MHz, waar 'n bekende modus besluit (die FT8 van 70,154 MHz gaan na Sporadic-E selfs binne die MS-venster). Spots met `<AS>`/`<ACS>` (aircraft scatter) word nie vir Sporadic-E gebruik nie.

## Spotfilters

Daar is **net een data-insameling vir die hele program**: elke spot gaan deur die filters en na die oortjie van elke voortplantingsmodus. Vanaf weergawe 0.4.18 is die filters, die plaaslike filter en die filtertoets in die oortjie **[INSTELLINGS](Settings-af)**, suboortjie **Spotverdeling**.

## CALL3

JTDX se **roepnaam → lokator**-databasis (meer as 'n miljoen roepname) in `data/call3.txt`. Knoppies **Herlaai** en **Voer 'n nuwe CALL3.TXT in**. Lokators van 6 karakters wat in spotkommentare gelees word, word by `data/call3_nuovi.txt` gevoeg.

Vanaf weergawe 0.4.16 is daar een CALL3-databasis vir die hele program, dié van die YA-MS Data-insameling (oortjie METEOR SCATTER), met dieselfde reëls as LIVETROPOVHF: bywerking vanaf MMMonVHF, handinskrywings, bygevoegde lêers, cluster-regstellings, geleerde lokators en G7VRD elke 6 uur; regstellings word in data/call3.txt geskryf en die lokators van data/call3_nuovi.txt word een keer ingevoer. Spots van WSJT-X, JTDX en MSHV kom ook oor UDP (bron WSJT-X UDP, poort 2237 aangestuur na 2238).

Hierdie filters uit YA-MS (CW, minimum afstand, frekwensievensters, MS-modusse, bande) geld net vir die oortjie METEOR SCATTER: hulle besluit watter spots in sy versameling kom. Die ander oortjies kry al die spots en gebruik hul eie filters.

## Suboortjies Spots en Voortplantingsdata

Die oortjie het twee suboortjies: **Spots** (sessies, PSKReporter MQTT, WSJT-X UDP, versamelde spots en CALL3) en **Voortplantingsdata**:

- **N0NBH** (hamqsl.com): VHF-banier en sondata, E-skip vir Europa en Noord-Amerika (DXrobot), aurora.
- **Bz vir aurora**: intydse sonwind van NOAA SWPC (DSCOVR/ACE, punt L1), die beste bron vir Bz.
- **Meteor scatter**: daaglikse meteooraktiwiteit (RMOB) van Make More Miles on VHF.

Dit word hoogstens een keer per uur afgelaai. Die opsomming verskyn op die SPORADIC-E-, F.A.I.- en AURORA-kaarte. Die **kontinentkieslys**, regs op die oortjiebalk, skuif die kaarte en kies die data van daardie kontinent.
