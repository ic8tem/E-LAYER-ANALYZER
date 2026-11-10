# インストール

[← Home](Home-ja)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** ページから適切なファイルをダウンロードしてください。プログラムと `data` フォルダーは常に 1 つの個人用フォルダーにまとめて置かれます: **C:\Users\...\AppData には一切ファイルを書き込みません**。

| システム | ファイル | インストール方法 |
|---|---|---|
| Windows 64 ビット | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | ダブルクリック。管理者権限なしで `C:\E-LAYER-ANALYZER` にインストールします。スタートメニューのショートカット (AppData に置かれるため) は作らず、任意でデスクトップアイコンのみ作成します。 |
| Windows 32 ビット | `...-win-x86-setup.exe` | 上と同じ、32 ビット版 Windows 用。 |
| Windows ポータブル | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | 任意の場所に展開し、`E-LAYER-ANALYZER.exe` を起動します。 |
| Linux 64 ビット (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb` の後、メニューから起動します。初回起動時にプログラムは自身を `~/E-LAYER-ANALYZER` にコピーし、その隣に `data` フォルダーを置きます。 |
| Linux ポータブル | `...-linux-x64-portatile.tar.gz` | 展開して `./E-LAYER-ANALYZER` を起動するか、`./installa.sh` を実行: `~/E-LAYER-ANALYZER` にインストールし、メニューにアイコンを追加します。 |
| Raspberry Pi (64 ビット) | `e-layer-analyzer_x.y.z_arm64.deb` または `...-linux-arm64-portatile.tar.gz` | Pi 3、4、5 で **64 ビット版 Raspberry Pi OS** (デスクトップ付き) が必要 (RAM 4 GB 推奨)。.deb は上と同じ、ポータブル版は Linux と同じです。 |
| Raspberry Pi (32 ビット) | `...-linux-arm-portatile.tar.gz` | Pi 2、3、4 で **32 ビット版 Raspberry Pi OS** が必要。展開して `./E-LAYER-ANALYZER` または `./installa.sh` を実行します。 |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) または `...-osx-x64.dmg` (Intel) | .dmg を開き、**Installa E-LAYER-ANALYZER.command** を右クリック > 開く。`~/E-LAYER-ANALYZER` にインストールします。 |
| Mac、アプリケーションのみ | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** を個人用フォルダー (例: `~/E-LAYER-ANALYZER`、Applications ではなく) に展開します: `data` フォルダーはアプリの隣に作成されます。初回起動時は右クリック > 開く。 |

## アップデート

新しいバージョンを旧バージョンの上にインストールしてください: 設定、収集スポット、ログ、`call3.txt` はそのまま残ります。

## 動作要件

- .NET のインストールは不要: 必要なものはすべてプログラムに含まれています。
- インターネット接続: 地図 (Natural Earth、初回起動時)、オーロラの NOAA データ、YA-MS のレーダー、Telnet セッションに必要です。
- Raspberry Pi Zero と Pi 1 は非対応です (.NET がサポートしないプロセッサーのため)。
