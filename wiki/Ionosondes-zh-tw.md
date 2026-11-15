# 電離層探測儀

[← Home](Home-zh-tw)

![IONOSONDE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

**SPORADIC-E** 的子頁籤（自 0.5.2 版起）：全球 **GIRO** 網路探測儀**實測的 foEs**，與程式由 spot 推得的值並列顯示。

## 資料

- 62 台 GIRO 探測儀（ULCAR 清單），附有從電離圖自動判讀的最新 **foEs**、**foF2** 與 **MUF(D)**。
- 資料在第一次開啟頁籤時送達，之後**每 15 分鐘**自動更新（核取方塊 *每 15 分鐘自動更新*）；*立即更新* 可立即取得。檔案庫在資料資料夾的 `giro` 中保存最近 48 小時。
- **GIRO** 的資料（Reinisch 與 Galkin 2011），授權 **CC BY-NC-SA 4.0**：僅限非商業用途。服務與 DM6AX 的 *MUF Muncher* 相同。

## 地圖與表格

- 每台探測儀是一個依最新 foEs 著色的圓點（顏色與 Es 雲相同），並標出以 MHz 為單位的數值；沒有最近資料時為**灰色**，最後一張電離圖超過 90 分鐘時帶**圓環**。
- 表格：代碼、探測儀、與你電台的 km、最後一張電離圖的 UTC、foEs、foF2、MUF(D) 與 **spot foEs≥**：由最近 30 分鐘內在探測儀 500 km 以內（可調整）反射的 Es spot 推得的最低 foEs，以及 spot 的數量。

## 所選探測儀

點選某一列或地圖上探測儀附近：

- **最近 24 小時的圖表**：foEs 黃色，foF2 淺藍色，MUF(D) 紫色點線；
- 1000、1500、2000 與 2400 km 處**散塊 E 層的 MUF**（MUF = foEs · sec φ，依所選 Es 高度）以及**開通的頻段**（28、40、50、70、144 MHz）；
- 若 spot 要求的值高於實測 foEs，程式會提示：雲可能遠離探測儀，或探測儀已到達其頻率範圍的極限（Chartier 2022）。
