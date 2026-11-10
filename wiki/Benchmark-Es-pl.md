# BENCHMARK Es

[← Strona główna](Home-pl)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Stanowisko testowe dla **E-sporadycznego** i **wieloskoku** na **całym logu ADIF**, także innej stacji: na podstawie rzeczywiście przeprowadzonych QSO program szkicuje skip i obłoki, dzień po dniu i godzina po godzinie.

## Wczytywanie logu

- **Wczytaj ADIF**: wybiera plik `.adi`/`.adif`. Program zapamiętuje plik i wczytuje go ponownie przy uruchomieniu.
- **Zamknij ADIF**: usuwa log z zakładki.
- **Stacja w środku**: znak wywoławczy i lokator stacji, która prowadziła log. Program proponuje najczęstszy `STATION_CALLSIGN` (lub `OPERATOR`) i `MY_GRIDSQUARE`; można je zmienić ręcznie i zatwierdzić przyciskiem **Zastosuj**. Środek nie zależy od ustawień ogólnych, więc można badać logi innych stacji.
- Pozycja nadawania: `MY_GRIDSQUARE` każdego QSO (jeśli brak, lokator środka). Pozycje odbioru: pola `GRIDSQUARE`.
- Używane QSO: 28, 50, 70 i 144 MHz z lokatorem. Można wykluczyć QSO z innym `PROP_MODE` (TR, MS, FAI, AUR, EME...) oraz te poniżej 500 km. Pasmo 432 MHz nigdy nie jest uwzględniane: E-sporadyczne nie ma zastosowania.

## Dni i godziny

- **QSO na dzień**: dzień, liczba QSO, pasma, odległość maksymalna, wieloskoki. Przy otwarciu program proponuje dzień z największą liczbą QSO.
- **Godziny dnia**: wiersz *Cały dzień* i po jednym wierszu dla każdej godziny UTC; kliknięcie pokazuje tylko QSO z tej godziny.
- **QSO**: lista dla wyboru; kliknięcie QSO (lub w pobliżu stacji DX na mapie) podświetla je i pokazuje jego szczegóły.

## Mapa

Mapa **azymutalna równoodległościowa** wyśrodkowana na stacji, z suwakiem **Odległość maksymalna** do powiększania (także kółkiem myszy): pierścienie pokazują **km od środka**.

- **Skip według sektorów** (10° azymutu): pomarańczowy łuk = najbliższe QSO w sektorze (krawędź strefy martwej), zielony łuk = najdalsze QSO, pomiędzy nimi osiągnięty obszar.
- **Obłoki Es** na kwadratach Maidenhead, pokolorowane według szacowanej foEs.
- Punkt środkowy pojedynczych skoków (żółty), pierwszy skok stacji (pomarańczowy, puste kółko = krótki pierwszy skok), pierwszy skok stacji DX (fioletowy).
- **Rozmyta żółta kropka** oznacza środek każdego skoku, gdzie przypuszczalnie znajduje się obłok Es (także środek krótkiego pierwszego skoku, gdy ta hipoteza jest włączona).
- Dla całego dnia **czerwona linia** łączy środek ciężkości punktów środkowych godzina po godzinie: to przemieszczanie się obłoków.

![Jedna godzina](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Proponowane obliczenia

1. **Skip według sektorów**: dla każdego sektora odległość skipu i zasięg, ze znakami wywoławczymi.
2. **Szacowana foEs** z prawa secansa (foEs ≥ f / sec φ): najkrótszy pojedynczy skok w każdym paśmie daje najwyższy szacunek; przy tej foEs oczekiwana MUF na 1000, 1500, 2000 i 2400 km oraz odległość, powyżej której otworzyłoby się pasmo 144 MHz.
3. **Obłoki Es** szacowane na kwadratach, z liczbą QSO i foEs.
4. **Wieloskok**: pierwszy skok stacji w dwóch hipotezach (skoki równe oraz krótki pierwszy skok do 1200 km, doświadczenie IC8TEM) porównany z obłokami widzianymi na pojedynczym skoku w ciągu ±30 minut; w promieniu 300 km hipoteza jest potwierdzona. Program zlicza potwierdzenia każdej hipotezy.
5. **Przemieszczanie się obłoków** godzina po godzinie: km, kierunek i prędkość.

Obliczenia są orientacyjne: QSO mówi, że trasa była otwarta, a nie gdzie dokładnie znajdował się obłok. Im czystsze dane (6-znakowe lokatory, poprawny `PROP_MODE`), tym bardziej wiarygodny skip.
