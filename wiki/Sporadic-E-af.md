# SPORADIC-E

[← Home](Home-af)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## Die kaart

'n Knoppie bo die kaart kies die aansig:

- **Azimutaal - enkele sprong**: azimutale ekwidistante kaart gesentreer op jou stasie, vaste radius van 2500 km, met die sones van die IK3XTV-tabel (500 km minimum, 1500-2000 km optimaal, 2400 km maksimum vir 'n enkele sprong).
- **Azimutaal - maksimum afstand (multi-sprong)**: dieselfde kaart met die skuifbalk **Maksimum afstand** (van 300 tot 20000 km, ook met die muiswieletjie); die grense van die dubbele (5000 km) en driedubbele sprong (6900 km) verskyn ook, sodat die verloop van multi-sprong gevolg kan word.
- **Reghoekig**: die vorige wêreldkaart, met die knoppies Europa en Wêreld.

In die azimutale aansigte wys die strepieringe die **km vanaf die middel** (die stap verander met die zoem), die rand merk die maksimum afstand wat gewys word, en die muiswyser gee die afstand en azimut vanaf die middel.

![Multi-sprong](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## Wat dit wys

- Die **spots** van die laaste paar minute (15 min - 12 h) op die **28, 40 (8 m, by verstek af), 50, 70 en 144 MHz**-bande, uit die telnet-sessies geneem. Lokators kom uit die spotkommentaar (bv. `JN70CN<ES>JO62QM`) of uit die CALL3-databasis.
- Spots met **<MS>**, **<AU>** of **<TR>** in die teks word **verwerp**: hulle het nie met sporadiese E te doen nie, maar is nuttig vir meteor scatter, aurora of tropo.
- **Transekwatoriaal, outeur se reël (IC8TEM)**: na Afrika en Suid-Amerika, as die middelpunt in die magnetiese inklinasieband tussen -20° en +20° val, is die kontak transekwatoriaal (TEP) en nie sporadiese E nie, tensy die spot `<ES>` vermeld.
- **144 MHz, outeur se reël (IC8TEM)**: sporadiese E op 2 m begin **verder as 900 km** (korter spots word verwerp). 'n Enkele 144 MHz-spot **word nie op sy eie geplot nie**: dit verskyn wanneer binne **5 minute** 'n tweede 144 MHz-spot aankom met die wolk op dieselfde plek (refleksiepunte binne 300 km), of wanneer daar 'n **kortsprong-50 MHz**-spot naby is (binne 1200 km, in die laaste 15 minute). Die balk bo-aan wys hoeveel 144 MHz-spots op bevestiging wag.
- Vir elke spot: afstand, **aantal spronge** volgens die **IK3XTV**-tabel, elevasie, **minimum foEs** (sekanswet).
- **Eerste sprong van albei stasies**: oranje vierkant = die spotter se eerste sprong, pers = die DX se eerste sprong (gelyke spronge); geel = middelpunt van 'n enkele sprong.
- **Kort eerste sprong** (hol sirkels): in multi-sprongopeninge het die outeur 'n kort eerste sprong binne **1200 km** van sy eie QTH ervaar; die lengte kan ingestel word.
- **Es-wolke** op **Maidenhead-vierkante** (2° x 1°), gekleur volgens minimum foEs: blou < 5, ligblou < 7, groen < 10, geel < 14, oranje < 20, rooi > 20 MHz.
- Tabel van **Es-wolke**: vir elke vierkant die **MUF vir jou stasie** (rooi = 144 MHz moontlik, oranje = 70, geel = 50).

## IK3XTV-tabel

| | minimum | optimaal | maksimum |
|---|---|---|---|
| Enkele sprong | 500-650 km | 1500-2000 km | 2200-2400 km |
| Dubbele sprong | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Driedubbele sprong | – | 4800-6400 km | 6900 km |

## Hoe om dit te gebruik

- **Klik op 'n spot**: die pad word uitgelig, met refleksiepunte, lokators en azimute in die besonderhede.
- **Klik op 'n spot** in die tabel: die pad lig op die kaart op (geel gloed) en, as dit buite sig is, skuif die kaart of vergroot dit sy radius om dit te wys. Die spot bly gekies wanneer nuwe spots aankom.
- **Die tabel volg die kaart**: met 'n ingestelde zoem wys die spottabel en die wolktabel slegs wat op die kaart sigbaar is.
- **Klik op die kaart**: die punt word as 'n moontlike Es-wolk behandel, met die foEs wat vir 28, 50, 70 en 144 MHz nodig is.

## Nota oor 432 MHz

Sporadiese E **geld nie vir 432 MHz nie**: die Es-MUF bereik ongeveer 150-200 MHz en die ARRL-rekords lys Es-kontakte slegs op 50 en 144 MHz. 70 cm-spots word nie hier ontleed nie.

## Vierkante gekleur volgens frekwensie en flikkerende MUF

Die Maidenhead-vierkante van die refleksiepunte word gekleur volgens die hoogste frekwensie van die spots: **groen** tot 29 MHz, **geel** tot 51, **oranje** tot 71, **rooi** tot 145 MHz. Uit die foEs van die vierkant word die waarskynlike MUF op die langste sprong (2400 km, IK3XTV-tabel) beraam: as dit 'n hoër band bereik, **flikker** die vierkant in die kleur van daardie band (beginsel van IK3XTV en Jacobs-Leinwoll: kort skip beteken hoë MUF).

Regs bo op die kaart verskyn die N0NBH-opsomming (E-skip vir die gekose kontinent, SFI, K- en A-indekse).

## Reëls per band (weergawe 0.4.18)

- **50 MHz**: geen Sporadic-E binne 450 km nie. FT8- en FT4-spots is Sporadic-E slegs as hulle deel van 'n wolk is: 'n ander 50 MHz-spot binne 4 minute met sy middelpunt binne 150 km. 'n Alleenstaande FT8/FT4-spot gaan na meteor scatter.
- **70 MHz**: geen Sporadic-E binne 450 km nie. FT8 en FT4 gaan na Sporadic-E selfs binne die meteor scatter-venster van die bandplan (die bekende modus besluit eerste, dan die frekwensie).
- **40 MHz (8 m)**: minimum afstand afgelei van die 450 km van 50 MHz met die sekanswet (ongeveer 330 km); slegs afstande binne die eerste sprong van die E-laaggeometrie (ongeveer 2440 km). Padkleur: geel.
- **144 MHz**: Sporadic-E vanaf 900 km, met bevestiging; Q65- en JT65-spots is EME en word weggegooi, tensy `<ES>` in die teks staan.
- G0ISW (Propagation identification, 2024) sien die kortste Sporadic-E verder as 400 km op 50 en 70 MHz en verder as 1400 km op 144 MHz: die outeur se reëls stem ooreen op 50 en 70 MHz en is wyer op 144 MHz, maar vra 'n bevestiging.
- Die volgorde waarin al die reëls toegepas word, met die waardes in gebruik, word gewys in **[INSTELLINGS](Settings-af)** › Spotverdeling.
