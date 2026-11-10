# BENCHMARK Es

[← Home](Home-et)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Katsestend **sporaadilise E** ja **mitmehüppelise levi** jaoks **terve ADIF-logi** peal, ka teise jaama logi peal: tegelikult peetud QSO-de põhjal visandab programm skipi ja pilved päev-päevalt ja tund-tunnilt.

## Logi laadimine

- **Laadi ADIF**: valib faili `.adi`/`.adif`. Programm jätab faili meelde ja laadib selle käivitamisel uuesti.
- **Eemalda ADIF**: eemaldab logi sakilt.
- **Jaam keskel**: logi pidanud jaama kutsung ja lokaator. Programm pakub kõige sagedasemat `STATION_CALLSIGN` (või `OPERATOR`) ja `MY_GRIDSQUARE` välja; neid saab käsitsi muuta ja nupuga **Rakenda** kinnitada. Keskpunkt ei sõltu üldseadetest, nii et uurida saab ka teiste jaamade logisid.
- Saatmiskoht: iga QSO `MY_GRIDSQUARE` (kui see puudub, siis keskel oleva jaama lokaator). Vastuvõtukohad: väljad `GRIDSQUARE`.
- Kasutatavad QSO-d: 28, 50, 70 ja 144 MHz koos lokaatoriga. Muu `PROP_MODE`-ga (TR, MS, FAI, AUR, EME...) ja alla 500 km QSO-d saab välja jätta. 432 MHz ei ole kunagi kaasatud: sporaadiline E selle kohta ei kehti.

## Päevad ja tunnid

- **QSO-d päevade kaupa**: päev, QSO-de arv, sagedusalad, suurim kaugus, mitmehüppelised. Avamisel pakub programm välja päeva, kus on kõige rohkem QSO-sid.
- **Päeva tunnid**: rida *Kogu päev* ja üks rida iga UTC tunni kohta; klõps näitab ainult selle tunni QSO-sid.
- **QSO**: valiku loetelu; QSO-l (või kaardil DX-jaama lähedal) klõpsamine tõstab selle esile ja näitab üksikasju.

## Kaart

Jaamale tsentreeritud **asimuutne kaugustruu** kaart, suurendamiseks liugur **Maksimaalne kaugus** (ka hiire rullikuga): ringid näitavad **kaugust keskpunktist km-des**.

- **Skip sektorite kaupa** (10° asimuuti): oranž kaar = sektori lähim QSO (vaikusetsooni serv), roheline kaar = kaugeim QSO, nende vahel saavutatud ala.
- **Es-pilved** Maidenheadi ruutudel, värvitud hinnangulise foEs järgi.
- Ühe hüppe keskpunkt (kollane), jaama esimene hüpe (oranž, õõnes ring = lühike esimene hüpe), DX-jaama esimene hüpe (lilla).
- **Hägune kollane täpp** tähistab iga hüppe keskpunkti, kus eeldatakse Es-pilve asukohta (ka lühikese esimese hüppe keskpunkti, kui see hüpotees on sisse lülitatud).
- Kogu päeva puhul ühendab **punane joon** keskpunktide raskuskeskmeid tund-tunnilt: see on pilvede nihkumine.

![Üks tund](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Pakutud arvutused

1. **Skip sektorite kaupa**: iga sektori skipi kaugus ja ulatus koos kutsungitega.
2. **Hinnanguline foEs** sekansi seaduse järgi (foEs ≥ f / sec φ): iga sagedusala lühim ühe hüppega side annab kõrgeima hinnangu; selle foEs-iga oodatav MUF 1000, 1500, 2000 ja 2400 km juures ning kaugus, millest alates avaneks 144 MHz.
3. Ruutudel hinnangulised **Es-pilved** koos QSO-de arvu ja foEs-iga.
4. **Mitmehüppeline levi**: jaama esimene hüpe kahe hüpoteesi järgi (võrdsed hüpped ning lühike esimene hüpe kuni 1200 km, IC8TEM-i kogemus) võrrelduna ±30 minuti jooksul ühe hüppega nähtud pilvedega; 300 km piires hüpotees kinnitub. Programm loendab iga hüpoteesi kinnitused.
5. **Pilvede nihkumine** tund-tunnilt: km, suund ja kiirus.

Arvutused on ligikaudsed: QSO näitab, et trass oli avatud, mitte täpselt seda, kus pilv asus. Mida puhtamad on andmed (6-märgilised lokaatorid, õige `PROP_MODE`), seda usaldusväärsem on skip.
