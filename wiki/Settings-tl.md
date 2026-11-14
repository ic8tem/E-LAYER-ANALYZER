# MGA SETTING

[← Home](Home-tl)

Mula sa bersiyon 0.4.18, tinitipon ng tab na **MGA SETTING** ang mga pangkalahatang opsiyon ng programa at ang pamamahagi ng mga spot sa mga tab. May dalawa itong sub-tab.

## Pangkalahatan

| Bahagi | Ang itinatakda mo |
|---|---|
| Istasyon | Callsign, locator at taas, ang mga ito rin sa bar sa itaas; kinukumpirma ang mga ito ng **Ilapat** para sa lahat ng tab. |
| Hitsura | Wika, kulay (ang mga scheme ng YAEC at YA-MS at, mula sa bersiyon 0.5.0, ang anim na tema ng IP-TRX-CTRL: Maliwanag, Gabi, Itim, Asul, Lila, Berde) at **laki ng titik ng buong programa** (85-140 %, gaya ng sa YA-MS). Muling binubuo ng wika at kulay ang window: nananatili ang mga setting at ang mga spot. |
| Mga mapa at datos ng pagtataya | Ang kontinente, ang parehong menu ng bar ng mga tab. |
| Oras | Kalagayan ng NTP at ang pagkakaiba ng oras ng server at ng orasan ng PC. |
| Mga update | Pagsusuri sa GitHub sa pagbukas at bawat 6 na oras, awtomatiko at tahimik na update, button na **Suriin ngayon**. Hindi para sa mga package ng sistema (.deb, .rpm, Arch, Slackware) ang awtomatikong update. |
| Datos | Button na nagbubukas ng folder na `data`. Nananatili ang API key ng Bureau of Meteorology sa DATA COLLECTION › Datos ng propagasyon. |

## Pamamahagi ng mga spot

Sa kaliwa, inililista ng isang talahanayan ang mga patakarang ginagamit ng programa para pagpasyahan kung saan pupunta ang bawat spot, **ayon sa pagkakasunod ng paglalapat sa mga ito**: ang unang patakarang tumutugma ang nagpapasya ng tab. Ang mga halagang ipinapakita ay ang mga nakatakda sa mga filter.

| # | Kung ang spot... | pupunta sa |
|---|---|---|
| 1 | ay nasa labas ng lokal na filter (mas mababa sa 27990 kHz o mas mataas sa 145 MHz; 430-440 MHz aurora lamang) | itinapon |
| 2 | ay may AUE, AU-E o AURORAL E | AURORA (aurora-E) |
| 3 | ay may salita ng aurora (default AU, AUR) | AURORA |
| 4 | ay may salita ng F.A.I. (default FAI; mga bandang 6, 4 at 2 m: hindi umaabot sa 432 MHz ang F.A.I.) | F.A.I. |
| 5 | ay may mode ng meteor scatter o `<MS>` | METEOR SCATTER |
| 6 | ay nasa isang MS window ng bandplan (hindi eksklusibo sa 50 MHz; hindi kasama ang FT8 at FT4 sa 50 at 70 MHz) | METEOR SCATTER |
| 7 | ay FT8 o FT4 sa 50 MHz na walang ibang spot sa loob ng 4 na minuto na ang gitnang punto ay nasa loob ng 150 km | meteor scatter (hindi Sporadic-E) |
| 8 | ay Q65 o JT65 sa 144 MHz (maliban kung may `<ES>`) | EME: itinapon |
| 9 | ay may tag ng ibang propagasyon | hindi Sporadic-E |
| 10 | ay trans-equatorial | hindi Sporadic-E (TEP) |
| 11-15 | ay nasa 28, 40, 50, 70 o 144 MHz sa loob ng mga patakaran sa distansiya | SPORADIC-E |
| 16 | walang tugmang patakaran (tropo, 432 MHz, HF...) | walang tab (nananatili sa Mga nakolektang spot) |

Sa kanan ang **lokal na filter** at ang mga filter ng bawat mode, sa limang sub-tab: Meteor scatter (itinatakda ang filter ng YA-MS sa tab na METEOR SCATTER), Sporadic-E, F.A.I., Aurora at **Subok ng mga filter**, kung saan nagdidikit ka ng isang linya ng cluster at nakikita kung paano ito inuuri. Ibinabalik ng button na **Ibalik ang mga patakaran ng may-akda** ang mga default na halaga.
