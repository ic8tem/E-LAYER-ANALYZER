# انسٹالیشن

[← Home](Home-ur)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** صفحے سے درست فائل ڈاؤن لوڈ کریں۔ پروگرام اور `data` فولڈر ہمیشہ آپ کے اپنے ایک فولڈر میں ایک ساتھ رہتے ہیں: **C:\Users\...\AppData میں کبھی کوئی فائل نہیں لکھی جاتی**۔

| سسٹم | فائل | کیسے انسٹال کریں |
|---|---|---|
| Windows 64 بٹ | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | ڈبل کلک کریں۔ `C:\E-LAYER-ANALYZER` میں انسٹال ہوتا ہے، ایڈمنسٹریٹر حقوق درکار نہیں۔ Start مینو میں کوئی شارٹ کٹ نہیں (وہ AppData میں ہوتا)، صرف اختیاری ڈیسک ٹاپ آئیکن۔ |
| Windows 32 بٹ | `...-windows-x86-setup.exe` | اوپر کی طرح، 32 بٹ Windows کے لیے۔ |
| Windows پورٹیبل | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | کہیں بھی نکالیں اور `E-LAYER-ANALYZER.exe` چلائیں۔ |
| Linux (PC) | `...-linux-x64.tar.gz` | نکالیں اور `./installa.sh` چلائیں: مینو میں آئیکن کے ساتھ `~/E-LAYER-ANALYZER` میں انسٹال ہوتا ہے۔ |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | **64 بٹ Raspberry Pi OS** اور ڈیسک ٹاپ کے ساتھ Pi 3، 4 یا 5 (4 GB RAM تجویز کردہ)۔ نکالیں اور `./installa.sh` چلائیں۔ |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) یا `...-mac-intel.dmg` | .dmg کھولیں، **Installa E-LAYER-ANALYZER.command** پر دایاں کلک > Open۔ `~/E-LAYER-ANALYZER` میں انسٹال ہوتا ہے۔ |

## اپ ڈیٹ کرنا

نیا ورژن پرانے کے اوپر انسٹال کریں: ترتیبات، جمع شدہ اسپاٹ، لاگ اور `call3.txt` محفوظ رہتے ہیں۔

## ضروریات

- کوئی .NET انسٹال کرنے کی ضرورت نہیں: پروگرام میں ضرورت کی ہر چیز شامل ہے۔
- انٹرنیٹ کنکشن نقشے (Natural Earth، پہلی بار چلانے پر)، NOAA کے aurora ڈیٹا، YA-MS کے ریڈاروں اور Telnet سیشنوں کے لیے۔
- Raspberry Pi Zero اور Pi 1 معاون نہیں (ان کے پروسیسر .NET سنبھال نہیں سکتا)۔
