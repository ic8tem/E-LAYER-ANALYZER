# F.A.I.

[← Home](Home-fr)

Le F.A.I. (Field Aligned Irregularities) est une diffusion sur des irrégularités alignées sur le champ géomagnétique, typique des soirées d'été après l'E sporadique. Le signal diffusé repart sur un **cône** dont l'axe suit le champ : deux stations peuvent se contacter si elles se trouvent sur des **contours supplémentaires** (a1 + a2 = 180°, article IC8FAX-IC8TEM ; contours ±c de K9SE). Le champ magnétique est calculé avec le modèle **IGRF-14**.

## Vue Scatter trail (paire A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Saisissez **Station A** et **Station B** (indicatif recherché dans CALL3 et/ou locator). A laissé vide = votre propre station.
- La carte montre le **scatter trail** : tous les points à l'altitude de la couche, vus par les deux stations, qui satisfont la condition de réflexion à 2,5° près (classes 0-1, 1-1,5, 1,5-2, 2-2,5°).
- L'encadré en haut à droite affiche le **QTF et l'élévation** de A (rouge) et de B (vert) vers chaque point.
- Le programme suggère le **scatter point le plus probable** (écart minimal, trajet le plus court).
- **Réfraction 4/3** : activée par défaut, elle étend les horizons.

## Vue Iso-réflexion (lieux d'iso-réflexion)

![Iso-réflexion](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

C'est la construction géométrique qui sert à **vérifier si un point de réflexion est le bon**.

- **Clic gauche** sur la carte : vous choisissez le point à vérifier. **Clic droit** : déplace la station B.
- **Vérifier le point conseillé** : ouvre le graphique sur le point suggéré par le programme.
- Courbes de niveau à **0, ±1, ±2, ±4, ±8, ±16°** : bandes colorées dans les schémas sombres, **traits épais** dans les schémas clairs.
- Verdict sur la paire : **satisfaisant** à moins de 1°, **acceptable** à moins de 2,5°, **non satisfaisant** au-delà.
- Si le point est sous l'horizon de l'une des stations, la fenêtre **Calcul impossible** s'affiche, et rien d'autre.

## Vérifications

| Liaison | Scatter point | Écart | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (au-dessus de Gênes) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (sud-ouest de l'Allemagne, à 28 km de Bruxelles) | 0,88° | 338° / 87° |
| DB0FAI - Dresde via Sheffield (DL5MCG) | – | 0,3° | – |

Scatter points connus dans la liste : Genève, Bruxelles, Budapest, JN92, JN41/42.
