# F.A.I.

[← Home](Home-pt-BR)

F.A.I. (Field Aligned Irregularities) é o espalhamento por irregularidades alinhadas ao campo geomagnético, típico das noites de verão depois da E esporádica. O sinal espalhado sai sobre um **cone** cujo eixo está ao longo do campo: duas estações podem fazer contato se estiverem sobre **contornos suplementares** (a1 + a2 = 180°, artigo IC8FAX-IC8TEM; contornos ±c de K9SE). O campo magnético é calculado com o modelo **IGRF-14**.

## Vista Scatter trail (par A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Digite **Estação A** e **Estação B** (indicativo procurado no CALL3 e/ou locator). A deixada vazia = a sua estação.
- O mapa mostra o **scatter trail**: todos os pontos na altura da camada, vistos pelas duas estações, que satisfazem a condição de reflexão dentro de 2,5° (classes 0-1, 1-1,5, 1,5-2, 2-2,5°).
- O quadro no canto superior direito mostra **QTF e elevação** de A (vermelho) e B (verde) para cada ponto.
- O programa sugere o **ponto de espalhamento mais provável** (desvio mínimo, trajeto mais curto).
- **Refração 4/3**: ativada por padrão, estende os horizontes.

## Vista Isoflexão (lugares de isoflexão)

![Isoflexão](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

É a construção geométrica usada para **verificar se um ponto de reflexão está correto**.

- **Clique esquerdo** no mapa: escolhe o ponto a verificar. **Clique direito**: move a estação B.
- **Verificar o ponto recomendado**: abre o gráfico no ponto sugerido pelo programa.
- Curvas de nível em **0, ±1, ±2, ±4, ±8, ±16°**: faixas coloridas nos esquemas escuros, **linhas grossas** nos esquemas claros.
- Veredito sobre o par: **satisfatório** dentro de 1°, **aceitável** dentro de 2,5°, **não satisfatório** além disso.
- Se o ponto estiver abaixo do horizonte de uma estação, aparece o aviso **Cálculo impossível** e nada mais.

## Verificações

| Contato | Ponto de espalhamento | Desvio | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (sobre Gênova) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (SO da Alemanha, a 28 km de Bruxelas) | 0,88° | 338° / 87° |
| DB0FAI - Dresden via Sheffield (DL5MCG) | – | 0,3° | – |

Pontos de espalhamento conhecidos na lista: Genebra, Bruxelas, Budapeste, JN92, JN41/42.
