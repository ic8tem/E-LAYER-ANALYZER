# DATA COLLECTION

[← Strona główna](Home-pl)

## Sesje telnet

| Sesja | Domyślnie | Uwagi |
|---|---|---|
| 1. DX cluster do wyboru | on0dxk.dyndns.org 8000 | Na ON0DXK spoty VHF są filtrowane: `set/filter dxbm/pass 6` (potem 4, 2). Inny przykład: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW i RTTY, 7001 = FT8. |
| 3. Klaster z PSKReporter | **dm4x.ddns.net 8500** | Klaster DM4X; alternatywnie HamAlert (hamalert.org 7300, wymagane konto). |

- Automatyczne logowanie znakiem wywoławczym (u góry lub w polu Login); **polecenia początkowe** oddzielone `;`.
- **Ponowne połączenie** po 30 s w razie zerwania łącza; **Połącz przy uruchomieniu**.
- Domyślnie zapisywane są tylko spoty od **28 MHz wzwyż**.

## Zebrane spoty

Ostatnie 2000 spotów, z filtrami pasma i źródła. Archiwum: jeden plik CSV na dzień w `data/spot`. Przy każdym zamknięciu programu baza spotów jest opróżniana (pamięć i pliki w `data/spot`): każda sesja zaczyna od zera.

**Filtr lokalny** (pole opcjonalne): wiersze "DX de" poniżej 27990,0 kHz lub powyżej 145 MHz są odrzucane, nie są wyświetlane w konsoli ani zapamiętywane. **Nieprecyzyjne lokatory** (mniej niż 6 znaków) pobierane są z CALL3: jeśli CALL3 ma lokator 6-znakowy, zawsze ma on pierwszeństwo. Tak jak w YA-MS, spotami meteor scatter (a nie E-sporadycznego) są spoty w typowych oknach częstotliwości MS (50190-50300, 70100-70400, 144290-144399 kHz) bez napisów innej propagacji (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) oraz spoty z emisjami wyłącznie MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) lub z `<MS>`. Spoty z `<AS>`/`<ACS>` (aircraft scatter) nie są używane dla E-sporadycznego.

## Kolumna filtrów (prawe 40%)

Zbieranie danych jest **jedno dla całego programu**: każdy spot przechodzi przez te filtry i trafia do zakładki odpowiedniego rodzaju propagacji (wewnątrz E-LAYER ANALYZER zakładka Zbieranie danych YA-MS nie jest wyświetlana). Na górze pole **filtra lokalnego**; poniżej pięć podzakładek:

| Podzakładka | Co ustawiasz | Wartości domyślne (reguły autora) |
|---|---|---|
| Meteor scatter | filtr YA-MS: okna częstotliwości MS, napisy innej propagacji, emisje MS i `<MS>`, pasma | jak w YA-MS |
| E-sporadyczne | napisy `< >` wykluczające spot, reguła 144 MHz, reguła transrównikowa | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, krótki skok 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | słowa F.A.I., pasma, okres na mapie | FAI, wszystkie pasma, 24 godziny |
| Zorza | napisy zorzowe `< >`, pasma, okres na mapie | `AU` `AUR` (z `< >` lub bez), wszystkie pasma, 2 godziny |
| Test filtrów | wklej wiersz z klastra i zobacz, jak klasyfikują go wszystkie filtry | |

Przycisk **Przywróć reguły autora** przywraca wartości domyślne.

## CALL3

Archiwum JTDX **znak wywoławczy → lokator** (ponad milion znaków) w `data/call3.txt`. Przyciski **Wczytaj ponownie** i **Importuj nowy CALL3.TXT**. Lokatory 6-znakowe odczytane z komentarzy spotów są dodawane do `data/call3_nuovi.txt`.

Od wersji 0.4.16 cały program ma jedną bazę CALL3, tę ze Zbierania danych YA-MS (zakładka METEOR SCATTER), z tymi samymi regułami co LIVETROPOVHF: aktualizacja z MMMonVHF, wpisy ręczne, dodane pliki, poprawki z klastra, nauczone lokatory i G7VRD co 30 minut; poprawki są zapisywane w data/call3.txt, a lokatory z data/call3_nuovi.txt importuje się raz. Spoty z WSJT-X, JTDX i MSHV przychodzą też przez UDP (źródło WSJT-X UDP, port 2237 z przekazywaniem na 2238).

Te filtry z YA-MS (CW, minimalna odległość, okna częstotliwości, tryby MS, pasma) dotyczą tylko zakładki METEOR SCATTER: decydują, które spoty trafiają do jej zbioru. Pozostałe zakładki dostają wszystkie spoty i używają własnych filtrów.

## Podzakładki Spoty i Dane propagacyjne

Zakładka ma dwie podzakładki: **Spoty** (sesje, zebrane spoty, CALL3 i kolumna filtrów) oraz **Dane propagacyjne**:

- **N0NBH** (hamqsl.com): baner VHF i dane słoneczne, E-skip Europa i Ameryka Północna (DXrobot), zorza.
- **Bz dla zorzy**: wiatr słoneczny w czasie rzeczywistym z NOAA SWPC (DSCOVR/ACE, punkt L1), najlepsze źródło Bz.
- **Meteor scatter**: dzienna aktywność meteorowa (RMOB) z Make More Miles on VHF.

Są pobierane najwyżej raz na godzinę. Podsumowanie pojawia się na mapach SPORADIC-E, F.A.I. i AURORA. **Menu kontynentu**, po prawej stronie paska zakładek, przesuwa mapy i wybiera dane tego kontynentu.
