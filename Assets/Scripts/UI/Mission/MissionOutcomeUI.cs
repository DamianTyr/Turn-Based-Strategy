using UnityEngine;

public class MissionOutcomeUI : MonoBehaviour
{
    [SerializeField] private GameObject missionOutcomeUI;
    private VictoryConditions _victoryConditions;

    private void Start()
    {
        _victoryConditions = FindObjectOfType<VictoryConditions>();
        _victoryConditions.onMissionSuccesfull += OnMissionSuccesfull;
        missionOutcomeUI.SetActive(false);
    }

    private void OnMissionSuccesfull()
    {
        missionOutcomeUI.SetActive(true);
    }

    private void OnDestroy()
    {
        _victoryConditions.onMissionSuccesfull -= OnMissionSuccesfull;
    }
}
