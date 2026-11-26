# E-LAYER ANALYZER

**E-LAYER ANALYZER** - analisi dei modi di propagazione VHF che coinvolgono lo strato E: E-sporadico (anche multi-salto
extracontinentale), F.A.I., meteor scatter (programma YA-MS integrato) e aurora radio.
A cura di Costantino Cerrotta IC8TEM. .NET 8 + Avalonia, stessa grafica di YA-MS.
Nessun dato in AppData: tutto sta nella cartella `data` accanto al programma.

## Distribuzioni e aggiornamenti

| Sistema | Pacchetto (pagina Releases) | Aggiornamento automatico |
|---|---|---|
| Windows 10 e 11: 64 bit, 32 bit, ARM64 (Snapdragon) | `-win-x64-setup.exe`, `-win-x86-`, `-win-arm64-` oppure gli zip portatili | sì |
| Windows 7 SP1, 8 e 8.1 (64 e 32 bit, .NET 6) | `-win7-x64-setup.exe`, `-win7-x86-` oppure gli zip portatili `-win7-...` | sì |
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
| IONOSONDE | foEs, foF2 e MUF(D) in diretta dalle ionosonde GIRO (ogni 15 minuti), grafico di 24 ore, MUF dell'E-sporadico a 1000-2400 km con le bande che si aprono, confronto con la foEs ricavata dagli spot |
| NUBI Es NEL TEMPO | nubi Es passo per passo (5-15 minuti) su celle di 1-4°, foEs minima e NmE con il metodo di Chartier 2022, da spot della sessione, log ADIF o spot CSV |
| Catene multi-salto 50 MHz | dentro SPORADIC-E, Precursori multi-salto: primo e ultimo salto corto con MUF intorno ai 50 MHz e salti centrali (condizioni di IC8TEM), catene dal QTH, corridoi in tutto il mondo e «cerca la nube» (solo dove ci sono stazioni), proiezione del proprio segnale (punti rosa), avviso in alto, verifica nel BENCHMARK 50 MHz con gli spot CSV; doppio salto uguale fino a circa 4700 km (salti «2 =», ITU-R P.534-6) |
| F.A.I. | Scatter trail degli scatter point per una coppia di stazioni e curve di iso-riflessione (IGRF-14) |
| METEOR SCATTER | YA-MS completo (ultima versione) |
| AURORA | ovale OVATION, Kp, vento solare (NOAA SWPC), Scatter trail e iso-riflessione aurorale, spot <AU>/<AUR> con il calcolo |
| DATA COLLECTION | 3 sessioni telnet (cluster a scelta, RBN, cluster DM4X con PSKReporter), PSKReporter MQTT, WSJT-X UDP, spot raccolti, archivio CALL3 |
| IMPOSTAZIONI | opzioni generali (stazione, lingua, colori, carattere, continente, ora, aggiornamenti) e smistamento degli spot tra le linguette, con i filtri e la prova di una riga |
| LOG ADIF | quadrati lavorati e confermati, banda per banda dai 28 MHz in su |
| FONTI | fonti citate dal programma |
| SINTESI DELLE SCELTE DI CALCOLO / MANUALE D'USO / RELAZIONE TECNICA | formule e scelte di calcolo, manuale d'uso approfondito, relazione tecnica sulle scelte (esportabili in Word) |
| ABACO DEI SALTI | quanti salti servono in 50 MHz da 500 a 12000 km (passo 100 km), con la foEs di ogni salto e le fonti (esportabile in Word) |

Fonti principali: G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, "IL F.A.I., quello strano fenomeno del dopo E-sporadico" (2011);
https://qsl.net/ic8fax/page4.html; tabella dei salti Es di F. Egano IK3XTV.

## Collegamenti in rete e sicurezza

