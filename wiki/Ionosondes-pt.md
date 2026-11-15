# IONOSSONDAS

[← Home](Home-pt)

![IONOSSONDAS](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

Subseparador de **SPORADIC-E** (a partir da versão 0.5.2): a **foEs medida** pelas ionossondas da rede **GIRO** em todo o mundo, ao lado da que o programa obtém a partir dos spots.

## Os dados

- 63 ionossondas GIRO (lista ULCAR), com a última **foEs**, **foF2** e **MUF(D)** lidas automaticamente dos ionogramas.
- Os dados chegam na primeira vez que se abre o separador e depois **a cada 15 minutos**, sozinhos (caixa *Atualizar sozinho a cada 15 minutos*); *Atualizar agora* pede-os de imediato. O arquivo guarda as últimas 48 horas na pasta `giro` dos dados.
- Dados da **GIRO** (Reinisch e Galkin 2011), licença **CC BY-NC-SA 4.0**: apenas uso não comercial. O serviço é o mesmo do *MUF Muncher* de DM6AX.

## Mapa e tabela

- Cada ionossonda é um ponto colorido com a última foEs (mesmas cores das nuvens Es) e o valor em MHz; **cinzenta** se não tiver dados recentes, com o **anel** se o último ionograma tiver mais de 90 minutos.
- Tabela: Código, Ionossonda, km a partir da sua estação, UTC do último ionograma, foEs, foF2, MUF(D) e **Spots foEs≥**: a foEs mínima obtida a partir dos spots Es dos últimos 30 minutos que refletem a menos de 500 km da ionossonda (ajustável), com o número de spots.

## A ionossonda escolhida

Clique numa linha ou perto de uma ionossonda no mapa:

- **gráfico das últimas 24 horas**: foEs amarela, foF2 azul-clara, MUF(D) violeta pontilhada;
- **MUF da esporádica E** a 1000, 1500, 2000 e 2400 km (MUF = foEs · sec φ, à altitude Es escolhida) com as **bandas que se abrem** (28, 40, 50, 70, 144 MHz);
- se os spots pedirem mais do que a foEs medida, o programa assinala-o: a nuvem pode estar longe da ionossonda, ou então a ionossonda chegou ao limite da sua gama de frequências (Chartier 2022).
