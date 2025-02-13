public class CharacterData
{
    private string _characterName;

    public CharacterData(string characterName)
    {
        _characterName = characterName;
    }

    public string GetName()
    {
        return _characterName;
    }
}
