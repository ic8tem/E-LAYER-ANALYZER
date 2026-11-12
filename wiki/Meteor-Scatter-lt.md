# METEOR SCATTER

[← Home](Home-lt)

Šioje kortelėje yra visa **YA-MS 1.8.0** – IC8TEM meteor scatter programa: dangaus skliautas su radiantais, SKiYMET radarų srautas (Collm, Sodankylä), srautų efektyvumo skaičiavimas, eksperimentinis prognozių įgyvendinimas, geometrija ir slopinimas, GRAVES, kvadratų žemėlapis, **Srautai realiuoju laiku** (Global Meteor Network išmatuoti ZHR ir CMOR radaro radiantai), **Šaltinių žemėlapis** (Collm, Sodankylä, GRAVES ir CMOR stoties atžvilgiu) bei dokumentas su formulėmis **Efektyvumas - paaiškinimas**.

YA-MS 1.5.0 prideda kortelę **Duomenų rinkimas** (meteor scatter spotai, filtruojami pagal MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). E-LAYER ANALYZER viduje ji neatveria savų telnet seansų: ji gauna DATA COLLECTION seansų eilutes, todėl nėra dvigubų prisijungimų tuo pačiu šaukiniu.

- YA-MS naudoja E-LAYER ANALYZER viršuje pasirinktą kalbą ir spalvų schemą.
- YA-MS nustatymai saugomi faile `data/config.ini`.
- Kiekviena nauja YA-MS versija perkeliama scenarijumi `tools/sync_yams.py`.

Išsamiau – [YA-MS saugykloje](https://github.com/ic8tem/YA-MS).
