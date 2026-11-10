# BENCHMARK Es

[← Home](Home-pt-BR)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Bancada de testes para a **E esporádica** e o **multi-salto** sobre um **log ADIF inteiro**, até mesmo de outra estação: a partir dos QSOs realmente feitos, o programa esboça o skip e as nuvens, dia a dia e hora a hora.

## Carregamento do log

- **Carregar ADIF**: escolhe o arquivo `.adi`/`.adif`. O programa lembra o arquivo e o recarrega na inicialização.
- **Descarregar ADIF**: remove o log da aba.
- **Estação no centro**: indicativo e locator da estação que fez o log. O programa propõe o `STATION_CALLSIGN` (ou `OPERATOR`) e o `MY_GRIDSQUARE` mais frequentes; podem ser alterados à mão e confirmados com **Aplicar**. O centro não depende das configurações gerais, por isso é possível estudar logs de outras estações.
- Posição de transmissão: o `MY_GRIDSQUARE` de cada QSO (se faltar, o locator do centro). Posições de recepção: os campos `GRIDSQUARE`.
- QSOs usados: 28, 50, 70 e 144 MHz com locator. Os QSOs com outro `PROP_MODE` (TR, MS, FAI, AUR, EME...) e os abaixo de 500 km podem ser excluídos. 432 MHz nunca é incluído: a E esporádica não se aplica.

## Dias e horas

- **QSOs por dia**: dia, número de QSOs, bandas, distância máxima, multi-saltos. Ao abrir, o programa propõe o dia com mais QSOs.
- **Horas do dia**: a linha *Dia inteiro* e uma linha para cada hora UTC; um clique mostra só os QSOs daquela hora.
- **QSO**: a lista da seleção; clicar num QSO (ou perto da estação DX no mapa) o destaca e mostra os seus detalhes.

## O mapa

Mapa **azimutal equidistante** centrado na estação, com o controle deslizante **Distância máxima** para o zoom (também com a roda do mouse): os anéis mostram os **km a partir do centro**.

- **Skip por setor** (10° de azimute): arco laranja = QSO mais próximo do setor (borda da zona de skip), arco verde = QSO mais distante, no meio a área alcançada.
- **Nuvens Es** nos quadrados Maidenhead, coloridas pela foEs estimada.
- Ponto médio dos saltos simples (amarelo), primeiro salto da estação (laranja, círculo vazado = primeiro salto curto), primeiro salto da estação DX (roxo).
- Para o dia inteiro, a **linha vermelha** une o centroide dos pontos médios hora a hora: é a deriva das nuvens.

![Uma hora](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Cálculos propostos

1. **Skip por setor**: para cada setor, a distância de skip e o alcance, com os indicativos.
2. **foEs estimada** com a lei da secante (foEs ≥ f / sec φ): o salto simples mais curto em cada banda dá a estimativa mais alta; com essa foEs, a MUF esperada a 1000, 1500, 2000 e 2400 km e a distância além da qual abriria o 144 MHz.
3. **Nuvens Es** estimadas nos quadrados, com o número de QSOs e a foEs.
4. **Multi-salto**: o primeiro salto da estação nas duas hipóteses (saltos iguais, e primeiro salto curto dentro de 1200 km, experiência de IC8TEM) comparado com as nuvens vistas em salto simples dentro de ±30 minutos; dentro de 300 km a hipótese é confirmada. O programa conta as confirmações de cada hipótese.
5. **Deriva das nuvens** hora a hora: km, direção e velocidade.

Os cálculos são indicativos: um QSO diz que o trajeto estava aberto, não exatamente onde estava a nuvem. Quanto mais limpos os dados (locators de 6 caracteres, `PROP_MODE` correto), mais confiável o skip.
