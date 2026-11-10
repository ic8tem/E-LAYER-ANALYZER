# Kurulum

[← Home](Home-tr)

Doğru dosyayı **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** sayfasından indirin. Program ve `data` klasörü her zaman kendinize ait bir klasörde birlikte durur: **C:\Users\...\AppData içine asla hiçbir dosya yazılmaz**.

| Sistem | Dosya | Nasıl kurulur |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Çift tıklayın. `C:\E-LAYER-ANALYZER` klasörüne kurulur, yönetici hakları gerekmez. Başlat menüsü kısayolu yoktur (AppData içinde dururdu), yalnızca isteğe bağlı masaüstü simgesi vardır. |
| Windows 32 bit | `...-win-x86-setup.exe` | Yukarıdaki gibi, 32 bit Windows için. |
| Windows taşınabilir | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | İstediğiniz yere açın ve `E-LAYER-ANALYZER.exe`'yi çalıştırın. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, ardından menüden başlatın: ilk açılışta program kendini `~/E-LAYER-ANALYZER` klasörüne kopyalar, `data` klasörü de yanında olur. |
| Linux taşınabilir | `...-linux-x64-portatile.tar.gz` | Açın ve `./E-LAYER-ANALYZER` dosyasını çalıştırın ya da `./installa.sh` komutunu çalıştırın: menüde bir simgeyle `~/E-LAYER-ANALYZER` klasörüne kurulur. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` ya da `...-linux-arm64-portatile.tar.gz` | **64 bit Raspberry Pi OS** ve masaüstüyle Pi 3, 4 ya da 5 (4 GB RAM önerilir). .deb yukarıdaki gibi; taşınabilir sürüm Linux'taki gibi. |
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | **32 bit Raspberry Pi OS** ile Pi 2, 3 ya da 4. Açın ve `./E-LAYER-ANALYZER` ya da `./installa.sh` komutunu çalıştırın. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ya da `...-osx-x64.dmg` (Intel) | .dmg dosyasını açın, **Installa E-LAYER-ANALYZER.command** üzerine sağ tıklayın > Aç. `~/E-LAYER-ANALYZER` klasörüne kurulur. |
| Mac, yalnızca uygulama | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** dosyasını kendinize ait bir klasöre açın (örneğin `~/E-LAYER-ANALYZER`, Applications değil): `data` klasörü uygulamanın yanında oluşturulur. İlk açılışta sağ tıklayın > Aç. |

## Güncelleme

Yeni sürümü eskisinin üzerine kurun: ayarlar, toplanan spot'lar, loglar ve `call3.txt` korunur.

## Gereksinimler

- Kurulacak .NET yok: program ihtiyaç duyduğu her şeyi içerir.
- Harita (Natural Earth, ilk açılışta), NOAA aurora verileri, YA-MS radarları ve telnet oturumları için internet bağlantısı.
- Raspberry Pi Zero ve Pi 1 desteklenmez (işlemcileri .NET tarafından desteklenmez).
