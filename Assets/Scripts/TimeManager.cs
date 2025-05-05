using UnityEngine;

/// <summary>
/// Singleton class to manage time scales globally and for specific timers in the game.
/// </summary>
public class TimeManager : MonoBehaviour
{
    // Singleton instance of TimeManager
    public static TimeManager Instance;

    // Controls the global time scale of the game (range limited to 0 to 1)
    [Range(0, 1)] public float globalTimeScale = 1f;

    // Time scale specific to timers, allowing independent adjustments
    public float timerTimeScale = 1f;

    /// <summary>
    /// Sets up the singleton instance when the script is first initialized.
    /// </summary>
    void Awake() => Instance = this;

    /// <summary>
    /// Updates the global time scale every frame.
    /// </summary>
    void Update()
    {
        Time.timeScale = globalTimeScale;
    }

    /// <summary>
    /// Returns the delta time adjusted for the selected time scale (global or timer).
    /// </summary>
    /// <param name="isTimer">If true, uses the timer-specific time scale; otherwise, uses the global time scale.</param>
    /// <returns>Adjusted delta time based on the chosen time scale.</returns>
    public float GetDeltaTime(bool isTimer = false)
    {
        return (isTimer ? timerTimeScale : globalTimeScale) * Time.unscaledDeltaTime;
    }
}
