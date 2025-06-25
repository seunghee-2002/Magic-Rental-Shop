using System;

namespace MagicRentalShop.Data.RuntimeInstances
{
    /// <summary>
    /// 플레이어가 실제로 보유한 재료의 런타임 인스턴스
    /// MaterialData(정적)와 달리 개별 수량을 가짐
    /// </summary>
    [System.Serializable]
    public class MaterialInstance
    {
        // 기본 정보
        public MaterialData data;            // 원본 재료 데이터 참조
        
        // 재료 상태
        public int quantity;                 // 보유 수량
        
        /// <summary>
        /// 새로운 재료 인스턴스 생성
        /// </summary>
        /// <param name="materialData">재료 원본 데이터</param>
        /// <param name="initialQuantity">초기 수량</param>
        /// <returns>새로운 MaterialInstance</returns>
        public static MaterialInstance CreateNew(MaterialData materialData, int initialQuantity = 1)
        {
            return new MaterialInstance
            {
                data = materialData,
                quantity = initialQuantity
            };
        }
        
        /// <summary>
        /// 재료 이름 (원본 데이터에서 가져옴)
        /// </summary>
        public string Name => data != null ? data.materialName : "Unknown Material";
        
        /// <summary>
        /// 재료 등급 (원본 데이터에서 가져옴)
        /// </summary>
        public Grade Grade => data != null ? data.grade : Grade.Common;
        
        /// <summary>
        /// 재료 아이콘 (원본 데이터에서 가져옴)
        /// </summary>
        public UnityEngine.Sprite Icon => data != null ? data.icon : null;
        
        /// <summary>
        /// 재료 설명 (원본 데이터에서 가져옴)
        /// </summary>
        public string Description => data != null ? data.description : "";
        
        /// <summary>
        /// 재료 ID (원본 데이터에서 가져옴)
        /// </summary>
        public string ID => data != null ? data.id : "";
        
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
        /// 수량 소모 (제작 시 사용)
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
        /// 수량이 0인지 확인 (인벤토리에서 제거할지 판단용)
        /// </summary>
        public bool IsEmpty => quantity <= 0;
        
        /// <summary>
        /// 재료 인스턴스 비교 (MaterialData ID 기준)
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is MaterialInstance other)
                return ID == other.ID;
            return false;
        }
        
        public override int GetHashCode()
        {
            return ID?.GetHashCode() ?? 0;
        }
        
        /// <summary>
        /// 디버그용 문자열 출력
        /// </summary>
        public override string ToString()
        {
            return $"{Name} x{quantity} (Grade: {Grade}, ID: {ID})";
        }
    }
}