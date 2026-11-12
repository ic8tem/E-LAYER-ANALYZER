# Installation

[← Home](Home-da)

Hent den rigtige fil fra siden **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programmet og mappen `data` hører altid sammen i en mappe, du selv vælger: **der skrives aldrig nogen fil til C:\Users\...\AppData**.

| System | Fil | Sådan installeres det |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dobbeltklik. Installeres i `C:\E-LAYER-ANALYZER`, uden administratorrettigheder. Ingen genvej i menuen Start (den ville ligge i AppData), kun det valgfrie ikon på skrivebordet. |
| Windows 32 bit | `...-win-x86-setup.exe` | Som ovenfor, til 32 bit-Windows. |
| Windows portabel | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Pak ud hvor som helst, og kør `E-LAYER-ANALYZER.exe`. |
| Linux 64 bit (pc) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, og start det derefter fra menuen: ved første opstart kopierer programmet sig selv til `~/E-LAYER-ANALYZER`, med mappen `data` ved siden af. |
| Linux portabel | `...-linux-x64-portatile.tar.gz` | Pak ud, og kør `./E-LAYER-ANALYZER`, eller kør `./installa.sh`: installeres i `~/E-LAYER-ANALYZER` med et ikon i menuen. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` eller `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 eller 5 med **64 bit Raspberry Pi OS** og skrivebord (4 GB RAM anbefales). .deb-filen som ovenfor; den portable version som til Linux. |
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` eller `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eller 4 med **32 bit Raspberry Pi OS**. .deb-filen som ovenfor; den portable version: pak ud, og kør `./E-LAYER-ANALYZER` eller `./installa.sh`. |
| Raspberry Pi med Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` eller `...-linux-arm64-portatile.tar.gz` | Som root: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, og start derefter `e-layer-analyzer` fra menuen eller terminalen: ved første start kopierer programmet sig til `~/E-LAYER-ANALYZER`. Den portable version som til Linux. |
| Linux armv7l (andre 32 bit ARM-boards) | `...-linux-armv7l-portatile.tar.gz` | Som den portable version til Linux. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eller `...-osx-x64.dmg` (Intel) | Åbn .dmg-filen, højreklik på **Installa E-LAYER-ANALYZER.command** > Åbn. Installeres i `~/E-LAYER-ANALYZER`. |
| Mac, kun programmet | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Pak **E-LAYER-ANALYZER.app** ud i en mappe, du selv vælger (for eksempel `~/E-LAYER-ANALYZER`, ikke Applications): mappen `data` oprettes ved siden af programmet. Ved første opstart: højreklik > Åbn. |

## Opdatering

Installér den nye version oven i den gamle: indstillinger, indsamlede spots, logs og `call3.txt` bevares.

**Automatisk opdatering (fra version 0.4.5).** Ved opstart og derefter hver 6. time kontrollerer programmet GitHub Releases. Når der er kommet en ny version, åbner det et vindue og viser et orange **▲ x.y.z** ved siden af NTP-indikatoren i linjen øverst:

- **Opdatér nu** henter den portable pakke til din computer (Windows 64/32 bit, Linux, Raspberry Pi, Mac), lukker programmet, erstatter filerne og genstarter det. Indstillinger, indsamlede spots, logs og `call3.txt` bevares.
- **Senere** spørger igen ved næste kontrol; **Spring versionen over** holder op med at foreslå den version.
- Et klik på versionen, der vises i linjen øverst (for eksempel `v0.4.5`), søger straks efter opdateringer. Den automatiske kontrol kan slås fra med afkrydsningsfeltet i samme vindue.
- Hvis programmappen ikke er skrivbar (for eksempel en systemmappe), så brug i stedet installationspakken fra release-siden.

**Manglende filer.** Ved opstart kontrollerer programmet, at alle de filer, det blev distribueret med, er til stede (oversættelser, dokumenter, tabeller, opført i `data/manifest.txt`). Mangler der nogen, viser et vindue en liste over dem, og **Hent fra GitHub** henter dem igen fra den samme version på GitHub.

## Krav

- Ingen .NET at installere: programmet indeholder alt, hvad det har brug for.
- Internetforbindelse til kortet (Natural Earth, ved første opstart), NOAA's nordlysdata, YA-MS' radarer og telnet-sessionerne.
- Raspberry Pi Zero og Pi 1 understøttes ikke (deres processorer håndteres ikke af .NET).
- Der findes ingen versioner til Linux i686 (32 bit-pc'er), fordi .NET ikke findes til det system, og heller ikke til FreeBSD, hvor Avalonias grafikbiblioteker mangler.
