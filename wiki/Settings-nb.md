# INNSTILLINGER

[← Home](Home-nb)

Fra versjon 0.4.18 samler fanen **INNSTILLINGER** de generelle innstillingene for programmet og fordelingen av spot mellom fanene. Den har to underfaner.

## Generelt

| Del | Hva du stiller inn |
|---|---|
| Stasjon | Kallesignal, lokator og høyde, de samme som i linjen øverst; **Bruk** bekrefter dem for alle faner. |
| Utseende | Språk, fargetema (skjemaene fra YAEC og YA-MS og, fra versjon 0.5.0, de seks temaene fra IP-TRX-CTRL: Lys, Natt, Svart, Blå, Lilla, Grønn) og **skriftstørrelse for hele programmet** (85-140 %, som i YA-MS). Språk og farger bygger opp vinduet på nytt: innstillinger og spot beholdes. |
| Kart og prognosedata | Kontinentet, den samme menyen som i fanelinjen. |
| Tid | NTP-status og forskjellen mellom servertiden og PC-ens klokke. |
| Oppdateringer | Sjekk på GitHub ved oppstart og hvert 30. minutt, automatisk og stille oppdatering, knappen **Sjekk nå**. Automatisk oppdatering gjelder ikke for systempakker (.deb, .rpm, Arch, Slackware). |
| Data | Knapp som åpner mappen `data`. API-nøkkelen til Bureau of Meteorology blir værende i DATA COLLECTION › Utbredelsesdata. |

## Fordeling av spot

Til venstre viser en tabell reglene programmet bruker for å avgjøre hvor hvert spot skal, **i den rekkefølgen de brukes**: den første regelen som passer, avgjør fanen. Verdiene som vises, er de som er stilt inn i filtrene.

| # | Hvis spotet... | går til |
|---|---|---|
| 1 | er utenfor det lokale filteret (under 27990 kHz eller over 145 MHz; 430-440 MHz bare nordlys) | forkastet |
| 2 | inneholder AUE, AU-E eller AURORAL E | AURORA (nordlys-E) |
| 3 | inneholder et nordlysord (standard AU, AUR) | AURORA |
| 4 | inneholder et F.A.I.-ord (standard FAI; bånd 6, 4 og 2 m: F.A.I. når ikke opp til 432 MHz) | F.A.I. |
| 5 | har en meteor scatter-modus eller `<MS>` | METEOR SCATTER |
| 6 | faller i et MS-vindu i båndplanen (ikke eksklusivt på 50 MHz; FT8 og FT4 på 50 og 70 MHz unntatt) | METEOR SCATTER |
| 7 | er FT8 eller FT4 på 50 MHz uten et annet spot innen 4 min og midtpunkt innenfor 150 km | meteor scatter (ikke sporadisk E) |
| 8 | er Q65 eller JT65 på 144 MHz (med mindre `<ES>`) | EME: forkastet |
| 9 | inneholder et merke for annen utbredelse | ikke sporadisk E |
| 10 | er transekvatorial | ikke sporadisk E (TEP) |
| 11-15 | er på 28, 40, 50, 70 eller 144 MHz innenfor avstandsreglene | SPORADIC-E |
| 16 | passer ikke med noen regel (tropo, 432 MHz, HF...) | ingen fane (blir værende i Innsamlede spot) |

Til høyre ligger det **lokale filteret** og filtrene for hver utbredelsesmåte, i fem underfaner: Meteor scatter (YA-MS-filteret stilles inn i fanen METEOR SCATTER), Sporadisk E, F.A.I., Nordlys og **Test av filtre**, der du limer inn en klyngelinje og ser hvordan den klassifiseres. Knappen **Gjenopprett forfatterens regler** setter tilbake standardverdiene.
