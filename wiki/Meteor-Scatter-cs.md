# METEOR SCATTER

[← Home](Home-cs)

Tato karta obsahuje kompletní **YA-MS 1.6.0**, program pro meteor scatter od IC8TEM: nebeskou klenbu s radianty, tok z radarů SKiYMET (Collm, Sodankylä), výpočet účinnosti rojů, experimentální implementaci předpovědí, geometrii a útlum, GRAVES, mapu čtverců, **Roje živě** (ZHR měřené sítí Global Meteor Network a radianty z radaru CMOR), **Mapu zdrojů** (Collm, Sodankylä, GRAVES a CMOR vzhledem ke stanici) a dokument **Účinnost - vysvětlení** se vzorci.

YA-MS 1.5.0 přidává kartu **Sběr dat** (spoty meteor scatter filtrované podle MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Uvnitř E-LAYER ANALYZER neotevírá žádné vlastní relace telnet: přijímá řádky z relací DATA COLLECTION, takže nedochází k dvojímu přihlášení se stejnou značkou.

YA-MS 1.6.0 přeuspořádává karty do **Živě a data v reálném čase** (provoz meteor scatter živě s oblakem každého spojení, tok z radaru, GRAVES, sběr dat) a **Předpovědní výpočet** (předpověď, geometrie, výpočet účinnosti, implementace předpovědí).

- YA-MS používá jazyk a barevné schéma zvolené nahoře v E-LAYER ANALYZER.
- Nastavení YA-MS se ukládají do `data/config.ini`.
- Každá nová verze YA-MS se přebírá skriptem `tools/sync_yams.py`.

Podrobnosti viz [repozitář YA-MS](https://github.com/ic8tem/YA-MS).
