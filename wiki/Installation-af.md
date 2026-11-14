# Installering

[← Home](Home-af)

Laai die regte lêer van die **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**-bladsy af. Die program en die `data`-gids bly altyd saam in 'n gids van jou eie: **geen lêer word ooit na C:\Users\...\AppData geskryf nie**.

| Stelsel | Lêer | Hoe om te installeer |
|---|---|---|
| Windows 64-bis | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dubbelklik. Installeer na `C:\E-LAYER-ANALYZER`, geen administrateursregte nodig nie. Geen kortpad in die Start-kieslys nie (dit sou in AppData beland), slegs die opsionele lessenaarikoon. |
| Windows 32-bis | `...-win-x86-setup.exe` | Soos hierbo, vir 32-bis-Windows. |
| Windows op ARM (Snapdragon-rekenaars) | `...-win-arm64-setup.exe` | Soos hierbo, vir Windows op ARM-verwerkers. |
| Windows draagbaar | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Pak enige plek uit en voer `E-LAYER-ANALYZER.exe` uit. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, begin dit dan vanuit die kieslys: by die eerste opstart kopieer die program homself na `~/E-LAYER-ANALYZER`, met die `data`-gids daarlangs. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (ook `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (of `zypper install`), dan soos die .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (of `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, dan soos die .deb. |
| Slackware (x86_64 en ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (of `-aarch64-`) | As root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, begin dan `e-layer-analyzer` vanuit die kieslys of die terminaal: by die eerste opstart kopieer die program homself na `~/E-LAYER-ANALYZER`. |
| Enige Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (of `-aarch64.AppImage`) | Maak dit uitvoerbaar en begin dit: by die eerste opstart kopieer dit die program na `~/E-LAYER-ANALYZER` en begin dit van daar af. |
| Linux draagbaar | `...-linux-x64-portatile.tar.gz` | Pak uit en voer `./E-LAYER-ANALYZER` uit, of voer `./installa.sh` uit: installeer na `~/E-LAYER-ANALYZER` met 'n ikoon in die kieslys. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, dan soos die draagbare weergawe vir Linux. |
| Raspberry Pi (64-bis) | `e-layer-analyzer_x.y.z_arm64.deb` of `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 of 5 met **64-bis-Raspberry Pi OS** en lessenaar (4 GB RAM aanbeveel). Die .deb soos hierbo; die draagbare weergawe soos vir Linux. |
| Raspberry Pi (32-bis) | `e-layer-analyzer_x.y.z_armhf.deb` of `...-linux-arm-portatile.tar.gz` | Pi 2, 3 of 4 met **32-bis-Raspberry Pi OS**. Die .deb soos hierbo; die draagbare weergawe: pak uit en voer `./E-LAYER-ANALYZER` of `./installa.sh` uit. |
| Raspberry Pi met Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` of `...-linux-arm64-portatile.tar.gz` | Soos Slackware hierbo. Die draagbare weergawe soos vir Linux. |
| Linux armv7l (ander 32-bis-ARM-borde) | `...-linux-armv7l-portatile.tar.gz` | Soos die draagbare weergawe vir Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Eksperimenteel, nie geverifieer nie.** Benodig die gemeenskap se .NET 8 (`pkg install dotnet`) en die inheemse SkiaSharp- en HarfBuzzSharp-biblioteke wat vir FreeBSD saamgestel is, wat Avalonia nie verskaf nie: sonder hulle begin die program nie. Sien `LEGGIMI-FREEBSD.txt` in die pakket. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) of `...-osx-x64.dmg` (Intel) | Open die .dmg, regsklik op **Installa E-LAYER-ANALYZER.command** > Open. Installeer na `~/E-LAYER-ANALYZER`. |
| Mac, slegs toepassing | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Pak **E-LAYER-ANALYZER.app** uit in 'n gids van jou eie (byvoorbeeld `~/E-LAYER-ANALYZER`, nie Applications nie): die `data`-gids word langs die toepassing geskep. By die eerste opstart regsklik > Open. |

## Bywerking

Installeer die nuwe weergawe oor die ou een: instellings, versamelde spots, logs en `call3.txt` word behou.

**Stelselpakkette (.deb, .rpm, Arch, Slackware) word nie outomaties opgedateer nie.** Daarmee meld die program net die nuwe weergawe: laai die nuwe pakket van die Releases-bladsy af en installeer dit soos die eerste keer (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Outomatiese opdaterings werk met die Windows-installeerder en -zips, die Mac-app, die draagbare tar.gz-pakkette en die AppImage.

**Outomatiese bywerking (vanaf weergawe 0.4.5).** By opstart en daarna elke 30 minute kyk die program na die GitHub Releases. Wanneer 'n nuwe weergawe uit is, maak dit 'n venster oop en wys 'n oranje **▲ x.y.z** langs die NTP-aanwyser in die boonste balk:

**Vanaf weergawe 0.4.12 is opdaterings outomaties en stil**: wanneer daar 'n nuwe weergawe op GitHub is, laai die program dit af, installeer dit en begin self weer sonder om iets te vra. Ontbrekende lêers word ook sonder vra weer afgelaai. Die opdateringsvenster verskyn net met 'n klik op die weergawe bo, of as jy outomatiese opdaterings in INSTELLINGS (Algemeen) afskakel.

- **Werk nou by** laai die draagbare pakket vir jou rekenaar af (Windows 64/32-bis, Linux, Raspberry Pi, Mac), maak die program toe, vervang die lêers en herbegin dit. Instellings, versamelde spots, logs en `call3.txt` word behou.
- **Later** vra weer by die volgende kontrole; **Slaan hierdie weergawe oor** hou op om daardie weergawe aan te bied.
- 'n Klik op die weergawe wat in die boonste balk gewys word (byvoorbeeld `v0.4.5`) kyk dadelik na bywerkings. Die outomatiese kontrole kan met die merkblokkie in dieselfde venster afgeskakel word.
- As daar nie in die programgids geskryf kan word nie (byvoorbeeld 'n stelselgids), gebruik eerder die installeerpakket van die vrystellingsbladsy.

**Ontbrekende lêers.** By opstart kyk die program of al die lêers waarmee dit versprei is, teenwoordig is (vertalings, dokumente, tabelle, gelys in `data/manifest.txt`). As enige ontbreek, lys 'n venster hulle en **Laai af van GitHub** haal hulle weer af uit dieselfde weergawe op GitHub.

## Vereistes

- Geen .NET om te installeer nie: die program bevat alles wat dit nodig het.
- Internetverbinding vir die kaart (Natural Earth, by die eerste opstart), die NOAA-auroradata, die YA-MS-radars en die telnet-sessies.
- Raspberry Pi Zero en Pi 1 word nie ondersteun nie (hul verwerkers word nie deur .NET hanteer nie).
- Daar is geen weergawes vir Linux i686 (32-bis-rekenaars) en OpenBSD nie, omdat .NET nie vir daardie stelsels bestaan nie. Vir FreeBSD is daar net die eksperimentele pakket.
