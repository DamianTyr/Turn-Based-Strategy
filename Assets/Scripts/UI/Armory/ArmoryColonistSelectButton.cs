using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryColonistSelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Button _button;
    private CharacterData _characterData;
    private ArmorySlotSelector _armorySlotSelector;

    private void Start()
    {
        _armorySlotSelector = FindObjectOfType<ArmorySlotSelector>();
        _button.onClick.AddListener(() =>
        {
            _armorySlotSelector.SpawnArmoryColonistInSelectedSlot(_characterData);
        });
    }

    public void SetCharacterData(CharacterData characterData)
    {
        _characterData = characterData;
        _nameText.text = characterData.GetName();
    }
}
