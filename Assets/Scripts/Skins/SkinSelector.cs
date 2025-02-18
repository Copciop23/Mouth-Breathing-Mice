using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{

    public SpriteRenderer characterSpriteRenderer; // If changing an in-game character sprite
    public Sprite[] skins; // Drag & drop skins in Inspector
    public Slider skinSlider; // Reference to the UI Slider

    private void Start()
    {
        // Ensure slider values match the number of skins
        skinSlider.minValue = 0;
        skinSlider.maxValue = skins.Length - 1;
        skinSlider.wholeNumbers = true;

        // Set the initial skin
        ChangeSkin((int)skinSlider.value);

        // Add listener to detect slider changes
        skinSlider.onValueChanged.AddListener(delegate { ChangeSkin((int)skinSlider.value); });
    }

    private void ChangeSkin(int skinIndex)
    {
        if (skins == null || skins.Length == 0) return;

        // Update Sprite Renderer

        if (characterSpriteRenderer != null)
        {
            characterSpriteRenderer.sprite = skins[skinIndex];
        }
    }
}
