# Pag-install

[← Home](Home-tl)

I-download ang tamang file mula sa pahinang **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Laging magkasama ang programa at ang `data` folder sa isang folder na sa iyo: **walang file na isinusulat kailanman sa C:\Users\...\AppData**.

| Sistema | File | Paano i-install |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | I-double-click. Nag-i-install sa `C:\E-LAYER-ANALYZER`, hindi kailangan ng karapatang administrator. Walang shortcut sa Start menu (malalagay ito sa AppData), ang opsiyonal na icon sa desktop lamang. |
| Windows 32 bit | `...-windows-x86-setup.exe` | Gaya ng nasa itaas, para sa Windows na 32-bit. |
| Windows portable | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | I-extract kahit saan at patakbuhin ang `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | I-extract at patakbuhin ang `./installa.sh`: nag-i-install sa `~/E-LAYER-ANALYZER` na may icon sa menu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 o 5 na may **Raspberry Pi OS na 64-bit** at desktop (inirerekomenda ang 4 GB na RAM). I-extract at patakbuhin ang `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) o `...-mac-intel.dmg` | Buksan ang .dmg, i-right-click ang **Installa E-LAYER-ANALYZER.command** > Open. Nag-i-install sa `~/E-LAYER-ANALYZER`. |

## Pag-update

I-install ang bagong bersyon sa ibabaw ng luma: pinananatili ang mga setting, nakolektang spot, log at `call3.txt`.

## Mga kailangan

- Walang .NET na kailangang i-install: nasa programa na ang lahat ng kailangan nito.
- Koneksiyon sa internet para sa mapa (Natural Earth, sa unang pagsisimula), ang datos ng aurora ng NOAA, ang mga radar ng YA-MS at ang mga telnet session.
- Hindi suportado ang Raspberry Pi Zero at Pi 1 (hindi hinahawakan ng .NET ang kanilang mga processor).
