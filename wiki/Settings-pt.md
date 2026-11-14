# DEFINIÇÕES

[← Home](Home-pt)

A partir da versão 0.4.18 o separador **DEFINIÇÕES** reúne as opções gerais do programa e o encaminhamento dos spots entre os separadores. Tem dois subseparadores.

## Gerais

| Secção | O que se configura |
|---|---|
| Estação | Indicativo, locator e altitude, os mesmos da barra superior; **Aplicar** confirma-os para todos os separadores. |
| Aspeto | Idioma, esquema de cores e **tamanho do tipo de letra de todo o programa** (85-140 %, como no YA-MS). O idioma e as cores reconstroem a janela: as definições e os spots mantêm-se. |
| Mapas e dados de previsão | O continente, o mesmo menu da barra dos separadores. |
| Hora | Estado do NTP e diferença entre a hora dos servidores e o relógio do PC. |
| Atualizações | Verificação no GitHub ao arrancar e a cada 6 horas, atualização automática e silenciosa, botão **Verificar agora**. A atualização automática não se aplica aos pacotes de sistema (.deb, .rpm, Arch, Slackware). |
| Dados | Botão que abre a pasta `data`. A chave API do Bureau of Meteorology fica em DATA COLLECTION › Dados de propagação. |

## Encaminhamento dos spots

À esquerda, uma tabela enumera as regras que o programa usa para decidir para onde vai cada spot, **pela ordem em que se aplicam**: a primeira regra que corresponde decide o separador. Os valores mostrados são os definidos nos filtros.

| # | Se o spot... | vai para |
|---|---|---|
| 1 | está fora do filtro local (abaixo de 27990 kHz ou acima de 145 MHz; 430-440 MHz só aurora) | descartado |
| 2 | contém AUE, AU-E ou AURORAL E | AURORA (aurora-E) |
| 3 | contém uma palavra de aurora (por omissão AU, AUR) | AURORA |
| 4 | contém uma palavra de F.A.I. (por omissão FAI; bandas 6, 4 e 2 m: o F.A.I. não chega aos 432 MHz) | F.A.I. |
| 5 | tem um modo de meteor scatter ou `<MS>` | METEOR SCATTER |
| 6 | cai numa janela MS do plano de banda (não exclusiva em 50 MHz; FT8 e FT4 em 50 e 70 MHz excluídos) | METEOR SCATTER |
| 7 | é FT8 ou FT4 em 50 MHz sem outro spot em 4 min e ponto médio a menos de 150 km | meteor scatter (não esporádica E) |
| 8 | é Q65 ou JT65 em 144 MHz (salvo `<ES>`) | EME: descartado |
| 9 | contém uma etiqueta de outra propagação | não esporádica E |
| 10 | é transequatorial | não esporádica E (TEP) |
| 11-15 | está em 28, 40, 50, 70 ou 144 MHz dentro das regras de distância | SPORADIC-E |
| 16 | não corresponde a nenhuma regra (tropo, 432 MHz, HF...) | nenhum separador (fica em Spots recolhidos) |

À direita estão o **filtro local** e os filtros de cada modo, em cinco subseparadores: Meteor scatter (o filtro do YA-MS configura-se no separador METEOR SCATTER), Esporádica E, F.A.I., Aurora e **Teste dos filtros**, onde se cola uma linha do cluster e se vê como é classificada. O botão **Repor as regras do autor** repõe os valores por omissão.
