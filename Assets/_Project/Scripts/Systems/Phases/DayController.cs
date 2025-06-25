using UnityEngine;
using MagicRentalShop.Data;
using MagicRentalShop.Data.RuntimeInstances;
using MagicRentalShop.Core;
using MagicRentalShop.UI.Common;

namespace MagicRentalShop.Systems.Phases
{
    /// <summary>
    /// 낮 페이즈 UI 흐름 제어
    /// 2단계: 기본 틀만 구현, 실제 기능은 3단계, 6-7단계에서 추가
    /// </summary>
    public class DayController : MonoBehaviour
    {
        [Header("UI 참조 (3단계에서 연결)")]
        public GameObject dayViewPanel;          // DayView 패널
        
        [Header("선택 상태")]
        public CustomerInstance selectedCustomer; // 현재 선택된 고객 (3단계에서 사용)
        public WeaponInstance selectedWeapon;     // 현재 선택된 무기 (3단계에서 사용)
        
        [Header("디버그")]
        public bool enableDebugLogs = true;
        
        // 싱글톤 패턴
        public static DayController Instance { get; private set; }
        
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
        /// 낮 페이즈 활성화
        /// GameController에서 호출됨
        /// </summary>
        public void Activate()
        {
            if (enableDebugLogs)
                Debug.Log($"DayController: 낮 페이즈 시작 (Day {playerData?.currentDay})");
            
            // UI 패널 활성화
            if (dayViewPanel != null)
                dayViewPanel.SetActive(true);
            
            // 인벤토리를 Selection 모드로 설정 (무기 선택용)
            SetInventoryMode();
            
            // TODO: 3단계에서 추가할 기능들
            // - 고객 목록 요청: RequestCustomers()
            // - 고객 목록 UI 갱신: RefreshCustomerList()
            
            // TODO: 6-7단계에서 추가할 Hero 기능들
            // - Hero 목록 갱신: RefreshHeroList()
            // - Hero 잠금해제 상태 확인: CheckHeroUnlockStatus()
            
            ProcessDaySetup();
        }
        
        /// <summary>
        /// 낮 페이즈 비활성화
        /// GameController에서 호출됨
        /// </summary>
        public void Deactivate()
        {
            if (enableDebugLogs)
                Debug.Log("DayController: 낮 페이즈 종료");
            
            // 선택 상태 초기화
            ClearSelections();
            
            // UI 패널 비활성화
            if (dayViewPanel != null)
                dayViewPanel.SetActive(false);
        }
        
        #endregion
        
        #region 낮 페이즈 설정
        
        /// <summary>
        /// 낮 페이즈 초기 설정
        /// </summary>
        private void ProcessDaySetup()
        {
            // 인벤토리 Selection 모드 설정
            SetInventoryMode();
            
            // 선택 상태 초기화
            ClearSelections();
            
            // TODO: 추후 단계에서 추가할 설정들
            // - 오늘 방문할 고객들 생성
            // - Hero 상태 확인 (부상 회복 등)
            
            if (enableDebugLogs)
                Debug.Log("DayController: 낮 페이즈 설정 완료");
        }
        
        /// <summary>
        /// 인벤토리 모드 설정
        /// </summary>
        private void SetInventoryMode()
        {
            // InventoryView를 Selection 모드로 설정 (무기 선택용)
            var inventoryView = FindObjectOfType<UI.Common.InventoryView>();
            if (inventoryView != null)
            {
                inventoryView.SetMode(InventoryMode.Selection);
                if (enableDebugLogs)
                    Debug.Log("DayController: 인벤토리를 Selection 모드로 설정");
            }
        }
        
        /// <summary>
        /// 선택 상태 초기화
        /// </summary>
        private void ClearSelections()
        {
            selectedCustomer = null;
            selectedWeapon = null;
            
            // 인벤토리 선택 강조 해제
            var inventoryView = FindObjectOfType<UI.Common.InventoryView>();
            if (inventoryView != null)
            {
                inventoryView.HighlightForSelection(false);
            }
        }
        
        #endregion
        
        #region 고객 시스템 (TODO: 3단계)
        
