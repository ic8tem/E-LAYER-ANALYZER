# BENCHMARK Es

[← Domů](Home-cs)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Testovací lavice pro **sporadickou vrstvu E** a **vícenásobný skok** na **celém deníku ADIF**, i z jiné stanice: ze skutečně navázaných QSO program načrtne skip a oblaka, den po dni a hodinu po hodině.

## Načtení deníku

- **Načíst ADIF**: vybere soubor `.adi`/`.adif`. Program si soubor zapamatuje a při spuštění jej znovu načte.
- **Uvolnit ADIF**: odebere deník z karty.
- **Stanice ve středu**: značka a lokátor stanice, která deník vytvořila. Program nabídne nejčastější `STATION_CALLSIGN` (nebo `OPERATOR`) a `MY_GRIDSQUARE`; lze je ručně změnit a potvrdit tlačítkem **Použít**. Střed nezávisí na obecném nastavení, takže lze studovat deníky jiných stanic.
- Poloha vysílání: `MY_GRIDSQUARE` každého QSO (pokud chybí, lokátor ve středu). Polohy příjmu: pole `GRIDSQUARE`.
- Použitá QSO: 28, 50, 70 a 144 MHz s lokátorem. QSO s jiným `PROP_MODE` (TR, MS, FAI, AUR, EME...) a ta pod 500 km lze vyloučit. 432 MHz se nikdy nezahrnuje: sporadická vrstva E se ho netýká.

## Dny a hodiny

- **QSO za den**: den, počet QSO, pásma, maximální vzdálenost, vícenásobné skoky. Při otevření program nabídne den s nejvíce QSO.
- **Hodiny dne**: řádek *Celý den* a jeden řádek pro každou hodinu UTC; kliknutí zobrazí jen QSO dané hodiny.
- **QSO**: seznam pro výběr; kliknutí na QSO (nebo poblíž stanice DX na mapě) jej zvýrazní a zobrazí jeho podrobnosti.

## Mapa

**Azimutální ekvidistantní** mapa se středem ve stanici, s posuvníkem **Maximální vzdálenost** pro zvětšení (také kolečkem myši): kruhy ukazují **km od středu**.

- **Skip podle sektoru** (10° azimutu): oranžový oblouk = nejbližší QSO v sektoru (okraj pásma ticha), zelený oblouk = nejvzdálenější QSO, mezi nimi dosažená oblast.
- **Oblaka Es** na čtvercích Maidenhead, obarvená podle odhadnuté foEs.
- Střed trasy jednoho skoku (žlutý), první skok stanice (oranžový, prázdný kruh = krátký první skok), první skok stanice DX (fialový).
- **Rozmazaná žlutá tečka** označuje střed každého skoku, kde se předpokládá oblak Es (také střed krátkého prvního skoku, když je tato hypotéza zapnutá).
- Pro celý den spojuje **červená čára** těžiště středů tras hodinu po hodině: je to posun oblaků.

![Jedna hodina](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Navržené výpočty

1. **Skip podle sektoru**: pro každý sektor vzdálenost skipu a dosah, se značkami.
2. **Odhadnutá foEs** zákonem sekanty (foEs ≥ f / sec φ): nejkratší jednoduchý skok na každém pásmu dává nejvyšší odhad; s touto foEs očekávaná MUF na 1000, 1500, 2000 a 2400 km a vzdálenost, nad kterou by se otevřelo 144 MHz.
3. **Oblaka Es** odhadnutá na čtvercích, s počtem QSO a foEs.
4. **Vícenásobný skok**: první skok stanice podle obou hypotéz (stejné skoky a krátký první skok do 1200 km, zkušenost IC8TEM) porovnaný s oblaky viděnými jedním skokem v rozmezí ±30 minut; do 300 km je hypotéza potvrzena. Program počítá potvrzení každé hypotézy.
5. **Posun oblaků** hodinu po hodině: km, směr a rychlost.

Výpočty jsou orientační: QSO říká, že trasa byla otevřená, ne kde přesně byl oblak. Čím čistší data (6znakové lokátory, správný `PROP_MODE`), tím spolehlivější skip.
