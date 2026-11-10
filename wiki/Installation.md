# Installation

[← Home](Home)

Download the right file from the **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** page. The program and the `data` folder always stay together in a folder of your own: **no file is ever written to C:\Users\...\AppData**.

| System | File | How to install |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Double-click. Installs to `C:\E-LAYER-ANALYZER`, no administrator rights needed. No Start menu shortcut (it would live in AppData), only the optional desktop icon. |
| Windows 32 bit | `...-win-x86-setup.exe` | As above, for 32-bit Windows. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Extract anywhere and run `E-LAYER-ANALYZER.exe`. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, then start it from the menu: on first start the program copies itself to `~/E-LAYER-ANALYZER`, with the `data` folder next to it. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | Extract and run `./E-LAYER-ANALYZER`, or run `./installa.sh`: installs to `~/E-LAYER-ANALYZER` with an icon in the menu. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` or `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 or 5 with **64-bit Raspberry Pi OS** and desktop (4 GB of RAM recommended). The .deb as above; the portable as for Linux. |
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 or 4 with **32-bit Raspberry Pi OS**. Extract and run `./E-LAYER-ANALYZER` or `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) or `...-osx-x64.dmg` (Intel) | Open the .dmg, right-click **Installa E-LAYER-ANALYZER.command** > Open. Installs to `~/E-LAYER-ANALYZER`. |
| Mac, application only | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Extract **E-LAYER-ANALYZER.app** into a folder of your own (for example `~/E-LAYER-ANALYZER`, not Applications): the `data` folder is created next to the app. On first start right-click > Open. |

## Updating

Install the new version over the old one: settings, collected spots, logs and `call3.txt` are kept.

## Requirements

- No .NET to install: the program contains everything it needs.
- Internet connection for the map (Natural Earth, on first start), the NOAA aurora data, the YA-MS radars and the telnet sessions.
- Raspberry Pi Zero and Pi 1 are not supported (their processors are not handled by .NET).
