# AURORA

[← Kezdőlap](Home-hu)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **aurora-ovális** (árnyalva), az utolsó 3 nap **Kp**-indexe, **napszél** (Bz, sebesség, sűrűség), 5 percenként frissítve és a `data/swpc` mappába mentve.
- Ugyanazok a nézetek, mint az F.A.I.-nál: **Scatter trail** egy állomáspárra és **Izoflexió** egy pont ellenőrzésére, 110 km-es alapértelmezett magassággal.
- **Ajánlott antennairány mindkét állomásra**: az egyes állomások merőlegességi vonala mentén (pontok az állomás színében) a legnagyobb ovális-valószínűségű pont. Ökölszabály (G3WPO): kb. 45°-tól kezdve, észak és kelet között.
- Példa: IO70VJ - JO59BC: szórási pont JP01AK a Shetland-szigetek előtt, eltérés 0,97°, QTF 11° és 299°.
- **Aurora-spotok a térképen**: a szövegükben **<AU>** vagy **<AUR>** jelölést tartalmazó spotok aurora-spotok (a szerző szabálya). Bejelölt *<AU>/<AUR> spotok a térképen a számítással* négyzet mellett a fül kirajzolja az utolsó 2 óra spotjait (legfeljebb 30-at): minden spottoló-DX párra kiszámítja a Scatter trailt, és megmutatja a legvalószínűbb szórási pontot (narancs kör), az eredményekben mindkét állomás QTF-jével és elevációjával.
- A legvalószínűbb szórási pont mellett az eredmények megadják mindkét állomás **QTF-tartományát** a teljes Scatter trail mentén (eltérés ≤ 1,5°).
- **Lange-Hesse-ablak**: a szórási pontot lehetőleg az egyes állomásoktól 370-1100 km-re választja, abban az ablakban, amelyet Lange-Hesse 54° geomágneses szélességen számított. A 0.4.5-ös verziótól az ablak az egyes állomások IGRF-14 inklinációjával eltolódik: közelebb a délebbre, távolabb az északabbra fekvő állomásoknál (pl. Capri kb. 260-990 km, Oslo 470-1200 km). Az eredmények mindkét állomás ablakát mutatják.

## BENCHMARK AURORA

Az AURORA fülnek két allapja van: **REAL-TIME** (a fent leírt számítás) és **BENCHMARK AURORA**, a modell tesztpadja **298 valódi aurora-QSO-n** időponttal és lokátorral: EI5FK publikált naplói (IO51RT, Írország, 2003-2005, 144 és 432 MHz) és PA0O naplójának térképe (JO33HG, Hollandia).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Minden QSO-ra a program ugyanazzal a modellel számítja ki a szórási pontot (IGRF-14, magasság 110 km, 4/3-os refrakció, 3°-os tűrés).
- Színek: **zöld** = a szórási pont mindkét állomás Lange-Hesse-ablakán belül; **sárga** = magyarázott, de ezen az ablakon kívül; **narancs** = 2000 km feletti QSO; **piros** = nincs szórási pont, a modell által nem magyarázott QSO.
- Az összefoglaló megadja a százalékokat, összehasonlításként a rögzített 370-1100 km-es ablakkal is, és naplónként a számított QTF tartományát és mediánját. Egy sorra kattintva kirajzolja az útvonalat és mindkét antennairányt.
- Eredmény (0.4.18-as verzió, az egyes QSO-k évének IGRF-14 tere): **298 QSO-ból 297 magyarázott (100%)**, 267 (90%) mindkét állomás Lange-Hesse-ablakán belül (a rögzített 370-1100 km-es ablakkal 275, azaz 92%); EI5FK medián QTF-je 32° és 53° között, vagyis a G3WPO által javasolt északkelet. A naplók nem tartalmazzák az antennairányt: az ellenőrzés a geometrián alapul, nem a QTF-en.
- **Javítás**: a 0.4.4-es verziótól a 0.4.17-ig a tesztpad az IGRF-14 betöltése előtt indult, és a dipólussal és a rögzített ablakkal számolt; az akkor közzétett eredmények (243 QSO, 82%, medián QTF 25-41°) a dipólus eredményei voltak. A 0.4.18-ban javítva.
