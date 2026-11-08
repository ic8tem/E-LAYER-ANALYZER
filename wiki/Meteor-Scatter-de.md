# METEOR SCATTER

[← Home](Home-de)

Dieser Reiter enthält das vollständige **YA-MS 1.5.1**, das Meteorscatter-Programm von IC8TEM: Himmelskugel mit den Radianten, Radarfluss SKiYMET (Collm, Sodankylä), Berechnung der Effektivität der Meteorströme, experimentelle Umsetzung der Vorhersagen, Geometrie und Dämpfung, GRAVES, Feldkarte, **Ströme live** (vom Global Meteor Network gemessene ZHR und Radianten des CMOR-Radars), **Karte der Quellen** (Collm, Sodankylä, GRAVES und CMOR relativ zur Station) und das Dokument **Effektivität - Erklärung** mit den Formeln.

YA-MS 1.5.0 bringt den Reiter **Datensammlung** hinzu (Meteorscatter-Spots, gefiltert nach MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Innerhalb von E-LAYER ANALYZER öffnet er keine eigenen Telnet-Sitzungen: Er erhält die Zeilen der Sitzungen von DATA COLLECTION, sodass es keine doppelten Logins mit demselben Rufzeichen gibt.

- YA-MS verwendet die oben in E-LAYER ANALYZER gewählte Sprache und das dort gewählte Farbschema.
- Die Einstellungen von YA-MS werden in `data/config.ini` gespeichert.
- Jede neue Version von YA-MS wird mit dem Skript `tools/sync_yams.py` übernommen.

Einzelheiten im [YA-MS-Repository](https://github.com/ic8tem/YA-MS).
