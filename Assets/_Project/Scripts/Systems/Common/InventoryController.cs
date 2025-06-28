using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using MagicRentalShop.Core;
using MagicRentalShop.Data;
using MagicRentalShop.Data.RuntimeInstances;

namespace MagicRentalShop.Systems.Common
{
    /// <summary>
    /// 플레이어 인벤토리 관리 시스템
    /// 무기와 재료의 추가/제거/조회를 담당
    /// </summary>
    public class InventoryController : MonoBehaviour
    {
        [Header("설정")]
        public int maxInventorySize = 100;       // 최대 인벤토리 크기
        
        [Header("이벤트")]
        public UnityEvent<WeaponInstance> OnWeaponAdded;        // 무기 추가 시
        public UnityEvent<WeaponInstance> OnWeaponRemoved;      // 무기 제거 시  
        public UnityEvent<MaterialInstance> OnMaterialAdded;    // 재료 추가 시
        public UnityEvent<MaterialInstance> OnMaterialChanged;  // 재료 수량 변경 시
        
        // 싱글톤 패턴
        public static InventoryController Instance { get; private set; }
        
        // 캐시된 PlayerData 참조
        private PlayerData playerData;
        
        private void Awake()
        {
            // 싱글톤 설정
            if (Instance == null)
            {
                Instance = this;
                transform.parent = null; // 루트 오브젝트로 이동
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // GameController에서 PlayerData 가져오기
            if (GameController.Instance != null)
            {
                playerData = GameController.Instance.PlayerData;
            }
        }
        
        #region 무기 관리
        
        /// <summary>
        /// 무기 추가 (구매, 제작 완료 시 사용)
        /// </summary>
        /// <param name="weaponData">추가할 무기 데이터</param>
        /// <param name="quantity">추가할 수량</param>
        /// <returns>생성된 WeaponInstance</returns>
        public WeaponInstance AddWeapon(WeaponData weaponData, int quantity = 1)
        {
            if (playerData == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return null;
            }
            
            if (weaponData == null)
            {
                Debug.LogError("InventoryController: weaponData가 null입니다.");
                return null;
            }
            
            if (GetTotalItemCount() >= maxInventorySize)
            {
                Debug.LogWarning("InventoryController: 인벤토리가 가득 찼습니다.");
                return null;
            }
            
            // 기존에 같은 무기가 있는지 확인
            WeaponInstance existingWeapon = playerData.ownedWeapons
                .FirstOrDefault(w => w.data != null && w.data.id == weaponData.id);
            
            if (existingWeapon != null)
            {
                // 기존 무기 수량 증가
                existingWeapon.AddQuantity(quantity);
                OnWeaponAdded?.Invoke(existingWeapon);
                Debug.Log($"InventoryController: 무기 수량 증가 - {existingWeapon.Name} (+{quantity})");
                return existingWeapon;
            }
            else
            {
                // 새로운 무기 추가
                WeaponInstance newWeapon = WeaponInstance.CreateNew(weaponData, quantity);
                playerData.ownedWeapons.Add(newWeapon);
                
                OnWeaponAdded?.Invoke(newWeapon);
                Debug.Log($"InventoryController: 새 무기 추가 - {newWeapon.Name} x{quantity}");
                
                return newWeapon;
            }
        }
        
        /// <summary>
        /// 무기 제거 (판매, 모험 실패 시 사용)
        /// </summary>
        /// <param name="weapon">제거할 무기</param>
        /// <param name="quantity">제거할 수량 (기본값: 1)</param>
        /// <returns>제거 성공 여부</returns>
        public bool RemoveWeapon(WeaponInstance weapon, int quantity = 1)
        {
            if (playerData == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return false;
            }
            
            if (weapon == null || !playerData.ownedWeapons.Contains(weapon))
            {
                Debug.LogWarning("InventoryController: 제거할 무기를 찾을 수 없습니다.");
                return false;
            }
            
            int consumed = weapon.ConsumeQuantity(quantity);
            
            if (weapon.IsEmpty)
            {
                // 수량이 0이 되면 인벤토리에서 완전 제거
                playerData.ownedWeapons.Remove(weapon);
                OnWeaponRemoved?.Invoke(weapon);
                Debug.Log($"InventoryController: 무기 완전 제거됨 - {weapon.Name}");
            }
            else
            {
                OnWeaponAdded?.Invoke(weapon); // 수량 변경 알림
                Debug.Log($"InventoryController: 무기 수량 감소 - {weapon.Name} (-{consumed})");
            }
            
            return consumed > 0;
        }
        
        /// <summary>
        /// 보유한 모든 무기 목록 가져오기
        /// </summary>
        /// <returns>무기 목록</returns>
        public List<WeaponInstance> GetAllWeapons()
        {
            if (playerData?.ownedWeapons == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return new List<WeaponInstance>();
            }
            
            return new List<WeaponInstance>(playerData.ownedWeapons);
        }
        
        /// <summary>
        /// 특정 조건의 무기들 가져오기
        /// </summary>
        /// <param name="grade">등급 필터 (null이면 전체)</param>
        /// <param name="element">속성 필터 (null이면 전체)</param>
        /// <returns>필터링된 무기 목록</returns>
        public List<WeaponInstance> GetWeapons(Grade? grade = null, Element? element = null)
        {
            if (playerData == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return new List<WeaponInstance>();
            }
            
            var weapons = playerData.ownedWeapons.AsEnumerable();
            
            if (grade.HasValue)
                weapons = weapons.Where(w => w.Grade == grade.Value);
                
            if (element.HasValue)
                weapons = weapons.Where(w => w.Element == element.Value);
                
            return weapons.ToList();
        }
        
        /// <summary>
        /// 무기 인스턴스 ID로 무기 찾기
        /// </summary>
        /// <param name="weaponInstanceID">무기 인스턴스 ID</param>
        /// <returns>찾은 무기 (없으면 null)</returns>
        public WeaponInstance GetWeaponByInstanceID(string weaponInstanceID)
        {
            if (playerData == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return null;
            }
            
            return playerData.ownedWeapons.FirstOrDefault(w => w.uniqueID == weaponInstanceID);
        }
        
        /// <summary>
        /// 무기 보유 수량 확인
        /// </summary>
        /// <param name="weaponData">확인할 무기</param>
        /// <returns>보유 수량</returns>
        public int GetWeaponQuantity(WeaponData weaponData)
        {
            if (playerData == null)
                return 0;
            
            WeaponInstance weapon = playerData.ownedWeapons
                .FirstOrDefault(w => w.data != null && w.data.id == weaponData.id);
            
            return weapon?.quantity ?? 0;
        }
        
        /// <summary>
        /// 무기 대여 가능 여부 확인
        /// </summary>
        /// <param name="weaponData">필요한 무기</param>
        /// <param name="requiredQuantity">필요한 수량</param>
        /// <returns>대여 가능하면 true</returns>
        public bool CanRentWeapon(WeaponData weaponData, int requiredQuantity = 1)
        {
            if (playerData == null)
                return false;
            
            return GetWeaponQuantity(weaponData) >= requiredQuantity;
        }
        
        #endregion
        
        #region 재료 관리
        
        /// <summary>
        /// 재료 추가 (모험 성공 시 사용)
        /// </summary>
        /// <param name="materialData">추가할 재료 데이터</param>
        /// <param name="quantity">추가할 수량</param>
        /// <returns>업데이트된 MaterialInstance</returns>
        public MaterialInstance AddMaterial(MaterialData materialData, int quantity = 1)
        {
            if (playerData == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return null;
            }
            
            if (materialData == null || quantity <= 0)
            {
                Debug.LogError("InventoryController: 잘못된 재료 데이터 또는 수량입니다.");
                return null;
            }
            
            // 기존에 같은 재료가 있는지 확인
            MaterialInstance existingMaterial = playerData.ownedMaterials
                .FirstOrDefault(m => m.ID == materialData.id);
            
            if (existingMaterial != null)
            {
                // 기존 재료 수량 증가
                existingMaterial.AddQuantity(quantity);
                OnMaterialChanged?.Invoke(existingMaterial);
                Debug.Log($"InventoryController: 재료 수량 증가 - {existingMaterial.Name} (+{quantity})");
                return existingMaterial;
            }
            else
            {
                // 새로운 재료 추가
                if (GetTotalItemCount() >= maxInventorySize)
                {
                    Debug.LogWarning("InventoryController: 인벤토리가 가득 찼습니다.");
                    return null;
                }
                
                MaterialInstance newMaterial = MaterialInstance.CreateNew(materialData, quantity);
                playerData.ownedMaterials.Add(newMaterial);
                
                OnMaterialAdded?.Invoke(newMaterial);
                Debug.Log($"InventoryController: 새 재료 추가 - {newMaterial.Name} x{quantity}");
                return newMaterial;
            }
        }
        
        /// <summary>
        /// 재료 소모 (제작 시 사용)
        /// </summary>
        /// <param name="materialData">소모할 재료 데이터</param>
        /// <param name="quantity">소모할 수량</param>
        /// <returns>실제로 소모된 수량</returns>
        public int ConsumeMaterial(MaterialData materialData, int quantity)
        {
            if (playerData == null)
                return 0;
            
            if (materialData == null || quantity <= 0)
                return 0;
            
            MaterialInstance material = playerData.ownedMaterials
                .FirstOrDefault(m => m.ID == materialData.id);
            
            if (material == null)
                return 0;
            
            int consumed = material.ConsumeQuantity(quantity);
            
            // 수량이 0이 되면 인벤토리에서 제거
            if (material.IsEmpty)
            {
                playerData.ownedMaterials.Remove(material);
                Debug.Log($"InventoryController: 재료 완전 소모로 제거 - {material.Name}");
            }
            else
            {
                OnMaterialChanged?.Invoke(material);
            }
            
            Debug.Log($"InventoryController: 재료 소모 - {material.Name} (-{consumed})");
            return consumed;
        }
        
        /// <summary>
        /// 재료 보유 수량 확인
        /// </summary>
        /// <param name="materialData">확인할 재료</param>
        /// <returns>보유 수량</returns>
        public int GetMaterialQuantity(MaterialData materialData)
        {
            if (playerData == null)
                return 0;
            
            MaterialInstance material = playerData.ownedMaterials
                .FirstOrDefault(m => m.ID == materialData.id);
            
            return material?.quantity ?? 0;
        }
        
        /// <summary>
        /// 재료 제작 가능 여부 확인
        /// </summary>
        /// <param name="materialData">필요한 재료</param>
        /// <param name="requiredQuantity">필요한 수량</param>
        /// <returns>제작 가능하면 true</returns>
        public bool CanCraftWithMaterial(MaterialData materialData, int requiredQuantity)
        {
            if (playerData == null)
                return false;
            
            return GetMaterialQuantity(materialData) >= requiredQuantity;
        }
        
        /// <summary>
        /// 보유한 모든 재료 목록 가져오기
        /// </summary>
        /// <returns>재료 목록</returns>
        public List<MaterialInstance> GetAllMaterials()
        {
            // PlayerData가 아직 초기화되지 않았으면 빈 리스트 반환
            if (playerData?.ownedMaterials == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return new List<MaterialInstance>();
            }
            return new List<MaterialInstance>(playerData.ownedMaterials);
        }
        
        #endregion
        
        #region 유틸리티
        
        /// <summary>
        /// 전체 아이템 개수 (무기 + 재료 종류)
        /// </summary>
        /// <returns>전체 아이템 개수</returns>
        public int GetTotalItemCount()
        {
            if (playerData == null)
                return 0;
            
            return playerData.ownedWeapons.Count + playerData.ownedMaterials.Count;
        }
        
        /// <summary>
        /// 인벤토리 여유 공간
        /// </summary>
        /// <returns>여유 공간</returns>
        public int GetRemainingSpace()
        {
            if (playerData == null)
                return maxInventorySize;
            
            return maxInventorySize - GetTotalItemCount();
        }
        
        /// <summary>
        /// 인벤토리가 가득 찼는지 확인
        /// </summary>
        /// <returns>가득 찼으면 true</returns>
        public bool IsInventoryFull()
        {
            if (playerData == null)
                return false;
            
            return GetTotalItemCount() >= maxInventorySize;
        }
        
        /// <summary>
        /// 무기 판매 가격 계산 (구매가의 50%)
        /// TODO: 4단계에서 구현
        /// </summary>
        /// <param name="weapon">판매할 무기</param>
        /// <returns>판매 가격</returns>
        public int CalculateSellPrice(WeaponInstance weapon)
        {
            if (playerData == null)
                return 0;
            
            return weapon?.SellPrice ?? 0;
        }
        
        /// <summary>
        /// 무기 판매 처리
        /// TODO: 4단계에서 구현
        /// </summary>
        /// <param name="weapon">판매할 무기</param>
        /// <returns>판매 가격</returns>
        public int SellWeapon(WeaponInstance weapon)
        {
            if (playerData == null)
                return 0;
            
            int sellPrice = CalculateSellPrice(weapon);
            if (RemoveWeapon(weapon))
            {
                playerData.ChangeGold(sellPrice);
                Debug.Log($"InventoryController: 무기 판매됨 - {weapon.Name} ({sellPrice}골드)");
                return sellPrice;
            }
            
            return 0;
        }
        
        /// <summary>
        /// 디버그용 인벤토리 상태 출력
        /// </summary>
        [ContextMenu("Debug Inventory")]
        public void DebugInventory()
        {
            if (playerData == null)
            {
                Debug.LogWarning("InventoryController: PlayerData가 아직 초기화되지 않았습니다.");
                return;
            }
            
            Debug.Log($"=== 인벤토리 상태 ({GetTotalItemCount()}/{maxInventorySize}) ===");
            Debug.Log($"무기 ({playerData.ownedWeapons.Count}개):");
            foreach (var weapon in playerData.ownedWeapons)
            {
                Debug.Log($"  - {weapon}");
            }
            
            Debug.Log($"재료 ({playerData.ownedMaterials.Count}종류):");
            foreach (var material in playerData.ownedMaterials)
            {
                Debug.Log($"  - {material}");
            }
        }
        
        // playerData 초기화 여부 확인용 메서드
        public bool IsInitialized()
        {
            return playerData != null;
        }
        
        #endregion
    }
}