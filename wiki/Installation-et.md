# Paigaldamine

[← Home](Home-et)

Laadi õige fail alla lehelt **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programm ja kaust `data` on alati koos sinu enda valitud kaustas: **kausta C:\Users\...\AppData ei kirjutata kunagi ühtegi faili**.

| Süsteem | Fail | Kuidas paigaldada |
|---|---|---|
| Windows 64 bitti | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Topeltklõps. Paigaldub kausta `C:\E-LAYER-ANALYZER`, administraatoriõigusi pole vaja. Start-menüü otseteed ei looda (see asuks AppData kaustas), ainult valikuline töölaua ikoon. |
| Windows 32 bitti | `...-win-x86-setup.exe` | Nagu eespool, 32-bitise Windowsi jaoks. |
| Windows, kaasaskantav | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Paki lahti ükskõik kuhu ja käivita `E-LAYER-ANALYZER.exe`. |
| Linux 64 bitti (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, seejärel käivita menüüst: esimesel käivitusel kopeerib programm end kausta `~/E-LAYER-ANALYZER`, kaust `data` selle kõrval. |
| Linux, kaasaskantav | `...-linux-x64-portatile.tar.gz` | Paki lahti ja käivita `./E-LAYER-ANALYZER` või käivita `./installa.sh`: paigaldub kausta `~/E-LAYER-ANALYZER` ja lisab ikooni menüüsse. |
| Raspberry Pi (64 bitti) | `e-layer-analyzer_x.y.z_arm64.deb` või `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 või 5 **64-bitise Raspberry Pi OS-iga** ja töölauaga (soovitatav 4 GB RAM-i). .deb nagu eespool; kaasaskantav versioon nagu Linuxi puhul. |
| Raspberry Pi (32 bitti) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 või 4 **32-bitise Raspberry Pi OS-iga**. Paki lahti ja käivita `./E-LAYER-ANALYZER` või `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) või `...-osx-x64.dmg` (Intel) | Ava .dmg, paremklõps failil **Installa E-LAYER-ANALYZER.command** > Open. Paigaldub kausta `~/E-LAYER-ANALYZER`. |
| Mac, ainult rakendus | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Paki **E-LAYER-ANALYZER.app** lahti oma valitud kausta (näiteks `~/E-LAYER-ANALYZER`, mitte Applications): kaust `data` luuakse rakenduse kõrvale. Esimesel käivitusel paremklõps > Open. |

## Uuendamine

Paigalda uus versioon vana peale: seaded, kogutud spotid, logid ja `call3.txt` säilivad.

## Nõuded

- .NET-i pole vaja paigaldada: programm sisaldab kõike vajalikku.
- Internetiühendus on vajalik kaardi (Natural Earth, esimesel käivitusel), NOAA aurora andmete, YA-MS-i radarite ja telneti seansside jaoks.
- Raspberry Pi Zero ja Pi 1 ei ole toetatud (.NET ei toeta nende protsessoreid).
