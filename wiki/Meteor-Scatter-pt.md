# METEOR SCATTER

[← Home](Home-pt)

Este separador contém o **YA-MS 1.8.3** completo, o programa de meteor scatter de IC8TEM: esfera celeste com os radiantes, fluxo dos radares SKiYMET (Collm, Sodankylä), cálculo da eficiência das chuvas, implementação experimental das previsões, geometria e atenuação, GRAVES, mapa de quadrados, **Chuvas em direto** (ZHR medidos pela Global Meteor Network e radiantes do radar CMOR), **Mapa das fontes** (Collm, Sodankylä, GRAVES e CMOR em relação à estação) e o documento **Explicação do cálculo da eficiência** com as fórmulas.

O YA-MS 1.5.0 acrescenta o separador **Recolha de dados** (spots de meteor scatter filtrados por MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`). Dentro do E-LAYER ANALYZER não abre sessões telnet próprias: recebe as linhas das sessões de DATA COLLECTION, por isso não há logins duplicados com o mesmo indicativo.

O YA-MS 1.6.0 reorganiza os separadores em **Em direto e dados em tempo real** (tráfego meteor scatter em direto com a nuvem de cada contacto, fluxo radar, GRAVES, recolha de dados) e **Cálculo de previsão** (previsão, geometria, cálculo da eficiência, implementação das previsões).

As versões 1.7.0 e 1.8.0 do YA-MS acrescentam a comparação dos radiantes com o catálogo IAU MDC, as contagens diárias da Global Meteor Network por chuva, o índice k a partir dos índices de população, o raio inicial do rasto de Stober e outros (2021), a duração dos pings entre bandas (λ² por omissão ou as medições HamSCI MSQP de NN4SA) e a comparação com as deteções BRAMS em Sonneberg (Perseidas 2026). A atualização automática do YA-MS está desligada dentro do E-LAYER ANALYZER, que se atualiza sozinho a partir das Releases do GitHub.

O YA-MS 1.8.1 trata o lóbulo da antena como diagrama vertical (caixa «diagrama da antena», ativada por predefinição): os pontos de reflexão são ponderados com o ganho vertical das duas antenas e as eficiências já não desaparecem nos percursos longos. Acrescenta o aviso «nenhuma zona comum», destaca a caixa do ângulo β (a escolha que mais pesa), usa uma só data de cálculo, traz o «Manual de utilização» e a comparação com 181 767 spots reais em 50 MHz do Meteor Scatter QSO Party da HamSCI de agosto de 2025: as horas e os lados do percurso são fiáveis, as percentagens são indicativas.

- O YA-MS usa o idioma e o esquema de cores escolhidos no topo do E-LAYER ANALYZER.
- As definições do YA-MS são guardadas em `data/config.ini`.
- Cada nova versão do YA-MS é incorporada com o script `tools/sync_yams.py`.

Para mais pormenores, ver o [repositório do YA-MS](https://github.com/ic8tem/YA-MS).
