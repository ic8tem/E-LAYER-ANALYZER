# SPORADIC-E

[← Home](Home)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## The map

A button above the map chooses the view:

- **Azimuthal**: azimuthal equidistant map centred on your station, with the **zoom** slider (300 to 20000 km from the centre, also with the mouse wheel).
- **Mercator**: the world map, with the Europe and World buttons.

In both projections the **distance circles of Flavio IK3XTV** are drawn around the station: 500 km (single-hop minimum), 1500-2000 km (optimum), 2400 km (single-hop maximum), 5000 km (double-hop maximum) and 6900 km (triple-hop maximum).

In the azimuthal views the dashed rings show the **km from the centre** (the step changes with the zoom), the edge marks the maximum distance shown, and the mouse cursor gives the distance and azimuth from the centre.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## What it shows

- The **spots** of the last few minutes (15 min - 12 h) on the **28, 40 (8 m, off by default), 50, 70 and 144 MHz** bands, taken from the telnet sessions. Locators come from the spot comment (e.g. `JN70CN<ES>JO62QM`) or from the CALL3 database.
- Spots with **<MS>**, **<AU>** or **<TR>** in the text are **discarded**: they do not concern sporadic-E but are useful for meteor scatter, aurora or tropo.
- **Transequatorial, author's rule (IC8TEM)**: towards Africa and South America, if the midpoint falls in the magnetic dip band between -20° and +20°, the contact is transequatorial (TEP) and not sporadic-E, unless the spot states `<ES>`.
- **144 MHz, author's rule (IC8TEM)**: sporadic-E on 2 m starts **beyond 900 km** (shorter spots are discarded). A single 144 MHz spot **is not plotted on its own**: it appears when within **5 minutes** a second 144 MHz spot arrives with the cloud in the same place (reflection points within 300 km), or when there is a **short-hop 50 MHz** spot nearby (within 1200 km, in the last 15 minutes). The bar at the top shows how many 144 MHz spots are waiting for confirmation.
- For each spot: distance, **number of hops** according to the **IK3XTV** table, elevation, **minimum foEs** (secant law).
- **First hop of both stations**: orange square = the spotter's first hop, purple = the DX's first hop (equal hops); yellow = midpoint of a single hop.
- **Short first hop** (hollow circles): in multi-hop openings the author has experienced a short first hop within **1200 km** of his own QTH; the length can be set.
- **Es clouds** on **Maidenhead squares** (2° x 1°), coloured by minimum foEs: blue < 5, light blue < 7, green < 10, yellow < 14, orange < 20, red > 20 MHz.
- **Es clouds** table: for each square, the **MUF for your station** (red = 144 MHz possible, orange = 70, yellow = 50).

## IK3XTV table

| | minimum | optimum | maximum |
|---|---|---|---|
| Single hop | 500-650 km | 1500-2000 km | 2200-2400 km |
| Double hop | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Triple hop | – | 4800-6400 km | 6900 km |

## How to use it

- **Click a spot**: the path is highlighted, with reflection points, locators and azimuths in the details.
- **Click a spot** in the table: the path lights up on the map (yellow glow) and, if it is out of view, the map moves or widens its radius to show it. The spot stays selected when new spots arrive.
- **The table follows the map**: with a zoom set, the spot table and the cloud table show only what is visible on the map.
- **Click the map**: the point is treated as a possible Es cloud, with the foEs needed for 28, 50, 70 and 144 MHz.

## Note on 432 MHz

Sporadic-E **does not apply to 432 MHz**: the Es MUF reaches about 150-200 MHz and the ARRL records list Es contacts only on 50 and 144 MHz. 70 cm spots are not analysed here.

## Squares coloured by frequency and MUF blinking

The Maidenhead squares of the reflection points are coloured by the highest spot frequency: **green** up to 29 MHz, **yellow** up to 51, **orange** up to 71, **red** up to 145 MHz. From the square's foEs the probable MUF on the longest hop (2400 km, IK3XTV table) is estimated: if it reaches a higher band, the square **blinks** in that band's colour (principle of IK3XTV and Jacobs-Leinwoll: short skip means high MUF).

At the top right of the map the N0NBH summary appears (E-skip for the chosen continent, SFI, K and A indices).

## Rules by band (version 0.4.18)

- **50 MHz**: no Sporadic-E within 450 km. FT8 and FT4 spots are Sporadic-E only if they belong to a cloud: another 50 MHz spot within 4 minutes with its midpoint within 150 km. An isolated FT8/FT4 spot goes to meteor scatter.
- **70 MHz**: no Sporadic-E within 450 km. FT8 and FT4 go to Sporadic-E even inside the meteor scatter window of the band plan (the known mode decides first, then the frequency).
- **40 MHz (8 m)**: minimum distance derived from the 450 km of 50 MHz with the secant law (about 330 km); only distances within the first hop of the E-layer geometry (about 2440 km). Path colour: yellow.
- **144 MHz**: Sporadic-E from 900 km, with confirmation; Q65 and JT65 spots are EME and are discarded, unless `<ES>` is in the text.
- G0ISW (Propagation identification, 2024) sees the shortest Sporadic-E beyond 400 km on 50 and 70 MHz and beyond 1400 km on 144 MHz: the author's rules agree on 50 and 70 MHz and are wider on 144 MHz, but ask for a confirmation.
- The order in which all rules apply, with the values in use, is shown in **[SETTINGS](Settings)** › Spot routing.
