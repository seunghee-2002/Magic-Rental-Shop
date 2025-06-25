using System;

namespace MagicRentalShop
{
    [Serializable]
    public enum GamePhase
    {
        Morning,
        Day,
        Night
    }

    [Serializable]
    public enum Grade
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4
    }

    [Serializable]
    public enum Element
    {
        Fire,
        Water,
        Earth,
        Air,
        Thunder,
        Ice,
        Light,
        Dark
    }

    [Serializable]
    public enum InventoryMode
    {
        Normal,      // 기본 모드 (정보 확인만)
        Selection,   // 선택 모드 (무기 장착용)
        Management   // 관리 모드 (무기 판매용)
    }
}