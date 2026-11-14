# Installatie

[← Home](Home-nl)

Download het juiste bestand van de pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Het programma en de map `data` blijven altijd samen in een map naar keuze: **er wordt nooit een bestand naar C:\Users\...\AppData geschreven**.

| Systeem | Bestand | Installeren |
|---|---|---|
| Windows 64-bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dubbelklikken. Installeert in `C:\E-LAYER-ANALYZER`, zonder beheerdersrechten. Geen snelkoppeling in het menu Start (die zou in AppData staan), alleen het optionele pictogram op het bureaublad. |
| Windows 32-bit | `...-win-x86-setup.exe` | Zoals hierboven, voor 32-bit Windows. |
| Windows op ARM (Snapdragon-pc's) | `...-win-arm64-setup.exe` | Zoals hierboven, voor Windows op ARM-processors. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Overal uitpakken en `E-LAYER-ANALYZER.exe` starten. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, daarna starten vanuit het menu: bij de eerste start kopieert het programma zichzelf naar `~/E-LAYER-ANALYZER`, met de map `data` ernaast. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (ook `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (of `zypper install`), daarna zoals de .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (of `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, daarna zoals de .deb. |
| Slackware (x86_64 en ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (of `-aarch64-`) | Als root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, daarna `e-layer-analyzer` starten vanuit het menu of de terminal: bij de eerste start kopieert het programma zichzelf naar `~/E-LAYER-ANALYZER`. |
| Elke Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (of `-aarch64.AppImage`) | Uitvoerbaar maken en starten: bij de eerste start kopieert het het programma naar `~/E-LAYER-ANALYZER` en start het van daaruit. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | Uitpakken en `./E-LAYER-ANALYZER` starten, of `./installa.sh` uitvoeren: installeert in `~/E-LAYER-ANALYZER` met een pictogram in het menu. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, daarna zoals de portable versie voor Linux. |
| Raspberry Pi (64-bit) | `e-layer-analyzer_x.y.z_arm64.deb` of `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 of 5 met **64-bit Raspberry Pi OS** en desktop (4 GB RAM aanbevolen). De .deb zoals hierboven; de portable versie zoals bij Linux. |
| Raspberry Pi (32-bit) | `e-layer-analyzer_x.y.z_armhf.deb` of `...-linux-arm-portatile.tar.gz` | Pi 2, 3 of 4 met **32-bit Raspberry Pi OS**. De .deb zoals hierboven; de portable versie: uitpakken en `./E-LAYER-ANALYZER` of `./installa.sh` uitvoeren. |
| Raspberry Pi met Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` of `...-linux-arm64-portatile.tar.gz` | Zoals Slackware hierboven. De portable versie zoals bij Linux. |
| Linux armv7l (andere 32-bit ARM-borden) | `...-linux-armv7l-portatile.tar.gz` | Zoals de portable versie voor Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Experimenteel, niet gecontroleerd.** Heeft de .NET 8 van de gemeenschap nodig (`pkg install dotnet`) en de voor FreeBSD gebouwde native bibliotheken SkiaSharp en HarfBuzzSharp, die Avalonia niet levert: zonder die start het programma niet. Zie `LEGGIMI-FREEBSD.txt` in het pakket. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) of `...-osx-x64.dmg` (Intel) | Open de .dmg, rechtsklik op **Installa E-LAYER-ANALYZER.command** > Open. Installeert in `~/E-LAYER-ANALYZER`. |
| Mac, alleen de app | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** uitpakken in een map naar keuze (bijvoorbeeld `~/E-LAYER-ANALYZER`, niet Applications): de map `data` wordt naast de app aangemaakt. Bij de eerste start rechtsklik > Open. |

## Bijwerken

Installeer de nieuwe versie over de oude heen: instellingen, verzamelde spots, logs en `call3.txt` blijven behouden.

**Systeempakketten (.deb, .rpm, Arch, Slackware) worden niet automatisch bijgewerkt.** Daarmee meldt het programma alleen de nieuwe versie: download het nieuwe pakket van de pagina Releases en installeer het zoals de eerste keer (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Automatische updates werken met het Windows-installatieprogramma en de zips, de Mac-app, de draagbare tar.gz-pakketten en de AppImage.

**Automatisch bijwerken (vanaf versie 0.4.5).** Bij het opstarten en daarna elke 6 uur controleert het programma de GitHub-Releases. Als er een nieuwe versie is, opent het een venster en toont het een oranje **▲ x.y.z** naast de NTP-aanduiding in de bovenste balk:

**Vanaf versie 0.4.12 zijn updates automatisch en stil**: als er een nieuwe versie op GitHub staat, downloadt het programma die, installeert die en herstart vanzelf zonder iets te vragen. Ontbrekende bestanden worden ook zonder vragen opnieuw gedownload. Het updatevenster verschijnt alleen met een klik op de versie bovenaan, of als je de automatische updates uitschakelt in INSTELLINGEN (Algemeen).

- **Nu bijwerken** downloadt het draagbare pakket voor je computer (Windows 64/32-bit, Linux, Raspberry Pi, Mac), sluit het programma, vervangt de bestanden en start het opnieuw. Instellingen, verzamelde spots, logs en `call3.txt` blijven behouden.
- **Later** vraagt het opnieuw bij de volgende controle; **Deze versie overslaan** stelt die versie niet meer voor.
- Een klik op de versie in de bovenste balk (bijvoorbeeld `v0.4.5`) zoekt meteen naar updates. De automatische controle kan worden uitgeschakeld met het vakje in hetzelfde venster.
- Als de programmamap niet beschrijfbaar is (bijvoorbeeld een systeemmap), gebruik dan het installatiepakket van de releasepagina.

**Ontbrekende bestanden.** Bij het opstarten controleert het programma of alle bestanden waarmee het is verspreid aanwezig zijn (vertalingen, documenten, tabellen, vermeld in `data/manifest.txt`). Ontbreekt er een, dan toont een venster de lijst en haalt **Downloaden van GitHub** ze opnieuw op uit dezelfde versie op GitHub.

## Vereisten

- Geen .NET te installeren: het programma bevat alles wat het nodig heeft.
- Internetverbinding voor de kaart (Natural Earth, bij de eerste start), de aurora-gegevens van NOAA, de radars van YA-MS en de telnet-sessies.
- Raspberry Pi Zero en Pi 1 worden niet ondersteund (hun processors worden door .NET niet ondersteund).
- Er zijn geen versies voor Linux i686 (32-bit pc's) en OpenBSD, omdat .NET voor die systemen niet bestaat. Voor FreeBSD is er alleen het experimentele pakket.
