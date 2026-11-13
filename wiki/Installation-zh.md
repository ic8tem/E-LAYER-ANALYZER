# 安装

[← Home](Home-zh)

从 **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** 页面下载适合的文件。程序和 `data` 文件夹始终一起放在您自己的一个文件夹中：**绝不会向 C:\Users\...\AppData 写入任何文件**。

| 系统 | 文件 | 安装方法 |
|---|---|---|
| Windows 64 位 | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | 双击。安装到 `C:\E-LAYER-ANALYZER`，无需管理员权限。不创建开始菜单快捷方式（它会位于 AppData 中），只有可选的桌面图标。 |
| Windows 32 位 | `...-win-x86-setup.exe` | 同上，适用于 32 位 Windows。 |
| Windows 便携版 | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | 解压到任意位置，运行 `E-LAYER-ANALYZER.exe`。 |
| Linux 64 位（PC） | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb`，然后从菜单启动：首次启动时程序会将自身复制到 `~/E-LAYER-ANALYZER`，`data` 文件夹位于其旁边。 |
| Linux 便携版 | `...-linux-x64-portatile.tar.gz` | 解压后运行 `./E-LAYER-ANALYZER`，或运行 `./installa.sh`：安装到 `~/E-LAYER-ANALYZER`，并在菜单中添加图标。 |
| Raspberry Pi（64 位） | `e-layer-analyzer_x.y.z_arm64.deb` 或 `...-linux-arm64-portatile.tar.gz` | Pi 3、4 或 5，需 **64 位 Raspberry Pi OS** 及桌面（推荐 4 GB 内存）。.deb 同上；便携版同 Linux。 |
| Raspberry Pi（32 位） | `e-layer-analyzer_x.y.z_armhf.deb` 或 `...-linux-arm-portatile.tar.gz` | Pi 2、3 或 4，需 **32 位 Raspberry Pi OS**。.deb 同上；便携版：解压后运行 `./E-LAYER-ANALYZER` 或 `./installa.sh`。 |
| 使用 Slackware ARM64 的 Raspberry Pi | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` 或 `...-linux-arm64-portatile.tar.gz` | 以 root 身份：`installpkg e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz`，然后从菜单或终端启动 `e-layer-analyzer`：首次启动时程序会将自身复制到 `~/E-LAYER-ANALYZER`。便携版同 Linux。 |
| Linux armv7l（其他 32 位 ARM 板） | `...-linux-armv7l-portatile.tar.gz` | 同 Linux 便携版。 |
| Mac | `...-osx-arm64.dmg`（Apple Silicon M1-M4）或 `...-osx-x64.dmg`（Intel） | 打开 .dmg，右键单击 **Installa E-LAYER-ANALYZER.command** > 打开。安装到 `~/E-LAYER-ANALYZER`。 |
| Mac，仅应用程序 | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | 将 **E-LAYER-ANALYZER.app** 解压到您自己的一个文件夹（例如 `~/E-LAYER-ANALYZER`，不要放在 Applications）：`data` 文件夹会在应用旁边创建。首次启动时右键单击 > 打开。 |

## 更新

将新版本直接安装在旧版本之上：设置、已收集的 spot、日志和 `call3.txt` 都会保留。

**自动更新（从 0.4.5 版起）。** 程序在启动时以及之后每 6 小时检查 GitHub 的 Releases。有新版本时，会打开一个窗口，并在顶部栏的 NTP 指示旁显示橙色的 **▲ x.y.z**：

**从 0.4.12 版起，更新为自动且静默**：GitHub 上有新版本时，程序会下载、安装并自动重新启动，不询问任何问题。缺失的文件也会在不询问的情况下重新下载。只有点击顶部的版本号，或在 data/ela.ini 文件中写入 agg_automatici=0 时，才会出现更新窗口。

- **立即更新** 会下载适合你电脑的便携版软件包（Windows 64/32 位、Linux、Raspberry Pi、Mac），关闭程序，替换文件并重新启动。设置、已收集的 spot、日志和 `call3.txt` 都会保留。
- **稍后** 会在下次检查时再次询问；**跳过此版本** 则不再提示该版本。
- 点击顶部栏中显示的版本（例如 `v0.4.5`）会立即检查更新。可以用同一窗口中的复选框关闭自动检查。
- 如果程序文件夹不可写（例如系统文件夹），请改用 Release 页面上的安装包。

**缺失的文件。** 启动时，程序会检查随程序分发的所有文件（翻译、文档、表格，列于 `data/manifest.txt`）是否齐全。如有缺失，会弹出窗口列出这些文件，**从 GitHub 下载** 会从 GitHub 上的同一版本重新获取它们。

## 系统要求

- 无需安装 .NET：程序已包含所需的一切。
- 需要互联网连接以获取地图（Natural Earth，首次启动时）、NOAA 极光数据、YA-MS 雷达数据以及 telnet 会话。
- 不支持 Raspberry Pi Zero 和 Pi 1（.NET 不支持其处理器）。
- 没有适用于 Linux i686（32 位 PC）的版本，因为该系统没有 .NET；也没有适用于 FreeBSD 的版本，因为那里缺少 Avalonia 的图形库。
