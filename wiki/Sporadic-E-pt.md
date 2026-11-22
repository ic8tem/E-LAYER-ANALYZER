# SPORADIC-E

[← Home](Home-pt)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## O mapa

Um botão acima do mapa escolhe a vista:

- **Azimutal**: mapa azimutal equidistante centrado na sua estação, com o cursor de **zoom** (de 300 a 20000 km a partir do centro, também com a roda do rato).
- **Mercator**: o mapa-múndi, com os botões Europa e Mundo.

Nas duas projeções são desenhados à volta da estação os **círculos de distância de Flavio IK3XTV**: 500 km (mínimo do salto único), 1500-2000 km (ótimo), 2400 km (máximo do salto único), 5000 km (máximo do salto duplo) e 6900 km (máximo do salto triplo).

Nas vistas azimutais, os anéis tracejados indicam os **km a partir do centro** (o passo muda com o zoom), o bordo marca a distância máxima mostrada e o cursor do rato indica a distância e o azimute a partir do centro.

![Multi-salto](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## O que mostra

- Os **spots** dos últimos minutos (15 min - 12 h) nas bandas de **28, 40 (8 m, desligada por omissão), 50, 70 e 144 MHz**, obtidos das sessões telnet. Os locators vêm do comentário do spot (p. ex. `JN70CN<ES>JO62QM`) ou da base de dados CALL3.
- Os spots com **<MS>**, **<AU>** ou **<TR>** no texto são **descartados**: não dizem respeito à esporádica E, mas são úteis para meteor scatter, aurora ou tropo.
- **Transequatorial, regra do autor (IC8TEM)**: em direção a África e à América do Sul, se o ponto médio cair na faixa de inclinação magnética entre -15° e +15° (valor retomado de Cracknell, UKSMG: zonas do TEP a cerca de 5-15° da linha de inclinação zero), o contacto é transequatorial (TEP) e não esporádica E, a menos que o spot indique `<ES>`.
- **144 MHz, regra do autor (IC8TEM)**: a esporádica E nos 2 m começa **para lá dos 900 km** (os spots mais curtos são descartados). Um único spot de 144 MHz **não é desenhado sozinho**: aparece quando, em **5 minutos**, chega um segundo spot de 144 MHz com a nuvem no mesmo sítio (pontos de reflexão a menos de 300 km), ou quando há perto um spot de **50 MHz de salto curto** (a menos de 1200 km, nos últimos 15 minutos). A barra no topo mostra quantos spots de 144 MHz aguardam confirmação.
- Para cada spot: distância, **número de saltos** segundo a tabela **IK3XTV**, elevação, **foEs mínima** (lei da secante).
- **Primeiro salto de ambas as estações**: quadrado laranja = primeiro salto do spotter, violeta = primeiro salto do DX (saltos iguais); amarelo = ponto médio de um salto único.
- **Primeiro salto curto** (círculos vazios): nas aberturas multi-salto o autor verificou um primeiro salto curto até **1200 km** do seu QTH; o comprimento pode ser ajustado.
- **Nuvens Es** nos **quadrados Maidenhead** (2° x 1°), coloridas pela foEs mínima: azul < 5, azul-claro < 7, verde < 10, amarelo < 14, laranja < 20, vermelho > 20 MHz.
- Tabela **Nuvens Es**: para cada quadrado, a **MUF para a sua estação** (vermelho = 144 MHz possível, laranja = 70, amarelo = 50).

## Tabela IK3XTV

| | mínima | ótima | máxima |
|---|---|---|---|
| Salto único | 500-650 km | 1500-2000 km | 2200-2400 km |
| Salto duplo | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Salto triplo | – | 4800-6400 km | 6900 km |

## Como usar

- **Clique num spot**: o percurso é realçado, com pontos de reflexão, locators e azimutes nos detalhes.
- **Clique num spot** da tabela: o percurso acende-se no mapa (halo amarelo) e, se estiver fora da vista, o mapa desloca-se ou alarga o raio para o mostrar. O spot continua selecionado quando chegam spots novos.
- **A tabela segue o mapa**: com um zoom aplicado, a tabela dos spots e a das nuvens mostram apenas o que é visível no mapa.
- **Clique no mapa**: o ponto é tratado como uma possível nuvem Es, com a foEs necessária para 28, 50, 70 e 144 MHz.

## Nota sobre os 432 MHz

A esporádica E **não se aplica aos 432 MHz**: a MUF da Es chega a cerca de 150-200 MHz e os recordes da ARRL registam contactos Es apenas em 50 e 144 MHz. Os spots de 70 cm não são analisados aqui.

## Quadrados coloridos por frequência e MUF a piscar

Os quadrados Maidenhead dos pontos de reflexão são coloridos pela frequência mais alta dos spots: **verde** até 29 MHz, **amarelo** até 51, **laranja** até 71, **vermelho** até 145 MHz. A partir da foEs do quadrado estima-se a MUF provável no salto mais longo (2400 km, tabela IK3XTV): se atingir uma banda mais alta, o quadrado **pisca** com a cor dessa banda (princípio de IK3XTV e de Jacobs-Leinwoll: skip curto significa MUF alta). Para os 144 MHz vale a regra do autor: o quadrado pisca a vermelho só se a nuvem der um salto único em 50 MHz entre 450 e 500 km (450 km com uma tolerância de 50 km).

No canto superior direito do mapa aparece a síntese de N0NBH (E-skip do continente escolhido, SFI, índices K e A).

## Regras por banda (versão 0.4.18)

- **50 MHz**: nenhuma esporádica E até 450 km. Os spots FT8 e FT4 só são esporádica E se fizerem parte de uma nuvem: outro spot em 50 MHz em 4 minutos com o ponto médio a menos de 150 km. Um spot FT8/FT4 isolado vai para o meteor scatter.
- **70 MHz**: nenhuma esporádica E até 450 km. FT8 e FT4 vão para a esporádica E mesmo dentro da janela de meteor scatter do plano de banda (decide primeiro o modo conhecido, depois a frequência).
- **40 MHz (8 m)**: distância mínima obtida a partir dos 450 km dos 50 MHz com a lei da secante (cerca de 330 km); só as distâncias dentro do primeiro salto da geometria da camada E (cerca de 2440 km). Cor do percurso: amarelo.
- **144 MHz**: esporádica E a partir de 900 km, com confirmação; os spots Q65 e JT65 são EME e são descartados, salvo `<ES>` no texto.
- G0ISW (Propagation identification, 2024) vê a esporádica E mais curta acima de 400 km em 50 e 70 MHz e acima de 1400 km em 144 MHz: as regras do autor coincidem em 50 e 70 MHz e são mais largas em 144 MHz, mas pedem uma confirmação.
- A ordem em que todas as regras se aplicam, com os valores em uso, é mostrada em **[DEFINIÇÕES](Settings-pt)** › Encaminhamento dos spots.

## Cadeias de saltos curtos (a partir da versão 0.6.0)

![Cadeia a partir do QTH](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1n_catena_dal_qth.png)

No subseparador **Precursores de multi-salto** o programa procura as cadeias do multi-salto em 50 MHz com as três condições do autor: **primeiro e último salto curto** (500-1200 km) com MUF em torno dos 50 MHz, que exigem uma foEs de 11-21 MHz, e **saltos centrais** até 2200 km, que exigem cerca de 9,3 MHz.

- **Nuvens fortes** (quadrados laranja): spots Es de todo o mundo e ionossondas GIRO com foEs de pelo menos 11,3 MHz.
- **A partir do QTH**: primeiro salto curto a partir da sua estação e último salto curto sobre outra nuvem forte ao longo da mesma direção, também no mesmo continente (por exemplo no Cazaquistão); o círculo é a zona de chegada.
- **Corredor**: duas nuvens fortes quaisquer, em qualquer direção, por exemplo **Caraíbas - Península Ibérica** ou **Europa - África Ocidental** (TEP excluído); «diz-lhe respeito» se uma extremidade estiver a menos de 1200 km de si.
- **Procurar a nuvem**: há só o primeiro salto curto; as zonas azul-claras dizem onde deveria estar a nuvem do salto curto final.
- **ALERTA** se os saltos centrais estiverem cobertos por nuvens, **PRECURSOR** se faltarem só zonas sem observadores.
- Em DEFINIÇÕES há o **aviso** no topo (e opcionalmente uma janela) para os ALERTAS que lhe dizem respeito.
- As cadeias que chegam **a mar aberto** ou onde não há estações (CALL3 e spots) não se mostram.
- **Salto duplo igual** (desde a versão 0.6.3, saltos «2 =»): até cerca de 4700 km, por exemplo para o Cazaquistão, com a nuvem a um quarto do percurso como no método UIT-R P.534-6.
- O separador **ÁBACO DOS SALTOS** resume quantos saltos são precisos em 50 MHz de 500 a 12000 km, com a foEs de cada salto e as fontes.
- **Alcance do meu sinal em 6 m** (pontos rosa): onde o seu sinal em 50 MHz aterra após 2-5 saltos, sobre as nuvens dos últimos 30 minutos estimadas a partir dos spots de todas as bandas (28-144 MHz) e das ionossondas; só onde há estações.
- **Corredor de saltos iguais** (a partir da versão 0.6.5): duas nuvens a 1400-2350 km uma da outra ligam duas zonas a 2800-4700 km com dois saltos iguais; a verificação na expedição 5U5K confirma o limite de cerca de 4700 km.
- **SINAL PRECOCE** (verde, a partir da versão 0.6.5): nuvem Es em 28 ou 24 MHz onde os 50 MHz ainda não abriram; nos ensaios os 50 MHz abriram ali em menos de uma hora em 50% dos casos (30% sem ele), com 9-16 minutos de antecedência.
- **APRS repetido** (vermelho, a partir da versão 0.6.5): pelo menos dois saltos APRS em 2 m acima de 900 km sobre a mesma nuvem em 10 minutos = ALERTA de 144 MHz; o separador pisca.
- **Quanto valem os alertas** (na estação do autor, verões 2024-2026): ALERTAS certos em 46-58% (QSO multi-salto na mesma direção em 2 horas) contra 8-10% sem alerta; mas 43% das aberturas chegam sem aviso. O alcance chega a menos de 500 km da estação distante em 13,5% das aberturas, contra 2-4% se rodado.
- As nuvens **não se deslocam**: o ensaio de 22/11/2026 mostra que deslocá-las com a sua velocidade não melhora a previsão a 15-45 minutos.

![Corredor](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1o_catena_corridoio.png)
