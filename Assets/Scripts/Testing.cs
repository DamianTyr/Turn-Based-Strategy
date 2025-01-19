using UnityEngine;

public class Testing : MonoBehaviour
{
    private SceneChanger _sceneChanger;

    private void Start()
    {
        _sceneChanger = FindObjectOfType<SceneChanger>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _sceneChanger.LoadScene("ColonyScene");
            Debug.Log("Loading Colony Scene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _sceneChanger.LoadScene("ArmoryScene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _sceneChanger.LoadScene("MissionScene");
        }
    }
}

