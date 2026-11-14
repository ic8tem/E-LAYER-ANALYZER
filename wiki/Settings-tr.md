# AYARLAR

[← Home](Home-tr)

0.4.18 sürümünden itibaren **AYARLAR** sekmesi programın genel seçeneklerini ve spot'ların sekmeler arasında yönlendirilmesini toplar. İki alt sekmesi vardır.

## Genel

| Bölüm | Ne ayarlanır |
|---|---|
| İstasyon | Çağrı işareti, lokatör ve yükseklik, üst çubuktakilerle aynı; **Uygula** bunları tüm sekmeler için onaylar. |
| Görünüm | Dil, renk şeması ve **tüm programın yazı tipi boyutu** (%85-140, YA-MS'teki gibi). Dil ve renkler pencereyi yeniden oluşturur: ayarlar ve spot'lar korunur. |
| Haritalar ve tahmin verileri | Kıta, sekme çubuğundaki menünün aynısı. |
| Saat | NTP durumu ve sunucu saati ile bilgisayar saati arasındaki fark. |
| Güncellemeler | Başlangıçta ve her 6 saatte bir GitHub'da denetim, otomatik ve sessiz güncelleme, **Şimdi denetle** düğmesi. Otomatik güncellemeler sistem paketleri (.deb, .rpm, Arch, Slackware) için geçerli değildir. |
| Veriler | `data` klasörünü açan düğme. Bureau of Meteorology API anahtarı DATA COLLECTION › Yayılım verileri bölümünde kalır. |

## Spot yönlendirme

Solda bir tablo, programın her spot'un nereye gideceğine karar verdiği kuralları **uygulandıkları sırayla** listeler: uyan ilk kural sekmeyi belirler. Gösterilen değerler filtrelerde ayarlanmış olanlardır.

| # | Spot... | gider |
|---|---|---|
| 1 | yerel filtrenin dışındaysa (27990 kHz altı ya da 145 MHz üstü; 430-440 MHz'de yalnızca aurora) | atılır |
| 2 | AUE, AU-E ya da AURORAL E içeriyorsa | AURORA (aurora-E) |
| 3 | bir aurora sözcüğü içeriyorsa (başlangıçta AU, AUR) | AURORA |
| 4 | bir F.A.I. sözcüğü içeriyorsa (başlangıçta FAI; bantlar 6, 4 ve 2 m: F.A.I. 432 MHz'e ulaşmaz) | F.A.I. |
| 5 | bir meteor scatter modu ya da `<MS>` içeriyorsa | METEOR SCATTER |
| 6 | bant planının bir MS penceresine düşüyorsa (50 MHz'de dışlayıcı değil; 50 ve 70 MHz'de FT8 ve FT4 hariç) | METEOR SCATTER |
| 7 | 50 MHz'de, 4 dk içinde orta noktası 150 km içinde başka spot olmayan FT8 ya da FT4 ise | meteor scatter (Sporadic-E değil) |
| 8 | 144 MHz'de Q65 ya da JT65 ise (`<ES>` yoksa) | EME: atılır |
| 9 | başka bir yayılım etiketi içeriyorsa | Sporadic-E değil |
| 10 | transekvatoryal ise | Sporadic-E değil (TEP) |
| 11-15 | 28, 40, 50, 70 ya da 144 MHz'de mesafe kuralları içindeyse | SPORADIC-E |
| 16 | hiçbir kurala uymuyorsa (tropo, 432 MHz, HF...) | hiçbir sekme (Toplanan spot'larda kalır) |

Sağda **yerel filtre** ve her modun filtreleri beş alt sekmede yer alır: Meteor scatter (YA-MS filtresi METEOR SCATTER sekmesinde ayarlanır), Sporadic-E, F.A.I., Aurora ve bir cluster satırını yapıştırıp nasıl sınıflandırıldığını gördüğünüz **Filtre testi**. **Yazarın kurallarını geri yükle** düğmesi başlangıç değerlerini geri getirir.
