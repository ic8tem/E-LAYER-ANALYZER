# Installation

[← Home](Home-fr)

Téléchargez le bon fichier depuis la page **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Le programme et le dossier `data` restent toujours ensemble dans un dossier à vous : **aucun fichier n'est jamais écrit dans C:\Users\...\AppData**.

| Système | Fichier | Installation |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Double-clic. S'installe dans `C:\E-LAYER-ANALYZER`, sans droits d'administrateur. Pas de raccourci dans le menu Démarrer (il se trouverait dans AppData), seulement l'icône facultative sur le bureau. |
| Windows 32 bits | `...-win-x86-setup.exe` | Comme ci-dessus, pour Windows 32 bits. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Décompressez n'importe où et lancez `E-LAYER-ANALYZER.exe`. |
| Linux 64 bits (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, puis lancez-le depuis le menu : au premier démarrage le programme se copie dans `~/E-LAYER-ANALYZER`, avec le dossier `data` à côté. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | Décompressez et lancez `./E-LAYER-ANALYZER`, ou lancez `./installa.sh` : s'installe dans `~/E-LAYER-ANALYZER` avec une icône dans le menu. |
| Raspberry Pi (64 bits) | `e-layer-analyzer_x.y.z_arm64.deb` ou `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 ou 5 avec **Raspberry Pi OS 64 bits** et bureau (4 Go de RAM recommandés). Le .deb comme ci-dessus ; la version portable comme pour Linux. |
| Raspberry Pi (32 bits) | `e-layer-analyzer_x.y.z_armhf.deb` ou `...-linux-arm-portatile.tar.gz` | Pi 2, 3 ou 4 avec **Raspberry Pi OS 32 bits**. Le .deb comme ci-dessus ; la version portable : décompressez et lancez `./E-LAYER-ANALYZER` ou `./installa.sh`. |
| Raspberry Pi avec Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` ou `...-linux-arm64-portatile.tar.gz` | En root : `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, puis lancez `e-layer-analyzer` depuis le menu ou le terminal : au premier démarrage le programme se copie dans `~/E-LAYER-ANALYZER`. La version portable comme pour Linux. |
| Linux armv7l (autres cartes ARM 32 bits) | `...-linux-armv7l-portatile.tar.gz` | Comme la version portable pour Linux. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ou `...-osx-x64.dmg` (Intel) | Ouvrez le .dmg, clic droit sur **Installa E-LAYER-ANALYZER.command** > Ouvrir. S'installe dans `~/E-LAYER-ANALYZER`. |
| Mac, application seule | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Décompressez **E-LAYER-ANALYZER.app** dans un dossier à vous (par exemple `~/E-LAYER-ANALYZER`, pas dans Applications) : le dossier `data` est créé à côté de l'app. Au premier démarrage, clic droit > Ouvrir. |

## Mise à jour

Installez la nouvelle version par-dessus l'ancienne : les réglages, les spots collectés, les logs et `call3.txt` sont conservés.

**Mise à jour automatique (depuis la version 0.4.5).** Au démarrage puis toutes les 6 heures, le programme consulte les Releases GitHub. Quand une nouvelle version est sortie, il ouvre une fenêtre et affiche un **▲ x.y.z** orange à côté de l'indicateur NTP dans la barre du haut :

**Depuis la version 0.4.12, les mises à jour sont automatiques et silencieuses **: quand une nouvelle version est sur GitHub, le programme la télécharge, l'installe et redémarre tout seul sans rien demander. Les fichiers manquants sont aussi retéléchargés sans demander. La fenêtre de mise à jour n'apparaît qu'en cliquant sur la version en haut, ou si vous écrivez agg_automatici=0 dans le fichier data/ela.ini.

- **Mettre à jour** télécharge le paquet portable pour votre ordinateur (Windows 64/32 bits, Linux, Raspberry Pi, Mac), ferme le programme, remplace les fichiers et le redémarre. Les réglages, les spots collectés, les logs et `call3.txt` sont conservés.
- **Plus tard** redemande à la vérification suivante ; **Ignorer cette version** cesse de proposer cette version.
- Un clic sur la version affichée dans la barre du haut (par exemple `v0.4.5`) recherche immédiatement les mises à jour. La vérification automatique peut être désactivée avec la case de la même fenêtre.
- Si le dossier du programme n'est pas accessible en écriture (par exemple un dossier système), utilisez plutôt le paquet d'installation de la page de la release.

**Fichiers manquants.** Au démarrage, le programme vérifie que tous les fichiers avec lesquels il a été distribué sont présents (traductions, documents, tables, listés dans `data/manifest.txt`). S'il en manque, une fenêtre les énumère et **Télécharger depuis GitHub** les récupère dans la même version sur GitHub.

## Configuration requise

- Pas de .NET à installer : le programme contient tout ce dont il a besoin.
- Connexion internet pour la carte (Natural Earth, au premier démarrage), les données d'aurore de la NOAA, les radars de YA-MS et les sessions telnet.
- Les Raspberry Pi Zero et Pi 1 ne sont pas pris en charge (leurs processeurs ne sont pas gérés par .NET).
- Il n'existe pas de version pour Linux i686 (PC 32 bits), car .NET n'existe pas pour ce système, ni pour FreeBSD, où manquent les bibliothèques graphiques d'Avalonia.
