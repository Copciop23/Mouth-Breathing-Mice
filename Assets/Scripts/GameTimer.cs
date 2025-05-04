using TMPro;
using UnityEngine;

/// <summary>
/// A game timer that can be used for various modes like PVP or Boss.
/// Tracks time, updates a countdown display, and supports pause/resume functionality.
/// </summary>
public class GameTimer : MonoBehaviour
{
    public enum TimerType { PVP, Boss } // Enum to differentiate between game modes
    public TimerType timerType; // Current timer mode (e.g., PVP or Boss)
    public float totalTime = 99f; // Total time for the timer in seconds
    public TMP_Text countdownText; // Reference to the UI text element for displaying the timer
    private float currentTime; // Current remaining time
    private bool isPaused = false; // Indicates whether the timer is paused

    /// <summary>
    /// Initializes the timer at the start of the game.
    /// </summary>
    void Start()
    {
        ResetTimer(); // Set the timer to the initial total time
    }

    /// <summary>
    /// Updates the timer each frame if it is not paused.
    /// </summary>
    void Update()
    {
        // Decrease time only if the timer is running and time is remaining
        if (!isPaused && currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Decrease current time by the frame's delta time
            UpdateCountdownText(); // Update the timer's UI text

            // Check if the timer has reached zero
            if (currentTime <= 0)
            {
                Debug.Log("TIME'S UP!"); // Log a message when the time is up
                enabled = false; // Disable this script when the timer ends
            }
        }
    }

    /// <summary>
    /// Pauses the timer, stopping it from decrementing.
    /// </summary>
    public void PauseTimer()
    {
        isPaused = true; // Set the paused state to true
    }

    /// <summary>
    /// Resumes the timer, allowing it to decrement again.
    /// </summary>
    public void ResumeTimer()
    {
        isPaused = false; // Set the paused state to false
    }

    /// <summary>
    /// Resets the timer to its initial total time and updates the display.
    /// </summary>
    public void ResetTimer()
    {
        currentTime = totalTime; // Reset the current time to the total time
        UpdateCountdownText(); // Update the UI text to reflect the reset time
    }

    /// <summary>
    /// Updates the text element to display the current remaining time.
    /// </summary>
    void UpdateCountdownText()
    {
        countdownText.text = Mathf.CeilToInt(currentTime).ToString(); // Display the time as an integer
    }
}
