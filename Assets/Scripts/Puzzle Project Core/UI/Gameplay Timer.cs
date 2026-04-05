using MatrixUtils.Attributes;
using MatrixUtils.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameplayTimer : MonoBehaviour
{
    [SerializeField, RequiredField] TMP_Text m_timerText;
    [SerializeField] UnityEvent<float> m_onTimerFinished = new();
    StopwatchTimer m_timer;
    void Awake() => m_timer = new StopwatchTimer();
    public void StartTimer()
    {
        m_timer
            .OnTimeUpdated(time => m_timerText.text = "Time: " + time.ToString("00.00"))
            .OnPause(() => m_onTimerFinished.Invoke(m_timer.CurrentTime))
            .Start();
    }
    public void StopTimer() => m_timer?.Pause();
    public void OnDestroy() => m_timer?.Dispose();
}
