# BENCHMARK 50 MHz

[← Home](Home-id)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Bangku uji untuk **sporadic-E multi-lompatan di 50 MHz** menggunakan log ADIF satu stasiun dan, secara opsional, stasiun kedua.

## Memuat log

- **Stasiun 1**: *Muat ADIF* / *Lepas ADIF*. Program mengambil `STATION_CALLSIGN` dan `MY_GRIDSQUARE` dari log; posisi pemancar adalah `MY_GRIDSQUARE` setiap QSO.
- **Stasiun 2** (opsional): tombol yang sama di baris kedua; kotak centang *Stasiun 2* menampilkan atau menyembunyikannya. Lintasan stasiun 1 berwarna biru muda, lintasan stasiun 2 ungu.
- Program memakai QSO 50 MHz dengan locator (locator yang tidak ada atau hanya 4 karakter diambil dari CALL3), yang multi-lompatan: di atas 3100 km (aturan penulis; jumlah lompatan mengikuti tabel IK3XTV).
- Kontak **transekuatorial** dikecualikan (aturan penulis): ke arah Afrika dan Amerika Selatan, dengan titik tengah di pita inklinasi magnetik antara -20° dan +20°, kecuali log memuat `PROP_MODE=ES`.

## Hari dan jam

Pilih **hari** (saat dibuka, hari dengan QSO multi-lompatan terbanyak) dan jam **dari… sampai…** UTC, dengan langkah 15 menit. Kotak centang **foEs** (opsional) mewarnai lintasan menurut foEs minimum dan menampilkannya di tabel.

## Peta

Peta **Mercator** dengan pusat bebas: roda = zoom, seret = geser.

![Lintasan](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Titik kuning buram** menandai pusat setiap lompatan, tempat awan Es diperkirakan berada; pada lintasan di bawah mouse titik itu menjadi lebih besar.
- **Mouse di dekat lintasan**: garis dan ikon diperbesar dan muncul **kotak WWL 6 karakter** stasiun DX.
- **Klik kiri** pada lintasan itu: sebuah popup menanyakan apakah akan ditampilkan **semua QSO dalam file** (semua hari) yang ujung jauhnya berada **dalam 150 km** dari titik tersebut. *Ya*: peta dibersihkan dan menampilkan QSO-QSO itu di dalam lingkaran oranye; *Tidak*: peta tetap seperti semula. *Kembali ke hari dan jam* kembali ke pilihan sebelumnya.

![Sekitar](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Rincian

Untuk lintasan yang dipilih: jarak dan penilaian menurut tabel IK3XTV, azimut di kedua arah, lompatan sama dengan kotak titik-titik pantulnya, lompatan pertama pendek dalam 1200 km (pengalaman IC8TEM) dan, dengan kotak centang foEs, foEs minimum.
