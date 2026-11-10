# METEOR SCATTER

[← Home](Home-pt-BR)

Esta aba contém o **YA-MS 1.6.0** completo, o programa de meteor scatter de IC8TEM: esfera celeste com os radiantes, fluxo dos radares SKiYMET (Collm, Sodankylä), cálculo da eficiência das chuvas de meteoros, implementação experimental das previsões, geometria e atenuação, GRAVES, mapa dos quadrados, **Chuvas ao vivo** (ZHR medida pela Global Meteor Network e radiantes do radar CMOR), **Mapa das fontes** (Collm, Sodankylä, GRAVES e CMOR em relação à estação) e o documento **Eficiência - explicação** com as fórmulas.

O YA-MS 1.5.0 acrescenta a aba **Coleta de dados** (spots de meteor scatter filtrados por MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro do E-LAYER ANALYZER ela não abre sessões telnet próprias: recebe as linhas das sessões de DATA COLLECTION, de modo que não há logins duplos com o mesmo indicativo.

O YA-MS 1.6.0 reorganiza as abas em **Ao vivo e dados em tempo real** (tráfego de meteor scatter ao vivo com a nuvem de cada contato, fluxo dos radares, GRAVES, coleta de dados) e **Cálculo de previsão** (previsão, geometria, cálculo da eficiência, implementação das previsões).

- O YA-MS usa o idioma e o esquema de cores escolhidos no topo do E-LAYER ANALYZER.
- As configurações do YA-MS ficam em `data/config.ini`.
- Cada nova versão do YA-MS é incorporada com o script `tools/sync_yams.py`.

Para os detalhes, veja o [repositório do YA-MS](https://github.com/ic8tem/YA-MS).
