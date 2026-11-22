# 电离层测高仪

[← Home](Home-zh)

![IONOSONDE](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1m_ionosonde.png)

**SPORADIC-E** 的子选项卡（自 0.5.2 版起）：全球 **GIRO** 网络测高仪**实测的 foEs**，与程序由 spot 推得的值并列显示。

## 数据

- 62 台 GIRO 测高仪（ULCAR 列表），带有从电离图自动判读的最新 **foEs**、**foF2** 和 **MUF(D)**。
- 数据在第一次打开选项卡时到达，之后**每 15 分钟**自动更新（复选框 *每 15 分钟自动更新*）；*立即更新* 可立即获取。档案在数据文件夹的 `giro` 中保存最近 48 小时。
- **GIRO** 的数据（Reinisch 和 Galkin 2011），许可证 **CC BY-NC-SA 4.0**：仅限非商业用途。服务与 DM6AX 的 *MUF Muncher* 相同。

## 地图和表格

- 每台测高仪是一个按最新 foEs 着色的圆点（颜色与 Es 云相同），并标出以 MHz 为单位的数值；没有最近数据时为**灰色**，最后一张电离图超过 90 分钟时带**圆环**。
- 表格：代码、测高仪、距本台的 km、最后一张电离图的 UTC、foEs、foF2、MUF(D) 和 **Spot foEs≥**：由最近 30 分钟内在测高仪 500 km 以内（可调）反射的 Es spot 推得的最低 foEs，以及 spot 的数量。
- **h'Es GIRO**（自 0.6.5 版起，REAL-TIME 中的框）：Es 高度变为 2500 km 以内测高仪最新 h'Es 的中位数（80-120 km）。

## 所选测高仪

点击某一行或地图上测高仪附近：

- **最近 24 小时的图表**：foEs 黄色，foF2 浅蓝色，MUF(D) 紫色点线；
- 1000、1500、2000 和 2400 km 处**偶发 E 层的 MUF**（MUF = foEs · sec φ，按所选 Es 高度）以及**打开的频段**（28、40、50、70、144 MHz）；
- 如果 spot 要求的值高于实测 foEs，程序会给出提示：云可能远离测高仪，或者测高仪已到达其频率范围的极限（Chartier 2022）。
