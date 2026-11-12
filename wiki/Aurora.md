# AURORA

[← Home](Home)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **auroral oval** (shaded), **Kp** of the last 3 days, **solar wind** (Bz, speed, density), updated every 5 minutes and saved in `data/swpc`.
- The same views as F.A.I.: **Scatter trail** for a pair of stations and **Isoflection** to check a point, with a default height of 110 km.
- **Recommended beam heading for both stations**: along each station's line of perpendicularity (dots in its colour), the point with the highest oval probability. Rule of thumb (G3WPO): start at about 45°, between north and east.
- Example IO70VJ - JO59BC: scatter point JP01AK off the Shetlands, deviation 0.97°, QTF 11° and 299°.
- **Aurora spots on the map**: spots with **<AU>** or **<AUR>** in the text are aurora spots (author's rule). With the box *<AU>/<AUR> spots on the map with the calculation* ticked, the tab plots the spots of the last 2 hours (at most 30): for each spotter-DX pair it computes the scatter trail and shows the most likely scatter point (orange circle), with the QTF and elevation of both stations in the results.
- Besides the most likely scatter point, the results give the **QTF range** of both stations along the whole Scatter trail (deviation ≤ 1.5°).
- **Lange-Hesse window**: the scatter point is preferred within 370-1100 km of each station, the window Lange-Hesse computed at 54° geomagnetic latitude. From version 0.4.5 the window moves with the IGRF-14 inclination of each station: closer for stations further south, farther for stations further north (e.g. Capri about 260-990 km, Oslo 470-1200 km). The results show the window of both stations.

## BENCHMARK AURORA

The AURORA tab has two sub-tabs: **REAL-TIME** (the calculation described above) and **BENCHMARK AURORA**, a test bench of the model on **298 real aurora QSOs** with time and locator: the published logs of EI5FK (IO51RT, Ireland, 2003-2005, 144 and 432 MHz) and the log map of PA0O (JO33HG, Netherlands).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- For each QSO the program computes the scatter point with the same model (IGRF-14, height 110 km, 4/3 refraction, tolerance 3°).
- Colours: **green** = scatter point inside the Lange-Hesse window of both stations; **yellow** = explained but outside that window; **orange** = QSO beyond 2000 km; **red** = no scatter point, QSO not explained by the model.
- The summary gives the percentages, also with the fixed 370-1100 km window for comparison, and, for each log, the range and median of the computed QTF. Click a row to draw the path and both beam headings.
- Result (version 0.4.4): **296 of 298 QSOs explained (99%)**, 243 (82%) inside the 370-1100 km window; median QTF of EI5FK between 25° and 41°, i.e. the north-east suggested by G3WPO. The logs do not give the beam heading: the check is on geometry, not on QTF. With the moving window (version 0.4.5) the result is the same, 243 QSOs (82%), as with the fixed window: the stations of the test bench (Ireland, Great Britain, Netherlands, Germany) have an inclination of 66-70°, close to the Lange-Hesse reference (window of EI5FK 350-1080 km); the difference concerns stations further south or further north, for which the test bench has no QSOs.
