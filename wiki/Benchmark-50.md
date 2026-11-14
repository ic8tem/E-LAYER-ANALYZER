# BENCHMARK 50 MHz

[← Home](Home)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Test bench for **multi-hop sporadic-E on 50 MHz** using the ADIF log of one station and, optionally, of a second station.

## Loading the logs

- **Station 1**: *Load ADIF* / *Unload ADIF*. The program takes `STATION_CALLSIGN` and `MY_GRIDSQUARE` from the log; the transmitting position is the `MY_GRIDSQUARE` of each QSO.
- **Station 2** (optional): same buttons on the second row; the *Station 2* checkbox shows or hides it. Station 1 paths are light blue, station 2 paths purple.
- The program uses 50 MHz QSOs with a locator (missing or 4-character locators are taken from CALL3), multi-hop: beyond 3100 km (author's rule; the number of hops follows the IK3XTV table).
- **Transequatorial** contacts are excluded (author's rule): towards Africa and South America, with the midpoint in the magnetic dip band between -15° and +15°, unless the log has `PROP_MODE=ES`.

## Day and time

Choose the **day** (on opening, the one with the most multi-hop QSOs) and the time **from… to…** UTC, in 15-minute steps. The **foEs** checkbox (optional) colours the paths by minimum foEs and shows it in the table.

## The map

**Mercator** map with a free centre: wheel = zoom, drag = pan.

![Path](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- A **blurred yellow dot** marks the centre of every hop, where the Es cloud is presumed to be; it gets bigger on the path under the mouse.
- **Mouse near a path**: the line and icons are enlarged and the DX station's **6-character WWL square** appears.
- **Left click** on that path: a popup asks whether to show **all QSOs in the file** (all days) with the far-end position **within 150 km** of that point. *Yes*: the map is cleared and shows those QSOs inside the orange circle; *No*: the map stays as it is. *Back to day and time* returns to the previous selection.

![Surroundings](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Details

For the chosen path: distance and IK3XTV table rating, azimuth in both directions, equal hops with the squares of the reflection points, short first hop within 1200 km (IC8TEM experience) and, with the foEs checkbox, the minimum foEs.
