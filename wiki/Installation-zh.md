# 安装

[← Home](Home-zh)

从 **[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** 页面下载适合的文件。程序和 `data` 文件夹始终一起放在您自己的一个文件夹中：**绝不会向 C:\Users\...\AppData 写入任何文件**。

| 系统 | 文件 | 安装方法 |
|---|---|---|
| Windows 64 位 | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | 双击。安装到 `C:\E-LAYER-ANALYZER`，无需管理员权限。不创建开始菜单快捷方式（它会位于 AppData 中），只有可选的桌面图标。 |
| Windows 32 位 | `...-windows-x86-setup.exe` | 同上，适用于 32 位 Windows。 |
| Windows 便携版 | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | 解压到任意位置，运行 `E-LAYER-ANALYZER.exe`。 |
| Linux（PC） | `...-linux-x64.tar.gz` | 解压后运行 `./installa.sh`：安装到 `~/E-LAYER-ANALYZER`，并在菜单中添加图标。 |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3、4 或 5，需 **64 位 Raspberry Pi OS** 及桌面（推荐 4 GB 内存）。解压后运行 `./installa.sh`。 |
| Mac | `...-mac-apple-silicon.dmg`（M1-M4）或 `...-mac-intel.dmg` | 打开 .dmg，右键单击 **Installa E-LAYER-ANALYZER.command** > 打开。安装到 `~/E-LAYER-ANALYZER`。 |

## 更新

将新版本直接安装在旧版本之上：设置、已收集的 spot、日志和 `call3.txt` 都会保留。

## 系统要求

- 无需安装 .NET：程序已包含所需的一切。
- 需要互联网连接以获取地图（Natural Earth，首次启动时）、NOAA 极光数据、YA-MS 雷达数据以及 telnet 会话。
- 不支持 Raspberry Pi Zero 和 Pi 1（.NET 不支持其处理器）。
