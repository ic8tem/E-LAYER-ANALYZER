# E-LAYER ANALYZER

*parengė Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Português (Brasil)](Home-pt-br) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · **Lietuvių** · [Latviešu](Home-lv) · [Polski](Home-pl) · [Čeština](Home-cs) · [Magyar](Home-hu) · [Română](Home-ro) · [Ελληνικά](Home-el) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [ไทย](Home-th) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · [中文 (繁體)](Home-zh-tw)

**E-LAYER ANALYZER** – nemokama programa radijo mėgėjams, analizuojanti visus VHF sklidimo būdus, kuriuose dalyvauja jonosferos **E sluoksnis**: **sporadinį E** (taip pat tarpžemyninį kelių šuolių), **F.A.I.** (išilgai geomagnetinio lauko išsidėsčiusius netolygumus), **meteor scatter** (su įtaisyta programa YA-MS) ir **radijo pašvaistę**. Ji veikia **Windows (64 ir 32 bitų), Linux, Raspberry Pi ir Mac** sistemose, **36 kalbomis**, ir nieko nerašo už savo aplanko ribų.

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Kortelės

| Kortelė | Ką ji daro |
|---|---|
| [SPORADIC-E](Sporadic-E-lt) | Es spotai žemėlapyje, abiejų stočių pirmas šuolis, Es debesys Maidenhead kvadratuose, MUF jūsų stočiai |
| [BENCHMARK Es](Benchmark-Es-lt) | Bandymų stendas visam ADIF žurnalui (ir kitos stoties): QSO pagal dienas ir valandas, skip pagal sektorius, įvertinta foEs, kelių šuolių patikra, debesų slinkimas |
| [BENCHMARK 50 MHz](Benchmark-50-lt) | Kelių šuolių sporadinis E 50 MHz juostoje pagal vieną ar du ADIF žurnalus: Merkatoriaus žemėlapis, diena ir laikas, neprivaloma foEs, padidinta trasa su WWL, QSO 150 km spinduliu |
| [JONOZONDAI](Ionosondes-lt) | GIRO jonozondų foEs, foF2 ir MUF(D) realiuoju laiku, 24 valandų grafikas, sporadinio E MUF su atsiveriančiomis juostomis, palyginimas su spotais |
| [Es DEBESYS LAIKE](Es-Clouds-Over-Time-lt) | Es debesys žingsnis po žingsnio pagal Chartier 2022 metodą: mažiausia foEs ir NmE langelyje, iš sesijos spotų, ADIF žurnalo arba CSV spotų |
| [F.A.I.](FAI-lt) | Stočių poros sklaidos taškų Scatter trail ir izoatspindžio linijos taškui patikrinti |
| [METEOR SCATTER](Meteor-Scatter-lt) | Visa programa YA-MS (srautų efektyvumas, radarai, prognozės) |
| [AURORA](Aurora-lt) | Pašvaistės ovalas, NOAA Kp ir Saulės vėjas, pašvaistės Scatter trail ir izoatspindys, rekomenduojama antenos kryptis |
| [DATA COLLECTION](Data-Collection-lt) | Trys telnet seansai (DX klasteris, RBN, DM4X su PSKReporter), surinkti spotai, CALL3 duomenų bazė |
| [LOG ADIF](Log-ADIF-lt) | Dirbti ir patvirtinti kvadratai kiekvienoje juostoje nuo 28 MHz ir aukščiau, iš jūsų ADIF arba iš LoTW |
| [ŠALTINIAI, SKAIČIAVIMO SPRENDIMŲ SANTRAUKA, NAUDOTOJO VADOVAS, TECHNINĖ ATASKAITA](Sources-and-Technical-Document-lt) | Šaltiniai, formulės ir skaičiavimo sprendimai, išsamus naudotojo vadovas, techninė ataskaita: galima eksportuoti į Word |

## Pradžia

1. [Diegimas](Installation-lt)
2. [Pirmasis paleidimas](Getting-Started-lt)
3. [Spalvos ir kalbos](Colours-and-Languages-lt)
4. [Duomenų aplankas](Data-Folder-lt)
5. [Dažnai užduodami klausimai](FAQ-lt)

## Pagrindiniai šaltiniai

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV, sporadinio E atstumų lentelė ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)).
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (YA-MS šaltinis).

Visas sąrašas pateiktas programos kortelėje ŠALTINIAI.

## Licencija

E-LAYER ANALYZER yra laisva programinė įranga, licencijuota pagal **GNU GPL-3.0** (failas COPYING), tą pačią kaip YA-MS. Ji platinama be jokių garantijų. Išorinių šaltinių duomenys lieka atitinkamų institucijų nuosavybe, taikant jų sąlygas (pavyzdžiui, GIRO duomenys: CC BY-NC-SA 4.0, tik nekomerciniam naudojimui).
