# Installation

[← Home](Home-de)

Laden Sie die passende Datei von der Seite **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** herunter. Das Programm und der Ordner `data` bleiben immer zusammen in einem Ordner Ihrer Wahl: **Es wird nie eine Datei nach C:\Users\...\AppData geschrieben**.

| System | Datei | Installation |
|---|---|---|
| Windows 64 Bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Doppelklick. Installiert nach `C:\E-LAYER-ANALYZER`, ohne Administratorrechte. Keine Verknüpfung im Startmenü (sie läge in AppData), nur das optionale Desktop-Symbol. |
| Windows 32 Bit | `...-win-x86-setup.exe` | Wie oben, für 32-Bit-Windows. |
| Windows portabel | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | An beliebiger Stelle entpacken und `E-LAYER-ANALYZER.exe` starten. |
| Linux 64 Bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, dann aus dem Menü starten: Beim ersten Start kopiert sich das Programm nach `~/E-LAYER-ANALYZER`, mit dem Ordner `data` daneben. |
| Linux portabel | `...-linux-x64-portatile.tar.gz` | Entpacken und `./E-LAYER-ANALYZER` starten oder `./installa.sh` ausführen: installiert nach `~/E-LAYER-ANALYZER` mit einem Symbol im Menü. |
| Raspberry Pi (64 Bit) | `e-layer-analyzer_x.y.z_arm64.deb` oder `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 oder 5 mit **64-Bit-Raspberry-Pi-OS** und Desktop (4 GB RAM empfohlen). Die .deb wie oben; die portable Version wie bei Linux. |
| Raspberry Pi (32 Bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 oder 4 mit **32-Bit-Raspberry-Pi-OS**. Entpacken und `./E-LAYER-ANALYZER` oder `./installa.sh` ausführen. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) oder `...-osx-x64.dmg` (Intel) | Die .dmg öffnen, Rechtsklick auf **Installa E-LAYER-ANALYZER.command** > Öffnen. Installiert nach `~/E-LAYER-ANALYZER`. |
| Mac, nur Anwendung | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** in einen Ordner Ihrer Wahl entpacken (zum Beispiel `~/E-LAYER-ANALYZER`, nicht Applications): Der Ordner `data` wird neben der App angelegt. Beim ersten Start Rechtsklick > Öffnen. |

## Aktualisieren

Installieren Sie die neue Version über die alte: Einstellungen, gesammelte Spots, Logs und `call3.txt` bleiben erhalten.

## Voraussetzungen

- Kein .NET zu installieren: Das Programm enthält alles, was es braucht.
- Internetverbindung für die Karte (Natural Earth, beim ersten Start), die NOAA-Auroradaten, die YA-MS-Radare und die Telnet-Sitzungen.
- Raspberry Pi Zero und Pi 1 werden nicht unterstützt (ihre Prozessoren werden von .NET nicht unterstützt).
