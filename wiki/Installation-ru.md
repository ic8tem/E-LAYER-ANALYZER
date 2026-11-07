# Установка

[← Home](Home-ru)

Скачайте подходящий файл со страницы **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Программа и папка `data` всегда находятся вместе в вашей собственной папке: **в C:\Users\...\AppData не записывается ни одного файла**.

| Система | Файл | Как установить |
|---|---|---|
| Windows 64 бит | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Двойной щелчок. Устанавливается в `C:\E-LAYER-ANALYZER` без прав администратора. Ярлыка в меню «Пуск» нет (он хранился бы в AppData), только необязательный значок на рабочем столе. |
| Windows 32 бит | `...-windows-x86-setup.exe` | Так же, как выше, для 32-битной Windows. |
| Windows, портативная версия | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Распакуйте в любое место и запустите `E-LAYER-ANALYZER.exe`. |
| Linux (ПК) | `...-linux-x64.tar.gz` | Распакуйте и запустите `./installa.sh`: программа установится в `~/E-LAYER-ANALYZER` со значком в меню. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 или 5 с **64-битной Raspberry Pi OS** и рабочим столом (рекомендуется 4 GB RAM). Распакуйте и запустите `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) или `...-mac-intel.dmg` | Откройте .dmg, щёлкните правой кнопкой по **Installa E-LAYER-ANALYZER.command** > Открыть. Устанавливается в `~/E-LAYER-ANALYZER`. |

## Обновление

Установите новую версию поверх предыдущей: настройки, собранные споты, логи и `call3.txt` сохранятся.

## Требования

- Устанавливать .NET не нужно: программа содержит всё необходимое.
- Доступ в интернет для карты (Natural Earth, при первом запуске), данных NOAA об авроре, радаров YA-MS и сессий telnet.
- Raspberry Pi Zero и Pi 1 не поддерживаются (их процессоры не поддерживаются .NET).
