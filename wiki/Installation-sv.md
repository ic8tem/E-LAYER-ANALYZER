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
| Raspberry Pi (32 bitar) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eller 4 med **32-bitars Raspberry Pi OS**. Packa upp och kör `./E-LAYER-ANALYZER` eller `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eller `...-osx-x64.dmg` (Intel) | Öppna .dmg-filen, högerklicka på **Installa E-LAYER-ANALYZER.command** > Öppna. Installeras i `~/E-LAYER-ANALYZER`. |
| Mac, endast programmet | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Packa upp **E-LAYER-ANALYZER.app** i en egen mapp (till exempel `~/E-LAYER-ANALYZER`, inte Applications): mappen `data` skapas bredvid programmet. Vid första starten: högerklicka > Öppna. |

## Uppdatering

Installera den nya versionen ovanpå den gamla: inställningar, insamlade spots, loggar och `call3.txt` behålls.

## Krav

- Ingen .NET behöver installeras: programmet innehåller allt det behöver.
- Internetanslutning för kartan (Natural Earth, vid första starten), NOAA:s auroradata, YA-MS-radarerna och telnetsessionerna.
- Raspberry Pi Zero och Pi 1 stöds inte (deras processorer hanteras inte av .NET).
