# E-LAYER ANALYZER

*রচনা: Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · **বাংলা** · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · [中文](Home-zh)

**E-LAYER ANALYZER** রেডিও অ্যামেচারদের জন্য একটি বিনামূল্যের প্রোগ্রাম, যা আয়নমণ্ডলের **E স্তর** জড়িত সব VHF প্রোপাগেশন মোড বিশ্লেষণ করে: **স্পোরাডিক-E** (আন্তঃমহাদেশীয় মাল্টি-হপসহ), **F.A.I.** (ভূচৌম্বক ক্ষেত্র বরাবর সারিবদ্ধ অনিয়মিততা), **মিটিওর স্ক্যাটার** (অন্তর্নির্মিত YA-MS প্রোগ্রামসহ) এবং **রেডিও অরোরা**। এটি চলে **Windows (64 ও 32 বিট), Linux, Raspberry Pi ও Mac**-এ, **36টি ভাষায়**, এবং নিজের ফোল্ডারের বাইরে কিছুই লেখে না।

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## ট্যাবসমূহ

| ট্যাব | কী করে |
|---|---|
| [SPORADIC-E](Sporadic-E-bn) | মানচিত্রে Es স্পট, দুই স্টেশনের প্রথম হপ, Maidenhead বর্গে Es মেঘ, আপনার স্টেশনের MUF |
| [BENCHMARK Es](Benchmark-Es-bn) | পুরো একটি ADIF লগের ওপর পরীক্ষা-বেঞ্চ (অন্য স্টেশনেরও হতে পারে): দিন ও ঘণ্টা অনুযায়ী QSO, সেক্টর অনুযায়ী স্কিপ, আনুমানিক foEs, মাল্টি-হপ যাচাই, মেঘের সরণ |
| [BENCHMARK 50 MHz](Benchmark-50-bn) | এক বা দুটি ADIF লগ থেকে 50 MHz-এ মাল্টি-হপ স্পোরাডিক-E: Mercator মানচিত্র, দিন ও সময়, ঐচ্ছিক foEs, WWL-সহ বড় করে দেখানো পথ, 150 km-এর মধ্যে QSO |
| [F.A.I.](FAI-bn) | এক জোড়া স্টেশনের স্ক্যাটার পয়েন্টগুলোর Scatter trail, এবং একটি বিন্দু যাচাইয়ের জন্য সম-প্রতিফলন রেখা |
| [METEOR SCATTER](Meteor-Scatter-bn) | সম্পূর্ণ YA-MS প্রোগ্রাম (উল্কাবৃষ্টির দক্ষতা, রাডার, পূর্বাভাস) |
| [AURORA](Aurora-bn) | অরোরা ওভাল, NOAA Kp ও সৌর বায়ু, অরোরা Scatter trail ও সম-প্রতিফলন, প্রস্তাবিত বিম-দিক |
| [DATA COLLECTION](Data-Collection-bn) | তিনটি টেলনেট সেশন (DX cluster, RBN, PSKReporter-সহ DM4X), সংগৃহীত স্পট, CALL3 ডেটাবেস |
| [LOG ADIF](Log-ADIF-bn) | 28 MHz থেকে ওপরে ব্যান্ড অনুযায়ী সংযোগকৃত ও নিশ্চিত বর্গ, আপনার ADIF বা LoTW থেকে |
| [উৎস, হিসাবের নির্বাচনসমূহের সারসংক্ষেপ, ব্যবহার নির্দেশিকা, প্রযুক্তিগত প্রতিবেদন](Sources-and-Technical-Document-bn) | উৎস, সূত্র ও হিসাবের নির্বাচন, বিস্তারিত ব্যবহার নির্দেশিকা, প্রযুক্তিগত প্রতিবেদন: Word-এ রপ্তানিযোগ্য |

## শুরু করা

1. [ইনস্টলেশন](Installation-bn)
2. [প্রথমবার চালু](Getting-Started-bn)
3. [রঙ ও ভাষা](Colours-and-Languages-bn)
4. [ডেটা ফোল্ডার](Data-Folder-bn)
5. [প্রায়শ জিজ্ঞাসিত প্রশ্ন](FAQ-bn)

## প্রধান উৎস

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011।
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html)।
- F. Egano IK3XTV, স্পোরাডিক-E দূরত্বের সারণি ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/))।
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (YA-MS-এর উৎস)।

সম্পূর্ণ তালিকা প্রোগ্রামের উৎস ট্যাবে আছে।
