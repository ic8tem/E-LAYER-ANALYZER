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
| Raspberry Pi (32 bita) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 eða 4 með **32 bita Raspberry Pi OS**. Afþjappaðu og keyrðu `./E-LAYER-ANALYZER` eða `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) eða `...-osx-x64.dmg` (Intel) | Opnaðu .dmg-skrána, hægrismelltu á **Installa E-LAYER-ANALYZER.command** > Opna. Sett upp í `~/E-LAYER-ANALYZER`. |
| Mac, aðeins forritið | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Afþjappaðu **E-LAYER-ANALYZER.app** í eigin möppu (til dæmis `~/E-LAYER-ANALYZER`, ekki Applications): mappan `data` er búin til við hliðina á forritinu. Við fyrstu ræsingu hægrismelltu > Opna. |

## Uppfærsla

Settu nýju útgáfuna upp yfir þá gömlu: stillingar, söfnuð spot, dagbækur og `call3.txt` haldast.

## Kröfur

- Ekkert .NET þarf að setja upp: forritið inniheldur allt sem það þarf.
- Nettenging fyrir kortið (Natural Earth, við fyrstu ræsingu), norðurljósagögn NOAA, ratsjár YA-MS og telnet-loturnar.
- Raspberry Pi Zero og Pi 1 eru ekki studd (.NET styður ekki örgjörva þeirra).
