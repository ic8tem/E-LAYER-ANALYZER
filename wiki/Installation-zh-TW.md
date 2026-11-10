# 安裝

[← Home](Home-zh-TW)

請從 **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** 頁面下載適合的檔案。程式和 `data` 資料夾始終放在一起，位於您自己的資料夾中：**任何檔案都不會寫入 C:\Users\...\AppData**。

| 系統 | 檔案 | 安裝方式 |
|---|---|---|
| Windows 64 位元 | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | 按兩下即可。安裝到 `C:\E-LAYER-ANALYZER`，不需要系統管理員權限。不會在「開始」功能表建立捷徑（捷徑會位於 AppData），只有可選的桌面圖示。 |
| Windows 32 位元 | `...-win-x86-setup.exe` | 同上，適用於 32 位元 Windows。 |
| Windows 可攜版 | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | 解壓縮到任意位置，執行 `E-LAYER-ANALYZER.exe`。 |
| Linux 64 位元（PC） | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`，然後從選單啟動：首次啟動時程式會把自己複製到 `~/E-LAYER-ANALYZER`，`data` 資料夾就在旁邊。 |
| Linux 可攜版 | `...-linux-x64-portatile.tar.gz` | 解壓縮後執行 `./E-LAYER-ANALYZER`，或執行 `./installa.sh`：安裝到 `~/E-LAYER-ANALYZER`，並在選單中建立圖示。 |
| Raspberry Pi（64 位元） | `e-layer-analyzer_x.y.z_arm64.deb` 或 `...-linux-arm64-portatile.tar.gz` | Pi 3、4 或 5，搭配 **64 位元 Raspberry Pi OS** 和桌面環境（建議 4 GB RAM）。.deb 同上；可攜版同 Linux。 |
| Raspberry Pi（32 位元） | `...-linux-arm-portatile.tar.gz` | Pi 2、3 或 4，搭配 **32 位元 Raspberry Pi OS**。解壓縮後執行 `./E-LAYER-ANALYZER` 或 `./installa.sh`。 |
| Mac | `...-osx-arm64.dmg`（Apple Silicon M1-M4）或 `...-osx-x64.dmg`（Intel） | 開啟 .dmg，在 **Installa E-LAYER-ANALYZER.command** 上按右鍵 > 開啟。安裝到 `~/E-LAYER-ANALYZER`。 |
| Mac，僅應用程式 | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | 把 **E-LAYER-ANALYZER.app** 解壓縮到您自己的資料夾（例如 `~/E-LAYER-ANALYZER`，不要放在「應用程式」）：`data` 資料夾會建立在 app 旁邊。首次啟動時按右鍵 > 開啟。 |

## 更新

直接把新版本安裝在舊版本上：設定、已收集的 spot、日誌和 `call3.txt` 都會保留。

## 系統需求

- 不需要安裝 .NET：程式本身已包含所需的一切。
- 需要網路連線，用於地圖（Natural Earth，首次啟動時）、NOAA 極光資料、YA-MS 的雷達以及 telnet 連線。
- 不支援 Raspberry Pi Zero 和 Pi 1（.NET 不支援它們的處理器）。
