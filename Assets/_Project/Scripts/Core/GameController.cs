using UnityEngine;
using MagicRentalShop.Data;
using MagicRentalShop.Systems.Phases;

namespace MagicRentalShop.Core
{
    /// <summary>
    /// 게임의 전체 흐름과 시간, 날짜를 관리하는 핵심 컨트롤러
    /// 2단계: 페이즈 컨트롤러 연동 및 시간 관리 시스템 구현
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("게임 설정")]
        public GameConfig gameConfig;           // 게임 설정 ScriptableObject
        
        [Header("페이즈 컨트롤러 참조")]
        public MorningController morningController;
        public DayController dayController;
        public NightController nightController;
        
        [Header("시간 관리")]
        public bool enablePhaseTimer = true;    // 페이즈 타이머 활성화 여부
        public bool pauseTimer = false;         // 타이머 일시정지
        
        [Header("디버그")]
        public bool enableDebugLogs = true;
        public bool enableAutoAdvance = false;  // 자동 페이즈 전환 (테스트용)
        
        // 싱글톤 패턴
        public static GameController Instance { get; private set; }
        
        // 게임 데이터
        public PlayerData PlayerData { get; private set; }
        public GameConfig GameConfig => gameConfig;
        
        // 시간 관리 변수
        private float currentPhaseTime;
        private bool isPhaseTransitioning = false;
        
        // 이벤트
        public System.Action<GamePhase> OnPhaseChanged;
        public System.Action<int> OnDayAdvanced;
        public System.Action<float> OnPhaseTimeUpdated;
        
        #region Unity 라이프사이클
        
        private void Awake()
        {
            // 싱글톤 설정
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            StartGame();
        }
        
        private void Update()
        {
            if (enablePhaseTimer && !pauseTimer && !isPhaseTransitioning)
            {
                UpdatePhaseTimer();
            }
        }
        
        #endregion
        
        #region 게임 초기화
        
        /// <summary>
        /// 게임 초기화
        /// </summary>
        private void InitializeGame()
        {
            // PlayerData 초기화
            PlayerData = new PlayerData();
            
            if (gameConfig != null)
            {
                PlayerData.InitializeNewGame(gameConfig);
                if (enableDebugLogs)
                    Debug.Log($"GameController: PlayerData 초기화 완료 - 시작 골드: {PlayerData.gold}");
            }
            else
            {
                Debug.LogError("GameController: GameConfig가 설정되지 않았습니다!");
            }
        }
        
        /// <summary>
        /// 게임 시작
        /// </summary>
        public void StartGame()
        {
            if (enableDebugLogs)
                Debug.Log("GameController: 게임 시작");
            
            // 초기 페이즈 시작 (아침)
            StartMorning();
        }
        
        #endregion
        
        #region 페이즈 관리
        
        /// <summary>
        /// 아침 페이즈 시작
        /// </summary>
        public void StartMorning()
        {
            if (isPhaseTransitioning) return;
            
            ChangePhase(GamePhase.Morning);
            
            // 시간 설정
            currentPhaseTime = gameConfig.dayDuration;
            PlayerData.UpdatePhase(GamePhase.Morning, currentPhaseTime);
            
            // 아침 컨트롤러 활성화
            if (morningController != null)
                morningController.Activate();
            
            if (enableDebugLogs)
                Debug.Log($"GameController: 아침 페이즈 시작 - Day {PlayerData.currentDay}");
        }
        
        /// <summary>
        /// 낮 페이즈 시작
        /// </summary>
        public void StartDay()
        {
            if (isPhaseTransitioning) return;
            
            ChangePhase(GamePhase.Day);
            
            // 시간 설정
            currentPhaseTime = gameConfig.dayDuration;
            PlayerData.UpdatePhase(GamePhase.Day, currentPhaseTime);
            
            // 이전 페이즈 비활성화
            if (morningController != null)
                morningController.Deactivate();
            
            // 낮 컨트롤러 활성화
            if (dayController != null)
                dayController.Activate();
            
            if (enableDebugLogs)
                Debug.Log($"GameController: 낮 페이즈 시작 - Day {PlayerData.currentDay}");
        }
        
        /// <summary>
        /// 밤 페이즈 시작
        /// </summary>
        public void StartNight()
        {
            if (isPhaseTransitioning) return;
            
            ChangePhase(GamePhase.Night);
            
            // 밤은 시간 제한 없음
            currentPhaseTime = 0f;
            PlayerData.UpdatePhase(GamePhase.Night, 0f);
            
            // 이전 페이즈 비활성화
            if (dayController != null)
                dayController.Deactivate();
            
            // 밤 컨트롤러 활성화
            if (nightController != null)
                nightController.Activate();
            
            if (enableDebugLogs)
                Debug.Log($"GameController: 밤 페이즈 시작 - Day {PlayerData.currentDay}");
        }
        
        /// <summary>
        /// 다음 날로 진행
        /// </summary>
        public void AdvanceToNextDay()
        {
            if (isPhaseTransitioning) return;
            
            // 이전 페이즈 비활성화
            if (nightController != null)
                nightController.Deactivate();
            
            // 날짜 진행
            PlayerData.AdvanceDay();
            
            // 이벤트 발생
            OnDayAdvanced?.Invoke(PlayerData.currentDay);
            
            if (enableDebugLogs)
                Debug.Log($"GameController: 새로운 하루 시작 - Day {PlayerData.currentDay}");
            
            // TODO: 3단계에서 추가할 기능들
            // - 자동저장: PersistenceController.SaveGame()
            // - 모험 일수 감소: AdventureController.ProcessDayChange()
            // - Hero 회복 처리: ProcessHeroRecovery() (6-7단계)
            
            // 새로운 아침 시작
            StartMorning();
        }
        
        /// <summary>
        /// 페이즈 변경 처리
        /// </summary>
        /// <param name="newPhase">새로운 페이즈</param>
        private void ChangePhase(GamePhase newPhase)
        {
            if (PlayerData.currentPhase == newPhase) return;
            
            isPhaseTransitioning = true;
            
            GamePhase oldPhase = PlayerData.currentPhase;
            PlayerData.currentPhase = newPhase;
            
            // 이벤트 발생
            OnPhaseChanged?.Invoke(newPhase);
            
            if (enableDebugLogs)
                Debug.Log($"GameController: 페이즈 변경 - {oldPhase} → {newPhase}");
            
            isPhaseTransitioning = false;
        }
        
        #endregion
        
        #region 시간 관리
        
        /// <summary>
        /// 페이즈 타이머 업데이트
        /// </summary>
        private void UpdatePhaseTimer()
        {
            // 밤 페이즈는 시간 제한 없음
            if (PlayerData.currentPhase == GamePhase.Night)
                return;
            
            if (currentPhaseTime > 0)
            {
                currentPhaseTime -= Time.deltaTime;
                PlayerData.phaseRemainingTime = currentPhaseTime;
                
                // 시간 업데이트 이벤트 발생
                OnPhaseTimeUpdated?.Invoke(currentPhaseTime);
                
                // 시간 종료 시 자동 전환
                if (currentPhaseTime <= 0)
                {
                    OnPhaseTimeExpired();
                }
            }
        }
        
        /// <summary>
        /// 페이즈 시간 만료 처리
        /// </summary>
        private void OnPhaseTimeExpired()
        {
            if (enableDebugLogs)
                Debug.Log($"GameController: 페이즈 시간 만료 - {PlayerData.currentPhase}");
            
            // 자동 페이즈 전환
            switch (PlayerData.currentPhase)
            {
                case GamePhase.Morning:
                    StartDay();
                    break;
                case GamePhase.Day:
                    StartNight();
                    break;
                case GamePhase.Night:
                    // 밤은 수동으로만 전환
                    break;
            }
        }
        
        /// <summary>
        /// 수동 페이즈 전환 (버튼 클릭용)
        /// </summary>
        public void ManualAdvancePhase()
        {
            if (isPhaseTransitioning) return;
            
            switch (PlayerData.currentPhase)
            {
                case GamePhase.Morning:
                    StartDay();
                    break;
                case GamePhase.Day:
                    StartNight();
                    break;
                case GamePhase.Night:
                    AdvanceToNextDay();
                    break;
            }
        }
        
        /// <summary>
        /// 페이즈 타이머 일시정지/재개
        /// </summary>
        /// <param name="pause">true면 일시정지, false면 재개</param>
        public void PausePhaseTimer(bool pause)
        {
            pauseTimer = pause;
            if (enableDebugLogs)
                Debug.Log($"GameController: 페이즈 타이머 {(pause ? "일시정지" : "재개")}");
        }
        
        /// <summary>
        /// 남은 시간 비율 (0~1)
        /// </summary>
        public float GetPhaseTimeRatio()
        {
            if (PlayerData.currentPhase == GamePhase.Night)
                return 0f;
            
            return currentPhaseTime / gameConfig.dayDuration;
        }
        
        #endregion
        
        #region 상태 조회
        
        /// <summary>
        /// 현재 게임 상태 정보
        /// </summary>
        public string GetGameStateInfo()
        {
            return $"Day {PlayerData.currentDay} - {PlayerData.currentPhase} - {PlayerData.gold}G - {currentPhaseTime:F1}s";
        }
        
        /// <summary>
        /// 페이즈 전환 가능 여부
        /// </summary>
        public bool CanAdvancePhase()
        {
            return !isPhaseTransitioning;
        }
        
        #endregion
        
        #region 디버그 및 테스트
        
        /// <summary>
        /// 강제 페이즈 전환 (디버그용)
        /// </summary>
        [ContextMenu("Debug: Manual Advance Phase")]
        public void DebugManualAdvancePhase()
        {
            ManualAdvancePhase();
        }
        
        /// <summary>
        /// 시간 빠르게 진행 (디버그용)
        /// </summary>
        [ContextMenu("Debug: Fast Forward Time")]
        public void DebugFastForwardTime()
        {
            if (PlayerData.currentPhase != GamePhase.Night)
            {
                currentPhaseTime = Mathf.Max(0f, currentPhaseTime - 60f);
                Debug.Log($"GameController: 시간 빠르게 진행 - 남은 시간: {currentPhaseTime:F1}초");
            }
        }
        
        /// <summary>
        /// 골드 추가 (디버그용)
        /// </summary>
        [ContextMenu("Debug: Add 1000 Gold")]
        public void DebugAddGold()
        {
            PlayerData.ChangeGold(1000);
            Debug.Log($"GameController: 골드 1000 추가 - 현재: {PlayerData.gold}골드");
        }
        
        /// <summary>
        /// 게임 상태 출력 (디버그용)
        /// </summary>
        [ContextMenu("Debug: Print Game State")]
        public void DebugPrintGameState()
        {
            Debug.Log("=== 게임 상태 ===");
            Debug.Log(GetGameStateInfo());
            Debug.Log($"페이즈 타이머 활성화: {enablePhaseTimer}");
            Debug.Log($"페이즈 전환 중: {isPhaseTransitioning}");
            Debug.Log($"타이머 일시정지: {pauseTimer}");
            Debug.Log($"PlayerData: {PlayerData}");
        }
        
        #endregion
    }
}