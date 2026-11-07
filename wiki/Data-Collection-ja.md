# DATA COLLECTION

[← Home](Home-ja)

## Telnet セッション

| セッション | 初期設定 | 備考 |
|---|---|---|
| 1. 任意の DX クラスター | on0dxk.dyndns.org 8000 | ON0DXK では VHF がフィルターされています: `set/filter dxbm/pass 6` (続けて 4、2)。別の例: hamqth.com 7300。 |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW と RTTY、7001 = FT8。 |
| 3. PSKReporter 対応クラスター | **dm4x.ddns.net 8500** | DM4X のクラスター。代わりに HamAlert (hamalert.org 7300、アカウントが必要)。 |

- コールサイン (上部または **ログイン** 欄) で自動ログイン。**初期コマンド** は `;` で区切ります。
- 回線が切れたら 30 秒後に **再接続**。**起動時に接続** も可能です。
- 既定では **28 MHz 以上** のスポットのみ保存します。

## 収集スポット

直近 2000 件のスポットを、バンドとソースで絞り込んで表示します。アーカイブ: `data/spot` に 1 日 1 つの CSV ファイル。 プログラムを閉じるたびに、スポットのデータベース（メモリと `data/spot` のファイル）は空になり、毎回まっさらな状態から始まります。

## CALL3

JTDX の **コールサイン → ロケーター** アーカイブ (100 万以上のコールサイン) が `data/call3.txt` にあります。ボタン **再読込** と **新しい CALL3.TXT をインポート**。スポットのコメントから読み取った 6 桁ロケーターは `data/call3_nuovi.txt` に追加されます。
