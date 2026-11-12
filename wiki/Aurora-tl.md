# AURORA

[← Home](Home-tl)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Auroral oval** ng NOAA OVATION Prime (may lilim), **Kp** ng huling 3 araw, **solar wind** (Bz, bilis, densidad), ina-update bawat 5 minuto at sine-save sa `data/swpc`.
- Parehong mga tanaw ng F.A.I.: **Scatter trail** para sa isang pares ng istasyon at **Iso-repleksiyon** para suriin ang isang punto, na may default na taas na 110 km.
- **Inirerekomendang tutok ng beam para sa dalawang istasyon**: sa kahabaan ng linya ng perpendicularity ng bawat istasyon (mga tuldok sa kulay nito), ang puntong may pinakamataas na probabilidad ng oval. Praktikal na tuntunin (G3WPO): magsimula sa mga 45°, sa pagitan ng hilaga at silangan.
- Halimbawa IO70VJ - JO59BC: scatter point JP01AK sa labas ng Shetlands, paglihis 0.97°, QTF 11° at 299°.
- **Mga spot ng aurora sa mapa**: ang mga spot na may **<AU>** o **<AUR>** sa teksto ay mga spot ng aurora (tuntunin ng may-akda). Kapag naka-tsek ang kahong *Mga spot na <AU>/<AUR> sa mapa na may kalkulasyon*, iginuguhit ng tab ang mga spot ng huling 2 oras (hanggang 30): para sa bawat pares na spotter-DX, kinakalkula nito ang scatter trail at ipinapakita ang pinakamalamang na scatter point (kahel na bilog), kasama ang QTF at elebasyon ng dalawang istasyon sa mga resulta.
- Bukod sa pinakamalamang na scatter point, ibinibigay ng mga resulta ang **saklaw ng QTF** ng dalawang istasyon sa buong Scatter trail (paglihis ≤ 1.5°).
- **Saklaw ni Lange-Hesse**: mas pinipili ang scatter point sa loob ng 370-1100 km mula sa bawat istasyon, ang saklaw na kinalkula ni Lange-Hesse sa 54° geomagnetic latitude. Mula sa bersiyon 0.4.5, gumagalaw ang saklaw ayon sa IGRF-14 inclination ng bawat istasyon: mas malapit para sa mga istasyong mas nasa timog, mas malayo para sa mga istasyong mas nasa hilaga (halimbawa Capri mga 260-990 km, Oslo 470-1200 km). Ibinibigay ng mga resulta ang saklaw ng dalawang istasyon.

## BENCHMARK AURORA

May dalawang sub-tab ang tab na AURORA: **REAL-TIME** (ang kalkulasyong inilarawan sa itaas) at **BENCHMARK AURORA**, isang pagsubok ng modelo sa **298 totoong QSO ng aurora** na may oras at locator: ang mga nailathalang log ni EI5FK (IO51RT, Ireland, 2003-2005, 144 at 432 MHz) at ang mapa ng log ni PA0O (JO33HG, Netherlands).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Para sa bawat QSO, kinakalkula ng programa ang scatter point gamit ang parehong modelo (IGRF-14, taas na 110 km, repraksiyong 4/3, tolerance na 3°).
- Mga kulay: **berde** = scatter point sa loob ng saklaw ni Lange-Hesse ng dalawang istasyon; **dilaw** = naipaliwanag pero labas sa saklaw na iyon; **kahel** = QSO na lampas sa 2000 km; **pula** = walang scatter point, QSO na hindi naipapaliwanag ng modelo.
- Ibinibigay ng buod ang mga porsiyento, pati gamit ang nakapirming saklaw na 370-1100 km bilang paghahambing, at, para sa bawat log, ang saklaw at median ng kinalkulang QTF. Mag-click sa isang hanay para iguhit ang landas at ang dalawang direksiyon ng antena.
- Resulta (bersiyon 0.4.4): **296 sa 298 QSO ang naipaliwanag (99%)**, 243 (82%) sa loob ng saklaw na 370-1100 km; median na QTF ni EI5FK sa pagitan ng 25° at 41°, ibig sabihin ang hilagang-silangan na iminumungkahi ni G3WPO. Hindi nakatala sa mga log ang direksiyon ng antena: ang pagsusuri ay sa heometriya, hindi sa QTF. Sa gumagalaw na saklaw (bersiyon 0.4.5) pareho ang resulta, 243 QSO (82%), gaya ng sa nakapirming saklaw: ang mga istasyon ng pagsubok (Ireland, Great Britain, Netherlands, Germany) ay may inclination na 66-70°, malapit sa reference ni Lange-Hesse (saklaw ni EI5FK 350-1080 km); ang pagkakaiba ay para sa mga istasyong mas nasa timog o mas nasa hilaga, na walang QSO sa pagsubok.
