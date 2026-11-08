# METEOR SCATTER

[← Home](Home-af)

Hierdie oortjie bevat die volledige **YA-MS 1.5.1**, IC8TEM se meteor scatter-program: hemelsfeer met die radiante, SKiYMET-radarvloed (Collm, Sodankylä), berekening van swermdoeltreffendheid, eksperimentele implementering van die voorspellings, geometrie en verswakking, GRAVES, vierkantkaart, **Regstreekse swerms** (ZHR gemeet deur die Global Meteor Network en radiante van die CMOR-radar), **Bronnekaart** (Collm, Sodankylä, GRAVES en CMOR relatief tot die stasie) en die dokument **Doeltreffendheid - verduideliking** met die formules.

YA-MS 1.5.0 voeg die oortjie **Data-insameling** by (meteor scatter-spots gefiltreer volgens MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Binne E-LAYER ANALYZER open dit geen eie telnet-sessies nie: dit ontvang die reëls van die DATA COLLECTION-sessies, sodat daar geen dubbele aanmeldings met dieselfde roepnaam is nie.

- YA-MS gebruik die taal en kleurskema wat bo-aan E-LAYER ANALYZER gekies is.
- YA-MS-instellings word in `data/config.ini` gestoor.
- Elke nuwe weergawe van YA-MS word met die skrip `tools/sync_yams.py` ingebring.

Vir besonderhede, sien die [YA-MS-bewaarplek](https://github.com/ic8tem/YA-MS).
