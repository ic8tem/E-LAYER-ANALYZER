# Installation

[← Home](Home-fr)

Téléchargez le bon fichier depuis la page **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Le programme et le dossier `data` restent toujours ensemble dans un dossier à vous : **aucun fichier n'est jamais écrit dans C:\Users\...\AppData**.

| Système | Fichier | Installation |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Double-clic. S'installe dans `C:\E-LAYER-ANALYZER`, sans droits d'administrateur. Pas de raccourci dans le menu Démarrer (il se trouverait dans AppData), seulement l'icône facultative sur le bureau. |
| Windows 32 bits | `...-win-x86-setup.exe` | Comme ci-dessus, pour Windows 32 bits. |
| Windows sur ARM (PC Snapdragon) | `...-win-arm64-setup.exe` | Comme ci-dessus, pour Windows sur processeurs ARM. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Décompressez n'importe où et lancez `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, puis lancez-le depuis le menu : au premier démarrage le programme se copie dans `~/E-LAYER-ANALYZER`, avec le dossier `data` à côté. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (aussi `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (ou `zypper install`), puis comme le .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (ou `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, puis comme le .deb. |
| Slackware (x86_64 et ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (ou `-aarch64-`) | En root : `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, puis lancez `e-layer-analyzer` depuis le menu ou le terminal : au premier démarrage le programme se copie dans `~/E-LAYER-ANALYZER`. |
| Tout Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (ou `-aarch64.AppImage`) | Rendez-le exécutable et lancez-le : au premier démarrage il copie le programme dans `~/E-LAYER-ANALYZER` et le lance depuis là. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | Décompressez et lancez `./E-LAYER-ANALYZER`, ou lancez `./installa.sh` : s'installe dans `~/E-LAYER-ANALYZER` avec une icône dans le menu. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, puis comme la version portable pour Linux. |
| Raspberry Pi (64 bits) | `e-layer-analyzer_x.y.z_arm64.deb` ou `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 ou 5 avec **Raspberry Pi OS 64 bits** et bureau (4 Go de RAM recommandés). Le .deb comme ci-dessus ; la version portable comme pour Linux. |
| Raspberry Pi (32 bits) | `e-layer-analyzer_x.y.z_armhf.deb` ou `...-linux-arm-portatile.tar.gz` | Pi 2, 3 ou 4 avec **Raspberry Pi OS 32 bits**. Le .deb comme ci-dessus ; la version portable : décompressez et lancez `./E-LAYER-ANALYZER` ou `./installa.sh`. |
| Raspberry Pi avec Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` ou `...-linux-arm64-portatile.tar.gz` | Comme Slackware ci-dessus. La version portable comme pour Linux. |
| Linux armv7l (autres cartes ARM 32 bits) | `...-linux-armv7l-portatile.tar.gz` | Comme la version portable pour Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Expérimental, non vérifié.** Il faut le .NET 8 de la communauté (`pkg install dotnet`) et les bibliothèques natives SkiaSharp et HarfBuzzSharp compilées pour FreeBSD, qu'Avalonia ne fournit pas : sans elles, le programme ne démarre pas. Voir `LEGGIMI-FREEBSD.txt` dans le paquet. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ou `...-osx-x64.dmg` (Intel) | Ouvrez le .dmg, clic droit sur **Installa E-LAYER-ANALYZER.command** > Ouvrir. S'installe dans `~/E-LAYER-ANALYZER`. |
| Mac, application seule | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Décompressez **E-LAYER-ANALYZER.app** dans un dossier à vous (par exemple `~/E-LAYER-ANALYZER`, pas dans Applications) : le dossier `data` est créé à côté de l'app. Au premier démarrage, clic droit > Ouvrir. |

## Mise à jour

Installez la nouvelle version par-dessus l'ancienne : les réglages, les spots collectés, les logs et `call3.txt` sont conservés.

**Les paquets système (.deb, .rpm, Arch, Slackware) ne se mettent pas à jour automatiquement.** Avec eux, le programme signale seulement la nouvelle version : téléchargez le nouveau paquet depuis la page des Releases et installez-le comme la première fois (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Les mises à jour automatiques fonctionnent avec l'installateur et les zips de Windows, l'app du Mac, les paquets tar.gz portables et l'AppImage.

**Mise à jour automatique (depuis la version 0.4.5).** Au démarrage puis toutes les 6 heures, le programme consulte les Releases GitHub. Quand une nouvelle version est sortie, il ouvre une fenêtre et affiche un **▲ x.y.z** orange à côté de l'indicateur NTP dans la barre du haut :

**Depuis la version 0.4.12, les mises à jour sont automatiques et silencieuses** : quand une nouvelle version est sur GitHub, le programme la télécharge, l'installe et redémarre tout seul sans rien demander. Les fichiers manquants sont aussi retéléchargés sans demander. La fenêtre de mise à jour n'apparaît qu'en cliquant sur la version en haut, ou si vous désactivez les mises à jour automatiques dans RÉGLAGES (Général).

- **Mettre à jour** télécharge le paquet portable pour votre ordinateur (Windows 64/32 bits, Linux, Raspberry Pi, Mac), ferme le programme, remplace les fichiers et le redémarre. Les réglages, les spots collectés, les logs et `call3.txt` sont conservés.
- **Plus tard** redemande à la vérification suivante ; **Ignorer cette version** cesse de proposer cette version.
- Un clic sur la version affichée dans la barre du haut (par exemple `v0.4.5`) recherche immédiatement les mises à jour. La vérification automatique peut être désactivée avec la case de la même fenêtre.
- Si le dossier du programme n'est pas accessible en écriture (par exemple un dossier système), utilisez plutôt le paquet d'installation de la page de la release.

**Fichiers manquants.** Au démarrage, le programme vérifie que tous les fichiers avec lesquels il a été distribué sont présents (traductions, documents, tables, listés dans `data/manifest.txt`). S'il en manque, une fenêtre les énumère et **Télécharger depuis GitHub** les récupère dans la même version sur GitHub.

## Configuration requise

- Pas de .NET à installer : le programme contient tout ce dont il a besoin.
- Connexion internet pour la carte (Natural Earth, au premier démarrage), les données d'aurore de la NOAA, les radars de YA-MS et les sessions telnet.
- Les Raspberry Pi Zero et Pi 1 ne sont pas pris en charge (leurs processeurs ne sont pas gérés par .NET).
- Il n'existe pas de version pour Linux i686 (PC 32 bits) ni pour OpenBSD, car .NET n'existe pas pour ces systèmes. FreeBSD n'a que le paquet expérimental.
