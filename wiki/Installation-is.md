# Uppsetning

[← Home](Home-is)

Sæktu réttu skrána af síðunni **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Forritið og mappan `data` eru alltaf saman í eigin möppu: **engin skrá er nokkurn tíma skrifuð í C:\Users\...\AppData**.

| Kerfi | Skrá | Hvernig á að setja upp |
|---|---|---|
| Windows 64 bita | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Tvísmelltu. Sett upp í `C:\E-LAYER-ANALYZER`, engin stjórnandaréttindi nauðsynleg. Engin flýtileið í Start-valmyndinni (hún yrði í AppData), aðeins valfrjálst tákn á skjáborðinu. |
| Windows 32 bita | `...-win-x86-setup.exe` | Eins og hér að ofan, fyrir 32 bita Windows. |
| Windows færanleg útgáfa | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Afþjappaðu hvar sem er og keyrðu `E-LAYER-ANALYZER.exe`. |
| Linux 64 bita (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, ræstu það síðan úr valmyndinni: við fyrstu ræsingu afritar forritið sig í `~/E-LAYER-ANALYZER`, með möppuna `data` við hliðina. |
| Linux færanleg útgáfa | `...-linux-x64-portatile.tar.gz` | Afþjappaðu og keyrðu `./E-LAYER-ANALYZER`, eða keyrðu `./installa.sh`: sett upp í `~/E-LAYER-ANALYZER` með tákni í valmyndinni. |
| Raspberry Pi (64 bita) | `e-layer-analyzer_x.y.z_arm64.deb` eða `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 eða 5 með **64 bita Raspberry Pi OS** og skjáborði (mælt með 4 GB vinnsluminni). .deb eins og hér að ofan; færanlega útgáfan eins og fyrir Linux. |
| Raspberry Pi (32 bita) | `e-layer-analyzer_x.y.z_armhf.deb` eða `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eða 4 með **32 bita Raspberry Pi OS**. .deb eins og hér að ofan; færanlega útgáfan: afþjappaðu og keyrðu `./E-LAYER-ANALYZER` eða `./installa.sh`. |
| Raspberry Pi með Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` eða `...-linux-arm64-portatile.tar.gz` | Sem root: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, ræstu síðan `e-layer-analyzer` úr valmyndinni eða skipanalínunni: við fyrstu ræsingu afritar forritið sig í `~/E-LAYER-ANALYZER`. Færanlega útgáfan eins og fyrir Linux. |
| Linux armv7l (önnur 32 bita ARM-borð) | `...-linux-armv7l-portatile.tar.gz` | Eins og færanlega útgáfan fyrir Linux. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eða `...-osx-x64.dmg` (Intel) | Opnaðu .dmg-skrána, hægrismelltu á **Installa E-LAYER-ANALYZER.command** > Opna. Sett upp í `~/E-LAYER-ANALYZER`. |
| Mac, aðeins forritið | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Afþjappaðu **E-LAYER-ANALYZER.app** í eigin möppu (til dæmis `~/E-LAYER-ANALYZER`, ekki Applications): mappan `data` er búin til við hliðina á forritinu. Við fyrstu ræsingu hægrismelltu > Opna. |

## Uppfærsla

Settu nýju útgáfuna upp yfir þá gömlu: stillingar, söfnuð spot, dagbækur og `call3.txt` haldast.

**Sjálfvirk uppfærsla (frá útgáfu 0.4.5).** Við ræsingu og síðan á 6 klukkustunda fresti athugar forritið GitHub Releases. Þegar ný útgáfa er komin út opnar það glugga og sýnir appelsínugult **▲ x.y.z** við hlið NTP-vísisins í efri stikunni:

**Frá útgáfu 0.4.12 eru uppfærslur sjálfvirkar og hljóðlausar**: þegar ný útgáfa er á GitHub sækir forritið hana, setur hana upp og endurræsir sig sjálft án þess að spyrja. Skrár sem vantar eru líka sóttar aftur án þess að spyrja. Uppfærsluglugginn birtist aðeins með smelli á útgáfuna efst, eða ef þú skrifar agg_automatici=0 í skrána data/ela.ini.

- **Uppfæra núna** sækir færanlega pakkann fyrir tölvuna þína (Windows 64/32 bita, Linux, Raspberry Pi, Mac), lokar forritinu, skiptir út skránum og endurræsir það. Stillingar, söfnuð spot, dagbækur og `call3.txt` haldast.
- **Seinna** spyr aftur við næstu athugun; **Sleppa þessari útgáfu** hættir að bjóða þá útgáfu.
- Smellur á útgáfuna sem sýnd er í efri stikunni (til dæmis `v0.4.5`) leitar strax að uppfærslum. Slökkva má á sjálfvirku athuguninni með gátreitnum í sama glugga.
- Ef ekki er hægt að skrifa í möppu forritsins (til dæmis kerfismöppu) skaltu frekar nota uppsetningarpakkann af síðunni Releases.

**Skrár vantar.** Við ræsingu athugar forritið hvort allar skrárnar sem því var dreift með séu til staðar (þýðingar, skjöl, töflur, taldar upp í `data/manifest.txt`). Ef einhverja vantar birtir gluggi lista yfir þær og **Sækja af GitHub** sækir þær aftur úr sömu útgáfu á GitHub.

## Kröfur

- Ekkert .NET þarf að setja upp: forritið inniheldur allt sem það þarf.
- Nettenging fyrir kortið (Natural Earth, við fyrstu ræsingu), norðurljósagögn NOAA, ratsjár YA-MS og telnet-loturnar.
- Raspberry Pi Zero og Pi 1 eru ekki studd (.NET styður ekki örgjörva þeirra).
- Engar útgáfur eru til fyrir Linux i686 (32 bita PC-tölvur), því .NET er ekki til fyrir það kerfi, né fyrir FreeBSD, þar sem myndrænu söfn Avalonia vantar.
