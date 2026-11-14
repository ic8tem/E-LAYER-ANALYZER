# BENCHMARK 50 MHz

[← Home](Home-tr)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Bir istasyonun ve isteğe bağlı olarak ikinci bir istasyonun ADIF loguyla **50 MHz'de çok atlamalı sporadik E** için deney tezgâhı.

## Logların yüklenmesi

- **İstasyon 1**: *ADIF yükle* / *ADIF'i kaldır*. Program `STATION_CALLSIGN` ve `MY_GRIDSQUARE` değerlerini logdan alır; gönderen konum her QSO'nun `MY_GRIDSQUARE` alanıdır.
- **İstasyon 2** (isteğe bağlı): ikinci satırda aynı düğmeler; *İstasyon 2* onay kutusu onu gösterir ya da gizler. İstasyon 1'in yolları açık mavi, istasyon 2'ninkiler mordur.
- Program lokatörlü 50 MHz QSO'larını kullanır (eksik ya da 4 karakterli lokatörler CALL3'ten alınır), çok atlamalı olanları: 3100 km'nin ötesi (yazarın kuralı; atlama sayısı IK3XTV tablosunu izler).
- **Transekvatoryal** bağlantılar hariç tutulur (yazarın kuralı): Afrika ve Güney Amerika yönünde, orta nokta -15° ile +15° arasındaki manyetik eğim kuşağındaysa; logda `PROP_MODE=ES` yoksa.

## Gün ve saat

**Günü** (açılışta en çok çok atlamalı QSO'su olan gün) ve UTC saatini **başlangıç… bitiş…** olarak, 15 dakikalık adımlarla seçin. **foEs** onay kutusu (isteğe bağlı) yolları en düşük foEs'e göre renklendirir ve değeri tabloda gösterir.

## Harita

Serbest merkezli **Mercator** haritası: tekerlek = yakınlaştırma, sürükleme = kaydırma.

![Yol](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Bulanık sarı bir nokta**, Es bulutunun bulunduğu varsayılan her atlamanın merkezini işaretler; farenin altındaki yolda büyür.
- **Fare bir yolun yakınında**: çizgi ve simgeler büyür ve karşı istasyonun **6 karakterli WWL karesi** görünür.
- O yola **sol tık**: bir açılır pencere, uzak ucu o noktanın **150 km yakınında** olan **dosyadaki tüm QSO'ların** (tüm günler) gösterilip gösterilmeyeceğini sorar. *Evet*: harita temizlenir ve bu QSO'ları turuncu dairenin içinde gösterir; *Hayır*: harita olduğu gibi kalır. *Gün ve saate dön* önceki seçime döner.

![Çevre](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Ayrıntılar

Seçilen yol için: mesafe ve IK3XTV tablosuna göre değerlendirme, iki yöndeki azimut, yansıma noktalarının kareleriyle eşit atlamalar, 1200 km içinde kısa ilk atlama (IC8TEM'in deneyimi) ve foEs onay kutusuyla en düşük foEs.
