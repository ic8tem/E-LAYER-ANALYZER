# BENCHMARK Es

[← Home](Home-is)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Prófunarbekkur fyrir **sporadic-E** og **fjölstökk** á **heilli ADIF-dagbók**, jafnvel frá annarri stöð: út frá QSO sem raunverulega voru gerð dregur forritið upp skip og ský, dag fyrir dag og klukkustund fyrir klukkustund.

## Hleðsla dagbókarinnar

- **Hlaða ADIF**: velur `.adi`/`.adif`-skrána. Forritið man skrána og hleður henni aftur við ræsingu.
- **Fjarlægja ADIF**: fjarlægir dagbókina af flipanum.
- **Stöð í miðju**: kallmerki og locator stöðvarinnar sem hélt dagbókina. Forritið leggur til algengasta `STATION_CALLSIGN` (eða `OPERATOR`) og `MY_GRIDSQUARE`; þeim má breyta handvirkt og staðfesta með **Nota**. Miðjan er óháð almennu stillingunum, svo hægt er að rannsaka dagbækur annarra stöðva.
- Sendistaður: `MY_GRIDSQUARE` hvers QSO (ef það vantar, locatorinn í miðjunni). Móttökustaðir: reitirnir `GRIDSQUARE`.
- QSO sem notuð eru: 28, 50, 70 og 144 MHz með locator. Hægt er að sleppa QSO með annan `PROP_MODE` (TR, MS, FAI, AUR, EME...) og þeim sem eru undir 500 km. 432 MHz er aldrei tekið með: sporadic-E á ekki við þar.

## Dagar og klukkustundir

- **QSO eftir dögum**: dagur, fjöldi QSO, tíðnisvið, mesta fjarlægð, fjölstökk. Þegar flipinn er opnaður leggur forritið til daginn með flest QSO.
- **Tímar dagsins**: línan *Allur dagurinn* og ein lína fyrir hverja UTC-klukkustund; smellur sýnir aðeins QSO þeirrar klukkustundar.
- **QSO**: listinn fyrir valið; smellur á QSO (eða nálægt mótstöðinni á kortinu) auðkennir það og sýnir smáatriði þess.

## Kortið

**Jafnfjarlægðar-áttarhornskort** með stöðina í miðju, með sleðanum **Hámarksfjarlægð** til aðdráttar (líka með músarhjólinu): hringirnir sýna **km frá miðju**.

- **Skip eftir geirum** (10° áttarhorns): appelsínugulur bogi = næsta QSO í geiranum (jaðar skip-svæðisins), grænn bogi = fjarlægasta QSO, á milli þeirra svæðið sem náðist.
- **Es-ský** á Maidenhead-reitunum, lituð eftir áætluðu foEs.
- Miðpunktur eins stökks (gulur), fyrsta stökk stöðvarinnar (appelsínugulur, holur hringur = stutt fyrsta stökk), fyrsta stökk mótstöðvarinnar (fjólublár).
- **Óskýr gulur depill** merkir miðju hvers stökks, þar sem Es-skýið er talið vera (einnig miðju stutta fyrsta stökksins, þegar sú tilgáta er virk).
- Fyrir allan daginn tengir **rauða línan** þyngdarmiðju miðpunktanna klukkustund fyrir klukkustund: það er rek skýjanna.

![Ein klukkustund](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Tillögur að útreikningum

1. **Skip eftir geirum**: fyrir hvern geira skip-fjarlægðin og drægnin, með kallmerkjunum.
2. **Áætlað foEs** með sekantlögmálinu (foEs ≥ f / sec φ): stysta staka stökkið á hverju tíðnisviði gefur hæsta matið; með þessu foEs, vænt MUF við 1000, 1500, 2000 og 2400 km og fjarlægðin handan sem 144 MHz myndi opnast.
3. **Es-ský** áætluð á reitunum, með fjölda QSO og foEs.
4. **Fjölstökk**: fyrsta stökk stöðvarinnar samkvæmt tveimur tilgátum (jöfn stökk, og stutt fyrsta stökk innan 1200 km, reynsla IC8TEM) borið saman við skýin sem sáust í einu stökki innan ±30 mínútna; innan 300 km er tilgátan staðfest. Forritið telur staðfestingar hvorrar tilgátu.
5. **Rek skýjanna** klukkustund fyrir klukkustund: km, stefna og hraði.

Útreikningarnir eru til viðmiðunar: QSO segir að leiðin hafi verið opin, ekki nákvæmlega hvar skýið var. Því hreinni sem gögnin eru (6 stafa locatorar, réttur `PROP_MODE`), því áreiðanlegra er skipið.
