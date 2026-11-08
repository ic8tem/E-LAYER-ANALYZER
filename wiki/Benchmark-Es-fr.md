# BENCHMARK Es

[← Home](Home-fr)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Banc d'essai pour l'**E sporadique** et le **multi-bond** sur un **log ADIF complet**, même celui d'une autre station : à partir des QSO réellement effectués, le programme esquisse le skip et les nuages, jour par jour et heure par heure.

## Chargement du log

- **Charger l'ADIF** : choisit le fichier `.adi`/`.adif`. Le programme mémorise le fichier et le recharge au démarrage.
- **Retirer l'ADIF** : retire le log de l'onglet.
- **Station au centre** : indicatif et locator de la station qui a fait le log. Le programme propose le `STATION_CALLSIGN` (ou `OPERATOR`) et le `MY_GRIDSQUARE` les plus fréquents ; on peut les modifier à la main et les confirmer avec **Appliquer**. Le centre ne dépend pas des réglages généraux, ce qui permet d'étudier les logs d'autres stations.
- Position d'émission : le `MY_GRIDSQUARE` de chaque QSO (s'il manque, le locator du centre). Positions de réception : les champs `GRIDSQUARE`.
- QSO utilisés : 28, 50, 70 et 144 MHz avec un locator. Les QSO avec un autre `PROP_MODE` (TR, MS, FAI, AUR, EME...) et ceux de moins de 500 km peuvent être exclus. Le 432 MHz n'est jamais inclus : l'E sporadique ne s'y applique pas.

## Jours et heures

- **QSO par journée** : jour, nombre de QSO, bandes, distance maximale, multi-bonds. À l'ouverture, le programme propose le jour qui compte le plus de QSO.
- **Heures de la journée** : la ligne *Toute la journée* et une ligne pour chaque heure UTC ; un clic n'affiche que les QSO de cette heure.
- **QSO** : la liste correspondant à la sélection ; un clic sur un QSO (ou près de la station DX sur la carte) le met en évidence et en affiche le détail.

## La carte

Carte **azimutale équidistante** centrée sur la station, avec le curseur **Distance maximale** pour zoomer (aussi avec la molette de la souris) : les anneaux indiquent les **km depuis le centre**.

- **Skip par secteur** (10° d'azimut) : arc orange = QSO le plus proche dans le secteur (bord de la zone de silence), arc vert = QSO le plus lointain, entre les deux la zone atteinte.
- **Nuages Es** sur les carrés Maidenhead, colorés selon la foEs estimée.
- Point milieu des bonds uniques (jaune), premier bond de la station (orange, cercle creux = premier bond court), premier bond de la station DX (violet).
- Pour la journée entière, la **ligne rouge** relie heure par heure le barycentre des points milieux : c'est la dérive des nuages.

![Une heure](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Calculs proposés

1. **Skip par secteur** : pour chaque secteur, la distance de skip et la portée, avec les indicatifs.
2. **foEs estimée** avec la loi de la sécante (foEs ≥ f / sec φ) : le bond unique le plus court sur chaque bande donne l'estimation la plus haute ; avec cette foEs, la MUF attendue à 1000, 1500, 2000 et 2400 km et la distance au-delà de laquelle le 144 MHz s'ouvrirait.
3. **Nuages Es** estimés sur les carrés, avec le nombre de QSO et la foEs.
4. **Multi-bond** : le premier bond de la station selon deux hypothèses (bonds égaux, et premier bond court à moins de 1200 km, expérience d'IC8TEM), comparé aux nuages vus en bond unique à ±30 minutes ; à moins de 300 km, l'hypothèse est confirmée. Le programme compte les confirmations de chaque hypothèse.
5. **Dérive des nuages** heure par heure : km, direction et vitesse.

Les calculs sont indicatifs : un QSO dit que le trajet était ouvert, pas exactement où se trouvait le nuage. Plus les données sont propres (locators à 6 caractères, `PROP_MODE` correct), plus le skip est fiable.
