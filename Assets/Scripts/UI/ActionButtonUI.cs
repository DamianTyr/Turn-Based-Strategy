using TMPro;
using Combat;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction _baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this._baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        button.onClick.AddListener(() =>
        {
             UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == _baseAction);
    }
}
