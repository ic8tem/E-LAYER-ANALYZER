# インストール

[← Home](Home-ja)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** ページから適切なファイルをダウンロードしてください。プログラムと `data` フォルダーは常に 1 つの個人用フォルダーにまとめて置かれます: **C:\Users\...\AppData には一切ファイルを書き込みません**。

| システム | ファイル | インストール方法 |
|---|---|---|
| Windows 64 ビット | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | ダブルクリック。管理者権限なしで `C:\E-LAYER-ANALYZER` にインストールします。スタートメニューのショートカット (AppData に置かれるため) は作らず、任意でデスクトップアイコンのみ作成します。 |
| Windows 32 ビット | `...-windows-x86-setup.exe` | 上と同じ、32 ビット版 Windows 用。 |
| Windows ポータブル | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | 任意の場所に展開し、`E-LAYER-ANALYZER.exe` を起動します。 |
| Linux (PC) | `...-linux-x64.tar.gz` | 展開して `./installa.sh` を実行: `~/E-LAYER-ANALYZER` にインストールし、メニューにアイコンを追加します。 |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | Pi 3、4、5 で **64 ビット版 Raspberry Pi OS** (デスクトップ付き) が必要 (RAM 4 GB 推奨)。展開して `./installa.sh` を実行します。 |
| Mac | `...-mac-apple-silicon.dmg` (M1-M4) または `...-mac-intel.dmg` | .dmg を開き、**Installa E-LAYER-ANALYZER.command** を右クリック > 開く。`~/E-LAYER-ANALYZER` にインストールします。 |

## アップデート

新しいバージョンを旧バージョンの上にインストールしてください: 設定、収集スポット、ログ、`call3.txt` はそのまま残ります。

## 動作要件

- .NET のインストールは不要: 必要なものはすべてプログラムに含まれています。
- インターネット接続: 地図 (Natural Earth、初回起動時)、オーロラの NOAA データ、YA-MS のレーダー、Telnet セッションに必要です。
- Raspberry Pi Zero と Pi 1 は非対応です (.NET がサポートしないプロセッサーのため)。
