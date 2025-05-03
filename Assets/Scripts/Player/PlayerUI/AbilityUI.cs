using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Dash dashScript;
    [SerializeField] private SpringBoots springBootsScript;
    [SerializeField] private Fireball fireballScript;
    [SerializeField] private HawkTuah hawkTuahScript;
    [SerializeField] private ShieldController shieldScript;
    [SerializeField] private Slider dashSlider;
    [SerializeField] private Slider bootsSlider;
    [SerializeField] private Slider fireballSlider;
    [SerializeField] private Slider hawktuahSlider;
    [SerializeField] private Slider shieldSlider;
    private float hawkTuahCooldown = 0;
    private float shieldCooldown = 0;

    private void Start()
    {
        UpdateCooldownUI();
    }
    private void Update()
    {
        UpdateCooldownUI();
    }

    private void UpdateCooldownUI()
    {
        dashSlider.value = dashScript.DashTimer;
        bootsSlider.value = springBootsScript.CurrentJumpPower;
        fireballSlider.value = fireballScript.RemainingCooldown;

        hawktuahSlider.value = hawkTuahScript.RemainingCooldown;
        shieldSlider.value = shieldScript.RemainingCooldown;

    }
}

