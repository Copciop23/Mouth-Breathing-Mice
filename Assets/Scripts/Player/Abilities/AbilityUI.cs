using System.Collections;
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

        dashCooldownText.text = "Dash: Ready";
    }

    private void Update()
    {
        if (!dashScript.canDash)
        {
            dashCooldownText.text = "Dash: " + "<color=#FF0000>" +dashScript.DashTimer.ToString("F1") + "s";
        }
        else
        {
            dashCooldownText.text = "Dash:" + "<color=#00FF00> READY" ;
        }
    }
}
