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
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` or `...-linux-arm-portatile.tar.gz` | Pi 2, 3 or 4 with **32-bit Raspberry Pi OS**. The .deb as above; the portable: extract and run `./E-LAYER-ANALYZER` or `./installa.sh`. |
| Raspberry Pi with Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` or `...-linux-arm64-portatile.tar.gz` | As root: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, then start `e-layer-analyzer` from the menu or the terminal: on first start the program copies itself to `~/E-LAYER-ANALYZER`. The portable as for Linux. |
| Linux armv7l (other 32-bit ARM boards) | `...-linux-armv7l-portatile.tar.gz` | As the Linux portable. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) or `...-osx-x64.dmg` (Intel) | Open the .dmg, right-click **Installa E-LAYER-ANALYZER.command** > Open. Installs to `~/E-LAYER-ANALYZER`. |
| Mac, application only | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Extract **E-LAYER-ANALYZER.app** into a folder of your own (for example `~/E-LAYER-ANALYZER`, not Applications): the `data` folder is created next to the app. On first start right-click > Open. |

## Updating

Install the new version over the old one: settings, collected spots, logs and `call3.txt` are kept.

**Automatic update (from version 0.4.5).** At start-up and then every 6 hours the program checks the GitHub Releases. When a new version is out it opens a window and shows an orange **▲ x.y.z** next to the NTP indicator in the top bar:

- **Update now** downloads the portable package for your computer (Windows 64/32 bit, Linux, Raspberry Pi, Mac), closes the program, replaces the files and restarts it. Settings, collected spots, logs and `call3.txt` are kept.
- **Later** asks again at the next check; **Skip this version** stops proposing that version.
- Clicking the version shown in the top bar (for example `v0.4.5`) checks for updates at once. The automatic check can be switched off with the box in the same window.
- If the program folder cannot be written (for example a system folder), use the installation package from the release page instead.

**Missing files.** At start-up the program checks that all the files it was distributed with are present (translations, documents, tables, listed in `data/manifest.txt`). If any is missing, a window lists them and **Download from GitHub** fetches them again from the same version on GitHub.

## Requirements

- No .NET to install: the program contains everything it needs.
- Internet connection for the map (Natural Earth, on first start), the NOAA aurora data, the YA-MS radars and the telnet sessions.
- Raspberry Pi Zero and Pi 1 are not supported (their processors are not handled by .NET).
- There are no versions for Linux i686 (32-bit PCs), because .NET does not exist for that system, nor for FreeBSD, where the Avalonia graphics libraries are missing.
