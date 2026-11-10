# BENCHMARK 50 MHz

[← Home](Home-zh)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

使用一个台站的 ADIF 日志（以及可选的第二个台站的日志）对 **50 MHz 多跳偶发 E 层**进行测试的平台。

## 载入日志

- **台站 1**：*载入 ADIF* / *移除 ADIF*。程序从日志中读取 `STATION_CALLSIGN` 和 `MY_GRIDSQUARE`；发射位置为每个 QSO 的 `MY_GRIDSQUARE`。
- **台站 2**（可选）：第二行上的相同按钮；*台站 2* 复选框用于显示或隐藏它。台站 1 的路径为浅蓝色，台站 2 的路径为紫色。
- 程序使用带网格坐标的 50 MHz QSO（缺失或只有 4 位的网格坐标从 CALL3 中获取），且为多跳：超过 3100 km（作者规则；跳数依据 IK3XTV 表）。
- **跨赤道**通联会被排除（作者规则）：朝向非洲和南美洲、且中点位于磁倾角 -20° 至 +20° 带内的通联，除非日志中注明了 `PROP_MODE=ES`。

## 日期与时段

选择**日期**（打开时默认为多跳 QSO 最多的那一天）以及 UTC 时段**从… 到…**，步长 15 分钟。**foEs** 复选框（可选）按最低 foEs 为路径着色，并在表格中显示该值。

## 地图

中心可自由移动的**墨卡托**地图：滚轮 = 缩放，拖动 = 平移。

![路径](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- 一个**模糊的黄色圆点**标出每一跳的中心，即推定 Es 云所在之处；鼠标下方路径上的圆点会变大。
- **鼠标靠近路径**：线条和图标放大，并显示对方电台的 **6 位 WWL 方格**。
- 在该路径上**单击左键**：弹出窗口询问是否显示**文件中所有**对端位置在该点 **150 km 以内**的 QSO（所有日期）。*是*：地图被清空，并在橙色圆内显示这些 QSO；*否*：地图保持不变。*返回日期和时段*可回到之前的选择。

![附近](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## 详情

对所选路径给出：距离及其在 IK3XTV 表中的评级、双向方位、等长跳及各反射点所在方格、1200 km 以内的短首跳（IC8TEM 经验），以及勾选 foEs 复选框时的最低 foEs。
