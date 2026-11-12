# METEOR SCATTER

[← Home](Home-pt-br)

Esta aba contém o **YA-MS 1.8.3** completo, o programa de meteor scatter de IC8TEM: esfera celeste com os radiantes, fluxo dos radares SKiYMET (Collm, Sodankylä), cálculo da eficiência das chuvas, implementação experimental das previsões, geometria e atenuação, GRAVES, mapa de quadrados, **Chuvas ao vivo** (ZHR medidos pela Global Meteor Network e radiantes do radar CMOR), **Mapa das fontes** (Collm, Sodankylä, GRAVES e CMOR em relação à estação) e o documento **Explicação do cálculo da eficiência** com as fórmulas.

O YA-MS 1.5.0 acrescenta a aba **Coleta de dados** (spots de meteor scatter filtrados por MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro do E-LAYER ANALYZER ela não abre sessões telnet próprias: recebe as linhas das sessões de DATA COLLECTION, por isso não há logins duplicados com o mesmo indicativo.

O YA-MS 1.6.0 reorganiza as abas em **Ao vivo e dados em tempo real** (tráfego meteor scatter ao vivo com a nuvem de cada contato, fluxo radar, GRAVES, coleta de dados) e **Cálculo de previsão** (previsão, geometria, cálculo da eficiência, implementação das previsões).

As versões 1.7.0 e 1.8.0 do YA-MS acrescentam a comparação dos radiantes com o catálogo IAU MDC, as contagens diárias da Global Meteor Network por chuva, o índice k a partir dos índices de população, o raio inicial do rastro de Stober e outros (2021), a duração dos pings entre bandas (λ² por padrão ou as medições HamSCI MSQP de NN4SA) e a comparação com as detecções BRAMS em Sonneberg (Perseidas 2026). A atualização automática do YA-MS fica desativada dentro do E-LAYER ANALYZER, que se atualiza sozinho a partir das Releases do GitHub.

O YA-MS 1.8.1 trata o lóbulo da antena como diagrama vertical (caixa «diagrama da antena», ligada por padrão): os pontos de reflexão são ponderados pelo ganho vertical das duas antenas e as eficiências não somem mais nos percursos longos. Acrescenta o aviso «nenhuma zona comum», destaca a caixa do ângulo β (a escolha que mais pesa), usa uma só data de cálculo, traz o «Manual de uso» e a comparação com 181 767 spots reais em 50 MHz do Meteor Scatter QSO Party da HamSCI de agosto de 2025: as horas e os lados do percurso são confiáveis, as porcentagens são indicativas.

- O YA-MS usa o idioma e o esquema de cores escolhidos no topo do E-LAYER ANALYZER.
- As configurações do YA-MS são salvas em `data/config.ini`.
- Cada nova versão do YA-MS é incorporada com o script `tools/sync_yams.py`.

Para mais detalhes, veja o [repositório do YA-MS](https://github.com/ic8tem/YA-MS).
