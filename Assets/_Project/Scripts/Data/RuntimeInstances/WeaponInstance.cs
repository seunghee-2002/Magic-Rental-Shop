using System;
using MagicRentalShop.Data;

namespace MagicRentalShop.Data.RuntimeInstances
{
    /// <summary>
    /// 플레이어가 실제로 보유한 무기의 런타임 인스턴스
    /// WeaponData(정적)와 달리 개별 상태(강화 레벨 등)를 가짐
    /// </summary>
    [System.Serializable]
    public class WeaponInstance
    {
        // 기본 정보
        public WeaponData data;              // 원본 무기 데이터 참조
        public string uniqueID;              // 각 무기 인스턴스의 고유 식별자
        
        // 무기 상태
        public int quantity;                 // 보유 수량
        public int enhancementLevel;         // 강화 레벨 (현재는 미사용, 추후 확장용)
        
        /// <summary>
        /// 새로운 무기 인스턴스 생성
        /// </summary>
        /// <param name="weaponData">무기 원본 데이터</param>
        /// <param name="initialQuantity">초기 수량</param>
        /// <returns>새로운 WeaponInstance</returns>
        public static WeaponInstance CreateNew(WeaponData weaponData, int initialQuantity = 1)
        {
            return new WeaponInstance
            {
                data = weaponData,
                uniqueID = Guid.NewGuid().ToString(),
                quantity = initialQuantity,
                enhancementLevel = 0
            };
        }
        
        /// <summary>
        /// 무기 이름 (원본 데이터에서 가져옴)
        /// </summary>
        public string Name => data != null ? data.weaponName : "Unknown Weapon";
        
        /// <summary>
        /// 무기 등급 (원본 데이터에서 가져옴)
        /// </summary>
        public Grade Grade => data != null ? data.grade : Grade.Common;
        
        /// <summary>
        /// 무기 속성 (원본 데이터에서 가져옴)
        /// </summary>
        public Element Element => data != null ? data.element : Element.Fire;
        
        /// <summary>
        /// 무기 아이콘 (원본 데이터에서 가져옴)
        /// </summary>
        public UnityEngine.Sprite Icon => data != null ? data.icon : null;
        
        /// <summary>
        /// 무기 설명 (원본 데이터에서 가져옴)
        /// </summary>
        public string Description => data != null ? data.description : "";
        
        /// <summary>
        /// 무기 가격 (원본 데이터에서 가져옴)
        /// </summary>
        public int BasePrice => data != null ? data.basePrice : 0;
        
        /// <summary>
        /// 판매 가격 계산 (구매가의 50%)
        /// </summary>
        public int SellPrice => (int)(BasePrice * 0.5f);
        
        /// <summary>
        /// 수량이 0인지 확인 (인벤토리에서 제거할지 판단용)
        /// </summary>
        public bool IsEmpty => quantity <= 0;
        
        /// <summary>
        /// 수량 추가
        /// </summary>
        /// <param name="amount">추가할 수량</param>
        public void AddQuantity(int amount)
        {
            if (amount > 0)
                quantity += amount;
        }
        
        /// <summary>
        /// 수량 소모 (대여 시 사용)
        /// </summary>
        /// <param name="amount">소모할 수량</param>
        /// <returns>실제로 소모된 수량</returns>
        public int ConsumeQuantity(int amount)
        {
            if (amount <= 0) return 0;
            
            int consumed = UnityEngine.Mathf.Min(amount, quantity);
            quantity -= consumed;
            return consumed;
        }
        
        /// <summary>
        /// 충분한 수량이 있는지 확인
        /// </summary>
        /// <param name="requiredAmount">필요한 수량</param>
        /// <returns>충분한 수량이 있으면 true</returns>
        public bool HasEnoughQuantity(int requiredAmount)
        {
            return quantity >= requiredAmount;
        }
        
        /// <summary>
        /// 무기 인스턴스 비교 (uniqueID 기준)
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is WeaponInstance other)
                return uniqueID == other.uniqueID;
            return false;
        }
        
        public override int GetHashCode()
        {
            return uniqueID?.GetHashCode() ?? 0;
        }
        
        /// <summary>
        /// 디버그용 문자열 출력
        /// </summary>
        public override string ToString()
        {
            return $"{Name} x{quantity} (Grade: {Grade}, Element: {Element}, ID: {uniqueID})";
        }
    }
}