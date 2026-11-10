# AURORA

[← Home](Home-is)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Norðurljósasporbaugur** NOAA OVATION Prime (skyggður), **Kp** síðustu 3 daga, **sólvindur** (Bz, hraði, þéttleiki), uppfært á 5 mínútna fresti og vistað í `data/swpc`.
- Sömu sýnir og í F.A.I.: **Scatter trail** fyrir par stöðva og **Jafnendurvarp** til að prófa punkt, með sjálfgefna hæð 110 km.
- **Ráðlögð loftnetsstefna fyrir báðar stöðvar**: eftir hornréttulínu hvorrar stöðvar (punktar í lit hennar), punkturinn með mestar líkur í sporbaugnum. Þumalregla (G3WPO): byrjaðu við um 45°, milli norðurs og austurs.
- Dæmi IO70VJ - JO59BC: scatter point JP01AK undan Hjaltlandi, frávik 0,97°, QTF 11° og 299°.
- **Norðurljósa-spot á kortinu**: spot með **<AU>** eða **<AUR>** í textanum eru norðurljósa-spot (regla höfundar). Þegar hakað er við reitinn *<AU>/<AUR>-spot á kortinu með útreikningi* teiknar flipinn spot síðustu 2 klukkustunda (í mesta lagi 30): fyrir hvert par spottara og DX reiknar hann scatter trail og sýnir líklegasta scatter point (appelsínugulur hringur), með QTF og hæðarhorni beggja stöðva í niðurstöðunum.
- Auk líklegasta scatter point sýna niðurstöðurnar **QTF-bil** beggja stöðva eftir öllum Scatter trail (frávik ≤ 1,5°).

## BENCHMARK AURORA

Flipinn AURORA hefur tvo undirflipa: **REAL-TIME** (útreikninginn sem lýst er hér að ofan) og **BENCHMARK AURORA**, prófunarbekk líkansins á **298 raunverulegum norðurljósa-QSO** með tíma og locator: birtar dagbækur EI5FK (IO51RT, Írlandi, 2003-2005, 144 og 432 MHz) og dagbókarkort PA0O (JO33HG, Hollandi).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Fyrir hvert QSO reiknar forritið scatter point með sama líkani (IGRF-14, hæð 110 km, ljósbrot 4/3, vikmörk 3°).
- Litir: **grænt** = scatter point í 370-1100 km fjarlægð frá báðum stöðvum (Lange-Hesse); **gult** = skýrt en utan þess glugga; **appelsínugult** = QSO lengra en 2000 km; **rautt** = enginn scatter point, QSO ekki skýrt af líkaninu.
- Samantektin gefur hlutföllin í prósentum og, fyrir hverja dagbók, bil og miðgildi reiknaðs QTF. Smelltu á línu til að teikna leiðina og báðar miðanirnar.
- Niðurstaða (útgáfa 0.4.4): **296 af 298 QSO skýrð (99%)**, 243 (82%) innan 370-1100 km gluggans; miðgildi QTF hjá EI5FK milli 25° og 41°, þ.e. norðaustrið sem G3WPO leggur til. Dagbækurnar gefa ekki upp miðunina: prófunin snýr að rúmfræðinni, ekki að QTF.
