# SPORADIC-E

[← Home](Home-et)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kaart

Kaardi kohal olev nupp valib vaate:

- **Asimuutne**: sinu jaamale tsentreeritud asimuutne kaugustruu kaart **suurenduse** liuguriga (300 kuni 20000 km keskpunktist, ka hiirerattaga).
- **Mercator**: maailmakaart nuppudega Euroopa ja Maailm.

Mõlemas projektsioonis joonistatakse jaama ümber **Flavio IK3XTV kaugusringid**: 500 km (ühe hüppe miinimum), 1500-2000 km (optimum), 2400 km (ühe hüppe maksimum), 5000 km (kahe hüppe maksimum) ja 6900 km (kolme hüppe maksimum).

Asimuutsetes vaadetes näitavad katkendlikud ringid **kaugust keskpunktist km-des** (samm muutub koos suurendusega), serv tähistab suurimat näidatavat kaugust ning hiirekursor annab kauguse ja asimuudi keskpunktist.

![Mitmehüppeline levi](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Mida see näitab

- Viimaste minutite **spotid** (15 min - 12 h) sagedusaladel **28, 40 (8 m, vaikimisi väljas), 50, 70 ja 144 MHz**, võetud telneti seanssidest. Lokaatorid võetakse spoti kommentaarist (nt `JN70CN<ES>JO62QM`) või CALL3 andmebaasist.
- Spotid, mille tekstis on **<MS>**, **<AU>** või **<TR>**, **jäetakse kõrvale**: need ei puuduta sporaadilist E-d, kuid on kasulikud meteor scatteri, aurora või tropo jaoks.
- **Transekvatoriaalne levi, autori (IC8TEM) reegel**: Aafrika ja Lõuna-Ameerika suunas, kui keskpunkt jääb magnetilise inklinatsiooni vööndisse -15° kuni +15° (väärtus võetud Cracknellilt, UKSMG: TEP-i tsoonid umbes 5-15° nullinklinatsiooni joonest), on side transekvatoriaalne (TEP), mitte sporaadiline E, välja arvatud juhul, kui spotis on märgitud `<ES>`.
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

## Sageduse järgi värvitud ruudud ja vilkuv MUF

Peegelduspunktide Maidenheadi ruudud värvitakse spottide kõrgeima sageduse järgi: **roheline** kuni 29 MHz, **kollane** kuni 51, **oranž** kuni 71, **punane** kuni 145 MHz. Ruudu foEs-ist hinnatakse tõenäoline MUF pikimal hüppel (2400 km, IK3XTV tabel): kui see ulatub kõrgemale sagedusalale, siis ruut **vilgub** selle sagedusala värviga (IK3XTV ja Jacobs-Leinwolli põhimõte: lühike skip tähendab kõrget MUF-i). 144 MHz puhul kehtib autori reegel: ruut vilgub punaselt ainult siis, kui pilv annab 50 MHz ühe hüppe 450 ja 500 km vahel (450 km 50 km tolerantsiga).

Kaardi paremas ülanurgas kuvatakse N0NBH kokkuvõte (valitud kontinendi E-skip, SFI, K- ja A-indeks).

## Reeglid sagedusalade kaupa (versioon 0.4.18)

- **50 MHz**: 450 km piires sporaadilist E-d ei ole. FT8 ja FT4 spotid on sporaadiline E ainult siis, kui need kuuluvad pilve: teine 50 MHz spot 4 minuti jooksul keskpunktiga 150 km piires. Üksik FT8/FT4 spot läheb meteoorhajumise alla.
- **70 MHz**: 450 km piires sporaadilist E-d ei ole. FT8 ja FT4 lähevad sporaadilise E alla ka sagedusplaani meteoorhajumise aknas (esmalt otsustab teadaolev viis, seejärel sagedus).
- **40 MHz (8 m)**: minimaalne kaugus tuletatud 50 MHz 450 km-st sekantsiseadusega (umbes 330 km); ainult kaugused E-kihi geomeetria esimese hüppe piires (umbes 2440 km). Trassi värv: kollane.
- **144 MHz**: sporaadiline E alates 900 km-st, kinnitusega; Q65 ja JT65 spotid on EME ja jäetakse kõrvale, välja arvatud `<ES>` tekstis.
- G0ISW (Propagation identification, 2024) näeb lühimat sporaadilist E-d üle 400 km 50 ja 70 MHz-l ning üle 1400 km 144 MHz-l: autori reeglid on 50 ja 70 MHz-l kooskõlas ja 144 MHz-l laiemad, kuid nõuavad kinnitust.
- Kõigi reeglite rakendamise järjekord koos kasutusel olevate väärtustega on näha sakil **[SEADED](Settings-et)** › Spottide suunamine.

## Lühikeste hüpetega ahelad (alates versioonist 0.6.0)

![Ahel QTH-st](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

Alamsakis **Mitmehüppelise levi eelkäijad** otsib programm 50 MHz mitmehüppelisi ahelaid autori kolme tingimuse järgi: **esimene ja viimane lühike hüpe** (500-1200 km) MUF-iga umbes 50 MHz, mis nõuavad foEs-i 11-21 MHz, ning **keskmised hüpped** kuni 2200 km, mis nõuavad umbes 9,3 MHz.

- **Tugevad pilved** (oranžid ruudud): kogu maailma Es-spotid ja GIRO ionosondid foEs-iga vähemalt 11,3 MHz.
- **QTH-st**: esimene lühike hüpe sinu jaamast ja viimane lühike hüpe teisele tugevale pilvele samas suunas, ka samal mandril (näiteks Kasahstanis); ring on saabumistsoon.
- **Koridor**: mis tahes kaks tugevat pilve, igas suunas, näiteks **Kariibi meri - Pürenee poolsaar** või **Euroopa - Lääne-Aafrika** (TEP välja arvatud); «puudutab sind», kui üks ots on sinust 1200 km piires.
- **Otsi pilve**: on ainult esimene lühike hüpe; helesinised tsoonid näitavad, kus peaks olema viimase lühikese hüppe pilv.
- **HOIATUS**, kui keskmised hüpped on pilvedega kaetud, **EELKÄIJA**, kui puuduvad ainult vaatlejateta tsoonid.
- SEADETES on üleval **teade** (soovi korral ka aken) sind puudutavate HOIATUSTE kohta.
- Ahelaid, mis jõuavad **avamerele** või sinna, kus jaamu pole (CALL3 ja spotid), ei näidata.
- **Võrdne topelthüpe** (alates versioonist 0.6.3, hüpped «2 =»): kuni umbes 4700 km, näiteks Kasahstani suunas, pilvega teekonna veerandil nagu meetodis ITU-R P.534-6.
- Sakk **HÜPETE TABEL** võtab kokku, mitu hüpet on 50 MHz-l vaja 500 kuni 12000 km jaoks, iga hüppe foEs-i ja allikatega.
- **Minu signaali leviala 6 m-l** (roosad punktid): kuhu sinu 50 MHz signaal jõuab pärast 2-5 hüpet, üle viimase 30 minuti pilvede, mis on hinnatud kõigi sagedusalade (28-144 MHz) spottidest ja ionosondidelt; ainult seal, kus on jaamu.
- **Võrdsete hüpetega koridor** (alates versioonist 0.6.5): kaks teineteisest 1400-2350 km kaugusel olevat pilve ühendavad kaks 2800-4700 km kaugusel olevat tsooni kahe võrdse hüppega; kontroll ekspeditsiooni 5U5K peal kinnitab umbes 4700 km piiri.
- **VARAJANE MÄRK** (roheline, alates versioonist 0.6.5): Es-pilv 28 või 24 MHz-l, kus 50 MHz pole veel avanenud; katsetes avanes 50 MHz seal tunni jooksul 50% juhtudest (ilma selleta 30%), 9-16 minutit varem.
- **Korduv APRS** (punane, alates versioonist 0.6.5): vähemalt kaks APRS-hüpet 2 m-l üle 900 km samal pilvel 10 minuti jooksul = 144 MHz HOIATUS; sakk vilgub.
- **Mida hoiatused väärt on** (autori jaamal, suved 2024-2026): HOIATUSED õnnestuvad 46-58% (mitmehüppeline QSO samas suunas 2 tunni jooksul) võrreldes 8-10%-ga ilma hoiatuseta; kuid 43% avanemistest saabub ette teatamata. Leviala jõuab kaugest jaamast 500 km piiresse 13,5% avanemistest, pööratuna 2-4%.
- Pilvi **ei nihutata**: 22.11.2026 katse näitab, et nende nihutamine oma kiirusega ei paranda 15-45 minuti prognoosi.

![Koridor](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)
