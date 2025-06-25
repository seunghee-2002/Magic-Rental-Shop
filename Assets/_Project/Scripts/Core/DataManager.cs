using System.Collections.Generic;
using UnityEngine;
using MagicRentalShop.Data;

namespace MagicRentalShop.Core
{
    /// <summary>
    /// 모든 정적 데이터를 로드하여 Dictionary 형태로 보관하는 '데이터 사전'
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        [Header("데이터 경로")]
        public string weaponDataPath = "Data/Weapons";
        public string customerDataPath = "Data/Customers";
        public string dungeonDataPath = "Data/Dungeons";
        public string materialDataPath = "Data/Materials";
        public string recipeDataPath = "Data/Recipes";
        public string eventDataPath = "Data/Events";
        
        [Header("디버그")]
        public bool enableDebugLogs = true;
        
        // 싱글톤 패턴
        public static DataManager Instance { get; private set; }
        
        // 데이터 저장소
        private Dictionary<string, WeaponData> weaponDatas = new Dictionary<string, WeaponData>();
        private Dictionary<string, CustomerData> customerDatas = new Dictionary<string, CustomerData>();
        private Dictionary<string, DungeonData> dungeonDatas = new Dictionary<string, DungeonData>();
        private Dictionary<string, MaterialData> materialDatas = new Dictionary<string, MaterialData>();
        private Dictionary<string, RecipeData> recipeDatas = new Dictionary<string, RecipeData>();
        private Dictionary<string, DailyEventData> eventDatas = new Dictionary<string, DailyEventData>();
        
