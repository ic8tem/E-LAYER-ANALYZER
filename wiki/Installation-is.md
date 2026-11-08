# Uppsetning

[← Home](Home-is)

Sæktu réttu skrána af síðunni **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Forritið og mappan `data` eru alltaf saman í eigin möppu: **engin skrá er nokkurn tíma skrifuð í C:\Users\...\AppData**.

| Kerfi | Skrá | Hvernig á að setja upp |
|---|---|---|
| Windows 64 bita | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Tvísmelltu. Sett upp í `C:\E-LAYER-ANALYZER`, engin stjórnandaréttindi nauðsynleg. Engin flýtileið í Start-valmyndinni (hún yrði í AppData), aðeins valfrjálst tákn á skjáborðinu. |
| Windows 32 bita | `...-windows-x86-setup.exe` | Eins og hér að ofan, fyrir 32 bita Windows. |
| Windows færanleg útgáfa | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Afþjappaðu hvar sem er og keyrðu `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Afþjappaðu og keyrðu `./installa.sh`: sett upp í `~/E-LAYER-ANALYZER` með tákni í valmyndinni. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 eða 5 með **64 bita Raspberry Pi OS** og skjáborði (mælt með 4 GB vinnsluminni). Afþjappaðu og keyrðu `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) eða `...-mac-intel.dmg` | Opnaðu .dmg-skrána, hægrismelltu á **Installa E-LAYER-ANALYZER.command** > Opna. Sett upp í `~/E-LAYER-ANALYZER`. |

## Uppfærsla

Settu nýju útgáfuna upp yfir þá gömlu: stillingar, söfnuð spot, dagbækur og `call3.txt` haldast.

## Kröfur

- Ekkert .NET þarf að setja upp: forritið inniheldur allt sem það þarf.
- Nettenging fyrir kortið (Natural Earth, við fyrstu ræsingu), norðurljósagögn NOAA, ratsjár YA-MS og telnet-loturnar.
- Raspberry Pi Zero og Pi 1 eru ekki studd (.NET styður ekki örgjörva þeirra).
