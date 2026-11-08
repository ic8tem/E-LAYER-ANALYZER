# DATA COLLECTION

[← Home](Home-is)

## Telnet-lotur

| Lota | Sjálfgefið | Athugasemdir |
|---|---|---|
| 1. DX cluster að eigin vali | on0dxk.dyndns.org 8000 | Á ON0DXK eru VHF-spot síuð: `set/filter dxbm/pass 6` (síðan 4, 2). Annað dæmi: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW og RTTY, 7001 = FT8. |
| 3. Klasi með PSKReporter | **dm4x.ddns.net 8500** | Klasi DM4X; einnig má nota HamAlert (hamalert.org 7300, aðgangur nauðsynlegur). |

- Sjálfvirk innskráning með kallmerkinu (efst eða í reitnum Innskráning); **upphafsskipanir** aðskildar með `;`.
- **Tengja aftur** eftir 30 s ef línan rofnar; **Tengja við ræsingu**.
- Sjálfgefið eru aðeins spot frá **28 MHz og upp úr** vistuð.

## Söfnuð spot

Síðustu 2000 spotin, með síum eftir tíðnisviði og lind. Safn: ein CSV-skrá á dag í `data/spot`. Í hvert sinn sem forritinu er lokað er spot-gagnagrunnurinn tæmdur (minnið og skrárnar í `data/spot`): hver lota byrjar frá grunni.

**Staðbundin sía** (valfrjáls reitur): "DX de"-línum undir 27990,0 kHz eða yfir 145 MHz er hent, þær ekki sýndar á skjáborðinu og ekki geymdar. **Ónákvæmir locatorar** (færri en 6 stafir) eru sóttir í CALL3: ef CALL3 hefur 6 stafa locator gengur hann alltaf fyrir. Eins og í YA-MS eru meteor scatter-spot (ekki sporadic-E) þau sem eru í dæmigerðu MS-tíðnigluggunum (50190-50300, 70100-70400, 144290-144399 kHz) án merkja um aðra útbreiðslu (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), og þau sem hafa hætti sem aðeins eru notaðir fyrir MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) eða `<MS>`. Spot með `<AS>`/`<ACS>` (aircraft scatter) eru ekki notuð fyrir sporadic-E.

## CALL3

Gagnagrunnur JTDX fyrir **kallmerki → locator** (yfir milljón kallmerki) í `data/call3.txt`. Hnapparnir **Endurhlaða** og **Flytja inn nýja CALL3.TXT**. 6 stafa locatorum sem lesnir eru úr athugasemdum spota er bætt við `data/call3_nuovi.txt`.
