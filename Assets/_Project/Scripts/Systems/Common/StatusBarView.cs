using UnityEngine;
using TMPro;
using MagicRentalShop.Core;
using MagicRentalShop.Data;

namespace MagicRentalShop.UI.Common
{
    /// <summary>
    /// 상태 표시줄 UI (페이즈 패널과 인벤토리 사이 구분선 역할)
    /// 2단계: 실시간 데이터 업데이트 및 시간 표시 구현
    /// </summary>
    public class StatusBarView : MonoBehaviour
    {
        [Header("UI 참조")]
        public TMP_Text goldText;               // 골드 표시
        public TMP_Text dayText;                // 날짜 표시
        public TMP_Text phaseText;              // 페이즈 표시
        public TMP_Text timeText;               // 남은 시간 표시
        
        [Header("레이아웃 설정")]
        public float barHeight = 30f;           // 얇은 바 높이
        
        [Header("시간 표시 설정")]
        public bool showTimeInNightPhase = false; // 밤 페이즈에서 시간 표시 여부
        public Color normalTimeColor = Color.white; // 일반 시간 색상
        public Color urgentTimeColor = Color.red;   // 긴급 시간 색상 (30초 이하)
        
        [Header("디버그")]
        public bool enableDebugLogs = false;
        
        // 현재 상태 캐시
        private int currentGold = -1;
        private int currentDay = -1;
        private GamePhase currentPhase = GamePhase.Morning;
        private float currentTime = -1f;
        
        private void Start()
        {
            InitializeStatusBar();
        }
        
        #region 초기화
        
        /// <summary>
        /// StatusBar 초기화
        /// </summary>
        private void InitializeStatusBar()
        {
            // 초기 값 설정
            UpdateGold(0);
            UpdateDay(1);
            UpdatePhase(GamePhase.Morning);
            UpdateTime(0f);
            
            if (enableDebugLogs)
                Debug.Log("StatusBarView: 초기화 완료");
        }
        
        #endregion
        
        #region 데이터 업데이트
        
        /// <summary>
        /// 골드 업데이트
        /// </summary>
        /// <param name="gold">새로운 골드 양</param>
        public void UpdateGold(int gold)
        {
            if (currentGold == gold) return; // 중복 업데이트 방지
            
            currentGold = gold;
            
            if (goldText != null)
            {
                goldText.text = $"골드: {gold:N0}G";
                
                if (enableDebugLogs)
                    Debug.Log($"StatusBarView: 골드 업데이트 - {gold}G");
            }
        }
        
        /// <summary>
        /// 날짜 업데이트
        /// </summary>
        /// <param name="day">새로운 날짜</param>
        public void UpdateDay(int day)
        {
            if (currentDay == day) return; // 중복 업데이트 방지
            
            currentDay = day;
            
            if (dayText != null)
            {
                dayText.text = $"{day}일차";
                
                if (enableDebugLogs)
                    Debug.Log($"StatusBarView: 날짜 업데이트 - {day}일차");
            }
        }
        
        /// <summary>
        /// 페이즈 업데이트
        /// </summary>
        /// <param name="phase">새로운 페이즈</param>
        public void UpdatePhase(GamePhase phase)
        {
            if (currentPhase == phase) return; // 중복 업데이트 방지
            
            currentPhase = phase;
            
            if (phaseText != null)
            {
                string phaseDisplayText = GetPhaseDisplayText(phase);
                phaseText.text = phaseDisplayText;
                
                if (enableDebugLogs)
                    Debug.Log($"StatusBarView: 페이즈 업데이트 - {phaseDisplayText}");
            }
        }
        
        /// <summary>
        /// 시간 업데이트
        /// </summary>
        /// <param name="remainingTime">남은 시간 (초)</param>
        public void UpdateTime(float remainingTime)
        {
            // 시간이 크게 변하지 않았으면 스킵 (성능 최적화)
            if (Mathf.Abs(currentTime - remainingTime) < 0.1f) return;
            
            currentTime = remainingTime;
            
            if (timeText != null)
            {
                UpdateTimeDisplay(remainingTime);
            }
        }
        
        #endregion
        
        #region 시간 표시 처리
        
        /// <summary>
        /// 시간 표시 업데이트
        /// </summary>
        /// <param name="remainingTime">남은 시간</param>
        private void UpdateTimeDisplay(float remainingTime)
        {
            // 밤 페이즈에서 시간 표시 안함 (설정에 따라)
            if (currentPhase == GamePhase.Night && !showTimeInNightPhase)
            {
                timeText.text = "--:--";
                timeText.color = normalTimeColor;
                return;
            }
            
            // 시간이 0 이하면 시간 표시 안함
            if (remainingTime <= 0)
            {
                timeText.text = "00:00";
                timeText.color = normalTimeColor;
                return;
            }
            
            // 분:초 형태로 변환
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            
            string timeString = $"{minutes:D2}:{seconds:D2}";
            timeText.text = timeString;
            
            // 긴급 상황 색상 적용 (30초 이하)
            if (remainingTime <= 30f && currentPhase != GamePhase.Night)
            {
                timeText.color = urgentTimeColor;
            }
            else
            {
                timeText.color = normalTimeColor;
            }
            
            if (enableDebugLogs && remainingTime <= 10f)
                Debug.Log($"StatusBarView: 시간 경고 - {timeString}");
        }
        
