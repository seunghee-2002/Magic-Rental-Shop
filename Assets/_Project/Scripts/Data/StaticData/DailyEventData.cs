using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 일일 이벤트 종류
    /// </summary>
    public enum DailyEventType
    {
        GoldBonus,          // 골드 보너스
        GoldPenalty,        // 골드 페널티
        WeaponShopDiscount, // 무기상점 할인
        ExtraWeapons,       // 무기상점 추가 진열
        ExtraCustomers      // 추가 고객 방문
    }
    
    /// <summary>
    /// 일일 이벤트의 정적 데이터 (ScriptableObject)
    /// </summary>
    [CreateAssetMenu(fileName = "DailyEventData", menuName = "Magic Rental Shop/Daily Event Data")]
    public class DailyEventData : ScriptableObject
    {
        [Header("기본 정보")]
        public string id;                    // 이벤트 고유 ID
        public string eventName;             // 이벤트 이름
        public string description;           // 이벤트 설명
        public Sprite icon;                  // 이벤트 아이콘
        
        [Header("이벤트 효과")]
        public DailyEventType eventType;     // 이벤트 종류
        public int effectValue;              // 효과 수치
        
        [Header("발생 확률")]
        [Range(0f, 100f)]
        public float probability = 10f;      // 발생 확률 (%)
        
        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = name; // 파일명을 기본 ID로 사용
            }
        }
    }
}