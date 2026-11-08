# METEOR SCATTER

[← Home](Home-pt)

Este separador contém o **YA-MS 1.5.1** completo, o programa de meteor scatter de IC8TEM: esfera celeste com os radiantes, fluxo dos radares SKiYMET (Collm, Sodankylä), cálculo da eficiência das chuvas, implementação experimental das previsões, geometria e atenuação, GRAVES, mapa de quadrados, **Chuvas em direto** (ZHR medidos pela Global Meteor Network e radiantes do radar CMOR), **Mapa das fontes** (Collm, Sodankylä, GRAVES e CMOR em relação à estação) e o documento **Eficiência - explicação** com as fórmulas.

O YA-MS 1.5.0 acrescenta o separador **Recolha de dados** (spots de meteor scatter filtrados por MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro do E-LAYER ANALYZER não abre sessões telnet próprias: recebe as linhas das sessões de DATA COLLECTION, por isso não há logins duplicados com o mesmo indicativo.

- O YA-MS usa o idioma e o esquema de cores escolhidos no topo do E-LAYER ANALYZER.
- As definições do YA-MS são guardadas em `data/config.ini`.
- Cada nova versão do YA-MS é incorporada com o script `tools/sync_yams.py`.

Para mais pormenores, ver o [repositório do YA-MS](https://github.com/ic8tem/YA-MS).
