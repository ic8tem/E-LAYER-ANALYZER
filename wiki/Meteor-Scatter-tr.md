# METEOR SCATTER

[← Home](Home-tr)

Bu sekme, IC8TEM'in meteor scatter programı **YA-MS 1.8.0**'in tamamını içerir: radyantlarla gök küresi, SKiYMET radar akısı (Collm, Sodankylä), yağmur verimi hesabı, tahminlerin deneysel uygulaması, geometri ve zayıflama, GRAVES, kare haritası, **Canlı yağmurlar** (Global Meteor Network'ün ölçtüğü ZHR ve CMOR radarından radyantlar), **Kaynaklar haritası** (Collm, Sodankylä, GRAVES ve CMOR'un istasyona göre konumu) ve formülleri içeren **Verim - açıklama** belgesi.

YA-MS 1.5.0, **Veri toplama** sekmesini ekler (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>` ile filtrelenmiş meteor scatter spot'ları). E-LAYER ANALYZER içinde kendi telnet oturumlarını açmaz: DATA COLLECTION oturumlarının satırlarını alır, böylece aynı çağrı işaretiyle çift oturum açılmaz.

YA-MS 1.6.0 sekmeleri **Canlı ve gerçek zamanlı veriler** (her bağlantının bulutuyla canlı meteor scatter trafiği, radar akısı, GRAVES, veri toplama) ve **Tahmin hesabı** (tahmin, geometri, verim hesabı, tahminlerin uygulanması) gruplarında yeniden düzenler.

YA-MS 1.7.0 ve 1.8.0; radyantların IAU MDC kataloğuyla karşılaştırılmasını, Global Meteor Network'ün yağmur başına günlük sayımlarını, popülasyon indekslerinden elde edilen k indeksini, Stober ve diğerlerinin (2021) başlangıç iz yarıçapını, bantlar arası ping süresini (varsayılan olarak λ² ya da NN4SA'nın HamSCI MSQP ölçümleri) ve Sonneberg'deki BRAMS tespitleriyle karşılaştırmayı (Perseidler 2026) ekler. YA-MS'in otomatik güncellemesi, kendini GitHub Releases'ten güncelleyen E-LAYER ANALYZER içinde kapalıdır.

- YA-MS, E-LAYER ANALYZER'ın üst kısmında seçilen dili ve renk şemasını kullanır.
- YA-MS ayarları `data/config.ini` içinde saklanır.
- YA-MS'in her yeni sürümü `tools/sync_yams.py` betiğiyle aktarılır.

Ayrıntılar için [YA-MS deposuna](https://github.com/ic8tem/YA-MS) bakın.
