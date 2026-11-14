# AURORA

[← Home](Home-tr)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **aurora ovali** (gölgeli), son 3 günün **Kp** değeri, **güneş rüzgârı** (Bz, hız, yoğunluk); 5 dakikada bir güncellenir ve `data/swpc` içinde kaydedilir.
- F.A.I. ile aynı görünümler: bir istasyon çifti için **Scatter trail** ve bir noktayı denetlemek için **İzo-yansıma**; varsayılan yükseklik 110 km.
- **İki istasyon için önerilen anten yönü**: her istasyonun diklik çizgisi boyunca (kendi rengindeki noktalar), oval olasılığının en yüksek olduğu nokta. Pratik kural (G3WPO): kuzey ile doğu arasında, yaklaşık 45°'den başlayın.
- Örnek IO70VJ - JO59BC: scatter point JP01AK, Shetland açıklarında, sapma 0,97°, QTF 11° ve 299°.
- **Haritada aurora spot'ları**: metninde **<AU>** ya da **<AUR>** bulunan spot'lar aurora spot'larıdır (yazarın kuralı). *Haritada hesaplamalı <AU>/<AUR> spot'ları* kutusu işaretliyken sekme son 2 saatin spot'larını çizer (en çok 30): her spotter-DX çifti için scatter trail'i hesaplar ve en olası scatter point'i (turuncu daire) gösterir; sonuçlarda iki istasyonun QTF ve elevasyonu yer alır.
- En olası scatter point'in yanı sıra sonuçlar, iki istasyonun tüm Scatter trail boyunca **QTF aralığını** verir (sapma ≤ 1,5°).
- **Lange-Hesse penceresi**: scatter point tercihen her istasyondan 370-1100 km uzaklıkta aranır; bu, Lange-Hesse'nin 54° jeomanyetik enlemde hesapladığı penceredir. 0.4.5 sürümünden itibaren pencere her istasyonun IGRF-14 eğimiyle kayar: daha güneydeki istasyonlar için daha yakın, daha kuzeydekiler için daha uzak (örneğin Capri yaklaşık 260-990 km, Oslo 470-1200 km). Sonuçlar iki istasyonun penceresini gösterir.

## BENCHMARK AURORA

AURORA sekmesinin iki alt sekmesi vardır: **REAL-TIME** (yukarıda anlatılan hesap) ve **BENCHMARK AURORA**, modelin saat ve lokatörlü **298 gerçek aurora QSO'su** üzerinde bir test tezgâhı: EI5FK'nin yayımlanmış logları (IO51RT, İrlanda, 2003-2005, 144 ve 432 MHz) ve PA0O'nun log haritası (JO33HG, Hollanda).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Program her QSO için scatter point'i aynı modelle hesaplar (IGRF-14, yükseklik 110 km, 4/3 kırılma, 3° tolerans).
- Renkler: **yeşil** = scatter point her iki istasyonun Lange-Hesse penceresi içinde; **sarı** = açıklanmış ama bu pencerenin dışında; **turuncu** = 2000 km'nin ötesinde QSO; **kırmızı** = scatter point yok, QSO model tarafından açıklanmıyor.
- Özet yüzdeleri, karşılaştırma için sabit 370-1100 km penceresiyle de, ve her log için hesaplanan QTF'nin aralığını ve medyanını verir. Bir satıra tıklamak yolu ve iki anten yönünü çizer.
- Sonuç (sürüm 0.4.18, her QSO'nun yılının IGRF-14 alanı): **298 QSO'dan 297'si açıklandı (%100)**, 267'si (%90) her iki istasyonun Lange-Hesse penceresi içinde (sabit 370-1100 km penceresiyle 275, yani %92); EI5FK'nin hesaplanan medyan QTF'si 32° ile 53° arasında, yani G3WPO'nun önerdiği kuzeydoğu. Loglar anten yönünü vermez: denetim QTF üzerinde değil, geometri üzerindedir.
- **Düzeltme**: 0.4.4 sürümünden 0.4.17'ye kadar test tezgâhı IGRF-14 yüklenmeden önce başlıyordu ve dipol ile sabit pencereyle hesaplıyordu; o zaman yayımlanan sonuçlar (243 QSO, %82, medyan QTF 25-41°) dipolündü. 0.4.18'de düzeltildi.
