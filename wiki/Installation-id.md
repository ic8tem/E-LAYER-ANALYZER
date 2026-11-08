# Instalasi

[← Home](Home-id)

Unduh file yang tepat dari halaman **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Program dan folder `data` selalu berada bersama di satu folder milik Anda sendiri: **tidak ada file yang pernah ditulis ke C:\Users\...\AppData**.

| Sistem | File | Cara memasang |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Klik dua kali. Terpasang di `C:\E-LAYER-ANALYZER`, tanpa perlu hak administrator. Tidak ada pintasan di menu Start (pintasan itu akan berada di AppData), hanya ikon desktop opsional. |
| Windows 32 bit | `...-windows-x86-setup.exe` | Seperti di atas, untuk Windows 32-bit. |
| Windows portabel | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Ekstrak di mana saja lalu jalankan `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Ekstrak lalu jalankan `./installa.sh`: terpasang di `~/E-LAYER-ANALYZER` dengan ikon di menu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 atau 5 dengan **Raspberry Pi OS 64-bit** dan desktop (disarankan RAM 4 GB). Ekstrak lalu jalankan `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) atau `...-mac-intel.dmg` | Buka .dmg, klik kanan **Installa E-LAYER-ANALYZER.command** > Open. Terpasang di `~/E-LAYER-ANALYZER`. |

## Pembaruan

Pasang versi baru di atas versi lama: pengaturan, spot terkumpul, log dan `call3.txt` tetap tersimpan.

## Persyaratan

- Tidak ada .NET yang perlu dipasang: program sudah berisi semua yang dibutuhkannya.
- Koneksi internet untuk peta (Natural Earth, pada start pertama), data aurora NOAA, radar YA-MS dan sesi telnet.
- Raspberry Pi Zero dan Pi 1 tidak didukung (prosesornya tidak ditangani oleh .NET).
