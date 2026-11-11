# Instalação

[← Home](Home-pt)

Descarregue o ficheiro certo da página **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. O programa e a pasta `data` ficam sempre juntos numa pasta sua: **nunca é escrito nenhum ficheiro em C:\Users\...\AppData**.

| Sistema | Ficheiro | Como instalar |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Duplo clique. Instala em `C:\E-LAYER-ANALYZER`, sem direitos de administrador. Sem atalho no menu Iniciar (ficaria em AppData), apenas o ícone opcional no ambiente de trabalho. |
| Windows 32 bits | `...-win-x86-setup.exe` | Como acima, para Windows de 32 bits. |
| Windows portátil | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Extraia onde quiser e execute `E-LAYER-ANALYZER.exe`. |
| Linux 64 bits (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, depois inicie-o a partir do menu: no primeiro arranque o programa copia-se para `~/E-LAYER-ANALYZER`, com a pasta `data` ao lado. |
| Linux portátil | `...-linux-x64-portatile.tar.gz` | Extraia e execute `./E-LAYER-ANALYZER`, ou execute `./installa.sh`: instala em `~/E-LAYER-ANALYZER` com um ícone no menu. |
| Raspberry Pi (64 bits) | `e-layer-analyzer_x.y.z_arm64.deb` ou `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 ou 5 com **Raspberry Pi OS de 64 bits** e ambiente de trabalho (recomendam-se 4 GB de RAM). O .deb como acima; a versão portátil como para Linux. |
| Raspberry Pi (32 bits) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 ou 4 com **Raspberry Pi OS de 32 bits**. Extraia e execute `./E-LAYER-ANALYZER` ou `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ou `...-osx-x64.dmg` (Intel) | Abra o .dmg, clique direito em **Installa E-LAYER-ANALYZER.command** > Abrir. Instala em `~/E-LAYER-ANALYZER`. |
| Mac, só a aplicação | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Extraia **E-LAYER-ANALYZER.app** para uma pasta sua (por exemplo `~/E-LAYER-ANALYZER`, não Applications): a pasta `data` é criada ao lado da aplicação. No primeiro arranque, clique direito > Abrir. |

## Atualização

Instale a nova versão por cima da anterior: as definições, os spots recolhidos, os logs e o `call3.txt` são mantidos.

**Atualização automática (a partir da versão 0.4.5).** Ao arrancar e depois a cada 6 horas o programa consulta as Releases do GitHub. Quando sai uma versão nova, abre uma janela e mostra um **▲ x.y.z** laranja ao lado do indicador NTP na barra superior:

- **Atualizar agora** descarrega o pacote portátil para o seu computador (Windows 64/32 bits, Linux, Raspberry Pi, Mac), fecha o programa, substitui os ficheiros e reinicia-o. As definições, os spots recolhidos, os logs e o `call3.txt` são mantidos.
- **Mais tarde** volta a perguntar na verificação seguinte; **Saltar esta versão** deixa de propor essa versão.
- Um clique na versão mostrada na barra superior (por exemplo `v0.4.5`) procura atualizações de imediato. A verificação automática pode ser desligada com a caixa na mesma janela.
- Se não for possível escrever na pasta do programa (por exemplo, uma pasta do sistema), use antes o pacote de instalação da página da release.

**Ficheiros em falta.** Ao arrancar, o programa verifica se estão presentes todos os ficheiros com que foi distribuído (traduções, documentos, tabelas, enumerados em `data/manifest.txt`). Se faltar algum, uma janela enumera-os e **Descarregar do GitHub** volta a obtê-los da mesma versão no GitHub.

## Requisitos

- Não é preciso instalar o .NET: o programa contém tudo aquilo de que precisa.
- Ligação à internet para o mapa (Natural Earth, no primeiro arranque), os dados de aurora da NOAA, os radares do YA-MS e as sessões telnet.
- O Raspberry Pi Zero e o Pi 1 não são suportados (os seus processadores não são suportados pelo .NET).
