# Installation

[← Home](Home-sv)

Ladda ned rätt fil från sidan **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programmet och mappen `data` hålls alltid ihop i en egen mapp: **ingen fil skrivs någonsin till C:\Users\...\AppData**.

| System | Fil | Så installerar du |
|---|---|---|
| Windows 64 bitar | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dubbelklicka. Installeras i `C:\E-LAYER-ANALYZER`, inga administratörsrättigheter behövs. Ingen genväg i Start-menyn (den skulle hamna i AppData), bara den valfria ikonen på skrivbordet. |
| Windows 32 bitar | `...-win-x86-setup.exe` | Som ovan, för 32-bitars Windows. |
| Windows portabel | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Packa upp var som helst och kör `E-LAYER-ANALYZER.exe`. |
| Linux 64 bitar (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, starta det sedan från menyn: vid första starten kopierar programmet sig självt till `~/E-LAYER-ANALYZER`, med mappen `data` bredvid. |
| Linux portabel | `...-linux-x64-portatile.tar.gz` | Packa upp och kör `./E-LAYER-ANALYZER`, eller kör `./installa.sh`: installeras i `~/E-LAYER-ANALYZER` med en ikon i menyn. |
| Raspberry Pi (64 bitar) | `e-layer-analyzer_x.y.z_arm64.deb` eller `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 eller 5 med **64-bitars Raspberry Pi OS** och skrivbord (4 GB RAM rekommenderas). .deb-filen som ovan; den portabla versionen som för Linux. |
| Raspberry Pi (32 bitar) | `e-layer-analyzer_x.y.z_armhf.deb` eller `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eller 4 med **32-bitars Raspberry Pi OS**. .deb-filen som ovan; den portabla versionen: packa upp och kör `./E-LAYER-ANALYZER` eller `./installa.sh`. |
| Raspberry Pi med Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` eller `...-linux-arm64-portatile.tar.gz` | Som root: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, starta sedan `e-layer-analyzer` från menyn eller terminalen: vid första starten kopierar programmet sig till `~/E-LAYER-ANALYZER`. Den portabla versionen som för Linux. |
| Linux armv7l (andra 32-bitars ARM-kort) | `...-linux-armv7l-portatile.tar.gz` | Som den portabla versionen för Linux. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eller `...-osx-x64.dmg` (Intel) | Öppna .dmg-filen, högerklicka på **Installa E-LAYER-ANALYZER.command** > Öppna. Installeras i `~/E-LAYER-ANALYZER`. |
| Mac, endast programmet | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Packa upp **E-LAYER-ANALYZER.app** i en egen mapp (till exempel `~/E-LAYER-ANALYZER`, inte Applications): mappen `data` skapas bredvid programmet. Vid första starten: högerklicka > Öppna. |

## Uppdatering

Installera den nya versionen ovanpå den gamla: inställningar, insamlade spots, loggar och `call3.txt` behålls.

**Automatisk uppdatering (från version 0.4.5).** Vid start och sedan var 6:e timme kontrollerar programmet GitHub Releases. När en ny version har kommit ut öppnar det ett fönster och visar ett orange **▲ x.y.z** bredvid NTP-indikatorn i fältet högst upp:

- **Uppdatera nu** hämtar det portabla paketet för din dator (Windows 64/32 bitar, Linux, Raspberry Pi, Mac), stänger programmet, ersätter filerna och startar om det. Inställningar, insamlade spots, loggar och `call3.txt` behålls.
- **Senare** frågar igen vid nästa kontroll; **Hoppa över versionen** slutar föreslå den versionen.
- Ett klick på versionen i fältet högst upp (till exempel `v0.4.5`) söker direkt efter uppdateringar. Den automatiska kontrollen kan stängas av med rutan i samma fönster.
- Om programmappen inte är skrivbar (till exempel en systemmapp), använd i stället installationspaketet från releasesidan.

**Saknade filer.** Vid start kontrollerar programmet att alla filer som det distribuerades med finns (översättningar, dokument, tabeller, listade i `data/manifest.txt`). Saknas någon listas de i ett fönster, och **Hämta från GitHub** hämtar dem igen från samma version på GitHub.

## Krav

- Ingen .NET behöver installeras: programmet innehåller allt det behöver.
- Internetanslutning för kartan (Natural Earth, vid första starten), NOAA:s auroradata, YA-MS-radarerna och telnetsessionerna.
- Raspberry Pi Zero och Pi 1 stöds inte (deras processorer hanteras inte av .NET).
- Det finns inga versioner för Linux i686 (32-bitars pc), eftersom .NET inte finns för det systemet, och inte heller för FreeBSD, där Avalonias grafikbibliotek saknas.
