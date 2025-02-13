using System;
using System.Collections;
using Mission;
using UnityEngine;

public class VictoryConditions : MonoBehaviour
{
    public Action onMissionSuccesfull;
    
    private SceneChanger _sceneChanger;
    private UnitManager _unitManager;
    
    void Start()
    {
        _sceneChanger = FindObjectOfType<SceneChanger>();
        _unitManager = FindObjectOfType<UnitManager>();
        _unitManager.OnAllEnemiesKilled += OnAllEnemiesKilled;
    }

    private void OnAllEnemiesKilled()
    {
        onMissionSuccesfull?.Invoke();
        StartCoroutine(EndMissionInSeconds(5));
    }

    private IEnumerator EndMissionInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _sceneChanger.LoadScene("ColonyScene");
    }

    private void OnDestroy()
    {
        _unitManager.OnAllEnemiesKilled -= OnAllEnemiesKilled;
    }
}
