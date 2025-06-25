# Magic Rental Shop 단계별 개발 계획서

**목적:** 복잡한 시스템을 점진적으로 구현하여 각 단계마다 플레이 가능한 프로토타입을 완성합니다.

---

## 1단계: 핵심 데이터 구조 + 기본 아키텍처 구축

### 목표
Unity 프로젝트 기본 골격과 데이터 구조 완성

### 구현 범위
**Data Layer:**
- `GameConfig.cs` (기본 설정만)
- `WeaponData.cs`, `CustomerData.cs`, `DungeonData.cs` 
- `PlayerData.cs` (기본 필드만)
- `Enums.cs` (Grade, Element, GamePhase)

**Core System:**
- `GameController.cs` (기본 싱글톤)
- `DataManager.cs` (ScriptableObject 로드)
- `UIManager.cs` (기본 팝업 관리)

**기본 UI:**
- `StatusBarView` (골드, 날짜, 페이즈만)
- `AlertPopup`, `LoadingPopup`

### 완료 기준
- Unity에서 실행 시 StatusBar에 기본 정보 표시
- DataManager로 무기/고객/던전 데이터 로드 확인
- AlertPopup 테스트 가능

---

## 2단계: 기본 게임 루프 + 인벤토리

### 목표  
3페이즈 전환과 인벤토리 시스템 구현

### 구현 범위
**Phase System:**
- `MorningController.cs`, `DayController.cs`, `NightController.cs` (기본 틀)
- `MorningView`, `DayView`, `NightView` (빈 패널)
- 페이즈 전환 로직 + 시간 관리

**Inventory System:**
- `InventoryView.cs` (무기/재료 탭)
- `InventoryController.cs`
- `WeaponInstance.cs`, `MaterialInstance.cs`

**Common UI:**
- StatusBar 실시간 업데이트
- 3:2 비율 레이아웃 완성

### 완료 기준
- 아침→낮→밤 자동 전환 작동
- 인벤토리에서 무기/재료 확인 가능
- StatusBar 시간 카운트다운 정상 작동

---

## 3단계: 고객-무기-던전 매칭 시스템

### 목표
게임의 핵심 메커니즘 완성

### 구현 범위
**Customer System:**
- `CustomerManager.cs` (고객 생성)
- `CustomerInstance.cs`
- `DayView` → `CustomerButton` → `CustomerInfoPanel`

**Adventure System:**
- `AdventureController.cs` (모험 관리)
- `AdventureInstance.cs`
- 무기 선택 → 모험 예약 → 결과 처리

**Success Rate Calculator:**
- `SuccessRateCalculator.cs` (속성 상성 + 기본 계산)
- 성공/실패 결과 + 무기 회수

**Night System:**
- `NightView` → 진행중/완료 모험 표시
- `AdventureInfoPanel`, `ResultInfoPanel`

### 완료 기준
- 고객에게 무기 장착 → 모험 보내기 가능
- 성공률 계산 정확성 확인
- 밤에 결과 확인 후 다음 날 진행

---

## 4단계: 무기 상점 + 경제 시스템

### 목표
게임의 경제 루프 완성

### 📋 구현 범위
**Weapon Shop:**
- `WeaponShopController.cs` 
- `WeaponShopPanel` → `BuyWeaponButton` → `BuyWeaponPanel`
- 일수별 무기 등급 확률 적용

**Economy:**
- 골드 증감 시스템
- 무기 가격 계산 (등급 + 일수 보정)
- 보상 시스템 (골드 + 재료)

**Morning System:**
- `MorningView` 완성
- 이벤트 시스템 기본 틀

### 완료 기준
- 무기 구매 → 인벤토리 추가
- 모험 성공 → 골드/재료 획득
- 기본적인 경제 루프 순환

---

## 5단계: 대장간 시스템

### 목표
제작 시스템으로 게임 깊이 추가

