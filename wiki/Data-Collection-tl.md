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

**Lokal na filter** (opsiyonal na kahon): itinatapon ang mga linyang "DX de" na mas mababa sa 27990.0 kHz o mas mataas sa 145 MHz, hindi ipinapakita sa console at hindi iniimbak. Ang **di-tiyak na locator** (mas kaunti sa 6 na character) ay kinukuha mula sa CALL3: kung may 6-character na locator ang CALL3, laging ito ang nananaig. Gaya sa YA-MS, ang mga spot na meteor scatter (hindi Sporadic-E) ay ang mga nasa karaniwang frequency window ng MS (50190-50300, 70100-70400, 144290-144399 kHz) na walang tag ng ibang propagasyon (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), at ang mga may mode na pang-MS lamang (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) o may `<MS>`. Hindi ginagamit para sa Sporadic-E ang mga spot na may `<AS>`/`<ACS>` (aircraft scatter).

## CALL3

Database ng JTDX na **callsign → locator** (mahigit isang milyong callsign) sa `data/call3.txt`. Mga button na **I-reload** at **Mag-import ng bagong CALL3.TXT**. Idinadagdag sa `data/call3_nuovi.txt` ang mga 6-character na locator na nabasa sa mga komento ng spot.

Mula sa bersyon 0.4.16, iisa na lang ang database ng CALL3 para sa buong programa, ang sa Pagkolekta ng data ng YA-MS (tab na METEOR SCATTER), na may parehong mga patakaran ng LIVETROPOVHF: pag-update mula sa MMMonVHF, mano-manong entry, idinagdag na file, pagwawasto mula sa cluster, natutunang locator at G7VRD tuwing 30 minuto; isinusulat ang mga pagwawasto sa data/call3.txt at minsang ini-import ang mga locator ng data/call3_nuovi.txt. Dumarating din sa UDP ang mga spot mula sa WSJT-X, JTDX at MSHV (pinagmulang WSJT-X UDP, port 2237 na ipinapasa sa 2238).

Ang mga filter na ito mula sa YA-MS (CW, pinakamaliit na layo, mga window ng frequency, mga mode na MS, mga banda) ay para lang sa tab na METEOR SCATTER: sila ang nagpapasya kung aling mga spot ang papasok sa koleksiyon nito. Natatanggap ng ibang mga tab ang lahat ng spot at gumagamit sila ng sarili nilang mga filter.
