# DATA COLLECTION

[← Home](Home-pt)

## Sessões telnet

| Sessão | Por omissão | Notas |
|---|---|---|
| 1. DX cluster à escolha | on0dxk.dyndns.org 8000 | No ON0DXK os spots VHF estão filtrados: `set/filter dxbm/pass 6` (depois 4, 2). Outro exemplo: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW e RTTY, 7001 = FT8. |
| 3. Cluster com PSKReporter | **dm4x.ddns.net 8500** | Cluster de DM4X; em alternativa o HamAlert (hamalert.org 7300, requer conta). |

- Login automático com o indicativo (no topo ou no campo Login); **comandos iniciais** separados por `;`.
- **Voltar a ligar** após 30 s se a linha cair; **Ligar no arranque**.
- Por omissão só são guardados os spots **a partir dos 28 MHz**.

## Spots recolhidos

Os últimos 2000 spots, com filtros por banda e por fonte. Arquivo: um ficheiro CSV por dia em `data/spot`. Sempre que o programa fecha, a base de dados dos spots é esvaziada (memória e ficheiros em `data/spot`): cada sessão começa do zero.

**Filtro local** (caixa opcional, em DEFINIÇÕES): as linhas "DX de" abaixo de 27990,0 kHz ou acima de 145 MHz são descartadas, não aparecem na consola e não são guardadas; entre 430 e 440 MHz passam só os spots de aurora. Os **locators imprecisos** (menos de 6 caracteres) são obtidos do CALL3: se o CALL3 tiver um locator de 6 caracteres, este prevalece sempre. Como no YA-MS, os spots de meteor scatter (não de esporádica E) são os que estão nas janelas de frequência típicas de MS (50190-50300, 70100-70400, 144290-144399 kHz) sem etiquetas de outra propagação (`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`), e os que têm modos exclusivos de MS (MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M) ou `<MS>`. Exceção: FT8 e FT4 em 50 e 70 MHz, onde decide o modo conhecido (o FT8 de 70,154 MHz vai para a esporádica E mesmo dentro da janela MS). Os spots com `<AS>`/`<ACS>` (aircraft scatter) não são usados para a esporádica E.

## Filtros dos spots

A recolha de dados é **uma só para todo o programa**: cada spot passa pelos filtros e segue para o separador de cada modo de propagação. A partir da versão 0.4.18 os filtros, o filtro local e o teste dos filtros estão no separador **[DEFINIÇÕES](Settings-pt)**, subseparador **Encaminhamento dos spots**.

## CALL3

Base de dados **indicativo → locator** do JTDX (mais de um milhão de indicativos) em `data/call3.txt`. Botões **Recarregar** e **Importar um novo CALL3.TXT**. Os locators de 6 caracteres lidos nos comentários dos spots são acrescentados a `data/call3_nuovi.txt`.

A partir da versão 0.4.16 há uma só base CALL3 para todo o programa, a da Recolha de dados do YA-MS (separador METEOR SCATTER), com as mesmas regras do LIVETROPOVHF: atualização a partir do MMMonVHF a cada 12 horas, entradas manuais, ficheiros adicionados, correções do cluster, locators aprendidos, G7VRD a cada 30 minutos e, como última fonte, a camada JTDX (data/grid_data.bin), nunca escrita em call3.txt; as correções são escritas em data/call3.txt e os locators de data/call3_nuovi.txt são importados uma vez. Os spots do WSJT-X, JTDX e MSHV chegam também por UDP (fonte WSJT-X UDP, porta 2237 com reenvio para a 2238).

Estes filtros, que vêm do YA-MS (CW, distância mínima, janelas de frequência, modos MS, bandas), valem só para o separador METEOR SCATTER: decidem que spots entram na sua recolha. Os outros separadores recebem todos os spots e usam os seus próprios filtros.

## Subseparadores Spots e Dados de propagação

O separador tem dois subseparadores: **Spots** (sessões, PSKReporter MQTT, WSJT-X UDP, spots recolhidos e CALL3) e **Dados de propagação**:

- **N0NBH** (hamqsl.com): banner VHF e dados solares, E-skip da Europa e da América do Norte (DXrobot), aurora.
- **Bz para a aurora**: vento solar em tempo real da NOAA SWPC (DSCOVR/ACE, ponto L1), a melhor fonte para o Bz.
- **Meteor scatter**: atividade meteórica do dia (RMOB) de Make More Miles on VHF.

São descarregados no máximo uma vez por hora. A síntese aparece nos mapas de SPORADIC-E, F.A.I. e AURORA. O **menu do continente**, à direita na barra dos separadores, desloca os mapas e escolhe os dados desse continente.
