# F.A.I.

[← Home](Home-is)

F.A.I. (Field Aligned Irregularities) er dreifing frá óreglum sem raðast eftir segulsviði jarðar, dæmigerð á sumarkvöldum eftir sporadic-E. Dreifða merkið fer út eftir **keilu** sem hefur ás sinn eftir sviðinu: tvær stöðvar geta náð sambandi ef þær liggja á **viðbótarferlum** (a1 + a2 = 180°, grein IC8FAX-IC8TEM; ±c-ferlar K9SE). Segulsviðið er reiknað með líkaninu **IGRF-14**.

## Sýnin Scatter trail (par A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Sláðu inn **Stöð A** og **Stöð B** (kallmerki sem flett er upp í CALL3 og/eða locator). A skilin eftir auð = þín eigin stöð.
- Kortið sýnir **scatter trail**: alla punkta í hæð lagsins, sem báðar stöðvar sjá, sem uppfylla endurvarpsskilyrðið innan 2,5° (flokkar 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Reiturinn efst til hægri sýnir **QTF og hæðarhorn** A (rautt) og B (grænt) að hverjum punkti.
- Forritið leggur til **líklegasta scatter point** (minnsta frávik, stysta leið).
- **Ljósbrot 4/3**: kveikt sjálfgefið, víkkar sjóndeildarhringina.

## Sýnin Jafnendurvarp (jafnendurvarpslínur)

![Jafnendurvarp](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Þetta er rúmfræðilega smíðin sem notuð er til að **prófa hvort endurvarpspunktur sé réttur**.

- **Vinstri smellur** á kortið: þú velur punktinn sem á að prófa. **Hægri smellur**: færir stöð B.
- **Prófa ráðlagðan punkt**: opnar grafið á punktinum sem forritið leggur til.
- Ferlar við **0, ±1, ±2, ±4, ±8, ±16°**: lituð belti í dökku litaþemunum, **þykkar línur** í ljósu þemunum.
- Niðurstaða fyrir parið: **fullnægjandi** innan 1°, **ásættanlegt** innan 2,5°, **ófullnægjandi** þar fyrir utan.
- Ef punkturinn er undir sjóndeildarhring annarrar stöðvarinnar birtist sprettiglugginn **Útreikningur ekki mögulegur** og ekkert annað.

## Prófanir

| Samband | Scatter point | Frávik | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (yfir Genúa) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (suðvestur-Þýskaland, 28 km frá Brussel) | 0,88° | 338° / 87° |
| DB0FAI - Dresden um Sheffield (DL5MCG) | – | 0,3° | – |

Þekktir scatter point á listanum: Genf, Brussel, Búdapest, JN92, JN41/42.
