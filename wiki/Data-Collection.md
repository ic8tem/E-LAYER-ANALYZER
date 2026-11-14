# DATA COLLECTION

[← Home](Home)

## Telnet sessions

| Session | Default | Notes |
|---|---|---|
| 1. DX cluster of your choice | on0dxk.dyndns.org 8000 | On ON0DXK the VHF spots are filtered: `set/filter dxbm/pass 6` (then 4, 2). Another example: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW and RTTY, 7001 = FT8. |
| 3. Cluster with PSKReporter | **dm4x.ddns.net 8500** | DM4X's cluster; alternatively HamAlert (hamalert.org 7300, account required). |

- Automatic login with the callsign (at the top or in the Login field); **initial commands** separated by `;`.
- **Reconnect** after 30 s if the line drops; **Connect at startup**.
- By default only spots from **28 MHz up** are saved.

## Collected spots

The last 2000 spots, with band and source filters. Archive: one CSV file per day in `data/spot`. Every time the program closes, the spot database is emptied (memory and the files in `data/spot`): each session starts from scratch.

**Local filter** (optional box, in SETTINGS): "DX de" lines below 27990.0 kHz or above 145 MHz are dropped, not shown in the console and not stored; between 430 and 440 MHz only aurora spots pass. **Imprecise locators** (fewer than 6 characters) are taken from CALL3: if CALL3 has a 6-character locator, it always prevails. As in YA-MS, meteor scatter (not Sporadic-E) spots are those in the typical MS frequency windows (50190-50300, 70100-70400, 144290-144399 kHz) without tags of another propagation (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), and those with MS-only modes (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) or with `<MS>`. Exception: FT8 and FT4 on 50 and 70 MHz, where a known mode decides (the 70.154 MHz FT8 goes to Sporadic-E even inside the MS window). Spots with `<AS>`/`<ACS>` (aircraft scatter) are not used for Sporadic-E.

## Spot filters

Data collection is **one for the whole program**: every spot goes through the filters and on to the tab of each propagation mode. From version 0.4.18 the filters, the local filter and the filter test are in the **[SETTINGS](Settings)** tab, sub-tab **Spot routing**.

## CALL3

JTDX **callsign → locator** database (over a million callsigns) in `data/call3.txt`. Buttons **Reload** and **Import a new CALL3.TXT**. 6-character locators read in spot comments are added to `data/call3_nuovi.txt`.

From version 0.4.16 there is a single CALL3 database for the whole program, the one of the YA-MS Data collection (METEOR SCATTER tab), with the same rules as LIVETROPOVHF: update from MMMonVHF, manual entries, added files, cluster corrections, learned locators and G7VRD every 6 hours; corrections are written into data/call3.txt and the locators of data/call3_nuovi.txt are imported once. Spots from WSJT-X, JTDX and MSHV also arrive via UDP (source WSJT-X UDP, port 2237 forwarded to 2238).

These filters, which come from YA-MS (CW, minimum distance, frequency windows, MS modes, bands), apply only to the METEOR SCATTER tab: they decide which spots enter its collection. The other tabs receive all spots and use their own filters.

## Spot and Propagation data sub-tabs

The tab has two sub-tabs: **Spot** (sessions, PSKReporter MQTT, WSJT-X UDP, collected spots and CALL3) and **Propagation data**:

- **N0NBH** (hamqsl.com): VHF banner and solar data, E-skip Europe and North America (DXrobot), aurora.
- **Bz for aurora**: NOAA SWPC real-time solar wind (DSCOVR/ACE, L1 point), the best source for Bz.
- **Meteor scatter**: daily meteor activity (RMOB) from Make More Miles on VHF.

They are downloaded at most once an hour. The summary appears on the SPORADIC-E, F.A.I. and AURORA maps. The **continent menu**, at the right of the tab bar, moves the maps and selects the data of that continent.
