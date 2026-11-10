# Instalación

[← Home](Home-es)

Descarga el archivo adecuado de la página **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. El programa y la carpeta `data` están siempre juntos en una carpeta tuya: **nunca se escribe ningún archivo en C:\Users\...\AppData**.

| Sistema | Archivo | Cómo instalar |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Doble clic. Se instala en `C:\E-LAYER-ANALYZER`, sin derechos de administrador. Sin acceso directo en el menú Inicio (estaría en AppData), solo el icono opcional en el escritorio. |
| Windows 32 bits | `...-win-x86-setup.exe` | Como el anterior, para Windows de 32 bits. |
| Windows portable | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Descomprime donde quieras y ejecuta `E-LAYER-ANALYZER.exe`. |
| Linux 64 bits (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, luego inícialo desde el menú: en el primer arranque el programa se copia en `~/E-LAYER-ANALYZER`, con la carpeta `data` al lado. |
| Linux portable | `...-linux-x64-portatile.tar.gz` | Descomprime y ejecuta `./E-LAYER-ANALYZER`, o ejecuta `./installa.sh`: se instala en `~/E-LAYER-ANALYZER` con un icono en el menú. |
| Raspberry Pi (64 bits) | `e-layer-analyzer_x.y.z_arm64.deb` o `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 o 5 con **Raspberry Pi OS de 64 bits** y escritorio (se recomiendan 4 GB de RAM). El .deb como arriba; el portable como en Linux. |
| Raspberry Pi (32 bits) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 o 4 con **Raspberry Pi OS de 32 bits**. Descomprime y ejecuta `./E-LAYER-ANALYZER` o `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) o `...-osx-x64.dmg` (Intel) | Abre el .dmg, clic derecho en **Installa E-LAYER-ANALYZER.command** > Abrir. Se instala en `~/E-LAYER-ANALYZER`. |
| Mac, solo la aplicación | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Descomprime **E-LAYER-ANALYZER.app** en una carpeta tuya (por ejemplo `~/E-LAYER-ANALYZER`, no en Applications): la carpeta `data` se crea junto a la app. En el primer arranque, clic derecho > Abrir. |

## Actualización

Instala la nueva versión encima de la anterior: se conservan los ajustes, los spots recogidos, los logs y `call3.txt`.

## Requisitos

- No hay que instalar .NET: el programa contiene todo lo que necesita.
- Conexión a internet para el mapa (Natural Earth, en el primer arranque), los datos de aurora de NOAA, los radares de YA-MS y las sesiones telnet.
- La Raspberry Pi Zero y la Pi 1 no son compatibles (.NET no admite sus procesadores).
