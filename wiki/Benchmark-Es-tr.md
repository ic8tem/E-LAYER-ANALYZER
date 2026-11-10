# BENCHMARK Es

[← Home](Home-tr)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

**Bütün bir ADIF logu** üzerinde, başka bir istasyonun logu üzerinde bile, **sporadik E** ve **çok atlama** için deney tezgâhı: program gerçekten yapılmış QSO'lardan skip'i ve bulutları gün gün, saat saat çıkarır.

## Logun yüklenmesi

- **ADIF yükle**: `.adi`/`.adif` dosyasını seçer. Program dosyayı hatırlar ve açılışta yeniden yükler.
- **ADIF'i kaldır**: logu sekmeden kaldırır.
- **Merkezdeki istasyon**: logu tutan istasyonun çağrı işareti ve lokatörü. Program en sık geçen `STATION_CALLSIGN` (ya da `OPERATOR`) ve `MY_GRIDSQUARE` değerlerini önerir; bunlar elle değiştirilip **Uygula** ile onaylanabilir. Merkez genel ayarlara bağlı değildir, böylece başka istasyonların logları da incelenebilir.
- Gönderen konum: her QSO'nun `MY_GRIDSQUARE` alanı (yoksa merkezdeki lokatör). Alan konumlar: `GRIDSQUARE` alanları.
- Kullanılan QSO'lar: lokatörlü 28, 50, 70 ve 144 MHz QSO'ları. Başka bir `PROP_MODE` (TR, MS, FAI, AUR, EME...) taşıyan QSO'lar ve 500 km'nin altındakiler hariç tutulabilir. 432 MHz hiçbir zaman dahil edilmez: sporadik E geçerli değildir.

## Günler ve saatler

- **Günlere göre QSO'lar**: gün, QSO sayısı, bantlar, en büyük mesafe, çok atlamalar. Açılışta program en çok QSO'lu günü önerir.
- **Günün saatleri**: *Bütün gün* satırı ve her UTC saati için bir satır; bir tıklama yalnızca o saatin QSO'larını gösterir.
- **QSO**: seçime ait liste; bir QSO'ya (ya da haritada karşı istasyonun yakınına) tıklamak onu vurgular ve ayrıntılarını gösterir.

## Harita

İstasyonu merkez alan, yakınlaştırma için **En büyük mesafe** kaydırıcılı (fare tekerleğiyle de) **azimutal eşuzaklık** haritası: halkalar **merkezden km** değerini gösterir.

- **Sektörlere göre skip** (10° azimut): turuncu yay = sektördeki en yakın QSO (skip bölgesinin kenarı), yeşil yay = en uzak QSO, aradaki alan ulaşılan bölge.
- Tahmini foEs'e göre renklendirilmiş, Maidenhead kareleri üzerinde **Es bulutları**.
- Tek atlamaların orta noktası (sarı), istasyonun ilk atlaması (turuncu; içi boş daire = kısa ilk atlama), karşı istasyonun ilk atlaması (mor).
- **Bulanık sarı bir nokta**, Es bulutunun bulunduğu varsayılan her atlamanın merkezini işaretler (bu varsayım etkinse kısa ilk atlamanın merkezini de).
- Bütün gün için **kırmızı çizgi** orta noktaların ağırlık merkezini saat saat birleştirir: bu, bulutların kaymasıdır.

![Bir saat](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Önerilen hesaplar

1. **Sektörlere göre skip**: her sektör için skip mesafesi ve menzil, çağrı işaretleriyle.
2. Sekant yasasıyla **tahmini foEs** (foEs ≥ f / sec φ): her banttaki en kısa tek atlama en yüksek tahmini verir; bu foEs ile 1000, 1500, 2000 ve 2400 km'de beklenen MUF ve 144 MHz'in açılacağı mesafe.
3. Kareler üzerinde tahmin edilen **Es bulutları**, QSO sayısı ve foEs ile.
4. **Çok atlama**: istasyonun ilk atlaması iki varsayım altında (eşit atlamalar ve 1200 km içinde kısa ilk atlama, IC8TEM'in deneyimi) ±30 dakika içinde tek atlamayla görülen bulutlarla karşılaştırılır; 300 km içinde varsayım doğrulanmış sayılır. Program her varsayımın doğrulamalarını sayar.
5. Saat saat **bulut kayması**: km, yön ve hız.

Hesaplar yol göstericidir: bir QSO yolun açık olduğunu söyler, bulutun tam olarak nerede olduğunu değil. Veriler ne kadar temizse (6 karakterli lokatörler, doğru `PROP_MODE`), skip o kadar güvenilirdir.
