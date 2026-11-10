# BENCHMARK 50 MHz

[← 首頁](Home-zh-tw)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

使用一個電台（並可選擇加入第二個電台）的 ADIF 日誌，測試 **50 MHz 多跳散塊 E 層**的平台。

## 載入日誌

- **電台 1**：*載入 ADIF* / *移除 ADIF*。程式從日誌中取得 `STATION_CALLSIGN` 與 `MY_GRIDSQUARE`；發射位置是每筆通聯的 `MY_GRIDSQUARE`。
- **電台 2**（選用）：第二列有相同的按鈕；*電台 2* 勾選框可顯示或隱藏它。電台 1 的路徑為淺藍色，電台 2 的路徑為紫色。
- 程式使用有網格座標的 50 MHz 通聯（缺少或只有 4 碼的網格座標從 CALL3 取得），且為多跳：超過 3100 km（作者規則；跳數依循 IK3XTV 表）。
- 排除**跨赤道**通聯（作者規則）：朝向非洲與南美洲、中點位於磁傾角 -20° 到 +20° 的帶狀區域內者，除非日誌中有 `PROP_MODE=ES`。

## 日期與時段

選擇**日期**（開啟時為多跳通聯最多的那一天）與 UTC 時段 **從… 到…**，以 15 分鐘為單位。**foEs** 勾選框（選用）會依最低 foEs 為路徑著色，並列於表格中。

## 地圖

中心可自由設定的**麥卡托**地圖：滾輪 = 縮放，拖曳 = 平移。

![路徑](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **模糊的黃色圓點**標示每一跳的中心，即推定 Es 雲所在處；在滑鼠下方的路徑上，圓點會變大。
- **滑鼠靠近路徑**：線條與圖示會放大，並顯示對方電台的 **6 碼 WWL 網格**。
- 在該路徑上**按左鍵**：會跳出視窗詢問是否顯示**檔案中所有通聯**（所有日期）中，終點位於該點 **150 km 以內**者。*是*：清空地圖並在橘色圓內顯示這些通聯；*否*：地圖保持不變。*回到日期與時段* 會回到先前的選擇。

![附近](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## 詳細資訊

所選路徑的：距離與 IK3XTV 表的分類、雙向方位角、等距跳及各反射點所在網格、1200 km 以內的短首跳（IC8TEM 的經驗），以及勾選 foEs 時的最低 foEs。
