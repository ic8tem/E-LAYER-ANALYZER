# Pag-install

[← Home](Home-tl)

I-download ang tamang file mula sa pahinang **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Laging magkasama ang programa at ang `data` folder sa isang folder na sa iyo: **walang file na isinusulat kailanman sa C:\Users\...\AppData**.

| Sistema | File | Paano i-install |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | I-double-click. Nag-i-install sa `C:\E-LAYER-ANALYZER`, hindi kailangan ng karapatang administrator. Walang shortcut sa Start menu (malalagay ito sa AppData), ang opsiyonal na icon sa desktop lamang. |
| Windows 32 bit | `...-win-x86-setup.exe` | Gaya ng nasa itaas, para sa Windows na 32-bit. |
| Windows on ARM (mga PC na Snapdragon) | `...-win-arm64-setup.exe` | Gaya ng nasa itaas, para sa Windows na may ARM processor. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | I-extract kahit saan at patakbuhin ang `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, saka simulan ito mula sa menu: sa unang pagsisimula, kinokopya ng programa ang sarili nito sa `~/E-LAYER-ANALYZER`, kasama ang `data` folder sa tabi nito. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (pati `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (o `zypper install`), saka gaya ng .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (o `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, saka gaya ng .deb. |
| Slackware (x86_64 at ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (o `-aarch64-`) | Bilang root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, pagkatapos ay patakbuhin ang `e-layer-analyzer` mula sa menu o sa terminal: sa unang pagsisimula, kinokopya ng programa ang sarili sa `~/E-LAYER-ANALYZER`. |
| Anumang Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (o `-aarch64.AppImage`) | Gawin itong executable at patakbuhin: sa unang pagsisimula, kinokopya nito ang programa sa `~/E-LAYER-ANALYZER` at doon ito pinapatakbo. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | I-extract at patakbuhin ang `./E-LAYER-ANALYZER`, o patakbuhin ang `./installa.sh`: nag-i-install sa `~/E-LAYER-ANALYZER` na may icon sa menu. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, saka gaya ng Linux portable. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` o `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 o 5 na may **Raspberry Pi OS na 64-bit** at desktop (inirerekomenda ang 4 GB na RAM). Ang .deb gaya ng nasa itaas; ang portable gaya ng sa Linux. |
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` o `...-linux-arm-portatile.tar.gz` | Pi 2, 3 o 4 na may **Raspberry Pi OS na 32-bit**. Ang .deb gaya ng nasa itaas; ang portable: i-extract at patakbuhin ang `./E-LAYER-ANALYZER` o `./installa.sh`. |
| Raspberry Pi na may Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` o `...-linux-arm64-portatile.tar.gz` | Gaya ng Slackware sa itaas. Ang portable gaya ng sa Linux. |
| Linux armv7l (iba pang ARM board na 32-bit) | `...-linux-armv7l-portatile.tar.gz` | Gaya ng Linux portable. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Eksperimental, hindi pa nasusuri.** Kailangan ang .NET 8 ng komunidad (`pkg install dotnet`) at ang mga native library na SkiaSharp at HarfBuzzSharp na kinompile para sa FreeBSD, na hindi ibinibigay ng Avalonia: kung wala ang mga ito, hindi tatakbo ang programa. Tingnan ang `LEGGIMI-FREEBSD.txt` sa package. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) o `...-osx-x64.dmg` (Intel) | Buksan ang .dmg, i-right-click ang **Installa E-LAYER-ANALYZER.command** > Open. Nag-i-install sa `~/E-LAYER-ANALYZER`. |
| Mac, application lamang | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | I-extract ang **E-LAYER-ANALYZER.app** sa isang folder na sa iyo (halimbawa `~/E-LAYER-ANALYZER`, hindi sa Applications): nalilikha ang `data` folder sa tabi ng app. Sa unang pagsisimula, i-right-click > Open. |

## Pag-update

I-install ang bagong bersyon sa ibabaw ng luma: pinananatili ang mga setting, nakolektang spot, log at `call3.txt`.

**Hindi kusang nag-a-update ang mga package ng sistema (.deb, .rpm, Arch, Slackware).** Sa mga ito, ipinapaalam lamang ng programa ang bagong bersiyon: i-download ang bagong package mula sa pahinang Releases at i-install ito gaya ng una (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Gumagana ang awtomatikong update sa installer at mga zip ng Windows, sa app ng Mac, sa mga portable na package na tar.gz at sa AppImage.

**Awtomatikong update (mula sa bersiyon 0.4.5).** Sa pagbukas at pagkatapos ay bawat 30 minuto, tinitingnan ng programa ang mga Release sa GitHub. Kapag may bagong bersiyon, nagbubukas ito ng window at nagpapakita ng kulay-kahel na **▲ x.y.z** sa tabi ng indicator ng NTP sa bar sa itaas:

**Mula sa bersyon 0.4.12, awtomatiko at tahimik ang mga update**: kapag may bagong bersyon sa GitHub, dina-download ito ng programa, ini-install, at kusang nagre-restart nang walang tinatanong. Ang mga nawawalang file ay muling dina-download din nang hindi nagtatanong. Lumalabas lang ang window ng update kapag kinlik ang bersyon sa itaas, o kung papatayin ang awtomatikong update sa MGA SETTING (Pangkalahatan).

- Dina-download ng **I-update ngayon** ang portable na package para sa iyong computer (Windows 64/32 bit, Linux, Raspberry Pi, Mac), isinasara ang programa, pinapalitan ang mga file at muli itong binubuksan. Pinananatili ang mga setting, nakolektang spot, log at `call3.txt`.
- Nagtatanong muli ang **Mamaya** sa susunod na pagsusuri; tumitigil ang **Laktawan ang bersiyong ito** sa pagmumungkahi ng bersiyong iyon.
- Ang pag-click sa bersiyong ipinapakita sa bar sa itaas (halimbawa `v0.4.5`) ay agad na tumitingin ng mga update. Maaaring patayin ang awtomatikong pagsusuri gamit ang checkbox sa parehong window.
- Kung hindi masulatan ang folder ng programa (halimbawa isang system folder), gamitin na lamang ang installation package mula sa pahina ng release.

**Mga nawawalang file.** Sa pagbukas, tinitingnan ng programa kung naroon ang lahat ng file na kasama nang ipamahagi ito (mga salin, dokumento, talahanayan, na nakalista sa `data/manifest.txt`). Kung may nawawala, inililista ang mga ito ng isang window at kinukuha muli ng **I-download mula sa GitHub** mula sa parehong bersiyon sa GitHub.

## Mga kailangan

- Walang .NET na kailangang i-install: nasa programa na ang lahat ng kailangan nito.
- Koneksiyon sa internet para sa mapa (Natural Earth, sa unang pagsisimula), ang datos ng aurora ng NOAA, ang mga radar ng YA-MS at ang mga telnet session.
- Hindi suportado ang Raspberry Pi Zero at Pi 1 (hindi hinahawakan ng .NET ang kanilang mga processor).
- Walang bersiyon para sa Linux i686 (mga PC na 32-bit) at OpenBSD, dahil walang .NET para sa mga sistemang iyon. Eksperimental na package lamang ang mayroon para sa FreeBSD.
