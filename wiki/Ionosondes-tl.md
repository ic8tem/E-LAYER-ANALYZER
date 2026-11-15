# MGA IONOSONDE

[← Home](Home-tl)

![IONOSONDE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Sub-tab ng **SPORADIC-E** (mula sa bersiyon 0.5.2): ang **sinukat na foEs** ng mga ionosonde ng network na **GIRO** sa buong mundo, katabi ng hinahango ng programa mula sa mga spot.

## Ang datos

- 63 GIRO ionosonde (listahan ng ULCAR), na may huling **foEs**, **foF2** at **MUF(D)** na awtomatikong binasa mula sa mga ionogram.
- Kusang dumarating ang datos sa unang pagbukas ng tab at pagkatapos ay **tuwing 15 minuto** (kahon na *Kusang i-update tuwing 15 minuto*); hinihingi ito agad ng *I-update ngayon*. Iniingatan ng archive ang huling 48 oras sa folder na `giro` ng datos.
- Datos ng **GIRO** (Reinisch at Galkin 2011), lisensiyang **CC BY-NC-SA 4.0**: para lamang sa hindi pangkomersiyal na gamit. Ang serbisyo ay pareho sa *MUF Muncher* ni DM6AX.

## Mapa at talahanayan

- Ang bawat ionosonde ay isang tuldok na may kulay ayon sa huling foEs (parehong mga kulay ng mga ulap ng Es) at ang halaga sa MHz; **abo** kung walang bagong datos, may **singsing** kung higit sa 90 minuto na ang huling ionogram.
- Talahanayan: Code, Ionosonde, km mula sa iyong istasyon, UTC ng huling ionogram, foEs, foF2, MUF(D) at **Spot foEs≥**: ang pinakamababang foEs na hinango mula sa mga Es spot ng huling 30 minuto na nagre-reflect sa loob ng 500 km mula sa ionosonde (naaayos), kasama ang bilang ng mga spot.

## Ang napiling ionosonde

I-click ang isang hilera o malapit sa isang ionosonde sa mapa:

- **graph ng huling 24 oras**: dilaw na foEs, mapusyaw na asul na foF2, lilang tuldok-tuldok na MUF(D);
- **MUF ng E-sporadic** sa 1000, 1500, 2000 at 2400 km (MUF = foEs · sec φ, sa napiling taas ng Es) kasama ang **mga bandang nagbubukas** (28, 40, 50, 70, 144 MHz);
- kung mas mataas ang hinihingi ng mga spot kaysa sa sinukat na foEs, ipinapaalam ito ng programa: maaaring malayo ang ulap sa ionosonde, o naabot na ng ionosonde ang hangganan ng saklaw ng frequency nito (Chartier 2022).
