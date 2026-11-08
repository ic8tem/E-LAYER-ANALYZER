# SPORADIC-E

[← Home](Home-pt)

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## O mapa

Um botão por cima do mapa escolhe a vista:

- **Azimutal - salto único**: mapa azimutal equidistante centrado na sua estação, raio fixo de 2500 km, com as zonas da tabela IK3XTV (500 km mínimo, 1500-2000 km ótimo, 2400 km máximo do salto único).
- **Azimutal - distância máxima (multi-salto)**: o mesmo mapa com o controlo **Distância máxima** (de 300 a 20000 km, também com a roda do rato); aparecem ainda os limites do salto duplo (5000 km) e triplo (6900 km), para se poder acompanhar a evolução do multi-salto.
- **Retangular**: o mapa-múndi anterior, com os botões Europa e Mundo.

Nas vistas azimutais os anéis tracejados indicam os **km a partir do centro** (o intervalo muda com o zoom), o bordo marca a distância máxima mostrada e o cursor do rato indica a distância e o azimute a partir do centro.

![Multi-salto](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1c_sporadic_e_multisalto.png)

## O que mostra

- Os **spots** dos últimos minutos (15 min - 12 h) nas bandas de **28, 50, 70 e 144 MHz**, obtidos das sessões telnet. Os locators vêm do comentário do spot (p. ex. `JN70CN<ES>JO62QM`) ou da base de dados CALL3.
- Os spots com **<MS>**, **<AU>** ou **<TR>** no texto são **descartados**: não dizem respeito à esporádica E, mas são úteis para meteor scatter, aurora ou tropo.
- **Transequatorial, regra do autor (IC8TEM)**: em direção a África e à América do Sul, se o ponto médio cair na faixa de inclinação magnética entre -20° e +20°, o contacto é transequatorial (TEP) e não esporádica E, a menos que o spot indique `<ES>`.
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
