# انسٹالیشن

[← Home](Home-ur)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** صفحے سے درست فائل ڈاؤن لوڈ کریں۔ پروگرام اور `data` فولڈر ہمیشہ آپ کے اپنے ایک فولڈر میں ایک ساتھ رہتے ہیں: **C:\Users\...\AppData میں کبھی کوئی فائل نہیں لکھی جاتی**۔

| سسٹم | فائل | کیسے انسٹال کریں |
|---|---|---|
| Windows 64 بٹ | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | ڈبل کلک کریں۔ `C:\E-LAYER-ANALYZER` میں انسٹال ہوتا ہے، ایڈمنسٹریٹر حقوق درکار نہیں۔ Start مینو میں کوئی شارٹ کٹ نہیں (وہ AppData میں ہوتا)، صرف اختیاری ڈیسک ٹاپ آئیکن۔ |
| Windows 32 بٹ | `...-win-x86-setup.exe` | اوپر کی طرح، 32 بٹ Windows کے لیے۔ |
| ARM پر Windows (Snapdragon PC) | `...-win-arm64-setup.exe` | اوپر کی طرح، ARM پروسیسر والے Windows کے لیے۔ |
| Windows پورٹیبل | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | کہیں بھی نکالیں اور `E-LAYER-ANALYZER.exe` چلائیں۔ |
| Debian، Ubuntu، Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`، پھر مینو سے چلائیں: پہلی بار چلانے پر پروگرام خود کو `~/E-LAYER-ANALYZER` میں کاپی کر لیتا ہے، اور `data` فولڈر اس کے ساتھ ہوتا ہے۔ |
| Fedora، openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (نیز `.aarch64.rpm`، `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (یا `zypper install`)، پھر .deb کی طرح۔ |
| Arch، Manjaro، EndeavourOS، Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (یا `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`، پھر .deb کی طرح۔ |
| Slackware (x86_64 اور ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (یا `-aarch64-`) | root کے طور پر: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`، پھر مینو یا ٹرمینل سے `e-layer-analyzer` چلائیں: پہلی بار چلنے پر پروگرام خود کو `~/E-LAYER-ANALYZER` میں کاپی کر لیتا ہے۔ |
| کوئی بھی Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (یا `-aarch64.AppImage`) | اسے قابلِ اجرا بنائیں اور چلائیں: پہلی بار چلنے پر یہ پروگرام کو `~/E-LAYER-ANALYZER` میں کاپی کرتی ہے اور وہیں سے چلاتی ہے۔ |
| Linux پورٹیبل | `...-linux-x64-portatile.tar.gz` | نکالیں اور `./E-LAYER-ANALYZER` چلائیں، یا `./installa.sh` چلائیں: مینو میں آئیکن کے ساتھ `~/E-LAYER-ANALYZER` میں انسٹال ہوتا ہے۔ |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`، پھر Linux پورٹیبل کی طرح۔ |
| Raspberry Pi (64 بٹ) | `e-layer-analyzer_x.y.z_arm64.deb` یا `...-linux-arm64-portatile.tar.gz` | **64 بٹ Raspberry Pi OS** اور ڈیسک ٹاپ کے ساتھ Pi 3، 4 یا 5 (4 GB RAM تجویز کردہ)۔ .deb اوپر کی طرح؛ پورٹیبل Linux کی طرح۔ |
| Raspberry Pi (32 بٹ) | `e-layer-analyzer_x.y.z_armhf.deb` یا `...-linux-arm-portatile.tar.gz` | **32 بٹ Raspberry Pi OS** کے ساتھ Pi 2، 3 یا 4۔ .deb اوپر کی طرح؛ پورٹیبل: نکالیں اور `./E-LAYER-ANALYZER` یا `./installa.sh` چلائیں۔ |
| Slackware ARM64 کے ساتھ Raspberry Pi | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` یا `...-linux-arm64-portatile.tar.gz` | اوپر والے Slackware کی طرح۔ پورٹیبل Linux کی طرح۔ |
| Linux armv7l (دوسرے 32 بٹ ARM بورڈ) | `...-linux-armv7l-portatile.tar.gz` | Linux پورٹیبل کی طرح۔ |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **تجرباتی، غیر تصدیق شدہ۔** کمیونٹی کا .NET 8 (`pkg install dotnet`) اور FreeBSD کے لیے بنی SkiaSharp اور HarfBuzzSharp مقامی لائبریریاں درکار ہیں، جو Avalonia فراہم نہیں کرتا: ان کے بغیر پروگرام شروع نہیں ہوتا۔ پیکیج میں `LEGGIMI-FREEBSD.txt` دیکھیں۔ |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) یا `...-osx-x64.dmg` (Intel) | .dmg کھولیں، **Installa E-LAYER-ANALYZER.command** پر دایاں کلک > Open۔ `~/E-LAYER-ANALYZER` میں انسٹال ہوتا ہے۔ |
| Mac، صرف ایپلیکیشن | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** کو اپنے ایک فولڈر میں نکالیں (مثلاً `~/E-LAYER-ANALYZER`، Applications نہیں): `data` فولڈر ایپ کے ساتھ بنتا ہے۔ پہلی بار چلانے پر دایاں کلک > Open۔ |

## اپ ڈیٹ کرنا

نیا ورژن پرانے کے اوپر انسٹال کریں: ترتیبات، جمع شدہ اسپاٹ، لاگ اور `call3.txt` محفوظ رہتے ہیں۔

**سسٹم پیکیج (.deb، .rpm، Arch، Slackware) خود اپ ڈیٹ نہیں ہوتے۔** ان کے ساتھ پروگرام صرف نئے ورژن کی اطلاع دیتا ہے: Releases صفحے سے نیا پیکیج ڈاؤن لوڈ کریں اور پہلی بار کی طرح انسٹال کریں (`sudo apt install ./...deb`، `sudo dnf install ./...rpm`، `sudo pacman -U ./...pkg.tar.zst`، `upgradepkg ...txz`)۔ خودکار اپ ڈیٹس Windows کے انسٹالر اور zip، Mac کی ایپ، پورٹیبل tar.gz پیکیجوں اور AppImage کے ساتھ کام کرتی ہیں۔

**خودکار اپ ڈیٹ (ورژن 0.4.5 سے)۔** شروع ہونے پر اور پھر ہر 30 منٹ بعد پروگرام GitHub کی Releases دیکھتا ہے۔ جب نیا ورژن آ جائے تو یہ ایک ونڈو کھولتا ہے اور اوپر کی پٹی میں NTP اشارے کے ساتھ نارنجی **▲ x.y.z** دکھاتا ہے:

**ورژن 0.4.12 سے اپ ڈیٹس خودکار اور خاموش ہیں**: جب GitHub پر نیا ورژن ہو تو پروگرام اسے ڈاؤن لوڈ اور انسٹال کر کے بغیر کچھ پوچھے خود دوبارہ شروع ہو جاتا ہے۔ غائب فائلیں بھی بغیر پوچھے دوبارہ ڈاؤن لوڈ ہوتی ہیں۔ اپ ڈیٹ کی ونڈو صرف اوپر ورژن پر کلک کرنے سے یا ترتیبات (عمومی) میں خودکار اپ ڈیٹ بند کرنے پر ظاہر ہوتی ہے۔

- **ابھی اپ ڈیٹ کریں** آپ کے کمپیوٹر کا پورٹیبل پیکیج (Windows 64/32 بٹ، Linux، Raspberry Pi، Mac) ڈاؤن لوڈ کرتا ہے، پروگرام بند کرتا ہے، فائلیں بدلتا ہے اور اسے دوبارہ شروع کرتا ہے۔ ترتیبات، جمع شدہ اسپاٹ، لاگ اور `call3.txt` محفوظ رہتے ہیں۔
- **بعد میں** اگلی جانچ پر دوبارہ پوچھتا ہے؛ **یہ ورژن چھوڑ دیں** اس ورژن کو پیش کرنا بند کر دیتا ہے۔
- اوپر کی پٹی میں دکھائے گئے ورژن پر کلک کرنے سے (مثلاً `v0.4.5`) فوراً اپ ڈیٹس دیکھی جاتی ہیں۔ خودکار جانچ اسی ونڈو کے چیک باکس سے بند کی جا سکتی ہے۔
- اگر پروگرام کے فولڈر میں لکھا نہ جا سکے (مثلاً کوئی سسٹم فولڈر)، تو اس کے بجائے Release کے صفحے سے انسٹالیشن پیکیج استعمال کریں۔

**غائب فائلیں۔** شروع ہونے پر پروگرام دیکھتا ہے کہ وہ تمام فائلیں موجود ہیں جن کے ساتھ اسے تقسیم کیا گیا تھا (ترجمے، دستاویزات، جدولیں، جو `data/manifest.txt` میں درج ہیں)۔ اگر کوئی غائب ہو تو ایک ونڈو ان کی فہرست دکھاتی ہے اور **GitHub سے ڈاؤن لوڈ کریں** انہیں GitHub پر اسی ورژن سے دوبارہ لے آتا ہے۔

## ضروریات

- کوئی .NET انسٹال کرنے کی ضرورت نہیں: پروگرام میں ضرورت کی ہر چیز شامل ہے۔
- انٹرنیٹ کنکشن نقشے (Natural Earth، پہلی بار چلانے پر)، NOAA کے aurora ڈیٹا، YA-MS کے ریڈاروں اور Telnet سیشنوں کے لیے۔
- Raspberry Pi Zero اور Pi 1 معاون نہیں (ان کے پروسیسر .NET سنبھال نہیں سکتا)۔
- Linux i686 (32 بٹ PC) اور OpenBSD کے لیے کوئی ورژن نہیں، کیونکہ ان سسٹموں کے لیے .NET موجود نہیں۔ FreeBSD کے لیے صرف تجرباتی پیکیج ہے۔
