# METEOR SCATTER

[← Home](Home)

This tab contains the complete **YA-MS 1.8.0**, IC8TEM's meteor scatter program: celestial sphere with the radiants, SKiYMET radar flux (Collm, Sodankylä), shower efficiency calculation, experimental implementation of the forecasts, geometry and attenuation, GRAVES, grid square map, **Live showers** (ZHR measured by the Global Meteor Network and radiants from the CMOR radar), **Sources map** (Collm, Sodankylä, GRAVES and CMOR relative to the station) and the **Efficiency - explanation** document with the formulas.

YA-MS 1.5.0 adds the **Data collection** tab (meteor scatter spots filtered by MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Inside E-LAYER ANALYZER it opens no telnet sessions of its own: it receives the lines of the DATA COLLECTION sessions, so there are no double logins with the same callsign.

YA-MS 1.6.0 reorganises the tabs into **Live and real-time data** (live meteor scatter traffic with the cloud of each contact, radar flux, GRAVES, data collection) and **Forecast calculation** (forecast, geometry, efficiency calculation, implementation of the forecasts).

YA-MS 1.7.0 and 1.8.0 add the comparison of the radiants with the IAU MDC catalogue, the daily Global Meteor Network counts per shower, the k index from the population indices, the initial trail radius of Stober et al. (2021), the ping duration across bands (λ² by default or the HamSCI MSQP measurements of NN4SA) and the comparison with the BRAMS detections at Sonneberg (Perseids 2026). The automatic update of YA-MS is switched off inside E-LAYER ANALYZER, which updates itself from the GitHub Releases.

- YA-MS uses the language and colour scheme chosen at the top of E-LAYER ANALYZER.
- YA-MS settings are stored in `data/config.ini`.
- Each new version of YA-MS is brought in with the `tools/sync_yams.py` script.

For details see the [YA-MS repository](https://github.com/ic8tem/YA-MS).
