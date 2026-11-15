# ZAMAN İÇİNDE Es BULUTLARI

[← Home](Home-tr)

![ZAMAN İÇİNDE Es BULUTLARI](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

**SPORADIC-E** alt sekmesi (0.5.2 sürümünden itibaren): **sporadik E bulutlarının** adım adım nasıl doğduğu, yer değiştirdiği ve söndüğü.

## Yöntem

**Chartier, Hanley ve Emmons (2022)** yöntemi: her sporadik E bağlantısı, yansıma noktasında foEs'in **en az f / sec φ** olduğunu söyler. Noktalar hücrelerde ve zaman adımlarında toplanır ve her hücre adımın **en yüksek değerini** alır. En küçük elektron yoğunluğu NmE = 1,24·10¹⁰ · foEs² elektron/metreküptür.

Program yalnızca orta noktayı değil, kendi modelinin yansıma noktalarını (tüm atlamalar) kullanır. Değer bir **alt sınırdır**: boş bir hücre yalnızca bağlantı olmadığı anlamına gelir.

## Kaynak ve denetimler

- **Oturumun spot'ları** (sekme her açıldığında güncellenir) ya da **ADIF logu ya da CSV spot'ları yükle**.
- **Oturumun spot'larını dışa aktar (CSV)**: spot arşivi program kapanırken boşaltılır, böylece onları daha sonra yeniden görebilirsiniz.
- **Bantlar MHz** (28, 40, 50, 70, 144), **Adım** (5, 10 ya da 15 dakika), **Hücre** (1°, 2° ya da 4°), **Es yüksekliği km**.
- Zaman çubuğu: ⏮ ◀ *Başlat/Durdur* ▶ ⏭ ve kaydırıcı; açılışta en çok hücresi olan adımdan başlanır.
- *Önceki adımlar soluk* seçilenden önceki üç adımı gösterir; *Bağlantılar* yolları bant renkleriyle çizer.

## Tablo ve grafik

- Adımın hücreleri: Hücre (güneybatı köşesi ve merkezin lokatörü), foEs≥, NmE≥, Bağlantılar, Bantlar. Bir hücreye tıklayınca bağlantıları vurgulanır.
- Grafik: her adım için en yüksek en küçük foEs (sarı çizgi) ve hücre sayısı (çubuklar).
