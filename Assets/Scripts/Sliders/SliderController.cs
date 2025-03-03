using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    // Public slider references
    public Slider skinSlider;
    public Slider jumpingPowerSlider;
    public Slider speedSlider;

    // Array of sprites for the skin selection (set this in Inspector)
    public Sprite[] skins;

    // Reference to character visuals (UI Image or SpriteRenderer)
    public Image characterImage;
    public SpriteRenderer characterSpriteRenderer;

    // Reference to player controller (for jumping power updates)
    public PlayerStats player;

    // Reference to player movement (for speed updates)
    public Movement movement;

    // Reference to the text labels for values
    public TextMeshProUGUI jumpingPowerValueLabel;
    public TextMeshProUGUI skinValueLabel;
    public TextMeshProUGUI speedValueLabel;

    private void Start()
    {
        // Initialize skin slider (assuming index-based skin selection)
        skinSlider.minValue = 0;
        skinSlider.maxValue = skins.Length - 1;
        skinSlider.wholeNumbers = true;
        skinSlider.onValueChanged.AddListener(ChangeSkin);

        // Initialize jumping power slider (example values)
        jumpingPowerSlider.minValue = 1f;
        jumpingPowerSlider.maxValue = 10f;
        jumpingPowerSlider.onValueChanged.AddListener(UpdateJumpPower);

        // Initialize speed slider (example values)
        speedSlider.minValue = 1f;
        speedSlider.maxValue = 10f;
        speedSlider.onValueChanged.AddListener(UpdateSpeed);
        
        // Setting initial values for labels
        UpdateJumpPower(jumpingPowerSlider.value);
        ChangeSkin(skinSlider.value);
        UpdateSpeed(jumpingPowerSlider.value);
    }

    // Called when the skin slider value changes
    private void ChangeSkin(float value)
    {
        int index = Mathf.RoundToInt(value);
        if (skins != null && skins.Length > index)
        {
            if (characterImage != null)
            {
                characterImage.sprite = skins[index];
            }
            if (characterSpriteRenderer != null)
            {
                characterSpriteRenderer.sprite = skins[index];
            }

            if (skinValueLabel != null) {
                skinValueLabel.text = (index + 1).ToString();
            }
        }
    }

    // Called when the jumping power slider value changes
    private void UpdateJumpPower(float value)
    {
        // Update the player's jumping power via a method in your player controller
        if (player != null)
        {
            movement.SetJumpPower(value); // Make sure PlayerStats has a SetJumpPower() method
        }

        if (jumpingPowerValueLabel != null) {
            jumpingPowerValueLabel.text = value.ToString("0");
        }
    }

    // Called when the speed slider value changes
    private void UpdateSpeed(float value)
    {
        // Update the player's speed via a method in your player movement controller
        if (movement != null)
        {
            movement.SetSpeed(value);
        }

        if (speedValueLabel != null) {
            speedValueLabel.text = value.ToString("0");
        }
    }
}
