# 安裝

[← 首頁](Home-zh-tw)

請從 **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** 頁面下載適合的檔案。程式與 `data` 資料夾永遠一起放在你自己的資料夾中：**絕不會有任何檔案寫入 C:\Users\...\AppData**。

| 系統 | 檔案 | 安裝方式 |
|---|---|---|
| Windows 64 位元 | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | 按兩下即可。安裝到 `C:\E-LAYER-ANALYZER`，不需要系統管理員權限。不會建立「開始」功能表捷徑（那會放在 AppData 中），只有可選的桌面圖示。 |
| Windows 32 位元 | `...-win-x86-setup.exe` | 同上，適用於 32 位元 Windows。 |
| Windows on ARM（Snapdragon 電腦） | `...-win-arm64-setup.exe` | 同上，適用於 ARM 處理器的 Windows。 |
| Windows 免安裝版 | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | 解壓縮到任何位置，執行 `E-LAYER-ANALYZER.exe`。 |
| Debian、Ubuntu、Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`，然後從選單啟動：首次啟動時，程式會將自己複製到 `~/E-LAYER-ANALYZER`，`data` 資料夾就在旁邊。 |
| Fedora、openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm`（另有 `.aarch64.rpm`、`.armv7hl.rpm`） | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm`（或 `zypper install`），之後同 .deb。 |
| Arch、Manjaro、EndeavourOS、Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`（或 `-aarch64`） | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`，之後同 .deb。 |
| Slackware（x86_64 與 ARM64） | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`（或 `-aarch64-`） | 以 root 身分：`installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz`，然後從選單或終端機啟動 `e-layer-analyzer`：首次啟動時，程式會將自己複製到 `~/E-LAYER-ANALYZER`。 |
| 任何 Linux（AppImage） | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage`（或 `-aarch64.AppImage`） | 設為可執行後執行：首次啟動時會將程式複製到 `~/E-LAYER-ANALYZER`，並從那裡執行。 |
| Linux 免安裝版 | `...-linux-x64-portatile.tar.gz` | 解壓縮後執行 `./E-LAYER-ANALYZER`，或執行 `./installa.sh`：安裝到 `~/E-LAYER-ANALYZER`，並在選單中建立圖示。 |
| Alpine Linux（musl） | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig`，之後同 Linux 免安裝版。 |
| Raspberry Pi（64 位元） | `e-layer-analyzer_x.y.z_arm64.deb` 或 `...-linux-arm64-portatile.tar.gz` | Pi 3、4 或 5，使用含桌面的 **64 位元 Raspberry Pi OS**（建議 4 GB 記憶體）。.deb 安裝方式同上；免安裝版同 Linux。 |
| Raspberry Pi（32 位元） | `e-layer-analyzer_x.y.z_armhf.deb` 或 `...-linux-arm-portatile.tar.gz` | Pi 2、3 或 4，使用 **32 位元 Raspberry Pi OS**。.deb 安裝方式同上；免安裝版：解壓縮後執行 `./E-LAYER-ANALYZER` 或 `./installa.sh`。 |
| 使用 Slackware ARM64 的 Raspberry Pi | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` 或 `...-linux-arm64-portatile.tar.gz` | 同上方的 Slackware。免安裝版同 Linux。 |
| Linux armv7l（其他 32 位元 ARM 板） | `...-linux-armv7l-portatile.tar.gz` | 同 Linux 免安裝版。 |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **實驗性，未經驗證。** 需要社群版 .NET 8（`pkg install dotnet`），以及為 FreeBSD 建置的 SkiaSharp 與 HarfBuzzSharp 原生函式庫，而 Avalonia 不提供這些函式庫：沒有它們程式無法啟動。請見套件中的 `LEGGIMI-FREEBSD.txt`。 |
| Mac | `...-osx-arm64.dmg`（Apple Silicon M1-M4）或 `...-osx-x64.dmg`（Intel） | 開啟 .dmg，在 **Installa E-LAYER-ANALYZER.command** 上按右鍵 > 開啟。安裝到 `~/E-LAYER-ANALYZER`。 |
| Mac，僅應用程式 | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | 將 **E-LAYER-ANALYZER.app** 解壓縮到你自己的資料夾（例如 `~/E-LAYER-ANALYZER`，不要放在「應用程式」）：`data` 資料夾會建立在 app 旁邊。首次啟動時按右鍵 > 開啟。 |

## 更新

將新版本直接安裝在舊版本上：設定、已收集的 spot、日誌與 `call3.txt` 都會保留。

**系統套件（.deb、.rpm、Arch、Slackware）不會自動更新。** 使用這些套件時，程式只會提示新版本：請從 Releases 頁面下載新套件，並像第一次一樣安裝（`sudo apt install ./...deb`、`sudo dnf install ./...rpm`、`sudo pacman -U ./...pkg.tar.zst`、`upgradepkg ...txz`）。自動更新適用於 Windows 安裝程式與 zip、Mac 的 app、免安裝的 tar.gz 套件與 AppImage。

**自動更新（從 0.4.5 版起）。** 程式在啟動時以及之後每 6 小時檢查 GitHub 的 Releases。有新版本時，會開啟一個視窗，並在上方工具列的 NTP 指示旁顯示橘色的 **▲ x.y.z**：

**自 0.4.12 版起，更新為自動且靜默**：GitHub 上有新版本時，程式會下載、安裝並自動重新啟動，不詢問任何問題。缺少的檔案也會在不詢問的情況下重新下載。只有點擊頂部的版本號，或在 設定（一般）中關閉自動更新時，才會出現更新視窗。

- **立即更新** 會下載適用於你電腦的免安裝版套件（Windows 64/32 位元、Linux、Raspberry Pi、Mac），關閉程式，替換檔案並重新啟動。設定、已收集的 spot、日誌與 `call3.txt` 都會保留。
- **稍後** 會在下次檢查時再次詢問；**略過此版本** 則不再提示該版本。
- 按一下上方工具列中顯示的版本（例如 `v0.4.5`）會立即檢查更新。可以用同一視窗中的核取方塊關閉自動檢查。
- 如果程式資料夾無法寫入（例如系統資料夾），請改用 Release 頁面上的安裝套件。

**缺少的檔案。** 啟動時，程式會檢查隨程式發布的所有檔案（翻譯、文件、表格，列於 `data/manifest.txt`）是否齊全。若有缺少，會以視窗列出這些檔案，**從 GitHub 下載** 會從 GitHub 上的同一版本重新取得它們。

## 系統需求

- 不需要安裝 .NET：程式已包含所需的一切。
- 需要網際網路連線，用於地圖（Natural Earth，首次啟動時）、NOAA 極光資料、YA-MS 的雷達與 telnet 連線階段。
- 不支援 Raspberry Pi Zero 與 Pi 1（.NET 不支援它們的處理器）。
- 沒有適用於 Linux i686（32 位元 PC）與 OpenBSD 的版本，因為這些系統沒有 .NET。FreeBSD 只有實驗性套件。
