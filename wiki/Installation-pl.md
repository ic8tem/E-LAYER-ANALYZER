# Instalacja

[← Strona główna](Home-pl)

Pobierz odpowiedni plik ze strony **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Program i folder `data` zawsze pozostają razem we własnym folderze: **żaden plik nigdy nie jest zapisywany w C:\Users\...\AppData**.

| System | Plik | Jak zainstalować |
|---|---|---|
| Windows 64 bity | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Kliknij dwukrotnie. Instaluje się w `C:\E-LAYER-ANALYZER`, bez uprawnień administratora. Brak skrótu w menu Start (znajdowałby się w AppData), tylko opcjonalna ikona na pulpicie. |
| Windows 32 bity | `...-win-x86-setup.exe` | Jak wyżej, dla 32-bitowego Windows. |
| Windows na ARM (komputery Snapdragon) | `...-win-arm64-setup.exe` | Jak wyżej, dla Windows na procesorach ARM. |
| Windows przenośny | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Rozpakuj w dowolnym miejscu i uruchom `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, a następnie uruchom z menu: przy pierwszym uruchomieniu program kopiuje się do `~/E-LAYER-ANALYZER`, z folderem `data` obok. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (także `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (lub `zypper install`), dalej jak dla .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (lub `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, dalej jak dla .deb. |
| Slackware (x86_64 i ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (lub `-aarch64-`) | Jako root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, potem uruchom `e-layer-analyzer` z menu lub z terminala: przy pierwszym uruchomieniu program kopiuje się do `~/E-LAYER-ANALYZER`. |
| Dowolny Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (lub `-aarch64.AppImage`) | Nadaj mu prawo wykonywania i uruchom: przy pierwszym uruchomieniu kopiuje program do `~/E-LAYER-ANALYZER` i uruchamia go stamtąd. |
| Linux przenośny | `...-linux-x64-portatile.tar.gz` | Rozpakuj i uruchom `./E-LAYER-ANALYZER` albo uruchom `./installa.sh`: instaluje w `~/E-LAYER-ANALYZER` z ikoną w menu. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, dalej jak wersja przenośna dla Linuksa. |
| Raspberry Pi (64 bity) | `e-layer-analyzer_x.y.z_arm64.deb` lub `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 lub 5 z **64-bitowym Raspberry Pi OS** i pulpitem (zalecane 4 GB RAM). Pakiet .deb jak wyżej; wersja przenośna jak dla Linuksa. |
| Raspberry Pi (32 bity) | `e-layer-analyzer_x.y.z_armhf.deb` lub `...-linux-arm-portatile.tar.gz` | Pi 2, 3 lub 4 z **32-bitowym Raspberry Pi OS**. Pakiet .deb jak wyżej; wersja przenośna: rozpakuj i uruchom `./E-LAYER-ANALYZER` lub `./installa.sh`. |
| Raspberry Pi ze Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` lub `...-linux-arm64-portatile.tar.gz` | Jak Slackware powyżej. Wersja przenośna jak dla Linuksa. |
| Linux armv7l (inne 32-bitowe płytki ARM) | `...-linux-armv7l-portatile.tar.gz` | Jak wersja przenośna dla Linuksa. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Eksperymentalny, niezweryfikowany.** Wymaga .NET 8 ze społeczności (`pkg install dotnet`) oraz natywnych bibliotek SkiaSharp i HarfBuzzSharp skompilowanych dla FreeBSD, których Avalonia nie dostarcza: bez nich program się nie uruchomi. Zobacz `LEGGIMI-FREEBSD.txt` w pakiecie. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) lub `...-osx-x64.dmg` (Intel) | Otwórz plik .dmg, kliknij prawym przyciskiem **Installa E-LAYER-ANALYZER.command** > Otwórz. Instaluje w `~/E-LAYER-ANALYZER`. |
| Mac, tylko aplikacja | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Rozpakuj **E-LAYER-ANALYZER.app** do własnego folderu (na przykład `~/E-LAYER-ANALYZER`, nie do Aplikacji): folder `data` zostanie utworzony obok aplikacji. Przy pierwszym uruchomieniu prawy przycisk > Otwórz. |

## Aktualizacja

Zainstaluj nową wersję na starej: ustawienia, zebrane spoty, logi i `call3.txt` zostają zachowane.

**Pakiety systemowe (.deb, .rpm, Arch, Slackware) nie aktualizują się automatycznie.** Z nimi program tylko zgłasza nową wersję: pobierz nowy pakiet ze strony Releases i zainstaluj go tak jak za pierwszym razem (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Automatyczne aktualizacje działają z instalatorem i plikami zip dla Windows, aplikacją na Maca, przenośnymi pakietami tar.gz i AppImage.

**Automatyczna aktualizacja (od wersji 0.4.5).** Przy starcie, a potem co 6 godzin program sprawdza wydania (Releases) na GitHubie. Gdy ukaże się nowa wersja, otwiera okno i pokazuje pomarańczowe **▲ x.y.z** obok wskaźnika NTP na górnym pasku:

**Od wersji 0.4.12 aktualizacje są automatyczne i ciche**: gdy na GitHubie jest nowa wersja, program ją pobiera, instaluje i sam uruchamia się ponownie, o nic nie pytając. Brakujące pliki również są pobierane ponownie bez pytania. Okno aktualizacji pojawia się tylko po kliknięciu wersji u góry albo jeśli wyłączysz automatyczne aktualizacje w zakładce USTAWIENIA (Ogólne).

- **Aktualizuj teraz** pobiera pakiet przenośny dla twojego komputera (Windows 64/32 bity, Linux, Raspberry Pi, Mac), zamyka program, zastępuje pliki i uruchamia go ponownie. Ustawienia, zebrane spoty, logi i `call3.txt` zostają zachowane.
- **Później** pyta ponownie przy następnym sprawdzeniu; **Pomiń tę wersję** przestaje proponować tę wersję.
- Kliknięcie wersji wyświetlanej na górnym pasku (na przykład `v0.4.5`) od razu sprawdza aktualizacje. Automatyczne sprawdzanie można wyłączyć polem wyboru w tym samym oknie.
- Jeśli do folderu programu nie można zapisywać (na przykład folder systemowy), użyj zamiast tego pakietu instalacyjnego ze strony wydania.

**Brakujące pliki.** Przy starcie program sprawdza, czy są wszystkie pliki, z którymi był rozpowszechniany (tłumaczenia, dokumenty, tabele, wymienione w `data/manifest.txt`). Jeśli któregoś brakuje, okno wyświetla ich listę, a **Pobierz z GitHub** pobiera je ponownie z tej samej wersji na GitHubie.

## Wymagania

- Nie trzeba instalować .NET: program zawiera wszystko, czego potrzebuje.
- Połączenie z internetem dla mapy (Natural Earth, przy pierwszym uruchomieniu), danych zorzowych NOAA, radarów YA-MS i sesji telnet.
- Raspberry Pi Zero i Pi 1 nie są obsługiwane (ich procesory nie są obsługiwane przez .NET).
- Nie ma wersji dla Linux i686 (32-bitowe komputery PC) ani OpenBSD, ponieważ .NET nie istnieje dla tych systemów. FreeBSD ma tylko pakiet eksperymentalny.
