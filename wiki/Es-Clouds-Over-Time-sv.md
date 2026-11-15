# Es-MOLN ÖVER TID

[← Home](Home-sv)

![Es-MOLN ÖVER TID](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Underflik till **SPORADIC-E** (från version 0.5.2): hur **molnen av sporadiskt E** uppstår, förflyttar sig och försvinner, steg för steg.

## Metoden

Metoden av **Chartier, Hanley och Emmons (2022)**: varje förbindelse via sporadiskt E visar att foEs i reflexionspunkten är **minst f / sec φ**. Punkterna samlas i celler och i tidssteg, och varje cell får stegets **högsta värde**. Den minsta elektrontätheten är NmE = 1,24·10¹⁰ · foEs² elektroner per kubikmeter.

Programmet använder reflexionspunkterna i sin egen modell (alla hopp), inte bara mittpunkten. Värdet är en **nedre gräns**: en tom cell betyder bara att det inte finns några förbindelser.

## Källa och kontroller

- **Sessionens spots** (uppdateras varje gång fliken öppnas) eller **Läs in ADIF-logg eller CSV-spots**.
- **Exportera sessionens spots (CSV)**: spotarkivet töms när programmet stängs, så på detta sätt kan du se dem igen senare.
- **Band MHz** (28, 40, 50, 70, 144), **Steg** (5, 10 eller 15 minuter), **Cell** (1°, 2° eller 4°), **Es-höjd km**.
- Tidslinje: ⏮ ◀ *Starta/Stoppa* ▶ ⏭ och reglaget; vid öppning börjar visningen med steget som har flest celler.
- *Tidigare steg nedtonade* visar de tre stegen före det valda; *Förbindelser* ritar sträckorna i bandens färger.

## Tabell och diagram

- Stegets celler: Cell (sydvästra hörnet och lokatorn för mitten), foEs≥, NmE≥, Förbindelser, Band. Klicka på en cell: dess förbindelser markeras.
- Diagram: högsta minsta foEs (gul linje) och antal celler (staplar) för varje steg.
