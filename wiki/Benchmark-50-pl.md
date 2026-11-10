# BENCHMARK 50 MHz

[← Home](Home-pl)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Stanowisko testowe dla **wieloskokowej sporadycznej warstwy E na 50 MHz** z użyciem logu ADIF jednej stacji i opcjonalnie drugiej stacji.

## Wczytywanie logów

- **Stacja 1**: *Wczytaj ADIF* / *Usuń ADIF*. Program pobiera z logu `STATION_CALLSIGN` i `MY_GRIDSQUARE`; pozycją nadawania jest `MY_GRIDSQUARE` każdego QSO.
- **Stacja 2** (opcjonalnie): te same przyciski w drugim wierszu; pole *Stacja 2* ją pokazuje lub ukrywa. Trasy stacji 1 są jasnoniebieskie, trasy stacji 2 fioletowe.
- Program używa QSO na 50 MHz z lokatorem (brakujące lub 4-znakowe lokatory są brane z CALL3), wieloskokowych: powyżej 3100 km (reguła autora; liczba skoków według tabeli IK3XTV).
- Łączności **transrównikowe** są wykluczane (reguła autora): w kierunku Afryki i Ameryki Południowej, z punktem środkowym w pasie inklinacji magnetycznej od -20° do +20°, chyba że log ma `PROP_MODE=ES`.

## Dzień i godzina

Wybierz **dzień** (przy otwarciu ten z największą liczbą QSO wieloskokowych) oraz godzinę **od… do…** UTC, w krokach co 15 minut. Pole **foEs** (opcjonalne) koloruje trasy według minimalnej foEs i pokazuje ją w tabeli.

## Mapa

Mapa **Merkatora** ze swobodnym środkiem: kółko = zoom, przeciąganie = przesuwanie.

![Trasa](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Mysz w pobliżu trasy**: linia i ikony są powiększane i pojawia się **6-znakowy kwadrat WWL** stacji DX.
- **Lewy klik** na tej trasie: okno pyta, czy pokazać **wszystkie QSO z pliku** (ze wszystkich dni), w których pozycja drugiej strony leży **w promieniu 150 km** od tego punktu. *Tak*: mapa jest czyszczona i pokazuje te QSO wewnątrz pomarańczowego okręgu; *Nie*: mapa pozostaje bez zmian. *Powrót do dnia i godziny* przywraca poprzedni wybór.

![Okolice](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Szczegóły

Dla wybranej trasy: odległość i ocena według tabeli IK3XTV, azymut w obu kierunkach, równe skoki z kwadratami punktów odbicia, krótki pierwszy skok do 1200 km (doświadczenie IC8TEM) oraz, przy zaznaczonym polu foEs, minimalna foEs.
