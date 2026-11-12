# F.A.I.

[← Home](Home-id)

F.A.I. (Field Aligned Irregularities) adalah hamburan dari iregularitas yang sejajar dengan medan geomagnetik, khas pada sore hari di musim panas setelah sporadic-E. Sinyal yang terhambur keluar pada sebuah **kerucut** yang sumbunya searah medan: dua stasiun dapat saling bekerja jika keduanya berada pada **kontur suplementer** (a1 + a2 = 180°, artikel IC8FAX-IC8TEM; kontur ±c dari K9SE). Medan magnet dihitung dengan model **IGRF-14**.

## Tampilan scatter trail (pasangan A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Isi **Stasiun A** dan **Stasiun B** (callsign dicari di CALL3 dan/atau locator). A dibiarkan kosong = stasiun Anda sendiri.
- Peta menampilkan **scatter trail**: semua titik pada ketinggian lapisan, terlihat oleh kedua stasiun, yang memenuhi syarat pantulan dalam 2,5° (kelas 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Kotak di kanan atas menampilkan **QTF dan elevasi** A (merah) dan B (hijau) ke setiap titik.
- Program menyarankan **scatter point yang paling mungkin** (deviasi minimum, lintasan terpendek).
- **Refraksi 4/3**: aktif secara bawaan, memperluas cakrawala.

## Tampilan iso-refleksi (kurva iso-refleksi)

![Iso-refleksi](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Ini adalah konstruksi geometris yang dipakai untuk **memeriksa apakah sebuah titik pantul sudah benar**.

- **Klik kiri** pada peta: Anda memilih titik yang akan diperiksa. **Klik kanan**: memindahkan stasiun B.
- **Verifikasi titik yang disarankan**: membuka grafik pada titik yang disarankan program.
- Garis kontur pada **0, ±1, ±2, ±4, ±8, ±16°**: pita berwarna pada skema gelap, **garis tebal** pada skema terang.
- Penilaian untuk pasangan: **memuaskan** dalam 1°, **dapat diterima** dalam 2,5°, **tidak memuaskan** di luar itu.
- Jika titik berada di bawah cakrawala salah satu stasiun, hanya muncul popup **Perhitungan tidak mungkin**.

## Verifikasi

| Kontak | Scatter point | Deviasi | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (di atas Genoa) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (Jerman barat daya, 28 km dari Brussel) | 0,88° | 338° / 87° |
| Beacon DB0FAI (305°) - Dresden via Sheffield (DL5MCG; Dresden mengarah ke 290° dan melaporkan aurora) | – | 0,3° | – |
| DL5MCG - SV1DH (uji 1987 via Budapest, 1569 km) | Budapest di jejak; sel di bawah 1° mulai 35 km | 2,16° | 95° / 341° |

Scatter point dikenal dalam daftar: Jenewa, Brussel, Budapest, JN92, JN41/42.

Menu **Verifikasi...** memuat pasangan uji IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE dan DL5MCG-SV1DH.

**Perlengkapan** (DL5MCG): F.A.I. di 144 MHz memerlukan setidaknya yagi panjang dan 100 W (SV1DH juga pernah dikontak dengan yagi 8 elemen dan 50 W). Arahkan antena ke scatter point yang ditunjukkan program, bukan ke stasiun lawan.
