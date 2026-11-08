# Installation

[← Home](Home-de)

Laden Sie die passende Datei von der Seite **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** herunter. Das Programm und der Ordner `data` bleiben immer zusammen in einem Ordner Ihrer Wahl: **Es wird nie eine Datei nach C:\Users\...\AppData geschrieben**.

| System | Datei | Installation |
|---|---|---|
| Windows 64 Bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Doppelklick. Installiert nach `C:\E-LAYER-ANALYZER`, ohne Administratorrechte. Keine Verknüpfung im Startmenü (sie läge in AppData), nur das optionale Desktop-Symbol. |
| Windows 32 Bit | `...-windows-x86-setup.exe` | Wie oben, für 32-Bit-Windows. |
| Windows portabel | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | An beliebiger Stelle entpacken und `E-LAYER-ANALYZER.exe` starten. |
| Linux (PC) | `...-linux-x64.tar.gz` | Entpacken und `./installa.sh` ausführen: installiert nach `~/E-LAYER-ANALYZER` mit einem Symbol im Menü. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 oder 5 mit **64-Bit-Raspberry-Pi-OS** und Desktop (4 GB RAM empfohlen). Entpacken und `./installa.sh` ausführen. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) oder `...-mac-intel.dmg` | Die .dmg öffnen, Rechtsklick auf **Installa E-LAYER-ANALYZER.command** > Öffnen. Installiert nach `~/E-LAYER-ANALYZER`. |

## Aktualisieren

Installieren Sie die neue Version über die alte: Einstellungen, gesammelte Spots, Logs und `call3.txt` bleiben erhalten.

## Voraussetzungen

- Kein .NET zu installieren: Das Programm enthält alles, was es braucht.
- Internetverbindung für die Karte (Natural Earth, beim ersten Start), die NOAA-Auroradaten, die YA-MS-Radare und die Telnet-Sitzungen.
- Raspberry Pi Zero und Pi 1 werden nicht unterstützt (ihre Prozessoren werden von .NET nicht unterstützt).
