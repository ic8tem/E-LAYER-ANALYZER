# انسٹالیشن

[← Home](Home-ur)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** صفحے سے درست فائل ڈاؤن لوڈ کریں۔ پروگرام اور `data` فولڈر ہمیشہ آپ کے اپنے ایک فولڈر میں ایک ساتھ رہتے ہیں: **C:\Users\...\AppData میں کبھی کوئی فائل نہیں لکھی جاتی**۔

| سسٹم | فائل | کیسے انسٹال کریں |
|---|---|---|
| Windows 64 بٹ | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | ڈبل کلک کریں۔ `C:\E-LAYER-ANALYZER` میں انسٹال ہوتا ہے، ایڈمنسٹریٹر حقوق درکار نہیں۔ Start مینو میں کوئی شارٹ کٹ نہیں (وہ AppData میں ہوتا)، صرف اختیاری ڈیسک ٹاپ آئیکن۔ |
| Windows 32 بٹ | `...-win-x86-setup.exe` | اوپر کی طرح، 32 بٹ Windows کے لیے۔ |
| Windows پورٹیبل | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | کہیں بھی نکالیں اور `E-LAYER-ANALYZER.exe` چلائیں۔ |
| Linux 64 بٹ (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`، پھر مینو سے چلائیں: پہلی بار چلانے پر پروگرام خود کو `~/E-LAYER-ANALYZER` میں کاپی کر لیتا ہے، اور `data` فولڈر اس کے ساتھ ہوتا ہے۔ |
| Linux پورٹیبل | `...-linux-x64-portatile.tar.gz` | نکالیں اور `./E-LAYER-ANALYZER` چلائیں، یا `./installa.sh` چلائیں: مینو میں آئیکن کے ساتھ `~/E-LAYER-ANALYZER` میں انسٹال ہوتا ہے۔ |
| Raspberry Pi (64 بٹ) | `e-layer-analyzer_x.y.z_arm64.deb` یا `...-linux-arm64-portatile.tar.gz` | **64 بٹ Raspberry Pi OS** اور ڈیسک ٹاپ کے ساتھ Pi 3، 4 یا 5 (4 GB RAM تجویز کردہ)۔ .deb اوپر کی طرح؛ پورٹیبل Linux کی طرح۔ |
| Raspberry Pi (32 بٹ) | `...-linux-arm-portatile.tar.gz` | **32 بٹ Raspberry Pi OS** کے ساتھ Pi 2، 3 یا 4۔ نکالیں اور `./E-LAYER-ANALYZER` یا `./installa.sh` چلائیں۔ |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) یا `...-osx-x64.dmg` (Intel) | .dmg کھولیں، **Installa E-LAYER-ANALYZER.command** پر دایاں کلک > Open۔ `~/E-LAYER-ANALYZER` میں انسٹال ہوتا ہے۔ |
| Mac، صرف ایپلیکیشن | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** کو اپنے ایک فولڈر میں نکالیں (مثلاً `~/E-LAYER-ANALYZER`، Applications نہیں): `data` فولڈر ایپ کے ساتھ بنتا ہے۔ پہلی بار چلانے پر دایاں کلک > Open۔ |

## اپ ڈیٹ کرنا

نیا ورژن پرانے کے اوپر انسٹال کریں: ترتیبات، جمع شدہ اسپاٹ، لاگ اور `call3.txt` محفوظ رہتے ہیں۔

## ضروریات

- کوئی .NET انسٹال کرنے کی ضرورت نہیں: پروگرام میں ضرورت کی ہر چیز شامل ہے۔
- انٹرنیٹ کنکشن نقشے (Natural Earth، پہلی بار چلانے پر)، NOAA کے aurora ڈیٹا، YA-MS کے ریڈاروں اور Telnet سیشنوں کے لیے۔
- Raspberry Pi Zero اور Pi 1 معاون نہیں (ان کے پروسیسر .NET سنبھال نہیں سکتا)۔
