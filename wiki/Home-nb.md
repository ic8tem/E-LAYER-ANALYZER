# E-LAYER ANALYZER

*av Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Português (Brasil)](Home-pt-BR) · [Dansk](Home-da) · **Norsk** · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Polski](Home-pl) · [Čeština](Home-cs) · [Magyar](Home-hu) · [Română](Home-ro) · [Ελληνικά](Home-el) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [ไทย](Home-th) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh) · [中文 (繁體)](Home-zh-TW)

**E-LAYER ANALYZER** er et gratis program for radioamatører som analyserer alle VHF-utbredelsesmåter der ionosfærens **E-lag** er involvert: **sporadisk E** (også interkontinental multihopp), **F.A.I.** (field-aligned irregularities, uregelmessigheter langs magnetfeltet), **meteor scatter** (med det innebygde programmet YA-MS) og **radionordlys**. Det kjører på **Windows (64 og 32 bit), Linux, Raspberry Pi og Mac**, på **28 språk**, og skriver ingenting utenfor sin egen mappe.

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Fanene

| Fane | Hva den gjør |
|---|---|
| [SPORADIC-E](Sporadic-E-nb) | Es-spot på kartet, første hopp for begge stasjonene, Es-skyer på Maidenhead-ruter, MUF for din stasjon |
| [BENCHMARK Es](Benchmark-Es-nb) | Testbenk for en hel ADIF-logg (også fra en annen stasjon): QSO-er per dag og per time, skip per sektor, estimert foEs, kontroll av multihopp, skyenes forflytning |
| [BENCHMARK 50 MHz](Benchmark-50-nb) | Multihopp sporadisk E på 50 MHz fra én eller to ADIF-logger: Mercator-kart, dag og tid, valgfri foEs, forstørret strekning med WWL, QSO-er innenfor 150 km |
| [F.A.I.](FAI-nb) | Scatter trail for scatter-punktene til et stasjonspar, og isorefleksjonskurver for å kontrollere et punkt |
| [METEOR SCATTER](Meteor-Scatter-nb) | Hele programmet YA-MS (svermeffektivitet, radarer, prognoser) |
| [AURORA](Aurora-nb) | Nordlysoval, Kp og solvind fra NOAA, scatter trail og isorefleksjon for nordlys, anbefalt antenneretning |
| [DATA COLLECTION](Data-Collection-nb) | Tre telnet-økter (DX-cluster, RBN, DM4X med PSKReporter), innsamlede spot, CALL3-database |
| [LOG ADIF](Log-ADIF-nb) | Kjørte og bekreftede ruter bånd for bånd fra 28 MHz og oppover, fra din ADIF eller fra LoTW |
| [KILDER, SAMMENDRAG AV BEREGNINGSVALGENE, BRUKERHÅNDBOK, TEKNISK RAPPORT](Sources-and-Technical-Document-nb) | Kilder, formler og beregningsvalg, utførlig brukerhåndbok, teknisk rapport: kan eksporteres til Word |

## Komme i gang

1. [Installasjon](Installation-nb)
2. [Komme i gang](Getting-Started-nb)
3. [Fargeskjemaer og språk](Colours-and-Languages-nb)
4. [Datamappe](Data-Folder-nb)
5. [Ofte stilte spørsmål](FAQ-nb)

## Hovedkilder

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV, avstandstabell for sporadisk E ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)).
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (kilde for YA-MS).

Den fullstendige listen står i programmets fane KILDER.
