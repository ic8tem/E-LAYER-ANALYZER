# Installering

[← Home](Home-af)

Laai die regte lêer van die **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**-bladsy af. Die program en die `data`-gids bly altyd saam in 'n gids van jou eie: **geen lêer word ooit na C:\Users\...\AppData geskryf nie**.

| Stelsel | Lêer | Hoe om te installeer |
|---|---|---|
| Windows 64-bis | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dubbelklik. Installeer na `C:\E-LAYER-ANALYZER`, geen administrateursregte nodig nie. Geen kortpad in die Start-kieslys nie (dit sou in AppData beland), slegs die opsionele lessenaarikoon. |
| Windows 32-bis | `...-win-x86-setup.exe` | Soos hierbo, vir 32-bis-Windows. |
| Windows draagbaar | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Pak enige plek uit en voer `E-LAYER-ANALYZER.exe` uit. |
| Linux 64-bis (rekenaar) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, begin dit dan vanuit die kieslys: by die eerste opstart kopieer die program homself na `~/E-LAYER-ANALYZER`, met die `data`-gids daarlangs. |
| Linux draagbaar | `...-linux-x64-portatile.tar.gz` | Pak uit en voer `./E-LAYER-ANALYZER` uit, of voer `./installa.sh` uit: installeer na `~/E-LAYER-ANALYZER` met 'n ikoon in die kieslys. |
| Raspberry Pi (64-bis) | `e-layer-analyzer_x.y.z_arm64.deb` of `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 of 5 met **64-bis-Raspberry Pi OS** en lessenaar (4 GB RAM aanbeveel). Die .deb soos hierbo; die draagbare weergawe soos vir Linux. |
| Raspberry Pi (32-bis) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 of 4 met **32-bis-Raspberry Pi OS**. Pak uit en voer `./E-LAYER-ANALYZER` of `./installa.sh` uit. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) of `...-osx-x64.dmg` (Intel) | Open die .dmg, regsklik op **Installa E-LAYER-ANALYZER.command** > Open. Installeer na `~/E-LAYER-ANALYZER`. |
| Mac, slegs toepassing | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Pak **E-LAYER-ANALYZER.app** uit in 'n gids van jou eie (byvoorbeeld `~/E-LAYER-ANALYZER`, nie Applications nie): die `data`-gids word langs die toepassing geskep. By die eerste opstart regsklik > Open. |

## Bywerking

Installeer die nuwe weergawe oor die ou een: instellings, versamelde spots, logs en `call3.txt` word behou.

## Vereistes

- Geen .NET om te installeer nie: die program bevat alles wat dit nodig het.
- Internetverbinding vir die kaart (Natural Earth, by die eerste opstart), die NOAA-auroradata, die YA-MS-radars en die telnet-sessies.
- Raspberry Pi Zero en Pi 1 word nie ondersteun nie (hul verwerkers word nie deur .NET hanteer nie).
