# METEOR SCATTER

[← Home](Home-ja)

このタブには、IC8TEM のメテオスキャッター用プログラム **YA-MS 1.5.1** がそのまま入っています: 放射点を表示する天球、SKiYMET レーダー (Collm、Sodankylä) の流量、流星群の効率計算、予報の実験的な実装、ジオメトリと減衰、GRAVES、グリッド地図、**流星群ライブ** (Global Meteor Network が測定した ZHR と CMOR レーダーの放射点)、**データ提供元の地図** (局から見た Collm、Sodankylä、GRAVES、CMOR)、そして式を収めた文書 **効率 - 解説**。

YA-MS 1.5.0 では **データ収集** タブ（MSK144、FSK441、FSK41、PSK2K、JTMS、JT6M、`<MS>` で絞り込んだ流星散乱スポット）が加わりました。E-LAYER ANALYZER の中では独自の telnet セッションを開かず、DATA COLLECTION のセッションの行を受け取るため、同じコールサインでの二重ログインは起きません。

- YA-MS の言語と配色は、E-LAYER ANALYZER の上部で選んだものが使われます。
- YA-MS の設定は `data/config.ini` に保存されます。
- YA-MS の新しいバージョンは、スクリプト `tools/sync_yams.py` で取り込みます。

詳しくは [YA-MS のリポジトリ](https://github.com/ic8tem/YA-MS) を参照してください。
