# AWAN Es DARI WAKTU KE WAKTU

[← Home](Home-id)

![NUBI Es NEL TEMPO](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Subtab **SPORADIC-E** (sejak versi 0.5.2): bagaimana **awan E-sporadis** lahir, bergeser, dan padam, langkah demi langkah.

## Metode

Metode **Chartier, Hanley, dan Emmons (2022)**: setiap hubungan E-sporadis menyatakan bahwa di titik pantul foEs **setidaknya f / sec φ**. Titik-titik dikumpulkan dalam sel dan langkah waktu, dan setiap sel mengambil **nilai tertinggi** dari langkah tersebut. Kerapatan elektron minimum adalah NmE = 1,24·10¹⁰ · foEs² elektron per meter kubik.

Program memakai titik pantul dari modelnya sendiri (semua lompatan), bukan hanya titik tengah. Nilainya adalah **batas bawah**: sel kosong hanya berarti tidak ada hubungan.

## Sumber dan kontrol

- **Spot sesi** (diperbarui setiap kali tab dibuka) atau **Muat log ADIF atau spot CSV**.
- **Ekspor spot sesi (CSV)**: arsip spot dikosongkan saat program ditutup, jadi Anda bisa melihatnya kembali nanti.
- **Band MHz** (28, 40, 50, 70, 144), **Langkah** (5, 10, atau 15 menit), **Sel** (1°, 2°, atau 4°), **Ketinggian Es km**.
- Bilah waktu: ⏮ ◀ *Mulai/Hentikan* ▶ ⏭ dan penggeser; saat dibuka dimulai dari langkah dengan sel terbanyak.
- *Langkah sebelumnya memudar* menampilkan tiga langkah sebelum langkah yang dipilih; *Hubungan* menggambar lintasan dengan warna band.

## Tabel dan grafik

- Sel pada langkah ini: Sel (sudut barat daya dan locator pusat), foEs≥, NmE≥, Hubungan, Band. Klik pada sebuah sel: hubungannya disorot.
- Grafik: foEs minimum tertinggi (garis kuning) dan jumlah sel (batang) untuk setiap langkah.
