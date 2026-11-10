# Installazione

[← Home](Home-it)

Scarica il file adatto dalla pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programma e cartella `data` stanno sempre insieme in una cartella personale: **nessun file viene scritto in C:\Users\...\AppData**.

| Sistema | File | Come si installa |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Doppio clic. Installa in `C:\E-LAYER-ANALYZER`, senza permessi di amministratore. Nessun collegamento nel menu Start (che starebbe in AppData), solo l'icona facoltativa sul desktop. |
| Windows 32 bit | `...-win-x86-setup.exe` | Come sopra, per Windows a 32 bit. |
| Windows portatile | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Estrai dove vuoi e avvia `E-LAYER-ANALYZER.exe`. |
| Linux 64 bit (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, poi avvia dal menu: al primo avvio il programma si copia in `~/E-LAYER-ANALYZER`, con la cartella `data` accanto. |
| Linux portatile | `...-linux-x64-portatile.tar.gz` | Estrai e avvia `./E-LAYER-ANALYZER`, oppure lancia `./installa.sh`: installa in `~/E-LAYER-ANALYZER` con l'icona nel menu. |
| Raspberry Pi (64 bit) | `e-layer-analyzer_x.y.z_arm64.deb` o `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 o 5 con **Raspberry Pi OS a 64 bit** e desktop (consigliati 4 GB di RAM). Il .deb come sopra; il portatile come Linux. |
| Raspberry Pi (32 bit) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 o 4 con **Raspberry Pi OS a 32 bit**. Estrai e avvia `./E-LAYER-ANALYZER` o `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) o `...-osx-x64.dmg` (Intel) | Apri il .dmg, clic destro su **Installa E-LAYER-ANALYZER.command** > Apri. Installa in `~/E-LAYER-ANALYZER`. |
| Mac, sola applicazione | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Estrai **E-LAYER-ANALYZER.app** in una tua cartella (per esempio `~/E-LAYER-ANALYZER`, non in Applicazioni): la cartella `data` si crea accanto all'app. Al primo avvio clic destro > Apri. |

## Aggiornare

Installa la nuova versione sopra la precedente: impostazioni, spot raccolti, log e `call3.txt` restano.

## Requisiti

- Nessun .NET da installare: il programma contiene tutto.
- Rete internet per la mappa (Natural Earth, al primo avvio), i dati NOAA dell'aurora, i radar di YA-MS e le sessioni telnet.
- Raspberry Pi Zero e Pi 1 non sono supportati (processori non gestiti da .NET).
