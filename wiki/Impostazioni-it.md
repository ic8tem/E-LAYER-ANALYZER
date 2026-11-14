# IMPOSTAZIONI

[← Home](Home-it)

Dalla versione 0.4.18 la linguetta **IMPOSTAZIONI** raccoglie le opzioni generali del programma e lo smistamento degli spot tra le linguette. Ha due sottolinguette.

## Generali

| Sezione | Che cosa si imposta |
|---|---|
| Stazione | Nominativo, locator e quota, gli stessi della riga in alto; **Applica** li conferma per tutte le linguette. |
| Aspetto | Lingua, colorazione (gli schemi di YAEC e YA-MS e, dalla versione 0.5.0, i sei temi di IP-TRX-CTRL: Chiaro, Notte, Nero, Blu, Viola, Verde) e **dimensione del carattere di tutto il programma** (85-140 %, come in YA-MS). Lingua e colorazione ricostruiscono la finestra: impostazioni e spot restano. |
| Mappe e dati previsionali | Il continente, lo stesso menu della barra delle linguette. |
| Ora | Stato del controllo NTP e scarto tra l'ora dei server e l'orologio del PC. |
| Aggiornamenti | Controllo su GitHub all'avvio e ogni 6 ore, aggiornamento automatico e silenzioso, pulsante **Controlla adesso**. L'aggiornamento automatico non vale per i pacchetti di sistema (.deb, .rpm, Arch, Slackware). |
| Dati | Pulsante che apre la cartella `data`. La chiave API del Bureau of Meteorology resta in DATA COLLECTION › Dati di propagazione. |

## Smistamento degli spot

A sinistra una tabella elenca le regole con cui il programma decide dove va ogni spot, **nell'ordine in cui si applicano**: la prima che vale decide la linguetta. I valori mostrati sono quelli impostati nei filtri.

| # | Se lo spot... | va a |
|---|---|---|
| 1 | è fuori dal filtro locale (sotto 27990 kHz o sopra 145 MHz; 430-440 MHz solo aurora) | scartato |
| 2 | contiene AUE, AU-E o AURORAL E | AURORA (aurora-E) |
| 3 | contiene una parola dell'aurora (di partenza AU, AUR) | AURORA |
| 4 | contiene una parola del F.A.I. (di partenza FAI; bande 6, 4 e 2 m: il F.A.I. non arriva a 432 MHz) | F.A.I. |
| 5 | ha un modo del meteor scatter o `<MS>` | METEOR SCATTER |
| 6 | cade in una finestra MS del bandplan (non esclusiva in 50 MHz; esclusi FT8 e FT4 in 50 e 70 MHz) | METEOR SCATTER |
| 7 | è FT8 o FT4 in 50 MHz senza un altro spot entro 4 min con il punto medio entro 150 km | meteor scatter (non E-sporadico) |
| 8 | è Q65 o JT65 in 144 MHz (salvo `<ES>`) | EME: scartato |
| 9 | contiene una scritta di altra propagazione | non E-sporadico |
| 10 | è transequatoriale | non E-sporadico (TEP) |
| 11-15 | è in 28, 40, 50, 70 o 144 MHz entro le regole delle distanze | SPORADIC-E |
| 16 | non rientra in nessuna regola (tropo, 432 MHz, HF...) | nessuna linguetta (resta in Spot raccolti) |

A destra ci sono il **filtro locale** e i filtri di ciascun modo, in cinque sottolinguette: Meteor scatter (il filtro di YA-MS si imposta nella linguetta METEOR SCATTER), E-sporadico, F.A.I., Aurora e **Prova dei filtri**, dove si incolla una riga del cluster e si vede come viene classificata. Il pulsante **Ripristina le regole dell'autore** riporta i valori di partenza.
