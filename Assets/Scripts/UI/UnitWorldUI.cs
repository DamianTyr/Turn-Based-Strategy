using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mission;
using UnityEngine.Serialization;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [FormerlySerializedAs("transform")] [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointChange += Unit_OnOnAnyActionPointChange;
        GameStateManager.Instance.OnGameStateChanged += GameStateManager_OnGameStateChanged;
        healthSystem.OnDamage += HealthSystemOnDamage;
        
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void GameStateManager_OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.RealTime) actionPointsText.enabled = false;
        else actionPointsText.enabled = true;
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
