# SPORADIC-E

[← Home](Home-lt)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Žemėlapis

Mygtukas virš žemėlapio parenka vaizdą:

- **Azimutinis**: azimutinis vienodų atstumų žemėlapis, kurio centras – jūsų stotis, su **mastelio** slankikliu (nuo 300 iki 20000 km nuo centro, taip pat pelės ratuku).
- **Merkatoriaus**: pasaulio žemėlapis su mygtukais Europa ir Pasaulis.

Abiejose projekcijose aplink stotį nubraižomi **Flavio IK3XTV atstumų apskritimai**: 500 km (vieno šuolio minimumas), 1500-2000 km (optimumas), 2400 km (vieno šuolio maksimumas), 5000 km (dvigubo šuolio maksimumas) ir 6900 km (trigubo šuolio maksimumas).

Azimutiniuose vaizduose brūkšniniai apskritimai rodo **km nuo centro** (žingsnis keičiasi su masteliu), kraštas žymi didžiausią rodomą atstumą, o pelės žymeklis rodo atstumą ir azimutą nuo centro.

![Keli šuoliai](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ką rodo

- Paskutinių minučių (15 min - 12 h) **spotai** **28, 40 (8 m, pagal nutylėjimą išjungta), 50, 70 ir 144 MHz** juostose, gauti iš telnet seansų. Lokatoriai imami iš spoto komentaro (pvz., `JN70CN<ES>JO62QM`) arba iš CALL3 duomenų bazės.
- Spotai, kurių tekste yra **<MS>**, **<AU>** arba **<TR>**, **atmetami**: jie nesusiję su sporadiniu E, bet naudingi meteor scatter, pašvaistei arba tropo.
- **Transekvatorinis sklidimas, autoriaus (IC8TEM) taisyklė**: Afrikos ir Pietų Amerikos kryptimi, jei vidurio taškas patenka į magnetinės inklinacijos juostą tarp -15° ir +15° (reikšmė paimta iš Cracknell, UKSMG: TEP zonos maždaug 5-15° nuo nulinės inklinacijos linijos), ryšys yra transekvatorinis (TEP), o ne sporadinis E, nebent spote nurodyta `<ES>`.
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

## Kvadratai, nuspalvinti pagal dažnį, ir mirksinti MUF

Atspindžio taškų Maidenhead kvadratai nuspalvinami pagal didžiausią spotų dažnį: **žalia** iki 29 MHz, **geltona** iki 51, **oranžinė** iki 71, **raudona** iki 145 MHz. Iš kvadrato foEs įvertinama tikėtina MUF ilgiausiame šuolyje (2400 km, IK3XTV lentelė): jei ji pasiekia aukštesnę juostą, kvadratas **mirksi** tos juostos spalva (IK3XTV ir Jacobs-Leinwoll principas: trumpas skip reiškia aukštą MUF). 144 MHz juostai galioja autoriaus taisyklė: kvadratas mirksi raudonai tik tada, kai debesis duoda vieną 50 MHz šuolį nuo 450 iki 500 km (450 km su 50 km tolerancija).

Žemėlapio viršuje dešinėje rodoma N0NBH santrauka (pasirinkto žemyno E-skip, SFI, K ir A indeksai).

## Taisyklės pagal juostą (versija 0.4.18)

- **50 MHz**: iki 450 km sporadinio E nėra. FT8 ir FT4 spotai yra sporadinis E tik tada, kai priklauso debesiui: kitas 50 MHz spotas per 4 minutes su vidurio tašku iki 150 km. Pavienis FT8/FT4 spotas priskiriamas meteor scatter.
- **70 MHz**: iki 450 km sporadinio E nėra. FT8 ir FT4 priskiriami sporadiniam E net juostų plano meteor scatter lange (pirmiausia sprendžia žinomas režimas, paskui dažnis).
- **40 MHz (8 m)**: mažiausias atstumas išvestas iš 50 MHz 450 km sekanto dėsniu (apie 330 km); tik atstumai E sluoksnio geometrijos pirmo šuolio ribose (apie 2440 km). Trasos spalva: geltona.
- **144 MHz**: sporadinis E nuo 900 km, su patvirtinimu; Q65 ir JT65 spotai yra EME ir atmetami, nebent tekste yra `<ES>`.
- G0ISW (Propagation identification, 2024) trumpiausią sporadinį E stebi daugiau nei 400 km 50 ir 70 MHz juostose ir daugiau nei 1400 km 144 MHz juostoje: autoriaus taisyklės sutampa 50 ir 70 MHz juostose, o 144 MHz juostoje yra platesnės, bet reikalauja patvirtinimo.
- Visų taisyklių taikymo tvarka su naudojamomis reikšmėmis rodoma kortelėje **[NUSTATYMAI](Settings-lt)** › Spotų skirstymas.
