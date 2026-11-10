# SPORADIC-E

[← Home](Home-pl)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Mapa

Przycisk nad mapą wybiera widok:

- **Azymutalna**: mapa azymutalna równoodległościowa wyśrodkowana na twojej stacji, z suwakiem **zoomu** (od 300 do 20000 km od środka, także kółkiem myszy).
- **Merkator**: mapa świata, z przyciskami Europa i Świat.

W obu odwzorowaniach wokół stacji rysowane są **okręgi odległości Flavia IK3XTV**: 500 km (minimum pojedynczego skoku), 1500-2000 km (optimum), 2400 km (maksimum pojedynczego skoku), 5000 km (maksimum podwójnego skoku) i 6900 km (maksimum potrójnego skoku).

W widokach azymutalnych przerywane pierścienie pokazują **km od środka** (krok zmienia się z zoomem), krawędź wyznacza maksymalną pokazywaną odległość, a kursor myszy podaje odległość i azymut od środka.

![Wieloskok](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Co pokazuje

- **Spoty** z ostatnich minut (15 min - 12 h) na pasmach **28, 50, 70 i 144 MHz**, pobrane z sesji telnet. Lokatory pochodzą z komentarza spotu (np. `JN70CN<ES>JO62QM`) lub z bazy CALL3.
- Spoty z **<MS>**, **<AU>** lub **<TR>** w tekście są **odrzucane**: nie dotyczą sporadycznej warstwy E, ale przydają się dla meteor scatter, zorzy lub tropo.
- **Transrównikowa, reguła autora (IC8TEM)**: w kierunku Afryki i Ameryki Południowej, jeśli punkt środkowy wypada w pasie inklinacji magnetycznej od -20° do +20°, łączność jest transrównikowa (TEP), a nie przez sporadyczną warstwę E, chyba że spot podaje `<ES>`.
- **144 MHz, reguła autora (IC8TEM)**: sporadyczna warstwa E na 2 m zaczyna się **powyżej 900 km** (krótsze spoty są odrzucane). Pojedynczy spot na 144 MHz **nie jest rysowany sam**: pojawia się, gdy w ciągu **5 minut** nadejdzie drugi spot na 144 MHz z obłokiem w tym samym miejscu (punkty odbicia w promieniu 300 km) albo gdy w pobliżu jest spot **krótkiego skoku na 50 MHz** (do 1200 km, w ostatnich 15 minutach). Pasek na górze pokazuje, ile spotów na 144 MHz czeka na potwierdzenie.
- Dla każdego spotu: odległość, **liczba skoków** według tabeli **IK3XTV**, elewacja, **minimalna foEs** (prawo secansa).
- **Pierwszy skok obu stacji**: pomarańczowy kwadrat = pierwszy skok spottera, fioletowy = pierwszy skok DX (równe skoki); żółty = punkt środkowy pojedynczego skoku.
- **Krótki pierwszy skok** (puste kółka): przy otwarciach wieloskokowych autor obserwował krótki pierwszy skok do **1200 km** od swojego QTH; długość można ustawić.
- **Obłoki Es** na **kwadratach Maidenhead** (2° x 1°), kolorowane według minimalnej foEs: niebieski < 5, jasnoniebieski < 7, zielony < 10, żółty < 14, pomarańczowy < 20, czerwony > 20 MHz.
- Tabela **Obłoki Es**: dla każdego kwadratu **MUF dla twojej stacji** (czerwony = możliwe 144 MHz, pomarańczowy = 70, żółty = 50).

## Tabela IK3XTV

| | minimum | optimum | maksimum |
|---|---|---|---|
| Pojedynczy skok | 500-650 km | 1500-2000 km | 2200-2400 km |
| Podwójny skok | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Potrójny skok | – | 4800-6400 km | 6900 km |

## Jak używać

- **Kliknij spot**: trasa jest wyróżniona, a w szczegółach są punkty odbicia, lokatory i azymuty.
- **Kliknij spot** w tabeli: trasa podświetla się na mapie (żółta poświata), a jeśli jest poza widokiem, mapa przesuwa się lub zwiększa promień, aby ją pokazać. Spot pozostaje zaznaczony, gdy przychodzą nowe spoty.
- **Tabela podąża za mapą**: przy ustawionym zoomie tabela spotów i tabela obłoków pokazują tylko to, co widać na mapie.
- **Kliknij mapę**: punkt jest traktowany jako możliwy obłok Es, z foEs potrzebną dla 28, 50, 70 i 144 MHz.

## Uwaga o 432 MHz

Sporadyczna warstwa E **nie dotyczy 432 MHz**: MUF Es sięga około 150-200 MHz, a rekordy ARRL wymieniają łączności Es tylko na 50 i 144 MHz. Spoty z pasma 70 cm nie są tu analizowane.

## Kwadraty kolorowane według częstotliwości i migająca MUF

Kwadraty Maidenhead punktów odbicia są kolorowane według najwyższej częstotliwości spotów: **zielony** do 29 MHz, **żółty** do 51, **pomarańczowy** do 71, **czerwony** do 145 MHz. Z foEs kwadratu szacowana jest prawdopodobna MUF na najdłuższym skoku (2400 km, tabela IK3XTV): jeśli osiąga wyższe pasmo, kwadrat **miga** w kolorze tego pasma (zasada IK3XTV i Jacobsa-Leinwolla: krótki skip oznacza wysoką MUF).

W prawym górnym rogu mapy pojawia się podsumowanie N0NBH (E-skip dla wybranego kontynentu, SFI, wskaźniki K i A).
