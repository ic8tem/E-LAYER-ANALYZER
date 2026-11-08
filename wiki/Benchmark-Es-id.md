# BENCHMARK Es

[← Home](Home-id)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Bangku uji untuk **sporadic-E** dan **multi-lompatan** pada **satu log ADIF utuh**, bahkan dari stasiun lain: dari QSO yang benar-benar dilakukan, program menggambarkan skip dan awan, hari demi hari dan jam demi jam.

## Memuat log

- **Muat ADIF**: memilih file `.adi`/`.adif`. Program mengingat file itu dan memuatnya ulang saat mulai.
- **Lepas ADIF**: menghapus log dari tab.
- **Stasiun di tengah**: callsign dan locator stasiun pemilik log. Program mengusulkan `STATION_CALLSIGN` (atau `OPERATOR`) dan `MY_GRIDSQUARE` yang paling sering muncul; keduanya dapat diubah secara manual dan dikonfirmasi dengan **Terapkan**. Pusat peta tidak bergantung pada pengaturan umum, sehingga log stasiun lain pun dapat dipelajari.
- Posisi pemancar: `MY_GRIDSQUARE` setiap QSO (jika tidak ada, locator di tengah). Posisi penerima: field `GRIDSQUARE`.
- QSO yang dipakai: 28, 50, 70 dan 144 MHz dengan locator. QSO dengan `PROP_MODE` lain (TR, MS, FAI, AUR, EME...) dan QSO di bawah 500 km dapat dikecualikan. 432 MHz tidak pernah disertakan: sporadic-E tidak berlaku.

## Hari dan jam

- **QSO per hari**: hari, jumlah QSO, band, jarak maksimum, multi-lompatan. Saat dibuka, program mengusulkan hari dengan QSO terbanyak.
- **Jam dalam sehari**: baris *Sepanjang hari* dan satu baris untuk setiap jam UTC; satu klik hanya menampilkan QSO pada jam itu.
- **QSO**: daftar untuk pilihan tersebut; mengklik sebuah QSO (atau di dekat stasiun DX pada peta) menyorotnya dan menampilkan rinciannya.

## Peta

Peta **azimutal ekuidistan** yang berpusat di stasiun, dengan penggeser **Jarak maksimum** untuk zoom (juga dengan roda mouse): cincin-cincinnya menunjukkan **km dari tengah**.

- **Skip per sektor** (10° azimut): busur oranye = QSO terdekat di sektor (tepi zona skip), busur hijau = QSO terjauh, di antaranya area yang tercapai.
- **Awan Es** pada kotak Maidenhead, diwarnai menurut foEs perkiraan.
- Titik tengah satu lompatan (kuning), lompatan pertama stasiun (oranye, lingkaran kosong = lompatan pertama pendek), lompatan pertama stasiun DX (ungu).
- Untuk sepanjang hari, **garis merah** menghubungkan sentroid titik-titik tengah jam demi jam: itulah pergeseran awan.

![Satu jam](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Perhitungan yang diusulkan

1. **Skip per sektor**: untuk setiap sektor, jarak skip dan jangkauannya, beserta callsign.
2. **Perkiraan foEs** dengan hukum sekan (foEs ≥ f / sec φ): satu lompatan terpendek di setiap band memberikan perkiraan tertinggi; dengan foEs ini, MUF yang diharapkan pada 1000, 1500, 2000 dan 2400 km serta jarak di mana 144 MHz akan terbuka.
3. **Awan Es** yang diperkirakan pada kotak-kotak, dengan jumlah QSO dan foEs.
4. **Multi-lompatan**: lompatan pertama stasiun di bawah dua hipotesis (lompatan sama, dan lompatan pertama pendek dalam 1200 km, pengalaman IC8TEM) dibandingkan dengan awan yang terlihat dalam satu lompatan dalam ±30 menit; dalam 300 km hipotesis dianggap terkonfirmasi. Program menghitung konfirmasi setiap hipotesis.
5. **Pergeseran awan** jam demi jam: km, arah dan kecepatan.

Perhitungan ini bersifat indikatif: sebuah QSO menyatakan bahwa lintasan terbuka, bukan tepatnya di mana awan berada. Makin bersih datanya (locator 6 karakter, `PROP_MODE` yang benar), makin andal skip-nya.
