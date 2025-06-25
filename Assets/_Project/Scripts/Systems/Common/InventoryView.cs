using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using MagicRentalShop.Core;
using MagicRentalShop.Data.RuntimeInstances;
using MagicRentalShop.Systems.Common;

namespace MagicRentalShop.UI.Common
{
    /// <summary>
    /// 인벤토리 UI 관리
    /// 무기/재료 탭 전환 및 모드별 상호작용 처리
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        [Header("탭 시스템")]
        public Button weaponTabButton;           // 무기 탭 버튼
        public Button materialTabButton;         // 재료 탭 버튼
        public Transform weaponContentRoot;      // 무기 리스트의 부모 Transform
        public Transform materialContentRoot;    // 재료 리스트의 부모 Transform
        
        [Header("페이지네이션 (TODO: 3단계)")]
        public GameObject weaponPageNavigation;  // 무기용 페이지네이션 UI
        public GameObject materialPageNavigation; // 재료용 페이지네이션 UI
        
        [Header("모드 표시")]
        public GameObject managementModeIndicator; // Management 모드 표시 UI
        public TMP_Text modeStatusText;          // 현재 모드 상태 텍스트
        
        [Header("디버그")]
        public TMP_Text debugInfoText;           // 디버그 정보 표시
        
        [Header("이벤트")]
        public UnityEvent<WeaponInstance> OnWeaponClicked;     // 무기 클릭 시
        public UnityEvent<MaterialInstance> OnMaterialClicked; // 재료 클릭 시
        public UnityEvent<WeaponInstance> OnWeaponSellClicked; // 무기 판매 클릭 시 (4단계)
        
        // 현재 상태
        public InventoryMode currentMode = InventoryMode.Normal;  // public으로 변경
        private bool isWeaponTabActive = true;  // 현재 무기 탭이 활성화되어 있는지
        
        // 임시 아이템 리스트 (실제 UI 아이템들이 생성될 예정)
        private List<GameObject> currentWeaponItems = new List<GameObject>();
        private List<GameObject> currentMaterialItems = new List<GameObject>();
        
