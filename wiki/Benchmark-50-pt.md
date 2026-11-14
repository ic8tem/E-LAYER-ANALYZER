# BENCHMARK 50 MHz

[← Home](Home-pt)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Banco de ensaio para a **esporádica E multi-salto nos 50 MHz** com o log ADIF de uma estação e, opcionalmente, de uma segunda estação.

## Carregar os logs

- **Estação 1**: *Carregar ADIF* / *Remover ADIF*. O programa obtém `STATION_CALLSIGN` e `MY_GRIDSQUARE` do log; a posição de emissão é o `MY_GRIDSQUARE` de cada QSO.
- **Estação 2** (opcional): os mesmos botões na segunda linha; a caixa *Estação 2* mostra-a ou oculta-a. Os percursos da estação 1 são azul-claros, os da estação 2 violeta.
- O programa usa os QSO de 50 MHz com locator (os locators em falta ou de 4 caracteres são obtidos do CALL3), multi-salto: para lá de 3100 km (regra do autor; o número de saltos segue a tabela IK3XTV).
- Os contactos **transequatoriais** são excluídos (regra do autor): em direção a África e à América do Sul, com o ponto médio na faixa de inclinação magnética entre -15° e +15°, a menos que o log tenha `PROP_MODE=ES`.

## Dia e horário

Escolha o **dia** (ao abrir, o que tem mais QSO multi-salto) e o horário **das… às…** UTC, em passos de 15 minutos. A caixa **foEs** (opcional) colore os percursos pela foEs mínima e mostra-a na tabela.

## O mapa

Mapa **Mercator** com centro livre: roda = zoom, arrastar = deslocar.

![Percurso](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- Um **ponto amarelo desfocado** marca o centro de cada salto, onde se presume a nuvem Es; fica maior no percurso sob o rato.
- **Rato perto de um percurso**: a linha e os ícones são ampliados e aparece o **quadrado WWL de 6 caracteres** do correspondente.
- **Clique esquerdo** nesse percurso: uma janela pergunta se quer mostrar **todos os QSO do ficheiro** (todos os dias) com a extremidade distante **a menos de 150 km** desse ponto. *Sim*: o mapa é limpo e mostra esses QSO dentro do círculo laranja; *Não*: o mapa fica como está. *Voltar ao dia e horário* regressa à seleção anterior.

![Proximidades](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detalhes

Para o percurso escolhido: distância e classificação segundo a tabela IK3XTV, azimute nos dois sentidos, saltos iguais com os quadrados dos pontos de reflexão, primeiro salto curto até 1200 km (experiência de IC8TEM) e, com a caixa foEs, a foEs mínima.
