# Es MĀKOŅI LAIKĀ

[← Home](Home-lv)

![Es MĀKOŅI LAIKĀ](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

**SPORADIC-E** apakšcilne (kopš versijas 0.5.2): kā **sporādiskā E mākoņi** rodas, pārvietojas un izdziest, solis pa solim.

## Metode

**Chartier, Hanley un Emmons (2022)** metode: katrs sporādiskā E sakars norāda, ka atstarošanas punktā foEs ir **vismaz f / sec φ**. Punkti tiek apkopoti šūnās un laika soļos, un katra šūna saņem soļa **lielāko vērtību**. Minimālais elektronu blīvums ir NmE = 1,24·10¹⁰ · foEs² elektroni uz kubikmetru.

Programma izmanto sava modeļa atstarošanas punktus (visus lēcienus), nevis tikai viduspunktu. Vērtība ir **apakšējā robeža**: tukša šūna nozīmē tikai to, ka nav sakaru.

## Avots un vadīklas

- **Sesijas spoti** (atjaunināti katru reizi, kad atver cilni) vai **Ielādēt ADIF žurnālu vai CSV spotus**.
- **Eksportēt sesijas spotus (CSV)**: spotu arhīvs tiek iztukšots, aizverot programmu, tāpēc tā tos varēsiet pārskatīt vēlāk.
- **Joslas MHz** (28, 40, 50, 70, 144), **Solis** (5, 10 vai 15 min), **Šūna** (1°, 2° vai 4°), **Es augstums km**.
- Laika josla: ⏮ ◀ *Sākt/Apturēt* ▶ ⏭ un slīdnis; atverot sāk no soļa ar visvairāk šūnām.
- *Iepriekšējie soļi blāvi* rāda trīs soļus pirms izvēlētā; *Sakari* zīmē trases joslu krāsās.

## Tabula un grafiks

- Soļa šūnas: Šūna (dienvidrietumu stūris un centra lokators), foEs≥, NmE≥, Sakari, Joslas. Noklikšķinot uz šūnas, tiek izcelti tās sakari.
- Grafiks: lielākā minimālā foEs (dzeltena līnija) un šūnu skaits (stabiņi) katram solim.
