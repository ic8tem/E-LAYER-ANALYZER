# BENCHMARK 50 MHz

[← Strona główna](Home-pl)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Stanowisko testowe dla **wieloskokowego E-sporadycznego na 50 MHz** na podstawie logu ADIF jednej stacji oraz, opcjonalnie, drugiej stacji.

## Wczytywanie logów

- **Stacja 1**: *Wczytaj ADIF* / *Zamknij ADIF*. Program pobiera z logu `STATION_CALLSIGN` i `MY_GRIDSQUARE`; pozycją nadawania jest `MY_GRIDSQUARE` każdego QSO.
- **Stacja 2** (opcjonalnie): te same przyciski w drugim wierszu; pole *Stacja 2* pokazuje ją lub ukrywa. Trasy stacji 1 są błękitne, trasy stacji 2 fioletowe.
- Program używa QSO na 50 MHz z lokatorem (brakujące lub 4-znakowe lokatory pobierane są z CALL3), wieloskokowych: powyżej 3100 km (reguła autora; liczba skoków jest zgodna z tabelą IK3XTV).
- Łączności **transrównikowe** są wykluczane (reguła autora): w kierunku Afryki i Ameryki Południowej, z punktem środkowym w pasie inklinacji magnetycznej od -15° do +15°, chyba że log ma `PROP_MODE=ES`.

## Dzień i godziny

Wybierz **dzień** (przy otwarciu ten z największą liczbą wieloskokowych QSO) i przedział godzin **od… do…** UTC, z krokiem 15 minut. Pole **foEs** (opcjonalne) koloruje trasy według minimalnej foEs i pokazuje ją w tabeli.

## Mapa

Mapa **Merkatora** z dowolnym środkiem: kółko = powiększenie, przeciąganie = przesunięcie.

![Trasa](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Rozmyta żółta kropka** oznacza środek każdego skoku, gdzie przypuszczalnie znajduje się obłok Es; na trasie pod kursorem myszy staje się większa.
- **Mysz blisko trasy**: linia i ikony są powiększane i pojawia się **6-znakowy kwadrat WWL** stacji DX.
- **Lewy klik** na tej trasie: okienko pyta, czy pokazać **wszystkie QSO z pliku** (wszystkie dni) z drugim końcem trasy **w promieniu 150 km** od tego punktu. *Tak*: mapa jest czyszczona i pokazuje te QSO wewnątrz pomarańczowego koła; *Nie*: mapa pozostaje bez zmian. *Powrót do dnia i godziny* przywraca poprzedni wybór.

![Okolice](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Szczegóły

Dla wybranej trasy: odległość i ocena według tabeli IK3XTV, azymut w obu kierunkach, skoki równe z kwadratami punktów odbicia, krótki pierwszy skok do 1200 km (doświadczenie IC8TEM) oraz, przy zaznaczonym polu foEs, minimalna foEs.

## Sprawdzenie łańcuchów (od wersji 0.6.0)

**Wczytaj spoty, ADIF lub ALL.TXT** (spoty wyeksportowane z OBŁOKI Es W CZASIE, logi ADIF innych stacji, dekodowania ALL.TXT z JTDX lub WSJT-X: znak wywoławczy rozpoznaje się po wierszach nadawania) i **Sprawdź łańcuchy**: program odtwarza na otwarciach wieloskokowych stacji 1 regułę łańcuchów z krótkimi skokami i podaje, ile otwarć by przewidział (ALERT, PREKURSOR lub tylko pierwszy krótki skok), medianę wyprzedzenia oraz alerty, po których nie było QSO wieloskokowych. Bez wczytanych źródeł obłoki pochodzą tylko z logu, a ostatni krótki skok jest prawie niewidoczny: potrzebne są dane odległych stacji.

**Test alertów** (od wersji 0.6.5): dla ALERTÓW i PREKURSORÓW odtwarzanych co 15 minut raport mówi, ile się sprawdziło (QSO wieloskokowe w tym samym kierunku w ciągu następnych 2 godzin), fałszywe alarmy i wartość odniesienia w tych samych sektorach bez łańcuchów.

![Sprawdzenie](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1p_benchmark_50_verifica_catene.png)
