using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 99f;
    public TMP_Text countdownText;
    private float currentTime;

    void Start()
    {
        currentTime = totalTime;
        UpdateCountdownText();
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateCountdownText();
        }
        else
        {
            Debug.Log("TIME'S UP!");
            enabled = false;
        }
    }

    void UpdateCountdownText() {
        countdownText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
