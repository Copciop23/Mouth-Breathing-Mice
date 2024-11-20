using UnityEngine;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Dash dashScript;
    [SerializeField] private SpringBoots springboots;
    [SerializeField] private TextMeshProUGUI dashCooldownText;

    private void Start()
    {
        if (dashScript == null)
            dashScript = FindObjectOfType<Dash>();
        if (springboots == null)
        {
            springboots = FindObjectOfType<SpringBoots>();
        }
        UpdateCooldownUI();
    }
        private void Update()
        {
            if (dashScript == null|| springboots == null) return;
            UpdateCooldownUI();
        }

        private void UpdateCooldownUI()
        {
            if (!dashScript.canDash)
            {
                dashCooldownText.text = "<color=#7FFFD4>Dash: <color=#FF0000>" + dashScript.DashTimer.ToString("F1") + "s</color>\n" + "Charge: " + springboots.CurrentJumpPower.ToString("F1");
            }
            else
            {
                dashCooldownText.text = "<color=#7FFFD4>Dash: <color=#00FF00>READY</color>\n"+"Charge: "+ springboots.CurrentJumpPower.ToString("F1");
            }
        }
    } 

