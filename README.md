# E-LAYER ANALYZER

**E-LAYER ANALYZER** - analisi dei modi di propagazione VHF che coinvolgono lo strato E: E-sporadico (anche multi-salto
extracontinentale), F.A.I., meteor scatter (programma YA-MS integrato) e aurora radio.
A cura di Costantino Cerrotta IC8TEM. Windows (64 e 32 bit), Linux, Raspberry Pi (ARM64) e Mac (.NET 8 + Avalonia), stessa grafica di YA-MS.
Nessun dato in AppData: tutto sta nella cartella `data` accanto al programma.

| Linguetta | Contenuto |
|---|---|
| SPORADIC-E | mappa azimutale (singolo salto o distanza massima) o rettangolare, spot con primi salti di entrambe le stazioni, nubi Es sui quadrati Maidenhead, tabella dei salti di IK3XTV, regola dei 144 MHz |
| BENCHMARK Es | log ADIF intero (anche di altre stazioni): QSO per giornata e ora, skip per settori, foEs, verifica del multi-salto, spostamento delle nubi |
| F.A.I. | Scatter trail degli scatter point per una coppia di stazioni e curve di iso-riflessione (IGRF-14) |
| METEOR SCATTER | YA-MS completo (ultima versione) |
| AURORA | ovale OVATION, Kp, vento solare (NOAA SWPC), Scatter trail e iso-riflessione aurorale, spot <AU>/<AUR> con il calcolo |
| DATA COLLECTION | 3 sessioni telnet (cluster a scelta, RBN, cluster DM4X con PSKReporter), filtro locale sotto 27990 kHz, spot raccolti, archivio CALL3 |
| LOG ADIF | quadrati lavorati e confermati, banda per banda dai 28 MHz in su |
| FONTI / DOCUMENTO TECNICO | fonti e formule del programma |

Fonti principali: G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, "IL F.A.I., quello strano fenomeno del dopo E-sporadico" (2011);
https://qsl.net/ic8fax/page4.html; tabella dei salti Es di F. Egano IK3XTV.

## Compilazione
Gli installatori li produce GitHub Actions (`.github/workflows/build.yml`) a ogni aggiornamento del ramo `release`
(versione dal file `VERSION`). A ogni push su `main`: compilazione di controllo su Linux, registro sul ramo `ci-log`
e fotografie delle linguette sul ramo `ci-screenshots`.
In locale: `dotnet publish E-LAYER-ANALYZER.csproj -c Release -r win-x64|linux-x64|osx-arm64 --self-contained true`.
Documenti Word: `python3 tools/txt2docx.py data/ela_documento_tecnico.txt data/ela_documento_tecnico.docx`.
