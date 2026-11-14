# AURORA

[← Home](Home-nl)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Aurora-ovaal** NOAA OVATION Prime (gearceerd), **Kp** van de laatste 3 dagen, **zonnewind** (Bz, snelheid, dichtheid), elke 5 minuten bijgewerkt en opgeslagen in `data/swpc`.
- Dezelfde weergaven als bij F.A.I.: **Scatter trail** voor een paar stations en **Iso-reflectie** om een punt te controleren, met een standaardhoogte van 110 km.
- **Aanbevolen antennerichting voor beide stations**: langs de loodlijn van elk station (punten in zijn kleur) het punt met de hoogste kans in het ovaal. Vuistregel (G3WPO): begin rond 45°, tussen noord en oost.
- Voorbeeld IO70VJ - JO59BC: scatterpunt JP01AK voor de Shetlandeilanden, afwijking 0,97°, QTF 11° en 299°.
- **Aurora-spots op de kaart**: spots met **<AU>** of **<AUR>** in de tekst zijn aurora-spots (regel van de auteur). Met het vakje *<AU>/<AUR>-spots op de kaart met de berekening* aangevinkt, tekent het tabblad de spots van de laatste 2 uur (hooguit 30): voor elk paar spotter-DX berekent het de scatter trail en toont het het meest waarschijnlijke scatterpunt (oranje cirkel), met QTF en elevatie van beide stations in de resultaten.
- Naast het meest waarschijnlijke scatterpunt vermelden de resultaten het **QTF-bereik** van beide stations langs het hele Scatter trail (afwijking ≤ 1,5°).
- **Lange-Hesse-venster**: het scatterpunt krijgt de voorkeur binnen 370-1100 km van elk station, het venster dat Lange-Hesse berekende op 54° geomagnetische breedte. Vanaf versie 0.4.5 verschuift het venster met de IGRF-14-inclinatie van elk station: dichterbij voor stations verder naar het zuiden, verder weg voor stations verder naar het noorden (bijv. Capri ongeveer 260-990 km, Oslo 470-1200 km). De resultaten tonen het venster van beide stations.

## BENCHMARK AURORA

Het tabblad AURORA heeft twee subtabs: **REAL-TIME** (de hierboven beschreven berekening) en **BENCHMARK AURORA**, een testbank van het model op **298 echte aurora-QSO's** met tijd en locator: de gepubliceerde logs van EI5FK (IO51RT, Ierland, 2003-2005, 144 en 432 MHz) en de logkaart van PA0O (JO33HG, Nederland).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Voor elk QSO berekent het programma het scatterpunt met hetzelfde model (IGRF-14, hoogte 110 km, refractie 4/3, tolerantie 3°).
- Kleuren: **groen** = scatterpunt binnen het Lange-Hesse-venster van beide stations; **geel** = verklaard maar buiten dat venster; **oranje** = QSO verder dan 2000 km; **rood** = geen scatterpunt, QSO niet verklaard door het model.
- De samenvatting geeft de percentages, ook met het vaste venster van 370-1100 km ter vergelijking, en voor elk log het bereik en de mediaan van de berekende QTF. Klik op een rij om het traject en de twee antennerichtingen te tekenen.
- Resultaat (versie 0.4.18, IGRF-14-veld van het jaar van elk QSO): **297 van de 298 QSO's verklaard (100%)**, 267 (90%) binnen het Lange-Hesse-venster van beide stations (275, 92%, met het vaste venster van 370-1100 km); mediane QTF van EI5FK tussen 32° en 53°, dus het noordoosten dat G3WPO aanraadt. De logs vermelden de antennerichting niet: de controle gaat over de geometrie, niet over de QTF.
- **Correctie**: van versie 0.4.4 tot 0.4.17 startte de testbank vóór het laden van IGRF-14 en rekende hij met de dipool en het vaste venster; de toen gepubliceerde resultaten (243 QSO's, 82%, mediane QTF 25-41°) waren die van de dipool. Gecorrigeerd in 0.4.18.
