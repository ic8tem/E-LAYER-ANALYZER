# SPORADIC-E

[← Home](Home-et)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kaart

Kaardi kohal olev nupp valib vaate:

- **Asimuutne - üks hüpe**: sinu jaamale tsentreeritud asimuutne kaugustruu kaart, fikseeritud raadius 2500 km, IK3XTV tabeli tsoonidega (minimaalne 500 km, optimaalne 1500-2000 km, ühe hüppe maksimum 2400 km).
- **Asimuutne - maksimaalne kaugus (mitmehüppeline)**: sama kaart liuguriga **Maksimaalne kaugus** (300 kuni 20000 km, ka hiire rullikuga); näha on ka kahe hüppe (5000 km) ja kolme hüppe (6900 km) piirid, nii et mitmehüppelise levi arengut saab jälgida.
- **Ristkülikuline**: varasem maailmakaart nuppudega Euroopa ja Maailm.

Asimuutsetes vaadetes näitavad katkendlikud ringid **kaugust keskpunktist km-des** (samm muutub koos suurendusega), serv tähistab suurimat näidatavat kaugust ning hiirekursor annab kauguse ja asimuudi keskpunktist.

![Mitmehüppeline levi](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Mida see näitab

- Viimaste minutite **spotid** (15 min - 12 h) sagedusaladel **28, 50, 70 ja 144 MHz**, võetud telneti seanssidest. Lokaatorid võetakse spoti kommentaarist (nt `JN70CN<ES>JO62QM`) või CALL3 andmebaasist.
- Spotid, mille tekstis on **<MS>**, **<AU>** või **<TR>**, **jäetakse kõrvale**: need ei puuduta sporaadilist E-d, kuid on kasulikud meteor scatteri, aurora või tropo jaoks.
- **Transekvatoriaalne levi, autori (IC8TEM) reegel**: Aafrika ja Lõuna-Ameerika suunas, kui keskpunkt jääb magnetilise inklinatsiooni vööndisse -20° kuni +20°, on side transekvatoriaalne (TEP), mitte sporaadiline E, välja arvatud juhul, kui spotis on märgitud `<ES>`.
- **144 MHz, autori (IC8TEM) reegel**: sporaadiline E 2 m sagedusalal algab **üle 900 km** (lühemad spotid jäetakse kõrvale). Üksikut 144 MHz spotti **eraldi ei kuvata**: see ilmub, kui **5 minuti** jooksul saabub teine 144 MHz spot, mille pilv on samas kohas (peegelduspunktid kuni 300 km kaugusel), või kui läheduses on **lühikese hüppega 50 MHz** spot (kuni 1200 km, viimase 15 minuti jooksul). Ülemine riba näitab, mitu 144 MHz spotti ootab kinnitust.
- Iga spoti kohta: kaugus, **hüpete arv** **IK3XTV** tabeli järgi, elevatsioon, **minimaalne foEs** (sekansi seadus).
- **Mõlema jaama esimene hüpe**: oranž ruut = spotteri esimene hüpe, lilla = DX-i esimene hüpe (võrdsed hüpped); kollane = ühe hüppe keskpunkt.
- **Lühike esimene hüpe** (õõnsad ringid): mitmehüppelistel avanemistel on autor kogenud lühikest esimest hüpet kuni **1200 km** kaugusel oma QTH-st; pikkust saab seadistada.
- **Es-pilved** **Maidenheadi ruutudel** (2° x 1°), värvitud minimaalse foEs järgi: sinine < 5, helesinine < 7, roheline < 10, kollane < 14, oranž < 20, punane > 20 MHz.
- Tabel **Es-pilved**: iga ruudu kohta **MUF sinu jaama jaoks** (punane = 144 MHz võimalik, oranž = 70, kollane = 50).

## IK3XTV tabel

| | minimaalne | optimaalne | maksimaalne |
|---|---|---|---|
| Üks hüpe | 500-650 km | 1500-2000 km | 2200-2400 km |
| Kaks hüpet | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Kolm hüpet | – | 4800-6400 km | 6900 km |

## Kuidas kasutada

- **Klõpsa spotil**: trass tõstetakse esile, üksikasjades on peegelduspunktid, lokaatorid ja asimuudid.
- **Klõpsa spotil** tabelis: trass süttib kaardil (kollane kuma) ja kui see on vaateväljast väljas, nihkub kaart või suureneb selle raadius, et trassi näidata. Spot jääb valituks ka uute spottide saabumisel.
- **Tabel järgib kaarti**: kui suurendus on seatud, näitavad spotide ja pilvede tabel ainult seda, mis on kaardil nähtav.
- **Klõpsa kaardil**: punkti käsitletakse võimaliku Es-pilvena ning näidatakse 28, 50, 70 ja 144 MHz jaoks vajalikku foEs-i.

## Märkus 432 MHz kohta

Sporaadiline E **ei kehti 432 MHz kohta**: Es-i MUF ulatub umbes 150-200 MHz-ni ja ARRL-i rekordid loetlevad Es-sidesid ainult 50 ja 144 MHz-l. 70 cm spotte siin ei analüüsita.
