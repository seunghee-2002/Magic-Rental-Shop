using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 고객의 정적 데이터 (ScriptableObject)
    /// Hero 시스템에서도 재활용됨
    /// </summary>
    [CreateAssetMenu(fileName = "CustomerData", menuName = "Magic Rental Shop/Customer Data")]
    public class CustomerData : ScriptableObject
    {
        [Header("기본 정보")]
        public string id;                    // 고객 고유 ID
        public string customerName;          // 고객 이름
        public string description;           // 고객 설명
        public Grade grade;                  // 고객 등급
        public Sprite icon;                  // 고객 아이콘
        
        [Header("고객 속성")]
        public Element element;              // 고객 속성
        
        [Header("Hero 전환 정보 (6-7단계)")]
        [Range(0f, 10f)]
        public float baseConversionRate = 1f; // 기본 Hero 전환 확률 (%)
        
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