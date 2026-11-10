# DATA COLLECTION

[← Home](Home-pl)

## Sesje telnet

| Sesja | Domyślnie | Uwagi |
|---|---|---|
| 1. Dowolny DX cluster | on0dxk.dyndns.org 8000 | Na ON0DXK spoty VHF są filtrowane: `set/filter dxbm/pass 6` (potem 4, 2). Inny przykład: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW i RTTY, 7001 = FT8. |
| 3. Cluster z PSKReporter | **dm4x.ddns.net 8500** | Cluster DM4X; alternatywnie HamAlert (hamalert.org 7300, wymagane konto). |

- Automatyczne logowanie znakiem (na górze lub w polu Login); **polecenia początkowe** rozdzielone `;`.
- **Połącz ponownie** po 30 s, jeśli linia zostanie zerwana; **Połącz przy starcie**.
- Domyślnie zapisywane są tylko spoty **od 28 MHz w górę**.

## Zebrane spoty

Ostatnie 2000 spotów, z filtrami pasma i źródła. Archiwum: jeden plik CSV na dzień w `data/spot`. Przy każdym zamknięciu programu baza spotów jest opróżniana (pamięć i pliki w `data/spot`): każda sesja zaczyna od zera.

**Filtr lokalny** (opcjonalne pole): wiersze „DX de” poniżej 27990,0 kHz lub powyżej 145 MHz są odrzucane, nie są pokazywane w konsoli ani zapisywane. **Nieprecyzyjne lokatory** (mniej niż 6 znaków) są brane z CALL3: jeśli CALL3 ma lokator 6-znakowy, zawsze ma on pierwszeństwo. Tak jak w YA-MS, spoty meteor scatter (a nie sporadycznej warstwy E) to te w typowych oknach częstotliwości MS (50190-50300, 70100-70400, 144290-144399 kHz) bez znaczników innej propagacji (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) oraz te z emisjami wyłącznie MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) lub z `<MS>`. Spoty z `<AS>`/`<ACS>` (aircraft scatter) nie są używane dla sporadycznej warstwy E.

## Kolumna filtrów (prawe 40%)

Zbieranie danych jest **jedno dla całego programu**: każdy spot przechodzi przez te filtry i trafia do zakładki każdego rodzaju propagacji (wewnątrz E-LAYER ANALYZER zakładka Zbieranie danych z YA-MS nie jest pokazywana). Na górze pole **filtra lokalnego**; poniżej pięć podzakładek:

| Podzakładka | Co ustawiasz | Wartości domyślne (reguły autora) |
|---|---|---|
| Meteor scatter | filtr YA-MS: okna częstotliwości MS, znaczniki innej propagacji, emisje MS i `<MS>`, pasma | jak w YA-MS |
| Sporadyczna E | znaczniki `< >` wykluczające spot, reguła 144 MHz, reguła transrównikowa | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, krótki skok 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | słowa F.A.I., pasma, okres na mapie | FAI, wszystkie pasma, 24 godziny |
| Zorza | znaczniki `< >` zorzy, pasma, okres na mapie | `AU` `AUR` (z `< >` lub bez), wszystkie pasma, 2 godziny |
| Test filtrów | wklej wiersz z clustera i zobacz, jak klasyfikują go wszystkie filtry | |

Przycisk **Przywróć reguły autora** przywraca wartości domyślne.

## CALL3

Baza danych JTDX **znak → lokator** (ponad milion znaków) w `data/call3.txt`. Przyciski **Wczytaj ponownie** i **Importuj nowy CALL3.TXT**. 6-znakowe lokatory odczytane z komentarzy spotów są dopisywane do `data/call3_nuovi.txt`.

## Podzakładki Spot i Dane propagacyjne

Zakładka ma dwie podzakładki: **Spot** (sesje, zebrane spoty, CALL3 i kolumna filtrów) oraz **Dane propagacyjne**:

- **N0NBH** (hamqsl.com): baner VHF i dane słoneczne, E-skip Europa i Ameryka Północna (DXrobot), zorza.
- **Bz dla zorzy**: wiatr słoneczny w czasie rzeczywistym z NOAA SWPC (DSCOVR/ACE, punkt L1), najlepsze źródło Bz.
- **Meteor scatter**: dzienna aktywność meteorowa (RMOB) z Make More Miles on VHF.

Są pobierane najwyżej raz na godzinę. Podsumowanie pojawia się na mapach SPORADIC-E, F.A.I. i AURORA. **Menu kontynentów**, po prawej stronie paska zakładek, przesuwa mapy i wybiera dane danego kontynentu.
