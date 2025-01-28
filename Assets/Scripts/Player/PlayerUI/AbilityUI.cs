using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Dash dashScript;
    [SerializeField] private Slider dashSlider;

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
            if (!dashScript.canDash)
            {
            dashSlider.value = dashScript.DashTimer;
            }
            else
            {
            }
        }
    } 

