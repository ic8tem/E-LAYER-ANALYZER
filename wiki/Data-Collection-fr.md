# DATA COLLECTION

[← Home](Home-fr)

## Sessions telnet

| Session | Par défaut | Remarques |
|---|---|---|
| 1. DX cluster au choix | on0dxk.dyndns.org 8000 | Sur ON0DXK, les spots VHF sont filtrés : `set/filter dxbm/pass 6` (puis 4, 2). Autre exemple : hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW et RTTY, 7001 = FT8. |
| 3. Cluster avec PSKReporter | **dm4x.ddns.net 8500** | Le cluster de DM4X ; en alternative, HamAlert (hamalert.org 7300, compte nécessaire). |

- Login automatique avec l'indicatif (en haut ou dans le champ Login) ; **commandes initiales** séparées par `;`.
- **Reconnexion** au bout de 30 s si la ligne tombe ; **Connecter au démarrage**.
- Par défaut, seuls les spots à partir de **28 MHz** sont enregistrés.

## Spots collectés

Les 2000 derniers spots, avec des filtres par bande et par source. Archive : un fichier CSV par jour dans `data/spot`. À chaque fermeture du programme, la base des spots est vidée (mémoire et fichiers de `data/spot`) : chaque session repart de zéro.

**Filtre local** (case facultative) : les lignes « DX de » sous 27990,0 kHz ou au-dessus de 145 MHz sont écartées, ni affichées dans la console, ni enregistrées. Les **locators imprécis** (moins de 6 caractères) sont pris dans CALL3 : si CALL3 a un locator à 6 caractères, il l'emporte toujours. Comme dans YA-MS, les spots meteor scatter (et non Sporadic-E) sont ceux situés dans les fenêtres de fréquences typiques du MS (50190-50300, 70100-70400, 144290-144399 kHz) sans balise d'une autre propagation (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), ainsi que ceux portant des modes propres au MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) ou `<MS>`. Les spots avec `<AS>`/`<ACS>` (aircraft scatter) ne sont pas utilisés pour le Sporadic-E.

## CALL3

Base **indicatif → locator** de JTDX (plus d'un million d'indicatifs) dans `data/call3.txt`. Boutons **Recharger** et **Importer un nouveau CALL3.TXT**. Les locators à 6 caractères lus dans les commentaires des spots sont ajoutés à `data/call3_nuovi.txt`.

Depuis la version 0.4.16, il n'y a qu'une base CALL3 pour tout le programme, celle de la Collecte de données de YA-MS (onglet METEOR SCATTER), avec les mêmes règles que LIVETROPOVHF : mise à jour depuis MMMonVHF, entrées manuelles, fichiers ajoutés, corrections du cluster, locators appris et G7VRD toutes les 30 minutes ; les corrections sont écrites dans data/call3.txt et les locators de data/call3_nuovi.txt sont importés une fois. Les spots de WSJT-X, JTDX et MSHV arrivent aussi par UDP (source WSJT-X UDP, port 2237 renvoyé vers 2238).

Ces filtres, issus de YA-MS (CW, distance minimale, fenêtres de fréquence, modes MS, bandes), ne valent que pour l'onglet METEOR SCATTER : ils décident quels spots entrent dans sa collecte. Les autres onglets reçoivent tous les spots et utilisent leurs propres filtres.
