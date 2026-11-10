# DATA COLLECTION

[← Home](Home-zh-TW)

## Telnet 連線

| 連線 | 預設 | 說明 |
|---|---|---|
| 1. 自選的 DX cluster | on0dxk.dyndns.org 8000 | ON0DXK 上的 VHF spot 預設被篩除：`set/filter dxbm/pass 6`（然後 4、2）。另一個例子：hamqth.com 7300。 |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW 和 RTTY，7001 = FT8。 |
| 3. 附 PSKReporter 的 cluster | **dm4x.ddns.net 8500** | DM4X 的 cluster；也可改用 HamAlert（hamalert.org 7300，需要帳號）。 |

- 以呼號自動登入（頂部或 Login 欄位）；**初始指令**以 `;` 分隔。
- 線路中斷時 30 秒後**重新連線**；**啟動時連線**。
- 預設只儲存 **28 MHz 以上**的 spot。

## 已收集的 spot

最近 2000 個 spot，可依頻段和來源篩選。封存：`data/spot` 中每天一個 CSV 檔案。每次關閉程式時，spot 資料庫都會清空（記憶體和 `data/spot` 中的檔案）：每次工作階段都從頭開始。

**本機篩選**（可選方塊）：低於 27990.0 kHz 或高於 145 MHz 的「DX de」行會被捨棄，不在主控台中顯示，也不儲存。**不精確的網格座標**（少於 6 個字元）從 CALL3 取得：如果 CALL3 中有 6 字元的網格座標，一律以它為準。與 YA-MS 相同，流星散射（而非偶發 E 層）spot 是指位於典型 MS 頻率視窗（50190-50300、70100-70400、144290-144399 kHz）且沒有其他傳播標籤（`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`）者，以及使用 MS 專用模式（MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M）或帶有 `<MS>` 者。帶有 `<AS>`/`<ACS>`（aircraft scatter）的 spot 不用於偶發 E 層。

## 篩選欄（右側 40%）

資料收集**全程式只有一個**：每個 spot 都經過這些篩選，再送到各傳播方式的分頁（在 E-LAYER ANALYZER 內不顯示 YA-MS 的資料收集分頁）。頂部是**本機篩選**核取方塊；下方有五個子分頁：

| 子分頁 | 可設定的內容 | 預設值（作者規則） |
|---|---|---|
| 流星散射 | YA-MS 的篩選：MS 頻率視窗、其他傳播標籤、MS 模式和 `<MS>`、頻段 | 與 YA-MS 相同 |
| 偶發 E 層 | 排除 spot 的 `< >` 標籤、144 MHz 規則、跨赤道規則 | `<MS> <AU> <TR> <AS> <ACS>`…；900 km、5 min、300 km、50 MHz 短跳 1200 km/15 min；±20° |
| F.A.I. | F.A.I. 關鍵字、頻段、地圖上的時段 | FAI、所有頻段、24 小時 |
| 極光 | 極光 `< >` 標籤、頻段、地圖上的時段 | `AU` `AUR`（有無 `< >` 皆可）、所有頻段、2 小時 |
| 篩選測試 | 貼上一行 cluster 資料，查看所有篩選如何分類它 | |

**恢復作者規則**按鈕可還原預設值。

## CALL3

JTDX 的**呼號 → 網格座標**資料庫（超過一百萬個呼號），位於 `data/call3.txt`。按鈕：**重新載入**和**匯入新的 CALL3.TXT**。從 spot 註解中讀到的 6 字元網格座標會加入 `data/call3_nuovi.txt`。

## Spot 與傳播資料子分頁

此分頁有兩個子分頁：**Spot**（連線、已收集的 spot、CALL3 和篩選欄）以及**傳播資料**：

- **N0NBH**（hamqsl.com）：VHF 橫幅和太陽資料、歐洲和北美的 E-skip（DXrobot）、極光。
- **極光用的 Bz**：NOAA SWPC 的即時太陽風（DSCOVR/ACE，L1 點），是 Bz 的最佳來源。
- **流星散射**：Make More Miles on VHF 提供的每日流星活動（RMOB）。

這些資料最多每小時下載一次。摘要會顯示在 SPORADIC-E、F.A.I. 和 AURORA 的地圖上。分頁列右側的**洲別選單**會移動地圖並選取該洲的資料。
