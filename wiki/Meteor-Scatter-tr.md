# METEOR SCATTER

[← Home](Home-tr)

Bu sekme, IC8TEM'in meteor scatter programı **YA-MS 1.5.1**'in tamamını içerir: radyantlarla gök küresi, SKiYMET radar akısı (Collm, Sodankylä), yağmur verimi hesabı, tahminlerin deneysel uygulaması, geometri ve zayıflama, GRAVES, kare haritası, **Canlı yağmurlar** (Global Meteor Network'ün ölçtüğü ZHR ve CMOR radarından radyantlar), **Kaynaklar haritası** (Collm, Sodankylä, GRAVES ve CMOR'un istasyona göre konumu) ve formülleri içeren **Verim - açıklama** belgesi.

YA-MS 1.5.0, **Veri toplama** sekmesini ekler (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>` ile filtrelenmiş meteor scatter spot'ları). E-LAYER ANALYZER içinde kendi telnet oturumlarını açmaz: DATA COLLECTION oturumlarının satırlarını alır, böylece aynı çağrı işaretiyle çift oturum açılmaz.

- YA-MS, E-LAYER ANALYZER'ın üst kısmında seçilen dili ve renk şemasını kullanır.
- YA-MS ayarları `data/config.ini` içinde saklanır.
- YA-MS'in her yeni sürümü `tools/sync_yams.py` betiğiyle aktarılır.

Ayrıntılar için [YA-MS deposuna](https://github.com/ic8tem/YA-MS) bakın.
