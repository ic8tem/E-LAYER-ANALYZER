# Es-PILVED AJAS

[← Home](Home-et)

![Es-PILVED AJAS](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

**SPORADIC-E** alamsakk (alates versioonist 0.5.2): kuidas **sporaadilise E pilved** tekivad, liiguvad ja kustuvad, samm-sammult.

## Meetod

**Chartieri, Hanley ja Emmonsi (2022)** meetod: iga sporaadilise E side näitab, et peegelduspunktis on foEs **vähemalt f / sec φ**. Punktid kogutakse lahtritesse ja ajasammudesse ning iga lahter võtab sammu **kõrgeima väärtuse**. Minimaalne elektrontihedus on NmE = 1,24·10¹⁰ · foEs² elektroni kuupmeetri kohta.

Programm kasutab oma mudeli peegelduspunkte (kõik hüpped), mitte ainult keskpunkti. Väärtus on **alampiir**: tühi lahter tähendab ainult seda, et sidesid pole.

## Allikas ja juhtelemendid

- **Seansi spotid** (värskenduvad saki igal avamisel) või **Laadi ADIF-logi või CSV-spotid**.
- **Ekspordi seansi spotid (CSV)**: spotiarhiiv tühjeneb programmi sulgemisel, nii saad neid hiljem vaadata.
- **Sagedusalad MHz** (28, 40, 50, 70, 144), **Samm** (5, 10 või 15 minutit), **Lahter** (1°, 2° või 4°), **Es kõrgus km**.
- Ajariba: ⏮ ◀ *Käivita/Peata* ▶ ⏭ ja liugur; avamisel alustatakse sammust, kus on kõige rohkem lahtreid.
- *Eelmised sammud tuhmilt* näitab kolme valitud sammule eelnevat sammu; *Sided* joonistab trassid sagedusalade värvidega.

## Tabel ja graafik

- Sammu lahtrid: Lahter (edelanurk ja keskpunkti lokaator), foEs≥, NmE≥, Sided, Sagedusalad. Klõps lahtril tõstab esile selle sided.
- Graafik: kõrgeim minimaalne foEs (kollane joon) ja lahtrite arv (tulbad) iga sammu kohta.
