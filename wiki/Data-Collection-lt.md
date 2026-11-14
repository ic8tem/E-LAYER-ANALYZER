# DATA COLLECTION

[← Home](Home-lt)

## Telnet seansai

| Seansas | Numatytasis | Pastabos |
|---|---|---|
| 1. Pasirinktas DX klasteris | on0dxk.dyndns.org 8000 | ON0DXK VHF spotai filtruojami: `set/filter dxbm/pass 6` (paskui 4, 2). Kitas pavyzdys: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW ir RTTY, 7001 = FT8. |
| 3. Klasteris su PSKReporter | **dm4x.ddns.net 8500** | DM4X klasteris; alternatyva – HamAlert (hamalert.org 7300, reikalinga paskyra). |

- Automatinis prisijungimas su šaukiniu (viršuje arba lauke Prisijungimas); **pradinės komandos** atskiriamos `;`.
- **Prisijungti iš naujo** po 30 s, jei ryšys nutrūksta; **Prisijungti paleidžiant**.
- Pagal nutylėjimą saugomi tik spotai **nuo 28 MHz ir aukščiau**.

## Surinkti spotai

Paskutiniai 2000 spotų, su juostos ir šaltinio filtrais. Archyvas: vienas CSV failas per dieną aplanke `data/spot`. Kiekvieną kartą uždarant programą spotų duomenų bazė išvaloma (atmintis ir failai aplanke `data/spot`): kiekvienas seansas pradedamas iš naujo.

**Vietinis filtras** (neprivalomas langelis, kortelėje NUSTATYMAI): "DX de" eilutės žemiau 27990,0 kHz arba aukščiau 145 MHz atmetamos, nerodomos konsolėje ir nesaugomos; tarp 430 ir 440 MHz praleidžiami tik pašvaistės spotai. **Netikslūs lokatoriai** (mažiau nei 6 simboliai) imami iš CALL3: jei CALL3 turi 6 simbolių lokatorių, jis visada turi pirmenybę. Kaip ir YA-MS, meteor scatter (ne sporadinio E) spotai yra tie, kurie patenka į tipinius MS dažnių langus (50190-50300, 70100-70400, 144290-144399 kHz) be kito sklidimo žymų (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), ir tie, kuriuose yra tik MS naudojama moda (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) arba `<MS>`. Išimtis: FT8 ir FT4 50 ir 70 MHz juostose, kur sprendžia žinomas režimas (70,154 MHz FT8 priskiriamas sporadiniam E net MS lange). Spotai su `<AS>`/`<ACS>` (aircraft scatter) sporadiniam E nenaudojami.

## Spotų filtrai

Duomenų rinkimas **visai programai yra vienas**: kiekvienas spotas pereina per filtrus ir patenka į kiekvieno sklidimo būdo kortelę. Nuo 0.4.18 versijos filtrai, vietinis filtras ir filtrų bandymas yra kortelėje **[NUSTATYMAI](Settings-lt)**, pakortelėje **Spotų skirstymas**.

## CALL3

JTDX duomenų bazė **šaukinys → lokatorius** (daugiau nei milijonas šaukinių) faile `data/call3.txt`. Mygtukai **Įkelti iš naujo** ir **Importuoti naują CALL3.TXT...**. Spotų komentaruose perskaityti 6 simbolių lokatoriai pridedami į `data/call3_nuovi.txt`.

Nuo 0.4.16 versijos visai programai yra viena CALL3 duomenų bazė – YA-MS Duomenų rinkimo (kortelė METEOR SCATTER), su tomis pačiomis taisyklėmis kaip LIVETROPOVHF: atnaujinimas iš MMMonVHF kas 12 valandų, rankiniai įrašai, pridėti failai, klasterio pataisos, išmokti lokatoriai, G7VRD kas 30 minučių ir, kaip paskutinis šaltinis, JTDX sluoksnis (data/grid_data.bin), niekada neįrašomas į call3.txt; pataisos įrašomos į data/call3.txt, o data/call3_nuovi.txt lokatoriai importuojami vieną kartą. WSJT-X, JTDX ir MSHV spotai ateina ir per UDP (šaltinis WSJT-X UDP, prievadas 2237 su persiuntimu į 2238).

Šie iš YA-MS kilę filtrai (CW, mažiausias atstumas, dažnių langai, MS režimai, juostos) galioja tik kortelei METEOR SCATTER: jie nusprendžia, kurie spotai patenka į jos rinkinį. Kitos kortelės gauna visus spotus ir naudoja savo filtrus.

## Pakortelės Spotai ir Sklidimo duomenys

Kortelė turi dvi pakorteles: **Spotai** (sesijos, PSKReporter MQTT, WSJT-X UDP, surinkti spotai ir CALL3) ir **Sklidimo duomenys**:

- **N0NBH** (hamqsl.com): VHF reklamjuostė ir saulės duomenys, Europos ir Šiaurės Amerikos E-skip (DXrobot), pašvaistė.
- **Bz pašvaistei**: NOAA SWPC saulės vėjas realiuoju laiku (DSCOVR/ACE, taškas L1), geriausias Bz šaltinis.
- **Meteor scatter**: dienos meteorų aktyvumas (RMOB) iš Make More Miles on VHF.

Jie atsisiunčiami ne dažniau kaip kartą per valandą. Santrauka rodoma SPORADIC-E, F.A.I. ir AURORA žemėlapiuose. **Žemyno meniu**, kortelių juostos dešinėje, perkelia žemėlapius ir parenka to žemyno duomenis.
