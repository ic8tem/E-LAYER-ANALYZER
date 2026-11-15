# OBŁOKI Es W CZASIE

[← Strona główna](Home-pl)

![OBŁOKI Es W CZASIE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Podzakładka **SPORADIC-E** (od wersji 0.5.2): jak powstają, przesuwają się i gasną **obłoki E-sporadycznego**, krok po kroku.

## Metoda

Metoda **Chartiera, Hanleya i Emmonsa (2022)**: każda łączność E-sporadyczna mówi, że w punkcie odbicia foEs wynosi **co najmniej f / sec φ**. Punkty zbiera się w komórki i kroki czasu, a każda komórka przyjmuje **najwyższą wartość** z kroku. Minimalna gęstość elektronowa to NmE = 1,24·10¹⁰ · foEs² elektronów na metr sześcienny.

Program używa punktów odbicia własnego modelu (wszystkie skoki), a nie tylko punktu środkowego. Wartość jest **dolną granicą**: pusta komórka oznacza tylko brak łączności.

## Źródło i elementy sterujące

- **Spoty z sesji** (odświeżane przy każdym otwarciu zakładki) albo **Wczytaj log ADIF lub spoty CSV**.
- **Eksportuj spoty z sesji (CSV)**: archiwum spotów opróżnia się przy zamknięciu programu, więc możesz je obejrzeć później.
- **Pasma MHz** (28, 40, 50, 70, 144), **Krok** (5, 10 lub 15 minut), **Komórka** (1°, 2° lub 4°), **Wysokość Es km**.
- Pasek czasu: ⏮ ◀ *Uruchom/Zatrzymaj* ▶ ⏭ i suwak; po otwarciu startuje się od kroku z największą liczbą komórek.
- *Poprzednie kroki wyblakłe* pokazuje trzy kroki przed wybranym; *Łączności* rysuje trasy w kolorach pasm.

## Tabela i wykres

- Komórki kroku: Komórka (południowo-zachodni róg i lokator środka), foEs≥, NmE≥, Łączności, Pasma. Kliknięcie komórki wyróżnia jej łączności.
- Wykres: najwyższa minimalna foEs (żółta linia) i liczba komórek (słupki) dla każdego kroku.
