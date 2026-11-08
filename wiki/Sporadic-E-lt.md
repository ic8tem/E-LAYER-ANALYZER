# SPORADIC-E

[← Home](Home-lt)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Žemėlapis

Mygtukas virš žemėlapio parenka vaizdą:

- **Azimutinis - vienas šuolis**: azimutinis vienodų atstumų žemėlapis, kurio centras – jūsų stotis, fiksuotas 2500 km spindulys, su IK3XTV lentelės zonomis (minimalus 500 km, optimalus 1500-2000 km, vieno šuolio maksimalus 2400 km).
- **Azimutinis - maksimalus atstumas (keli šuoliai)**: tas pats žemėlapis su slankikliu **Maksimalus atstumas** (nuo 300 iki 20000 km, taip pat pelės ratuku); rodomos ir dvigubo (5000 km) bei trigubo šuolio (6900 km) ribos, todėl galima stebėti kelių šuolių sklidimo raidą.
- **Stačiakampis**: ankstesnis pasaulio žemėlapis su mygtukais Europa ir Pasaulis.

Azimutiniuose vaizduose brūkšniniai apskritimai rodo **km nuo centro** (žingsnis keičiasi su masteliu), kraštas žymi didžiausią rodomą atstumą, o pelės žymeklis rodo atstumą ir azimutą nuo centro.

![Keli šuoliai](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ką rodo

- Paskutinių minučių (15 min - 12 h) **spotai** **28, 50, 70 ir 144 MHz** juostose, gauti iš telnet seansų. Lokatoriai imami iš spoto komentaro (pvz., `JN70CN<ES>JO62QM`) arba iš CALL3 duomenų bazės.
- Spotai, kurių tekste yra **<MS>**, **<AU>** arba **<TR>**, **atmetami**: jie nesusiję su sporadiniu E, bet naudingi meteor scatter, pašvaistei arba tropo.
- **Transekvatorinis sklidimas, autoriaus (IC8TEM) taisyklė**: Afrikos ir Pietų Amerikos kryptimi, jei vidurio taškas patenka į magnetinės inklinacijos juostą tarp -20° ir +20°, ryšys yra transekvatorinis (TEP), o ne sporadinis E, nebent spote nurodyta `<ES>`.
- **144 MHz, autoriaus (IC8TEM) taisyklė**: sporadinis E 2 m juostoje prasideda **toliau nei 900 km** (trumpesni spotai atmetami). Pavienis 144 MHz spotas **atskirai nerodomas**: jis pasirodo, kai per **5 minutes** ateina antras 144 MHz spotas su debesiu toje pačioje vietoje (atspindžio taškai iki 300 km vienas nuo kito) arba kai netoliese yra **trumpo šuolio 50 MHz** spotas (iki 1200 km, per paskutines 15 minučių). Viršutinė juosta rodo, kiek 144 MHz spotų laukia patvirtinimo.
- Kiekvienam spotui: atstumas, **šuolių skaičius** pagal **IK3XTV** lentelę, elevacija, **minimali foEs** (sekanto dėsnis).
- **Abiejų stočių pirmas šuolis**: oranžinis kvadratas = spoterio pirmas šuolis, violetinis = DX pirmas šuolis (vienodi šuoliai); geltonas = vieno šuolio vidurio taškas.
- **Trumpas pirmas šuolis** (tušti apskritimai): kelių šuolių atsivėrimų metu autorius patyrė trumpą pirmą šuolį iki **1200 km** nuo savo QTH; ilgį galima nustatyti.
- **Es debesys** **Maidenhead kvadratuose** (2° x 1°), nuspalvinti pagal minimalią foEs: mėlyna < 5, žydra < 7, žalia < 10, geltona < 14, oranžinė < 20, raudona > 20 MHz.
- Lentelė **Es debesys**: kiekvienam kvadratui **MUF jūsų stočiai** (raudona = 144 MHz įmanoma, oranžinė = 70, geltona = 50).

## IK3XTV lentelė

| | minimalus | optimalus | maksimalus |
|---|---|---|---|
| Vienas šuolis | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dvigubas šuolis | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Trigubas šuolis | – | 4800-6400 km | 6900 km |

## Kaip naudotis

- **Spustelėkite spotą**: trasa paryškinama, detalėse rodomi atspindžio taškai, lokatoriai ir azimutai.
- **Spustelėkite spotą** lentelėje: trasa žemėlapyje nušvinta (geltonas švytėjimas), o jei ji nematoma, žemėlapis pasislenka arba padidina spindulį, kad ją parodytų. Spotas lieka pasirinktas ir atėjus naujiems spotams.
- **Lentelė seka žemėlapį**: nustačius mastelį, spotų ir debesų lentelės rodo tik tai, kas matoma žemėlapyje.
- **Spustelėkite žemėlapį**: taškas laikomas galimu Es debesiu, rodoma 28, 50, 70 ir 144 MHz reikalinga foEs.

## Pastaba dėl 432 MHz

Sporadinis E **netaikomas 432 MHz**: Es MUF siekia apie 150-200 MHz, o ARRL rekorduose Es ryšiai nurodyti tik 50 ir 144 MHz juostose. 70 cm spotai čia neanalizuojami.
