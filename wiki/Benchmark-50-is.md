# BENCHMARK 50 MHz

[← Home](Home-is)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Prófunarbekkur fyrir **fjölstökks-sporadic-E á 50 MHz** út frá ADIF-dagbók einnar stöðvar og, ef vill, annarrar stöðvar.

## Hleðsla dagbókanna

- **Stöð 1**: *Hlaða ADIF* / *Fjarlægja ADIF*. Forritið tekur `STATION_CALLSIGN` og `MY_GRIDSQUARE` úr dagbókinni; sendistaðurinn er `MY_GRIDSQUARE` hvers QSO.
- **Stöð 2** (valfrjáls): sömu hnappar í annarri röðinni; gátreiturinn *Stöð 2* sýnir hana eða felur. Leiðir stöðvar 1 eru ljósbláar, leiðir stöðvar 2 fjólubláar.
- Forritið notar QSO á 50 MHz með locator (locatorar sem vantar eða eru 4 stafa eru sóttir í CALL3), fjölstökk: handan 3100 km (regla höfundar; fjöldi stökka fylgir töflu IK3XTV).
- **Þvermiðbaugs**-sambönd eru undanskilin (regla höfundar): í átt að Afríku og Suður-Ameríku, með miðpunktinn í beltinu þar sem segulhallinn er milli -20° og +20°, nema dagbókin hafi `PROP_MODE=ES`.

## Dagur og tími

Veldu **dag** (við opnun sá með flest fjölstökks-QSO) og tíma **frá… til…** UTC, í 15 mínútna skrefum. Gátreiturinn **foEs** (valfrjáls) litar leiðirnar eftir lágmarks-foEs og sýnir það í töflunni.

## Kortið

**Mercator-kort** með frjálsri miðju: hjól = aðdráttur, draga = hliðra.

![Leið](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Mús nálægt leið**: línan og táknin stækka og **6 stafa WWL-reitur** mótstöðvarinnar birtist.
- **Vinstri smellur** á þá leið: sprettigluggi spyr hvort sýna eigi **öll QSO í skránni** (alla daga) þar sem staðsetning hins endans er **innan 150 km** frá þeim punkti. *Já*: kortið er hreinsað og sýnir þau QSO innan appelsínugula hringsins; *Nei*: kortið helst óbreytt. *Aftur í dag og tíma* snýr aftur að fyrra vali.

![Nágrenni](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Smáatriði

Fyrir valda leið: fjarlægð og mat samkvæmt töflu IK3XTV, áttarhorn í báðar áttir, jöfn stökk með reitum endurvarpspunktanna, stutt fyrsta stökk innan 1200 km (reynsla IC8TEM) og, með gátreitnum foEs, lágmarks-foEs.
