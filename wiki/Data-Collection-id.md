# DATA COLLECTION

[← Home](Home-id)

## Sesi telnet

| Sesi | Bawaan | Catatan |
|---|---|---|
| 1. DX cluster pilihan | on0dxk.dyndns.org 8000 | Di ON0DXK spot VHF difilter: `set/filter dxbm/pass 6` (lalu 4, 2). Contoh lain: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW dan RTTY, 7001 = FT8. |
| 3. Cluster dengan PSKReporter | **dm4x.ddns.net 8500** | Cluster milik DM4X; sebagai alternatif HamAlert (hamalert.org 7300, perlu akun). |

- Login otomatis dengan callsign (di bagian atas atau di isian Login); **perintah awal** dipisah dengan `;`.
- **Sambung ulang** setelah 30 s jika saluran terputus; **Sambung saat mulai**.
- Secara bawaan hanya spot **dari 28 MHz ke atas** yang disimpan.

## Spot terkumpul

2000 spot terakhir, dengan filter band dan sumber. Arsip: satu file CSV per hari di `data/spot`. Setiap kali program ditutup, basis data spot dikosongkan (memori dan file di `data/spot`): setiap sesi dimulai dari nol.

**Filter lokal** (kotak opsional): baris "DX de" di bawah 27990,0 kHz atau di atas 145 MHz dibuang, tidak ditampilkan di konsol dan tidak disimpan. **Locator yang kurang presisi** (kurang dari 6 karakter) diambil dari CALL3: jika CALL3 memiliki locator 6 karakter, locator itu selalu didahulukan. Seperti di YA-MS, spot meteor scatter (bukan Sporadic-E) adalah spot di jendela frekuensi khas MS (50190-50300, 70100-70400, 144290-144399 kHz) tanpa tag propagasi lain (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), serta spot dengan mode khusus MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) atau dengan `<MS>`. Spot dengan `<AS>`/`<ACS>` (aircraft scatter) tidak dipakai untuk Sporadic-E.

## CALL3

Basis data **callsign → locator** JTDX (lebih dari satu juta callsign) di `data/call3.txt`. Tombol **Muat ulang** dan **Impor CALL3.TXT baru**. Locator 6 karakter yang terbaca di komentar spot ditambahkan ke `data/call3_nuovi.txt`.

Sejak versi 0.4.16 hanya ada satu basis data CALL3 untuk seluruh program, yaitu milik Pengumpulan data YA-MS (tab METEOR SCATTER), dengan aturan yang sama seperti LIVETROPOVHF: pembaruan dari MMMonVHF, entri manual, berkas tambahan, koreksi dari cluster, lokator yang dipelajari dan G7VRD setiap 6 jam; koreksi ditulis ke data/call3.txt dan lokator dari data/call3_nuovi.txt diimpor sekali. Spot dari WSJT-X, JTDX dan MSHV juga datang lewat UDP (sumber WSJT-X UDP, port 2237 diteruskan ke 2238).
