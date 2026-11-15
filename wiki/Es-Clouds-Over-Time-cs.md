# OBLAKA Es V ČASE

[← Domů](Home-cs)

![OBLAKA Es V ČASE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Podkarta **SPORADIC-E** (od verze 0.5.2): jak **oblaka sporadické vrstvy E** vznikají, posouvají se a zanikají, krok za krokem.

## Metoda

Metoda **Chartiera, Hanleyho a Emmonse (2022)**: každé spojení přes sporadickou vrstvu E říká, že v bodě odrazu je foEs **nejméně f / sec φ**. Body se seskupují do buněk a časových kroků a každá buňka převezme **nejvyšší hodnotu** kroku. Minimální elektronová hustota je NmE = 1,24·10¹⁰ · foEs² elektronů na metr krychlový.

Program používá body odrazu vlastního modelu (všechny skoky), ne jen střed trasy. Hodnota je **dolní mez**: prázdná buňka znamená jen to, že není žádné spojení.

## Zdroj a ovládání

- **Spoty relace** (aktualizují se při každém otevření karty) nebo **Načíst deník ADIF nebo spoty CSV**.
- **Exportovat spoty relace (CSV)**: archiv spotů se při zavření programu vyprázdní, takže je tak můžete prohlédnout později.
- **Pásma MHz** (28, 40, 50, 70, 144), **Krok** (5, 10 nebo 15 minut), **Buňka** (1°, 2° nebo 4°), **Výška Es km**.
- Časová lišta: ⏮ ◀ *Spustit/Zastavit* ▶ ⏭ a posuvník; po otevření se začíná krokem s nejvíce buňkami.
- *Vybledlé předchozí kroky* ukazuje tři kroky před zvoleným; *Spojení* kreslí trasy barvami pásem.

## Tabulka a graf

- Buňky kroku: Buňka (jihozápadní roh a lokátor středu), foEs≥, NmE≥, Spojení, Pásma. Kliknutí na buňku: zvýrazní její spojení.
- Graf: nejvyšší minimální foEs (žlutá čára) a počet buněk (sloupce) pro každý krok.
