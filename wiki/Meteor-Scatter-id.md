# METEOR SCATTER

[← Home](Home-id)

Tab ini berisi **YA-MS 1.8.7** lengkap, program meteor scatter karya IC8TEM: bola langit dengan radian, fluks radar SKiYMET (Collm, Sodankylä), perhitungan efisiensi hujan meteor, implementasi eksperimental prakiraan, geometri dan redaman, GRAVES, peta kotak, **Hujan meteor langsung** (ZHR yang diukur Global Meteor Network dan radian dari radar CMOR), **Peta sumber** (Collm, Sodankylä, GRAVES dan CMOR relatif terhadap stasiun) serta dokumen **Penjelasan perhitungan efisiensi** beserta rumus-rumusnya.

YA-MS 1.5.0 menambahkan tab **Pengumpulan data** (spot meteor scatter yang difilter menurut MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Di dalam E-LAYER ANALYZER tab ini tidak membuka sesi telnet sendiri: tab ini menerima baris-baris dari sesi DATA COLLECTION, sehingga tidak ada login ganda dengan callsign yang sama.

YA-MS 1.6.0 menata ulang tab ke dalam **Langsung dan data waktu nyata** (lalu lintas meteor scatter langsung dengan awan setiap kontak, fluks radar, GRAVES, pengumpulan data) dan **Perhitungan prakiraan** (prakiraan, geometri, perhitungan efisiensi, implementasi prakiraan).

YA-MS 1.7.0 dan 1.8.0 menambahkan perbandingan radian dengan katalog IAU MDC, hitungan harian Global Meteor Network per hujan meteor, indeks k yang diturunkan dari indeks populasi, jari-jari awal jejak meteor menurut Stober dkk. (2021), durasi ping antarpita (λ² sebagai bawaan atau pengukuran HamSCI MSQP dari NN4SA) dan perbandingan dengan deteksi BRAMS di Sonneberg (Perseid 2026). Pembaruan otomatis YA-MS dimatikan di dalam E-LAYER ANALYZER, yang memperbarui dirinya sendiri dari Releases GitHub.

YA-MS 1.8.1 memperlakukan lobus antena sebagai pola vertikal (kotak «pola antena», aktif secara bawaan): titik pantulan diberi bobot dengan penguatan vertikal kedua antena dan efisiensi tidak lagi hilang pada lintasan panjang. Ditambahkan peringatan «tidak ada zona bersama», kotak sudut β yang ditonjolkan (pilihan yang paling berpengaruh), satu tanggal perhitungan, «Panduan penggunaan» dan perbandingan dengan 181 767 spot nyata di 50 MHz dari HamSCI Meteor Scatter QSO Party Agustus 2025: jam dan sisi lintasan dapat diandalkan, persentasenya bersifat indikatif.

- YA-MS memakai bahasa dan skema warna yang dipilih di bagian atas E-LAYER ANALYZER.
- Pengaturan YA-MS disimpan di `data/config.ini`.
- Setiap versi baru YA-MS dimasukkan dengan skrip `tools/sync_yams.py`.

Untuk rinciannya lihat [repositori YA-MS](https://github.com/ic8tem/YA-MS).
