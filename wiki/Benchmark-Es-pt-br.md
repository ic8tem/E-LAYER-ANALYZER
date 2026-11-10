# BENCHMARK Es

[← Home](Home-pt-br)

![BENCHMARK Es](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1b_benchmark_es.png)

Banco de testes para a **esporádica E** e o **multi-salto** sobre um **log ADIF completo**, mesmo de outra estação: a partir dos QSO efetivamente feitos, o programa esboça o skip e as nuvens, dia a dia e hora a hora.

## Carregar o log

- **Carregar ADIF**: escolhe o arquivo `.adi`/`.adif`. O programa memoriza o arquivo e volta a carregá-lo na inicialização.
- **Remover ADIF**: retira o log da aba.
- **Estação no centro**: indicativo e locator da estação que fez o log. O programa propõe o `STATION_CALLSIGN` (ou `OPERATOR`) e o `MY_GRIDSQUARE` mais frequentes; podem ser alterados à mão e confirmados com **Aplicar**. O centro não depende das configurações gerais, por isso é possível estudar logs de outras estações.
- Posição de transmissão: o `MY_GRIDSQUARE` de cada QSO (se faltar, o locator do centro). Posições de recepção: os campos `GRIDSQUARE`.
- QSO usados: 28, 50, 70 e 144 MHz com locator. É possível excluir os QSO com outro `PROP_MODE` (TR, MS, FAI, AUR, EME...) e os inferiores a 500 km. Os 432 MHz nunca são incluídos: a esporádica E não se aplica.

## Dias e horas

- **QSO por dia**: dia, número de QSO, bandas, distância máxima, multi-saltos. Ao abrir, o programa propõe o dia com mais QSO.
- **Horas do dia**: a linha *Todo o dia* e uma linha para cada hora UTC; um clique mostra só os QSO dessa hora.
- **QSO**: a lista da seleção; clicar em um QSO (ou perto da estação DX no mapa) destaca-o e mostra os seus detalhes.

## O mapa

Mapa **azimutal equidistante** centrado na estação, com o controle **Distância máxima** para o zoom (também com a roda do mouse): os anéis indicam os **km a partir do centro**.

- **Skip por setores** (10° de azimute): arco laranja = QSO mais próximo no setor (limite da zona de skip), arco verde = QSO mais distante; entre os dois, a área alcançada.
- **Nuvens Es** nos quadrados Maidenhead, coloridas pela foEs estimada.
- Ponto médio dos saltos únicos (amarelo), primeiro salto da estação (laranja; círculo vazio = primeiro salto curto), primeiro salto da estação DX (violeta).
- Um **ponto amarelo desfocado** marca o centro de cada salto, onde se presume a nuvem Es (também o centro do primeiro salto curto, quando essa hipótese está ativa).
- Para o dia inteiro, a **linha vermelha** une hora a hora o baricentro dos pontos médios: é o deslocamento das nuvens.

![Uma hora](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1e_benchmark_es_ora.png)

## Cálculos propostos

1. **Skip por setores**: para cada setor, a distância de skip e o alcance, com os indicativos.
2. **foEs estimada** com a lei da secante (foEs ≥ f / sec φ): o salto único mais curto de cada banda dá a estimativa mais alta; com esta foEs, a MUF esperada a 1000, 1500, 2000 e 2400 km e a distância além da qual abririam os 144 MHz.
3. **Nuvens Es** estimadas nos quadrados, com o número de QSO e a foEs.
4. **Multi-salto**: o primeiro salto da estação segundo as duas hipóteses (saltos iguais, e primeiro salto curto até 1200 km, experiência de IC8TEM) comparado com as nuvens vistas em salto único em ±30 minutos; a menos de 300 km a hipótese é confirmada. O programa conta as confirmações de cada hipótese.
5. **Deslocamento das nuvens** hora a hora: km, direção e velocidade.

Os cálculos são indicativos: um QSO diz que o percurso estava aberto, não exatamente onde estava a nuvem. Quanto mais limpos forem os dados (locators de 6 caracteres, `PROP_MODE` correto), mais confiável é o skip.
