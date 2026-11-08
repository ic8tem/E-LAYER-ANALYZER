# Installation

[← Home](Home-fr)

Téléchargez le bon fichier depuis la page **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Le programme et le dossier `data` restent toujours ensemble dans un dossier à vous : **aucun fichier n'est jamais écrit dans C:\Users\...\AppData**.

| Système | Fichier | Installation |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Double-clic. S'installe dans `C:\E-LAYER-ANALYZER`, sans droits d'administrateur. Pas de raccourci dans le menu Démarrer (il se trouverait dans AppData), seulement l'icône facultative sur le bureau. |
| Windows 32 bits | `...-windows-x86-setup.exe` | Comme ci-dessus, pour Windows 32 bits. |
| Windows portable | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Décompressez n'importe où et lancez `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Décompressez et lancez `./installa.sh` : s'installe dans `~/E-LAYER-ANALYZER` avec une icône dans le menu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 ou 5 avec **Raspberry Pi OS 64 bits** et bureau (4 Go de RAM recommandés). Décompressez et lancez `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) ou `...-mac-intel.dmg` | Ouvrez le .dmg, clic droit sur **Installa E-LAYER-ANALYZER.command** > Ouvrir. S'installe dans `~/E-LAYER-ANALYZER`. |

## Mise à jour

Installez la nouvelle version par-dessus l'ancienne : les réglages, les spots collectés, les logs et `call3.txt` sont conservés.

## Configuration requise

- Pas de .NET à installer : le programme contient tout ce dont il a besoin.
- Connexion internet pour la carte (Natural Earth, au premier démarrage), les données d'aurore de la NOAA, les radars de YA-MS et les sessions telnet.
- Les Raspberry Pi Zero et Pi 1 ne sont pas pris en charge (leurs processeurs ne sont pas gérés par .NET).
