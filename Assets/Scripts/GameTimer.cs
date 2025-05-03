using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public enum TimerType { PVP, Boss }
    public TimerType timerType;
    public float totalTime = 99f;
    public TMP_Text countdownText;
    private float currentTime;
    private bool isPaused = false;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!isPaused && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateCountdownText();

            if (currentTime <= 0)
            {
                Debug.Log("TIME'S UP!");
                enabled = false;
            }
        }
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }

    public void ResetTimer()
    {
        currentTime = totalTime;
        UpdateCountdownText();
    }

    void UpdateCountdownText()
    {
        countdownText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}