# F.A.I.

[← Home](Home-tr)

F.A.I. (Field Aligned Irregularities, alan hizalı düzensizlikler), yer manyetik alanı boyunca hizalanmış düzensizliklerden gelen saçılmadır; yaz akşamlarında sporadik E'nin ardından tipiktir. Saçılan sinyal ekseni alan doğrultusunda olan bir **koni** üzerinde yayılır: iki istasyon **tümler konturlar** üzerinde bulunuyorsa birbiriyle çalışabilir (a1 + a2 = 180°, IC8FAX-IC8TEM makalesi; K9SE'nin ±c konturları). Manyetik alan **IGRF-14** modeliyle hesaplanır.

## Scatter trail görünümü (A-B çifti)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- **A istasyonu** ve **B istasyonunu** girin (CALL3'te aranan çağrı işareti ve/veya lokatör). A boş bırakılırsa = kendi istasyonunuz.
- Harita **scatter trail**'i gösterir: katman yüksekliğinde, iki istasyonun da gördüğü ve yansıma koşulunu 2,5° içinde sağlayan tüm noktalar (sınıflar 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Sağ üstteki kutu, A'nın (kırmızı) ve B'nin (yeşil) her noktaya doğru **QTF ve elevasyonunu** gösterir.
- Program **en olası scatter point**'i önerir (en küçük sapma, en kısa yol).
- **Kırılma 4/3**: varsayılan olarak açıktır, ufukları genişletir.

## İzo-yansıma görünümü (izo-yansıma eğrileri)

![İzo-yansıma](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Bu, **bir yansıma noktasının doğru olup olmadığını denetlemek** için kullanılan geometrik yapıdır.

- Haritaya **sol tık**: denetlenecek noktayı seçersiniz. **Sağ tık**: B istasyonunu taşır.
- **Önerilen noktayı doğrula**: grafiği programın önerdiği nokta üzerinde açar.
- **0, ±1, ±2, ±4, ±8, ±16°**'de kontur çizgileri: koyu şemalarda renkli şeritler, açık şemalarda **kalın çizgiler**.
- Çift hakkında hüküm: 1° içinde **tatmin edici**, 2,5° içinde **kabul edilebilir**, ötesinde **tatmin edici değil**.
- Nokta istasyonlardan birinin ufkunun altındaysa yalnızca **Hesaplama yapılamıyor** açılır penceresi çıkar.

## Doğrulamalar

| Bağlantı | Scatter point | Sapma | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (Cenova üzerinde) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (GB Almanya, Brüksel'e 28 km) | 0,88° | 338° / 87° |
| DB0FAI beaconı (305°) - Sheffield üzerinden Dresden (DL5MCG; Dresden 290°'ye yönlendirmişti ve aurora bildirmişti) | – | 0,3° | – |
| DL5MCG - SV1DH (Budapeşte üzerinden 1987 testi, 1569 km) | Budapeşte iz üzerinde; 1°'nin altındaki hücreler 35 km'den itibaren | 2,16° | 95° / 341° |

Listedeki bilinen scatter point'ler: Cenevre, Brüksel, Budapeşte, JN92, JN41/42.

**Doğrulamalar...** menüsü IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE ve DL5MCG-SV1DH test çiftlerini yükler.

**Donanım** (DL5MCG): 144 MHz'de F.A.I. için en az uzun bir yagi ve 100 W gerekir (SV1DH 8 elemanlı bir yagi ve 50 W ile de çalışılmıştır). Anteni karşı istasyona değil, programın verdiği scatter point'e yönlendirin.
