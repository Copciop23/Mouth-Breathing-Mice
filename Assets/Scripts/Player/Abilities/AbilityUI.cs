using UnityEngine;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Dash dashScript;
    [SerializeField] private TextMeshProUGUI dashCooldownText;

    private void Start()
    {
        if (dashScript == null)
            dashScript = FindObjectOfType<Dash>();

        UpdateCooldownUI();
    }

    private void Update()
    {
        if (dashScript == null) return;

        UpdateCooldownUI();
    }

    private void UpdateCooldownUI()
    {
        if (!dashScript.canDash)
        {
            dashCooldownText.text = "<color=#7FFFD4>Dash: <color=#FF0000>" + dashScript.DashTimer.ToString("F1") + "s</color>";
        }
        else
        {
            dashCooldownText.text = "<color=#7FFFD4>Dash: <color=#00FF00>READY</color>";
        }
    }
}
