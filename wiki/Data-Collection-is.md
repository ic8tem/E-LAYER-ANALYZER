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

**Staðbundin sía** (valfrjáls reitur, í STILLINGAR): "DX de"-línum undir 27990,0 kHz eða yfir 145 MHz er hent, þær ekki sýndar á skjáborðinu og ekki geymdar; milli 430 og 440 MHz fara aðeins norðurljósaspot í gegn. **Ónákvæmir locatorar** (færri en 6 stafir) eru sóttir í CALL3: ef CALL3 hefur 6 stafa locator gengur hann alltaf fyrir. Eins og í YA-MS eru meteor scatter-spot (ekki sporadic-E) þau sem eru í dæmigerðu MS-tíðnigluggunum (50190-50300, 70100-70400, 144290-144399 kHz) án merkja um aðra útbreiðslu (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), og þau sem hafa hætti sem aðeins eru notaðir fyrir MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) eða `<MS>`. Undantekning: FT8 og FT4 á 50 og 70 MHz, þar sem þekktur háttur ræður (FT8 á 70,154 MHz fer í sporadic-E jafnvel innan MS-gluggans). Spot með `<AS>`/`<ACS>` (aircraft scatter) eru ekki notuð fyrir sporadic-E.

## Síur spota

Gagnasöfnunin er **aðeins ein fyrir allt forritið**: hvert spot fer í gegnum síurnar og á flipa hverrar útbreiðsluaðferðar. Frá útgáfu 0.4.18 eru síurnar, staðbundna sían og prófun sía í flipanum **[STILLINGAR](Settings-is)**, undirflipanum **Flokkun spota**.

## CALL3

Gagnagrunnur JTDX fyrir **kallmerki → locator** (yfir milljón kallmerki) í `data/call3.txt`. Hnapparnir **Endurhlaða** og **Flytja inn nýja CALL3.TXT**. 6 stafa locatorum sem lesnir eru úr athugasemdum spota er bætt við `data/call3_nuovi.txt`.

Frá útgáfu 0.4.16 er einn CALL3-gagnagrunnur fyrir allt forritið, sá úr Gagnasöfnun YA-MS (flipinn METEOR SCATTER), með sömu reglum og LIVETROPOVHF: uppfærsla frá MMMonVHF, handskráningar, viðbættar skrár, leiðréttingar úr klasa, lærðir reitir og G7VRD á 6 klukkustunda fresti; leiðréttingar eru skrifaðar í data/call3.txt og reitirnir úr data/call3_nuovi.txt fluttir inn einu sinni. Spott frá WSJT-X, JTDX og MSHV koma líka um UDP (heimild WSJT-X UDP, gátt 2237 framsend á 2238).

Þessar síur úr YA-MS (CW, lágmarksfjarlægð, tíðnigluggar, MS-hættir, bönd) gilda aðeins um flipann METEOR SCATTER: þær ráða hvaða spott fara í söfnun hans. Hinir fliparnir fá öll spott og nota sínar eigin síur.

## Undirfliparnir Spot og Útbreiðslugögn

Flipinn hefur tvo undirflipa: **Spot** (lotur, PSKReporter MQTT, WSJT-X UDP, söfnuð spot og CALL3) og **Útbreiðslugögn**:

- **N0NBH** (hamqsl.com): VHF-borði og sólargögn, E-skip fyrir Evrópu og Norður-Ameríku (DXrobot), norðurljós.
- **Bz fyrir norðurljós**: sólvindur í rauntíma frá NOAA SWPC (DSCOVR/ACE, punktur L1), besta heimildin fyrir Bz.
- **Meteor scatter**: loftsteinavirkni dagsins (RMOB) frá Make More Miles on VHF.

Þau eru sótt í mesta lagi einu sinni á klukkustund. Samantektin birtist á kortum SPORADIC-E, F.A.I. og AURORA. **Valmynd heimsálfu**, hægra megin í flipastikunni, færir kortin og velur gögn þeirrar heimsálfu.
