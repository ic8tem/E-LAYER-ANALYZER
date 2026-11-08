# F.A.I.

[← Home](Home-et)

F.A.I. (Field Aligned Irregularities) on hajumine geomagnetvälja suunalistelt ebahomogeensustelt, tüüpiline suveõhtutel pärast sporaadilist E-d. Hajunud signaal lahkub **koonusel**, mille telg on välja suunaline: kaks jaama saavad omavahel sidet pidada, kui nad asuvad **täiendavatel kontuuridel** (a1 + a2 = 180°, IC8FAX-i ja IC8TEM-i artikkel; K9SE ±c kontuurid). Magnetväli arvutatakse mudeliga **IGRF-14**.

## Vaade Scatter trail (paar A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Sisesta **Jaam A** ja **Jaam B** (kutsung otsitakse CALL3-st ja/või lokaator). Tühi A = sinu enda jaam.
- Kaart näitab **Scatter traili**: kõik kihi kõrgusel olevad punktid, mida mõlemad jaamad näevad ja mis rahuldavad peegeldustingimust 2,5° piires (klassid 0-1, 1-1,5, 1,5-2, 2-2,5°).
- Paremal üleval olev kast näitab A (punane) ja B (roheline) **QTF-i ja elevatsiooni** iga punkti suunas.
- Programm soovitab **kõige tõenäolisemat hajumispunkti** (minimaalne hälve, lühim trass).
- **Refraktsioon 4/3**: vaikimisi sees, laiendab horisonte.

## Vaade Isopeegeldus (isopeegelduse jooned)

![Isopeegeldus](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

See on geomeetriline konstruktsioon, millega **kontrollitakse, kas peegelduspunkt on õige**.

- **Vasakklõps** kaardil: valid kontrollitava punkti. **Paremklõps**: nihutab jaama B.
- **Kontrolli soovitatud punkti**: avab graafiku programmi soovitatud punktiga.
- Kontuurjooned **0, ±1, ±2, ±4, ±8, ±16°** juures: tumedates skeemides värvilised ribad, heledates skeemides **paksud jooned**.
- Paari hinnang: **rahuldav** 1° piires, **vastuvõetav** 2,5° piires, **mitterahuldav** üle selle.
- Kui punkt on ühe jaama horisondi all, ilmub hüpikaken **Arvutus pole võimalik** ja muud midagi.

## Kontrollid

| Side | Hajumispunkt | Hälve | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (Genova kohal) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (Edela-Saksamaa, Brüsselist 28 km) | 0,88° | 338° / 87° |
| DB0FAI - Dresden üle Sheffieldi (DL5MCG) | – | 0,3° | – |

Loendis olevad teadaolevad hajumispunktid: Genf, Brüssel, Budapest, JN92, JN41/42.
