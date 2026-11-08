# BENCHMARK 50 MHz

[← Home](Home-af)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Toetsbank vir **multi-sprong sporadiese E op 50 MHz** met die ADIF-log van een stasie en, opsioneel, van 'n tweede stasie.

## Die logs laai

- **Stasie 1**: *Laai ADIF* / *Verwyder ADIF*. Die program neem `STATION_CALLSIGN` en `MY_GRIDSQUARE` uit die log; die uitsaaiposisie is die `MY_GRIDSQUARE` van elke QSO.
- **Stasie 2** (opsioneel): dieselfde knoppies op die tweede ry; die merkblokkie *Stasie 2* wys of verberg dit. Stasie 1 se paaie is ligblou, stasie 2 se paaie pers.
- Die program gebruik 50 MHz-QSO's met 'n lokator (ontbrekende lokators of lokators met 4 karakters word uit CALL3 geneem), multi-sprong: verder as 3100 km (outeur se reël; die aantal spronge volg die IK3XTV-tabel).
- **Transekwatoriale** kontakte word uitgesluit (outeur se reël): na Afrika en Suid-Amerika, met die middelpunt in die magnetiese inklinasieband tussen -20° en +20°, tensy die log `PROP_MODE=ES` het.

## Dag en tyd

Kies die **dag** (by die oopmaak dié met die meeste multi-sprong-QSO's) en die tyd **van… tot…** UTC, in stappe van 15 minute. Die merkblokkie **foEs** (opsioneel) kleur die paaie volgens minimum foEs en wys dit in die tabel.

## Die kaart

**Mercator**-kaart met 'n vrye middelpunt: wieletjie = zoem, sleep = skuif.

![Pad](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Muis naby 'n pad**: die lyn en ikone word vergroot en die DX-stasie se **WWL-vierkant van 6 karakters** verskyn.
- **Linksklik** op daardie pad: 'n opspringvenster vra of **al die QSO's in die lêer** (alle dae) gewys moet word waarvan die verre kant **binne 150 km** van daardie punt is. *Ja*: die kaart word skoongemaak en wys daardie QSO's binne die oranje sirkel; *Nee*: die kaart bly soos dit is. *Terug na dag en tyd* keer terug na die vorige keuse.

![Omgewing](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Besonderhede

Vir die gekose pad: afstand en gradering volgens die IK3XTV-tabel, azimut in albei rigtings, gelyke spronge met die vierkante van die refleksiepunte, kort eerste sprong binne 1200 km (IC8TEM se ervaring) en, met die foEs-merkblokkie, die minimum foEs.
