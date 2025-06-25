using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 던전의 정적 데이터 (ScriptableObject)
    /// </summary>
    [CreateAssetMenu(fileName = "DungeonData", menuName = "Magic Rental Shop/Dungeon Data")]
    public class DungeonData : ScriptableObject
    {
        [Header("기본 정보")]
        public string id;                    // 던전 고유 ID
        public string dungeonName;           // 던전 이름
        public string description;           // 던전 설명
        public Grade grade;                  // 던전 등급
        public Sprite icon;                  // 던전 아이콘
        
        [Header("던전 속성")]
        public Element element;              // 던전 속성
        public int baseLevel;                // 기본 레벨
        
        [Header("보상 정보")]
        public int baseGoldReward = 300;     // 기본 골드 보상
        public List<MaterialData> availableMaterials = new List<MaterialData>(); // 획득 가능한 재료들
        
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