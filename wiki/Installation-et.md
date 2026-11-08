# Paigaldamine

[← Home](Home-et)

Laadi õige fail alla lehelt **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programm ja kaust `data` on alati koos sinu enda valitud kaustas: **kausta C:\Users\...\AppData ei kirjutata kunagi ühtegi faili**.

| Süsteem | Fail | Kuidas paigaldada |
|---|---|---|
| Windows 64 bitti | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Topeltklõps. Paigaldub kausta `C:\E-LAYER-ANALYZER`, administraatoriõigusi pole vaja. Start-menüü otseteed ei looda (see asuks AppData kaustas), ainult valikuline töölaua ikoon. |
| Windows 32 bitti | `...-windows-x86-setup.exe` | Nagu eespool, 32-bitise Windowsi jaoks. |
| Windows, kaasaskantav | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Paki lahti ükskõik kuhu ja käivita `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Paki lahti ja käivita `./installa.sh`: paigaldub kausta `~/E-LAYER-ANALYZER` ja lisab ikooni menüüsse. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 või 5 **64-bitise Raspberry Pi OS-iga** ja töölauaga (soovitatav 4 GB RAM-i). Paki lahti ja käivita `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) või `...-mac-intel.dmg` | Ava .dmg, paremklõps failil **Installa E-LAYER-ANALYZER.command** > Open. Paigaldub kausta `~/E-LAYER-ANALYZER`. |

## Uuendamine

Paigalda uus versioon vana peale: seaded, kogutud spotid, logid ja `call3.txt` säilivad.

## Nõuded

- .NET-i pole vaja paigaldada: programm sisaldab kõike vajalikku.
- Internetiühendus on vajalik kaardi (Natural Earth, esimesel käivitusel), NOAA aurora andmete, YA-MS-i radarite ja telneti seansside jaoks.
- Raspberry Pi Zero ja Pi 1 ei ole toetatud (.NET ei toeta nende protsessoreid).
