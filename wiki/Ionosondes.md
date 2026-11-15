# IONOSONDES

[← Home](Home)

![IONOSONDES](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Sub-tab of **SPORADIC-E** (from version 0.5.2): the **foEs measured** by the ionosondes of the **GIRO** network all over the world, next to the one the program derives from the spots.

## The data

- 63 GIRO ionosondes (ULCAR list), with the latest **foEs**, **foF2** and **MUF(D)** read automatically from the ionograms.
- The data arrive the first time the tab is opened and then **every 15 minutes**, by themselves (checkbox *Update by itself every 15 minutes*); *Update now* requests them at once. The archive keeps the last 48 hours in the `giro` folder of the data.
- Data from **GIRO** (Reinisch and Galkin 2011), licence **CC BY-NC-SA 4.0**: non-commercial use only. The service is the same as in *MUF Muncher* by DM6AX.

## Map and table

- Each ionosonde is a coloured dot with the latest foEs (same colours as the Es clouds) and the value in MHz; **grey** if it has no recent data, with the **ring** if the last ionogram is more than 90 minutes old.
- Table: Code, Ionosonde, km from your station, UTC of the last ionogram, foEs, foF2, MUF(D) and **Spot foEs≥**: the minimum foEs derived from the Es spots of the last 30 minutes that reflect within 500 km of the ionosonde (adjustable), with the number of spots.

## The selected ionosonde

Click on a row or near an ionosonde on the map:

- **chart of the last 24 hours**: foEs yellow, foF2 light blue, MUF(D) violet dotted;
- **sporadic-E MUF** at 1000, 1500, 2000 and 2400 km (MUF = foEs · sec φ, at the chosen Es height) with the **bands that open** (28, 40, 50, 70, 144 MHz);
- if the spots require more than the measured foEs, the program flags it: the cloud may be far from the ionosonde, or the ionosonde has reached the limit of its frequency range (Chartier 2022).
