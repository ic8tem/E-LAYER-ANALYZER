# SPORADIC-E

[← Home](Home-fr)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## La carte

Un bouton au-dessus de la carte permet de choisir la vue :

- **Azimutale - bond unique** : carte azimutale équidistante centrée sur votre station, rayon fixe de 2500 km, avec les zones du tableau d'IK3XTV (500 km minimum, 1500-2000 km optimum, 2400 km maximum en bond unique).
- **Azimutale - distance maximale (multi-bond)** : la même carte avec le curseur **Distance maximale** (de 300 à 20000 km, aussi avec la molette de la souris) ; les limites du double bond (5000 km) et du triple bond (6900 km) apparaissent aussi, ce qui permet de suivre l'évolution du multi-bond.
- **Rectangulaire** : la carte du monde précédente, avec les boutons Europe et Monde.

Dans les vues azimutales, les anneaux en tirets indiquent les **km depuis le centre** (le pas change avec le zoom), le bord marque la distance maximale affichée, et le curseur de la souris donne la distance et l'azimut depuis le centre.

![Multi-bond](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Ce qu'il montre

- Les **spots** des dernières minutes (15 min - 12 h) sur les bandes **28, 50, 70 et 144 MHz**, provenant des sessions telnet. Les locators sont tirés du commentaire du spot (par ex. `JN70CN<ES>JO62QM`) ou de la base CALL3.
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
