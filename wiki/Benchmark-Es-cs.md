# BENCHMARK Es

[← Home](Home-cs)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Zkušební stav pro **sporadickou vrstvu E** a **vícenásobný skok** nad **celým ADIF deníkem**, i deníkem jiné stanice: ze skutečně navázaných QSO program načrtne pásmo ticha a oblaka, den po dni a hodinu po hodině.

## Načtení deníku

- **Načíst ADIF**: zvolí soubor `.adi`/`.adif`. Program si soubor zapamatuje a při spuštění ho znovu načte.
- **Uvolnit ADIF**: odstraní deník z karty.
- **Stanice ve středu**: značka a lokátor stanice, která deník vytvořila. Program navrhne nejčastější `STATION_CALLSIGN` (nebo `OPERATOR`) a `MY_GRIDSQUARE`; lze je ručně změnit a potvrdit tlačítkem **Použít**. Střed nezávisí na obecném nastavení, takže lze studovat deníky jiných stanic.
- Vysílací poloha: `MY_GRIDSQUARE` každého QSO (pokud chybí, lokátor ve středu). Přijímací polohy: pole `GRIDSQUARE`.
- Použitá QSO: 28, 50, 70 a 144 MHz s lokátorem. QSO s jiným `PROP_MODE` (TR, MS, FAI, AUR, EME...) a QSO pod 500 km lze vyloučit. 432 MHz se nikdy nezahrnuje: sporadická vrstva E se tam neuplatňuje.

## Dny a hodiny

- **QSO za den**: den, počet QSO, pásma, maximální vzdálenost, vícenásobné skoky. Při otevření program navrhne den s nejvíce QSO.
- **Hodiny dne**: řádek *Celý den* a jeden řádek pro každou hodinu UTC; kliknutí zobrazí jen QSO z dané hodiny.
- **QSO**: seznam pro výběr; kliknutí na QSO (nebo u DX stanice na mapě) ho zvýrazní a zobrazí podrobnosti.

## Mapa

**Azimutální ekvidistantní** mapa se středem na stanici, s posuvníkem **Maximální vzdálenost** pro zoom (také kolečkem myši): kruhy ukazují **km od středu**.

- **Pásmo ticha podle sektorů** (10° azimutu): oranžový oblouk = nejbližší QSO v sektoru (okraj pásma ticha), zelený oblouk = nejvzdálenější QSO, mezi nimi dosažená oblast.
- **Oblaka Es** na čtvercích Maidenhead, obarvená podle odhadnuté foEs.
- Střed jednoduchých skoků (žlutě), první skok stanice (oranžově, prázdný kruh = krátký první skok), první skok DX stanice (fialově).
- Pro celý den **červená čára** spojuje těžiště středů trasy hodinu po hodině: je to drift oblaků.

![One hour](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Navržené výpočty

1. **Pásmo ticha podle sektorů**: pro každý sektor vzdálenost pásma ticha a rozsah, se značkami.
2. **Odhad foEs** zákonem sekanty (foEs ≥ f / sec φ): nejkratší jednoduchý skok na každém pásmu dává nejvyšší odhad; s touto foEs očekávaná MUF na 1000, 1500, 2000 a 2400 km a vzdálenost, za kterou by se otevřelo 144 MHz.
3. **Oblaka Es** odhadnutá na čtvercích, s počtem QSO a foEs.
4. **Vícenásobný skok**: první skok stanice podle dvou hypotéz (stejné skoky a krátký první skok do 1200 km, zkušenost IC8TEM) porovnaný s oblaky viděnými v jednoduchém skoku v rozmezí ±30 minut; do 300 km je hypotéza potvrzena. Program počítá potvrzení každé hypotézy.
5. **Drift oblaků** hodinu po hodině: km, směr a rychlost.

Výpočty jsou orientační: QSO říká, že trasa byla otevřená, ne přesně, kde bylo oblako. Čím čistší data (6znakové lokátory, správný `PROP_MODE`), tím spolehlivější pásmo ticha.
