# İYONOSONDLAR

[← Home](Home-tr)

![İYONOSONDLAR](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

**SPORADIC-E** alt sekmesi (0.5.2 sürümünden itibaren): dünya genelinde **GIRO** ağının iyonosondlarının **ölçtüğü foEs**, programın spot'lardan elde ettiği değerin yanında.

## Veriler

- İyonogramlardan otomatik olarak okunan son **foEs**, **foF2** ve **MUF(D)** ile 63 GIRO iyonosondu (ULCAR listesi).
- Veriler sekme ilk açıldığında ve ardından **her 15 dakikada bir** kendiliğinden gelir (*Her 15 dakikada bir kendiliğinden güncelle* kutusu); *Şimdi güncelle* onları hemen ister. Arşiv son 48 saati verilerin `giro` klasöründe tutar.
- **GIRO** verileri (Reinisch ve Galkin 2011), **CC BY-NC-SA 4.0** lisansı: yalnızca ticari olmayan kullanım. Servis, DM6AX'in *MUF Muncher* programındakiyle aynıdır.

## Harita ve tablo

- Her iyonosond son foEs'le (Es bulutlarıyla aynı renkler) renklendirilmiş bir nokta ve MHz değeridir; güncel verisi yoksa **gri**, son iyonogram 90 dakikadan eskiyse **halkalıdır**.
- Tablo: Kod, İyonosond, istasyonunuzdan km, son iyonogramın UTC saati, foEs, foF2, MUF(D) ve **Spot foEs≥**: son 30 dakikanın, iyonosonda 500 km (ayarlanabilir) içinde yansıyan Es spot'larından elde edilen en küçük foEs, spot sayısıyla.

## Seçilen iyonosond

Bir satıra ya da haritada bir iyonosondun yakınına tıklayın:

- **son 24 saatin grafiği**: foEs sarı, foF2 açık mavi, MUF(D) noktalı mor;
- 1000, 1500, 2000 ve 2400 km'de **sporadik E MUF'u** (MUF = foEs · sec φ, seçilen Es yüksekliğinde) ve **açılan bantlar** (28, 40, 50, 70, 144 MHz);
- spot'lar ölçülen foEs'ten fazlasını gerektiriyorsa program bunu bildirir: bulut iyonosonddan uzak olabilir ya da iyonosond frekans aralığının sınırına ulaşmıştır (Chartier 2022).
