# Instalação

[← Home](Home-pt)

Descarregue o ficheiro certo da página **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. O programa e a pasta `data` ficam sempre juntos numa pasta sua: **nunca é escrito nenhum ficheiro em C:\Users\...\AppData**.

| Sistema | Ficheiro | Como instalar |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Duplo clique. Instala em `C:\E-LAYER-ANALYZER`, sem direitos de administrador. Sem atalho no menu Iniciar (ficaria em AppData), apenas o ícone opcional no ambiente de trabalho. |
| Windows 32 bits | `...-windows-x86-setup.exe` | Como acima, para Windows de 32 bits. |
| Windows portátil | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Extraia onde quiser e execute `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Extraia e execute `./installa.sh`: instala em `~/E-LAYER-ANALYZER` com um ícone no menu. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 ou 5 com **Raspberry Pi OS de 64 bits** e ambiente de trabalho (recomendam-se 4 GB de RAM). Extraia e execute `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) ou `...-mac-intel.dmg` | Abra o .dmg, clique direito em **Installa E-LAYER-ANALYZER.command** > Abrir. Instala em `~/E-LAYER-ANALYZER`. |

## Atualização

Instale a nova versão por cima da anterior: as definições, os spots recolhidos, os logs e o `call3.txt` são mantidos.

## Requisitos

- Não é preciso instalar o .NET: o programa contém tudo aquilo de que precisa.
- Ligação à internet para o mapa (Natural Earth, no primeiro arranque), os dados de aurora da NOAA, os radares do YA-MS e as sessões telnet.
- O Raspberry Pi Zero e o Pi 1 não são suportados (os seus processadores não são suportados pelo .NET).
