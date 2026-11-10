# Diegimas

[← Home](Home-lt)

Atsisiųskite tinkamą failą iš puslapio **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programa ir aplankas `data` visada būna kartu jūsų pasirinktame aplanke: **į C:\Users\...\AppData niekada nerašomas joks failas**.

| Sistema | Failas | Kaip įdiegti |
|---|---|---|
| Windows 64 bitų | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dukart spustelėkite. Įdiegiama į `C:\E-LAYER-ANALYZER`, administratoriaus teisių nereikia. Nuorodos meniu Start nėra (ji būtų AppData aplanke), tik neprivaloma piktograma darbalaukyje. |
| Windows 32 bitų | `...-win-x86-setup.exe` | Kaip aukščiau, 32 bitų Windows. |
| Windows nešiojamoji | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Išskleiskite bet kur ir paleiskite `E-LAYER-ANALYZER.exe`. |
| Linux 64 bitų (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, tada paleiskite iš meniu: pirmą kartą paleidžiant programa nusikopijuoja į `~/E-LAYER-ANALYZER`, šalia – aplankas `data`. |
| Linux nešiojamoji | `...-linux-x64-portatile.tar.gz` | Išskleiskite ir paleiskite `./E-LAYER-ANALYZER` arba paleiskite `./installa.sh`: įdiegiama į `~/E-LAYER-ANALYZER`, piktograma pridedama į meniu. |
| Raspberry Pi (64 bitų) | `e-layer-analyzer_x.y.z_arm64.deb` arba `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 arba 5 su **64 bitų Raspberry Pi OS** ir darbalaukiu (rekomenduojama 4 GB RAM). .deb – kaip aukščiau; nešiojamoji versija – kaip Linux. |
| Raspberry Pi (32 bitų) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 arba 4 su **32 bitų Raspberry Pi OS**. Išskleiskite ir paleiskite `./E-LAYER-ANALYZER` arba `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) arba `...-osx-x64.dmg` (Intel) | Atverkite .dmg, dešiniuoju pelės mygtuku spustelėkite **Installa E-LAYER-ANALYZER.command** > Open. Įdiegiama į `~/E-LAYER-ANALYZER`. |
| Mac, tik programa | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Išskleiskite **E-LAYER-ANALYZER.app** į savo pasirinktą aplanką (pavyzdžiui, `~/E-LAYER-ANALYZER`, ne Applications): aplankas `data` sukuriamas šalia programos. Pirmą kartą paleidžiant spustelėkite dešiniuoju pelės mygtuku > Open. |

## Atnaujinimas

Įdiekite naują versiją ant senosios: nustatymai, surinkti spotai, žurnalai ir `call3.txt` išsaugomi.

## Reikalavimai

- .NET diegti nereikia: programoje yra viskas, ko jai reikia.
- Interneto ryšys reikalingas žemėlapiui (Natural Earth, pirmą kartą paleidžiant), NOAA pašvaistės duomenims, YA-MS radarams ir telnet seansams.
- Raspberry Pi Zero ir Pi 1 nepalaikomi (.NET nepalaiko jų procesorių).
