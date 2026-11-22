# IONOSONDID

[← Home](Home-et)

![IONOSONDID](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

**SPORADIC-E** alamsakk (alates versioonist 0.5.2): **GIRO** võrgu ionosondide **mõõdetud foEs** kogu maailmas, selle kõrval, mille programm spotidest tuletab.

## Andmed

- 62 GIRO ionosondi (ULCAR-i loend), ionogrammidelt automaatselt loetud viimase **foEs**, **foF2** ja **MUF(D)**-ga.
- Andmed saabuvad saki esimesel avamisel ja seejärel **iga 15 minuti järel** ise (märkeruut *Värskenda ise iga 15 minuti järel*); *Värskenda kohe* küsib need kohe. Arhiiv hoiab viimase 48 tunni andmeid andmekaustas `giro`.
- **GIRO** andmed (Reinisch ja Galkin 2011), litsents **CC BY-NC-SA 4.0**: ainult mitteäriline kasutus. Teenus on sama mis DM6AX-i programmis *MUF Muncher*.

## Kaart ja tabel

- Iga ionosond on värviline täpp viimase foEs-iga (samad värvid nagu Es-pilvedel) ja väärtusega MHz-des; **hall**, kui hiljutisi andmeid pole, **rõngaga**, kui viimane ionogramm on vanem kui 90 minutit.
- Tabel: Kood, Ionosond, km sinu jaamast, viimase ionogrammi UTC, foEs, foF2, MUF(D) ja **Spotid foEs≥**: viimase 30 minuti Es-spotidest tuletatud minimaalne foEs, mille peegeldus on ionosondist 500 km piires (muudetav), koos spottide arvuga.
- **h'Es GIRO** (alates versioonist 0.6.5, ruut sakil REAL-TIME): Es-kõrguseks saab 2500 km piires olevate ionosondide värskete h'Es-ide mediaan (80-120 km).

## Valitud ionosond

Klõpsa real või ionosondi lähedal kaardil:

- **viimase 24 tunni graafik**: foEs kollane, foF2 helesinine, MUF(D) lilla punktiir;
- **sporaadilise E MUF** 1000, 1500, 2000 ja 2400 km juures (MUF = foEs · sec φ, valitud Es-kõrgusel) koos **avanevate sagedusaladega** (28, 40, 50, 70, 144 MHz);
- kui spotid nõuavad rohkem kui mõõdetud foEs, annab programm sellest teada: pilv võib olla ionosondist kaugel või on ionosond jõudnud oma sagedusvahemiku piirini (Chartier 2022).
