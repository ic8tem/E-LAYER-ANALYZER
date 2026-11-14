# BENCHMARK 50 MHz

[← Home](Home-fr)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Banc d'essai pour l'**E sporadique multi-bond sur 50 MHz** à partir du log ADIF d'une station et, en option, de celui d'une seconde station.

## Chargement des logs

- **Station 1** : *Charger l'ADIF* / *Retirer l'ADIF*. Le programme prend `STATION_CALLSIGN` et `MY_GRIDSQUARE` dans le log ; la position d'émission est le `MY_GRIDSQUARE` de chaque QSO.
- **Station 2** (facultative) : mêmes boutons sur la deuxième ligne ; la case *Station 2* l'affiche ou la masque. Les trajets de la station 1 sont en bleu clair, ceux de la station 2 en violet.
- Le programme utilise les QSO 50 MHz avec un locator (les locators manquants ou à 4 caractères sont pris dans CALL3), en multi-bond : au-delà de 3100 km (règle de l'auteur ; le nombre de bonds suit le tableau d'IK3XTV).
- Les liaisons **transéquatoriales** sont exclues (règle de l'auteur) : vers l'Afrique et l'Amérique du Sud, avec le point milieu dans la bande d'inclinaison magnétique comprise entre -15° et +15°, sauf si le log indique `PROP_MODE=ES`.

## Jour et heure

Choisissez le **jour** (à l'ouverture, celui qui compte le plus de QSO multi-bond) et l'horaire **de… à…** UTC, par pas de 15 minutes. La case **foEs** (facultative) colore les trajets selon la foEs minimale et l'affiche dans le tableau.

## La carte

Carte **Mercator** à centre libre : molette = zoom, glisser = déplacer.

![Trajet](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- Un **point jaune flou** marque le centre de chaque bond, où l'on présume que se trouve le nuage Es ; il devient plus grand sur le trajet sous la souris.
- **Souris près d'un trajet** : la ligne et les icônes sont agrandies et le **carré WWL à 6 caractères** de la station DX apparaît.
- **Clic gauche** sur ce trajet : une fenêtre demande s'il faut afficher **tous les QSO du fichier** (tous les jours) dont l'extrémité distante se trouve **à moins de 150 km** de ce point. *Oui* : la carte est effacée et montre ces QSO à l'intérieur du cercle orange ; *Non* : la carte reste telle quelle. *Retour au jour et à l'horaire* revient à la sélection précédente.

![Environs](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Détails

Pour le trajet choisi : distance et classement selon le tableau d'IK3XTV, azimut dans les deux sens, bonds égaux avec les carrés des points de réflexion, premier bond court à moins de 1200 km (expérience d'IC8TEM) et, avec la case foEs, la foEs minimale.
