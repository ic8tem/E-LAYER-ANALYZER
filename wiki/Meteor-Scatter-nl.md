# METEOR SCATTER

[← Home](Home-nl)

Dit tabblad bevat het volledige **YA-MS 1.8.14**, het meteorscatter-programma van IC8TEM: hemelbol met de radianten, radarflux SKiYMET (Collm, Sodankylä), berekening van de efficiëntie van de zwermen, experimentele voorspelling, geometrie en demping, GRAVES, vakkenkaart, **Zwermen live** (ZHR gemeten door het Global Meteor Network en radianten van de CMOR-radar), **Kaart van de bronnen** (Collm, Sodankylä, GRAVES en CMOR ten opzichte van het station) en het document **Uitleg van de rendementsberekening** met de formules.

YA-MS 1.5.0 voegt het tabblad **Gegevensverzameling** toe (meteorscatter-spots gefilterd op MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Binnen E-LAYER ANALYZER opent het geen eigen telnet-sessies: het ontvangt de regels van de sessies van DATA COLLECTION, zodat er geen dubbele logins met dezelfde roepnaam zijn.

YA-MS 1.6.0 deelt de tabbladen opnieuw in onder **Live en realtimegegevens** (meteorscatter-verkeer live met de wolk van elk contact, radarflux, GRAVES, gegevensverzameling) en **Voorspellingsberekening** (voorspelling, geometrie, efficiëntieberekening, uitvoering van de voorspellingen).

YA-MS 1.7.0 en 1.8.0 voegen de vergelijking van de radianten met de catalogus IAU MDC toe, de dagelijkse tellingen van het Global Meteor Network per zwerm, de index k uit de populatie-indices, de beginstraal van het meteorspoor volgens Stober e.a. (2021), de pingduur tussen de banden (standaard λ² of de metingen HamSCI MSQP van NN4SA) en de vergelijking met de BRAMS-detecties in Sonneberg (Perseïden 2026). De automatische update van YA-MS staat uit binnen E-LAYER ANALYZER, dat zichzelf bijwerkt via de GitHub-Releases.

YA-MS 1.8.1 behandelt de antennelob als verticaal diagram (vakje «antennediagram», standaard aan): de reflectiepunten worden gewogen met de verticale winst van beide antennes en de rendementen verdwijnen niet meer op lange paden. Nieuw zijn de waarschuwing «geen gemeenschappelijke zone», het gemarkeerde vakje voor de hoek β (de keuze die het zwaarst weegt), één enkele berekeningsdatum, de «Gebruikershandleiding» en de vergelijking met 181 767 echte spots op 50 MHz van de HamSCI Meteor Scatter QSO Party van augustus 2025: de uren en de kanten van het pad zijn betrouwbaar, de percentages zijn indicatief.

- YA-MS gebruikt de taal en het kleurenschema die bovenaan in E-LAYER ANALYZER zijn gekozen.
- De instellingen van YA-MS worden opgeslagen in `data/config.ini`.
- Elke nieuwe versie van YA-MS wordt overgenomen met het script `tools/sync_yams.py`.

Zie voor details de [YA-MS-repository](https://github.com/ic8tem/YA-MS).
