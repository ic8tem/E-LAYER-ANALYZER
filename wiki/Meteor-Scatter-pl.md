# METEOR SCATTER

[← Strona główna](Home-pl)

Ta zakładka zawiera pełny **YA-MS 1.6.0**, program meteor scatter autorstwa IC8TEM: sferę niebieską z radiantami, strumień z radarów SKiYMET (Collm, Sodankylä), obliczanie efektywności rojów, eksperymentalne prognozy, geometrię i tłumienie, GRAVES, mapę kwadratów, **Roje na żywo** (ZHR mierzone przez Global Meteor Network i radianty z radaru CMOR), **Mapę źródeł** (Collm, Sodankylä, GRAVES i CMOR względem stacji) oraz dokument **Efektywność - wyjaśnienie** ze wzorami.

YA-MS 1.5.0 dodaje zakładkę **Zbieranie danych** (spoty meteor scatter filtrowane według MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Wewnątrz E-LAYER ANALYZER nie otwiera własnych sesji telnet: otrzymuje wiersze z sesji DATA COLLECTION, więc nie ma podwójnych logowań tym samym znakiem wywoławczym.

YA-MS 1.6.0 porządkuje zakładki w grupy **Na żywo i dane w czasie rzeczywistym** (ruch meteor scatter na żywo z obłokiem każdej łączności, strumień radarowy, GRAVES, zbieranie danych) oraz **Obliczenie prognozy** (prognoza, geometria, obliczanie efektywności, eksperymentalne prognozy).

- YA-MS używa języka i schematu kolorów wybranych u góry E-LAYER ANALYZER.
- Ustawienia YA-MS są zapisywane w `data/config.ini`.
- Każda nowa wersja YA-MS jest włączana za pomocą skryptu `tools/sync_yams.py`.

Szczegóły w [repozytorium YA-MS](https://github.com/ic8tem/YA-MS).
