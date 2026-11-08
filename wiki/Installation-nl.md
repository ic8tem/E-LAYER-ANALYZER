# Installatie

[← Home](Home-nl)

Download het juiste bestand van de pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Het programma en de map `data` blijven altijd samen in een map naar keuze: **er wordt nooit een bestand naar C:\Users\...\AppData geschreven**.

| Systeem | Bestand | Installeren |
|---|---|---|
| Windows 64-bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Dubbelklikken. Installeert in `C:\E-LAYER-ANALYZER`, zonder beheerdersrechten. Geen snelkoppeling in het menu Start (die zou in AppData staan), alleen het optionele pictogram op het bureaublad. |
| Windows 32-bit | `...-windows-x86-setup.exe` | Zoals hierboven, voor 32-bit Windows. |
| Windows portable | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Overal uitpakken en `E-LAYER-ANALYZER.exe` starten. |
| Linux (pc) | `...-linux-x64.tar.gz` | Uitpakken en `./installa.sh` uitvoeren: installeert in `~/E-LAYER-ANALYZER` met een pictogram in het menu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 of 5 met **64-bit Raspberry Pi OS** en desktop (4 GB RAM aanbevolen). Uitpakken en `./installa.sh` uitvoeren. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) of `...-mac-intel.dmg` | Open de .dmg, rechtsklik op **Installa E-LAYER-ANALYZER.command** > Open. Installeert in `~/E-LAYER-ANALYZER`. |

## Bijwerken

Installeer de nieuwe versie over de oude heen: instellingen, verzamelde spots, logs en `call3.txt` blijven behouden.

## Vereisten

- Geen .NET te installeren: het programma bevat alles wat het nodig heeft.
- Internetverbinding voor de kaart (Natural Earth, bij de eerste start), de aurora-gegevens van NOAA, de radars van YA-MS en de telnet-sessies.
- Raspberry Pi Zero en Pi 1 worden niet ondersteund (hun processors worden door .NET niet ondersteund).
