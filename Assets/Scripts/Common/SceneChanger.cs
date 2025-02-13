using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string selectedMissionSceneToLoad;
    
    public event Action onBeforeSceneChange;
    public event Action onAfterSceneChange;
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        onBeforeSceneChange?.Invoke();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        if (sceneName != "ArmoryScene") selectedMissionSceneToLoad = "";
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        onAfterSceneChange?.Invoke();
    }

    public string GetSelectedMissionToLoad()
    {
        return selectedMissionSceneToLoad;
    }

    public void SetMissionSceneToLoad(string missionSceneName)
    {
        selectedMissionSceneToLoad = missionSceneName;
    }

    public void LoadSelectedMissionScene()
    {
        LoadScene(selectedMissionSceneToLoad);
    }
}
