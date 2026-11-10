# METEOR SCATTER

[← Home](Home-pt-br)

Esta aba contém o **YA-MS 1.6.0** completo, o programa de meteor scatter de IC8TEM: esfera celeste com os radiantes, fluxo dos radares SKiYMET (Collm, Sodankylä), cálculo da eficiência das chuvas, implementação experimental das previsões, geometria e atenuação, GRAVES, mapa de quadrados, **Chuvas ao vivo** (ZHR medidos pela Global Meteor Network e radiantes do radar CMOR), **Mapa das fontes** (Collm, Sodankylä, GRAVES e CMOR em relação à estação) e o documento **Eficiência - explicação** com as fórmulas.

O YA-MS 1.5.0 acrescenta a aba **Coleta de dados** (spots de meteor scatter filtrados por MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro do E-LAYER ANALYZER ela não abre sessões telnet próprias: recebe as linhas das sessões de DATA COLLECTION, por isso não há logins duplicados com o mesmo indicativo.

O YA-MS 1.6.0 reorganiza as abas em **Ao vivo e dados em tempo real** (tráfego meteor scatter ao vivo com a nuvem de cada contato, fluxo radar, GRAVES, coleta de dados) e **Cálculo de previsão** (previsão, geometria, cálculo da eficiência, implementação das previsões).

- O YA-MS usa o idioma e o esquema de cores escolhidos no topo do E-LAYER ANALYZER.
- As configurações do YA-MS são salvas em `data/config.ini`.
- Cada nova versão do YA-MS é incorporada com o script `tools/sync_yams.py`.

Para mais detalhes, veja o [repositório do YA-MS](https://github.com/ic8tem/YA-MS).
