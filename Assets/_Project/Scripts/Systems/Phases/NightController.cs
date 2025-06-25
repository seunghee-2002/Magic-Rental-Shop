using UnityEngine;
using System.Collections.Generic;
using MagicRentalShop.Core;
using MagicRentalShop.Data;
using MagicRentalShop.Data.RuntimeInstances;

namespace MagicRentalShop.Systems.Phases
{
    /// <summary>
    /// 밤 페이즈 UI 흐름 제어
    /// 2단계: 기본 틀만 구현, 실제 기능은 3단계, 6-7단계에서 추가
    /// </summary>
    public class NightController : MonoBehaviour
    {
        [Header("UI 참조 (3단계에서 연결)")]
        public GameObject nightViewPanel;        // NightView 패널
        
        [Header("디버그")]
        public bool enableDebugLogs = true;
        
        // 싱글톤 패턴
        public static NightController Instance { get; private set; }
        
        // 캐시된 참조들
        private PlayerData playerData;
        private GameConfig gameConfig;
        
        // 임시 데이터 (3단계에서 실제 데이터로 교체)
        private List<AdventureInstance> tempOngoingAdventures = new List<AdventureInstance>();
        private List<AdventureResultData> tempCompletedAdventures = new List<AdventureResultData>();
        
        private void Awake()
        {
            // 싱글톤 설정
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // 참조 초기화
            if (GameController.Instance != null)
            {
                playerData = GameController.Instance.PlayerData;
                gameConfig = GameController.Instance.GameConfig;
            }
        }
        
        #region 페이즈 제어
        
        /// <summary>
        /// 밤 페이즈 활성화
        /// GameController에서 호출됨
        /// </summary>
        public void Activate()
        {
            if (enableDebugLogs)
                Debug.Log($"NightController: 밤 페이즈 시작 (Day {playerData?.currentDay})");
            
            // UI 패널 활성화
            if (nightViewPanel != null)
                nightViewPanel.SetActive(true);
            
            // 인벤토리를 Normal 모드로 설정 (정보 확인만)
            SetInventoryMode();
            
            // TODO: 3단계에서 추가할 기능들
            // - 진행중 모험 목록 가져오기: GetOngoingAdventures()
            // - 완료된 모험 목록 가져오기: GetCompletedAdventures()
            // - Customer/Hero 통합 정렬: SortAdventuresByType()
            
            ProcessNightSetup();
        }
        
        /// <summary>
        /// 밤 페이즈 비활성화
        /// GameController에서 호출됨
        /// </summary>
        public void Deactivate()
        {
            if (enableDebugLogs)
                Debug.Log("NightController: 밤 페이즈 종료");
            
            // UI 패널 비활성화
            if (nightViewPanel != null)
                nightViewPanel.SetActive(false);
        }
        
        #endregion
        
        #region 밤 페이즈 설정
        
        /// <summary>
        /// 밤 페이즈 초기 설정
        /// </summary>
        private void ProcessNightSetup()
        {
            // 인벤토리 Normal 모드 설정
            SetInventoryMode();
            
            // TODO: 추후 단계에서 추가할 설정들
            // - 모험 결과 처리
            // - Hero 전환 확률 계산
            // - Hero 레벨업 처리
            
            // 임시: 모험 목록 로드
            LoadAdventureData();
            
            if (enableDebugLogs)
                Debug.Log("NightController: 밤 페이즈 설정 완료");
        }
        
        /// <summary>
        /// 인벤토리 모드 설정
        /// </summary>
        private void SetInventoryMode()
        {
            // InventoryView를 Normal 모드로 설정 (정보 확인만)
            var inventoryView = FindObjectOfType<UI.Common.InventoryView>();
            if (inventoryView != null)
            {
                inventoryView.SetMode(InventoryMode.Normal);
                if (enableDebugLogs)
                    Debug.Log("NightController: 인벤토리를 Normal 모드로 설정");
            }
        }
        
        /// <summary>
        /// 모험 데이터 로드 (임시 구현)
        /// TODO: 3단계에서 AdventureController에서 가져오도록 변경
        /// </summary>
        private void LoadAdventureData()
        {
            // 임시 데이터 초기화
            tempOngoingAdventures.Clear();
            tempCompletedAdventures.Clear();
            
            // TODO: AdventureController에서 실제 데이터 가져오기
            // tempOngoingAdventures = AdventureController.Instance.GetOngoingAdventures();
            // tempCompletedAdventures = AdventureController.Instance.GetCompletedAdventures();
            
            if (enableDebugLogs)
                Debug.Log($"NightController: 모험 데이터 로드됨 - 진행중: {tempOngoingAdventures.Count}, 완료: {tempCompletedAdventures.Count}");
        }
        
        #endregion
        
        #region 모험 결과 처리 (TODO: 3단계)
        
