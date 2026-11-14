# DATA COLLECTION

[← Domů](Home-cs)

## Relace telnet

| Relace | Výchozí | Poznámky |
|---|---|---|
| 1. DX cluster dle vlastní volby | on0dxk.dyndns.org 8000 | Na ON0DXK jsou spoty VKV filtrovány: `set/filter dxbm/pass 6` (pak 4, 2). Jiný příklad: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW a RTTY, 7001 = FT8. |
| 3. Cluster s PSKReporter | **dm4x.ddns.net 8500** | Cluster DM4X; alternativně HamAlert (hamalert.org 7300, vyžaduje účet). |

- Automatické přihlášení značkou (nahoře nebo v poli Přihlášení); **úvodní příkazy** oddělené `;`.
- **Opětovné připojení** po 30 s, pokud linka spadne; **Připojit při spuštění**.
- Ve výchozím stavu se ukládají jen spoty **od 28 MHz výše**.

## Shromážděné spoty

Posledních 2000 spotů s filtry podle pásma a zdroje. Archiv: jeden soubor CSV na den v `data/spot`. Při každém zavření programu se databáze spotů vyprázdní (paměť i soubory v `data/spot`): každá relace začíná od nuly.

**Místní filtr** (volitelné pole): řádky „DX de“ pod 27990,0 kHz nebo nad 145 MHz se zahazují, nezobrazují se v konzoli a neukládají se. **Nepřesné lokátory** (méně než 6 znaků) se berou z CALL3: pokud má CALL3 6znakový lokátor, má vždy přednost. Stejně jako v YA-MS jsou spoty meteor scatter (ne sporadické vrstvy E) ty v typických kmitočtových oknech MS (50190-50300, 70100-70400, 144290-144399 kHz) bez tagů jiného šíření (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) a ty s druhy provozu výhradně pro MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) nebo s `<MS>`. Spoty s `<AS>`/`<ACS>` (aircraft scatter) se pro sporadickou vrstvu E nepoužívají.

## Sloupec filtrů (pravých 40 %)

Sběr dat je **jediný pro celý program**: každý spot prochází těmito filtry a dále do karty příslušného druhu šíření (uvnitř E-LAYER ANALYZER se karta Sběr dat programu YA-MS nezobrazuje). Nahoře je zaškrtávací pole **místního filtru**; pod ním pět podkaret:

| Podkarta | Co nastavujete | Výchozí hodnoty (pravidla autora) |
|---|---|---|
| Meteor scatter | filtr YA-MS: kmitočtová okna MS, tagy jiného šíření, druhy provozu MS a `<MS>`, pásma | jako v YA-MS |
| Sporadická vrstva E | tagy `< >`, které spot vylučují, pravidlo 144 MHz, transekvatoriální pravidlo | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, krátký skok 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | slova F.A.I., pásma, období na mapě | FAI, všechna pásma, 24 hodin |
| Aurora | tagy aurory `< >`, pásma, období na mapě | `AU` `AUR` (s `< >` nebo bez), všechna pásma, 2 hodiny |
| Test filtru | vložte řádek clusteru a uvidíte, jak jej všechny filtry klasifikují | |

Tlačítko **Obnovit pravidla autora** vrátí výchozí hodnoty.

## CALL3

Databáze **značka → lokátor** z JTDX (přes milion značek) v `data/call3.txt`. Tlačítka **Znovu načíst** a **Importovat nový CALL3.TXT**. 6znakové lokátory přečtené v komentářích spotů se přidávají do `data/call3_nuovi.txt`.

Od verze 0.4.16 má celý program jedinou databázi CALL3, tu ze Sběru dat YA-MS (karta METEOR SCATTER), se stejnými pravidly jako LIVETROPOVHF: aktualizace z MMMonVHF, ruční záznamy, přidané soubory, opravy z clusteru, naučené lokátory a G7VRD každých 6 hodin; opravy se zapisují do data/call3.txt a lokátory z data/call3_nuovi.txt se jednou importují. Spoty z WSJT-X, JTDX a MSHV přicházejí i přes UDP (zdroj WSJT-X UDP, port 2237 s přeposíláním na 2238).

## Podkarty Spoty a Data o šíření

Karta má dvě podkarty: **Spoty** (relace, shromážděné spoty, CALL3 a sloupec filtrů) a **Data o šíření**:

- **N0NBH** (hamqsl.com): banner VKV a sluneční data, E-skip Evropa a Severní Amerika (DXrobot), aurora.
- **Bz pro auroru**: sluneční vítr v reálném čase od NOAA SWPC (DSCOVR/ACE, bod L1), nejlepší zdroj pro Bz.
- **Meteor scatter**: denní meteorická aktivita (RMOB) z Make More Miles on VHF.

Stahují se nejvýše jednou za hodinu. Souhrn se zobrazuje na mapách SPORADIC-E, F.A.I. a AURORA. **Nabídka kontinentu** vpravo v liště karet přesune mapy a vybere data daného kontinentu.
