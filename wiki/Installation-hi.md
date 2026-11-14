# इंस्टॉलेशन

[← Home](Home-hi)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** पेज से सही फ़ाइल डाउनलोड करें। प्रोग्राम और `data` फ़ोल्डर हमेशा आपके अपने एक फ़ोल्डर में साथ रहते हैं: **C:\Users\...\AppData में कभी कोई फ़ाइल नहीं लिखी जाती**।

| सिस्टम | फ़ाइल | कैसे इंस्टॉल करें |
|---|---|---|
| Windows 64 बिट | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | डबल-क्लिक करें। `C:\E-LAYER-ANALYZER` में इंस्टॉल होता है, एडमिनिस्ट्रेटर अधिकारों की ज़रूरत नहीं। Start मेनू शॉर्टकट नहीं (वह AppData में होता), केवल वैकल्पिक डेस्कटॉप आइकन। |
| Windows 32 बिट | `...-win-x86-setup.exe` | ऊपर जैसा ही, 32-बिट Windows के लिए। |
| ARM पर Windows (Snapdragon PC) | `...-win-arm64-setup.exe` | ऊपर जैसा, ARM प्रोसेसर वाले Windows के लिए। |
| Windows पोर्टेबल | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | कहीं भी निकालें और `E-LAYER-ANALYZER.exe` चलाएँ। |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, फिर मेनू से शुरू करें: पहली बार शुरू होने पर प्रोग्राम ख़ुद को `~/E-LAYER-ANALYZER` में कॉपी कर लेता है, उसके बगल में `data` फ़ोल्डर के साथ। |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (साथ ही `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (या `zypper install`), फिर .deb जैसा। |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (या `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, फिर .deb जैसा। |
| Slackware (x86_64 और ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (या `-aarch64-`) | root के रूप में: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, फिर मेनू या टर्मिनल से `e-layer-analyzer` चलाएँ: पहली बार शुरू होने पर प्रोग्राम स्वयं को `~/E-LAYER-ANALYZER` में कॉपी कर लेता है। |
| कोई भी Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (या `-aarch64.AppImage`) | इसे निष्पादन योग्य बनाएँ और चलाएँ: पहली बार शुरू होने पर यह प्रोग्राम को `~/E-LAYER-ANALYZER` में कॉपी करती है और वहीं से चलाती है। |
| Linux पोर्टेबल | `...-linux-x64-portatile.tar.gz` | निकालें और `./E-LAYER-ANALYZER` चलाएँ, या `./installa.sh` चलाएँ: मेनू में आइकन के साथ `~/E-LAYER-ANALYZER` में इंस्टॉल होता है। |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, फिर Linux पोर्टेबल जैसा। |
| Raspberry Pi (64 बिट) | `e-layer-analyzer_x.y.z_arm64.deb` या `...-linux-arm64-portatile.tar.gz` | **64-बिट Raspberry Pi OS** और डेस्कटॉप वाला Pi 3, 4 या 5 (4 GB RAM अनुशंसित)। .deb ऊपर जैसा; पोर्टेबल Linux जैसा। |
| Raspberry Pi (32 बिट) | `e-layer-analyzer_x.y.z_armhf.deb` या `...-linux-arm-portatile.tar.gz` | **32-बिट Raspberry Pi OS** वाला Pi 2, 3 या 4। .deb ऊपर जैसा; पोर्टेबल: निकालें और `./E-LAYER-ANALYZER` या `./installa.sh` चलाएँ। |
| Slackware ARM64 वाला Raspberry Pi | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` या `...-linux-arm64-portatile.tar.gz` | ऊपर वाले Slackware जैसा। पोर्टेबल Linux जैसा। |
| Linux armv7l (अन्य 32-बिट ARM बोर्ड) | `...-linux-armv7l-portatile.tar.gz` | Linux पोर्टेबल जैसा। |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **प्रायोगिक, असत्यापित।** समुदाय का .NET 8 (`pkg install dotnet`) और FreeBSD के लिए बनी SkiaSharp और HarfBuzzSharp नेटिव लाइब्रेरी चाहिए, जो Avalonia नहीं देता: उनके बिना प्रोग्राम शुरू नहीं होता। पैकेज में `LEGGIMI-FREEBSD.txt` देखें। |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) या `...-osx-x64.dmg` (Intel) | .dmg खोलें, **Installa E-LAYER-ANALYZER.command** पर दायाँ क्लिक > Open। `~/E-LAYER-ANALYZER` में इंस्टॉल होता है। |
| Mac, केवल ऐप्लिकेशन | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** को अपने एक फ़ोल्डर में निकालें (उदाहरण के लिए `~/E-LAYER-ANALYZER`, Applications नहीं): `data` फ़ोल्डर ऐप के बगल में बनता है। पहली बार शुरू करने पर दायाँ क्लिक > Open। |

## अपडेट करना

नया संस्करण पुराने के ऊपर ही इंस्टॉल करें: सेटिंग्स, एकत्रित स्पॉट, लॉग और `call3.txt` सुरक्षित रहते हैं।

**सिस्टम पैकेज (.deb, .rpm, Arch, Slackware) अपने आप अपडेट नहीं होते।** उनके साथ प्रोग्राम केवल नए संस्करण की सूचना देता है: Releases पृष्ठ से नया पैकेज डाउनलोड करें और पहली बार की तरह इंस्टॉल करें (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`)। स्वचालित अपडेट Windows के इंस्टॉलर और zip, Mac के ऐप, पोर्टेबल tar.gz पैकेजों और AppImage के साथ काम करते हैं।

**स्वचालित अपडेट (संस्करण 0.4.5 से)।** शुरू होने पर और फिर हर 30 मिनट में प्रोग्राम GitHub की Releases जाँचता है। जब नया संस्करण आता है तो वह एक विंडो खोलता है और ऊपरी पट्टी में NTP संकेतक के पास नारंगी **▲ x.y.z** दिखाता है:

**संस्करण 0.4.12 से अपडेट स्वचालित और मौन हैं**: जब GitHub पर नया संस्करण होता है तो प्रोग्राम उसे डाउनलोड करके इंस्टॉल करता है और बिना कुछ पूछे स्वयं फिर से शुरू हो जाता है। गायब फ़ाइलें भी बिना पूछे फिर से डाउनलोड होती हैं। अपडेट विंडो केवल ऊपर संस्करण पर क्लिक करने से या सेटिंग्स (सामान्य) में स्वचालित अपडेट बंद करने पर दिखती है।

- **अभी अपडेट करें** आपके कंप्यूटर का पोर्टेबल पैकेज (Windows 64/32 बिट, Linux, Raspberry Pi, Mac) डाउनलोड करता है, प्रोग्राम बंद करता है, फ़ाइलें बदलता है और उसे फिर से शुरू करता है। सेटिंग्स, एकत्रित स्पॉट, लॉग और `call3.txt` सुरक्षित रहते हैं।
- **बाद में** अगली जाँच पर फिर पूछता है; **यह संस्करण छोड़ें** उस संस्करण को प्रस्तावित करना बंद कर देता है।
- ऊपरी पट्टी में दिखाए गए संस्करण पर क्लिक करने से (उदाहरण के लिए `v0.4.5`) तुरंत अपडेट जाँचे जाते हैं। स्वचालित जाँच उसी विंडो के चेकबॉक्स से बंद की जा सकती है।
- यदि प्रोग्राम के फ़ोल्डर में लिखा नहीं जा सकता (उदाहरण के लिए कोई सिस्टम फ़ोल्डर), तो इसके बजाय Release पृष्ठ से इंस्टॉलेशन पैकेज का उपयोग करें।

**गायब फ़ाइलें।** शुरू होने पर प्रोग्राम जाँचता है कि वे सभी फ़ाइलें मौजूद हैं जिनके साथ उसे वितरित किया गया था (अनुवाद, दस्तावेज़, तालिकाएँ, जो `data/manifest.txt` में सूचीबद्ध हैं)। यदि कोई गायब हो, तो एक विंडो उनकी सूची दिखाती है और **GitHub से डाउनलोड करें** उन्हें GitHub पर उसी संस्करण से फिर से ले आता है।

## आवश्यकताएँ

- .NET इंस्टॉल करने की ज़रूरत नहीं: प्रोग्राम में वह सब है जिसकी उसे ज़रूरत है।
- मानचित्र (Natural Earth, पहली बार शुरू होने पर), NOAA ऑरोरा डेटा, YA-MS रडार और टेलनेट सत्रों के लिए इंटरनेट कनेक्शन।
- Raspberry Pi Zero और Pi 1 समर्थित नहीं हैं (उनके प्रोसेसर .NET द्वारा समर्थित नहीं हैं)।
- Linux i686 (32-बिट PC) और OpenBSD के लिए कोई संस्करण नहीं है, क्योंकि उन सिस्टमों के लिए .NET उपलब्ध नहीं है। FreeBSD के लिए केवल प्रायोगिक पैकेज है।
