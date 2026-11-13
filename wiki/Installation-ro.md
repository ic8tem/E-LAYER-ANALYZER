# Instalare

[← Home](Home-ro)

Descarcă fișierul potrivit de pe pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programul și dosarul `data` stau întotdeauna împreună, într-un dosar ales de tine: **niciun fișier nu este scris vreodată în C:\Users\...\AppData**.

| Sistem | Fișier | Cum se instalează |
|---|---|---|
| Windows 64 de biți | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dublu clic. Se instalează în `C:\E-LAYER-ANALYZER`, fără drepturi de administrator. Nicio scurtătură în meniul Start (ar sta în AppData), doar pictograma opțională de pe desktop. |
| Windows 32 de biți | `...-win-x86-setup.exe` | Ca mai sus, pentru Windows pe 32 de biți. |
| Windows portabil | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Dezarhivează oriunde și pornește `E-LAYER-ANALYZER.exe`. |
| Linux 64 de biți (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, apoi pornește-l din meniu: la prima pornire programul se copiază în `~/E-LAYER-ANALYZER`, cu dosarul `data` alături. |
| Linux portabil | `...-linux-x64-portatile.tar.gz` | Dezarhivează și pornește `./E-LAYER-ANALYZER`, sau rulează `./installa.sh`: se instalează în `~/E-LAYER-ANALYZER` cu o pictogramă în meniu. |
| Raspberry Pi (64 de biți) | `e-layer-analyzer_x.y.z_arm64.deb` sau `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 sau 5 cu **Raspberry Pi OS pe 64 de biți** și desktop (recomandat 4 GB de RAM). Pachetul .deb ca mai sus; versiunea portabilă ca pentru Linux. |
| Raspberry Pi (32 de biți) | `e-layer-analyzer_x.y.z_armhf.deb` sau `...-linux-arm-portatile.tar.gz` | Pi 2, 3 sau 4 cu **Raspberry Pi OS pe 32 de biți**. Pachetul .deb ca mai sus; versiunea portabilă: dezarhivează și pornește `./E-LAYER-ANALYZER` sau `./installa.sh`. |
| Raspberry Pi cu Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` sau `...-linux-arm64-portatile.tar.gz` | Ca root: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, apoi pornește `e-layer-analyzer` din meniu sau din terminal: la prima pornire programul se copiază în `~/E-LAYER-ANALYZER`. Versiunea portabilă ca pentru Linux. |
| Linux armv7l (alte plăci ARM pe 32 de biți) | `...-linux-armv7l-portatile.tar.gz` | Ca versiunea portabilă pentru Linux. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) sau `...-osx-x64.dmg` (Intel) | Deschide fișierul .dmg, clic dreapta pe **Installa E-LAYER-ANALYZER.command** > Open. Se instalează în `~/E-LAYER-ANALYZER`. |
| Mac, doar aplicația | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Dezarhivează **E-LAYER-ANALYZER.app** într-un dosar ales de tine (de exemplu `~/E-LAYER-ANALYZER`, nu Applications): dosarul `data` se creează lângă aplicație. La prima pornire clic dreapta > Open. |

## Actualizare

Instalează noua versiune peste cea veche: setările, spoturile colectate, logurile și `call3.txt` se păstrează.

**Actualizare automată (de la versiunea 0.4.5).** La pornire și apoi la fiecare 6 ore programul verifică Releases de pe GitHub. Când apare o versiune nouă, deschide o fereastră și afișează un **▲ x.y.z** portocaliu lângă indicatorul NTP din bara de sus:

**Începând cu versiunea 0.4.12 actualizările sunt automate și silențioase**: când pe GitHub există o versiune nouă, programul o descarcă, o instalează și repornește singur fără să întrebe nimic. Și fișierele lipsă se descarcă din nou fără să întrebe. Fereastra de actualizare apare doar cu un clic pe versiunea de sus sau dacă scrii agg_automatici=0 în fișierul data/ela.ini.

- **Actualizează acum** descarcă pachetul portabil pentru calculatorul tău (Windows 64/32 de biți, Linux, Raspberry Pi, Mac), închide programul, înlocuiește fișierele și îl repornește. Setările, spoturile colectate, logurile și `call3.txt` se păstrează.
- **Mai târziu** întreabă din nou la următoarea verificare; **Omite această versiune** nu mai propune versiunea respectivă.
- Un clic pe versiunea afișată în bara de sus (de exemplu `v0.4.5`) verifică imediat actualizările. Verificarea automată se poate dezactiva din caseta din aceeași fereastră.
- Dacă în dosarul programului nu se poate scrie (de exemplu un dosar de sistem), folosește în schimb pachetul de instalare de pe pagina lansării.

**Fișiere lipsă.** La pornire programul verifică dacă sunt prezente toate fișierele cu care a fost distribuit (traduceri, documente, tabele, enumerate în `data/manifest.txt`). Dacă lipsește vreunul, o fereastră le enumeră, iar **Descarcă de pe GitHub** le preia din nou din aceeași versiune de pe GitHub.

## Cerințe

- Nu trebuie instalat .NET: programul conține tot ce îi trebuie.
- Conexiune la internet pentru hartă (Natural Earth, la prima pornire), datele de auroră NOAA, radarele YA-MS și sesiunile telnet.
- Raspberry Pi Zero și Pi 1 nu sunt suportate (procesoarele lor nu sunt suportate de .NET).
- Nu există versiuni pentru Linux i686 (PC-uri pe 32 de biți), pentru că .NET nu există pentru acel sistem, nici pentru FreeBSD, unde lipsesc bibliotecile grafice Avalonia.
