using System.Collections;
using TMPro;
using UnityEngine;

public class FightStartingTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text countdownText; // Reference to the text component for displaying the countdown

    [Header("Settings")]
    [SerializeField] private float popScale = 2f; // Scale factor for the "pop" effect
    [SerializeField] private float numberDuration = 0.375f; // Duration of the pop animation for each number

    /// <summary>
    /// Runs the countdown timer and displays numbers with a pop effect.
    /// </summary>
    /// <param name="duration">Total duration of the countdown in seconds.</param>
    public IEnumerator RunCountdown(float duration)
    {
        // Enable the countdown text at the start
        countdownText.gameObject.SetActive(true);

        float timer = duration; // Initialize the timer

        // Loop until the timer reaches zero
        while (timer > 0)
        {
            int currentNumber = Mathf.CeilToInt(timer); // Convert the timer value to an integer
            countdownText.text = currentNumber.ToString(); // Update the displayed text

            yield return StartCoroutine(PopEffect()); // Trigger the pop animation
            timer -= 1f; // Decrement the timer by 1 second

            // Wait for 1 second in unscaled time (works even when the game is paused)
            float elapsed = 0f;
            while (elapsed < 1f)
            {
                elapsed += Time.unscaledDeltaTime; // Accumulate time in unscaled time
                yield return null; // Wait for the next frame
            }
        }

        // Display "GO!" at the end of the countdown
        countdownText.text = "GO!";
        yield return StartCoroutine(PopEffect()); // Play the pop animation for "GO!"
        countdownText.gameObject.SetActive(false); // Hide the countdown text
    }

    /// <summary>
    /// Pauses the game for 1 second in real time, then resumes.
    /// </summary>
    private IEnumerator WaitAndResume()
    {
        Time.timeScale = 0; // Pause the game
        yield return new WaitForSecondsRealtime(1f); // Wait for 1 second in real time
        Time.timeScale = 1; // Resume the game
    }

    /// <summary>
    /// Plays a "pop" effect where the text scales up and back down smoothly.
    /// </summary>
    private IEnumerator PopEffect()
    {
        float elapsed = 0; // Track elapsed time
        Vector3 startScale = Vector3.one * 0.5f; // Initial small scale
        Vector3 peakScale = Vector3.one * popScale; // Maximum pop scale

        // Animate the scale over the duration
        while (elapsed < numberDuration)
        {
            float t = elapsed / numberDuration; // Calculate the normalized time (0 to 1)

            // Determine the scale based on the animation phase (scaling up or down)
            countdownText.transform.localScale = Vector3.Lerp(
                t < 0.5f ? startScale : peakScale, // Start at small or peak scale
                t < 0.5f ? peakScale : Vector3.one, // End at peak or normal scale
                t < 0.5f ? t * 2 : (t - 0.5f) * 2 // Adjust interpolation for each phase
            );

            elapsed += Time.unscaledDeltaTime; // Accumulate elapsed time in unscaled time
            yield return null; // Wait for the next frame
        }
    }
}
