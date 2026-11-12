# AURORA

[← Home](Home-da)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Nordlysoval** fra NOAA OVATION Prime (skraveret), **Kp** for de seneste 3 døgn, **solvind** (Bz, hastighed, tæthed), opdateret hvert 5. minut og gemt i `data/swpc`.
- Samme visninger som i F.A.I.: **Scatter trail** for et par stationer og **Iso-refleksion** til at kontrollere et punkt, med en standardhøjde på 110 km.
- **Anbefalet antenneretning for begge stationer**: langs hver stations vinkelrethedslinje (prikker i dens farve) punktet med den højeste ovalsandsynlighed. Tommelfingerregel (G3WPO): start ved ca. 45°, mellem nord og øst.
- Eksempel IO70VJ - JO59BC: scatterpunkt JP01AK ud for Shetlandsøerne, afvigelse 0,97°, QTF 11° og 299°.
- **Nordlys-spots på kortet**: spots med **<AU>** eller **<AUR>** i teksten er nordlys-spots (forfatterens regel). Med flueben i feltet *<AU>/<AUR>-spots på kortet med beregningen* tegner fanen spots fra de seneste 2 timer (højst 30): for hvert par spotter-DX beregnes scatter trail, og det mest sandsynlige scatterpunkt vises (orange cirkel) med QTF og elevation for begge stationer i resultaterne.
- Ud over det mest sandsynlige scatterpunkt angiver resultaterne **QTF-intervallet** for begge stationer langs hele Scatter trail (afvigelse ≤ 1,5°).
- **Lange-Hesse-vindue**: scatterpunktet foretrækkes inden for 370-1100 km fra hver station, det vindue, Lange-Hesse beregnede ved 54° geomagnetisk bredde. Fra version 0.4.5 flytter vinduet sig med hver stations IGRF-14-inklination: tættere på for stationer længere mod syd, længere væk for stationer længere mod nord (f.eks. Capri ca. 260-990 km, Oslo 470-1200 km). Resultaterne viser begge stationers vindue.

## BENCHMARK AURORA

Fanen AURORA har to underfaner: **REAL-TIME** (beregningen beskrevet ovenfor) og **BENCHMARK AURORA**, en prøvebænk for modellen med **298 virkelige nordlys-QSO'er** med tid og lokator: EI5FK's offentliggjorte logs (IO51RT, Irland, 2003-2005, 144 og 432 MHz) og PA0O's logkort (JO33HG, Nederlandene).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- For hvert QSO beregner programmet scatterpunktet med samme model (IGRF-14, højde 110 km, refraktion 4/3, tolerance 3°).
- Farver: **grøn** = scatterpunkt inden for begge stationers Lange-Hesse-vindue; **gul** = forklaret, men uden for det vindue; **orange** = QSO over 2000 km; **rød** = intet scatterpunkt, QSO ikke forklaret af modellen.
- Resuméet giver procentdelene, også med det faste vindue 370-1100 km til sammenligning, og for hver log intervallet og medianen for den beregnede QTF. Klik på en række for at tegne strækningen og de to antenneretninger.
- Resultat (version 0.4.4): **296 af 298 QSO'er forklaret (99 %)**, 243 (82 %) inden for vinduet 370-1100 km; median-QTF for EI5FK mellem 25° og 41°, altså den nordøstlige retning, som G3WPO foreslår. Logsene angiver ikke antenneretningen: kontrollen gælder geometrien, ikke QTF. Med det flytbare vindue (version 0.4.5) er resultatet det samme, 243 QSO'er (82 %), som med det faste vindue: prøvebænkens stationer (Irland, Storbritannien, Nederlandene, Tyskland) har en inklination på 66-70°, tæt på Lange-Hesses reference (vinduet for EI5FK 350-1080 km); forskellen vedrører stationer længere mod syd eller nord, som prøvebænken ikke har QSO'er for.
