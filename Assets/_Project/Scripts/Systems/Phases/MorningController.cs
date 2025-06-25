using UnityEngine;
using MagicRentalShop.Data;
using MagicRentalShop.Core;
using MagicRentalShop.UI.Common;

namespace MagicRentalShop.Systems.Phases
{
    /// <summary>
    /// 아침 페이즈 UI 흐름 제어
    /// 2단계: 기본 틀만 구현, 실제 기능은 4-5단계에서 추가
    /// </summary>
    public class MorningController : MonoBehaviour
    {
        [Header("UI 참조 (3단계에서 연결)")]
        public GameObject morningViewPanel;      // MorningView 패널
        
        [Header("디버그")]
        public bool enableDebugLogs = true;
        
        // 싱글톤 패턴
        public static MorningController Instance { get; private set; }
        
        // 캐시된 참조들
        private PlayerData playerData;
        private GameConfig gameConfig;
        
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
        /// 아침 페이즈 활성화
        /// GameController에서 호출됨
        /// </summary>
        public void Activate()
        {
            if (enableDebugLogs)
                Debug.Log($"MorningController: 아침 페이즈 시작 (Day {playerData?.currentDay})");
            
            // UI 패널 활성화
            if (morningViewPanel != null)
                morningViewPanel.SetActive(true);
            
            // 인벤토리를 Management 모드로 설정 (무기 판매 등)
            SetInventoryMode();
            
            // TODO: 4단계에서 추가할 기능들
            // - 월세 경고 확인: CheckAndShowRentWarning()
            // - 일일 이벤트 처리: ProcessDailyEvent()
            // - 무기 상점 목록 갱신: RefreshWeaponShop()
            
            ProcessMorningSetup();
        }
        
        /// <summary>
        /// 아침 페이즈 비활성화
        /// GameController에서 호출됨
        /// </summary>
        public void Deactivate()
        {
            if (enableDebugLogs)
                Debug.Log("MorningController: 아침 페이즈 종료");
            
            // UI 패널 비활성화
            if (morningViewPanel != null)
                morningViewPanel.SetActive(false);
        }
        
        #endregion
        
        #region 아침 페이즈 설정
        
        /// <summary>
        /// 아침 페이즈 초기 설정
        /// </summary>
        private void ProcessMorningSetup()
        {
            // 인벤토리 Management 모드 설정
            SetInventoryMode();
            
            // TODO: 추후 단계에서 추가할 설정들
            // - 일일 이벤트 확인 및 적용
            // - 무기 상점 새로고침 상태 초기화
            // - 대장간 제작 완료 확인
            
            if (enableDebugLogs)
                Debug.Log("MorningController: 아침 페이즈 설정 완료");
        }
        
        /// <summary>
        /// 인벤토리 모드 설정
        /// </summary>
        private void SetInventoryMode()
        {
            // InventoryView를 Management 모드로 설정
            var inventoryView = FindObjectOfType<UI.Common.InventoryView>();
            if (inventoryView != null)
            {
                inventoryView.SetMode(InventoryMode.Management);
                if (enableDebugLogs)
                    Debug.Log("MorningController: 인벤토리를 Management 모드로 설정");
            }
        }
        
        #endregion
        
        #region UI 이벤트 처리 (TODO: 4-5단계)
        
        /// <summary>
        /// 무기 상점 버튼 클릭 처리
        /// TODO: 4단계에서 구현
        /// </summary>
        public void OnClickWeaponShop()
        {
            if (enableDebugLogs)
                Debug.Log("MorningController: 무기 상점 클릭 (4단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPanel<WeaponShopPanel>()
        }
        
        /// <summary>
        /// 대장간 버튼 클릭 처리
        /// TODO: 5단계에서 구현
        /// </summary>
        public void OnClickBlacksmith()
        {
            if (enableDebugLogs)
                Debug.Log("MorningController: 대장간 클릭 (5단계에서 구현 예정)");
            
            // 대장간 해금 확인
            if (playerData != null && playerData.currentDay < gameConfig.blacksmithUnlockDay)
            {
                Debug.Log($"MorningController: 대장간 아직 해금되지 않음 ({gameConfig.blacksmithUnlockDay}일차에 해금)");
                return;
            }
            
            // TODO: UIManager.OpenPanel<BlacksmithPanel>()
        }
        
        /// <summary>
        /// 이벤트 확인 버튼 클릭 처리
        /// TODO: 4단계에서 구현
        /// </summary>
        public void OnClickEvent()
        {
            if (enableDebugLogs)
                Debug.Log("MorningController: 이벤트 확인 클릭 (4단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPopup<EventInfoPanel>()
        }
        
        #endregion
        
        #region 추후 단계 구현 예정 (스텁)
        
        /// <summary>
        /// 월세 경고 확인 및 표시
        /// TODO: 4단계에서 구현
        /// </summary>
        private void CheckAndShowRentWarning()
        {
            // TODO: RentCalculator.IsRentWarningDay() 확인
            // TODO: UIManager.ShowRentWarning() 호출
        }
        
        /// <summary>
        /// 일일 이벤트 처리
        /// TODO: 4단계에서 구현
        /// </summary>
        private void ProcessDailyEvent()
        {
            // TODO: DailyEventManager.TriggerDailyEvent() 호출
        }
        
        #endregion
        
        #region 디버그 및 테스트
        
        /// <summary>
        /// 디버그용 아침 페이즈 강제 활성화
        /// </summary>
        [ContextMenu("Test Activate Morning")]
        public void TestActivate()
        {
            Activate();
        }
        
        /// <summary>
        /// 현재 아침 페이즈 상태 출력
        /// </summary>
        [ContextMenu("Debug Morning State")]
        public void DebugMorningState()
        {
            Debug.Log("=== 아침 페이즈 상태 ===");
            Debug.Log($"현재 날짜: {playerData?.currentDay}일차");
            Debug.Log($"보유 골드: {playerData?.gold}골드");
            Debug.Log($"대장간 해금 여부: {(playerData?.currentDay >= gameConfig?.blacksmithUnlockDay ? "해금됨" : "잠금됨")}");
            Debug.Log($"MorningView 활성화: {morningViewPanel?.activeInHierarchy}");
        }
        
        #endregion
    }
}