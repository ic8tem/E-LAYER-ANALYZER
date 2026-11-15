# NORI Es ÎN TIMP

[← Home](Home-ro)

![NORI Es ÎN TIMP](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Sub-filă a **SPORADIC-E** (din versiunea 0.5.2): cum se nasc, se deplasează și se sting **norii de E-sporadic**, pas cu pas.

## Metoda

Metoda lui **Chartier, Hanley și Emmons (2022)**: fiecare legătură E-sporadic spune că în punctul de reflexie foEs este **cel puțin f / sec φ**. Punctele se adună în celule și în pași de timp, iar fiecare celulă ia **valoarea cea mai mare** a pasului. Densitatea electronică minimă este NmE = 1,24·10¹⁰ · foEs² electroni pe metru cub.

Programul folosește punctele de reflexie ale propriului model (toate salturile), nu doar punctul median. Valoarea este o **limită inferioară**: o celulă goală înseamnă doar că nu există nicio legătură.

## Sursa și comenzile

- **Spoturile sesiunii** (se actualizează la fiecare deschidere a filei) sau **Încarcă un log ADIF sau spoturi CSV**.
- **Exportă spoturile sesiunii (CSV)**: arhiva spoturilor se golește la închiderea programului, așa că le poți revedea mai târziu.
- **Benzi MHz** (28, 40, 50, 70, 144), **Pas** (5, 10 sau 15 minute), **Celulă** (1°, 2° sau 4°), **Altitudine Es km**.
- Bara timpului: ⏮ ◀ *Pornește/Oprește* ▶ ⏭ și cursorul; la deschidere se pornește de la pasul cu cele mai multe celule.
- *Pașii anteriori estompați* arată cei trei pași dinaintea celui ales; *Legături* desenează traseele cu culorile benzilor.

## Tabelul și graficul

- Celulele pasului: Celulă (colțul de sud-vest și locatorul centrului), foEs≥, NmE≥, Legături, Benzi. Clic pe o celulă: îi evidențiază legăturile.
- Graficul: cea mai mare foEs minimă (linia galbenă) și numărul de celule (bare) pentru fiecare pas.
