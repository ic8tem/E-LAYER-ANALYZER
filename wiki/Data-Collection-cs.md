# DATA COLLECTION

[← Home](Home-cs)

## Relace telnet

| Relace | Výchozí | Poznámky |
|---|---|---|
| 1. DX cluster dle vaší volby | on0dxk.dyndns.org 8000 | Na ON0DXK se VHF spoty filtrují: `set/filter dxbm/pass 6` (pak 4, 2). Další příklad: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW a RTTY, 7001 = FT8. |
| 3. Cluster s PSKReporter | **dm4x.ddns.net 8500** | Cluster DM4X; alternativně HamAlert (hamalert.org 7300, vyžaduje účet). |

- Automatické přihlášení značkou (nahoře nebo v poli Login); **počáteční příkazy** oddělené `;`.
- **Opětovné připojení** po 30 s, pokud linka spadne; **Připojit při spuštění**.
- Ve výchozím stavu se ukládají jen spoty **od 28 MHz výše**.

## Sebrané spoty

Posledních 2000 spotů, s filtry podle pásma a zdroje. Archiv: jeden CSV soubor na den v `data/spot`. Při každém zavření programu se databáze spotů vyprázdní (paměť i soubory v `data/spot`): každá relace začíná od nuly.

**Místní filtr** (volitelné políčko): řádky „DX de“ pod 27990,0 kHz nebo nad 145 MHz se zahodí, nezobrazí se v konzoli a neuloží se. **Nepřesné lokátory** (méně než 6 znaků) se berou z CALL3: má-li CALL3 6znakový lokátor, vždy má přednost. Stejně jako v YA-MS jsou spoty meteor scatter (ne Sporadic-E) ty, které leží v typických frekvenčních oknech MS (50190-50300, 70100-70400, 144290-144399 kHz) bez značek jiného šíření (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), a ty s módy výhradně pro MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) nebo s `<MS>`. Spoty s `<AS>`/`<ACS>` (aircraft scatter) se pro Sporadic-E nepoužívají.

## Sloupec filtrů (pravých 40 %)

Sběr dat je **jeden pro celý program**: každý spot projde těmito filtry a pokračuje na kartu příslušného druhu šíření (uvnitř E-LAYER ANALYZER se karta Sběr dat programu YA-MS nezobrazuje). Nahoře zaškrtávací políčko **místního filtru**; pod ním pět podkaret:

| Podkarta | Co nastavujete | Výchozí hodnoty (pravidla autora) |
|---|---|---|
| Meteor scatter | filtr YA-MS: frekvenční okna MS, značky jiného šíření, módy MS a `<MS>`, pásma | jako v YA-MS |
| Sporadic-E | značky `< >`, které spot vyřadí, pravidlo 144 MHz, transekvatoriální pravidlo | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, krátký skok 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | slova F.A.I., pásma, období na mapě | FAI, všechna pásma, 24 hodin |
| Aurora | značky `< >` aurory, pásma, období na mapě | `AU` `AUR` (s `< >` i bez), všechna pásma, 2 hodiny |
| Test filtru | vložte řádek z clusteru a uvidíte, jak ho všechny filtry zařadí | |

Tlačítko **Obnovit pravidla autora** vrátí výchozí hodnoty.

## CALL3

Databáze **značka → lokátor** z JTDX (přes milion značek) v `data/call3.txt`. Tlačítka **Znovu načíst** a **Importovat nový CALL3.TXT**. 6znakové lokátory přečtené v komentářích spotů se přidávají do `data/call3_nuovi.txt`.

## Podkarty Spot a Data o šíření

Karta má dvě podkarty: **Spot** (relace, sebrané spoty, CALL3 a sloupec filtrů) a **Data o šíření**:

- **N0NBH** (hamqsl.com): VHF banner a sluneční data, E-skip Evropa a Severní Amerika (DXrobot), aurora.
- **Bz pro auroru**: sluneční vítr NOAA SWPC v reálném čase (DSCOVR/ACE, bod L1), nejlepší zdroj pro Bz.
- **Meteor scatter**: denní meteorická aktivita (RMOB) z Make More Miles on VHF.

Stahují se nejvýše jednou za hodinu. Souhrn se zobrazuje na mapách SPORADIC-E, F.A.I. a AURORA. **Nabídka kontinentů** vpravo od lišty karet posune mapy a vybere data daného kontinentu.
