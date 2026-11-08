# BENCHMARK 50 MHz

[← Home](Home-ko)

![BENCHMARK 50 MHz](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1g_benchmark_50.png)

한 국의 ADIF 로그와, 필요하면 두 번째 국의 로그를 사용하는 **50 MHz 멀티홉 스포라딕 E** 시험대입니다.

## 로그 불러오기

- **국 1**: *ADIF 불러오기* / *ADIF 제거*. 프로그램은 로그에서 `STATION_CALLSIGN`과 `MY_GRIDSQUARE`를 가져오며, 송신 위치는 각 QSO의 `MY_GRIDSQUARE`입니다.
- **국 2**(선택): 두 번째 줄의 같은 버튼; *국 2* 확인란으로 표시하거나 숨깁니다. 국 1의 경로는 하늘색, 국 2의 경로는 보라색입니다.
- 프로그램은 로케이터가 있는 50 MHz QSO(없거나 4자리인 로케이터는 CALL3에서 가져옴) 중 멀티홉, 즉 3100 km 이상인 것을 사용합니다(작성자 규칙; 홉 수는 IK3XTV 표를 따름).
- **적도 횡단** 교신은 제외합니다(작성자 규칙): 아프리카와 남아메리카 방향으로 중간점이 자기 복각 -20°와 +20° 사이의 띠에 있는 경우. 단, 로그에 `PROP_MODE=ES`가 있으면 예외입니다.

## 날짜와 시간대

**날짜**(열 때는 멀티홉 QSO가 가장 많은 날)와 UTC 시간 **시작… 종료…**를 15분 단위로 고릅니다. **foEs** 확인란(선택)을 켜면 경로를 최소 foEs로 색칠하고 표에도 표시합니다.

## 지도

중심이 자유로운 **메르카토르** 지도: 휠 = 확대/축소, 드래그 = 이동.

![경로](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1h_benchmark_50_tratta.png)

- **경로 근처에 마우스**: 선과 아이콘이 커지고 상대국의 **6자리 WWL 그리드**가 나타납니다.
- 그 경로를 **왼쪽 클릭**: 상대편 위치가 그 지점에서 **150 km 이내**인 **파일의 모든 QSO**(모든 날짜)를 표시할지 묻는 팝업이 뜹니다. *예*: 지도를 비우고 주황 원 안의 해당 QSO를 표시; *아니오*: 지도를 그대로 둠. *날짜와 시간대로 돌아가기*로 이전 선택으로 돌아갑니다.

![주변](https://raw.githubusercontent.com/ic8tem/E-LAYER-ANALYZER/ci-screenshots/chiaro/1j_benchmark_50_dintorni.png)

## 상세 정보

선택한 경로에 대해: 거리와 IK3XTV 표에 따른 평가, 양방향 방위, 반사점 그리드가 포함된 균등 홉, 1200 km 이내의 짧은 첫 홉(IC8TEM 경험), 그리고 foEs 확인란을 켠 경우 최소 foEs.
