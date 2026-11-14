# DATA COLLECTION

[← Home](Home-et)

## Telneti seansid

| Seanss | Vaikimisi | Märkused |
|---|---|---|
| 1. Vabalt valitav DX-klaster | on0dxk.dyndns.org 8000 | ON0DXK-l on VHF-spotid filtreeritud: `set/filter dxbm/pass 6` (seejärel 4, 2). Teine näide: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW ja RTTY, 7001 = FT8. |
| 3. Klaster PSKReporteriga | **dm4x.ddns.net 8500** | DM4X-i klaster; alternatiivina HamAlert (hamalert.org 7300, vajalik konto). |

- Automaatne sisselogimine kutsungiga (üleval või väljal Login); **algkäsud** eraldatakse märgiga `;`.
- **Ühenda uuesti** 30 s pärast, kui ühendus katkeb; **Ühenda käivitamisel**.
- Vaikimisi salvestatakse ainult spotid **alates 28 MHz-st**.

## Kogutud spotid

Viimased 2000 spotti, sagedusala ja allika filtritega. Arhiiv: üks CSV-fail päevas kaustas `data/spot`. Iga kord, kui programm suletakse, spotide andmebaas tühjendatakse (mälu ja kausta `data/spot` failid): iga seanss algab nullist.

**Kohalik filter** (valikuline kast): "DX de" read alla 27990,0 kHz või üle 145 MHz jäetakse kõrvale, neid ei näidata konsoolis ega salvestata. **Ebatäpsed lokaatorid** (alla 6 märgi) võetakse CALL3-st: kui CALL3-s on 6-märgiline lokaator, on see alati eelistatud. Nagu YA-MS-is, on meteor scatteri (mitte sporaadilise E) spotid need, mis on tüüpilistes MS-sagedusakendes (50190-50300, 70100-70400, 144290-144399 kHz) ilma muu levi märgenditeta (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), ning need, milles on ainult MS-i jaoks kasutatav moodus (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) või `<MS>`. Spotte märgendiga `<AS>`/`<ACS>` (aircraft scatter) sporaadilise E jaoks ei kasutata.

## CALL3

JTDX-i andmebaas **kutsung → lokaator** (üle miljoni kutsungi) failis `data/call3.txt`. Nupud **Laadi uuesti** ja **Impordi uus CALL3.TXT...**. Spotide kommentaaridest loetud 6-märgilised lokaatorid lisatakse faili `data/call3_nuovi.txt`.

Alates versioonist 0.4.16 on kogu programmil üks CALL3 andmebaas, YA-MS-i Andmekogumise oma (sakk METEOR SCATTER), samade reeglitega nagu LIVETROPOVHF: uuendamine MMMonVHF-ist, käsitsi kirjed, lisatud failid, klastri parandused, õpitud lokaatorid ja G7VRD iga 30 minuti järel; parandused kirjutatakse faili data/call3.txt ja failist data/call3_nuovi.txt imporditakse lokaatorid üks kord. WSJT-X-i, JTDX-i ja MSHV spotid tulevad ka UDP kaudu (allikas WSJT-X UDP, port 2237 edastusega porti 2238).

Need YA-MS-ist pärit filtrid (CW, minimaalne kaugus, sagedusaknad, MS-režiimid, sagedusalad) kehtivad ainult sakile METEOR SCATTER: need otsustavad, millised spotid selle kogusse jõuavad. Teised sakid saavad kõik spotid ja kasutavad oma filtreid.
