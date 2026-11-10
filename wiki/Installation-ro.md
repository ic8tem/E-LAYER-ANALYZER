# Instalare

[← Home](Home-ro)

Descărcați fișierul potrivit de pe pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programul și dosarul `data` stau mereu împreună într-un dosar al dumneavoastră: **nu se scrie niciodată nimic în C:\Users\...\AppData**.

| Sistem | Fișier | Cum se instalează |
|---|---|---|
| Windows 64 de biți | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dublu clic. Se instalează în `C:\E-LAYER-ANALYZER`, fără drepturi de administrator. Fără scurtătură în meniul Start (ar sta în AppData), doar pictograma opțională de pe desktop. |
| Windows 32 de biți | `...-win-x86-setup.exe` | Ca mai sus, pentru Windows pe 32 de biți. |
| Windows portabil | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Dezarhivați oriunde și rulați `E-LAYER-ANALYZER.exe`. |
| Linux 64 de biți (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, apoi porniți-l din meniu: la prima pornire programul se copiază în `~/E-LAYER-ANALYZER`, cu dosarul `data` alături. |
| Linux portabil | `...-linux-x64-portatile.tar.gz` | Dezarhivați și rulați `./E-LAYER-ANALYZER`, sau rulați `./installa.sh`: se instalează în `~/E-LAYER-ANALYZER` cu o pictogramă în meniu. |
| Raspberry Pi (64 de biți) | `e-layer-analyzer_x.y.z_arm64.deb` sau `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 sau 5 cu **Raspberry Pi OS pe 64 de biți** și desktop (recomandat 4 GB de RAM). Pachetul .deb ca mai sus; varianta portabilă ca pentru Linux. |
| Raspberry Pi (32 de biți) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 sau 4 cu **Raspberry Pi OS pe 32 de biți**. Dezarhivați și rulați `./E-LAYER-ANALYZER` sau `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) sau `...-osx-x64.dmg` (Intel) | Deschideți .dmg-ul, clic dreapta pe **Installa E-LAYER-ANALYZER.command** > Deschide. Se instalează în `~/E-LAYER-ANALYZER`. |
| Mac, doar aplicația | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Dezarhivați **E-LAYER-ANALYZER.app** într-un dosar al dumneavoastră (de exemplu `~/E-LAYER-ANALYZER`, nu în Aplicații): dosarul `data` se creează lângă aplicație. La prima pornire clic dreapta > Deschide. |

## Actualizare

Instalați noua versiune peste cea veche: setările, spoturile colectate, logurile și `call3.txt` se păstrează.

## Cerințe

- Nu trebuie instalat .NET: programul conține tot ce îi trebuie.
- Conexiune la Internet pentru hartă (Natural Earth, la prima pornire), datele NOAA despre aurore, radarele YA-MS și sesiunile telnet.
- Raspberry Pi Zero și Pi 1 nu sunt suportate (procesoarele lor nu sunt suportate de .NET).
