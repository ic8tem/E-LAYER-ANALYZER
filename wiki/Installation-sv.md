# Installation

[← Home](Home-sv)

Ladda ned rätt fil från sidan **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programmet och mappen `data` hålls alltid ihop i en egen mapp: **ingen fil skrivs någonsin till C:\Users\...\AppData**.

| System | Fil | Så installerar du |
|---|---|---|
| Windows 64 bitar | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Dubbelklicka. Installeras i `C:\E-LAYER-ANALYZER`, inga administratörsrättigheter behövs. Ingen genväg i Start-menyn (den skulle hamna i AppData), bara den valfria ikonen på skrivbordet. |
| Windows 32 bitar | `...-windows-x86-setup.exe` | Som ovan, för 32-bitars Windows. |
| Windows portabel | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Packa upp var som helst och kör `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Packa upp och kör `./installa.sh`: installeras i `~/E-LAYER-ANALYZER` med en ikon i menyn. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 eller 5 med **64-bitars Raspberry Pi OS** och skrivbord (4 GB RAM rekommenderas). Packa upp och kör `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) eller `...-mac-intel.dmg` | Öppna .dmg-filen, högerklicka på **Installa E-LAYER-ANALYZER.command** > Öppna. Installeras i `~/E-LAYER-ANALYZER`. |

## Uppdatering

Installera den nya versionen ovanpå den gamla: inställningar, insamlade spots, loggar och `call3.txt` behålls.

## Krav

- Ingen .NET behöver installeras: programmet innehåller allt det behöver.
- Internetanslutning för kartan (Natural Earth, vid första starten), NOAA:s auroradata, YA-MS-radarerna och telnetsessionerna.
- Raspberry Pi Zero och Pi 1 stöds inte (deras processorer hanteras inte av .NET).
