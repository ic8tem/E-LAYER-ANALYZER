# AURORA

[← Home](Home-fr)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Ovale auroral** NOAA OVATION Prime (ombré), **Kp** des 3 derniers jours, **vent solaire** (Bz, vitesse, densité), mis à jour toutes les 5 minutes et enregistrés dans `data/swpc`.
- Les mêmes vues que pour le F.A.I. : **Scatter trail** pour une paire de stations et **Iso-réflexion** pour vérifier un point, avec une altitude par défaut de 110 km.
- **Pointage conseillé pour les deux stations** : le long de la ligne de perpendicularité de chaque station (pointillés de sa couleur), le point où la probabilité de l'ovale est la plus forte. Règle pratique (G3WPO) : partir d'environ 45°, entre le nord et l'est.
- Exemple IO70VJ - JO59BC : scatter point JP01AK au large des Shetland, écart 0,97°, QTF 11° et 299°.
- **Spots d'aurore sur la carte** : les spots portant **<AU>** ou **<AUR>** dans le texte sont des spots d'aurore (règle de l'auteur). Avec la case *Spots <AU>/<AUR> sur la carte avec le calcul* cochée, l'onglet trace les spots des 2 dernières heures (30 au plus) : pour chaque paire spotter-DX, il calcule le scatter trail et montre le scatter point le plus probable (cercle orange), avec le QTF et l'élévation des deux stations dans les résultats.
- En plus du scatter point le plus probable, les résultats indiquent la **plage de QTF** des deux stations le long de tout le Scatter trail (écart ≤ 1,5°).
- **Fenêtre de Lange-Hesse** : le scatter point est préféré à 370-1100 km de chaque station, la fenêtre calculée par Lange-Hesse à 54° de latitude géomagnétique. Depuis la version 0.4.5, la fenêtre se déplace avec l'inclinaison IGRF-14 de chaque station : plus proche pour les stations plus au sud, plus lointaine pour les stations plus au nord (par ex. Capri environ 260-990 km, Oslo 470-1200 km). Les résultats indiquent la fenêtre des deux stations.

## BENCHMARK AURORA

L'onglet AURORA a deux sous-onglets : **REAL-TIME** (le calcul décrit ci-dessus) et **BENCHMARK AURORA**, un banc d'essai du modèle sur **298 QSO d'aurore réels** avec heure et locator : les logs publiés d'EI5FK (IO51RT, Irlande, 2003-2005, 144 et 432 MHz) et la carte du log de PA0O (JO33HG, Pays-Bas).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Pour chaque QSO, le programme calcule le scatter point avec le même modèle (IGRF-14, altitude 110 km, réfraction 4/3, tolérance 3°).
- Couleurs : **vert** = scatter point dans la fenêtre de Lange-Hesse des deux stations ; **jaune** = expliqué mais hors de cette fenêtre ; **orange** = QSO au-delà de 2000 km ; **rouge** = aucun scatter point, QSO non expliqué par le modèle.
- Le récapitulatif donne les pourcentages, aussi avec la fenêtre fixe 370-1100 km pour comparaison, et, pour chaque log, la plage et la médiane du QTF calculé. Un clic sur une ligne dessine le trajet et les deux pointages.
- Résultat (version 0.4.18, champ IGRF-14 de l'année de chaque QSO) : **297 QSO expliqués sur 298 (100 %)**, 267 (90 %) dans la fenêtre de Lange-Hesse des deux stations (275, soit 92 %, avec la fenêtre fixe 370-1100 km) ; QTF médian d'EI5FK entre 32° et 53°, c'est-à-dire le nord-est suggéré par G3WPO. Les logs n'indiquent pas le pointage : la vérification porte sur la géométrie, non sur le QTF.
- **Correction** : de la version 0.4.4 à la 0.4.17, le banc d'essai démarrait avant le chargement d'IGRF-14 et calculait avec le dipôle et la fenêtre fixe ; les résultats publiés alors (243 QSO, 82 %, QTF médian 25-41°) étaient ceux du dipôle. Corrigé dans la 0.4.18.
