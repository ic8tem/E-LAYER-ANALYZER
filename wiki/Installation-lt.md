# Diegimas

[← Home](Home-lt)

Atsisiųskite tinkamą failą iš puslapio **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programa ir aplankas `data` visada būna kartu jūsų pasirinktame aplanke: **į C:\Users\...\AppData niekada nerašomas joks failas**.

| Sistema | Failas | Kaip įdiegti |
|---|---|---|
| Windows 64 bitų | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Dukart spustelėkite. Įdiegiama į `C:\E-LAYER-ANALYZER`, administratoriaus teisių nereikia. Nuorodos meniu Start nėra (ji būtų AppData aplanke), tik neprivaloma piktograma darbalaukyje. |
| Windows 32 bitų | `...-windows-x86-setup.exe` | Kaip aukščiau, 32 bitų Windows. |
| Windows nešiojamoji | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Išskleiskite bet kur ir paleiskite `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Išskleiskite ir paleiskite `./installa.sh`: įdiegiama į `~/E-LAYER-ANALYZER`, piktograma pridedama į meniu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 arba 5 su **64 bitų Raspberry Pi OS** ir darbalaukiu (rekomenduojama 4 GB RAM). Išskleiskite ir paleiskite `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) arba `...-mac-intel.dmg` | Atverkite .dmg, dešiniuoju pelės mygtuku spustelėkite **Installa E-LAYER-ANALYZER.command** > Open. Įdiegiama į `~/E-LAYER-ANALYZER`. |

## Atnaujinimas

Įdiekite naują versiją ant senosios: nustatymai, surinkti spotai, žurnalai ir `call3.txt` išsaugomi.

## Reikalavimai

- .NET diegti nereikia: programoje yra viskas, ko jai reikia.
- Interneto ryšys reikalingas žemėlapiui (Natural Earth, pirmą kartą paleidžiant), NOAA pašvaistės duomenims, YA-MS radarams ir telnet seansams.
- Raspberry Pi Zero ir Pi 1 nepalaikomi (.NET nepalaiko jų procesorių).
