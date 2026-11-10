# SPORADIC-E

[← Home](Home-cs)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Mapa

Tlačítko nad mapou volí pohled:

- **Azimutální**: azimutální ekvidistantní mapa se středem na vaší stanici, s posuvníkem **zoomu** (300 až 20000 km od středu, také kolečkem myši).
- **Mercator**: mapa světa s tlačítky Evropa a Svět.

V obou projekcích se kolem stanice kreslí **kruhy vzdálenosti podle Flavia IK3XTV**: 500 km (minimum jednoduchého skoku), 1500-2000 km (optimum), 2400 km (maximum jednoduchého skoku), 5000 km (maximum dvojitého skoku) a 6900 km (maximum trojitého skoku).

V azimutálních pohledech ukazují čárkované kruhy **km od středu** (krok se mění se zoomem), okraj vyznačuje maximální zobrazenou vzdálenost a kurzor myši udává vzdálenost a azimut od středu.

![Multi-hop](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Co zobrazuje

- **Spoty** z posledních minut (15 min - 12 h) na pásmech **28, 50, 70 a 144 MHz**, převzaté z relací telnet. Lokátory pocházejí z komentáře spotu (např. `JN70CN<ES>JO62QM`) nebo z databáze CALL3.
- Spoty s **<MS>**, **<AU>** nebo **<TR>** v textu se **zahazují**: netýkají se sporadické vrstvy E, ale jsou užitečné pro meteor scatter, auroru nebo tropo.
- **Transekvatoriální, pravidlo autora (IC8TEM)**: směrem do Afriky a Jižní Ameriky, padne-li střed trasy do pásma magnetické inklinace mezi -20° a +20°, je spojení transekvatoriální (TEP), a ne sporadická vrstva E, pokud spot neuvádí `<ES>`.
- **144 MHz, pravidlo autora (IC8TEM)**: sporadická vrstva E na 2 m začíná **nad 900 km** (kratší spoty se zahazují). Jediný spot na 144 MHz **se samostatně nevykresluje**: objeví se, když do **5 minut** přijde druhý spot na 144 MHz s oblakem na stejném místě (body odrazu do 300 km), nebo když je poblíž spot s **krátkým skokem na 50 MHz** (do 1200 km, v posledních 15 minutách). Lišta nahoře ukazuje, kolik spotů na 144 MHz čeká na potvrzení.
- Pro každý spot: vzdálenost, **počet skoků** podle tabulky **IK3XTV**, elevace, **minimální foEs** (zákon sekanty).
- **První skok obou stanic**: oranžový čtverec = první skok spottera, fialový = první skok DX (stejné skoky); žlutý = střed jednoduchého skoku.
- **Krátký první skok** (prázdné kruhy): při otevřeních s vícenásobným skokem autor zažil krátký první skok do **1200 km** od svého QTH; délku lze nastavit.
- **Oblaka Es** na **čtvercích Maidenhead** (2° x 1°), obarvená podle minimální foEs: modrá < 5, světle modrá < 7, zelená < 10, žlutá < 14, oranžová < 20, červená > 20 MHz.
- Tabulka **oblaků Es**: pro každý čtverec **MUF pro vaši stanici** (červená = možné 144 MHz, oranžová = 70, žlutá = 50).

## Tabulka IK3XTV

| | minimum | optimum | maximum |
|---|---|---|---|
| Jednoduchý skok | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dvojitý skok | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Trojitý skok | – | 4800-6400 km | 6900 km |

## Jak ji používat

- **Kliknutí na spot**: trasa se zvýrazní, s body odrazu, lokátory a azimuty v podrobnostech.
- **Kliknutí na spot** v tabulce: trasa se na mapě rozsvítí (žlutá záře) a pokud je mimo zobrazení, mapa se posune nebo zvětší poloměr, aby ji ukázala. Spot zůstane vybraný i při příchodu nových spotů.
- **Tabulka sleduje mapu**: při nastaveném zoomu ukazují tabulka spotů a tabulka oblaků jen to, co je vidět na mapě.
- **Kliknutí do mapy**: bod se bere jako možné oblako Es, s foEs potřebnou pro 28, 50, 70 a 144 MHz.

## Poznámka k 432 MHz

Sporadická vrstva E se **na 432 MHz neuplatňuje**: MUF Es dosahuje zhruba 150-200 MHz a rekordy ARRL uvádějí spojení přes Es jen na 50 a 144 MHz. Spoty na 70 cm se zde neanalyzují.

## Čtverce obarvené podle frekvence a blikání MUF

Čtverce Maidenhead bodů odrazu se barví podle nejvyšší frekvence spotu: **zelená** do 29 MHz, **žlutá** do 51, **oranžová** do 71, **červená** do 145 MHz. Z foEs čtverce se odhaduje pravděpodobná MUF na nejdelším skoku (2400 km, tabulka IK3XTV): dosáhne-li vyššího pásma, čtverec **bliká** barvou tohoto pásma (princip IK3XTV a Jacobse-Leinwolla: krátký skip znamená vysokou MUF).

Vpravo nahoře na mapě se zobrazuje souhrn N0NBH (E-skip pro zvolený kontinent, SFI, indexy K a A).
