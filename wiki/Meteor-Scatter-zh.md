# METEOR SCATTER

[← Home](Home-zh)

本选项卡包含完整的 **YA-MS 1.8.0**，即 IC8TEM 的流星散射程序：带辐射点的天球、SKiYMET 雷达流量（Collm、Sodankylä）、流星雨效率计算、试验性预报、几何与衰减、GRAVES、方格地图、**实时流星雨**（Global Meteor Network 实测的 ZHR 和 CMOR 雷达的辐射点）、**来源地图**（Collm、Sodankylä、GRAVES 和 CMOR 相对于本台的位置），以及包含公式的**效率 - 说明**文档。

YA-MS 1.5.0 新增了**数据收集**选项卡（按 MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M、`<MS>` 过滤的流星散射 spot）。在 E-LAYER ANALYZER 内部，它不会打开自己的 telnet 会话：它接收 DATA COLLECTION 各会话的行，因此不会出现用同一呼号重复登录的情况。

- YA-MS 使用在 E-LAYER ANALYZER 顶部选择的语言和配色方案。
- YA-MS 的设置保存在 `data/config.ini` 中。
- 每个新版本的 YA-MS 都通过 `tools/sync_yams.py` 脚本引入。

详情请见 [YA-MS 仓库](https://github.com/ic8tem/YA-MS)。
