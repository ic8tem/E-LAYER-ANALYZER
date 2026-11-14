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

**本地过滤**（可选复选框）：低于 27990.0 kHz 或高于 145 MHz 的 "DX de" 行会被丢弃，既不在控制台中显示，也不保存。**不精确的网格坐标**（少于 6 位）从 CALL3 中获取：如果 CALL3 中有 6 位网格坐标，则始终以其为准。与 YA-MS 相同，流星散射（而非偶发 E 层）spot 是指：落在典型 MS 频率窗口（50190-50300、70100-70400、144290-144399 kHz）内且不带其他传播方式标签（`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`）的 spot，以及使用仅用于 MS 的模式（MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M）或带有 `<MS>` 的 spot。带 `<AS>`/`<ACS>`（飞机散射）的 spot 不用于偶发 E 层分析。

## CALL3

JTDX 的**呼号 → 网格坐标**数据库（超过一百万个呼号），位于 `data/call3.txt`。按钮：**重新载入**和**导入新的 CALL3.TXT**。从 spot 注释中读到的 6 位网格坐标会添加到 `data/call3_nuovi.txt` 中。

从 0.4.16 版起，整个程序只有一个 CALL3 数据库，即 YA-MS 数据收集（METEOR SCATTER 选项卡）中的数据库，规则与 LIVETROPOVHF 相同：从 MMMonVHF 更新、手工条目、添加的文件、集群修正、学到的定位符以及每 6 小时一次的 G7VRD；修正写入 data/call3.txt，data/call3_nuovi.txt 中的定位符导入一次。WSJT-X、JTDX 和 MSHV 的报告也可通过 UDP 接收（来源 WSJT-X UDP，端口 2237，转发到 2238）。
