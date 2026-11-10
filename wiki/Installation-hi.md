# इंस्टॉलेशन

[← Home](Home-hi)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** पेज से सही फ़ाइल डाउनलोड करें। प्रोग्राम और `data` फ़ोल्डर हमेशा आपके अपने एक फ़ोल्डर में साथ रहते हैं: **C:\Users\...\AppData में कभी कोई फ़ाइल नहीं लिखी जाती**।

| सिस्टम | फ़ाइल | कैसे इंस्टॉल करें |
|---|---|---|
| Windows 64 बिट | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | डबल-क्लिक करें। `C:\E-LAYER-ANALYZER` में इंस्टॉल होता है, एडमिनिस्ट्रेटर अधिकारों की ज़रूरत नहीं। Start मेनू शॉर्टकट नहीं (वह AppData में होता), केवल वैकल्पिक डेस्कटॉप आइकन। |
| Windows 32 बिट | `...-win-x86-setup.exe` | ऊपर जैसा ही, 32-बिट Windows के लिए। |
| Windows पोर्टेबल | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | कहीं भी निकालें और `E-LAYER-ANALYZER.exe` चलाएँ। |
| Linux 64 बिट (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, फिर मेनू से शुरू करें: पहली बार शुरू होने पर प्रोग्राम ख़ुद को `~/E-LAYER-ANALYZER` में कॉपी कर लेता है, उसके बगल में `data` फ़ोल्डर के साथ। |
| Linux पोर्टेबल | `...-linux-x64-portatile.tar.gz` | निकालें और `./E-LAYER-ANALYZER` चलाएँ, या `./installa.sh` चलाएँ: मेनू में आइकन के साथ `~/E-LAYER-ANALYZER` में इंस्टॉल होता है। |
| Raspberry Pi (64 बिट) | `e-layer-analyzer_x.y.z_arm64.deb` या `...-linux-arm64-portatile.tar.gz` | **64-बिट Raspberry Pi OS** और डेस्कटॉप वाला Pi 3, 4 या 5 (4 GB RAM अनुशंसित)। .deb ऊपर जैसा; पोर्टेबल Linux जैसा। |
| Raspberry Pi (32 बिट) | `...-linux-arm-portatile.tar.gz` | **32-बिट Raspberry Pi OS** वाला Pi 2, 3 या 4। निकालें और `./E-LAYER-ANALYZER` या `./installa.sh` चलाएँ। |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) या `...-osx-x64.dmg` (Intel) | .dmg खोलें, **Installa E-LAYER-ANALYZER.command** पर दायाँ क्लिक > Open। `~/E-LAYER-ANALYZER` में इंस्टॉल होता है। |
| Mac, केवल ऐप्लिकेशन | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** को अपने एक फ़ोल्डर में निकालें (उदाहरण के लिए `~/E-LAYER-ANALYZER`, Applications नहीं): `data` फ़ोल्डर ऐप के बगल में बनता है। पहली बार शुरू करने पर दायाँ क्लिक > Open। |

## अपडेट करना

नया संस्करण पुराने के ऊपर ही इंस्टॉल करें: सेटिंग्स, एकत्रित स्पॉट, लॉग और `call3.txt` सुरक्षित रहते हैं।

## आवश्यकताएँ

- .NET इंस्टॉल करने की ज़रूरत नहीं: प्रोग्राम में वह सब है जिसकी उसे ज़रूरत है।
- मानचित्र (Natural Earth, पहली बार शुरू होने पर), NOAA ऑरोरा डेटा, YA-MS रडार और टेलनेट सत्रों के लिए इंटरनेट कनेक्शन।
- Raspberry Pi Zero और Pi 1 समर्थित नहीं हैं (उनके प्रोसेसर .NET द्वारा समर्थित नहीं हैं)।
