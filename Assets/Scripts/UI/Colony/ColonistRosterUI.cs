using System.Collections.Generic;
using UnityEngine;

public class ColonistRosterUI : MonoBehaviour
{
    [SerializeField] private Transform colonistRosterButtonPrefab;
    [SerializeField] private Transform colonistRosterContainerUI;
    
    private CharacterManager _characterManager;

    private void Start()
    {
        _characterManager = FindObjectOfType<CharacterManager>();
        _characterManager.OnCharacterListUpdated += OnCharacterListUpdated;
    }

    private void OnCharacterListUpdated()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform buttonTransform in colonistRosterContainerUI)
        {
            Destroy(buttonTransform.gameObject);
        }
        List<Character> characters = _characterManager.GetCharacterList();
        foreach (Character character in characters)
        {
            Transform characterButtonTransform = Instantiate(colonistRosterButtonPrefab, colonistRosterContainerUI);
            ColonistRosterButtonUI colonistRosterButtonUI = characterButtonTransform.GetComponent<ColonistRosterButtonUI>();
            colonistRosterButtonUI.SetCharacter(character);
        }
    }

    private void OnDestroy()
    {
        _characterManager.OnCharacterListUpdated -= OnCharacterListUpdated;
    }
}
