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

## Trumpų šuolių grandinės (nuo 0.6.0 versijos)

![Grandinė iš QTH](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

Pakortelėje **Kelių šuolių pirmtakai** programa ieško kelių šuolių grandinių 50 MHz juostoje pagal tris autoriaus sąlygas: **pirmas ir paskutinis trumpas šuolis** (500-1200 km) su MUF apie 50 MHz, kuriems reikia 11-21 MHz foEs, ir **viduriniai šuoliai** iki 2200 km, kuriems reikia apie 9,3 MHz.

- **Stiprūs debesys** (oranžiniai kvadratai): viso pasaulio Es spotai ir GIRO jonozondai, kurių foEs ne mažesnė kaip 11,3 MHz.
- **Iš QTH**: pirmas trumpas šuolis iš jūsų stoties ir paskutinis trumpas šuolis ant kito stipraus debesies ta pačia kryptimi, taip pat tame pačiame žemyne (pavyzdžiui, Kazachstane); apskritimas – tikslo zona.
- **Koridorius**: bet kurie du stiprūs debesys, bet kuria kryptimi, pavyzdžiui, **Karibai - Pirėnų pusiasalis** arba **Europa - Vakarų Afrika** (be TEP); «liečia jus», jei vienas galas yra iki 1200 km nuo jūsų.
- **Ieškoti debesies**: yra tik pirmas trumpas šuolis; žydros zonos rodo, kur turėtų būti paskutinio trumpo šuolio debesis.
- **ĮSPĖJIMAS**, jei viduriniai šuoliai padengti debesimis, **PIRMTAKAS**, jei trūksta tik zonų be stebėtojų.
- NUSTATYMUOSE yra **pranešimas** viršuje (pasirinktinai ir langas) apie jus liečiančius ĮSPĖJIMUS.
- Grandinės, pasiekiančios **atvirą jūrą** arba vietas be stočių (CALL3 ir spotai), nerodomos.
- **Vienodas dvigubas šuolis** (nuo 0.6.3 versijos, šuoliai «2 =»): iki maždaug 4700 km, pavyzdžiui, į Kazachstaną, su debesiu trasos ketvirtadalyje kaip metode ITU-R P.534-6.
- Kortelė **ŠUOLIŲ LENTELĖ** apibendrina, kiek šuolių reikia 50 MHz juostoje nuo 500 iki 12000 km, su kiekvieno šuolio foEs ir šaltiniais.
- **Mano signalo aprėptis 6 m juostoje** (rožiniai taškai): kur jūsų 50 MHz signalas nusileidžia po 2-5 šuolių, virš paskutinių 30 minučių debesų, įvertintų iš visų juostų (28-144 MHz) spotų ir jonozondų; tik ten, kur yra stočių.
- **Lygių šuolių koridorius** (nuo 0.6.5 versijos): du debesys 1400-2350 km vienas nuo kito dviem lygiais šuoliais sujungia dvi zonas 2800-4700 km atstumu; patikrinimas su ekspedicija 5U5K patvirtina maždaug 4700 km ribą.
- **ANKSTYVASIS ŽENKLAS** (žalias, nuo 0.6.5 versijos): Es debesis 28 arba 24 MHz dažnyje ten, kur 50 MHz dar neatsivėrė; bandymuose 50 MHz ten atsivėrė per valandą 50% atvejų (be jo – 30%), 9-16 minučių anksčiau.
- **Pasikartojęs APRS** (raudonas, nuo 0.6.5 versijos): bent du APRS šuoliai 2 m dažnyje virš 900 km per tą patį debesį per 10 minučių = 144 MHz ĮSPĖJIMAS; kortelė mirksi.
- **Ko verti įspėjimai** (autoriaus stotyje, 2024-2026 m. vasaros): ĮSPĖJIMAI pasitvirtino 41-57% (kelių šuolių QSO ta pačia kryptimi per valandą), palyginti su 5-7% be įspėjimo; bet 47% atsivėrimų ateina be perspėjimo. Aprėptis pasiekia 500 km spindulį aplink tolimą stotį 13,5% atsivėrimų, pasukta – 2-4%.
- Debesys **neperkeliami**: 2026-11-22 bandymas rodo, kad jų perkėlimas jų greičiu nepagerina 15-45 minučių prognozės.
- **TEP prognozė** (mėlyna, nuo 0.6.5 versijos): indeksas 0-100 zonų už magnetinio pusiaujo kryptimi = metų laikas × SFI × Kp (tik eTEP) × vietos laikas × GIRO jonozondų foF2; SFI sveria, bet nenulina, nes per Saulės minimumą TEP tampa retas, bet neišnyksta.

![Koridorius](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)

## PROGNOZĖ (nuo 0.7.0)

Poskirtukas **PROGNOZĖ** atpažįsta besiformuojančius Es debesis (paskutinių 30 minučių spotai ir GIRO jonozondai) ir tikrina iš jūsų stoties, 50 ir 144 MHz atskirai: tinkamą pirmąjį šuolį, tada dvigubą Es šuolį, tada – tik jei nėra pirmojo šuolio – tropo + Es su virtualia stotimi A′, pasiekiama kanalu (LIVETROPOVHF tropo modulis, Open-Meteo profiliai, ITU-R P.834 kritinis kampas). REAL-TIME 144 MHz spotai tarp 2400 ir 3400 km gali tapti **TR+ES**, su perkeltu debesiu.
