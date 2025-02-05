using PlayerInput;
using UnityEngine;

public class WorldMapLocation : MonoBehaviour, IRaycastable
{
    [SerializeField] private string missionSceneName;

    private SceneChanger _sceneChanger;
    
    void Start()
    {
        _sceneChanger = FindObjectOfType<SceneChanger>();
    }
    
    public CursorType GetCursorType()
    {
        return CursorType.None;
    }

    public bool HandleRaycast(MouseInputHandler callingController, RaycastHit hit)
    {
        return true;
    }

    public void HandleRaycastStop()
    {
        
    }

    public void HandleMouseClick()
    {
        _sceneChanger.SetMissionSceneToLoad(missionSceneName);
        _sceneChanger.LoadScene("ArmoryScene");
    }
}
