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
| Raspberry Pi (32 bitų) | `e-layer-analyzer_x.y.z_armhf.deb` arba `...-linux-arm-portatile.tar.gz` | Pi 2, 3 arba 4 su **32 bitų Raspberry Pi OS**. .deb – kaip aukščiau; nešiojamoji versija: išskleiskite ir paleiskite `./E-LAYER-ANALYZER` arba `./installa.sh`. |
| Raspberry Pi su Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` arba `...-linux-arm64-portatile.tar.gz` | Root teisėmis: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, tada paleiskite `e-layer-analyzer` iš meniu arba terminalo: pirmą kartą paleista programa nusikopijuoja į `~/E-LAYER-ANALYZER`. Nešiojamoji versija – kaip Linux. |
| Linux armv7l (kitos 32 bitų ARM plokštės) | `...-linux-armv7l-portatile.tar.gz` | Kaip Linux nešiojamoji versija. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) arba `...-osx-x64.dmg` (Intel) | Atverkite .dmg, dešiniuoju pelės mygtuku spustelėkite **Installa E-LAYER-ANALYZER.command** > Open. Įdiegiama į `~/E-LAYER-ANALYZER`. |
| Mac, tik programa | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Išskleiskite **E-LAYER-ANALYZER.app** į savo pasirinktą aplanką (pavyzdžiui, `~/E-LAYER-ANALYZER`, ne Applications): aplankas `data` sukuriamas šalia programos. Pirmą kartą paleidžiant spustelėkite dešiniuoju pelės mygtuku > Open. |

## Atnaujinimas

Įdiekite naują versiją ant senosios: nustatymai, surinkti spotai, žurnalai ir `call3.txt` išsaugomi.

**Automatinis atnaujinimas (nuo 0.4.5 versijos).** Paleidžiant ir vėliau kas 30 minučių programa tikrina GitHub Releases. Pasirodžius naujai versijai, ji atidaro langą ir viršutinėje juostoje šalia NTP indikatoriaus parodo oranžinį **▲ x.y.z**:

**Nuo 0.4.12 versijos atnaujinimai yra automatiniai ir tylūs**: kai GitHub yra nauja versija, programa ją atsisiunčia, įdiegia ir pati paleidžiama iš naujo nieko neklausdama. Trūkstami failai taip pat atsisiunčiami iš naujo neklausiant. Atnaujinimo langas atsiranda tik spustelėjus versiją viršuje arba jei faile data/ela.ini įrašai agg_automatici=0.

- **Atnaujinti dabar** atsisiunčia jūsų kompiuteriui skirtą nešiojamąjį paketą (Windows 64/32 bitų, Linux, Raspberry Pi, Mac), uždaro programą, pakeičia failus ir ją paleidžia iš naujo. Nustatymai, surinkti spotai, žurnalai ir `call3.txt` išsaugomi.
- **Vėliau** paklausia dar kartą per kitą patikrinimą; **Praleisti šią versiją** nustoja siūlyti tą versiją.
- Spustelėjus viršutinėje juostoje rodomą versiją (pavyzdžiui, `v0.4.5`), atnaujinimai patikrinami iš karto. Automatinį tikrinimą galima išjungti žymimuoju langeliu tame pačiame lange.
- Jei į programos aplanką negalima rašyti (pavyzdžiui, sisteminis aplankas), vietoj to naudokite diegimo paketą iš leidimo puslapio.

**Trūkstami failai.** Paleidžiant programa tikrina, ar yra visi failai, su kuriais ji buvo platinama (vertimai, dokumentai, lentelės, išvardyti `data/manifest.txt`). Jei kurio nors trūksta, langas juos išvardija, o **Atsisiųsti iš GitHub** juos vėl atsisiunčia iš tos pačios versijos GitHub.

## Reikalavimai

- .NET diegti nereikia: programoje yra viskas, ko jai reikia.
- Interneto ryšys reikalingas žemėlapiui (Natural Earth, pirmą kartą paleidžiant), NOAA pašvaistės duomenims, YA-MS radarams ir telnet seansams.
- Raspberry Pi Zero ir Pi 1 nepalaikomi (.NET nepalaiko jų procesorių).
- Versijų Linux i686 (32 bitų kompiuteriams) nėra, nes .NET šiai sistemai neegzistuoja, taip pat nėra FreeBSD, kurioje trūksta Avalonia grafikos bibliotekų.
