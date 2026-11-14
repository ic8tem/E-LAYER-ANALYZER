# Telepítés

[← Kezdőlap](Home-hu)

Töltse le a megfelelő fájlt a **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** oldalról. A program és a `data` mappa mindig együtt marad egy saját mappában: **soha egyetlen fájl sem kerül a C:\Users\...\AppData mappába**.

| Rendszer | Fájl | Telepítés módja |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dupla kattintás. A `C:\E-LAYER-ANALYZER` mappába települ, rendszergazdai jog nélkül. Nincs parancsikon a Start menüben (az AppData-ban lenne), csak az opcionális asztali ikon. |
| Windows 32 bit | `...-win-x86-setup.exe` | Mint fent, 32 bites Windowsra. |
| Windows ARM-on (Snapdragon PC-k) | `...-win-arm64-setup.exe` | Mint fent, ARM processzoros Windowsra. |
| Windows hordozható | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Csomagolja ki bárhová, és indítsa el az `E-LAYER-ANALYZER.exe` fájlt. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, majd indítsa el a menüből: első indításkor a program átmásolja magát a `~/E-LAYER-ANALYZER` mappába, mellette a `data` mappával. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (van `.aarch64.rpm`, `.armv7hl.rpm` is) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (vagy `zypper install`), majd mint a .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (vagy `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, majd mint a .deb. |
| Slackware (x86_64 és ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (vagy `-aarch64-`) | Rootként: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, majd indítsa el az `e-layer-analyzer` programot a menüből vagy a terminálból: első indításkor a program a `~/E-LAYER-ANALYZER` mappába másolja magát. |
| Bármely Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (vagy `-aarch64.AppImage`) | Tegye futtathatóvá, és indítsa el: első indításkor a programot a `~/E-LAYER-ANALYZER` mappába másolja, és onnan indítja. |
| Linux hordozható | `...-linux-x64-portatile.tar.gz` | Csomagolja ki, és indítsa el a `./E-LAYER-ANALYZER` fájlt, vagy futtassa a `./installa.sh` szkriptet: a `~/E-LAYER-ANALYZER` mappába telepít, ikonnal a menüben. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, majd mint a Linuxos hordozható változat. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` vagy `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 vagy 5 **64 bites Raspberry Pi OS**-sel és asztali környezettel (4 GB RAM ajánlott). A .deb mint fent; a hordozható mint Linuxon. |
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` vagy `...-linux-arm-portatile.tar.gz` | Pi 2, 3 vagy 4 **32 bites Raspberry Pi OS**-sel. A .deb mint fent; a hordozható: csomagolja ki, és indítsa el a `./E-LAYER-ANALYZER` fájlt vagy a `./installa.sh` szkriptet. |
| Raspberry Pi Slackware ARM64-gyel | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` vagy `...-linux-arm64-portatile.tar.gz` | Mint a fenti Slackware. A hordozható mint Linuxon. |
| Linux armv7l (más 32 bites ARM-kártyák) | `...-linux-armv7l-portatile.tar.gz` | Mint a Linuxos hordozható változat. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Kísérleti, nem ellenőrzött.** Szükséges a közösségi .NET 8 (`pkg install dotnet`), valamint a FreeBSD-re fordított SkiaSharp és HarfBuzzSharp natív könyvtárak, amelyeket az Avalonia nem ad: nélkülük a program nem indul. Lásd a csomagban a `LEGGIMI-FREEBSD.txt` fájlt. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) vagy `...-osx-x64.dmg` (Intel) | Nyissa meg a .dmg fájlt, jobb kattintás az **Installa E-LAYER-ANALYZER.command** fájlra > Megnyitás. A `~/E-LAYER-ANALYZER` mappába telepít. |
| Mac, csak az alkalmazás | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Csomagolja ki az **E-LAYER-ANALYZER.app** alkalmazást egy saját mappába (például `~/E-LAYER-ANALYZER`, ne az Alkalmazások mappába): a `data` mappa az alkalmazás mellett jön létre. Első indításkor jobb kattintás > Megnyitás. |

## Frissítés

Telepítse az új verziót a régire: a beállítások, a gyűjtött spotok, a naplók és a `call3.txt` megmaradnak.

**A rendszercsomagok (.deb, .rpm, Arch, Slackware) nem frissülnek automatikusan.** Velük a program csak jelzi az új verziót: töltse le az új csomagot a Releases oldalról, és telepítse úgy, mint először (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Az automatikus frissítés a Windows telepítővel és zipekkel, a Mac alkalmazással, a hordozható tar.gz csomagokkal és az AppImage-dzsel működik.

**Automatikus frissítés (a 0.4.5-ös verziótól).** Indításkor, majd 30 percenként a program ellenőrzi a GitHub Releases oldalát. Ha új verzió jelent meg, megnyit egy ablakot, és a felső sávban az NTP-jelző mellett narancssárga **▲ x.y.z** feliratot mutat:

**A 0.4.12-es verziótól a frissítések automatikusak és csendesek**: ha a GitHubon új verzió van, a program letölti, telepíti és magától újraindul, semmit sem kérdezve. A hiányzó fájlokat is kérdés nélkül újratölti. A frissítési ablak csak a fenti verzióra kattintva jelenik meg, vagy ha a BEÁLLÍTÁSOK fülön (Általános) kikapcsolja az automatikus frissítést.

- A **Frissítés most** letölti a számítógépéhez való hordozható csomagot (Windows 64/32 bit, Linux, Raspberry Pi, Mac), bezárja a programot, lecseréli a fájlokat és újraindítja. A beállítások, a gyűjtött spotok, a naplók és a `call3.txt` megmaradnak.
- A **Később** a következő ellenőrzéskor újra rákérdez; a **Verzió kihagyása** után az adott verziót a program többé nem ajánlja fel.
- A felső sávban látható verzióra (például `v0.4.5`) kattintva a program azonnal frissítést keres. Az automatikus ellenőrzés ugyanabban az ablakban egy jelölőnégyzettel kikapcsolható.
- Ha a program mappája nem írható (például rendszermappa), használja helyette a kiadás oldalán található telepítőcsomagot.

**Hiányzó fájlok.** Indításkor a program ellenőrzi, hogy megvan-e minden fájl, amellyel terjesztették (fordítások, dokumentumok, táblázatok, a `data/manifest.txt` felsorolása szerint). Ha valamelyik hiányzik, egy ablak felsorolja őket, és a **Letöltés a GitHubról** újra letölti őket ugyanabból a verzióból a GitHubról.

## Követelmények

- Nem kell .NET-et telepíteni: a program mindent tartalmaz, amire szüksége van.
- Internetkapcsolat a térképhez (Natural Earth, első indításkor), a NOAA aurora-adataihoz, a YA-MS radarjaihoz és a telnet-kapcsolatokhoz.
- A Raspberry Pi Zero és a Pi 1 nem támogatott (processzorukat a .NET nem kezeli).
- Linux i686-ra (32 bites PC-k) és OpenBSD-re nincs változat, mert ezekre a rendszerekre nincs .NET. FreeBSD-re csak a kísérleti csomag van.
