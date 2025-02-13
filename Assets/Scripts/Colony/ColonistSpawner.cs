using System.Collections.Generic;
using UnityEngine;

public class ColonistSpawner : MonoBehaviour
{
    [SerializeField] private Transform testSpawnLocation;
    [SerializeField] private Transform colonistPrefab;

    private CharacterManager _characterManager;
    
    private void Start()
    {
        _characterManager = FindObjectOfType<CharacterManager>();
        _characterManager.OnCharacterDataListRestored += OnCharacterDataListRestored;
    }

    private void OnCharacterDataListRestored(List<CharacterData> characterDataList)
    {
        foreach (CharacterData characterData in characterDataList)
        {
            SpawnColonist(characterData);
        }
     
    }

    public void SpawnColonist(CharacterData characterData)
    {
        //TODO: We will need to spawn differntly depending if the game is loaded or colonnist return from a mission
        
        Transform colonistTransform = Instantiate(colonistPrefab, testSpawnLocation.position, Quaternion.identity);
        Character character = colonistTransform.GetComponent<Character>();
        character.SetCharacterName(characterData.GetName());
        _characterManager.AddToCharacterList(character);
    }

    private void OnDestroy()
    {
        _characterManager.OnCharacterDataListRestored -= OnCharacterDataListRestored;
    }
}
