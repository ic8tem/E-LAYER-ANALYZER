# SPORADIC-E

[← Home](Home)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## The map

A button above the map chooses the view:

- **Azimuthal - single hop**: azimuthal equidistant map centred on your station, fixed radius 2500 km, with the zones of the IK3XTV table (500 km minimum, 1500-2000 km optimum, 2400 km maximum single hop).
- **Azimuthal - maximum distance (multi-hop)**: the same map with the **Maximum distance** slider (from 300 to 20000 km, also with the mouse wheel); the limits of the double (5000 km) and triple hop (6900 km) also appear, so the evolution of multi-hop can be followed.
- **Rectangular**: the previous world map, with the Europe and World buttons.

In the azimuthal views the dashed rings show the **km from the centre** (the step changes with the zoom), the edge marks the maximum distance shown, and the mouse cursor gives the distance and azimuth from the centre.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## What it shows

- The **spots** of the last few minutes (15 min - 12 h) on the **28, 50, 70 and 144 MHz** bands, taken from the telnet sessions. Locators come from the spot comment (e.g. `JN70CN<ES>JO62QM`) or from the CALL3 database.
- Spots with **<MS>**, **<AU>** or **<TR>** in the text are **discarded**: they do not concern sporadic-E but are useful for meteor scatter, aurora or tropo.
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
- **Click the map**: the point is treated as a possible Es cloud, with the foEs needed for 28, 50, 70 and 144 MHz.

## Note on 432 MHz

Sporadic-E **does not apply to 432 MHz**: the Es MUF reaches about 150-200 MHz and the ARRL records list Es contacts only on 50 and 144 MHz. 70 cm spots are not analysed here.
