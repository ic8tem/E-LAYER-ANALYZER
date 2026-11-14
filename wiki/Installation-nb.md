# Installasjon

[← Home](Home-nb)

Last ned riktig fil fra siden **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programmet og mappen `data` hører alltid sammen i en mappe du velger selv: **ingen fil skrives noen gang til C:\Users\...\AppData**.

| System | Fil | Slik installerer du |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dobbeltklikk. Installeres i `C:\E-LAYER-ANALYZER`, uten administratorrettigheter. Ingen snarvei i Start-menyen (den ville ligget i AppData), bare det valgfrie ikonet på skrivebordet. |
| Windows 32 bit | `...-win-x86-setup.exe` | Som ovenfor, for 32-bits Windows. |
| Windows portabel | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Pakk ut hvor som helst og kjør `E-LAYER-ANALYZER.exe`. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, og start det deretter fra menyen: ved første oppstart kopierer programmet seg selv til `~/E-LAYER-ANALYZER`, med mappen `data` ved siden av. |
| Linux portabel | `...-linux-x64-portatile.tar.gz` | Pakk ut og kjør `./E-LAYER-ANALYZER`, eller kjør `./installa.sh`: installeres i `~/E-LAYER-ANALYZER` med et ikon i menyen. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` eller `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 eller 5 med **64-bits Raspberry Pi OS** og skrivebord (4 GB RAM anbefales). .deb-filen som ovenfor; den portable versjonen som for Linux. |
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` eller `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eller 4 med **32-bits Raspberry Pi OS**. .deb-filen som ovenfor; den portable versjonen: pakk ut og kjør `./E-LAYER-ANALYZER` eller `./installa.sh`. |
| Raspberry Pi med Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` eller `...-linux-arm64-portatile.tar.gz` | Som root: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, og start deretter `e-layer-analyzer` fra menyen eller terminalen: ved første oppstart kopierer programmet seg til `~/E-LAYER-ANALYZER`. Den portable versjonen som for Linux. |
| Linux armv7l (andre 32-bits ARM-kort) | `...-linux-armv7l-portatile.tar.gz` | Som den portable versjonen for Linux. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eller `...-osx-x64.dmg` (Intel) | Åpne .dmg-filen, høyreklikk på **Installa E-LAYER-ANALYZER.command** > Åpne. Installeres i `~/E-LAYER-ANALYZER`. |
| Mac, bare programmet | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Pakk ut **E-LAYER-ANALYZER.app** i en mappe du velger selv (for eksempel `~/E-LAYER-ANALYZER`, ikke Applications): mappen `data` opprettes ved siden av programmet. Ved første oppstart: høyreklikk > Åpne. |

## Oppdatering

Installer den nye versjonen oppå den gamle: innstillinger, innsamlede spot, logger og `call3.txt` beholdes.

**Automatisk oppdatering (fra versjon 0.4.5).** Ved oppstart og deretter hvert 30. minutt sjekker programmet GitHub Releases. Når en ny versjon er kommet, åpner det et vindu og viser en oransje **▲ x.y.z** ved siden av NTP-indikatoren i linjen øverst:

**Fra versjon 0.4.12 er oppdateringene automatiske og stille**: når det finnes en ny versjon på GitHub, laster programmet den ned, installerer den og starter på nytt av seg selv uten å spørre om noe. Manglende filer lastes også ned på nytt uten å spørre. Oppdateringsvinduet vises bare med et klikk på versjonen øverst, eller hvis du skriver agg_automatici=0 i filen data/ela.ini.

- **Oppdater nå** laster ned den portable pakken for datamaskinen din (Windows 64/32 bit, Linux, Raspberry Pi, Mac), lukker programmet, erstatter filene og starter det på nytt. Innstillinger, innsamlede spot, logger og `call3.txt` beholdes.
- **Senere** spør igjen ved neste sjekk; **Hopp over versjonen** slutter å foreslå den versjonen.
- Et klikk på versjonen som vises i linjen øverst (for eksempel `v0.4.5`), ser straks etter oppdateringer. Den automatiske sjekken kan slås av med avkrysningsboksen i samme vindu.
- Hvis programmappen ikke er skrivbar (for eksempel en systemmappe), bruk heller installasjonspakken fra release-siden.

**Manglende filer.** Ved oppstart sjekker programmet at alle filene det ble distribuert med, finnes (oversettelser, dokumenter, tabeller, oppført i `data/manifest.txt`). Mangler noen, lister et vindu dem opp, og **Last ned fra GitHub** henter dem på nytt fra den samme versjonen på GitHub.

## Krav

- Ingen .NET å installere: programmet inneholder alt det trenger.
- Internettforbindelse for kartet (Natural Earth, ved første oppstart), nordlysdata fra NOAA, YA-MS-radarene og telnet-øktene.
- Raspberry Pi Zero og Pi 1 støttes ikke (prosessorene deres håndteres ikke av .NET).
- Det finnes ingen versjoner for Linux i686 (32-bits PC-er), fordi .NET ikke finnes for det systemet, og heller ikke for FreeBSD, der grafikkbibliotekene til Avalonia mangler.
