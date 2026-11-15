# NUAGES Es DANS LE TEMPS

[← Home](Home-fr)

![NUAGES Es DANS LE TEMPS](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Sous-onglet de **SPORADIC-E** (depuis la version 0.5.2) : comment les **nuages d'E sporadique** naissent, se déplacent et s'éteignent, pas à pas.

## La méthode

Méthode de **Chartier, Hanley et Emmons (2022)** : chaque liaison en E sporadique indique qu'au point de réflexion la foEs vaut **au moins f / sec φ**. Les points sont regroupés en cellules et en pas de temps, et chaque cellule prend la **valeur la plus élevée** du pas. La densité électronique minimale est NmE = 1,24·10¹⁰ · foEs² électrons par mètre cube.

Le programme utilise les points de réflexion de son propre modèle (tous les bonds), et pas seulement le point milieu. La valeur est une **limite inférieure** : une cellule vide signifie seulement qu'il n'y a aucune liaison.

## Source et commandes

- **Spots de la session** (mis à jour à chaque ouverture de l'onglet) ou **Charger un log ADIF ou des spots CSV**.
- **Exporter les spots de la session (CSV)** : l'archive des spots est vidée à la fermeture du programme, vous pouvez ainsi les revoir plus tard.
- **Bandes MHz** (28, 40, 50, 70, 144), **Pas** (5, 10 ou 15 minutes), **Cellule** (1°, 2° ou 4°), **Altitude Es km**.
- Barre du temps : ⏮ ◀ *Démarrer/Arrêter* ▶ ⏭ et le curseur ; à l'ouverture, on part du pas qui a le plus de cellules.
- *Pas précédents estompés* montre les trois pas avant celui choisi ; *Liaisons* dessine les trajets avec les couleurs des bandes.

## Tableau et graphique

- Cellules du pas : Cellule (coin sud-ouest et locator du centre), foEs≥, NmE≥, Liaisons, Bandes. Clic sur une cellule : ses liaisons sont mises en évidence.
- Graphique : foEs minimale la plus élevée (ligne jaune) et nombre de cellules (barres) pour chaque pas.
