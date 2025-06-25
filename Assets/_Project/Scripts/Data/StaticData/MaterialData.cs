using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 재료의 정적 데이터 (ScriptableObject)
    /// </summary>
    [CreateAssetMenu(fileName = "MaterialData", menuName = "Magic Rental Shop/Material Data")]
    public class MaterialData : ScriptableObject
    {
        [Header("기본 정보")]
        public string id;                    // 재료 고유 ID
        public string materialName;          // 재료 이름
        public string description;           // 재료 설명
        public Grade grade;                  // 재료 등급
        public Sprite icon;                  // 재료 아이콘
        
        [Header("획득 정보 (Hero 시스템 확장)")]
        public List<string> availableDungeonIDs = new List<string>(); // 이 재료를 얻을 수 있는 던전 ID 목록
        
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