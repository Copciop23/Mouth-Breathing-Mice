using UnityEngine;
using UnityEngine.UI;

public class AbilityUI2 : MonoBehaviour
{
    [SerializeField] private DashP2 dashScript;
    [SerializeField] private SpringBootsP2 springBootsScript;
    [SerializeField] private FireballP2 fireballScript;
    [SerializeField] private HawkTuahP2 hawkTuahScript;
    [SerializeField] private ShieldControllerP2 shieldScript;
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
