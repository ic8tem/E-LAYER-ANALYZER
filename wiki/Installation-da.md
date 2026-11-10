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
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eller 4 med **32 bit Raspberry Pi OS**. Pak ud, og kør `./E-LAYER-ANALYZER` eller `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eller `...-osx-x64.dmg` (Intel) | Åbn .dmg-filen, højreklik på **Installa E-LAYER-ANALYZER.command** > Åbn. Installeres i `~/E-LAYER-ANALYZER`. |
| Mac, kun programmet | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Pak **E-LAYER-ANALYZER.app** ud i en mappe, du selv vælger (for eksempel `~/E-LAYER-ANALYZER`, ikke Applications): mappen `data` oprettes ved siden af programmet. Ved første opstart: højreklik > Åbn. |

## Opdatering

Installér den nye version oven i den gamle: indstillinger, indsamlede spots, logs og `call3.txt` bevares.

## Krav

- Ingen .NET at installere: programmet indeholder alt, hvad det har brug for.
- Internetforbindelse til kortet (Natural Earth, ved første opstart), NOAA's nordlysdata, YA-MS' radarer og telnet-sessionerne.
- Raspberry Pi Zero og Pi 1 understøttes ikke (deres processorer håndteres ikke af .NET).
