# SPORADIC-E

[← Home](Home-id)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Peta

Sebuah tombol di atas peta memilih tampilan:

- **Azimutal**: peta azimutal ekuidistan yang berpusat di stasiun Anda, dengan penggeser **zoom** (300 hingga 20000 km dari tengah, juga dengan roda mouse).
- **Mercator**: peta dunia, dengan tombol Eropa dan Dunia.

Pada kedua proyeksi, **lingkaran jarak Flavio IK3XTV** digambar di sekitar stasiun: 500 km (minimum satu lompatan), 1500-2000 km (optimal), 2400 km (maksimum satu lompatan), 5000 km (maksimum dua lompatan) dan 6900 km (maksimum tiga lompatan).

Pada tampilan azimutal, cincin putus-putus menunjukkan **km dari tengah** (langkahnya berubah sesuai zoom), tepi peta menandai jarak maksimum yang ditampilkan, dan kursor mouse memberikan jarak dan azimut dari tengah.

![Multi-lompatan](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Yang ditampilkan

- **Spot** beberapa menit terakhir (15 menit - 12 jam) pada band **28, 40 (8 m, mati sejak awal), 50, 70 dan 144 MHz**, diambil dari sesi telnet. Locator diambil dari komentar spot (mis. `JN70CN<ES>JO62QM`) atau dari basis data CALL3.
- Spot dengan **<MS>**, **<AU>** atau **<TR>** di teksnya **dibuang**: spot tersebut tidak berkaitan dengan sporadic-E tetapi berguna untuk meteor scatter, aurora atau tropo.
- **Transekuatorial, aturan penulis (IC8TEM)**: ke arah Afrika dan Amerika Selatan, jika titik tengah jatuh di pita inklinasi magnetik antara -15° dan +15°, kontak tersebut adalah transekuatorial (TEP), bukan sporadic-E, kecuali spot menyatakan `<ES>`.
- **144 MHz, aturan penulis (IC8TEM)**: sporadic-E di 2 m dimulai **di atas 900 km** (spot yang lebih pendek dibuang). Satu spot 144 MHz **tidak digambar sendirian**: spot itu muncul jika dalam **5 menit** tiba spot 144 MHz kedua dengan awan di tempat yang sama (titik pantul dalam 300 km), atau jika ada spot **50 MHz lompatan pendek** di dekatnya (dalam 1200 km, pada 15 menit terakhir). Bilah di atas menampilkan berapa banyak spot 144 MHz yang menunggu konfirmasi.
- Untuk setiap spot: jarak, **jumlah lompatan** menurut tabel **IK3XTV**, elevasi, **foEs minimum** (hukum sekan).
- **Lompatan pertama kedua stasiun**: kotak oranye = lompatan pertama spotter, ungu = lompatan pertama DX (lompatan sama); kuning = titik tengah satu lompatan.
- **Lompatan pertama pendek** (lingkaran kosong): pada bukaan multi-lompatan penulis pernah mengalami lompatan pertama pendek dalam **1200 km** dari QTH-nya sendiri; panjangnya dapat diatur.
- **Awan Es** pada **kotak Maidenhead** (2° x 1°), diwarnai menurut foEs minimum: biru < 5, biru muda < 7, hijau < 10, kuning < 14, oranye < 20, merah > 20 MHz.
- Tabel **Awan Es**: untuk setiap kotak, **MUF untuk stasiun Anda** (merah = 144 MHz mungkin, oranye = 70, kuning = 50).

## Tabel IK3XTV

| | minimum | optimal | maksimum |
|---|---|---|---|
| Satu lompatan | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dua lompatan | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Tiga lompatan | – | 4800-6400 km | 6900 km |

## Cara menggunakan

- **Klik sebuah spot**: lintasannya disorot, dengan titik pantul, locator dan azimut di rincian.
- **Klik sebuah spot** di tabel: lintasannya menyala di peta (cahaya kuning) dan, jika berada di luar tampilan, peta bergeser atau memperbesar radiusnya untuk menampilkannya. Spot tetap terpilih saat spot baru tiba.
- **Tabel mengikuti peta**: saat zoom diatur, tabel spot dan tabel awan hanya menampilkan yang terlihat di peta.
- **Klik peta**: titik tersebut diperlakukan sebagai kemungkinan awan Es, dengan foEs yang diperlukan untuk 28, 50, 70 dan 144 MHz.

## Catatan tentang 432 MHz

Sporadic-E **tidak berlaku untuk 432 MHz**: MUF Es mencapai sekitar 150-200 MHz dan rekor ARRL mencantumkan kontak Es hanya di 50 dan 144 MHz. Spot 70 cm tidak dianalisis di sini.

## Kotak berwarna menurut frekuensi dan MUF berkedip

Kotak Maidenhead titik pantul diwarnai menurut frekuensi spot tertinggi: **hijau** hingga 29 MHz, **kuning** hingga 51, **oranye** hingga 71, **merah** hingga 145 MHz. Dari foEs kotak diperkirakan MUF yang mungkin pada lompatan terpanjang (2400 km, tabel IK3XTV): jika mencapai band yang lebih tinggi, kotak **berkedip** dengan warna band itu (prinsip IK3XTV dan Jacobs-Leinwoll: skip pendek berarti MUF tinggi).

Di kanan atas peta muncul ringkasan N0NBH (E-skip benua yang dipilih, SFI, indeks K dan A).

## Aturan per band (versi 0.4.18)

- **50 MHz**: tidak ada Sporadic-E dalam 450 km. Spot FT8 dan FT4 adalah Sporadic-E hanya jika menjadi bagian dari sebuah awan: spot 50 MHz lain dalam 4 menit dengan titik tengah dalam 150 km. Spot FT8/FT4 tunggal masuk meteor scatter.
- **70 MHz**: tidak ada Sporadic-E dalam 450 km. FT8 dan FT4 masuk Sporadic-E bahkan di dalam jendela meteor scatter bandplan (mode yang diketahui menentukan lebih dulu, baru frekuensi).
- **40 MHz (8 m)**: jarak minimum diturunkan dari 450 km di 50 MHz dengan hukum sekan (sekitar 330 km); hanya jarak dalam lompatan pertama geometri lapisan E (sekitar 2440 km). Warna lintasan: kuning.
- **144 MHz**: Sporadic-E mulai 900 km, dengan konfirmasi; spot Q65 dan JT65 adalah EME dan dibuang, kecuali ada `<ES>` di teks.
- G0ISW (Propagation identification, 2024) melihat Sporadic-E terpendek di atas 400 km di 50 dan 70 MHz dan di atas 1400 km di 144 MHz: aturan penulis selaras di 50 dan 70 MHz dan lebih longgar di 144 MHz, tetapi meminta konfirmasi.
- Urutan penerapan semua aturan, dengan nilai yang dipakai, ditampilkan di **[PENGATURAN](Settings-id)** › Distribusi spot.
