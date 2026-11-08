# 설치

[← Home](Home-ko)

**[Releases](https://github.com/ic8tem/E-LAYER-ANALYZER/releases)** 페이지에서 알맞은 파일을 다운로드하세요. 프로그램과 `data` 폴더는 항상 사용자의 폴더 안에 함께 있습니다: **C:\Users\...\AppData에는 어떤 파일도 기록하지 않습니다**.

| 시스템 | 파일 | 설치 방법 |
|---|---|---|
| Windows 64비트 | `E-LAYER-ANALYZER-x.y.z-windows-x64-setup.exe` | 더블클릭. `C:\E-LAYER-ANALYZER`에 설치되며 관리자 권한이 필요 없습니다. 시작 메뉴 바로 가기는 없고(AppData에 만들어지기 때문) 선택적인 바탕 화면 아이콘만 있습니다. |
| Windows 32비트 | `...-windows-x86-setup.exe` | 위와 같음, 32비트 Windows용. |
| Windows 포터블 | `...-windows-x64-portatile.zip` / `...-x86-portatile.zip` | 아무 곳에나 압축 해제하고 `E-LAYER-ANALYZER.exe`를 실행. |
| Linux (PC) | `...-linux-x64.tar.gz` | 압축 해제 후 `./installa.sh` 실행: 메뉴 아이콘과 함께 `~/E-LAYER-ANALYZER`에 설치. |
| Raspberry Pi | `...-raspberry-arm64.tar.gz` | **64비트 Raspberry Pi OS**와 데스크톱이 있는 Pi 3, 4 또는 5(RAM 4 GB 권장). 압축 해제 후 `./installa.sh` 실행. |
| Mac | `...-mac-apple-silicon.dmg`(M1-M4) 또는 `...-mac-intel.dmg` | .dmg를 열고 **Installa E-LAYER-ANALYZER.command**를 오른쪽 클릭 > Open. `~/E-LAYER-ANALYZER`에 설치. |

## 업데이트

새 버전을 기존 버전 위에 설치하세요: 설정, 수집된 스폿, 로그, `call3.txt`는 유지됩니다.

## 요구 사항

- .NET을 설치할 필요가 없습니다: 프로그램에 필요한 모든 것이 들어 있습니다.
- 지도(Natural Earth, 첫 실행 시), NOAA 오로라 데이터, YA-MS 레이더, 텔넷 세션에는 인터넷 연결이 필요합니다.
- Raspberry Pi Zero와 Pi 1은 지원하지 않습니다(.NET이 해당 프로세서를 지원하지 않음).
