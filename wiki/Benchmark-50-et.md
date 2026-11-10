# BENCHMARK 50 MHz

[← Home](Home-et)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Katsestend **mitmehüppelise sporaadilise E jaoks 50 MHz-l**, kasutades ühe jaama ja soovi korral teise jaama ADIF-logi.

## Logide laadimine

- **Jaam 1**: *Laadi ADIF* / *Eemalda ADIF*. Programm võtab logist `STATION_CALLSIGN` ja `MY_GRIDSQUARE`; saatmiskoht on iga QSO `MY_GRIDSQUARE`.
- **Jaam 2** (valikuline): samad nupud teisel real; märkeruut *Jaam 2* näitab või peidab selle. Jaama 1 trassid on helesinised, jaama 2 trassid lillad.
- Programm kasutab lokaatoriga 50 MHz QSO-sid (puuduvad või 4-märgilised lokaatorid võetakse CALL3-st), mitmehüppelisi: üle 3100 km (autori reegel; hüpete arv järgib IK3XTV tabelit).
- **Transekvatoriaalsed** sided jäetakse välja (autori reegel): Aafrika ja Lõuna-Ameerika suunas, kui keskpunkt on magnetilise inklinatsiooni vööndis -20° kuni +20°, välja arvatud juhul, kui logis on `PROP_MODE=ES`.

## Päev ja kellaaeg

Vali **päev** (avamisel see, kus on kõige rohkem mitmehüppelisi QSO-sid) ja aeg **alates… kuni…** UTC, 15-minutiliste sammudega. Märkeruut **foEs** (valikuline) värvib trassid minimaalse foEs järgi ja näitab seda tabelis.

## Kaart

Vaba keskpunktiga **Mercatori** kaart: rullik = suum, lohistamine = nihutamine.

![Trass](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Hägune kollane täpp** tähistab iga hüppe keskpunkti, kus eeldatakse Es-pilve asukohta; hiire all oleval trassil muutub see suuremaks.
- **Hiir trassi lähedal**: joon ja ikoonid suurenevad ning ilmub DX-jaama **6-märgiline WWL-ruut**.
- **Vasakklõps** sellel trassil: hüpikaken küsib, kas näidata **kõiki faili QSO-sid** (kõik päevad), mille kaugema otsa asukoht on selle punkti suhtes **150 km piires**. *Jah*: kaart tühjendatakse ja näitab neid QSO-sid oranži ringi sees; *Ei*: kaart jääb samaks. *Tagasi päeva ja kellaaja juurde* taastab eelmise valiku.

![Ümbrus](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Üksikasjad

Valitud trassi kohta: kaugus ja hinnang IK3XTV tabeli järgi, asimuut mõlemas suunas, võrdsed hüpped peegelduspunktide ruutudega, lühike esimene hüpe kuni 1200 km (IC8TEM-i kogemus) ning foEs-i märkeruuduga minimaalne foEs.