        /// <summary>
        /// 페이즈 표시 텍스트 가져오기
        /// </summary>
        /// <param name="phase">페이즈</param>
        /// <returns>표시할 텍스트</returns>
        private string GetPhaseDisplayText(GamePhase phase)
        {
            return phase switch
            {
                GamePhase.Morning => "아침",
                GamePhase.Day => "낮",
                GamePhase.Night => "밤",
                _ => "알 수 없음"
            };
        }
        
        #endregion
        
        #region 애니메이션 효과 (TODO: 8단계)
        
        /// <summary>
        /// 골드 변경 애니메이션
        /// TODO: 8단계에서 구현
        /// </summary>
        /// <param name="oldGold">이전 골드</param>
        /// <param name="newGold">새로운 골드</param>
        private void AnimateGoldChange(int oldGold, int newGold)
        {
            // TODO: 골드 증감 시 부드러운 카운트업/다운 애니메이션
            // TODO: 골드 증가 시 녹색, 감소 시 빨간색 효과
        }
        
        /// <summary>
        /// 페이즈 전환 애니메이션
        /// TODO: 8단계에서 구현
        /// </summary>
        /// <param name="newPhase">새로운 페이즈</param>
        private void AnimatePhaseTransition(GamePhase newPhase)
        {
            // TODO: 페이즈 전환 시 부드러운 색상 변화 애니메이션
        }
        
        #endregion
        
        #region 유틸리티
        
        /// <summary>
        /// 모든 데이터 강제 새로고침
        /// </summary>
        public void ForceRefreshAll()
        {
            // 캐시 초기화하여 강제 업데이트
            currentGold = -1;
            currentDay = -1;
            currentTime = -1f;
            
            // GameController에서 현재 데이터 가져와서 업데이트
            if (GameController.Instance != null)
            {
                var playerData = GameController.Instance.PlayerData;
                if (playerData != null)
                {
                    UpdateGold(playerData.gold);
                    UpdateDay(playerData.currentDay);
                    UpdatePhase(playerData.currentPhase);
                    UpdateTime(playerData.phaseRemainingTime);
                }
            }
            
            if (enableDebugLogs)
                Debug.Log("StatusBarView: 전체 데이터 강제 새로고침 완료");
        }
        
        /// <summary>
        /// StatusBar 높이 설정
        /// </summary>
        /// <param name="height">새로운 높이</param>
        public void SetBarHeight(float height)
        {
            barHeight = height;
            
            // RectTransform 높이 조정
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
            }
        }
        
        #endregion
        
        #region 디버그 및 테스트
        
        /// <summary>
        /// 테스트용 골드 변경
        /// </summary>
        [ContextMenu("Debug: Test Gold Update")]
        public void DebugTestGoldUpdate()
        {
            int testGold = Random.Range(0, 10000);
            UpdateGold(testGold);
            Debug.Log($"StatusBarView: 골드 테스트 - {testGold}G");
        }
        
        /// <summary>
        /// 테스트용 시간 카운트다운
        /// </summary>
        [ContextMenu("Debug: Test Time Countdown")]
        public void DebugTestTimeCountdown()
        {
            StartCoroutine(TestTimeCountdownCoroutine());
        }
        
        /// <summary>
        /// 시간 카운트다운 코루틴 (테스트용)
        /// </summary>
        private System.Collections.IEnumerator TestTimeCountdownCoroutine()
        {
            float testTime = 60f; // 60초부터 시작
            
            while (testTime > 0)
            {
                UpdateTime(testTime);
                testTime -= 1f;
                yield return new WaitForSeconds(0.1f);
            }
            
            UpdateTime(0f);
            Debug.Log("StatusBarView: 시간 카운트다운 테스트 완료");
        }
        
        /// <summary>
        /// StatusBar 상태 출력
        /// </summary>
        [ContextMenu("Debug: Print StatusBar State")]
        public void DebugPrintState()
        {
            Debug.Log("=== StatusBar 상태 ===");
            Debug.Log($"골드: {currentGold}G");
            Debug.Log($"날짜: {currentDay}일차");
            Debug.Log($"페이즈: {currentPhase}");
            Debug.Log($"남은 시간: {currentTime:F1}초");
            Debug.Log($"UI 컴포넌트 연결 상태:");
            Debug.Log($"  - goldText: {goldText != null}");
            Debug.Log($"  - dayText: {dayText != null}");
            Debug.Log($"  - phaseText: {phaseText != null}");
            Debug.Log($"  - timeText: {timeText != null}");
        }
        
        #endregion
    }
}