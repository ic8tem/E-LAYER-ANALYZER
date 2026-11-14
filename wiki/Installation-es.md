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
| Raspberry Pi (32 bits) | `e-layer-analyzer_x.y.z_armhf.deb` o `...-linux-arm-portatile.tar.gz` | Pi 2, 3 o 4 con **Raspberry Pi OS de 32 bits**. El .deb como arriba; el portable: descomprime y ejecuta `./E-LAYER-ANALYZER` o `./installa.sh`. |
| Raspberry Pi con Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` o `...-linux-arm64-portatile.tar.gz` | Como root: `installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`, luego inicia `e-layer-analyzer` desde el menú o el terminal: en el primer arranque el programa se copia en `~/E-LAYER-ANALYZER`. El portable como en Linux. |
| Linux armv7l (otras placas ARM de 32 bits) | `...-linux-armv7l-portatile.tar.gz` | Como el portable para Linux. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) o `...-osx-x64.dmg` (Intel) | Abre el .dmg, clic derecho en **Installa E-LAYER-ANALYZER.command** > Abrir. Se instala en `~/E-LAYER-ANALYZER`. |
| Mac, solo la aplicación | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Descomprime **E-LAYER-ANALYZER.app** en una carpeta tuya (por ejemplo `~/E-LAYER-ANALYZER`, no en Applications): la carpeta `data` se crea junto a la app. En el primer arranque, clic derecho > Abrir. |

## Actualización

Instala la nueva versión encima de la anterior: se conservan los ajustes, los spots recogidos, los logs y `call3.txt`.

**Actualización automática (desde la versión 0.4.5).** Al iniciar y luego cada 30 minutos el programa consulta las Releases de GitHub. Cuando sale una versión nueva, abre una ventana y muestra un **▲ x.y.z** naranja junto al indicador NTP en la barra superior:

**Desde la versión 0.4.12 las actualizaciones son automáticas y silenciosas**: cuando hay una versión nueva en GitHub, el programa la descarga, la instala y se reinicia solo sin preguntar nada. También los archivos que faltan se vuelven a descargar sin preguntar. La ventana de actualización aparece solo con un clic en la versión de arriba, o si escribes agg_automatici=0 en el archivo data/ela.ini.

- **Actualizar ahora** descarga el paquete portátil para tu ordenador (Windows de 64/32 bits, Linux, Raspberry Pi, Mac), cierra el programa, sustituye los archivos y lo reinicia. Se conservan los ajustes, los spots recogidos, los logs y `call3.txt`.
- **Más tarde** vuelve a preguntar en la siguiente comprobación; **Omitir esta versión** deja de proponer esa versión.
- Un clic en la versión que aparece en la barra superior (por ejemplo `v0.4.5`) busca actualizaciones al momento. La comprobación automática se puede desactivar con la casilla de la misma ventana.
- Si no se puede escribir en la carpeta del programa (por ejemplo, una carpeta del sistema), usa en su lugar el paquete de instalación de la página de la release.

**Archivos que faltan.** Al iniciar, el programa comprueba que estén todos los archivos con los que se distribuyó (traducciones, documentos, tablas, enumerados en `data/manifest.txt`). Si falta alguno, una ventana los enumera y **Descargar de GitHub** los vuelve a obtener de la misma versión en GitHub.

## Requisitos

- No hay que instalar .NET: el programa contiene todo lo que necesita.
- Conexión a internet para el mapa (Natural Earth, en el primer arranque), los datos de aurora de NOAA, los radares de YA-MS y las sesiones telnet.
- La Raspberry Pi Zero y la Pi 1 no son compatibles (.NET no admite sus procesadores).
- No hay versiones para Linux i686 (PC de 32 bits), porque .NET no existe para ese sistema, ni para FreeBSD, donde faltan las bibliotecas gráficas de Avalonia.
