# AURORA

[← Strona główna](Home-pl)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Owal zorzowy** NOAA OVATION Prime (cieniowany), **Kp** z ostatnich 3 dni, **wiatr słoneczny** (Bz, prędkość, gęstość), aktualizowane co 5 minut i zapisywane w `data/swpc`.
- Te same widoki co w F.A.I.: **Scatter trail** dla pary stacji i **Izorefleksja** do sprawdzenia punktu, z domyślną wysokością 110 km.
- **Zalecany kierunek anteny dla obu stacji**: wzdłuż linii prostopadłości każdej stacji (kropki w jej kolorze) punkt o najwyższym prawdopodobieństwie owalu. Praktyczna reguła (G3WPO): zacznij od około 45°, między północą a wschodem.
- Przykład IO70VJ - JO59BC: scatter point JP01AK na morzu przy Szetlandach, odchylenie 0,97°, QTF 11° i 299°.
- **Spoty zorzowe na mapie**: spoty z **<AU>** lub **<AUR>** w tekście są spotami zorzowymi (reguła autora). Przy zaznaczonym polu *Spoty <AU>/<AUR> na mapie z obliczeniem* zakładka rysuje spoty z ostatnich 2 godzin (najwyżej 30): dla każdej pary spotter-DX oblicza scatter trail i pokazuje najbardziej prawdopodobny scatter point (pomarańczowe koło), z QTF i elewacją obu stacji w wynikach.
- Oprócz najbardziej prawdopodobnego scatter pointu wyniki podają **zakres QTF** obu stacji wzdłuż całego Scatter trail (odchylenie ≤ 1,5°).
- **Okno Lange-Hessego**: scatter point jest preferowany w odległości 370-1100 km od każdej stacji, czyli w oknie, które Lange-Hesse obliczył dla 54° szerokości geomagnetycznej. Od wersji 0.4.5 okno przesuwa się zgodnie z inklinacją IGRF-14 każdej stacji: bliżej dla stacji położonych bardziej na południe, dalej dla stacji bardziej na północ (np. Capri około 260-990 km, Oslo 470-1200 km). Wyniki podają okno obu stacji.

## BENCHMARK AURORA

Zakładka AURORA ma dwie podzakładki: **REAL-TIME** (obliczenia opisane powyżej) oraz **BENCHMARK AURORA**, stanowisko testowe modelu na **298 rzeczywistych QSO zorzowych** z godziną i lokatorem: opublikowanych logach EI5FK (IO51RT, Irlandia, 2003-2005, 144 i 432 MHz) i mapie logu PA0O (JO33HG, Holandia).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Dla każdego QSO program oblicza scatter point tym samym modelem (IGRF-14, wysokość 110 km, refrakcja 4/3, tolerancja 3°).
- Kolory: **zielony** = scatter point w oknie Lange-Hessego obu stacji; **żółty** = wyjaśnione, ale poza tym oknem; **pomarańczowy** = QSO powyżej 2000 km; **czerwony** = brak scatter pointu, QSO niewyjaśnione przez model.
- Podsumowanie podaje wartości procentowe, także ze stałym oknem 370-1100 km dla porównania, oraz, dla każdego logu, zakres i medianę obliczonego QTF. Kliknij wiersz, aby narysować trasę i oba kierunki anten.
- Wynik (wersja 0.4.18, pole IGRF-14 z roku każdego QSO): **297 z 298 QSO wyjaśnionych (100%)**, 267 (90%) w oknie Lange-Hessego obu stacji (275, czyli 92%, ze stałym oknem 370-1100 km); obliczony medianowy QTF EI5FK między 32° a 53°, czyli północny wschód zalecany przez G3WPO. Logi nie podają kierunku anteny: weryfikacja dotyczy geometrii, a nie QTF.
- **Poprawka**: od wersji 0.4.4 do 0.4.17 stanowisko testowe uruchamiało się przed wczytaniem IGRF-14 i liczyło z dipolem i stałym oknem; opublikowane wtedy wyniki (243 QSO, 82%, medianowy QTF 25-41°) pochodziły z dipola. Poprawione w 0.4.18.
