# Instalace

[← Domů](Home-cs)

Stáhněte si správný soubor ze stránky **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Program a složka `data` zůstávají vždy spolu ve vaší vlastní složce: **do C:\Users\...\AppData se nikdy nezapisuje žádný soubor**.

| Systém | Soubor | Jak nainstalovat |
|---|---|---|
| Windows 64 bitů | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dvojklik. Instaluje se do `C:\E-LAYER-ANALYZER`, bez práv správce. Žádný zástupce v nabídce Start (byl by v AppData), jen volitelná ikona na ploše. |
| Windows 32 bitů | `...-win-x86-setup.exe` | Jako výše, pro 32bitové Windows. |
| Windows přenosná verze | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Rozbalte kamkoli a spusťte `E-LAYER-ANALYZER.exe`. |
| Linux 64 bitů (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, pak spusťte z nabídky: při prvním spuštění se program zkopíruje do `~/E-LAYER-ANALYZER` se složkou `data` vedle sebe. |
| Linux přenosná verze | `...-linux-x64-portatile.tar.gz` | Rozbalte a spusťte `./E-LAYER-ANALYZER`, nebo spusťte `./installa.sh`: nainstaluje se do `~/E-LAYER-ANALYZER` s ikonou v nabídce. |
| Raspberry Pi (64 bitů) | `e-layer-analyzer_x.y.z_arm64.deb` nebo `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 nebo 5 s **64bitovým Raspberry Pi OS** a desktopem (doporučeno 4 GB RAM). Balíček .deb jako výše; přenosnou verzi jako pro Linux. |
| Raspberry Pi (32 bitů) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 nebo 4 s **32bitovým Raspberry Pi OS**. Rozbalte a spusťte `./E-LAYER-ANALYZER` nebo `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) nebo `...-osx-x64.dmg` (Intel) | Otevřete .dmg, pravým tlačítkem na **Installa E-LAYER-ANALYZER.command** > Otevřít. Instaluje se do `~/E-LAYER-ANALYZER`. |
| Mac, jen aplikace | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Rozbalte **E-LAYER-ANALYZER.app** do vlastní složky (například `~/E-LAYER-ANALYZER`, ne do Aplikací): složka `data` se vytvoří vedle aplikace. Při prvním spuštění pravé tlačítko > Otevřít. |

## Aktualizace

Nainstalujte novou verzi přes starou: nastavení, shromážděné spoty, deníky a `call3.txt` zůstanou zachovány.

## Požadavky

- Není třeba instalovat .NET: program obsahuje vše, co potřebuje.
- Připojení k internetu pro mapu (Natural Earth, při prvním spuštění), data aurory NOAA, radary YA-MS a relace telnet.
- Raspberry Pi Zero a Pi 1 nejsou podporovány (jejich procesory .NET nepodporuje).
