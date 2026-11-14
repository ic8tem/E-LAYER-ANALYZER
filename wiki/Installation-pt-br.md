# Instalação

[← Home](Home-pt-br)

Baixe o arquivo certo na página **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. O programa e a pasta `data` ficam sempre juntos em uma pasta sua: **nenhum arquivo é gravado em C:\Users\...\AppData**.

| Sistema | Arquivo | Como instalar |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Clique duplo. Instala em `C:\E-LAYER-ANALYZER`, sem precisar de direitos de administrador. Sem atalho no menu Iniciar (ficaria em AppData), apenas o ícone opcional na área de trabalho. |
| Windows 32 bits | `...-win-x86-setup.exe` | Como acima, para Windows de 32 bits. |
| Windows em ARM (PCs Snapdragon) | `...-win-arm64-setup.exe` | Como acima, para Windows em processadores ARM. |
| Windows portátil | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Extraia onde quiser e execute `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, depois inicie-o a partir do menu: na primeira execução o programa copia-se para `~/E-LAYER-ANALYZER`, com a pasta `data` ao lado. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (também `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (ou `zypper install`), depois como o .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (ou `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, depois como o .deb. |
| Slackware (x86_64 e ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (ou `-aarch64-`) | Como root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` e depois execute `e-layer-analyzer` a partir do menu ou do terminal: na primeira execução o programa copia-se para `~/E-LAYER-ANALYZER`. |
| Qualquer Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (ou `-aarch64.AppImage`) | Torne-o executável e execute-o: na primeira execução copia o programa para `~/E-LAYER-ANALYZER` e executa-o a partir daí. |
| Linux portátil | `...-linux-x64-portatile.tar.gz` | Extraia e execute `./E-LAYER-ANALYZER`, ou execute `./installa.sh`: instala em `~/E-LAYER-ANALYZER` com um ícone no menu. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, depois como a versão portátil para Linux. |
| Raspberry Pi (64 bits) | `e-layer-analyzer_x.y.z_arm64.deb` ou `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 ou 5 com **Raspberry Pi OS de 64 bits** e ambiente de área de trabalho (recomendam-se 4 GB de RAM). O .deb como acima; a versão portátil como no Linux. |
| Raspberry Pi (32 bits) | `e-layer-analyzer_x.y.z_armhf.deb` ou `...-linux-arm-portatile.tar.gz` | Pi 2, 3 ou 4 com **Raspberry Pi OS de 32 bits**. O .deb como acima; a versão portátil: extraia e execute `./E-LAYER-ANALYZER` ou `./installa.sh`. |
| Raspberry Pi com Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` ou `...-linux-arm64-portatile.tar.gz` | Como o Slackware acima. A versão portátil como para Linux. |
| Linux armv7l (outras placas ARM de 32 bits) | `...-linux-armv7l-portatile.tar.gz` | Como a versão portátil para Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Experimental, não verificado.** Precisa do .NET 8 da comunidade (`pkg install dotnet`) e das bibliotecas nativas SkiaSharp e HarfBuzzSharp compiladas para FreeBSD, que o Avalonia não fornece: sem elas o programa não inicia. Ver `LEGGIMI-FREEBSD.txt` no pacote. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ou `...-osx-x64.dmg` (Intel) | Abra o .dmg, clique com o botão direito em **Installa E-LAYER-ANALYZER.command** > Abrir. Instala em `~/E-LAYER-ANALYZER`. |
| Mac, só o aplicativo | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Extraia **E-LAYER-ANALYZER.app** para uma pasta sua (por exemplo `~/E-LAYER-ANALYZER`, não Applications): a pasta `data` é criada ao lado do aplicativo. Na primeira inicialização, clique com o botão direito > Abrir. |

## Atualização

Instale a nova versão por cima da anterior: as configurações, os spots coletados, os logs e o `call3.txt` são mantidos.

**Os pacotes de sistema (.deb, .rpm, Arch, Slackware) não se atualizam automaticamente.** Com eles o programa só assinala a versão nova: baixe o pacote novo da página Releases e instale-o como da primeira vez (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). A atualização automática funciona com o instalador e os zips do Windows, o aplicativo do Mac, os pacotes portáteis tar.gz e a AppImage.

**Atualização automática (a partir da versão 0.4.5).** Ao iniciar e depois a cada 6 horas o programa consulta as Releases do GitHub. Quando sai uma versão nova, ele abre uma janela e mostra um **▲ x.y.z** laranja ao lado do indicador NTP na barra superior:

**A partir da versão 0.4.12 as atualizações são automáticas e silenciosas**: quando há uma versão nova no GitHub, o programa baixa, instala e reinicia sozinho sem perguntar nada. Os arquivos que faltam também são baixados de novo sem perguntar. A janela de atualização só aparece com um clique na versão no topo, ou se você desligar a atualização automática em CONFIGURAÇÕES (Gerais).

- **Atualizar agora** baixa o pacote portátil para o seu computador (Windows 64/32 bits, Linux, Raspberry Pi, Mac), fecha o programa, substitui os arquivos e o reinicia. As configurações, os spots coletados, os logs e o `call3.txt` são mantidos.
- **Mais tarde** pergunta de novo na próxima verificação; **Pular esta versão** deixa de propor essa versão.
- Clicar na versão mostrada na barra superior (por exemplo `v0.4.5`) verifica atualizações na hora. A verificação automática pode ser desativada com a caixa na mesma janela.
- Se a pasta do programa não permitir gravação (por exemplo, uma pasta do sistema), use o pacote de instalação da página da release.

**Arquivos ausentes.** Ao iniciar, o programa verifica se estão presentes todos os arquivos com que foi distribuído (traduções, documentos, tabelas, listados em `data/manifest.txt`). Se faltar algum, uma janela os lista e **Baixar do GitHub** os obtém novamente da mesma versão no GitHub.

## Requisitos

- Não é preciso instalar o .NET: o programa contém tudo aquilo de que precisa.
- Conexão com a internet para o mapa (Natural Earth, na primeira inicialização), os dados de aurora da NOAA, os radares do YA-MS e as sessões telnet.
- O Raspberry Pi Zero e o Pi 1 não são suportados (os seus processadores não são suportados pelo .NET).
- Não há versões para Linux i686 (PC de 32 bits) nem para OpenBSD, porque o .NET não existe para esses sistemas. Para FreeBSD há apenas o pacote experimental.
