# BENCHMARK 50 MHz

[← Home](Home-lt)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Bandymų stendas **kelių šuolių sporadiniam E 50 MHz juostoje**, naudojant vienos stoties ir, jei norima, antros stoties ADIF žurnalą.

## Žurnalų įkėlimas

- **Stotis 1**: *Įkelti ADIF* / *Pašalinti ADIF*. Programa paima iš žurnalo `STATION_CALLSIGN` ir `MY_GRIDSQUARE`; siuntimo vieta – kiekvieno QSO `MY_GRIDSQUARE`.
- **Stotis 2** (neprivaloma): tie patys mygtukai antroje eilutėje; žymimasis langelis *Stotis 2* ją rodo arba slepia. Stoties 1 trasos žydros, stoties 2 trasos violetinės.
- Programa naudoja 50 MHz QSO su lokatoriumi (trūkstami arba 4 simbolių lokatoriai imami iš CALL3), kelių šuolių: toliau nei 3100 km (autoriaus taisyklė; šuolių skaičius pagal IK3XTV lentelę).
- **Transekvatoriniai** ryšiai neįtraukiami (autoriaus taisyklė): Afrikos ir Pietų Amerikos kryptimi, kai vidurio taškas yra magnetinės inklinacijos juostoje tarp -20° ir +20°, nebent žurnale yra `PROP_MODE=ES`.

## Diena ir laikas

Pasirinkite **dieną** (atidarius – tą, kurioje daugiausia kelių šuolių QSO) ir laiką **nuo… iki…** UTC, 15 minučių žingsniais. Žymimasis langelis **foEs** (neprivalomas) nuspalvina trasas pagal minimalią foEs ir parodo ją lentelėje.

## Žemėlapis

**Merkatoriaus** žemėlapis su laisvu centru: ratukas = mastelis, tempimas = slinkimas.

![Trasa](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Išsiliejęs geltonas taškelis** pažymi kiekvieno šuolio centrą, kur spėjama esant Es debesiui; ant trasos po pele jis padidėja.
- **Pelė prie trasos**: linija ir piktogramos padidinamos, pasirodo DX stoties **6 simbolių WWL kvadratas**.
- **Kairysis spustelėjimas** ant tos trasos: iššokantis langas klausia, ar rodyti **visus failo QSO** (visas dienas), kurių tolimojo galo vieta yra **iki 150 km** nuo to taško. *Taip*: žemėlapis išvalomas ir rodo tuos QSO oranžiniame apskritime; *Ne*: žemėlapis lieka toks pat. *Grįžti į dieną ir laiką* grąžina ankstesnį pasirinkimą.

![Apylinkės](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detalės

Pasirinktai trasai: atstumas ir įvertinimas pagal IK3XTV lentelę, azimutas abiem kryptimis, vienodi šuoliai su atspindžio taškų kvadratais, trumpas pirmas šuolis iki 1200 km (IC8TEM patirtis) ir, pažymėjus foEs langelį, minimali foEs.
