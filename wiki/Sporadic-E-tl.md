# SPORADIC-E

[← Home](Home-tl)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Ang mapa

Pinipili ng isang button sa itaas ng mapa ang tanaw:

- **Azimuthal - iisang hop**: azimuthal equidistant na mapa na nakasentro sa iyong istasyon, nakapirming radius na 2500 km, na may mga sona ng talahanayan ni IK3XTV (500 km pinakamababa, 1500-2000 km pinakamainam, 2400 km pinakamalayo para sa iisang hop).
- **Azimuthal - pinakamalayong distansiya (multi-hop)**: ang parehong mapa na may slider na **Pinakamalayong distansiya** (mula 300 hanggang 20000 km, maaari ring gamit ang gulong ng mouse); lumalabas din ang mga hangganan ng dobleng hop (5000 km) at triple hop (6900 km), kaya masusundan ang pag-unlad ng multi-hop.
- **Parihaba**: ang dating mapa ng mundo, na may mga button na Europa at Mundo.

Sa mga azimuthal na tanaw, ipinapakita ng mga putol-putol na singsing ang **km mula sa gitna** (nagbabago ang pagitan ayon sa zoom), minamarkahan ng gilid ang pinakamalayong distansiyang ipinapakita, at ibinibigay ng cursor ng mouse ang distansiya at azimuth mula sa gitna.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ano ang ipinapakita

- Ang mga **spot** ng huling ilang minuto (15 min - 12 h) sa mga bandang **28, 50, 70 at 144 MHz**, kinuha mula sa mga telnet session. Kinukuha ang mga locator mula sa komento ng spot (hal. `JN70CN<ES>JO62QM`) o mula sa database ng CALL3.
- **Itinatapon** ang mga spot na may **<MS>**, **<AU>** o **<TR>** sa teksto: hindi ito tungkol sa sporadic-E pero kapaki-pakinabang para sa meteor scatter, aurora o tropo.
- **Transequatorial, tuntunin ng may-akda (IC8TEM)**: patungo sa Africa at South America, kung ang gitnang punto ay nasa banda ng magnetic dip sa pagitan ng -20° at +20°, transequatorial (TEP) ang contact at hindi sporadic-E, maliban kung nakasaad sa spot ang `<ES>`.
- **144 MHz, tuntunin ng may-akda (IC8TEM)**: nagsisimula ang sporadic-E sa 2 m **lampas sa 900 km** (itinatapon ang mas maiikling spot). Ang isang spot sa 144 MHz ay **hindi iginuguhit nang mag-isa**: lumalabas ito kapag sa loob ng **5 minuto** ay may dumating na ikalawang spot sa 144 MHz na ang ulap ay nasa parehong lugar (mga punto ng repleksiyon sa loob ng 300 km), o kapag may malapit na spot na **maikling hop sa 50 MHz** (sa loob ng 1200 km, sa huling 15 minuto). Ipinapakita ng bar sa itaas kung ilang spot sa 144 MHz ang naghihintay ng kumpirmasyon.
- Para sa bawat spot: distansiya, **bilang ng hop** ayon sa talahanayan ni **IK3XTV**, elebasyon, **pinakamababang foEs** (secant law).
- **Unang hop ng dalawang istasyon**: kahel na parisukat = unang hop ng spotter, lila = unang hop ng DX (pantay na hop); dilaw = gitnang punto ng iisang hop.
- **Maikling unang hop** (mga bilog na walang laman): sa mga multi-hop na pagbubukas, naranasan ng may-akda ang maikling unang hop sa loob ng **1200 km** mula sa sarili niyang QTH; maaaring itakda ang haba.
- **Mga ulap ng Es** sa **mga Maidenhead grid square** (2° x 1°), kinulayan ayon sa pinakamababang foEs: asul < 5, mapusyaw na asul < 7, berde < 10, dilaw < 14, kahel < 20, pula > 20 MHz.
- Talahanayan ng **mga ulap ng Es**: para sa bawat grid square, ang **MUF para sa iyong istasyon** (pula = posible ang 144 MHz, kahel = 70, dilaw = 50).

## Talahanayan ni IK3XTV

| | pinakamababa | pinakamainam | pinakamalayo |
|---|---|---|---|
| Iisang hop | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dobleng hop | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Triple hop | – | 4800-6400 km | 6900 km |

## Paano gamitin

- **I-click ang isang spot**: naha-highlight ang landas, kasama ang mga punto ng repleksiyon, locator at azimuth sa mga detalye.
- **I-click ang isang spot** sa talahanayan: umiilaw ang landas sa mapa (dilaw na sinag) at, kung wala ito sa tanaw, gumagalaw ang mapa o pinalalaki ang radius nito para maipakita ito. Nananatiling napili ang spot kahit dumating ang mga bagong spot.
- **Sinusundan ng talahanayan ang mapa**: kapag may nakatakdang zoom, ang mga nakikita lamang sa mapa ang ipinapakita ng talahanayan ng mga spot at ng talahanayan ng mga ulap.
- **I-click ang mapa**: itinuturing ang punto bilang posibleng ulap ng Es, kasama ang foEs na kailangan para sa 28, 50, 70 at 144 MHz.

## Tala tungkol sa 432 MHz

**Hindi umiiral sa 432 MHz** ang sporadic-E: umaabot ang MUF ng Es sa mga 150-200 MHz at sa 50 at 144 MHz lamang naglilista ng mga Es contact ang mga rekord ng ARRL. Hindi sinusuri dito ang mga spot sa 70 cm.
