# SPORADIC-E

[← Home](Home-is)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kortið

Hnappur fyrir ofan kortið velur sýnina:

- **Áttarhornskort**: jafnfjarlægðarkort með þína stöð í miðju, með **aðdráttarsleðanum** (frá 300 til 20000 km frá miðju, líka með músarhjólinu).
- **Mercator**: heimskortið, með hnöppunum Evrópa og Heimurinn.

Í báðum vörpunum eru **fjarlægðarhringir Flavio IK3XTV** teiknaðir umhverfis stöðina: 500 km (lágmark staks stökks), 1500-2000 km (kjörfjarlægð), 2400 km (hámark staks stökks), 5000 km (hámark tvöfalds stökks) og 6900 km (hámark þrefalds stökks).

Í áttarhornssýnunum sýna brotnu hringirnir **km frá miðju** (bilið breytist með aðdrætti), jaðarinn markar mestu sýndu fjarlægðina, og músarbendillinn gefur fjarlægð og áttarhorn frá miðju.

![Fjölstökk](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Hvað hann sýnir

- **Spot** síðustu mínútna (15 mín. - 12 klst.) á tíðnisviðunum **28, 40 (8 m, sjálfgefið óvirkt), 50, 70 og 144 MHz**, tekin úr telnet-lotunum. Locatorar koma úr athugasemd spotsins (t.d. `JN70CN<ES>JO62QM`) eða úr CALL3-gagnagrunninum.
- Spotum með **<MS>**, **<AU>** eða **<TR>** í textanum er **hent**: þau varða ekki sporadic-E en eru gagnleg fyrir meteor scatter, norðurljós eða tropo.
- **Þvermiðbaugsútbreiðsla, regla höfundar (IC8TEM)**: í átt að Afríku og Suður-Ameríku, ef miðpunkturinn lendir í beltinu þar sem segulhallinn er milli -15° og +15° (gildi tekið frá Cracknell, UKSMG: TEP-svæði um 5-15° frá núllhallalínunni), er sambandið þvermiðbaugs (TEP) en ekki sporadic-E, nema spotið tilgreini `<ES>`.
- **144 MHz, regla höfundar (IC8TEM)**: sporadic-E á 2 m byrjar **handan 900 km** (styttri spotum er hent). Stakt spot á 144 MHz **er ekki teiknað eitt og sér**: það birtist þegar innan **5 mínútna** berst annað spot á 144 MHz með skýið á sama stað (endurvarpspunktar innan 300 km), eða þegar **stutt stökk á 50 MHz** er í grenndinni (innan 1200 km, á síðustu 15 mínútum). Stikan efst sýnir hve mörg spot á 144 MHz bíða staðfestingar.
- Fyrir hvert spot: fjarlægð, **fjöldi stökka** samkvæmt töflu **IK3XTV**, hæðarhorn, **lágmarks-foEs** (sekantlögmálið).
- **Fyrsta stökk beggja stöðva**: appelsínugulur ferningur = fyrsta stökk spottarans, fjólublár = fyrsta stökk DX (jöfn stökk); gulur = miðpunktur eins stökks.
- **Stutt fyrsta stökk** (holir hringir): í fjölstökksopnunum hefur höfundurinn reynt stutt fyrsta stökk innan **1200 km** frá eigin QTH; lengdina má stilla.
- **Es-ský** á **Maidenhead-reitum** (2° x 1°), lituð eftir lágmarks-foEs: blátt < 5, ljósblátt < 7, grænt < 10, gult < 14, appelsínugult < 20, rautt > 20 MHz.
- Taflan **Es-ský**: fyrir hvern reit, **MUF fyrir þína stöð** (rautt = 144 MHz mögulegt, appelsínugult = 70, gult = 50).

## Tafla IK3XTV

| | lágmark | kjörfjarlægð | hámark |
|---|---|---|---|
| Eitt stökk | 500-650 km | 1500-2000 km | 2200-2400 km |
| Tvöfalt stökk | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Þrefalt stökk | – | 4800-6400 km | 6900 km |

## Hvernig á að nota hann

- **Smelltu á spot**: leiðin er auðkennd, með endurvarpspunktum, locatorum og áttarhornum í smáatriðunum.
- **Smelltu á spot** í töflunni: leiðin lýsist upp á kortinu (gulur bjarmi) og, ef hún er utan sýnar, færist kortið eða radíusinn stækkar til að sýna hana. Spotið helst valið þegar ný spot berast.
- **Taflan fylgir kortinu**: þegar aðdráttur er stilltur sýna spot-taflan og skýjataflan aðeins það sem sést á kortinu.
- **Smelltu á kortið**: punkturinn er meðhöndlaður sem mögulegt Es-ský, með foEs sem þarf fyrir 28, 50, 70 og 144 MHz.

## Athugasemd um 432 MHz

Sporadic-E **á ekki við um 432 MHz**: MUF Es nær um 150-200 MHz og met ARRL telja aðeins Es-sambönd á 50 og 144 MHz. Spot á 70 cm eru ekki greind hér.

## Reitir litaðir eftir tíðni og blikkandi MUF

Maidenhead-reitir endurvarpspunktanna eru litaðir eftir hæstu tíðni spotanna: **grænt** upp í 29 MHz, **gult** upp í 51, **appelsínugult** upp í 71, **rautt** upp í 145 MHz. Út frá foEs reitsins er líkleg MUF á lengsta stökkinu metin (2400 km, tafla IK3XTV): ef hún nær hærra tíðnisviði **blikkar** reiturinn í lit þess tíðnisviðs (meginregla IK3XTV og Jacobs-Leinwoll: stutt skip þýðir háa MUF). Fyrir 144 MHz gildir regla höfundarins: reiturinn blikkar rauður aðeins ef skýið gefur stakt stökk á 50 MHz milli 450 og 500 km (450 km með 50 km vikmörkum).

Efst til hægri á kortinu birtist samantekt N0NBH (E-skip fyrir valda heimsálfu, SFI, K- og A-stuðlar).

## Reglur eftir tíðnisviði (útgáfa 0.4.18)

- **50 MHz**: ekkert sporadic-E innan 450 km. FT8- og FT4-spot eru aðeins sporadic-E ef þau tilheyra skýi: annað spot á 50 MHz innan 4 mínútna með miðpunkt innan 150 km. Stakt FT8/FT4-spot fer í meteor scatter.
- **70 MHz**: ekkert sporadic-E innan 450 km. FT8 og FT4 fara í sporadic-E jafnvel innan meteor scatter-glugga tíðniskipulagsins (þekktur háttur ræður fyrst, síðan tíðnin).
- **40 MHz (8 m)**: lágmarksfjarlægð leidd af 450 km fyrir 50 MHz með sekantlögmálinu (um 330 km); aðeins fjarlægðir innan fyrsta stökks í rúmfræði E-lagsins (um 2440 km). Litur leiðar: gulur.
- **144 MHz**: sporadic-E frá 900 km, með staðfestingu; Q65- og JT65-spot eru EME og er hent, nema `<ES>` sé í textanum.
- G0ISW (Propagation identification, 2024) sér stysta sporadic-E yfir 400 km á 50 og 70 MHz og yfir 1400 km á 144 MHz: reglur höfundarins samræmast á 50 og 70 MHz og eru rýmri á 144 MHz, en krefjast staðfestingar.
- Röðin sem öllum reglunum er beitt í, með gildunum í notkun, sést í **[STILLINGAR](Settings-is)** › Flokkun spota.

## Keðjur með stuttum stökkum (frá útgáfu 0.6.0)

![Keðja frá QTH](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

Í undirflipanum **Fyrirboðar fjölstökks** leitar forritið að fjölstökkskeðjum á 50 MHz samkvæmt þremur skilyrðum höfundar: **fyrsta og síðasta stutta stökk** (500-1200 km) með MUF um 50 MHz, sem krefjast foEs upp á 11-21 MHz, og **miðstökk** allt að 2200 km, sem krefjast um 9,3 MHz.

- **Sterk ský** (appelsínugulir ferningar): Es-spot alls staðar að úr heiminum og GIRO-jónósöndur með foEs að minnsta kosti 11,3 MHz.
- **Frá QTH**: fyrsta stutta stökk frá stöðinni þinni og síðasta stutta stökk á öðru sterku skýi í sömu átt, líka í sömu heimsálfu (til dæmis í Kasakstan); hringurinn er komusvæðið.
- **Gangur**: hvaða tvö sterk ský sem er, í hvaða átt sem er, til dæmis **Karíbahaf - Íberíuskagi** eða **Evrópa - Vestur-Afríka** (TEP undanskilið); «varðar þig» ef annar endinn er innan 1200 km frá þér.
- **Leita að skýi**: aðeins fyrsta stutta stökkið er til staðar; ljósbláu svæðin sýna hvar ský síðasta stutta stökksins ætti að vera.
- **VIÐVÖRUN** ef miðstökkin eru þakin skýjum, **FYRIRBOÐI** ef aðeins vantar svæði án athugenda.
- Í STILLINGAR er **tilkynning** efst (valfrjálst einnig gluggi) fyrir VIÐVARANIR sem varða þig.
- Keðjur sem enda **á opnu hafi** eða þar sem engar stöðvar eru (CALL3 og spot) eru ekki sýndar.
- **Jafnt tvístökk** (frá útgáfu 0.6.3, stökk «2 =»): upp að um 4700 km, til dæmis til Kasakstan, með skýið á fjórðungi leiðarinnar eins og í aðferðinni ITU-R P.534-6.
- Flipinn **STÖKKATAFLA** dregur saman hve mörg stökk þarf á 50 MHz frá 500 til 12000 km, með foEs hvers stökks og heimildunum.
- **Lendingarsvæði merkis míns á 6 m** (bleikir punktar): hvar merkið þitt á 50 MHz lendir eftir 2-5 stökk, yfir skýjum síðustu 30 mínútna sem áætluð eru út frá spotum allra banda (28-144 MHz) og jónósöndum; aðeins þar sem stöðvar eru.
- **Gangur með jöfnum stökkum** (frá útgáfu 0.6.5): tvö ský í 1400-2350 km fjarlægð hvort frá öðru tengja tvö svæði í 2800-4700 km með tveimur jöfnum stökkum; athugunin á leiðangrinum 5U5K staðfestir mörkin um 4700 km.
- **FYRSTA MERKI** (grænt, frá útgáfu 0.6.5): Es-ský á 28 eða 24 MHz þar sem 50 MHz hefur ekki enn opnast; í prófununum opnaðist 50 MHz þar innan klukkustundar í 50% tilvika (30% án þess), 9-16 mínútum fyrr.
- **Endurtekið APRS** (rautt, frá útgáfu 0.6.5): að minnsta kosti tvö APRS-stökk á 2 m yfir 900 km á sama skýi innan 10 mínútna = 144 MHz-VIÐVÖRUN; flipinn blikkar.
- **Hvers virði viðvaranirnar eru** (á stöð höfundar, sumrin 2024-2026): VIÐVARANIR heppnaðar í 46-58% (fjölstökks-QSO í sömu átt innan 2 klukkustunda) á móti 8-10% án viðvörunar; en 43% opnana koma án fyrirvara. Lendingarsvæðið nær innan 500 km frá fjarlægu stöðinni í 13,5% opnana, á móti 2-4% þegar því er snúið.
- Skýin **eru ekki færð**: prófunin frá 22.11.2026 sýnir að það bætir ekki spána á 15-45 mínútum að færa þau með hraða sínum.

![Gangur](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)
