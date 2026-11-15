# Es-WOLKE OOR TYD

[← Home](Home-af)

![Es-WOLKE OOR TYD](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Suboortjie van **SPORADIC-E** (vanaf weergawe 0.5.2): hoe die **sporadiese-E-wolke** ontstaan, skuif en uitdoof, stap vir stap.

## Die metode

Metode van **Chartier, Hanley en Emmons (2022)**: elke sporadiese-E-verbinding sê dat die foEs by die refleksiepunt **minstens f / sec φ** is. Die punte word in selle en tydstappe versamel, en elke sel neem die **hoogste waarde** van die stap. Die minimum elektrondigtheid is NmE = 1,24·10¹⁰ · foEs² elektrone per kubieke meter.

Die program gebruik die refleksiepunte van sy eie model (al die spronge), nie net die middelpunt nie. Die waarde is 'n **ondergrens**: 'n leë sel beteken net geen verbinding nie.

## Bron en kontroles

- **Spots van die sessie** (word by elke opening van die oortjie bygewerk) of **Laai ADIF-log of CSV-spots**.
- **Voer die spots van die sessie uit (CSV)**: die spotargief word leeggemaak wanneer die program sluit, so kan jy hulle later weer bekyk.
- **Bande MHz** (28, 40, 50, 70, 144), **Stap** (5, 10 of 15 minute), **Sel** (1°, 2° of 4°), **Es-hoogte km**.
- Tydbalk: ⏮ ◀ *Begin/Stop* ▶ ⏭ en die skuifbalk; by opening begin dit by die stap met die meeste selle.
- *Vorige stappe vaag* wys die drie stappe voor die gekose een; *Verbindings* teken die paaie in die kleure van die bande.

## Tabel en grafiek

- Selle van die stap: Sel (suidwestelike hoek en lokator van die middel), foEs≥, NmE≥, Verbindings, Bande. Klik op 'n sel om die verbindings daarvan uit te lig.
- Grafiek: hoogste minimum foEs (geel lyn) en aantal selle (stawe) vir elke stap.
