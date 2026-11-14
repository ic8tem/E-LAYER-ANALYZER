# DATA COLLECTION

[← Home](Home-zh)

## Telnet 会话

| 会话 | 默认 | 说明 |
|---|---|---|
| 1. 自选 DX cluster | on0dxk.dyndns.org 8000 | ON0DXK 默认过滤了 VHF spot：`set/filter dxbm/pass 6`（然后 4、2）。另一个例子：hamqth.com 7300。 |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW 和 RTTY，7001 = FT8。 |
| 3. 含 PSKReporter 的 cluster | **dm4x.ddns.net 8500** | DM4X 的 cluster；也可以用 HamAlert（hamalert.org 7300，需注册账号）。 |

- 使用呼号自动登录（填写在顶部或“登录”字段中）；**初始命令**以 `;` 分隔。
- 线路中断后 30 s **重新连接**；**启动时连接**。
- 默认只保存 **28 MHz 及以上**的 spot。

## 已收集的 spot

最近的 2000 条 spot，可按频段和来源过滤。档案：`data/spot` 中每天一个 CSV 文件。每次程序关闭时，spot 数据库都会被清空（内存以及 `data/spot` 中的文件）：每次会话都从头开始。

**本地过滤**（可选复选框，位于“设置”中）：低于 27990.0 kHz 或高于 145 MHz 的 "DX de" 行会被丢弃，不在控制台显示，也不保存；在 430 至 440 MHz 之间只放行极光 spot。**不精确的网格坐标**（少于 6 位）从 CALL3 获取：如果 CALL3 中有 6 位网格坐标，则始终以它为准。与 YA-MS 相同，流星散射（而非偶发 E 层）spot 是指位于 MS 典型频率窗口（50190-50300、70100-70400、144290-144399 kHz）内且没有其他传播方式标签（`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`）的 spot，以及使用 MS 专用模式（MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M）或带有 `<MS>` 的 spot。例外：50 和 70 MHz 的 FT8 和 FT4，由已知的模式决定（70.154 MHz 的 FT8 即使在 MS 窗口内也归入偶发 E 层）。带有 `<AS>`/`<ACS>`（飞机散射）的 spot 不用于偶发 E 层。

## spot 过滤

数据收集**全程序只有一个**：每条 spot 都经过过滤，再送到各传播方式的选项卡。从 0.4.18 版起，过滤、本地过滤和过滤测试位于 **[设置](Settings-zh)** 选项卡的 **spot 分流** 子选项卡。

## CALL3

JTDX 的**呼号 → 网格坐标**数据库（超过一百万个呼号），位于 `data/call3.txt`。按钮：**重新载入**和**导入新的 CALL3.TXT**。从 spot 注释中读到的 6 位网格坐标会添加到 `data/call3_nuovi.txt` 中。

从 0.4.16 版起，整个程序只有一个 CALL3 数据库，即 YA-MS 数据收集（METEOR SCATTER 选项卡）中的数据库，规则与 LIVETROPOVHF 相同：从 MMMonVHF 更新、手工条目、添加的文件、集群修正、学到的定位符以及每 6 小时一次的 G7VRD；修正写入 data/call3.txt，data/call3_nuovi.txt 中的定位符导入一次。WSJT-X、JTDX 和 MSHV 的报告也可通过 UDP 接收（来源 WSJT-X UDP，端口 2237，转发到 2238）。

这些来自 YA-MS 的过滤条件（CW、最小距离、频率窗口、MS 模式、波段）只适用于 METEOR SCATTER 选项卡：它们决定哪些报告进入该选项卡的收集。其他选项卡接收全部报告并使用各自的过滤条件。

## Spot 和传播数据子选项卡

该选项卡有两个子选项卡：**Spot**（会话、PSKReporter MQTT、WSJT-X UDP、已收集的 spot 和 CALL3）和 **传播数据**：

- **N0NBH**（hamqsl.com）：VHF 横幅和太阳数据、欧洲和北美洲的 E-skip（DXrobot）、极光。
- **极光用的 Bz**：NOAA SWPC 的实时太阳风（DSCOVR/ACE，L1 点），Bz 的最佳来源。
- **流星散射**：Make More Miles on VHF 的当日流星活动（RMOB）。

最多每小时下载一次。摘要显示在 SPORADIC-E、F.A.I. 和 AURORA 地图上。选项卡栏右侧的**大洲菜单**可移动地图并选择该大洲的数据。
