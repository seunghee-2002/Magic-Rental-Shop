using UnityEngine;

namespace MagicRentalShop.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Magic Rental Shop/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("기본 게임 설정")]
        public int startingGold = 5000;
        public float dayDuration = 300f; // 5분
        public int blacksmithUnlockDay = 10;
        public int weaponShopItemCount = 5;
        
        [Header("UI 설정")]
        public int inventoryMaxSize = 100;
        public int dailyCustomerCount = 5;
        public int uiPageSize = 8;
        
        [Header("경제 설정")]
        public int weaponShopRefreshCost = 1000;
        public float weaponSellRate = 0.5f; // 판매가 = 구매가 * 0.5
    }
}