        private void Start()
        {
            InitializeUI();
            RefreshInventoryDisplay();
            
            // InventoryController 이벤트 구독
            if (InventoryController.Instance != null)
            {
                InventoryController.Instance.OnWeaponAdded.AddListener(OnWeaponAddedToInventory);
                InventoryController.Instance.OnWeaponRemoved.AddListener(OnWeaponRemovedFromInventory);
                InventoryController.Instance.OnMaterialAdded.AddListener(OnMaterialAddedToInventory);
                InventoryController.Instance.OnMaterialChanged.AddListener(OnMaterialChangedInInventory);
            }
        }
        
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (InventoryController.Instance != null)
            {
                InventoryController.Instance.OnWeaponAdded.RemoveListener(OnWeaponAddedToInventory);
                InventoryController.Instance.OnWeaponRemoved.RemoveListener(OnWeaponRemovedFromInventory);
                InventoryController.Instance.OnMaterialAdded.RemoveListener(OnMaterialAddedToInventory);
                InventoryController.Instance.OnMaterialChanged.RemoveListener(OnMaterialChangedInInventory);
            }
        }
        
        #region 초기화
        
        /// <summary>
        /// UI 초기화
        /// </summary>
        private void InitializeUI()
        {
            // 탭 버튼 이벤트 설정
            if (weaponTabButton != null)
                weaponTabButton.onClick.AddListener(() => SwitchTab(true));
                
            if (materialTabButton != null)
                materialTabButton.onClick.AddListener(() => SwitchTab(false));
            
            // 초기 탭 설정
            SwitchTab(true); // 무기 탭으로 시작
            
            // 초기 모드 설정
            SetMode(InventoryMode.Normal);
        }
        
        #endregion
        
        #region 탭 전환
        
        /// <summary>
        /// 탭 전환 (무기/재료)
        /// </summary>
        /// <param name="showWeapons">true면 무기 탭, false면 재료 탭</param>
        public void SwitchTab(bool showWeapons)
        {
            isWeaponTabActive = showWeapons;
            
            // 탭 버튼 상태 업데이트
            if (weaponTabButton != null)
                weaponTabButton.interactable = !showWeapons;
                
            if (materialTabButton != null)
                materialTabButton.interactable = showWeapons;
            
            // 컨텐츠 표시/숨김
            if (weaponContentRoot != null)
                weaponContentRoot.gameObject.SetActive(showWeapons);
                
            if (materialContentRoot != null)
                materialContentRoot.gameObject.SetActive(!showWeapons);
            
            // 페이지네이션 표시/숨김 (TODO: 3단계에서 구현)
            if (weaponPageNavigation != null)
                weaponPageNavigation.SetActive(showWeapons);
                
            if (materialPageNavigation != null)
                materialPageNavigation.SetActive(!showWeapons);
            
            RefreshCurrentTabDisplay();
            
            Debug.Log($"InventoryView: 탭 전환됨 - {(showWeapons ? "무기" : "재료")}");
        }
        
        #endregion
        
        #region 모드 관리
        
        /// <summary>
        /// 인벤토리 모드 설정
        /// </summary>
        /// <param name="mode">새로운 모드</param>
        public void SetMode(InventoryMode mode)
        {
            currentMode = mode;
            
            // Management 모드 표시 UI 업데이트
            if (managementModeIndicator != null)
                managementModeIndicator.SetActive(mode == InventoryMode.Management);
            
            // 모드 상태 텍스트 업데이트
            if (modeStatusText != null)
            {
                switch (mode)
                {
                    case InventoryMode.Normal:
                        modeStatusText.text = "정보 확인";
                        break;
                    case InventoryMode.Selection:
                        modeStatusText.text = "무기 선택";
                        break;
                    case InventoryMode.Management:
                        modeStatusText.text = "관리 모드";
                        break;
                }
            }
            
            RefreshInventoryDisplay();
            
            Debug.Log($"InventoryView: 모드 변경됨 - {mode}");
        }
        
        /// <summary>
        /// 선택 모드 강조 표시
        /// </summary>
        /// <param name="enable">강조 활성화 여부</param>
        public void HighlightForSelection(bool enable)
        {
            // TODO: 3단계에서 무기 선택을 위한 시각적 효과 구현
            if (enable)
            {
                SetMode(InventoryMode.Selection);
            }
            else if (currentMode == InventoryMode.Selection)
            {
                SetMode(InventoryMode.Normal);
            }
        }
        
        #endregion
        
        #region 인벤토리 표시
        
        /// <summary>
        /// 인벤토리 전체 표시 새로고침
        /// </summary>
        public void RefreshInventoryDisplay()
        {
            RefreshWeaponDisplay();
            RefreshMaterialDisplay();
            UpdateDebugInfo();
        }
        
        /// <summary>
        /// 현재 활성 탭 표시 새로고침
        /// </summary>
        private void RefreshCurrentTabDisplay()
        {
            if (isWeaponTabActive)
                RefreshWeaponDisplay();
            else
                RefreshMaterialDisplay();
        }
        
        /// <summary>
        /// 무기 목록 표시 새로고침
        /// </summary>
        private void RefreshWeaponDisplay()
        {
            // 기존 아이템들 정리
            ClearWeaponItems();
            
            if (InventoryController.Instance == null) return;
            
            var weapons = InventoryController.Instance.GetAllWeapons();
            
            foreach (var weapon in weapons)
            {
                CreateWeaponItem(weapon);
            }
            
            Debug.Log($"InventoryView: 무기 표시 새로고침 완료 ({weapons.Count}개)");
        }
        
        /// <summary>
        /// 재료 목록 표시 새로고침
        /// </summary>
        private void RefreshMaterialDisplay()
        {
            // 기존 아이템들 정리
            ClearMaterialItems();
            
            if (InventoryController.Instance == null) return;
            
            var materials = InventoryController.Instance.GetAllMaterials();
            
            foreach (var material in materials)
            {
                CreateMaterialItem(material);
            }
            
            Debug.Log($"InventoryView: 재료 표시 새로고침 완료 ({materials.Count}종류)");
        }
        
        #endregion
        
        #region 아이템 생성 (임시 구현)
        
        /// <summary>
        /// 무기 아이템 UI 생성 (임시 구현)
        /// TODO: 3단계에서 실제 프리팹으로 교체
        /// </summary>
        /// <param name="weapon">무기 인스턴스</param>
        private void CreateWeaponItem(WeaponInstance weapon)
        {
            GameObject itemGO = new GameObject($"Weapon_{weapon.Name}");
            itemGO.transform.SetParent(weaponContentRoot);
            
            // 임시 텍스트 컴포넌트 추가
            TMP_Text text = itemGO.AddComponent<TextMeshProUGUI>();
            text.text = $"{weapon.Name} x{weapon.quantity} ({weapon.Grade})";
            text.fontSize = 14;
            
            // 임시 버튼 컴포넌트 추가
            Button button = itemGO.AddComponent<Button>();
            button.onClick.AddListener(() => OnWeaponItemClicked(weapon));
            
            // Management 모드일 때 판매 버튼 추가 (TODO: 4단계)
            if (currentMode == InventoryMode.Management)
            {
                // 판매 관련 UI 추가 예정
            }
            
            currentWeaponItems.Add(itemGO);
        }
        
        /// <summary>
        /// 재료 아이템 UI 생성 (임시 구현)
        /// TODO: 3단계에서 실제 프리팹으로 교체
        /// </summary>
        /// <param name="material">재료 인스턴스</param>
        private void CreateMaterialItem(MaterialInstance material)
        {
            GameObject itemGO = new GameObject($"Material_{material.Name}");
            itemGO.transform.SetParent(materialContentRoot);
            
            // 임시 텍스트 컴포넌트 추가
            TMP_Text text = itemGO.AddComponent<TextMeshProUGUI>();
            text.text = $"{material.Name} x{material.quantity} ({material.Grade})";
            text.fontSize = 14;
            
            // 임시 버튼 컴포넌트 추가
            Button button = itemGO.AddComponent<Button>();
            button.onClick.AddListener(() => OnMaterialItemClicked(material));
            
            currentMaterialItems.Add(itemGO);
        }
        
        /// <summary>
        /// 무기 아이템들 정리
        /// </summary>
        private void ClearWeaponItems()
        {
            foreach (var item in currentWeaponItems)
            {
                if (item != null)
                    DestroyImmediate(item);
            }
            currentWeaponItems.Clear();
        }
        
        /// <summary>
        /// 재료 아이템들 정리
        /// </summary>
        private void ClearMaterialItems()
        {
            foreach (var item in currentMaterialItems)
            {
                if (item != null)
                    DestroyImmediate(item);
            }
            currentMaterialItems.Clear();
        }
        
        #endregion
        
        #region 이벤트 처리
        
        /// <summary>
        /// 무기 아이템 클릭 처리
        /// </summary>
        /// <param name="weapon">클릭된 무기</param>
        private void OnWeaponItemClicked(WeaponInstance weapon)
        {
            Debug.Log($"InventoryView: 무기 클릭됨 - {weapon.Name} (모드: {currentMode})");
            OnWeaponClicked?.Invoke(weapon);
        }
        
        /// <summary>
        /// 재료 아이템 클릭 처리
        /// </summary>
        /// <param name="material">클릭된 재료</param>
        private void OnMaterialItemClicked(MaterialInstance material)
        {
            Debug.Log($"InventoryView: 재료 클릭됨 - {material.Name} (모드: {currentMode})");
            OnMaterialClicked?.Invoke(material);
        }
        
        // InventoryController 이벤트 리스너들
        private void OnWeaponAddedToInventory(WeaponInstance weapon)
        {
            if (isWeaponTabActive)
                RefreshWeaponDisplay();
            UpdateDebugInfo();
        }
        
        private void OnWeaponRemovedFromInventory(WeaponInstance weapon)
        {
            if (isWeaponTabActive)
                RefreshWeaponDisplay();
            UpdateDebugInfo();
        }
        
        private void OnMaterialAddedToInventory(MaterialInstance material)
        {
            if (!isWeaponTabActive)
                RefreshMaterialDisplay();
            UpdateDebugInfo();
        }
        
        private void OnMaterialChangedInInventory(MaterialInstance material)
        {
            if (!isWeaponTabActive)
                RefreshMaterialDisplay();
            UpdateDebugInfo();
        }
        
        #endregion
        
        #region 디버그
        
        /// <summary>
        /// 디버그 정보 업데이트
        /// </summary>
        private void UpdateDebugInfo()
        {
            if (debugInfoText == null || InventoryController.Instance == null) return;
            
            int weaponCount = InventoryController.Instance.GetAllWeapons().Count;
            int materialCount = InventoryController.Instance.GetAllMaterials().Count;
            int totalCount = InventoryController.Instance.GetTotalItemCount();
            int maxSize = InventoryController.Instance.maxInventorySize;
            
            debugInfoText.text = $"인벤토리: {totalCount}/{maxSize}\n무기: {weaponCount}개, 재료: {materialCount}종류\n모드: {currentMode}";
        }
        
        /// <summary>
        /// 테스트용 아이템 추가 (디버그)
        /// </summary>
        [ContextMenu("Test Add Items")]
        public void TestAddItems()
        {
            if (InventoryController.Instance == null)
            {
                Debug.LogWarning("InventoryController가 없습니다.");
                return;
            }
            
            if (DataManager.Instance == null)
            {
                Debug.LogWarning("DataManager가 없습니다.");
                return;
            }
            
            // TODO: 3단계에서 실제 데이터로 테스트
            Debug.Log("테스트용 아이템 추가는 3단계에서 구현됩니다.");
        }
        
        #endregion
    }
}