# DATA COLLECTION

[← Home](Home-tl)

## Mga telnet session

| Session | Default | Mga tala |
|---|---|---|
| 1. DX cluster na iyong pinili | on0dxk.dyndns.org 8000 | Sa ON0DXK, naka-filter ang mga spot sa VHF: `set/filter dxbm/pass 6` (pagkatapos ay 4, 2). Isa pang halimbawa: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW at RTTY, 7001 = FT8. |
| 3. Cluster na may PSKReporter | **dm4x.ddns.net 8500** | Cluster ni DM4X; bilang alternatibo, HamAlert (hamalert.org 7300, kailangan ng account). |

- Awtomatikong login gamit ang callsign (sa itaas o sa field na Login); **mga panimulang command** na hinihiwalay ng `;`.
- **Kumonekta muli** pagkalipas ng 30 s kapag naputol ang linya; **Kumonekta sa pagsisimula**.
- Bilang default, ang mga spot lamang mula **28 MHz pataas** ang sine-save.

## Mga nakolektang spot

Ang huling 2000 spot, may filter ayon sa banda at sanggunian. Archive: isang CSV file bawat araw sa `data/spot`. Sa bawat pagsasara ng programa, inaalisan ng laman ang database ng mga spot (ang memorya at ang mga file sa `data/spot`): nagsisimula mula sa wala ang bawat session.

**Lokal na filter** (opsiyonal na kahon, sa MGA SETTING): itinatapon ang mga linyang "DX de" na mas mababa sa 27990.0 kHz o mas mataas sa 145 MHz, hindi ipinapakita sa console at hindi iniimbak; sa pagitan ng 430 at 440 MHz, mga spot ng aurora lamang ang pinapadaan. Ang **hindi tiyak na mga locator** (kulang sa 6 na karakter) ay kinukuha sa CALL3: kung may locator na 6 na karakter ang CALL3, iyon ang laging nananaig. Gaya ng sa YA-MS, mga spot ng meteor scatter (hindi Sporadic-E) ang mga nasa karaniwang frequency window ng MS (50190-50300, 70100-70400, 144290-144399 kHz) na walang tag ng ibang propagasyon (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), at ang mga may mode na pang-MS lamang (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) o may `<MS>`. Eksepsiyon: FT8 at FT4 sa 50 at 70 MHz, kung saan ang kilalang mode ang nagpapasya (napupunta sa Sporadic-E ang FT8 sa 70.154 MHz kahit nasa loob ng MS window). Hindi ginagamit sa Sporadic-E ang mga spot na may `<AS>`/`<ACS>` (aircraft scatter).

## Mga filter ng spot

**Iisa lang ang pangongolekta ng data** para sa buong programa: bawat spot ay dumaraan sa mga filter at pumupunta sa tab ng bawat mode ng propagasyon. Mula sa bersiyon 0.4.18, ang mga filter, ang lokal na filter at ang subok ng mga filter ay nasa tab na **[MGA SETTING](Settings-tl)**, sub-tab na **Pamamahagi ng mga spot**.

## CALL3

Database ng JTDX na **callsign → locator** (mahigit isang milyong callsign) sa `data/call3.txt`. Mga button na **I-reload** at **Mag-import ng bagong CALL3.TXT**. Idinadagdag sa `data/call3_nuovi.txt` ang mga 6-character na locator na nabasa sa mga komento ng spot.

Mula sa bersyon 0.4.16, iisa na lang ang database ng CALL3 para sa buong programa, ang sa Pagkolekta ng data ng YA-MS (tab na METEOR SCATTER), na may parehong mga patakaran ng LIVETROPOVHF: pag-update mula sa MMMonVHF, mano-manong entry, idinagdag na file, pagwawasto mula sa cluster, natutunang locator at G7VRD tuwing 6 na oras; isinusulat ang mga pagwawasto sa data/call3.txt at minsang ini-import ang mga locator ng data/call3_nuovi.txt. Dumarating din sa UDP ang mga spot mula sa WSJT-X, JTDX at MSHV (pinagmulang WSJT-X UDP, port 2237 na ipinapasa sa 2238).

Ang mga filter na ito mula sa YA-MS (CW, pinakamaliit na layo, mga window ng frequency, mga mode na MS, mga banda) ay para lang sa tab na METEOR SCATTER: sila ang nagpapasya kung aling mga spot ang papasok sa koleksiyon nito. Natatanggap ng ibang mga tab ang lahat ng spot at gumagamit sila ng sarili nilang mga filter.

## Mga sub-tab na Spot at Datos ng propagasyon

May dalawang sub-tab ang tab: **Spot** (mga session, PSKReporter MQTT, WSJT-X UDP, mga nakolektang spot at CALL3) at **Datos ng propagasyon**:

- **N0NBH** (hamqsl.com): VHF banner at datos ng araw, E-skip ng Europa at Hilagang Amerika (DXrobot), aurora.
- **Bz para sa aurora**: real-time na solar wind ng NOAA SWPC (DSCOVR/ACE, punto L1), ang pinakamahusay na pinagmulan ng Bz.
- **Meteor scatter**: araw-araw na aktibidad ng meteor (RMOB) mula sa Make More Miles on VHF.

Dina-download ang mga ito nang hindi hihigit sa isang beses bawat oras. Lumalabas ang buod sa mga mapa ng SPORADIC-E, F.A.I. at AURORA. Inililipat ng **menu ng kontinente**, sa kanan ng bar ng mga tab, ang mga mapa at pinipili ang datos ng kontinenteng iyon.
