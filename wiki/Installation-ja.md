# インストール

[← Home](Home-ja)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** ページから適切なファイルをダウンロードしてください。プログラムと `data` フォルダーは常に 1 つの個人用フォルダーにまとめて置かれます: **C:\Users\...\AppData には一切ファイルを書き込みません**。

| システム | ファイル | インストール方法 |
|---|---|---|
| Windows 64 ビット | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | ダブルクリック。管理者権限なしで `C:\E-LAYER-ANALYZER` にインストールします。スタートメニューのショートカット (AppData に置かれるため) は作らず、任意でデスクトップアイコンのみ作成します。 |
| Windows 32 ビット | `...-win-x86-setup.exe` | 上と同じ、32 ビット版 Windows 用。 |
| Windows on ARM (Snapdragon PC) | `...-win-arm64-setup.exe` | 上と同じ、ARM プロセッサーの Windows 用。 |
| Windows ポータブル | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` / `...-win-arm64-portatile.zip` | 任意の場所に展開し、`E-LAYER-ANALYZER.exe` を起動します。 |
| Debian、Ubuntu、Mint | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb` の後、メニューから起動します。初回起動時にプログラムは自身を `~/E-LAYER-ANALYZER` にコピーし、その隣に `data` フォルダーを置きます。 |
| Fedora、openSUSE | `e-layer-analyzer-x.y.z.x86_64.rpm` (`.aarch64.rpm`、`.armv7hl.rpm` もあり) | `sudo dnf install ./e-layer-analyzer-x.y.z.x86_64.rpm` (または `zypper install`)、その後は .deb と同じ。 |
| Arch、Manjaro、EndeavourOS、Arch Linux ARM | `e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst` (または `-aarch64`) | `sudo pacman -U ./e-layer-analyzer-x.y.z-1-x86_64.pkg.tar.zst`、その後は .deb と同じ。 |
| Slackware (x86_64 と ARM64) | `e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` (または `-aarch64-`) | root で `installpkg e-layer-analyzer-x.y.z-x86_64-1_ic8tem.txz` を実行し、メニューまたは端末から `e-layer-analyzer` を起動します: 初回起動時にプログラムは自身を `~/E-LAYER-ANALYZER` にコピーします。 |
| すべての Linux (AppImage) | `E-LAYER-ANALYZER-x.y.z-x86_64.AppImage` (または `-aarch64.AppImage`) | 実行可能にして起動します: 初回起動時にプログラムを `~/E-LAYER-ANALYZER` にコピーし、そこから起動します。 |
| Linux ポータブル | `...-linux-x64-portatile.tar.gz` | 展開して `./E-LAYER-ANALYZER` を起動するか、`./installa.sh` を実行: `~/E-LAYER-ANALYZER` にインストールし、メニューにアイコンを追加します。 |
| Alpine Linux (musl) | `...-alpine-x64-portatile.tar.gz` / `...-alpine-arm64-portatile.tar.gz` | `apk add icu-libs fontconfig` の後は Linux のポータブル版と同じ。 |
| Raspberry Pi (64 ビット) | `e-layer-analyzer_x.y.z_arm64.deb` または `...-linux-arm64-portatile.tar.gz` | Pi 3、4、5 で **64 ビット版 Raspberry Pi OS** (デスクトップ付き) が必要 (RAM 4 GB 推奨)。.deb は上と同じ、ポータブル版は Linux と同じです。 |
| Raspberry Pi (32 ビット) | `e-layer-analyzer_x.y.z_armhf.deb` または `...-linux-arm-portatile.tar.gz` | Pi 2、3、4 で **32 ビット版 Raspberry Pi OS** が必要。.deb は上と同じ、ポータブル版は展開して `./E-LAYER-ANALYZER` または `./installa.sh` を実行します。 |
| Raspberry Pi (Slackware ARM64) | `e-layer-analyzer-x.y.z-aarch64-1_ic8tem.txz` または `...-linux-arm64-portatile.tar.gz` | 上の Slackware と同じ。ポータブル版は Linux と同じです。 |
| Linux armv7l (その他の 32 ビット ARM ボード) | `...-linux-armv7l-portatile.tar.gz` | Linux のポータブル版と同じ。 |
| FreeBSD amd64 | `...-freebsd-amd64-sperimentale.tar.gz` | **実験的、未検証。** コミュニティ版の .NET 8 (`pkg install dotnet`) と、Avalonia が提供していない FreeBSD 用にビルドされた SkiaSharp と HarfBuzzSharp のネイティブライブラリが必要です: これらがないとプログラムは起動しません。パッケージ内の `LEGGIMI-FREEBSD.txt` を参照してください。 |
| Mac | `...-osx-arm64.dmg` (Apple Silicon M1-M4) または `...-osx-x64.dmg` (Intel) | .dmg を開き、**Installa E-LAYER-ANALYZER.command** を右クリック > 開く。`~/E-LAYER-ANALYZER` にインストールします。 |
| Mac、アプリケーションのみ | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app** を個人用フォルダー (例: `~/E-LAYER-ANALYZER`、Applications ではなく) に展開します: `data` フォルダーはアプリの隣に作成されます。初回起動時は右クリック > 開く。 |

