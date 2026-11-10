# BENCHMARK Es

[← Home](Home-af)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Toetsbank vir **sporadiese E** en **multi-sprong** op 'n **hele ADIF-log**, selfs een van 'n ander stasie: uit die QSO's wat werklik gemaak is, skets die program die skip en die wolke, dag vir dag en uur vir uur.

## Die log laai

- **Laai ADIF**: kies die `.adi`/`.adif`-lêer. Die program onthou die lêer en laai dit weer by opstart.
- **Verwyder ADIF**: verwyder die log uit die oortjie.
- **Stasie in die middel**: roepnaam en lokator van die stasie wat die log gemaak het. Die program stel die mees algemene `STATION_CALLSIGN` (of `OPERATOR`) en `MY_GRIDSQUARE` voor; hulle kan met die hand verander en met **Pas toe** bevestig word. Die middelpunt hang nie van die algemene instellings af nie, sodat logs van ander stasies bestudeer kan word.
- Uitsaaiposisie: die `MY_GRIDSQUARE` van elke QSO (as dit ontbreek, die lokator in die middel). Ontvangsposisies: die `GRIDSQUARE`-velde.
- QSO's wat gebruik word: 28, 50, 70 en 144 MHz met 'n lokator. QSO's met 'n ander `PROP_MODE` (TR, MS, FAI, AUR, EME...) en dié onder 500 km kan uitgesluit word. 432 MHz word nooit ingesluit nie: sporadiese E geld nie daar nie.

## Dae en ure

- **QSO's per dag**: dag, aantal QSO's, bande, maksimum afstand, multi-spronge. By die oopmaak stel die program die dag met die meeste QSO's voor.
- **Ure van die dag**: die ry *Die hele dag* en een ry vir elke UTC-uur; 'n klik wys slegs die QSO's van daardie uur.
- **QSO**: die lys vir die keuse; 'n klik op 'n QSO (of naby die DX-stasie op die kaart) lig dit uit en wys die besonderhede daarvan.

## Die kaart

**Azimutale ekwidistante** kaart gesentreer op die stasie, met die skuifbalk **Maksimum afstand** om te zoem (ook met die muiswieletjie): die ringe wys die **km vanaf die middel**.

- **Skip per sektor** (10° azimut): oranje boog = naaste QSO in die sektor (rand van die stiltesone), groen boog = verste QSO, daartussen die gebied wat bereik is.
- **Es-wolke** op die Maidenhead-vierkante, gekleur volgens die geskatte foEs.
- Middelpunt van enkele spronge (geel), eerste sprong van die stasie (oranje, hol sirkel = kort eerste sprong), eerste sprong van die DX-stasie (pers).
- 'n **Vaag geel kolletjie** merk die middel van elke sprong, waar die Es-wolk vermoedelik is (ook die middel van die kort eerste sprong, as dié hipotese aan is).
- Vir die hele dag verbind die **rooi lyn** uur vir uur die swaartepunt van die middelpunte: dit is die drywing van die wolke.

![Een uur](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Voorgestelde berekeninge

1. **Skip per sektor**: vir elke sektor die skip-afstand en die bereik, met die roepname.
2. **Geskatte foEs** met die sekanswet (foEs ≥ f / sec φ): die kortste enkele sprong op elke band gee die hoogste skatting; met hierdie foEs, die verwagte MUF op 1000, 1500, 2000 en 2400 km en die afstand waarbo 144 MHz sou oopgaan.
3. **Es-wolke** geskat op die vierkante, met die aantal QSO's en die foEs.
4. **Multi-sprong**: die stasie se eerste sprong volgens die twee hipoteses (gelyke spronge, en kort eerste sprong binne 1200 km, IC8TEM se ervaring) vergelyk met die wolke wat binne ±30 minute in enkele sprong gesien is; binne 300 km word die hipotese bevestig. Die program tel die bevestigings van elke hipotese.
5. **Wolkdrywing** uur vir uur: km, rigting en snelheid.

Die berekeninge is aanduidend: 'n QSO sê dat die pad oop was, nie presies waar die wolk was nie. Hoe skoner die data (lokators van 6 karakters, korrekte `PROP_MODE`), hoe betroubaarder die skip.
