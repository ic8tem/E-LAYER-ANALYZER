# DATA COLLECTION

[← Home](Home-it)

## Sessioni telnet

| Sessione | Partenza | Note |
|---|---|---|
| 1. DX cluster a scelta | on0dxk.dyndns.org 8000 | Su ON0DXK le VHF sono filtrate: `set/filter dxbm/pass 6` (poi 4, 2). Altro esempio: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW e RTTY, 7001 = FT8. |
| 3. Cluster con PSKReporter | **dm4x.ddns.net 8500** | Cluster di DM4X; in alternativa HamAlert (hamalert.org 7300, serve un account). |

- Login automatico con il nominativo (in alto o nel campo Login); **comandi iniziali** separati da `;`.
- **Riconnetti** dopo 30 s se la linea cade; **Connetti all'avvio**.
- Di partenza si salvano solo gli spot da **28 MHz in su**.

## Spot raccolti

Ultimi 2000 spot con filtro per banda e fonte. Archivio: un file CSV al giorno in `data/spot`.

## CALL3

Archivio **nominativo → locator** di JTDX (oltre un milione di nominativi) in `data/call3.txt`. Pulsanti **Ricarica** e **Importa un nuovo CALL3.TXT**. I locator a 6 caratteri letti nei commenti degli spot si aggiungono a `data/call3_nuovi.txt`.
