# Instalace

[← Domů](Home-cs)

Stáhněte si správný soubor ze stránky **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Program a složka `data` zůstávají vždy spolu ve vaší vlastní složce: **do C:\Users\...\AppData se nikdy nezapisuje žádný soubor**.

| Systém | Soubor | Jak nainstalovat |
|---|---|---|
| Windows 64 bitů | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Dvojklik. Instaluje se do `C:\E-LAYER-ANALYZER`, bez práv správce. Žádný zástupce v nabídce Start (byl by v AppData), jen volitelná ikona na ploše. |
| Windows 32 bitů | `...-win-x86-setup.exe` | Jako výše, pro 32bitové Windows. |
| Windows na ARM (PC se Snapdragonem) | `...-win-arm64-setup.exe` | Jako výše, pro Windows na procesorech ARM. |
| Windows přenosná verze | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Rozbalte kamkoli a spusťte `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, pak spusťte z nabídky: při prvním spuštění se program zkopíruje do `~/E-LAYER-ANALYZER` se složkou `data` vedle sebe. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (také `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (nebo `zypper install`), pak jako u .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (nebo `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, pak jako u .deb. |
| Slackware (x86_64 a ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (nebo `-aarch64-`) | Jako root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, pak spusťte `e-layer-analyzer` z nabídky nebo z terminálu: při prvním spuštění se program zkopíruje do `~/E-LAYER-ANALYZER`. |
| Jakýkoli Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (nebo `-aarch64.AppImage`) | Nastavte jako spustitelný a spusťte: při prvním spuštění zkopíruje program do `~/E-LAYER-ANALYZER` a spustí jej odtamtud. |
| Linux přenosná verze | `...-linux-x64-portatile.tar.gz` | Rozbalte a spusťte `./E-LAYER-ANALYZER`, nebo spusťte `./installa.sh`: nainstaluje se do `~/E-LAYER-ANALYZER` s ikonou v nabídce. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, pak jako přenosná verze pro Linux. |
| Raspberry Pi (64 bitů) | `e-layer-analyzer_x.y.z_arm64.deb` nebo `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 nebo 5 s **64bitovým Raspberry Pi OS** a desktopem (doporučeno 4 GB RAM). Balíček .deb jako výše; přenosnou verzi jako pro Linux. |
| Raspberry Pi (32 bitů) | `e-layer-analyzer_x.y.z_armhf.deb` nebo `...-linux-arm-portatile.tar.gz` | Pi 2, 3 nebo 4 s **32bitovým Raspberry Pi OS**. Balíček .deb jako výše; přenosnou verzi rozbalte a spusťte `./E-LAYER-ANALYZER` nebo `./installa.sh`. |
| Raspberry Pi se Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` nebo `...-linux-arm64-portatile.tar.gz` | Jako Slackware výše. Přenosnou verzi jako pro Linux. |
| Linux armv7l (jiné 32bitové desky ARM) | `...-linux-armv7l-portatile.tar.gz` | Jako přenosná verze pro Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Experimentální, neověřeno.** Vyžaduje komunitní .NET 8 (`pkg install dotnet`) a nativní knihovny SkiaSharp a HarfBuzzSharp zkompilované pro FreeBSD, které Avalonia nedodává: bez nich se program nespustí. Viz `LEGGIMI-FREEBSD.txt` v balíčku. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) nebo `...-osx-x64.dmg` (Intel) | Otevřete .dmg, pravým tlačítkem na **Installa E-LAYER-ANALYZER.command** > Otevřít. Instaluje se do `~/E-LAYER-ANALYZER`. |
| Mac, jen aplikace | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Rozbalte **E-LAYER-ANALYZER.app** do vlastní složky (například `~/E-LAYER-ANALYZER`, ne do Aplikací): složka `data` se vytvoří vedle aplikace. Při prvním spuštění pravé tlačítko > Otevřít. |

## Aktualizace

Nainstalujte novou verzi přes starou: nastavení, shromážděné spoty, deníky a `call3.txt` zůstanou zachovány.

**Systémové balíčky (.deb, .rpm, Arch, Slackware) se automaticky neaktualizují.** S nimi program novou verzi jen oznámí: stáhněte nový balíček ze stránky Releases a nainstalujte jej stejně jako poprvé (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Automatické aktualizace fungují s instalátorem a zipy pro Windows, s aplikací pro Mac, s přenosnými balíčky tar.gz a s AppImage.

**Automatická aktualizace (od verze 0.4.5).** Při spuštění a pak každých 30 minut program kontroluje Releases na GitHubu. Když vyjde nová verze, otevře okno a vedle ukazatele NTP v horní liště zobrazí oranžové **▲ x.y.z**:

**Od verze 0.4.12 jsou aktualizace automatické a tiché**: když je na GitHubu nová verze, program ji stáhne, nainstaluje a sám se restartuje, aniž by se na cokoli ptal. Chybějící soubory se také znovu stáhnou bez ptaní. Okno aktualizace se objeví jen po kliknutí na verzi nahoře nebo pokud vypnete automatické aktualizace v NASTAVENÍ (Obecné).

- **Aktualizovat nyní** stáhne přenosný balíček pro váš počítač (Windows 64/32 bitů, Linux, Raspberry Pi, Mac), zavře program, nahradí soubory a znovu jej spustí. Nastavení, shromážděné spoty, deníky a `call3.txt` zůstanou zachovány.
- **Později** se zeptá znovu při příští kontrole; **Přeskočit tuto verzi** přestane tuto verzi nabízet.
- Kliknutím na verzi zobrazenou v horní liště (například `v0.4.5`) se aktualizace zkontrolují okamžitě. Automatickou kontrolu lze vypnout zaškrtávacím políčkem ve stejném okně.
- Pokud do složky programu nelze zapisovat (například systémová složka), použijte místo toho instalační balíček ze stránky vydání.

**Chybějící soubory.** Při spuštění program kontroluje, zda jsou přítomny všechny soubory, se kterými byl distribuován (překlady, dokumenty, tabulky, uvedené v `data/manifest.txt`). Pokud některý chybí, okno je vypíše a **Stáhnout z GitHubu** je znovu stáhne ze stejné verze na GitHubu.

## Požadavky

- Není třeba instalovat .NET: program obsahuje vše, co potřebuje.
- Připojení k internetu pro mapu (Natural Earth, při prvním spuštění), data aurory NOAA, radary YA-MS a relace telnet.
- Raspberry Pi Zero a Pi 1 nejsou podporovány (jejich procesory .NET nepodporuje).
- Verze pro Linux i686 (32bitová PC) a OpenBSD neexistují, protože pro tyto systémy není .NET. Pro FreeBSD je jen experimentální balíček.
