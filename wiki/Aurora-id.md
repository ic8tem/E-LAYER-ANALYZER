# AURORA

[← Home](Home-id)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Oval aurora** NOAA OVATION Prime (berarsir), **Kp** 3 hari terakhir, **angin surya** (Bz, kecepatan, kerapatan), diperbarui setiap 5 menit dan disimpan di `data/swpc`.
- Tampilan yang sama seperti F.A.I.: **Scatter trail** untuk sepasang stasiun dan **Iso-refleksi** untuk memverifikasi sebuah titik, dengan ketinggian bawaan 110 km.
- **Arah antena yang disarankan untuk kedua stasiun**: di sepanjang garis tegak lurus setiap stasiun (titik-titik dalam warnanya), titik dengan probabilitas oval tertinggi. Aturan praktis (G3WPO): mulailah sekitar 45°, di antara utara dan timur.
- Contoh IO70VJ - JO59BC: scatter point JP01AK di lepas pantai Shetland, deviasi 0,97°, QTF 11° dan 299°.
- **Spot aurora di peta**: spot dengan **<AU>** atau **<AUR>** di teksnya adalah spot aurora (aturan penulis). Dengan kotak *Spot <AU>/<AUR> di peta dengan perhitungan* dicentang, tab menggambar spot 2 jam terakhir (paling banyak 30): untuk setiap pasangan spotter-DX tab menghitung scatter trail dan menampilkan scatter point yang paling mungkin (lingkaran oranye), dengan QTF dan elevasi kedua stasiun di hasil.
- Selain scatter point paling mungkin, hasil menampilkan **rentang QTF** kedua stasiun di sepanjang seluruh Scatter trail (deviasi ≤ 1,5°).

## BENCHMARK AURORA

Tab AURORA memiliki dua subtab: **REAL-TIME** (perhitungan yang dijelaskan di atas) dan **BENCHMARK AURORA**, bangku uji model pada **298 QSO aurora nyata** dengan waktu dan locator: log EI5FK yang telah dipublikasikan (IO51RT, Irlandia, 2003-2005, 144 dan 432 MHz) dan peta log PA0O (JO33HG, Belanda).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Untuk setiap QSO program menghitung scatter point dengan model yang sama (IGRF-14, ketinggian 110 km, refraksi 4/3, toleransi 3°).
- Warna: **hijau** = scatter point pada 370-1100 km dari kedua stasiun (Lange-Hesse); **kuning** = terjelaskan tetapi di luar jendela itu; **oranye** = QSO lebih dari 2000 km; **merah** = tidak ada scatter point, QSO tidak terjelaskan oleh model.
- Ringkasan memberikan persentase dan, untuk setiap log, rentang dan median QTF terhitung. Klik sebuah baris untuk menggambar lintasan dan kedua arah antena.
- Hasil (versi 0.4.4): **296 dari 298 QSO terjelaskan (99%)**, 243 (82%) di dalam jendela 370-1100 km; median QTF EI5FK antara 25° dan 41°, yaitu arah timur laut yang disarankan G3WPO. Log tidak mencantumkan arah antena: pemeriksaannya pada geometri, bukan pada QTF.