        /// <summary>
        /// 진행중 모험 상세 정보 표시
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="adventure">모험 인스턴스</param>
        public void ShowAdventureDetail(AdventureInstance adventure)
        {
            if (enableDebugLogs)
                Debug.Log($"NightController: 진행중 모험 상세 정보 표시 (3단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPopup<AdventureInfoPanel>(adventure)
        }
        
        /// <summary>
        /// 완료된 모험 결과 표시
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="result">모험 결과</param>
        public void ShowResultDetail(AdventureResultData result)
        {
            if (enableDebugLogs)
                Debug.Log($"NightController: 완료된 모험 결과 표시 (3단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPopup<ResultInfoPanel>(result)
        }
        
        /// <summary>
        /// Customer/Hero 모험 통합 정렬
        /// TODO: 6-7단계에서 구현
        /// </summary>
        /// <param name="adventures">모험 목록</param>
        /// <returns>정렬된 모험 목록 (Customer 우선, Hero 나중)</returns>
        public List<AdventureInstance> SortAdventuresByType(List<AdventureInstance> adventures)
        {
            if (enableDebugLogs)
                Debug.Log("NightController: Customer/Hero 모험 통합 정렬 (6-7단계에서 구현 예정)");
            
            // TODO: Customer 모험들 먼저, Hero 모험들 나중으로 정렬
            // TODO: 각 그룹 내에서는 남은 일수 순 정렬
            
            return adventures;
        }
        
        /// <summary>
        /// Customer/Hero 결과 통합 정렬
        /// TODO: 6-7단계에서 구현
        /// </summary>
        /// <param name="results">결과 목록</param>
        /// <returns>정렬된 결과 목록 (Customer 우선, Hero 나중)</returns>
        public List<AdventureResultData> SortResultsByType(List<AdventureResultData> results)
        {
            if (enableDebugLogs)
                Debug.Log("NightController: Customer/Hero 결과 통합 정렬 (6-7단계에서 구현 예정)");
            
            // TODO: Customer 결과들 먼저, Hero 결과들 나중으로 정렬
            // TODO: 각 그룹 내에서는 완료 순서대로 정렬
            
            return results;
        }
        
        #endregion
        
        #region Hero 시스템 (TODO: 6-7단계)
        
        /// <summary>
        /// Hero 전환 처리
        /// TODO: 6-7단계에서 구현
        /// </summary>
        /// <param name="result">모험 결과</param>
        public void ProcessHeroConversion(AdventureResultData result)
        {
            if (enableDebugLogs)
                Debug.Log($"NightController: Hero 전환 처리 (6-7단계에서 구현 예정)");
            
            // TODO: 성공한 Customer만 대상
            // TODO: HeroConversionCalculator.CalculateConversionRate()
            // TODO: 전환 성공 시 CustomerManager.RemoveFromPool() + HeroManager.AddHero()
        }
        
        /// <summary>
        /// Hero 레벨업 처리
        /// TODO: 6-7단계에서 구현
        /// </summary>
        /// <param name="result">모험 결과</param>
        public void ProcessHeroLevelUp(AdventureResultData result)
        {
            if (enableDebugLogs)
                Debug.Log($"NightController: Hero 레벨업 처리 (6-7단계에서 구현 예정)");
            
            // TODO: Hero 성공 시에만 처리
            // TODO: HeroManager.LevelUpHero()
        }
        
        #endregion
        
        #region 다음 날 진행
        
        /// <summary>
        /// 다음 날로 넘어가기
        /// TODO: 3단계에서 구현
        /// </summary>
        public void GoToNextDay()
        {
            if (enableDebugLogs)
                Debug.Log("NightController: 다음 날로 진행");
            
            // TODO: 자동저장
            // TODO: PersistenceController.SaveGame()
            
            // TODO: 모험 일수 감소 처리
            // TODO: AdventureController.ProcessDayChange()
            
            // GameController에 다음 날 시작 요청
            if (GameController.Instance != null)
            {
                GameController.Instance.AdvanceToNextDay();
            }
        }
        
        #endregion
        
        #region UI 이벤트 처리
        
        /// <summary>
        /// 진행중 모험 아이템 클릭 처리
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="adventureIndex">모험 인덱스</param>
        public void OnAdventureItemClicked(int adventureIndex)
        {
            if (enableDebugLogs)
                Debug.Log($"NightController: 진행중 모험 클릭됨 - Index {adventureIndex} (3단계에서 구현 예정)");
            
            // TODO: tempOngoingAdventures[adventureIndex]로 ShowAdventureDetail() 호출
        }
        
        /// <summary>
        /// 완료된 모험 결과 버튼 클릭 처리
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="resultIndex">결과 인덱스</param>
        public void OnResultButtonClicked(int resultIndex)
        {
            if (enableDebugLogs)
                Debug.Log($"NightController: 완료된 모험 결과 클릭됨 - Index {resultIndex} (3단계에서 구현 예정)");
            
            // TODO: tempCompletedAdventures[resultIndex]로 ShowResultDetail() 호출
        }
        
        /// <summary>
        /// 다음 날 버튼 클릭 처리
        /// </summary>
        public void OnNextDayButtonClicked()
        {
            GoToNextDay();
        }
        
        #endregion
        
        #region 디버그 및 테스트
        
        /// <summary>
        /// 디버그용 밤 페이즈 강제 활성화
        /// </summary>
        [ContextMenu("Test Activate Night")]
        public void TestActivate()
        {
            Activate();
        }
        
        /// <summary>
        /// 현재 밤 페이즈 상태 출력
        /// </summary>
        [ContextMenu("Debug Night State")]
        public void DebugNightState()
        {
            Debug.Log("=== 밤 페이즈 상태 ===");
            Debug.Log($"현재 날짜: {playerData?.currentDay}일차");
            Debug.Log($"보유 골드: {playerData?.gold}골드");
            Debug.Log($"진행중 모험: {tempOngoingAdventures.Count}개");
            Debug.Log($"완료된 모험: {tempCompletedAdventures.Count}개");
            Debug.Log($"NightView 활성화: {nightViewPanel?.activeInHierarchy}");
        }
        
        /// <summary>
        /// 테스트용 임시 모험 데이터 생성
        /// </summary>
        [ContextMenu("Test Create Adventure Data")]
        public void TestCreateAdventureData()
        {
            // TODO: 3단계에서 실제 데이터로 교체
            Debug.Log("NightController: 테스트용 모험 데이터 생성 (3단계에서 구현 예정)");
        }
        
        #endregion
    }
}