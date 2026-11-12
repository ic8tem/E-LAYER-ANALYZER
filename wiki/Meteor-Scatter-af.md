# METEOR SCATTER

[← Home](Home-af)

Hierdie oortjie bevat die volledige **YA-MS 1.8.0**, IC8TEM se meteor scatter-program: hemelsfeer met die radiante, SKiYMET-radarvloed (Collm, Sodankylä), berekening van swermdoeltreffendheid, eksperimentele implementering van die voorspellings, geometrie en verswakking, GRAVES, vierkantkaart, **Regstreekse swerms** (ZHR gemeet deur die Global Meteor Network en radiante van die CMOR-radar), **Bronnekaart** (Collm, Sodankylä, GRAVES en CMOR relatief tot die stasie) en die dokument **Doeltreffendheid - verduideliking** met die formules.

YA-MS 1.5.0 voeg die oortjie **Data-insameling** by (meteor scatter-spots gefiltreer volgens MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Binne E-LAYER ANALYZER open dit geen eie telnet-sessies nie: dit ontvang die reëls van die DATA COLLECTION-sessies, sodat daar geen dubbele aanmeldings met dieselfde roepnaam is nie.

YA-MS 1.6.0 herorganiseer die oortjies in **Regstreeks en intydse data** (regstreekse meteor scatter-verkeer met die wolk van elke verbinding, radarvloed, GRAVES, data-insameling) en **Voorspellingsberekening** (voorspelling, geometrie, doeltreffendheidsberekening, implementering van die voorspellings).

YA-MS 1.7.0 en 1.8.0 voeg die volgende by: die vergelyking van die radiante met die IAU MDC-katalogus, die daaglikse tellings van die Global Meteor Network per swerm, die k-indeks uit die populasie-indekse, die aanvanklike spoorradius van Stober en ander (2021), die pingduur oor die bande heen (λ² by verstek of die HamSCI MSQP-metings van NN4SA) en die vergelyking met die BRAMS-waarnemings in Sonneberg (Perseïede 2026). Die outomatiese opdatering van YA-MS is binne E-LAYER ANALYZER afgeskakel, wat homself vanaf die GitHub Releases opdateer.

- YA-MS gebruik die taal en kleurskema wat bo-aan E-LAYER ANALYZER gekies is.
- YA-MS-instellings word in `data/config.ini` gestoor.
- Elke nuwe weergawe van YA-MS word met die skrip `tools/sync_yams.py` ingebring.

Vir besonderhede, sien die [YA-MS-bewaarplek](https://github.com/ic8tem/YA-MS).
