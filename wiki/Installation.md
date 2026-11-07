# Installation

[← Home](Home)

Download the right file from the **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** page. The program and the `data` folder always stay together in a folder of your own: **no file is ever written to C:\Users\...\AppData**.

| System | File | How to install |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Double-click. Installs to `C:\E-LAYER-ANALYZER`, no administrator rights needed. No Start menu shortcut (it would live in AppData), only the optional desktop icon. |
| Windows 32 bit | `...-windows-x86-setup.exe` | As above, for 32-bit Windows. |
| Windows portable | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Extract anywhere and run `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Extract and run `./installa.sh`: installs to `~/E-LAYER-ANALYZER` with an icon in the menu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 or 5 with **64-bit Raspberry Pi OS** and desktop (4 GB of RAM recommended). Extract and run `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) or `...-mac-intel.dmg` | Open the .dmg, right-click **Installa E-LAYER-ANALYZER.command** > Open. Installs to `~/E-LAYER-ANALYZER`. |

## Updating

Install the new version over the old one: settings, collected spots, logs and `call3.txt` are kept.

## Requirements

- No .NET to install: the program contains everything it needs.
- Internet connection for the map (Natural Earth, on first start), the NOAA aurora data, the YA-MS radars and the telnet sessions.
- Raspberry Pi Zero and Pi 1 are not supported (their processors are not handled by .NET).