        private void Awake()
        {
            // 싱글톤 설정
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// 모든 정적 데이터 로드 및 초기화
        /// </summary>
        public void Init()
        {
            LoadAllData();
            
            if (enableDebugLogs)
                Debug.Log("DataManager: 모든 데이터 로드 완료");
        }
        
        #region 데이터 로드
        
        /// <summary>
        /// 모든 데이터 로드
        /// </summary>
        private void LoadAllData()
        {
            LoadWeaponData();
            LoadCustomerData();
            LoadDungeonData();
            LoadMaterialData();
            LoadRecipeData();
            LoadEventData();
        }
        
        /// <summary>
        /// 무기 데이터 로드
        /// </summary>
        private void LoadWeaponData()
        {
            WeaponData[] weapons = Resources.LoadAll<WeaponData>(weaponDataPath);
            weaponDatas.Clear();
            
            foreach (var weapon in weapons)
            {
                if (weapon != null && !string.IsNullOrEmpty(weapon.id))
                {
                    weaponDatas[weapon.id] = weapon;
                }
            }
            
            if (enableDebugLogs)
                Debug.Log($"DataManager: 무기 데이터 {weaponDatas.Count}개 로드됨");
        }
        
        /// <summary>
        /// 고객 데이터 로드
        /// </summary>
        private void LoadCustomerData()
        {
            CustomerData[] customers = Resources.LoadAll<CustomerData>(customerDataPath);
            customerDatas.Clear();
            
            foreach (var customer in customers)
            {
                if (customer != null && !string.IsNullOrEmpty(customer.id))
                {
                    customerDatas[customer.id] = customer;
                }
            }
            
            if (enableDebugLogs)
                Debug.Log($"DataManager: 고객 데이터 {customerDatas.Count}개 로드됨");
        }
        
        /// <summary>
        /// 던전 데이터 로드
        /// </summary>
        private void LoadDungeonData()
        {
            DungeonData[] dungeons = Resources.LoadAll<DungeonData>(dungeonDataPath);
            dungeonDatas.Clear();
            
            foreach (var dungeon in dungeons)
            {
                if (dungeon != null && !string.IsNullOrEmpty(dungeon.id))
                {
                    dungeonDatas[dungeon.id] = dungeon;
                }
            }
            
            if (enableDebugLogs)
                Debug.Log($"DataManager: 던전 데이터 {dungeonDatas.Count}개 로드됨");
        }
        
        /// <summary>
        /// 재료 데이터 로드
        /// </summary>
        private void LoadMaterialData()
        {
            MaterialData[] materials = Resources.LoadAll<MaterialData>(materialDataPath);
            materialDatas.Clear();
            
            foreach (var material in materials)
            {
                if (material != null && !string.IsNullOrEmpty(material.id))
                {
                    materialDatas[material.id] = material;
                }
            }
            
            if (enableDebugLogs)
                Debug.Log($"DataManager: 재료 데이터 {materialDatas.Count}개 로드됨");
        }
        
        /// <summary>
        /// 레시피 데이터 로드
        /// </summary>
        private void LoadRecipeData()
        {
            RecipeData[] recipes = Resources.LoadAll<RecipeData>(recipeDataPath);
            recipeDatas.Clear();
            
            foreach (var recipe in recipes)
            {
                if (recipe != null && !string.IsNullOrEmpty(recipe.id))
                {
                    recipeDatas[recipe.id] = recipe;
                }
            }
            
            if (enableDebugLogs)
                Debug.Log($"DataManager: 레시피 데이터 {recipeDatas.Count}개 로드됨");
        }
        
        /// <summary>
        /// 이벤트 데이터 로드
        /// </summary>
        private void LoadEventData()
        {
            DailyEventData[] events = Resources.LoadAll<DailyEventData>(eventDataPath);
            eventDatas.Clear();
            
            foreach (var eventData in events)
            {
                if (eventData != null && !string.IsNullOrEmpty(eventData.id))
                {
                    eventDatas[eventData.id] = eventData;
                }
            }
            
            if (enableDebugLogs)
                Debug.Log($"DataManager: 이벤트 데이터 {eventDatas.Count}개 로드됨");
        }
        
        #endregion
        
        #region 데이터 조회
        
        /// <summary>
        /// 무기 데이터 가져오기
        /// </summary>
        public WeaponData GetWeaponData(string id)
        {
            weaponDatas.TryGetValue(id, out WeaponData data);
            return data;
        }
        
        /// <summary>
        /// 고객 데이터 가져오기
        /// </summary>
        public CustomerData GetCustomerData(string id)
        {
            customerDatas.TryGetValue(id, out CustomerData data);
            return data;
        }
        
        /// <summary>
        /// 던전 데이터 가져오기
        /// </summary>
        public DungeonData GetDungeonData(string id)
        {
            dungeonDatas.TryGetValue(id, out DungeonData data);
            return data;
        }
        
        /// <summary>
        /// 재료 데이터 가져오기
        /// </summary>
        public MaterialData GetMaterialData(string id)
        {
            materialDatas.TryGetValue(id, out MaterialData data);
            return data;
        }
        
        /// <summary>
        /// 레시피 데이터 가져오기
        /// </summary>
        public RecipeData GetRecipeData(string id)
        {
            recipeDatas.TryGetValue(id, out RecipeData data);
            return data;
        }
        
        /// <summary>
        /// 이벤트 데이터 가져오기
        /// </summary>
        public DailyEventData GetEventData(string id)
        {
            eventDatas.TryGetValue(id, out DailyEventData data);
            return data;
        }
        
        /// <summary>
        /// 모든 고객 데이터 가져오기 (Hero 도감용)
        /// </summary>
        public List<CustomerData> GetAllCustomerData()
        {
            return new List<CustomerData>(customerDatas.Values);
        }
        
        /// <summary>
        /// 모든 무기 데이터 가져오기
        /// </summary>
        public List<WeaponData> GetAllWeaponData()
        {
            return new List<WeaponData>(weaponDatas.Values);
        }
        
        /// <summary>
        /// 등급별 무기 데이터 가져오기
        /// </summary>
        public List<WeaponData> GetWeaponDataByGrade(Grade grade)
        {
            List<WeaponData> result = new List<WeaponData>();
            foreach (var weapon in weaponDatas.Values)
            {
                if (weapon.grade == grade)
                    result.Add(weapon);
            }
            return result;
        }
        
        /// <summary>
        /// 등급별 고객 데이터 가져오기
        /// </summary>
        public List<CustomerData> GetCustomerDataByGrade(Grade grade)
        {
            List<CustomerData> result = new List<CustomerData>();
            foreach (var customer in customerDatas.Values)
            {
                if (customer.grade == grade)
                    result.Add(customer);
            }
            return result;
        }
        
        /// <summary>
        /// 재료 획득 던전 정보 가져오기
        /// </summary>
        public List<string> GetMaterialDungeonInfo(string materialID)
        {
            MaterialData material = GetMaterialData(materialID);
            return material?.availableDungeonIDs ?? new List<string>();
        }
        
        #endregion
        
        #region 디버그
        
        /// <summary>
        /// 로드된 데이터 상태 출력
        /// </summary>
        [ContextMenu("Debug: Print Data Status")]
        public void DebugPrintDataStatus()
        {
            Debug.Log("=== DataManager 상태 ===");
            Debug.Log($"무기: {weaponDatas.Count}개");
            Debug.Log($"고객: {customerDatas.Count}개");
            Debug.Log($"던전: {dungeonDatas.Count}개");
            Debug.Log($"재료: {materialDatas.Count}개");
            Debug.Log($"레시피: {recipeDatas.Count}개");
            Debug.Log($"이벤트: {eventDatas.Count}개");
        }
        
        /// <summary>
        /// 강제로 데이터 재로드
        /// </summary>
        [ContextMenu("Debug: Reload All Data")]
        public void DebugReloadAllData()
        {
            LoadAllData();
            Debug.Log("DataManager: 모든 데이터 재로드 완료");
        }
        
        #endregion
    }
}