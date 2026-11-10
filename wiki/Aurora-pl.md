# AURORA

[← Strona główna](Home-pl)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Owal zorzowy** NOAA OVATION Prime (cieniowany), **Kp** z ostatnich 3 dni, **wiatr słoneczny** (Bz, prędkość, gęstość), aktualizowane co 5 minut i zapisywane w `data/swpc`.
- Te same widoki co w F.A.I.: **Scatter trail** dla pary stacji i **Izorefleksja** do sprawdzenia punktu, z domyślną wysokością 110 km.
- **Zalecany kierunek anteny dla obu stacji**: wzdłuż linii prostopadłości każdej stacji (kropki w jej kolorze) punkt o najwyższym prawdopodobieństwie owalu. Praktyczna reguła (G3WPO): zacznij od około 45°, między północą a wschodem.
- Przykład IO70VJ - JO59BC: scatter point JP01AK na morzu przy Szetlandach, odchylenie 0,97°, QTF 11° i 299°.
- **Spoty zorzowe na mapie**: spoty z **<AU>** lub **<AUR>** w tekście są spotami zorzowymi (reguła autora). Przy zaznaczonym polu *Spoty <AU>/<AUR> na mapie z obliczeniem* zakładka rysuje spoty z ostatnich 2 godzin (najwyżej 30): dla każdej pary spotter-DX oblicza scatter trail i pokazuje najbardziej prawdopodobny scatter point (pomarańczowe koło), z QTF i elewacją obu stacji w wynikach.
- Oprócz najbardziej prawdopodobnego scatter pointu wyniki podają **zakres QTF** obu stacji wzdłuż całego Scatter trail (odchylenie ≤ 1,5°).

## BENCHMARK AURORA

Zakładka AURORA ma dwie podzakładki: **REAL-TIME** (obliczenia opisane powyżej) oraz **BENCHMARK AURORA**, stanowisko testowe modelu na **298 rzeczywistych QSO zorzowych** z godziną i lokatorem: opublikowanych logach EI5FK (IO51RT, Irlandia, 2003-2005, 144 i 432 MHz) i mapie logu PA0O (JO33HG, Holandia).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Dla każdego QSO program oblicza scatter point tym samym modelem (IGRF-14, wysokość 110 km, refrakcja 4/3, tolerancja 3°).
- Kolory: **zielony** = scatter point 370-1100 km od obu stacji (Lange-Hesse); **żółty** = wyjaśnione, ale poza tym oknem; **pomarańczowy** = QSO powyżej 2000 km; **czerwony** = brak scatter pointu, QSO niewyjaśnione przez model.
- Podsumowanie podaje wartości procentowe oraz, dla każdego logu, zakres i medianę obliczonego QTF. Kliknij wiersz, aby narysować trasę i oba kierunki anten.
- Wynik (wersja 0.4.4): **296 z 298 QSO wyjaśnionych (99%)**, 243 (82%) w oknie 370-1100 km; medianowy QTF EI5FK między 25° a 41°, czyli północny wschód zalecany przez G3WPO. Logi nie podają kierunku anteny: weryfikacja dotyczy geometrii, a nie QTF.
