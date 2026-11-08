# Instalación

[← Home](Home-es)

Descarga el archivo adecuado de la página **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. El programa y la carpeta `data` están siempre juntos en una carpeta tuya: **nunca se escribe ningún archivo en C:\Users\...\AppData**.

| Sistema | Archivo | Cómo instalar |
|---|---|---|
| Windows 64 bits | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Doble clic. Se instala en `C:\E-LAYER-ANALYZER`, sin derechos de administrador. Sin acceso directo en el menú Inicio (estaría en AppData), solo el icono opcional en el escritorio. |
| Windows 32 bits | `...-windows-x86-setup.exe` | Como el anterior, para Windows de 32 bits. |
| Windows portable | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Descomprime donde quieras y ejecuta `E-LAYER-ANALYZER.exe`. |
| Linux (PC) | `...-linux-x64.tar.gz` | Descomprime y ejecuta `./installa.sh`: se instala en `~/E-LAYER-ANALYZER` con un icono en el menú. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 o 5 con **Raspberry Pi OS de 64 bits** y escritorio (se recomiendan 4 GB de RAM). Descomprime y ejecuta `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) o `...-mac-intel.dmg` | Abre el .dmg, clic derecho en **Installa E-LAYER-ANALYZER.command** > Abrir. Se instala en `~/E-LAYER-ANALYZER`. |

## Actualización

Instala la nueva versión encima de la anterior: se conservan los ajustes, los spots recogidos, los logs y `call3.txt`.

## Requisitos

- No hay que instalar .NET: el programa contiene todo lo que necesita.
- Conexión a internet para el mapa (Natural Earth, en el primer arranque), los datos de aurora de NOAA, los radares de YA-MS y las sesiones telnet.
- La Raspberry Pi Zero y la Pi 1 no son compatibles (.NET no admite sus procesadores).
