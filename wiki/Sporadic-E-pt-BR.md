# SPORADIC-E

[← Home](Home-pt-BR)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## O mapa

Um botão acima do mapa escolhe a vista:

- **Azimutal**: mapa azimutal equidistante centrado na sua estação, com o controle deslizante de **zoom** (de 300 a 20000 km a partir do centro, também com a roda do mouse).
- **Mercator**: o mapa-múndi, com os botões Europa e Mundo.

Nas duas projeções são desenhados em volta da estação os **círculos de distância de Flavio IK3XTV**: 500 km (mínimo do salto simples), 1500-2000 km (ótimo), 2400 km (máximo do salto simples), 5000 km (máximo do salto duplo) e 6900 km (máximo do salto triplo).

Nas vistas azimutais os anéis tracejados mostram os **km a partir do centro** (o passo muda com o zoom), a borda marca a distância máxima mostrada, e o cursor do mouse indica a distância e o azimute a partir do centro.

![Multi-salto](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## O que mostra

- Os **spots** dos últimos minutos (15 min - 12 h) nas bandas de **28, 50, 70 e 144 MHz**, tirados das sessões telnet. Os locators vêm do comentário do spot (p. ex. `JN70CN<ES>JO62QM`) ou do banco de dados CALL3.
- Os spots com **<MS>**, **<AU>** ou **<TR>** no texto são **descartados**: não dizem respeito à E esporádica, mas são úteis para meteor scatter, aurora ou tropo.
- **Transequatorial, regra do autor (IC8TEM)**: em direção à África e à América do Sul, se o ponto médio cair na faixa de inclinação magnética entre -20° e +20°, o contato é transequatorial (TEP) e não E esporádica, a menos que o spot declare `<ES>`.
- **144 MHz, regra do autor (IC8TEM)**: a E esporádica em 2 m começa **além de 900 km** (os spots mais curtos são descartados). Um único spot em 144 MHz **não é plotado sozinho**: aparece quando, dentro de **5 minutos**, chega um segundo spot em 144 MHz com a nuvem no mesmo lugar (pontos de reflexão dentro de 300 km), ou quando há perto um spot de **salto curto em 50 MHz** (dentro de 1200 km, nos últimos 15 minutos). A barra no topo mostra quantos spots em 144 MHz aguardam confirmação.
- Para cada spot: distância, **número de saltos** segundo a tabela de **IK3XTV**, elevação, **foEs mínima** (lei da secante).
- **Primeiro salto das duas estações**: quadrado laranja = primeiro salto do spotter, roxo = primeiro salto do DX (saltos iguais); amarelo = ponto médio de um salto simples.
- **Primeiro salto curto** (círculos vazados): nas aberturas multi-salto o autor observou um primeiro salto curto dentro de **1200 km** do seu QTH; o comprimento pode ser ajustado.
- **Nuvens Es** nos **quadrados Maidenhead** (2° x 1°), coloridas pela foEs mínima: azul < 5, azul-claro < 7, verde < 10, amarelo < 14, laranja < 20, vermelho > 20 MHz.
- Tabela **Nuvens Es**: para cada quadrado, a **MUF para a sua estação** (vermelho = 144 MHz possível, laranja = 70, amarelo = 50).

## Tabela de IK3XTV

| | mínimo | ótimo | máximo |
|---|---|---|---|
| Salto simples | 500-650 km | 1500-2000 km | 2200-2400 km |
| Salto duplo | 2800-3000 km | 3200-4000 km | 4400-5000 km |
| Salto triplo | – | 4800-6400 km | 6900 km |

## Como usar

- **Clique num spot**: o trajeto é destacado, com pontos de reflexão, locators e azimutes nos detalhes.
- **Clique num spot** na tabela: o trajeto se acende no mapa (brilho amarelo) e, se estiver fora da vista, o mapa se desloca ou amplia o raio para mostrá-lo. O spot continua selecionado quando chegam novos spots.
- **A tabela acompanha o mapa**: com um zoom definido, a tabela de spots e a de nuvens mostram só o que está visível no mapa.
- **Clique no mapa**: o ponto é tratado como uma possível nuvem Es, com a foEs necessária para 28, 50, 70 e 144 MHz.

## Nota sobre os 432 MHz

A E esporádica **não se aplica aos 432 MHz**: a MUF Es chega a cerca de 150-200 MHz e os recordes da ARRL registram contatos Es só em 50 e 144 MHz. Os spots em 70 cm não são analisados aqui.

## Quadrados coloridos por frequência e MUF piscante

Os quadrados Maidenhead dos pontos de reflexão são coloridos pela maior frequência dos spots: **verde** até 29 MHz, **amarelo** até 51, **laranja** até 71, **vermelho** até 145 MHz. A partir da foEs do quadrado estima-se a MUF provável no salto mais longo (2400 km, tabela de IK3XTV): se ela alcançar uma banda mais alta, o quadrado **pisca** na cor dessa banda (princípio de IK3XTV e Jacobs-Leinwoll: skip curto significa MUF alta).

No canto superior direito do mapa aparece o resumo N0NBH (E-skip do continente escolhido, SFI, índices K e A).
