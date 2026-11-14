# DATA COLLECTION

[← Home](Home-de)

## Telnet-Sitzungen

| Sitzung | Standard | Hinweise |
|---|---|---|
| 1. DX-Cluster nach Wahl | on0dxk.dyndns.org 8000 | Bei ON0DXK sind die VHF-Spots gefiltert: `set/filter dxbm/pass 6` (dann 4, 2). Ein weiteres Beispiel: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW und RTTY, 7001 = FT8. |
| 3. Cluster mit PSKReporter | **dm4x.ddns.net 8500** | Cluster von DM4X; alternativ HamAlert (hamalert.org 7300, Konto erforderlich). |

- Automatisches Login mit dem Rufzeichen (oben oder im Feld Login); **Befehle nach dem Login**, getrennt durch `;`.
- **Neu verbinden** nach 30 s, wenn die Leitung abbricht; **Beim Start verbinden**.
- Standardmäßig werden nur Spots **ab 28 MHz** gespeichert.

## Gesammelte Spots

Die letzten 2000 Spots, mit Filtern nach Band und Quelle. Archiv: eine CSV-Datei pro Tag in `data/spot`. Bei jedem Schließen des Programms wird die Spot-Datenbank geleert (Speicher und die Dateien in `data/spot`): Jede Sitzung beginnt von vorn.

**Lokaler Filter** (optionales Kästchen): „DX de“-Zeilen unter 27990,0 kHz oder über 145 MHz werden verworfen, nicht in der Konsole angezeigt und nicht gespeichert. **Ungenaue Locator** (weniger als 6 Zeichen) werden aus CALL3 genommen: Hat CALL3 einen 6-stelligen Locator, hat dieser immer Vorrang. Wie in YA-MS gelten als Meteorscatter-Spots (nicht Sporadic-E) diejenigen in den typischen MS-Frequenzfenstern (50190-50300, 70100-70400, 144290-144399 kHz) ohne Kennzeichen einer anderen Ausbreitung (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) sowie diejenigen mit reinen MS-Betriebsarten (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) oder mit `<MS>`. Spots mit `<AS>`/`<ACS>` (Aircraft Scatter) werden für Sporadic-E nicht verwendet.

## CALL3

JTDX-Datenbank **Rufzeichen → Locator** (über eine Million Rufzeichen) in `data/call3.txt`. Schaltflächen **Neu laden** und **Neue CALL3.TXT importieren**. In den Kommentaren der Spots gelesene 6-stellige Locator werden zu `data/call3_nuovi.txt` hinzugefügt.

Ab Version 0.4.16 gibt es eine einzige CALL3-Datenbank für das ganze Programm, die der Datensammlung von YA-MS (Reiter METEOR SCATTER), mit denselben Regeln wie LIVETROPOVHF: Aktualisierung von MMMonVHF, Handeinträge, hinzugefügte Dateien, Cluster-Korrekturen, gelernte Locator und G7VRD alle 6 Stunden; Korrekturen werden in data/call3.txt geschrieben und die Locator aus data/call3_nuovi.txt einmal übernommen. Spots von WSJT-X, JTDX und MSHV kommen auch über UDP (Quelle WSJT-X UDP, Port 2237 mit Weiterleitung an 2238).
