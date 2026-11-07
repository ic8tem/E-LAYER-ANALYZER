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

The last 2000 spots, with band and source filters. Archive: one CSV file per day in `data/spot`.

## CALL3

JTDX **callsign → locator** database (over a million callsigns) in `data/call3.txt`. Buttons **Reload** and **Import a new CALL3.TXT**. 6-character locators read in spot comments are added to `data/call3_nuovi.txt`.
