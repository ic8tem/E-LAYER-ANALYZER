# METEOR SCATTER

[← Domů](Home-cs)

Tato karta obsahuje kompletní **YA-MS 1.8.3**, program pro meteor scatter od IC8TEM: nebeskou sféru s radianty, tok z radarů SKiYMET (Collm, Sodankylä), výpočet účinnosti rojů, experimentální provádění předpovědí, geometrii a útlum, GRAVES, mapu čtverců, **Živé roje** (ZHR změřené Global Meteor Network a radianty z radaru CMOR), **Mapu zdrojů** (Collm, Sodankylä, GRAVES a CMOR vzhledem ke stanici) a dokument **Vysvětlení výpočtu účinnosti** se vzorci.

YA-MS 1.5.0 přidává kartu **Sběr dat** (spoty meteor scatter filtrované podle MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Uvnitř E-LAYER ANALYZER neotevírá žádné vlastní relace telnet: dostává řádky z relací DATA COLLECTION, takže nedochází ke dvojímu přihlášení se stejnou značkou.

YA-MS 1.6.0 přeskupuje karty do skupin **Živá data a data v reálném čase** (živý provoz meteor scatter s oblakem každého spojení, tok z radaru, GRAVES, sběr dat) a **Výpočet předpovědi** (předpověď, geometrie, výpočet účinnosti, provádění předpovědí).

YA-MS 1.7.0 a 1.8.0 přidávají porovnání radiantů s katalogem IAU MDC, denní počty Global Meteor Network podle rojů, index k odvozený z populačních indexů, počáteční poloměr stopy podle Stober a kol. (2021), dobu trvání pingů mezi pásmy (výchozí λ² nebo měření HamSCI MSQP od NN4SA) a porovnání s detekcemi BRAMS ze stanice Sonneberg (Perseidy 2026). Automatická aktualizace YA-MS je uvnitř E-LAYER ANALYZER vypnutá; program se aktualizuje sám z GitHub Releases.

YA-MS 1.8.1 bere lalok antény jako svislý diagram (políčko «diagram antény», ve výchozím stavu zapnuté): body odrazu se váží svislým ziskem obou antén a účinnosti na dlouhých trasách už nemizí. Přibylo upozornění «žádná společná zóna», zvýrazněné políčko úhlu β (volba, která váží nejvíc), jediné datum výpočtu, «Uživatelská příručka» a srovnání se 181 767 skutečnými spoty na 50 MHz z HamSCI Meteor Scatter QSO Party ze srpna 2025: hodiny a strany trasy jsou spolehlivé, procenta jsou orientační.

- YA-MS používá jazyk a barevné schéma zvolené nahoře v E-LAYER ANALYZER.
- Nastavení YA-MS se ukládají do `data/config.ini`.
- Každá nová verze YA-MS se přebírá skriptem `tools/sync_yams.py`.

Podrobnosti viz [repozitář YA-MS](https://github.com/ic8tem/YA-MS).
