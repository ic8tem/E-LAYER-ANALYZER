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

**Filtro locale** (casella facoltativa, in IMPOSTAZIONI): le righe "DX de" sotto 27990,0 kHz o sopra 145 MHz si scartano, non compaiono nella console e non si memorizzano; tra 430 e 440 MHz passano solo gli spot di aurora. I **locator non precisi** (meno di 6 caratteri) si prendono da CALL3: se CALL3 ha un locator a 6 caratteri, prevale sempre quello. Come in YA-MS, sono di meteor scatter (e non di E-sporadico) gli spot nelle finestre di frequenza tipiche del MS (50190-50300, 70100-70400, 144290-144399 kHz) senza scritte di altra propagazione (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), e quelli con modi solo MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) o con `<MS>`. Fanno eccezione FT8 e FT4 in 50 e 70 MHz, dove decide il modo quando è noto (l'FT8 di 70,154 MHz va all'E-sporadico anche dentro la finestra MS). Gli spot con `<AS>`/`<ACS>` (aircraft scatter) non si usano per l'E-sporadico.

## Filtri degli spot

La raccolta dei dati è **una sola per tutto il programma**: ogni spot passa per i filtri e va alle linguette di ciascun modo di propagazione. Dalla versione 0.4.18 i filtri, il filtro locale e la prova dei filtri stanno nella linguetta **[IMPOSTAZIONI](Impostazioni-it)**, sottolinguetta **Smistamento degli spot**.

## CALL3

Archivio **nominativo → locator** di JTDX (oltre un milione di nominativi) in `data/call3.txt`. Pulsanti **Ricarica** e **Importa un nuovo CALL3.TXT**. I locator a 6 caratteri letti nei commenti degli spot si aggiungono a `data/call3_nuovi.txt`.

Dalla versione 0.4.16 l'archivio CALL3 è uno solo per tutto il programma, quello della Raccolta dati di YA-MS (linguetta METEOR SCATTER), con le stesse regole di LIVETROPOVHF: aggiornamento da MMMonVHF ogni 12 ore, voci a mano, file aggiunti, correzioni dal cluster, locatori imparati, G7VRD ogni 30 minuti e, come ultima fonte, lo strato JTDX (data/grid_data.bin), mai scritto in call3.txt; le correzioni si scrivono dentro data/call3.txt e i locatori di data/call3_nuovi.txt si importano una volta. Gli spot di WSJT-X, JTDX e MSHV arrivano anche via UDP (fonte WSJT-X UDP, porta 2237 con inoltro alla 2238).

Questi filtri, che vengono da YA-MS (CW, distanza minima, finestre di frequenza, modi MS, bande), valgono solo per la linguetta METEOR SCATTER: decidono quali spot entrano nella sua raccolta. Le altre linguette ricevono tutti gli spot e usano i propri filtri.

## Sottolinguette Spot e Dati di propagazione

La linguetta ha due sottolinguette: **Spot** (sessioni, PSKReporter MQTT, WSJT-X UDP, spot raccolti e CALL3) e **Dati di propagazione**:

- **N0NBH** (hamqsl.com): banner VHF e dati solari, E-skip Europa e Nord America (DXrobot), aurora.
- **Bz per l'aurora**: NOAA SWPC, vento solare in tempo reale (DSCOVR/ACE, punto L1), la fonte migliore per il Bz.
- **Meteor scatter**: attività meteorica del giorno (RMOB) da Make More Miles on VHF.

Si scaricano al massimo una volta l'ora. La sintesi compare sulle mappe di SPORADIC-E, F.A.I. e AURORA. Il **menu del continente**, a destra nella barra delle linguette, sposta le mappe e sceglie i dati del continente.
