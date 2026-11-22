# SPORADIC-E

[← Domů](Home-cs)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Mapa

Tlačítko nad mapou volí zobrazení:

- **Azimutální**: azimutální ekvidistantní mapa se středem ve vaší stanici, s posuvníkem **zvětšení** (300 až 20000 km od středu, také kolečkem myši).
- **Mercator**: mapa světa s tlačítky Evropa a Svět.

V obou projekcích se kolem stanice kreslí **kruhy vzdáleností podle Flavia IK3XTV**: 500 km (minimum jednoho skoku), 1500-2000 km (optimum), 2400 km (maximum jednoho skoku), 5000 km (maximum dvojitého skoku) a 6900 km (maximum trojitého skoku).

V azimutálních zobrazeních ukazují čárkované kruhy **km od středu** (krok se mění se zvětšením), okraj označuje maximální zobrazenou vzdálenost a kurzor myši ukazuje vzdálenost a azimut od středu.

![Vícenásobný skok](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Co zobrazuje

- **Spoty** posledních minut (15 min - 12 h) na pásmech **28, 40 (8 m, ve výchozím stavu vypnuté), 50, 70 a 144 MHz**, převzaté z relací telnet. Lokátory pocházejí z komentáře spotu (např. `JN70CN<ES>JO62QM`) nebo z databáze CALL3.
- Spoty s **<MS>**, **<AU>** nebo **<TR>** v textu se **zahazují**: netýkají se sporadické vrstvy E, ale jsou užitečné pro meteor scatter, auroru nebo tropo.
- **Transekvatoriální, pravidlo autora (IC8TEM)**: směrem do Afriky a Jižní Ameriky, pokud střed trasy padne do pásu magnetické inklinace mezi -15° a +15° (hodnota převzatá od Cracknella, UKSMG: oblasti TEP asi 5-15° od čáry nulové inklinace), je spojení transekvatoriální (TEP) a ne přes sporadickou vrstvu E, pokud spot neuvádí `<ES>`.
- **144 MHz, pravidlo autora (IC8TEM)**: sporadická vrstva E na 2 m začíná **nad 900 km** (kratší spoty se zahazují). Jediný spot na 144 MHz **se samostatně nekreslí**: objeví se, když do **5 minut** přijde druhý spot na 144 MHz s oblakem na stejném místě (body odrazu do 300 km), nebo když je poblíž spot **50 MHz s krátkým skokem** (do 1200 km, za posledních 15 minut). Lišta nahoře ukazuje, kolik spotů na 144 MHz čeká na potvrzení.
- Pro každý spot: vzdálenost, **počet skoků** podle tabulky **IK3XTV**, elevace, **minimální foEs** (zákon sekanty).
- **První skok obou stanic**: oranžový čtverec = první skok spottera, fialový = první skok DX (stejné skoky); žlutý = střed trasy jednoho skoku.
- **Krátký první skok** (prázdné kruhy): při otevřeních s vícenásobným skokem má autor zkušenost s krátkým prvním skokem do **1200 km** od vlastního QTH; délku lze nastavit.
- **Oblaka Es** na **čtvercích Maidenhead** (2° x 1°), obarvená podle minimální foEs: modrá < 5, světle modrá < 7, zelená < 10, žlutá < 14, oranžová < 20, červená > 20 MHz.
- Tabulka **Oblaka Es**: pro každý čtverec **MUF pro vaši stanici** (červená = 144 MHz možné, oranžová = 70, žlutá = 50).

## Tabulka IK3XTV

| | minimum | optimum | maximum |
|---|---|---|---|
| Jeden skok | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dvojitý skok | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Trojitý skok | – | 4800-6400 km | 6900 km |

## Jak ji používat

- **Kliknutí na spot**: trasa se zvýrazní, s body odrazu, lokátory a azimuty v podrobnostech.
- **Kliknutí na spot** v tabulce: trasa se na mapě rozsvítí (žlutý svit), a pokud je mimo zorné pole, mapa se posune nebo zvětší poloměr, aby ji ukázala. Spot zůstane vybrán, i když přicházejí nové spoty.
- **Tabulka sleduje mapu**: při nastaveném zvětšení ukazují tabulka spotů a tabulka oblaků jen to, co je vidět na mapě.
- **Kliknutí na mapu**: bod se posuzuje jako možný oblak Es, s foEs potřebnou pro 28, 50, 70 a 144 MHz.

## Poznámka k 432 MHz

Sporadická vrstva E **se netýká 432 MHz**: MUF Es dosahuje asi 150-200 MHz a rekordy ARRL uvádějí spojení přes Es jen na 50 a 144 MHz. Spoty v pásmu 70 cm se zde neanalyzují.

## Čtverce obarvené podle kmitočtu a blikání MUF

Čtverce Maidenhead bodů odrazu jsou obarveny podle nejvyššího kmitočtu spotů: **zelená** do 29 MHz, **žlutá** do 51, **oranžová** do 71, **červená** do 145 MHz. Z foEs čtverce se odhaduje pravděpodobná MUF na nejdelším skoku (2400 km, tabulka IK3XTV): pokud dosáhne vyššího pásma, čtverec **bliká** barvou tohoto pásma (princip IK3XTV a Jacobse-Leinwolla: krátký skip znamená vysokou MUF). Pro 144 MHz platí pravidlo autora: čtverec bliká červeně jen tehdy, když oblak dává jeden skok na 50 MHz mezi 450 a 500 km (450 km s tolerancí 50 km).

Vpravo nahoře na mapě se zobrazuje souhrn N0NBH (E-skip pro zvolený kontinent, SFI, indexy K a A).

## Pravidla podle pásma (verze 0.4.18)

- **50 MHz**: do 450 km nejde o sporadickou vrstvu E. Spoty FT8 a FT4 jsou sporadickou vrstvou E jen tehdy, když jsou součástí oblaku: jiný spot na 50 MHz do 4 minut se středem trasy do 150 km. Osamocený spot FT8/FT4 jde do meteor scatter.
- **70 MHz**: do 450 km nejde o sporadickou vrstvu E. FT8 a FT4 jdou do sporadické vrstvy E i uvnitř okna meteor scatter v kmitočtovém plánu (nejprve rozhoduje známý druh provozu, pak kmitočet).
- **40 MHz (8 m)**: minimální vzdálenost odvozená ze 450 km pro 50 MHz zákonem sekanty (asi 330 km); jen vzdálenosti do prvního skoku geometrie vrstvy E (asi 2440 km). Barva trasy: žlutá.
- **144 MHz**: sporadická vrstva E od 900 km, s potvrzením; spoty Q65 a JT65 jsou EME a zahazují se, pokud text neobsahuje `<ES>`.
- G0ISW (Propagation identification, 2024) vidí nejkratší sporadickou vrstvu E nad 400 km na 50 a 70 MHz a nad 1400 km na 144 MHz: pravidla autora se shodují na 50 a 70 MHz a na 144 MHz jsou širší, ale vyžadují potvrzení.
- Pořadí, v jakém se všechna pravidla uplatňují, s používanými hodnotami, ukazuje **[NASTAVENÍ](Settings-cs)** › Třídění spotů.

## Řetězce s krátkými skoky (od verze 0.6.0)

![Řetězec z QTH](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

V podkartě **Předzvěsti vícenásobného skoku** program hledá řetězce vícenásobného skoku na 50 MHz se třemi podmínkami autora: **první a poslední krátký skok** (500-1200 km) s MUF kolem 50 MHz, které vyžadují foEs 11-21 MHz, a **střední skoky** do 2200 km, které vyžadují asi 9,3 MHz.

- **Silná oblaka** (oranžové čtverce): spoty Es z celého světa a ionosondy GIRO s foEs nejméně 11,3 MHz.
- **Z QTH**: první krátký skok od vaší stanice a poslední krátký skok na jiný silný oblak ve stejném směru, i na stejném kontinentu (například v Kazachstánu); kruh je zóna příchodu.
- **Koridor**: libovolná dvě silná oblaka v jakémkoli směru, například **Karibik - Pyrenejský poloostrov** nebo **Evropa - západní Afrika** (TEP vyloučeno); «týká se vás», pokud je jeden konec do 1200 km od vás.
- **Hledej oblak**: je jen první krátký skok; světle modré zóny ukazují, kde by měl ležet oblak krátkého koncového skoku.
- **VÝSTRAHA**, pokud jsou střední skoky pokryté oblaky, **PŘEDZVĚST**, pokud chybějí jen zóny bez pozorovatelů.
- V NASTAVENÍ je **upozornění** nahoře (a volitelně okno) pro VÝSTRAHY, které se vás týkají.
- Řetězce, které končí **na otevřeném moři** nebo tam, kde nejsou stanice (CALL3 a spoty), se nezobrazují.
- **Stejný dvojitý skok** (od verze 0.6.3, skoky «2 =»): do asi 4700 km, například směrem do Kazachstánu, s oblakem ve čtvrtině trasy jako v metodě ITU-R P.534-6.
- Karta **TABULKA SKOKŮ** shrnuje, kolik skoků je potřeba na 50 MHz od 500 do 12000 km, s foEs každého skoku a zdroji.
- **Stopa mého signálu na 6 m** (růžové body): kam dopadá tvůj signál na 50 MHz po 2-5 skocích, nad oblaky za posledních 30 minut odhadnutými ze spotů všech pásem (28-144 MHz) a z ionosond; jen tam, kde jsou stanice.
- **Koridor se stejnými skoky** (od verze 0.6.5): dvě oblaka 1400-2350 km od sebe spojují dvěma stejnými skoky dvě zóny ve vzdálenosti 2800-4700 km; kontrola na expedici 5U5K potvrzuje hranici asi 4700 km.
- **ČASNÁ ZNÁMKA** (zelená, od verze 0.6.5): oblak Es v pásmu 28 nebo 24 MHz tam, kde pásmo 50 MHz ještě není otevřené; ve zkouškách se tam pásmo 50 MHz otevřelo do hodiny v 50 % případů (30 % bez ní), 9-16 minut předem.
- **Opakovaný APRS** (červený, od verze 0.6.5): nejméně dva skoky APRS v pásmu 2 m nad 900 km přes stejný oblak do 10 minut = VÝSTRAHA 144 MHz; karta bliká.
- **Jakou mají výstrahy hodnotu** (na stanici autora, léta 2024-2026): VÝSTRAHY úspěšné v 41-57 % (QSO vícenásobným skokem ve stejném směru do hodiny) proti 5-7 % bez výstrahy; ale 47 % otevření přichází bez varování. Stopa dosáhne do 500 km od vzdálené stanice ve 13,5 % otevření, po otočení ve 2-4 %.
- Oblaka **se neposouvají**: zkouška z 22. 11. 2026 ukazuje, že jejich posunutí jejich rychlostí nezlepšuje předpověď na 15-45 minut.
- **Předpověď TEP** (modrá, od verze 0.6.5): index 0-100 směrem k zónám za magnetickým rovníkem = roční období × SFI × Kp (jen eTEP) × místní čas × foF2 ionosond GIRO; SFI váží, ale nenuluje, protože při slunečním minimu je TEP vzácný, ale nezmizí.

![Koridor](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)