        /// <summary>
        /// 고객 선택 처리
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="customer">선택된 고객</param>
        public void SelectCustomer(CustomerInstance customer)
        {
            selectedCustomer = customer;
            selectedWeapon = null; // 무기 선택 초기화
            
            if (enableDebugLogs)
                Debug.Log($"DayController: 고객 선택됨 - {customer?.Name} (3단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPanel<CustomerInfoPanel>(customer)
        }
        
        /// <summary>
        /// 무기 장착 처리
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="weapon">장착할 무기</param>
        public void EquipWeapon(WeaponInstance weapon)
        {
            if (selectedCustomer == null)
            {
                Debug.LogWarning("DayController: 고객이 선택되지 않았습니다.");
                return;
            }
            
            selectedWeapon = weapon;
            
            if (enableDebugLogs)
                Debug.Log($"DayController: 무기 장착됨 - {weapon?.Name} → {selectedCustomer?.Name}");
            
            // TODO: UI 업데이트 및 모험 예약 처리
        }
        
        /// <summary>
        /// 모험 시작 처리
        /// TODO: 3단계에서 구현
        /// </summary>
        public void StartAdventure()
        {
            if (selectedCustomer == null || selectedWeapon == null)
            {
                Debug.LogWarning("DayController: 고객 또는 무기가 선택되지 않았습니다.");
                return;
            }
            
            if (enableDebugLogs)
                Debug.Log($"DayController: 모험 시작 - {selectedCustomer.Name} + {selectedWeapon.Name}");
            
            // TODO: AdventureController.StartAdventure(selectedCustomer, selectedWeapon)
            
            // 선택 상태 초기화
            ClearSelections();
        }
        
        #endregion
        
        #region Hero 시스템 (TODO: 6-7단계)
        
        /// <summary>
        /// Hero 메뉴 버튼 클릭 처리
        /// TODO: 6-7단계에서 구현
        /// </summary>
        public void OnClickHeroMenu()
        {
            if (enableDebugLogs)
                Debug.Log("DayController: Hero 메뉴 클릭 (6-7단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPopup<HeroMenuPanel>()
        }
        
        /// <summary>
        /// Hero 도감 열기
        /// TODO: 6-7단계에서 구현
        /// </summary>
        public void OpenHeroCollection()
        {
            if (enableDebugLogs)
                Debug.Log("DayController: Hero 도감 열기 (6-7단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPanel<HeroCollectionPanel>()
        }
        
        /// <summary>
        /// Hero 모험 보내기 열기
        /// TODO: 6-7단계에서 구현
        /// </summary>
        public void OpenHeroAdventure()
        {
            if (enableDebugLogs)
                Debug.Log("DayController: Hero 모험 보내기 열기 (6-7단계에서 구현 예정)");
            
            // TODO: UIManager.OpenPanel<HeroListPanel>()
        }
        
        /// <summary>
        /// Hero 선택 처리
        /// TODO: 6-7단계에서 구현
        /// </summary>
        /// <param name="hero">선택된 Hero</param>
        public void SelectHero(HeroInstance hero)
        {
            if (enableDebugLogs)
                Debug.Log($"DayController: Hero 선택됨 - {hero?.Name} (6-7단계에서 구현 예정)");
            
            // TODO: Hero 모험 처리 로직 (Customer와 유사)
        }
        
        #endregion
        
        #region UI 이벤트 처리
        
        /// <summary>
        /// 고객 버튼 클릭 처리
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="customerIndex">고객 인덱스</param>
        public void OnCustomerButtonClicked(int customerIndex)
        {
            if (enableDebugLogs)
                Debug.Log($"DayController: 고객 버튼 클릭됨 - Index {customerIndex} (3단계에서 구현 예정)");
            
            // TODO: CustomerManager에서 고객 정보 가져와서 SelectCustomer() 호출
        }
        
        /// <summary>
        /// 무기 선택 완료 처리 (InventoryView에서 호출)
        /// TODO: 3단계에서 구현
        /// </summary>
        /// <param name="weapon">선택된 무기</param>
        public void OnWeaponSelected(WeaponInstance weapon)
        {
            EquipWeapon(weapon);
        }
        
        #endregion
        
        #region 디버그 및 테스트
        
        /// <summary>
        /// 디버그용 낮 페이즈 강제 활성화
        /// </summary>
        [ContextMenu("Test Activate Day")]
        public void TestActivate()
        {
            Activate();
        }
        
        /// <summary>
        /// 현재 낮 페이즈 상태 출력
        /// </summary>
        [ContextMenu("Debug Day State")]
        public void DebugDayState()
        {
            Debug.Log("=== 낮 페이즈 상태 ===");
            Debug.Log($"현재 날짜: {playerData?.currentDay}일차");
            Debug.Log($"보유 골드: {playerData?.gold}골드");
            Debug.Log($"선택된 고객: {selectedCustomer?.Name ?? "없음"}");
            Debug.Log($"선택된 무기: {selectedWeapon?.Name ?? "없음"}");
            Debug.Log($"DayView 활성화: {dayViewPanel?.activeInHierarchy}");
        }
        
        #endregion
    }
}