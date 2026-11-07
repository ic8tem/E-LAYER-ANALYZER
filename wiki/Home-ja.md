# E-LAYER ANALYZER

*作者: Costantino IC8TEM* — [English](Home) · [Italiano](Home-it) · **日本語** · [Русский](Home-ru)

**E-LAYER ANALYZER** は、電離層の **E 層** が関わるすべての VHF 伝搬モードを解析する、アマチュア無線家向けの無料プログラムです: **スポラディックE** (大陸間のマルチホップを含む)、**F.A.I.** (磁力線に沿った不規則構造)、**メテオスキャッター** (YA-MS プログラムを内蔵) および **電波オーロラ**。**Windows (64 ビットと 32 ビット)、Linux、Raspberry Pi、Mac** で動作し、**28 言語** に対応、自分のフォルダーの外には何も書き込みません。

![SPORADIC-E](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/1_sporadic_e.png)

## タブ

| タブ | 用途 |
|---|---|
| [SPORADIC-E](Sporadic-E-ja) | 地図上の Es スポット、両局の第 1 ホップ、マイデンヘッドグリッド上の Es 雲、自局の MUF |
| [BENCHMARK Es](Benchmark-Es-ja) | ADIF ログ全体 (他局のログでも可) を使った検証台: 日別・時間別の QSO、セクター別スキップ、推定 foEs、マルチホップの検証、雲の移動 |
| [BENCHMARK 50 MHz](Benchmark-50-ja) | 1 つまたは 2 つの ADIF ログによる 50 MHz のマルチホップ スポラディックE: メルカトル図法の地図、日と時刻、任意の foEs、WWL 付きのパス拡大表示、150 km 以内の QSO |
| [F.A.I.](FAI-ja) | 2 局間の散乱点の Scatter trail と、地点検証付きの等反射線 (isoflection loci) |
| [METEOR SCATTER](Meteor-Scatter-ja) | YA-MS プログラムのフル機能 (流星群の効率、レーダー、予報) |
| [AURORA](Aurora-ja) | NOAA のオーロラオーバル、Kp、太陽風、オーロラの Scatter trail と等反射線、推奨方向 |
| [DATA COLLECTION](Data-Collection-ja) | 3 つの Telnet セッション (DX クラスター、RBN、PSKReporter 対応の DM4X)、収集スポット、CALL3 アーカイブ |
| [LOG ADIF](Log-ADIF-ja) | 28 MHz 以上のバンドごとの交信済・確認済グリッド (自分の ADIF または LoTW から) |
| [出典と技術文書](Sources-and-Technical-Document-ja) | すべての出典とすべての式 (Word にエクスポート可能) |

## はじめに

1. [インストール](Installation-ja)
2. [初回起動](Getting-Started-ja)
3. [配色と言語](Colours-and-Languages-ja)
4. [データフォルダー](Data-Folder-ja)
5. [よくある質問](FAQ-ja)

## 主な出典

- G. Cerrotta IC8FAX, C. Cerrotta IC8TEM, *IL F.A.I., quello strano fenomeno del dopo E-sporadico*, Capri, 2011.
- G. Cerrotta IC8FAX, [Field Aligned Irregularities](https://qsl.net/ic8fax/page4.html).
- F. Egano IK3XTV, スポラディックE の距離の表 ([qsl.net/ik3xtv](https://www.qsl.net/ik3xtv/))。
- C. Cerrotta IC8TEM, G. Sartori IV3GTH, *METEOR-SCATTER* (YA-MS の出典)。

完全な一覧はプログラムの「出典」タブにあります。
