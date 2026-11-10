# BENCHMARK Es

[← Home](Home-nl)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Testbank voor **sporadische E** en **meervoudige sprongen** op een **volledig ADIF-log**, ook van een ander station: uit de werkelijk gemaakte QSO's schetst het programma de skip en de wolken, dag na dag en uur na uur.

## Het log laden

- **ADIF laden**: kiest het `.adi`/`.adif`-bestand. Het programma onthoudt het bestand en laadt het bij het opstarten opnieuw.
- **ADIF verwijderen**: haalt het log uit het tabblad.
- **Station in het midden**: roepnaam en locator van het station dat het log heeft gemaakt. Het programma stelt de vaakst voorkomende `STATION_CALLSIGN` (of `OPERATOR`) en `MY_GRIDSQUARE` voor; je kunt ze met de hand wijzigen en met **Toepassen** bevestigen. Het midden hangt niet af van de algemene instellingen, zodat je ook logs van andere stations kunt bestuderen.
- Zendpositie: de `MY_GRIDSQUARE` van elke QSO (als die ontbreekt, de locator in het midden). Ontvangstposities: de velden `GRIDSQUARE`.
- Gebruikte QSO's: 28, 50, 70 en 144 MHz met een locator. QSO's met een andere `PROP_MODE` (TR, MS, FAI, AUR, EME...) en die onder 500 km kunnen worden uitgesloten. 432 MHz wordt nooit meegenomen: sporadische E is daar niet van toepassing.

## Dagen en uren

- **QSO's per dag**: dag, aantal QSO's, banden, maximale afstand, meervoudige sprongen. Bij het openen stelt het programma de dag met de meeste QSO's voor.
- **Uren van de dag**: de regel *Hele dag* en een regel voor elk UTC-uur; een klik toont alleen de QSO's van dat uur.
- **QSO**: de lijst voor de selectie; een klik op een QSO (of op de kaart bij het tegenstation) markeert het en toont de details.

## De kaart

**Azimutale equidistante** kaart gecentreerd op het station, met de schuifregelaar **Maximale afstand** om te zoomen (ook met het scrollwiel): de ringen tonen de **km vanaf het midden**.

- **Skip per sector** (10° azimut): oranje boog = dichtstbijzijnde QSO van de sector (rand van de skipzone), groene boog = verste QSO, daartussen het bereikte gebied.
- **Es-wolken** op de Maidenhead-vakken, gekleurd naar de geschatte foEs.
- Middelpunt van de enkele sprongen (geel), eerste sprong van het station (oranje, open cirkel = korte eerste sprong), eerste sprong van het tegenstation (paars).
- Een **vage gele stip** markeert het midden van elke sprong, waar de Es-wolk vermoed wordt (ook het midden van de korte eerste sprong, als die hypothese actief is).
- Voor de hele dag verbindt de **rode lijn** uur na uur het zwaartepunt van de middelpunten: dat is de drift van de wolken.

![Eén uur](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Voorgestelde berekeningen

1. **Skip per sector**: voor elke sector de skipafstand en het bereik, met de roepnamen.
2. **Geschatte foEs** met de secanswet (foEs ≥ f / sec φ): de kortste enkele sprong op elke band geeft de hoogste schatting; met deze foEs de verwachte MUF op 1000, 1500, 2000 en 2400 km en de afstand waarboven 144 MHz zou opengaan.
3. **Es-wolken** geschat op de vakken, met het aantal QSO's en de foEs.
4. **Meervoudige sprong**: de eerste sprong van het station onder de twee hypotheses (gelijke sprongen, en korte eerste sprong binnen 1200 km, ervaring van IC8TEM), vergeleken met de wolken die binnen ±30 minuten met één sprong zijn gezien; binnen 300 km is de hypothese bevestigd. Het programma telt de bevestigingen van elke hypothese.
5. **Drift van de wolken** uur na uur: km, richting en snelheid.

De berekeningen zijn indicatief: een QSO zegt dat het traject open was, niet precies waar de wolk was. Hoe schoner de gegevens (locators van 6 tekens, juiste `PROP_MODE`), hoe betrouwbaarder de skip.
