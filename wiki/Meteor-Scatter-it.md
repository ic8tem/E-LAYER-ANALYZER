# METEOR SCATTER

[← Home](Home-it)

La linguetta contiene **YA-MS 1.5.0** completo, il programma di IC8TEM per il meteor scatter: volta celeste con i radianti, flusso dei radar SKiYMET (Collm, Sodankylä), calcolo dell'efficienza degli sciami, attuazione sperimentale delle previsioni, geometria e attenuazione, GRAVES, mappa dei quadrati, **Sciami in diretta** (ZHR misurati dal Global Meteor Network e radianti del radar CMOR), **Mappa delle fonti** (Collm, Sodankylä, GRAVES e CMOR rispetto alla stazione) e il documento **Efficienza - spiegazione** con le formule.

YA-MS 1.5.0 aggiunge la linguetta **Raccolta dati** (spot meteor scatter filtrati con MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro E-LAYER ANALYZER non apre sessioni telnet proprie: riceve le righe delle sessioni di DATA COLLECTION, così non ci sono doppi login con lo stesso nominativo.

- La lingua e lo schema di colori di YA-MS sono quelli scelti in alto in E-LAYER ANALYZER.
- Le impostazioni di YA-MS stanno in `data/config.ini`.
- Ogni nuova versione di YA-MS si porta dentro con lo script `tools/sync_yams.py`.

Per i dettagli vedi il [repository di YA-MS](https://github.com/ic8tem/YA-MS).
