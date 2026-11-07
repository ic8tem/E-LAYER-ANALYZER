# Installazione

[← Home](Home-it)

Scarica il file adatto dalla pagina **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Programma e cartella `data` stanno sempre insieme in una cartella personale: **nessun file viene scritto in C:\Users\...\AppData**.

| Sistema | File | Come si installa |
|---|---|---|
| Windows 64 bit | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Doppio clic. Installa in `C:\E-LAYER-ANALYZER`, senza permessi di amministratore. Nessun collegamento nel menu Start (che starebbe in AppData), solo l'icona facoltativa sul desktop. |
| Windows 32 bit | `...-windows-x86-setup.exe` | Come sopra, per Windows a 32 bit. |
| Windows portatile | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Estrai dove vuoi e avvia `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Estrai e lancia `./installa.sh`: installa in `~/E-LAYER-ANALYZER` con l'icona nel menu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 o 5 con **Raspberry Pi OS a 64 bit** e desktop (consigliati 4 GB di RAM). Estrai e lancia `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) o `...-mac-intel.dmg` | Apri il .dmg, clic destro su **Installa E-LAYER-ANALYZER.command** > Apri. Installa in `~/E-LAYER-ANALYZER`. |

## Aggiornare

Installa la nuova versione sopra la precedente: impostazioni, spot raccolti, log e `call3.txt` restano.

## Requisiti

- Nessun .NET da installare: il programma contiene tutto.
- Rete internet per la mappa (Natural Earth, al primo avvio), i dati NOAA dell'aurora, i radar di YA-MS e le sessioni telnet.
- Raspberry Pi Zero e Pi 1 non sono supportati (processori non gestiti da .NET).
