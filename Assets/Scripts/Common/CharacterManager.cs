using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Saving;
using UnityEngine;

public class CharacterManager : MonoBehaviour, ISaveable
{
    private List<Character> characterList = new();
    private Character _selectedCharacter;
    private List<CharacterData> _characterDataList = new();
    
    [SerializeField] private bool isNewGame;
    
    public Action OnCharacterListUpdated;
    public Action<List<CharacterData>> OnCharacterDataListRestored;
    public Action<Character> OnSelectedCharacterSet;
    
    private void Start()
    {
        FindExistingCharacters();
        if (isNewGame)
        {
            HandleNewGame();
            return;
        }
        //Otherwise remove characters placed in the scene
        DestroyAllCharacters();
    }
    
    private void FindExistingCharacters()
    {
        Character[] characterArray = FindObjectsOfType<Character>();
        characterList = characterArray.ToList();
    }
    
    private void DestroyAllCharacters()
    {
        foreach (Character character in characterList)
        {
            Destroy(character.gameObject);
        }
        characterList.Clear();
    }
    
    private void HandleNewGame()
    {
        foreach (Character character in characterList)
        {
            CharacterData characterData = new CharacterData(character.GetCharacterName());
            _characterDataList.Add(characterData);
        }
        OnCharacterListUpdated?.Invoke();
        isNewGame = false;
    }

    public void AddToCharacterList(Character character)
    {
        characterList.Add(character);
        StartCoroutine(TriggerEventNextFrame());
    }

    private IEnumerator TriggerEventNextFrame()
    {
        yield return new WaitForEndOfFrame();
        OnCharacterListUpdated?.Invoke();
    }

    public List<Character> GetCharacterList()
    {
        return characterList;
    }

    public void SetSelectedCharacter(Character character)
    {
        _selectedCharacter = character;
        OnSelectedCharacterSet?.Invoke(character);
    }

    public Character GetSelectedCharacter()
    {
        return _selectedCharacter;
    }

    public List<CharacterData> GetCharacterDataList()
    {
        return _characterDataList;
    }

    public void CaptureState(string guid)
    {
        ES3.Save(guid, _characterDataList);
    }

    public void RestoreState(string guid)
    {
        ES3.Load(guid, _characterDataList);
        
        FindExistingCharacters();
        DestroyAllCharacters();
        
        OnCharacterDataListRestored?.Invoke(_characterDataList);
    }
}
