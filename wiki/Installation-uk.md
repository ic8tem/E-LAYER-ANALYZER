# Встановлення

[← Home](Home-uk)

Завантажте потрібний файл зі сторінки **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Програма і тека `data` завжди лишаються разом у вашій власній теці: **у C:\Users\...\AppData не записується жоден файл**.

| Система | Файл | Як встановити |
|---|---|---|
| Windows 64 біт | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | Подвійний клік. Встановлюється в `C:\E-LAYER-ANALYZER`, права адміністратора не потрібні. Ярлика в меню «Пуск» немає (він був би в AppData), лише необов'язковий значок на робочому столі. |
| Windows 32 біт | `...-win-x86-setup.exe` | Як вище, для 32-бітної Windows. |
| Windows на ARM (ПК зі Snapdragon) | `...-win-arm64-setup.exe` | Як вище, для Windows на процесорах ARM. |
| Windows портативна | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | Розпакуйте будь-куди й запустіть `E-LAYER-ANALYZER.exe`. |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, потім запустіть із меню: під час першого запуску програма копіює себе в `~/E-LAYER-ANALYZER`, з текою `data` поруч. |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (також `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (або `zypper install`), далі як із .deb. |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (або `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, далі як із .deb. |
| Slackware (x86_64 і ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (або `-aarch64-`) | Від імені root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, потім запустіть `e-layer-analyzer` з меню або з термінала: під час першого запуску програма копіюється в `~/E-LAYER-ANALYZER`. |
| Будь-який Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (або `-aarch64.AppImage`) | Зробіть файл виконуваним і запустіть: під час першого запуску він копіює програму в `~/E-LAYER-ANALYZER` і запускає її звідти. |
| Linux портативна | `...-linux-x64-portatile.tar.gz` | Розпакуйте й запустіть `./E-LAYER-ANALYZER` або запустіть `./installa.sh`: встановлюється в `~/E-LAYER-ANALYZER` зі значком у меню. |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, далі як портативна версія для Linux. |
| Raspberry Pi (64 біт) | `e-layer-analyzer_x.y.z_arm64.deb` або `...-linux-arm64-portatile.tar.gz` | Pi 3, 4 або 5 з **64-бітною Raspberry Pi OS** і робочим столом (рекомендовано 4 GB RAM). Пакет .deb — як вище; портативна версія — як для Linux. |
| Raspberry Pi (32 біт) | `e-layer-analyzer_x.y.z_armhf.deb` або `...-linux-arm-portatile.tar.gz` | Pi 2, 3 або 4 з **32-бітною Raspberry Pi OS**. Пакет .deb — як вище; портативна версія: розпакуйте й запустіть `./E-LAYER-ANALYZER` або `./installa.sh`. |
| Raspberry Pi зі Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` або `...-linux-arm64-portatile.tar.gz` | Як Slackware вище. Портативна версія — як для Linux. |
| Linux armv7l (інші 32-бітні плати ARM) | `...-linux-armv7l-portatile.tar.gz` | Як портативна версія для Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **Експериментальний, неперевірений.** Потрібні .NET 8 від спільноти (`pkg install dotnet`) і нативні бібліотеки SkiaSharp та HarfBuzzSharp, зібрані для FreeBSD, яких Avalonia не надає: без них програма не запускається. Див. `LEGGIMI-FREEBSD.txt` у пакеті. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) або `...-osx-x64.dmg` (Intel) | Відкрийте .dmg, правий клік на **Installa E-LAYER-ANALYZER.command** > Open. Встановлюється в `~/E-LAYER-ANALYZER`. |
| Mac, лише застосунок | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | Розпакуйте **E-LAYER-ANALYZER.app** у власну теку (наприклад, `~/E-LAYER-ANALYZER`, не в Applications): тека `data` створюється поруч із застосунком. Під час першого запуску правий клік > Open. |

## Оновлення

Встановіть нову версію поверх старої: налаштування, зібрані споти, журнали і `call3.txt` зберігаються.

**Системні пакети (.deb, .rpm, Arch, Slackware) не оновлюються автоматично.** З ними програма лише повідомляє про нову версію: завантажте новий пакет зі сторінки Releases і встановіть його так само, як першого разу (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`). Автоматичні оновлення працюють з інсталятором і zip для Windows, застосунком Mac, портативними пакетами tar.gz і AppImage.

**Автоматичне оновлення (починаючи з версії 0.4.5).** Під час запуску, а потім кожні 30 хвилин програма перевіряє Releases на GitHub. Коли виходить нова версія, вона відкриває вікно і показує помаранчеве **▲ x.y.z** поруч з індикатором NTP на верхній панелі:

**Починаючи з версії 0.4.12 оновлення автоматичні й тихі**: коли на GitHub є нова версія, програма завантажує її, встановлює та сама перезапускається, нічого не питаючи. Відсутні файли теж завантажуються знову без запитань. Вікно оновлення з'являється лише після клацання на версії вгорі або якщо вимкнути автоматичні оновлення в НАЛАШТУВАННЯ (Загальні).

- **Оновити зараз** завантажує портативний пакет для вашого комп'ютера (Windows 64/32 біт, Linux, Raspberry Pi, Mac), закриває програму, замінює файли і перезапускає її. Налаштування, зібрані споти, журнали і `call3.txt` зберігаються.
- **Пізніше** запитає знову під час наступної перевірки; **Пропустити цю версію** більше не пропонує цю версію.
- Клацання по версії, показаній на верхній панелі (наприклад, `v0.4.5`), одразу перевіряє наявність оновлень. Автоматичну перевірку можна вимкнути прапорцем у тому ж вікні.
- Якщо тека програми недоступна для запису (наприклад, системна тека), використайте натомість інсталяційний пакет зі сторінки релізу.

**Відсутні файли.** Під час запуску програма перевіряє, що на місці всі файли, з якими її поширювали (переклади, документи, таблиці, перелічені в `data/manifest.txt`). Якщо якогось бракує, вікно показує їх список, а **Завантажити з GitHub** завантажує їх знову з тієї самої версії на GitHub.

## Вимоги

- .NET встановлювати не потрібно: програма містить усе необхідне.
- Інтернет-з'єднання для мапи (Natural Earth, під час першого запуску), даних аврори від NOAA, радарів YA-MS і сесій telnet.
- Raspberry Pi Zero і Pi 1 не підтримуються (.NET не підтримує їхні процесори).
- Версій для Linux i686 (32-бітні ПК) і OpenBSD немає, бо для цих систем не існує .NET. Для FreeBSD є лише експериментальний пакет.
