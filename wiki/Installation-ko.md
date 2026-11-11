# 설치

[← Home](Home-ko)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** 페이지에서 알맞은 파일을 다운로드하세요. 프로그램과 `data` 폴더는 항상 사용자의 폴더 안에 함께 있습니다: **C:\Users\...\AppData에는 어떤 파일도 기록하지 않습니다**.

| 시스템 | 파일 | 설치 방법 |
|---|---|---|
| Windows 64비트 | `E-LAYER-ANALYZER-x.y.z-win-x64-setup.exe` | 더블클릭. `C:\E-LAYER-ANALYZER`에 설치되며 관리자 권한이 필요 없습니다. 시작 메뉴 바로 가기는 없고(AppData에 만들어지기 때문) 선택적인 바탕 화면 아이콘만 있습니다. |
| Windows 32비트 | `...-win-x86-setup.exe` | 위와 같음, 32비트 Windows용. |
| Windows 포터블 | `...-win-x64-portatile.zip` / `...-win-x86-portatile.zip` | 아무 곳에나 압축 해제하고 `E-LAYER-ANALYZER.exe`를 실행. |
| Linux 64비트 (PC) | `e-layer-analyzer_x.y.z_amd64.deb` | `sudo apt install ./e-layer-analyzer_x.y.z_amd64.deb` 후 메뉴에서 실행: 첫 실행 시 프로그램이 자신을 `~/E-LAYER-ANALYZER`에 복사하며, `data` 폴더는 그 옆에 있습니다. |
| Linux 포터블 | `...-linux-x64-portatile.tar.gz` | 압축 해제 후 `./E-LAYER-ANALYZER` 실행, 또는 `./installa.sh` 실행: 메뉴 아이콘과 함께 `~/E-LAYER-ANALYZER`에 설치. |
| Raspberry Pi (64비트) | `e-layer-analyzer_x.y.z_arm64.deb` 또는 `...-linux-arm64-portatile.tar.gz` | **64비트 Raspberry Pi OS**와 데스크톱이 있는 Pi 3, 4 또는 5(RAM 4 GB 권장). .deb는 위와 같고, 포터블은 Linux와 같습니다. |
| Raspberry Pi (32비트) | `...-linux-arm-portatile.tar.gz` | **32비트 Raspberry Pi OS**가 있는 Pi 2, 3 또는 4. 압축 해제 후 `./E-LAYER-ANALYZER` 또는 `./installa.sh` 실행. |
| Mac | `...-osx-arm64.dmg`(Apple Silicon M1-M4) 또는 `...-osx-x64.dmg`(Intel) | .dmg를 열고 **Installa E-LAYER-ANALYZER.command**를 오른쪽 클릭 > Open. `~/E-LAYER-ANALYZER`에 설치. |
| Mac, 애플리케이션만 | `...-osx-arm64-app.zip` / `...-osx-x64-app.zip` | **E-LAYER-ANALYZER.app**을 사용자의 폴더에 압축 해제(예: `~/E-LAYER-ANALYZER`, Applications가 아님): `data` 폴더는 앱 옆에 만들어집니다. 첫 실행 시 오른쪽 클릭 > Open. |

## 업데이트

새 버전을 기존 버전 위에 설치하세요: 설정, 수집된 스폿, 로그, `call3.txt`는 유지됩니다.

**자동 업데이트(버전 0.4.5부터).** 프로그램은 시작할 때와 그 후 6시간마다 GitHub의 Releases를 확인합니다. 새 버전이 나오면 창을 열고 상단 표시줄의 NTP 표시 옆에 주황색 **▲ x.y.z**를 보여 줍니다:

- **지금 업데이트**는 사용 중인 컴퓨터용 포터블 패키지(Windows 64/32비트, Linux, Raspberry Pi, Mac)를 다운로드하고, 프로그램을 닫은 뒤 파일을 교체하고 다시 시작합니다. 설정, 수집된 스폿, 로그, `call3.txt`는 유지됩니다.
- **나중에**는 다음 확인 때 다시 묻고, **이 버전 건너뛰기**는 그 버전을 더 이상 제안하지 않습니다.
- 상단 표시줄에 표시된 버전(예: `v0.4.5`)을 클릭하면 바로 업데이트를 확인합니다. 자동 확인은 같은 창의 확인란으로 끌 수 있습니다.
- 프로그램 폴더에 쓸 수 없는 경우(예: 시스템 폴더)에는 대신 Release 페이지의 설치 패키지를 사용하세요.

**누락된 파일.** 프로그램은 시작할 때 배포 시 함께 제공된 모든 파일(번역, 문서, 표, `data/manifest.txt`에 나열됨)이 있는지 확인합니다. 빠진 파일이 있으면 창에 그 목록이 표시되고, **GitHub에서 다운로드**가 GitHub의 같은 버전에서 파일을 다시 가져옵니다.

## 요구 사항

- .NET을 설치할 필요가 없습니다: 프로그램에 필요한 모든 것이 들어 있습니다.
- 지도(Natural Earth, 첫 실행 시), NOAA 오로라 데이터, YA-MS 레이더, 텔넷 세션에는 인터넷 연결이 필요합니다.
- Raspberry Pi Zero와 Pi 1은 지원하지 않습니다(.NET이 해당 프로세서를 지원하지 않음).
