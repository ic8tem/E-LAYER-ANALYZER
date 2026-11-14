# AURORA

[← Domů](Home-cs)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Polární ovál** NOAA OVATION Prime (vystínovaný), **Kp** za poslední 3 dny, **sluneční vítr** (Bz, rychlost, hustota), aktualizované každých 5 minut a ukládané do `data/swpc`.
- Stejná zobrazení jako u F.A.I.: **Scatter trail** pro dvojici stanic a **Izoflexe** k ověření bodu, s výchozí výškou 110 km.
- **Doporučené směrování antény pro obě stanice**: podél čáry kolmosti každé stanice (tečky v její barvě) bod s nejvyšší pravděpodobností oválu. Praktické pravidlo (G3WPO): začít asi na 45°, mezi severem a východem.
- Příklad IO70VJ - JO59BC: bod rozptylu JP01AK na moři u Shetland, odchylka 0,97°, QTF 11° a 299°.
- **Spoty aurory na mapě**: spoty s **<AU>** nebo **<AUR>** v textu jsou spoty aurory (pravidlo autora). Se zaškrtnutým polem *Spoty <AU>/<AUR> na mapě s výpočtem* karta kreslí spoty za poslední 2 hodiny (nejvýše 30): pro každou dvojici spotter-DX vypočítá scatter trail a zobrazí nejpravděpodobnější bod rozptylu (oranžový kruh), s QTF a elevací obou stanic ve výsledcích.
- Kromě nejpravděpodobnějšího bodu rozptylu uvádějí výsledky **rozsah QTF** obou stanic podél celé Scatter trail (odchylka ≤ 1,5°).
- **Okno Lange-Hesse**: bod rozptylu se upřednostňuje ve vzdálenosti 370-1100 km od každé stanice, v okně, které Lange-Hesse vypočítal při 54° geomagnetické šířky. Od verze 0.4.5 se okno posouvá podle inklinace IGRF-14 každé stanice: blíže pro stanice dále na jihu, dále pro stanice více na severu (např. Capri asi 260-990 km, Oslo 470-1200 km). Výsledky ukazují okno obou stanic.

## BENCHMARK AURORA

Karta AURORA má dvě podkarty: **REAL-TIME** (výpočet popsaný výše) a **BENCHMARK AURORA**, testovací lavici modelu na **298 skutečných QSO přes auroru** s časem a lokátorem: publikované deníky EI5FK (IO51RT, Irsko, 2003-2005, 144 a 432 MHz) a mapa deníku PA0O (JO33HG, Nizozemsko).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Pro každé QSO program vypočítá bod rozptylu stejným modelem (IGRF-14, výška 110 km, refrakce 4/3, tolerance 3°).
- Barvy: **zelená** = bod rozptylu uvnitř okna Lange-Hesse obou stanic; **žlutá** = vysvětleno, ale mimo toto okno; **oranžová** = QSO nad 2000 km; **červená** = žádný bod rozptylu, QSO nevysvětlené modelem.
- Souhrn dává procenta, také s pevným oknem 370-1100 km pro srovnání, a pro každý deník rozsah a medián vypočteného QTF. Kliknutím na řádek se nakreslí trasa a obě směrování antén.
- Výsledek (verze 0.4.18, pole IGRF-14 roku každého QSO): **297 z 298 QSO vysvětleno (100 %)**, 267 (90 %) uvnitř okna Lange-Hesse obou stanic (275, tj. 92 %, s pevným oknem 370-1100 km); medián QTF stanice EI5FK mezi 32° a 53°, tj. severovýchod doporučený G3WPO. Deníky neuvádějí směrování antény: ověření se týká geometrie, ne QTF.
- **Oprava**: od verze 0.4.4 do 0.4.17 se testovací lavice spouštěla před načtením IGRF-14 a počítala s dipólem a pevným oknem; tehdy zveřejněné výsledky (243 QSO, 82 %, medián QTF 25-41°) byly výsledky dipólu. Opraveno ve verzi 0.4.18.
