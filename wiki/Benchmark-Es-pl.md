# BENCHMARK Es

[← Home](Home-pl)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Stanowisko testowe dla **sporadycznej warstwy E** i **wieloskoku** na **całym logu ADIF**, nawet innej stacji: na podstawie faktycznie przeprowadzonych QSO program szkicuje skip i obłoki, dzień po dniu i godzina po godzinie.

## Wczytywanie logu

- **Wczytaj ADIF**: wybiera plik `.adi`/`.adif`. Program zapamiętuje plik i wczytuje go ponownie przy starcie.
- **Usuń ADIF**: usuwa log z zakładki.
- **Stacja w środku**: znak i lokator stacji, która prowadziła log. Program proponuje najczęstszy `STATION_CALLSIGN` (lub `OPERATOR`) i `MY_GRIDSQUARE`; można je zmienić ręcznie i potwierdzić przyciskiem **Zastosuj**. Środek nie zależy od ustawień ogólnych, więc można badać logi innych stacji.
- Pozycja nadawania: `MY_GRIDSQUARE` każdego QSO (jeśli brak, lokator środka). Pozycje odbioru: pola `GRIDSQUARE`.
- Używane QSO: 28, 50, 70 i 144 MHz z lokatorem. QSO z innym `PROP_MODE` (TR, MS, FAI, AUR, EME...) oraz te poniżej 500 km można wykluczyć. 432 MHz nigdy nie jest uwzględniane: sporadyczna warstwa E tam nie występuje.

## Dni i godziny

- **QSO na dzień**: dzień, liczba QSO, pasma, maksymalna odległość, wieloskoki. Przy otwarciu program proponuje dzień z największą liczbą QSO.
- **Godziny dnia**: wiersz *Cały dzień* i po jednym wierszu dla każdej godziny UTC; kliknięcie pokazuje tylko QSO z tej godziny.
- **QSO**: lista dla wybranego zakresu; kliknięcie QSO (lub w pobliżu stacji DX na mapie) wyróżnia je i pokazuje jego szczegóły.

## Mapa

Mapa **azymutalna równoodległościowa** wyśrodkowana na stacji, z suwakiem **Maksymalna odległość** do powiększania (także kółkiem myszy): pierścienie pokazują **km od środka**.

- **Skip według sektorów** (10° azymutu): pomarańczowy łuk = najbliższe QSO w sektorze (krawędź strefy skip), zielony łuk = najdalsze QSO, pomiędzy nimi osiągnięty obszar.
- **Obłoki Es** na kwadratach Maidenhead, kolorowane według szacowanej foEs.
- Punkt środkowy pojedynczych skoków (żółty), pierwszy skok stacji (pomarańczowy, puste kółko = krótki pierwszy skok), pierwszy skok stacji DX (fioletowy).
- Dla całego dnia **czerwona linia** łączy godzina po godzinie środek ciężkości punktów środkowych: to dryf obłoków.

![Jedna godzina](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Proponowane obliczenia

1. **Skip według sektorów**: dla każdego sektora odległość skip i zasięg, ze znakami.
2. **Szacowana foEs** z prawa secansa (foEs ≥ f / sec φ): najkrótszy pojedynczy skok na każdym paśmie daje najwyższe oszacowanie; z tą foEs oczekiwana MUF na 1000, 1500, 2000 i 2400 km oraz odległość, powyżej której otworzyłoby się 144 MHz.
3. **Obłoki Es** szacowane na kwadratach, z liczbą QSO i foEs.
4. **Wieloskok**: pierwszy skok stacji w dwóch hipotezach (równe skoki oraz krótki pierwszy skok do 1200 km, doświadczenie IC8TEM) porównany z obłokami widzianymi w pojedynczym skoku w przedziale ±30 minut; w promieniu 300 km hipoteza jest potwierdzona. Program liczy potwierdzenia każdej hipotezy.
5. **Dryf obłoków** godzina po godzinie: km, kierunek i prędkość.

Obliczenia są orientacyjne: QSO mówi, że trasa była otwarta, a nie dokładnie, gdzie był obłok. Im czystsze dane (6-znakowe lokatory, poprawny `PROP_MODE`), tym bardziej wiarygodny skip.
