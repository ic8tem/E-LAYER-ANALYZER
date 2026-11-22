# JONOZONDI

[← Home](Home-lv)

![JONOZONDI](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

**SPORADIC-E** apakšcilne (kopš versijas 0.5.2): **GIRO** tīkla jonozondu **izmērītā foEs** visā pasaulē, blakus tai, ko programma secina no spotiem.

## Dati

- 62 GIRO jonozondi (ULCAR saraksts) ar pēdējo **foEs**, **foF2** un **MUF(D)**, kas automātiski nolasīti no jonogrammām.
- Dati tiek iegūti, pirmo reizi atverot cilni, un pēc tam **ik pēc 15 minūtēm**, automātiski (izvēles rūtiņa *Atjaunināt automātiski ik pēc 15 minūtēm*); *Atjaunināt tagad* tos pieprasa nekavējoties. Arhīvs glabā pēdējās 48 stundas mapē `giro`.
- **GIRO** dati (Reinisch un Galkin 2011), licence **CC BY-NC-SA 4.0**: tikai nekomerciālai lietošanai. Pakalpojums ir tas pats, ko izmanto DM6AX programma *MUF Muncher*.

## Karte un tabula

- Katrs jonozonds ir krāsains punkts ar pēdējo foEs (tās pašas krāsas kā Es mākoņiem) un vērtību MHz; **pelēks**, ja nav svaigu datu, ar **apli**, ja pēdējā jonogramma ir vecāka par 90 minūtēm.
- Tabula: Kods, Jonozonds, km no jūsu stacijas, pēdējās jonogrammas UTC, foEs, foF2, MUF(D) un **Spoti foEs≥**: minimālā foEs, kas secināta no pēdējo 30 minūšu Es spotiem, kas atstarojas līdz 500 km no jonozonda (maināms), ar spotu skaitu.
- **h'Es GIRO** (no versijas 0.6.5, rūtiņa cilnē REAL-TIME): par Es augstumu kļūst jaunāko jonozondu h'Es mediāna 2500 km rādiusā (80-120 km).

## Izvēlētais jonozonds

Noklikšķiniet uz rindas vai jonozonda tuvumā kartē:

- **pēdējo 24 stundu grafiks**: foEs dzeltena, foF2 ciānzila, MUF(D) violeta punktēta;
- **sporādiskā E MUF** 1000, 1500, 2000 un 2400 km attālumā (MUF = foEs · sec φ, izvēlētajā Es augstumā) ar **joslām, kas atveras** (28, 40, 50, 70, 144 MHz);
- ja spotiem vajag vairāk nekā izmērītā foEs, programma to norāda: mākonis var būt tālu no jonozonda vai jonozonds ir sasniedzis sava frekvenču diapazona robežu (Chartier 2022).
