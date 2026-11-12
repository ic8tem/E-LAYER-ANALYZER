# METEOR SCATTER

[← Home](Home-ko)

이 탭에는 IC8TEM의 유성 산란 프로그램 **YA-MS 1.8.1** 전체가 들어 있습니다: 복사점이 표시된 천구, SKiYMET 레이더 플럭스(Collm, Sodankylä), 유성우 효율 계산, 예보 실험 적용, 기하와 감쇠, GRAVES, 그리드 지도, **실시간 유성우**(Global Meteor Network가 측정한 ZHR과 CMOR 레이더의 복사점), **소스 지도**(국 기준 Collm, Sodankylä, GRAVES, CMOR의 위치), 공식이 담긴 **효율 계산 설명** 문서.

YA-MS 1.5.0에는 **데이터 수집** 탭이 추가되었습니다(MSK144, FSK441, FSK41, PSK2K, JTMS, JT6M, `<MS>`로 걸러 낸 유성 산란 스폿). E-LAYER ANALYZER 안에서는 자체 텔넷 세션을 열지 않고 DATA COLLECTION 세션의 줄을 받으므로, 같은 호출부호로 이중 로그인하는 일이 없습니다.

YA-MS 1.6.0은 탭을 **라이브 및 실시간 데이터**(각 교신의 구름을 보여 주는 실시간 유성 산란 트래픽, 레이더 플럭스, GRAVES, 데이터 수집)와 **예보 계산**(예보, 기하, 효율 계산, 예보 실험 적용)으로 재구성합니다.

YA-MS 1.7.0과 1.8.0은 복사점과 IAU MDC 목록의 비교, 유성우별 Global Meteor Network 일일 집계, 개체수 지수에서 구한 k 지수, Stober 외(2021)의 흔적 초기 반경, 밴드 간 ping 지속 시간(기본값 λ² 또는 NN4SA의 HamSCI MSQP 측정), Sonneberg의 BRAMS 검출과의 비교(2026년 페르세우스자리 유성우)를 추가합니다. E-LAYER ANALYZER 안에서는 YA-MS의 자동 업데이트가 꺼져 있으며, E-LAYER ANALYZER는 GitHub의 Release에서 스스로 업데이트합니다.

YA-MS 1.8.1은 안테나 로브를 수직 패턴으로 다룹니다(「안테나 패턴」 상자, 기본 켬). 반사점에 두 안테나의 수직 이득으로 가중치를 주므로 긴 경로에서도 효율이 더 이상 사라지지 않습니다. 「공통 구역 없음」 경고, 강조된 β 각 상자(가장 영향이 큰 선택), 하나로 통일된 계산 날짜, 「사용 설명서」, 그리고 2025년 8월 HamSCI Meteor Scatter QSO Party의 50 MHz 실제 스폿 181 767건과의 비교가 추가되었습니다. 시간대와 경로의 양쪽은 믿을 만하고, 백분율은 참고용입니다.

- YA-MS는 E-LAYER ANALYZER 위쪽에서 선택한 언어와 색 구성을 사용합니다.
- YA-MS 설정은 `data/config.ini`에 저장됩니다.
- YA-MS의 새 버전은 `tools/sync_yams.py` 스크립트로 가져옵니다.

자세한 내용은 [YA-MS 저장소](https://github.com/ic8tem/YA-MS)를 참조하세요.
