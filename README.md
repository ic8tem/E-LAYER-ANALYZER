# E-LAYER ANALYZER

**E-LAYER ANALYZER** - analisi dei modi di propagazione VHF che coinvolgono lo strato E: sporadico E (anche multi-salto
extracontinentale), F.A.I., meteor scatter (programma YA-MS integrato) e aurora radio.
A cura di Costantino Cerrotta IC8TEM. Windows, Linux e Mac (.NET 8 + Avalonia), stessa grafica di YA-MS.

| Linguetta | Contenuto |
|---|---|
| SPORADIC-E | mappa (65 %) degli spot, primi salti di entrambe le stazioni, nubi Es sui quadrati Maidenhead, tabella dei salti di IK3XTV |
| F.A.I. | scia degli scatter point per una coppia di stazioni e curve di iso-riflessione (IGRF-14) |
| METEOR SCATTER | YA-MS completo |
| AURORA | ovale OVATION, Kp, vento solare (NOAA SWPC), scia e iso-riflessione aurorale |
| DATA COLLECTION | 3 sessioni telnet (cluster a scelta, RBN, cluster con PSKReporter), spot raccolti, archivio CALL3 |
| FONTI / DOCUMENTO TECNICO | fonti e formule del programma |

Fonti principali: G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, "IL F.A.I., quello strano fenomeno del dopo E-sporadico" (2011);
https://qsl.net/ic8fax/page4.html; tabella dei salti Es di F. Egano IK3XTV.

## Compilazione
Gli installatori li produce GitHub Actions (`.github/workflows/build.yml`) a ogni aggiornamento del ramo `release`
(versione dal file `VERSION`). A ogni push su `main`: compilazione di controllo su Linux, registro sul ramo `ci-log`
e fotografie delle linguette sul ramo `ci-screenshots`.
In locale: `dotnet publish E-LAYER-ANALYZER.csproj -c Release -r win-x64|linux-x64|osx-arm64 --self-contained true`.
Documenti Word: `python3 tools/txt2docx.py data/ela_documento_tecnico.txt data/ela_documento_tecnico.docx`.
