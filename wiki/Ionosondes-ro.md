# IONOSONDE

[← Home](Home-ro)

![IONOSONDE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Sub-filă a **SPORADIC-E** (din versiunea 0.5.2): **foEs măsurată** de ionosondele rețelei **GIRO** din toată lumea, alături de cea pe care programul o obține din spoturi.

## Datele

- 62 de ionosonde GIRO (lista ULCAR), cu ultima **foEs**, **foF2** și **MUF(D)** citite automat din ionograme.
- Datele sosesc prima dată când se deschide fila și apoi **la fiecare 15 minute**, singure (caseta *Actualizează singur la fiecare 15 minute*); *Actualizează acum* le cere imediat. Arhiva păstrează ultimele 48 de ore în dosarul `giro` al datelor.
- Date **GIRO** (Reinisch și Galkin 2011), licență **CC BY-NC-SA 4.0**: doar uz necomercial. Serviciul este același ca în *MUF Muncher* de DM6AX.

## Harta și tabelul

- Fiecare ionosondă este un punct colorat cu ultima foEs (aceleași culori ca norii Es) și valoarea în MHz; **gri** dacă nu are date recente, cu **inel** dacă ultima ionogramă are peste 90 de minute.
- Tabel: Cod, Ionosondă, km de stația ta, UTC al ultimei ionograme, foEs, foF2, MUF(D) și **Spoturi foEs≥**: foEs minimă obținută din spoturile Es din ultimele 30 de minute care se reflectă în limita a 500 km de ionosondă (reglabil), cu numărul de spoturi.

## Ionosonda aleasă

Clic pe un rând sau lângă o ionosondă pe hartă:

- **graficul ultimelor 24 de ore**: foEs galben, foF2 albastru-deschis, MUF(D) violet punctat;
- **MUF-ul E-sporadic** la 1000, 1500, 2000 și 2400 km (MUF = foEs · sec φ, la altitudinea Es aleasă) cu **benzile care se deschid** (28, 40, 50, 70, 144 MHz);
- dacă spoturile cer mai mult decât foEs măsurată, programul semnalează acest lucru: norul poate fi departe de ionosondă, sau ionosonda a ajuns la limita domeniului său de frecvență (Chartier 2022).
