# AURORA

[← 首頁](Home-zh-tw)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **極光橢圓區**（漸層顯示）、最近 3 天的 **Kp**、**太陽風**（Bz、速度、密度），每 5 分鐘更新並儲存在 `data/swpc`。
- 與 F.A.I. 相同的檢視：一對電台的 **Scatter trail**，以及用來驗證某一點的**等反射線**，預設高度 110 km。
- **兩個電台的建議天線指向**：沿每個電台的垂直條件線（以該電台顏色顯示的點線），選擇橢圓區機率最高的點。經驗法則（G3WPO）：從約 45° 開始，介於北與東之間。
- 範例 IO70VJ - JO59BC：散射點 JP01AK，位於設得蘭群島外海，偏差 0.97°，QTF 11° 與 299°。
- **地圖上的極光 spot**：文字中含有 **<AU>** 或 **<AUR>** 的 spot 屬於極光 spot（作者規則）。勾選 *在地圖上顯示 <AU>/<AUR> spot 及其計算* 後，此頁籤會繪出最近 2 小時（最多 30 小時）的 spot：對每個 spotter-DX 配對計算 Scatter trail，並顯示最可能的散射點（橘色圓），結果中列出兩個電台的 QTF 與仰角。
- 除了最可能的散射點，結果還會列出兩個電台沿整條 Scatter trail 的 **QTF 範圍**（偏差 ≤ 1.5°）。
- **Lange-Hesse 範圍**：散射點優先位於距各電台 370-1100 km 之內，這是 Lange-Hesse 在地磁緯度 54° 算出的範圍。自 0.4.5 版起，範圍會依各電台的 IGRF-14 磁傾角平移：越南的電台越近，越北的電台越遠（例如卡布里島約 260-990 km，奧斯陸 470-1200 km）。結果會列出兩個電台的範圍。

## BENCHMARK AURORA

AURORA 頁籤有兩個子頁籤：**REAL-TIME**（上述的計算）與 **BENCHMARK AURORA**，後者以 **298 筆含時間與網格座標的實際極光通聯**測試模型：EI5FK 已發表的日誌（IO51RT，愛爾蘭，2003-2005，144 與 432 MHz）以及 PA0O 的日誌地圖（JO33HG，荷蘭）。

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- 程式以相同模型（IGRF-14，高度 110 km，折射 4/3，容許 3°）計算每筆通聯的散射點。
- 顏色：**綠色** = 散射點位於兩個電台的 Lange-Hesse 範圍內；**黃色** = 可解釋但在此範圍外；**橘色** = 超過 2000 km 的通聯；**紅色** = 沒有散射點，模型無法解釋的通聯。
- 摘要列出百分比（也列出以固定範圍 370-1100 km 計算的百分比，以供比較），以及每份日誌計算出的 QTF 範圍與中位數。點選某一列可繪出路徑與兩個天線指向。
- 結果（0.4.18 版，每筆通聯年份的 IGRF-14 磁場）：**298 筆通聯中解釋了 297 筆（100%）**，其中 267 筆（90%）在兩個電台的 Lange-Hesse 範圍內（使用固定範圍 370-1100 km 為 275 筆，92%）；EI5FK 計算出的 QTF 中位數在 32° 至 53° 之間，也就是 G3WPO 建議的東北方。日誌沒有記錄天線指向：驗證的是幾何，而非 QTF。
- **更正**：從 0.4.4 版到 0.4.17 版，測試平台在載入 IGRF-14 之前啟動，以偶極與固定範圍計算；當時公布的結果（243 筆通聯，82%，QTF 中位數 25-41°）是偶極的結果。已在 0.4.18 版修正。
