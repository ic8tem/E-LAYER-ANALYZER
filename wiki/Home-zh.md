# E-LAYER ANALYZER

*作者：Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · [Deutsch](Home-de) · [Nederlands](Home-nl) · [Français](Home-fr) · [Español](Home-es) · [Português](Home-pt) · [Dansk](Home-da) · [Norsk](Home-nb) · [Svenska](Home-sv) · [Íslenska](Home-is) · [Eesti](Home-et) · [Lietuvių](Home-lt) · [Latviešu](Home-lv) · [Русский](Home-ru) · [Українська](Home-uk) · [Türkçe](Home-tr) · [Afrikaans](Home-af) · [العربية](Home-ar) · [עברית](Home-he) · [اردو](Home-ur) · [हिन्दी](Home-hi) · [বাংলা](Home-bn) · [Bahasa Indonesia](Home-id) · [Tagalog](Home-tl) · [日本語](Home-ja) · [한국어](Home-ko) · **中文**

**E-LAYER ANALYZER** 是一款面向业余无线电爱好者的免费程序，用于分析所有涉及电离层 **E 层**的 VHF 传播方式：**偶发 E 层**（包括洲际多跳）、**F.A.I.**（场向不规则体）、**流星散射**（内置 YA-MS 程序）以及**无线电极光**。它可在 **Windows（64 位和 32 位）、Linux、Raspberry Pi 和 Mac** 上运行，提供 **28 种语言**，并且不会在自身文件夹之外写入任何内容。

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## 选项卡

| 选项卡 | 功能 |
|---|---|
| [SPORADIC-E](Sporadic-E-zh) | 地图上的 Es spot、双方电台的首跳、Maidenhead 方格上的 Es 云、本台 MUF |
| [BENCHMARK Es](Benchmark-Es-zh) | 针对整份 ADIF 日志（也可以是其他台站的）的测试平台：每日和每小时 QSO、按扇区统计静区（skip）、估算 foEs、多跳验证、云的漂移 |
| [BENCHMARK 50 MHz](Benchmark-50-zh) | 基于一份或两份 ADIF 日志的 50 MHz 多跳偶发 E 层：墨卡托地图、日期和时段、可选 foEs、带 WWL 的放大路径、150 km 范围内的 QSO |
| [F.A.I.](FAI-zh) | 一对台站散射点的 Scatter trail，以及用于验证某一点的等反射线 |
| [METEOR SCATTER](Meteor-Scatter-zh) | 完整的 YA-MS 程序（流星雨效率、雷达、预报） |
| [AURORA](Aurora-zh) | 极光椭圆、NOAA Kp 和太阳风、极光 Scatter trail 与等反射线、推荐天线指向 |
| [DATA COLLECTION](Data-Collection-zh) | 三个 telnet 会话（DX cluster、RBN、带 PSKReporter 的 DM4X）、已收集的 spot、CALL3 数据库 |
| [LOG ADIF](Log-ADIF-zh) | 从 28 MHz 起逐频段显示已通联和已确认的方格，数据来自您的 ADIF 或 LoTW |
| [来源、计算选择摘要、使用手册、技术报告](Sources-and-Technical-Document-zh) | 来源、公式与计算选择、详细的使用手册、技术报告：均可导出为 Word |

## 入门

1. [安装](Installation-zh)
2. [初次启动](Getting-Started-zh)
3. [配色与语言](Colours-and-Languages-zh)
4. [数据文件夹](Data-Folder-zh)
5. [常见问题](FAQ-zh)

## 主要来源

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*，Capri，2011。
- G. Cerrotta IC8FAX，[Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html)。
- F. Egano IK3XTV，偶发 E 层距离表（[qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/)）。
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER*（YA-MS 的来源）。

完整列表见程序的“来源”选项卡。
