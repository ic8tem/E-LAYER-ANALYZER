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

**Lokaler Filter** (optionales Kästchen, in EINSTELLUNGEN): „DX de“-Zeilen unter 27990,0 kHz oder über 145 MHz werden verworfen, nicht in der Konsole angezeigt und nicht gespeichert; zwischen 430 und 440 MHz passieren nur Aurora-Spots. **Ungenaue Locator** (weniger als 6 Zeichen) werden aus CALL3 genommen: Hat CALL3 einen 6-stelligen Locator, hat dieser immer Vorrang. Wie in YA-MS gelten als Meteorscatter-Spots (nicht Sporadic-E) diejenigen in den typischen MS-Frequenzfenstern (50190-50300, 70100-70400, 144290-144399 kHz) ohne Kennzeichen einer anderen Ausbreitung (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) sowie diejenigen mit reinen MS-Betriebsarten (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) oder mit `<MS>`. Ausnahme: FT8 und FT4 auf 50 und 70 MHz, wo eine bekannte Betriebsart entscheidet (das FT8 auf 70,154 MHz geht zu Sporadic-E, auch innerhalb des MS-Fensters). Spots mit `<AS>`/`<ACS>` (Aircraft Scatter) werden für Sporadic-E nicht verwendet.

## Spot-Filter

Es gibt **nur eine Datensammlung für das ganze Programm**: Jeder Spot läuft durch die Filter und geht an den Reiter der jeweiligen Ausbreitungsart. Ab Version 0.4.18 stehen die Filter, der lokale Filter und der Filtertest im Reiter **[EINSTELLUNGEN](Settings-de)**, Unterreiter **Spot-Verteilung**.

## CALL3

JTDX-Datenbank **Rufzeichen → Locator** (über eine Million Rufzeichen) in `data/call3.txt`. Schaltflächen **Neu laden** und **Neue CALL3.TXT importieren**. In den Kommentaren der Spots gelesene 6-stellige Locator werden zu `data/call3_nuovi.txt` hinzugefügt.

Ab Version 0.4.16 gibt es eine einzige CALL3-Datenbank für das ganze Programm, die der Datensammlung von YA-MS (Reiter METEOR SCATTER), mit denselben Regeln wie LIVETROPOVHF: Aktualisierung von MMMonVHF alle 12 Stunden, Handeinträge, hinzugefügte Dateien, Cluster-Korrekturen, gelernte Locator, G7VRD alle 30 Minuten und als letzte Quelle die JTDX-Schicht (data/grid_data.bin), nie in call3.txt geschrieben; Korrekturen werden in data/call3.txt geschrieben und die Locator aus data/call3_nuovi.txt einmal übernommen. Spots von WSJT-X, JTDX und MSHV kommen auch über UDP (Quelle WSJT-X UDP, Port 2237 mit Weiterleitung an 2238).

Diese Filter aus YA-MS (CW, Mindestentfernung, Frequenzfenster, MS-Modi, Bänder) gelten nur für den Reiter METEOR SCATTER: Sie entscheiden, welche Spots in seine Sammlung kommen. Die anderen Reiter erhalten alle Spots und nutzen ihre eigenen Filter.

## Unterreiter Spot und Ausbreitungsdaten

Der Reiter hat zwei Unterreiter: **Spot** (Sitzungen, PSKReporter MQTT, WSJT-X UDP, gesammelte Spots und CALL3) und **Ausbreitungsdaten**:

- **N0NBH** (hamqsl.com): VHF-Banner und Sonnendaten, E-skip Europa und Nordamerika (DXrobot), Aurora.
- **Bz für die Aurora**: Sonnenwind in Echtzeit von NOAA SWPC (DSCOVR/ACE, Punkt L1), die beste Quelle für Bz.
- **Meteorscatter**: tägliche Meteoraktivität (RMOB) von Make More Miles on VHF.

Sie werden höchstens einmal pro Stunde heruntergeladen. Die Zusammenfassung erscheint auf den Karten SPORADIC-E, F.A.I. und AURORA. Das **Kontinent-Menü** rechts in der Reiterleiste verschiebt die Karten und wählt die Daten dieses Kontinents.
