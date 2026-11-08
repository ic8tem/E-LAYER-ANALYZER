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

Ultimi 2000 spot con filtro per banda e fonte. Archivio: un file CSV al giorno in `data/spot`. A ogni chiusura del programma il database degli spot si svuota (memoria e file di `data/spot`): ogni sessione riparte da zero.

**Filtro locale** (casella, facoltativo): le righe "DX de" sotto 27990,0 kHz o sopra 145 MHz si scartano, non compaiono nella console e non si memorizzano. I **locator non precisi** (meno di 6 caratteri) si prendono da CALL3: se CALL3 ha un locator a 6 caratteri, prevale sempre quello. Come in YA-MS, sono di meteor scatter (e non di E-sporadico) gli spot nelle finestre di frequenza tipiche del MS (50190-50300, 70100-70400, 144290-144399 kHz) senza scritte di altra propagazione (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), e quelli con modi solo MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) o con `<MS>`. Gli spot con `<AS>`/`<ACS>` (aircraft scatter) non si usano per l'E-sporadico.

## Colonna dei filtri (40% a destra)

La raccolta dei dati è **una sola per tutto il programma**: ogni spot passa per questi filtri e va alle linguette di ciascun modo di propagazione (dentro E-LAYER ANALYZER la Raccolta dati di YA-MS non compare). In cima la casella del **filtro locale**; sotto cinque sottolinguette:

| Sottolinguetta | Cosa si imposta | Di partenza (regole dell'autore) |
|---|---|---|
| Meteor scatter | il filtro di YA-MS: finestre di frequenza MS, scritte di altra propagazione, modi MS e `<MS>`, bande | come in YA-MS |
| E-sporadico | scritte `< >` che escludono lo spot, regola dei 144 MHz, regola transequatoriale | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, salto corto 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | parole del F.A.I., bande, periodo sulla mappa | FAI, tutte le bande, 24 ore |
| Aurora | scritte `< >` dell'aurora, bande, periodo sulla mappa | `AU` `AUR` (con o senza `< >`), tutte le bande, 2 ore |
| Prova dei filtri | si incolla una riga del cluster e si vede come la classificano tutti i filtri | |

Il pulsante **Ripristina le regole dell'autore** riporta i valori di partenza.

## CALL3

Archivio **nominativo → locator** di JTDX (oltre un milione di nominativi) in `data/call3.txt`. Pulsanti **Ricarica** e **Importa un nuovo CALL3.TXT**. I locator a 6 caratteri letti nei commenti degli spot si aggiungono a `data/call3_nuovi.txt`.

## Sottolinguette Spot e Dati di propagazione

La linguetta ha due sottolinguette: **Spot** (sessioni, spot raccolti, CALL3 e colonna dei filtri) e **Dati di propagazione**:

- **N0NBH** (hamqsl.com): banner VHF e dati solari, E-skip Europa e Nord America (DXrobot), aurora.
- **Bz per l'aurora**: NOAA SWPC, vento solare in tempo reale (DSCOVR/ACE, punto L1), la fonte migliore per il Bz.
- **Meteor scatter**: attività meteorica del giorno (RMOB) da Make More Miles on VHF.

Si scaricano al massimo una volta l'ora. La sintesi compare sulle mappe di SPORADIC-E, F.A.I. e AURORA. Il **menu del continente**, a destra nella barra delle linguette, sposta le mappe e sceglie i dati del continente.
