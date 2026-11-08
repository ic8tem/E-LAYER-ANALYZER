# BENCHMARK Es

[← Home](Home-tl)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Test bench para sa **sporadic-E** at **multi-hop** sa isang **buong ADIF log**, kahit mula sa ibang istasyon: mula sa mga QSO na talagang nagawa, binabalangkas ng programa ang skip at ang mga ulap, araw-araw at oras-oras.

## Pagkarga ng log

- **Ikarga ang ADIF**: pinipili ang `.adi`/`.adif` file. Tinatandaan ng programa ang file at muli itong ikinakarga sa pagsisimula.
- **Alisin ang ADIF**: inaalis ang log sa tab.
- **Istasyon sa gitna**: callsign at locator ng istasyong gumawa ng log. Iminumungkahi ng programa ang pinakamadalas na `STATION_CALLSIGN` (o `OPERATOR`) at `MY_GRIDSQUARE`; maaaring baguhin ang mga ito nang mano-mano at kumpirmahin gamit ang **Ilapat**. Hindi nakadepende ang gitna sa pangkalahatang setting, kaya mapag-aaralan ang mga log ng ibang istasyon.
- Posisyon ng nagpapadala: ang `MY_GRIDSQUARE` ng bawat QSO (kung wala, ang locator sa gitna). Mga posisyon ng tumatanggap: ang mga field na `GRIDSQUARE`.
- Mga QSO na ginagamit: 28, 50, 70 at 144 MHz na may locator. Maaaring hindi isama ang mga QSO na may ibang `PROP_MODE` (TR, MS, FAI, AUR, EME...) at ang mga mas maikli sa 500 km. Hindi kailanman isinasama ang 432 MHz: hindi umiiral doon ang sporadic-E.

## Mga araw at oras

- **QSO bawat araw**: araw, bilang ng QSO, mga banda, pinakamalayong distansiya, mga multi-hop. Sa pagbubukas, iminumungkahi ng programa ang araw na may pinakamaraming QSO.
- **Mga oras ng araw**: ang hanay na *Buong araw* at isang hanay para sa bawat oras na UTC; ipinapakita ng click ang mga QSO lamang ng oras na iyon.
- **QSO**: ang listahan para sa pinili; ang pag-click sa isang QSO (o malapit sa DX station sa mapa) ay nagha-highlight dito at nagpapakita ng mga detalye nito.

## Ang mapa

**Azimuthal equidistant** na mapa na nakasentro sa istasyon, na may slider na **Pinakamalayong distansiya** para sa zoom (maaari ring gamit ang gulong ng mouse): ipinapakita ng mga singsing ang **km mula sa gitna**.

- **Skip ayon sa sektor** (10° ng azimuth): kahel na arko = pinakamalapit na QSO sa sektor (gilid ng skip zone), berdeng arko = pinakamalayong QSO, sa pagitan nila ang naabot na lugar.
- **Mga ulap ng Es** sa mga Maidenhead grid square, kinulayan ayon sa tinatayang foEs.
- Gitnang punto ng mga iisang hop (dilaw), unang hop ng istasyon (kahel, bilog na walang laman = maikling unang hop), unang hop ng DX station (lila).
- Para sa buong araw, pinag-uugnay ng **pulang linya** ang sentroid ng mga gitnang punto oras-oras: ito ang pag-usad ng mga ulap.

![Isang oras](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Mga mungkahing kalkulasyon

1. **Skip ayon sa sektor**: para sa bawat sektor, ang distansiya ng skip at ang abot, kasama ang mga callsign.
2. **Tinatayang foEs** gamit ang secant law (foEs ≥ f / sec φ): ang pinakamaikling iisang hop sa bawat banda ang nagbibigay ng pinakamataas na tantiya; gamit ang foEs na ito, ang inaasahang MUF sa 1000, 1500, 2000 at 2400 km at ang distansiyang lampas dito ay magbubukas ang 144 MHz.
3. **Mga ulap ng Es** na tinantiya sa mga grid square, kasama ang bilang ng QSO at ang foEs.
4. **Multi-hop**: ang unang hop ng istasyon sa ilalim ng dalawang hinuha (pantay na hop, at maikling unang hop sa loob ng 1200 km, ayon sa karanasan ni IC8TEM) na inihahambing sa mga ulap na nakita sa iisang hop sa loob ng ±30 minuto; sa loob ng 300 km, kumpirmado ang hinuha. Binibilang ng programa ang mga kumpirmasyon ng bawat hinuha.
5. **Pag-usad ng mga ulap** oras-oras: km, direksiyon at bilis.

Pahiwatig lamang ang mga kalkulasyon: sinasabi ng isang QSO na bukas ang landas, hindi kung saan eksakto ang ulap. Kapag mas malinis ang datos (6-character na locator, tamang `PROP_MODE`), mas maaasahan ang skip.
