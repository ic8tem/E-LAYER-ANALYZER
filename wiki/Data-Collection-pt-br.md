# DATA COLLECTION

[← Home](Home-pt-br)

## Sessões telnet

| Sessão | Padrão | Notas |
|---|---|---|
| 1. DX cluster à escolha | on0dxk.dyndns.org 8000 | No ON0DXK os spots VHF estão filtrados: `set/filter dxbm/pass 6` (depois 4, 2). Outro exemplo: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW e RTTY, 7001 = FT8. |
| 3. Cluster com PSKReporter | **dm4x.ddns.net 8500** | Cluster de DM4X; como alternativa, o HamAlert (hamalert.org 7300, requer conta). |

- Login automático com o indicativo (no topo ou no campo Login); **comandos iniciais** separados por `;`.
- **Reconectar** após 30 s se a linha cair; **Conectar na inicialização**.
- Por padrão só são salvos os spots **a partir de 28 MHz**.

## Spots coletados

Os últimos 2000 spots, com filtros por banda e por fonte. Arquivo: um arquivo CSV por dia em `data/spot`. Sempre que o programa é fechado, a base de dados dos spots é esvaziada (memória e arquivos em `data/spot`): cada sessão começa do zero.

**Filtro local** (caixa opcional): as linhas "DX de" abaixo de 27990,0 kHz ou acima de 145 MHz são descartadas, não aparecem no console e não são salvas. Os **locators imprecisos** (menos de 6 caracteres) são obtidos do CALL3: se o CALL3 tiver um locator de 6 caracteres, este prevalece sempre. Como no YA-MS, os spots de meteor scatter (e não de esporádica E) são os que estão nas janelas de frequência típicas do MS (50190-50300, 70100-70400, 144290-144399 kHz) sem etiquetas de outra propagação (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), e os que têm modos exclusivos de MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) ou `<MS>`. Os spots com `<AS>`/`<ACS>` (aircraft scatter) não são usados para a esporádica E.

## Coluna dos filtros (40% à direita)

A coleta de dados é **uma só para todo o programa**: cada spot passa por estes filtros e segue para a aba de cada modo de propagação (dentro do E-LAYER ANALYZER a aba Coleta de dados do YA-MS não é mostrada). No topo, a caixa do **filtro local**; abaixo, cinco subabas:

| Subaba | O que se configura | Padrão (regras do autor) |
|---|---|---|
| Meteor scatter | o filtro do YA-MS: janelas de frequência MS, etiquetas de outra propagação, modos MS e `<MS>`, bandas | como no YA-MS |
| Esporádica E | etiquetas `< >` que excluem o spot, regra dos 144 MHz, regra transequatorial | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, salto curto em 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | palavras F.A.I., bandas, período no mapa | FAI, todas as bandas, 24 horas |
| Aurora | etiquetas `< >` de aurora, bandas, período no mapa | `AU` `AUR` (com ou sem `< >`), todas as bandas, 2 horas |
| Teste dos filtros | cole uma linha do cluster e veja como todos os filtros a classificam | |

O botão **Restaurar as regras do autor** restaura os valores padrão.

## CALL3

Base de dados **indicativo → locator** do JTDX (mais de um milhão de indicativos) em `data/call3.txt`. Botões **Recarregar** e **Importar um novo CALL3.TXT**. Os locators de 6 caracteres lidos nos comentários dos spots são acrescentados a `data/call3_nuovi.txt`.

A partir da versão 0.4.16 há uma única base CALL3 para todo o programa, a da Coleta de dados do YA-MS (aba METEOR SCATTER), com as mesmas regras do LIVETROPOVHF: atualização a partir do MMMonVHF, entradas manuais, arquivos adicionados, correções do cluster, locators aprendidos e G7VRD a cada 30 minutos; as correções são gravadas em data/call3.txt e os locators de data/call3_nuovi.txt são importados uma vez. Os spots do WSJT-X, JTDX e MSHV chegam também por UDP (fonte WSJT-X UDP, porta 2237 com reenvio para a 2238).

Estes filtros, que vêm do YA-MS (CW, distância mínima, janelas de frequência, modos MS, bandas), valem só para a aba METEOR SCATTER: decidem quais spots entram na sua coleta. As outras abas recebem todos os spots e usam seus próprios filtros.

## Subabas Spots e Dados de propagação

A aba tem duas subabas: **Spots** (sessões, spots coletados, CALL3 e coluna dos filtros) e **Dados de propagação**:

- **N0NBH** (hamqsl.com): banner VHF e dados solares, E-skip da Europa e da América do Norte (DXrobot), aurora.
- **Bz para a aurora**: vento solar em tempo real da NOAA SWPC (DSCOVR/ACE, ponto L1), a melhor fonte para o Bz.
- **Meteor scatter**: atividade meteórica do dia (RMOB) de Make More Miles on VHF.

São baixados no máximo uma vez por hora. A síntese aparece nos mapas de SPORADIC-E, F.A.I. e AURORA. O **menu do continente**, à direita na barra de abas, desloca os mapas e escolhe os dados desse continente.
