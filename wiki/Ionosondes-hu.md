# IONOSZONDÁK

[← Kezdőlap](Home-hu)

![IONOSZONDÁK](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

A **SPORADIC-E** allapja (a 0.5.2-es verziótól): a **GIRO** hálózat ionoszondái által világszerte **mért foEs**, a program által a spotokból származtatott érték mellett.

## Az adatok

- 62 GIRO ionoszonda (ULCAR-lista), az ionogramokból automatikusan kiolvasott utolsó **foEs**, **foF2** és **MUF(D)** értékkel.
- Az adatok a fül első megnyitásakor érkeznek, majd **15 percenként**, maguktól (*Automatikus frissítés 15 percenként* jelölőnégyzet); a *Frissítés most* azonnal lekéri őket. Az archívum az utolsó 48 órát tartja az adatok `giro` mappájában.
- A **GIRO** adatai (Reinisch és Galkin 2011), **CC BY-NC-SA 4.0** licenc: csak nem kereskedelmi használatra. A szolgáltatás ugyanaz, mint DM6AX *MUF Muncher* programjában.

## Térkép és táblázat

- Minden ionoszonda egy színes pont az utolsó foEs-sel (ugyanazok a színek, mint az Es-felhőknél) és az MHz-ben kifejezett értékkel; **szürke**, ha nincs aktuális adata, **gyűrűvel**, ha az utolsó ionogram 90 percnél régebbi.
- Táblázat: Kód, Ionoszonda, km a saját állomástól, az utolsó ionogram UTC-ideje, foEs, foF2, MUF(D) és **Spotok foEs≥**: az utolsó 30 perc azon Es-spotjaiból származtatott minimális foEs, amelyek az ionoszondától 500 km-en belül verődnek vissza (állítható), a spotok számával.
- **h'Es GIRO** (a 0.6.5-ös verziótól, jelölőnégyzet a REAL-TIME fülön): az Es-magasság a 2500 km-en belüli ionoszondák friss h'Es-értékeinek mediánja lesz (80-120 km).

## A kiválasztott ionoszonda

Kattintás egy sorra vagy egy ionoszonda közelében a térképen:

- **az utolsó 24 óra grafikonja**: foEs sárga, foF2 világoskék, MUF(D) lila pontozott;
- **a sporadikus E MUF-ja** 1000, 1500, 2000 és 2400 km-en (MUF = foEs · sec φ, a választott Es-magasságon) a **megnyíló sávokkal** (28, 40, 50, 70, 144 MHz);
- ha a spotok többet kívánnak a mért foEs-nél, a program jelzi: a felhő messze lehet az ionoszondától, vagy az ionoszonda elérte frekvenciatartománya határát (Chartier 2022).
