# AURORA

[← Home](Home-zh)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **极光椭圆**（阴影区）、最近 3 天的 **Kp**、**太阳风**（Bz、速度、密度），每 5 分钟更新一次并保存在 `data/swpc` 中。
- 与 F.A.I. 相同的视图：针对一对台站的 **Scatter trail**，以及用于验证某一点的**等反射线**，默认高度为 110 km。
- **两个台站的推荐天线指向**：沿每个台站的垂直线（以该台站颜色的点表示）取极光椭圆概率最高的点。经验法则（G3WPO）：从大约 45° 开始，即北与东之间。
- 示例 IO70VJ - JO59BC：散射点 JP01AK 位于设得兰群岛外海，偏差 0.97°，QTF 11° 和 299°。
- **地图上的极光 spot**：文本中带有 **<AU>** 或 **<AUR>** 的 spot 为极光 spot（作者规则）。勾选*在地图上显示 <AU>/<AUR> spot 并计算*后，本选项卡会绘出最近 2 小时内的 spot（最多 30 条）：对每对报告者-DX，计算其 Scatter trail，并显示最可能的散射点（橙色圆），结果中给出两个台站的 QTF 和仰角。
- 除最可能的散射点外，结果还给出两个台站沿整条 Scatter trail 的 **QTF 范围**（偏差 ≤ 1.5°）。
- **Lange-Hesse 窗口**：散射点优先位于距每个台站 370-1100 km 之内，这是 Lange-Hesse 在地磁纬度 54° 处算出的窗口。从 0.4.5 版起，窗口随每个台站的 IGRF-14 倾角平移：越往南的台站越近，越往北的越远（例如卡普里约 260-990 km，奥斯陆 470-1200 km）。结果中给出两个台站的窗口。

## BENCHMARK AURORA

AURORA 选项卡有两个子选项卡：**REAL-TIME**（上述计算）和 **BENCHMARK AURORA**，后者用 **298 个带时间和网格坐标的真实极光 QSO** 检验模型：EI5FK 已发表的日志（IO51RT，爱尔兰，2003-2005，144 和 432 MHz）以及 PA0O 的日志地图（JO33HG，荷兰）。

![BENCHMARK AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/4k_benchmark_aurora.png)

- 程序用同一模型（IGRF-14，高度 110 km，4/3 折射，容差 3°）计算每个 QSO 的散射点。
- 颜色：**绿色** = 散射点位于两站的 Lange-Hesse 窗口内；**黄色** = 可以解释，但在此窗口之外；**橙色** = 超过 2000 km 的 QSO；**红色** = 没有散射点，模型无法解释的 QSO。
- 摘要给出各项百分比（也给出按固定窗口 370-1100 km 计算的百分比，以作比较），以及每个日志计算得到的 QTF 的范围和中位数。点击某一行即可绘出路径和两个天线指向。
- 结果（0.4.4 版）：**298 个 QSO 中有 296 个得到解释（99%）**，其中 243 个（82%）在 370-1100 km 窗口内；EI5FK 的 QTF 中位数在 25° 至 41° 之间，也就是 G3WPO 建议的东北方向。日志中没有记录天线指向：检验的是几何，而不是 QTF。 使用移动窗口（0.4.5 版）结果相同，与固定窗口一样为 243 个 QSO（82%）：测试平台的台站（爱尔兰、英国、荷兰、德国）倾角为 66-70°，接近 Lange-Hesse 的参考点（EI5FK 的窗口为 350-1080 km）；差别出现在更南或更北的台站，而测试平台没有这些台站的 QSO。
