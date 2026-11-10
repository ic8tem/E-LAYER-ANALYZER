# Pag-install

[← Home](Home-tl)

I-download ang tamang file mula sa pahinang **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Laging magkasama ang programa at ang `data` folder sa isang folder na sa iyo: **walang file na isinusulat kailanman sa C:\Users\...\AppData**.

| Sistema | File | Paano i-install |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | I-double-click. Nag-i-install sa `C:\E-LAYER-ANALYZER`, hindi kailangan ng karapatang administrator. Walang shortcut sa Start menu (malalagay ito sa AppData), ang opsiyonal na icon sa desktop lamang. |
| Windows 32 bit | `...-win-x86-setup.exe` | Gaya ng nasa itaas, para sa Windows na 32-bit. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | I-extract kahit saan at patakbuhin ang `E-LAYER-ANALYZER.exe`. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, saka simulan ito mula sa menu: sa unang pagsisimula, kinokopya ng programa ang sarili nito sa `~/E-LAYER-ANALYZER`, kasama ang `data` folder sa tabi nito. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | I-extract at patakbuhin ang `./E-LAYER-ANALYZER`, o patakbuhin ang `./installa.sh`: nag-i-install sa `~/E-LAYER-ANALYZER` na may icon sa menu. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` o `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 o 5 na may **Raspberry Pi OS na 64-bit** at desktop (inirerekomenda ang 4 GB na RAM). Ang .deb gaya ng nasa itaas; ang portable gaya ng sa Linux. |
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 o 4 na may **Raspberry Pi OS na 32-bit**. I-extract at patakbuhin ang `./E-LAYER-ANALYZER` o `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) o `...-osx-x64.dmg` (Intel) | Buksan ang .dmg, i-right-click ang **Installa E-LAYER-ANALYZER.command** > Open. Nag-i-install sa `~/E-LAYER-ANALYZER`. |
| Mac, application lamang | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | I-extract ang **E-LAYER-ANALYZER.app** sa isang folder na sa iyo (halimbawa `~/E-LAYER-ANALYZER`, hindi sa Applications): nalilikha ang `data` folder sa tabi ng app. Sa unang pagsisimula, i-right-click > Open. |

## Pag-update

I-install ang bagong bersyon sa ibabaw ng luma: pinananatili ang mga setting, nakolektang spot, log at `call3.txt`.

## Mga kailangan

- Walang .NET na kailangang i-install: nasa programa na ang lahat ng kailangan nito.
- Koneksiyon sa internet para sa mapa (Natural Earth, sa unang pagsisimula), ang datos ng aurora ng NOAA, ang mga radar ng YA-MS at ang mga telnet session.
- Hindi suportado ang Raspberry Pi Zero at Pi 1 (hindi hinahawakan ng .NET ang kanilang mga processor).
