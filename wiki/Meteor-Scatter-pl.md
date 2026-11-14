# METEOR SCATTER

[← Strona główna](Home-pl)

Ta zakładka zawiera pełny **YA-MS 1.8.9**, program meteor scatter autorstwa IC8TEM: sferę niebieską z radiantami, strumień z radarów SKiYMET (Collm, Sodankylä), obliczanie efektywności rojów, eksperymentalne prognozy, geometrię i tłumienie, GRAVES, mapę kwadratów, **Roje na żywo** (ZHR mierzone przez Global Meteor Network i radianty z radaru CMOR), **Mapę źródeł** (Collm, Sodankylä, GRAVES i CMOR względem stacji) oraz dokument **Wyjaśnienie obliczania wydajności** ze wzorami.

YA-MS 1.5.0 dodaje zakładkę **Zbieranie danych** (spoty meteor scatter filtrowane według MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Wewnątrz E-LAYER ANALYZER nie otwiera własnych sesji telnet: otrzymuje wiersze z sesji DATA COLLECTION, więc nie ma podwójnych logowań tym samym znakiem wywoławczym.

YA-MS 1.6.0 porządkuje zakładki w grupy **Na żywo i dane w czasie rzeczywistym** (ruch meteor scatter na żywo z obłokiem każdej łączności, strumień radarowy, GRAVES, zbieranie danych) oraz **Obliczenie prognozy** (prognoza, geometria, obliczanie efektywności, eksperymentalne prognozy).

YA-MS 1.7.0 i 1.8.0 dodają porównanie radiantów z katalogiem IAU MDC, dzienne zliczenia Global Meteor Network dla każdego roju, indeks k wyznaczany ze współczynników populacji, początkowy promień śladu według Stober i in. (2021), czas trwania pingów w różnych pasmach (domyślnie λ² lub pomiary HamSCI MSQP wykonane przez NN4SA) oraz porównanie z detekcjami BRAMS w Sonnebergu (Perseidy 2026). Automatyczna aktualizacja YA-MS jest wyłączona wewnątrz E-LAYER ANALYZER, który aktualizuje się sam z Releases na GitHubie.

YA-MS 1.8.1 traktuje listek anteny jako charakterystykę pionową (pole «charakterystyka anteny», domyślnie włączone): punkty odbicia są ważone pionowym zyskiem obu anten i wydajności nie znikają już na długich trasach. Dochodzi ostrzeżenie «brak wspólnej strefy», wyróżnione pole kąta β (wybór, który waży najwięcej), jedna data obliczeń, «Instrukcja obsługi» oraz porównanie z 181 767 prawdziwymi spotami na 50 MHz z HamSCI Meteor Scatter QSO Party z sierpnia 2025: godziny i strony trasy są wiarygodne, procenty są orientacyjne.

- YA-MS używa języka i schematu kolorów wybranych u góry E-LAYER ANALYZER.
- Ustawienia YA-MS są zapisywane w `data/config.ini`.
- Każda nowa wersja YA-MS jest włączana za pomocą skryptu `tools/sync_yams.py`.

Szczegóły w [repozytorium YA-MS](https://github.com/ic8tem/YA-MS).
