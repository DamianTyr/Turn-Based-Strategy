using UnityEngine;

public class StartMissionButton : MonoBehaviour
{
    [SerializeField] private GameObject uiContainer;
    private SceneChanger _sceneChanger;
    
    private void Start()
    {
        _sceneChanger = FindObjectOfType<SceneChanger>();
        if (_sceneChanger.GetSelectedMissionToLoad() == "")
        {
            uiContainer.SetActive(false);
            return;
        }
        uiContainer.SetActive(true);
    }
    
    //Triggered via UI event
    public void StartMissionScene()
    {
        _sceneChanger.LoadSelectedMissionScene();
    }
}
