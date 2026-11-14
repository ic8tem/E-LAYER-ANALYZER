# AURORA

[← Home](Home-nb)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Nordlysoval** fra NOAA OVATION Prime (skravert), **Kp** for de siste 3 døgnene, **solvind** (Bz, hastighet, tetthet), oppdatert hvert 5. minutt og lagret i `data/swpc`.
- De samme visningene som i F.A.I.: **Scatter trail** for et stasjonspar og **Isorefleksjon** for å kontrollere et punkt, med standardhøyde 110 km.
- **Anbefalt antenneretning for begge stasjonene**: langs hver stasjons vinkelretthetslinje (prikker i stasjonens farge) punktet med høyest ovalsannsynlighet. Tommelfingerregel (G3WPO): start fra ca. 45°, mellom nord og øst.
- Eksempel IO70VJ - JO59BC: scatter-punkt JP01AK utenfor Shetland, avvik 0,97°, QTF 11° og 299°.
- **Nordlys-spot på kartet**: spot med **<AU>** eller **<AUR>** i teksten er nordlys-spot (forfatterens regel). Med boksen *<AU>/<AUR>-spot på kartet med beregningen* avkrysset tegner fanen spot fra de siste 2 timene (høyst 30): for hvert par spotter-DX beregnes scatter trail, og det mest sannsynlige scatter-punktet vises (oransje sirkel), med QTF og elevasjon for begge stasjonene i resultatene.
- I tillegg til det mest sannsynlige scatter-punktet angir resultatene **QTF-intervallet** for begge stasjonene langs hele Scatter trail (avvik ≤ 1,5°).
- **Lange-Hesse-vindu**: scatter-punktet foretrekkes innenfor 370-1100 km fra hver stasjon, vinduet Lange-Hesse beregnet ved 54° geomagnetisk bredde. Fra versjon 0.4.5 flytter vinduet seg med IGRF-14-inklinasjonen til hver stasjon: nærmere for stasjoner lenger sør, lenger unna for stasjoner lenger nord (f.eks. Capri ca. 260-990 km, Oslo 470-1200 km). Resultatene viser vinduet til begge stasjonene.

## BENCHMARK AURORA

Fanen AURORA har to underfaner: **REAL-TIME** (beregningen som er beskrevet ovenfor) og **BENCHMARK AURORA**, en prøvebenk for modellen med **298 virkelige nordlys-QSO-er** med tid og lokator: de publiserte loggene til EI5FK (IO51RT, Irland, 2003-2005, 144 og 432 MHz) og loggkartet til PA0O (JO33HG, Nederland).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- For hvert QSO beregner programmet scatter-punktet med samme modell (IGRF-14, høyde 110 km, refraksjon 4/3, toleranse 3°).
- Farger: **grønn** = scatter-punkt innenfor Lange-Hesse-vinduet til begge stasjonene; **gul** = forklart, men utenfor det vinduet; **oransje** = QSO over 2000 km; **rød** = ingen scatter-punkt, QSO ikke forklart av modellen.
- Sammendraget gir prosentandelene, også med det faste vinduet 370-1100 km til sammenligning, og for hver logg intervallet og medianen for den beregnede QTF-en. Klikk på en rad for å tegne strekningen og de to antenneretningene.
- Resultat (versjon 0.4.18, IGRF-14-feltet for året til hvert QSO): **297 av 298 QSO-er forklart (100 %)**, 267 (90 %) innenfor Lange-Hesse-vinduet til begge stasjonene (275, 92 %, med det faste vinduet 370-1100 km); beregnet median-QTF for EI5FK mellom 32° og 53°, altså den nordøstlige retningen som G3WPO anbefaler. Loggene oppgir ikke antenneretningen: kontrollen gjelder geometrien, ikke QTF.
- **Rettelse**: fra versjon 0.4.4 til 0.4.17 startet prøvebenken før IGRF-14 var lastet inn, og regnet med dipolen og det faste vinduet; resultatene som ble publisert den gang (243 QSO-er, 82 %, median-QTF 25-41°), var dipolens. Rettet i 0.4.18.
