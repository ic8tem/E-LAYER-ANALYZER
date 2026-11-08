# Installering

[← Home](Home-af)

Laai die regte lêer van die **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**-bladsy af. Die program en die `data`-gids bly altyd saam in 'n gids van jou eie: **geen lêer word ooit na C:\Users\...\AppData geskryf nie**.

| Stelsel | Lêer | Hoe om te installeer |
|---|---|---|
| Windows 64-bis | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Dubbelklik. Installeer na `C:\E-LAYER-ANALYZER`, geen administrateursregte nodig nie. Geen kortpad in die Start-kieslys nie (dit sou in AppData beland), slegs die opsionele lessenaarikoon. |
| Windows 32-bis | `...-windows-x86-setup.exe` | Soos hierbo, vir 32-bis-Windows. |
| Windows draagbaar | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Pak enige plek uit en voer `E-LAYER-ANALYZER.exe` uit. |
| Linux (rekenaar) | `...-linux-x64.tar.gz` | Pak uit en voer `./installa.sh` uit: installeer na `~/E-LAYER-ANALYZER` met 'n ikoon in die kieslys. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 of 5 met **64-bis-Raspberry Pi OS** en lessenaar (4 GB RAM aanbeveel). Pak uit en voer `./installa.sh` uit. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) of `...-mac-intel.dmg` | Open die .dmg, regsklik op **Installa E-LAYER-ANALYZER.command** > Open. Installeer na `~/E-LAYER-ANALYZER`. |

## Bywerking

Installeer die nuwe weergawe oor die ou een: instellings, versamelde spots, logs en `call3.txt` word behou.

## Vereistes

- Geen .NET om te installeer nie: die program bevat alles wat dit nodig het.
- Internetverbinding vir die kaart (Natural Earth, by die eerste opstart), die NOAA-auroradata, die YA-MS-radars en die telnet-sessies.
- Raspberry Pi Zero en Pi 1 word nie ondersteun nie (hul verwerkers word nie deur .NET hanteer nie).
