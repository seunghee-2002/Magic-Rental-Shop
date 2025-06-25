using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 무기의 정적 데이터 (ScriptableObject)
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Magic Rental Shop/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("기본 정보")]
        public string id;                    // 무기 고유 ID
        public string weaponName;            // 무기 이름
        public string description;           // 무기 설명
        public Grade grade;                  // 무기 등급
        public Sprite icon;                  // 무기 아이콘
        
        [Header("무기 속성")]
        public Element element;              // 무기 속성
        public List<Element> secondaryElements = new List<Element>(); // 2차 속성 (Legendary용)
        
        [Header("경제 정보")]
        public int basePrice;                // 기본 가격
        
        /// <summary>
        /// Legendary 무기인지 확인 (2개 속성 보유 가능)
        /// </summary>
        public bool IsLegendary => grade == Grade.Legendary;
        
        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = name; // 파일명을 기본 ID로 사용
            }
            
            // Legendary가 아닌데 2차 속성이 있으면 경고
            if (!IsLegendary && secondaryElements.Count > 0)
            {
                secondaryElements.Clear();
                Debug.LogWarning($"{weaponName}: Legendary 등급이 아닌 무기는 2차 속성을 가질 수 없습니다.");
            }
        }
    }
}