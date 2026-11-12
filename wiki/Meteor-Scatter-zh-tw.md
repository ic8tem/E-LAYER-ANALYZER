# METEOR SCATTER

[← 首頁](Home-zh-tw)

此頁籤包含完整的 **YA-MS 1.8.0**，即 IC8TEM 的流星散射程式：顯示輻射點的天球、SKiYMET 雷達通量（Collm、Sodankylä）、流星雨效率計算、預報（實驗性）、幾何與路徑損耗、GRAVES、網格地圖、**即時流星雨**（Global Meteor Network 量測的 ZHR 與 CMOR 雷達的輻射點）、**資料來源地圖**（Collm、Sodankylä、GRAVES 與 CMOR 相對於電台的位置），以及附公式的**效率 - 說明**文件。

YA-MS 1.5.0 加入了**資料收集**頁籤（以 MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M、`<MS>` 篩選的流星散射 spot）。在 E-LAYER ANALYZER 內，它不會開啟自己的 telnet 連線階段：而是接收 DATA COLLECTION 連線階段的資料行，因此不會以同一呼號重複登入。

YA-MS 1.6.0 將各頁籤重新整理為 **Live 與即時資料**（即時流星散射通訊及每筆通聯的雲團、雷達通量、GRAVES、資料收集）與**預報計算**（預報、幾何、效率計算、預報的實作）。

- YA-MS 使用在 E-LAYER ANALYZER 上方所選的語言與配色方案。
- YA-MS 的設定儲存在 `data/config.ini`。
- 每個新版的 YA-MS 都以 `tools/sync_yams.py` 腳本納入。

詳情請見 [YA-MS 儲存庫](https://github.com/ic8tem/YA-MS)。
