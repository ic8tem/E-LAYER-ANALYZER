# ইনস্টলেশন

[← Home](Home-bn)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** পাতা থেকে সঠিক ফাইলটি ডাউনলোড করুন। প্রোগ্রাম ও `data` ফোল্ডার সর্বদা আপনার নিজের একটি ফোল্ডারে একসাথে থাকে: **C:\Users\...\AppData-তে কখনো কোনো ফাইল লেখা হয় না**।

| সিস্টেম | ফাইল | কীভাবে ইনস্টল করবেন |
|---|---|---|
| Windows 64 বিট | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | ডাবল-ক্লিক করুন। `C:\E-LAYER-ANALYZER`-এ ইনস্টল হয়, অ্যাডমিনিস্ট্রেটর অধিকার লাগে না। Start মেনুতে কোনো শর্টকাট নেই (সেটি AppData-তে থাকত), কেবল ঐচ্ছিক ডেস্কটপ আইকন। |
| Windows 32 বিট | `...-win-x86-setup.exe` | ওপরের মতোই, 32-বিট Windows-এর জন্য। |
| ARM-এ Windows (Snapdragon PC) | `...-win-arm64-setup.exe` | ওপরের মতো, ARM প্রসেসরের Windows-এর জন্য। |
| Windows পোর্টেবল | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | যেকোনো জায়গায় এক্সট্র্যাক্ট করে `E-LAYER-ANALYZER.exe` চালান। |
| Debian, Ubuntu, Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`, তারপর মেনু থেকে চালু করুন: প্রথমবার চালুর সময় প্রোগ্রামটি নিজেকে `~/E-LAYER-ANALYZER`-এ কপি করে, পাশে `data` ফোল্ডারসহ। |
| Fedora, openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (এছাড়া `.aarch64.rpm`, `.armv7hl.rpm`) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (অথবা `zypper install`), তারপর .deb-এর মতো। |
| Arch, Manjaro, EndeavourOS, Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (অথবা `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`, তারপর .deb-এর মতো। |
| Slackware (x86_64 ও ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (অথবা `-aarch64-`) | root হিসেবে: `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`, তারপর মেনু বা টার্মিনাল থেকে `e-layer-analyzer` চালু করুন: প্রথমবার চালুর সময় প্রোগ্রাম নিজেকে `~/E-LAYER-ANALYZER`-এ কপি করে। |
| যেকোনো Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (অথবা `-aarch64.AppImage`) | এক্সিকিউটেবল করে চালান: প্রথমবার চালুর সময় এটি প্রোগ্রামটিকে `~/E-LAYER-ANALYZER`-এ কপি করে সেখান থেকে চালায়। |
| Linux পোর্টেবল | `...-linux-x64-portatile.tar.gz` | এক্সট্র্যাক্ট করে `./E-LAYER-ANALYZER` চালান, অথবা `./installa.sh` চালান: মেনুতে আইকনসহ `~/E-LAYER-ANALYZER`-এ ইনস্টল হয়। |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`, তারপর Linux পোর্টেবলের মতো। |
| Raspberry Pi (64 বিট) | `e-layer-analyzer_x.y.z_arm64.deb` অথবা `...-linux-arm64-portatile.tar.gz` | **64-বিট Raspberry Pi OS** ও ডেস্কটপসহ Pi 3, 4 বা 5 (4 GB RAM প্রস্তাবিত)। .deb ওপরের মতো; পোর্টেবল সংস্করণ Linux-এর মতো। |
| Raspberry Pi (32 বিট) | `e-layer-analyzer_x.y.z_armhf.deb` অথবা `...-linux-arm-portatile.tar.gz` | **32-বিট Raspberry Pi OS**-সহ Pi 2, 3 বা 4। .deb ওপরের মতো; পোর্টেবল: এক্সট্র্যাক্ট করে `./E-LAYER-ANALYZER` বা `./installa.sh` চালান। |
| Slackware ARM64-সহ Raspberry Pi | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` অথবা `...-linux-arm64-portatile.tar.gz` | ওপরের Slackware-এর মতো। পোর্টেবল সংস্করণ Linux-এর মতো। |
| Linux armv7l (অন্যান্য 32-বিট ARM বোর্ড) | `...-linux-armv7l-portatile.tar.gz` | Linux পোর্টেবলের মতো। |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **পরীক্ষামূলক, যাচাই করা হয়নি।** কমিউনিটির .NET 8 (`pkg install dotnet`) এবং FreeBSD-এর জন্য কম্পাইল করা SkiaSharp ও HarfBuzzSharp নেটিভ লাইব্রেরি দরকার, যা Avalonia দেয় না: এগুলো ছাড়া প্রোগ্রাম চালু হয় না। প্যাকেজের `LEGGIMI-FREEBSD.txt` দেখুন। |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) অথবা `...-osx-x64.dmg` (Intel) | .dmg খুলুন, **Installa E-LAYER-ANALYZER.command**-এ ডান ক্লিক > Open। `~/E-LAYER-ANALYZER`-এ ইনস্টল হয়। |
| Mac, কেবল অ্যাপ্লিকেশন | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** আপনার নিজের একটি ফোল্ডারে এক্সট্র্যাক্ট করুন (যেমন `~/E-LAYER-ANALYZER`, Applications নয়): অ্যাপের পাশে `data` ফোল্ডার তৈরি হয়। প্রথমবার চালুর সময় ডান ক্লিক > Open। |

## হালনাগাদ

নতুন সংস্করণ পুরোনোটির ওপরেই ইনস্টল করুন: সেটিংস, সংগৃহীত স্পট, লগ ও `call3.txt` রক্ষিত থাকে।

**সিস্টেম প্যাকেজ (.deb, .rpm, Arch, Slackware) নিজে হালনাগাদ হয় না।** এগুলোর ক্ষেত্রে প্রোগ্রাম কেবল নতুন সংস্করণের খবর জানায়: Releases পাতা থেকে নতুন প্যাকেজ ডাউনলোড করে প্রথমবারের মতো ইনস্টল করুন (`sudo apt install ./...deb`, `sudo dnf install ./...rpm`, `sudo pacman -U ./...pkg.tar.zst`, `upgradepkg ...txz`)। স্বয়ংক্রিয় হালনাগাদ কাজ করে Windows-এর ইনস্টলার ও zip, Mac-এর অ্যাপ, পোর্টেবল tar.gz প্যাকেজ ও AppImage-এর সঙ্গে।

**স্বয়ংক্রিয় হালনাগাদ (সংস্করণ 0.4.5 থেকে)।** চালু হওয়ার সময় এবং তারপর প্রতি 30 মিনিটে প্রোগ্রাম GitHub-এর Releases যাচাই করে। নতুন সংস্করণ বের হলে এটি একটি উইন্ডো খোলে এবং ওপরের বারে NTP নির্দেশকের পাশে কমলা রঙের **▲ x.y.z** দেখায়:

**সংস্করণ 0.4.12 থেকে আপডেট স্বয়ংক্রিয় ও নীরব**: GitHub-এ নতুন সংস্করণ থাকলে প্রোগ্রাম সেটি ডাউনলোড ও ইনস্টল করে কিছু না জিজ্ঞেস করে নিজেই আবার চালু হয়। অনুপস্থিত ফাইলও জিজ্ঞেস না করে আবার ডাউনলোড হয়। আপডেট উইন্ডোটি শুধু উপরের সংস্করণে ক্লিক করলে বা সেটিংস (সাধারণ)-এ স্বয়ংক্রিয় হালনাগাদ বন্ধ করলে দেখা যায়।

- **এখনই হালনাগাদ** আপনার কম্পিউটারের পোর্টেবল প্যাকেজ (Windows 64/32 বিট, Linux, Raspberry Pi, Mac) ডাউনলোড করে, প্রোগ্রাম বন্ধ করে, ফাইলগুলো বদলে দেয় এবং আবার চালু করে। সেটিংস, সংগৃহীত স্পট, লগ ও `call3.txt` রক্ষিত থাকে।
- **পরে** পরের যাচাইয়ে আবার জিজ্ঞাসা করে; **এই সংস্করণ বাদ দিন** সেই সংস্করণটি আর প্রস্তাব করে না।
- ওপরের বারে দেখানো সংস্করণে (যেমন `v0.4.5`) ক্লিক করলে সঙ্গে সঙ্গে হালনাগাদ যাচাই হয়। একই উইন্ডোর চেকবক্স দিয়ে স্বয়ংক্রিয় যাচাই বন্ধ করা যায়।
- প্রোগ্রামের ফোল্ডারে লেখা না গেলে (যেমন কোনো সিস্টেম ফোল্ডার), তার বদলে Release পাতা থেকে ইনস্টলেশন প্যাকেজ ব্যবহার করুন।

**অনুপস্থিত ফাইল।** চালু হওয়ার সময় প্রোগ্রাম যাচাই করে যে যেসব ফাইলসহ এটি বিতরণ করা হয়েছিল তার সবগুলো আছে কি না (অনুবাদ, নথি, সারণি, যেগুলো `data/manifest.txt`-এ তালিকাভুক্ত)। কোনোটি না থাকলে একটি উইন্ডো সেগুলোর তালিকা দেখায় এবং **GitHub থেকে ডাউনলোড** সেগুলো GitHub-এর একই সংস্করণ থেকে আবার নিয়ে আসে।

## প্রয়োজনীয়তা

- কোনো .NET ইনস্টল করতে হয় না: প্রোগ্রামে প্রয়োজনীয় সবকিছু আছে।
- ইন্টারনেট সংযোগ দরকার মানচিত্রের জন্য (Natural Earth, প্রথমবার চালুর সময়), NOAA-র অরোরা তথ্য, YA-MS-এর রাডার এবং টেলনেট সেশনের জন্য।
- Raspberry Pi Zero ও Pi 1 সমর্থিত নয় (তাদের প্রসেসর .NET সমর্থন করে না)।
- Linux i686 (32-বিট PC) ও OpenBSD-এর জন্য কোনো সংস্করণ নেই, কারণ সেই সিস্টেমগুলোর জন্য .NET নেই। FreeBSD-এর জন্য কেবল পরীক্ষামূলক প্যাকেজ আছে।
