using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Range(0, 1)] public float globalTimeScale = 1f;
    public float timerTimeScale = 1f;

    void Awake() => Instance = this;

    void Update()
    {
        Time.timeScale = globalTimeScale;
    }

    public float GetDeltaTime(bool isTimer = false)
    {
        return (isTimer ? timerTimeScale : globalTimeScale) * Time.unscaledDeltaTime;
    }
}