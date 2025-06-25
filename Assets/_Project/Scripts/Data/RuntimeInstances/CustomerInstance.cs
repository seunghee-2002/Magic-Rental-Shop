using System;
using MagicRentalShop.Data;

namespace MagicRentalShop.Data.RuntimeInstances
{
    /// <summary>
    /// 방문한 고객의 런타임 인스턴스
    /// CustomerData(정적)와 달리 개별 상태(레벨, 던전 등)를 가짐
    /// TODO: 3단계에서 완전히 구현
    /// </summary>
    [System.Serializable]
    public class CustomerInstance
    {
        // 기본 정보
        public CustomerData data;           // 원본 고객 데이터 참조
        public string uniqueID;             // 각 고객 인스턴스의 고유 식별자
        
        // 고객 상태
        public int level;                   // 고객 레벨
        public DungeonData assignedDungeon; // 배정된 던전
        
        /// <summary>
        /// 새로운 고객 인스턴스 생성
        /// TODO: 3단계에서 완전히 구현
        /// </summary>
        /// <param name="customerData">고객 원본 데이터</param>
        /// <returns>새로운 CustomerInstance</returns>
        public static CustomerInstance CreateNew(CustomerData customerData)
        {
            return new CustomerInstance
            {
                data = customerData,
                uniqueID = Guid.NewGuid().ToString(),
                level = 1, // TODO: 실제 레벨 계산
                assignedDungeon = null // TODO: 던전 배정 로직
            };
        }
        
        /// <summary>
        /// 고객 이름 (원본 데이터에서 가져옴)
        /// </summary>
        public string Name => data != null ? data.customerName : "Unknown Customer";
        
        /// <summary>
        /// 고객 등급 (원본 데이터에서 가져옴)
        /// </summary>
        public Grade Grade => data != null ? data.grade : Grade.Common;
        
        /// <summary>
        /// 고객 속성 (원본 데이터에서 가져옴)
        /// </summary>
        public Element Element => data != null ? data.element : Element.Fire;
        
        /// <summary>
        /// 고객 아이콘 (원본 데이터에서 가져옴)
        /// </summary>
        public UnityEngine.Sprite Icon => data != null ? data.icon : null;
        
        /// <summary>
        /// 고객 설명 (원본 데이터에서 가져옴)
        /// </summary>
        public string Description => data != null ? data.description : "";
        
        /// <summary>
        /// 고객 ID (원본 데이터에서 가져옴)
        /// </summary>
        public string ID => data != null ? data.id : "";
        
        /// <summary>
        /// 디버그용 문자열 출력
        /// </summary>
        public override string ToString()
        {
            return $"{Name} Lv.{level} (Grade: {Grade}, Element: {Element}, ID: {uniqueID})";
        }
    }
}