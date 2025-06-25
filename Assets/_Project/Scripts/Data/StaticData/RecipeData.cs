using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 제작 레시피 데이터
    /// </summary>
    [System.Serializable]
    public class RecipeIngredient
    {
        public MaterialData material;        // 필요한 재료
        public int quantity;                 // 필요한 수량
    }
    
    /// <summary>
    /// 제작 레시피의 정적 데이터 (ScriptableObject)
    /// </summary>
    [CreateAssetMenu(fileName = "RecipeData", menuName = "Magic Rental Shop/Recipe Data")]
    public class RecipeData : ScriptableObject
    {
        [Header("기본 정보")]
        public string id;                    // 레시피 고유 ID
        public string recipeName;            // 레시피 이름
        public string description;           // 레시피 설명
        public Sprite icon;                  // 레시피 아이콘
        
        [Header("제작 정보")]
        public WeaponData resultWeapon;      // 제작될 무기
        public List<RecipeIngredient> ingredients = new List<RecipeIngredient>(); // 필요한 재료들
        public int craftingCost;             // 제작 비용 (골드)
        public int craftingDays = 1;         // 제작 소요 일수
        
        [Header("해금 조건")]
        public int unlockDay = 1;            // 해금되는 일차
        
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