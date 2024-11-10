using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Combat.Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Combat.Unit.OnAnyActionPointChange += Unit_OnOnAnyActionPointChange;
        healthSystem.OnDamage += HealthSystemOnDamage;
        
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void HealthSystemOnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Unit_OnOnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
