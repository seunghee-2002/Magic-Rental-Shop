using MagicRentalShop.Data;

namespace MagicRentalShop.Data.RuntimeInstances
{
    /// <summary>
    /// 완료된 모험의 결과 데이터
    /// TODO: 3단계에서 완전히 구현
    /// </summary>
    [System.Serializable]
    public class AdventureResultData
    {
        // 모험 참가자
        public CustomerInstance customer;    // 모험한 고객
        public WeaponInstance weapon;        // 사용한 무기
        public DungeonData dungeon;          // 모험한 던전
        
        // 결과
        public bool isSuccess;               // 성공 여부
        public bool isWeaponRecovered;       // 무기 회수 여부
        public int goldReward;               // 골드 보상
        public MaterialInstance[] materialRewards; // 재료 보상
        
        // Hero 시스템 (6-7단계)
        public bool isHero;                  // Hero 모험이었는지 구분
        public bool heroConverted;           // Customer가 Hero로 전환되었는지
        public bool heroLevelUp;             // Hero 레벨업 여부
        public int heroNewLevel;             // Hero 새로운 레벨
        
        /// <summary>
        /// 새로운 모험 결과 생성
        /// TODO: 3단계에서 완전히 구현
        /// </summary>
        public static AdventureResultData CreateNew(CustomerInstance customer, WeaponInstance weapon, DungeonData dungeon)
        {
            return new AdventureResultData
            {
                customer = customer,
                weapon = weapon,
                dungeon = dungeon,
                isSuccess = false, // TODO: 성공률 계산
                isWeaponRecovered = false, // TODO: 회수 확률 계산
                goldReward = 0, // TODO: 보상 계산
                materialRewards = new MaterialInstance[0], // TODO: 재료 보상
                isHero = false, // TODO: Hero 시스템
                heroConverted = false,
                heroLevelUp = false,
                heroNewLevel = 0
            };
        }
        
        /// <summary>
        /// 결과 요약 문자열
        /// </summary>
        public string GetResultSummary()
        {
            string heroMark = isHero ? "★" : "";
            string result = isSuccess ? "성공" : "실패";
            string weaponStatus = isWeaponRecovered ? "회수됨" : "분실";
            
            return $"{customer?.Name}{heroMark}: {result}, 무기 {weaponStatus}";
        }
        
        /// <summary>
        /// 디버그용 문자열 출력
        /// </summary>
        public override string ToString()
        {
            return GetResultSummary();
        }
    }
}