## アップデート

新しいバージョンを旧バージョンの上にインストールしてください: 設定、収集スポット、ログ、`call3.txt` はそのまま残ります。

**システムパッケージ (.deb、.rpm、Arch、Slackware) は自動では更新されません。** これらではプログラムは新しいバージョンを知らせるだけです: Release ページから新しいパッケージをダウンロードし、最初と同じようにインストールしてください (`sudo apt install ./...deb`、`sudo dnf install ./...rpm`、`sudo pacman -U ./...pkg.tar.zst`、`upgradepkg ...txz`)。自動更新は Windows のインストーラーと zip、Mac のアプリ、ポータブルの tar.gz パッケージ、AppImage で機能します。

**自動更新 (バージョン 0.4.5 から)。** プログラムは起動時とその後 30 分ごとに GitHub の Releases を確認します。新しいバージョンが出ていればウィンドウを開き、上部のバーの NTP 表示の横にオレンジ色の **▲ x.y.z** を表示します:

**バージョン 0.4.12 から更新は自動かつ無言で行われます**: GitHub に新しいバージョンがあると、プログラムがダウンロードしてインストールし、何も尋ねずに自動で再起動します。不足しているファイルも確認なしで再ダウンロードされます。更新ウィンドウは上部のバージョンをクリックしたとき、または「設定」(一般) で自動更新をオフにしたときだけ表示されます。

- **今すぐ更新** は、お使いのコンピューター用のポータブルパッケージ (Windows 64/32 ビット、Linux、Raspberry Pi、Mac) をダウンロードし、プログラムを閉じてファイルを置き換え、再起動します。設定、収集スポット、ログ、`call3.txt` はそのまま残ります。
- **後で** は次回の確認時に再び尋ねます。**このバージョンをスキップ** はそのバージョンを提案しなくなります。
- 上部のバーに表示されたバージョン (例: `v0.4.5`) をクリックすると、すぐに更新を確認します。自動確認は同じウィンドウのチェックボックスでオフにできます。
- プログラムのフォルダーに書き込めない場合 (例: システムフォルダー) は、代わりに Release ページのインストールパッケージを使ってください。

**不足しているファイル。** プログラムは起動時に、配布時に含まれていたファイル (翻訳、文書、表。`data/manifest.txt` に記載) がすべてそろっているかを確認します。不足があればウィンドウにその一覧が表示され、**GitHub からダウンロード** で GitHub の同じバージョンから取り直せます。

## 動作要件

- .NET のインストールは不要: 必要なものはすべてプログラムに含まれています。
- インターネット接続: 地図 (Natural Earth、初回起動時)、オーロラの NOAA データ、YA-MS のレーダー、Telnet セッションに必要です。
- Raspberry Pi Zero と Pi 1 は非対応です (.NET がサポートしないプロセッサーのため)。
- Linux i686 (32 ビット PC) と OpenBSD 用のバージョンはありません。これらのシステム用の .NET が存在しないためです。FreeBSD には実験的なパッケージだけがあります。
