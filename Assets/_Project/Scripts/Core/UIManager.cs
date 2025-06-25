using UnityEngine;
using MagicRentalShop.Data;
using MagicRentalShop.UI.Common;

namespace MagicRentalShop.Core
{
    /// <summary>
    /// 모든 UI 패널과 팝업의 생성, 표시, 소멸을 전담하는 중앙 관리자
    /// 2단계: 기본 패널 관리 및 StatusBar 연동
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("공통 UI")]
        public StatusBarView statusBar;         // 상태 표시줄 (중간 구분선)
        public InventoryView inventory;         // 인벤토리 패널 (하단)
        
        [Header("레이아웃 설정")]
        public Transform phaseContentArea;      // 페이즈별 패널이 들어갈 영역 (상단 3/5)
        
        [Header("페이즈 패널들 (3단계에서 연결)")]
        public GameObject morningViewPanel;     // MorningView 프리팹
        public GameObject dayViewPanel;         // DayView 프리팹  
        public GameObject nightViewPanel;      // NightView 프리팹
        
        [Header("공통 팝업들 (3단계에서 구현)")]
        public GameObject alertPopupPrefab;     // AlertPopup 프리팹
        public GameObject loadingPopupPrefab;   // LoadingPopup 프리팹
        
        [Header("디버그")]
        public bool enableDebugLogs = true;
        
        // 싱글톤 패턴
        public static UIManager Instance { get; private set; }
        
        // 현재 활성화된 패널
        private GameObject currentPhasePanel;
        
        // 캐시된 참조들
        private PlayerData playerData;
        
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
            InitializeUI();
            SubscribeToEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        #region 초기화
        
        /// <summary>
        /// UI 초기화
        /// </summary>
        private void InitializeUI()
        {
            // PlayerData 참조 가져오기
            if (GameController.Instance != null)
            {
                playerData = GameController.Instance.PlayerData;
            }
            
            // StatusBar 초기화
            if (statusBar != null)
            {
                UpdateStatusBar();
                if (enableDebugLogs)
                    Debug.Log("UIManager: StatusBar 초기화 완료");
            }
            
            // 인벤토리 초기화 (기본 모드는 Normal)
            if (inventory != null)
            {
                inventory.SetMode(InventoryMode.Normal);
                if (enableDebugLogs)
                    Debug.Log("UIManager: 인벤토리 초기화 완료");
            }
            
            // 초기 페이즈 패널 숨김
            HideAllPhasePanels();
        }
        
        /// <summary>
        /// 이벤트 구독
        /// </summary>
        private void SubscribeToEvents()
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.OnPhaseChanged += OnPhaseChanged;
                GameController.Instance.OnDayAdvanced += OnDayAdvanced;
                GameController.Instance.OnPhaseTimeUpdated += OnPhaseTimeUpdated;
            }
        }
        
        /// <summary>
        /// 이벤트 구독 해제
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.OnPhaseChanged -= OnPhaseChanged;
                GameController.Instance.OnDayAdvanced -= OnDayAdvanced;
                GameController.Instance.OnPhaseTimeUpdated -= OnPhaseTimeUpdated;
            }
        }
        
        #endregion
        
        #region 페이즈 패널 관리
        
        /// <summary>
        /// 페이즈 패널 설정
        /// </summary>
        /// <param name="phase">표시할 페이즈</param>
        public void SetPhasePanel(GamePhase phase)
        {
            if (enableDebugLogs)
                Debug.Log($"UIManager: 페이즈 패널 설정 - {phase}");
            
            // 이전 패널 숨김
            HideAllPhasePanels();
            
            // 새 패널 표시
            GameObject targetPanel = GetPhasePanelByType(phase);
            if (targetPanel != null)
            {
                targetPanel.SetActive(true);
                currentPhasePanel = targetPanel;
                
                // 패널을 phaseContentArea에 배치 (3단계에서 구현)
                // TODO: 실제 프리팹을 phaseContentArea에 인스턴스화
            }
            else
            {
                Debug.LogWarning($"UIManager: {phase} 패널을 찾을 수 없습니다. 3단계에서 프리팹을 연결해주세요.");
            }
        }
        
        /// <summary>
        /// 페이즈별 패널 가져오기
        /// </summary>
        /// <param name="phase">페이즈</param>
        /// <returns>해당 패널 GameObject</returns>
        private GameObject GetPhasePanelByType(GamePhase phase)
        {
            switch (phase)
            {
                case GamePhase.Morning:
                    return morningViewPanel;
                case GamePhase.Day:
                    return dayViewPanel;
                case GamePhase.Night:
                    return nightViewPanel;
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// 모든 페이즈 패널 숨김
        /// </summary>
        private void HideAllPhasePanels()
        {
            if (morningViewPanel != null) morningViewPanel.SetActive(false);
            if (dayViewPanel != null) dayViewPanel.SetActive(false);
            if (nightViewPanel != null) nightViewPanel.SetActive(false);
            
            currentPhasePanel = null;
        }
        
        #endregion
        
        #region StatusBar 관리
        
        /// <summary>
        /// StatusBar 전체 업데이트
        /// </summary>
        public void UpdateStatusBar()
        {
            if (statusBar == null || playerData == null) return;
            
            statusBar.UpdateGold(playerData.gold);
            statusBar.UpdateDay(playerData.currentDay);
            statusBar.UpdatePhase(playerData.currentPhase);
            statusBar.UpdateTime(playerData.phaseRemainingTime);
        }
        
        /// <summary>
        /// StatusBar 골드만 업데이트
        /// </summary>
        /// <param name="gold">새로운 골드 양</param>
        public void UpdateStatusBarGold(int gold)
        {
            if (statusBar != null)
                statusBar.UpdateGold(gold);
        }
        
        /// <summary>
        /// StatusBar 시간만 업데이트
        /// </summary>
        /// <param name="remainingTime">남은 시간</param>
        public void UpdateStatusBarTime(float remainingTime)
        {
            if (statusBar != null)
                statusBar.UpdateTime(remainingTime);
        }
        
        #endregion
        
        #region 인벤토리 관리
        
        /// <summary>
        /// 인벤토리 모드 설정
        /// </summary>
        /// <param name="mode">인벤토리 모드</param>
        public void SetInventoryMode(InventoryMode mode)
        {
            if (inventory != null)
            {
                inventory.SetMode(mode);
                if (enableDebugLogs)
                    Debug.Log($"UIManager: 인벤토리 모드 변경 - {mode}");
            }
        }
        
        /// <summary>
        /// 인벤토리 표시 새로고침
        /// </summary>
        public void RefreshInventory()
        {
            if (inventory != null)
            {
                inventory.RefreshInventoryDisplay();
                if (enableDebugLogs)
                    Debug.Log("UIManager: 인벤토리 새로고침");
            }
        }
        
        #endregion
        
        #region 팝업 관리 (TODO: 3단계)
        
        /// <summary>
        /// 알림 팝업 표시
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="title">제목</param>
        /// <param name="message">메시지</param>
        /// <param name="onOk">확인 버튼 콜백</param>
        public void ShowAlert(string title, string message, System.Action onOk = null)
        {
            if (enableDebugLogs)
                Debug.Log($"UIManager: 알림 팝업 표시 (3단계에서 구현 예정) - {title}: {message}");
            
            // TODO: AlertPopup 인스턴스화 및 설정
        }
        
        /// <summary>
        /// 로딩 팝업 표시
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="message">로딩 메시지</param>
        public void ShowLoading(string message)
        {
            if (enableDebugLogs)
                Debug.Log($"UIManager: 로딩 팝업 표시 (3단계에서 구현 예정) - {message}");
            
            // TODO: LoadingPopup 인스턴스화 및 설정
        }
        
        /// <summary>
        /// 로딩 팝업 숨김
        /// TODO: 3단계에서 구현
        /// </summary>
        public void HideLoading()
        {
            if (enableDebugLogs)
                Debug.Log("UIManager: 로딩 팝업 숨김 (3단계에서 구현 예정)");
            
            // TODO: LoadingPopup 비활성화
        }
        
        #endregion
        
        #region 이벤트 리스너
        
        /// <summary>
        /// 페이즈 변경 이벤트 처리
        /// </summary>
        /// <param name="newPhase">새로운 페이즈</param>
        private void OnPhaseChanged(GamePhase newPhase)
        {
            SetPhasePanel(newPhase);
            UpdateStatusBar();
            
            if (enableDebugLogs)
                Debug.Log($"UIManager: 페이즈 변경 UI 업데이트 완료 - {newPhase}");
        }
        
        /// <summary>
        /// 날짜 진행 이벤트 처리
        /// </summary>
        /// <param name="newDay">새로운 날짜</param>
        private void OnDayAdvanced(int newDay)
        {
            UpdateStatusBar();
            
            if (enableDebugLogs)
                Debug.Log($"UIManager: 날짜 진행 UI 업데이트 완료 - Day {newDay}");
        }
        
        /// <summary>
        /// 페이즈 시간 업데이트 이벤트 처리
        /// </summary>
        /// <param name="remainingTime">남은 시간</param>
        private void OnPhaseTimeUpdated(float remainingTime)
        {
            UpdateStatusBarTime(remainingTime);
        }
        
        #endregion
        
        #region 디버그 및 테스트
        
        /// <summary>
        /// 디버그용 페이즈 패널 강제 전환
        /// </summary>
        /// <param name="phase">전환할 페이즈</param>
        [ContextMenu("Debug: Test Phase Panel")]
        public void DebugTestPhasePanel()
        {
            // Morning -> Day -> Night 순으로 테스트
            GamePhase currentPhase = playerData?.currentPhase ?? GamePhase.Morning;
            GamePhase nextPhase = currentPhase switch
            {
                GamePhase.Morning => GamePhase.Day,
                GamePhase.Day => GamePhase.Night,
                GamePhase.Night => GamePhase.Morning,
                _ => GamePhase.Morning
            };
            
            SetPhasePanel(nextPhase);
            Debug.Log($"UIManager: 테스트 페이즈 패널 전환 - {currentPhase} → {nextPhase}");
        }
        
        /// <summary>
        /// 인벤토리 모드 테스트
        /// </summary>
        [ContextMenu("Debug: Test Inventory Mode")]
        public void DebugTestInventoryMode()
        {
            if (inventory == null) return;
            
            // Normal -> Selection -> Management 순으로 테스트
            InventoryMode currentMode = inventory.currentMode;
            InventoryMode nextMode = currentMode switch
            {
                InventoryMode.Normal => InventoryMode.Selection,
                InventoryMode.Selection => InventoryMode.Management,
                InventoryMode.Management => InventoryMode.Normal,
                _ => InventoryMode.Normal
            };
            
            SetInventoryMode(nextMode);
            Debug.Log($"UIManager: 인벤토리 모드 테스트 - {currentMode} → {nextMode}");
        }
        
        /// <summary>
        /// StatusBar 업데이트 테스트
        /// </summary>
        [ContextMenu("Debug: Test StatusBar Update")]
        public void DebugTestStatusBarUpdate()
        {
            UpdateStatusBar();
            Debug.Log("UIManager: StatusBar 업데이트 테스트 완료");
        }
        
        /// <summary>
        /// UI 상태 출력
        /// </summary>
        [ContextMenu("Debug: Print UI State")]
        public void DebugPrintUIState()
        {
            Debug.Log("=== UI 상태 ===");
            Debug.Log($"현재 페이즈 패널: {currentPhasePanel?.name ?? "없음"}");
            Debug.Log($"StatusBar 활성화: {statusBar != null && statusBar.gameObject.activeInHierarchy}");
            Debug.Log($"인벤토리 활성화: {inventory != null && inventory.gameObject.activeInHierarchy}");
            Debug.Log($"PlayerData 연결: {playerData != null}");
        }
        
        #endregion
    }
}