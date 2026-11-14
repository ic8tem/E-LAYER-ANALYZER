# Instalação

[← Home](Home-pt)

Descarregue o ficheiro certo da página **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. O programa e a pasta `data` ficam sempre juntos numa pasta sua: **nunca é escrito nenhum ficheiro em C:\Users\...\AppData**.

| Sistema | Ficheiro | Como instalar |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Duplo clique. Instala em `C:\E-LAYER-ANALYZER`, sem direitos de administrador. Sem atalho no menu Iniciar (ficaria em AppData), apenas o ícone opcional no ambiente de trabalho. |
| Windows 32 bits | `...-win-x86-setup.exe` | Como acima, para Windows de 32 bits. |
| Windows em ARM (PC Snapdragon) | `...-win-arm64-setup.exe` | Como acima, para Windows em processadores ARM. |
| Windows portátil | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Extraia onde quiser e execute `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, depois inicie-o a partir do menu: no primeiro arranque o programa copia-se para `~/E-LAYER-ANALYZER`, com a pasta `data` ao lado. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (também `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (ou `zypper install`), depois como o .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (ou `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, depois como o .deb. |
| Slackware (x86_64 e ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (ou `-aarch64-`) | Como root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` e depois execute `e-layer-analyzer` a partir do menu ou do terminal: no primeiro arranque o programa copia-se para `~/E-LAYER-ANALYZER`. |
| Qualquer Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (ou `-aarch64.AppImage`) | Torne-o executável e execute-o: no primeiro arranque copia o programa para `~/E-LAYER-ANALYZER` e executa-o a partir daí. |
| Linux portátil | `...-linux-x64-portatile.tar.gz` | Extraia e execute `./E-LAYER-ANALYZER`, ou execute `./installa.sh`: instala em `~/E-LAYER-ANALYZER` com um ícone no menu. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, depois como a versão portátil para Linux. |
| Raspberry Pi (64 bits) | `e-layer-analyzer_x.y.z_arm64.deb` ou `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 ou 5 com **Raspberry Pi OS de 64 bits** e ambiente de trabalho (recomendam-se 4 GB de RAM). O .deb como acima; a versão portátil como para Linux. |
| Raspberry Pi (32 bits) | `e-layer-analyzer_x.y.z_armhf.deb` ou `...-linux-arm-portatile.tar.gz` | Pi 2, 3 ou 4 com **Raspberry Pi OS de 32 bits**. O .deb como acima; a versão portátil: extraia e execute `./E-LAYER-ANALYZER` ou `./installa.sh`. |
| Raspberry Pi com Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` ou `...-linux-arm64-portatile.tar.gz` | Como o Slackware acima. A versão portátil como para Linux. |
| Linux armv7l (outras placas ARM de 32 bits) | `...-linux-armv7l-portatile.tar.gz` | Como a versão portátil para Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Experimental, não verificado.** Precisa do .NET 8 da comunidade (`pkg install dotnet`) e das bibliotecas nativas SkiaSharp e HarfBuzzSharp compiladas para FreeBSD, que o Avalonia não fornece: sem elas o programa não arranca. Ver `LEGGIMI-FREEBSD.txt` no pacote. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ou `...-osx-x64.dmg` (Intel) | Abra o .dmg, clique direito em **Installa E-LAYER-ANALYZER.command** > Abrir. Instala em `~/E-LAYER-ANALYZER`. |
| Mac, só a aplicação | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Extraia **E-LAYER-ANALYZER.app** para uma pasta sua (por exemplo `~/E-LAYER-ANALYZER`, não Applications): a pasta `data` é criada ao lado da aplicação. No primeiro arranque, clique direito > Abrir. |

## Atualização

Instale a nova versão por cima da anterior: as definições, os spots recolhidos, os logs e o `call3.txt` são mantidos.

**Os pacotes de sistema (.deb, .rpm, Arch, Slackware) não se atualizam automaticamente.** Com eles o programa só assinala a versão nova: descarregue o pacote novo da página Releases e instale-o como da primeira vez (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). A atualização automática funciona com o instalador e os zip de Windows, a aplicação do Mac, os pacotes portáteis tar.gz e a AppImage.

**Atualização automática (a partir da versão 0.4.5).** Ao arrancar e depois a cada 30 minutos o programa consulta as Releases do GitHub. Quando sai uma versão nova, abre uma janela e mostra um **▲ x.y.z** laranja ao lado do indicador NTP na barra superior:

**A partir da versão 0.4.12 as atualizações são automáticas e silenciosas**: quando há uma versão nova no GitHub, o programa descarrega-a, instala-a e reinicia sozinho sem perguntar nada. Também os ficheiros em falta são descarregados de novo sem perguntar. A janela de atualização só aparece com um clique na versão em cima, ou se desligar a atualização automática em DEFINIÇÕES (Gerais).

- **Atualizar agora** descarrega o pacote portátil para o seu computador (Windows 64/32 bits, Linux, Raspberry Pi, Mac), fecha o programa, substitui os ficheiros e reinicia-o. As definições, os spots recolhidos, os logs e o `call3.txt` são mantidos.
- **Mais tarde** volta a perguntar na verificação seguinte; **Saltar esta versão** deixa de propor essa versão.
- Um clique na versão mostrada na barra superior (por exemplo `v0.4.5`) procura atualizações de imediato. A verificação automática pode ser desligada com a caixa na mesma janela.
- Se não for possível escrever na pasta do programa (por exemplo, uma pasta do sistema), use antes o pacote de instalação da página da release.

**Ficheiros em falta.** Ao arrancar, o programa verifica se estão presentes todos os ficheiros com que foi distribuído (traduções, documentos, tabelas, enumerados em `data/manifest.txt`). Se faltar algum, uma janela enumera-os e **Descarregar do GitHub** volta a obtê-los da mesma versão no GitHub.

## Requisitos

- Não é preciso instalar o .NET: o programa contém tudo aquilo de que precisa.
- Ligação à internet para o mapa (Natural Earth, no primeiro arranque), os dados de aurora da NOAA, os radares do YA-MS e as sessões telnet.
- O Raspberry Pi Zero e o Pi 1 não são suportados (os seus processadores não são suportados pelo .NET).
- Não há versões para Linux i686 (PC de 32 bits) nem para OpenBSD, porque o .NET não existe para esses sistemas. Para FreeBSD há apenas o pacote experimental.
