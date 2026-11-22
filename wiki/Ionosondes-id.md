# IONOSONDE

[← Home](Home-id)

![IONOSONDE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Subtab **SPORADIC-E** (sejak versi 0.5.2): **foEs yang diukur** oleh ionosonde jaringan **GIRO** di seluruh dunia, di samping foEs yang diturunkan program dari spot.

## Data

- 62 ionosonde GIRO (daftar ULCAR), dengan **foEs**, **foF2**, dan **MUF(D)** terakhir yang dibaca otomatis dari ionogram.
- Data datang saat tab pertama kali dibuka lalu **setiap 15 menit**, dengan sendirinya (kotak *Perbarui otomatis setiap 15 menit*); *Perbarui sekarang* memintanya segera. Arsip menyimpan 48 jam terakhir di folder `giro` dari data.
- Data **GIRO** (Reinisch dan Galkin 2011), lisensi **CC BY-NC-SA 4.0**: hanya penggunaan nonkomersial. Layanannya sama dengan *MUF Muncher* dari DM6AX.

## Peta dan tabel

- Setiap ionosonde adalah titik berwarna menurut foEs terakhir (warna yang sama dengan awan Es) dengan nilainya dalam MHz; **abu-abu** jika tidak ada data terbaru, dengan **cincin** jika ionogram terakhir berumur lebih dari 90 menit.
- Tabel: Kode, Ionosonde, km dari stasiun Anda, UTC ionogram terakhir, foEs, foF2, MUF(D), dan **Spot foEs≥**: foEs minimum yang diturunkan dari spot Es 30 menit terakhir yang memantul dalam 500 km dari ionosonde (dapat diatur), dengan jumlah spot.
- **h'Es GIRO** (sejak versi 0.6.5, kotak di REAL-TIME): ketinggian Es menjadi median h'Es terbaru ionosonde dalam 2500 km (80-120 km).

## Ionosonde yang dipilih

Klik pada sebuah baris atau di dekat sebuah ionosonde di peta:

- **grafik 24 jam terakhir**: foEs kuning, foF2 biru muda, MUF(D) ungu bertitik;
- **MUF E-sporadis** pada 1000, 1500, 2000, dan 2400 km (MUF = foEs · sec φ, pada ketinggian Es yang dipilih) dengan **band yang terbuka** (28, 40, 50, 70, 144 MHz);
- jika spot meminta lebih dari foEs yang diukur, program memberi tahu: awan bisa jauh dari ionosonde, atau ionosonde telah mencapai batas rentang frekuensinya (Chartier 2022).
