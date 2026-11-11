# Installatie

[← Home](Home-nl)

Download het juiste bestand van de pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Het programma en de map `data` blijven altijd samen in een map naar keuze: **er wordt nooit een bestand naar C:\Users\...\AppData geschreven**.

| Systeem | Bestand | Installeren |
|---|---|---|
| Windows 64-bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dubbelklikken. Installeert in `C:\E-LAYER-ANALYZER`, zonder beheerdersrechten. Geen snelkoppeling in het menu Start (die zou in AppData staan), alleen het optionele pictogram op het bureaublad. |
| Windows 32-bit | `...-win-x86-setup.exe` | Zoals hierboven, voor 32-bit Windows. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Overal uitpakken en `E-LAYER-ANALYZER.exe` starten. |
| Linux 64-bit (pc) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, daarna starten vanuit het menu: bij de eerste start kopieert het programma zichzelf naar `~/E-LAYER-ANALYZER`, met de map `data` ernaast. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | Uitpakken en `./E-LAYER-ANALYZER` starten, of `./installa.sh` uitvoeren: installeert in `~/E-LAYER-ANALYZER` met een pictogram in het menu. |
| Raspberry Pi (64-bit) | `e-layer-analyzer_x.y.z_arm64.deb` of `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 of 5 met **64-bit Raspberry Pi OS** en desktop (4 GB RAM aanbevolen). De .deb zoals hierboven; de portable versie zoals bij Linux. |
| Raspberry Pi (32-bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 of 4 met **32-bit Raspberry Pi OS**. Uitpakken en `./E-LAYER-ANALYZER` of `./installa.sh` uitvoeren. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) of `...-osx-x64.dmg` (Intel) | Open de .dmg, rechtsklik op **Installa E-LAYER-ANALYZER.command** > Open. Installeert in `~/E-LAYER-ANALYZER`. |
| Mac, alleen de app | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** uitpakken in een map naar keuze (bijvoorbeeld `~/E-LAYER-ANALYZER`, niet Applications): de map `data` wordt naast de app aangemaakt. Bij de eerste start rechtsklik > Open. |

## Bijwerken

Installeer de nieuwe versie over de oude heen: instellingen, verzamelde spots, logs en `call3.txt` blijven behouden.

**Automatisch bijwerken (vanaf versie 0.4.5).** Bij het opstarten en daarna elke 6 uur controleert het programma de GitHub-Releases. Als er een nieuwe versie is, opent het een venster en toont het een oranje **▲ x.y.z** naast de NTP-aanduiding in de bovenste balk:

- **Nu bijwerken** downloadt het draagbare pakket voor je computer (Windows 64/32-bit, Linux, Raspberry Pi, Mac), sluit het programma, vervangt de bestanden en start het opnieuw. Instellingen, verzamelde spots, logs en `call3.txt` blijven behouden.
- **Later** vraagt het opnieuw bij de volgende controle; **Deze versie overslaan** stelt die versie niet meer voor.
- Een klik op de versie in de bovenste balk (bijvoorbeeld `v0.4.5`) zoekt meteen naar updates. De automatische controle kan worden uitgeschakeld met het vakje in hetzelfde venster.
- Als de programmamap niet beschrijfbaar is (bijvoorbeeld een systeemmap), gebruik dan het installatiepakket van de releasepagina.

**Ontbrekende bestanden.** Bij het opstarten controleert het programma of alle bestanden waarmee het is verspreid aanwezig zijn (vertalingen, documenten, tabellen, vermeld in `data/manifest.txt`). Ontbreekt er een, dan toont een venster de lijst en haalt **Downloaden van GitHub** ze opnieuw op uit dezelfde versie op GitHub.

## Vereisten

- Geen .NET te installeren: het programma bevat alles wat het nodig heeft.
- Internetverbinding voor de kaart (Natural Earth, bij de eerste start), de aurora-gegevens van NOAA, de radars van YA-MS en de telnet-sessies.
- Raspberry Pi Zero en Pi 1 worden niet ondersteund (hun processors worden door .NET niet ondersteund).
