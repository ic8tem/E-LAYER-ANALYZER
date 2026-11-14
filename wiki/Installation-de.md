# Installation

[← Home](Home-de)

Laden Sie die passende Datei von der Seite **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** herunter. Das Programm und der Ordner `data` bleiben immer zusammen in einem Ordner Ihrer Wahl: **Es wird nie eine Datei nach C:\Users\...\AppData geschrieben**.

| System | Datei | Installation |
|---|---|---|
| Windows 64 Bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Doppelklick. Installiert nach `C:\E-LAYER-ANALYZER`, ohne Administratorrechte. Keine Verknüpfung im Startmenü (sie läge in AppData), nur das optionale Desktop-Symbol. |
| Windows 32 Bit | `...-win-x86-setup.exe` | Wie oben, für 32-Bit-Windows. |
| Windows auf ARM (Snapdragon-PCs) | `...-win-arm64-setup.exe` | Wie oben, für Windows auf ARM-Prozessoren. |
| Windows portabel | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | An beliebiger Stelle entpacken und `E-LAYER-ANALYZER.exe` starten. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, dann aus dem Menü starten: Beim ersten Start kopiert sich das Programm nach `~/E-LAYER-ANALYZER`, mit dem Ordner `data` daneben. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (auch `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (oder `zypper install`), dann wie bei der .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (oder `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, dann wie bei der .deb. |
| Slackware (x86_64 und ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (oder `-aarch64-`) | Als root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, dann `e-layer-analyzer` aus dem Menü oder dem Terminal starten: Beim ersten Start kopiert sich das Programm nach `~/E-LAYER-ANALYZER`. |
| Jedes Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (oder `-aarch64.AppImage`) | Ausführbar machen und starten: Beim ersten Start kopiert es das Programm nach `~/E-LAYER-ANALYZER` und startet es von dort. |
| Linux portabel | `...-linux-x64-portatile.tar.gz` | Entpacken und `./E-LAYER-ANALYZER` starten oder `./installa.sh` ausführen: installiert nach `~/E-LAYER-ANALYZER` mit einem Symbol im Menü. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, dann wie die portable Version für Linux. |
| Raspberry Pi (64 Bit) | `e-layer-analyzer_x.y.z_arm64.deb` oder `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 oder 5 mit **64-Bit-Raspberry-Pi-OS** und Desktop (4 GB RAM empfohlen). Die .deb wie oben; die portable Version wie bei Linux. |
| Raspberry Pi (32 Bit) | `e-layer-analyzer_x.y.z_armhf.deb` oder `...-linux-arm-portatile.tar.gz` | Pi 2, 3 oder 4 mit **32-Bit-Raspberry-Pi-OS**. Die .deb wie oben; die portable Version: entpacken und `./E-LAYER-ANALYZER` oder `./installa.sh` ausführen. |
| Raspberry Pi mit Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` oder `...-linux-arm64-portatile.tar.gz` | Wie Slackware oben. Die portable Version wie bei Linux. |
| Linux armv7l (andere 32-Bit-ARM-Boards) | `...-linux-armv7l-portatile.tar.gz` | Wie die portable Version für Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Experimentell, nicht geprüft.** Braucht das .NET 8 der Community (`pkg install dotnet`) und die für FreeBSD kompilierten nativen Bibliotheken SkiaSharp und HarfBuzzSharp, die Avalonia nicht liefert: Ohne sie startet das Programm nicht. Siehe `LEGGIMI-FREEBSD.txt` im Paket. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) oder `...-osx-x64.dmg` (Intel) | Die .dmg öffnen, Rechtsklick auf **Installa E-LAYER-ANALYZER.command** > Öffnen. Installiert nach `~/E-LAYER-ANALYZER`. |
| Mac, nur Anwendung | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** in einen Ordner Ihrer Wahl entpacken (zum Beispiel `~/E-LAYER-ANALYZER`, nicht Applications): Der Ordner `data` wird neben der App angelegt. Beim ersten Start Rechtsklick > Öffnen. |

## Aktualisieren

Installieren Sie die neue Version über die alte: Einstellungen, gesammelte Spots, Logs und `call3.txt` bleiben erhalten.

**Systempakete (.deb, .rpm, Arch, Slackware) aktualisieren sich nicht automatisch.** Mit ihnen meldet das Programm nur die neue Version: Laden Sie das neue Paket von der Release-Seite herunter und installieren Sie es wie beim ersten Mal (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Die automatische Aktualisierung funktioniert mit dem Windows-Installationsprogramm und den Zips, der Mac-App, den portablen tar.gz-Paketen und dem AppImage.

**Automatische Aktualisierung (ab Version 0.4.5).** Beim Start und danach alle 6 Stunden prüft das Programm die GitHub-Releases. Ist eine neue Version erschienen, öffnet es ein Fenster und zeigt in der oberen Leiste neben der NTP-Anzeige ein orangefarbenes **▲ x.y.z**:

**Ab Version 0.4.12 erfolgen Updates automatisch und still**: Liegt auf GitHub eine neue Version vor, lädt das Programm sie herunter, installiert sie und startet sich selbst neu, ohne etwas zu fragen. Auch fehlende Dateien werden ohne Rückfrage neu geladen. Das Update-Fenster erscheint nur mit einem Klick auf die Version oben oder wenn Sie die automatische Aktualisierung in EINSTELLUNGEN (Allgemein) ausschalten.

- **Jetzt aktualisieren** lädt das portable Paket für Ihren Computer herunter (Windows 64/32 Bit, Linux, Raspberry Pi, Mac), schließt das Programm, ersetzt die Dateien und startet es neu. Einstellungen, gesammelte Spots, Logs und `call3.txt` bleiben erhalten.
- **Später** fragt bei der nächsten Prüfung erneut; **Version überspringen** schlägt diese Version nicht mehr vor.
- Ein Klick auf die in der oberen Leiste angezeigte Version (zum Beispiel `v0.4.5`) sucht sofort nach Updates. Die automatische Prüfung lässt sich mit dem Kästchen im selben Fenster abschalten.
- Ist der Programmordner nicht beschreibbar (zum Beispiel ein Systemordner), verwenden Sie stattdessen das Installationspaket von der Release-Seite.

**Fehlende Dateien.** Beim Start prüft das Programm, ob alle Dateien vorhanden sind, mit denen es ausgeliefert wurde (Übersetzungen, Dokumente, Tabellen, aufgeführt in `data/manifest.txt`). Fehlt eine davon, listet ein Fenster die fehlenden auf, und **Von GitHub laden** holt sie erneut aus derselben Version auf GitHub.

## Voraussetzungen

- Kein .NET zu installieren: Das Programm enthält alles, was es braucht.
- Internetverbindung für die Karte (Natural Earth, beim ersten Start), die NOAA-Auroradaten, die YA-MS-Radare und die Telnet-Sitzungen.
- Raspberry Pi Zero und Pi 1 werden nicht unterstützt (ihre Prozessoren werden von .NET nicht unterstützt).
- Es gibt keine Versionen für Linux i686 (32-Bit-PCs) und OpenBSD, weil .NET für diese Systeme nicht existiert. Für FreeBSD gibt es nur das experimentelle Paket.
