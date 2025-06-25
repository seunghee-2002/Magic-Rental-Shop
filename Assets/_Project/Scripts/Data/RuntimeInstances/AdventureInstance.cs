using MagicRentalShop.Data;

namespace MagicRentalShop.Data.RuntimeInstances
{
    /// <summary>
    /// 진행 중인 모험의 런타임 인스턴스
    /// TODO: 3단계에서 완전히 구현
    /// </summary>
    [System.Serializable]
    public class AdventureInstance
    {
        // 모험 참가자
        public CustomerInstance customer;    // 모험하는 고객 (Hero일 수도 있음)
        public WeaponInstance weapon;        // 대여한 무기
        public DungeonData dungeon;          // 모험할 던전
        
        // 모험 상태
        public int remainingDays;            // 남은 일수
        public bool isHero;                  // Hero 모험인지 구분 플래그
        
        /// <summary>
        /// 새로운 모험 인스턴스 생성
        /// TODO: 3단계에서 완전히 구현
        /// </summary>
        public static AdventureInstance CreateNew(CustomerInstance customer, WeaponInstance weapon, DungeonData dungeon, int duration)
        {
            return new AdventureInstance
            {
                customer = customer,
                weapon = weapon,
                dungeon = dungeon,
                remainingDays = duration,
                isHero = false // TODO: Hero 시스템에서 설정
            };
        }
        
        /// <summary>
        /// 모험이 완료되었는지 확인
        /// </summary>
        public bool IsCompleted => remainingDays <= 0;
        
        /// <summary>
        /// 하루 경과 처리
        /// </summary>
        public void AdvanceDay()
        {
            if (remainingDays > 0)
                remainingDays--;
        }
        
        /// <summary>
        /// 디버그용 문자열 출력
        /// </summary>
        public override string ToString()
        {
            string heroMark = isHero ? "★" : "";
            return $"{customer?.Name}{heroMark} → {dungeon?.dungeonName} ({remainingDays}일 남음)";
        }
    }
}