# JÓNÓSÖNDUR

[← Home](Home-is)

![JÓNÓSÖNDUR](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Undirflipi **SPORADIC-E** (frá útgáfu 0.5.2): **foEs mæld** af jónósöndum **GIRO**-netsins um allan heim, við hlið þeirrar sem forritið leiðir af spotunum.

## Gögnin

- 63 GIRO-jónósöndur (ULCAR-listinn), með síðustu **foEs**, **foF2** og **MUF(D)** lesin sjálfvirkt úr jónóritunum.
- Gögnin berast þegar flipinn er fyrst opnaður og síðan **á 15 mínútna fresti**, sjálfkrafa (reiturinn *Uppfæra sjálfkrafa á 15 mínútna fresti*); *Uppfæra núna* sækir þau strax. Safnið geymir síðustu 48 klukkustundir í möppunni `giro` í gögnunum.
- Gögn frá **GIRO** (Reinisch og Galkin 2011), leyfi **CC BY-NC-SA 4.0**: aðeins til notkunar í öðrum en viðskiptalegum tilgangi. Þjónustan er sú sama og í *MUF Muncher* eftir DM6AX.

## Kort og tafla

- Hver jónósanda er litaður punktur með síðustu foEs (sömu litir og Es-skýin) og gildinu í MHz; **grá** ef hún hefur engin nýleg gögn, með **hring** ef síðasta jónóritið er eldra en 90 mínútna.
- Tafla: Kóði, Jónósanda, km frá stöðinni þinni, UTC síðasta jónórits, foEs, foF2, MUF(D) og **Spot foEs≥**: lágmarks-foEs leidd af Es-spotum síðustu 30 mínútna sem endurvarpast innan 500 km frá jónósöndunni (stillanlegt), með fjölda spotanna.

## Valin jónósanda

Smelltu á línu eða nálægt jónósöndu á kortinu:

- **graf síðustu 24 klukkustunda**: foEs gul, foF2 ljósblá, MUF(D) fjólublá punktalína;
- **MUF sporadic-E** á 1000, 1500, 2000 og 2400 km (MUF = foEs · sec φ, í valinni Es-hæð) með **tíðnisviðunum sem opnast** (28, 40, 50, 70, 144 MHz);
- ef spotin krefjast meira en mældrar foEs lætur forritið vita: skýið getur verið fjarri jónósöndunni, eða jónósandan er komin að mörkum tíðnisviðs síns (Chartier 2022).
