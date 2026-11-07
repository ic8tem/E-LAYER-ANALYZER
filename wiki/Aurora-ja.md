# AURORA

[← Home](Home-ja)

![AURORA](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/aurora/aurora_scia_IO70VJ_JO59BC.png)

- NOAA OVATION Prime の **オーロラオーバル** (グラデーション表示)、過去 3 日間の **Kp**、**太陽風** (Bz、速度、密度)。5 分ごとに更新し、`data/swpc` に保存します。
- F.A.I. と同じ表示: 2 局間の **Scatter trail** と、地点を検証する **等反射線**。既定の高度は 105 km です。
- **両局の推奨方向**: 各局の直交線 (その局の色の点線) 上で、オーバル確率が最も高い地点。目安 (G3WPO): 北と東の間、約 45° から始めます。
- 例 IO70VJ - JO59BC: 散乱点はシェトランド諸島沖の JP01AK、ずれ 0.97°、QTF 11° と 299°。
- **地図上のオーロラ・スポット**：テキストに **<AU>** または **<AUR>** を含むスポットはオーロラ（作者のルール）。「<AU>/<AUR> スポットを計算付きで地図に表示」にチェックを入れると、直近 2 時間（最大 30 件）のスポットを描画し、スポッター–DX の各ペアについて Scatter trail を計算して、最も可能性の高い scatter point（オレンジの円）と両局の QTF・仰角を結果に表示します。
