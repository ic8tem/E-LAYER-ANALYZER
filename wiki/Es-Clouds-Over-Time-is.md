# Es-SKÝ Í TÍMANS RÁS

[← Home](Home-is)

![Es-SKÝ Í TÍMANS RÁS](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Undirflipi **SPORADIC-E** (frá útgáfu 0.5.2): hvernig **sporadic-E-skýin** myndast, færast og hverfa, skref fyrir skref.

## Aðferðin

Aðferð **Chartier, Hanley og Emmons (2022)**: hvert sporadic-E-samband segir að í endurvarpspunktinum sé foEs **að minnsta kosti f / sec φ**. Punktunum er safnað í hólf og tímaskref, og hvert hólf tekur **hæsta gildi** skrefsins. Lágmarks-rafeindaþéttleikinn er NmE = 1,24·10¹⁰ · foEs² rafeindir á rúmmetra.

Forritið notar endurvarpspunkta eigin líkans (öll stökk), ekki aðeins miðpunktinn. Gildið er **neðri mörk**: tómt hólf þýðir aðeins að engin sambönd séu.

## Uppspretta og stýringar

- **Spot lotunnar** (uppfærð í hvert sinn sem flipinn er opnaður) eða **Hlaða ADIF-dagbók eða CSV-spotum**.
- **Flytja út spot lotunnar (CSV)**: spotasafnið tæmist þegar forritinu er lokað, svo þú getur skoðað þau síðar.
- **Tíðnisvið MHz** (28, 40, 50, 70, 144), **Skref** (5, 10 eða 15 mínútur), **Hólf** (1°, 2° eða 4°), **Es-hæð km**.
- Tímastika: ⏮ ◀ *Ræsa/Stöðva* ▶ ⏭ og sleðinn; við opnun er byrjað á skrefinu með flest hólf.
- *Fyrri skref dofnuð* sýnir þrjú skref á undan því valda; *Sambönd* teiknar leiðirnar í litum tíðnisviðanna.

## Tafla og graf

- Hólf skrefsins: Hólf (suðvesturhorn og locator miðjunnar), foEs≥, NmE≥, Sambönd, Tíðnisvið. Smellur á hólf auðkennir sambönd hans.
- Graf: hæsta lágmarks-foEs (gul lína) og fjöldi hólfa (súlur) fyrir hvert skref.
