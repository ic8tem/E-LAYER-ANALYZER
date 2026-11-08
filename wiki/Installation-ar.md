# التثبيت

[← Home](Home-ar)

نزّل الملف المناسب من صفحة **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. يبقى البرنامج ومجلد `data` معاً دائماً في مجلد خاص بك: **لا يُكتب أي ملف أبداً في C:\Users\...\AppData**.

| النظام | الملف | طريقة التثبيت |
|---|---|---|
| Windows 64 بت | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | انقر نقراً مزدوجاً. يُثبَّت في `C:\E-LAYER-ANALYZER`، دون الحاجة إلى صلاحيات المسؤول. لا يوجد اختصار في قائمة ابدأ (لكان موجوداً في AppData)، بل أيقونة اختيارية على سطح المكتب فقط. |
| Windows 32 بت | `...-windows-x86-setup.exe` | كما سبق، لنظام Windows ‏32 بت. |
| Windows محمول | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | فك الضغط في أي مكان وشغّل `E-LAYER-ANALYZER.exe`. |
| Linux (حاسوب شخصي) | `...-linux-x64.tar.gz` | فك الضغط وشغّل `./installa.sh`: يُثبَّت في `~/E-LAYER-ANALYZER` مع أيقونة في القائمة. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3 أو 4 أو 5 مع **Raspberry Pi OS ‏64 بت** وسطح المكتب (يُنصح بـ 4 GB من RAM). فك الضغط وشغّل `./installa.sh`. |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) أو `...-mac-intel.dmg` | افتح ملف ‎.dmg، وانقر بزر الفأرة الأيمن على **Installa E-LAYER-ANALYZER.command** > Open. يُثبَّت في `~/E-LAYER-ANALYZER`. |

## التحديث

ثبّت الإصدار الجديد فوق القديم: تُحفظ الإعدادات والبلاغات المجمَّعة والسجلات والملف `call3.txt`.

## المتطلبات

- لا حاجة إلى تثبيت .NET: يحتوي البرنامج على كل ما يحتاجه.
- اتصال بالإنترنت للخريطة (Natural Earth، عند التشغيل الأول)، ولبيانات الشفق من NOAA، ولرادارات YA-MS، ولجلسات telnet.
- لا يُدعم Raspberry Pi Zero وPi 1 (لا يتعامل .NET مع معالجاتهما).
