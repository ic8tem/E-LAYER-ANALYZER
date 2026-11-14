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

**Vietinis filtras** (neprivalomas langelis): "DX de" eilutės žemiau 27990,0 kHz arba aukščiau 145 MHz atmetamos, nerodomos konsolėje ir nesaugomos. **Netikslūs lokatoriai** (mažiau nei 6 simboliai) imami iš CALL3: jei CALL3 turi 6 simbolių lokatorių, jis visada turi pirmenybę. Kaip ir YA-MS, meteor scatter (ne sporadinio E) spotai yra tie, kurie patenka į tipinius MS dažnių langus (50190-50300, 70100-70400, 144290-144399 kHz) be kito sklidimo žymų (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), ir tie, kuriuose yra tik MS naudojama moda (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) arba `<MS>`. Spotai su `<AS>`/`<ACS>` (aircraft scatter) sporadiniam E nenaudojami.

## CALL3

JTDX duomenų bazė **šaukinys → lokatorius** (daugiau nei milijonas šaukinių) faile `data/call3.txt`. Mygtukai **Įkelti iš naujo** ir **Importuoti naują CALL3.TXT...**. Spotų komentaruose perskaityti 6 simbolių lokatoriai pridedami į `data/call3_nuovi.txt`.

Nuo 0.4.16 versijos visai programai yra viena CALL3 duomenų bazė – YA-MS Duomenų rinkimo (kortelė METEOR SCATTER), su tomis pačiomis taisyklėmis kaip LIVETROPOVHF: atnaujinimas iš MMMonVHF, rankiniai įrašai, pridėti failai, klasterio pataisos, išmokti lokatoriai ir G7VRD kas 6 valandas; pataisos įrašomos į data/call3.txt, o data/call3_nuovi.txt lokatoriai importuojami vieną kartą. WSJT-X, JTDX ir MSHV spotai ateina ir per UDP (šaltinis WSJT-X UDP, prievadas 2237 su persiuntimu į 2238).
