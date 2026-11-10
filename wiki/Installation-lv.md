# Instalēšana

[← Home](Home-lv)

Lejupielādē piemēroto failu no lapas **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programma un mape `data` vienmēr paliek kopā tavā mapē: **mapē C:\Users\...\AppData netiek ierakstīts neviens fails**.

| Sistēma | Fails | Kā instalēt |
|---|---|---|
| Windows 64 biti | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dubultklikšķis. Instalē mapē `C:\E-LAYER-ANALYZER`, administratora tiesības nav vajadzīgas. Nav saīsnes izvēlnē Sākt (tā atrastos AppData), tikai neobligāta ikona uz darbvirsmas. |
| Windows 32 biti | `...-win-x86-setup.exe` | Tāpat kā iepriekš, 32 bitu Windows. |
| Windows portatīvā | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Atarhivē jebkur un palaid `E-LAYER-ANALYZER.exe`. |
| Linux 64 biti (dators) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, pēc tam palaid no izvēlnes: pirmajā palaišanā programma nokopē sevi mapē `~/E-LAYER-ANALYZER`, ar mapi `data` blakus. |
| Linux portatīvā | `...-linux-x64-portatile.tar.gz` | Atarhivē un palaid `./E-LAYER-ANALYZER` vai palaid `./installa.sh`: instalē mapē `~/E-LAYER-ANALYZER` ar ikonu izvēlnē. |
| Raspberry Pi (64 biti) | `e-layer-analyzer_x.y.z_arm64.deb` vai `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 vai 5 ar **64 bitu Raspberry Pi OS** un darbvirsmu (ieteicami 4 GB RAM). .deb kā iepriekš; portatīvā versija kā Linux. |
| Raspberry Pi (32 biti) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 vai 4 ar **32 bitu Raspberry Pi OS**. Atarhivē un palaid `./E-LAYER-ANALYZER` vai `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) vai `...-osx-x64.dmg` (Intel) | Atver .dmg, labais klikšķis uz **Installa E-LAYER-ANALYZER.command** > Open. Instalē mapē `~/E-LAYER-ANALYZER`. |
| Mac, tikai lietotne | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Atarhivē **E-LAYER-ANALYZER.app** savā mapē (piemēram, `~/E-LAYER-ANALYZER`, nevis Applications): mape `data` tiek izveidota blakus lietotnei. Pirmajā palaišanā labais klikšķis > Open. |

## Atjaunināšana

Instalē jauno versiju virs vecās: iestatījumi, savāktie spoti, žurnāli un `call3.txt` saglabājas.

## Prasības

- .NET nav jāinstalē: programmā ir viss nepieciešamais.
- Interneta savienojums kartei (Natural Earth, pirmajā palaišanā), NOAA auroras datiem, YA-MS radariem un telnet sesijām.
- Raspberry Pi Zero un Pi 1 netiek atbalstīti (.NET neatbalsta to procesorus).
