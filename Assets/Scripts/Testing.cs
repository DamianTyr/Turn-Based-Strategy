using UnityEngine;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
    private SceneChanger _sceneChanger;

    private void Start()
    {
        _sceneChanger = FindObjectOfType<SceneChanger>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name == "ColonyScene")
            {
                _sceneChanger.LoadScene("MissionScene");
            }

            if (scene.name == "MissionScene")
            {
                _sceneChanger.LoadScene("ColonyScene");
            }
        }
    }
}

