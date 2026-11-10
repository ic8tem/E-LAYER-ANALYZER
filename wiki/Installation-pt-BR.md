# Instalação

[← Home](Home-pt-BR)

Baixe o arquivo certo na página **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. O programa e a pasta `data` ficam sempre juntos, numa pasta sua: **nenhum arquivo é gravado em C:\Users\...\AppData**.

| Sistema | Arquivo | Como instalar |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Clique duas vezes. Instala em `C:\E-LAYER-ANALYZER`, sem precisar de direitos de administrador. Sem atalho no menu Iniciar (ficaria em AppData), só o ícone opcional na área de trabalho. |
| Windows 32 bits | `...-win-x86-setup.exe` | Como acima, para Windows de 32 bits. |
| Windows portátil | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Extraia em qualquer lugar e execute `E-LAYER-ANALYZER.exe`. |
| Linux 64 bits (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, depois inicie pelo menu: na primeira inicialização o programa se copia para `~/E-LAYER-ANALYZER`, com a pasta `data` ao lado. |
| Linux portátil | `...-linux-x64-portatile.tar.gz` | Extraia e execute `./E-LAYER-ANALYZER`, ou execute `./installa.sh`: instala em `~/E-LAYER-ANALYZER` com um ícone no menu. |
| Raspberry Pi (64 bits) | `e-layer-analyzer_x.y.z_arm64.deb` ou `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 ou 5 com **Raspberry Pi OS de 64 bits** e desktop (4 GB de RAM recomendados). O .deb como acima; o portátil como no Linux. |
| Raspberry Pi (32 bits) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 ou 4 com **Raspberry Pi OS de 32 bits**. Extraia e execute `./E-LAYER-ANALYZER` ou `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) ou `...-osx-x64.dmg` (Intel) | Abra o .dmg, clique com o botão direito em **Installa E-LAYER-ANALYZER.command** > Abrir. Instala em `~/E-LAYER-ANALYZER`. |
| Mac, só o aplicativo | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Extraia **E-LAYER-ANALYZER.app** numa pasta sua (por exemplo `~/E-LAYER-ANALYZER`, não em Aplicativos): a pasta `data` é criada ao lado do app. Na primeira inicialização, botão direito > Abrir. |

## Atualização

Instale a nova versão por cima da antiga: configurações, spots coletados, logs e `call3.txt` são preservados.

## Requisitos

- Nenhum .NET para instalar: o programa contém tudo de que precisa.
- Conexão com a Internet para o mapa (Natural Earth, na primeira inicialização), os dados de aurora da NOAA, os radares do YA-MS e as sessões telnet.
- Raspberry Pi Zero e Pi 1 não são suportados (seus processadores não são atendidos pelo .NET).
