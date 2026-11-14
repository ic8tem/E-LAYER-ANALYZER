# PENGATURAN

[← Home](Home-id)

Sejak versi 0.4.18 tab **PENGATURAN** menghimpun opsi umum program dan distribusi spot ke tab-tab. Tab ini memiliki dua subtab.

## Umum

| Bagian | Yang Anda atur |
|---|---|
| Stasiun | Callsign, locator dan ketinggian, sama dengan yang di bilah atas; **Terapkan** mengonfirmasinya untuk semua tab. |
| Tampilan | Bahasa, skema warna (skema YAEC dan YA-MS serta, sejak versi 0.5.0, enam tema IP-TRX-CTRL: Terang, Malam, Hitam, Biru, Ungu, Hijau) dan **ukuran huruf seluruh program** (85-140 %, seperti di YA-MS). Bahasa dan warna membangun ulang jendela: pengaturan dan spot tetap ada. |
| Peta dan data prakiraan | Benua, menu yang sama dengan yang ada di bilah tab. |
| Waktu | Status NTP dan selisih antara waktu server dan jam PC. |
| Pembaruan | Pemeriksaan di GitHub saat mulai dan setiap 30 menit, pembaruan otomatis dan diam-diam, tombol **Periksa sekarang**. Pembaruan otomatis tidak berlaku untuk paket sistem (.deb, .rpm, Arch, Slackware). |
| Data | Tombol yang membuka folder `data`. Kunci API Bureau of Meteorology tetap di DATA COLLECTION › Data propagasi. |

## Distribusi spot

Di sebelah kiri sebuah tabel mencantumkan aturan yang dipakai program untuk menentukan ke mana setiap spot pergi, **menurut urutan penerapannya**: aturan pertama yang berlaku menentukan tabnya. Nilai yang ditampilkan adalah nilai yang diatur di filter.

| # | Jika spot... | masuk ke |
|---|---|---|
| 1 | berada di luar filter lokal (di bawah 27990 kHz atau di atas 145 MHz; 430-440 MHz hanya aurora) | dibuang |
| 2 | memuat AUE, AU-E atau AURORAL E | AURORA (aurora-E) |
| 3 | memuat kata aurora (bawaan AU, AUR) | AURORA |
| 4 | memuat kata F.A.I. (bawaan FAI; band 6, 4 dan 2 m: F.A.I. tidak mencapai 432 MHz) | F.A.I. |
| 5 | memiliki mode meteor scatter atau `<MS>` | METEOR SCATTER |
| 6 | jatuh di jendela MS bandplan (tidak eksklusif di 50 MHz; FT8 dan FT4 di 50 dan 70 MHz dikecualikan) | METEOR SCATTER |
| 7 | adalah FT8 atau FT4 di 50 MHz tanpa spot lain dalam 4 menit dengan titik tengah dalam 150 km | meteor scatter (bukan Sporadic-E) |
| 8 | adalah Q65 atau JT65 di 144 MHz (kecuali `<ES>`) | EME: dibuang |
| 9 | memuat tag propagasi lain | bukan Sporadic-E |
| 10 | transekuatorial | bukan Sporadic-E (TEP) |
| 11-15 | berada di 28, 40, 50, 70 atau 144 MHz dalam batas aturan jarak | SPORADIC-E |
| 16 | tidak masuk aturan mana pun (tropo, 432 MHz, HF...) | tidak ke tab mana pun (tetap di Spot terkumpul) |

Di sebelah kanan ada **filter lokal** dan filter setiap mode, dalam lima subtab: Meteor scatter (filter YA-MS diatur di tab METEOR SCATTER), Sporadic-E, F.A.I., Aurora dan **Uji filter**, tempat Anda menempelkan satu baris kluster dan melihat bagaimana baris itu diklasifikasikan. Tombol **Pulihkan aturan penulis** mengembalikan nilai bawaan.
