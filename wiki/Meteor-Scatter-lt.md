# METEOR SCATTER

[← Home](Home-lt)

Šioje kortelėje yra visa **YA-MS 1.8.6** – IC8TEM meteor scatter programa: dangaus skliautas su radiantais, SKiYMET radarų srautas (Collm, Sodankylä), srautų efektyvumo skaičiavimas, eksperimentinis prognozių įgyvendinimas, geometrija ir slopinimas, GRAVES, kvadratų žemėlapis, **Srautai realiuoju laiku** (Global Meteor Network išmatuoti ZHR ir CMOR radaro radiantai), **Šaltinių žemėlapis** (Collm, Sodankylä, GRAVES ir CMOR stoties atžvilgiu) bei dokumentas su formulėmis **Efektyvumo skaičiavimo paaiškinimas**.

YA-MS 1.5.0 prideda kortelę **Duomenų rinkimas** (meteor scatter spotai, filtruojami pagal MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). E-LAYER ANALYZER viduje ji neatveria savų telnet seansų: ji gauna DATA COLLECTION seansų eilutes, todėl nėra dvigubų prisijungimų tuo pačiu šaukiniu.

YA-MS 1.6.0 pertvarko korteles į grupes **Tiesiogiai ir realiojo laiko duomenys** (meteor scatter eismas realiuoju laiku su kiekvieno ryšio debesiu, radarų srautas, GRAVES, duomenų rinkimas) ir **Prognozės skaičiavimas** (prognozė, geometrija, efektyvumo skaičiavimas, prognozių įgyvendinimas).

YA-MS 1.7.0 ir 1.8.0 prideda radiantų palyginimą su IAU MDC katalogu, Global Meteor Network dienos skaičiavimus pagal srautą, iš populiacijos indeksų gautą k indeksą, pradinį pėdsako spindulį pagal Stober ir kt. (2021), pingų trukmę skirtingose juostose (numatytasis λ² arba NN4SA HamSCI MSQP matavimai) ir palyginimą su BRAMS aptikimais Sonneberge (Perseidai 2026). Automatinis YA-MS atnaujinimas E-LAYER ANALYZER viduje išjungtas: E-LAYER ANALYZER atsinaujina pats iš GitHub Releases.

YA-MS 1.8.1 antenos skiltį laiko vertikaliąja diagrama (langelis «antenos diagrama», numatytasis įjungtas): atspindžio taškai sveriami abiejų antenų vertikaliuoju stiprinimu, ir efektyvumai ilgose trasose nebedingsta. Nauja: įspėjimas «nėra bendros zonos», paryškintas kampo β langelis (labiausiai sveriantis pasirinkimas), viena skaičiavimo data, «Naudotojo vadovas» ir palyginimas su 181 767 tikrais 50 MHz spotais iš HamSCI Meteor Scatter QSO Party 2025 m. rugpjūčio: valandos ir trasos pusės patikimos, procentai orientaciniai.

- YA-MS naudoja E-LAYER ANALYZER viršuje pasirinktą kalbą ir spalvų schemą.
- YA-MS nustatymai saugomi faile `data/config.ini`.
- Kiekviena nauja YA-MS versija perkeliama scenarijumi `tools/sync_yams.py`.

Išsamiau – [YA-MS saugykloje](https://github.com/ic8tem/YA-MS).
