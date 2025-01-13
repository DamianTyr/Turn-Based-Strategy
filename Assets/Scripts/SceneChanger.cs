using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        onAfterSceneChange?.Invoke();
    }
}
