# BENCHMARK 50 MHz

[← Home](Home-zh-TW)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

使用一個電台（以及可選的第二個電台）的 ADIF 日誌，針對 **50 MHz 多跳偶發 E 層**的測試平台。

## 載入日誌

- **電台 1**：*載入 ADIF* / *卸載 ADIF*。程式從日誌中取得 `STATION_CALLSIGN` 和 `MY_GRIDSQUARE`；發射位置為每筆 QSO 的 `MY_GRIDSQUARE`。
- **電台 2**（可選）：第二列有相同的按鈕；*電台 2* 核取方塊可顯示或隱藏它。電台 1 的路徑為淺藍色，電台 2 的路徑為紫色。
- 程式使用帶網格座標的 50 MHz QSO（缺少或只有 4 個字元的網格座標從 CALL3 取得），多跳：超過 3100 km（作者規則；跳數依 IK3XTV 表）。
- 排除**跨赤道**通聯（作者規則）：朝向非洲和南美洲，且中點位於磁傾角 -20° 到 +20° 之間的帶狀區域，除非日誌中有 `PROP_MODE=ES`。

## 日期與時段

選擇**日期**（開啟時為多跳 QSO 最多的那一天）以及 UTC 時段**從…到…**，間隔 15 分鐘。**foEs** 核取方塊（可選）依最小 foEs 為路徑著色，並在表格中顯示。

## 地圖

**麥卡托**地圖，中心可自由移動：滾輪 = 縮放，拖曳 = 平移。

![路徑](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **滑鼠靠近某條路徑**：線條和圖示會放大，並顯示 DX 電台的 **6 字元 WWL 方格**。
- 在該路徑上**按左鍵**：彈出視窗詢問是否顯示**檔案中所有 QSO**（所有日期）中對方位置距該點 **150 km 以內**者。*是*：清除地圖，並在橙色圓圈內顯示這些 QSO；*否*：地圖保持不變。*返回日期與時段*會回到先前的選擇。

![周邊](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## 詳細資料

對所選路徑：距離和 IK3XTV 表中的評等、雙向方位角、等跳及各反射點的方格、1200 km 以內的短第一跳（IC8TEM 的經驗），以及勾選 foEs 時的最小 foEs。
