# MGA ULAP NG Es SA PAGLIPAS NG ORAS

[← Home](Home-tl)

![NUBI Es NEL TEMPO](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Sub-tab ng **SPORADIC-E** (mula sa bersiyon 0.5.2): kung paano isinisilang, gumagalaw at namamatay ang **mga ulap ng E-sporadic**, hakbang-hakbang.

## Ang paraan

Paraan nina **Chartier, Hanley at Emmons (2022)**: sinasabi ng bawat koneksiyong E-sporadic na sa reflection point ang foEs ay **hindi bababa sa f / sec φ**. Tinitipon ang mga punto sa mga cell at sa mga hakbang ng oras, at kinukuha ng bawat cell ang **pinakamataas na halaga** ng hakbang. Ang pinakamababang electron density ay NmE = 1.24·10¹⁰ · foEs² electron bawat metro kubiko.

Ginagamit ng programa ang mga reflection point ng sarili nitong modelo (lahat ng hop), hindi lamang ang midpoint. Ang halaga ay isang **mababang hangganan**: ang walang lamang cell ay nangangahulugang walang koneksiyon lamang.

## Pinagmulan at mga kontrol

- **Mga spot ng session** (nag-a-update sa bawat pagbukas ng tab) o **I-load ang ADIF log o CSV spot**.
- **I-export ang mga spot ng session (CSV)**: nauubos ang archive ng mga spot kapag isinara ang programa, kaya maaari mo silang muling makita sa ibang pagkakataon.
- **Mga banda MHz** (28, 40, 50, 70, 144), **Hakbang** (5, 10 o 15 minuto), **Cell** (1°, 2° o 4°), **Taas ng Es km**.
- Bar ng oras: ⏮ ◀ *Simulan/Ihinto* ▶ ⏭ at ang slider; sa pagbukas, nagsisimula sa hakbang na may pinakamaraming cell.
- Ipinapakita ng *Malabong naunang hakbang* ang tatlong hakbang bago ang napili; iginuguhit ng *Mga koneksiyon* ang mga ruta sa mga kulay ng banda.

## Talahanayan at graph

- Mga cell ng hakbang: Cell (timog-kanlurang sulok at locator ng gitna), foEs≥, NmE≥, Mga koneksiyon, Mga banda. I-click ang isang cell: itinatampok ang mga koneksiyon nito.
- Graph: pinakamataas na minimum foEs (dilaw na linya) at bilang ng mga cell (mga bar) para sa bawat hakbang.
