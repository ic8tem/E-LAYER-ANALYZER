# Telepítés

[← Home](Home-hu)

Töltse le a megfelelő fájlt a **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** oldalról. A program és a `data` mappa mindig együtt marad egy saját mappában: **a C:\Users\...\AppData mappába soha semmi nem íródik**.

| Rendszer | Fájl | Hogyan telepítsük |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dupla kattintás. A `C:\E-LAYER-ANALYZER` mappába telepít, rendszergazdai jog nem kell. Nincs Start menü parancsikon (az AppData-ban lenne), csak az opcionális asztali ikon. |
| Windows 32 bit | `...-win-x86-setup.exe` | Mint fent, 32 bites Windowsra. |
| Windows hordozható | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Csomagolja ki bárhová, és indítsa el az `E-LAYER-ANALYZER.exe` fájlt. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, majd indítsa el a menüből: első indításkor a program átmásolja magát a `~/E-LAYER-ANALYZER` mappába, mellette a `data` mappával. |
| Linux hordozható | `...-linux-x64-portatile.tar.gz` | Csomagolja ki és indítsa el a `./E-LAYER-ANALYZER` fájlt, vagy futtassa a `./installa.sh` szkriptet: a `~/E-LAYER-ANALYZER` mappába telepít, ikonnal a menüben. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` vagy `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 vagy 5 **64 bites Raspberry Pi OS**-sel és asztali környezettel (4 GB RAM ajánlott). A .deb mint fent; a hordozható mint Linuxon. |
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 vagy 4 **32 bites Raspberry Pi OS**-sel. Csomagolja ki és indítsa el a `./E-LAYER-ANALYZER` vagy a `./installa.sh` fájlt. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) vagy `...-osx-x64.dmg` (Intel) | Nyissa meg a .dmg fájlt, jobb kattintás az **Installa E-LAYER-ANALYZER.command** fájlra > Megnyitás. A `~/E-LAYER-ANALYZER` mappába telepít. |
| Mac, csak alkalmazás | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Csomagolja ki az **E-LAYER-ANALYZER.app** alkalmazást egy saját mappába (például `~/E-LAYER-ANALYZER`, nem az Alkalmazások közé): a `data` mappa az alkalmazás mellett jön létre. Első indításkor jobb kattintás > Megnyitás. |

## Frissítés

Telepítse az új verziót a régire: a beállítások, az összegyűjtött spotok, a naplók és a `call3.txt` megmaradnak.

## Követelmények

- Nem kell .NET-et telepíteni: a program mindent tartalmaz, amire szüksége van.
- Internetkapcsolat a térképhez (Natural Earth, első indításkor), a NOAA aurora-adataihoz, a YA-MS radarjaihoz és a telnet-munkamenetekhez.
- A Raspberry Pi Zero és Pi 1 nem támogatott (processzoraikat a .NET nem kezeli).
