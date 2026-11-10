# F.A.I.

[← Domů](Home-cs)

F.A.I. (Field Aligned Irregularities) je rozptyl na nehomogenitách orientovaných podél geomagnetického pole, typický pro letní večery po sporadické vrstvě E. Rozptýlený signál vychází po **kuželu**, jehož osa leží podél pole: dvě stanice spolu mohou pracovat, pokud leží na **doplňkových obrysech** (a1 + a2 = 180°, článek IC8FAX-IC8TEM; obrysy ±c podle K9SE). Magnetické pole se počítá modelem **IGRF-14**.

## Zobrazení Scatter trail (dvojice A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Zadejte **Stanici A** a **Stanici B** (značka hledaná v CALL3 a/nebo lokátor). Prázdné A = vaše vlastní stanice.
- Mapa ukazuje **scatter trail**: všechny body ve výšce vrstvy, viděné oběma stanicemi, které splňují podmínku odrazu do 2,5° (třídy 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Rámeček vpravo nahoře ukazuje **QTF a elevaci** A (červeně) a B (zeleně) ke každému bodu.
- Program navrhne **nejpravděpodobnější bod rozptylu** (minimální odchylka, nejkratší trasa).
- **Refrakce 4/3**: ve výchozím stavu zapnutá, rozšiřuje obzory.

## Zobrazení Izoflexe (isoflection loci)

![Izoflexe](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Je to geometrická konstrukce sloužící k **ověření, zda je bod odrazu správný**.

- **Levé kliknutí** na mapu: zvolíte bod k ověření. **Pravé kliknutí**: přesune stanici B.
- **Ověřit doporučený bod**: otevře graf na bodu navrženém programem.
- Vrstevnice na **0, ±1, ±2, ±4, ±8, ±16°**: barevné pásy v tmavých schématech, **silné čáry** ve světlých schématech.
- Verdikt o dvojici: **uspokojivé** do 1°, **přijatelné** do 2,5°, **neuspokojivé** nad.
- Pokud je bod pod obzorem jedné ze stanic, objeví se okno **Výpočet není možný** a nic dalšího.

## Ověření

| Spojení | Bod rozptylu | Odchylka | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (nad Janovem) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (JZ Německo, 28 km od Bruselu) | 0,88° | 338° / 87° |
| DB0FAI - Drážďany přes Sheffield (DL5MCG) | – | 0,3° | – |

Známé body rozptylu v seznamu: Ženeva, Brusel, Budapešť, JN92, JN41/42.
