# AURORA

[← Home](Home-pt-br)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- **Oval auroral** NOAA OVATION Prime (sombreado), **Kp** dos últimos 3 dias, **vento solar** (Bz, velocidade, densidade), atualizados a cada 5 minutos e salvos em `data/swpc`.
- As mesmas vistas do F.A.I.: **Scatter trail** para um par de estações e **Iso-reflexão** para verificar um ponto, com uma altitude padrão de 110 km.
- **Apontamento recomendado para ambas as estações**: ao longo da linha de perpendicularidade de cada estação (pontos na sua cor), o ponto com a maior probabilidade do oval. Regra prática (G3WPO): começar em cerca de 45°, entre o norte e o leste.
- Exemplo IO70VJ - JO59BC: scatter point JP01AK ao largo das Shetland, desvio 0,97°, QTF 11° e 299°.
- **Spots de aurora no mapa**: os spots com **<AU>** ou **<AUR>** no texto são spots de aurora (regra do autor). Com a caixa *Spots <AU>/<AUR> no mapa com o cálculo* marcada, a aba desenha os spots das últimas 2 horas (no máximo 30): para cada par spotter-DX calcula o scatter trail e mostra o scatter point mais provável (círculo laranja), com o QTF e a elevação de ambas as estações nos resultados.
- Além do scatter point mais provável, os resultados indicam o **intervalo de QTF** de ambas as estações ao longo de todo o Scatter trail (desvio ≤ 1,5°).

## BENCHMARK AURORA

A aba AURORA tem duas subabas: **REAL-TIME** (o cálculo descrito acima) e **BENCHMARK AURORA**, um banco de testes do modelo sobre **298 QSO reais de aurora** com hora e locator: os logs publicados de EI5FK (IO51RT, Irlanda, 2003-2005, 144 e 432 MHz) e o mapa do log de PA0O (JO33HG, Países Baixos).

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- Para cada QSO o programa calcula o scatter point com o mesmo modelo (IGRF-14, altitude 110 km, refração 4/3, tolerância 3°).
- Cores: **verde** = scatter point a 370-1100 km de ambas as estações (Lange-Hesse); **amarelo** = explicado, mas fora dessa janela; **laranja** = QSO além de 2000 km; **vermelho** = nenhum scatter point, QSO não explicado pelo modelo.
- O resumo dá as porcentagens e, para cada log, o intervalo e a mediana do QTF calculado. Clique em uma linha para desenhar o percurso e os dois apontamentos.
- Resultado (versão 0.4.4): **296 de 298 QSO explicados (99%)**, 243 (82%) dentro da janela de 370-1100 km; QTF mediano de EI5FK entre 25° e 41°, ou seja, o nordeste sugerido por G3WPO. Os logs não informam o apontamento: a verificação é feita sobre a geometria, não sobre o QTF.
