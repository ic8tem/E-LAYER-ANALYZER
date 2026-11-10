# BENCHMARK 50 MHz

[← Home](Home-pt-BR)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

Bancada de testes para a **E esporádica multi-salto em 50 MHz** usando o log ADIF de uma estação e, opcionalmente, de uma segunda estação.

## Carregamento dos logs

- **Estação 1**: *Carregar ADIF* / *Descarregar ADIF*. O programa tira `STATION_CALLSIGN` e `MY_GRIDSQUARE` do log; a posição de transmissão é o `MY_GRIDSQUARE` de cada QSO.
- **Estação 2** (opcional): os mesmos botões na segunda linha; a caixa *Estação 2* a mostra ou oculta. Os trajetos da estação 1 são azul-claros, os da estação 2 roxos.
- O programa usa os QSOs em 50 MHz com locator (locators ausentes ou de 4 caracteres são tirados do CALL3), multi-salto: além de 3100 km (regra do autor; o número de saltos segue a tabela de IK3XTV).
- Os contatos **transequatoriais** são excluídos (regra do autor): em direção à África e à América do Sul, com o ponto médio na faixa de inclinação magnética entre -20° e +20°, a menos que o log tenha `PROP_MODE=ES`.

## Dia e horário

Escolha o **dia** (ao abrir, o que tem mais QSOs multi-salto) e o horário **de… até…** UTC, em passos de 15 minutos. A caixa **foEs** (opcional) colore os trajetos pela foEs mínima e a mostra na tabela.

## O mapa

Mapa de **Mercator** com centro livre: roda = zoom, arrastar = deslocar.

![Trajeto](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **Mouse perto de um trajeto**: a linha e os ícones são ampliados e aparece o **quadrado WWL de 6 caracteres** da estação DX.
- **Clique esquerdo** nesse trajeto: um aviso pergunta se devem ser mostrados **todos os QSOs do arquivo** (todos os dias) com a posição da outra ponta **dentro de 150 km** desse ponto. *Sim*: o mapa é limpo e mostra esses QSOs dentro do círculo laranja; *Não*: o mapa fica como está. *Voltar ao dia e horário* retorna à seleção anterior.

![Arredores](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## Detalhes

Para o trajeto escolhido: distância e classificação na tabela de IK3XTV, azimute nos dois sentidos, saltos iguais com os quadrados dos pontos de reflexão, primeiro salto curto dentro de 1200 km (experiência de IC8TEM) e, com a caixa foEs, a foEs mínima.
