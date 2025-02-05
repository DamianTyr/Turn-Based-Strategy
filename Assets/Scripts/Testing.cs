using UnityEngine;

public class Testing : MonoBehaviour
{
    private SceneChanger _sceneChanger;
    private WorldMapCamera _worldMapCamera;
    
    private void Start()
    {
        _sceneChanger = FindObjectOfType<SceneChanger>();
        _worldMapCamera = FindObjectOfType<WorldMapCamera>();
        _sceneChanger.onAfterSceneChange += SceneChangerOnonBeforeSceneChange;
    }

    private void SceneChangerOnonBeforeSceneChange()
    {
        _worldMapCamera = FindObjectOfType<WorldMapCamera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _sceneChanger.LoadScene("ColonyScene");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _sceneChanger.LoadScene("ArmoryScene");
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            _worldMapCamera.ToggleWorldMapCamera();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 3;
        }
    }
}

