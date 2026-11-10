# E-LAYER ANALYZER

*作者：Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · [中文 (简体)](Home-zh) · **中文 (繁體)**

**E-LAYER ANALYZER** 是一套供業餘無線電玩家使用的免費軟體，用來分析所有與電離層 **E 層**相關的 VHF 傳播模式：**散塊 E 層**（Es，包括跨洲多跳）、**F.A.I.**（沿磁場排列的不規則體）、**流星散射**（內建 YA-MS 程式）以及**無線電極光**。它可在 **Windows（64 與 32 位元）、Linux、Raspberry Pi 與 Mac** 上執行，提供 **36 種語言**，而且不會在自己的資料夾以外寫入任何東西。

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## 頁籤

| 頁籤 | 功能 |
|---|---|
| [SPORADIC-E](Sporadic-E-zh-tw) | 地圖上的 Es spot、兩個電台的首跳、梅登黑德網格上的 Es 雲、本台的 MUF |
| [BENCHMARK Es](Benchmark-Es-zh-tw) | 以完整 ADIF 日誌（也可以是其他電台的）進行的測試平台：每日與每小時的通聯、依扇區的 skip、估計的 foEs、多跳驗證、雲的移動 |
| [BENCHMARK 50 MHz](Benchmark-50-zh-tw) | 由一或兩份 ADIF 日誌分析 50 MHz 的多跳散塊 E 層：麥卡托地圖、日期與時段、選用的 foEs、放大並顯示 WWL 的路徑、150 km 以內的通聯 |
| [F.A.I.](FAI-zh-tw) | 一對電台散射點的 Scatter trail，以及用來驗證某一點的等反射線 |
| [METEOR SCATTER](Meteor-Scatter-zh-tw) | 完整的 YA-MS 程式（流星雨效率、雷達、預報） |
| [AURORA](Aurora-zh-tw) | 極光橢圓區、NOAA 的 Kp 與太陽風、極光 Scatter trail 與等反射線、建議的天線指向 |
| [DATA COLLECTION](Data-Collection-zh-tw) | 三個 telnet 連線階段（DX cluster、RBN、含 PSKReporter 的 DM4X）、已收集的 spot、CALL3 資料庫 |
| [LOG ADIF](Log-ADIF-zh-tw) | 從 28 MHz 起逐頻段列出已通聯與已確認的網格，資料來自你的 ADIF 或 LoTW |
| [資料來源、計算選擇摘要、使用手冊、技術報告](Sources-and-Technical-Document-zh-tw) | 資料來源、公式與計算選擇、詳細使用手冊、技術報告：可匯出為 Word |

## 入門

1. [安裝](Installation-zh-tw)
2. [開始使用](Getting-Started-zh-tw)
3. [配色與語言](Colours-and-Languages-zh-tw)
4. [資料資料夾](Data-Folder-zh-tw)
5. [常見問題](FAQ-zh-tw)

## 主要資料來源

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html)。
- F. Egano IK3XTV，散塊 E 層距離表（[qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)）。
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER*（YA-MS 的資料來源）。

完整清單在程式的 資料來源 頁籤中。
