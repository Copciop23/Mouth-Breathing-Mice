using System.Collections;
using TMPro;
using UnityEngine;

public class FightStartingTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text countdownText;

    [Header("Settings")]
    [SerializeField] private float popScale = 2f;
    [SerializeField] private float numberDuration = 0.375f;

    public IEnumerator RunCountdown(float duration)
    {
        countdownText.gameObject.SetActive(true);
        float timer = duration;

        while (timer > 0)
        {
            int currentNumber = Mathf.CeilToInt(timer);
            countdownText.text = currentNumber.ToString();

            yield return StartCoroutine(PopEffect());
            timer -= 1f;

            // Wait using unscaled time (works even when game is paused)
            float elapsed = 0f;
            while (elapsed < 1f)
            {
                elapsed += Time.unscaledDeltaTime; // Critical fix!
                yield return null;
            }
        }

        countdownText.text = "GO!";
        yield return StartCoroutine(PopEffect());
        countdownText.gameObject.SetActive(false);
    }

    IEnumerator WaitAndResume()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
    }

    private IEnumerator PopEffect()
    {
        float elapsed = 0;
        Vector3 startScale = Vector3.one * 0.5f;
        Vector3 peakScale = Vector3.one * popScale;

        while (elapsed < numberDuration)
        {
            float t = elapsed / numberDuration;
            countdownText.transform.localScale = Vector3.Lerp(
                t < 0.5f ? startScale : peakScale,
                t < 0.5f ? peakScale : Vector3.one,
                t < 0.5f ? t * 2 : (t - 0.5f) * 2
            );

            elapsed += Time.unscaledDeltaTime; // Critical fix!
            yield return null;
        }
    }
}