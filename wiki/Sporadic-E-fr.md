# SPORADIC-E

[← Home](Home-fr)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## La carte

Un bouton au-dessus de la carte permet de choisir la vue :

- **Azimutale** : carte azimutale équidistante centrée sur votre station, avec le curseur de **zoom** (de 300 à 20000 km du centre, aussi avec la molette de la souris).
- **Mercator** : la carte du monde, avec les boutons Europe et Monde.

Dans les deux projections, les **cercles de distance de Flavio IK3XTV** sont tracés autour de la station : 500 km (minimum du bond simple), 1500-2000 km (optimum), 2400 km (maximum du bond simple), 5000 km (maximum du double bond) et 6900 km (maximum du triple bond).

Dans les vues azimutales, les anneaux en tirets indiquent les **km depuis le centre** (le pas change avec le zoom), le bord marque la distance maximale affichée, et le curseur de la souris donne la distance et l'azimut depuis le centre.

![Multi-bond](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ce qu'il montre

- Les **spots** des dernières minutes (15 min - 12 h) sur les bandes **28, 40 (8 m, désactivée par défaut), 50, 70 et 144 MHz**, provenant des sessions telnet. Les locators sont tirés du commentaire du spot (par ex. `JN70CN<ES>JO62QM`) ou de la base CALL3.
- Les spots portant **<MS>**, **<AU>** ou **<TR>** dans le texte sont **écartés** : ils ne relèvent pas de l'E sporadique, mais servent pour le meteor scatter, l'aurore ou la tropo.
- **Transéquatorial, règle de l'auteur (IC8TEM)** : vers l'Afrique et l'Amérique du Sud, si le point milieu tombe dans la bande d'inclinaison magnétique comprise entre -20° et +20°, la liaison est transéquatoriale (TEP) et non en E sporadique, sauf si le spot indique `<ES>`.
- **144 MHz, règle de l'auteur (IC8TEM)** : l'E sporadique sur 2 m commence **au-delà de 900 km** (les spots plus courts sont écartés). Un spot isolé sur 144 MHz **n'est pas tracé seul** : il apparaît quand, dans les **5 minutes**, arrive un second spot 144 MHz avec le nuage au même endroit (points de réflexion à moins de 300 km), ou quand il existe à proximité un spot **50 MHz à bond court** (dans un rayon de 1200 km, au cours des 15 dernières minutes). La barre du haut indique combien de spots 144 MHz attendent une confirmation.
- Pour chaque spot : distance, **nombre de bonds** selon le tableau d'**IK3XTV**, élévation, **foEs minimale** (loi de la sécante).
- **Premier bond des deux stations** : carré orange = premier bond du spotter, violet = premier bond du DX (bonds égaux) ; jaune = point milieu d'un bond unique.
- **Premier bond court** (cercles creux) : lors des ouvertures multi-bond, l'auteur a constaté un premier bond court à moins de **1200 km** de son QTH ; la longueur est réglable.
- **Nuages Es** sur les **carrés Maidenhead** (2° x 1°), colorés selon la foEs minimale : bleu < 5, bleu clair < 7, vert < 10, jaune < 14, orange < 20, rouge > 20 MHz.
- Tableau des **nuages Es** : pour chaque carré, la **MUF pour votre station** (rouge = 144 MHz possible, orange = 70, jaune = 50).

## Tableau d'IK3XTV

| | minimum | optimum | maximum |
|---|---|---|---|
| Bond unique | 500-650 km | 1500-2000 km | 2200-2400 km |
| Double bond | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Triple bond | – | 4800-6400 km | 6900 km |

## Utilisation

- **Cliquez sur un spot** : le trajet est mis en évidence, avec les points de réflexion, les locators et les azimuts dans le détail.
- **Cliquez sur un spot** dans le tableau : le trajet s'allume sur la carte (halo jaune) et, s'il est hors champ, la carte se déplace ou élargit son rayon pour le montrer. Le spot reste sélectionné quand de nouveaux spots arrivent.
- **Le tableau suit la carte** : avec un zoom réglé, le tableau des spots et celui des nuages ne montrent que ce qui est visible sur la carte.
- **Cliquez sur la carte** : le point est traité comme un possible nuage Es, avec la foEs nécessaire pour 28, 50, 70 et 144 MHz.

## Remarque sur le 432 MHz

L'E sporadique **ne s'applique pas au 432 MHz** : la MUF de l'Es atteint environ 150-200 MHz et les records de l'ARRL ne recensent des liaisons Es que sur 50 et 144 MHz. Les spots 70 cm ne sont pas analysés ici.

## Carrés colorés selon la fréquence et MUF clignotante

Les carrés Maidenhead des points de réflexion sont colorés selon la fréquence la plus haute des spots : **vert** jusqu'à 29 MHz, **jaune** jusqu'à 51, **orange** jusqu'à 71, **rouge** jusqu'à 145 MHz. À partir de la foEs du carré, on estime la MUF probable sur le bond le plus long (2400 km, table IK3XTV) : si elle atteint une bande plus haute, le carré **clignote** dans la couleur de cette bande (principe d'IK3XTV et de Jacobs-Leinwoll : skip court signifie MUF élevée).

En haut à droite de la carte apparaît la synthèse de N0NBH (E-skip pour le continent choisi, SFI, indices K et A).

## Règles par bande (version 0.4.18)

- **50 MHz** : pas d'E sporadique en deçà de 450 km. Les spots FT8 et FT4 ne sont de l'E sporadique que s'ils font partie d'un nuage : un autre spot en 50 MHz dans les 4 minutes avec le point milieu à moins de 150 km. Un spot FT8/FT4 isolé va au meteor scatter.
- **70 MHz** : pas d'E sporadique en deçà de 450 km. FT8 et FT4 vont à l'E sporadique même dans la fenêtre du meteor scatter du plan de bande (le mode connu décide d'abord, puis la fréquence).
- **40 MHz (8 m)** : distance minimale déduite des 450 km du 50 MHz avec la loi de la sécante (environ 330 km) ; seulement les distances à l'intérieur du premier bond de la géométrie de la couche E (environ 2440 km). Couleur du trajet : jaune.
- **144 MHz** : E sporadique à partir de 900 km, avec confirmation ; les spots Q65 et JT65 sont de l'EME et sont écartés, sauf `<ES>` dans le texte.
- G0ISW (Propagation identification, 2024) voit l'E sporadique le plus court au-delà de 400 km en 50 et 70 MHz et au-delà de 1400 km en 144 MHz : les règles de l'auteur concordent en 50 et 70 MHz et sont plus larges en 144 MHz, mais demandent une confirmation.
- L'ordre dans lequel s'appliquent toutes les règles, avec les valeurs utilisées, est affiché dans **[RÉGLAGES](Settings-fr)** › Aiguillage des spots.
