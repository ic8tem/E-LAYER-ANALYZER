# DATA COLLECTION

[← Home](Home-pt-BR)

## Sessões telnet

| Sessão | Padrão | Notas |
|---|---|---|
| 1. DX cluster à sua escolha | on0dxk.dyndns.org 8000 | No ON0DXK os spots VHF são filtrados: `set/filter dxbm/pass 6` (depois 4, 2). Outro exemplo: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW e RTTY, 7001 = FT8. |
| 3. Cluster com PSKReporter | **dm4x.ddns.net 8500** | O cluster de DM4X; como alternativa HamAlert (hamalert.org 7300, conta necessária). |

- Login automático com o indicativo (no topo ou no campo Login); **comandos iniciais** separados por `;`.
- **Reconectar** depois de 30 s se a linha cair; **Conectar na inicialização**.
- Por padrão só são salvos os spots **a partir de 28 MHz**.

## Spots coletados

Os últimos 2000 spots, com filtros por banda e por fonte. Arquivo: um arquivo CSV por dia em `data/spot`. Toda vez que o programa é fechado, o banco de dados dos spots é esvaziado (memória e os arquivos em `data/spot`): cada sessão começa do zero.

**Filtro local** (caixa opcional): as linhas "DX de" abaixo de 27990,0 kHz ou acima de 145 MHz são descartadas, não mostradas no console e não armazenadas. Os **locators imprecisos** (menos de 6 caracteres) são tirados do CALL3: se o CALL3 tiver um locator de 6 caracteres, ele sempre prevalece. Como no YA-MS, os spots de meteor scatter (não de E esporádica) são os que estão nas janelas de frequência típicas de MS (50190-50300, 70100-70400, 144290-144399 kHz) sem tags de outra propagação (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), e os com modos exclusivamente MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) ou com `<MS>`. Os spots com `<AS>`/`<ACS>` (aircraft scatter) não são usados para a E esporádica.

## Coluna de filtros (40% à direita)

A coleta de dados é **uma só para todo o programa**: cada spot passa por esses filtros e segue para a aba de cada modo de propagação (dentro do E-LAYER ANALYZER a aba Coleta de dados do YA-MS não é mostrada). No topo a caixa do **filtro local**; abaixo, cinco subabas:

| Subaba | O que se configura | Padrões (regras do autor) |
|---|---|---|
| Meteor scatter | o filtro do YA-MS: janelas de frequência MS, tags de outra propagação, modos MS e `<MS>`, bandas | como no YA-MS |
| E esporádica | tags `< >` que excluem o spot, regra dos 144 MHz, regra transequatorial | `<MS> <AU> <TR> <AS> <ACS>`…; 900 km, 5 min, 300 km, salto curto 50 MHz 1200 km/15 min; ±20° |
| F.A.I. | palavras F.A.I., bandas, período no mapa | FAI, todas as bandas, 24 horas |
| Aurora | tags `< >` de aurora, bandas, período no mapa | `AU` `AUR` (com ou sem `< >`), todas as bandas, 2 horas |
| Teste dos filtros | cole uma linha do cluster e veja como todos os filtros a classificam | |

O botão **Restaurar as regras do autor** traz de volta os padrões.

## CALL3

Banco de dados **indicativo → locator** do JTDX (mais de um milhão de indicativos) em `data/call3.txt`. Botões **Recarregar** e **Importar um novo CALL3.TXT**. Os locators de 6 caracteres lidos nos comentários dos spots são adicionados a `data/call3_nuovi.txt`.

## Subabas Spot e Dados de propagação

A aba tem duas subabas: **Spot** (sessões, spots coletados, CALL3 e coluna de filtros) e **Dados de propagação**:

- **N0NBH** (hamqsl.com): banner VHF e dados solares, E-skip Europa e América do Norte (DXrobot), aurora.
- **Bz para a aurora**: vento solar em tempo real da NOAA SWPC (DSCOVR/ACE, ponto L1), a melhor fonte para o Bz.
- **Meteor scatter**: atividade meteórica diária (RMOB) de Make More Miles on VHF.

São baixados no máximo uma vez por hora. O resumo aparece nos mapas de SPORADIC-E, F.A.I. e AURORA. O **menu de continentes**, à direita da barra de abas, move os mapas e seleciona os dados daquele continente.
