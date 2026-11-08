# AURORA

[← Home](Home-zh)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime **极光椭圆**（阴影区）、最近 3 天的 **Kp**、**太阳风**（Bz、速度、密度），每 5 分钟更新一次并保存在 `data/swpc` 中。
- 与 F.A.I. 相同的视图：针对一对台站的 **Scatter trail**，以及用于验证某一点的**等反射线**，默认高度为 105 km。
- **两个台站的推荐天线指向**：沿每个台站的垂直线（以该台站颜色的点表示）取极光椭圆概率最高的点。经验法则（G3WPO）：从大约 45° 开始，即北与东之间。
- 示例 IO70VJ - JO59BC：散射点 JP01AK 位于设得兰群岛外海，偏差 0.97°，QTF 11° 和 299°。
- **地图上的极光 spot**：文本中带有 **<AU>** 或 **<AUR>** 的 spot 为极光 spot（作者规则）。勾选*在地图上显示 <AU>/<AUR> spot 并计算*后，本选项卡会绘出最近 2 小时内的 spot（最多 30 条）：对每对报告者-DX，计算其 Scatter trail，并显示最可能的散射点（橙色圆），结果中给出两个台站的 QTF 和仰角。
