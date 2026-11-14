# DATA COLLECTION

[← 首頁](Home-zh-tw)

## Telnet 連線階段

| 連線階段 | 預設 | 說明 |
|---|---|---|
| 1. 自選 DX cluster | on0dxk.dyndns.org 8000 | ON0DXK 上的 VHF spot 已被過濾：`set/filter dxbm/pass 6`（再來 4、2）。另一個例子：hamqth.com 7300。 |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW 與 RTTY，7001 = FT8。 |
| 3. 含 PSKReporter 的 Cluster | **dm4x.ddns.net 8500** | DM4X 的 Cluster；另一選擇是 HamAlert（hamalert.org 7300，需要帳號）。 |

- 以呼號自動登入（上方或 登入 欄位中的呼號）；**初始指令**以 `;` 分隔。
- 斷線時 30 s 後**重新連線**；**啟動時連線**。
- 預設只儲存 **28 MHz 以上**的 spot。

## 已收集的 spot

最近 2000 筆 spot，可依頻段與來源篩選。存檔：`data/spot` 中每天一個 CSV 檔案。每次關閉程式時，spot 資料庫都會清空（記憶體與 `data/spot` 中的檔案）：每次連線階段都從頭開始。

**本機篩選**（選用勾選框）：低於 27990.0 kHz 或高於 145 MHz 的 "DX de" 行會被捨棄，不會顯示在主控台，也不會儲存。**不精確的網格座標**（少於 6 碼）從 CALL3 取得：若 CALL3 有 6 碼的網格座標，一律以其為準。與 YA-MS 相同，落在 MS 典型頻率窗內（50190-50300、70100-70400、144290-144399 kHz）且沒有其他傳播方式標記（`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`）的 spot，以及使用 MS 專屬模式（MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M）或帶有 `<MS>` 的 spot，屬於流星散射（而非散塊 E 層）。帶有 `<AS>`/`<ACS>`（飛機散射）的 spot 不會用於散塊 E 層。

## 篩選器欄（右側 40%）

整個程式**共用同一套**資料收集：每一筆 spot 都會經過這些篩選器，再送到各傳播模式的頁籤（在 E-LAYER ANALYZER 內不會顯示 YA-MS 的 資料收集 頁籤）。最上方是**本機篩選**勾選框；下方有五個子頁籤：

| 子頁籤 | 設定內容 | 預設值（作者規則） |
|---|---|---|
| 流星散射 | YA-MS 的篩選器：MS 頻率窗、其他傳播方式標記、MS 模式與 `<MS>`、頻段 | 與 YA-MS 相同 |
| 散塊 E 層 | 排除 spot 的 `< >` 標記、144 MHz 規則、跨赤道規則 | `<MS> <AU> <TR> <AS> <ACS>`…；900 km、5 分鐘、300 km、50 MHz 短跳 1200 km/15 分鐘；±20° |
| F.A.I. | F.A.I. 字詞、頻段、地圖上的時間範圍 | FAI、所有頻段、24 小時 |
| 極光 | 極光 `< >` 標記、頻段、地圖上的時間範圍 | `AU` `AUR`（加不加 `< >` 皆可）、所有頻段、2 小時 |
| 篩選器測試 | 貼上一行 Cluster 內容，查看所有篩選器如何分類它 | |

**恢復作者規則** 按鈕會恢復預設值。

## CALL3

JTDX 的**呼號 → 網格座標**資料庫（超過一百萬個呼號），位於 `data/call3.txt`。按鈕：**重新載入** 與 **匯入新的 CALL3.TXT**。在 spot 備註中讀到的 6 碼網格座標會加入 `data/call3_nuovi.txt`。

從 0.4.16 版起，整個程式只有一個 CALL3 資料庫，即 YA-MS 資料收集（METEOR SCATTER 分頁）中的資料庫，規則與 LIVETROPOVHF 相同：從 MMMonVHF 更新、手動條目、加入的檔案、叢集修正、學到的定位碼以及每 6 小時一次的 G7VRD；修正寫入 data/call3.txt，data/call3_nuovi.txt 中的定位碼匯入一次。WSJT-X、JTDX 和 MSHV 的報告也可透過 UDP 接收（來源 WSJT-X UDP，連接埠 2237，轉送到 2238）。

這些來自 YA-MS 的過濾條件（CW、最小距離、頻率視窗、MS 模式、波段）只適用於 METEOR SCATTER 分頁：它們決定哪些報告進入該分頁的收集。其他分頁接收全部報告並使用各自的過濾條件。

## spot 與 傳播資料 子頁籤

此頁籤有兩個子頁籤：**spot**（連線階段、已收集的 spot、CALL3 與篩選器欄）與 **傳播資料**：

- **N0NBH**（hamqsl.com）：VHF 橫幅與太陽資料、歐洲與北美洲的 E-skip（DXrobot）、極光。
- **極光用的 Bz**：NOAA SWPC 即時太陽風（DSCOVR/ACE，L1 點），Bz 的最佳來源。
- **流星散射**：Make More Miles on VHF 的當日流星活動（RMOB）。

這些資料最多每小時下載一次。摘要會顯示在 SPORADIC-E、F.A.I. 與 AURORA 地圖上。頁籤列右側的**洲別選單**會移動地圖並選擇該洲的資料。
