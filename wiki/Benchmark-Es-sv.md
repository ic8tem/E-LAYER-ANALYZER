# BENCHMARK Es

[← Home](Home-sv)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Testbänk för **sporadiskt E** och **multihopp** på en **hel ADIF-logg**, även en från en annan station: utifrån de QSO som faktiskt körts skissar programmet skip och moln, dag för dag och timme för timme.

## Läsa in loggen

- **Läs in ADIF**: väljer `.adi`/`.adif`-filen. Programmet minns filen och läser in den igen vid start.
- **Ta bort ADIF**: tar bort loggen från fliken.
- **Station i mitten**: anropssignal och lokator för den station som har fört loggen. Programmet föreslår den vanligaste `STATION_CALLSIGN` (eller `OPERATOR`) och `MY_GRIDSQUARE`; de kan ändras för hand och bekräftas med **Verkställ**. Mitten beror inte på de allmänna inställningarna, så loggar från andra stationer kan studeras.
- Sändarposition: `MY_GRIDSQUARE` för varje QSO (om den saknas, lokatorn i mitten). Mottagarpositioner: fälten `GRIDSQUARE`.
- QSO som används: 28, 50, 70 och 144 MHz med lokator. QSO med annat `PROP_MODE` (TR, MS, FAI, AUR, EME...) och sådana under 500 km kan uteslutas. 432 MHz tas aldrig med: sporadiskt E gäller inte där.

## Dagar och timmar

- **QSO per dag**: dag, antal QSO, band, största avstånd, multihopp. När fliken öppnas föreslår programmet dagen med flest QSO.
- **Dagens timmar**: raden *Hela dagen* och en rad för varje UTC-timme; ett klick visar bara den timmens QSO.
- **QSO**: listan för urvalet; ett klick på ett QSO (eller nära motstationen på kartan) markerar det och visar dess detaljer.

## Kartan

**Azimutal ekvidistant** karta centrerad på stationen, med reglaget **Största avstånd** för att zooma (även med mushjulet): ringarna visar **km från mitten**.

- **Skip per sektor** (10° azimut): orange båge = närmaste QSO i sektorn (skipzonens kant), grön båge = det mest avlägsna QSO:t, däremellan det område som nåtts.
- **Es-moln** på Maidenhead-rutorna, färgade efter uppskattad foEs.
- Mittpunkt för enkelhopp (gul), stationens första hopp (orange, ihålig cirkel = kort första hopp), DX-stationens första hopp (lila).
- För hela dagen förenar den **röda linjen** mittpunkternas tyngdpunkt timme för timme: det är molnens drift.

![En timme](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Föreslagna beräkningar

1. **Skip per sektor**: för varje sektor skipavståndet och räckvidden, med anropssignalerna.
2. **Uppskattad foEs** med sekantlagen (foEs ≥ f / sec φ): det kortaste enkelhoppet på varje band ger den högsta uppskattningen; med denna foEs, den förväntade MUF på 1000, 1500, 2000 och 2400 km och det avstånd bortom vilket 144 MHz skulle öppna.
3. **Es-moln** uppskattade på rutorna, med antal QSO och foEs.
4. **Multihopp**: stationens första hopp enligt de två hypoteserna (lika hopp, och kort första hopp inom 1200 km, IC8TEM:s erfarenhet) jämfört med de moln som setts i enkelhopp inom ±30 minuter; inom 300 km är hypotesen bekräftad. Programmet räknar bekräftelserna för varje hypotes.
5. **Molnens drift** timme för timme: km, riktning och hastighet.

Beräkningarna är vägledande: ett QSO säger att sträckan var öppen, inte exakt var molnet låg. Ju renare data (lokatorer med 6 tecken, korrekt `PROP_MODE`), desto mer tillförlitligt skip.
