# Instalēšana

[← Home](Home-lv)

Lejupielādē piemēroto failu no lapas **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programma un mape `data` vienmēr paliek kopā tavā mapē: **mapē C:\Users\...\AppData netiek ierakstīts neviens fails**.

| Sistēma | Fails | Kā instalēt |
|---|---|---|
| Windows 64 biti | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Dubultklikšķis. Instalē mapē `C:\E-LAYER-ANALYZER`, administratora tiesības nav vajadzīgas. Nav saīsnes izvēlnē Sākt (tā atrastos AppData), tikai neobligāta ikona uz darbvirsmas. |
| Windows 32 biti | `...-windows-x86-setup.exe` | Tāpat kā iepriekš, 32 bitu Windows. |
| Windows portatīvā | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Atarhivē jebkur un palaid `E-LAYER-ANALYZER.exe`. |
| Linux (dators) | `...-linux-x64.tar.gz` | Atarhivē un palaid `./installa.sh`: instalē mapē `~/E-LAYER-ANALYZER` ar ikonu izvēlnē. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 vai 5 ar **64 bitu Raspberry Pi OS** un darbvirsmu (ieteicami 4 GB RAM). Atarhivē un palaid `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) vai `...-mac-intel.dmg` | Atver .dmg, labais klikšķis uz **Installa E-LAYER-ANALYZER.command** > Open. Instalē mapē `~/E-LAYER-ANALYZER`. |

## Atjaunināšana

Instalē jauno versiju virs vecās: iestatījumi, savāktie spoti, žurnāli un `call3.txt` saglabājas.

## Prasības

- .NET nav jāinstalē: programmā ir viss nepieciešamais.
- Interneta savienojums kartei (Natural Earth, pirmajā palaišanā), NOAA auroras datiem, YA-MS radariem un telnet sesijām.
- Raspberry Pi Zero un Pi 1 netiek atbalstīti (.NET neatbalsta to procesorus).