- **Aggiornamenti:** il programma legge le Release di GitHub all'avvio e ogni 30 minuti. Dalla 0.6.7 installa solo pacchetti firmati dall'autore (`SHA256SUMS.txt.sig`, firma ECDSA) e con l'impronta giusta.
- **Dati in sola lettura (HTTPS):** NOAA, ionosonde GIRO, N0NBH, BOM, modello IGRF, mappe, radar meteorici, CALL3; per i locator mancanti si mandano a G7VRD i nominativi da cercare.
- **Cluster telnet e APRS-IS:** solo il tuo nominativo e i comandi del filtro, in chiaro come prevedono questi servizi.
- **PSKReporter:** lettura degli spot; in più il programma rimanda le stazioni MS che decodifichi, col tuo nominativo, se il tuo programma digitale non lo fa (attivo di partenza, casella in IMPOSTAZIONI).
- **Scambio YA-MS:** i messaggi MS rivolti ad altri utenti viaggiano cifrati su tre server MQTT pubblici (attivo di partenza, casella in IMPOSTAZIONI). Chiunque potrebbe pubblicare chiavi o messaggi falsi: sono comunque messaggi radio.
- **LoTW:** nome utente e password solo in HTTPS; la password non si salva.
- **WSJT-X, JTDX, MSHV:** ricezione UDP solo dal tuo computer (127.0.0.1) di partenza. Il programma non apre porte verso internet e non manda dati all'autore.

## Compilazione
Gli installatori li produce GitHub Actions (`.github/workflows/build.yml`) a ogni aggiornamento del ramo `release`
(versione dal file `VERSION`). A ogni push su `main`: compilazione di controllo su Linux, registro sul ramo `ci-log`
e fotografie delle linguette sul ramo `ci-screenshots`.
In locale: `dotnet publish E-LAYER-ANALYZER.csproj -c Release -r win-x64|win-x86|win-arm64|linux-x64|linux-arm64|linux-arm|linux-musl-x64|linux-musl-arm64|osx-arm64|osx-x64 --self-contained true`; pacchetti Linux con `installer/make-deb.sh`, `make-rpm.sh`, `make-arch.sh`, `make-txz.sh`, `make-appimage.sh`.
Documenti Word: `python3 tools/txt2docx.py data/ela_documento_tecnico.txt data/ela_documento_tecnico.docx`.

## Ringraziamenti
Per lo sviluppo dell'E-sporadico in 50 MHz: Johan IW2DNI e Fabio IZ7NLJ.

## Licenza
E-LAYER ANALYZER è software libero: puoi ridistribuirlo e modificarlo secondo i termini della GNU General Public License versione 3 ([COPYING](COPYING)), la stessa di YA-MS che il programma contiene. È distribuito senza alcuna garanzia. I dati delle fonti esterne restano dei rispettivi enti, con le loro condizioni (per esempio i dati delle ionosonde GIRO, CC BY-NC-SA 4.0, solo uso non commerciale); l'elenco è nella linguetta FONTI.

### Verifica delle licenze (23/11/2026)
- **Codice**: GNU GPL-3.0; `COPYING` è il testo ufficiale della Free Software Foundation, integro, e GitHub riconosce la licenza.
- **Librerie incluse**: Avalonia 11.2.3 (MIT), carattere Inter (SIL Open Font License 1.1), runtime .NET (MIT): tutte compatibili con la GPL-3.0.
- **Dati inclusi nel pacchetto**: coefficienti IGRF-14 (IAGA, distribuiti da NOAA NCEI come materiale governativo non soggetto a copyright); `call3.txt` di JTDX (archivio nominativo → locator, fonte [21]).
- **Dati scaricati mentre il programma è in uso** (non inclusi nel pacchetto): Natural Earth (pubblico dominio); NOAA SWPC (dati del governo degli Stati Uniti, pubblico dominio); ionosonde GIRO (CC BY-NC-SA 4.0, solo uso non commerciale); APRS-IS in sola lettura, senza password e senza inviare pacchetti; spot di DX cluster, RBN e PSKReporter, pubblici. L'archivio degli spot resta sul computer dell'utente.

## Sostieni il progetto
E-LAYER ANALYZER è gratuito. Se ti è utile puoi offrirmi un caffè su **[Ko-fi](https://ko-fi.com/ic8tem)**.
