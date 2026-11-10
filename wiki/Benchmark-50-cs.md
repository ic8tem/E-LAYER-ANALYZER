# BENCHMARK 50 MHz

[← Home](Home-cs)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Zkušební stav pro **vícenásobný skok sporadické vrstvy E na 50 MHz** z ADIF deníku jedné stanice a volitelně i druhé stanice.

## Načtení deníků

- **Stanice 1**: *Načíst ADIF* / *Uvolnit ADIF*. Program převezme z deníku `STATION_CALLSIGN` a `MY_GRIDSQUARE`; vysílací poloha je `MY_GRIDSQUARE` každého QSO.
- **Stanice 2** (volitelná): stejná tlačítka na druhém řádku; zaškrtávací políčko *Stanice 2* ji zobrazí nebo skryje. Trasy stanice 1 jsou světle modré, trasy stanice 2 fialové.
- Program používá QSO na 50 MHz s lokátorem (chybějící nebo 4znakové lokátory se berou z CALL3), s vícenásobným skokem: nad 3100 km (pravidlo autora; počet skoků podle tabulky IK3XTV).
- **Transekvatoriální** spojení jsou vyloučena (pravidlo autora): směrem do Afriky a Jižní Ameriky, se středem trasy v pásmu magnetické inklinace mezi -20° a +20°, pokud deník neuvádí `PROP_MODE=ES`.

## Den a čas

Zvolte **den** (při otevření ten s nejvíce QSO s vícenásobným skokem) a čas **od… do…** UTC, po 15 minutách. Zaškrtávací políčko **foEs** (volitelné) obarví trasy podle minimální foEs a zobrazí ji v tabulce.

## Mapa

**Mercatorova** mapa s volným středem: kolečko = zoom, tažení = posun.

![Path](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Myš u trasy**: čára a ikony se zvětší a objeví se **6znakový čtverec WWL** DX stanice.
- **Levé kliknutí** na tuto trasu: okno se zeptá, zda zobrazit **všechna QSO v souboru** (všechny dny) s polohou protějšího konce **do 150 km** od tohoto bodu. *Ano*: mapa se vymaže a ukáže tato QSO uvnitř oranžového kruhu; *Ne*: mapa zůstane, jak je. *Zpět na den a čas* vrátí předchozí výběr.

![Surroundings](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Podrobnosti

Pro zvolenou trasu: vzdálenost a hodnocení podle tabulky IK3XTV, azimut v obou směrech, stejné skoky se čtverci bodů odrazu, krátký první skok do 1200 km (zkušenost IC8TEM) a se zaškrtnutým foEs minimální foEs.
