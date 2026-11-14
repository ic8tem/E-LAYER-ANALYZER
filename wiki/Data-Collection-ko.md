# DATA COLLECTION

[← Home](Home-ko)

## 텔넷 세션

| 세션 | 기본값 | 비고 |
|---|---|---|
| 1. 사용자 지정 DX 클러스터 | on0dxk.dyndns.org 8000 | ON0DXK에서는 VHF 스폿이 필터링되어 있습니다: `set/filter dxbm/pass 6`(이어서 4, 2). 다른 예: hamqth.com 7300. |
| 2. Reverse Beacon Network | telnet.reversebeacon.net 7001 | 7000 = CW와 RTTY, 7001 = FT8. |
| 3. PSKReporter 클러스터 | **dm4x.ddns.net 8500** | DM4X의 클러스터; 대안으로 HamAlert(hamalert.org 7300, 계정 필요). |

- 호출부호로 자동 로그인(맨 위 또는 로그인 필드); `;`로 구분한 **초기 명령**.
- 회선이 끊기면 30 s 후 **재연결**; **시작 시 연결**.
- 기본적으로 **28 MHz 이상**의 스폿만 저장합니다.

## 수집된 스폿

최근 스폿 2000개를 밴드와 소스로 필터링하여 표시합니다. 아카이브: `data/spot`에 하루 하나의 CSV 파일. 프로그램을 닫을 때마다 스폿 데이터베이스(메모리와 `data/spot`의 파일)를 비웁니다: 세션마다 처음부터 시작합니다.

**로컬 필터**(선택 상자): 27990.0 kHz 미만 또는 145 MHz 초과의 "DX de" 줄은 버리며 콘솔에 표시하지도 저장하지도 않습니다. **부정확한 로케이터**(6자리 미만)는 CALL3에서 가져옵니다: CALL3에 6자리 로케이터가 있으면 항상 그것이 우선합니다. YA-MS와 마찬가지로 유성 산란(스포라딕 E가 아닌) 스폿은 다른 전파 모드 태그(`<TR> <AS> <ACS> <AU> <ES> <FAI> <F2>`) 없이 MS 대표 주파수 창(50190-50300, 70100-70400, 144290-144399 kHz) 안에 있는 스폿, 그리고 MS 전용 모드(MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M)나 `<MS>`가 있는 스폿입니다. `<AS>`/`<ACS>`(항공기 산란) 스폿은 스포라딕 E에 사용하지 않습니다.

## CALL3

`data/call3.txt`의 JTDX **호출부호 → 로케이터** 데이터베이스(호출부호 100만 개 이상). **다시 불러오기**와 **새 CALL3.TXT 가져오기** 버튼. 스폿 코멘트에서 읽은 6자리 로케이터는 `data/call3_nuovi.txt`에 추가됩니다.

버전 0.4.16부터 프로그램 전체에 CALL3 데이터베이스는 하나뿐이며, YA-MS 데이터 수집(METEOR SCATTER 탭)의 것으로 LIVETROPOVHF와 같은 규칙을 따릅니다: MMMonVHF에서 업데이트, 손 입력, 추가 파일, 클러스터 수정, 학습한 로케이터, 6시간마다 G7VRD. 수정은 data/call3.txt에 기록되고 data/call3_nuovi.txt의 로케이터는 한 번 가져옵니다. WSJT-X, JTDX, MSHV의 스팟은 UDP로도 들어옵니다(출처 WSJT-X UDP, 포트 2237, 2238로 전달).

YA-MS에서 온 이 필터(CW, 최소 거리, 주파수 창, MS 모드, 밴드)는 METEOR SCATTER 탭에만 적용되며, 어떤 스팟이 그 수집에 들어갈지 정합니다. 다른 탭은 모든 스팟을 받아 각자의 필터를 사용합니다.
