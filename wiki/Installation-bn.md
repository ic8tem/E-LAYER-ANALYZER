# ইনস্টলেশন

[← Home](Home-bn)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** পাতা থেকে সঠিক ফাইলটি ডাউনলোড করুন। প্রোগ্রাম ও `data` ফোল্ডার সর্বদা আপনার নিজের একটি ফোল্ডারে একসাথে থাকে: **C:\Users\...\AppData-তে কখনো কোনো ফাইল লেখা হয় না**।

| সিস্টেম | ফাইল | কীভাবে ইনস্টল করবেন |
|---|---|---|
| Windows 64 বিট | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | ডাবল-ক্লিক করুন। `C:\E-LAYER-ANALYZER`-এ ইনস্টল হয়, অ্যাডমিনিস্ট্রেটর অধিকার লাগে না। Start মেনুতে কোনো শর্টকাট নেই (সেটি AppData-তে থাকত), কেবল ঐচ্ছিক ডেস্কটপ আইকন। |
| Windows 32 বিট | `...-windows-x86-setup.exe` | ওপরের মতোই, 32-বিট Windows-এর জন্য। |
| Windows পোর্টেবল | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | যেকোনো জায়গায় এক্সট্র্যাক্ট করে `E-LAYER-ANALYZER.exe` চালান। |
| Linux (PC) | `...-linux-x64.tar.gz` | এক্সট্র্যাক্ট করে `./installa.sh` চালান: মেনুতে আইকনসহ `~/E-LAYER-ANALYZER`-এ ইনস্টল হয়। |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | **64-বিট Raspberry Pi OS** ও ডেস্কটপসহ Pi 3, 4 বা 5 (4 GB RAM প্রস্তাবিত)। এক্সট্র্যাক্ট করে `./installa.sh` চালান। |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) অথবা `...-mac-intel.dmg` | .dmg খুলুন, **Installa E-LAYER-ANALYZER.command**-এ ডান ক্লিক > Open। `~/E-LAYER-ANALYZER`-এ ইনস্টল হয়। |

## হালনাগাদ

নতুন সংস্করণ পুরোনোটির ওপরেই ইনস্টল করুন: সেটিংস, সংগৃহীত স্পট, লগ ও `call3.txt` রক্ষিত থাকে।

## প্রয়োজনীয়তা

- কোনো .NET ইনস্টল করতে হয় না: প্রোগ্রামে প্রয়োজনীয় সবকিছু আছে।
- ইন্টারনেট সংযোগ দরকার মানচিত্রের জন্য (Natural Earth, প্রথমবার চালুর সময়), NOAA-র অরোরা তথ্য, YA-MS-এর রাডার এবং টেলনেট সেশনের জন্য।
- Raspberry Pi Zero ও Pi 1 সমর্থিত নয় (তাদের প্রসেসর .NET সমর্থন করে না)।
