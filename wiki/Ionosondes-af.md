# IONOSONDES

[← Home](Home-af)

![IONOSONDES](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Suboortjie van **SPORADIC-E** (vanaf weergawe 0.5.2): die **gemete foEs** van die ionosondes van die **GIRO**-netwerk wêreldwyd, langs dié wat die program uit die spots aflei.

## Die data

- 62 GIRO-ionosondes (ULCAR-lys), met die jongste **foEs**, **foF2** en **MUF(D)** outomaties uit die ionogramme gelees.
- Die data kom die eerste keer wat die oortjie oopgemaak word en daarna **elke 15 minute** vanself (blokkie *Werk self elke 15 minute by*); *Werk nou by* vra dit dadelik. Die argief hou die laaste 48 uur in die `giro`-gids van die data.
- Data van **GIRO** (Reinisch en Galkin 2011), lisensie **CC BY-NC-SA 4.0**: slegs nie-kommersiële gebruik. Die diens is dieselfde as dié van *MUF Muncher* van DM6AX.

## Kaart en tabel

- Elke ionosonde is 'n gekleurde kolletjie met die jongste foEs (dieselfde kleure as die Es-wolke) en die waarde in MHz; **grys** as dit geen onlangse data het nie, met die **ring** as die laaste ionogram ouer as 90 minute is.
- Tabel: Kode, Ionosonde, km vanaf jou stasie, UTC van die laaste ionogram, foEs, foF2, MUF(D) en **Spots foEs≥**: die minimum foEs afgelei van die Es-spots van die laaste 30 minute wat binne 500 km van die ionosonde weerkaats (verstelbaar), met die aantal spots.

## Die gekose ionosonde

Klik op 'n ry of naby 'n ionosonde op die kaart:

- **grafiek van die laaste 24 uur**: foEs geel, foF2 ligblou, MUF(D) pers gestippeld;
- **sporadiese-E-MUF** op 1000, 1500, 2000 en 2400 km (MUF = foEs · sec φ, op die gekose Es-hoogte) met die **bande wat oopgaan** (28, 40, 50, 70, 144 MHz);
- as die spots meer vra as die gemete foEs, dui die program dit aan: die wolk kan ver van die ionosonde wees, of die ionosonde het die grens van sy frekwensiebereik bereik (Chartier 2022).
