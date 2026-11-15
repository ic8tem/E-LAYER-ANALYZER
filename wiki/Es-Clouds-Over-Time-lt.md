# Es DEBESYS LAIKE

[← Home](Home-lt)

![Es DEBESYS LAIKE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

**SPORADIC-E** vidinė kortelė (nuo 0.5.2 versijos): kaip **sporadinio E debesys** atsiranda, juda ir užgęsta, žingsnis po žingsnio.

## Metodas

**Chartier, Hanley ir Emmons (2022)** metodas: kiekvienas sporadinio E ryšys rodo, kad atspindžio taške foEs yra **ne mažesnė kaip f / sec φ**. Taškai renkami į langelius ir laiko žingsnius, o kiekvienas langelis gauna **didžiausią** žingsnio **reikšmę**. Mažiausias elektronų tankis yra NmE = 1,24·10¹⁰ · foEs² elektronų kubiniame metre.

Programa naudoja savo modelio atspindžio taškus (visus šuolius), o ne tik vidurio tašką. Reikšmė yra **apatinė riba**: tuščias langelis reiškia tik tai, kad ryšių nėra.

## Šaltinis ir valdikliai

- **Sesijos spotai** (atnaujinami kaskart atvėrus kortelę) arba **Įkelti ADIF žurnalą arba CSV spotus**.
- **Eksportuoti sesijos spotus (CSV)**: spotų archyvas ištuštinamas uždarius programą, todėl juos galėsite peržiūrėti vėliau.
- **Juostos MHz** (28, 40, 50, 70, 144), **Žingsnis** (5, 10 arba 15 min.), **Langelis** (1°, 2° arba 4°), **Es aukštis km**.
- Laiko juosta: ⏮ ◀ *Paleisti/Sustabdyti* ▶ ⏭ ir slankiklis; atvėrus pradedama nuo žingsnio su daugiausia langelių.
- *Ankstesni žingsniai blankiai* rodo tris žingsnius prieš pasirinktą; *Ryšiai* nubraižo trasas juostų spalvomis.

## Lentelė ir grafikas

- Žingsnio langeliai: Langelis (pietvakarių kampas ir centro lokatorius), foEs≥, NmE≥, Ryšiai, Juostos. Spustelėjus langelį paryškinami jo ryšiai.
- Grafikas: didžiausia mažiausia foEs (geltona linija) ir langelių skaičius (stulpeliai) kiekvienam žingsniui.
