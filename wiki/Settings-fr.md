# RÉGLAGES

[← Home](Home-fr)

Depuis la version 0.4.18, l'onglet **RÉGLAGES** rassemble les options générales du programme et l'aiguillage des spots entre les onglets. Il a deux sous-onglets.

## Général

| Section | Ce que l'on règle |
|---|---|
| Station | Indicatif, locator et altitude, les mêmes que dans la barre du haut ; **Appliquer** les confirme pour tous les onglets. |
| Apparence | Langue, couleurs (les schémas de YAEC et de YA-MS et, depuis la version 0.5.0, les six thèmes d'IP-TRX-CTRL : Clair, Nuit, Noir, Bleu, Violet, Vert) **mode compact** pour les portables de 13-15 pouces (depuis la 0.5.2 : automatique sous 850 pixels de hauteur, toujours ou jamais) et **taille des caractères de tout le programme** (85-140 %, comme dans YA-MS). La langue et les couleurs reconstruisent la fenêtre : les réglages et les spots sont conservés. |
| Chaînes multi-bond en 50 MHz | Depuis la version 0.6.0 : avertissement en haut (et en option une fenêtre) quand naît une ALERTE multi-bond en 50 MHz qui concerne votre station : chaîne avec le premier bond court depuis chez vous, ou corridor avec une extrémité à moins de 1200 km. La même chaîne ne se répète pas avant une heure. |
| Archive des spots | À partir de la version 0.6.5 : à la fermeture chaque jour est compressé dans `data/archivio_spot` (activée par défaut). |
| APRS en 144 MHz | À partir de la version 0.6.5 : APRS-IS en lecture seule, sans mot de passe ; les bonds APRS en 2 m au-delà de 900 km répétés à court terme donnent une ALERTE 144 MHz. |
| Alerte F.A.I. | À partir de la version 0.6.5 : l'onglet F.A.I. clignote quand le F.A.I. est possible ou en cours. |
| Cartes et données de prévision | Le continent, le même menu que dans la barre des onglets. |
| Heure | État NTP et différence entre l'heure des serveurs et l'horloge du PC. |
| Mises à jour | Vérification sur GitHub au démarrage et toutes les 30 minutes, mise à jour automatique et silencieuse, bouton **Vérifier maintenant**. Les mises à jour automatiques ne valent pas pour les paquets système (.deb, .rpm, Arch, Slackware). |
| Données | Bouton qui ouvre le dossier `data`. La clé API du Bureau of Meteorology reste dans DATA COLLECTION › Données de propagation. |

## Aiguillage des spots

À gauche, un tableau énumère les règles avec lesquelles le programme décide où va chaque spot, **dans l'ordre où elles s'appliquent** : la première règle qui s'applique décide de l'onglet. Les valeurs affichées sont celles réglées dans les filtres.

| # | Si le spot... | va à |
|---|---|---|
| 1 | est hors du filtre local (sous 27990 kHz ou au-dessus de 145 MHz ; 430-440 MHz aurore seulement) | écarté |
| 2 | contient AUE, AU-E ou AURORAL E | AURORA (aurore E) |
| 3 | contient un mot de l'aurore (par défaut AU, AUR) | AURORA |
| 4 | contient un mot du F.A.I. (par défaut FAI ; bandes 6, 4 et 2 m : le F.A.I. n'atteint pas le 432 MHz) | F.A.I. |
| 5 | a un mode du meteor scatter ou `<MS>` | METEOR SCATTER |
| 6 | tombe dans une fenêtre MS du plan de bande (non exclusive en 50 MHz ; FT8 et FT4 en 50 et 70 MHz exclus) | METEOR SCATTER |
| 7 | est du FT8 ou du FT4 en 50 MHz sans autre spot dans les 4 min avec le point milieu à moins de 150 km | meteor scatter (pas d'E sporadique) |
| 8 | est du Q65 ou du JT65 en 144 MHz (sauf `<ES>`) | EME : écarté |
| 9 | contient une mention d'une autre propagation | pas d'E sporadique |
| 10 | est transéquatorial | pas d'E sporadique (TEP) |
| 11-15 | est en 28, 40, 50, 70 ou 144 MHz dans les règles de distance | SPORADIC-E |
| 16 | ne relève d'aucune règle (tropo, 432 MHz, HF...) | aucun onglet (reste dans Spots collectés) |

À droite se trouvent le **filtre local** et les filtres de chaque mode, en cinq sous-onglets : Meteor scatter (le filtre de YA-MS se règle dans l'onglet METEOR SCATTER), E sporadique, F.A.I., Aurore et **Test des filtres**, où l'on colle une ligne du cluster pour voir comment elle est classée. Le bouton **Rétablir les règles de l'auteur** remet les valeurs par défaut.
