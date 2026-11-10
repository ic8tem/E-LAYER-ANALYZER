# BENCHMARK 50 MHz

[← Home](Home-tl)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Test bench para sa **multi-hop na sporadic-E sa 50 MHz** gamit ang ADIF log ng isang istasyon at, kung gusto, ng ikalawang istasyon.

## Pagkarga ng mga log

- **Istasyon 1**: *Ikarga ang ADIF* / *Alisin ang ADIF*. Kinukuha ng programa ang `STATION_CALLSIGN` at `MY_GRIDSQUARE` mula sa log; ang posisyon ng nagpapadala ay ang `MY_GRIDSQUARE` ng bawat QSO.
- **Istasyon 2** (opsiyonal): parehong mga button sa ikalawang hanay; ipinapakita o itinatago ito ng checkbox na *Istasyon 2*. Mapusyaw na asul ang mga landas ng istasyon 1, lila ang mga landas ng istasyon 2.
- Ginagamit ng programa ang mga QSO sa 50 MHz na may locator (kinukuha mula sa CALL3 ang mga nawawala o 4-character na locator), multi-hop: lampas sa 3100 km (tuntunin ng may-akda; sumusunod sa talahanayan ni IK3XTV ang bilang ng hop).
- Hindi isinasama ang mga **transequatorial** na contact (tuntunin ng may-akda): patungo sa Africa at South America, kapag ang gitnang punto ay nasa banda ng magnetic dip sa pagitan ng -20° at +20°, maliban kung may `PROP_MODE=ES` ang log.

## Araw at oras

Piliin ang **araw** (sa pagbubukas, ang may pinakamaraming multi-hop na QSO) at ang oras **mula… hanggang…** UTC, sa mga hakbang na 15 minuto. Kinukulayan ng checkbox na **foEs** (opsiyonal) ang mga landas ayon sa pinakamababang foEs at ipinapakita ito sa talahanayan.

## Ang mapa

Mapang **Mercator** na malaya ang gitna: gulong = zoom, hilahin = ilipat.

![Landas](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- Minamarkahan ng **malabong dilaw na tuldok** ang gitna ng bawat hop, kung saan ipinapalagay na naroon ang ulap ng Es; lumalaki ito sa landas na nasa ilalim ng mouse.
- **Mouse malapit sa isang landas**: pinalalaki ang linya at mga icon at lumalabas ang **6-character na WWL square** ng DX station.
- **Kaliwang click** sa landas na iyon: may popup na nagtatanong kung ipapakita ang **lahat ng QSO sa file** (lahat ng araw) na ang posisyon ng kabilang dulo ay **nasa loob ng 150 km** mula sa puntong iyon. *Oo*: nililinis ang mapa at ipinapakita ang mga QSO na iyon sa loob ng kahel na bilog; *Hindi*: nananatili ang mapa. Ibinabalik ng *Bumalik sa araw at oras* ang naunang pinili.

![Paligid](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Mga detalye

Para sa napiling landas: distansiya at klasipikasyon ayon sa talahanayan ni IK3XTV, azimuth sa dalawang direksiyon, pantay na hop kasama ang mga grid square ng mga punto ng repleksiyon, maikling unang hop sa loob ng 1200 km (karanasan ni IC8TEM) at, kapag naka-tsek ang foEs, ang pinakamababang foEs.
