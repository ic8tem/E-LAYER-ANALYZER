# F.A.I.

[← Home](Home-cs)

F.A.I. (Field Aligned Irregularities) je rozptyl na nehomogenitách orientovaných podél geomagnetického pole, typický pro letní večery po sporadické vrstvě E. Rozptýlený signál odchází po **kuželu**, jehož osa leží podél pole: dvě stanice se mohou spojit, pokud leží na **doplňkových křivkách** (a1 + a2 = 180°, článek IC8FAX-IC8TEM; křivky ±c podle K9SE). Magnetické pole se počítá modelem **IGRF-14**.

## Pohled Scatter trail (dvojice A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Zadejte **Stanici A** a **Stanici B** (značka vyhledaná v CALL3 a/nebo lokátor). Prázdná A = vaše vlastní stanice.
- Mapa ukazuje **scatter trail**: všechny body ve výšce vrstvy, viditelné z obou stanic, které splňují podmínku odrazu s tolerancí 2,5° (třídy 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Rámeček vpravo nahoře ukazuje **QTF a elevaci** stanice A (červeně) a B (zeleně) k jednotlivým bodům.
- Program navrhne **nejpravděpodobnější bod rozptylu** (minimální odchylka, nejkratší trasa).
- **Refrakce 4/3**: ve výchozím stavu zapnutá, rozšiřuje obzory.

## Pohled Izoreflexe (izoreflexní křivky)

![Isoflection](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

Jde o geometrickou konstrukci, která slouží ke **kontrole, zda je bod odrazu správný**.

- **Levé kliknutí** na mapu: zvolíte bod ke kontrole. **Pravé kliknutí**: přesune stanici B.
- **Zkontrolovat doporučený bod**: otevře graf na bodu navrženém programem.
- Vrstevnice na **0, ±1, ±2, ±4, ±8, ±16°**: barevné pásy v tmavých schématech, **silné čáry** ve světlých schématech.
- Hodnocení dvojice: **vyhovující** do 1°, **přijatelné** do 2,5°, **nevyhovující** nad tuto hodnotu.
- Je-li bod pod obzorem jedné ze stanic, objeví se okno **Výpočet není možný** a nic dalšího.

## Kontroly

| Spojení | Bod rozptylu | Odchylka | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (nad Janovem) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (JZ Německo, 28 km od Bruselu) | 0,88° | 338° / 87° |
| DB0FAI - Drážďany přes Sheffield (DL5MCG) | – | 0,3° | – |

Známé body rozptylu v seznamu: Ženeva, Brusel, Budapešť, JN92, JN41/42.
