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

**Yerel filtre** (isteğe bağlı kutu, AYARLAR'da): 27990,0 kHz'in altındaki ya da 145 MHz'in üstündeki "DX de" satırları atılır, konsolda gösterilmez ve saklanmaz; 430 ile 440 MHz arasında yalnızca aurora spot'ları geçer. **Belirsiz lokatörler** (6 karakterden az) CALL3'ten alınır: CALL3'te 6 karakterli bir lokatör varsa her zaman o geçerlidir. YA-MS'te olduğu gibi meteor scatter (Sporadic-E değil) spot'ları, tipik MS frekans pencerelerinde (50190-50300, 70100-70400, 144290-144399 kHz) başka bir yayılım etiketi (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) taşımayanlar ile yalnızca MS'e özgü modlarla (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) ya da `<MS>` ile gelenlerdir. İstisna: 50 ve 70 MHz'de FT8 ve FT4; burada bilinen mod belirler (70,154 MHz'teki FT8, MS penceresinin içinde bile Sporadic-E'ye gider). `<AS>`/`<ACS>` (aircraft scatter) içeren spot'lar Sporadic-E için kullanılmaz.

## Spot filtreleri

Veri toplama **tüm program için tektir**: her spot filtrelerden geçer ve her yayılım türünün sekmesine gider. 0.4.18 sürümünden itibaren filtreler, yerel filtre ve filtre testi **[AYARLAR](Settings-tr)** sekmesinde, **Spot yönlendirme** alt sekmesindedir.

## CALL3

`data/call3.txt` içinde JTDX **çağrı işareti → lokatör** veritabanı (bir milyondan fazla çağrı işareti). **Yeniden yükle** ve **Yeni CALL3.TXT içe aktar** düğmeleri. Spot yorumlarında okunan 6 karakterli lokatörler `data/call3_nuovi.txt` dosyasına eklenir.

0.4.16 sürümünden itibaren tüm program için tek bir CALL3 veritabanı vardır: YA-MS Veri toplama'nınki (METEOR SCATTER sekmesi), LIVETROPOVHF ile aynı kurallarla: 12 saatte bir MMMonVHF'den güncelleme, elle girişler, eklenen dosyalar, küme düzeltmeleri, öğrenilen lokatörler, 30 dakikada bir G7VRD ve son kaynak olarak, call3.txt'ye asla yazılmayan JTDX katmanı (data/grid_data.bin); düzeltmeler data/call3.txt içine yazılır ve data/call3_nuovi.txt lokatörleri bir kez içe aktarılır. WSJT-X, JTDX ve MSHV spotları UDP ile de gelir (kaynak WSJT-X UDP, 2237 portu, 2238'e yönlendirme).

YA-MS'ten gelen bu filtreler (CW, en az mesafe, frekans pencereleri, MS modları, bantlar) yalnızca METEOR SCATTER sekmesi için geçerlidir: hangi spotların onun toplamasına gireceğine karar verirler. Diğer sekmeler tüm spotları alır ve kendi filtrelerini kullanır.

## Spot ve Yayılım verileri alt sekmeleri

Sekmenin iki alt sekmesi vardır: **Spot'lar** (oturumlar, PSKReporter MQTT, WSJT-X UDP, toplanan spot'lar ve CALL3) ve **Yayılım verileri**:

- **N0NBH** (hamqsl.com): VHF afişi ve güneş verileri, Avrupa ve Kuzey Amerika E-skip'i (DXrobot), aurora.
- **Aurora için Bz**: NOAA SWPC gerçek zamanlı güneş rüzgârı (DSCOVR/ACE, L1 noktası), Bz için en iyi kaynak.
- **Meteor scatter**: Make More Miles on VHF'den günlük meteor etkinliği (RMOB).

En fazla saatte bir indirilirler. Özet SPORADIC-E, F.A.I. ve AURORA haritalarında görünür. Sekme çubuğunun sağındaki **kıta menüsü** haritaları kaydırır ve o kıtanın verilerini seçer.
