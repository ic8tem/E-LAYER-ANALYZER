# METEOR SCATTER

[← Home](Home-ja)

このタブには、IC8TEM のメテオスキャッター用プログラム **YA-MS 1.8.14** がそのまま入っています: 放射点を表示する天球、SKiYMET レーダー (Collm、Sodankylä) の流量、流星群の効率計算、予報の実験的な実装、ジオメトリと減衰、GRAVES、グリッド地図、**流星群ライブ** (Global Meteor Network が測定した ZHR と CMOR レーダーの放射点)、**データ提供元の地図** (局から見た Collm、Sodankylä、GRAVES、CMOR)、そして式を収めた文書 **効率の計算の説明**。

YA-MS 1.5.0 では **データ収集** タブ（MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M、`<MS>` で絞り込んだ流星散乱スポット）が加わりました。E-LAYER ANALYZER の中では独自の telnet セッションを開かず、DATA COLLECTION のセッションの行を受け取るため、同じコールサインでの二重ログインは起きません。

YA-MS 1.6.0 ではタブが **ライブとリアルタイムデータ**（各交信の雲を表示するライブ流星散乱トラフィック、レーダーフラックス、GRAVES、データ収集）と **予測計算**（予測計算、幾何、効率計算、予測の実施）に再編成されました。

YA-MS 1.7.0 と 1.8.0 では、放射点と IAU MDC カタログとの比較、流星群ごとの Global Meteor Network の日別カウント、個数指数から求めた k 指数、Stober ほか (2021) による流星痕の初期半径、バンド間の ping 継続時間 (既定は λ²、または NN4SA による HamSCI MSQP の測定)、Sonneberg の BRAMS 検出との比較 (2026 年ペルセウス座流星群) が加わりました。E-LAYER ANALYZER の中では YA-MS の自動更新はオフになっており、E-LAYER ANALYZER は GitHub の Releases から自身を更新します。

YA-MS 1.8.1 はアンテナのローブを垂直パターンとして扱います（「アンテナパターン」、既定でオン）。反射点を 2 本のアンテナの垂直利得で重み付けするため、長い経路でも効率が消えなくなりました。「共通ゾーンなし」の警告、β 角のチェックの強調表示（最も影響の大きい選択）、計算日を 1 つに統一、「ユーザーマニュアル」、そして 2025 年 8 月の HamSCI Meteor Scatter QSO Party による 50 MHz の実際のスポット 181 767 件との比較を追加しました。時間帯と経路の両側は信頼でき、パーセントは目安です。

- YA-MS の言語と配色は、E-LAYER ANALYZER の上部で選んだものが使われます。
- YA-MS の設定は `data/config.ini` に保存されます。
- YA-MS の新しいバージョンは、スクリプト `tools/sync_yams.py` で取り込みます。

詳しくは [YA-MS のリポジトリ](https://github.com/ic8tem/YA-MS) を参照してください。
