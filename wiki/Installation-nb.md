# Installasjon

[← Home](Home-nb)

Last ned riktig fil fra siden **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programmet og mappen `data` hører alltid sammen i en mappe du velger selv: **ingen fil skrives noen gang til C:\Users\...\AppData**.

| System | Fil | Slik installerer du |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dobbeltklikk. Installeres i `C:\E-LAYER-ANALYZER`, uten administratorrettigheter. Ingen snarvei i Start-menyen (den ville ligget i AppData), bare det valgfrie ikonet på skrivebordet. |
| Windows 32 bit | `...-win-x86-setup.exe` | Som ovenfor, for 32-bits Windows. |
| Windows på ARM (Snapdragon-PC-er) | `...-win-arm64-setup.exe` | Som ovenfor, for Windows på ARM-prosessorer. |
| Windows portabel | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Pakk ut hvor som helst og kjør `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, og start det deretter fra menyen: ved første oppstart kopierer programmet seg selv til `~/E-LAYER-ANALYZER`, med mappen `data` ved siden av. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (også `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (eller `zypper install`), deretter som .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (eller `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, deretter som .deb. |
| Slackware (x86_64 og ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (eller `-aarch64-`) | Som root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, og start deretter `e-layer-analyzer` fra menyen eller terminalen: ved første oppstart kopierer programmet seg selv til `~/E-LAYER-ANALYZER`. |
| Alle Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (eller `-aarch64.AppImage`) | Gjør den kjørbar og kjør den: ved første oppstart kopierer den programmet til `~/E-LAYER-ANALYZER` og kjører det derfra. |
| Linux portabel | `...-linux-x64-portatile.tar.gz` | Pakk ut og kjør `./E-LAYER-ANALYZER`, eller kjør `./installa.sh`: installeres i `~/E-LAYER-ANALYZER` med et ikon i menyen. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, deretter som den portable Linux-versjonen. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` eller `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 eller 5 med **64-bits Raspberry Pi OS** og skrivebord (4 GB RAM anbefales). .deb-filen som ovenfor; den portable versjonen som for Linux. |
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` eller `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eller 4 med **32-bits Raspberry Pi OS**. .deb-filen som ovenfor; den portable versjonen: pakk ut og kjør `./E-LAYER-ANALYZER` eller `./installa.sh`. |
| Raspberry Pi med Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` eller `...-linux-arm64-portatile.tar.gz` | Som Slackware ovenfor. Den portable versjonen som for Linux. |
| Linux armv7l (andre 32-bits ARM-kort) | `...-linux-armv7l-portatile.tar.gz` | Som den portable versjonen for Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Eksperimentell, ikke verifisert.** Krever .NET 8 fra fellesskapet (`pkg install dotnet`) og de native bibliotekene SkiaSharp og HarfBuzzSharp bygget for FreeBSD, som Avalonia ikke leverer: uten dem starter ikke programmet. Se `LEGGIMI-FREEBSD.txt` i pakken. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eller `...-osx-x64.dmg` (Intel) | Åpne .dmg-filen, høyreklikk på **Installa E-LAYER-ANALYZER.command** > Åpne. Installeres i `~/E-LAYER-ANALYZER`. |
| Mac, bare programmet | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Pakk ut **E-LAYER-ANALYZER.app** i en mappe du velger selv (for eksempel `~/E-LAYER-ANALYZER`, ikke Applications): mappen `data` opprettes ved siden av programmet. Ved første oppstart: høyreklikk > Åpne. |

## Oppdatering

Installer den nye versjonen oppå den gamle: innstillinger, innsamlede spot, logger og `call3.txt` beholdes.

**Systempakker (.deb, .rpm, Arch, Slackware) oppdateres ikke automatisk.** Med dem varsler programmet bare om den nye versjonen: last ned den nye pakken fra siden Releases og installer den som første gang (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Automatisk oppdatering fungerer med installasjonsprogrammet og zip-filene for Windows, Mac-appen, de portable tar.gz-pakkene og AppImage.

**Automatisk oppdatering (fra versjon 0.4.5).** Ved oppstart og deretter hvert 30. minutt sjekker programmet GitHub Releases. Når en ny versjon er kommet, åpner det et vindu og viser en oransje **▲ x.y.z** ved siden av NTP-indikatoren i linjen øverst:

**Fra versjon 0.4.12 er oppdateringene automatiske og stille**: når det finnes en ny versjon på GitHub, laster programmet den ned, installerer den og starter på nytt av seg selv uten å spørre om noe. Manglende filer lastes også ned på nytt uten å spørre. Oppdateringsvinduet vises bare med et klikk på versjonen øverst, eller hvis du slår av automatisk oppdatering i INNSTILLINGER (Generelt).

- **Oppdater nå** laster ned den portable pakken for datamaskinen din (Windows 64/32 bit, Linux, Raspberry Pi, Mac), lukker programmet, erstatter filene og starter det på nytt. Innstillinger, innsamlede spot, logger og `call3.txt` beholdes.
- **Senere** spør igjen ved neste sjekk; **Hopp over versjonen** slutter å foreslå den versjonen.
- Et klikk på versjonen som vises i linjen øverst (for eksempel `v0.4.5`), ser straks etter oppdateringer. Den automatiske sjekken kan slås av med avkrysningsboksen i samme vindu.
- Hvis programmappen ikke er skrivbar (for eksempel en systemmappe), bruk heller installasjonspakken fra release-siden.

**Manglende filer.** Ved oppstart sjekker programmet at alle filene det ble distribuert med, finnes (oversettelser, dokumenter, tabeller, oppført i `data/manifest.txt`). Mangler noen, lister et vindu dem opp, og **Last ned fra GitHub** henter dem på nytt fra den samme versjonen på GitHub.

## Krav

- Ingen .NET å installere: programmet inneholder alt det trenger.
- Internettforbindelse for kartet (Natural Earth, ved første oppstart), nordlysdata fra NOAA, YA-MS-radarene og telnet-øktene.
- Raspberry Pi Zero og Pi 1 støttes ikke (prosessorene deres håndteres ikke av .NET).
- Det finnes ingen versjoner for Linux i686 (32-bits PC-er) og OpenBSD, fordi .NET ikke finnes for disse systemene. For FreeBSD finnes bare den eksperimentelle pakken.
