# METEOR SCATTER

[← Home](Home-pl)

Ta zakładka zawiera pełny **YA-MS 1.6.0**, program meteor scatter IC8TEM: sferę niebieską z radiantami, strumień z radarów SKiYMET (Collm, Sodankylä), obliczanie efektywności rojów, eksperymentalną implementację prognoz, geometrię i tłumienie, GRAVES, mapę kwadratów, **Roje na żywo** (ZHR mierzony przez Global Meteor Network i radianty z radaru CMOR), **Mapę źródeł** (Collm, Sodankylä, GRAVES i CMOR względem stacji) oraz dokument **Efektywność - objaśnienie** ze wzorami.

YA-MS 1.5.0 dodaje zakładkę **Zbieranie danych** (spoty meteor scatter filtrowane według MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Wewnątrz E-LAYER ANALYZER nie otwiera ona własnych sesji telnet: otrzymuje wiersze z sesji DATA COLLECTION, dzięki czemu nie ma podwójnych logowań tym samym znakiem.

YA-MS 1.6.0 porządkuje zakładki w grupy **Dane na żywo i w czasie rzeczywistym** (ruch meteor scatter na żywo z obłokiem każdej łączności, strumień z radarów, GRAVES, zbieranie danych) oraz **Obliczanie prognozy** (prognoza, geometria, obliczanie efektywności, implementacja prognoz).

- YA-MS używa języka i schematu kolorów wybranych na górze E-LAYER ANALYZER.
- Ustawienia YA-MS są zapisywane w `data/config.ini`.
- Każda nowa wersja YA-MS jest włączana skryptem `tools/sync_yams.py`.

Szczegóły w [repozytorium YA-MS](https://github.com/ic8tem/YA-MS).
