# F.A.I.

[← Home](Home-pt-br)

O F.A.I. (Field Aligned Irregularities) é a dispersão causada por irregularidades alinhadas com o campo geomagnético, típica das noites de verão depois da esporádica E. O sinal disperso sai em um **cone** cujo eixo está orientado segundo o campo: duas estações podem fazer contato se estiverem sobre **contornos suplementares** (a1 + a2 = 180°, artigo IC8FAX-IC8TEM; contornos ±c de K9SE). O campo magnético é calculado com o modelo **IGRF-14**.

## Vista Scatter trail (par A-B)

![Scatter trail](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2c_fai_scia_IC8FAX_G4LOH.png)

- Digite **Estação A** e **Estação B** (indicativo procurado no CALL3 e/ou locator). A vazio = a sua própria estação.
- O mapa mostra o **scatter trail**: todos os pontos à altitude da camada, vistos por ambas as estações, que cumprem a condição de reflexão até 2,5° (classes 0-1, 1-1,5, 1,5-2, 2-2,5°).
- O quadro no canto superior direito mostra **QTF e elevação** de A (vermelho) e B (verde) para cada ponto.
- O programa sugere o **scatter point mais provável** (desvio mínimo, percurso mais curto).
- **Refração 4/3**: ativada por padrão, amplia os horizontes.

## Vista Iso-reflexão (lugares de iso-reflexão)

![Iso-reflexão](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/2e_fai_verifica_IC8FAX_G4LOH.png)

É a construção geométrica que serve para **verificar se um ponto de reflexão está correto**.

- **Clique esquerdo** no mapa: você escolhe o ponto a verificar. **Clique direito**: desloca a estação B.
- **Verificar o ponto recomendado**: abre o gráfico no ponto sugerido pelo programa.
- Curvas de nível a **0, ±1, ±2, ±4, ±8, ±16°**: faixas coloridas nos esquemas escuros, **linhas grossas** nos esquemas claros.
- Veredicto sobre o par: **satisfatório** até 1°, **aceitável** até 2,5°, **não satisfatório** acima disso.
- Se o ponto estiver abaixo do horizonte de uma das estações, aparece a janela **Cálculo impossível** e nada mais.

## Verificações

| Contato | Scatter point | Desvio | QTF |
|---|---|---|---|
| IC8FAX - F6DRO | JN44GH (sobre Gênova) | 0,97° | 314° / 77° |
| IC8FAX - G4LOH | JN49FR (SO da Alemanha, a 28 km de Bruxelas) | 0,88° | 338° / 87° |
| Farol DB0FAI (305°) - Dresden via Sheffield (DL5MCG; Dresden apontava para 290° e declarou aurora) | – | 0,3° | – |
| DL5MCG - SV1DH (teste de 1987 via Budapeste, 1569 km) | Budapeste no Scatter trail; células abaixo de 1° a partir de 35 km | 2,16° | 95° / 341° |

Scatter points conhecidos na lista: Genebra, Bruxelas, Budapeste, JN92, JN41/42.

O menu **Verificações...** carrega os pares de teste IC8FAX-F6DRO, IC8FAX-G4LOH, IC8FAX-FA1MXE e DL5MCG-SV1DH.

**Equipamento** (DL5MCG): o F.A.I. em 144 MHz exige pelo menos uma yagi longa e 100 W (SV1DH também foi trabalhado com uma yagi de 8 elementos e 50 W). Aponte a antena para o scatter point indicado pelo programa, não para a outra estação.
