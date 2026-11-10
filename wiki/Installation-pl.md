# Instalacja

[← Strona główna](Home-pl)

Pobierz odpowiedni plik ze strony **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Program i folder `data` zawsze pozostają razem we własnym folderze: **żaden plik nigdy nie jest zapisywany w C:\Users\...\AppData**.

| System | Plik | Jak zainstalować |
|---|---|---|
| Windows 64 bity | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Kliknij dwukrotnie. Instaluje się w `C:\E-LAYER-ANALYZER`, bez uprawnień administratora. Brak skrótu w menu Start (znajdowałby się w AppData), tylko opcjonalna ikona na pulpicie. |
| Windows 32 bity | `...-win-x86-setup.exe` | Jak wyżej, dla 32-bitowego Windows. |
| Windows przenośny | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Rozpakuj w dowolnym miejscu i uruchom `E-LAYER-ANALYZER.exe`. |
| Linux 64 bity (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, a następnie uruchom z menu: przy pierwszym uruchomieniu program kopiuje się do `~/E-LAYER-ANALYZER`, z folderem `data` obok. |
| Linux przenośny | `...-linux-x64-portatile.tar.gz` | Rozpakuj i uruchom `./E-LAYER-ANALYZER` albo uruchom `./installa.sh`: instaluje w `~/E-LAYER-ANALYZER` z ikoną w menu. |
| Raspberry Pi (64 bity) | `e-layer-analyzer_x.y.z_arm64.deb` lub `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 lub 5 z **64-bitowym Raspberry Pi OS** i pulpitem (zalecane 4 GB RAM). Pakiet .deb jak wyżej; wersja przenośna jak dla Linuksa. |
| Raspberry Pi (32 bity) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 lub 4 z **32-bitowym Raspberry Pi OS**. Rozpakuj i uruchom `./E-LAYER-ANALYZER` lub `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) lub `...-osx-x64.dmg` (Intel) | Otwórz plik .dmg, kliknij prawym przyciskiem **Installa E-LAYER-ANALYZER.command** > Otwórz. Instaluje w `~/E-LAYER-ANALYZER`. |
| Mac, tylko aplikacja | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Rozpakuj **E-LAYER-ANALYZER.app** do własnego folderu (na przykład `~/E-LAYER-ANALYZER`, nie do Aplikacji): folder `data` zostanie utworzony obok aplikacji. Przy pierwszym uruchomieniu prawy przycisk > Otwórz. |

## Aktualizacja

Zainstaluj nową wersję na starej: ustawienia, zebrane spoty, logi i `call3.txt` zostają zachowane.

## Wymagania

- Nie trzeba instalować .NET: program zawiera wszystko, czego potrzebuje.
- Połączenie z internetem dla mapy (Natural Earth, przy pierwszym uruchomieniu), danych zorzowych NOAA, radarów YA-MS i sesji telnet.
- Raspberry Pi Zero i Pi 1 nie są obsługiwane (ich procesory nie są obsługiwane przez .NET).
