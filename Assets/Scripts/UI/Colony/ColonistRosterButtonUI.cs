using TMPro;
using UnityEngine;

public class ColonistRosterButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    
    public void SetCharacter(Character character)
    {
        buttonText.text = character.GetCharacterName();
    }
}
