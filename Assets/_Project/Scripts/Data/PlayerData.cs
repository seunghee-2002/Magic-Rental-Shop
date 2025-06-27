using System.Collections.Generic;
using UnityEngine;
using MagicRentalShop.Data.RuntimeInstances;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 게임의 모든 동적 상태를 담는 저장용 데이터 클래스
    /// 2단계: 기본 게임 루프 + 인벤토리를 위한 기본 필드들
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        // 기본 게임 정보
        public int gold = 5000;                    // 현재 보유 골드 (GameConfig.startingGold에서 초기화)
        public int currentDay = 1;                 // 현재 게임 진행 일자
        public GamePhase currentPhase = GamePhase.Morning; // 현재 페이즈
        public float phaseRemainingTime = 300f;    // 현재 페이즈 남은 시간(초)
        
        // 인벤토리 (2단계)
        public List<WeaponInstance> ownedWeapons = new List<WeaponInstance>();   // 보유한 무기 목록
        public List<MaterialInstance> ownedMaterials = new List<MaterialInstance>(); // 보유한 재료 목록
        
        // TODO: 추후 단계에서 추가될 필드들
        // [Header("고객 시스템 (3단계)")]
        // public List<CustomerInstance> visitingCustomers;
        
        // [Header("Hero 시스템 (6-7단계)")]
        // public List<HeroInstance> ownedHeroes;
        // public List<InjuredHeroData> injuredHeroes;
        // public Dictionary<string, HeroCollectionData> heroCollection;
        
        // [Header("모험 시스템 (3단계)")]
        // public List<AdventureInstance> ongoingAdventures;
        // public List<AdventureResultData> completedAdventures;
        
        // [Header("제작 시스템 (5단계)")]
        // public List<CraftingInstance> ongoingCrafting;
        
        // [Header("상점 시스템 (4단계)")]
        // public List<string> purchasedWeaponIDs; // 오늘 구매한 무기 (새로고침용)
        // public List<string> unlockedRecipeIDs;  // 해금된 레시피
        
        public PlayerData()
        {
            ownedWeapons = new List<WeaponInstance>();
            ownedMaterials = new List<MaterialInstance>();
            
            Debug.Log("PlayerData: 생성자에서 리스트 초기화 완료");
        }

        /// <summary>
        /// PlayerData 초기화 (새 게임 시작 시)
        /// </summary>
        /// <param name="gameConfig">게임 설정</param>
        public void InitializeNewGame(GameConfig gameConfig)
        {
            if (gameConfig == null) return;
            
            gold = gameConfig.startingGold;
            currentDay = 1;
            currentPhase = GamePhase.Morning;
            phaseRemainingTime = gameConfig.dayDuration;
            
            // 인벤토리 초기화
            ownedWeapons.Clear();
            ownedMaterials.Clear();
            
            // TODO: 테스트용 초기 아이템 추가 (3단계에서 제거)
            // AddTestItems();
        }
        
        /// <summary>
        /// 디버그/테스트용 초기 아이템 추가 (임시)
        /// </summary>
        private void AddTestItems()
        {
            // 실제 WeaponData, MaterialData가 로드된 후 DataManager를 통해 추가
            // 현재는 주석 처리
        }
        
        /// <summary>
        /// 페이즈 전환 시 데이터 업데이트
        /// </summary>
        /// <param name="newPhase">새로운 페이즈</param>
        /// <param name="remainingTime">남은 시간</param>
        public void UpdatePhase(GamePhase newPhase, float remainingTime)
        {
            currentPhase = newPhase;
            phaseRemainingTime = remainingTime;
        }
        
        /// <summary>
        /// 날짜 진행 (밤 → 아침 전환 시)
        /// </summary>
        public void AdvanceDay()
        {
            currentDay++;
            currentPhase = GamePhase.Morning;
            // phaseRemainingTime은 GameController에서 설정
        }
        
        /// <summary>
        /// 골드 변경 (음수 가능)
        /// </summary>
        /// <param name="amount">변경할 골드 양</param>
        /// <returns>변경 후 골드</returns>
        public int ChangeGold(int amount)
        {
            gold += amount;
            if (gold < 0) gold = 0; // 음수 방지
            return gold;
        }
        
        /// <summary>
        /// 골드 지출 가능 여부 확인
        /// </summary>
        /// <param name="amount">필요한 골드</param>
        /// <returns>지출 가능하면 true</returns>
        public bool CanAfford(int amount)
        {
            return gold >= amount;
        }
        
        /// <summary>
        /// 디버그용 정보 출력
        /// </summary>
        public override string ToString()
        {
            return $"Day {currentDay}, Phase: {currentPhase}, Gold: {gold}, " +
                   $"Weapons: {ownedWeapons.Count}, Materials: {ownedMaterials.Count}";
        }
    }
}