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
| Raspberry Pi (32 bitti) | `e-layer-analyzer_x.y.z_armhf.deb` või `...-linux-arm-portatile.tar.gz` | Pi 2, 3 või 4 **32-bitise Raspberry Pi OS-iga**. .deb nagu eespool; kaasaskantav versioon: paki lahti ja käivita `./E-LAYER-ANALYZER` või `./installa.sh`. |
| Raspberry Pi Slackware ARM64-ga | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` või `...-linux-arm64-portatile.tar.gz` | Root-kasutajana: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, seejärel käivita `e-layer-analyzer` menüüst või terminalist: esimesel käivitamisel kopeerib programm end kausta `~/E-LAYER-ANALYZER`. Kaasaskantav versioon nagu Linuxi puhul. |
| Linux armv7l (muud 32-bitised ARM-plaadid) | `...-linux-armv7l-portatile.tar.gz` | Nagu Linuxi kaasaskantav versioon. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) või `...-osx-x64.dmg` (Intel) | Ava .dmg, paremklõps failil **Installa E-LAYER-ANALYZER.command** > Open. Paigaldub kausta `~/E-LAYER-ANALYZER`. |
| Mac, ainult rakendus | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Paki **E-LAYER-ANALYZER.app** lahti oma valitud kausta (näiteks `~/E-LAYER-ANALYZER`, mitte Applications): kaust `data` luuakse rakenduse kõrvale. Esimesel käivitusel paremklõps > Open. |

## Uuendamine

Paigalda uus versioon vana peale: seaded, kogutud spotid, logid ja `call3.txt` säilivad.

**Automaatne uuendamine (alates versioonist 0.4.5).** Käivitamisel ja seejärel iga 30 minuti järel kontrollib programm GitHubi Releases-lehte. Kui ilmub uus versioon, avab programm akna ja näitab ülemisel ribal NTP näidiku kõrval oranži **▲ x.y.z**:

**Alates versioonist 0.4.12 on uuendused automaatsed ja vaiksed**: kui GitHubis on uus versioon, laadib programm selle alla, paigaldab ja käivitub ise uuesti midagi küsimata. Ka puuduvad failid laaditakse uuesti alla küsimata. Uuendusaken ilmub ainult üleval versioonil klõpsates või kui kirjutad faili data/ela.ini rea agg_automatici=0.

- **Uuenda kohe** laadib alla sinu arvutile sobiva kaasaskantava paketi (Windows 64/32 bitti, Linux, Raspberry Pi, Mac), sulgeb programmi, asendab failid ja käivitab selle uuesti. Seaded, kogutud spotid, logid ja `call3.txt` säilivad.
- **Hiljem** küsib uuesti järgmisel kontrollil; **Jäta see versioon vahele** lõpetab selle versiooni pakkumise.
- Klõps ülemisel ribal näidatud versioonil (näiteks `v0.4.5`) kontrollib uuendusi kohe. Automaatse kontrolli saab välja lülitada samas aknas oleva märkeruuduga.
- Kui programmi kausta ei saa kirjutada (näiteks süsteemikaust), kasuta selle asemel väljalaske lehel olevat paigalduspaketti.

**Puuduvad failid.** Käivitamisel kontrollib programm, kas olemas on kõik failid, millega see levitati (tõlked, dokumendid, tabelid, loetletud failis `data/manifest.txt`). Kui mõni puudub, loetleb aken need ja **Laadi alla GitHubist** toob need uuesti samast versioonist GitHubis.

## Nõuded

- .NET-i pole vaja paigaldada: programm sisaldab kõike vajalikku.
- Internetiühendus on vajalik kaardi (Natural Earth, esimesel käivitusel), NOAA aurora andmete, YA-MS-i radarite ja telneti seansside jaoks.
- Raspberry Pi Zero ja Pi 1 ei ole toetatud (.NET ei toeta nende protsessoreid).
- Linux i686 (32-bitised arvutid) jaoks versioone pole, sest .NET-i selle süsteemi jaoks ei ole olemas, ega ka FreeBSD jaoks, kus puuduvad Avalonia graafikateegid.
