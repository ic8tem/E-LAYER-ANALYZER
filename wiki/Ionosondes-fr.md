# IONOSONDES

[← Home](Home-fr)

![IONOSONDES](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Sous-onglet de **SPORADIC-E** (depuis la version 0.5.2) : la **foEs mesurée** par les ionosondes du réseau **GIRO** dans le monde entier, à côté de celle que le programme déduit des spots.

## Les données

- 63 ionosondes GIRO (liste ULCAR), avec la dernière **foEs**, **foF2** et **MUF(D)** lues automatiquement sur les ionogrammes.
- Les données arrivent la première fois que l'on ouvre l'onglet, puis **toutes les 15 minutes**, toutes seules (case *Mise à jour automatique toutes les 15 minutes*) ; *Mettre à jour maintenant* les demande tout de suite. L'archive garde les 48 dernières heures dans le dossier `giro` des données.
- Données de **GIRO** (Reinisch et Galkin 2011), licence **CC BY-NC-SA 4.0** : usage non commercial uniquement. Le service est le même que dans *MUF Muncher* de DM6AX.

## Carte et tableau

- Chaque ionosonde est un point coloré avec la dernière foEs (mêmes couleurs que les nuages Es) et la valeur en MHz ; **grise** si elle n'a pas de données récentes, avec l'**anneau** si le dernier ionogramme a plus de 90 minutes.
- Tableau : Code, Ionosonde, km depuis votre station, UTC du dernier ionogramme, foEs, foF2, MUF(D) et **Spot foEs≥** : la foEs minimale déduite des spots Es des 30 dernières minutes qui se réfléchissent à moins de 500 km de l'ionosonde (réglable), avec le nombre de spots.

## L'ionosonde choisie

Clic sur une ligne ou près d'une ionosonde sur la carte :

- **graphique des dernières 24 heures** : foEs jaune, foF2 bleu clair, MUF(D) violet en pointillés ;
- **MUF de l'E sporadique** à 1000, 1500, 2000 et 2400 km (MUF = foEs · sec φ, à l'altitude Es choisie) avec les **bandes qui s'ouvrent** (28, 40, 50, 70, 144 MHz) ;
- si les spots demandent plus que la foEs mesurée, le programme le signale : le nuage peut être loin de l'ionosonde, ou l'ionosonde a atteint la limite de sa gamme de fréquences (Chartier 2022).
