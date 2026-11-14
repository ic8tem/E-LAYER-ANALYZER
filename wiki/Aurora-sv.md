# AURORA

[← Home](Home-sv)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Norrskensovalen** från NOAA OVATION Prime (skuggad), **Kp** för de senaste 3 dygnen, **solvinden** (Bz, hastighet, täthet), uppdaterade var 5:e minut och sparade i `data/swpc`.
- Samma vyer som i F.A.I.: **Scatter trail** för ett par stationer och **Isoreflexion** för att kontrollera en punkt, med standardhöjden 110 km.
- **Rekommenderad antennriktning för båda stationerna**: längs varje stations vinkelräthetslinje (prickar i dess färg), den punkt som har högst sannolikhet i ovalen. Tumregel (G3WPO): börja kring 45°, mellan norr och öster.
- Exempel IO70VJ - JO59BC: scatterpunkt JP01AK utanför Shetlandsöarna, avvikelse 0,97°, QTF 11° och 299°.
- **Auroraspots på kartan**: spots med **<AU>** eller **<AUR>** i texten är auroraspots (författarens regel). Med rutan *<AU>/<AUR>-spots på kartan med beräkningen* ikryssad ritar fliken ut spotsen från de senaste 2 timmarna (högst 30): för varje par spotter-DX beräknar den scatter trail och visar den mest sannolika scatterpunkten (orange cirkel), med QTF och elevation för båda stationerna i resultaten.
- Utöver den mest sannolika scatterpunkten anger resultaten **QTF-intervallet** för båda stationerna längs hela Scatter trail (avvikelse ≤ 1,5°).
- **Lange-Hesse-fönster**: scatterpunkten föredras inom 370-1100 km från varje station, det fönster som Lange-Hesse beräknade vid 54° geomagnetisk latitud. Från version 0.4.5 flyttas fönstret med varje stations IGRF-14-inklination: närmare för stationer längre söderut, längre bort för stationer längre norrut (t.ex. Capri ca 260-990 km, Oslo 470-1200 km). Resultaten visar båda stationernas fönster.

## BENCHMARK AURORA

Fliken AURORA har två underflikar: **REAL-TIME** (beräkningen som beskrivs ovan) och **BENCHMARK AURORA**, en provbänk för modellen med **298 verkliga aurora-QSO** med tid och lokator: de publicerade loggarna från EI5FK (IO51RT, Irland, 2003-2005, 144 och 432 MHz) och loggkartan från PA0O (JO33HG, Nederländerna).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- För varje QSO beräknar programmet scatterpunkten med samma modell (IGRF-14, höjd 110 km, refraktion 4/3, tolerans 3°).
- Färger: **grönt** = scatterpunkt inom båda stationernas Lange-Hesse-fönster; **gult** = förklarat men utanför det fönstret; **orange** = QSO över 2000 km; **rött** = ingen scatterpunkt, QSO inte förklarat av modellen.
- Sammanfattningen ger procentandelarna, även med det fasta fönstret 370-1100 km för jämförelse, och för varje logg intervallet och medianen för det beräknade QTF. Klicka på en rad för att rita sträckan och de två antennriktningarna.
- Resultat (version 0.4.18, IGRF-14-fältet för året för varje QSO): **297 av 298 QSO förklarade (100 %)**, 267 (90 %) inom båda stationernas Lange-Hesse-fönster (275, 92 %, med det fasta fönstret 370-1100 km); EI5FK:s beräknade median-QTF mellan 32° och 53°, alltså den nordost som G3WPO rekommenderar. Loggarna anger inte antennriktningen: kontrollen gäller geometrin, inte QTF.
- **Rättelse**: från version 0.4.4 till 0.4.17 startade provbänken innan IGRF-14 hade lästs in och räknade med dipolen och det fasta fönstret; de resultat som publicerades då (243 QSO, 82 %, median-QTF 25-41°) var dipolens. Rättat i 0.4.18.
