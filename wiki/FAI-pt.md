# F.A.I.

[← Home](Home-pt)

O F.A.I. (Field Aligned Irregularities) é a dispersão causada por irregularidades alinhadas com o campo geomagnético, típica das noites de verão depois da esporádica E. O sinal disperso parte sobre um **cone** cujo eixo está orientado segundo o campo: duas estações podem comunicar se estiverem sobre **contornos suplementares** (a1 + a2 = 180°, artigo IC8FAX-IC8TEM; contornos ±c de K9SE). O campo magnético é calculado com o modelo **IGRF-14**.

## Vista Scatter trail (par A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Introduza **Estação A** e **Estação B** (indicativo procurado no CALL3 e/ou locator). A vazio = a sua própria estação.
- O mapa mostra o **scatter trail**: todos os pontos à altitude da camada, vistos por ambas as estações, que cumprem a condição de reflexão até 2,5° (classes 0-1, 1-1,5, 1,5-2, 2-2,5°).
- A caixa no canto superior direito mostra **QTF e elevação** de A (vermelho) e B (verde) para cada ponto.
- O programa sugere o **scatter point mais provável** (desvio mínimo, percurso mais curto).
- **Refração 4/3**: ativa por omissão, alarga os horizontes.

## Vista Iso-reflexão (lugares de iso-reflexão)

![Iso-reflexão](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

É a construção geométrica que serve para **verificar se um ponto de reflexão está certo**.

- **Clique esquerdo** no mapa: escolhe o ponto a verificar. **Clique direito**: desloca a estação B.
- **Verificar o ponto recomendado**: abre o gráfico no ponto sugerido pelo programa.
- Curvas de nível a **0, ±1, ±2, ±4, ±8, ±16°**: faixas coloridas nos esquemas escuros, **linhas grossas** nos esquemas claros.
- Veredicto sobre o par: **satisfatório** até 1°, **aceitável** até 2,5°, **não satisfatório** acima disso.
- Se o ponto estiver abaixo do horizonte de uma das estações, aparece a janela **Cálculo impossível** e nada mais.

## Verificações

| Contacto | Scatter point | Desvio | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (sobre Génova) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (SO da Alemanha, a 28 km de Bruxelas) | 0,88° | 338° / 87° |
| DB0FAI - Dresden via Sheffield (DL5MCG) | – | 0,3° | – |

Scatter points conhecidos na lista: Genebra, Bruxelas, Budapeste, JN92, JN41/42.
