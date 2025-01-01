using System;
using System.Collections;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
    private SavingSystem _savingSystem;
    
    private void Start()
    {
        _savingSystem = FindObjectOfType<SavingSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name == "ColonyScene")
            {
                StartCoroutine(LoadSceneAsync("MissionScene"));
            }
            if (scene.name == "MissionScene")
            {
                StartCoroutine(LoadSceneAsync("ColonyScene"));
            }
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _savingSystem.Save();
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        _savingSystem.Load();
    }
}
