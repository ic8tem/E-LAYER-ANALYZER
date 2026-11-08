# Kurulum

[← Home](Home-tr)

Doğru dosyayı **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** sayfasından indirin. Program ve `data` klasörü her zaman kendinize ait bir klasörde birlikte durur: **C:\Users\...\AppData içine asla hiçbir dosya yazılmaz**.

| Sistem | Dosya | Nasıl kurulur |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Çift tıklayın. `C:\E-LAYER-ANALYZER` klasörüne kurulur, yönetici hakları gerekmez. Başlat menüsü kısayolu yoktur (AppData içinde dururdu), yalnızca isteğe bağlı masaüstü simgesi vardır. |
| Windows 32 bit | `...-windows-x86-setup.exe` | Yukarıdaki gibi, 32 bit Windows için. |
| Windows taşınabilir | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | İstediğiniz yere açın ve `E-LAYER-ANALYZER.exe`'yi çalıştırın. |
| Linux (PC) | `...-linux-x64.tar.gz` | Açın ve `./installa.sh` komutunu çalıştırın: menüde bir simgeyle `~/E-LAYER-ANALYZER` klasörüne kurulur. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | **64 bit Raspberry Pi OS** ve masaüstüyle Pi 3, 4 ya da 5 (4 GB RAM önerilir). Açın ve `./installa.sh` komutunu çalıştırın. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) ya da `...-mac-intel.dmg` | .dmg dosyasını açın, **Installa E-LAYER-ANALYZER.command** üzerine sağ tıklayın > Aç. `~/E-LAYER-ANALYZER` klasörüne kurulur. |

## Güncelleme

Yeni sürümü eskisinin üzerine kurun: ayarlar, toplanan spot'lar, loglar ve `call3.txt` korunur.

## Gereksinimler

- Kurulacak .NET yok: program ihtiyaç duyduğu her şeyi içerir.
- Harita (Natural Earth, ilk açılışta), NOAA aurora verileri, YA-MS radarları ve telnet oturumları için internet bağlantısı.
- Raspberry Pi Zero ve Pi 1 desteklenmez (işlemcileri .NET tarafından desteklenmez).
