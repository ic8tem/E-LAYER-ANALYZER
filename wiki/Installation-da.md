# Installation

[← Home](Home-da)

Hent den rigtige fil fra siden **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programmet og mappen `data` hører altid sammen i en mappe, du selv vælger: **der skrives aldrig nogen fil til C:\Users\...\AppData**.

| System | Fil | Sådan installeres det |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Dobbeltklik. Installeres i `C:\E-LAYER-ANALYZER`, uden administratorrettigheder. Ingen genvej i menuen Start (den ville ligge i AppData), kun det valgfrie ikon på skrivebordet. |
| Windows 32 bit | `...-windows-x86-setup.exe` | Som ovenfor, til 32 bit-Windows. |
| Windows portabel | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Pak ud hvor som helst, og kør `E-LAYER-ANALYZER.exe`. |
| Linux (pc) | `...-linux-x64.tar.gz` | Pak ud, og kør `./installa.sh`: installeres i `~/E-LAYER-ANALYZER` med et ikon i menuen. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 eller 5 med **64 bit Raspberry Pi OS** og skrivebord (4 GB RAM anbefales). Pak ud, og kør `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) eller `...-mac-intel.dmg` | Åbn .dmg-filen, højreklik på **Installa E-LAYER-ANALYZER.command** > Åbn. Installeres i `~/E-LAYER-ANALYZER`. |

## Opdatering

Installér den nye version oven i den gamle: indstillinger, indsamlede spots, logs og `call3.txt` bevares.

## Krav

- Ingen .NET at installere: programmet indeholder alt, hvad det har brug for.
- Internetforbindelse til kortet (Natural Earth, ved første opstart), NOAA's nordlysdata, YA-MS' radarer og telnet-sessionerne.
- Raspberry Pi Zero og Pi 1 understøttes ikke (deres processorer håndteres ikke af .NET).
