# Installasjon

[← Home](Home-nb)

Last ned riktig fil fra siden **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programmet og mappen `data` hører alltid sammen i en mappe du velger selv: **ingen fil skrives noen gang til C:\Users\...\AppData**.

| System | Fil | Slik installerer du |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Dobbeltklikk. Installeres i `C:\E-LAYER-ANALYZER`, uten administratorrettigheter. Ingen snarvei i Start-menyen (den ville ligget i AppData), bare det valgfrie ikonet på skrivebordet. |
| Windows 32 bit | `...-windows-x86-setup.exe` | Som ovenfor, for 32-bits Windows. |
| Windows portabel | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Pakk ut hvor som helst og kjør `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Pakk ut og kjør `./installa.sh`: installeres i `~/E-LAYER-ANALYZER` med et ikon i menyen. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 eller 5 med **64-bits Raspberry Pi OS** og skrivebord (4 GB RAM anbefales). Pakk ut og kjør `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) eller `...-mac-intel.dmg` | Åpne .dmg-filen, høyreklikk på **Installa E-LAYER-ANALYZER.command** > Åpne. Installeres i `~/E-LAYER-ANALYZER`. |

## Oppdatering

Installer den nye versjonen oppå den gamle: innstillinger, innsamlede spot, logger og `call3.txt` beholdes.

## Krav

- Ingen .NET å installere: programmet inneholder alt det trenger.
- Internettforbindelse for kartet (Natural Earth, ved første oppstart), nordlysdata fra NOAA, YA-MS-radarene og telnet-øktene.
- Raspberry Pi Zero og Pi 1 støttes ikke (prosessorene deres håndteres ikke av .NET).
