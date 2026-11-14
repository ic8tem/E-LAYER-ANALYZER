# NASTAVENÍ

[← Domů](Home-cs)

Od verze 0.4.18 soustřeďuje karta **NASTAVENÍ** obecné volby programu a třídění spotů mezi karty. Má dvě podkarty.

## Obecné

| Sekce | Co nastavujete |
|---|---|
| Stanice | Značka, lokátor a výška, stejné jako v horní liště; **Použít** je potvrdí pro všechny karty. |
| Vzhled | Jazyk, barevné schéma (schémata YAEC a YA-MS a od verze 0.5.0 šest motivů IP-TRX-CTRL: Světlý, Noc, Černý, Modrá, Fialová, Zelená) a **velikost písma celého programu** (85-140 %, jako v YA-MS). Jazyk a barvy znovu sestaví okno: nastavení a spoty zůstanou. |
| Mapy a předpovědní data | Kontinent, stejná nabídka jako v liště karet. |
| Čas | Stav NTP a rozdíl mezi časem serverů a hodinami PC. |
| Aktualizace | Kontrola na GitHubu při spuštění a každých 6 hodin, automatická a tichá aktualizace, tlačítko **Zkontrolovat nyní**. Automatické aktualizace neplatí pro systémové balíčky (.deb, .rpm, Arch, Slackware). |
| Data | Tlačítko, které otevře složku `data`. Klíč API Bureau of Meteorology zůstává v DATA COLLECTION › Data o šíření. |

## Třídění spotů

Vlevo tabulka uvádí pravidla, podle kterých program rozhoduje, kam každý spot jde, **v pořadí, v jakém se uplatňují**: o kartě rozhodne první pravidlo, které platí. Zobrazené hodnoty jsou ty, které jsou nastavené ve filtrech.

| # | Pokud spot... | jde do |
|---|---|---|
| 1 | je mimo místní filtr (pod 27990 kHz nebo nad 145 MHz; 430-440 MHz jen aurora) | zahozen |
| 2 | obsahuje AUE, AU-E nebo AURORAL E | AURORA (aurora-E) |
| 3 | obsahuje slovo aurory (výchozí AU, AUR) | AURORA |
| 4 | obsahuje slovo F.A.I. (výchozí FAI; pásma 6, 4 a 2 m: F.A.I. nedosahuje 432 MHz) | F.A.I. |
| 5 | má druh provozu meteor scatter nebo `<MS>` | METEOR SCATTER |
| 6 | padne do okna MS kmitočtového plánu (na 50 MHz ne výlučné; FT8 a FT4 na 50 a 70 MHz vyloučeny) | METEOR SCATTER |
| 7 | je FT8 nebo FT4 na 50 MHz bez jiného spotu do 4 min se středem trasy do 150 km | meteor scatter (ne sporadická vrstva E) |
| 8 | je Q65 nebo JT65 na 144 MHz (pokud není `<ES>`) | EME: zahozen |
| 9 | obsahuje tag jiného šíření | ne sporadická vrstva E |
| 10 | je transekvatoriální | ne sporadická vrstva E (TEP) |
| 11-15 | je na 28, 40, 50, 70 nebo 144 MHz v mezích pravidel vzdálenosti | SPORADIC-E |
| 16 | nespadá pod žádné pravidlo (tropo, 432 MHz, KV...) | žádná karta (zůstává ve Shromážděných spotech) |

Vpravo je **místní filtr** a filtry jednotlivých druhů šíření v pěti podkartách: Meteor scatter (filtr YA-MS se nastavuje na kartě METEOR SCATTER), Sporadická vrstva E, F.A.I., Aurora a **Test filtru**, kam vložíte řádek clusteru a uvidíte, jak je klasifikován. Tlačítko **Obnovit pravidla autora** vrátí výchozí hodnoty.
