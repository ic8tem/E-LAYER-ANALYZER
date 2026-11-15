# Es-FELHŐK AZ IDŐBEN

[← Kezdőlap](Home-hu)

![Es-FELHŐK AZ IDŐBEN](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

A **SPORADIC-E** allapja (a 0.5.2-es verziótól): hogyan születnek, mozognak és hunynak ki a **sporadikus E felhői**, lépésről lépésre.

## A módszer

**Chartier, Hanley és Emmons (2022)** módszere: minden sporadikus E összeköttetés azt mondja, hogy a visszaverődési pontban a foEs **legalább f / sec φ**. A pontokat cellákba és időlépésekbe gyűjti, és minden cella a lépés **legmagasabb értékét** kapja. A minimális elektronsűrűség NmE = 1,24·10¹⁰ · foEs² elektron köbméterenként.

A program a saját modelljének visszaverődési pontjait használja (minden ugrás), nem csak a felezőpontot. Az érték **alsó korlát**: az üres cella csak annyit jelent, hogy nincs összeköttetés.

## Forrás és kezelőszervek

- **A munkamenet spotjai** (a fül minden megnyitásakor frissülnek) vagy **ADIF-napló vagy CSV-spotok betöltése**.
- **A munkamenet spotjainak exportálása (CSV)**: a spotarchívum a program bezárásakor kiürül, így később is megnézheti őket.
- **Sávok MHz** (28, 40, 50, 70, 144), **Lépés** (5, 10 vagy 15 perc), **Cella** (1°, 2° vagy 4°), **Es-magasság km**.
- Idősáv: ⏮ ◀ *Indítás/Leállítás* ▶ ⏭ és a csúszka; megnyitáskor a legtöbb cellát tartalmazó lépéstől indul.
- Az *Előző lépések halványan* a kiválasztott előtti három lépést mutatja; az *Összeköttetések* az útvonalakat a sávok színeivel rajzolja ki.

## Táblázat és grafikon

- A lépés cellái: Cella (délnyugati sarok és a középpont lokátora), foEs≥, NmE≥, Összeköttetések, Sávok. Kattintás egy cellára: kiemeli az összeköttetéseit.
- Grafikon: a legmagasabb minimális foEs (sárga vonal) és a cellák száma (oszlopok) lépésenként.
