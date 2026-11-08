# METEOR SCATTER

[← Home](Home-fr)

Cet onglet contient le programme complet **YA-MS 1.5.1**, le programme meteor scatter d'IC8TEM : sphère céleste avec les radiants, flux des radars SKiYMET (Collm, Sodankylä), calcul de l'efficacité des essaims, mise en œuvre expérimentale des prévisions, géométrie et affaiblissement, GRAVES, carte des carrés, **Essaims en direct** (ZHR mesurés par le Global Meteor Network et radiants du radar CMOR), **Carte des sources** (Collm, Sodankylä, GRAVES et CMOR par rapport à la station) et le document **Efficacité - explication** avec les formules.

YA-MS 1.5.0 ajoute l'onglet **Collecte de données** (spots meteor scatter filtrés par MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dans E-LAYER ANALYZER, il n'ouvre aucune session telnet à lui : il reçoit les lignes des sessions de DATA COLLECTION, ce qui évite les doubles connexions avec le même indicatif.

- YA-MS utilise la langue et le schéma de couleurs choisis en haut d'E-LAYER ANALYZER.
- Les réglages de YA-MS sont enregistrés dans `data/config.ini`.
- Chaque nouvelle version de YA-MS est intégrée avec le script `tools/sync_yams.py`.

Pour les détails, voir le [dépôt YA-MS](https://github.com/ic8tem/YA-MS).
