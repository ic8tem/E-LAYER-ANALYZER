# AURORA

[← Home](Home-af)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime se **aurorale ovaal** (geskadu), **Kp** van die laaste 3 dae, **sonwind** (Bz, snelheid, digtheid), elke 5 minute bygewerk en in `data/swpc` gestoor.
- Dieselfde aansigte as F.A.I.: **Scatter trail** vir 'n paar stasies en **Iso-refleksie** om 'n punt na te gaan, met 'n verstekhoogte van 110 km.
- **Aanbevole antennarigting vir albei stasies**: langs elke stasie se loodlyn (kolletjies in sy kleur), die punt met die hoogste ovaalwaarskynlikheid. Duimreël (G3WPO): begin by ongeveer 45°, tussen noord en oos.
- Voorbeeld IO70VJ - JO59BC: scatter point JP01AK voor die Shetland-eilande, afwyking 0,97°, QTF 11° en 299°.
- **Aurora-spots op die kaart**: spots met **<AU>** of **<AUR>** in die teks is aurora-spots (outeur se reël). Met die blokkie *<AU>/<AUR>-spots op die kaart met die berekening* gemerk, plot die oortjie die spots van die laaste 2 uur (hoogstens 30): vir elke spotter-DX-paar bereken dit die scatter trail en wys die mees waarskynlike scatter point (oranje sirkel), met die QTF en elevasie van albei stasies in die resultate.
- Benewens die mees waarskynlike scatter point gee die resultate die **QTF-interval** van albei stasies langs die hele Scatter trail (afwyking ≤ 1,5°).
- **Lange-Hesse-venster**: die scatter point word verkieslik binne 370-1100 km van elke stasie gesoek, die venster wat Lange-Hesse op 54° geomagnetiese breedte bereken het. Vanaf weergawe 0.4.5 verskuif die venster met die IGRF-14-inklinasie van elke stasie: nader vir stasies verder suid, verder vir stasies verder noord (bv. Capri ongeveer 260-990 km, Oslo 470-1200 km). Die resultate wys die venster van albei stasies.

## BENCHMARK AURORA

Die AURORA-oortjie het twee suboortjies: **REAL-TIME** (die berekening hierbo beskryf) en **BENCHMARK AURORA**, 'n toetsbank vir die model op **298 werklike aurora-QSO's** met tyd en lokator: die gepubliseerde logs van EI5FK (IO51RT, Ierland, 2003-2005, 144 en 432 MHz) en die logkaart van PA0O (JO33HG, Nederland).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Vir elke QSO bereken die program die scatter point met dieselfde model (IGRF-14, hoogte 110 km, refraksie 4/3, toleransie 3°).
- Kleure: **groen** = scatter point binne die Lange-Hesse-venster van albei stasies; **geel** = verklaar maar buite daardie venster; **oranje** = QSO verder as 2000 km; **rooi** = geen scatter point nie, QSO nie deur die model verklaar nie.
- Die opsomming gee die persentasies, ook met die vaste venster van 370-1100 km ter vergelyking, en, vir elke log, die interval en mediaan van die berekende QTF. Klik op 'n ry om die pad en albei antennarigtings te teken.
- Resultaat (weergawe 0.4.18, IGRF-14-veld van die jaar van elke QSO): **297 van 298 QSO's verklaar (100%)**, 267 (90%) binne die Lange-Hesse-venster van albei stasies (275, 92%, met die vaste venster van 370-1100 km); mediaan-QTF van EI5FK tussen 32° en 53°, dit wil sê die noordooste wat G3WPO aanbeveel. Die logs gee nie die antennarigting nie: die kontrole is op geometrie, nie op QTF nie.
- **Regstelling**: van weergawe 0.4.4 tot 0.4.17 het die toetsbank begin voordat IGRF-14 gelaai is en met die dipool en die vaste venster bereken; die resultate wat toe gepubliseer is (243 QSO's, 82%, mediaan-QTF 25-41°) was dié van die dipool. Reggestel in 0.4.18.
