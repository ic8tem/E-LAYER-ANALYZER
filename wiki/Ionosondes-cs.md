# IONOSONDY

[← Domů](Home-cs)

![IONOSONDY](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Podkarta **SPORADIC-E** (od verze 0.5.2): **foEs měřená** ionosondami sítě **GIRO** po celém světě, vedle té, kterou program odvozuje ze spotů.

## Data

- 63 ionosond GIRO (seznam ULCAR) s poslední **foEs**, **foF2** a **MUF(D)** čtenými automaticky z ionogramů.
- Data přicházejí při prvním otevření karty a pak **každých 15 minut**, sama (pole *Aktualizovat automaticky každých 15 minut*); *Aktualizovat nyní* si je vyžádá okamžitě. Archiv uchovává posledních 48 hodin ve složce `giro` v datech.
- Data **GIRO** (Reinisch a Galkin 2011), licence **CC BY-NC-SA 4.0**: jen pro nekomerční použití. Služba je stejná jako u *MUF Muncher* od DM6AX.

## Mapa a tabulka

- Každá ionosonda je barevná tečka s poslední foEs (stejné barvy jako oblaka Es) a hodnotou v MHz; **šedá**, pokud nemá aktuální data, s **kroužkem**, pokud je poslední ionogram starší než 90 minut.
- Tabulka: Kód, Ionosonda, km od vaší stanice, UTC posledního ionogramu, foEs, foF2, MUF(D) a **Spoty foEs≥**: minimální foEs odvozená ze spotů Es za posledních 30 minut, které se odrážejí do 500 km od ionosondy (nastavitelné), s počtem spotů.

## Vybraná ionosonda

Kliknutí na řádek nebo poblíž ionosondy na mapě:

- **graf posledních 24 hodin**: foEs žlutá, foF2 světle modrá, MUF(D) fialová tečkovaná;
- **MUF sporadické vrstvy E** na 1000, 1500, 2000 a 2400 km (MUF = foEs · sec φ, ve zvolené výšce Es) s **pásmy, která se otevírají** (28, 40, 50, 70, 144 MHz);
- pokud spoty vyžadují více než naměřenou foEs, program to oznámí: oblak může být daleko od ionosondy, nebo ionosonda dosáhla hranice svého kmitočtového rozsahu (Chartier 2022).
