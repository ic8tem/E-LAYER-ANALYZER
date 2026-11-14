# Installazione

[← Home](Home-it)

Scarica il file adatto dalla pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programma e cartella `data` stanno sempre insieme in una cartella personale: **nessun file viene scritto in C:\Users\...\AppData**.

| Sistema | File | Come si installa |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Doppio clic. Installa in `C:\E-LAYER-ANALYZER`, senza permessi di amministratore. Nessun collegamento nel menu Start (che starebbe in AppData), solo l'icona facoltativa sul desktop. |
| Windows 32 bit | `...-win-x86-setup.exe` | Come sopra, per Windows a 32 bit. |
| Windows su ARM (PC con Snapdragon) | `...-win-arm64-setup.exe` | Come sopra, per Windows su processori ARM. |
| Windows portatile | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Estrai dove vuoi e avvia `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, poi avvia dal menu: al primo avvio il programma si copia in `~/E-LAYER-ANALYZER`, con la cartella `data` accanto. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (anche `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (o `zypper install`), poi come il .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (o `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, poi come il .deb. |
| Slackware (x86_64 e ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (o `-aarch64-`) | Da root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, poi avvia `e-layer-analyzer` dal menu o dal terminale: al primo avvio il programma si copia in `~/E-LAYER-ANALYZER`. |
| Qualsiasi Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (o `-aarch64.AppImage`) | Rendilo eseguibile e avvialo: al primo avvio copia il programma in `~/E-LAYER-ANALYZER` e lo avvia da lì. |
| Linux portatile | `...-linux-x64-portatile.tar.gz` | Estrai e avvia `./E-LAYER-ANALYZER`, oppure lancia `./installa.sh`: installa in `~/E-LAYER-ANALYZER` con l'icona nel menu. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, poi come il portatile per Linux. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` o `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 o 5 con **Raspberry Pi OS a 64 bit** e desktop (consigliati 4 GB di RAM). Il .deb come sopra; il portatile come per Linux. |
| Raspberry Pi (32 bit) | `e-layer-analyzer_x.y.z_armhf.deb` o `...-linux-arm-portatile.tar.gz` | Pi 2, 3 o 4 con **Raspberry Pi OS a 32 bit**. Il .deb come sopra; il portatile: estrai e avvia `./E-LAYER-ANALYZER` o `./installa.sh`. |
| Raspberry Pi con Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` o `...-linux-arm64-portatile.tar.gz` | Come Slackware sopra. Il portatile come per Linux. |
| Linux armv7l (altre schede ARM a 32 bit) | `...-linux-armv7l-portatile.tar.gz` | Come il portatile per Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Sperimentale, non verificato.** Servono il .NET 8 della comunità (`pkg install dotnet`) e le librerie native SkiaSharp e HarfBuzzSharp compilate per FreeBSD, che Avalonia non fornisce: senza di loro il programma non parte. Vedi `LEGGIMI-FREEBSD.txt` nel pacchetto. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) o `...-osx-x64.dmg` (Intel) | Apri il .dmg, clic destro su **Installa E-LAYER-ANALYZER.command** > Apri. Installa in `~/E-LAYER-ANALYZER`. |
| Mac, sola applicazione | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Estrai **E-LAYER-ANALYZER.app** in una tua cartella (per esempio `~/E-LAYER-ANALYZER`, non in Applicazioni): la cartella `data` si crea accanto all'app. Al primo avvio clic destro > Apri. |

## Aggiornare

Installa la nuova versione sopra la precedente: impostazioni, spot raccolti, log e `call3.txt` restano.

**I pacchetti di sistema (.deb, .rpm, Arch, Slackware) non si aggiornano da soli.** Con questi il programma segnala soltanto la versione nuova: scarica il pacchetto nuovo dalla pagina delle Release e installalo come la prima volta (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). L'aggiornamento automatico funziona con l'installatore e gli zip di Windows, l'app del Mac, i portatili tar.gz e l'AppImage.

**Aggiornamento automatico (dalla versione 0.4.5).** All'avvio e poi ogni 6 ore il programma controlla le Release su GitHub. Quando esce una versione nuova apre una finestra e mostra **▲ x.y.z** in arancione accanto alla scritta NTP nella barra in alto:

- **Aggiorna ora** scarica il pacchetto portatile adatto al computer (Windows 64/32 bit, Linux, Raspberry Pi, Mac), chiude il programma, sostituisce i file e lo riavvia. Impostazioni, spot raccolti, log e `call3.txt` restano.
- **Più tardi** lo ripropone al controllo successivo; **Salta questa versione** non propone più quella versione.
- Un clic sulla versione mostrata nella barra in alto (per esempio `v0.4.5`) controlla subito gli aggiornamenti. Il controllo automatico si spegne con la casella nella stessa finestra.
- Se la cartella del programma non è scrivibile (per esempio una cartella di sistema), si aggiorna con il pacchetto di installazione dalla pagina della release.

**File mancanti.** All'avvio il programma controlla di avere tutti i file con cui è stato distribuito (traduzioni, documenti, tabelle, elencati in `data/manifest.txt`). Se ne manca qualcuno, una finestra li elenca e **Scarica da GitHub** li riprende dalla stessa versione pubblicata su GitHub.

## Requisiti

- Nessun .NET da installare: il programma contiene tutto.
- Rete internet per la mappa (Natural Earth, al primo avvio), i dati NOAA dell'aurora, i radar di YA-MS e le sessioni telnet.
- Raspberry Pi Zero e Pi 1 non sono supportati (processori non gestiti da .NET).
- Non ci sono versioni per Linux i686 (PC a 32 bit) e OpenBSD, perché .NET non esiste per quei sistemi. Per FreeBSD c'è solo il pacchetto sperimentale.
