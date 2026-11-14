# INDSTILLINGER

[← Home](Home-da)

Fra version 0.4.18 samler fanen **INDSTILLINGER** programmets generelle indstillinger og fordelingen af spots mellem fanerne. Den har to underfaner.

## Generelt

| Afsnit | Hvad du indstiller |
|---|---|
| Station | Kaldesignal, lokator og højde, de samme som i linjen øverst; **Anvend** bekræfter dem for alle faner. |
| Udseende | Sprog, farveskema (skemaerne fra YAEC og YA-MS og fra version 0.5.0 de seks temaer fra IP-TRX-CTRL: Lys, Nat, Sort, Blå, Lilla, Grøn) og **skriftstørrelse for hele programmet** (85-140 %, som i YA-MS). Sprog og farver genopbygger vinduet: indstillinger og spots bevares. |
| Kort og prognosedata | Kontinentet, den samme menu som i fanelinjen. |
| Tid | NTP-status og forskellen mellem servertiden og pc'ens ur. |
| Opdateringer | Kontrol på GitHub ved opstart og hvert 30. minut, automatisk og lydløs opdatering, knappen **Kontrollér nu**. Automatisk opdatering gælder ikke for systempakker (.deb, .rpm, Arch, Slackware). |
| Data | Knap, der åbner mappen `data`. API-nøglen til Bureau of Meteorology bliver i DATA COLLECTION › Udbredelsesdata. |

## Fordeling af spots

Til venstre viser en tabel de regler, programmet bruger til at afgøre, hvor hvert spot skal hen, **i den rækkefølge de anvendes**: den første regel, der passer, afgør fanen. De viste værdier er dem, der er indstillet i filtrene.

| # | Hvis spottet... | går til |
|---|---|---|
| 1 | er uden for det lokale filter (under 27990 kHz eller over 145 MHz; 430-440 MHz kun nordlys) | kasseret |
| 2 | indeholder AUE, AU-E eller AURORAL E | AURORA (nordlys-E) |
| 3 | indeholder et nordlysord (standard AU, AUR) | AURORA |
| 4 | indeholder et F.A.I.-ord (standard FAI; bånd 6, 4 og 2 m: F.A.I. når ikke op til 432 MHz) | F.A.I. |
| 5 | har en meteor scatter-mode eller `<MS>` | METEOR SCATTER |
| 6 | falder i et MS-vindue i båndplanen (ikke eksklusivt på 50 MHz; FT8 og FT4 på 50 og 70 MHz undtaget) | METEOR SCATTER |
| 7 | er FT8 eller FT4 på 50 MHz uden et andet spot inden for 4 min og midtpunkt inden for 150 km | meteor scatter (ikke sporadisk E) |
| 8 | er Q65 eller JT65 på 144 MHz (medmindre `<ES>`) | EME: kasseret |
| 9 | indeholder et mærke for anden udbredelse | ikke sporadisk E |
| 10 | er transækvatorialt | ikke sporadisk E (TEP) |
| 11-15 | er på 28, 40, 50, 70 eller 144 MHz inden for afstandsreglerne | SPORADIC-E |
| 16 | passer ikke til nogen regel (tropo, 432 MHz, HF...) | ingen fane (bliver i Indsamlede spots) |

Til højre ligger det **lokale filter** og filtrene for hver udbredelsesmåde i fem underfaner: Meteor scatter (YA-MS-filteret indstilles i fanen METEOR SCATTER), Sporadisk E, F.A.I., Nordlys og **Test af filtre**, hvor du indsætter en cluster-linje og ser, hvordan den klassificeres. Knappen **Gendan forfatterens regler** sætter standardværdierne tilbage.
