# USTAWIENIA

[← Strona główna](Home-pl)

Od wersji 0.4.18 zakładka **USTAWIENIA** zbiera ogólne opcje programu i kierowanie spotów do zakładek. Ma dwie podzakładki.

## Ogólne

| Sekcja | Co ustawiasz |
|---|---|
| Stacja | Znak wywoławczy, lokator i wysokość, te same co na górnym pasku; **Zastosuj** zatwierdza je dla wszystkich zakładek. |
| Wygląd | Język, kolorystyka (schematy YAEC i YA-MS oraz, od wersji 0.5.0, sześć motywów IP-TRX-CTRL: Jasny, Noc, Czarny, Niebieski, Fioletowy, Zielony) i **rozmiar czcionki całego programu** (85-140 %, jak w YA-MS). Język i kolory przebudowują okno: ustawienia i spoty zostają zachowane. |
| Mapy i dane prognostyczne | Kontynent, to samo menu co na pasku zakładek. |
| Czas | Stan NTP i różnica między czasem serwerów a zegarem PC. |
| Aktualizacje | Sprawdzanie na GitHubie przy starcie i co 30 minut, automatyczna i cicha aktualizacja, przycisk **Sprawdź teraz**. Automatyczne aktualizacje nie dotyczą pakietów systemowych (.deb, .rpm, Arch, Slackware). |
| Dane | Przycisk, który otwiera folder `data`. Klucz API Bureau of Meteorology pozostaje w DATA COLLECTION › Dane propagacyjne. |

## Kierowanie spotów

Po lewej tabela wymienia reguły, według których program decyduje, dokąd trafia każdy spot, **w kolejności, w jakiej są stosowane**: pierwsza pasująca reguła decyduje o zakładce. Pokazane wartości to te ustawione w filtrach.

| # | Jeśli spot... | trafia do |
|---|---|---|
| 1 | jest poza filtrem lokalnym (poniżej 27990 kHz lub powyżej 145 MHz; 430-440 MHz tylko zorza) | odrzucony |
| 2 | zawiera AUE, AU-E lub AURORAL E | AURORA (zorza-E) |
| 3 | zawiera słowo zorzy (domyślnie AU, AUR) | AURORA |
| 4 | zawiera słowo F.A.I. (domyślnie FAI; pasma 6, 4 i 2 m: F.A.I. nie sięga 432 MHz) | F.A.I. |
| 5 | ma tryb meteor scatter lub `<MS>` | METEOR SCATTER |
| 6 | wypada w oknie MS bandplanu (na 50 MHz niewyłączne; FT8 i FT4 na 50 i 70 MHz wyłączone) | METEOR SCATTER |
| 7 | to FT8 lub FT4 na 50 MHz bez innego spotu w ciągu 4 min z punktem środkowym w promieniu 150 km | meteor scatter (nie E-sporadyczne) |
| 8 | to Q65 lub JT65 na 144 MHz (chyba że `<ES>`) | EME: odrzucony |
| 9 | zawiera napis innej propagacji | nie E-sporadyczne |
| 10 | jest transrównikowy | nie E-sporadyczne (TEP) |
| 11-15 | jest na 28, 40, 50, 70 lub 144 MHz w granicach reguł odległości | SPORADIC-E |
| 16 | nie pasuje do żadnej reguły (tropo, 432 MHz, KF...) | żadna zakładka (zostaje w Zebranych spotach) |

Po prawej są **filtr lokalny** i filtry każdego rodzaju propagacji, w pięciu podzakładkach: Meteor scatter (filtr YA-MS ustawia się w zakładce METEOR SCATTER), E-sporadyczne, F.A.I., Zorza i **Test filtrów**, gdzie wkleja się wiersz z klastra i widzi, jak zostaje sklasyfikowany. Przycisk **Przywróć reguły autora** przywraca wartości domyślne.
