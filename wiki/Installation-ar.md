# التثبيت

[← Home](Home-ar)

نزّل الملف المناسب من صفحة **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)**. يبقى البرنامج ومجلد `data` معاً دائماً في مجلد خاص بك: **لا يُكتب أي ملف أبداً في C:\Users\...\AppData**.

| النظام | الملف | طريقة التثبيت |
|---|---|---|
| Windows 64 بت | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | انقر نقراً مزدوجاً. يُثبَّت في `C:\E-LAYER-ANALYZER`، دون الحاجة إلى صلاحيات المسؤول. لا يوجد اختصار في قائمة ابدأ (لكان موجوداً في AppData)، بل أيقونة اختيارية على سطح المكتب فقط. |
| Windows 32 بت | `...-win-x86-setup.exe` | كما سبق، لنظام Windows ‏32 بت. |
| Windows على ARM ‏(حواسيب Snapdragon) | `...-win-arm64-setup.exe` | كما سبق، لنظام Windows على معالجات ARM. |
| Windows محمول | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | فك الضغط في أي مكان وشغّل `E-LAYER-ANALYZER.exe`. |
| Debian وUbuntu وMint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`، ثم شغّله من القائمة: عند التشغيل الأول ينسخ البرنامج نفسه إلى `~/E-LAYER-ANALYZER`، مع مجلد `data` بجانبه. |
| Fedora وopenSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (وأيضاً `.aarch64.rpm` و`.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (أو `zypper install`)، ثم كما مع .deb. |
| Arch وManjaro وEndeavourOS وArch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (أو `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`، ثم كما مع .deb. |
| Slackware ‏(x86_64 وARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (أو `-aarch64-`) | بصلاحيات root: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`، ثم شغّل `e-layer-analyzer` من القائمة أو من الطرفية: في التشغيل الأول ينسخ البرنامج نفسه إلى `~/E-LAYER-ANALYZER`. |
| أي توزيعة Linux ‏(AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (أو `-aarch64.AppImage`) | اجعله قابلاً للتنفيذ وشغّله: في التشغيل الأول ينسخ البرنامج إلى `~/E-LAYER-ANALYZER` ويشغّله من هناك. |
| Linux محمول | `...-linux-x64-portatile.tar.gz` | فك الضغط وشغّل `./E-LAYER-ANALYZER`، أو شغّل `./installa.sh`: يُثبَّت في `~/E-LAYER-ANALYZER` مع أيقونة في القائمة. |
| Alpine Linux ‏(musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`، ثم كالنسخة المحمولة لـ Linux. |
| Raspberry Pi (64 بت) | `e-layer-analyzer_x.y.z_arm64.deb` أو `...-linux-arm64-portatile.tar.gz` | Pi 3 أو 4 أو 5 مع **Raspberry Pi OS ‏64 بت** وسطح المكتب (يُنصح بـ 4 GB من RAM). ملف ‎.deb كما سبق؛ والنسخة المحمولة كما في Linux. |
| Raspberry Pi (32 بت) | `e-layer-analyzer_x.y.z_armhf.deb` أو `...-linux-arm-portatile.tar.gz` | Pi 2 أو 3 أو 4 مع **Raspberry Pi OS ‏32 بت**. ملف ‎.deb كما سبق؛ والنسخة المحمولة: فك الضغط وشغّل `./E-LAYER-ANALYZER` أو `./installa.sh`. |
| Raspberry Pi مع Slackware ARM64 | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` أو `...-linux-arm64-portatile.tar.gz` | كما في Slackware أعلاه. والنسخة المحمولة كما في Linux. |
| Linux armv7l (لوحات ARM أخرى بـ 32 بت) | `...-linux-armv7l-portatile.tar.gz` | كالنسخة المحمولة لـ Linux. |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **تجريبي وغير مُتحقَّق منه.** يحتاج إلى .NET 8 الخاص بالمجتمع (`pkg install dotnet`) وإلى مكتبتي SkiaSharp وHarfBuzzSharp الأصليتين المبنيتين لـ FreeBSD، اللتين لا توفرهما Avalonia: ومن دونهما لا يعمل البرنامج. راجع `LEGGIMI-FREEBSD.txt` في الحزمة. |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) أو `...-osx-x64.dmg` (Intel) | افتح ملف ‎.dmg، وانقر بزر الفأرة الأيمن على **Installa E-LAYER-ANALYZER.command** > Open. يُثبَّت في `~/E-LAYER-ANALYZER`. |
| Mac، التطبيق فقط | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | فك ضغط **E-LAYER-ANALYZER.app** في مجلد خاص بك (مثلاً `~/E-LAYER-ANALYZER`، وليس Applications): يُنشأ مجلد `data` بجانب التطبيق. عند التشغيل الأول انقر بزر الفأرة الأيمن > Open. |

## التحديث

ثبّت الإصدار الجديد فوق القديم: تُحفظ الإعدادات والبلاغات المجمَّعة والسجلات والملف `call3.txt`.

**حزم النظام (.deb و.rpm وArch وSlackware) لا تُحدَّث تلقائياً.** معها يكتفي البرنامج بالتنبيه إلى الإصدار الجديد: نزّل الحزمة الجديدة من صفحة Releases وثبّتها كما في المرة الأولى (`sudo apt install ./...deb`، `sudo dnf install ./...rpm`، `sudo pacman -U ./...pkg.tar.zst`، `upgradepkg ...txz`). وتعمل التحديثات التلقائية مع مثبّت Windows وملفات zip، وتطبيق Mac، وحزم tar.gz المحمولة، وAppImage.

**التحديث التلقائي (منذ الإصدار 0.4.5).** عند التشغيل ثم كل 30 دقيقة يتحقق البرنامج من صفحة Releases على GitHub. وعند صدور إصدار جديد يفتح نافذة ويعرض **▲ x.y.z** باللون البرتقالي بجانب مؤشر NTP في الشريط العلوي:

**منذ الإصدار 0.4.12 أصبحت التحديثات تلقائية وصامتة**: عندما يوجد إصدار جديد على GitHub ينزّله البرنامج ويثبّته ويعيد تشغيل نفسه دون أن يسأل شيئاً. كما تُنزَّل الملفات الناقصة من جديد دون سؤال. لا تظهر نافذة التحديث إلا بالنقر على رقم الإصدار في الأعلى أو إذا عطّلت التحديثات التلقائية في الإعدادات (عام).

- **حدّث الآن** ينزّل الحزمة المحمولة لحاسوبك (Windows ‏64/32 بت، وLinux، وRaspberry Pi، وMac)، ويغلق البرنامج، ويستبدل الملفات ثم يعيد تشغيله. تُحفظ الإعدادات والبلاغات المجمَّعة والسجلات والملف `call3.txt`.
- **لاحقاً** يسأل مجدداً عند التحقق التالي؛ و**تخطَّ هذا الإصدار** يتوقف عن اقتراح ذلك الإصدار.
- النقر على الإصدار المعروض في الشريط العلوي (مثلاً `v0.4.5`) يتحقق من التحديثات فوراً. ويمكن إيقاف التحقق التلقائي بمربع الاختيار في النافذة نفسها.
- إذا تعذّرت الكتابة في مجلد البرنامج (مثلاً مجلد نظام)، فاستخدم بدلاً من ذلك حزمة التثبيت من صفحة Release.

**الملفات الناقصة.** عند التشغيل يتحقق البرنامج من وجود جميع الملفات التي وُزِّع بها (الترجمات والمستندات والجداول، المدرجة في `data/manifest.txt`). وإن نقص شيء منها تعرضها نافذة، ويعيد **تنزيل من GitHub** تنزيلها من الإصدار نفسه على GitHub.

## المتطلبات

- لا حاجة إلى تثبيت .NET: يحتوي البرنامج على كل ما يحتاجه.
- اتصال بالإنترنت للخريطة (Natural Earth، عند التشغيل الأول)، ولبيانات الشفق من NOAA، ولرادارات YA-MS، ولجلسات telnet.
- لا يُدعم Raspberry Pi Zero وPi 1 (لا يتعامل .NET مع معالجاتهما).
- لا توجد نسخ لـ Linux i686 ‏(الحواسيب ذات 32 بت) ولا لـ OpenBSD، لأن .NET غير موجود لهذين النظامين. ولـ FreeBSD الحزمة التجريبية فقط.
