# DATA COLLECTION

[← Home](Home-tr)

## Telnet oturumları

| Oturum | Varsayılan | Notlar |
|---|---|---|
| 1. Seçilen DX cluster | on0dxk.dyndns.org 8000 | ON0DXK'de VHF spot'ları filtrelenmiştir: `set/filter dxbm/pass 6` (sonra 4, 2). Başka bir örnek: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW ve RTTY, 7001 = FT8. |
| 3. PSKReporter'lı cluster | **dm4x.ddns.net 8500** | DM4X'in cluster'ı; alternatif olarak HamAlert (hamalert.org 7300, hesap gerekir). |

- Çağrı işaretiyle otomatik oturum açma (en üstte ya da Giriş alanında); `;` ile ayrılmış **başlangıç komutları**.
- Hat düşerse 30 s sonra **Yeniden bağlan**; **Açılışta bağlan**.
- Varsayılan olarak yalnızca **28 MHz ve üstü** spot'lar kaydedilir.

## Toplanan spot'lar

Bant ve kaynak filtreleriyle son 2000 spot. Arşiv: `data/spot` içinde her gün için bir CSV dosyası. Program her kapandığında spot veritabanı boşaltılır (bellek ve `data/spot` içindeki dosyalar): her oturum sıfırdan başlar.

**Yerel filtre** (isteğe bağlı kutu): 27990,0 kHz'in altındaki ya da 145 MHz'in üstündeki "DX de" satırları atılır, konsolda gösterilmez ve saklanmaz. **Belirsiz lokatörler** (6 karakterden az) CALL3'ten alınır: CALL3'te 6 karakterli bir lokatör varsa her zaman o geçerlidir. YA-MS'te olduğu gibi meteor scatter (Sporadic-E değil) spot'ları, tipik MS frekans pencerelerinde (50190-50300, 70100-70400, 144290-144399 kHz) başka bir yayılım etiketi (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) taşımayanlar ile yalnızca MS'e özgü modlarla (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) ya da `<MS>` ile gelenlerdir. `<AS>`/`<ACS>` (aircraft scatter) içeren spot'lar Sporadic-E için kullanılmaz.

## CALL3

`data/call3.txt` içinde JTDX **çağrı işareti → lokatör** veritabanı (bir milyondan fazla çağrı işareti). **Yeniden yükle** ve **Yeni CALL3.TXT içe aktar** düğmeleri. Spot yorumlarında okunan 6 karakterli lokatörler `data/call3_nuovi.txt` dosyasına eklenir.

0.4.16 sürümünden itibaren tüm program için tek bir CALL3 veritabanı vardır: YA-MS Veri toplama'nınki (METEOR SCATTER sekmesi), LIVETROPOVHF ile aynı kurallarla: MMMonVHF'den güncelleme, elle girişler, eklenen dosyalar, küme düzeltmeleri, öğrenilen lokatörler ve 6 saatte bir G7VRD; düzeltmeler data/call3.txt içine yazılır ve data/call3_nuovi.txt lokatörleri bir kez içe aktarılır. WSJT-X, JTDX ve MSHV spotları UDP ile de gelir (kaynak WSJT-X UDP, 2237 portu, 2238'e yönlendirme).