### 📋 구현 범위
**Blacksmith System:**
- `BlacksmithController.cs`
- `RecipeData.cs` + 레시피 목록
- `CraftingInstance.cs` (시간 기반 제작)

**Complex UI:**
- `BlacksmithPanel` (레시피 + 제작현황 분할)
- `RecipeInfoPanel` → 재료 정보 연결
- `CraftingWeaponButton` (진행/완료 상태)

**Material System:**
- `MaterialData` 확장 (`availableDungeonIDs`)
- 재료-던전 연결 정보

### 완료 기준
- 재료로 무기 제작 가능
- 제작 진행 상황 확인
- 시간 경과 후 무기 획득

---

## 6단계: Hero 시스템 1부 - 전환 메커니즘

### 목표
Hero 전환 시스템 구현

### 구현 범위
**Hero Core:**
- `HeroManager.cs` (기본 관리)
- `HeroInstance.cs`
- `HeroConversionCalculator.cs`

**Conversion System:**
- 모험 성공 시 Hero 전환 확률 계산
- Customer 풀에서 제거 → Hero 풀에 추가
- Hero 전용 성공률 보정 적용

**Hero UI Basic:**
- `HeroMenuPanel` (도감/모험 메뉴)
- `HeroListPanel` (보유 Hero 목록)
- Hero 모험 보내기 (Customer와 동일)

### 완료 기준
- 고객이 Hero로 전환 확인
- Hero로 모험 보내기 가능
- Hero 전용 보정 정상 적용

---

## 7단계: Hero 시스템 2부 - 부상 & 도감

### 목표
Hero 시스템 완성

### 구현 범위
**Injury System:**
- `InjuredHeroData.cs`
- Hero 실패 시 10일 부상 처리
- 부상 회복 시스템

**Collection System:**
- `HeroCollectionData.cs`
- `HeroCollectionPanel` (그림자 ↔ 실제)
- Hero 잠금해제 시스템 (등급별 일수)

**Advanced UI:**
- Customer/Hero 통합 표시 (밤 UI)
- Hero 구분 표시 (별표)
- Hero 레벨업 시스템

### 완료 기준
- Hero 실패 → 부상 → 회복 순환
- Hero 도감 수집 확인
- Customer/Hero 구분 표시

---

## 8단계: 고급 시스템 + 완성도

### 목표
모든 고급 기능과 폴리시 완성

### 구현 범위
**Advanced Systems:**
- 월세 시스템 + 게임오버
- 무기 판매 시스템
- 무기 상점 새로고침

**Polish:**
- 저장/로드 시스템 완성
- 일일 이벤트 시스템
- UI 애니메이션

**Balance:**
- 수치 밸런싱
- Hero 보상/페널티 시스템
- 게임 난이도 조절

### 완료 기준
- 완전한 게임 루프 작동
- 저장/로드 정상 동작
- 게임오버 후 Hero 승계

---

## 각 단계별 작업 프로세스

### 1️구현 순서
```
스크립트 작성 → Unity 컴포넌트 생성 → UI 연결 → 테스트 → 다음 단계
```

### 2️테스트 방법
각 단계마다 **간단한 테스트 시나리오** 제공:
- 1단계: "StatusBar에 정보 표시되는가?"
- 3단계: "고객-무기-던전 매칭이 작동하는가?"
- 6단계: "Hero 전환이 실제로 일어나는가?"

### 3️문제 해결
단계별로 **핵심 이슈 예상** 및 **해결 방향** 제시

---

## 예상 개발 기간

- **1~2단계**: 기본 구조 (2-3일)
- **3~4단계**: 핵심 게임플레이 (3-4일)  
- **5단계**: 대장간 (1-2일)
- **6~7단계**: Hero 시스템 (3-4일)
- **8단계**: 완성도 (2-3일)

**총 예상 기간: 11~17일**

---

이 계획대로 하면 각 단계마다 "실제로 플레이할 수 있는 것"이 나와서 성취감도 있고, 문제 발견도 빨라질 거예요! 어떤 단계부터 시작할까요? 🚀