# Встановлення

[← Home](Home-uk)

Завантажте потрібний файл зі сторінки **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. Програма і тека `data` завжди лишаються разом у вашій власній теці: **у C:\Users\...\AppData не записується жоден файл**.

| Система | Файл | Як встановити |
|---|---|---|
| Windows 64 біт | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | Подвійний клік. Встановлюється в `C:\E-LAYER-ANALYZER`, права адміністратора не потрібні. Ярлика в меню «Пуск» немає (він був би в AppData), лише необов'язковий значок на робочому столі. |
| Windows 32 біт | `...-windows-x86-setup.exe` | Як вище, для 32-бітної Windows. |
| Windows портативна | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | Розпакуйте будь-куди й запустіть `E-LAYER-ANALYZER.exe`. |
| Linux (ПК) | `...-linux-x64.tar.gz` | Розпакуйте й запустіть `./installa.sh`: встановлюється в `~/E-LAYER-ANALYZER` зі значком у меню. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3, 4 або 5 з **64-бітною Raspberry Pi OS** і робочим столом (рекомендовано 4 GB RAM). Розпакуйте й запустіть `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) або `...-mac-intel.dmg` | Відкрийте .dmg, правий клік на **Installa E-LAYER-ANALYZER.command** > Open. Встановлюється в `~/E-LAYER-ANALYZER`. |

## Оновлення

Встановіть нову версію поверх старої: налаштування, зібрані споти, журнали і `call3.txt` зберігаються.

## Вимоги

- .NET встановлювати не потрібно: програма містить усе необхідне.
- Інтернет-з'єднання для мапи (Natural Earth, під час першого запуску), даних аврори від NOAA, радарів YA-MS і сесій telnet.
- Raspberry Pi Zero і Pi 1 не підтримуються (.NET не підтримує їхні процесори).
