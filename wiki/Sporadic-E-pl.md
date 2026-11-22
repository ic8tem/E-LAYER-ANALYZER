# SPORADIC-E

[← Strona główna](Home-pl)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Mapa

Przycisk nad mapą wybiera widok:

- **Azymutalna**: mapa azymutalna równoodległościowa wyśrodkowana na twojej stacji, z suwakiem **powiększenia** (od 300 do 20000 km od środka, także kółkiem myszy).
- **Merkator**: mapa świata, z przyciskami Europa i Świat.

W obu odwzorowaniach wokół stacji rysowane są **okręgi odległości Flavia IK3XTV**: 500 km (minimum pojedynczego skoku), 1500-2000 km (optymalna), 2400 km (maksimum pojedynczego skoku), 5000 km (maksimum podwójnego skoku) i 6900 km (maksimum potrójnego skoku).

W widokach azymutalnych przerywane pierścienie pokazują **km od środka** (krok zmienia się wraz z powiększeniem), krawędź oznacza maksymalną wyświetlaną odległość, a kursor myszy podaje odległość i azymut od środka.

![Wieloskok](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Co pokazuje

- **Spoty** z ostatnich minut (15 min - 12 h) na pasmach **28, 40 (8 m, domyślnie wyłączone), 50, 70 i 144 MHz**, pobrane z sesji telnet. Lokatory pochodzą z komentarza spotu (np. `JN70CN<ES>JO62QM`) lub z archiwum CALL3.
- Spoty z **<MS>**, **<AU>** lub **<TR>** w tekście są **odrzucane**: nie dotyczą E-sporadycznego, ale przydają się do meteor scatter, zorzy lub tropo.
- **Transrównikowa, reguła autora (IC8TEM)**: w kierunku Afryki i Ameryki Południowej, jeśli punkt środkowy wypada w pasie inklinacji magnetycznej od -15° do +15° (wartość przejęta od Cracknella, UKSMG: strefy TEP około 5-15° od linii zerowej inklinacji), łączność jest transrównikowa (TEP), a nie E-sporadyczna, chyba że spot deklaruje `<ES>`.
- **144 MHz, reguła autora (IC8TEM)**: E-sporadyczne na 2 m zaczyna się **powyżej 900 km** (krótsze spoty są odrzucane). Pojedynczy spot na 144 MHz **nie jest rysowany samodzielnie**: pojawia się, gdy w ciągu **5 minut** nadejdzie drugi spot na 144 MHz z obłokiem w tym samym miejscu (punkty odbicia w promieniu 300 km) lub gdy w pobliżu jest spot na **50 MHz z krótkim skokiem** (do 1200 km, w ciągu ostatnich 15 minut). Pasek u góry pokazuje, ile spotów na 144 MHz czeka na potwierdzenie.
- Dla każdego spotu: odległość, **liczba skoków** według tabeli **IK3XTV**, elewacja, **minimalna foEs** (prawo secansa).
- **Pierwszy skok obu stacji**: pomarańczowy kwadrat = pierwszy skok spottera, fioletowy = pierwszy skok DX (skoki równe); żółty = punkt środkowy pojedynczego skoku.
- **Krótki pierwszy skok** (puste kółka): przy otwarciach wieloskokowych autor obserwował krótki pierwszy skok do **1200 km** od własnego QTH; długość można ustawić.
- **Obłoki Es** na **kwadratach Maidenhead** (2° x 1°), pokolorowane według minimalnej foEs: niebieski < 5, błękitny < 7, zielony < 10, żółty < 14, pomarańczowy < 20, czerwony > 20 MHz.
- Tabela **Obłoki Es**: dla każdego kwadratu **MUF dla twojej stacji** (czerwony = możliwe 144 MHz, pomarańczowy = 70, żółty = 50).

## Tabela IK3XTV

| | minimalna | optymalna | maksymalna |
|---|---|---|---|
| Pojedynczy skok | 500-650 km | 1500-2000 km | 2200-2400 km |
| Podwójny skok | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Potrójny skok | – | 4800-6400 km | 6900 km |

## Jak używać

- **Kliknij spot**: trasa zostaje podświetlona, a w szczegółach pojawiają się punkty odbicia, lokatory i azymuty.
- **Kliknij spot** w tabeli: trasa podświetla się na mapie (żółta poświata), a jeśli jest poza widokiem, mapa przesuwa się lub zwiększa promień, aby ją pokazać. Spot pozostaje wybrany, gdy nadchodzą nowe spoty.
- **Tabela podąża za mapą**: przy ustawionym powiększeniu tabela spotów i tabela obłoków pokazują tylko to, co widać na mapie.
- **Kliknij mapę**: punkt jest traktowany jako możliwy obłok Es, z foEs potrzebną dla 28, 50, 70 i 144 MHz.

## Uwaga o 432 MHz

E-sporadyczne **nie dotyczy 432 MHz**: MUF Es sięga około 150-200 MHz, a rekordy ARRL wymieniają łączności Es tylko na 50 i 144 MHz. Spoty z pasma 70 cm nie są tu analizowane.

## Kwadraty kolorowane według częstotliwości i miganie MUF

Kwadraty Maidenhead punktów odbicia są kolorowane według najwyższej częstotliwości spotów: **zielony** do 29 MHz, **żółty** do 51, **pomarańczowy** do 71, **czerwony** do 145 MHz. Z foEs kwadratu szacowana jest prawdopodobna MUF na najdłuższym skoku (2400 km, tabela IK3XTV): jeśli osiąga wyższe pasmo, kwadrat **miga** kolorem tego pasma (zasada IK3XTV i Jacobsa-Leinwolla: krótki skip oznacza wysoką MUF). Dla 144 MHz obowiązuje reguła autora: kwadrat miga na czerwono tylko wtedy, gdy obłok daje pojedynczy skok na 50 MHz między 450 a 500 km (450 km z tolerancją 50 km).

W prawym górnym rogu mapy pojawia się podsumowanie N0NBH (E-skip dla wybranego kontynentu, SFI, indeksy K i A).

## Reguły według pasma (wersja 0.4.18)

- **50 MHz**: brak E-sporadycznego do 450 km. Spoty FT8 i FT4 są E-sporadyczne tylko wtedy, gdy należą do obłoku: inny spot na 50 MHz w ciągu 4 minut z punktem środkowym w promieniu 150 km. Pojedynczy spot FT8/FT4 trafia do meteor scatter.
- **70 MHz**: brak E-sporadycznego do 450 km. FT8 i FT4 trafiają do E-sporadycznego nawet w oknie meteor scatter bandplanu (najpierw decyduje znana emisja, potem częstotliwość).
- **40 MHz (8 m)**: odległość minimalna wyznaczona z 450 km dla 50 MHz prawem siecznej (około 330 km); tylko odległości w granicach pierwszego skoku geometrii warstwy E (około 2440 km). Kolor trasy: żółty.
- **144 MHz**: E-sporadyczne od 900 km, z potwierdzeniem; spoty Q65 i JT65 to EME i są odrzucane, chyba że w tekście jest `<ES>`.
- G0ISW (Propagation identification, 2024) obserwuje najkrótsze E-sporadyczne powyżej 400 km na 50 i 70 MHz i powyżej 1400 km na 144 MHz: reguły autora zgadzają się na 50 i 70 MHz, a na 144 MHz są szersze, ale wymagają potwierdzenia.
- Kolejność, w jakiej stosowane są wszystkie reguły, z używanymi wartościami, pokazuje **[USTAWIENIA](Settings-pl)** › Kierowanie spotów.

## Łańcuchy z krótkimi skokami (od wersji 0.6.0)

![Łańcuch z QTH](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

W podzakładce **Prekursory wieloskoku** program szuka łańcuchów wieloskoku na 50 MHz według trzech warunków autora: **pierwszy i ostatni krótki skok** (500-1200 km) z MUF około 50 MHz, wymagające foEs 11-21 MHz, oraz **skoki środkowe** do 2200 km, wymagające około 9,3 MHz.

- **Silne obłoki** (pomarańczowe kwadraty): spoty Es z całego świata i jonosondy GIRO z foEs co najmniej 11,3 MHz.
- **Z QTH**: pierwszy krótki skok od twojej stacji i ostatni krótki skok na innym silnym obłoku w tym samym kierunku, także na tym samym kontynencie (na przykład w Kazachstanie); okrąg to strefa docelowa.
- **Korytarz**: dowolne dwa silne obłoki, w każdym kierunku, na przykład **Karaiby - Półwysep Iberyjski** albo **Europa - Afryka Zachodnia** (bez TEP); «dotyczy cię», jeśli jeden koniec jest w promieniu 1200 km od ciebie.
- **Szukaj obłoku**: jest tylko pierwszy krótki skok; błękitne strefy pokazują, gdzie powinien być obłok końcowego krótkiego skoku.
- **ALERT**, jeśli skoki środkowe są pokryte obłokami, **PREKURSOR**, jeśli brakuje tylko stref bez obserwatorów.
- W USTAWIENIACH jest **komunikat** u góry (opcjonalnie także okno) dla ALERTÓW, które cię dotyczą.
- Łańcuchy, które docierają **na otwarte morze** lub tam, gdzie nie ma stacji (CALL3 i spoty), nie są pokazywane.
- **Równy podwójny skok** (od wersji 0.6.3, skoki «2 =»): do około 4700 km, na przykład w kierunku Kazachstanu, z obłokiem w jednej czwartej trasy jak w metodzie ITU-R P.534-6.
- Zakładka **TABELA SKOKÓW** podsumowuje, ile skoków potrzeba na 50 MHz od 500 do 12000 km, z foEs każdego skoku i źródłami.
- **Zasięg mojego sygnału na 6 m** (różowe punkty): gdzie dociera twój sygnał na 50 MHz po 2-5 skokach, nad obłokami z ostatnich 30 minut szacowanymi ze spotów ze wszystkich pasm (28-144 MHz) i z jonosond; tylko tam, gdzie są stacje.
- **Korytarz z równymi skokami** (od wersji 0.6.5): dwa obłoki odległe od siebie o 1400-2350 km łączą dwoma równymi skokami dwie strefy odległe o 2800-4700 km; sprawdzenie na wyprawie 5U5K potwierdza granicę około 4700 km.
- **WCZESNY SYGNAŁ** (zielony, od wersji 0.6.5): obłok Es w paśmie 28 lub 24 MHz tam, gdzie pasmo 50 MHz jeszcze się nie otworzyło; w próbach 50 MHz otworzyło się tam w ciągu godziny w 50% przypadków (30% bez niego), 9-16 minut wcześniej.
- **Powtórzony APRS** (czerwony, od wersji 0.6.5): co najmniej dwa skoki APRS w paśmie 2 m powyżej 900 km przez ten sam obłok w ciągu 10 minut = ALERT 144 MHz; zakładka miga.
- **Ile są warte alerty** (na stacji autora, lata 2024-2026): ALERTY trafne w 41-57% (QSO wieloskokowe w tym samym kierunku w ciągu godziny) wobec 5-7% bez alertu; ale 47% otwarć przychodzi bez ostrzeżenia. Zasięg dochodzi do promienia 500 km wokół odległej stacji w 13,5% otwarć, po obróceniu w 2-4%.
- Obłoki **nie są przesuwane**: próba z 22.11.2026 pokazuje, że przesuwanie ich z ich prędkością nie poprawia prognozy na 15-45 minut.
- **Prognoza TEP** (niebieska, od wersji 0.6.5): indeks 0-100 w kierunku stref za równikiem magnetycznym = pora roku × SFI × Kp (tylko eTEP) × czas lokalny × foF2 jonosond GIRO; SFI waży, ale nie zeruje, bo w minimum słonecznym TEP staje się rzadki, ale nie znika.

![Korytarz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)
