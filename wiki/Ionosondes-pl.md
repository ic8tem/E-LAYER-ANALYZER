# JONOSONDY

[← Strona główna](Home-pl)

![JONOSONDY](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Podzakładka **SPORADIC-E** (od wersji 0.5.2): **foEs zmierzona** przez jonosondy sieci **GIRO** na całym świecie, obok tej, którą program wyznacza ze spotów.

## Dane

- 63 jonosondy GIRO (lista ULCAR), z ostatnią **foEs**, **foF2** i **MUF(D)** odczytanymi automatycznie z jonogramów.
- Dane przychodzą przy pierwszym otwarciu zakładki, a potem **co 15 minut**, automatycznie (pole *Aktualizuj automatycznie co 15 minut*); *Aktualizuj teraz* pobiera je od razu. Archiwum przechowuje ostatnie 48 godzin w folderze `giro` danych.
- Dane **GIRO** (Reinisch i Galkin 2011), licencja **CC BY-NC-SA 4.0**: tylko użytek niekomercyjny. Serwis jest ten sam co w *MUF Muncher* DM6AX.

## Mapa i tabela

- Każda jonosonda to kolorowa kropka z ostatnią foEs (te same kolory co obłoki Es) i wartością w MHz; **szara**, jeśli nie ma aktualnych danych, z **pierścieniem**, jeśli ostatni jonogram ma ponad 90 minut.
- Tabela: Kod, Jonosonda, km od twojej stacji, UTC ostatniego jonogramu, foEs, foF2, MUF(D) i **Spoty foEs≥**: minimalna foEs wyznaczona ze spotów Es z ostatnich 30 minut, które odbijają się w promieniu 500 km od jonosondy (regulowanym), z liczbą spotów.

## Wybrana jonosonda

Kliknij wiersz lub miejsce obok jonosondy na mapie:

- **wykres ostatnich 24 godzin**: foEs żółta, foF2 błękitna, MUF(D) fioletowa kropkowana;
- **MUF E-sporadycznego** na 1000, 1500, 2000 i 2400 km (MUF = foEs · sec φ, przy wybranej wysokości Es) z **pasmami, które się otwierają** (28, 40, 50, 70, 144 MHz);
- jeśli spoty wymagają więcej niż zmierzona foEs, program to sygnalizuje: obłok może być daleko od jonosondy albo jonosonda osiągnęła granicę swojego zakresu częstotliwości (Chartier 2022).
