# F.A.I.

[← Home](Home-tl)

Ang F.A.I. (Field Aligned Irregularities) ay scatter mula sa mga iregularidad na nakahanay sa geomagnetic field, karaniwan sa mga gabi ng tag-init pagkatapos ng sporadic-E. Lumalabas ang na-scatter na signal sa isang **cone** na ang axis ay nasa direksiyon ng field: maaaring magkontak ang dalawang istasyon kung nasa **magkasuplementong contour** sila (a1 + a2 = 180°, artikulo nina IC8FAX-IC8TEM; mga ±c contour ni K9SE). Kinakalkula ang magnetic field gamit ang modelong **IGRF-14**.

## Tanaw na scatter trail (pares A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Ilagay ang **Istasyon A** at **Istasyon B** (callsign na hinahanap sa CALL3 at/o locator). Kapag walang laman ang A = ang sarili mong istasyon.
- Ipinapakita ng mapa ang **scatter trail**: lahat ng punto sa taas ng layer, nakikita ng dalawang istasyon, na tumutugon sa kondisyon ng repleksiyon sa loob ng 2.5° (mga klaseng 0-1, 1-1.5, 1.5-2, 2-2.5°).
- Ipinapakita ng kahon sa kanang itaas ang **QTF at elebasyon** ng A (pula) at B (berde) patungo sa bawat punto.
- Iminumungkahi ng programa ang **pinakamalamang na scatter point** (pinakamaliit na paglihis, pinakamaikling landas).
- **Repraksiyon 4/3**: naka-on bilang default, pinahahaba nito ang mga horizon.

## Tanaw na iso-repleksiyon (mga iso-repleksiyong locus)

![Iso-repleksiyon](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Ito ang heometrikong konstruksiyong ginagamit para **suriin kung tama ang isang punto ng repleksiyon**.

- **Kaliwang click** sa mapa: pinipili mo ang puntong susuriin. **Kanang click**: inililipat ang istasyon B.
- **Suriin ang inirerekomendang punto**: binubuksan ang graph sa puntong iminungkahi ng programa.
- Mga contour line sa **0, ±1, ±2, ±4, ±8, ±16°**: may-kulay na banda sa madidilim na scheme, **makakapal na linya** sa maliliwanag na scheme.
- Hatol sa pares: **kasiya-siya** sa loob ng 1°, **katanggap-tanggap** sa loob ng 2.5°, **hindi kasiya-siya** kapag lampas.
- Kung ang punto ay nasa ilalim ng horizon ng isang istasyon, lumalabas ang popup na **Hindi posible ang kalkulasyon** at wala nang iba.

## Mga pagsusuri

| Contact | Scatter point | Paglihis | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (sa itaas ng Genoa) | 0.97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (timog-kanlurang Germany, 28 km mula sa Brussels) | 0.88° | 338° / 87° |
| Beacon DB0FAI (305°) - Dresden via Sheffield (DL5MCG; nakatutok sa 290° ang Dresden at idineklarang aurora) | – | 0.3° | – |
| DL5MCG - SV1DH (pagsubok noong 1987 via Budapest, 1569 km) | Budapest nasa bakas; mga cell na mas mababa sa 1° mula 35 km | 2.16° | 95° / 341° |

Mga kilalang scatter point sa listahan: Geneva, Brussels, Budapest, JN92, JN41/42.

Ikinakarga ng menu na **Mga pagsusuri...** ang mga pares na pansubok IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE at DL5MCG-SV1DH.

**Kagamitan** (DL5MCG): para sa F.A.I. sa 144 MHz, kailangan ng hindi bababa sa isang mahabang yagi at 100 W (nakontak din si SV1DH gamit ang yagi na may 8 elemento at 50 W). Itutok ang antena sa scatter point na ibinibigay ng programa, hindi sa kabilang istasyon.
