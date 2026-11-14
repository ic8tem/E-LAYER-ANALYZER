# Установка

[← Home](Home-ru)

Скачайте подходящий файл со страницы **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Программа и папка `data` всегда находятся вместе в вашей собственной папке: **в C:\Users\...\AppData не записывается ни одного файла**.

| Система | Файл | Как установить |
|---|---|---|
| Windows 64 бит | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Двойной щелчок. Устанавливается в `C:\E-LAYER-ANALYZER` без прав администратора. Ярлыка в меню «Пуск» нет (он хранился бы в AppData), только необязательный значок на рабочем столе. |
| Windows 32 бит | `...-win-x86-setup.exe` | Так же, как выше, для 32-битной Windows. |
| Windows на ARM (ПК со Snapdragon) | `...-win-arm64-setup.exe` | Как выше, для Windows на процессорах ARM. |
| Windows, портативная версия | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Распакуйте в любое место и запустите `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, затем запустите из меню: при первом запуске программа копирует себя в `~/E-LAYER-ANALYZER`, с папкой `data` рядом. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (также `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (или `zypper install`), затем как с .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (или `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, затем как с .deb. |
| Slackware (x86_64 и ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (или `-aarch64-`) | От root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, затем запустите `e-layer-analyzer` из меню или из терминала: при первом запуске программа копирует себя в `~/E-LAYER-ANALYZER`. |
| Любой Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (или `-aarch64.AppImage`) | Сделайте файл исполняемым и запустите: при первом запуске он копирует программу в `~/E-LAYER-ANALYZER` и запускает её оттуда. |
| Linux, портативная версия | `...-linux-x64-portatile.tar.gz` | Распакуйте и запустите `./E-LAYER-ANALYZER` или запустите `./installa.sh`: программа установится в `~/E-LAYER-ANALYZER` со значком в меню. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, затем как портативная версия для Linux. |
| Raspberry Pi (64 бит) | `e-layer-analyzer_x.y.z_arm64.deb` или `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 или 5 с **64-битной Raspberry Pi OS** и рабочим столом (рекомендуется 4 GB RAM). Пакет .deb — как выше; портативная версия — как для Linux. |
| Raspberry Pi (32 бит) | `e-layer-analyzer_x.y.z_armhf.deb` или `...-linux-arm-portatile.tar.gz` | Pi 2, 3 или 4 с **32-битной Raspberry Pi OS**. Пакет .deb — как выше; портативная версия: распакуйте и запустите `./E-LAYER-ANALYZER` или `./installa.sh`. |
| Raspberry Pi со Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` или `...-linux-arm64-portatile.tar.gz` | Как Slackware выше. Портативная версия — как для Linux. |
| Linux armv7l (другие 32-битные платы ARM) | `...-linux-armv7l-portatile.tar.gz` | Как портативная версия для Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Экспериментально, не проверено.** Нужен .NET 8 от сообщества (`pkg install dotnet`) и нативные библиотеки SkiaSharp и HarfBuzzSharp, собранные для FreeBSD, которые Avalonia не предоставляет: без них программа не запускается. См. `LEGGIMI-FREEBSD.txt` в пакете. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) или `...-osx-x64.dmg` (Intel) | Откройте .dmg, щёлкните правой кнопкой по **Installa E-LAYER-ANALYZER.command** > Открыть. Устанавливается в `~/E-LAYER-ANALYZER`. |
| Mac, только приложение | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Распакуйте **E-LAYER-ANALYZER.app** в собственную папку (например, `~/E-LAYER-ANALYZER`, не в Applications): папка `data` создаётся рядом с приложением. При первом запуске щёлкните правой кнопкой > Открыть. |

## Обновление

Установите новую версию поверх предыдущей: настройки, собранные споты, логи и `call3.txt` сохранятся.

**Системные пакеты (.deb, .rpm, Arch, Slackware) не обновляются автоматически.** С ними программа только сообщает о новой версии: скачайте новый пакет со страницы Releases и установите его так же, как в первый раз (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Автоматические обновления работают с установщиком и zip для Windows, приложением для Mac, портативными пакетами tar.gz и AppImage.

**Автоматическое обновление (начиная с версии 0.4.5).** При запуске и затем каждые 30 минут программа проверяет Releases на GitHub. Когда выходит новая версия, она открывает окно и показывает оранжевое **▲ x.y.z** рядом с индикатором NTP на верхней панели:

**Начиная с версии 0.4.12 обновления автоматические и бесшумные**: когда на GitHub появляется новая версия, программа скачивает её, устанавливает и сама перезапускается, ничего не спрашивая. Недостающие файлы тоже скачиваются заново без вопросов. Окно обновления появляется только по щелчку на версии вверху или если выключить автоматические обновления в НАСТРОЙКАХ (Общие).

- **Обновить сейчас** скачивает портативный пакет для вашего компьютера (Windows 64/32 бит, Linux, Raspberry Pi, Mac), закрывает программу, заменяет файлы и перезапускает её. Настройки, собранные споты, логи и `call3.txt` сохраняются.
- **Позже** спросит снова при следующей проверке; **Пропустить эту версию** перестаёт предлагать эту версию.
- Щелчок по версии, показанной на верхней панели (например, `v0.4.5`), сразу проверяет наличие обновлений. Автоматическую проверку можно отключить флажком в том же окне.
- Если папка программы недоступна для записи (например, системная папка), используйте вместо этого установочный пакет со страницы релиза.

**Отсутствующие файлы.** При запуске программа проверяет, что на месте все файлы, с которыми она распространялась (переводы, документы, таблицы, перечисленные в `data/manifest.txt`). Если какого-то не хватает, окно выводит их список, а **Скачать с GitHub** загружает их заново из той же версии на GitHub.

## Требования

- Устанавливать .NET не нужно: программа содержит всё необходимое.
- Доступ в интернет для карты (Natural Earth, при первом запуске), данных NOAA об авроре, радаров YA-MS и сессий telnet.
- Raspberry Pi Zero и Pi 1 не поддерживаются (их процессоры не поддерживаются .NET).
- Версий для Linux i686 (32-битные ПК) и OpenBSD нет, потому что .NET для этих систем не существует. Для FreeBSD есть только экспериментальный пакет.
