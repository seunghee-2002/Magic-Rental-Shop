using System;
using MagicRentalShop.Data;

namespace MagicRentalShop.Data.RuntimeInstances
{
    /// <summary>
    /// Customer에서 전환된 Hero의 런타임 인스턴스
    /// TODO: 6-7단계에서 완전히 구현
    /// </summary>
    [System.Serializable]
    public class HeroInstance
    {
        // 기본 정보
        public CustomerData data;            // Hero의 원본 정보 (Customer 데이터 재활용)
        public string uniqueID;              // Hero의 고유 식별자
        
        // Hero 상태
        public int level;                    // Hero의 현재 레벨 (성장 가능)
        public int acquiredLevel;            // Hero로 전환 당시의 레벨 (기록용)
        public int convertedDate;            // Hero로 전환된 날짜
        
        /// <summary>
        /// 새로운 Hero 인스턴스 생성
        /// TODO: 6-7단계에서 완전히 구현
        /// </summary>
        /// <param name="customerData">원본 Customer 데이터</param>
        /// <param name="currentLevel">현재 레벨</param>
        /// <param name="conversionDate">전환된 날짜</param>
        /// <returns>새로운 HeroInstance</returns>
        public static HeroInstance CreateFromCustomer(CustomerData customerData, int currentLevel, int conversionDate)
        {
            return new HeroInstance
            {
                data = customerData,
                uniqueID = Guid.NewGuid().ToString(),
                level = currentLevel,
                acquiredLevel = currentLevel,
                convertedDate = conversionDate
            };
        }
        
        /// <summary>
        /// Hero 이름 (원본 데이터에서 가져옴)
        /// </summary>
        public string Name => data != null ? data.customerName : "Unknown Hero";
        
        /// <summary>
        /// Hero 등급 (원본 데이터에서 가져옴)
        /// </summary>
        public Grade Grade => data != null ? data.grade : Grade.Common;
        
        /// <summary>
        /// Hero 속성 (원본 데이터에서 가져옴)
        /// </summary>
        public Element Element => data != null ? data.element : Element.Fire;
        
        /// <summary>
        /// Hero 아이콘 (원본 데이터에서 가져옴)
        /// </summary>
        public UnityEngine.Sprite Icon => data != null ? data.icon : null;
        
        /// <summary>
        /// Hero 설명 (원본 데이터에서 가져옴)
        /// </summary>
        public string Description => data != null ? data.description : "";
        
        /// <summary>
        /// Hero ID (원본 데이터에서 가져옴)
        /// </summary>
        public string ID => data != null ? data.id : "";
        
        /// <summary>
        /// Hero 레벨업 처리
        /// TODO: 6-7단계에서 완전히 구현
        /// </summary>
        /// <param name="amount">레벨업 양</param>
        public void LevelUp(int amount = 1)
        {
            level += amount;
            // TODO: 최대 레벨 제한 확인
        }
        
        /// <summary>
        /// 디버그용 문자열 출력
        /// </summary>
        public override string ToString()
        {
            return $"★{Name} Lv.{level} (Grade: {Grade}, Element: {Element}, Acquired: Day {convertedDate})";
        }
    }
}