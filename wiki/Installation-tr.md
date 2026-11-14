# Kurulum

[← Home](Home-tr)

Doğru dosyayı **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** sayfasından indirin. Program ve `data` klasörü her zaman kendinize ait bir klasörde birlikte durur: **C:\Users\...\AppData içine asla hiçbir dosya yazılmaz**.

| Sistem | Dosya | Nasıl kurulur |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Çift tıklayın. `C:\E-LAYER-ANALYZER` klasörüne kurulur, yönetici hakları gerekmez. Başlat menüsü kısayolu yoktur (AppData içinde dururdu), yalnızca isteğe bağlı masaüstü simgesi vardır. |
| Windows 32 bit | `...-win-x86-setup.exe` | Yukarıdaki gibi, 32 bit Windows için. |
| ARM üzerinde Windows (Snapdragon bilgisayarlar) | `...-win-arm64-setup.exe` | Yukarıdaki gibi, ARM işlemcili Windows için. |
| Windows taşınabilir | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | İstediğiniz yere açın ve `E-LAYER-ANALYZER.exe`'yi çalıştırın. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, ardından menüden başlatın: ilk açılışta program kendini `~/E-LAYER-ANALYZER` klasörüne kopyalar, `data` klasörü de yanında olur. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (ayrıca `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (ya da `zypper install`), sonra .deb'deki gibi. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (ya da `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, sonra .deb'deki gibi. |
| Slackware (x86_64 ve ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (ya da `-aarch64-`) | Root olarak: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, ardından `e-layer-analyzer` komutunu menüden ya da terminalden başlatın: program ilk açılışta kendini `~/E-LAYER-ANALYZER` klasörüne kopyalar. |
| Herhangi bir Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (ya da `-aarch64.AppImage`) | Çalıştırılabilir yapın ve başlatın: ilk açılışta programı `~/E-LAYER-ANALYZER` klasörüne kopyalar ve oradan çalıştırır. |
| Linux taşınabilir | `...-linux-x64-portatile.tar.gz` | Açın ve `./E-LAYER-ANALYZER` dosyasını çalıştırın ya da `./installa.sh` komutunu çalıştırın: menüde bir simgeyle `~/E-LAYER-ANALYZER` klasörüne kurulur. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, sonra Linux taşınabilir sürümü gibi. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` ya da `...-linux-arm64-portatile.tar.gz` | **64 bit Raspberry Pi OS** ve masaüstüyle Pi 3, 4 ya da 5 (4 GB RAM önerilir). .deb yukarıdaki gibi; taşınabilir sürüm Linux'taki gibi. |
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` ya da `...-linux-arm-portatile.tar.gz` | **32 bit Raspberry Pi OS** ile Pi 2, 3 ya da 4. .deb yukarıdaki gibi; taşınabilir sürüm: açın ve `./E-LAYER-ANALYZER` ya da `./installa.sh` komutunu çalıştırın. |
| Slackware ARM64'lü Raspberry Pi | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` ya da `...-linux-arm64-portatile.tar.gz` | Yukarıdaki Slackware gibi. Taşınabilir sürüm Linux'taki gibi. |
| Linux armv7l (diğer 32 bit ARM kartlar) | `...-linux-armv7l-portatile.tar.gz` | Linux taşınabilir sürümü gibi. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Deneysel, doğrulanmamış.** Topluluğun .NET 8'i (`pkg install dotnet`) ve Avalonia'nın sağlamadığı, FreeBSD için derlenmiş SkiaSharp ve HarfBuzzSharp yerel kütüphaneleri gerekir: bunlar olmadan program başlamaz. Paketteki `LEGGIMI-FREEBSD.txt` dosyasına bakın. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ya da `...-osx-x64.dmg` (Intel) | .dmg dosyasını açın, **Installa E-LAYER-ANALYZER.command** üzerine sağ tıklayın > Aç. `~/E-LAYER-ANALYZER` klasörüne kurulur. |
| Mac, yalnızca uygulama | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** dosyasını kendinize ait bir klasöre açın (örneğin `~/E-LAYER-ANALYZER`, Applications değil): `data` klasörü uygulamanın yanında oluşturulur. İlk açılışta sağ tıklayın > Aç. |

## Güncelleme

Yeni sürümü eskisinin üzerine kurun: ayarlar, toplanan spot'lar, loglar ve `call3.txt` korunur.

**Sistem paketleri (.deb, .rpm, Arch, Slackware) otomatik olarak güncellenmez.** Bunlarla program yalnızca yeni sürümü bildirir: yeni paketi Releases sayfasından indirin ve ilk seferdeki gibi kurun (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Otomatik güncellemeler Windows kurulum programı ve zip'leri, Mac uygulaması, taşınabilir tar.gz paketleri ve AppImage ile çalışır.

**Otomatik güncelleme (0.4.5 sürümünden itibaren).** Program başlangıçta ve ardından her 6 saatte bir GitHub Releases'i denetler. Yeni bir sürüm çıktığında bir pencere açar ve üst çubuktaki NTP göstergesinin yanında turuncu bir **▲ x.y.z** gösterir:

**0.4.12 sürümünden itibaren güncellemeler otomatik ve sessizdir**: GitHub'da yeni bir sürüm olduğunda program onu indirir, kurar ve hiçbir şey sormadan kendini yeniden başlatır. Eksik dosyalar da sormadan yeniden indirilir. Güncelleme penceresi yalnızca üstteki sürüme tıklayınca ya da AYARLAR'da (Genel) otomatik güncellemeleri kapatırsanız görünür.

- **Şimdi güncelle** bilgisayarınız için taşınabilir paketi (Windows 64/32 bit, Linux, Raspberry Pi, Mac) indirir, programı kapatır, dosyaları değiştirir ve programı yeniden başlatır. Ayarlar, toplanan spot'lar, loglar ve `call3.txt` korunur.
- **Daha sonra** bir sonraki denetimde yeniden sorar; **Bu sürümü atla** o sürümü önermeyi bırakır.
- Üst çubukta gösterilen sürüme (örneğin `v0.4.5`) tıklamak güncellemeleri hemen denetler. Otomatik denetim aynı penceredeki onay kutusuyla kapatılabilir.
- Program klasörüne yazılamıyorsa (örneğin bir sistem klasörü), bunun yerine sürüm sayfasındaki kurulum paketini kullanın.

**Eksik dosyalar.** Başlangıçta program, birlikte dağıtıldığı tüm dosyaların (çeviriler, belgeler, tablolar; `data/manifest.txt` içinde listelenir) mevcut olduğunu denetler. Eksik olan varsa bir pencere bunları listeler ve **GitHub'dan indir** bunları GitHub'daki aynı sürümden yeniden indirir.

## Gereksinimler

- Kurulacak .NET yok: program ihtiyaç duyduğu her şeyi içerir.
- Harita (Natural Earth, ilk açılışta), NOAA aurora verileri, YA-MS radarları ve telnet oturumları için internet bağlantısı.
- Raspberry Pi Zero ve Pi 1 desteklenmez (işlemcileri .NET tarafından desteklenmez).
- Linux i686 (32 bit PC) ve OpenBSD için sürüm yoktur, çünkü bu sistemler için .NET bulunmaz. FreeBSD için yalnızca deneysel paket vardır.
