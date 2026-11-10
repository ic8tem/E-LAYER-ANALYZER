# Установка

[← Home](Home-ru)

Скачайте подходящий файл со страницы **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Программа и папка `data` всегда находятся вместе в вашей собственной папке: **в C:\Users\...\AppData не записывается ни одного файла**.

| Система | Файл | Как установить |
|---|---|---|
| Windows 64 бит | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Двойной щелчок. Устанавливается в `C:\E-LAYER-ANALYZER` без прав администратора. Ярлыка в меню «Пуск» нет (он хранился бы в AppData), только необязательный значок на рабочем столе. |
| Windows 32 бит | `...-win-x86-setup.exe` | Так же, как выше, для 32-битной Windows. |
| Windows, портативная версия | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | Распакуйте в любое место и запустите `E-LAYER-ANALYZER.exe`. |
| Linux 64 бит (ПК) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, затем запустите из меню: при первом запуске программа копирует себя в `~/E-LAYER-ANALYZER`, с папкой `data` рядом. |
| Linux, портативная версия | `...-linux-x64-portatile.tar.gz` | Распакуйте и запустите `./E-LAYER-ANALYZER` или запустите `./installa.sh`: программа установится в `~/E-LAYER-ANALYZER` со значком в меню. |
| Raspberry Pi (64 бит) | `e-layer-analyzer_x.y.z_arm64.deb` или `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 или 5 с **64-битной Raspberry Pi OS** и рабочим столом (рекомендуется 4 GB RAM). Пакет .deb — как выше; портативная версия — как для Linux. |
| Raspberry Pi (32 бит) | `...-linux-arm-portatile.tar.gz` | Pi 2, 3 или 4 с **32-битной Raspberry Pi OS**. Распакуйте и запустите `./E-LAYER-ANALYZER` или `./installa.sh`. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) или `...-osx-x64.dmg` (Intel) | Откройте .dmg, щёлкните правой кнопкой по **Installa E-LAYER-ANALYZER.command** > Открыть. Устанавливается в `~/E-LAYER-ANALYZER`. |
| Mac, только приложение | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Распакуйте **E-LAYER-ANALYZER.app** в собственную папку (например, `~/E-LAYER-ANALYZER`, не в Applications): папка `data` создаётся рядом с приложением. При первом запуске щёлкните правой кнопкой > Открыть. |

## Обновление

Установите новую версию поверх предыдущей: настройки, собранные споты, логи и `call3.txt` сохранятся.

## Требования

- Устанавливать .NET не нужно: программа содержит всё необходимое.
- Доступ в интернет для карты (Natural Earth, при первом запуске), данных NOAA об авроре, радаров YA-MS и сессий telnet.
- Raspberry Pi Zero и Pi 1 не поддерживаются (их процессоры не поддерживаются .NET).
