# Instalasi

[← Home](Home-id)

Unduh file yang tepat dari halaman **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Program dan folder `data` selalu berada bersama di satu folder milik Anda sendiri: **tidak ada file yang pernah ditulis ke C:\Users\...\AppData**.

| Sistem | File | Cara memasang |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Klik dua kali. Terpasang di `C:\E-LAYER-ANALYZER`, tanpa perlu hak administrator. Tidak ada pintasan di menu Start (pintasan itu akan berada di AppData), hanya ikon desktop opsional. |
| Windows 32 bit | `...-win-x86-setup.exe` | Seperti di atas, untuk Windows 32-bit. |
| Windows portabel | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Ekstrak di mana saja lalu jalankan `E-LAYER-ANALYZER.exe`. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, lalu jalankan dari menu: pada start pertama program menyalin dirinya ke `~/E-LAYER-ANALYZER`, dengan folder `data` di sebelahnya. |
| Linux portabel | `...-linux-x64-portatile.tar.gz` | Ekstrak lalu jalankan `./E-LAYER-ANALYZER`, atau jalankan `./installa.sh`: terpasang di `~/E-LAYER-ANALYZER` dengan ikon di menu. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` atau `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 atau 5 dengan **Raspberry Pi OS 64-bit** dan desktop (disarankan RAM 4 GB). File .deb seperti di atas; versi portabel seperti untuk Linux. |
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 atau 4 dengan **Raspberry Pi OS 32-bit**. Ekstrak lalu jalankan `./E-LAYER-ANALYZER` atau `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) atau `...-osx-x64.dmg` (Intel) | Buka .dmg, klik kanan **Installa E-LAYER-ANALYZER.command** > Open. Terpasang di `~/E-LAYER-ANALYZER`. |
| Mac, hanya aplikasi | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Ekstrak **E-LAYER-ANALYZER.app** ke folder milik Anda sendiri (misalnya `~/E-LAYER-ANALYZER`, bukan Applications): folder `data` dibuat di sebelah aplikasi. Pada start pertama klik kanan > Open. |

## Pembaruan

Pasang versi baru di atas versi lama: pengaturan, spot terkumpul, log dan `call3.txt` tetap tersimpan.

**Pembaruan otomatis (sejak versi 0.4.5).** Saat mulai lalu setiap 6 jam program memeriksa Releases di GitHub. Jika ada versi baru, program membuka sebuah jendela dan menampilkan **▲ x.y.z** berwarna oranye di samping indikator NTP pada bilah atas:

- **Perbarui sekarang** mengunduh paket portabel untuk komputer Anda (Windows 64/32 bit, Linux, Raspberry Pi, Mac), menutup program, mengganti file lalu menjalankannya kembali. Pengaturan, spot terkumpul, log dan `call3.txt` tetap tersimpan.
- **Nanti** menanyakan lagi pada pemeriksaan berikutnya; **Lewati versi ini** berhenti menawarkan versi tersebut.
- Mengklik versi yang tampil di bilah atas (misalnya `v0.4.5`) langsung memeriksa pembaruan. Pemeriksaan otomatis dapat dimatikan dengan kotak centang di jendela yang sama.
- Jika folder program tidak dapat ditulisi (misalnya folder sistem), gunakan paket instalasi dari halaman Release.

**File hilang.** Saat mulai program memeriksa apakah semua file yang disertakan saat distribusi ada (terjemahan, dokumen, tabel, yang tercantum di `data/manifest.txt`). Jika ada yang hilang, sebuah jendela menampilkan daftarnya dan **Unduh dari GitHub** mengambilnya kembali dari versi yang sama di GitHub.

## Persyaratan

- Tidak ada .NET yang perlu dipasang: program sudah berisi semua yang dibutuhkannya.
- Koneksi internet untuk peta (Natural Earth, pada start pertama), data aurora NOAA, radar YA-MS dan sesi telnet.
- Raspberry Pi Zero dan Pi 1 tidak didukung (prosesornya tidak ditangani oleh .NET).
