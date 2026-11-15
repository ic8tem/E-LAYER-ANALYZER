# JONOZONDAI

[← Home](Home-lt)

![JONOZONDAI](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

**SPORADIC-E** vidinė kortelė (nuo 0.5.2 versijos): **GIRO** tinklo jonozondų **išmatuota foEs** visame pasaulyje, šalia tos, kurią programa išveda iš spotų.

## Duomenys

- 62 GIRO jonozondai (ULCAR sąrašas), su paskutine **foEs**, **foF2** ir **MUF(D)**, automatiškai nuskaitytomis iš jonogramų.
- Duomenys gaunami pirmą kartą atvėrus kortelę, o vėliau **kas 15 minučių**, automatiškai (žymimasis langelis *Atnaujinti automatiškai kas 15 minučių*); *Atnaujinti dabar* juos užklausia iš karto. Archyve saugomos paskutinės 48 valandos duomenų aplanke `giro`.
- **GIRO** duomenys (Reinisch ir Galkin 2011), licencija **CC BY-NC-SA 4.0**: tik nekomerciniam naudojimui. Paslauga ta pati kaip DM6AX programoje *MUF Muncher*.

## Žemėlapis ir lentelė

- Kiekvienas jonozondas yra spalvotas taškas su paskutine foEs (tos pačios spalvos kaip Es debesų) ir reikšme MHz; **pilkas**, jei naujų duomenų nėra, su **žiedu**, jei paskutinė jonograma senesnė nei 90 minučių.
- Lentelė: Kodas, Jonozondas, km nuo jūsų stoties, paskutinės jonogramos UTC, foEs, foF2, MUF(D) ir **Spotai foEs≥**: mažiausia foEs, išvesta iš paskutinių 30 minučių Es spotų, atsispindinčių iki 500 km nuo jonozondo (keičiama), su spotų skaičiumi.

## Pasirinktas jonozondas

Spustelėkite eilutę arba šalia jonozondo žemėlapyje:

- **paskutinių 24 valandų grafikas**: foEs geltona, foF2 žydra, MUF(D) violetinė taškinė;
- **sporadinio E MUF** 1000, 1500, 2000 ir 2400 km atstumu (MUF = foEs · sec φ, pasirinktame Es aukštyje) su **atsiveriančiomis juostomis** (28, 40, 50, 70, 144 MHz);
- jei spotams reikia daugiau nei išmatuota foEs, programa tai praneša: debesis gali būti toli nuo jonozondo arba jonozondas pasiekė savo dažnių diapazono ribą (Chartier 2022).
