# E-LAYER ANALYZER

**E-LAYER ANALYZER** - analisi dei modi di propagazione VHF che coinvolgono lo strato E: E-sporadico (anche multi-salto
extracontinentale), F.A.I., meteor scatter (programma YA-MS integrato) e aurora radio.
A cura di Costantino Cerrotta IC8TEM. .NET 8 + Avalonia, stessa grafica di YA-MS.
Nessun dato in AppData: tutto sta nella cartella `data` accanto al programma.

## Distribuzioni e aggiornamenti

| Sistema | Pacchetto (pagina Releases) | Aggiornamento automatico |
|---|---|---|
| Windows 64 bit, 32 bit, ARM64 (Snapdragon) | `-win-x64-setup.exe`, `-win-x86-`, `-win-arm64-` oppure gli zip portatili | sì |
| Mac Apple Silicon e Intel | `-osx-arm64.dmg`, `-osx-x64.dmg` oppure `-app.zip` | sì |
| Qualsiasi Linux x86_64 e aarch64 | `.AppImage` oppure `-linux-x64-portatile.tar.gz`, `-linux-arm64-portatile.tar.gz` | sì |
| Raspberry Pi OS 64 e 32 bit, Linux armv7l | `-linux-arm64-`, `-linux-arm-`, `-linux-armv7l-portatile.tar.gz` | sì |
| Alpine Linux (musl) x86_64 e aarch64 | `-alpine-x64-portatile.tar.gz`, `-alpine-arm64-portatile.tar.gz` | sì |
| Debian, Ubuntu, Mint, Raspberry Pi OS | `_amd64.deb`, `_arm64.deb`, `_armhf.deb` | **no**: si installa il `.deb` nuovo |
| Fedora, openSUSE | `.x86_64.rpm`, `.aarch64.rpm`, `.armv7hl.rpm` | **no**: si installa il `.rpm` nuovo |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `-x86_64.pkg.tar.zst`, `-aarch64.pkg.tar.zst` | **no**: si installa il pacchetto nuovo |
| Slackware x86_64 e ARM64 | `-x86_64-1_ic8tem.txz`, `-aarch64-1_ic8tem.txz` | **no**: `upgradepkg` con il pacchetto nuovo |
| FreeBSD amd64 | `-freebsd-amd64-sperimentale.tar.gz` (**sperimentale, non verificato**) | sì, se il programma parte |

Gli aggiornamenti automatici e silenziosi funzionano con tutti i pacchetti **tranne quelli dei gestori di sistema** (`.deb`, `.rpm`,
Arch, Slackware): lì il programma segnala la versione nuova e si aggiorna installando il pacchetto nuovo, come la prima volta.
Non esistono versioni per Linux i686 (PC a 32 bit) e OpenBSD, perché .NET non esiste per quei sistemi.

[![Ko-fi](https://img.shields.io/badge/Ko--fi-ic8tem-FF5E5B?logo=ko-fi&logoColor=white)](https://ko-fi.com/ic8tem)

| Linguetta | Contenuto |
|---|---|
| SPORADIC-E | mappa azimutale (singolo salto o distanza massima) o rettangolare, spot con primi salti di entrambe le stazioni, nubi Es sui quadrati Maidenhead, tabella dei salti di IK3XTV, regola dei 144 MHz |
| BENCHMARK Es | log ADIF intero (anche di altre stazioni): QSO per giornata e ora, skip per settori, foEs, verifica del multi-salto, spostamento delle nubi |
| F.A.I. | Scatter trail degli scatter point per una coppia di stazioni e curve di iso-riflessione (IGRF-14) |
| METEOR SCATTER | YA-MS completo (ultima versione) |
| AURORA | ovale OVATION, Kp, vento solare (NOAA SWPC), Scatter trail e iso-riflessione aurorale, spot <AU>/<AUR> con il calcolo |
| DATA COLLECTION | 3 sessioni telnet (cluster a scelta, RBN, cluster DM4X con PSKReporter), PSKReporter MQTT, WSJT-X UDP, spot raccolti, archivio CALL3 |
| IMPOSTAZIONI | opzioni generali (stazione, lingua, colori, carattere, continente, ora, aggiornamenti) e smistamento degli spot tra le linguette, con i filtri e la prova di una riga |
| LOG ADIF | quadrati lavorati e confermati, banda per banda dai 28 MHz in su |
| FONTI | fonti citate dal programma |
| SINTESI DELLE SCELTE DI CALCOLO / MANUALE D'USO / RELAZIONE TECNICA | formule e scelte di calcolo, manuale d'uso approfondito, relazione tecnica sulle scelte (esportabili in Word) |

Fonti principali: G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, "IL F.A.I., quello strano fenomeno del dopo E-sporadico" (2011);
https://qsl.net/ic8fax/page4.html; tabella dei salti Es di F. Egano IK3XTV.

## Compilazione
Gli installatori li produce GitHub Actions (`.github/workflows/build.yml`) a ogni aggiornamento del ramo `release`
(versione dal file `VERSION`). A ogni push su `main`: compilazione di controllo su Linux, registro sul ramo `ci-log`
e fotografie delle linguette sul ramo `ci-screenshots`.
In locale: `dotnet publish E-LAYER-ANALYZER.csproj -c Release -r win-x64|win-x86|win-arm64|linux-x64|linux-arm64|linux-arm|linux-musl-x64|linux-musl-arm64|osx-arm64|osx-x64 --self-contained true`; pacchetti Linux con `installer/make-deb.sh`, `make-rpm.sh`, `make-arch.sh`, `make-txz.sh`, `make-appimage.sh`.
Documenti Word: `python3 tools/txt2docx.py data/ela_documento_tecnico.txt data/ela_documento_tecnico.docx`.

## Sostieni il progetto
E-LAYER ANALYZER è gratuito. Se ti è utile puoi offrirmi un caffè su **[Ko-fi](https://ko-fi.com/ic8tem)**.
