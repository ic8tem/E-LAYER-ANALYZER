# NUVENS Es NO TEMPO

[← Home](Home-pt-br)

![NUVENS Es NO TEMPO](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1l_nubi_es_nel_tempo.png)

Subaba de **SPORADIC-E** (a partir da versão 0.5.2): como as **nuvens de esporádica E** nascem, se deslocam e se apagam, passo a passo.

## O método

Método de **Chartier, Hanley e Emmons (2022)**: cada enlace de esporádica E diz que no ponto de reflexão a foEs é **pelo menos f / sec φ**. Os pontos são reunidos em células e em passos de tempo, e cada célula fica com o **valor mais alto** do passo. A densidade eletrônica mínima é NmE = 1,24·10¹⁰ · foEs² elétrons por metro cúbico.

O programa usa os pontos de reflexão do seu próprio modelo (todos os saltos), não só o ponto médio. O valor é um **limite inferior**: uma célula vazia significa apenas nenhum enlace.

## Origem e comandos

- **Spots da sessão** (são atualizados a cada abertura da aba) ou então **Carregar log ADIF ou spots CSV**.
- **Exportar os spots da sessão (CSV)**: o arquivo dos spots é esvaziado quando o programa fecha, e assim é possível revê-los mais tarde.
- **Bandas MHz** (28, 40, 50, 70, 144), **Passo** (5, 10 ou 15 minutos), **Célula** (1°, 2° ou 4°), **Altitude Es km**.
- Barra do tempo: ⏮ ◀ *Iniciar/Parar* ▶ ⏭ e o cursor; na abertura começa-se pelo passo com mais células.
- *Passos anteriores esmaecidos* mostra os três passos antes do escolhido; *Enlaces* desenha os percursos com as cores das bandas.

## Tabela e gráfico

- Células do passo: Célula (canto sudoeste e locator do centro), foEs≥, NmE≥, Enlaces, Bandas. Clique em uma célula: destaca os seus enlaces.
- Gráfico: foEs mínima mais alta (linha amarela) e número de células (barras) para cada passo.
