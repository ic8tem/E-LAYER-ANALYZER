# IONOSONDES

[← Home](Home-nl)

![IONOSONDES](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Subtab van **SPORADIC-E** (vanaf versie 0.5.2): de **gemeten foEs** van de ionosondes van het **GIRO**-netwerk over de hele wereld, naast de foEs die het programma uit de spots afleidt.

## De gegevens

- 62 GIRO-ionosondes (ULCAR-lijst), met de laatste **foEs**, **foF2** en **MUF(D)**, automatisch uit de ionogrammen gelezen.
- De gegevens komen binnen bij het eerste openen van het tabblad en daarna **elke 15 minuten**, vanzelf (vakje *Elke 15 minuten vanzelf bijwerken*); *Nu bijwerken* vraagt ze meteen op. Het archief bewaart de laatste 48 uur in de map `giro` van de gegevens.
- Gegevens van **GIRO** (Reinisch en Galkin 2011), licentie **CC BY-NC-SA 4.0**: alleen niet-commercieel gebruik. De dienst is dezelfde als in *MUF Muncher* van DM6AX.

## Kaart en tabel

- Elke ionosonde is een gekleurde stip met de laatste foEs (dezelfde kleuren als de Es-wolken) en de waarde in MHz; **grijs** als ze geen recente gegevens heeft, met de **ring** als het laatste ionogram ouder is dan 90 minuten.
- Tabel: Code, Ionosonde, km vanaf je station, UTC van het laatste ionogram, foEs, foF2, MUF(D) en **Spot foEs≥**: de minimale foEs afgeleid uit de Es-spots van de laatste 30 minuten die binnen 500 km van de ionosonde reflecteren (instelbaar), met het aantal spots.
- **h'Es GIRO** (vanaf versie 0.6.5, vakje in REAL-TIME): de Es-hoogte wordt de mediaan van de recente h'Es van de ionosondes binnen 2500 km (80-120 km).

## De gekozen ionosonde

Klik op een rij of naast een ionosonde op de kaart:

- **grafiek van de laatste 24 uur**: foEs geel, foF2 lichtblauw, MUF(D) paars gestippeld;
- **MUF van de sporadische E** op 1000, 1500, 2000 en 2400 km (MUF = foEs · sec φ, op de gekozen Es-hoogte) met de **banden die opengaan** (28, 40, 50, 70, 144 MHz);
- als de spots meer vragen dan de gemeten foEs, meldt het programma dat: de wolk kan ver van de ionosonde liggen, of de ionosonde heeft de grens van haar frequentiebereik bereikt (Chartier 2022).
