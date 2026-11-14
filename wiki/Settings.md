# SETTINGS

[← Home](Home)

From version 0.4.18 the **SETTINGS** tab gathers the general options of the program and the routing of the spots between the tabs. It has two sub-tabs.

## General

| Section | What you set |
|---|---|
| Station | Callsign, locator and height, the same as in the top bar; **Apply** confirms them for all tabs. |
| Appearance | Language, colour scheme (the YAEC and YA-MS schemes and, from version 0.5.0, the six IP-TRX-CTRL themes: Light, Night, Black, Blue, Purple, Green) and **font size of the whole program** (85-140 %, as in YA-MS). Language and colours rebuild the window: settings and spots are kept. |
| Maps and forecast data | The continent, the same menu as in the tab bar. |
| Time | NTP status and the difference between the server time and the PC clock. |
| Updates | Check on GitHub at start-up and every 30 minutes, automatic and silent update, **Check now** button. Automatic updates do not apply to system packages (.deb, .rpm, Arch, Slackware). |
| Data | Button that opens the `data` folder. The Bureau of Meteorology API key stays in DATA COLLECTION › Propagation data. |

## Spot routing

On the left a table lists the rules the program uses to decide where each spot goes, **in the order in which they apply**: the first rule that matches decides the tab. The values shown are those set in the filters.

| # | If the spot... | goes to |
|---|---|---|
| 1 | is outside the local filter (below 27990 kHz or above 145 MHz; 430-440 MHz only aurora) | discarded |
| 2 | contains AUE, AU-E or AURORAL E | AURORA (aurora-E) |
| 3 | contains an aurora word (default AU, AUR) | AURORA |
| 4 | contains an F.A.I. word (default FAI; bands 6, 4 and 2 m: F.A.I. does not reach 432 MHz) | F.A.I. |
| 5 | has a meteor scatter mode or `<MS>` | METEOR SCATTER |
| 6 | falls in an MS window of the band plan (not exclusive on 50 MHz; FT8 and FT4 on 50 and 70 MHz excluded) | METEOR SCATTER |
| 7 | is FT8 or FT4 on 50 MHz with no other spot within 4 min and midpoint within 150 km | meteor scatter (not Sporadic-E) |
| 8 | is Q65 or JT65 on 144 MHz (unless `<ES>`) | EME: discarded |
| 9 | contains a tag of another propagation | not Sporadic-E |
| 10 | is trans-equatorial | not Sporadic-E (TEP) |
| 11-15 | is on 28, 40, 50, 70 or 144 MHz within the distance rules | SPORADIC-E |
| 16 | matches no rule (tropo, 432 MHz, HF...) | no tab (stays in Collected spots) |

On the right are the **local filter** and the filters of each mode, in five sub-tabs: Meteor scatter (the YA-MS filter is set in the METEOR SCATTER tab), Sporadic-E, F.A.I., Aurora and **Filter test**, where you paste a cluster line and see how it is classified. The **Restore the author's rules** button brings back the defaults.
