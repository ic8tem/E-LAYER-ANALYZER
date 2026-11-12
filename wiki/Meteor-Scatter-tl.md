# METEOR SCATTER

[← Home](Home-tl)

Nasa tab na ito ang buong **YA-MS 1.8.0**, ang programang meteor scatter ni IC8TEM: celestial sphere na may mga radiant, flux ng SKiYMET radar (Collm, Sodankylä), kalkulasyon ng episyensya ng mga shower, eksperimental na pagpapatupad ng mga pagtataya, heometriya at attenuation, GRAVES, mapa ng grid square, **Mga live na shower** (ZHR na sinukat ng Global Meteor Network at mga radiant mula sa radar ng CMOR), **Mapa ng mga sanggunian** (Collm, Sodankylä, GRAVES at CMOR kaugnay ng istasyon) at ang dokumentong **Episyensya - paliwanag** kasama ang mga pormula.

Idinagdag ng YA-MS 1.5.0 ang tab na **Pangongolekta ng data** (mga spot na meteor scatter na sinala ayon sa MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Sa loob ng E-LAYER ANALYZER, hindi ito nagbubukas ng sarili nitong mga telnet session: tinatanggap nito ang mga linya ng mga session ng DATA COLLECTION, kaya walang dobleng login gamit ang parehong callsign.

Muling inaayos ng YA-MS 1.6.0 ang mga tab sa **Live at real-time na datos** (live na trapikong meteor scatter kasama ang ulap ng bawat contact, flux ng radar, GRAVES, pangongolekta ng data) at **Kalkulasyon ng pagtataya** (pagtataya, heometriya, kalkulasyon ng episyensya, pagpapatupad ng mga pagtataya).

Idinagdag ng YA-MS 1.7.0 at 1.8.0 ang paghahambing ng mga radiant sa katalogo ng IAU MDC, ang araw-araw na bilang ng Global Meteor Network bawat shower, ang k index na hinango mula sa mga population index, ang paunang radius ng bakas ayon kay Stober at iba pa (2021), ang tagal ng mga ping sa iba't ibang band (λ² bilang default o ang mga sukat ng HamSCI MSQP ni NN4SA) at ang paghahambing sa mga detection ng BRAMS sa Sonneberg (Perseids 2026). Naka-off ang awtomatikong update ng YA-MS sa loob ng E-LAYER ANALYZER, na nag-a-update nang mag-isa mula sa mga Release sa GitHub.

- Ginagamit ng YA-MS ang wika at scheme ng kulay na pinili sa itaas ng E-LAYER ANALYZER.
- Iniimbak ang mga setting ng YA-MS sa `data/config.ini`.
- Ipinapasok ang bawat bagong bersyon ng YA-MS gamit ang script na `tools/sync_yams.py`.

Para sa mga detalye, tingnan ang [repository ng YA-MS](https://github.com/ic8tem/YA-MS).
