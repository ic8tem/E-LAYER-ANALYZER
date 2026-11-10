# METEOR SCATTER

[← Home](Home-zh-TW)

此分頁包含完整的 **YA-MS 1.6.0**，即 IC8TEM 的流星散射程式：含輻射點的天球、SKiYMET 雷達流量（Collm、Sodankylä）、流星雨效率計算、預報的實驗性實作、幾何與衰減、GRAVES、方格地圖、**即時流星雨**（Global Meteor Network 測得的 ZHR 和 CMOR 雷達的輻射點）、**來源地圖**（Collm、Sodankylä、GRAVES 和 CMOR 相對於本台的位置），以及附有公式的**效率 - 說明**文件。

YA-MS 1.5.0 新增了**資料收集**分頁（依 MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M、`<MS>` 篩選的流星散射 spot）。在 E-LAYER ANALYZER 內部，它不會開啟自己的 telnet 連線：它接收 DATA COLLECTION 各連線的資料行，因此不會以同一個呼號重複登入。

YA-MS 1.6.0 把分頁重新整理為**實況與即時資料**（即時流星散射通聯，顯示每次通聯的雲、雷達流量、GRAVES、資料收集）和**預報計算**（預報、幾何、效率計算、預報的實作）。

- YA-MS 使用 E-LAYER ANALYZER 頂部所選的語言和配色方案。
- YA-MS 的設定儲存在 `data/config.ini`。
- YA-MS 的每個新版本都透過 `tools/sync_yams.py` 腳本匯入。

詳情請參閱 [YA-MS 儲存庫](https://github.com/ic8tem/YA-MS)。
